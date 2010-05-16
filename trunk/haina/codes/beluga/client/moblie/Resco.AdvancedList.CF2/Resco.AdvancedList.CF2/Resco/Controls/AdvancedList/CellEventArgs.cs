namespace Resco.Controls.AdvancedList
{
    using System;

    public class CellEventArgs : RowEventArgs
    {
        public Resco.Controls.AdvancedList.Cell Cell;
        public object CellData;
        public int CellIndex;

        public CellEventArgs(Row r, Resco.Controls.AdvancedList.Cell c, int ri, int ci, int yoff) : base(r, ri, yoff)
        {
            this.CellIndex = ci;
            this.Cell = c;
            if (c != null)
            {
                this.CellData = c[r, ci];
            }
            else
            {
                this.CellData = null;
            }
        }
    }
}

