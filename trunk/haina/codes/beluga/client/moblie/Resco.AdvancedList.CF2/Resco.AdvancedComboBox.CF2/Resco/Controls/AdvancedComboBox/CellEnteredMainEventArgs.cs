namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class CellEnteredMainEventArgs : EventArgs
    {
        private Resco.Controls.AdvancedComboBox.Cell m_cell;
        private int m_iCellIndex;
        private Resco.Controls.AdvancedComboBox.ListItem m_item;

        public CellEnteredMainEventArgs() : this(null, -1, null)
        {
        }

        public CellEnteredMainEventArgs(Resco.Controls.AdvancedComboBox.Cell cell, int iCellIndex, Resco.Controls.AdvancedComboBox.ListItem i)
        {
            this.m_cell = cell;
            this.m_iCellIndex = iCellIndex;
            this.m_item = i;
        }

        public Resco.Controls.AdvancedComboBox.Cell Cell
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

