namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class DrawLineData
    {
        public int CutLength;
        public int Index;
        public int Length;
        public int Width;

        public DrawLineData(int index, int length, int width)
        {
            this.Index = index;
            this.Length = length;
            this.Width = width;
            this.CutLength = length;
        }
    }
}

