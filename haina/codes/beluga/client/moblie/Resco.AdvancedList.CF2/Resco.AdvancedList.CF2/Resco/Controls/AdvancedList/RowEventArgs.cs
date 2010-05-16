namespace Resco.Controls.AdvancedList
{
    using System;

    public class RowEventArgs : EventArgs
    {
        public Row DataRow;
        public int Offset;
        public int RowIndex;

        public RowEventArgs(Row r, int i, int yoff)
        {
            this.DataRow = r;
            this.RowIndex = i;
            this.Offset = yoff;
        }
    }
}

