namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.DetailViewInternal;
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DVDateTimePicker : Control
    {
        public bool DisableEvents;
        private bool m_AllowNone;
        private DateTimePickerEx m_DateTime;
        private string m_Format;
        private ItemDateTime m_ParentItem;
        private RescoDateTimePickerStyle m_Style;
        private DateTimePickerEx m_TimeCombo;

        public event ItemEventHandler ValueChanged;

        internal DVDateTimePicker()
        {
            this.ParentItem = null;
            this.DisableEvents = false;
            this.m_DateTime = new DateTimePickerEx();
            base.Controls.Add(this.m_DateTime);
            this.m_DateTime.BringToFront();
            this.m_TimeCombo = new DateTimePickerEx();
            this.m_TimeCombo.Visible = false;
            this.m_TimeCombo.Bounds = new Rectangle(0, 0, 60, 0);
            this.m_TimeCombo.Format = Resco.Controls.DetailView.DateTimePickerFormat.CustomTime;
            this.m_TimeCombo.TimePickerStartTime = ItemDateTime.DTTimeFrom;
            this.m_TimeCombo.TimePickerMinuteInterval = ItemDateTime.DTIncrement;
            this.m_TimeCombo.TimePickerEndTime = ItemDateTime.DTTimeTo;
            base.Controls.Add(this.m_TimeCombo);
            this.Style = RescoDateTimePickerStyle.DateTime;
            this.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            this.m_TimeCombo.ValueChanged += new EventHandler(this.OnValueChanged);
            this.m_TimeCombo.NoneSelected += new EventHandler(this.OnValueChanged);
            this.m_DateTime.ValueChanged += new EventHandler(this.OnValueChanged);
            this.m_DateTime.NoneSelected += new EventHandler(this.OnValueChanged);
            this.m_TimeCombo.KeyDown += new KeyEventHandler(this.m_TimeCombo_KeyDown);
            this.m_TimeCombo.KeyPress += new KeyPressEventHandler(this.m_TimeCombo_KeyPress);
            this.m_TimeCombo.KeyUp += new KeyEventHandler(this.m_TimeCombo_KeyUp);
            this.m_DateTime.KeyDown += new KeyEventHandler(this.m_DateTime_KeyDown);
            this.m_DateTime.KeyPress += new KeyPressEventHandler(this.m_DateTime_KeyPress);
            this.m_DateTime.KeyUp += new KeyEventHandler(this.m_DateTime_KeyUp);
            this.m_TimeCombo.GotFocus += new EventHandler(this.m_textBox_GotFocus);
            this.m_TimeCombo.LostFocus += new EventHandler(this.m_textBox_LostFocus);
            this.m_DateTime.GotFocus += new EventHandler(this.m_textBox_GotFocus);
            this.m_DateTime.LostFocus += new EventHandler(this.m_textBox_LostFocus);
            this.AllowNone = true;
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
            if (this.AllowNone)
            {
                if ((this.Style == RescoDateTimePickerStyle.Date) && !this.m_DateTime.Checked)
                {
                    return null;
                }
                if ((this.Style == RescoDateTimePickerStyle.DateTime) && (!this.m_DateTime.Checked || !this.m_TimeCombo.Checked))
                {
                    return null;
                }
                if ((this.Style == RescoDateTimePickerStyle.Time) && !this.m_TimeCombo.Checked)
                {
                    return null;
                }
            }
            DateTime time = this.m_DateTime.Value;
            if (this.Style == RescoDateTimePickerStyle.Date)
            {
                return time;
            }
            return new DateTime(time.Year, time.Month, time.Day, this.m_TimeCombo.Value.Hour, this.m_TimeCombo.Value.Minute, this.m_TimeCombo.Value.Second, this.m_TimeCombo.Value.Millisecond);
        }

        public void Hide()
        {
            if (this.ParentItem != null)
            {
                this.ParentItem.DisableEvents = true;
                ItemDateTime parentItem = this.ParentItem;
                this.ValueChanged = (ItemEventHandler) Delegate.Remove(this.ValueChanged, new ItemEventHandler(parentItem.OnChanged));
                try
                {
                    this.ParentItem.EditValue = this.GetDate();
                }
                catch
                {
                }
                this.ParentItem.DisableEvents = false;
            }
            base.Visible = false;
            this.m_DateTime.Hide();
            this.m_TimeCombo.Hide();
        }

        private void m_DateTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (!this.DroppedDown)
            {
                if (e.KeyCode == Keys.Left)
                {
                    e = new KeyEventArgs(Keys.Up);
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (this.Style == RescoDateTimePickerStyle.Date)
                    {
                        e = new KeyEventArgs(Keys.Down);
                    }
                    else
                    {
                        this.m_TimeCombo.Focus();
                        this.m_TimeCombo.SelectNext();
                        return;
                    }
                }
            }
            base.OnKeyDown(e);
        }

        private void m_DateTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void m_DateTime_KeyUp(object sender, KeyEventArgs e)
        {
            if (!this.DroppedDown)
            {
                if (e.KeyCode == Keys.Left)
                {
                    e = new KeyEventArgs(Keys.Up);
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (this.Style == RescoDateTimePickerStyle.Date)
                    {
                        e = new KeyEventArgs(Keys.Down);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            base.OnKeyUp(e);
        }

        private void m_textBox_GotFocus(object sender, EventArgs e)
        {
            this.OnGotFocus(e);
        }

        private void m_textBox_LostFocus(object sender, EventArgs e)
        {
            this.OnLostFocus(e);
        }

        private void m_TimeCombo_KeyDown(object sender, KeyEventArgs e)
        {
            if (!this.DroppedDown)
            {
                if (e.KeyCode == Keys.Left)
                {
                    if (this.Style != RescoDateTimePickerStyle.Time)
                    {
                        this.m_DateTime.Focus();
                        this.m_DateTime.SelectPrevious();
                        return;
                    }
                    e = new KeyEventArgs(Keys.Up);
                }
                else if (e.KeyCode == Keys.Right)
                {
                    e = new KeyEventArgs(Keys.Down);
                }
            }
            base.OnKeyDown(e);
        }

        private void m_TimeCombo_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void m_TimeCombo_KeyUp(object sender, KeyEventArgs e)
        {
            if (!this.DroppedDown)
            {
                if (e.KeyCode == Keys.Left)
                {
                    if (this.Style != RescoDateTimePickerStyle.Time)
                    {
                        return;
                    }
                    e = new KeyEventArgs(Keys.Up);
                }
                else if (e.KeyCode == Keys.Right)
                {
                    e = new KeyEventArgs(Keys.Down);
                }
            }
            base.OnKeyUp(e);
        }

        protected override void OnResize(EventArgs e)
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

        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            if ((this.ParentItem != null) && !this.DisableEvents)
            {
                try
                {
                    this.ParentItem.EditValue = this.GetDate();
                    if ((this.ParentItem.EditValue == null) && (this.Style != RescoDateTimePickerStyle.Time))
                    {
                        base.Focus();
                        this.m_TimeCombo.Enabled = false;
                    }
                    else
                    {
                        this.m_TimeCombo.Enabled = true;
                    }
                    this.m_TimeCombo.Checked = this.m_DateTime.Checked;
                }
                catch
                {
                }
            }
        }

        internal void SetDate(object objNewDate)
        {
            this.m_DateTime.Checked = true;
            this.m_DateTime.Value = Convert.ToDateTime(objNewDate);
            try
            {
                this.m_TimeCombo.Value = Convert.ToDateTime(objNewDate);
            }
            catch
            {
                this.m_TimeCombo.Value = DateTime.Now;
            }
        }

        private bool SetFormat(string strFormat)
        {
            try
            {
                this.m_DateTime.Format = Resco.Controls.DetailView.DateTimePickerFormat.Custom;
                this.m_DateTime.CustomFormat = strFormat;
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
                this.m_TimeCombo.CustomFormat = this.Format;
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
                this.m_TimeCombo.CustomFormat = ItemDateTime.GetTimeFormatPart(this.Format);
                this.m_DateTime.Focus();
            }
            else if (this.Style == RescoDateTimePickerStyle.Time)
            {
                this.m_TimeCombo.Focus();
            }
            this.m_DateTime.NoneText = this.ParentItem.NoneText;
            this.m_TimeCombo.NoneText = this.ParentItem.NoneText;
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
                if (value)
                {
                    this.m_DateTime.ShowCheckBox = true;
                    this.m_TimeCombo.ShowCheckBox = true;
                }
                else
                {
                    this.m_DateTime.ShowCheckBox = false;
                    this.m_TimeCombo.ShowCheckBox = false;
                }
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

        public DateTimePickerEx DatePicker
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
                    return this.m_TimeCombo.DroppedDown;
                }
                return (this.m_DateTime.Visible && this.m_DateTime.DroppedDown);
            }
            set
            {
                if (this.m_TimeCombo.Visible && this.m_TimeCombo.Enabled)
                {
                    this.m_TimeCombo.DroppedDown = value;
                }
                else
                {
                    this.m_DateTime.DroppedDown = value;
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

        internal ItemDateTime ParentItem
        {
            get
            {
                return this.m_ParentItem;
            }
            set
            {
                this.m_ParentItem = value;
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

        public DateTimePickerEx TimePicker
        {
            get
            {
                return this.m_TimeCombo;
            }
        }
    }
}

