namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Forms;

    public class TouchDateTimePicker : UserControl
    {
        private const int KDatePickerDefaultHeight = 0xb3;
        private const int KTimePickerDefaultHeight = 120;
        private bool m_24hourFormat = true;
        private Point[] m_arrowPoints = new Point[3];
        private bool m_backColorVistaStyle;
        private Image m_backgroundImage;
        private Rectangle m_backgroundImageRect = new Rectangle(0, 0, 1, 1);
        private Bitmap m_bmp;
        private SolidBrush m_brushBack;
        private SolidBrush m_brushDisabled;
        private SolidBrush m_brushFore;
        private SolidBrush m_brushFrame;
        private SolidBrush m_brushSelectedBack;
        private SolidBrush m_brushSelectedFore;
        private bool m_bTodayChanged;
        private string m_customFormat = "";
        private TouchDtpRollerControl m_dayPicker = new TouchDtpRollerControl();
        private List<DayCell> m_Days = new List<DayCell>();
        private bool m_doParse = true;
        private LeftRightAligment m_dropDownAlign;
        private Resco.Controls.OutlookControls.DateTimePickerFormat m_format = Resco.Controls.OutlookControls.DateTimePickerFormat.Long;
        private Graphics m_graphics;
        private bool m_IsNone;
        private Pen m_penFrame;
        private PredefinedRollers m_PredifinedRollers;
        private string m_RollersCustomDateFormat = "";
        private string m_RollersCustomTimeFormat = "";
        private Color m_saturdayColor = Color.Blue;
        private SizeF m_scaleFactor = new SizeF(1f, 1f);
        private bool m_showDate = true;
        private bool m_ShowDropDown = true;
        private bool m_ShowFocus = true;
        private Color m_sundayColor = Color.Red;
        private string m_TextCached = "";
        private List<TextPart> m_TextParts = new List<TextPart>();
        private int m_TextSelected = -1;
        private TouchDtpRollerControl m_timePicker = new TouchDtpRollerControl();
        private Rectangle m_TimeTextRectangle = new Rectangle();
        private DateTime m_today = DateTime.Today;
        private DateTime m_Value;
        public static readonly DateTime MaxDateTime = DateTime.MaxValue;
        public static readonly DateTime MinDateTime = DateTime.MinValue;

        public event EventHandler CloseUp;

        public event EventHandler DropDown;

        public event EventHandler NoneButtonPressed;

        public event EventHandler TodayButtonPressed;

        public event EventHandler ValueChanged;

        static TouchDateTimePicker()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(TouchDateTimePicker), "");
            //}
        }

        public TouchDateTimePicker()
        {
            this.InitControl();
        }

        private void AddMonthRoller(string aDateMask, DateTime aDate)
        {
            bool anEnabled = true;
            bool anIsDefault = false;
            int month = DateTime.Today.Month;
            string aText = string.Format("{0:" + aDateMask + "}", aDate);
            if (month == aDate.Month)
            {
                anIsDefault = true;
            }
            else
            {
                anIsDefault = false;
            }
            this.m_PredifinedRollers.MonthRoller.RowItems.Add(new DateTimePickerRow(aText, anEnabled, anIsDefault));
        }

        private void CreateArrowPoints()
        {
            if (Const.DropDownWidth <= 13)
            {
                Const.DropArrowSize = new Size(7, 4);
            }
            this.m_arrowPoints[0].X = base.ClientSize.Width - (((int) this.m_scaleFactor.Width) * (Const.DropArrowSize.Width + 4));
            this.m_arrowPoints[0].Y = (base.ClientSize.Height - (((int) this.m_scaleFactor.Height) * (Const.DropArrowSize.Height - 1))) / 2;
            this.m_arrowPoints[1].X = (this.m_arrowPoints[0].X + (((int) this.m_scaleFactor.Width) * Const.DropArrowSize.Width)) + ((((int) this.m_scaleFactor.Width) == 2) ? 1 : 0);
            this.m_arrowPoints[1].Y = this.m_arrowPoints[0].Y;
            this.m_arrowPoints[2].X = this.m_arrowPoints[0].X + ((((int) this.m_scaleFactor.Width) * Const.DropArrowSize.Width) / 2);
            this.m_arrowPoints[2].Y = this.m_arrowPoints[0].Y + (((int) this.m_scaleFactor.Height) * Const.DropArrowSize.Height);
        }

        private void CreateGdiObjects()
        {
            if (this.m_brushFrame == null)
            {
                this.m_brushFrame = new SolidBrush(SystemColors.WindowFrame);
            }
            if (this.m_penFrame == null)
            {
                this.m_penFrame = new Pen(SystemColors.WindowFrame);
            }
            if ((this.m_brushFore == null) || (this.m_brushFore.Color != this.ForeColor))
            {
                this.m_brushFore = new SolidBrush(this.ForeColor);
            }
            if ((this.m_brushBack == null) || (this.m_brushBack.Color != this.BackColor))
            {
                this.m_brushBack = new SolidBrush(this.BackColor);
            }
            if (this.m_brushDisabled == null)
            {
                this.m_brushDisabled = new SolidBrush(SystemColors.GrayText);
            }
            if (this.m_brushSelectedFore == null)
            {
                this.m_brushSelectedFore = new SolidBrush(SystemColors.ActiveCaptionText);
            }
            if (this.m_brushSelectedBack == null)
            {
                this.m_brushSelectedBack = new SolidBrush(SystemColors.ActiveCaption);
            }
        }

        private void CreateMemoryBitmap()
        {
            if (((this.m_bmp == null) || (this.m_bmp.Width != base.ClientSize.Width)) || (this.m_bmp.Height != base.ClientSize.Height))
            {
                if (this.m_bmp != null)
                {
                    this.m_bmp.Dispose();
                    this.m_bmp = null;
                }
                if (this.m_graphics != null)
                {
                    this.m_graphics.Dispose();
                    this.m_graphics = null;
                }
                this.m_bmp = new Bitmap(base.ClientSize.Width, base.ClientSize.Height);
                this.m_graphics = Graphics.FromImage(this.m_bmp);
                this.CreateArrowPoints();
            }
        }

        private void DatePickerRoller_SelectedIndexChanged(object sender, RollerItemEventArgs e)
        {
            this.m_IsNone = false;
            DateTime selectedDate = this.GetSelectedDate();
            if (!selectedDate.Equals(DateTime.MinValue))
            {
                this.m_dayPicker.Value = selectedDate;
            }
        }

        private void DayPickerUpdateSize()
        {
            int num = 0xb3 * ((int) this.m_scaleFactor.Height);
            this.m_dayPicker.Height = num;
            int num2 = 7 * ((int) this.m_scaleFactor.Width);
            this.m_dayPicker.Width = ((this.m_PredifinedRollers.DayRoller.Width + this.m_PredifinedRollers.MonthRoller.Width) + this.m_PredifinedRollers.YearRoller.Width) + (2 * num2);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_dayPicker != null)
                {
                    try
                    {
                        this.m_dayPicker.Visible = false;
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                    this.m_dayPicker.Dispose();
                }
                if (this.m_timePicker != null)
                {
                    try
                    {
                        this.m_timePicker.Visible = false;
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                    this.m_timePicker.Dispose();
                }
                this.m_timePicker = null;
            }
            this.m_dayPicker = null;
            this.m_timePicker = null;
            base.Dispose(disposing);
        }

        private DayOfWeek GetFirstDayOfWeek()
        {
            if (this.FirstDayOfWeek == Resco.Controls.OutlookControls.Day.Default)
            {
                return DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;
            }
            return (DayOfWeek) this.FirstDayOfWeek;
        }

        private DateTime GetSelectedDate()
        {
            int num3;
            int month = this.m_PredifinedRollers.MonthRoller.SelectedIndex + 1;
            int day = Convert.ToInt32(this.m_PredifinedRollers.DayRoller.SelectedText);
            if (this.m_PredifinedRollers.YearRoller.SelectedText.Length < 4)
            {
                num3 = this.MinDate.Year + this.m_PredifinedRollers.YearRoller.SelectedIndex;
            }
            else
            {
                num3 = Convert.ToInt32(this.m_PredifinedRollers.YearRoller.SelectedText);
            }
            try
            {
                return new DateTime(num3, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
                return DateTime.MinValue;
            }
        }

        private DateTime GetSelectedTime()
        {
            int hour = this.m_PredifinedRollers.HourRoller.SelectedIndex + this.m_PredifinedRollers.FirstHour;
            int minute = Convert.ToInt32(this.m_PredifinedRollers.MinuteRoller.SelectedText);
            int selectedIndex = this.m_PredifinedRollers.AmAndPmRoller.SelectedIndex;
            if ((!this.m_24hourFormat && (selectedIndex == 0)) && (hour == 12))
            {
                hour = 0;
            }
            try
            {
                int year = this.m_timePicker.Value.Year;
                int month = this.m_timePicker.Value.Month;
                int day = this.m_timePicker.Value.Day;
                if ((!this.m_24hourFormat && (selectedIndex > 0)) && (hour < 12))
                {
                    hour += 12;
                }
                if ((!this.m_24hourFormat && (selectedIndex == 0)) && (hour == 12))
                {
                    hour = 0;
                }
                return new DateTime(year, month, day, hour, minute, 0);
            }
            catch (ArgumentOutOfRangeException)
            {
                return DateTime.MinValue;
            }
        }

        public void Hide()
        {
            this.Visible = false;
        }

        private void InitControl()
        {
            this.m_PredifinedRollers = new PredefinedRollers();
            this.InitDayPicker();
            this.InitTimePicker();
            this.Format = Const.DefaultFormat;
            this.Value = Const.DefaultStartDate;
            base.Size = Const.DefaultDateTimePickerSize;
            this.Font = Const.DefaultFont;
            this.m_Value = this.m_dayPicker.Value;
            this.m_backColorVistaStyle = false;
        }

        private void InitDayPicker()
        {
            this.m_dayPicker.Visible = false;
            this.m_dayPicker.VisibleBottomBar = true;
            this.m_dayPicker.m_strYearFormat = "yyyy";
            this.m_dayPicker.CloseUp += new EventHandler(this.OnDayPickerCloseUp);
            this.m_dayPicker.LostFocus += new EventHandler(this.OnDatePickerLostFocus);
            this.m_dayPicker.DayInfoClicked += new EventHandler(this.OnDatePickerDayInfoClicked);
            this.m_dayPicker.TodayButtonPressed += new EventHandler(this.OnDatePicker_TodayButtonPressed);
            this.m_dayPicker.NoneButtonPressed += new EventHandler(this.OnDatePicker_NoneButtonPressed);
            this.m_PredifinedRollers.InitDayRoller();
            this.m_PredifinedRollers.InitYearRoller();
            this.m_PredifinedRollers.InitMonthRoller();
            this.m_PredifinedRollers.DayRoller.SelectedIndexChanged += new Roller.SelectedIndexChangedEventHandler(this.DatePickerRoller_SelectedIndexChanged);
            this.m_PredifinedRollers.MonthRoller.SelectedIndexChanged += new Roller.SelectedIndexChangedEventHandler(this.DatePickerRoller_SelectedIndexChanged);
            this.m_PredifinedRollers.YearRoller.SelectedIndexChanged += new Roller.SelectedIndexChangedEventHandler(this.DatePickerRoller_SelectedIndexChanged);
            this.m_dayPicker.Items.Clear();
            this.m_dayPicker.Items.Add(this.m_PredifinedRollers.DayRoller);
            this.m_dayPicker.Items.Add(this.m_PredifinedRollers.MonthRoller);
            this.m_dayPicker.Items.Add(this.m_PredifinedRollers.YearRoller);
            this.m_PredifinedRollers.ValidDayRoller();
            this.DayPickerUpdateSize();
        }

        private void InitTimePicker()
        {
            this.m_timePicker.Visible = false;
            this.m_timePicker.CloseUp += new EventHandler(this.OnTimePickerCloseUp);
            this.m_timePicker.LostFocus += new EventHandler(this.OnTimePickerLostFocus);
            this.m_timePicker.DayInfoClicked += new EventHandler(this.OnTimePickerDayInfoClicked);
            this.m_timePicker.ShowDayInfo = false;
            this.m_PredifinedRollers.InitHourRoller();
            this.m_PredifinedRollers.InitMinutesRoller();
            this.m_PredifinedRollers.InitAmAndPmRoller();
            this.m_PredifinedRollers.HourRoller.SelectedIndexChanged += new Roller.SelectedIndexChangedEventHandler(this.TimePickerRoller_SelectedIndexChanged);
            this.m_PredifinedRollers.MinuteRoller.SelectedIndexChanged += new Roller.SelectedIndexChangedEventHandler(this.TimePickerRoller_SelectedIndexChanged);
            this.m_PredifinedRollers.AmAndPmRoller.SelectedIndexChanged += new Roller.SelectedIndexChangedEventHandler(this.TimePickerRoller_SelectedIndexChanged);
            this.m_timePicker.Items.Clear();
            this.m_timePicker.Items.Add(this.m_PredifinedRollers.HourRoller);
            this.m_timePicker.Items.Add(this.m_PredifinedRollers.MinuteRoller);
            if (DateTimeFormatInfo.CurrentInfo.ShortTimePattern.StartsWith("h:"))
            {
                this.m_24hourFormat = false;
                this.m_timePicker.Items.Add(this.m_PredifinedRollers.AmAndPmRoller);
            }
            else
            {
                this.m_24hourFormat = true;
            }
            this.TimePickerUpdateSize();
        }

        protected virtual void OnCloseUp(EventArgs e)
        {
            if (this.CloseUp != null)
            {
                this.CloseUp(this, e);
            }
        }

        private void OnDatePicker_NoneButtonPressed(object sender, EventArgs e)
        {
            this.m_IsNone = true;
            this.OnValueChanged(EventArgs.Empty);
            this.m_dayPicker.Hide();
            base.Focus();
            this.OnNoneButtonPressed(e);
        }

        private void OnDatePicker_TodayButtonPressed(object sender, EventArgs e)
        {
            this.m_IsNone = false;
            this.m_dayPicker.Value = this.TodayDate;
            this.OnValueChanged(EventArgs.Empty);
            this.m_dayPicker.Hide();
            base.Focus();
            this.OnTodayButtonPressed(e);
        }

        private void OnDatePickerDayInfoClicked(object sender, EventArgs e)
        {
            this.m_dayPicker.Hide();
            base.Focus();
        }

        private void OnDatePickerLostFocus(object sender, EventArgs e)
        {
            this.UpdateDate();
            this.m_dayPicker.Hide();
            base.Focus();
            this.OnCloseUp(e);
        }

        private void OnDayPickerCloseUp(object sender, EventArgs e)
        {
            this.OnCloseUp(e);
        }

        private void OnDayPickerKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.OnKeyUp(e);
            }
        }

        private void OnDayPickerValueChanged(object sender, EventArgs e)
        {
            this.m_IsNone = false;
            this.m_Value = this.m_dayPicker.Value;
            base.Invalidate();
            this.OnValueChanged(e);
        }

        protected virtual void OnDropDown(EventArgs e)
        {
            if (this.DropDown != null)
            {
                this.DropDown(this, e);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            int v = 0;
            int num2 = 0;
            if ((this.m_TextSelected < 0) || (this.m_TextSelected >= this.m_TextParts.Count))
            {
                base.OnKeyPress(e);
            }
            else
            {
                if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
                {
                    num2 = e.KeyChar - '0';
                }
                else
                {
                    return;
                }
                TextPart p = this.m_TextParts[this.m_TextSelected];
                DateTime date = this.Value;
                if ((p.Format.IndexOf("h") >= 0) || (p.Format.IndexOf("H") >= 0))
                {
                    v = date.Hour;
                }
                else if (p.Format.IndexOf("m") >= 0)
                {
                    v = date.Minute;
                }
                else if (p.Format.IndexOf("d") >= 0)
                {
                    v = date.Day;
                }
                else if (p.Format.IndexOf("M") >= 0)
                {
                    v = date.Month;
                }
                else
                {
                    if (p.Format.IndexOf("y") >= 0)
                    {
                        this.m_doParse = false;
                        string str = p.Text + num2;
                        if (str.Length > p.Format.Length)
                        {
                            str = str.Substring(1, str.Length - 1);
                        }
                        p.Text = str;
                        v = date.Year;
                        base.Invalidate();
                        return;
                    }
                    if (p.Format.IndexOf("s") >= 0)
                    {
                        v = date.Second;
                    }
                }
                v = (v * 10) + num2;
                try
                {
                    date = this.SetKeyDate(p, v, date);
                }
                catch
                {
                    try
                    {
                        string str2 = v.ToString();
                        if (str2.Length > 1)
                        {
                            v = Convert.ToInt32(str2.Remove(0, 1));
                        }
                        date = this.SetKeyDate(p, v, date);
                    }
                    catch
                    {
                        try
                        {
                            date = this.SetKeyDate(p, num2, date);
                        }
                        catch
                        {
                            return;
                        }
                    }
                }
                this.Value = date;
                base.OnKeyPress(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if ((this.m_format == Resco.Controls.OutlookControls.DateTimePickerFormat.Time) || (this.m_format == Resco.Controls.OutlookControls.DateTimePickerFormat.CustomTime))
                {
                    this.ShowTimePicker();
                }
                else
                {
                    this.ShowDayPicker();
                }
            }
            if (this.m_TextSelected < 0)
            {
                base.OnKeyUp(e);
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (((this.m_dayPicker == null) || !this.m_dayPicker.Focused) && ((this.m_timePicker == null) || !this.m_timePicker.Focused))
            {
                this.ValidateYear();
                this.m_TextSelected = -1;
                base.Invalidate();
                base.OnLostFocus(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.ValidateYear();
            bool visible = this.m_dayPicker.Visible;
            bool flag2 = this.m_timePicker.Visible;
            base.Focus();
            if ((this.m_format == Resco.Controls.OutlookControls.DateTimePickerFormat.Time) || (this.m_format == Resco.Controls.OutlookControls.DateTimePickerFormat.CustomTime))
            {
                if (!flag2)
                {
                    this.ShowTimePicker();
                }
            }
            else if ((this.m_format == Resco.Controls.OutlookControls.DateTimePickerFormat.Custom) && this.m_TimeTextRectangle.Contains(e.X, e.Y))
            {
                if (!visible && !flag2)
                {
                    this.ShowTimePicker();
                }
            }
            else if (!visible && !flag2)
            {
                this.ShowDayPicker();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        protected virtual void OnNoneButtonPressed(EventArgs e)
        {
            if (this.NoneButtonPressed != null)
            {
                this.NoneButtonPressed(this, e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int x = 4;
            this.CreateMemoryBitmap();
            this.CreateGdiObjects();
            if (!this.m_backColorVistaStyle)
            {
                this.m_graphics.Clear(this.BackColor);
            }
            else
            {
                GradientFill.DrawVistaGradient(this.m_graphics, this.BackColor, base.ClientRectangle, FillDirection.Vertical);
            }
            if (this.m_backgroundImage != null)
            {
                this.m_backgroundImageRect.Width = this.m_backgroundImage.Width;
                this.m_backgroundImageRect.Height = this.m_backgroundImage.Height;
                this.m_graphics.DrawImage(this.m_backgroundImage, new Rectangle(0, 0, base.ClientSize.Width, base.ClientSize.Height), this.m_backgroundImageRect, GraphicsUnit.Pixel);
            }
            if (this.m_doParse)
            {
                this.m_TextCached = this.TextParse(this.m_graphics, this.Value);
            }
            if (this.Focused && this.m_ShowFocus)
            {
                Rectangle rect = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
                using (Pen pen = new Pen(Color.Black))
                {
                    this.m_graphics.DrawRectangle(pen, rect);
                }
            }
            if (this.m_showDate)
            {
                if (this.m_IsNone)
                {
                    Size size = this.m_graphics.MeasureString(this.m_dayPicker.NoneText, this.Font).ToSize();
                    this.m_graphics.DrawString(this.m_dayPicker.NoneText, this.Font, base.Enabled ? this.m_brushFore : this.m_brushDisabled, (float) x, (float) ((base.ClientSize.Height - size.Height) / 2));
                    x += size.Width + 3;
                }
                else
                {
                    int num2 = 0;
                    int num3 = 0;
                    foreach (TextPart part in this.m_TextParts)
                    {
                        if (this.m_format == Resco.Controls.OutlookControls.DateTimePickerFormat.Custom)
                        {
                            if (part.Format.ToLower().StartsWith("h"))
                            {
                                num2 = x;
                            }
                            else if (part.Format.ToLower().StartsWith("m"))
                            {
                                num3 = x + part.Size.Width;
                            }
                        }
                        if (this.m_TextSelected == this.m_TextParts.IndexOf(part))
                        {
                            this.m_graphics.FillRectangle(this.m_brushSelectedBack, x, (base.ClientSize.Height - part.Size.Height) / 2, part.Size.Width, part.Size.Height);
                            this.m_graphics.DrawString(part.Text, this.Font, base.Enabled ? this.m_brushSelectedFore : this.m_brushDisabled, (float) x, (float) ((base.ClientSize.Height - part.Size.Height) / 2));
                        }
                        else
                        {
                            this.m_graphics.DrawString(part.Text, this.Font, base.Enabled ? this.m_brushFore : this.m_brushDisabled, (float) x, (float) ((base.ClientSize.Height - part.Size.Height) / 2));
                        }
                        x += part.Size.Width;
                    }
                    this.m_TimeTextRectangle = new Rectangle(num2, 0, num3 - num2, base.Height);
                }
                x += 3;
            }
            if (this.m_ShowDropDown)
            {
                this.m_graphics.FillPolygon(this.m_brushFrame, this.m_arrowPoints);
            }
            e.Graphics.DrawImage(this.m_bmp, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            base.Invalidate();
        }

        private void OnTimePickerClick(object sender, EventArgs e)
        {
            this.UpdateTime();
            this.m_timePicker.Hide();
            base.Focus();
            this.OnCloseUp(EventArgs.Empty);
        }

        private void OnTimePickerCloseUp(object sender, EventArgs e)
        {
            base.Focus();
            this.OnCloseUp(EventArgs.Empty);
        }

        private void OnTimePickerDayInfoClicked(object sender, EventArgs e)
        {
            this.m_timePicker.Hide();
            base.Focus();
        }

        private void OnTimePickerKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.UpdateTime();
                this.m_timePicker.Hide();
                base.Focus();
                this.OnCloseUp(EventArgs.Empty);
            }
        }

        private void OnTimePickerLostFocus(object sender, EventArgs e)
        {
            this.UpdateTime();
            this.m_timePicker.Hide();
            base.Focus();
            this.OnCloseUp(e);
        }

        protected virtual void OnTodayButtonPressed(EventArgs e)
        {
            if (this.TodayButtonPressed != null)
            {
                this.TodayButtonPressed(this, e);
            }
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.m_dayPicker.Scale(factor);
            this.m_timePicker.Scale(factor);
            Roller.m_ScaleFactor = factor;
            this.m_scaleFactor = factor;
            base.ScaleControl(factor, specified);
            this.DayPickerUpdateSize();
            this.TimePickerUpdateSize();
        }

        private void SelectDay(int index)
        {
            int num = ((7 + this.Value.DayOfWeek) - this.GetFirstDayOfWeek()) % 7;
            this.Value = this.Value.AddDays((double) (index - num));
            this.m_dayPicker.IsNone = false;
        }

        private void SetCustomDateFormat(string aStrDateFormat)
        {
            if ((aStrDateFormat != null) && !string.Empty.Equals(aStrDateFormat))
            {
                string[] subDateMask = aStrDateFormat.Split(new char[] { ' ' });
                this.SetRollersOrder(subDateMask);
                for (int i = 0; i < subDateMask.Length; i++)
                {
                    string strMask = subDateMask[i];
                    switch (strMask)
                    {
                        case "y":
                        case "yy":
                        case "yyy":
                        case "yyyy":
                            this.SetDateFormat_Year(strMask);
                            break;

                        case "M":
                        case "MM":
                        case "MMM":
                        case "MMMM":
                            this.SetDateFormat_Month(strMask);
                            break;

                        case "d":
                        case "dd":
                            this.SetDateFormat_Day(strMask);
                            break;

                        default:
                            throw new NotSupportedException();
                    }
                }
            }
        }

        private void SetCustomTimeFormat(string aStrTimeFormat)
        {
            if ((aStrTimeFormat != null) && !string.Empty.Equals(aStrTimeFormat))
            {
                string[] strArray;
                if (aStrTimeFormat.IndexOf(':') > -1)
                {
                    strArray = aStrTimeFormat.Split(new char[] { ':' });
                }
                else
                {
                    strArray = aStrTimeFormat.Split(new char[] { ' ' });
                }
                for (int i = 0; i < strArray.Length; i++)
                {
                    string strMask = strArray[i];
                    switch (strMask)
                    {
                        case "h":
                        case "hh":
                        case "H":
                        case "HH":
                            this.SetTimeFormat_Hour12(strMask);
                            break;

                        case "m":
                        case "mm":
                            break;

                        default:
                            throw new NotSupportedException();
                    }
                }
            }
        }

        private void SetDateFormat_Day(string strMask)
        {
            this.m_PredifinedRollers.DayRoller.RowItems.Clear();
            bool anEnabled = true;
            bool anIsDefault = false;
            int day = DateTime.Today.Day;
            string str = "0";
            for (int i = 1; i <= 0x1f; i++)
            {
                if (day == i)
                {
                    anIsDefault = true;
                }
                else
                {
                    anIsDefault = false;
                }
                if (i > this.MaxDate.Day)
                {
                    anEnabled = false;
                }
                if (strMask == "d")
                {
                    str = "0";
                }
                else
                {
                    str = "00";
                }
                string aText = string.Format("{0:" + str + "}", i);
                this.m_PredifinedRollers.DayRoller.RowItems.Add(new DateTimePickerRow(aText, anEnabled, anIsDefault));
            }
        }

        private void SetDateFormat_Month(string strMask)
        {
            this.m_PredifinedRollers.MonthRoller.RowItems.Clear();
            DateTime minDate = this.MinDate;
            do
            {
                this.AddMonthRoller(strMask, minDate);
                minDate = minDate.AddMonths(1);
            }
            while (minDate.Month != this.MaxDate.Month);
            this.AddMonthRoller(strMask, minDate);
        }

        private void SetDateFormat_Year(string strMask)
        {
            this.m_dayPicker.m_strYearFormat = strMask;
        }

        public void SetDateTimePickerParent(Control aParent)
        {
            aParent.Controls.Add(this.m_dayPicker);
            aParent.Controls.Add(this.m_timePicker);
        }

        private DateTime SetKeyDate(TextPart p, int v, DateTime date)
        {
            if ((p.Format.IndexOf("h") >= 0) || (p.Format.IndexOf("H") >= 0))
            {
                return new DateTime(date.Year, date.Month, date.Day, v, date.Minute, date.Second);
            }
            if (p.Format.IndexOf("m") >= 0)
            {
                return new DateTime(date.Year, date.Month, date.Day, date.Hour, v, date.Second);
            }
            if (p.Format.IndexOf("d") >= 0)
            {
                return new DateTime(date.Year, date.Month, v, date.Hour, date.Minute, date.Second);
            }
            if (p.Format.IndexOf("M") >= 0)
            {
                return new DateTime(date.Year, v, date.Day, date.Hour, date.Minute, date.Second);
            }
            if (p.Format.IndexOf("s") >= 0)
            {
                return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, v);
            }
            return date;
        }

        private void SetRollersOrder(string[] subDateMask)
        {
            this.m_dayPicker.Items.Clear();
            for (int i = 0; i < subDateMask.Length; i++)
            {
                string str = subDateMask[i];
                if (str.ToLower().StartsWith("d"))
                {
                    this.m_dayPicker.Items.Add(this.m_PredifinedRollers.DayRoller);
                }
                else if (str.ToLower().StartsWith("m"))
                {
                    this.m_dayPicker.Items.Add(this.m_PredifinedRollers.MonthRoller);
                }
                else if (str.ToLower().StartsWith("y"))
                {
                    this.m_dayPicker.Items.Add(this.m_PredifinedRollers.YearRoller);
                }
            }
        }

        private void SetTimeFormat_Hour12(string strMask)
        {
            this.m_PredifinedRollers.InitHourRoller(strMask);
            this.m_timePicker.Items.Clear();
            this.m_timePicker.Items.Add(this.m_PredifinedRollers.HourRoller);
            this.m_timePicker.Items.Add(this.m_PredifinedRollers.MinuteRoller);
            int num = 7 * ((int) this.m_scaleFactor.Width);
            this.m_timePicker.Width = (this.m_PredifinedRollers.HourRoller.Width + this.m_PredifinedRollers.MinuteRoller.Width) + (2 * num);
            if (strMask.StartsWith("h"))
            {
                this.m_24hourFormat = false;
                this.m_timePicker.Items.Add(this.m_PredifinedRollers.AmAndPmRoller);
                this.m_timePicker.Width += this.m_PredifinedRollers.AmAndPmRoller.Width;
            }
            else
            {
                this.m_24hourFormat = true;
            }
        }

        protected virtual bool ShouldSerializeDropDownAlign()
        {
            if (this.DropDownAlign == LeftRightAligment.Left)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeDropDownButtonWidth()
        {
            return (Const.DropDownWidth != 13);
        }

        protected virtual bool ShouldSerializeFirstDayOfWeek()
        {
            if (this.m_dayPicker.FirstDayOfWeek == Resco.Controls.OutlookControls.Day.Default)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeFormat()
        {
            if (this.Format == Resco.Controls.OutlookControls.DateTimePickerFormat.Long)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeRollersCustomDateFormat()
        {
            return ((this.m_RollersCustomDateFormat != null) && (this.m_RollersCustomDateFormat.Length != 0));
        }

        protected virtual bool ShouldSerializeRollersCustomTimeFormat()
        {
            return ((this.m_RollersCustomTimeFormat != null) && (this.m_RollersCustomTimeFormat.Length != 0));
        }

        protected virtual bool ShouldSerializeShowDropDown()
        {
            return !this.m_ShowDropDown;
        }

        protected virtual bool ShouldSerializeShowFocus()
        {
            return !this.m_ShowFocus;
        }

        protected virtual bool ShouldSerializeTodayDate()
        {
            if (this.m_today == DateTime.Today)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeValue()
        {
            if (this.Value.Date == DateTime.Today)
            {
                return false;
            }
            return true;
        }

        private void ShowDayPicker()
        {
            if (this.m_dayPicker.Parent == null)
            {
                base.TopLevelControl.Controls.Add(this.m_dayPicker);
            }
            Point p = new Point(base.Left, base.Bottom + 1);
            if (object.ReferenceEquals(this.m_dayPicker.Parent, base.TopLevelControl))
            {
                Point point2 = base.Parent.PointToScreen(base.Parent.Location);
                Point point3 = base.TopLevelControl.PointToScreen(base.Parent.Location);
                if (this.m_dropDownAlign == LeftRightAligment.Left)
                {
                    p.Offset(point2.X - point3.X, point2.Y - point3.Y);
                }
                else
                {
                    p.Offset(((point2.X - point3.X) + base.ClientSize.Width) - this.m_dayPicker.Width, point2.Y - point3.Y);
                }
            }
            else if (this.m_dropDownAlign == LeftRightAligment.Right)
            {
                p.Offset(base.ClientSize.Width - this.m_dayPicker.Width, 0);
            }
            if ((p.Y + this.m_dayPicker.Size.Height) > this.m_dayPicker.Parent.ClientRectangle.Height)
            {
                p.Y -= (base.ClientSize.Height + this.m_dayPicker.Size.Height) + 2;
                if (p.Y < 0)
                {
                    p.Y = this.m_dayPicker.Parent.ClientRectangle.Height - this.m_dayPicker.Size.Height;
                }
            }
            if ((base.TopLevelControl.PointToScreen(p).Y + this.m_dayPicker.Size.Height) > (Screen.PrimaryScreen.WorkingArea.Y + Screen.PrimaryScreen.WorkingArea.Height))
            {
                Point point4 = new Point(p.X, (Screen.PrimaryScreen.WorkingArea.Top + Screen.PrimaryScreen.WorkingArea.Height) - this.m_dayPicker.Size.Height);
                p.Y = base.TopLevelControl.PointToClient(point4).Y;
                if (p.Y < 0)
                {
                    p.Y = 0;
                }
            }
            if ((p.X + this.m_dayPicker.Size.Width) > this.m_dayPicker.Parent.ClientRectangle.Width)
            {
                p.X = this.m_dayPicker.Parent.ClientRectangle.Width - this.m_dayPicker.Size.Width;
            }
            this.m_dayPicker.Display(!this.m_dayPicker.Visible, p.X, p.Y);
            if (this.m_dayPicker.Visible)
            {
                this.OnDropDown(EventArgs.Empty);
            }
            else
            {
                this.OnCloseUp(EventArgs.Empty);
            }
        }

        private void ShowTimePicker()
        {
            if (this.m_timePicker.Parent == null)
            {
                base.TopLevelControl.Controls.Add(this.m_timePicker);
            }
            Point p = new Point(base.Left, base.Bottom + 1);
            if (object.ReferenceEquals(this.m_timePicker.Parent, base.TopLevelControl))
            {
                Point point2 = base.Parent.PointToScreen(base.Parent.Location);
                Point point3 = base.TopLevelControl.PointToScreen(base.Parent.Location);
                p.Offset(point2.X - point3.X, point2.Y - point3.Y);
            }
            if (this.m_dropDownAlign == LeftRightAligment.Right)
            {
                p.Offset(base.ClientSize.Width - this.m_timePicker.Width, 0);
            }
            if ((p.Y + this.m_timePicker.Size.Height) > this.m_timePicker.Parent.ClientRectangle.Height)
            {
                p.Y -= (base.ClientSize.Height + this.m_timePicker.Size.Height) + 2;
                if (p.Y < 0)
                {
                    p.Y = this.m_timePicker.Parent.ClientRectangle.Height - this.m_timePicker.Size.Height;
                }
            }
            if ((base.TopLevelControl.PointToScreen(p).Y + this.m_timePicker.Size.Height) > (Screen.PrimaryScreen.WorkingArea.Y + Screen.PrimaryScreen.WorkingArea.Height))
            {
                Point point4 = new Point(p.X, (Screen.PrimaryScreen.WorkingArea.Top + Screen.PrimaryScreen.WorkingArea.Height) - this.m_timePicker.Size.Height);
                p.Y = base.TopLevelControl.PointToClient(point4).Y;
                if (p.Y < 0)
                {
                    p.Y = 0;
                }
            }
            if ((p.X + this.m_timePicker.Size.Width) > this.m_timePicker.Parent.ClientRectangle.Width)
            {
                p.X = this.m_timePicker.Parent.ClientRectangle.Width - this.m_timePicker.Size.Width;
            }
            this.m_timePicker.Location = p;
            this.m_timePicker.Show();
            this.m_timePicker.BringToFront();
            this.m_timePicker.Focus();
            if (this.m_timePicker.Visible)
            {
                this.OnDropDown(EventArgs.Empty);
            }
            else
            {
                this.OnCloseUp(EventArgs.Empty);
            }
        }

        private string TextParse(Graphics gr, DateTime value)
        {
            string longDatePattern = "";
            switch (this.m_format)
            {
                case Resco.Controls.OutlookControls.DateTimePickerFormat.Long:
                    longDatePattern = DateTimeFormatInfo.CurrentInfo.LongDatePattern;
                    break;

                case Resco.Controls.OutlookControls.DateTimePickerFormat.Short:
                    longDatePattern = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                    break;

                case Resco.Controls.OutlookControls.DateTimePickerFormat.Time:
                    longDatePattern = DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
                    break;

                default:
                    if ((this.m_customFormat != null) && (this.m_customFormat.Length > 0))
                    {
                        longDatePattern = this.m_customFormat;
                    }
                    else if (this.m_format == Resco.Controls.OutlookControls.DateTimePickerFormat.CustomTime)
                    {
                        longDatePattern = DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
                    }
                    else
                    {
                        longDatePattern = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                    }
                    break;
            }
            this.m_TextParts = new List<TextPart>();
            longDatePattern = longDatePattern + '\x00ff';
            char ch = '\x00ff';
            string format = "";
            bool flag = false;
            foreach (char ch2 in longDatePattern)
            {
                if (ch2 == '\'')
                {
                    if (flag)
                    {
                        format = format + '\x00ff';
                    }
                    flag = !flag;
                    if ((format != null) && (format.Length > 0))
                    {
                        this.m_TextParts.Add(new TextPart(gr, value, format, this.Font));
                        format = "";
                    }
                    if (flag)
                    {
                        format = format + '\x00ff';
                    }
                }
                else if (flag)
                {
                    format = format + ch2;
                }
                else
                {
                    if (((ch2 != ch) && (format != null)) && (format.Length > 0))
                    {
                        this.m_TextParts.Add(new TextPart(gr, value, format, this.Font));
                        format = "";
                    }
                    format = format + ch2;
                    ch = ch2;
                }
            }
            StringBuilder builder = new StringBuilder();
            foreach (TextPart part in this.m_TextParts)
            {
                builder.Append(part.Text);
            }
            return builder.ToString();
        }

        private void TimePickerRoller_SelectedIndexChanged(object sender, RollerItemEventArgs e)
        {
            this.m_IsNone = false;
            DateTime selectedTime = this.GetSelectedTime();
            if (!selectedTime.Equals(DateTime.MinValue))
            {
                this.m_dayPicker.Value = selectedTime;
            }
        }

        private void TimePickerUpdateSize()
        {
            this.m_timePicker.Height = 120 * ((int) this.m_scaleFactor.Height);
            int num = 7 * ((int) this.m_scaleFactor.Width);
            int num2 = (this.m_PredifinedRollers.HourRoller.Width + this.m_PredifinedRollers.MinuteRoller.Width) + (2 * num);
            if (!this.m_24hourFormat)
            {
                num2 += this.m_PredifinedRollers.AmAndPmRoller.Width;
            }
            this.m_timePicker.Width = num2;
        }

        private void UpdateDate()
        {
            DateTime selectedDate = this.GetSelectedDate();
            if (!selectedDate.Equals(DateTime.MinValue))
            {
                this.m_dayPicker.Value = selectedDate;
            }
            if (!this.m_dayPicker.Value.Date.Equals(this.m_Value.Date))
            {
                this.OnValueChanged(EventArgs.Empty);
            }
            base.Invalidate();
        }

        private void UpdateTime()
        {
            DateTime selectedTime = this.GetSelectedTime();
            if (!selectedTime.Equals(DateTime.MinValue))
            {
                this.m_timePicker.Value = selectedTime;
            }
            this.OnValueChanged(EventArgs.Empty);
            if ((this.m_timePicker.Value.Hour != this.m_Value.Hour) || (this.m_timePicker.Value.Minute != this.m_Value.Minute))
            {
                this.OnValueChanged(EventArgs.Empty);
            }
            base.Invalidate();
        }

        private void ValidateYear()
        {
            if (!this.m_doParse)
            {
                TextPart part = null;
                foreach (TextPart part2 in this.m_TextParts)
                {
                    if (part2.Format.IndexOf("y") >= 0)
                    {
                        part = part2;
                        break;
                    }
                }
                if (part != null)
                {
                    DateTime time = this.Value;
                    int year = time.Year;
                    try
                    {
                        year = Convert.ToDateTime(string.Concat(new object[] { time.Month, "-", time.Day, "-", part.Text, " ", time.Hour, ":", time.Minute, ":", time.Second }), DateTimeFormatInfo.InvariantInfo).Year;
                        if (year < this.MinDate.Year)
                        {
                            year = this.MinDate.Year;
                        }
                        else if (year > this.MaxDate.Year)
                        {
                            year = this.MaxDate.Year;
                        }
                    }
                    catch
                    {
                    }
                    this.Value = new DateTime(year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
                    this.m_doParse = true;
                    base.Invalidate();
                }
            }
            this.m_doParse = true;
        }

        public Color ActualItemForeColor
        {
            get
            {
                return this.m_dayPicker.Renderer.ColorActualItemForeColor;
            }
            set
            {
                this.m_dayPicker.Renderer.ColorActualItemForeColor = value;
                this.m_timePicker.Renderer.ColorActualItemForeColor = value;
            }
        }

        public Color BackColorOfCalendar
        {
            get
            {
                return this.m_dayPicker.Renderer.ColorCalendarFrameColor;
            }
            set
            {
                this.m_dayPicker.Renderer.ColorCalendarFrameColor = value;
                this.m_dayPicker.BottomBar.SetButtonsBackColor(value);
                this.m_timePicker.Renderer.ColorCalendarFrameColor = value;
            }
        }

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
                    base.Invalidate();
                }
            }
        }

        public Image BackgroundImage
        {
            get
            {
                return this.m_backgroundImage;
            }
            set
            {
                if (this.m_backgroundImage != value)
                {
                    this.m_backgroundImage = value;
                    base.Invalidate();
                }
            }
        }

        public Color CalendarColorWindowFrame
        {
            get
            {
                return TouchDTPRenderer.ColorWindowFrame;
            }
            set
            {
                TouchDTPRenderer.ColorWindowFrame = value;
            }
        }

        public string CustomFormat
        {
            get
            {
                return this.m_customFormat;
            }
            set
            {
                if (value != this.m_customFormat)
                {
                    this.m_customFormat = value;
                    base.Invalidate();
                }
            }
        }

        public Size DatePickerSize
        {
            get
            {
                return this.m_dayPicker.Size;
            }
            set
            {
                this.m_dayPicker.Size = value;
                if (this.m_dayPicker.Visible)
                {
                    this.m_dayPicker.Refresh();
                }
            }
        }

        public Color DefaultTextColor
        {
            get
            {
                return this.m_dayPicker.Renderer.ColorDefaultText;
            }
            set
            {
                this.m_dayPicker.Renderer.ColorDefaultText = value;
                this.m_timePicker.Renderer.ColorDefaultText = value;
            }
        }

        public LeftRightAligment DropDownAlign
        {
            get
            {
                return this.m_dropDownAlign;
            }
            set
            {
                if (value != this.m_dropDownAlign)
                {
                    this.m_dropDownAlign = value;
                }
            }
        }

        public int DropDownButtonWidth
        {
            get
            {
                return Const.DropDownWidth;
            }
            set
            {
                if (Const.DropDownWidth != value)
                {
                    Const.DropDownWidth = value;
                    Const.DropArrowSize = new Size(Const.DropDownWidth / 2, base.Height / 4);
                    this.CreateArrowPoints();
                    base.Invalidate();
                }
            }
        }

        public bool DropDownVisible
        {
            get
            {
                if (!this.m_dayPicker.Visible)
                {
                    return this.m_timePicker.Visible;
                }
                return true;
            }
            set
            {
                if ((this.m_format == Resco.Controls.OutlookControls.DateTimePickerFormat.Time) || (this.m_format == Resco.Controls.OutlookControls.DateTimePickerFormat.CustomTime))
                {
                    if (value != this.m_timePicker.Visible)
                    {
                        if (value)
                        {
                            this.ShowTimePicker();
                        }
                        else
                        {
                            this.OnTimePickerLostFocus(this, EventArgs.Empty);
                        }
                    }
                }
                else if (value != this.m_dayPicker.Visible)
                {
                    this.ShowDayPicker();
                }
            }
        }

        internal Resco.Controls.OutlookControls.Day FirstDayOfWeek
        {
            get
            {
                return this.m_dayPicker.FirstDayOfWeek;
            }
            set
            {
                this.m_dayPicker.FirstDayOfWeek = value;
                base.Invalidate();
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
                if (value != base.Font)
                {
                    base.Font = value;
                }
            }
        }

        public Resco.Controls.OutlookControls.DateTimePickerFormat Format
        {
            get
            {
                return this.m_format;
            }
            set
            {
                this.m_format = value;
                base.Invalidate();
            }
        }

        public Color InvertedTextColor
        {
            get
            {
                return this.m_dayPicker.Renderer.ColorInvertedText;
            }
            set
            {
                this.m_dayPicker.Renderer.ColorInvertedText = value;
                this.m_timePicker.Renderer.ColorInvertedText = value;
            }
        }

        public bool IsNone
        {
            get
            {
                return this.m_IsNone;
            }
            set
            {
                this.m_IsNone = value;
            }
        }

        public DateTime MaxDate
        {
            get
            {
                return this.m_dayPicker.MaxDate;
            }
            set
            {
                if (value != this.m_dayPicker.MaxDate)
                {
                    this.m_dayPicker.MaxDate = value;
                    string aStrDateFormat = ((this.m_RollersCustomDateFormat == null) || (this.m_RollersCustomDateFormat.Length == 0)) ? "dd MMMM yyyy" : this.m_RollersCustomDateFormat;
                    this.SetCustomDateFormat(aStrDateFormat);
                    this.m_dayPicker.Value = this.m_dayPicker.Value;
                }
            }
        }

        public DateTime MinDate
        {
            get
            {
                return this.m_dayPicker.MinDate;
            }
            set
            {
                if (value != this.m_dayPicker.MinDate)
                {
                    this.m_dayPicker.MinDate = value;
                    string aStrDateFormat = ((this.m_RollersCustomDateFormat == null) || (this.m_RollersCustomDateFormat.Length == 0)) ? "dd MMMM yyyy" : this.m_RollersCustomDateFormat;
                    this.SetCustomDateFormat(aStrDateFormat);
                    this.m_dayPicker.Value = this.m_dayPicker.Value;
                }
            }
        }

        public int MinuteInterval
        {
            get
            {
                return this.m_PredifinedRollers.MinuteInterval;
            }
            set
            {
                this.m_PredifinedRollers.MinuteInterval = value;
                this.m_timePicker.MinuteInterval = value;
                this.m_dayPicker.MinuteInterval = value;
            }
        }

        public string NoneText
        {
            get
            {
                return this.m_dayPicker.NoneText;
            }
            set
            {
                this.m_dayPicker.NoneText = value;
            }
        }

        public PredefinedRollers Rollers
        {
            get
            {
                return this.m_PredifinedRollers;
            }
        }

        public string RollersCustomDateFormat
        {
            get
            {
                return this.m_RollersCustomDateFormat;
            }
            set
            {
                if (value != this.m_RollersCustomDateFormat)
                {
                    this.m_RollersCustomDateFormat = value;
                    this.SetCustomDateFormat(this.m_RollersCustomDateFormat);
                    base.Invalidate();
                }
            }
        }

        public string RollersCustomTimeFormat
        {
            get
            {
                return this.m_RollersCustomTimeFormat;
            }
            set
            {
                if (value != this.m_RollersCustomTimeFormat)
                {
                    this.m_RollersCustomTimeFormat = value;
                    this.SetCustomTimeFormat(this.m_RollersCustomTimeFormat);
                    base.Invalidate();
                }
            }
        }

        internal bool ShowDate
        {
            get
            {
                return this.m_showDate;
            }
            set
            {
                if (value != this.m_showDate)
                {
                    this.m_showDate = value;
                    base.Invalidate();
                }
            }
        }

        public bool ShowDropDown
        {
            get
            {
                return this.m_ShowDropDown;
            }
            set
            {
                if (this.m_ShowDropDown != value)
                {
                    this.m_ShowDropDown = value;
                    base.Invalidate();
                }
            }
        }

        public bool ShowFocus
        {
            get
            {
                return this.m_ShowFocus;
            }
            set
            {
                if (this.m_ShowFocus != value)
                {
                    this.m_ShowFocus = value;
                    base.Invalidate();
                }
            }
        }

        public bool ShowNone
        {
            get
            {
                return this.m_dayPicker.ShowNone;
            }
            set
            {
                this.m_dayPicker.ShowNone = value;
            }
        }

        public override string Text
        {
            get
            {
                return this.m_TextCached;
            }
            set
            {
                this.Value = DateTime.Parse(value);
            }
        }

        public Size TimePickerSize
        {
            get
            {
                return this.m_timePicker.Size;
            }
            set
            {
                this.m_timePicker.Size = value;
                if (this.m_timePicker.Visible)
                {
                    this.m_timePicker.Refresh();
                }
            }
        }

        private DateTime TodayDate
        {
            get
            {
                if (!this.m_bTodayChanged)
                {
                    return DateTime.Today;
                }
                return this.m_today;
            }
            set
            {
                if (value != DateTime.Today)
                {
                    this.m_bTodayChanged = true;
                }
                else
                {
                    this.m_bTodayChanged = false;
                }
                if (value != this.m_today)
                {
                    this.m_today = value;
                    this.m_dayPicker.TodayDate = value;
                    base.Invalidate();
                }
            }
        }

        public string TodayText
        {
            get
            {
                return this.m_dayPicker.TodayText;
            }
            set
            {
                this.m_dayPicker.TodayText = value;
            }
        }

        public DateTime Value
        {
            get
            {
                this.m_Value = new DateTime(this.m_dayPicker.Value.Year, this.m_dayPicker.Value.Month, this.m_dayPicker.Value.Day, this.m_timePicker.Value.Hour, this.m_timePicker.Value.Minute, 0);
                return this.m_Value;
            }
            set
            {
                this.m_dayPicker.Value = value;
                this.m_timePicker.Value = value;
                base.Invalidate();
            }
        }

        public bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                if (!value)
                {
                    if (this.m_dayPicker.Visible)
                    {
                        this.m_dayPicker.Hide();
                    }
                    if (this.m_timePicker.Visible)
                    {
                        this.m_timePicker.Hide();
                    }
                }
            }
        }

        private class Const
        {
            public static Size DefaultDateTimePickerSize = new Size(200, 20);
            public static Font DefaultFont = new Font("Tahoma", 8f, FontStyle.Bold);
            public static Resco.Controls.OutlookControls.DateTimePickerFormat DefaultFormat = Resco.Controls.OutlookControls.DateTimePickerFormat.Long;
            public static DateTime DefaultStartDate = DateTime.Now;
            public static Size DropArrowSize = new Size(7, 4);
            public static int DropDownWidth = 13;
        }

        private class DayCell
        {
            private Rectangle m_bounds = new Rectangle(0, 0, 0, 0);
            private string m_day = "";
            private int m_space;

            public DayCell(string day, int space)
            {
                this.m_day = day;
                this.m_space = space;
            }

            public void Draw(Graphics gr, int posx, int height, Font font, SolidBrush brush, SolidBrush back, Pen frame, bool border)
            {
                Size size = gr.MeasureString(this.m_day, font).ToSize();
                Point point = new Point(posx + this.m_space, (height - size.Height) / 2);
                this.m_bounds.X = posx;
                this.m_bounds.Y = 2;
                this.m_bounds.Width = size.Width + (2 * this.m_space);
                this.m_bounds.Height = height - 4;
                if (back != null)
                {
                    gr.FillRectangle(back, this.m_bounds);
                }
                gr.DrawString(this.m_day, font, brush, (float) point.X, (float) point.Y);
                if (border)
                {
                    gr.DrawRectangle(frame, new Rectangle(this.m_bounds.X, this.m_bounds.Y, this.m_bounds.Width - 1, this.m_bounds.Height - 1));
                }
            }

            public Rectangle Bounds
            {
                get
                {
                    return this.m_bounds;
                }
            }
        }

        private delegate void InvokeDelegate();
    }
}

