namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Forms;

    public class OutlookDateTimePicker : UserControl
    {
        private const int KTimePickerDefaultHeight = 0x56;
        private Point[] m_arrowPoints = new Point[3];
        private bool m_backColorVistaStyle;
        private Image m_backgroundImage;
        private Rectangle m_backgroundImageRect = new Rectangle(0, 0, 1, 1);
        private Bitmap m_bmp;
        private SolidBrush m_brushBack;
        private SolidBrush m_brushDisabled;
        private SolidBrush m_brushFore;
        private SolidBrush m_brushFrame;
        private SolidBrush m_brushPushed;
        private SolidBrush m_brushSaturday;
        private SolidBrush m_brushSelectedBack;
        private SolidBrush m_brushSelectedFore;
        private SolidBrush m_brushSunday;
        private bool m_bTimePickerDirty;
        private bool m_bTodayChanged;
        private string m_customFormat = "";
        private OutlookMonthCalendar m_dayPicker = new OutlookMonthCalendar();
        private List<DayCell> m_Days = new List<DayCell>();
        private bool m_doParse = true;
        private Point[] m_downPoints = new Point[3];
        private bool m_downPressed;
        private LeftRightAligment m_dropDownAlign;
        private DateTime m_endTime = new DateTime(1, 1, 1, 0x17, 0x3b, 0);
        private Resco.Controls.OutlookControls.DateTimePickerFormat m_format = Resco.Controls.OutlookControls.DateTimePickerFormat.Long;
        private Graphics m_graphics;
        private Color m_highlightColor = SystemColors.Window;
        private ImageAttributes m_imageAttr;
        private Bitmap m_imageNext;
        private Bitmap m_imagePrev;
        private Bitmap m_imageToday;
        private int m_minuteInterval = 30;
        private bool m_mouseOver;
        private int m_mouseOverNav;
        private bool m_mouseOverToday;
        private NavigatorInterval m_navJumpTime = NavigatorInterval.Week;
        private Pen m_penFrame;
        private int m_pressedDay = -1;
        private int m_pressedNav;
        private bool m_pressedToday;
        private Rectangle m_rectNext = new Rectangle(0, 0, 0, 0);
        private Rectangle m_rectPrev = new Rectangle(0, 0, 0, 0);
        private Rectangle m_rectToday = new Rectangle(0, 0, 0, 0);
        private Color m_saturdayColor = Color.Blue;
        private int m_scaleFactor = 1;
        private Color m_selectedBackColor = SystemColors.ActiveCaption;
        private Color m_selectedForeColor = SystemColors.ActiveCaptionText;
        private bool m_showDate = true;
        private bool m_showDays = true;
        private bool m_showToday = true;
        private bool m_showUpDown;
        private bool m_showWeekNav = true;
        private DateTime m_startTime = new DateTime(1, 1, 1, 0, 0, 0);
        private OutlookDateTimePickerStyle m_style;
        private Color m_sundayColor = Color.Red;
        private string m_TextCached = "";
        private List<TextPart> m_TextParts = new List<TextPart>();
        private int m_TextSelected = -1;
        private ListBoxEx m_timePicker = new ListBoxEx();
        private Timer m_Timer = new Timer();
        private int m_TimerCount;
        private DateTime m_today = DateTime.Today;
        private Point[] m_upPoints = new Point[3];
        private bool m_upPressed;
        private DateTime m_Value;
        public static readonly DateTime MaxDateTime = DateTime.MaxValue;
        public static readonly DateTime MinDateTime = DateTime.MinValue;

        public event EventHandler CloseUp;

        public event CustomizeDayCellHandler CustomizeMonthCalendarDayCell;

        public event EventHandler DropDown;

        public event EventHandler NoneSelected;

        public event OutlookDateTimePickerTodayEventHandler TodayClick;

        public event EventHandler ValueChanged;

        static OutlookDateTimePicker()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(OutlookDateTimePicker), "");
            //}
        }

        public OutlookDateTimePicker()
        {
            using (Graphics graphics = base.CreateGraphics())
            {
                this.m_scaleFactor = (int) (graphics.DpiX / 96f);
            }
            this.m_Timer.Enabled = false;
            this.m_Timer.Interval = 500;
            this.m_Timer.Tick += new EventHandler(this.OnTimerTick);
            this.m_dayPicker.Visible = false;
            this.m_dayPicker.m_doCloseUp = true;
            this.m_dayPicker.CloseUp += new EventHandler(this.OnDayPickerCloseUp);
            this.m_dayPicker.ValueChanged += new EventHandler(this.OnDayPickerValueChanged);
            this.m_dayPicker.NoneSelected += new EventHandler(this.OnDayPickerNoneSelected);
            this.m_dayPicker.KeyUp += new KeyEventHandler(this.OnDayPickerKeyUp);
            this.m_dayPicker.CustomizeDayCell += new CustomizeDayCellHandler(this.dayPicker_CustomizeDayCell);
            this.m_timePicker.Visible = false;
            this.m_timePicker.KeyUp += new KeyEventHandler(this.OnTimePickerKeyUp);
            this.m_timePicker.Click += new EventHandler(this.OnTimePickerClick);
            this.m_timePicker.SelectedIndexChanged += new EventHandler(this.OnTimePickerSelectedIndexChanged);
            this.m_timePicker.LostFocus += new EventHandler(this.OnTimePickerLostFocus);
            this.m_timePicker.Height = 0x56 * this.m_scaleFactor;
            this.FillTimePicker();
            this.Format = Const.DefaultFormat;
            this.Value = Const.DefaultStartDate;
            base.Size = Const.DefaultDateTimePickerSize;
            this.Font = Const.DefaultFont;
            if (this.m_scaleFactor >= 2f)
            {
                this.m_imageToday = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Resco.Controls.OutlookControls.Images.Today_vga.gif"));
                this.m_imagePrev = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Resco.Controls.OutlookControls.Images.Prev_vga.gif"));
                this.m_imageNext = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Resco.Controls.OutlookControls.Images.Next_vga.gif"));
            }
            else
            {
                this.m_imageToday = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Resco.Controls.OutlookControls.Images.Today.gif"));
                this.m_imagePrev = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Resco.Controls.OutlookControls.Images.Prev.gif"));
                this.m_imageNext = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Resco.Controls.OutlookControls.Images.Next.gif"));
            }
            this.m_imageAttr = new ImageAttributes();
            this.m_imageAttr.SetColorKey(Color.FromArgb(0xff, 0, 0xff), Color.FromArgb(0xff, 0, 0xff));
            this.m_Value = this.m_dayPicker.Value;
            this.m_backColorVistaStyle = false;
        }

        private void ChangeDay(int dir)
        {
            DateTime time = this.m_Value.AddDays((double) dir);
            if ((time.CompareTo(this.MaxDate) <= 0) && (time.CompareTo(this.MinDate) >= 0))
            {
                this.m_dayPicker.IsNone = false;
                this.m_Value = time;
                this.Value = this.m_Value;
            }
        }

        private void ChangeMonth(int dir)
        {
            DateTime time = this.m_Value.AddMonths(dir);
            if ((time.CompareTo(this.MaxDate) <= 0) && (time.CompareTo(this.MinDate) >= 0))
            {
                this.m_dayPicker.IsNone = false;
                this.m_Value = time;
                this.Value = this.m_Value;
            }
        }

        private void ChangeWeek(int dir)
        {
            DateTime time = this.m_Value.AddDays((double) (dir * 7));
            if ((time.CompareTo(this.MaxDate) <= 0) && (time.CompareTo(this.MinDate) >= 0))
            {
                this.m_dayPicker.IsNone = false;
                for (int i = 0; i < 7; i++)
                {
                    this.m_Value = this.m_Value.AddDays((double) dir);
                    base.Invalidate();
                    Application.DoEvents();
                }
                this.Value = this.m_Value;
            }
        }

        private void ChangeYear(int dir)
        {
            DateTime time = this.m_Value.AddYears(dir);
            if ((time.CompareTo(this.MaxDate) <= 0) && (time.CompareTo(this.MinDate) >= 0))
            {
                this.m_dayPicker.IsNone = false;
                this.m_Value = time;
                this.Value = this.m_Value;
            }
        }

        private void CreateArrowPoints()
        {
            if (Const.DropDownWidth <= 13)
            {
                Const.DropArrowSize = new Size(7, 4);
            }
            else
            {
                Const.DropArrowSize = new Size((this.m_scaleFactor * Const.DropDownWidth) / 2, base.Height / 4);
            }
            this.m_arrowPoints[0].X = base.ClientSize.Width - (Const.DropArrowSize.Width + (this.m_scaleFactor * 4));
            this.m_arrowPoints[0].Y = (base.ClientSize.Height - (Const.DropArrowSize.Height - 1)) / 2;
            this.m_arrowPoints[1].X = (this.m_arrowPoints[0].X + Const.DropArrowSize.Width) + ((this.m_scaleFactor == 2) ? 1 : 0);
            this.m_arrowPoints[1].Y = this.m_arrowPoints[0].Y;
            this.m_arrowPoints[2].X = this.m_arrowPoints[0].X + (Const.DropArrowSize.Width / 2);
            this.m_arrowPoints[2].Y = this.m_arrowPoints[0].Y + Const.DropArrowSize.Height;
            this.m_downPoints[0].X = base.ClientSize.Width - (((this.m_scaleFactor * Const.DropDownWidth) - Const.DropArrowSize.Width) / 2);
            this.m_downPoints[0].Y = (base.ClientSize.Height / 2) + (((base.ClientSize.Height / 2) - Const.DropArrowSize.Height) / 2);
            this.m_downPoints[1].X = this.m_downPoints[0].X - Const.DropArrowSize.Width;
            this.m_downPoints[1].Y = this.m_downPoints[0].Y;
            this.m_downPoints[2].X = this.m_downPoints[0].X - (Const.DropArrowSize.Width / 2);
            this.m_downPoints[2].Y = this.m_downPoints[0].Y + Const.DropArrowSize.Height;
            this.m_upPoints[0].X = this.m_downPoints[0].X;
            this.m_upPoints[0].Y = (((base.ClientSize.Height / 2) - Const.DropArrowSize.Height) / 2) + Const.DropArrowSize.Height;
            this.m_upPoints[1].X = this.m_upPoints[0].X - Const.DropArrowSize.Width;
            this.m_upPoints[1].Y = this.m_upPoints[0].Y;
            this.m_upPoints[2].X = this.m_upPoints[0].X - (Const.DropArrowSize.Width / 2);
            this.m_upPoints[2].Y = this.m_upPoints[0].Y - (Const.DropArrowSize.Height + 1);
        }

        private void CreateArrows(int left)
        {
            if (left != this.m_rectPrev.X)
            {
                this.m_rectPrev.X = left;
                this.m_rectPrev.Y = 2;
                this.m_rectPrev.Width = this.m_imagePrev.Width + (7 * this.m_scaleFactor);
                this.m_rectPrev.Height = (base.ClientSize.Height - 4) - 1;
                this.m_rectNext.X = (left + this.m_rectPrev.Width) + 6;
                this.m_rectNext.Y = 2;
                this.m_rectNext.Width = this.m_imagePrev.Width + (7 * this.m_scaleFactor);
                this.m_rectNext.Height = (base.ClientSize.Height - 4) - 1;
            }
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
            if ((this.m_brushPushed == null) || (this.m_brushPushed.Color != this.HighlightColor))
            {
                this.m_brushPushed = new SolidBrush(this.HighlightColor);
            }
            if ((this.m_brushSaturday == null) || (this.m_brushSaturday.Color != this.SaturdayColor))
            {
                this.m_brushSaturday = new SolidBrush(this.SaturdayColor);
            }
            if ((this.m_brushSunday == null) || (this.m_brushSunday.Color != this.SundayColor))
            {
                this.m_brushSunday = new SolidBrush(this.SundayColor);
            }
            if ((this.m_brushSelectedFore == null) || (this.m_brushSelectedFore.Color != this.SelectedForeColor))
            {
                this.m_brushSelectedFore = new SolidBrush(this.SelectedForeColor);
            }
            if ((this.m_brushSelectedBack == null) || (this.m_brushSelectedBack.Color != this.SelectedBackColor))
            {
                this.m_brushSelectedBack = new SolidBrush(this.SelectedBackColor);
            }
        }

        private void CreateMemoryBitmap()
        {
            if (((this.m_bmp == null) || (this.m_bmp.Width != base.ClientSize.Width)) || (this.m_bmp.Height != base.ClientSize.Height))
            {
                this.m_bmp = new Bitmap(base.ClientSize.Width, base.ClientSize.Height);
                this.m_graphics = Graphics.FromImage(this.m_bmp);
                this.CreateArrowPoints();
            }
        }

        private void DateIncDec(int direction)
        {
            if ((this.m_TextSelected >= 0) && (this.m_TextSelected < this.m_TextParts.Count))
            {
                TextPart part = this.m_TextParts[this.m_TextSelected];
                DateTime time = this.Value;
                if ((part.Format.IndexOf("h") >= 0) || (part.Format.IndexOf("H") >= 0))
                {
                    this.Value = time.AddHours((double) direction);
                }
                else if (part.Format.IndexOf("m") >= 0)
                {
                    this.Value = time.AddMinutes((double) direction);
                }
                else if (part.Format.IndexOf("d") >= 0)
                {
                    this.Value = time.AddDays((double) direction);
                }
                else if (part.Format.IndexOf("M") >= 0)
                {
                    this.Value = time.AddMonths(direction);
                }
                else if (part.Format.IndexOf("y") >= 0)
                {
                    this.Value = time.AddYears(direction);
                }
                else if (part.Format.IndexOf("s") >= 0)
                {
                    this.Value = time.AddSeconds((double) direction);
                }
                else if (part.Format.IndexOf("t") >= 0)
                {
                    this.Value = time.AddHours((double) (12 * direction));
                }
            }
        }

        private void dayPicker_CustomizeDayCell(object sender, DayCellEventArgs e)
        {
            this.OnCustomizeMonthCalendarDayCell(e);
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

        private int DrawDaysOfWeek(int left, Graphics g)
        {
            string weekDayNames = this.GetWeekDayNames();
            int num = 0;
            this.m_Days.Clear();
            DayOfWeek dayOfWeek = this.m_Value.DayOfWeek;
            int num2 = (this.TodayDate.Year * 100) + CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(this.TodayDate, DateTimeFormatInfo.CurrentInfo.CalendarWeekRule, this.GetFirstDayOfWeek());
            int num3 = (this.m_Value.Year * 100) + CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(this.m_Value, DateTimeFormatInfo.CurrentInfo.CalendarWeekRule, this.GetFirstDayOfWeek());
            for (int i = 0; i < 7; i++)
            {
                DayOfWeek week2 = (DayOfWeek)((i + (int)this.GetFirstDayOfWeek()) % 7);
                string day = weekDayNames[(int) week2].ToString();
                bool flag = false;
                if ((num3 == num2) && (week2 == this.TodayDate.DayOfWeek))
                {
                    flag = true;
                }
                DayCell cell = new DayCell(day, 3);
                cell.Draw(g, left, base.ClientSize.Height, this.Font, ((this.m_pressedDay == i) && this.m_mouseOver) ? this.m_brushFore : (((dayOfWeek == week2) && !this.m_dayPicker.IsNone) ? this.m_brushSelectedFore : ((week2 == DayOfWeek.Saturday) ? this.m_brushSaturday : ((week2 == DayOfWeek.Sunday) ? this.m_brushSunday : this.m_brushFore))), ((this.m_pressedDay == i) && this.m_mouseOver) ? this.m_brushPushed : (((dayOfWeek == week2) && !this.m_dayPicker.IsNone) ? this.m_brushSelectedBack : (flag ? this.m_brushPushed : null)), this.m_penFrame, flag | ((this.m_pressedDay == i) && this.m_mouseOver));
                left += cell.Bounds.Width;
                num += cell.Bounds.Width;
                this.m_Days.Add(cell);
            }
            return num;
        }

        private int DrawNavigation(int left, Graphics g)
        {
            int num = 0;
            this.CreateArrows(left);
            if (this.m_mouseOverNav == -1)
            {
                g.FillRectangle(this.m_brushPushed, this.m_rectPrev);
                g.DrawRectangle(this.m_penFrame, this.m_rectPrev);
            }
            Rectangle destRect = new Rectangle(this.m_rectPrev.X + (5 * this.m_scaleFactor), (base.ClientSize.Height - this.m_imagePrev.Height) / 2, this.m_imagePrev.Width, this.m_imagePrev.Height);
            g.DrawImage(this.m_imagePrev, destRect, 0, 0, this.m_imagePrev.Width, this.m_imagePrev.Height, GraphicsUnit.Pixel, this.m_imageAttr);
            num += this.m_rectPrev.Width;
            int num2 = (left + this.m_rectPrev.Width) + 3;
            g.DrawLine(this.m_penFrame, num2, 2, num2, base.ClientSize.Height - 3);
            num += 4;
            if (this.m_mouseOverNav == 1)
            {
                g.FillRectangle(this.m_brushPushed, this.m_rectNext);
                g.DrawRectangle(this.m_penFrame, this.m_rectNext);
            }
            destRect = new Rectangle(this.m_rectNext.X + (3 * this.m_scaleFactor), (base.ClientSize.Height - this.m_imageNext.Height) / 2, this.m_imageNext.Width, this.m_imageNext.Height);
            g.DrawImage(this.m_imageNext, destRect, 0, 0, this.m_imageNext.Width, this.m_imagePrev.Height, GraphicsUnit.Pixel, this.m_imageAttr);
            return (num + this.m_rectNext.Width);
        }

        private void DrawTodayButton(int left, Graphics g)
        {
            this.m_rectToday.X = left;
            this.m_rectToday.Y = 2;
            this.m_rectToday.Width = (this.m_imageToday.Width + 4) - 1;
            this.m_rectToday.Height = (base.ClientSize.Height - 4) - 1;
            if (this.m_mouseOverToday)
            {
                g.FillRectangle(this.m_brushPushed, this.m_rectToday);
                g.DrawRectangle(this.m_penFrame, this.m_rectToday);
            }
            Rectangle destRect = new Rectangle(left + 2, (base.ClientSize.Height - this.m_imageToday.Height) / 2, this.m_imageToday.Width, this.m_imageToday.Height);
            g.DrawImage(this.m_imageToday, destRect, 0, 0, this.m_imageToday.Width, this.m_imageToday.Height, GraphicsUnit.Pixel, this.m_imageAttr);
        }

        private void FillTimePicker()
        {
            this.m_timePicker.Items.Clear();
            DateTime startTime = this.m_startTime;
            using (Graphics graphics = base.CreateGraphics())
            {
                while (startTime <= this.m_endTime)
                {
                    string str = this.TextParse(graphics, startTime);
                    this.m_timePicker.Items.Add(str);
                    startTime = startTime.AddMinutes((double) this.m_minuteInterval);
                }
            }
        }

        private DayOfWeek GetFirstDayOfWeek()
        {
            if (this.FirstDayOfWeek == Resco.Controls.OutlookControls.Day.Default)
            {
                return DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;
            }
            return (DayOfWeek) this.FirstDayOfWeek;
        }

        private string GetWeekDayNames()
        {
            string str = "";
            if ((this.DaysOfWeek != null) && (this.DaysOfWeek != ""))
            {
                if (this.DaysOfWeek.Length < 7)
                {
                    this.DaysOfWeek = this.DaysOfWeek.PadRight(7, ' ');
                }
                return this.DaysOfWeek;
            }
            int startIndex = 0;
            if (CultureInfo.CurrentCulture.Name.StartsWith("zh"))
            {
                startIndex = 2;
            }
            for (int i = 0; i < 7; i++)
            {
                str = str + DateTimeFormatInfo.CurrentInfo.GetDayName((DayOfWeek) i).Substring(startIndex, 1);
            }
            return str;
        }

        public void GoToEditMode()
        {
            if (this.m_dayPicker.IsNone)
            {
                this.m_TextSelected = 0;
                base.Invalidate();
            }
            else
            {
                this.ValidateYear();
                this.m_TextSelected = -1;
                do
                {
                    this.m_TextSelected++;
                    if (this.m_TextSelected >= this.m_TextParts.Count)
                    {
                        this.m_TextSelected = -1;
                        break;
                    }
                }
                while (!this.m_TextParts[this.m_TextSelected].IsEditable);
                base.Invalidate();
            }
        }

        private void HandleNavigationButton(int dir)
        {
            switch (this.m_navJumpTime)
            {
                case NavigatorInterval.Day:
                    this.ChangeDay(dir);
                    return;

                case NavigatorInterval.Week:
                    this.ChangeWeek(dir);
                    return;

                case NavigatorInterval.Month:
                    this.ChangeMonth(dir);
                    return;

                case NavigatorInterval.Year:
                    this.ChangeYear(dir);
                    return;
            }
        }

        public void Hide()
        {
            this.Visible = false;
        }

        protected virtual void OnCloseUp(EventArgs e)
        {
            if (this.CloseUp != null)
            {
                this.CloseUp(this, e);
            }
        }

        private void OnCustomizeMonthCalendarDayCell(DayCellEventArgs e)
        {
            if (this.CustomizeMonthCalendarDayCell != null)
            {
                this.CustomizeMonthCalendarDayCell(this.m_dayPicker, e);
            }
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

        private void OnDayPickerNoneSelected(object sender, EventArgs e)
        {
            if (this.m_style == OutlookDateTimePickerStyle.WeekDayPicker)
            {
                this.m_Value = this.m_dayPicker.Value;
            }
            base.Invalidate();
            this.OnNoneSelected(e);
        }

        private void OnDayPickerValueChanged(object sender, EventArgs e)
        {
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.m_style != OutlookDateTimePickerStyle.DateTimePicker)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        this.Value = this.Value.AddDays(-1.0);
                        goto Label_0175;

                    case Keys.Up:
                        base.Parent.SelectNextControl(this, false, true, false, false);
                        goto Label_0175;

                    case Keys.Right:
                        this.Value = this.Value.AddDays(1.0);
                        goto Label_0175;

                    case Keys.Down:
                        base.Parent.SelectNextControl(this, true, true, false, false);
                        goto Label_0175;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        this.SelectPrevious();
                        break;

                    case Keys.Up:
                        if (!this.m_dayPicker.IsNone || (this.m_TextSelected != 0))
                        {
                            if (this.m_TextSelected == -1)
                            {
                                base.Parent.SelectNextControl(this, false, true, false, false);
                            }
                            else
                            {
                                this.DateIncDec(1);
                            }
                            break;
                        }
                        this.m_dayPicker.IsNone = false;
                        base.Invalidate();
                        break;

                    case Keys.Right:
                        this.SelectNext();
                        break;

                    case Keys.Down:
                        if (!this.m_dayPicker.IsNone || (this.m_TextSelected != 0))
                        {
                            if (this.m_TextSelected == -1)
                            {
                                base.Parent.SelectNextControl(this, true, true, false, false);
                            }
                            else
                            {
                                this.DateIncDec(-1);
                            }
                            break;
                        }
                        this.m_dayPicker.IsNone = false;
                        base.Invalidate();
                        break;
                }
                if (this.m_TextSelected < 0)
                {
                    base.OnKeyDown(e);
                }
                return;
            }
        Label_0175:
            base.OnKeyDown(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            int v = 0;
            int num2 = 0;
            if (this.m_style == OutlookDateTimePickerStyle.DateTimePicker)
            {
                if ((this.m_TextSelected < 0) || (this.m_TextSelected >= this.m_TextParts.Count))
                {
                    base.OnKeyPress(e);
                    return;
                }
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
            }
            base.OnKeyPress(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if ((this.m_style == OutlookDateTimePickerStyle.DateTimePicker) && ((this.m_format == Resco.Controls.OutlookControls.DateTimePickerFormat.Time) || (this.m_format == Resco.Controls.OutlookControls.DateTimePickerFormat.CustomTime)))
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
            if (!this.m_dayPicker.Focused)
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
            if (this.m_style == OutlookDateTimePickerStyle.DateTimePicker)
            {
                Rectangle rectangle = new Rectangle(base.ClientSize.Width - (this.m_scaleFactor * Const.DropDownWidth), 0, this.m_scaleFactor * Const.DropDownWidth, base.ClientSize.Height);
                if (!rectangle.Contains(e.X, e.Y))
                {
                    if (!this.m_dayPicker.IsNone)
                    {
                        int x = 4;
                        foreach (TextPart part in this.m_TextParts)
                        {
                            rectangle = new Rectangle(x, 0, part.Size.Width, base.ClientSize.Height);
                            if (rectangle.Contains(e.X, e.Y) && part.IsEditable)
                            {
                                this.m_TextSelected = this.m_TextParts.IndexOf(part);
                                base.Invalidate();
                                return;
                            }
                            x += part.Size.Width;
                        }
                        this.m_TextSelected = -1;
                        base.Invalidate();
                    }
                }
                else if (this.m_showUpDown)
                {
                    this.m_upPressed = false;
                    this.m_downPressed = false;
                    if (e.Y < (base.ClientSize.Height / 2))
                    {
                        this.m_upPressed = true;
                    }
                    else
                    {
                        this.m_downPressed = true;
                    }
                    this.m_TimerCount = 0;
                    this.m_Timer.Interval = 500;
                    this.m_Timer.Enabled = true;
                    base.Invalidate();
                }
                else if ((this.m_format != Resco.Controls.OutlookControls.DateTimePickerFormat.Time) && (this.m_format != Resco.Controls.OutlookControls.DateTimePickerFormat.CustomTime))
                {
                    if (!visible)
                    {
                        this.ShowDayPicker();
                    }
                }
                else if (!flag2)
                {
                    this.ShowTimePicker();
                }
            }
            else if (!visible && (this.m_graphics != null))
            {
                Size size = this.m_graphics.MeasureString(this.Text, this.Font).ToSize();
                Rectangle rectangle2 = new Rectangle(0, 0, size.Width, base.ClientSize.Height);
                if (this.m_showDate && rectangle2.Contains(e.X, e.Y))
                {
                    this.ShowDayPicker();
                }
                this.m_pressedDay = -1;
                this.m_mouseOver = false;
                if (this.m_showDays)
                {
                    int num2 = this.m_Days.Count;
                    for (int i = 0; i < num2; i++)
                    {
                        if (this.m_Days[i].Bounds.Contains(e.X, e.Y))
                        {
                            this.m_pressedDay = i;
                            this.m_mouseOver = true;
                            base.Invalidate();
                        }
                    }
                }
                this.m_pressedToday = false;
                this.m_mouseOverToday = false;
                if (this.m_showToday && this.m_rectToday.Contains(e.X, e.Y))
                {
                    this.m_pressedToday = true;
                    this.m_mouseOverToday = true;
                    base.Invalidate();
                }
                this.m_pressedNav = 0;
                this.m_mouseOverNav = 0;
                if (this.m_showWeekNav && this.m_rectPrev.Contains(e.X, e.Y))
                {
                    this.m_pressedNav = -1;
                    this.m_mouseOverNav = -1;
                    base.Invalidate();
                }
                else if (this.m_showWeekNav && this.m_rectNext.Contains(e.X, e.Y))
                {
                    this.m_pressedNav = 1;
                    this.m_mouseOverNav = 1;
                    base.Invalidate();
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.m_style == OutlookDateTimePickerStyle.WeekDayPicker)
            {
                if (((this.m_pressedDay >= 0) && (this.m_pressedDay < this.m_Days.Count)) && this.m_Days[this.m_pressedDay].Bounds.Contains(e.X, e.Y))
                {
                    if (!this.m_mouseOver)
                    {
                        this.m_mouseOver = true;
                        base.Invalidate();
                    }
                }
                else if (this.m_mouseOver)
                {
                    this.m_mouseOver = false;
                    base.Invalidate();
                }
                if (this.m_pressedToday && this.m_rectToday.Contains(e.X, e.Y))
                {
                    if (!this.m_mouseOverToday)
                    {
                        this.m_mouseOverToday = true;
                        base.Invalidate();
                    }
                }
                else if (this.m_mouseOverToday)
                {
                    this.m_mouseOverToday = false;
                    base.Invalidate();
                }
                if ((this.m_pressedNav == -1) && this.m_rectPrev.Contains(e.X, e.Y))
                {
                    if (this.m_mouseOverNav != -1)
                    {
                        this.m_mouseOverNav = -1;
                        base.Invalidate();
                    }
                }
                else if (this.m_mouseOverNav == -1)
                {
                    this.m_mouseOverNav = 0;
                    base.Invalidate();
                }
                if ((this.m_pressedNav == 1) && this.m_rectNext.Contains(e.X, e.Y))
                {
                    if (this.m_mouseOverNav != 1)
                    {
                        this.m_mouseOverNav = 1;
                        base.Invalidate();
                    }
                }
                else if (this.m_mouseOverNav == 1)
                {
                    this.m_mouseOverNav = 0;
                    base.Invalidate();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.m_Timer.Enabled = false;
            if (this.m_style == OutlookDateTimePickerStyle.DateTimePicker)
            {
                Rectangle rectangle = new Rectangle(base.ClientSize.Width - (this.m_scaleFactor * Const.DropDownWidth), 0, this.m_scaleFactor * Const.DropDownWidth, base.ClientSize.Height);
                if (this.m_showUpDown && rectangle.Contains(e.X, e.Y))
                {
                    if ((e.Y < (base.ClientSize.Height / 2)) && this.m_upPressed)
                    {
                        this.DateIncDec(1);
                    }
                    else if ((e.Y > (base.ClientSize.Height / 2)) && this.m_downPressed)
                    {
                        this.DateIncDec(-1);
                    }
                }
                this.m_upPressed = false;
                this.m_downPressed = false;
                base.Invalidate();
            }
            else
            {
                if (this.m_mouseOver)
                {
                    this.SelectDay(this.m_pressedDay);
                }
                if (this.m_mouseOverToday)
                {
                    OutlookDateTimePickerTodayEventArgs args = new OutlookDateTimePickerTodayEventArgs(this.TodayDate);
                    if (this.TodayClick != null)
                    {
                        this.TodayClick(this, args);
                    }
                    this.Value = args.Date;
                }
                if (this.m_mouseOverNav != 0)
                {
                    int pressedNav = this.m_pressedNav;
                    this.m_pressedNav = 0;
                    this.m_mouseOverNav = 0;
                    this.HandleNavigationButton(pressedNav);
                }
                this.m_pressedDay = -1;
                this.m_mouseOver = false;
                this.m_pressedToday = false;
                this.m_mouseOverToday = false;
                this.m_pressedNav = 0;
                this.m_mouseOverNav = 0;
                base.Invalidate();
            }
        }

        protected virtual void OnNoneSelected(EventArgs e)
        {
            if (this.NoneSelected != null)
            {
                this.NoneSelected(this, e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int left = 0;
            int x = 4;
            double num3 = 0.0;
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
            if (this.Focused)
            {
                Rectangle rect = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
                using (Pen pen = new Pen(Color.Black))
                {
                    this.m_graphics.DrawRectangle(pen, rect);
                }
            }
            if ((this.m_style == OutlookDateTimePickerStyle.DateTimePicker) || this.m_showDate)
            {
                if (this.m_dayPicker.IsNone)
                {
                    Size size = this.m_graphics.MeasureString(this.m_dayPicker.NoneText, this.Font).ToSize();
                    if ((this.m_style == OutlookDateTimePickerStyle.DateTimePicker) && (this.m_TextSelected == 0))
                    {
                        this.m_graphics.FillRectangle(this.m_brushSelectedBack, x, (base.ClientSize.Height - size.Height) / 2, size.Width, size.Height);
                        this.m_graphics.DrawString(this.m_dayPicker.NoneText, this.Font, base.Enabled ? this.m_brushSelectedFore : this.m_brushDisabled, (float) x, (float) ((base.ClientSize.Height - size.Height) / 2));
                    }
                    else
                    {
                        this.m_graphics.DrawString(this.m_dayPicker.NoneText, this.Font, base.Enabled ? this.m_brushFore : this.m_brushDisabled, (float) x, (float) ((base.ClientSize.Height - size.Height) / 2));
                    }
                    x += size.Width + 3;
                }
                else
                {
                    foreach (TextPart part in this.m_TextParts)
                    {
                        if ((this.m_style == OutlookDateTimePickerStyle.DateTimePicker) && (this.m_TextSelected == this.m_TextParts.IndexOf(part)))
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
                }
                x += 3;
                num3 += Const.PctToDays;
            }
            if (this.m_style == OutlookDateTimePickerStyle.DateTimePicker)
            {
                if (!this.m_showUpDown)
                {
                    this.m_graphics.FillPolygon(this.m_brushFrame, this.m_arrowPoints);
                }
                else
                {
                    if (this.m_upPressed)
                    {
                        this.m_graphics.FillRectangle(this.m_brushFrame, base.ClientSize.Width - (this.m_scaleFactor * Const.DropDownWidth), 0, this.m_scaleFactor * Const.DropDownWidth, base.ClientSize.Height / 2);
                    }
                    else
                    {
                        this.m_graphics.DrawRectangle(this.m_penFrame, base.ClientSize.Width - (this.m_scaleFactor * Const.DropDownWidth), 0, (this.m_scaleFactor * Const.DropDownWidth) - 1, (base.ClientSize.Height - 1) / 2);
                    }
                    if (this.m_downPressed)
                    {
                        this.m_graphics.FillRectangle(this.m_brushFrame, (int) (base.ClientSize.Width - (this.m_scaleFactor * Const.DropDownWidth)), (int) (base.ClientSize.Height / 2), (int) (this.m_scaleFactor * Const.DropDownWidth), (int) (base.ClientSize.Height / 2));
                    }
                    else
                    {
                        this.m_graphics.DrawRectangle(this.m_penFrame, (int) (base.ClientSize.Width - (this.m_scaleFactor * Const.DropDownWidth)), (int) (base.ClientSize.Height / 2), (int) ((this.m_scaleFactor * Const.DropDownWidth) - 1), (int) ((base.ClientSize.Height - 1) / 2));
                    }
                    this.m_graphics.FillPolygon(this.m_upPressed ? this.m_brushBack : this.m_brushFrame, this.m_upPoints);
                    this.m_graphics.FillPolygon(this.m_downPressed ? this.m_brushBack : this.m_brushFrame, this.m_downPoints);
                }
            }
            else
            {
                if (this.m_showDays)
                {
                    left = (int) (base.ClientSize.Width * num3);
                    if (left < x)
                    {
                        left = x;
                    }
                    else
                    {
                        x = left;
                    }
                    int num4 = this.DrawDaysOfWeek(left, this.m_graphics);
                    x += num4 + 3;
                    num3 += Const.PctToToday;
                }
                if (this.m_showToday)
                {
                    left = base.Width - (this.m_imageToday.Width + 6);
                    if (this.m_showWeekNav)
                    {
                        left -= (this.m_imagePrev.Width + this.m_imageNext.Width) + (0x16 * this.m_scaleFactor);
                    }
                    this.DrawTodayButton(left, this.m_graphics);
                    x += this.m_rectToday.Width;
                    num3 += Const.PctToWeekNav;
                }
                if (this.m_showWeekNav)
                {
                    left = base.Width - ((this.m_imagePrev.Width + this.m_imageNext.Width) + (0x16 * this.m_scaleFactor));
                    this.DrawNavigation(left, this.m_graphics);
                }
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
            this.m_timePicker.Hide();
            base.Focus();
            this.OnCloseUp(e);
        }

        private void OnTimePickerSelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateTime();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (this.m_showUpDown)
            {
                if (this.m_upPressed)
                {
                    this.DateIncDec((this.m_TimerCount < 10) ? 1 : 10);
                }
                else if (this.m_downPressed)
                {
                    this.DateIncDec((this.m_TimerCount < 10) ? -1 : -10);
                }
            }
            this.m_Timer.Interval = 100;
            this.m_TimerCount++;
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
            base.ScaleControl(factor, specified);
        }

        private void SelectDay(int index)
        {
            int num = ((7 + this.Value.DayOfWeek) - this.GetFirstDayOfWeek()) % 7;
            this.Value = this.Value.AddDays((double) (index - num));
            this.m_dayPicker.IsNone = false;
        }

        private void SelectNext()
        {
            if (this.m_dayPicker.IsNone)
            {
                this.m_TextSelected = (this.m_TextSelected >= 0) ? -1 : 0;
                base.Invalidate();
            }
            else
            {
                this.ValidateYear();
                do
                {
                    this.m_TextSelected++;
                    if (this.m_TextSelected >= this.m_TextParts.Count)
                    {
                        this.m_TextSelected = -1;
                        break;
                    }
                }
                while (!this.m_TextParts[this.m_TextSelected].IsEditable);
                base.Invalidate();
            }
        }

        private void SelectPrevious()
        {
            if (this.m_dayPicker.IsNone)
            {
                this.m_TextSelected = (this.m_TextSelected >= 0) ? -1 : 0;
                base.Invalidate();
                return;
            }
            this.ValidateYear();
        Label_002D:
            if (this.m_TextSelected < 0)
            {
                this.m_TextSelected = this.m_TextParts.Count - 1;
            }
            else
            {
                this.m_TextSelected--;
            }
            if (this.m_TextSelected < 0)
            {
                this.m_TextSelected = -1;
            }
            else if (!this.m_TextParts[this.m_TextSelected].IsEditable)
            {
                goto Label_002D;
            }
            base.Invalidate();
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

        protected virtual bool ShouldSerializeAnnuallyBoldedDates()
        {
            return (this.m_dayPicker.AnnuallyBoldedDates.Length > 0);
        }

        protected virtual bool ShouldSerializeBoldedDates()
        {
            return (this.m_dayPicker.BoldedDates.Length > 0);
        }

        protected virtual bool ShouldSerializeCalendarWeekRule()
        {
            if (this.m_dayPicker.CalendarWeekRule == DateTimeFormatInfo.CurrentInfo.CalendarWeekRule)
            {
                return false;
            }
            return true;
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

        protected virtual bool ShouldSerializeMonthlyBoldedDates()
        {
            return (this.m_dayPicker.MonthlyBoldedDates.Length > 0);
        }

        protected bool ShouldSerializeStyle()
        {
            if (this.m_style == OutlookDateTimePickerStyle.DateTimePicker)
            {
                return false;
            }
            return true;
        }

        protected bool ShouldSerializeTimePickerEndTime()
        {
            if ((this.m_endTime.Hour == 0x17) && (this.m_endTime.Minute == 0x3b))
            {
                return false;
            }
            return true;
        }

        protected bool ShouldSerializeTimePickerMinuteInterval()
        {
            if (this.TimePickerMinuteInterval == 30)
            {
                return false;
            }
            return true;
        }

        protected bool ShouldSerializeTimePickerStartTime()
        {
            if ((this.m_startTime.Hour == 0) && (this.m_startTime.Minute == 0))
            {
                return false;
            }
            return true;
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
            Point point2 = base.Parent.PointToScreen(base.Parent.Location);
            Point point3 = base.TopLevelControl.PointToScreen(base.Parent.Location);
            if ((this.m_dropDownAlign == LeftRightAligment.Left) || (this.m_style == OutlookDateTimePickerStyle.WeekDayPicker))
            {
                p.Offset(point2.X - point3.X, point2.Y - point3.Y);
            }
            else
            {
                p.Offset(((point2.X - point3.X) + base.ClientSize.Width) - this.m_dayPicker.Width, point2.Y - point3.Y);
            }
            if ((p.Y + this.m_dayPicker.Size.Height) > base.TopLevelControl.ClientRectangle.Height)
            {
                p.Y -= (base.ClientSize.Height + this.m_dayPicker.Size.Height) + 2;
                if (p.Y < 0)
                {
                    p.Y = base.TopLevelControl.ClientRectangle.Height - this.m_dayPicker.Size.Height;
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
            if ((p.X + this.m_dayPicker.Size.Width) > base.TopLevelControl.ClientRectangle.Width)
            {
                p.X = base.TopLevelControl.ClientRectangle.Width - this.m_dayPicker.Size.Width;
            }
            this.m_dayPicker.Display(!this.m_dayPicker.Visible, p.X, p.Y, this.m_dayPicker.BackColor, this.m_dayPicker.ForeColor);
            if (this.m_dayPicker.Visible)
            {
                this.OnDropDown(EventArgs.Empty);
            }
            if (!this.m_dayPicker.Visible)
            {
                this.OnCloseUp(EventArgs.Empty);
            }
        }

        private void ShowTimePicker()
        {
            if (this.m_bTimePickerDirty)
            {
                this.FillTimePicker();
            }
            this.m_bTimePickerDirty = false;
            this.m_timePicker.Width = base.Size.Width;
            if (this.m_timePicker.Parent == null)
            {
                base.TopLevelControl.Controls.Add(this.m_timePicker);
            }
            Point p = new Point(base.Left, base.Bottom + 1);
            Point point2 = base.Parent.PointToScreen(base.Parent.Location);
            Point point3 = base.TopLevelControl.PointToScreen(base.Parent.Location);
            p.Offset(point2.X - point3.X, point2.Y - point3.Y);
            if ((p.Y + this.m_timePicker.Size.Height) > base.TopLevelControl.ClientRectangle.Height)
            {
                p.Y -= (base.ClientSize.Height + this.m_timePicker.Size.Height) + 2;
                if (p.Y < 0)
                {
                    p.Y = base.TopLevelControl.ClientRectangle.Height - this.m_timePicker.Size.Height;
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
            if ((p.X + this.m_timePicker.Size.Width) > base.TopLevelControl.ClientRectangle.Width)
            {
                p.X = base.TopLevelControl.ClientRectangle.Width - this.m_timePicker.Size.Width;
            }
            this.m_timePicker.Location = p;
            DateTime time = Convert.ToDateTime(this.Text);
            foreach (string str in this.m_timePicker.Items)
            {
                if (Convert.ToDateTime(str) >= time)
                {
                    int index = this.m_timePicker.Items.IndexOf(str);
                    this.m_timePicker.SelectedIndex = index;
                    this.m_timePicker.EnsureVisible(index);
                    break;
                }
            }
            this.m_timePicker.Show();
            this.m_timePicker.BringToFront();
            this.m_timePicker.Focus();
            this.OnDropDown(EventArgs.Empty);
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

        private void UpdateTime()
        {
            DateTime time = this.Value;
            try
            {
                if (this.m_timePicker.SelectedIndex >= 0)
                {
                    DateTime time2 = Convert.ToDateTime(this.m_timePicker.Items[this.m_timePicker.SelectedIndex]);
                    time = new DateTime(time.Year, time.Month, time.Day, time2.Hour, time2.Minute, time2.Second);
                }
                this.Value = time;
            }
            catch (Exception)
            {
            }
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

        public DateTime[] AnnuallyBoldedDates
        {
            get
            {
                return this.m_dayPicker.AnnuallyBoldedDates;
            }
            set
            {
                this.m_dayPicker.AnnuallyBoldedDates = value;
                this.m_dayPicker.UpdateBoldedDates();
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

        public DateTime[] BoldedDates
        {
            get
            {
                return this.m_dayPicker.BoldedDates;
            }
            set
            {
                this.m_dayPicker.BoldedDates = value;
                this.m_dayPicker.UpdateBoldedDates();
            }
        }

        public System.Drawing.Font CalendarFont
        {
            get
            {
                return this.m_dayPicker.Font;
            }
            set
            {
                this.m_dayPicker.Font = value;
            }
        }

        public Color CalendarForeColor
        {
            get
            {
                return this.m_dayPicker.ForeColor;
            }
            set
            {
                this.m_dayPicker.ForeColor = value;
            }
        }

        public Color CalendarMonthBackground
        {
            get
            {
                return this.m_dayPicker.BackColor;
            }
            set
            {
                this.m_dayPicker.BackColor = value;
            }
        }

        public bool CalendarShowToday
        {
            get
            {
                return this.m_dayPicker.ShowToday;
            }
            set
            {
                this.m_dayPicker.ShowToday = value;
            }
        }

        public bool CalendarShowTodayBorder
        {
            get
            {
                return this.m_dayPicker.ShowTodayBorder;
            }
            set
            {
                this.m_dayPicker.ShowTodayBorder = value;
            }
        }

        public Size CalendarSize
        {
            get
            {
                return this.m_dayPicker.Size;
            }
            set
            {
                this.m_dayPicker.Size = value;
            }
        }

        public Color CalendarTitleBackColor
        {
            get
            {
                return this.m_dayPicker.TitleBackColor;
            }
            set
            {
                this.m_dayPicker.TitleBackColor = value;
            }
        }

        public Color CalendarTitleForeColor
        {
            get
            {
                return this.m_dayPicker.TitleForeColor;
            }
            set
            {
                this.m_dayPicker.TitleForeColor = value;
            }
        }

        public bool CalendarTitleVistaStyle
        {
            get
            {
                return this.m_dayPicker.TitleVistaStyle;
            }
            set
            {
                this.m_dayPicker.TitleVistaStyle = value;
            }
        }

        public Color CalendarTrailingForeColor
        {
            get
            {
                return this.m_dayPicker.TrailingForeColor;
            }
            set
            {
                this.m_dayPicker.TrailingForeColor = value;
            }
        }

        public System.Globalization.CalendarWeekRule CalendarWeekRule
        {
            get
            {
                return this.m_dayPicker.CalendarWeekRule;
            }
            set
            {
                this.m_dayPicker.CalendarWeekRule = value;
            }
        }

        public DayCellCollection CustomBoldedDates
        {
            get
            {
                return this.m_dayPicker.CustomBoldedDates;
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
                    if (this.m_format == Resco.Controls.OutlookControls.DateTimePickerFormat.CustomTime)
                    {
                        this.m_bTimePickerDirty = true;
                    }
                    base.Invalidate();
                }
            }
        }

        public string DaysOfWeek
        {
            get
            {
                return this.m_dayPicker.DaysOfWeek;
            }
            set
            {
                this.m_dayPicker.DaysOfWeek = value;
                base.Invalidate();
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

        public int DropDownHeight
        {
            get
            {
                return this.m_timePicker.Height;
            }
            set
            {
                this.m_timePicker.Height = value * this.m_scaleFactor;
                if (this.m_timePicker.Visible)
                {
                    this.m_timePicker.Refresh();
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

        public Resco.Controls.OutlookControls.Day FirstDayOfWeek
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
                if ((value == Resco.Controls.OutlookControls.DateTimePickerFormat.CustomTime) || (value == Resco.Controls.OutlookControls.DateTimePickerFormat.Time))
                {
                    this.m_bTimePickerDirty = true;
                }
                base.Invalidate();
            }
        }

        public Color HighlightColor
        {
            get
            {
                return this.m_highlightColor;
            }
            set
            {
                if (value != this.m_highlightColor)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.Window;
                    }
                    this.m_highlightColor = value;
                    base.Invalidate();
                }
            }
        }

        public bool IsNone
        {
            get
            {
                return this.m_dayPicker.IsNone;
            }
            set
            {
                this.m_dayPicker.IsNone = value;
            }
        }

        public static Keys KeyNextMonth
        {
            get
            {
                return OutlookMonthCalendar.KeyNextMonth;
            }
            set
            {
                OutlookMonthCalendar.KeyNextMonth = value;
            }
        }

        public static Keys KeyNoneDate
        {
            get
            {
                return OutlookMonthCalendar.KeyNoneDate;
            }
            set
            {
                OutlookMonthCalendar.KeyNoneDate = value;
            }
        }

        public static Keys KeyPreviousMonth
        {
            get
            {
                return OutlookMonthCalendar.KeyPreviousMonth;
            }
            set
            {
                OutlookMonthCalendar.KeyPreviousMonth = value;
            }
        }

        public static Keys KeyTodayDate
        {
            get
            {
                return OutlookMonthCalendar.KeyTodayDate;
            }
            set
            {
                OutlookMonthCalendar.KeyTodayDate = value;
            }
        }

        public static Keys KeyYearSelection
        {
            get
            {
                return OutlookMonthCalendar.KeyYearSelection;
            }
            set
            {
                OutlookMonthCalendar.KeyYearSelection = value;
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
                }
            }
        }

        public DateTime[] MonthlyBoldedDates
        {
            get
            {
                return this.m_dayPicker.MonthlyBoldedDates;
            }
            set
            {
                this.m_dayPicker.MonthlyBoldedDates = value;
                this.m_dayPicker.UpdateBoldedDates();
            }
        }

        public NavigatorInterval NavigatorJumpTime
        {
            get
            {
                return this.m_navJumpTime;
            }
            set
            {
                this.m_navJumpTime = value;
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

        public Color SaturdayColor
        {
            get
            {
                return this.m_saturdayColor;
            }
            set
            {
                if (value != this.m_saturdayColor)
                {
                    if (value.IsEmpty)
                    {
                        value = Color.Blue;
                    }
                    this.m_saturdayColor = value;
                    base.Invalidate();
                }
            }
        }

        public Color SelectedBackColor
        {
            get
            {
                return this.m_selectedBackColor;
            }
            set
            {
                if (value != this.m_selectedBackColor)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.ActiveCaption;
                    }
                    this.m_selectedBackColor = value;
                    base.Invalidate();
                }
            }
        }

        public Color SelectedCellBackColor
        {
            get
            {
                return this.m_dayPicker.SelectedCellBackColor;
            }
            set
            {
                this.m_dayPicker.SelectedCellBackColor = value;
            }
        }

        public Color SelectedCellBorderColor
        {
            get
            {
                return this.m_dayPicker.SelectedCellBorderColor;
            }
            set
            {
                this.m_dayPicker.SelectedCellBorderColor = value;
            }
        }

        public Color SelectedCellForeColor
        {
            get
            {
                return this.m_dayPicker.SelectedCellForeColor;
            }
            set
            {
                this.m_dayPicker.SelectedCellForeColor = value;
            }
        }

        public Color SelectedForeColor
        {
            get
            {
                return this.m_selectedForeColor;
            }
            set
            {
                if (value != this.m_selectedForeColor)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.ActiveCaptionText;
                    }
                    this.m_selectedForeColor = value;
                    base.Invalidate();
                }
            }
        }

        public bool ShowDate
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

        public bool ShowDays
        {
            get
            {
                return this.m_showDays;
            }
            set
            {
                if (value != this.m_showDays)
                {
                    this.m_showDays = value;
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

        public bool ShowTodayButton
        {
            get
            {
                return this.m_showToday;
            }
            set
            {
                if (value != this.m_showToday)
                {
                    this.m_showToday = value;
                    base.Invalidate();
                }
            }
        }

        public bool ShowUpDown
        {
            get
            {
                return this.m_showUpDown;
            }
            set
            {
                if (value != this.m_showUpDown)
                {
                    this.m_showUpDown = value;
                    base.Invalidate();
                }
            }
        }

        public bool ShowWeekNavigation
        {
            get
            {
                return this.m_showWeekNav;
            }
            set
            {
                if (value != this.m_showWeekNav)
                {
                    this.m_showWeekNav = value;
                    base.Invalidate();
                }
            }
        }

        public bool ShowWeekNumbers
        {
            get
            {
                return this.m_dayPicker.ShowWeekNumbers;
            }
            set
            {
                this.m_dayPicker.ShowWeekNumbers = value;
            }
        }

        public OutlookDateTimePickerStyle Style
        {
            get
            {
                return this.m_style;
            }
            set
            {
                if (this.m_style != value)
                {
                    this.m_style = value;
                    base.Invalidate();
                }
            }
        }

        public Color SundayColor
        {
            get
            {
                return this.m_sundayColor;
            }
            set
            {
                if (value != this.m_sundayColor)
                {
                    if (value.IsEmpty)
                    {
                        value = Color.Red;
                    }
                    this.m_sundayColor = value;
                    base.Invalidate();
                }
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

        public string TimePickerEndTime
        {
            get
            {
                return this.m_endTime.ToString("HH:mm");
            }
            set
            {
                DateTime time = Convert.ToDateTime(value);
                this.m_endTime = new DateTime(1, 1, 1, time.Hour, time.Minute, 0);
                this.m_bTimePickerDirty = true;
            }
        }

        public int TimePickerMinuteInterval
        {
            get
            {
                return this.m_minuteInterval;
            }
            set
            {
                this.m_minuteInterval = value;
                this.m_bTimePickerDirty = true;
            }
        }

        public string TimePickerStartTime
        {
            get
            {
                return this.m_startTime.ToString("HH:mm");
            }
            set
            {
                DateTime time = Convert.ToDateTime(value);
                this.m_startTime = new DateTime(1, 1, 1, time.Hour, time.Minute, 0);
                this.m_bTimePickerDirty = true;
            }
        }

        public DateTime TodayDate
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
                return this.m_dayPicker.Value;
            }
            set
            {
                this.m_dayPicker.Value = value;
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
            public static double PctToDays = 0.33;
            public static double PctToToday = 0.44;
            public static double PctToWeekNav = 0.1;
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

