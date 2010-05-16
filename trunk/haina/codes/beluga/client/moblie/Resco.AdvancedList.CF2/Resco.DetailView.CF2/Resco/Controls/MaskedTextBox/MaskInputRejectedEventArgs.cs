namespace Resco.Controls.MaskedTextBox
{
    using System;

    public class MaskInputRejectedEventArgs : EventArgs
    {
        private int m_position;

        public MaskInputRejectedEventArgs(int position)
        {
            this.m_position = position;
        }

        public int Position
        {
            get
            {
                return this.m_position;
            }
        }
    }
}

