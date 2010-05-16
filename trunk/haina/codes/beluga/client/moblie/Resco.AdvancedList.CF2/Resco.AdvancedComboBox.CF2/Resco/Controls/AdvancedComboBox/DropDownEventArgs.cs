namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class DropDownEventArgs : EventArgs
    {
        private bool m_bCancel;
        private int m_cellIndex;
        private int m_itemIndex;

        public DropDownEventArgs(bool bCancel, int ii, int ci)
        {
            this.m_bCancel = bCancel;
            this.m_itemIndex = ii;
            this.m_cellIndex = ci;
        }

        public bool Cancel
        {
            get
            {
                return this.m_bCancel;
            }
            set
            {
                this.m_bCancel = value;
            }
        }

        public int CellIndex
        {
            get
            {
                return this.m_cellIndex;
            }
            set
            {
                this.m_cellIndex = value;
            }
        }

        public int ItemIndex
        {
            get
            {
                return this.m_itemIndex;
            }
            set
            {
                this.m_itemIndex = value;
            }
        }
    }
}

