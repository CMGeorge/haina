namespace Resco.Controls.AdvancedList
{
    using System;

    public class CustomizeCellEventArgs : EventArgs
    {
        private Resco.Controls.AdvancedList.Cell m_cell;
        private object m_data;
        private Row m_dataRow;

        public CustomizeCellEventArgs(Resco.Controls.AdvancedList.Cell cell, object data, Row dataRow)
        {
            this.m_cell = cell.Clone();
            this.m_cell.Owner = cell.Owner;
            this.m_data = data;
            this.m_dataRow = dataRow;
        }

        public Resco.Controls.AdvancedList.Cell Cell
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

        public Row DataRow
        {
            get
            {
                return this.m_dataRow;
            }
        }
    }
}

