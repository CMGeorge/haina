namespace TouchScrolling
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal class OsbTouchTool
    {
        private static bool m_bEnableTouchScrolling;
        private bool m_bTouchScrolling;
        private float m_dpiX;
        private float m_dpiY;
        private bool m_LeftRightGesture;
        private int m_MinX = 50;
        private int m_MinY = 50;
        private Point m_MousePosition = new Point(0, 0);
        private static UserControl m_Parent;
        private int m_TouchAutoScrollDiffX;
        private int m_TouchAutoScrollDiffY;
        private Timer m_TouchScrollingTimer;
        private static int m_touchSensitivity;
        private int m_TouchTime0;
        private int m_TouchTime1;

        internal event GestureDetectedHandler GestureDetected;

        internal event MouseMoveDetectedHandler MouseMoveDetected;

        public OsbTouchTool(UserControl aParent)
        {
            m_Parent = aParent;
            this.m_TouchScrollingTimer = new Timer();
            this.m_TouchScrollingTimer.Enabled = false;
            this.m_TouchScrollingTimer.Interval = 100;
            this.m_TouchScrollingTimer.Tick += new EventHandler(this.OnTouchScrollingTimerTick);
            this.m_bTouchScrolling = false;
            this.m_TouchAutoScrollDiffY = 0;
            this.m_TouchAutoScrollDiffX = 0;
            this.m_LeftRightGesture = false;
        }

        public void Dispose()
        {
            if (this.m_TouchScrollingTimer != null)
            {
                this.m_TouchScrollingTimer.Tick -= new EventHandler(this.OnTouchScrollingTimerTick);
                this.m_TouchScrollingTimer.Dispose();
                this.m_TouchScrollingTimer = null;
            }
        }

        public void MouseDown(int aX, int aY)
        {
            this.m_MousePosition.X = aX;
            this.m_MousePosition.Y = aY;
            this.Reset();
        }

        public void MouseMove(int aX, int aY)
        {
            if (m_bEnableTouchScrolling)
            {
                int num = aY - this.m_MousePosition.Y;
                int num2 = aX - this.m_MousePosition.X;
                if ((Math.Abs(num) >= this.m_MinY) || (Math.Abs(num2) >= this.m_MinX))
                {
                    this.m_TouchAutoScrollDiffY += num;
                    this.m_TouchAutoScrollDiffX += num2;
                    if ((Environment.TickCount - this.m_TouchTime1) > 300)
                    {
                        this.m_TouchAutoScrollDiffY = 0;
                        this.m_TouchAutoScrollDiffX = 0;
                        this.m_TouchTime0 = Environment.TickCount;
                        this.m_TouchTime1 = this.m_TouchTime0;
                        this.m_bTouchScrolling = false;
                    }
                    else
                    {
                        this.m_TouchTime1 = Environment.TickCount;
                        if ((this.m_dpiY == 0f) || (this.m_dpiX == 0f))
                        {
                            using (Graphics graphics = m_Parent.CreateGraphics())
                            {
                                this.m_dpiX = graphics.DpiX / 96f;
                                this.m_dpiY = graphics.DpiY / 96f;
                            }
                        }
                        if ((this.m_bTouchScrolling || (Math.Abs(num) >= ((int) (m_touchSensitivity * this.m_dpiY)))) || (Math.Abs(num2) >= ((int) (m_touchSensitivity * this.m_dpiX))))
                        {
                            this.m_bTouchScrolling = true;
                            this.m_MousePosition.X = aX;
                            this.m_MousePosition.Y = aY;
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
                                GestureType gestureType = (this.m_TouchAutoScrollDiffY > 0) ? GestureType.Down : GestureType.Up;
                                this.OnMouseMoveDetected(gestureType, num2, -num);
                            }
                        }
                    }
                }
            }
        }

        public bool MouseUp(int aX, int aY)
        {
            bool flag = false;
            try
            {
                if (!m_bEnableTouchScrolling || !this.m_bTouchScrolling)
                {
                    return flag;
                }
                if ((Environment.TickCount - this.m_TouchTime1) > 300)
                {
                    this.Reset();
                    return false;
                }
                if (Math.Abs(this.m_TouchAutoScrollDiffX) < Math.Abs(this.m_TouchAutoScrollDiffY))
                {
                    this.m_LeftRightGesture = false;
                }
                else
                {
                    this.m_LeftRightGesture = true;
                }
                if (this.m_LeftRightGesture)
                {
                    this.OnMouseGestureDetected((this.m_TouchAutoScrollDiffX > 0) ? GestureType.Right : GestureType.Left);
                }
                else
                {
                    int num = (Environment.TickCount - this.m_TouchTime1) / 150;
                    if (num > 0)
                    {
                        this.m_TouchAutoScrollDiffY /= num;
                    }
                    this.m_TouchScrollingTimer.Enabled = true;
                }
                flag = true;
                this.m_bTouchScrolling = false;
            }
            catch (ObjectDisposedException)
            {
            }
            return flag;
        }

        private void OnMouseGestureDetected(GestureType gestureType)
        {
            if (this.GestureDetected != null)
            {
                GestureEventArgs e = new GestureEventArgs(gestureType);
                this.GestureDetected(this, e);
            }
        }

        private void OnMouseMoveDetected(GestureType gestureType, int aMoveX, int aMoveY)
        {
            if (this.MouseMoveDetected != null)
            {
                MouseMoveEventArgs e = new MouseMoveEventArgs(gestureType, aMoveX, aMoveY);
                this.MouseMoveDetected(this, e);
            }
        }

        private void OnTouchScrollingTimerTick(object sender, EventArgs e)
        {
            if (this.m_TouchAutoScrollDiffY < 0)
            {
                this.OnMouseMoveDetected(GestureType.Up, 0, -this.m_TouchAutoScrollDiffY);
                this.m_TouchAutoScrollDiffY += (Math.Abs(this.m_TouchAutoScrollDiffY) / 10) + 1;
                if (this.m_TouchAutoScrollDiffY > 0)
                {
                    this.m_TouchAutoScrollDiffY = 0;
                    this.m_TouchScrollingTimer.Enabled = false;
                }
            }
            else if (this.m_TouchAutoScrollDiffY > 0)
            {
                this.OnMouseMoveDetected(GestureType.Down, 0, -this.m_TouchAutoScrollDiffY);
                this.m_TouchAutoScrollDiffY -= (Math.Abs(this.m_TouchAutoScrollDiffY) / 10) + 1;
                if (this.m_TouchAutoScrollDiffY < 0)
                {
                    this.m_TouchAutoScrollDiffY = 0;
                    this.m_TouchScrollingTimer.Enabled = false;
                }
            }
            else
            {
                this.m_TouchScrollingTimer.Enabled = false;
            }
        }

        public void Reset()
        {
            if (m_bEnableTouchScrolling)
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

        public static bool EnableTouchScrolling
        {
            get
            {
                return m_bEnableTouchScrolling;
            }
            set
            {
                m_bEnableTouchScrolling = value;
            }
        }

        public static UserControl ParentControl
        {
            get
            {
                return m_Parent;
            }
            set
            {
                m_Parent = value;
            }
        }

        public static int TouchSensitivity
        {
            get
            {
                return m_touchSensitivity;
            }
            set
            {
                m_touchSensitivity = value;
            }
        }

        internal delegate void GestureDetectedHandler(object sender, OsbTouchTool.GestureEventArgs e);

        internal class GestureEventArgs : EventArgs
        {
            private OsbTouchTool.GestureType m_Gesture;

            public GestureEventArgs(OsbTouchTool.GestureType aGesture)
            {
                this.m_Gesture = aGesture;
            }

            public OsbTouchTool.GestureType Gesture
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

        internal delegate void MouseMoveDetectedHandler(object sender, OsbTouchTool.MouseMoveEventArgs e);

        internal class MouseMoveEventArgs : EventArgs
        {
            private OsbTouchTool.GestureType m_Gesture;
            private int m_MoveX;
            private int m_MoveY;

            public MouseMoveEventArgs(OsbTouchTool.GestureType aGesture, int aMoveX, int aMoveY)
            {
                this.m_Gesture = aGesture;
                this.m_MoveX = aMoveX;
                this.m_MoveY = aMoveY;
            }

            public OsbTouchTool.GestureType Gesture
            {
                get
                {
                    return this.m_Gesture;
                }
            }

            public int MoveX
            {
                get
                {
                    return this.m_MoveX;
                }
            }

            public int MoveY
            {
                get
                {
                    return this.m_MoveY;
                }
            }
        }
    }
}

