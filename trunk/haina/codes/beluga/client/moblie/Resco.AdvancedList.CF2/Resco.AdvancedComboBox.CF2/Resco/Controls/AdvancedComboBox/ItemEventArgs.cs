namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class ItemEventArgs : EventArgs
    {
        public int ItemIndex;
        public Resco.Controls.AdvancedComboBox.ListItem ListItem;
        public int Offset;

        public ItemEventArgs(Resco.Controls.AdvancedComboBox.ListItem i, int index, int yoff)
        {
            this.ListItem = i;
            this.ItemIndex = index;
            this.Offset = yoff;
        }
    }
}

