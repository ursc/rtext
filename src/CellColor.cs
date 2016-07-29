using System;
using System.Drawing;

namespace rtext
{
    public class CellColor
    {
        public Color BackColor;
        public Color SelectionBackColor;

        public CellColor(int back, int selectionBack)
        {
            BackColor = ColorFromInt(back);
            SelectionBackColor = ColorFromInt(selectionBack);
        }

        Color ColorFromInt(int c)
        {
            return Color.FromArgb((c & 0xFF0000) >> 16, (c & 0x00FF00) >> 8, c & 0x0000FF);
        }
    }
}
