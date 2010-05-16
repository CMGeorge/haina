namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Windows.Forms;

    internal interface ITool
    {
        void MouseDown(MouseEventArgs e);
        void MouseMove(MouseEventArgs e);
        void MouseUp(MouseEventArgs e);
        void Reset();

        OutlookWeekCalendar OutlookWeekCalendarControl { get; set; }
    }
}

