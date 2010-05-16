namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class CellEnteredEventArgs : EventArgs
    {
        private Resco.Controls.AdvancedComboBox.Cell m_cell;
        private object m_data;
        private Resco.Controls.AdvancedComboBox.ListItem m_item;

        public CellEnteredEventArgs(Resco.Controls.AdvancedComboBox.Cell c, Resco.Controls.AdvancedComboBox.ListItem i, object data)
        {
            this.m_cell = c;
            this.m_item = i;
            this.m_data = data;
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

