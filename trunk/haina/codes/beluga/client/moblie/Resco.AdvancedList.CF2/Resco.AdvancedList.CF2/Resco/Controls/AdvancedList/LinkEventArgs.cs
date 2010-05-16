namespace Resco.Controls.AdvancedList
{
    using System;

    public class LinkEventArgs : CellEventArgs
    {
        public string Target;

        public LinkEventArgs(Row r, Cell c, int ri, int ci, int yoff) : base(r, c, ri, ci, yoff)
        {
            this.Target = ((LinkCell) base.Cell).GetLink(base.CellData);
        }
    }
}

