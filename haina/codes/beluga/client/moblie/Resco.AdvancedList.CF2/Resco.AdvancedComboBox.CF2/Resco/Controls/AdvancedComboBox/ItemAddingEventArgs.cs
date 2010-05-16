namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.ComponentModel;

    public class ItemAddingEventArgs : CancelEventArgs
    {
        private Resco.Controls.AdvancedComboBox.ListItem m_item;
        private int m_nItemIndex;

        public ItemAddingEventArgs(int itemIndex)
        {
            this.m_nItemIndex = itemIndex;
            this.m_item = null;
        }

        public int ItemIndex
        {
            get
            {
                return this.m_nItemIndex;
            }
        }

        public Resco.Controls.AdvancedComboBox.ListItem ListItem
        {
            get
            {
                return this.m_item;
            }
            set
            {
                this.m_item = value;
            }
        }
    }
}

