namespace Resco.Controls.OutlookControls
{
    using System;

    public class MonthChangeAfterEventArgs : EventArgs
    {
        private DateTime m_newDate;
        private DateTime m_oldDate;

        public MonthChangeAfterEventArgs(DateTime oldDate, DateTime newDate)
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

