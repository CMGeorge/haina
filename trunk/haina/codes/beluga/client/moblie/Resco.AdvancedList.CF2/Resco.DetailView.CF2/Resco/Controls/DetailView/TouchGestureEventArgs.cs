namespace Resco.Controls.DetailView
{
    using System;

    public class TouchGestureEventArgs
    {
        private int m_difference;
        private TouchGesture m_direction;
        private bool m_handled;
        private TouchGestureType m_type;

        public TouchGestureEventArgs()
        {
            this.m_direction = TouchGesture.Up;
            this.m_handled = false;
            this.m_difference = 0;
            this.m_type = TouchGestureType.Gesture;
        }

        public TouchGestureEventArgs(TouchGesture direction)
        {
            this.m_direction = direction;
            this.m_handled = false;
            this.m_difference = 0;
            this.m_type = TouchGestureType.Gesture;
        }

        public int Difference
        {
            get
            {
                return this.m_difference;
            }
            internal set
            {
                this.m_difference = value;
            }
        }

        public TouchGesture Direction
        {
            get
            {
                return this.m_direction;
            }
            internal set
            {
                this.m_direction = value;
            }
        }

        public bool Handled
        {
            get
            {
                return this.m_handled;
            }
            set
            {
                this.m_handled = value;
            }
        }

        public static TouchGestureEventArgs Left
        {
            get
            {
                return new TouchGestureEventArgs(TouchGesture.Left);
            }
        }

        public static TouchGestureEventArgs Right
        {
            get
            {
                return new TouchGestureEventArgs(TouchGesture.Right);
            }
        }

        public TouchGestureType Type
        {
            get
            {
                return this.m_type;
            }
            internal set
            {
                this.m_type = value;
            }
        }
    }
}

