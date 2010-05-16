namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal class DrawTool : ITool
    {
        private OutlookWeekCalendar m_OutlookWeekCalendar;
        private DateTime m_SelectionStart;
        private bool m_SelectionStarted;
        private bool m_ToolTipClicked;

        public event EventHandler Complete;

        public void MouseDown(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && this.m_OutlookWeekCalendar.GetTimeAt(e.X, e.Y, out this.m_SelectionStart))
            {
                this.m_OutlookWeekCalendar.SelectionStart = this.m_SelectionStart;
                this.m_OutlookWeekCalendar.SelectionEnd = this.m_SelectionStart.AddMinutes((double) (60 / this.m_OutlookWeekCalendar.HourLines));
                this.m_SelectionStarted = true;
                this.m_OutlookWeekCalendar.Invalidate();
                this.m_OutlookWeekCalendar.Capture = true;
            }
        }

        public void MouseMove(MouseEventArgs e)
        {
            DateTime time;
            if (((e.Button == MouseButtons.Left) && this.m_SelectionStarted) && this.m_OutlookWeekCalendar.GetTimeAt(e.X, e.Y, out time))
            {
                time = time.AddMinutes((double) (60 / this.m_OutlookWeekCalendar.HourLines));
                if (time.Day == this.m_SelectionStart.Day)
                {
                    if (time < this.m_SelectionStart)
                    {
                        this.m_OutlookWeekCalendar.SelectionStart = time;
                        this.m_OutlookWeekCalendar.SelectionEnd = this.m_SelectionStart;
                    }
                    else
                    {
                        this.m_OutlookWeekCalendar.SelectionEnd = time;
                    }
                }
                this.m_OutlookWeekCalendar.Invalidate();
            }
        }

        public void MouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.m_OutlookWeekCalendar.Capture = false;
                if (this.m_SelectionStarted && !this.m_ToolTipClicked)
                {
                    this.m_OutlookWeekCalendar.RaiseSelectionChanged(SelectionEventArgs.SelectionType.ByMouse);
                }
                this.m_SelectionStarted = false;
                this.m_ToolTipClicked = false;
                if (this.Complete != null)
                {
                    this.Complete(this, EventArgs.Empty);
                }
            }
        }

        public void Reset()
        {
            this.m_SelectionStarted = false;
        }

        public OutlookWeekCalendar OutlookWeekCalendarControl
        {
            get
            {
                return this.m_OutlookWeekCalendar;
            }
            set
            {
                this.m_OutlookWeekCalendar = value;
            }
        }

        public bool ToolTipClicked
        {
            get
            {
                return this.m_ToolTipClicked;
            }
            set
            {
                this.m_ToolTipClicked = value;
            }
        }
    }
}

