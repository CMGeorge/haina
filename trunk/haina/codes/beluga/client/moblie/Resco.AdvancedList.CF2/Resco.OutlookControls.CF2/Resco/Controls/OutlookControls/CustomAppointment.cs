namespace Resco.Controls.OutlookControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;

    public class CustomAppointment
    {
        private bool m_AllDayEvent;
        private System.Drawing.Color m_BorderColor;
        private System.Drawing.Color m_color;
        internal int m_ConflictCount;
        internal bool m_Drawn;
        private DateTime m_End;
        private DateTime m_EndDate;
        private GradientColor m_gradientBackColor;
        private int[] m_IconIndexes;
        private string m_Location;
        private DateTime m_Start;
        private DateTime m_StartDate;
        private object m_Tag;
        private System.Drawing.Color m_TextColor;
        private string m_Title;
        private string m_ToolTip;

        public CustomAppointment()
        {
            this.m_color = System.Drawing.Color.White;
            this.m_TextColor = System.Drawing.Color.Black;
            this.m_BorderColor = System.Drawing.Color.Blue;
            this.m_Title = "New Appointment";
            this.m_ToolTip = string.Empty;
        }

        public CustomAppointment(CustomAppointment anAppointment)
        {
            this.m_color = System.Drawing.Color.White;
            this.m_TextColor = System.Drawing.Color.Black;
            this.m_BorderColor = System.Drawing.Color.Blue;
            this.m_Title = "New Appointment";
            this.m_ToolTip = string.Empty;
            foreach (PropertyInfo info in Type.GetType("Resco.Controls.OutlookControls.CustomAppointment").GetProperties())
            {
                MethodInfo getMethod = info.GetGetMethod();
                MethodInfo setMethod = info.GetSetMethod();
                object obj2 = getMethod.Invoke(anAppointment, null);
                object[] parameters = new object[] { obj2 };
                setMethod.Invoke(this, parameters);
            }
        }

        public override bool Equals(object obj)
        {
            CustomAppointment appointment = obj as CustomAppointment;
            if (appointment == null)
            {
                return false;
            }
            if (!this.Title.Equals(appointment.Title))
            {
                return false;
            }
            if (!this.Start.Equals(appointment.Start))
            {
                return false;
            }
            if (!this.End.Equals(appointment.End))
            {
                return false;
            }
            if ((this.Tag != null) && !this.Tag.Equals(appointment.Tag))
            {
                return false;
            }
            if (!this.GetHashCode().Equals(appointment.GetHashCode()))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        internal virtual void OnEndDateChanged()
        {
        }

        internal virtual void OnStartDateChanged()
        {
        }

        public virtual void OnTitleChanged()
        {
        }

        public virtual void OnToolTipChanged()
        {
        }

        private DateTime SetEndDate(DateTime time)
        {
            if (((time.Ticks != 0L) && (time.Hour == 0)) && (time.Minute == 0))
            {
                return time.AddSeconds(-1.0);
            }
            return time;
        }

        public bool AllDayEvent
        {
            get
            {
                return this.m_AllDayEvent;
            }
            set
            {
                this.m_AllDayEvent = value;
            }
        }

        public System.Drawing.Color BorderColor
        {
            get
            {
                return this.m_BorderColor;
            }
            set
            {
                this.m_BorderColor = value;
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this.m_color;
            }
            set
            {
                this.m_color = value;
            }
        }

        public DateTime End
        {
            get
            {
                return this.m_End;
            }
            set
            {
                this.m_End = value;
                this.EndDate = this.m_End;
            }
        }

        internal DateTime EndDate
        {
            get
            {
                return this.m_EndDate;
            }
            set
            {
                this.m_EndDate = this.SetEndDate(value);
                this.OnEndDateChanged();
            }
        }

        public GradientColor GradientBackColor
        {
            get
            {
                return this.m_gradientBackColor;
            }
            set
            {
                this.m_gradientBackColor = value;
            }
        }

        public int[] IconIndexes
        {
            get
            {
                return this.m_IconIndexes;
            }
            set
            {
                this.m_IconIndexes = value;
            }
        }

        public string Location
        {
            get
            {
                return this.m_Location;
            }
            set
            {
                this.m_Location = value;
            }
        }

        public DateTime Start
        {
            get
            {
                return this.m_Start;
            }
            set
            {
                this.m_Start = value;
                this.StartDate = this.m_Start;
            }
        }

        internal DateTime StartDate
        {
            get
            {
                return this.m_StartDate;
            }
            set
            {
                this.m_StartDate = value;
                this.OnStartDateChanged();
            }
        }

        public object Tag
        {
            get
            {
                return this.m_Tag;
            }
            set
            {
                this.m_Tag = value;
            }
        }

        public System.Drawing.Color TextColor
        {
            get
            {
                return this.m_TextColor;
            }
            set
            {
                this.m_TextColor = value;
            }
        }

        [DefaultValue("New Appointment")]
        public string Title
        {
            get
            {
                return this.m_Title;
            }
            set
            {
                this.m_Title = value;
                this.OnTitleChanged();
            }
        }

        public string ToolTip
        {
            get
            {
                return this.m_ToolTip;
            }
            set
            {
                this.m_ToolTip = value;
                this.OnToolTipChanged();
            }
        }

        public enum EIcons
        {
            Bell,
            Recurring,
            Paper,
            House,
            People,
            Key,
            IconCount
        }
    }
}

