using System;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public class WatermarkTextBox : TextBox
    {
        private string _watermarkText = "";
        public string WatermarkText
        {
            get { return _watermarkText; }
            set { _watermarkText = value; }
        }

        private bool _watermarkActive = true;
        public bool WatermarkActive
        {
            get { return _watermarkActive; }
            set { _watermarkActive = value; }
        }

        public WatermarkTextBox()
        {
            this._watermarkActive = true;
            this.Text = _watermarkText;
            this.ForeColor = Color.Gray;

            GotFocus += (source, e) => {
                RemoveWatermak();
            };

            LostFocus += (source, e) => {
                ApplyWatermark();
            };
        }

        public void RemoveWatermak()
        {
            if (this._watermarkActive) {
                this._watermarkActive = false;
                this.Text = "";
                this.ForeColor = Color.Black;
            }
        }

        public void ApplyWatermark()
        {
            if (!this._watermarkActive && string.IsNullOrEmpty(this.Text)
                || ForeColor == Color.Gray) {
                this._watermarkActive = true;
                this.Text = _watermarkText;
                this.ForeColor = Color.Gray;
            }
        }

        public void ApplyWatermark(string newText)
        {
            WatermarkText = newText;
            ApplyWatermark();
        }

    }
}