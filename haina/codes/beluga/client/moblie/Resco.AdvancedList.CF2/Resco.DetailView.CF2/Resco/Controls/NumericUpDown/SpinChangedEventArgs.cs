namespace Resco.Controls.NumericUpDown
{
    using System;

    public class SpinChangedEventArgs : EventArgs
    {
        private int m_increaseRate;

        public SpinChangedEventArgs(int increaseRate)
        {
            this.m_increaseRate = increaseRate;
        }

        public int IncreaseRate
        {
            get
            {
                return this.m_increaseRate;
            }
        }
    }
}

