using System;
using System.Text;
using System.Text.RegularExpressions;

using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

using Autodesk.AutoCAD.DatabaseServices;
using ACAD = Autodesk.AutoCAD.ApplicationServices;

namespace rtext
{
    public class FindReplacer
    {
        readonly bool isCaseSensitive;
        readonly bool isRegex;
        readonly string findWhat;
        readonly string replaceWith;
        readonly bool isReplacer;

        readonly StringComparison comparison;
        readonly Regex Rx;

        public FindReplacer(bool isCaseSensitive, bool isRegex, string findWhat, string replaceWith, bool replace)
        {
            this.isCaseSensitive = isCaseSensitive;
            this.isRegex = isRegex;
            this.findWhat = findWhat;
            this.replaceWith = replaceWith;
            this.isReplacer = replace;

            this.comparison = isCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            if (isRegex) {
                var options = RegexOptions.Compiled;
                if (!isCaseSensitive) {
                    options |= RegexOptions.IgnoreCase;
                }
                this.Rx = new Regex(findWhat, options);
            }
        }

        private bool IsMatch(string s)
        {
            if (isRegex) {
                return this.Rx.IsMatch(s);
            }
            return s.IndexOf(this.findWhat, this.comparison) > -1;
        }

        private string Replace(string s)
        {
            if (isRegex) {
                return this.Rx.Replace(s, this.replaceWith);
            }
            if (isCaseSensitive) {
                return s.Replace(this.findWhat, this.replaceWith);
            }

            var sb = new StringBuilder();
            int i0 = 0;
            int i1 = s.IndexOf(this.findWhat);
            while(i1 != -1) {
                sb.Append(s.Substring(i0, i1 - i0));
                sb.Append(this.replaceWith);
                i0 = i1 + this.findWhat.Length;
                i1 = s.IndexOf(this.findWhat, i0, StringComparison.OrdinalIgnoreCase);
            }
            sb.Append(s.Substring(i0));

            return sb.ToString();
        }

        public string ProcessFile(string filename, string password)
        {
            int processed = 0;
            string ext = Path.GetExtension(filename).ToLower();

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
                                    foreach (ObjectId objId2 in btr) {
                                        processed += this.ProcessObjectId(tr, objId2);
                                    }
                                }
                            }
                        }

                        if (this.isReplacer && processed > 0) {
                            tr.Commit();
                        } else {
                            tr.Abort();
                        }
                    }

                    if (this.isReplacer && processed > 0) {
                        if (ext == "dxf") {
                            acCurDb.DxfOut(filename, 16, acCurDb.OriginalFileVersion);
                        } else {
                            acCurDb.SaveAs(filename, acCurDb.OriginalFileVersion);
                        }
                    }
                } finally {
                    acCurDb.Dispose();
                }
            }

            return string.Format("{0}: {1}", this.isReplacer ? "replaced" : "found", processed);
        }

        private int ProcessObjectId(Transaction tr, ObjectId objId)
        {
            int processed = 0;
            var openMode = this.isReplacer ? OpenMode.ForWrite : OpenMode.ForRead;
            var obj = tr.GetObject(objId, openMode, false);

            if (obj is Table) {
                var tbl = obj as Table;
                if (tbl.NumRows > 0 && tbl.NumColumns > 0) {
                    for (var r = 0; r < tbl.NumRows; r++) {
                        for (var c = 0; c < tbl.NumColumns; c++) {
                            var s = tbl.GetTextString(r, c, 0, FormatOption.FormatOptionNone);
                            if (this.IsMatch(s)) {
                                if (this.isReplacer) {
                                    tbl.SetTextString(r, c, this.Replace(s));
                                }
                                processed++;
                            }
                        }
                    }
                }
            } else if (obj is MText) {
                var text = obj as MText;
                var s = text.Contents;
                if (this.IsMatch(s)) {
                    if (this.isReplacer) {
                        text.Contents = this.Replace(s);
                    }
                    processed++;
                }
            } else if (obj is DBText) {
                var text = obj as DBText;
                var s = text.TextString;
                if (this.IsMatch(s)) {
                    if (this.isReplacer) {
                        text.TextString = this.Replace(s);
                    }
                    processed++;
                }
            } else if (obj is BlockReference) {
                var br = obj as BlockReference;
                var brId = br.IsDynamicBlock ? br.DynamicBlockTableRecord : br.BlockTableRecord;
                var btr = (BlockTableRecord)tr.GetObject(brId, OpenMode.ForRead, false);
                foreach (ObjectId objId2 in btr) {
                    ProcessObjectId(tr, objId2);
                }
            }

            return processed;
        }
    }
}
