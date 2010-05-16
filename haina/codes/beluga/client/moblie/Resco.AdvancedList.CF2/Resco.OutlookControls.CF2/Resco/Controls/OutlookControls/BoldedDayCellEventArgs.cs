namespace Resco.Controls.OutlookControls
{
    using System;

    public class BoldedDayCellEventArgs : EventArgs
    {
        public Resco.Controls.OutlookControls.DayCell[] BoldedDates;
        public DateTime Date;
        public Resco.Controls.OutlookControls.DayCell OutputDate;

        public BoldedDayCellEventArgs(DateTime date, Resco.Controls.OutlookControls.DayCell[] boldedDates, Resco.Controls.OutlookControls.DayCell outputDate)
        {
            this.Date = date;
            this.BoldedDates = boldedDates;
            this.OutputDate = outputDate;
        }
    }
}

