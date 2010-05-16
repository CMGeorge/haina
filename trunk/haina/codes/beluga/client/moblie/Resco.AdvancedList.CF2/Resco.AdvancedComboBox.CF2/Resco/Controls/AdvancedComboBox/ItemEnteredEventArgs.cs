namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class ItemEnteredEventArgs : EventArgs
    {
        private int m_iItemIndex;
        private Resco.Controls.AdvancedComboBox.ListItem m_item;

        public ItemEnteredEventArgs(Resco.Controls.AdvancedComboBox.ListItem i, int iItemIndex)
        {
            this.m_item = i;
            this.m_iItemIndex = iItemIndex;
        }

        public int ItemIndex
        {
            get
            {
                return this.m_iItemIndex;
            }
        }

        public Resco.Controls.AdvancedComboBox.ListItem ListItem
        {
            get
            {
                return this.m_item;
            }
        }
    }
}

