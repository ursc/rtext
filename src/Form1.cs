using System;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

using Autodesk.AutoCAD.DatabaseServices;
using ACAD = Autodesk.AutoCAD.ApplicationServices;

namespace rtext
{
    public partial class Form1 : Form
    {
        #region enums
        public enum Result
        {
            OK = 0,
            WARNING = 1,
            ERROR = 2,
            CHECK = 3
        }
        #endregion

        #region properties
        private string openfilename;
        public string OpenFilename
        {
            get
            {
                return openfilename;
            }
        }
        #endregion

        #region fields
        readonly Font btnFontBold;
        readonly Font btnFontNormal;
        readonly string[] allowExtensions;
        readonly Dictionary<Result, CellColor> cellColors;
        #endregion

        public Form1()
        {
            InitializeComponent();

            btnFontBold = filesAddButton.Font;
            btnFontNormal = new Font(btnFontBold, FontStyle.Regular);
            allowExtensions = new[] { ".dwg", ".dxf", ".dwt" };
            cellColors = new Dictionary<Result, CellColor>() {
                { Result.OK,      new CellColor(0xA9FF7A, 0x96E06B) },
                { Result.WARNING, new CellColor(0xFFE88E, 0xFFD859) },
                { Result.ERROR,   new CellColor(0xFFADAD, 0xFF8C8C) },
                { Result.CHECK,   new CellColor(0xE8FFFF, 0xD1FFFF) },
            };

            filesDataGridView.DefaultCellStyle.BackColor = cellColors[Result.OK].BackColor;
            filesDataGridView.DefaultCellStyle.SelectionBackColor = cellColors[Result.OK].SelectionBackColor;

            AnyPropertyChanged();
        }

        void AnyPropertyChanged()
        {
            filesClearButton.Enabled = filesDataGridView.RowCount > 0;
            findButton.Enabled = filesClearButton.Enabled && !findTextBox.WatermarkActive && findTextBox.Text.Length > 0;
            replaceButton.Enabled = findButton.Enabled && !replaceTextBox.WatermarkActive && replaceTextBox.Text.Length > 0
                && findTextBox.Text != replaceTextBox.Text;

            filesClearButton.Font = filesClearButton.Enabled ? btnFontBold : btnFontNormal;
            findButton.Font = findButton.Enabled ? btnFontBold : btnFontNormal;
            replaceButton.Font = replaceButton.Enabled ? btnFontBold : btnFontNormal;
        }

        bool IsFileOpened(string filename)
        {
            foreach (ACAD.Document ad in ACAD.Application.DocumentManager) {
                if (string.Compare(filename, ad.Name, StringComparison.OrdinalIgnoreCase) == 0) {
                    return true;
                }
            }
            return false;
        }

        bool IsFileOpened()
        {
            foreach (DataGridViewRow row in filesDataGridView.Rows) {
                string filename = row.Cells[0].Value.ToString();
                if (IsFileOpened(filename)) {
                    MessageBox.Show(filename, "File is open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }

            }
            return false;
        }

        bool IsFileAdded(string checkFilename)
        {
            foreach (DataGridViewRow row in filesDataGridView.Rows) {
                string filename = row.Cells[0].Value.ToString();
                if (string.Compare(checkFilename, filename, StringComparison.OrdinalIgnoreCase) == 0) {
                    return true;
                }
            }
            return false;
        }

        bool AddFile(string filename)
        {
            var ext = Path.GetExtension(filename).ToLower();
            if (!allowExtensions.Contains(ext)) {
                return false;
            }

            if (IsFileAdded(filename)) {
                return false;
            }

            var info = new FileInfo(filename);
            var layouts = GetLayouts(filename);

            filesDataGridView.Rows.Add(filename, ext, info.Length, layouts, "");

            Application.DoEvents(); // .oOo.

            return true;
        }

        void AddFiles(string root)
        {
            int lastDir = 0;
            List<string> dirs = new List<string>() { root };
            while (lastDir < dirs.Count) {
                dirs.AddRange(Directory.GetDirectories(dirs[lastDir]));

                string[] files = Directory.GetFiles(dirs[lastDir]);
                for (int i = 0; i < files.Length; i++) {
                    AddFile(files[i]);
                }

                lastDir++;
            }
        }

        void SetCellsColor(int row, Result result, string[] cells)
        {
            foreach (var i in cells) {
                filesDataGridView.Rows[row].Cells[i].Style.BackColor = cellColors[result].BackColor;
                filesDataGridView.Rows[row].Cells[i].Style.SelectionBackColor = cellColors[result].SelectionBackColor;
            }
        }

        int GetLayouts(string filename)
        {
            int layouts = 0;
            string ext = Path.GetExtension(filename).ToLower();

            using (Database acCurDb = new Database(false, true)) {
                try {
                    if (ext == "dxf") {
                        acCurDb.DxfIn(filename, Environment.GetEnvironmentVariable("TEMP") + "\\dxf.log");
                    } else {
                        acCurDb.ReadDwgFile(filename, System.IO.FileShare.Read, true, "");
                    }

                    using (var tr = acCurDb.TransactionManager.StartTransaction()) {
                        var bt = (BlockTable)tr.GetObject(acCurDb.BlockTableId, OpenMode.ForRead, false);
                        foreach (ObjectId objId in bt) {
                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(objId, OpenMode.ForRead, false);
                            if (btr != null) {
                                if (btr.IsLayout) {
                                    layouts++;
                                }
                            }
                        }
                        tr.Abort();
                    }

                    acCurDb.Dispose();
                } catch {
                    layouts = -1;
                }
            }
            return layouts;
        }

        void Process(FindReplaceData data)
        {
            if (IsFileOpened()) {
                return;
            }

            foreach (DataGridViewRow row in filesDataGridView.Rows) {
                row.Cells["ColResult"].Value = "";
                SetCellsColor(row.Index, Result.CHECK, new[] { "ColResult" });

                Application.DoEvents(); // .oOo.

                data.Filename = row.Cells[0].Value.ToString();
                var result = ProcessFile(data);
                row.Cells["ColResult"].Value = data.Result;

                SetCellsColor(row.Index, result, new[] { "ColResult" });
            }
        }

        Result ProcessFile(FindReplaceData data)
        {
            string ext = Path.GetExtension(data.Filename).ToLower();
            string prefix = data.Replace ? "replaced: " : "found: ";

            using (Database acCurDb = new Database(false, true)) {
                try {
                    if (ext == "dxf") {
                        acCurDb.DxfIn(data.Filename, Environment.GetEnvironmentVariable("TEMP") + "\\dxf.log");
                    } else {
                        acCurDb.ReadDwgFile(data.Filename, System.IO.FileShare.Read, true, "");
                    }

                    int processed = 0;
                    using (var tr = acCurDb.TransactionManager.StartTransaction()) {
                        var bt = (BlockTable)tr.GetObject(acCurDb.BlockTableId, OpenMode.ForRead, false);
                        foreach (ObjectId objId in bt) {
                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(objId, OpenMode.ForRead, false);
                            if (btr != null) {
                                if (btr.IsLayout) {
                                    foreach (ObjectId objId2 in btr) {
                                        processed += ProcessObjectId(tr, objId2, data);
                                    }
                                }
                            }
                        }

                        if (data.Replace && processed > 0) {
                            tr.Commit();
                        } else {
                            tr.Abort();
                        }
                    }
                    data.Result = prefix + processed.ToString();

                    if (data.Replace && processed > 0) {
                        if (ext == "dxf") {
                            acCurDb.DxfOut(data.Filename, 16, acCurDb.OriginalFileVersion);
                        } else {
                            acCurDb.SaveAs(data.Filename, acCurDb.OriginalFileVersion);
                        }
                    }
                    acCurDb.Dispose();
                } catch (System.Exception ex) {
                    data.Result = ex.Message;
                    return Result.ERROR;
                }
            }
            return Result.OK;
        }

        int ProcessObjectId(Transaction tr, ObjectId objId, FindReplaceData data)
        {
            int processed = 0;
            var openMode = data.Replace ? OpenMode.ForWrite : OpenMode.ForRead;
            var ent = tr.GetObject(objId, openMode, false);
            var entType = ent.GetType();

            var comparison = data.IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            if (entType == typeof(Table)) {
                var tbl = ent as Table;
                if (tbl.NumRows > 0 && tbl.NumColumns > 0) {
                    for (var r = 0; r < tbl.NumRows; r++) {
                        for (var c = 0; c < tbl.NumColumns; c++) {
                            var s = tbl.GetTextString(r, c, 0, FormatOption.FormatOptionNone);
                            if (s.IndexOf(data.FindWhat, comparison) > -1) {
                                if (data.Replace) {
                                    tbl.SetTextString(r, c, s.Replace(data.FindWhat, data.ReplaceWith));
                                }
                                processed++;
                            }
                        }
                    }
                }
            }

            if (entType == typeof(MText)) {
                var text = ent as MText;
                var s = text.Contents;
                if (s.IndexOf(data.FindWhat, comparison) > -1) {
                    if (data.Replace) {
                        text.Contents = s.Replace(data.FindWhat, data.ReplaceWith);
                    }
                    processed++;
                }
            }

            if (entType == typeof(DBText)) {
                var text = ent as DBText;
                var s = text.TextString;
                if (s.IndexOf(data.FindWhat, comparison) > -1) {
                    if (data.Replace) {
                        text.TextString = s.Replace(data.FindWhat, data.ReplaceWith);
                    }
                    processed++;
                }
            }

            if (entType == typeof(BlockReference)) {
                var br = ent as BlockReference;
                var brId = br.IsDynamicBlock ? br.DynamicBlockTableRecord : br.BlockTableRecord;
                var btr = (BlockTableRecord)tr.GetObject(brId, OpenMode.ForRead, false);
                foreach (ObjectId objId2 in btr) {
                    ProcessObjectId(tr, objId2, data);
                }
            }

            return processed;
        }

        #region events
        void FilesCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) {
                if (0 != System.Convert.ToInt32(ACAD.Application.GetSystemVariable("SDI"))) {
                    System.Windows.Forms.MessageBox.Show("Not available in SDI mode.");
                    return;
                }

                openfilename = filesDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                Hide();
            }
        }

        void Form1FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        void Form1KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 27) {
                Hide();
            }
        }

        void Form1Shown(object sender, EventArgs e)
        {
            openfilename = "";
        }

        void FilesAddClick(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                flowLayoutPanel1.Enabled = false;
                foreach (string filename in openFileDialog1.FileNames) {
                    AddFile(filename);
                }
                flowLayoutPanel1.Enabled = true;

                filesDataGridView.ClearSelection();
                AnyPropertyChanged();
            }
        }

        void FolderClick(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                flowLayoutPanel1.Enabled = false; 
                AddFiles(folderBrowserDialog1.SelectedPath);
                flowLayoutPanel1.Enabled = true;

                filesDataGridView.ClearSelection();
                AnyPropertyChanged();
            }
        }

        void FilesClearClick(object sender, EventArgs e)
        {
            filesDataGridView.Rows.Clear();
            AnyPropertyChanged();
        }

        void FilesMouseClick(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hi = filesDataGridView.HitTest(e.X, e.Y);
            if (hi.Type != DataGridViewHitTestType.Cell) {
                filesDataGridView.ClearSelection();
            }
        }

        void FilesUserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            filesDataGridView.ClearSelection();
            AnyPropertyChanged();
        }

        void TextBoxTextChanged(object sender, EventArgs e)
        {
            AnyPropertyChanged();
        }

        void FindClick(object sender, EventArgs e)
        {
            flowLayoutPanel1.Enabled = false;
            var data = new FindReplaceData(caseCheckBox.Checked, rxCheckBox.Checked, findTextBox.Text, replaceTextBox.Text, false);
            Process(data);
            flowLayoutPanel1.Enabled = true;
        }

        void ReplaceClick(object sender, EventArgs e)
        {
            flowLayoutPanel1.Enabled = false;
            var data = new FindReplaceData(caseCheckBox.Checked, rxCheckBox.Checked, findTextBox.Text, replaceTextBox.Text, true);
            Process(data);
            flowLayoutPanel1.Enabled = true;
        }
        #endregion
    }
}
