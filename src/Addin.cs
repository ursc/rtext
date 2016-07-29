using System;
using System.IO;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;

namespace rtext
{
    public class Addin : Autodesk.AutoCAD.Runtime.IExtensionApplication
    {
        public static Form1 form;

        public void Terminate()
        {
        }
        public void Initialize()
        {
        }

        [CommandMethod("rt")]
        public void rtext()
        {
            if (form == null || form.IsDisposed) {
                form = new Form1();
            }

            form.ShowDialog();
            if (form.OpenFilename != "" && File.Exists(form.OpenFilename)) {
                Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(form.OpenFilename, false);
            }
        }
    }
}
