namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class LinkEventArgs : CellEventArgs
    {
        public string Target;

        public LinkEventArgs(ListItem item, Cell c, int ri, int ci, int yoff) : base(item, c, ri, ci, yoff)
        {
            this.Target = ((LinkCell) base.Cell).GetLink(base.CellData);
        }
    }
}

