namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Drawing;

    public class WrapTextData
    {
        public System.Drawing.Font Font;
        public int Height;
        public int LineHeight;
        public DrawLineData[] Lines;
        public int TextLength;
        public int Width;

        public WrapTextData(int textLength, System.Drawing.Font font, int width)
        {
            this.TextLength = textLength;
            this.Font = font;
            this.Width = width;
            this.Height = 0;
            this.LineHeight = 0;
            this.Lines = null;
        }
    }
}

