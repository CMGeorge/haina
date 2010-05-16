namespace Resco.Controls.OutlookControls
{
    using System;
    using TouchScrolling;

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
}

