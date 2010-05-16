namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.Design;
    using Resco.Controls.DetailView.DetailViewInternal;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.Windows.Forms;

    public class ItemTimeSpan : Item
    {
        private System.Windows.Forms.Control EditControl;
        private bool m_AllowNoneTime;
        private bool m_bDefaultFormat;
        private System.TimeSpan m_endTime;
        private string m_Format;
        private ImageAttributes m_ImageAttributes;
        private double m_minuteInterval;
        private string m_NoneText;
        private bool m_showDropDown;
        private bool m_showTimeNone;
        private bool m_showTimeUpDown;
        private System.TimeSpan m_startTime;

        public ItemTimeSpan()
        {
            this.m_NoneText = "None";
            this.m_showDropDown = true;
            this.m_bDefaultFormat = true;
            this.m_Format = DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
            this.m_startTime = new System.TimeSpan(ItemDateTime.DTTimeFrom.Hour, ItemDateTime.DTTimeFrom.Minute, ItemDateTime.DTTimeFrom.Second);
            this.m_endTime = new System.TimeSpan(ItemDateTime.DTTimeTo.Hour, ItemDateTime.DTTimeTo.Minute, ItemDateTime.DTTimeTo.Second);
            this.m_minuteInterval = ItemDateTime.DTIncrement;
            this.EditValue = null;
            this.m_ImageAttributes = new ImageAttributes();
            this.m_ImageAttributes.SetColorKey(Color.White, Color.White);
        }

        public ItemTimeSpan(Item toCopy) : base(toCopy)
        {
            this.m_NoneText = "None";
            this.m_showDropDown = true;
            this.m_bDefaultFormat = true;
            this.m_Format = DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
            this.m_startTime = new System.TimeSpan(ItemDateTime.DTTimeFrom.Hour, ItemDateTime.DTTimeFrom.Minute, ItemDateTime.DTTimeFrom.Second);
            this.m_endTime = new System.TimeSpan(ItemDateTime.DTTimeTo.Hour, ItemDateTime.DTTimeTo.Minute, ItemDateTime.DTTimeTo.Second);
            this.m_minuteInterval = ItemDateTime.DTIncrement;
            this.EditValue = null;
            if (toCopy is ItemTimeSpan)
            {
                this.AllowNoneTime = ((ItemTimeSpan) toCopy).AllowNoneTime;
                this.Format = ((ItemTimeSpan) toCopy).Format;
                this.NoneText = ((ItemTimeSpan) toCopy).NoneText;
                this.ShowTimeNone = ((ItemTimeSpan) toCopy).ShowTimeNone;
                this.ShowDropDown = ((ItemTimeSpan) toCopy).ShowDropDown;
                this.ShowTimeUpDown = ((ItemTimeSpan) toCopy).ShowTimeUpDown;
                this.TimePickerStartTime = ((ItemTimeSpan) toCopy).TimePickerStartTime;
                this.TimePickerEndTime = ((ItemTimeSpan) toCopy).TimePickerEndTime;
                this.TimePickerMinuteInterval = ((ItemTimeSpan) toCopy).TimePickerMinuteInterval;
            }
            this.m_ImageAttributes = new ImageAttributes();
            this.m_ImageAttributes.SetColorKey(Color.White, Color.White);
        }

        public ItemTimeSpan(string label) : this()
        {
            this.Label = this.Label;
        }

        protected override void Click(int yOffset, int parentWidth)
        {
            if (this.Enabled)
            {
                if (this.EditControl != null)
                {
                    this.EditControl.Focus();
                    this.OnGotFocus(this, new ItemEventArgs(this, 0, base.Name));
                }
                else
                {
                    base.DisableEvents = true;
                    Resco.Controls.DetailView.DetailView parent = base.Parent;
                    System.Windows.Forms.Control control = base.Parent.GetControl(this.GetDateTimePickerType());
                    if (control != null)
                    {
                        control.ForeColor = base.GetTextForeColor();
                        control.BackColor = base.GetTextBackColor();
                        control.Font = base.TextFont;
                        control.Enabled = true;
                        DateTimePickerInterface.SetFormat(control, Resco.Controls.DetailView.DateTimePickerFormat.CustomTime);
                        DateTimePickerInterface.SetCustomFormat(control, this.m_Format);
                        DateTimePickerInterface.SetShowNone(control, this.AllowNoneTime);
                        DateTimePickerInterface.SetShowUpDown(control, this.ShowTimeUpDown);
                        DateTimePickerInterface.SetShowDropDown(control, this.ShowDropDown);
                        DateTimePickerInterface.SetShowTimeNone(control, this.ShowTimeNone);
                        DateTimePickerInterface.SetNoneText(control, this.NoneText);
                        DateTimePickerInterface.SetTimePickerStartTime(control, new DateTime(1, 1, 1, this.m_startTime.Hours, this.m_startTime.Minutes, this.m_startTime.Seconds));
                        DateTimePickerInterface.SetTimePickerEndTime(control, new DateTime(1, 1, 1, this.m_endTime.Hours, this.m_endTime.Minutes, this.m_endTime.Seconds));
                        DateTimePickerInterface.SetTimePickerMinuteInterval(control, this.m_minuteInterval);
                        DateTimePickerInterface.SetChecked(control, true);
                        try
                        {
                            if (this.EditValue == null)
                            {
                                DateTimePickerInterface.SetValue(control, DateTime.Now);
                                if (this.AllowNoneTime)
                                {
                                    DateTimePickerInterface.SetChecked(control, false);
                                }
                            }
                            else
                            {
                                DateTimePickerInterface.SetValue(control, DateTime.Today.AddTicks(this.TimeSpan.Ticks));
                            }
                        }
                        catch
                        {
                            DateTimePickerInterface.SetValue(control, DateTime.Now);
                        }
                        this.EditControl = control;
                        DateTimePickerInterface.AddValueChangedEvent(control, new EventHandler(this.OnValueChanged));
                        DateTimePickerInterface.AddNoneSelectedEvent(control, new EventHandler(this.OnValueChanged));
                        control.Bounds = this.GetActivePartBounds(yOffset);
                        base.DisableEvents = false;
                        this.OnGotFocus(this, new ItemEventArgs(this, 0, base.Name));
                        base.DisableEvents = true;
                        if (this.EditControl != null)
                        {
                            this.EditControl.Show();
                            this.EditControl.Focus();
                            if (this.DropDownClicked(parentWidth, yOffset))
                            {
                                DateTimePickerInterface.SetDroppedDown(this.EditControl, true);
                            }
                        }
                    }
                    base.DisableEvents = false;
                    base.Click(yOffset, parentWidth);
                }
            }
        }

        public override Item Clone()
        {
            ItemTimeSpan span = new ItemTimeSpan(this);
            span.Value = this.Value;
            return span;
        }

        protected override void DrawItemTextArea(Graphics gr, ref Rectangle textBounds)
        {
            textBounds.Width -= 3 * Resco.Controls.DetailView.DetailView.ComboSize;
            base.DrawAlignmentString(gr, this.Text, base.TextFont, base.GetTextForeBrush(), textBounds, base.TextAlign, base.LineAlign, false);
            if (this.m_showDropDown)
            {
                Image bmpComboBox = Resco.Controls.DetailView.DetailView.BmpComboBox;
                Rectangle comboRectangle = Resco.Controls.DetailView.DetailView.ComboRectangle;
                comboRectangle.Offset(textBounds.Right, textBounds.Y);
                gr.DrawImage(bmpComboBox, comboRectangle, 0, 0, bmpComboBox.Width, bmpComboBox.Height, GraphicsUnit.Pixel, this.m_ImageAttributes);
            }
        }

        private bool DropDownClicked(int parentWidth, int yOffset)
        {
            Point lastMousePosition = base.Parent.LastMousePosition;
            Rectangle rectangle = new Rectangle();
            switch (base.Style)
            {
                case RescoItemStyle.LabelTop:
                    rectangle.X = parentWidth - ((4 * Resco.Controls.DetailView.DetailView.ComboSize) + Resco.Controls.DetailView.DetailView.ErrorSpacer);
                    rectangle.Y = yOffset + this.LabelHeight;
                    rectangle.Width = 4 * Resco.Controls.DetailView.DetailView.ComboSize;
                    rectangle.Height = this.Height - 1;
                    break;

                case RescoItemStyle.LabelRight:
                    rectangle.X = (parentWidth - (base.InternalLabelWidth + base.Parent.SeparatorWidth)) - (4 * Resco.Controls.DetailView.DetailView.ComboSize);
                    rectangle.Y = yOffset;
                    rectangle.Width = 4 * Resco.Controls.DetailView.DetailView.ComboSize;
                    rectangle.Height = this.Height - 1;
                    break;

                default:
                    rectangle.X = parentWidth - ((4 * Resco.Controls.DetailView.DetailView.ComboSize) + Resco.Controls.DetailView.DetailView.ErrorSpacer);
                    rectangle.Y = yOffset;
                    rectangle.Width = 4 * Resco.Controls.DetailView.DetailView.ComboSize;
                    rectangle.Height = this.Height - 1;
                    break;
            }
            return rectangle.Contains(lastMousePosition.X, lastMousePosition.Y);
        }

        internal virtual Type GetDateTimePickerType()
        {
            return typeof(DateTimePickerEx);
        }

        protected internal override bool HandleKey(Keys key)
        {
            if (key == Keys.Return)
            {
                return true;
            }
            if (((key != Keys.Up) && (key != Keys.Down)) && (key != Keys.Return))
            {
                return false;
            }
            return this.DroppedDown;
        }

        protected internal override bool HandleKeyUp(Keys key)
        {
            if (((key != Keys.Up) && (key != Keys.Down)) && (key != Keys.Return))
            {
                return false;
            }
            return this.DroppedDown;
        }

        protected override void Hide()
        {
            if (this.EditControl != null)
            {
                DateTimePickerInterface.SetDroppedDown(this.EditControl, false);
                this.EditControl.Hide();
                DateTimePickerInterface.RemoveValueChangedEvent(this.EditControl, new EventHandler(this.OnValueChanged));
                DateTimePickerInterface.RemoveNoneSelectedEvent(this.EditControl, new EventHandler(this.OnValueChanged));
            }
            base.Hide();
            this.EditControl = null;
        }

        protected override void MoveTop(int offset)
        {
            if (this.EditControl != null)
            {
                this.EditControl.Top += offset;
            }
            base.MoveTop(offset);
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (this.EditControl != null)
            {
                if (this.AllowNoneTime && !DateTimePickerInterface.GetChecked(this.EditControl))
                {
                    this.EditValue = null;
                }
                else
                {
                    this.TimeSpan = DateTimePickerInterface.GetValue(this.EditControl).TimeOfDay;
                }
            }
        }

        protected virtual bool ShouldSerializeTimePickerEndTime()
        {
            if ((this.m_endTime.Hours == 0x17) && (this.m_endTime.Minutes == 30))
            {
                return (this.m_endTime.Seconds != 0);
            }
            return true;
        }

        protected virtual bool ShouldSerializeTimePickerStartTime()
        {
            if ((this.m_startTime.Hours == 0) && (this.m_startTime.Minutes == 0))
            {
                return (this.m_startTime.Seconds != 0);
            }
            return true;
        }

        protected virtual bool ShouldSerializeTimeSpan()
        {
            return (this.Value != null);
        }

        private System.TimeSpan StringToTimeSpan(string value)
        {
            System.TimeSpan timeOfDay;
            try
            {
                timeOfDay = System.TimeSpan.Parse(value);
            }
            catch (Exception)
            {
                try
                {
                    timeOfDay = DateTime.ParseExact(value, this.m_Format, DateTimeFormatInfo.CurrentInfo).TimeOfDay;
                }
                catch (Exception)
                {
                    timeOfDay = DateTime.ParseExact(value, this.m_Format, DateTimeFormatInfo.InvariantInfo).TimeOfDay;
                }
            }
            return timeOfDay;
        }

        public override string ToString()
        {
            if (!(base.Name == "") && (base.Name != null))
            {
                return base.Name;
            }
            if (base.Site != null)
            {
                return base.Site.Name;
            }
            return "TimeSpan";
        }

        protected override void UpdateControl(object value)
        {
            if (this.EditControl != null)
            {
                if (value == null)
                {
                    if (this.AllowNoneTime)
                    {
                        this.EditControl.Focus();
                        DateTimePickerInterface.SetChecked(this.EditControl, false);
                    }
                }
                else if ((value is System.TimeSpan) || (value is string))
                {
                    DateTimePickerInterface.SetValue(this.EditControl, new DateTime(this.StringToTimeSpan(Convert.ToString(value)).Ticks));
                }
            }
        }

        [DefaultValue(false)]
        public bool AllowNoneTime
        {
            get
            {
                return this.m_AllowNoneTime;
            }
            set
            {
                this.m_AllowNoneTime = value;
                if (!this.m_AllowNoneTime && (this.Value == null))
                {
                    this.EditValue = DateTime.Now.TimeOfDay;
                }
            }
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.EditControl;
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.DetailView.Design.Browsable(false)]
        public bool DroppedDown
        {
            get
            {
                return ((this.EditControl != null) && DateTimePickerInterface.GetDroppedDown(this.EditControl));
            }
            set
            {
                if (this.EditControl != null)
                {
                    DateTimePickerInterface.SetDroppedDown(this.EditControl, value);
                }
            }
        }

        internal override bool Focused
        {
            get
            {
                if (this.EditControl != null)
                {
                    return this.EditControl.Focused;
                }
                return base.Focused;
            }
        }

        [DefaultValue("")]
        public string Format
        {
            get
            {
                if (this.m_bDefaultFormat)
                {
                    return "";
                }
                return this.m_Format;
            }
            set
            {
                if ((value == "") || (value == null))
                {
                    this.m_bDefaultFormat = true;
                    this.m_Format = DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
                    this.OnPropertyChanged();
                }
                else
                {
                    this.m_bDefaultFormat = false;
                    this.m_Format = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue("None")]
        public string NoneText
        {
            get
            {
                return this.m_NoneText;
            }
            set
            {
                if (this.m_NoneText != value)
                {
                    this.m_NoneText = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(true)]
        public bool ShowDropDown
        {
            get
            {
                return this.m_showDropDown;
            }
            set
            {
                if (this.m_showDropDown != value)
                {
                    this.m_showDropDown = value;
                    if (this.EditControl != null)
                    {
                        DateTimePickerInterface.SetShowDropDown(this.EditControl, value);
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(false)]
        public virtual bool ShowTimeNone
        {
            get
            {
                return this.m_showTimeNone;
            }
            set
            {
                if (value != this.m_showTimeNone)
                {
                    this.m_showTimeNone = value;
                    if (this.EditControl != null)
                    {
                        DateTimePickerInterface.SetShowTimeNone(this.EditControl, value);
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(false)]
        public virtual bool ShowTimeUpDown
        {
            get
            {
                return this.m_showTimeUpDown;
            }
            set
            {
                if (value != this.m_showTimeUpDown)
                {
                    this.m_showTimeUpDown = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(""), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get
            {
                if (this.EditValue == null)
                {
                    return this.NoneText;
                }
                System.TimeSpan timeSpan = this.TimeSpan;
                try
                {
                    DateTime time = new DateTime(timeSpan.Ticks);
                    return time.ToString(this.m_Format, DateTimeFormatInfo.CurrentInfo);
                }
                catch
                {
                    return timeSpan.ToString();
                }
            }
            set
            {
                if (((value == null) || (value == "")) || (value == this.NoneText))
                {
                    this.EditValue = null;
                }
                else
                {
                    this.EditValue = this.StringToTimeSpan(value);
                }
            }
        }

        public System.TimeSpan TimePickerEndTime
        {
            get
            {
                return this.m_endTime;
            }
            set
            {
                if (this.m_endTime != value)
                {
                    this.m_endTime = value;
                }
            }
        }

        [DefaultValue((double) 30.0)]
        public double TimePickerMinuteInterval
        {
            get
            {
                return this.m_minuteInterval;
            }
            set
            {
                if (this.m_minuteInterval != value)
                {
                    this.m_minuteInterval = value;
                }
            }
        }

        public System.TimeSpan TimePickerStartTime
        {
            get
            {
                return this.m_startTime;
            }
            set
            {
                if (this.m_startTime != value)
                {
                    this.m_startTime = value;
                }
            }
        }

        public System.TimeSpan TimeSpan
        {
            get
            {
                object editValue = this.EditValue;
                if (editValue == null)
                {
                    return new System.TimeSpan(0, 0, 0);
                }
                return (System.TimeSpan) editValue;
            }
            set
            {
                this.EditValue = value;
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), DefaultValue((string) null), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public override object Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (value != null)
                {
                    if (value is System.TimeSpan)
                    {
                        base.Value = value;
                    }
                    else
                    {
                        base.Value = this.StringToTimeSpan(Convert.ToString(value));
                    }
                }
                else
                {
                    base.Value = value;
                }
            }
        }
    }
}

