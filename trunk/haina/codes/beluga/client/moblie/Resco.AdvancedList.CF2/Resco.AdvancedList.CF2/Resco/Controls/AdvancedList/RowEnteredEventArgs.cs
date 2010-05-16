namespace Resco.Controls.AdvancedList
{
    using System;

    public class RowEnteredEventArgs : EventArgs
    {
        private int m_iRowIndex;
        private Resco.Controls.AdvancedList.Row m_row;

        public RowEnteredEventArgs(Resco.Controls.AdvancedList.Row r, int iRowIndex)
        {
            this.m_row = r;
            this.m_iRowIndex = iRowIndex;
        }

        public Resco.Controls.AdvancedList.Row Row
        {
            get
            {
                return this.m_row;
            }
        }

        public int RowIndex
        {
            get
            {
                return this.m_iRowIndex;
            }
        }
    }
}

