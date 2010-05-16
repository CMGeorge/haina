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

    public class ItemTouchDateTime : Item
    {
        public static bool AllowNullAsDate = true;
        private DVTouchDateTimePicker EditControl;
        private bool m_AllowNoneDate;
        private bool m_backColorVistaStyle;
        private bool m_bDefaultFormat;
        private RescoDateTimePickerStyle m_DateTimePickerStyle;
        private string m_Format;
        private ImageAttributes m_ImageAttributes;
        private System.DateTime m_MaxDate;
        private System.DateTime m_MinDate;
        private string m_NoneText;
        private bool m_showDropDown;
        private string m_TodayText;
        public static System.DateTime NoneDateTime = System.DateTime.MinValue;

        public ItemTouchDateTime()
        {
            this.m_bDefaultFormat = true;
            this.m_Format = "";
            this.m_showDropDown = true;
            this.m_MinDate = DateTimePickerEx.MinDateTime;
            this.m_MaxDate = DateTimePickerEx.MaxDateTime;
            this.EditValue = NoneDateTime;
            this.m_ImageAttributes = new ImageAttributes();
            this.m_ImageAttributes.SetColorKey(Color.White, Color.White);
            this.m_AllowNoneDate = false;
            this.m_NoneText = "None";
            this.m_TodayText = "Today";
            this.m_backColorVistaStyle = false;
            this.Format = "";
        }

        public ItemTouchDateTime(Item toCopy) : base(toCopy)
        {
            this.m_bDefaultFormat = true;
            this.m_Format = "";
            this.m_showDropDown = true;
            this.m_MinDate = DateTimePickerEx.MinDateTime;
            this.m_MaxDate = DateTimePickerEx.MaxDateTime;
            this.EditValue = System.DateTime.Now;
            this.m_ImageAttributes = new ImageAttributes();
            this.m_ImageAttributes.SetColorKey(Color.White, Color.White);
            if (toCopy is ItemTouchDateTime)
            {
                this.AllowNoneDate = ((ItemTouchDateTime) toCopy).AllowNoneDate;
                this.NoneText = ((ItemTouchDateTime) toCopy).NoneText;
                this.TodayText = ((ItemTouchDateTime) toCopy).TodayText;
                this.DateTimeStyle = ((ItemTouchDateTime) toCopy).DateTimeStyle;
                this.Format = ((ItemTouchDateTime) toCopy).Format;
                this.ShowDropDown = ((ItemTouchDateTime) toCopy).ShowDropDown;
                this.BackColorVistaStyle = ((ItemTouchDateTime) toCopy).BackColorVistaStyle;
            }
        }

        public ItemTouchDateTime(string Label, RescoDateTimePickerStyle DateTimePickerStyle)
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
            this.m_TodayText = "Today";
            this.m_backColorVistaStyle = false;
            this.Format = "";
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
                    DVTouchDateTimePicker control = base.Parent.GetControl(typeof(DVTouchDateTimePicker)) as DVTouchDateTimePicker;
                    if (control != null)
                    {
                        control.ForeColor = base.GetTextForeColor();
                        control.BackColor = base.GetTextBackColor();
                        control.Font = base.TextFont;
                        control.Enabled = true;
                        control.Style = this.DateTimeStyle;
                        base.InvokeSetProperty(control.DatePicker, "BackColorVistaStyle", this.BackColorVistaStyle);
                        base.InvokeSetProperty(control.TimePicker, "BackColorVistaStyle", this.BackColorVistaStyle);
                        control.Format = this.m_Format;
                        control.AllowNone = this.AllowNoneDate;
                        control.NoneText = this.NoneText;
                        control.TodayText = this.TodayText;
                        base.InvokeSetProperty(control.DatePicker, "ShowDropDown", this.m_showDropDown);
                        base.InvokeSetProperty(control.TimePicker, "ShowDropDown", this.m_showDropDown);
                        base.InvokeSetProperty(control.DatePicker, "MaxDate", this.MaxDate);
                        base.InvokeSetProperty(control.DatePicker, "MinDate", this.MinDate);
                        try
                        {
                            if (this.DateTime == NoneDateTime)
                            {
                                control.Date = System.DateTime.Now;
                                if (this.AllowNoneDate)
                                {
                                    control.TimePicker.Enabled = false;
                                    base.InvokeSetProperty(control.DatePicker, "IsNone", true);
                                    base.InvokeSetProperty(control.TimePicker, "IsNone", true);
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
                        control.ValueChanged += new EventHandler(this.OnValueChanged);
                        control.Bounds = this.GetActivePartBounds(yOffset);
                        base.DisableEvents = false;
                        this.OnGotFocus(this, new ItemEventArgs(this, 0, base.Name));
                        base.DisableEvents = true;
                        if (this.EditControl != null)
                        {
                            this.EditControl.Show();
                            if (this.ShowDropDown && this.DropDownClicked(parentWidth, yOffset))
                            {
                                this.DroppedDown = true;
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
            ItemTouchDateTime time = new ItemTouchDateTime(this);
            time.Value = this.Value;
            return time;
        }

        protected override void DrawItemTextArea(Graphics gr, ref Rectangle textBounds)
        {
            textBounds.Width -= 3 * Resco.Controls.DetailView.DetailView.ComboSize;
            try
            {
                base.DrawAlignmentString(gr, this.Text, base.TextFont, base.GetTextForeBrush(), textBounds, base.TextAlign, base.LineAlign, false);
            }
            catch (Exception)
            {
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

        protected internal override bool HandleKey(Keys key)
        {
            if ((key == Keys.Return) && !this.DroppedDown)
            {
                this.DroppedDown = true;
                return true;
            }
            return false;
        }

        protected override void Hide()
        {
            if (this.EditControl != null)
            {
                this.EditControl.ValueChanged -= new EventHandler(this.OnValueChanged);
                this.EditControl.Hide();
                this.EditValue = this.EditControl.Date;
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
                this.EditValue = this.EditControl.Date;
            }
        }

        protected virtual bool ShouldSerializeDateTimeStyle()
        {
            return (this.m_DateTimePickerStyle != RescoDateTimePickerStyle.DateTime);
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
            return "TouchDateTime";
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
                        base.InvokeSetProperty(this.EditControl.DatePicker, "IsNone", true);
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
        public bool BackColorVistaStyle
        {
            get
            {
                return this.m_backColorVistaStyle;
            }
            set
            {
                if (this.m_backColorVistaStyle != value)
                {
                    this.m_backColorVistaStyle = value;
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
                if (this.EditControl != null)
                {
                    this.EditControl.NoneText = value;
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
                        base.InvokeSetProperty(this.EditControl.DatePicker, "ShowDropDown", value);
                        base.InvokeSetProperty(this.EditControl.TimePicker, "ShowDropDown", value);
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue("")]
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
                if (((value == "") || (value == null)) || (value == this.NoneText))
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

        [DefaultValue("Today")]
        public string TodayText
        {
            get
            {
                return this.m_TodayText;
            }
            set
            {
                this.m_TodayText = value;
                if (this.EditControl != null)
                {
                    this.EditControl.TodayText = value;
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

