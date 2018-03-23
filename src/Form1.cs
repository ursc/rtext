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
        string password;
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
            password = string.Empty;

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

            while(true) {
	            using (Database acCurDb = new Database(false, true)) {
	                try {
	                    if (ext == "dxf") {
	                        acCurDb.DxfIn(filename, Environment.GetEnvironmentVariable("TEMP") + "\\dxf.log");
	                    } else {
                        	acCurDb.ReadDwgFile(filename, System.IO.FileShare.Read, true, password);
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

	                    break;
        			} catch (Autodesk.AutoCAD.Runtime.Exception ex) {
            			layouts = -1;
        				if (ex.ErrorStatus != Autodesk.AutoCAD.Runtime.ErrorStatus.SecErrorDecryptingData) {
        					break;
        				}
    					PasswordForm f = new PasswordForm();
    					f.Text = Path.GetFileName(filename);
    					f.textBoxPassword.Text = password;
    					if (f.ShowDialog() != DialogResult.OK) {
    						break;
    					}
    					File.AppendAllText(@"c:\art.log", "3: " + ex.ToString() + "\n");
    					password = f.textBoxPassword.Text;
	            	} catch {
	                    layouts = -1;
	                }
	            }
            }
            return layouts;
        }

        void Process(FindReplacer findReplacer)
        {
            if (IsFileOpened()) {
                return;
            }

            foreach (DataGridViewRow row in filesDataGridView.Rows) {
                row.Cells["ColResult"].Value = "";
                SetCellsColor(row.Index, Result.CHECK, new[] { "ColResult" });

                Application.DoEvents(); // .oOo.

                try {
                    row.Cells["ColResult"].Value = findReplacer.ProcessFile(row.Cells[0].Value.ToString(), password);
                    SetCellsColor(row.Index, Result.OK, new[] { "ColResult" });
                } catch (Exception ex) {
                    row.Cells["ColResult"].Value = ex.Message;
                    SetCellsColor(row.Index, Result.ERROR, new[] { "ColResult" });
                }
            }
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
            bool replace = sender == replaceButton;
            try {
                var findReplacer = new FindReplacer(caseCheckBox.Checked, rxCheckBox.Checked, findTextBox.Text, replaceTextBox.Text, replace);
                Process(findReplacer);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            flowLayoutPanel1.Enabled = true;
        }
        #endregion
    }
}
