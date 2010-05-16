namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections.Generic;

    public class ResolveAppointmentsEventArgs : EventArgs
    {
        private List<CustomAppointment> m_Appointments;
        private DateTime m_EndDate;
        private DateTime m_StartDate;

        public ResolveAppointmentsEventArgs(DateTime start, DateTime end)
        {
            this.m_StartDate = start;
            this.m_EndDate = end;
            this.m_Appointments = new List<CustomAppointment>();
        }

        public List<CustomAppointment> Appointments
        {
            get
            {
                return this.m_Appointments;
            }
            set
            {
                this.m_Appointments = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.m_EndDate;
            }
            set
            {
                this.m_EndDate = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.m_StartDate;
            }
            set
            {
                this.m_StartDate = value;
            }
        }
    }
}

