namespace Resco.Controls.AdvancedTree
{
    using System;

    public class LinkEventArgs : CellEventArgs
    {
        public string Target;

        public LinkEventArgs(Node n, Cell c, int ri, int ci, int yoff) : base(n, c, ri, ci, yoff)
        {
            this.Target = ((LinkCell) base.Cell).GetLink(base.CellData);
        }
    }
}

