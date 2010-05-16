namespace Resco.Controls.OutlookControls
{
    using System;

    public class DrawDayHeaderEventArgs : EventArgs
    {
        private DateTime m_Day;
        private string m_HeaderText;
        private int m_MaxWidth;

        public DrawDayHeaderEventArgs(string aHeaderText, DateTime aDay, int aMaxWidth)
        {
            this.m_HeaderText = aHeaderText;
            this.m_Day = aDay;
            this.m_MaxWidth = aMaxWidth;
        }

        public DateTime Day
        {
            get
            {
                return this.m_Day;
            }
            internal set
            {
                this.m_Day = value;
            }
        }

        public string HeaderText
        {
            get
            {
                return this.m_HeaderText;
            }
            set
            {
                this.m_HeaderText = value;
            }
        }

        public int MaxWidth
        {
            get
            {
                return this.m_MaxWidth;
            }
            internal set
            {
                this.m_MaxWidth = value;
            }
        }
    }
}

