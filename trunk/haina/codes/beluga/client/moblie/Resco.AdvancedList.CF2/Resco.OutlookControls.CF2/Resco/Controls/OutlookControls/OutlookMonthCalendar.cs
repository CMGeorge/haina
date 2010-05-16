namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class OutlookMonthCalendar : UserControl
    {
        public static string AdditionalTextNoneDate = "";
        public static string AdditionalTextTodayDate = "";
        private const uint EM_GETINPUTMODE = 0xdd;
        private const uint EM_SETINPUTMODE = 0xde;
        private const int GW_CHILD = 5;
        private const int GWL_WNDPROC = -4;
        public static Keys KeyNextMonth = Keys.NumPad9;
        public static Keys KeyNoneDate = Keys.D2;
        public static Keys KeyPreviousMonth = Keys.NumPad8;
        public static Keys KeyTodayDate = Keys.D5;
        public static Keys KeyYearSelection = Keys.D0;
        private bool m_activateDateClickedEvent;
        private Bitmap m_bmp;
        private SolidBrush m_brush = new SolidBrush(Color.Black);
        private SolidBrush m_brushBack;
        private SolidBrush m_brushCaptionBack;
        private SolidBrush m_brushCaptionText;
        private SolidBrush m_brushCur;
        private SolidBrush m_brushFrame;
        private SolidBrush m_brushOther;
        private SolidBrush m_brushOtherBack;
        private SolidBrush m_brushSelBack;
        private SolidBrush m_brushSelText;
        private SolidBrush m_brushWeekendSaturdayBack;
        private SolidBrush m_brushWeekendSundayBack;
        private SolidBrush m_brushWeekLettersBack;
        private SolidBrush m_brushWeekLettersSaturday;
        private SolidBrush m_brushWeekLettersSunday;
        private SolidBrush m_brushWeekLettersText;
        private bool m_bTodayChanged;
        private Size m_calendarDimensions = new Size(1, 1);
        private System.Globalization.CalendarWeekRule m_calendarWeekRule = DateTimeFormatInfo.CurrentInfo.CalendarWeekRule;
        private bool m_captureMouse;
        private System.Windows.Forms.ContextMenu m_ContextMenu;
        private int m_curMonth = -1;
        private DateTime m_curSel = DateTime.Today;
        private int m_curYear = -1;
        private DayCellCollection m_customBoldedDates = new DayCellCollection();
        private DayCellEventArgs m_dayCellArgs = new DayCellEventArgs();
        private List<DateTime[]> m_days = new List<DateTime[]>();
        private string m_DaysOfWeek = "";
        private Alignment m_DayTextAlignment;
        private DayOfWeek m_defaultFirstDayOfWeek = DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;
        private int m_defaultMonthCount = DateTimeFormatInfo.CurrentInfo.MonthNames.Length;
        private WindowProcCallback m_delegate;
        internal bool m_doCloseUp;
        private bool m_EnableMonthMenu = true;
        private bool m_EnableYearUpDown = true;
        private DateTime m_firstDate;
        private Resco.Controls.OutlookControls.Day m_firstDayOfWeek = Resco.Controls.OutlookControls.Day.Default;
        private System.Drawing.Font m_fontCaption;
        private System.Drawing.Font m_fontWeekLetters;
        private GradientColor m_gradientBackColor;
        private Bitmap m_gradientBmp;
        private Graphics m_gradientGraphics;
        private Graphics m_graphics;
        private DateTime m_hoverSel = DateTime.Today;
        private ImageAttributes m_ia = new ImageAttributes();
        private bool m_isNone;
        private CalendarLayout m_layout = new CalendarLayout();
        private DateTime m_MaxDate = MaxDateTime;
        private DateTime m_MinDate = MinDateTime;
        private int m_monthCount = 1;
        private System.Windows.Forms.ContextMenu m_monthMenu;
        private string m_noneText = "None";
        private Pen m_pen = new Pen(Color.Black);
        private Pen m_penBack;
        private Pen m_penDaysGrid;
        private Pen m_penFrame;
        private Pen m_penHoverBox;
        private Pen m_penMonthsGrid;
        private ArrayList m_rcMonths = new ArrayList();
        private ArrayList m_rcYears = new ArrayList();
        private bool m_redraw;
        private SizeF m_scaleFactor = new SizeF(1f, 1f);
        private int m_scrollChange;
        private int m_selectedMonthIndex;
        private bool m_showTodayBorder = true;
        private bool m_titleVistaStyle;
        private DateTime m_today = DateTime.Today;
        private string m_todayText = "Today:";
        private ToolTipInternal m_tooltip = new ToolTipInternal();
        private TooltipEventArgs m_tooltipArgs = new TooltipEventArgs();
        private int m_tooltipDelay = 0x3e8;
        private bool m_tooltiping;
        private int m_tooltipingDayIndex;
        private int m_tooltipingMonthIndex;
        private Timer m_tooltipTimer = new Timer();
        private TouchNavigatorTool m_TouchNavigatorTool;
        private bool m_useGradient;
        private IntPtr m_wndproc = IntPtr.Zero;
        private IntPtr m_wndprocReal = IntPtr.Zero;
        private Control m_yearUpDown;
        private bool m_yearUpDownFocused;
        private int m_yearUpDownIndex;
        private bool m_yearUpDownNumericUpDown = true;
        public static readonly DateTime MaxDateTime = DateTime.MaxValue;
        public static readonly DateTime MinDateTime = DateTime.MinValue;
        private const int VK_ESCAPE = 0x1b;
        private const int WM_DESTROY = 2;
        private const int WM_HOTKEY = 0x312;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;

        public event EventHandler CloseUp;

        public event CustomizeBoldedDayCellHandler CustomizeBoldedDayCell;

        public event CustomizeDayCellHandler CustomizeDayCell;

        public event CustomizeTooltipHandler CustomizeTooltip;

        public event DateClickedHandler DateClicked;

        public event MonthChangeAfterEventHandler MonthChangeAfter;

        public event MonthChangeBeforeEventHandler MonthChangeBefore;

        public event EventHandler NoneSelected;

        public event EventHandler TodayClicked;

        public event EventHandler ValueChanged;

        static OutlookMonthCalendar()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(OutlookMonthCalendar), "");
            //}
        }

        public OutlookMonthCalendar()
        {
            using (Graphics graphics = base.CreateGraphics())
            {
                this.m_scaleFactor.Width = graphics.DpiX / 96f;
                this.m_scaleFactor.Height = graphics.DpiY / 96f;
            }
            this.m_tooltipTimer.Enabled = false;
            this.m_tooltipTimer.Interval = this.m_tooltipDelay;
            this.m_tooltipTimer.Tick += new EventHandler(this.m_tooltipTimer_Tick);
            this.CalculateDays(this.m_monthCount);
            for (int i = 0; i < this.m_monthCount; i++)
            {
                this.m_rcMonths.Add(Rectangle.Empty);
                this.m_rcYears.Add(Rectangle.Empty);
            }
            this.CreateGdiObjects();
            this.InitMonthContextMenu();
            this.InitYearUpDown();
            base.Visible = true;
            base.Location = new Point(0, 0);
            base.Size = new Size(0xa4, 0x98);
            using (Graphics graphics2 = base.CreateGraphics())
            {
                this.m_layout.FontChange(graphics2, this.Font);
            }
            this.m_gradientBackColor = new GradientColor();
            this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
            this.m_useGradient = false;
            this.m_titleVistaStyle = false;
            this.m_TouchNavigatorTool = new TouchNavigatorTool(this);
            this.m_TouchNavigatorTool.GestureDetected += new TouchNavigatorTool.GestureDetectedHandler(this.TouchNavigatorTool_GestureDetected);
        }

        public void AddAnnuallyBoldedDate(DateTime date)
        {
            Resco.Controls.OutlookControls.DayCell cell = new Resco.Controls.OutlookControls.DayCell(date, BoldedDateType.Annually);
            this.m_customBoldedDates.Add(cell);
        }

        public void AddBoldedDate(DateTime date)
        {
            Resco.Controls.OutlookControls.DayCell cell = new Resco.Controls.OutlookControls.DayCell(date, BoldedDateType.Nonrecurrent);
            this.m_customBoldedDates.Add(cell);
        }

        public void AddMonthlyBoldedDate(DateTime date)
        {
            Resco.Controls.OutlookControls.DayCell cell = new Resco.Controls.OutlookControls.DayCell(date, BoldedDateType.Monthly);
            this.m_customBoldedDates.Add(cell);
        }

        private Point Align(Alignment align, int destX, int destY, int destWidth, int destHeight, int contentWidth, int contentHeight, int paddingX, int paddingY)
        {
            int x = 0;
            int y = 0;
            if (((align == Alignment.TopLeft) || (align == Alignment.TopCenter)) || (align == Alignment.TopRight))
            {
                y = destY + paddingY;
            }
            if (((align == Alignment.MiddleLeft) || (align == Alignment.MiddleCenter)) || (align == Alignment.MiddleRight))
            {
                y = destY + ((destHeight - contentHeight) / 2);
            }
            if (((align == Alignment.BottomLeft) || (align == Alignment.BottomCenter)) || (align == Alignment.BottomRight))
            {
                y = (destY + (destHeight - contentHeight)) - paddingY;
            }
            if (((align == Alignment.TopLeft) || (align == Alignment.MiddleLeft)) || (align == Alignment.BottomLeft))
            {
                x = destX + paddingX;
            }
            if (((align == Alignment.TopCenter) || (align == Alignment.MiddleCenter)) || (align == Alignment.BottomCenter))
            {
                x = destX + ((destWidth - contentWidth) / 2);
            }
            if (((align == Alignment.TopRight) || (align == Alignment.MiddleRight)) || (align == Alignment.BottomRight))
            {
                x = (destX + (destWidth - contentWidth)) - paddingX;
            }
            return new Point(x, y);
        }

        private void CalculateDayAndMonthIndexes(int x, int y, out int monthIndex, out int dayIndex)
        {
            for (int i = 0; i < this.m_monthCount; i++)
            {
                if (this.m_layout.GetMonthLayout(i).InnerBounds.Contains(x, y))
                {
                    monthIndex = i;
                    for (int j = 0; j < 0x2a; j++)
                    {
                        if (this.m_layout.GetMonthLayout(i).GetCellBounds(j).Contains(x, y))
                        {
                            dayIndex = j;
                            return;
                        }
                    }
                    dayIndex = -1;
                    return;
                }
            }
            monthIndex = -1;
            dayIndex = -1;
        }

        private void CalculateDays(int monthCount)
        {
            this.CalculateFirstDate();
            this.m_days.Clear();
            for (int i = 0; i < monthCount; i++)
            {
                DateTime[] timeArray = new DateTime[0x2a];
                DateTime time = this.GetFirstDate(i).AddMonths(1);
                time = new DateTime(time.Year, time.Month, 1);
                DateTime time2 = time.AddMonths(1);
                for (int j = 0; j < 0x2a; j++)
                {
                    DateTime time3 = this.GetFirstDate(i).AddDays((double) j);
                    if ((((time3 >= time) && (time3 < time2)) || ((i == 0) && (time3 < time))) || ((i == (monthCount - 1)) && (time3 >= time2)))
                    {
                        timeArray[j] = time3;
                    }
                    else
                    {
                        timeArray[j] = DateTime.MinValue;
                    }
                }
                this.m_days.Add(timeArray);
            }
        }

        private void CalculateFirstDate()
        {
            this.m_firstDate = this.CalculateFirstDate(0);
        }

        private DateTime CalculateFirstDate(int monthIndex)
        {
            int year = (this.m_curYear < 1) ? this.m_curSel.Year : this.m_curYear;
            int month = (this.m_curMonth < 1) ? this.m_curSel.Month : this.m_curMonth;
            DateTime time = new DateTime(year, month, 1).AddMonths(monthIndex);
            if (time.DayOfWeek == this.GetFirstDayOfWeek())
            {
                return time.AddDays(-7.0);
            }
            if (time.DayOfWeek == DayOfWeek.Sunday)
            {
                return time.AddDays((double) (-7 + this.GetFirstDayOfWeek()));
            }
            return time.AddDays((double)(-(int)time.DayOfWeek + (int)this.GetFirstDayOfWeek()));
        }

        [DllImport("coredll.dll")]
        private static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, int msg, int wParam, int lParam);
        private void Close()
        {
            this.m_doCloseUp = false;
            base.Hide();
            if (this.CloseUp != null)
            {
                this.CloseUp(this, EventArgs.Empty);
            }
        }

        private void CreateGdiObjects()
        {
            this.Font = new System.Drawing.Font("Tahoma", 8f, FontStyle.Regular);
            this.m_brushCur = new SolidBrush(SystemColors.WindowText);
            this.m_brushOther = new SolidBrush(SystemColors.GrayText);
            this.m_brushOtherBack = new SolidBrush(Color.Transparent);
            this.m_brushSelBack = new SolidBrush(SystemColors.Highlight);
            this.m_brushSelText = new SolidBrush(SystemColors.HighlightText);
            this.m_penHoverBox = new Pen(SystemColors.GrayText);
            this.m_penDaysGrid = new Pen(SystemColors.WindowFrame);
            this.m_penMonthsGrid = new Pen(SystemColors.WindowFrame);
            this.m_brushCaptionBack = new SolidBrush(SystemColors.ActiveCaption);
            this.m_brushCaptionText = new SolidBrush(SystemColors.ActiveCaptionText);
            this.m_fontCaption = new System.Drawing.Font("Tahoma", 8f, FontStyle.Bold);
            this.m_fontWeekLetters = new System.Drawing.Font("Tahoma", 8f, FontStyle.Bold);
            this.m_brushWeekLettersText = new SolidBrush(SystemColors.ActiveCaption);
            this.m_brushWeekLettersBack = new SolidBrush(SystemColors.Window);
            this.m_brushWeekLettersSaturday = new SolidBrush(Color.Transparent);
            this.m_brushWeekLettersSunday = new SolidBrush(Color.Transparent);
            this.m_brushWeekendSaturdayBack = new SolidBrush(Color.Transparent);
            this.m_brushWeekendSundayBack = new SolidBrush(Color.Transparent);
            this.m_brushBack = new SolidBrush(SystemColors.Window);
            this.m_penBack = new Pen(SystemColors.Window);
            this.m_brushFrame = new SolidBrush(SystemColors.WindowFrame);
            this.m_penFrame = new Pen(SystemColors.WindowFrame);
        }

        private void CreateMemoryBitmap()
        {
            if (((this.m_bmp == null) || (this.m_bmp.Width != base.Width)) || (this.m_bmp.Height != base.Height))
            {
                this.m_bmp = new Bitmap(base.Width, base.Height);
                this.m_graphics = Graphics.FromImage(this.m_bmp);
                this.m_gradientBmp = new Bitmap(base.Width, base.Height);
                this.m_gradientGraphics = Graphics.FromImage(this.m_gradientBmp);
                this.m_layout.FontChange(this.m_graphics, base.Font);
            }
        }

        internal void Display(bool visible, int x, int y, Color backColor, Color foreColor)
        {
            if (visible)
            {
                this.m_captureMouse = false;
                this.m_doCloseUp = true;
                this.YearUpDownFocused = false;
                this.BackColor = backColor;
                this.ForeColor = foreColor;
                base.Left = x;
                base.Top = y;
                base.BringToFront();
                base.Focus();
                this.m_hoverSel = this.m_curSel;
            }
            base.Visible = visible;
        }

        private void DisplayMonthMenu(int x, int y, int selectedMonthIndex)
        {
            try
            {
                this.m_selectedMonthIndex = selectedMonthIndex;
                this.m_monthMenu.Show(this, new Point(x, y));
            }
            catch (NotSupportedException)
            {
            }
        }

        public void DisplayYearUpDown()
        {
            int monthIndex = this.GetMonthIndex(this.m_curSel);
            this.DisplayYearUpDown((Rectangle) this.m_rcYears[monthIndex], this.m_curSel.Year, monthIndex);
        }

        private void DisplayYearUpDown(Rectangle rect, int year, int mIndex)
        {
            if (!base.Controls.Contains(this.m_yearUpDown) && ((this.Site == null) || !this.Site.DesignMode))
            {
                base.Controls.Add(this.m_yearUpDown);
            }
            if (this.m_yearUpDown.Visible)
            {
                this.YearUpDownFocused = false;
            }
            this.m_yearUpDownIndex = mIndex;
            this.m_yearUpDown.Text = year.ToString();
            if (!this.m_yearUpDownNumericUpDown)
            {
                SetInputMode(this.m_yearUpDown, InputMode.Numbers);
                ((TextBox) this.m_yearUpDown).SelectionStart = 0;
                ((TextBox) this.m_yearUpDown).SelectionLength = this.m_yearUpDown.Text.Length;
            }
            this.m_yearUpDown.Left = rect.Left - ((int) (3f * this.m_scaleFactor.Width));
            this.m_yearUpDown.Top = rect.Top - ((int) (3f * this.m_scaleFactor.Height));
            this.m_yearUpDown.Width = rect.Width + ((int) (40f * this.m_scaleFactor.Width));
            this.m_yearUpDown.Height = rect.Height + ((int) (6f * this.m_scaleFactor.Height));
            this.YearUpDownFocused = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_bmp != null)
                {
                    this.m_bmp.Dispose();
                    this.m_bmp = null;
                }
                if (this.m_brush != null)
                {
                    this.m_brush.Dispose();
                    this.m_brush = null;
                }
                if (this.m_brushBack != null)
                {
                    this.m_brushBack.Dispose();
                    this.m_brushBack = null;
                }
                if (this.m_brushCaptionBack != null)
                {
                    this.m_brushCaptionBack.Dispose();
                    this.m_brushCaptionBack = null;
                }
                if (this.m_brushCaptionText != null)
                {
                    this.m_brushCaptionText.Dispose();
                    this.m_brushCaptionText = null;
                }
                if (this.m_brushCur != null)
                {
                    this.m_brushCur.Dispose();
                    this.m_brushCur = null;
                }
                if (this.m_brushFrame != null)
                {
                    this.m_brushFrame.Dispose();
                    this.m_brushFrame = null;
                }
                if (this.m_brushOther != null)
                {
                    this.m_brushOther.Dispose();
                    this.m_brushOther = null;
                }
                if (this.m_brushOtherBack != null)
                {
                    this.m_brushOtherBack.Dispose();
                    this.m_brushOtherBack = null;
                }
                if (this.m_brushSelBack != null)
                {
                    this.m_brushSelBack.Dispose();
                    this.m_brushSelBack = null;
                }
                if (this.m_brushSelText != null)
                {
                    this.m_brushSelText.Dispose();
                    this.m_brushSelText = null;
                }
                if (this.m_brushWeekendSaturdayBack != null)
                {
                    this.m_brushWeekendSaturdayBack.Dispose();
                    this.m_brushWeekendSaturdayBack = null;
                }
                if (this.m_brushWeekendSundayBack != null)
                {
                    this.m_brushWeekendSundayBack.Dispose();
                    this.m_brushWeekendSundayBack = null;
                }
                if (this.m_brushWeekLettersBack != null)
                {
                    this.m_brushWeekLettersBack.Dispose();
                    this.m_brushWeekLettersBack = null;
                }
                if (this.m_brushWeekLettersSaturday != null)
                {
                    this.m_brushWeekLettersSaturday.Dispose();
                    this.m_brushWeekLettersSaturday = null;
                }
                if (this.m_brushWeekLettersSunday != null)
                {
                    this.m_brushWeekLettersSunday.Dispose();
                    this.m_brushWeekLettersSunday = null;
                }
                if (this.m_brushWeekLettersText != null)
                {
                    this.m_brushWeekLettersText.Dispose();
                    this.m_brushWeekLettersText = null;
                }
                if (this.m_fontCaption != null)
                {
                    this.m_fontCaption.Dispose();
                    this.m_fontCaption = null;
                }
                if (this.m_fontWeekLetters != null)
                {
                    this.m_fontWeekLetters.Dispose();
                    this.m_fontWeekLetters = null;
                }
                if (this.m_gradientBmp != null)
                {
                    this.m_gradientBmp.Dispose();
                    this.m_gradientBmp = null;
                }
                if (this.m_gradientGraphics != null)
                {
                    this.m_gradientGraphics.Dispose();
                    this.m_gradientGraphics = null;
                }
                if (this.m_graphics != null)
                {
                    this.m_graphics.Dispose();
                    this.m_graphics = null;
                }
                if (this.m_pen != null)
                {
                    this.m_pen.Dispose();
                    this.m_pen = null;
                }
                if (this.m_penBack != null)
                {
                    this.m_penBack.Dispose();
                    this.m_penBack = null;
                }
                if (this.m_penDaysGrid != null)
                {
                    this.m_penDaysGrid.Dispose();
                    this.m_penDaysGrid = null;
                }
                if (this.m_penFrame != null)
                {
                    this.m_penFrame.Dispose();
                    this.m_penFrame = null;
                }
                if (this.m_penHoverBox != null)
                {
                    this.m_penHoverBox.Dispose();
                    this.m_penHoverBox = null;
                }
                if (this.m_penMonthsGrid != null)
                {
                    this.m_penMonthsGrid.Dispose();
                    this.m_penMonthsGrid = null;
                }
                if (this.m_tooltip != null)
                {
                    this.m_tooltip.Dispose();
                    this.m_tooltip = null;
                }
                if (this.m_tooltipTimer != null)
                {
                    this.m_tooltipTimer.Dispose();
                    this.m_tooltipTimer = null;
                }
                if (this.m_yearUpDown != null)
                {
                    this.m_yearUpDown.Dispose();
                    this.m_yearUpDown = null;
                }
            }
            base.Dispose(disposing);
        }

        private void DrawBorders(Graphics g, MonthLayout layout)
        {
            this.DrawLeftBorder(g, layout);
            this.DrawUpperBorder(g, layout);
        }

        private void DrawBottomLabels(Graphics g)
        {
            if (this.m_layout.ShowTodayBar)
            {
                g.DrawString(string.Format("{0} {1}", this.m_todayText, this.TodayDate.ToShortDateString()) + AdditionalTextTodayDate, this.m_fontCaption, this.m_brushCur, (float) this.m_layout.TodayBarPos.X, (float) this.m_layout.TodayBarPos.Y);
            }
            if (this.m_layout.ShowNoneBar)
            {
                SolidBrush brushCaptionBack;
                if (this.m_isNone)
                {
                    brushCaptionBack = this.m_brushCaptionBack;
                }
                else
                {
                    brushCaptionBack = this.m_brushCur;
                }
                g.DrawString(this.m_noneText + AdditionalTextNoneDate, this.m_fontCaption, brushCaptionBack, (float) this.m_layout.NoneBarPos.X, (float) this.m_layout.NoneBarPos.Y);
            }
        }

        private void DrawControlFrame(Graphics g)
        {
            g.DrawRectangle(this.m_penFrame, 0, 0, base.Width - 1, base.Height - 1);
        }

        private void DrawDay(Graphics g, DateTime day, bool selected, bool drawBackground, bool redrawGridPart, Rectangle rect, int monthIndex)
        {
            int dayIndex = this.GetDayIndex(day, monthIndex);
            this.DrawDay(g, day, selected, drawBackground, redrawGridPart, rect, monthIndex, dayIndex);
        }

        private void DrawDay(Graphics g, DateTime day, bool selected, bool drawBackground, bool redrawGridPart, Rectangle rect, int monthIndex, int dIndex)
        {
            int num = this.DaysGrid ? 1 : 0;
            if (day == DateTime.MinValue)
            {
                if (drawBackground)
                {
                    this.m_brush.Color = this.m_brushBack.Color;
                    g.FillRectangle(this.m_brush, (int) (rect.X + num), (int) (rect.Y + num), (int) (rect.Width - (2 * num)), (int) (rect.Height - (2 * num)));
                }
            }
            else
            {
                Resco.Controls.OutlookControls.DayCell[] boldedDates = this.m_customBoldedDates[day];
                bool flag = (boldedDates != null) && (boldedDates.Length > 0);
                Resco.Controls.OutlookControls.DayCell outputDate = null;
                if (flag)
                {
                    outputDate = boldedDates[0];
                    this.OnCustomizeBoldedDayCell(new BoldedDayCellEventArgs(day, boldedDates, outputDate));
                }
                this.m_dayCellArgs._day = day;
                this.m_dayCellArgs._selected = selected;
                this.m_dayCellArgs._inactive = day.Month != (1 + (((this.m_curMonth - 1) + monthIndex) % (this.m_defaultMonthCount - 1)));
                this.m_dayCellArgs.Font = flag ? ((outputDate.Font == null) ? this.m_fontCaption : outputDate.Font) : base.Font;
                DayOfWeek week = (DayOfWeek)((dIndex + (int)this.GetFirstDayOfWeek()) % 7);
                this.m_dayCellArgs.ForeColor = ((week == DayOfWeek.Saturday) && (this.m_brushWeekLettersSaturday.Color != Color.Transparent)) ? this.m_brushWeekLettersSaturday.Color : (((week == DayOfWeek.Sunday) && (this.m_brushWeekLettersSunday.Color != Color.Transparent)) ? this.m_brushWeekLettersSunday.Color : (this.m_dayCellArgs.Inactive ? this.m_brushOther.Color : (selected ? this.m_brushSelText.Color : this.m_brushCur.Color)));
                if (flag)
                {
                    this.m_dayCellArgs.ForeColor = selected ? ((outputDate.SelForeColor == Color.Transparent) ? this.m_dayCellArgs.ForeColor : outputDate.SelForeColor) : ((outputDate.ForeColor == Color.Transparent) ? this.m_dayCellArgs.ForeColor : outputDate.ForeColor);
                }
                this.m_dayCellArgs.BackColor = selected ? this.m_brushSelBack.Color : (((week == DayOfWeek.Saturday) && (this.m_brushWeekendSaturdayBack.Color != Color.Transparent)) ? this.m_brushWeekendSaturdayBack.Color : (((week == DayOfWeek.Sunday) && (this.m_brushWeekendSundayBack.Color != Color.Transparent)) ? this.m_brushWeekendSundayBack.Color : (this.m_dayCellArgs.Inactive ? ((this.m_brushOtherBack.Color != Color.Transparent) ? this.m_brushOtherBack.Color : this.m_brushBack.Color) : this.m_brushBack.Color)));
                if (flag)
                {
                    this.m_dayCellArgs.BackColor = selected ? ((outputDate.SelBackColor == Color.Transparent) ? this.m_dayCellArgs.BackColor : outputDate.SelBackColor) : ((outputDate.BackColor == Color.Transparent) ? this.m_dayCellArgs.BackColor : outputDate.BackColor);
                }
                this.m_dayCellArgs.Text = (flag && (outputDate.Text != null)) ? outputDate.Text : day.Day.ToString();
                this.m_dayCellArgs.TextAlignment = flag ? outputDate.TextAlignment : this.m_DayTextAlignment;
                this.m_dayCellArgs.Image = flag ? outputDate.Image : null;
                this.m_dayCellArgs.ImageAlignment = flag ? outputDate.ImageAlignment : Alignment.BottomRight;
                this.m_dayCellArgs.ImageTransparentColor = flag ? outputDate.ImageTransparentColor : Color.Empty;
                this.m_dayCellArgs.ImageAutoTransparent = flag ? outputDate.ImageAutoTransparent : false;
                this.m_dayCellArgs.ImageAutoResize = flag ? outputDate.ImageAutoResize : false;
                this.OnCustomizeDayCell(this.m_dayCellArgs);
                if ((this.m_dayCellArgs.BackColor != this.BackColor) || drawBackground)
                {
                    if (this.m_useGradient && (this.m_dayCellArgs.BackColor == this.BackColor))
                    {
                        Rectangle destRect = new Rectangle(rect.X + num, rect.Y + num, rect.Width - (2 * num), rect.Height - (2 * num));
                        g.DrawImage(this.m_gradientBmp, destRect, destRect, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        this.m_brush.Color = this.m_dayCellArgs.BackColor;
                        g.FillRectangle(this.m_brush, (int) (rect.X + num), (int) (rect.Y + num), (int) (rect.Width - (2 * num)), (int) (rect.Height - (2 * num)));
                    }
                }
                if (this.m_dayCellArgs.Image != null)
                {
                    this.DrawImage(g, this.m_dayCellArgs.Image, this.m_dayCellArgs.ImageAlignment, rect.X + num, rect.Y + num, rect.Width - (2 * num), rect.Height - (2 * num), this.m_dayCellArgs.ImageAutoResize, this.m_dayCellArgs.ImageTransparentColor, this.m_dayCellArgs.ImageAutoTransparent);
                }
                if ((this.m_dayCellArgs.Text != null) && (this.m_dayCellArgs.Text.Length > 0))
                {
                    this.m_brush.Color = this.m_dayCellArgs.ForeColor;
                    this.DrawString(g, this.m_dayCellArgs.Text, this.m_dayCellArgs.Font, this.m_brush, this.m_dayCellArgs.TextAlignment, rect.X + num, rect.Y + num, rect.Width - (2 * num), rect.Height - (2 * num));
                }
                if (redrawGridPart && this.DaysGrid)
                {
                    g.DrawRectangle(this.m_penDaysGrid, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                }
            }
        }

        private void DrawDays(Graphics g, MonthLayout layout, int monthIndex)
        {
            int num = 0x2a;
            for (int i = 0; i < num; i++)
            {
                DateTime day = this.GetDay(monthIndex, i);
                this.DrawDay(g, this.GetDay(monthIndex, i), (day.Date == this.m_curSel.Date) && !this.m_isNone, false, false, layout.GetCellBounds(i), monthIndex, i);
            }
        }

        private void DrawDaysGrid(Graphics g, MonthLayout layout)
        {
            if (this.DaysGrid)
            {
                int cellCountX = layout.CellCountX;
                int cellCountY = layout.CellCountY;
                int num3 = (int) Math.Ceiling(((double) cellCountX) / 2.0);
                int num4 = (int) Math.Ceiling(((double) cellCountY) / 2.0);
                for (int i = 0; i < num3; i++)
                {
                    g.DrawRectangle(this.m_penDaysGrid, layout.GetCellBounds(i * 2).X, layout.InnerBounds.Y, layout.GetCellBounds(i * 2).Width - 1, layout.InnerBounds.Height);
                }
                for (int j = 0; j < num4; j++)
                {
                    g.DrawRectangle(this.m_penDaysGrid, layout.InnerBounds.X, layout.GetCellBounds((j * 2) * 7).Y, layout.InnerBounds.Width, layout.GetCellBounds((j * 2) * 7).Height - 1);
                }
                g.DrawRectangle(this.m_penDaysGrid, layout.InnerBounds);
            }
        }

        private void DrawHoverSelection(Graphics g, DateTime date, bool draw)
        {
            this.DrawControlFrame(g);
            int monthIndex = this.GetMonthIndex(date);
            int dayIndex = this.GetDayIndex(date, monthIndex);
            if (this.IsInsideAnyGrid(monthIndex, dayIndex))
            {
                if (draw)
                {
                    Rectangle cellBounds = this.m_layout.GetMonthLayout(monthIndex).GetCellBounds(dayIndex);
                    g.DrawRectangle(this.m_penHoverBox, cellBounds.X, cellBounds.Y, cellBounds.Width - 1, cellBounds.Height - 1);
                }
                else
                {
                    this.DrawDay(g, date, (((date.Year == this.m_curSel.Year) && (date.Month == this.m_curSel.Month)) && (date.Day == this.m_curSel.Day)) && !this.m_isNone, true, true, this.m_layout.GetMonthLayout(monthIndex).GetCellBounds(dayIndex), monthIndex);
                }
            }
        }

        private void DrawImage(Graphics g, Image image, Alignment align, int x, int y, int width, int height, bool stretch, Color transparentColor, bool autoTransparent)
        {
            if ((transparentColor == Color.Empty) && !autoTransparent)
            {
                this.m_ia.ClearColorKey();
            }
            if (transparentColor != Color.Empty)
            {
                this.m_ia.SetColorKey(transparentColor, transparentColor);
            }
            if (autoTransparent)
            {
                Bitmap bitmap = new Bitmap(image);
                Color pixel = bitmap.GetPixel(0, 0);
                this.m_ia.SetColorKey(pixel, pixel);
                bitmap.Dispose();
                bitmap = null;
            }
            if (stretch)
            {
                g.DrawImage(image, new Rectangle(x, y, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, this.m_ia);
            }
            else
            {
                Point point = this.Align(align, x, y, width, height, image.Width, image.Height, 0, 0);
                g.DrawImage(image, new Rectangle(point.X, point.Y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, this.m_ia);
            }
        }

        private void DrawLeftBorder(Graphics g, MonthLayout layout)
        {
            if (this.m_layout.ShowWeekNumbers)
            {
                g.DrawLine(this.m_penFrame, layout.InnerBounds.Left, layout.InnerBounds.Top + 3, layout.InnerBounds.Left, layout.InnerBounds.Bottom - 3);
            }
        }

        private void DrawMonth(Graphics g, MonthLayout layout, int monthIndex)
        {
            this.DrawMonthCaption(g, layout.MonthCaptionBounds, monthIndex);
            this.DrawWeekLetters(g, layout);
            this.DrawWeekNumbers(g, layout, monthIndex);
            this.DrawDays(g, layout, monthIndex);
            this.DrawDaysGrid(g, layout);
            this.DrawBorders(g, layout);
            this.DrawMonthFrame(g, layout);
        }

        private void DrawMonthCaption(Graphics g, Rectangle bounds, int monthIndex)
        {
            if (this.m_layout.ShowMonthCaption)
            {
                if (!this.m_titleVistaStyle)
                {
                    g.FillRectangle(this.m_brushCaptionBack, bounds);
                }
                else
                {
                    GradientFill.DrawVistaGradient(g, this.m_brushCaptionBack.Color, bounds, FillDirection.Vertical);
                }
                DateTime time = this.m_firstDate.AddMonths(1 + monthIndex);
                string text = time.ToString("MMMM yyyy");
                Size size = g.MeasureString(text, this.m_fontCaption).ToSize();
                int x = bounds.X + ((bounds.Width - size.Width) / 2);
                int y = bounds.Y + ((bounds.Height - size.Height) / 2);
                g.DrawString(text, this.m_fontCaption, this.m_brushCaptionText, (float) x, (float) y);
                text = time.ToString("MMMM");
                Size size2 = g.MeasureString(text, this.m_fontCaption).ToSize();
                Rectangle rectangle = new Rectangle(x, y, size2.Width, size2.Height);
                this.m_rcMonths[monthIndex] = rectangle;
                text = time.ToString("yyyy");
                size2 = g.MeasureString(text, this.m_fontCaption).ToSize();
                Rectangle rectangle2 = new Rectangle((x + size.Width) - size2.Width, y, size2.Width, size2.Height);
                this.m_rcYears[monthIndex] = rectangle2;
                g.FillRectangle(this.m_brushBack, this.m_layout.LeftArrowBounds);
                g.DrawRectangle(this.m_penFrame, this.m_layout.LeftArrowBounds);
                g.FillPolygon(this.m_brushFrame, this.m_layout.LeftArrowTriangle);
                g.FillRectangle(this.m_brushBack, this.m_layout.RightArrowBounds);
                g.DrawRectangle(this.m_penFrame, this.m_layout.RightArrowBounds);
                g.FillPolygon(this.m_brushFrame, this.m_layout.RightArrowTriangle);
            }
        }

        private void DrawMonthFrame(Graphics g, MonthLayout layout)
        {
            if (this.MonthsGrid)
            {
                g.DrawRectangle(this.m_penMonthsGrid, layout.X, layout.Y, layout.Width - 1, layout.Height - 1);
            }
        }

        private void DrawMonths(Graphics g)
        {
            for (int i = 0; i < (this.m_layout.CellCountX * this.m_layout.CellCountY); i++)
            {
                this.DrawMonth(g, this.m_layout.GetMonthLayout(i), i);
            }
        }

        private void DrawString(Graphics g, string text, System.Drawing.Font font, SolidBrush brush, Alignment align, Rectangle rect)
        {
            this.DrawString(g, text, font, brush, align, rect.X, rect.Y, rect.Width, rect.Height);
        }

        private void DrawString(Graphics g, string text, System.Drawing.Font font, SolidBrush brush, Alignment align, int x, int y, int width, int height)
        {
            if (align == Alignment.TopLeft)
            {
                g.DrawString(text, font, brush, (float) (x + 2), (float) y);
            }
            else
            {
                SizeF ef = g.MeasureString(text, font);
                int contentWidth = (int) Math.Ceiling((double) ef.Width);
                int contentHeight = (int) Math.Ceiling((double) ef.Height);
                Point point = this.Align(align, x, y, width, height, contentWidth, contentHeight, 2, 0);
                g.DrawString(text, font, brush, (float) point.X, (float) point.Y);
            }
        }

        private void DrawTodaySelection(Graphics g)
        {
            if (this.m_showTodayBorder)
            {
                int monthIndex = this.GetMonthIndex(this.TodayDate);
                int dayIndex = this.GetDayIndex(this.TodayDate, monthIndex);
                this.GetNumberOfDays(monthIndex);
                if ((monthIndex >= 0) && (monthIndex < this.m_monthCount))
                {
                    Rectangle cellBounds = this.m_layout.GetMonthLayout(monthIndex).GetCellBounds(dayIndex);
                    g.DrawRectangle(this.m_penFrame, cellBounds.X, cellBounds.Y, cellBounds.Width - 1, cellBounds.Height - 1);
                    g.DrawRectangle(this.m_penFrame, (int) (cellBounds.X + 1), (int) (cellBounds.Y + 1), (int) (cellBounds.Width - 3), (int) (cellBounds.Height - 3));
                }
            }
        }

        private void DrawUpperBorder(Graphics g, MonthLayout layout)
        {
            if (this.m_layout.ShowWeekLetters)
            {
                g.DrawLine(this.m_penFrame, layout.InnerBounds.Left + 3, layout.InnerBounds.Top, layout.InnerBounds.Right - 3, layout.InnerBounds.Top);
            }
        }

        private void DrawWeekLetters(Graphics g, MonthLayout layout)
        {
            if (this.m_layout.ShowWeekLetters)
            {
                string weekDayNames = this.GetWeekDayNames();
                if (!this.m_useGradient)
                {
                    g.FillRectangle(this.m_brushWeekLettersBack, layout.MonthCaptionBounds.Left, layout.MonthCaptionBounds.Bottom, layout.Width, this.m_layout.WeekLettersHeight);
                }
                for (int i = 0; i < 7; i++)
                {
                    int length = weekDayNames.Length / 7;
                    string str2 = weekDayNames.Substring(i * length, length);
                    Size size = g.MeasureString(str2.ToString(), this.m_fontWeekLetters).ToSize();
                    Point point = new Point(layout.GetCellBounds(i).Left + ((layout.GetCellBounds(i).Width - size.Width) / 2), layout.MonthCaptionBounds.Bottom + ((this.m_layout.WeekLettersHeight - size.Height) / 2));
                    DayOfWeek week = (DayOfWeek)((i + (int)this.GetFirstDayOfWeek()) % 7);
                    g.DrawString(str2.ToString(), this.m_fontWeekLetters, (week == DayOfWeek.Saturday) ? ((this.m_brushWeekLettersSaturday.Color == Color.Transparent) ? this.m_brushWeekLettersText : this.m_brushWeekLettersSaturday) : ((week == DayOfWeek.Sunday) ? ((this.m_brushWeekLettersSunday.Color == Color.Transparent) ? this.m_brushWeekLettersText : this.m_brushWeekLettersSunday) : this.m_brushWeekLettersText), (float) point.X, (float) point.Y);
                }
            }
        }

        private void DrawWeekNumbers(Graphics g, MonthLayout layout, int monthIndex)
        {
            if (this.m_layout.ShowWeekNumbers)
            {
                if (!this.m_useGradient)
                {
                    g.FillRectangle(this.m_brushWeekLettersBack, layout.X, layout.InnerBounds.Top, this.m_layout.WeekNumbersWidth, layout.InnerBounds.Height);
                }
                int monthCount = this.m_monthCount;
                for (int i = 0; i < 6; i++)
                {
                    DateTime day = this.GetDay(monthIndex, (i * 7) + 6);
                    if (day == DateTime.MinValue)
                    {
                        day = this.GetDay(monthIndex, i * 7);
                        if (day == DateTime.MinValue)
                        {
                            goto Label_01EE;
                        }
                        day = day.AddDays(6.0);
                    }
                    int num3 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(day, this.CalendarWeekRule, this.GetFirstDayOfWeek());
                    bool flag = num3 < 10;
                    string text = (flag ? "0" : "") + num3.ToString();
                    Size size = g.MeasureString(text, base.Font).ToSize();
                    Point point = new Point(layout.X + ((this.m_layout.WeekNumbersWidth - size.Width) / 2), layout.GetCellBounds((i * 7) + 6).Top + ((layout.GetCellBounds((i * 7) + 6).Height - size.Height) / 2));
                    int num4 = (((this.m_curMonth - 1) + monthIndex) % 12) + 1;
                    int month = day.AddDays(-6.0).Month;
                    g.DrawString(text, base.Font, (((month < num4) && (monthIndex == 0)) || ((month > num4) && (monthIndex == (monthCount - 1)))) ? this.m_brushOther : this.m_brushCur, (float) point.X, (float) point.Y);
                    if (flag)
                    {
                        g.DrawString("0", base.Font, this.m_brushWeekLettersBack, (float) point.X, (float) point.Y);
                    }
                Label_01EE:;
                }
            }
        }

        public bool GetDateAt(int x, int y, out DateTime date)
        {
            int num;
            int num2;
            this.CalculateDayAndMonthIndexes(x, y, out num, out num2);
            bool flag = this.IsInsideAnyGrid(num, num2);
            if (flag)
            {
                date = this.GetDay(num, num2);
                return flag;
            }
            date = DateTime.MinValue;
            return flag;
        }

        private DateTime GetDay(int monthIndex, int dayIndex)
        {
            if (this.IsInsideAnyGrid(monthIndex, dayIndex))
            {
                return this.m_days[monthIndex][dayIndex];
            }
            return DateTime.MinValue;
        }

        private int GetDayIndex(DateTime date, int mIndex)
        {
            if ((mIndex >= 0) && (mIndex < this.m_monthCount))
            {
                return (int) date.Subtract(this.GetFirstDate(mIndex)).TotalDays;
            }
            return -1;
        }

        private DateTime GetFirstDate(int monthIndex)
        {
            if (monthIndex == 0)
            {
                return this.m_firstDate.AddMonths(monthIndex);
            }
            return this.CalculateFirstDate(monthIndex);
        }

        private DayOfWeek GetFirstDayOfWeek()
        {
            if (this.m_firstDayOfWeek == Resco.Controls.OutlookControls.Day.Default)
            {
                return this.m_defaultFirstDayOfWeek;
            }
            return (DayOfWeek) this.m_firstDayOfWeek;
        }

        private int GetIndex(int mIndex, int dIndex)
        {
            int num = 0;
            for (int i = 0; i < mIndex; i++)
            {
                num += this.m_days[i].Length;
            }
            return (num + dIndex);
        }

        private static int GetInputMode(Control ctrl)
        {
            return (int) SendMessage(ctrl.Handle, 0xdd, 0, 0);
        }

        [DllImport("coredll.dll")]
        private static extern int GetLastError();
        private int GetMonthIndex(DateTime date)
        {
            for (int i = 0; i < this.m_monthCount; i++)
            {
                DateTime time;
                int dayIndex = this.GetDayIndex(date, i);
                if ((((dayIndex >= 0) && (i < this.m_days.Count)) && ((dayIndex < this.m_days[i].Length) && ((time = this.GetDay(i, dayIndex)) != DateTime.MinValue))) && (date.Month == time.Month))
                {
                    return i;
                }
            }
            return -1;
        }

        private int GetMonthIndex(int mIndex)
        {
            return ((((this.m_curMonth - 1) + mIndex) % 12) + 1);
        }

        private int GetNumberOfDays(int mIndex)
        {
            if ((mIndex >= 0) && (mIndex < this.m_monthCount))
            {
                return this.m_days[mIndex].Length;
            }
            return 0;
        }

        private int GetScrollChange()
        {
            if (this.ScrollChange == 0)
            {
                return this.m_monthCount;
            }
            return this.ScrollChange;
        }

        private string GetWeekDayNames()
        {
            string daysOfWeek = "";
            if ((this.m_DaysOfWeek != null) && (this.m_DaysOfWeek != ""))
            {
                if (this.m_DaysOfWeek.Length < 7)
                {
                    this.m_DaysOfWeek = this.m_DaysOfWeek.PadRight(7, ' ');
                }
                daysOfWeek = this.m_DaysOfWeek;
            }
            else
            {
                int startIndex = 0;
                if (CultureInfo.CurrentCulture.Name.StartsWith("zh"))
                {
                    startIndex = 2;
                }
                for (int j = 0; j < 7; j++)
                {
                    daysOfWeek = daysOfWeek + DateTimeFormatInfo.CurrentInfo.ShortestDayNames[j].Substring(startIndex, 1);
                }
            }
            int num3 = daysOfWeek.Length / 7;
            string str2 = "";
            for (int i = 0; i < daysOfWeek.Length; i++)
            {
                str2 = str2 + daysOfWeek[(i + ((int)this.GetFirstDayOfWeek() * num3)) % daysOfWeek.Length];
            }
            return str2;
        }

        private void InitMonthContextMenu()
        {
            this.m_monthMenu = new System.Windows.Forms.ContextMenu();
            for (int i = 1; i <= 12; i++)
            {
                MenuItem item = new MenuItem();
                this.m_monthMenu.MenuItems.Add(item);
                item.Click += new EventHandler(this.OnMonthMenuClick);
                item.Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
            }
            this.m_monthMenu.Popup += new EventHandler(this.OnMonthMenuPopup);
        }

        private void InitYearUpDown()
        {
            try
            {
                this.m_yearUpDown = new NumericUpDown();
                this.m_yearUpDownNumericUpDown = true;
            }
            catch
            {
                this.m_yearUpDown = new TextBox();
                this.m_yearUpDownNumericUpDown = false;
            }
            this.m_yearUpDown.Visible = false;
            if (this.m_yearUpDownNumericUpDown)
            {
                ((NumericUpDown) this.m_yearUpDown).ValueChanged += new EventHandler(this.OnYearUpDownValueChanged);
            }
            this.m_yearUpDown.KeyUp += new KeyEventHandler(this.OnYearUpDownKeyUp);
            this.m_yearUpDown.LostFocus += new EventHandler(this.OnYearUpDownLostFocus);
            if (this.m_yearUpDownNumericUpDown)
            {
                ((NumericUpDown) this.m_yearUpDown).Minimum = DateTime.MinValue.Year;
                ((NumericUpDown) this.m_yearUpDown).Maximum = DateTime.MaxValue.Year;
            }
            this.m_yearUpDown.Visible = false;
        }

        private bool IsInsideAnyGrid(int monthIndex, int dayIndex)
        {
            return ((((monthIndex >= 0) && (monthIndex < this.m_monthCount)) && (dayIndex >= 0)) && (dayIndex < this.GetNumberOfDays(monthIndex)));
        }

        private void m_gradientBackColor_PropertyChanged(object sender, EventArgs e)
        {
            this.m_redraw = true;
            base.Invalidate();
        }

        private void m_tooltipTimer_Tick(object sender, EventArgs e)
        {
            this.ShowTooltip(this.m_tooltipingDayIndex, this.m_tooltipingMonthIndex);
            this.m_tooltipTimer.Enabled = false;
            this.m_tooltiping = true;
        }

        public void NextMonth()
        {
            this.UpdateCurSel(this.m_curSel.AddMonths(this.GetScrollChange()), true, this.GetMonthIndex(this.m_curSel));
        }

        protected void OnCustomizeBoldedDayCell(BoldedDayCellEventArgs e)
        {
            if (this.CustomizeBoldedDayCell != null)
            {
                this.CustomizeBoldedDayCell(this, e);
            }
        }

        protected void OnCustomizeDayCell(DayCellEventArgs e)
        {
            if (this.CustomizeDayCell != null)
            {
                this.CustomizeDayCell(this, e);
            }
        }

        protected void OnCustomizeTooltip(TooltipEventArgs e)
        {
            if (this.CustomizeTooltip != null)
            {
                this.CustomizeTooltip(this, e);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (this.m_wndproc == IntPtr.Zero)
            {
                this.m_delegate = new WindowProcCallback(this.WindowProc);
                this.m_wndproc = Marshal.GetFunctionPointerForDelegate(this.m_delegate);
            }
            try
            {
                this.m_wndprocReal = SetWindowLong(base.Handle, -4, this.m_wndproc);
            }
            catch
            {
            }
            base.OnHandleCreated(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            int num = 0;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    num = -1;
                    break;

                case Keys.Up:
                    num = -7;
                    break;

                case Keys.Right:
                    num = 1;
                    break;

                case Keys.Down:
                    num = 7;
                    break;
            }
            if (e.KeyCode == KeyPreviousMonth)
            {
                this.UpdateCurSel(this.m_curSel.AddMonths(-1), true);
            }
            else if (e.KeyCode == KeyNextMonth)
            {
                this.UpdateCurSel(this.m_curSel.AddMonths(1), true);
            }
            else if (e.KeyCode == KeyYearSelection)
            {
                this.DisplayYearUpDown();
            }
            else if (e.KeyCode == KeyTodayDate)
            {
                this.m_redraw = true;
                this.UpdateCurSel(this.TodayDate, true);
            }
            else if (e.KeyCode == KeyNoneDate)
            {
                this.m_redraw = true;
                if (this.ShowNone)
                {
                    this.UpdateCurSel(this.TodayDate, true, true);
                }
            }
            if (num != 0)
            {
                DateTime newDate = this.m_curSel.AddDays((double) num);
                if (this.m_curSel.Month != newDate.Month)
                {
                    this.UpdateCurSel(newDate, true);
                }
                else
                {
                    this.UpdateCurSel(newDate, false);
                    if (this.ValueChanged != null)
                    {
                        this.ValueChanged(this, EventArgs.Empty);
                    }
                }
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return) && this.m_doCloseUp)
            {
                this.Close();
            }
            base.OnKeyUp(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if ((!this.m_yearUpDownFocused && (this.m_yearUpDown != null)) && (!this.m_yearUpDown.Focused && this.m_doCloseUp))
            {
                this.Close();
            }
            base.OnLostFocus(e);
        }

        protected void OnMonthChangeAfter(MonthChangeAfterEventArgs e)
        {
            if (this.MonthChangeAfter != null)
            {
                this.MonthChangeAfter(this, e);
            }
        }

        protected void OnMonthChangeBefore(MonthChangeBeforeEventArgs e)
        {
            if (this.MonthChangeBefore != null)
            {
                this.MonthChangeBefore(this, e);
            }
        }

        private void OnMonthMenuClick(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                DateTime newDate = DateTime.Parse(string.Format("{0}, {1} {2}", item.Text, 1, this.m_curSel.Year));
                newDate = newDate.AddDays((double) (-1 + Math.Min(this.m_curSel.Day, DateTime.DaysInMonth(newDate.Year, newDate.Month))));
                if (newDate.Month != this.GetMonthIndex(this.m_selectedMonthIndex))
                {
                    this.UpdateCurSel(newDate, true, this.m_selectedMonthIndex);
                }
            }
        }

        private void OnMonthMenuPopup(object sender, EventArgs e)
        {
            foreach (MenuItem item in this.m_monthMenu.MenuItems)
            {
                item.Checked = false;
            }
            int monthIndex = this.GetMonthIndex(this.m_selectedMonthIndex);
            if ((monthIndex > 0) && (monthIndex <= 12))
            {
                this.m_monthMenu.MenuItems[monthIndex - 1].Checked = true;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.m_graphics == null)
            {
                base.OnMouseDown(e);
                base.Show();
            }
            else if ((this.ContextMenu != null) && MouseUtils.IsContextMenu(e, base.Handle))
            {
                DateTime time;
                this.GetDateAt(e.X, e.Y, out time);
                this.UpdateCurSel(time, true);
                this.Refresh();
                this.ContextMenu.Show(this, new Point(e.X, e.Y));
            }
            else
            {
                this.m_activateDateClickedEvent = false;
                base.OnMouseDown(e);
                base.Focus();
                if (this.m_yearUpDown.Visible && !this.m_yearUpDown.Bounds.Contains(e.X, e.Y))
                {
                    this.OnYearUpDownValueChanged(null, EventArgs.Empty);
                    base.Focus();
                    bool doCloseUp = this.m_doCloseUp;
                    this.m_doCloseUp = false;
                    this.YearUpDownFocused = false;
                    this.m_doCloseUp = doCloseUp;
                }
                if (this.m_layout.LeftArrowBounds.Contains(e.X, e.Y))
                {
                    this.UpdateCurSel(this.m_curSel.AddMonths(-this.GetScrollChange()), true, this.GetMonthIndex(this.m_curSel));
                }
                else if (this.m_layout.RightArrowBounds.Contains(e.X, e.Y))
                {
                    this.UpdateCurSel(this.m_curSel.AddMonths(this.GetScrollChange()), true, this.GetMonthIndex(this.m_curSel));
                }
                else
                {
                    if (this.m_EnableMonthMenu)
                    {
                        for (int i = 0; i < this.m_rcMonths.Count; i++)
                        {
                            Rectangle rectangle4 = (Rectangle) this.m_rcMonths[i];
                            if (rectangle4.Contains(e.X, e.Y))
                            {
                                this.DisplayMonthMenu(e.X, e.Y, i);
                                return;
                            }
                        }
                    }
                    if (this.m_EnableYearUpDown)
                    {
                        for (int j = 0; j < this.m_rcYears.Count; j++)
                        {
                            Rectangle rectangle5 = (Rectangle) this.m_rcYears[j];
                            if (rectangle5.Contains(e.X, e.Y))
                            {
                                this.DisplayYearUpDown((Rectangle) this.m_rcYears[j], this.m_curSel.AddMonths(-this.GetMonthIndex(this.m_curSel)).AddMonths(j).Year, j);
                                return;
                            }
                        }
                    }
                    if (e.Y >= this.m_layout.TodayBarPos.Y)
                    {
                        if (this.m_layout.ShowTodayBar && (e.Y < this.m_layout.NoneBarPos.Y))
                        {
                            this.UpdateCurSel(this.TodayDate, true);
                            if (this.TodayClicked != null)
                            {
                                this.TodayClicked(this, EventArgs.Empty);
                            }
                            if (this.m_doCloseUp)
                            {
                                this.Close();
                            }
                        }
                        else
                        {
                            this.UpdateCurSel(this.m_curSel, false, true);
                            if (this.NoneSelected != null)
                            {
                                this.NoneSelected(this, EventArgs.Empty);
                            }
                            if (this.m_doCloseUp)
                            {
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        if ((this.m_layout.GetMonthLayout(0).MonthCaptionBounds.Bottom + this.m_layout.WeekLettersHeight) < e.Y)
                        {
                            this.m_activateDateClickedEvent = true;
                        }
                        this.m_captureMouse = true;
                        this.UpdateHoverCell(e.X, e.Y, true);
                        this.m_TouchNavigatorTool.MouseDown(e);
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.m_captureMouse)
            {
                this.UpdateHoverCell(e.X, e.Y, false);
            }
            this.m_TouchNavigatorTool.MouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.m_captureMouse)
            {
                this.m_tooltip.Hide();
                this.m_tooltipTimer.Enabled = false;
                this.m_tooltiping = false;
                this.m_captureMouse = false;
                int monthIndex = this.GetMonthIndex(this.m_hoverSel);
                int dayIndex = this.GetDayIndex(this.m_hoverSel, monthIndex);
                int numberOfDays = this.GetNumberOfDays(monthIndex);
                if ((dayIndex >= 0) && (dayIndex < numberOfDays))
                {
                    bool redraw = true;
                    if ((this.m_curMonth > 0) && (this.m_curYear > 0))
                    {
                        DateTime time = new DateTime(this.m_curYear, this.m_curMonth, 1);
                        if ((this.m_hoverSel >= time) && (this.m_hoverSel < time.AddMonths(this.m_monthCount)))
                        {
                            redraw = false;
                        }
                    }
                    this.UpdateCurSel(this.m_hoverSel, redraw);
                    if (this.m_doCloseUp)
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.UpdateCurSel(this.m_curSel, false);
                }
                if (this.m_activateDateClickedEvent)
                {
                    this.m_activateDateClickedEvent = false;
                    DateClickedEventArgs args = new DateClickedEventArgs(this.Value);
                    if (this.DateClicked != null)
                    {
                        this.DateClicked(this, args);
                    }
                }
                this.m_TouchNavigatorTool.MouseUp(e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!this.m_redraw)
            {
                goto Label_00A4;
            }
            if (((this.m_curYear >= 1) && (this.m_curMonth >= 1)) && (this.m_curSel >= new DateTime(this.m_curYear, this.m_curMonth, 1)))
            {
                DateTime time = new DateTime(this.m_curYear, this.m_curMonth, 1);
                if (this.m_curSel < time.AddMonths(this.m_monthCount))
                {
                    goto Label_0097;
                }
            }
            this.CalculateDays(this.m_monthCount);
            this.m_curMonth = this.m_curSel.Month;
            this.m_curYear = this.m_curSel.Year;
        Label_0097:
            this.Redraw();
            this.m_redraw = false;
        Label_00A4:
            e.Graphics.DrawImage(this.m_bmp, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            this.m_layout.Resize(0, 0, base.Width, base.Height);
            this.m_redraw = true;
            base.Invalidate();
            base.OnResize(e);
        }

        private void OnYearUpDownKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.OnYearUpDownValueChanged(null, EventArgs.Empty);
                bool doCloseUp = this.m_doCloseUp;
                this.m_doCloseUp = false;
                this.YearUpDownFocused = false;
                base.Focus();
                this.m_doCloseUp = doCloseUp;
                if (!this.m_yearUpDownNumericUpDown)
                {
                    SetInputMode(this.m_yearUpDown, InputMode.Text);
                }
            }
        }

        private void OnYearUpDownLostFocus(object sender, EventArgs e)
        {
        }

        private void OnYearUpDownValueChanged(object sender, EventArgs e)
        {
            if (this.m_yearUpDown.Visible)
            {
                DateTime newDate = new DateTime((this.m_curSel.Year - this.m_curSel.AddMonths(-this.GetMonthIndex(this.m_curSel) + this.m_yearUpDownIndex).Year) + Convert.ToInt32(this.m_yearUpDownNumericUpDown ? ((NumericUpDown) this.m_yearUpDown).Value : Convert.ToInt32(this.m_yearUpDown.Text)), this.m_curSel.Month, this.m_curSel.Day);
                this.UpdateCurSel(newDate, true, this.GetMonthIndex(this.m_curSel));
            }
        }

        public void PreviousMonth()
        {
            this.UpdateCurSel(this.m_curSel.AddMonths(-this.GetScrollChange()), true, this.GetMonthIndex(this.m_curSel));
        }

        private void Redraw()
        {
            this.CreateMemoryBitmap();
            this.CalculateFirstDate();
            if (!this.m_useGradient)
            {
                this.m_graphics.Clear(this.BackColor);
            }
            else
            {
                this.m_gradientGraphics.Clear(this.BackColor);
                Rectangle clientRectangle = base.ClientRectangle;
                if (this.ShowTitle)
                {
                    clientRectangle.Y += this.TitleHeight;
                }
                this.m_gradientBackColor.DrawGradient(this.m_gradientGraphics, base.ClientRectangle);
                this.m_graphics.DrawImage(this.m_gradientBmp, 0, 0);
            }
            this.DrawMonths(this.m_graphics);
            this.DrawControlFrame(this.m_graphics);
            this.DrawTodaySelection(this.m_graphics);
            if (!this.m_isNone)
            {
                this.DrawHoverSelection(this.m_graphics, this.m_hoverSel, true);
            }
            this.DrawBottomLabels(this.m_graphics);
        }

        private void RedrawBrokenBorders(DateTime day)
        {
            int monthIndex = this.GetMonthIndex(day);
            int dayIndex = this.GetDayIndex(day, monthIndex);
            this.RedrawBrokenBorders(monthIndex, dayIndex);
        }

        private void RedrawBrokenBorders(int mIndex, int dIndex)
        {
            int num = dIndex % 7;
            bool flag = false;
            if ((dIndex >= 0) && (dIndex <= 6))
            {
                this.DrawUpperBorder(this.m_graphics, this.m_layout.GetMonthLayout(mIndex));
                flag = true;
            }
            switch (num)
            {
                case 0:
                    this.DrawLeftBorder(this.m_graphics, this.m_layout.GetMonthLayout(mIndex));
                    flag = true;
                    break;

                case 6:
                    flag = true;
                    break;
            }
            if ((dIndex >= 0x23) && (dIndex <= 0x29))
            {
                flag = true;
            }
            if (flag)
            {
                this.DrawMonthFrame(this.m_graphics, this.m_layout.GetMonthLayout(mIndex));
                this.DrawControlFrame(this.m_graphics);
            }
        }

        public override void Refresh()
        {
            this.m_redraw = true;
            base.Refresh();
        }

        public void RemoveAllAnnuallyBoldedDates()
        {
            this.m_customBoldedDates.Clear(BoldedDateType.Annually);
        }

        public void RemoveAllBoldedDates()
        {
            this.m_customBoldedDates.Clear(BoldedDateType.Nonrecurrent);
        }

        public void RemoveAllMonthlyBoldedDates()
        {
            this.m_customBoldedDates.Clear(BoldedDateType.Monthly);
        }

        public void RemoveAnnuallyBoldedDate(DateTime date)
        {
            int count = this.m_customBoldedDates.Count;
            for (int i = 0; i < count; i++)
            {
                Resco.Controls.OutlookControls.DayCell cell = this.m_customBoldedDates[i];
                if (((cell.Type == BoldedDateType.Annually) && (date.Day == cell.Date.Day)) && (date.Month == cell.Date.Month))
                {
                    this.m_customBoldedDates.RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveBoldedDate(DateTime date)
        {
            int count = this.m_customBoldedDates.Count;
            for (int i = 0; i < count; i++)
            {
                Resco.Controls.OutlookControls.DayCell cell = this.m_customBoldedDates[i];
                if (((cell.Type == BoldedDateType.Nonrecurrent) && (date.Day == cell.Date.Day)) && ((date.Month == cell.Date.Month) & (date.Year == cell.Date.Year)))
                {
                    this.m_customBoldedDates.RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveMonthlyBoldedDate(DateTime date)
        {
            int count = this.m_customBoldedDates.Count;
            for (int i = 0; i < count; i++)
            {
                Resco.Controls.OutlookControls.DayCell cell = this.m_customBoldedDates[i];
                if ((cell.Type == BoldedDateType.Monthly) && (date.Day == cell.Date.Day))
                {
                    this.m_customBoldedDates.RemoveAt(i);
                    return;
                }
            }
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.m_layout.MonthCaptionHeight = (int) (this.m_layout.MonthCaptionHeight * factor.Height);
            base.ScaleControl(factor, specified);
        }

        [DllImport("coredll.dll")]
        private static extern uint SendMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);
        public void SetFirstMonth(DateTime newDate)
        {
            new DateTime(this.m_curYear, this.m_curMonth, 1);
            this.m_curMonth = newDate.Month;
            this.m_curYear = newDate.Year;
            this.m_curSel = newDate;
            this.m_hoverSel = newDate;
            this.CalculateDays(this.m_monthCount);
        }

        [DllImport("coredll.dll")]
        private static extern IntPtr SetFocus(IntPtr hWnd);
        private static void SetInputMode(Control ctrl, InputMode mode)
        {
            SendMessage(ctrl.Handle, 0xde, 0, (uint) mode);
        }

        [DllImport("coredll.dll")]
        private static extern IntPtr SetWindowLong(IntPtr hwnd, int nIndex, IntPtr dwNewLong);
        protected virtual bool ShouldSerializeAnnuallyBoldedDates()
        {
            return (this.m_customBoldedDates.GetBoldedDates(BoldedDateType.Annually).Count > 0);
        }

        protected virtual bool ShouldSerializeBoldedDates()
        {
            return (this.m_customBoldedDates.GetBoldedDates(BoldedDateType.Nonrecurrent).Count > 0);
        }

        protected virtual bool ShouldSerializeCalendarDimensions()
        {
            if (this.m_calendarDimensions.Width == 1)
            {
                return (this.m_calendarDimensions.Height != 1);
            }
            return true;
        }

        protected virtual bool ShouldSerializeCalendarWeekRule()
        {
            if (this.m_calendarWeekRule == DateTimeFormatInfo.CurrentInfo.CalendarWeekRule)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeDayTextAlignment()
        {
            return (this.m_DayTextAlignment != Alignment.TopLeft);
        }

        protected virtual bool ShouldSerializeFirstDayOfWeek()
        {
            if (this.m_firstDayOfWeek == Resco.Controls.OutlookControls.Day.Default)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeGradientBackColor()
        {
            return (((this.m_gradientBackColor.StartColor.ToArgb() != Color.White.ToArgb()) | (this.m_gradientBackColor.EndColor.ToArgb() != Color.White.ToArgb())) | (this.m_gradientBackColor.FillDirection != FillDirection.Vertical));
        }

        protected virtual bool ShouldSerializeMonthlyBoldedDates()
        {
            return (this.m_customBoldedDates.GetBoldedDates(BoldedDateType.Monthly).Count > 0);
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
            if (this.Value == DateTime.Today)
            {
                return false;
            }
            return true;
        }

        private void ShowTooltip(int dayIndex, int monthIndex)
        {
            if (!base.Controls.Contains(this.m_tooltip) && ((this.Site == null) || !this.Site.DesignMode))
            {
                base.Controls.Add(this.m_tooltip);
            }
            this.m_tooltipArgs._day = this.GetDay(monthIndex, dayIndex);
            this.m_tooltipArgs.Text = null;
            this.m_tooltipArgs.Font = base.Font;
            this.m_tooltipArgs.BackColor = SystemColors.Info;
            this.m_tooltipArgs.ForeColor = Color.Black;
            Resco.Controls.OutlookControls.DayCell[] cellArray = this.m_customBoldedDates[this.m_tooltipArgs._day];
            if ((cellArray != null) && (cellArray.Length > 0))
            {
                foreach (Resco.Controls.OutlookControls.DayCell cell in cellArray)
                {
                    this.m_tooltipArgs.Text = this.m_tooltipArgs.Text + cell.TooltipText + "\n";
                }
            }
            this.OnCustomizeTooltip(this.m_tooltipArgs);
            if ((this.m_tooltipArgs.Text != null) && (this.m_tooltipArgs.Text != ""))
            {
                this.m_tooltip.Text = this.m_tooltipArgs.Text;
                this.m_tooltip.Font = this.m_tooltipArgs.Font;
                this.m_tooltip.BackColor = this.m_tooltipArgs.BackColor;
                this.m_tooltip.ForeColor = this.m_tooltipArgs.ForeColor;
                Rectangle cellBounds = this.m_layout.GetMonthLayout(monthIndex).GetCellBounds(dayIndex);
                this.m_tooltip.MoveTo(new Point(cellBounds.X, cellBounds.Y));
                this.m_tooltip.Invalidate();
                if (!this.m_tooltip.Visible)
                {
                    this.m_tooltip.Show();
                }
            }
            else
            {
                this.m_tooltip.Hide();
            }
        }

        private void TouchNavigatorTool_GestureDetected(object sender, TouchNavigatorTool.GestureEventArgs e)
        {
            int months = this.m_calendarDimensions.Width * this.m_calendarDimensions.Height;
            switch (e.Gesture)
            {
                case TouchNavigatorTool.GestureType.Left:
                case TouchNavigatorTool.GestureType.Up:
                    this.Value = this.Value.AddMonths(months);
                    return;

                case TouchNavigatorTool.GestureType.Right:
                case TouchNavigatorTool.GestureType.Down:
                    this.Value = this.Value.AddMonths(-months);
                    return;
            }
        }

        public void UpdateBoldedDates()
        {
            this.m_redraw = true;
            base.Invalidate();
        }

        private void UpdateCurSel(DateTime newDate, bool redraw)
        {
            this.UpdateCurSel(newDate, redraw, -1, false);
        }

        private void UpdateCurSel(DateTime newDate, bool redraw, bool setNone)
        {
            this.UpdateCurSel(newDate, redraw, -1, setNone);
        }

        private void UpdateCurSel(DateTime newDate, bool redraw, int monthIndex)
        {
            this.UpdateCurSel(newDate, redraw, monthIndex, false);
        }

        private void UpdateCurSel(DateTime newDate, bool redraw, int monthIndex, bool setNone)
        {
            bool isNone = this.m_isNone;
            this.m_isNone = setNone;
            bool flag2 = (this.m_curSel.Month != newDate.Month) || (this.m_curSel.Year != newDate.Year);
            DateTime curSel = this.m_curSel;
            DateTime date = newDate;
            MonthChangeBeforeEventArgs e = new MonthChangeBeforeEventArgs(curSel, newDate);
            if (flag2)
            {
                this.OnMonthChangeBefore(e);
                newDate = e.NewDate;
            }
            bool flag3 = ((this.m_curSel != newDate) || (this.m_isNone != isNone)) ? !e.Cancel : false;
            if ((newDate >= this.MinDate) && (newDate <= this.MaxDate))
            {
                DateTime time3 = this.m_curSel;
                if (!e.Cancel)
                {
                    this.m_curSel = newDate;
                    this.m_hoverSel = newDate;
                }
                if (!e.Cancel)
                {
                    if (monthIndex == -1)
                    {
                        if ((this.m_curYear == -1) || (this.m_curMonth == -1))
                        {
                            this.m_curMonth = newDate.Month;
                            this.m_curYear = newDate.Year;
                            this.CalculateDays(this.m_monthCount);
                            redraw = true;
                        }
                        else
                        {
                            DateTime time4 = new DateTime(this.m_curYear, this.m_curMonth, 1);
                            if ((newDate < time4) || (newDate >= time4.AddMonths(this.m_monthCount)))
                            {
                                if (newDate < time4)
                                {
                                    this.m_curMonth = newDate.Month;
                                    this.m_curYear = newDate.Year;
                                }
                                if (newDate >= time4.AddMonths(this.m_monthCount))
                                {
                                    DateTime time5 = newDate.AddMonths(-(this.m_monthCount - 1));
                                    this.m_curMonth = time5.Month;
                                    this.m_curYear = time5.Year;
                                }
                                this.CalculateDays(this.m_monthCount);
                                redraw = true;
                            }
                            else
                            {
                                redraw = false;
                            }
                        }
                    }
                    else
                    {
                        DateTime time6 = newDate.AddMonths(-monthIndex);
                        this.m_curMonth = time6.Month;
                        this.m_curYear = time6.Year;
                        this.CalculateDays(this.m_monthCount);
                        redraw = true;
                    }
                }
                if (base.Visible && (this.m_graphics != null))
                {
                    if (redraw)
                    {
                        this.m_redraw = true;
                    }
                    else
                    {
                        if (e.Cancel || (newDate != date))
                        {
                            int mIndex = this.GetMonthIndex(date);
                            int dayIndex = this.GetDayIndex(date, mIndex);
                            if (this.IsInsideAnyGrid(mIndex, dayIndex))
                            {
                                if (this.DaysGrid)
                                {
                                    this.DrawHoverSelection(this.m_graphics, date, false);
                                }
                                this.DrawDay(this.m_graphics, date, false, true, false, this.m_layout.GetMonthLayout(mIndex).GetCellBounds(dayIndex), mIndex);
                            }
                            this.RedrawBrokenBorders(mIndex, dayIndex);
                        }
                        if (!e.Cancel)
                        {
                            int num3 = this.GetMonthIndex(time3);
                            int num4 = this.GetDayIndex(time3, num3);
                            if (this.IsInsideAnyGrid(num3, num4))
                            {
                                if (this.DaysGrid)
                                {
                                    this.DrawHoverSelection(this.m_graphics, time3, false);
                                }
                                this.DrawDay(this.m_graphics, time3, false, true, false, this.m_layout.GetMonthLayout(num3).GetCellBounds(num4), num3);
                            }
                            this.RedrawBrokenBorders(num3, num4);
                            if (!setNone)
                            {
                                num3 = this.GetMonthIndex(newDate);
                                num4 = this.GetDayIndex(newDate, num3);
                                if (this.IsInsideAnyGrid(num3, num4))
                                {
                                    this.DrawDay(this.m_graphics, newDate, true, true, false, this.m_layout.GetMonthLayout(num3).GetCellBounds(num4), num3);
                                }
                            }
                            this.DrawTodaySelection(this.m_graphics);
                            if (!setNone)
                            {
                                this.DrawHoverSelection(this.m_graphics, newDate, true);
                            }
                        }
                    }
                    base.Invalidate();
                }
                if (!e.Cancel && flag2)
                {
                    this.OnMonthChangeAfter(new MonthChangeAfterEventArgs(curSel, newDate));
                }
                if ((this.ValueChanged != null) && flag3)
                {
                    this.ValueChanged(this, EventArgs.Empty);
                }
            }
        }

        private void UpdateHoverCell(bool mouseDown, int newDIndex, int newMIndex)
        {
            DateTime day = this.GetDay(newMIndex, newDIndex);
            bool flag = this.IsInsideAnyGrid(newMIndex, newDIndex);
            if (flag && (day != DateTime.MinValue))
            {
                if ((this.m_hoverSel != day) || mouseDown)
                {
                    if (this.m_tooltiping)
                    {
                        this.ShowTooltip(newDIndex, newMIndex);
                    }
                    else
                    {
                        this.m_tooltipingDayIndex = newDIndex;
                        this.m_tooltipingMonthIndex = newMIndex;
                        this.m_tooltipTimer.Interval = this.m_tooltipDelay;
                        this.m_tooltipTimer.Enabled = true;
                    }
                }
            }
            else
            {
                this.m_tooltip.Hide();
                this.m_tooltipTimer.Enabled = false;
            }
            if (this.m_hoverSel != day)
            {
                this.DrawHoverSelection(this.m_graphics, this.m_hoverSel, false);
                this.DrawTodaySelection(this.m_graphics);
                this.RedrawBrokenBorders(this.m_hoverSel);
                if (flag)
                {
                    this.DrawHoverSelection(this.m_graphics, day, true);
                }
                base.Invalidate();
                this.m_hoverSel = day;
            }
        }

        private void UpdateHoverCell(int x, int y, bool mouseDown)
        {
            int monthIndex = -1;
            int dayIndex = -1;
            this.CalculateDayAndMonthIndexes(x, y, out monthIndex, out dayIndex);
            this.UpdateHoverCell(mouseDown, dayIndex, monthIndex);
        }

        protected virtual int WindowProc(IntPtr hwnd, int msg, int wParam, int lParam)
        {
            if (msg == 2)
            {
                SetWindowLong(base.Handle, -4, this.m_wndprocReal);
            }
            if ((msg == 0x101) && (wParam == 0x1b))
            {
                KeyEventArgs e = new KeyEventArgs((Keys) wParam);
                this.OnKeyUp(e);
                return 0;
            }
            return CallWindowProc(this.m_wndprocReal, hwnd, msg, wParam, lParam);
        }

        public DateTime[] AnnuallyBoldedDates
        {
            get
            {
                IList boldedDates = this.m_customBoldedDates.GetBoldedDates(BoldedDateType.Annually);
                DateTime[] timeArray = new DateTime[boldedDates.Count];
                for (int i = 0; i < boldedDates.Count; i++)
                {
                    timeArray[i] = ((Resco.Controls.OutlookControls.DayCell) boldedDates[i]).Date;
                }
                return timeArray;
            }
            set
            {
                this.m_customBoldedDates.Clear(BoldedDateType.Annually);
                if ((value != null) && (value.Length > 0))
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        this.AddAnnuallyBoldedDate(value[i]);
                    }
                }
            }
        }

        public override Color BackColor
        {
            get
            {
                if (this.m_brushBack != null)
                {
                    return this.m_brushBack.Color;
                }
                return base.BackColor;
            }
            set
            {
                if (value != this.m_brushBack.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.Window;
                    }
                    this.m_brushBack.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public DateTime[] BoldedDates
        {
            get
            {
                IList boldedDates = this.m_customBoldedDates.GetBoldedDates(BoldedDateType.Nonrecurrent);
                DateTime[] timeArray = new DateTime[boldedDates.Count];
                for (int i = 0; i < boldedDates.Count; i++)
                {
                    timeArray[i] = ((Resco.Controls.OutlookControls.DayCell) boldedDates[i]).Date;
                }
                return timeArray;
            }
            set
            {
                this.m_customBoldedDates.Clear(BoldedDateType.Nonrecurrent);
                if ((value != null) && (value.Length > 0))
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        this.AddBoldedDate(value[i]);
                    }
                }
            }
        }

        public Size CalendarDimensions
        {
            get
            {
                return this.m_calendarDimensions;
            }
            set
            {
                if (((value != this.m_calendarDimensions) && (value.Width > 0)) && (value.Height > 0))
                {
                    this.m_calendarDimensions = value;
                    this.m_monthCount = value.Width * value.Height;
                    this.m_layout.SetDimension(value.Width, value.Height);
                    this.UpdateCurSel(this.m_curSel, true);
                    this.CalculateDays(this.m_monthCount);
                    this.m_rcMonths.Clear();
                    this.m_rcYears.Clear();
                    for (int i = 0; i < this.m_monthCount; i++)
                    {
                        this.m_rcMonths.Add(Rectangle.Empty);
                        this.m_rcYears.Add(Rectangle.Empty);
                    }
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public System.Globalization.CalendarWeekRule CalendarWeekRule
        {
            get
            {
                return this.m_calendarWeekRule;
            }
            set
            {
                if (value != this.m_calendarWeekRule)
                {
                    this.m_calendarWeekRule = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public override System.Windows.Forms.ContextMenu ContextMenu
        {
            get
            {
                return this.m_ContextMenu;
            }
            set
            {
                this.m_ContextMenu = value;
            }
        }

        public DayCellCollection CustomBoldedDates
        {
            get
            {
                return this.m_customBoldedDates;
            }
        }

        public bool DaysGrid
        {
            get
            {
                return this.m_layout.ShowDaysGrid;
            }
            set
            {
                this.m_layout.ShowDaysGrid = value;
                this.m_redraw = true;
                base.Invalidate();
            }
        }

        public Color DaysGridColor
        {
            get
            {
                return this.m_penDaysGrid.Color;
            }
            set
            {
                if (value != this.m_penDaysGrid.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.WindowFrame;
                    }
                    this.m_penDaysGrid.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public string DaysOfWeek
        {
            get
            {
                return this.m_DaysOfWeek;
            }
            set
            {
                if (this.m_DaysOfWeek != value)
                {
                    this.m_DaysOfWeek = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public Alignment DayTextAlignment
        {
            get
            {
                return this.m_DayTextAlignment;
            }
            set
            {
                this.m_DayTextAlignment = value;
                this.m_redraw = true;
                base.Invalidate();
            }
        }

        public bool EnableMonthMenu
        {
            get
            {
                return this.m_EnableMonthMenu;
            }
            set
            {
                this.m_EnableMonthMenu = value;
            }
        }

        public bool EnableYearUpDown
        {
            get
            {
                return this.m_EnableYearUpDown;
            }
            set
            {
                this.m_EnableYearUpDown = value;
            }
        }

        public Resco.Controls.OutlookControls.Day FirstDayOfWeek
        {
            get
            {
                return this.m_firstDayOfWeek;
            }
            set
            {
                if (value != this.m_firstDayOfWeek)
                {
                    this.m_firstDayOfWeek = value;
                    this.CalculateFirstDate();
                    this.CalculateDays(this.m_monthCount);
                    this.m_redraw = true;
                    base.Invalidate();
                }
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
                    if (this.m_graphics != null)
                    {
                        this.m_layout.FontChange(this.m_graphics, value);
                    }
                    if (this.m_fontCaption != null)
                    {
                        this.m_fontCaption.Dispose();
                        this.m_fontCaption = null;
                    }
                    this.m_fontCaption = new System.Drawing.Font(value.Name, value.Size, FontStyle.Bold);
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public System.Drawing.Font FontWeekLetters
        {
            get
            {
                return this.m_fontWeekLetters;
            }
            set
            {
                if (value != this.m_fontWeekLetters)
                {
                    this.m_fontWeekLetters = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public override Color ForeColor
        {
            get
            {
                if (this.m_brushCur != null)
                {
                    return this.m_brushCur.Color;
                }
                return base.ForeColor;
            }
            set
            {
                if (value != this.m_brushCur.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.WindowText;
                    }
                    this.m_brushCur.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
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
                if (this.m_gradientBackColor != value)
                {
                    this.m_gradientBackColor.PropertyChanged -= new EventHandler(this.m_gradientBackColor_PropertyChanged);
                    this.m_gradientBackColor = value;
                    this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
                }
                this.m_redraw = true;
                base.Invalidate();
            }
        }

        public bool IsNone
        {
            get
            {
                return this.m_isNone;
            }
            set
            {
                if (this.m_layout.ShowNoneBar && (this.m_isNone != value))
                {
                    this.UpdateCurSel(this.m_curSel, true, value);
                    if (this.m_isNone && (this.NoneSelected != null))
                    {
                        this.NoneSelected(this, EventArgs.Empty);
                    }
                }
            }
        }

        public DateTime MaxDate
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
                    if (value > MaxDateTime)
                    {
                        throw new ArgumentException("DateTimePickerMaxDate", MaxDateTime.ToString("G"));
                    }
                    this.m_MaxDate = value;
                    if (this.m_MaxDate < this.Value)
                    {
                        this.Value = this.m_MaxDate;
                    }
                }
            }
        }

        public DateTime MinDate
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
                    if (value < MinDateTime)
                    {
                        throw new ArgumentException("DateTimePickerMinDate", MinDateTime.ToString("G"));
                    }
                    this.m_MinDate = value;
                    if (this.m_MinDate > this.Value)
                    {
                        this.Value = this.m_MinDate;
                    }
                }
            }
        }

        public DateTime[] MonthlyBoldedDates
        {
            get
            {
                IList boldedDates = this.m_customBoldedDates.GetBoldedDates(BoldedDateType.Monthly);
                DateTime[] timeArray = new DateTime[boldedDates.Count];
                for (int i = 0; i < boldedDates.Count; i++)
                {
                    timeArray[i] = ((Resco.Controls.OutlookControls.DayCell) boldedDates[i]).Date;
                }
                return timeArray;
            }
            set
            {
                this.m_customBoldedDates.Clear(BoldedDateType.Monthly);
                if ((value != null) && (value.Length > 0))
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        this.AddMonthlyBoldedDate(value[i]);
                    }
                }
            }
        }

        public bool MonthsGrid
        {
            get
            {
                return this.m_layout.ShowGrid;
            }
            set
            {
                this.m_layout.ShowGrid = value;
                this.m_redraw = true;
                base.Invalidate();
            }
        }

        public Color MonthsGridColor
        {
            get
            {
                return this.m_penMonthsGrid.Color;
            }
            set
            {
                if (value != this.m_penMonthsGrid.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.WindowFrame;
                    }
                    this.m_penMonthsGrid.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
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
                if (value != this.m_noneText)
                {
                    this.m_noneText = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public int ScrollChange
        {
            get
            {
                return this.m_scrollChange;
            }
            set
            {
                if (value >= 0)
                {
                    this.m_scrollChange = value;
                }
            }
        }

        public Color SelectedCellBackColor
        {
            get
            {
                return this.m_brushSelBack.Color;
            }
            set
            {
                if (value != this.m_brushSelBack.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.Highlight;
                    }
                    this.m_brushSelBack.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public Color SelectedCellBorderColor
        {
            get
            {
                return this.m_penHoverBox.Color;
            }
            set
            {
                if (value != this.m_penHoverBox.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.GrayText;
                    }
                    this.m_penHoverBox.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public Color SelectedCellForeColor
        {
            get
            {
                return this.m_brushSelText.Color;
            }
            set
            {
                if (value != this.m_brushSelText.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.HighlightText;
                    }
                    this.m_brushSelText.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public bool ShowNone
        {
            get
            {
                return this.m_layout.ShowNoneBar;
            }
            set
            {
                if (value != this.m_layout.ShowNoneBar)
                {
                    if (!this.m_layout.ShowNoneBar && this.IsNone)
                    {
                        this.IsNone = false;
                    }
                    if (!value)
                    {
                        base.Height -= this.m_layout.NoneBarHeight * this.m_calendarDimensions.Height;
                    }
                    this.m_layout.ShowNoneBar = value;
                    if (value)
                    {
                        base.Height += this.m_layout.NoneBarHeight * this.m_calendarDimensions.Height;
                    }
                    this.m_layout.Resize(0, 0, base.Width, base.Height);
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public bool ShowTitle
        {
            get
            {
                return this.m_layout.ShowMonthCaption;
            }
            set
            {
                if (value != this.m_layout.ShowMonthCaption)
                {
                    if (!value)
                    {
                        base.Height -= this.m_layout.MonthCaptionActualHeight * this.m_calendarDimensions.Height;
                    }
                    this.m_layout.ShowMonthCaption = value;
                    if (value)
                    {
                        base.Height += this.m_layout.MonthCaptionActualHeight * this.m_calendarDimensions.Height;
                    }
                    this.m_layout.Resize(0, 0, base.Width, base.Height);
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public bool ShowToday
        {
            get
            {
                return this.m_layout.ShowTodayBar;
            }
            set
            {
                if (value != this.m_layout.ShowTodayBar)
                {
                    if (!value)
                    {
                        base.Height -= this.m_layout.TodayBarHeight * this.m_calendarDimensions.Height;
                    }
                    this.m_layout.ShowTodayBar = value;
                    if (value)
                    {
                        base.Height += this.m_layout.TodayBarHeight * this.m_calendarDimensions.Height;
                    }
                    this.m_layout.Resize(0, 0, base.Width, base.Height);
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public bool ShowTodayBorder
        {
            get
            {
                return this.m_showTodayBorder;
            }
            set
            {
                if (value != this.m_showTodayBorder)
                {
                    this.m_showTodayBorder = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public bool ShowWeekLetters
        {
            get
            {
                return this.m_layout.ShowWeekLetters;
            }
            set
            {
                if (value != this.m_layout.ShowWeekLetters)
                {
                    if (!value)
                    {
                        base.Height -= this.m_layout.WeekLettersHeight * this.m_calendarDimensions.Height;
                    }
                    this.m_layout.ShowWeekLetters = value;
                    if (value)
                    {
                        base.Height += this.m_layout.WeekLettersHeight * this.m_calendarDimensions.Height;
                    }
                    this.m_layout.Resize(0, 0, base.Width, base.Height);
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public bool ShowWeekNumbers
        {
            get
            {
                return this.m_layout.ShowWeekNumbers;
            }
            set
            {
                if (value != this.m_layout.ShowWeekNumbers)
                {
                    if (!value)
                    {
                        base.Width -= this.m_layout.WeekNumbersWidth * this.m_calendarDimensions.Width;
                    }
                    this.m_layout.ShowWeekNumbers = value;
                    if (value)
                    {
                        base.Width += this.m_layout.WeekNumbersWidth * this.m_calendarDimensions.Width;
                    }
                    this.m_layout.Resize(0, 0, base.Width, base.Height);
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        public Color TitleBackColor
        {
            get
            {
                return this.m_brushCaptionBack.Color;
            }
            set
            {
                if (value != this.m_brushCaptionBack.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.ActiveCaption;
                    }
                    this.m_brushCaptionBack.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public Color TitleForeColor
        {
            get
            {
                return this.m_brushCaptionText.Color;
            }
            set
            {
                if (value != this.m_brushCaptionText.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.ActiveCaptionText;
                    }
                    this.m_brushCaptionText.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public int TitleHeight
        {
            get
            {
                return this.m_layout.MonthCaptionHeight;
            }
            set
            {
                this.m_layout.MonthCaptionHeight = value;
                this.m_layout.Resize(0, 0, base.Width, base.Height);
                if ((value < 0) && (this.m_graphics != null))
                {
                    this.m_layout.FontChange(this.m_graphics, this.Font);
                }
                this.m_redraw = true;
                base.Invalidate();
            }
        }

        public bool TitleVistaStyle
        {
            get
            {
                return this.m_titleVistaStyle;
            }
            set
            {
                if (this.m_titleVistaStyle != value)
                {
                    this.m_titleVistaStyle = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
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
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public string TodayText
        {
            get
            {
                return this.m_todayText;
            }
            set
            {
                if (value != this.m_todayText)
                {
                    this.m_todayText = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public int TooltipDelay
        {
            get
            {
                return this.m_tooltipDelay;
            }
            set
            {
                this.m_tooltipDelay = value;
            }
        }

        public bool TouchScrolling
        {
            get
            {
                return this.m_TouchNavigatorTool.EnableTouchScrolling;
            }
            set
            {
                this.m_TouchNavigatorTool.EnableTouchScrolling = value;
            }
        }

        public int TouchSensitivity
        {
            get
            {
                return this.m_TouchNavigatorTool.TouchSensitivity;
            }
            set
            {
                this.m_TouchNavigatorTool.TouchSensitivity = value;
            }
        }

        public Color TrailingBackColor
        {
            get
            {
                return this.m_brushOtherBack.Color;
            }
            set
            {
                if (value != this.m_brushOtherBack.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = Color.Transparent;
                    }
                    this.m_brushOtherBack.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public Color TrailingForeColor
        {
            get
            {
                return this.m_brushOther.Color;
            }
            set
            {
                if (value != this.m_brushOther.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.GrayText;
                    }
                    this.m_brushOther.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
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
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public DateTime Value
        {
            get
            {
                return this.m_curSel;
            }
            set
            {
                if (value != this.m_curSel)
                {
                    this.UpdateCurSel(value, true);
                }
            }
        }

        public Color WeekendSaturdayBackColor
        {
            get
            {
                return this.m_brushWeekendSaturdayBack.Color;
            }
            set
            {
                if (value != this.m_brushWeekendSaturdayBack.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = Color.Transparent;
                    }
                    this.m_brushWeekendSaturdayBack.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public Color WeekendSaturdayForeColor
        {
            get
            {
                return this.m_brushWeekLettersSaturday.Color;
            }
            set
            {
                if (value != this.m_brushWeekLettersSaturday.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = Color.Transparent;
                    }
                    this.m_brushWeekLettersSaturday.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public Color WeekendSundayBackColor
        {
            get
            {
                return this.m_brushWeekendSundayBack.Color;
            }
            set
            {
                if (value != this.m_brushWeekendSundayBack.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = Color.Transparent;
                    }
                    this.m_brushWeekendSundayBack.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public Color WeekendSundayForeColor
        {
            get
            {
                return this.m_brushWeekLettersSunday.Color;
            }
            set
            {
                if (value != this.m_brushWeekLettersSunday.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = Color.Transparent;
                    }
                    this.m_brushWeekLettersSunday.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public Color WeekLettersBackColor
        {
            get
            {
                return this.m_brushWeekLettersBack.Color;
            }
            set
            {
                if (value != this.m_brushWeekLettersBack.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.Window;
                    }
                    this.m_brushWeekLettersBack.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        public Color WeekLettersForeColor
        {
            get
            {
                return this.m_brushWeekLettersText.Color;
            }
            set
            {
                if (value != this.m_brushWeekLettersText.Color)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.ActiveCaption;
                    }
                    this.m_brushWeekLettersText.Color = value;
                    this.m_redraw = true;
                    base.Invalidate();
                }
            }
        }

        private bool YearUpDownFocused
        {
            get
            {
                return this.m_yearUpDownFocused;
            }
            set
            {
                if (this.m_yearUpDownFocused != value)
                {
                    this.m_yearUpDownFocused = value;
                    if (value)
                    {
                        this.m_yearUpDown.Show();
                        this.m_yearUpDown.Focus();
                    }
                    else
                    {
                        this.m_yearUpDown.Hide();
                    }
                }
            }
        }

        private enum InputMode
        {
            Spell,
            T9,
            Numbers,
            Text
        }

        private delegate int WindowProcCallback(IntPtr hwnd, int msg, int wParam, int lParam);
    }
}

