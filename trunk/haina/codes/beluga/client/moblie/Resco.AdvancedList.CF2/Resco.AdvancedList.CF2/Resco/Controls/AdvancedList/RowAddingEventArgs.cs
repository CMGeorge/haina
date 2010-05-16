namespace Resco.Controls.AdvancedList
{
    using System;
    using System.ComponentModel;

    public class RowAddingEventArgs : CancelEventArgs
    {
        private int m_nRowIndex;
        private Resco.Controls.AdvancedList.Row m_row;

        public RowAddingEventArgs(int rowIndex)
        {
            this.m_nRowIndex = rowIndex;
            this.m_row = null;
        }

        public Resco.Controls.AdvancedList.Row Row
        {
            get
            {
                return this.m_row;
            }
            set
            {
                this.m_row = value;
            }
        }

        public int RowIndex
        {
            get
            {
                return this.m_nRowIndex;
            }
        }
    }
}

