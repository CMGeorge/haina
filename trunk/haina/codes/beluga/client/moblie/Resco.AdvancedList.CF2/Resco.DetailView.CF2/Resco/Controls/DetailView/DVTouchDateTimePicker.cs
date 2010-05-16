namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.DetailViewInternal;
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DVTouchDateTimePicker : Control
    {
        public bool DisableEvents;
        private bool m_AllowNone;
        private Control m_DateTime = (Activator.CreateInstance(DateTimePickerInterface.GetTouchDateTimePickerType()) as Control);
        private string m_Format;
        private string m_noneText;
        private RescoDateTimePickerStyle m_Style;
        private Control m_TimeCombo;

        public event EventHandler ValueChanged;

        internal DVTouchDateTimePicker()
        {
            DateTimePickerInterface.InvokeSetProperty(this.m_DateTime, "BorderStyle", BorderStyle.FixedSingle);
            DateTimePickerInterface.InvokeSetProperty(this.m_DateTime, "ShowFocus", false);
            DateTimePickerInterface.InvokeAddEvent(this.m_DateTime, "ValueChanged", new EventHandler(this.OnValueChanged));
            base.Controls.Add(this.m_DateTime);
            this.m_DateTime.BringToFront();
            this.m_TimeCombo = Activator.CreateInstance(DateTimePickerInterface.GetTouchDateTimePickerType()) as Control;
            this.m_TimeCombo.Visible = false;
            this.m_TimeCombo.Bounds = new Rectangle(0, 0, 60, 0);
            DateTimePickerInterface.InvokeSetProperty(this.m_TimeCombo, "Format", 0x10);
            DateTimePickerInterface.InvokeSetProperty(this.m_TimeCombo, "BorderStyle", BorderStyle.FixedSingle);
            DateTimePickerInterface.InvokeSetProperty(this.m_TimeCombo, "ShowFocus", false);
            DateTimePickerInterface.InvokeAddEvent(this.m_TimeCombo, "ValueChanged", new EventHandler(this.OnValueChanged));
            base.Controls.Add(this.m_TimeCombo);
            this.Style = RescoDateTimePickerStyle.DateTime;
            this.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            this.AllowNone = true;
            this.NoneText = "None";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_DateTime != null)
                {
                    this.m_DateTime.Dispose();
                    this.m_DateTime = null;
                }
                if (this.m_TimeCombo != null)
                {
                    this.m_TimeCombo.Dispose();
                    this.m_TimeCombo = null;
                }
            }
            base.Dispose(disposing);
        }

        private object GetDate()
        {
            if (this.AllowNone && ((bool) DateTimePickerInterface.InvokeGetProperty(this.m_DateTime, "IsNone")))
            {
                return null;
            }
            DateTime time = (DateTime) DateTimePickerInterface.InvokeGetProperty(this.m_DateTime, "Value");
            if (this.Style == RescoDateTimePickerStyle.Date)
            {
                return time;
            }
            DateTime time2 = (DateTime) DateTimePickerInterface.InvokeGetProperty(this.m_TimeCombo, "Value");
            return new DateTime(time.Year, time.Month, time.Day, time2.Hour, time2.Minute, time2.Second, time2.Millisecond);
        }

        public void Hide()
        {
            this.DroppedDown = false;
            this.m_DateTime.Hide();
            this.m_TimeCombo.Hide();
            base.Hide();
        }

        protected override void OnResize(EventArgs e)
        {
            if ((this.m_DateTime == null) || (this.m_TimeCombo == null))
            {
                base.OnResize(e);
            }
            else
            {
                if (this.Style == RescoDateTimePickerStyle.Time)
                {
                    this.m_TimeCombo.Left = 0;
                    this.m_TimeCombo.Size = new Size(base.Width, base.Height);
                }
                else if (this.Style == RescoDateTimePickerStyle.Date)
                {
                    this.m_DateTime.Left = 0;
                    this.m_DateTime.Size = new Size(base.Width, base.Height);
                }
                else
                {
                    this.m_TimeCombo.Left = ItemDateTime.DTWidth;
                    this.m_TimeCombo.Size = new Size(base.Width - ItemDateTime.DTWidth, base.Height);
                }
                base.OnResize(e);
            }
        }

        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            if (!this.DisableEvents)
            {
                if ((this.GetDate() == null) && (this.Style != RescoDateTimePickerStyle.Time))
                {
                    this.m_TimeCombo.Enabled = false;
                }
                else
                {
                    this.m_TimeCombo.Enabled = true;
                }
                if (this.AllowNone)
                {
                    DateTimePickerInterface.InvokeSetProperty(this.m_TimeCombo, "IsNone", DateTimePickerInterface.InvokeGetProperty(this.m_DateTime, "IsNone"));
                }
                else
                {
                    DateTimePickerInterface.InvokeSetProperty(this.m_DateTime, "IsNone", false);
                    DateTimePickerInterface.InvokeSetProperty(this.m_TimeCombo, "IsNone", false);
                }
                this.m_TimeCombo.Invalidate();
                if (this.ValueChanged != null)
                {
                    this.ValueChanged(this, EventArgs.Empty);
                }
            }
        }

        private void SetDate(object objNewDate)
        {
            DateTimePickerInterface.InvokeSetProperty(this.m_DateTime, "IsNone", false);
            DateTimePickerInterface.InvokeSetProperty(this.m_TimeCombo, "IsNone", false);
            DateTimePickerInterface.InvokeSetProperty(this.m_DateTime, "Value", Convert.ToDateTime(objNewDate));
            try
            {
                DateTimePickerInterface.InvokeSetProperty(this.m_TimeCombo, "Value", Convert.ToDateTime(objNewDate));
            }
            catch
            {
                DateTimePickerInterface.InvokeSetProperty(this.m_TimeCombo, "Value", DateTime.Now);
            }
        }

        private bool SetFormat(string strFormat)
        {
            try
            {
                DateTimePickerInterface.InvokeSetProperty(this.m_DateTime, "Format", 8);
                DateTimePickerInterface.InvokeSetProperty(this.m_DateTime, "CustomFormat", strFormat);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Show()
        {
            base.Visible = true;
            base.Focus();
            if (this.Style == RescoDateTimePickerStyle.Time)
            {
                this.m_DateTime.Hide();
                this.m_TimeCombo.Left = 0;
                this.m_TimeCombo.Width = base.Width;
                DateTimePickerInterface.InvokeSetProperty(this.m_TimeCombo, "CustomFormat", this.Format);
            }
            else
            {
                this.m_DateTime.Size = new Size(ItemDateTime.DTWidth, base.Height);
                this.m_DateTime.Show();
                this.m_TimeCombo.Left = ItemDateTime.DTWidth;
                this.m_TimeCombo.Width = base.Width - ItemDateTime.DTWidth;
            }
            if (this.Style == RescoDateTimePickerStyle.Date)
            {
                this.m_TimeCombo.Hide();
                this.m_DateTime.Size = new Size(base.Width, base.Height);
                this.m_DateTime.Show();
                this.m_DateTime.Focus();
            }
            else
            {
                this.m_TimeCombo.Show();
            }
            if (this.Style == RescoDateTimePickerStyle.DateTime)
            {
                this.SetFormat(ItemDateTime.GetDateFormatPart(this.Format));
                DateTimePickerInterface.InvokeSetProperty(this.m_TimeCombo, "CustomFormat", ItemDateTime.GetTimeFormatPart(this.Format));
                this.m_DateTime.Focus();
            }
            else if (this.Style == RescoDateTimePickerStyle.Time)
            {
                this.m_TimeCombo.Focus();
            }
        }

        public bool AllowNone
        {
            get
            {
                return this.m_AllowNone;
            }
            set
            {
                this.m_AllowNone = value;
            }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                if (this.m_DateTime != null)
                {
                    this.m_DateTime.BackColor = value;
                }
                if (this.m_TimeCombo != null)
                {
                    this.m_TimeCombo.BackColor = value;
                }
                base.BackColor = value;
            }
        }

        public object Date
        {
            get
            {
                return this.GetDate();
            }
            set
            {
                this.DisableEvents = true;
                this.SetDate(value);
                this.DisableEvents = false;
            }
        }

        public Control DatePicker
        {
            get
            {
                return this.m_DateTime;
            }
        }

        public bool DroppedDown
        {
            get
            {
                if (this.m_TimeCombo.Visible)
                {
                    return (bool) DateTimePickerInterface.InvokeGetProperty(this.m_TimeCombo, "DropDownVisible");
                }
                return (this.m_DateTime.Visible && ((bool) DateTimePickerInterface.InvokeGetProperty(this.m_DateTime, "DropDownVisible")));
            }
            set
            {
                if (this.m_TimeCombo.Visible && this.m_TimeCombo.Enabled)
                {
                    DateTimePickerInterface.InvokeSetProperty(this.m_TimeCombo, "DropDownVisible", value);
                }
                if (this.m_DateTime.Visible && this.m_DateTime.Enabled)
                {
                    DateTimePickerInterface.InvokeSetProperty(this.m_DateTime, "DropDownVisible", value);
                }
            }
        }

        public bool Focused
        {
            get
            {
                if (!base.Focused && !this.m_TimeCombo.Focused)
                {
                    return this.m_DateTime.Focused;
                }
                return true;
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                if (this.m_DateTime != null)
                {
                    this.m_DateTime.Font = value;
                }
                if (this.m_TimeCombo != null)
                {
                    this.m_TimeCombo.Font = value;
                }
                base.Font = value;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                if (this.m_DateTime != null)
                {
                    this.m_DateTime.ForeColor = value;
                }
                if (this.m_TimeCombo != null)
                {
                    this.m_TimeCombo.ForeColor = value;
                }
                base.ForeColor = value;
            }
        }

        public string Format
        {
            get
            {
                return this.m_Format;
            }
            set
            {
                this.SetFormat(value);
                this.m_Format = value;
            }
        }

        public string NoneText
        {
            get
            {
                return this.m_noneText;
            }
            set
            {
                this.m_noneText = value;
                DateTimePickerInterface.InvokeSetProperty(this.m_DateTime, "NoneText", value);
                DateTimePickerInterface.InvokeSetProperty(this.m_TimeCombo, "NoneText", value);
            }
        }

        public RescoDateTimePickerStyle Style
        {
            get
            {
                return this.m_Style;
            }
            set
            {
                this.m_Style = value;
            }
        }

        public Control TimePicker
        {
            get
            {
                return this.m_TimeCombo;
            }
        }

        public string TodayText
        {
            get
            {
                return (string) DateTimePickerInterface.InvokeGetProperty(this.m_DateTime, "TodayText");
            }
            set
            {
                DateTimePickerInterface.InvokeSetProperty(this.m_DateTime, "TodayText", value);
            }
        }
    }
}

