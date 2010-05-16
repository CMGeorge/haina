namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal class TouchTool : ITool
    {
        private bool m_bTouchScrolling;
        private float m_dpiX;
        private float m_dpiY;
        private bool m_LeftRightGesture;
        private Point m_MousePosition = new Point(0, 0);
        private OutlookWeekCalendar m_OutlookWeekCalendar;
        private int m_TouchAutoScrollDiffX;
        private int m_TouchAutoScrollDiffY;
        private Timer m_TouchScrollingTimer = new Timer();
        private int m_TouchTime0;
        private int m_TouchTime1;

        public event EventHandler Complete;

        internal event GestureDetectedHandler GestureDetected;

        public TouchTool()
        {
            this.m_TouchScrollingTimer.Enabled = false;
            this.m_TouchScrollingTimer.Interval = 100;
            this.m_TouchScrollingTimer.Tick += new EventHandler(this.OnTouchScrollingTimerTick);
            this.m_bTouchScrolling = false;
            this.m_TouchAutoScrollDiffY = 0;
            this.m_TouchAutoScrollDiffX = 0;
            this.m_LeftRightGesture = false;
        }

        public void MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.m_MousePosition.X = e.X;
                this.m_MousePosition.Y = e.Y;
                this.Reset();
            }
        }

        public void MouseMove(MouseEventArgs e)
        {
            if (this.m_OutlookWeekCalendar.TouchScrolling)
            {
                int num = e.Y - this.m_MousePosition.Y;
                int num2 = e.X - this.m_MousePosition.X;
                if ((Math.Abs(num) >= this.m_OutlookWeekCalendar.HalfHourHeight) || (Math.Abs(num2) >= this.m_OutlookWeekCalendar.HalfHourHeight))
                {
                    this.m_TouchAutoScrollDiffY += num;
                    this.m_TouchAutoScrollDiffX += num2;
                    if ((Environment.TickCount - this.m_TouchTime1) > 300)
                    {
                        this.m_TouchAutoScrollDiffY = 0;
                        this.m_TouchAutoScrollDiffX = 0;
                        this.m_TouchTime0 = Environment.TickCount;
                        this.m_TouchTime1 = this.m_TouchTime0;
                        this.m_OutlookWeekCalendar.m_activeTool = this.m_OutlookWeekCalendar.m_drawTool;
                        this.m_OutlookWeekCalendar.m_drawTool.MouseMove(e);
                        this.m_bTouchScrolling = false;
                    }
                    else
                    {
                        this.m_TouchTime1 = Environment.TickCount;
                        if (this.m_dpiY == 0f)
                        {
                            this.m_dpiY = (this.m_OutlookWeekCalendar._BackBufferGraphics == null) ? 1f : (this.m_OutlookWeekCalendar._BackBufferGraphics.DpiY / 96f);
                        }
                        if (this.m_dpiX == 0f)
                        {
                            this.m_dpiX = (this.m_OutlookWeekCalendar._BackBufferGraphics == null) ? 1f : (this.m_OutlookWeekCalendar._BackBufferGraphics.DpiX / 96f);
                        }
                        if ((this.m_bTouchScrolling || (Math.Abs(num) >= ((int) (this.m_OutlookWeekCalendar.TouchSensitivity * this.m_dpiY)))) || (Math.Abs(num2) >= ((int) (this.m_OutlookWeekCalendar.TouchSensitivity * this.m_dpiX))))
                        {
                            this.m_bTouchScrolling = true;
                            this.m_MousePosition.X = e.X;
                            this.m_MousePosition.Y = e.Y;
                            if (Math.Abs(this.m_TouchAutoScrollDiffX) < Math.Abs(this.m_TouchAutoScrollDiffY))
                            {
                                this.m_LeftRightGesture = false;
                            }
                            else
                            {
                                this.m_LeftRightGesture = true;
                            }
                            if (!this.m_LeftRightGesture)
                            {
                                this.m_OutlookWeekCalendar.ScrollbarValue -= num / this.m_OutlookWeekCalendar.HalfHourHeight;
                            }
                        }
                    }
                }
            }
        }

        public void MouseUp(MouseEventArgs e)
        {
            try
            {
                if (this.m_OutlookWeekCalendar.TouchScrolling && this.m_bTouchScrolling)
                {
                    if (this.m_LeftRightGesture)
                    {
                        this.OnMouseGestureDetected((this.m_TouchAutoScrollDiffX > 0) ? GestureType.Right : GestureType.Left);
                    }
                    else
                    {
                        if ((Environment.TickCount - this.m_TouchTime1) > 300)
                        {
                            this.m_TouchAutoScrollDiffY = 0;
                            this.m_OutlookWeekCalendar.m_drawTool.MouseUp(e);
                            this.Reset();
                            return;
                        }
                        int num = (Environment.TickCount - this.m_TouchTime1) / 15;
                        if (num > 0)
                        {
                            this.m_TouchAutoScrollDiffY /= num;
                        }
                        this.m_TouchScrollingTimer.Enabled = true;
                    }
                    this.m_bTouchScrolling = false;
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private void OnMouseGestureDetected(GestureType gestureType)
        {
            if (this.GestureDetected != null)
            {
                GestureEventArgs e = new GestureEventArgs(gestureType);
                this.GestureDetected(this, e);
            }
        }

        private void OnTouchScrollingTimerTick(object sender, EventArgs e)
        {
            if (this.m_OutlookWeekCalendar.IsDisposed)
            {
                this.StopTouchScrollingTimer();
            }
            else if (this.m_TouchAutoScrollDiffY < 0)
            {
                this.m_OutlookWeekCalendar.ScrollbarValue++;
                this.m_TouchAutoScrollDiffY += (Math.Abs(this.m_TouchAutoScrollDiffY) / 10) + 1;
                if (this.m_TouchAutoScrollDiffY > 0)
                {
                    this.m_TouchAutoScrollDiffY = 0;
                    this.StopTouchScrollingTimer();
                }
            }
            else if (this.m_TouchAutoScrollDiffY > 0)
            {
                this.m_OutlookWeekCalendar.ScrollbarValue--;
                this.m_TouchAutoScrollDiffY -= (Math.Abs(this.m_TouchAutoScrollDiffY) / 10) + 1;
                if (this.m_TouchAutoScrollDiffY < 0)
                {
                    this.m_TouchAutoScrollDiffY = 0;
                    this.StopTouchScrollingTimer();
                }
            }
            else
            {
                this.StopTouchScrollingTimer();
            }
        }

        public void Reset()
        {
            if (this.m_OutlookWeekCalendar.TouchScrolling)
            {
                this.m_TouchScrollingTimer.Enabled = false;
                this.m_TouchAutoScrollDiffY = 0;
                this.m_TouchAutoScrollDiffX = 0;
                this.m_LeftRightGesture = false;
                this.m_TouchTime0 = Environment.TickCount;
                this.m_TouchTime1 = this.m_TouchTime0;
                this.m_bTouchScrolling = false;
            }
        }

        internal void StopTouchScrollingTimer()
        {
            this.m_TouchScrollingTimer.Enabled = false;
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

        internal delegate void GestureDetectedHandler(object sender, TouchTool.GestureEventArgs e);

        internal class GestureEventArgs : EventArgs
        {
            private TouchTool.GestureType m_Gesture;

            public GestureEventArgs(TouchTool.GestureType aGesture)
            {
                this.m_Gesture = aGesture;
            }

            public TouchTool.GestureType Gesture
            {
                get
                {
                    return this.m_Gesture;
                }
            }
        }

        internal enum GestureType
        {
            Left,
            Right,
            Up,
            Down
        }
    }
}

