namespace Resco.Controls.AdvancedTree
{
    using System;

    public class CellEventArgs : NodeEventArgs
    {
        public Resco.Controls.AdvancedTree.Cell Cell;
        public object CellData;
        public int CellIndex;

        public CellEventArgs(Node r, Resco.Controls.AdvancedTree.Cell c, int ri, int ci, int yoff) : base(r, ri, yoff)
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

