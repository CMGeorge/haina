namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;

    public class DrawEventArgs : EventArgs
    {
        private CustomAppointment m_Appointment;
        private System.Drawing.Graphics m_Graphics;
        private int m_GripWidth;
        private bool m_IsSelected;
        private Rectangle m_Rect;

        public DrawEventArgs(System.Drawing.Graphics aGraphics, Rectangle aRect, CustomAppointment anAppointment, bool anIsSelected, int aGripWidth)
        {
            this.m_Graphics = aGraphics;
            this.m_Rect = aRect;
            this.m_Appointment = anAppointment;
            this.m_IsSelected = anIsSelected;
            this.m_GripWidth = aGripWidth;
        }

        public CustomAppointment Appointment
        {
            get
            {
                return this.m_Appointment;
            }
        }

        public System.Drawing.Graphics Graphics
        {
            get
            {
                return this.m_Graphics;
            }
        }

        public bool IsSelectedApp
        {
            get
            {
                return this.m_IsSelected;
            }
        }

        public Rectangle Rect
        {
            get
            {
                return this.m_Rect;
            }
        }
    }
}

