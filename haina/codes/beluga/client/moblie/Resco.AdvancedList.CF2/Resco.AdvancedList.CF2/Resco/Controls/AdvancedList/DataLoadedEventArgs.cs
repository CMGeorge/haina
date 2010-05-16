namespace Resco.Controls.AdvancedList
{
    using System;

    public class DataLoadedEventArgs
    {
        private bool m_bLoadComplete;

        public DataLoadedEventArgs(bool bComplete)
        {
            this.m_bLoadComplete = bComplete;
        }

        public bool LoadComplete
        {
            get
            {
                return this.m_bLoadComplete;
            }
            set
            {
                this.m_bLoadComplete = value;
            }
        }
    }
}

