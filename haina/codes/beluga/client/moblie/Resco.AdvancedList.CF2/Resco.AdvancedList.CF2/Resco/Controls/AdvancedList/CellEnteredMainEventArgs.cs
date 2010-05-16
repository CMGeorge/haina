namespace Resco.Controls.AdvancedList
{
    using System;

    public class CellEnteredMainEventArgs : EventArgs
    {
        private Resco.Controls.AdvancedList.Cell m_cell;
        private int m_iCellIndex;
        private Resco.Controls.AdvancedList.Row m_row;

        public CellEnteredMainEventArgs() : this(null, -1, null)
        {
        }

        public CellEnteredMainEventArgs(Resco.Controls.AdvancedList.Cell cell, int iCellIndex, Resco.Controls.AdvancedList.Row r)
        {
            this.m_cell = cell;
            this.m_iCellIndex = iCellIndex;
            this.m_row = r;
        }

        public Resco.Controls.AdvancedList.Cell Cell
        {
            get
            {
                return this.m_cell;
            }
            set
            {
                this.m_cell = value;
            }
        }

        public int CellIndex
        {
            get
            {
                return this.m_iCellIndex;
            }
            set
            {
                this.m_iCellIndex = value;
            }
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
    }
}

