namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class CustomizeCellEventArgs : EventArgs
    {
        private Resco.Controls.AdvancedComboBox.Cell m_cell;
        private object m_data;
        private ListItem m_dataItem;

        public CustomizeCellEventArgs(Resco.Controls.AdvancedComboBox.Cell cell, object data, ListItem dataItem)
        {
            this.m_cell = cell.Clone();
            this.m_cell.Owner = cell.Owner;
            this.m_data = data;
            this.m_dataItem = dataItem;
        }

        public Resco.Controls.AdvancedComboBox.Cell Cell
        {
            get
            {
                return this.m_cell;
            }
        }

        public object Data
        {
            get
            {
                return this.m_data;
            }
            set
            {
                this.m_data = value;
            }
        }

        public ListItem DataItem
        {
            get
            {
                return this.m_dataItem;
            }
        }
    }
}

