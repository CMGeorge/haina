namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class CellEventArgs : ItemEventArgs
    {
        public Resco.Controls.AdvancedComboBox.Cell Cell;
        public object CellData;
        public int CellIndex;

        public CellEventArgs(ListItem i, Resco.Controls.AdvancedComboBox.Cell c, int ii, int ci, int yoff) : base(i, ii, yoff)
        {
            this.CellIndex = ci;
            this.Cell = c;
            if (c != null)
            {
                this.CellData = c[i, ci];
            }
            else
            {
                this.CellData = null;
            }
        }
    }
}

