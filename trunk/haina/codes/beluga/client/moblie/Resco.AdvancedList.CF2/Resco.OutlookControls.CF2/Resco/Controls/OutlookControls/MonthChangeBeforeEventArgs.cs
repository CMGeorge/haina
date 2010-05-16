namespace Resco.Controls.OutlookControls
{
    using System;
    using System.ComponentModel;

    public class MonthChangeBeforeEventArgs : CancelEventArgs
    {
        private DateTime m_newDate;
        private DateTime m_oldDate;

        public MonthChangeBeforeEventArgs(DateTime oldDate, DateTime newDate)
        {
            this.m_oldDate = oldDate;
            this.m_newDate = newDate;
        }

        public DateTime NewDate
        {
            get
            {
                return this.m_newDate;
            }
            set
            {
                this.m_newDate = value;
            }
        }

        public DateTime OldDate
        {
            get
            {
                return this.m_oldDate;
            }
        }
    }
}

