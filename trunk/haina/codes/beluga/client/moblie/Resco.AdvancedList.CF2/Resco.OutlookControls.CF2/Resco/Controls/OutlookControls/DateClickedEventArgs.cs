namespace Resco.Controls.OutlookControls
{
    using System;

    public class DateClickedEventArgs : EventArgs
    {
        internal DateTime _day;

        public DateClickedEventArgs(DateTime date)
        {
            this._day = date;
        }

        public DateTime Day
        {
            get
            {
                return this._day;
            }
        }
    }
}

