namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal class TouchNavigatorTool
    {
        private bool m_bEnableTouchScrolling;
        private bool m_bTouchScrolling;
        private float m_dpiX;
        private float m_dpiY;
        private bool m_LeftRightGesture;
        private int m_MinX = 15;
        private int m_MinY = 15;
        private Point m_MousePosition = new Point(0, 0);
        private UserControl m_Parent;
        private int m_TouchAutoScrollDiffX;
        private int m_TouchAutoScrollDiffY;
        private int m_touchSensitivity = 8;
        private int m_TouchTime0;
        private int m_TouchTime1;

        public event GestureDetectedHandler GestureDetected;

        public TouchNavigatorTool(UserControl aParent)
        {
            this.m_Parent = aParent;
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
            if (this.m_bEnableTouchScrolling)
            {
                int num = e.Y - this.m_MousePosition.Y;
                int num2 = e.X - this.m_MousePosition.X;
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
                            this.m_dpiX = this.m_Parent.CurrentAutoScaleDimensions.Width / 96f;
                            this.m_dpiY = this.m_Parent.CurrentAutoScaleDimensions.Height / 96f;
                        }
                        if ((this.m_bTouchScrolling || (Math.Abs(num) >= ((int) (this.m_touchSensitivity * this.m_dpiY)))) || (Math.Abs(num2) >= ((int) (this.m_touchSensitivity * this.m_dpiX))))
                        {
                            this.m_bTouchScrolling = true;
                            this.m_MousePosition.X = e.X;
                            this.m_MousePosition.Y = e.Y;
                        }
                    }
                }
            }
        }

        public void MouseUp(MouseEventArgs e)
        {
            try
            {
                if (this.m_bEnableTouchScrolling && this.m_bTouchScrolling)
                {
                    if ((Environment.TickCount - this.m_TouchTime1) > 300)
                    {
                        this.Reset();
                    }
                    else
                    {
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
                            this.OnMouseGestureDetected((this.m_TouchAutoScrollDiffY > 0) ? GestureType.Down : GestureType.Up);
                        }
                        this.m_bTouchScrolling = false;
                    }
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

        public void Reset()
        {
            if (this.m_bEnableTouchScrolling)
            {
                this.m_TouchAutoScrollDiffY = 0;
                this.m_TouchAutoScrollDiffX = 0;
                this.m_LeftRightGesture = false;
                this.m_TouchTime0 = Environment.TickCount;
                this.m_TouchTime1 = this.m_TouchTime0;
                this.m_bTouchScrolling = false;
            }
        }

        public bool EnableTouchScrolling
        {
            get
            {
                return this.m_bEnableTouchScrolling;
            }
            set
            {
                this.m_bEnableTouchScrolling = value;
            }
        }

        public int TouchSensitivity
        {
            get
            {
                return this.m_touchSensitivity;
            }
            set
            {
                this.m_touchSensitivity = value;
            }
        }

        public delegate void GestureDetectedHandler(object sender, TouchNavigatorTool.GestureEventArgs e);

        internal class GestureEventArgs : EventArgs
        {
            private TouchNavigatorTool.GestureType m_Gesture;

            public GestureEventArgs(TouchNavigatorTool.GestureType aGesture)
            {
                this.m_Gesture = aGesture;
            }

            public TouchNavigatorTool.GestureType Gesture
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

