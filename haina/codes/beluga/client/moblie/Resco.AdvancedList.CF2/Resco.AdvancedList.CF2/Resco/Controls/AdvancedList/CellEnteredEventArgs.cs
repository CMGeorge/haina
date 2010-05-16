namespace Resco.Controls.AdvancedList
{
    using System;

    public class CellEnteredEventArgs : EventArgs
    {
        private Resco.Controls.AdvancedList.Cell m_cell;
        private object m_data;
        private Resco.Controls.AdvancedList.Row m_row;

        public CellEnteredEventArgs(Resco.Controls.AdvancedList.Cell c, Resco.Controls.AdvancedList.Row r, object data)
        {
            this.m_cell = c;
            this.m_row = r;
            this.m_data = data;
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
        }

        public Resco.Controls.AdvancedList.Row Row
        {
            get
            {
                return this.m_row;
            }
        }
    }
}

