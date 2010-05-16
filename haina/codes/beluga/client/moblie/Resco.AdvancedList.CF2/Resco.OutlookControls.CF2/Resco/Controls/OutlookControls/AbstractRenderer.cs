namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public abstract class AbstractRenderer
    {
        protected int m_AppointmentMinimalWidth = 40;
        private Color m_BackColor = SystemColors.Control;
        private Font m_FontBase;
        private Font m_FontHeader;
        private Font m_FontHour;
        private Font m_FontMinute;
        private Color m_HalfHourSeperatorColor = SystemColors.GrayText;
        private Color m_HourColor;
        private Color m_HourLabelBGColor = SystemColors.Control;
        private Color m_HourLabelColor = SystemColors.ControlText;
        private Color m_HourLabelSelBGColor = SystemColors.Highlight;
        private Color m_HourLabelSelColor = SystemColors.HighlightText;
        private Color m_HourSeperatorColor = SystemColors.GrayText;
        private ImageList m_Icons;
        private Color m_SaturdayBackColor;
        private Color m_SelectionColor = SystemColors.Highlight;
        private Color m_SundayBackColor;
        protected bool m_useGradient;
        private Color m_WorkingHourColor;

        public event DrawAppointmentEventHandler ResolveDrawAppointment;

        public event DrawAppointmentEventHandler ResolveDrawGripper;

        protected AbstractRenderer()
        {
        }

        public abstract string DayHeader(DateTime aDate, int aMaxWidth);
        public abstract void DrawAllDayAppointment(Graphics gr, CustomAppointment app, ref Rectangle borderRect, bool anIsSelected);
        public abstract void DrawAppointment(Graphics g, Rectangle rect, CustomAppointment appointment, bool isSelected, int gripWidth, int hourLines, int halfHourHeight);
        public abstract void DrawDayBackground(Graphics g, Rectangle rect);
        public virtual void DrawDayGripper(Graphics g, Rectangle rect, int gripWidth)
        {
            using (Pen pen = new Pen(SystemColors.InactiveBorder))
            {
                g.DrawLine(pen, rect.Left, rect.Top - 1, rect.Left, rect.Height);
            }
        }

        public abstract void DrawDayHeader(Graphics g, Rectangle rect, DateTime date);
        public abstract void DrawDayHeader(Graphics g, Rectangle rect, string text);
        public abstract void DrawHalfHourLabel(Graphics g, Rectangle rect, string aText, bool selected);
        public abstract void DrawHourLabel(Graphics g, Rectangle rect, int hour, bool selected);
        public virtual void DrawHourRange(Graphics g, Rectangle rect, bool drawBorder, bool highlight)
        {
            using (SolidBrush brush = new SolidBrush(highlight ? this.SelectionColor : this.WorkingHourColor))
            {
                g.FillRectangle(brush, rect);
            }
            if (drawBorder)
            {
                Pen pen = new Pen(SystemColors.WindowFrame);
                g.DrawRectangle(pen, rect);
                pen.Dispose();
                pen = null;
            }
        }

        public static Color InterpolateColors(Color color1, Color color2, float percentage)
        {
            int r = color1.R;
            int g = color1.G;
            int b = color1.B;
            int num4 = color2.R;
            int num5 = color2.G;
            int num6 = color2.B;
            byte red = Convert.ToByte((float) (r + ((num4 - r) * percentage)));
            byte green = Convert.ToByte((float) (g + ((num5 - g) * percentage)));
            byte blue = Convert.ToByte((float) (b + ((num6 - b) * percentage)));
            return Color.FromArgb(red, green, blue);
        }

        internal virtual bool OnResolveDrawAppointment(DrawEventArgs e)
        {
            if (this.ResolveDrawAppointment != null)
            {
                this.ResolveDrawAppointment(this, e);
                return true;
            }
            return false;
        }

        internal virtual bool OnResolveDrawGripper(DrawEventArgs e)
        {
            if (this.ResolveDrawGripper != null)
            {
                this.ResolveDrawGripper(this, e);
                return true;
            }
            return false;
        }

        public virtual ImageList AppIcons
        {
            get
            {
                return this.m_Icons;
            }
            set
            {
                this.m_Icons = value;
            }
        }

        public int AppointmentMinimalWidth
        {
            get
            {
                return this.m_AppointmentMinimalWidth;
            }
            set
            {
                if (this.m_AppointmentMinimalWidth != value)
                {
                    this.m_AppointmentMinimalWidth = value;
                }
            }
        }

        public virtual Color BackColor
        {
            get
            {
                return this.m_BackColor;
            }
            internal set
            {
                this.m_BackColor = value;
            }
        }

        public virtual Font FontBase
        {
            get
            {
                if (this.m_FontBase == null)
                {
                    this.m_FontBase = new Font("Tahoma", 8f, FontStyle.Bold);
                }
                return this.m_FontBase;
            }
            internal set
            {
                this.m_FontBase = value;
            }
        }

        public virtual Font FontHeader
        {
            get
            {
                if ((this.m_FontHeader == null) || (this.FontBase.Name != this.m_FontHeader.Name))
                {
                    this.m_FontHeader = new Font(this.FontBase.Name, 10f, FontStyle.Regular);
                }
                return this.m_FontHeader;
            }
            internal set
            {
                this.m_FontHeader = value;
            }
        }

        public virtual Font FontHour
        {
            get
            {
                if ((this.m_FontHour == null) || (this.FontBase.Name != this.m_FontHour.Name))
                {
                    this.m_FontHour = new Font(this.FontBase.Name, 8f, FontStyle.Bold);
                }
                return this.m_FontHour;
            }
            internal set
            {
                this.m_FontHour = value;
            }
        }

        public virtual Font FontMinute
        {
            get
            {
                if ((this.m_FontMinute == null) || (this.FontBase.Name != this.m_FontMinute.Name))
                {
                    this.m_FontMinute = new Font(this.FontBase.Name, 5f, FontStyle.Regular);
                }
                return this.m_FontMinute;
            }
            internal set
            {
                this.m_FontMinute = value;
            }
        }

        public virtual Color HalfHourSeperatorColor
        {
            get
            {
                return this.m_HalfHourSeperatorColor;
            }
            internal set
            {
                this.m_HalfHourSeperatorColor = value;
            }
        }

        public virtual Color HourColor
        {
            get
            {
                if (this.m_HourColor == Color.Empty)
                {
                    this.m_HourColor = Color.FromArgb(0xff, 0xf4, 0xbc);
                }
                return this.m_HourColor;
            }
            internal set
            {
                this.m_HourColor = value;
            }
        }

        public virtual Color HourLabelBGColor
        {
            get
            {
                return this.m_HourLabelBGColor;
            }
            internal set
            {
                this.m_HourLabelBGColor = value;
            }
        }

        public virtual Color HourLabelColor
        {
            get
            {
                return this.m_HourLabelColor;
            }
            internal set
            {
                this.m_HourLabelColor = value;
            }
        }

        public virtual Color HourLabelSelectedBGColor
        {
            get
            {
                return this.m_HourLabelSelBGColor;
            }
            internal set
            {
                this.m_HourLabelSelBGColor = value;
            }
        }

        public virtual Color HourLabelSelectedColor
        {
            get
            {
                return this.m_HourLabelSelColor;
            }
            internal set
            {
                this.m_HourLabelSelColor = value;
            }
        }

        public virtual Color HourSeperatorColor
        {
            get
            {
                return this.m_HourSeperatorColor;
            }
            internal set
            {
                this.m_HourSeperatorColor = value;
            }
        }

        public Color SaturdayBackColor
        {
            get
            {
                if (this.m_SaturdayBackColor == Color.Empty)
                {
                    this.m_SaturdayBackColor = SystemColors.Control;
                }
                return this.m_SaturdayBackColor;
            }
            internal set
            {
                this.m_SaturdayBackColor = value;
            }
        }

        public virtual Color SelectionColor
        {
            get
            {
                return this.m_SelectionColor;
            }
            internal set
            {
                this.m_SelectionColor = value;
            }
        }

        public Color SundayBackColor
        {
            get
            {
                if (this.m_SundayBackColor == Color.Empty)
                {
                    this.m_SundayBackColor = SystemColors.Control;
                }
                return this.m_SundayBackColor;
            }
            internal set
            {
                this.m_SundayBackColor = value;
            }
        }

        public bool UseGradient
        {
            get
            {
                return this.m_useGradient;
            }
            set
            {
                if (this.m_useGradient != value)
                {
                    this.m_useGradient = value;
                }
            }
        }

        public virtual Color WorkingHourColor
        {
            get
            {
                if (this.m_WorkingHourColor == Color.Empty)
                {
                    this.m_WorkingHourColor = SystemColors.Info;
                }
                return this.m_WorkingHourColor;
            }
            internal set
            {
                this.m_WorkingHourColor = value;
            }
        }
    }
}

