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

    public class ItemDateTime : Item
    {
        public static bool AllowNullAsDate = true;
        public static double DTIncrement = 30.0;
        public static System.DateTime DTTimeFrom = new System.DateTime(1, 1, 1, 0, 0, 0);
        public static System.DateTime DTTimeTo = new System.DateTime(1, 1, 1, 0x17, 30, 0);
        public static int DTWidth = 90;
        private DVDateTimePicker EditControl;
        private bool m_AllowNoneDate;
        private bool m_bDefaultFormat;
        private bool m_bInteralError;
        private bool m_calendarTitleVistaStyle;
        private RescoDateTimePickerStyle m_DateTimePickerStyle;
        private TimeSpan m_endTime;
        private string m_Format;
        private ImageAttributes m_ImageAttributes;
        private System.DateTime m_MaxDate;
        private System.DateTime m_MinDate;
        private double m_minuteInterval;
        private string m_NoneText;
        private bool m_showDropDown;
        private bool m_showTimeNone;
        private bool m_showTimeUpDown;
        private TimeSpan m_startTime;
        public static System.DateTime NoneDateTime = System.DateTime.MinValue;

        public ItemDateTime()
        {
            this.m_bDefaultFormat = true;
            this.m_Format = "";
            this.m_showDropDown = true;
            this.m_MinDate = DateTimePickerEx.MinDateTime;
            this.m_MaxDate = DateTimePickerEx.MaxDateTime;
            this.DateTimeStyle = RescoDateTimePickerStyle.DateTime;
            this.EditValue = NoneDateTime;
            this.m_ImageAttributes = new ImageAttributes();
            this.m_ImageAttributes.SetColorKey(Color.White, Color.White);
            this.m_AllowNoneDate = false;
            this.m_NoneText = "None";
            this.Format = "";
            this.m_calendarTitleVistaStyle = false;
            this.m_startTime = new TimeSpan(DTTimeFrom.Hour, DTTimeFrom.Minute, DTTimeFrom.Second);
            this.m_endTime = new TimeSpan(DTTimeTo.Hour, DTTimeTo.Minute, DTTimeTo.Second);
            this.m_minuteInterval = DTIncrement;
        }

        public ItemDateTime(Item toCopy) : base(toCopy)
        {
            this.m_bDefaultFormat = true;
            this.m_Format = "";
            this.m_showDropDown = true;
            this.m_MinDate = DateTimePickerEx.MinDateTime;
            this.m_MaxDate = DateTimePickerEx.MaxDateTime;
            if (toCopy is ItemDateTime)
            {
                this.AllowNoneDate = ((ItemDateTime) toCopy).AllowNoneDate;
                this.DateTimeStyle = ((ItemDateTime) toCopy).DateTimeStyle;
                this.Format = ((ItemDateTime) toCopy).Format;
                this.NoneText = ((ItemDateTime) toCopy).NoneText;
                this.ShowTimeNone = ((ItemDateTime) toCopy).ShowTimeNone;
                this.CalendarTitleVistaStyle = ((ItemDateTime) toCopy).CalendarTitleVistaStyle;
                this.TimePickerStartTime = ((ItemDateTime) toCopy).TimePickerStartTime;
                this.TimePickerEndTime = ((ItemDateTime) toCopy).TimePickerEndTime;
                this.TimePickerMinuteInterval = ((ItemDateTime) toCopy).TimePickerMinuteInterval;
            }
            this.m_ImageAttributes = new ImageAttributes();
            this.m_ImageAttributes.SetColorKey(Color.White, Color.White);
        }

        public ItemDateTime(string Label, RescoDateTimePickerStyle DateTimePickerStyle)
        {
            this.m_bDefaultFormat = true;
            this.m_Format = "";
            this.m_showDropDown = true;
            this.m_MinDate = DateTimePickerEx.MinDateTime;
            this.m_MaxDate = DateTimePickerEx.MaxDateTime;
            this.Label = Label;
            this.DateTimeStyle = DateTimePickerStyle;
            this.EditValue = NoneDateTime;
            this.m_ImageAttributes = new ImageAttributes();
            this.m_ImageAttributes.SetColorKey(Color.White, Color.White);
            this.m_AllowNoneDate = false;
            this.m_NoneText = "None";
            this.Format = "";
            this.m_calendarTitleVistaStyle = false;
            this.m_startTime = new TimeSpan(DTTimeFrom.Hour, DTTimeFrom.Minute, DTTimeFrom.Second);
            this.m_endTime = new TimeSpan(DTTimeTo.Hour, DTTimeTo.Minute, DTTimeTo.Second);
            this.m_minuteInterval = DTIncrement;
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
                    DVDateTimePicker control = base.Parent.GetControl(typeof(DVDateTimePicker)) as DVDateTimePicker;
                    if (control != null)
                    {
                        control.ParentItem = this;
                        control.DatePicker.Font = base.TextFont;
                        control.TimePicker.Font = base.TextFont;
                        control.Enabled = true;
                        control.Style = this.DateTimeStyle;
                        control.Format = this.m_Format;
                        control.AllowNone = this.AllowNoneDate;
                        control.DatePicker.MonthCalendar.TitleVistaStyle = this.CalendarTitleVistaStyle;
                        control.DatePicker.MaxDate = this.MaxDate;
                        control.DatePicker.MinDate = this.MinDate;
                        control.TimePicker.ShowUpDown = this.ShowTimeUpDown;
                        control.DatePicker.ShowDropDown = this.m_showDropDown;
                        control.TimePicker.ShowDropDown = this.m_showDropDown;
                        control.TimePicker.ShowTimeNone = this.m_showTimeNone;
                        control.TimePicker.TimePickerStartTime = new System.DateTime(1, 1, 1, this.m_startTime.Hours, this.m_startTime.Minutes, this.m_startTime.Seconds);
                        control.TimePicker.TimePickerEndTime = new System.DateTime(1, 1, 1, this.m_endTime.Hours, this.m_endTime.Minutes, this.m_endTime.Seconds);
                        control.TimePicker.TimePickerMinuteInterval = this.m_minuteInterval;
                        try
                        {
                            if (this.DateTime == NoneDateTime)
                            {
                                control.Date = System.DateTime.Now;
                                if (this.AllowNoneDate)
                                {
                                    control.DatePicker.Checked = false;
                                    control.TimePicker.Checked = false;
                                }
                            }
                            else
                            {
                                control.TimePicker.Enabled = true;
                                control.Date = this.DateTime;
                            }
                        }
                        catch
                        {
                            control.Date = System.DateTime.Now;
                        }
                        this.EditControl = control;
                        control.ValueChanged += new ItemEventHandler(this.OnChanged);
                        control.Bounds = this.GetActivePartBounds(yOffset);
                        base.DisableEvents = false;
                        this.OnGotFocus(this, new ItemEventArgs(this, 0, base.Name));
                        base.DisableEvents = true;
                        if (this.EditControl != null)
                        {
                            this.EditControl.Show();
                            if (this.ShowDropDown && this.DropDownClicked(parentWidth, yOffset))
                            {
                                this.EditControl.DroppedDown = true;
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
            ItemDateTime time = new ItemDateTime(this);
            time.Value = this.Value;
            return time;
        }

        protected override void DrawItemTextArea(Graphics gr, ref Rectangle textBounds)
        {
            textBounds.Width -= 3 * Resco.Controls.DetailView.DetailView.ComboSize;
            try
            {
                base.DrawAlignmentString(gr, this.Text, base.TextFont, base.GetTextForeBrush(), textBounds, base.TextAlign, base.LineAlign, false);
                if (this.m_bInteralError)
                {
                    this.ErrorMessage = "";
                }
            }
            catch (Exception exception)
            {
                this.ErrorMessage = exception.Message;
                this.m_bInteralError = true;
            }
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

        internal static string GetDateFormatPart(string format)
        {
            string str = format;
            bool flag = true;
            while (flag)
            {
                int index = str.IndexOf("h");
                if (index >= 0)
                {
                    str = str.Remove(index, 1);
                }
                else
                {
                    index = str.IndexOf("H");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    index = str.IndexOf("m");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    index = str.IndexOf("s");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    index = str.IndexOf("S");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    index = str.IndexOf("t");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    index = str.IndexOf("T");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    index = str.IndexOf(":");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    flag = false;
                }
            }
            return str.Trim();
        }

        internal static string GetTimeFormatPart(string format)
        {
            string str = format;
            bool flag = true;
            while (flag)
            {
                int index = str.IndexOf("y");
                if (index >= 0)
                {
                    str = str.Remove(index, 1);
                }
                else
                {
                    index = str.IndexOf("Y");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    index = str.IndexOf("M");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    index = str.IndexOf("d");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    index = str.IndexOf("D");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    index = str.IndexOf("/");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    index = str.IndexOf(".");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    index = str.IndexOf("-");
                    if (index >= 0)
                    {
                        str = str.Remove(index, 1);
                        continue;
                    }
                    flag = false;
                }
            }
            return str.Trim();
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
                this.EditControl.Hide();
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

        internal bool ShouldSerializeDateTime()
        {
            return (this.DateTime != NoneDateTime);
        }

        protected virtual bool ShouldSerializeDateTimeStyle()
        {
            return (this.m_DateTimePickerStyle != RescoDateTimePickerStyle.DateTime);
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
            return "DateTime";
        }

        protected override void UpdateControl(object value)
        {
            if (this.EditControl != null)
            {
                if (value == null)
                {
                    if (this.AllowNoneDate)
                    {
                        this.EditControl.Focus();
                        this.EditControl.DatePicker.Checked = false;
                    }
                }
                else if ((value is System.DateTime) || (value is string))
                {
                    this.EditControl.Date = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool AllowNoneDate
        {
            get
            {
                return this.m_AllowNoneDate;
            }
            set
            {
                this.m_AllowNoneDate = value;
                if (!this.m_AllowNoneDate && (this.Value == null))
                {
                    this.EditValue = NoneDateTime;
                }
            }
        }

        [DefaultValue(false)]
        public bool CalendarTitleVistaStyle
        {
            get
            {
                return this.m_calendarTitleVistaStyle;
            }
            set
            {
                if (this.m_calendarTitleVistaStyle != value)
                {
                    this.m_calendarTitleVistaStyle = value;
                    this.OnPropertyChanged();
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

        public System.DateTime DateTime
        {
            get
            {
                object editValue = this.EditValue;
                if (editValue == null)
                {
                    return NoneDateTime;
                }
                return (System.DateTime) editValue;
            }
            set
            {
                this.EditValue = value;
            }
        }

        [DefaultValue(0)]
        public RescoDateTimePickerStyle DateTimeStyle
        {
            get
            {
                return this.m_DateTimePickerStyle;
            }
            set
            {
                if (value != this.m_DateTimePickerStyle)
                {
                    this.m_DateTimePickerStyle = value;
                    this.Format = this.Format;
                    this.OnPropertyChanged();
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public bool DroppedDown
        {
            get
            {
                return ((this.EditControl != null) && this.EditControl.DroppedDown);
            }
            set
            {
                if (this.EditControl != null)
                {
                    this.EditControl.DroppedDown = value;
                }
            }
        }

        private string ErrorMessage
        {
            get
            {
                return base.ErrorMessage;
            }
            set
            {
                base.ErrorMessage = value;
                this.m_bInteralError = false;
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
                if (!(value == "") && (value != null))
                {
                    this.m_bDefaultFormat = false;
                    this.m_Format = value;
                    this.OnPropertyChanged();
                }
                else
                {
                    this.m_bDefaultFormat = true;
                    switch (this.DateTimeStyle)
                    {
                        case RescoDateTimePickerStyle.Date:
                            this.m_Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                            break;

                        case RescoDateTimePickerStyle.Time:
                            this.m_Format = DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
                            break;

                        default:
                            this.m_Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " " + DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
                            break;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(true), DefaultValue("9998-12-31")]
        public System.DateTime MaxDate
        {
            get
            {
                return this.m_MaxDate;
            }
            set
            {
                if (value != this.m_MaxDate)
                {
                    if (value < this.m_MinDate)
                    {
                        throw new ArgumentException("InvalidLowBoundArgument", value.ToString("G"));
                    }
                    if (value > DateTimePickerEx.MaxDateTime)
                    {
                        throw new ArgumentException("DateTimePickerMaxDate", DateTimePickerEx.MaxDateTime.ToString("G"));
                    }
                    this.m_MaxDate = value;
                    if ((this.EditValue != null) && (this.m_MaxDate < this.DateTime))
                    {
                        this.EditValue = this.m_MaxDate;
                    }
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(true), DefaultValue("1753-1-1")]
        public System.DateTime MinDate
        {
            get
            {
                return this.m_MinDate;
            }
            set
            {
                if (value != this.m_MinDate)
                {
                    if (value > this.m_MaxDate)
                    {
                        throw new ArgumentException("InvalidHighBoundArgument", value.ToString("G"));
                    }
                    if (value < DateTimePickerEx.MinDateTime)
                    {
                        throw new ArgumentException("DateTimePickerMinDate", DateTimePickerEx.MinDateTime.ToString("G"));
                    }
                    this.m_MinDate = value;
                    if ((this.EditValue != null) && (this.m_MinDate > this.MinDate))
                    {
                        this.EditValue = this.m_MinDate;
                    }
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
                this.m_NoneText = value;
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
                        this.EditControl.TimePicker.ShowDropDown = value;
                        this.EditControl.DatePicker.ShowDropDown = value;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(false)]
        public bool ShowTimeNone
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
                        this.EditControl.TimePicker.ShowTimeNone = value;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(false)]
        public bool ShowTimeUpDown
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
                if (this.DateTime == NoneDateTime)
                {
                    return this.NoneText;
                }
                try
                {
                    return this.DateTime.ToString(this.m_Format, DateTimeFormatInfo.CurrentInfo);
                }
                catch
                {
                    return this.DateTime.ToString();
                }
            }
            set
            {
                if ((value == "") || (value == this.NoneText))
                {
                    this.EditValue = null;
                }
                else
                {
                    try
                    {
                        this.EditValue = Convert.ToDateTime(value, DateTimeFormatInfo.CurrentInfo);
                    }
                    catch (Exception)
                    {
                        this.EditValue = Convert.ToDateTime(value, DateTimeFormatInfo.InvariantInfo);
                    }
                }
            }
        }

        public TimeSpan TimePickerEndTime
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

        public TimeSpan TimePickerStartTime
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

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue((string) null), Resco.Controls.DetailView.Design.Browsable(false)]
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
                    if (value is System.DateTime)
                    {
                        base.Value = value;
                    }
                    else
                    {
                        base.Value = Convert.ToDateTime(value, DateTimeFormatInfo.CurrentInfo);
                    }
                }
                else if (this.AllowNoneDate && AllowNullAsDate)
                {
                    base.Value = value;
                }
                else
                {
                    base.Value = NoneDateTime;
                }
            }
        }
    }
}

