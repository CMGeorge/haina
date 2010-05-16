namespace Resco.Controls.DetailView.DetailViewInternal
{
    using Resco.Controls.DetailView;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class MonthCalendarEx : UserControl
    {
        public static string AdditionalTextNoneDate = "";
        public static string AdditionalTextTodayDate = "";
        public static Keys KeyNextMonth = Keys.NumPad9;
        public static Keys KeyNoneDate = Keys.D2;
        public static Keys KeyPreviousMonth = Keys.NumPad8;
        public static Keys KeyTodayDate = Keys.D5;
        public static Keys KeyYearSelection = Keys.D0;
        private ArrayList m_annualyBoldedDates = new ArrayList();
        private Bitmap m_bmp;
        private ArrayList m_boldedDates = new ArrayList();
        private SolidBrush m_brushBack;
        private SolidBrush m_brushCaptionBack;
        private SolidBrush m_brushCaptionText;
        private SolidBrush m_brushCur;
        private SolidBrush m_brushFrame;
        private SolidBrush m_brushOther;
        private SolidBrush m_brushSelBack;
        private SolidBrush m_brushSelText;
        private bool m_bTodayChanged;
        private System.Globalization.CalendarWeekRule m_calendarWeekRule = DateTimeFormatInfo.CurrentInfo.CalendarWeekRule;
        private bool m_captureMouse;
        private int m_curMonth = -1;
        private DateTime m_curSel = DateTime.Today;
        private int m_curYear = -1;
        private DateTimePickerEx m_DateTime;
        private DateTime[] m_days = new DateTime[0x2a];
        private string m_DaysOfWeek = "";
        internal bool m_doCloseUp;
        private bool m_doHide;
        private DateTime m_firstDate;
        private Resco.Controls.DetailView.DetailViewInternal.Day m_firstDayOfWeek = Resco.Controls.DetailView.DetailViewInternal.Day.Default;
        private System.Drawing.Font m_fontCaption;
        private Graphics m_graphics;
        private DateTime m_hoverSel = DateTime.Today;
        private bool m_isNone;
        private Point[] m_leftArrowPoints = new Point[3];
        private DateTime m_MaxDate = DateTimePickerEx.MaxDateTime;
        private DateTime m_MinDate = DateTimePickerEx.MinDateTime;
        private ArrayList m_monthlyBoldedDates = new ArrayList();
        private ContextMenu m_monthMenu;
        private string m_noneText = "None";
        private Pen m_penBack;
        private Pen m_penFrame;
        private Pen m_penHoverBox;
        private Rectangle m_rcLeftButton = Rectangle.Empty;
        private Rectangle m_rcMonth = Rectangle.Empty;
        private Rectangle m_rcRightButton = Rectangle.Empty;
        private Rectangle m_rcYear = Rectangle.Empty;
        private Point[] m_rightArrowPoints = new Point[3];
        private int m_scaleFactor = 1;
        private bool m_showNone;
        private bool m_showToday = true;
        private bool m_showTodayBorder = true;
        private bool m_showWeekNumbers;
        private bool m_titleVistaStyle;
        private DateTime m_today = DateTime.Today;
        private string m_todayText = "Today:";
        private Control m_yearUpDown;
        private bool m_yearUpDownNumericUpDown = true;

        public event EventHandler CloseUp;

        public event EventHandler NoneSelected;

        public event EventHandler ValueChanged;

        internal MonthCalendarEx()
        {
            this.CreateGdiObjects();
            this.m_scaleFactor = (int) (base.CreateGraphics().DpiX / 96f);
            this.InitMonthContextMenu();
            this.InitYearUpDown();
            base.Visible = true;
            base.Location = new Point(0, 0);
            base.ClientSize = new System.Drawing.Size(0xa4, (Const.BottomLabelsPos.Y + 12) + 5);
            this.m_titleVistaStyle = false;
            Imports.SetInputMode(this, Imports.InputMode.Text);
        }

        public void AddAnnuallyBoldedDate(DateTime date)
        {
            this.m_annualyBoldedDates.Add(date);
        }

        public void AddBoldedDate(DateTime date)
        {
            this.m_boldedDates.Add(date);
        }

        public void AddMonthlyBoldedDate(DateTime date)
        {
            this.m_monthlyBoldedDates.Add(date);
        }

        private void CalculateDays()
        {
            for (int i = 0; i < this.m_days.Length; i++)
            {
                this.m_days[i] = this.m_firstDate.AddDays((double) i);
            }
        }

        private void CalculateFirstDate()
        {
            this.m_firstDate = new DateTime(this.m_curSel.Year, this.m_curSel.Month, 1);
            if (this.m_firstDate.DayOfWeek == this.GetFirstDayOfWeek())
            {
                this.m_firstDate = this.m_firstDate.AddDays(-7.0);
            }
            else if (this.m_firstDate.DayOfWeek == DayOfWeek.Sunday)
            {
                this.m_firstDate = this.m_firstDate.AddDays((double) (-7 + this.GetFirstDayOfWeek()));
            }
            else
            {
                this.m_firstDate = this.m_firstDate.AddDays((double) (-(int)this.m_firstDate.DayOfWeek + this.GetFirstDayOfWeek()));
            }
        }

        private void Close()
        {
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
            this.m_brushSelBack = new SolidBrush(SystemColors.Highlight);
            this.m_brushSelText = new SolidBrush(SystemColors.HighlightText);
            this.m_penHoverBox = new Pen(SystemColors.GrayText);
            this.m_brushCaptionBack = new SolidBrush(SystemColors.ActiveCaption);
            this.m_brushCaptionText = new SolidBrush(SystemColors.ActiveCaptionText);
            this.m_fontCaption = new System.Drawing.Font("Tahoma", 8f, FontStyle.Bold);
            this.m_brushBack = new SolidBrush(SystemColors.Window);
            this.m_penBack = new Pen(SystemColors.Window);
            this.m_brushFrame = new SolidBrush(SystemColors.WindowFrame);
            this.m_penFrame = new Pen(SystemColors.WindowFrame);
        }

        private void CreateMemoryBitmap()
        {
            if (((this.m_bmp == null) || (this.m_bmp.Width != base.Width)) || (this.m_bmp.Height != base.Height))
            {
                if (this.m_bmp != null)
                {
                    this.m_bmp.Dispose();
                }
                this.m_bmp = null;
                if (this.m_graphics != null)
                {
                    this.m_graphics.Dispose();
                }
                this.m_graphics = null;
                this.m_bmp = new Bitmap(base.Width, base.Height);
                this.m_graphics = Graphics.FromImage(this.m_bmp);
                this.m_rcLeftButton = new Rectangle(this.m_scaleFactor * Const.ArrowButtonOffset.Width, this.m_scaleFactor * Const.ArrowButtonOffset.Height, this.m_scaleFactor * Const.ArrowButtonSize.Width, this.m_scaleFactor * Const.ArrowButtonSize.Height);
                this.m_rcRightButton = new Rectangle(base.ClientSize.Width - (this.m_scaleFactor * ((Const.ArrowButtonOffset.Width + Const.ArrowButtonSize.Width) + 1)), this.m_scaleFactor * Const.ArrowButtonOffset.Height, this.m_scaleFactor * Const.ArrowButtonSize.Width, this.m_scaleFactor * Const.ArrowButtonSize.Height);
                this.m_leftArrowPoints[0].X = this.m_scaleFactor * Const.ArrowPointsOffset.Width;
                this.m_leftArrowPoints[0].Y = this.m_scaleFactor * (Const.ArrowPointsOffset.Height + (Const.ArrowPointsSize.Height / 2));
                this.m_leftArrowPoints[1].X = this.m_leftArrowPoints[0].X + (this.m_scaleFactor * Const.ArrowPointsSize.Width);
                this.m_leftArrowPoints[1].Y = this.m_scaleFactor * Const.ArrowPointsOffset.Height;
                this.m_leftArrowPoints[2].X = this.m_leftArrowPoints[1].X;
                this.m_leftArrowPoints[2].Y = this.m_leftArrowPoints[1].Y + (this.m_scaleFactor * Const.ArrowPointsSize.Height);
                this.m_rightArrowPoints = (Point[]) this.m_leftArrowPoints.Clone();
                this.m_rightArrowPoints[0].X = base.Width - (this.m_scaleFactor * Const.ArrowPointsOffset.Width);
                this.m_rightArrowPoints[1].X = this.m_rightArrowPoints[2].X = this.m_rightArrowPoints[0].X - (this.m_scaleFactor * Const.ArrowPointsSize.Width);
            }
        }

        internal void Display(bool visible, int x, int y, Color backColor, Color foreColor, DateTimePickerEx parent)
        {
            this.m_DateTime = parent;
            if (visible)
            {
                this.m_captureMouse = false;
                this.m_doCloseUp = true;
                this.m_yearUpDown.Hide();
                this.BackColor = backColor;
                this.ForeColor = foreColor;
                base.Left = x;
                base.Top = y;
                base.BringToFront();
                this.m_hoverSel = this.m_curSel;
            }
            base.Visible = visible;
            if (base.Visible)
            {
                base.Focus();
            }
        }

        private void DisplayMonthMenu(int x, int y)
        {
            try
            {
                this.m_monthMenu.Show(this, new Point(x, y));
            }
            catch (NotSupportedException)
            {
            }
        }

        private void DisplayYearUpDown(int x, int y)
        {
            if (!base.Controls.Contains(this.m_yearUpDown) && ((this.Site == null) || !this.Site.DesignMode))
            {
                base.Controls.Add(this.m_yearUpDown);
            }
            this.m_yearUpDown.Text = this.m_curSel.Year.ToString();
            if (!this.m_yearUpDownNumericUpDown)
            {
                Imports.SetInputMode(this.m_yearUpDown, Imports.InputMode.Numbers);
                ((TextBox) this.m_yearUpDown).SelectionStart = 0;
                ((TextBox) this.m_yearUpDown).SelectionLength = this.m_yearUpDown.Text.Length;
            }
            this.m_yearUpDown.Left = this.m_rcYear.Left - (this.m_scaleFactor * 3);
            this.m_yearUpDown.Top = this.m_rcYear.Top - (this.m_scaleFactor * 3);
            this.m_yearUpDown.Width = this.m_rcYear.Width + (this.m_scaleFactor * 40);
            this.m_yearUpDown.Height = this.m_rcYear.Height + (this.m_scaleFactor * 6);
            this.m_yearUpDown.Show();
            this.m_yearUpDown.Focus();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_bmp != null)
                {
                    this.m_bmp.Dispose();
                }
                this.m_bmp = null;
                if (this.m_graphics != null)
                {
                    this.m_graphics.Dispose();
                }
                this.m_graphics = null;
                if (this.m_brushCur != null)
                {
                    this.m_brushCur.Dispose();
                }
                this.m_brushCur = null;
                if (this.m_brushOther != null)
                {
                    this.m_brushOther.Dispose();
                }
                this.m_brushOther = null;
                if (this.m_brushSelBack != null)
                {
                    this.m_brushSelBack.Dispose();
                }
                this.m_brushSelBack = null;
                if (this.m_brushSelText != null)
                {
                    this.m_brushSelText.Dispose();
                }
                this.m_brushSelText = null;
                if (this.m_penHoverBox != null)
                {
                    this.m_penHoverBox.Dispose();
                }
                this.m_penHoverBox = null;
                if (this.m_fontCaption != null)
                {
                    this.m_fontCaption.Dispose();
                }
                this.m_fontCaption = null;
                if (this.m_brushCaptionBack != null)
                {
                    this.m_brushCaptionBack.Dispose();
                }
                this.m_brushCaptionBack = null;
                if (this.m_brushCaptionText != null)
                {
                    this.m_brushCaptionText.Dispose();
                }
                this.m_brushCaptionText = null;
                if (this.m_brushBack != null)
                {
                    this.m_brushBack.Dispose();
                }
                this.m_brushBack = null;
                if (this.m_penBack != null)
                {
                    this.m_penBack.Dispose();
                }
                this.m_penBack = null;
                if (this.m_brushFrame != null)
                {
                    this.m_brushFrame.Dispose();
                }
                this.m_brushFrame = null;
                if (this.m_penFrame != null)
                {
                    this.m_penFrame.Dispose();
                }
                this.m_penFrame = null;
                if (this.m_monthMenu != null)
                {
                    this.m_monthMenu.Dispose();
                }
                this.m_monthMenu = null;
                if (this.m_yearUpDown != null)
                {
                    this.m_yearUpDown.Dispose();
                }
                this.m_yearUpDown = null;
                if (this.m_annualyBoldedDates != null)
                {
                    this.m_annualyBoldedDates.Clear();
                }
                this.m_annualyBoldedDates = null;
                if (this.m_boldedDates != null)
                {
                    this.m_boldedDates.Clear();
                }
                this.m_boldedDates = null;
                if (this.m_monthlyBoldedDates != null)
                {
                    this.m_monthlyBoldedDates.Clear();
                }
                this.m_monthlyBoldedDates = null;
            }
            base.Dispose(disposing);
        }

        private void DrawBottomLabels(Graphics g)
        {
            if (this.m_showToday)
            {
                g.DrawString(string.Format("{0} {1}", this.m_todayText, this.TodayDate.ToShortDateString()) + AdditionalTextTodayDate, this.m_fontCaption, this.m_brushCur, (float) (Const.BottomLabelsPos.X * this.m_scaleFactor), (float) (Const.BottomLabelsPos.Y * this.m_scaleFactor));
            }
            if (this.m_showNone)
            {
                float num = Const.BottomLabelsPos.Y + (this.m_showToday ? 17f : 0f);
                g.DrawString(this.m_noneText + AdditionalTextNoneDate, this.m_fontCaption, this.m_brushCur, (float) (Const.BottomLabelsPos.X * this.m_scaleFactor), num * this.m_scaleFactor);
            }
        }

        private void DrawCaption(Graphics g)
        {
            if (!this.m_titleVistaStyle)
            {
                g.FillRectangle(this.m_brushCaptionBack, 0, 0, base.Width, 0x1c * this.m_scaleFactor);
            }
            else
            {
                GradientFill.DrawVistaGradient(g, this.m_brushCaptionBack.Color, new Rectangle(0, 0, base.Width, 0x1c * this.m_scaleFactor), FillDirection.Vertical);
            }
            string text = this.m_curSel.ToString("MMMM yyyy");
            System.Drawing.Size size = g.MeasureString(text, this.m_fontCaption).ToSize();
            int num = (base.Width - size.Width) / 2;
            int num2 = ((0x1c * this.m_scaleFactor) - size.Height) / 2;
            g.DrawString(text, this.m_fontCaption, this.m_brushCaptionText, (float) num, (float) num2);
            text = this.m_curSel.ToString("MMMM");
            System.Drawing.Size size2 = g.MeasureString(text, this.m_fontCaption).ToSize();
            this.m_rcMonth.X = num;
            this.m_rcMonth.Y = num2;
            this.m_rcMonth.Width = size2.Width;
            this.m_rcMonth.Height = size2.Height;
            text = this.m_curSel.ToString("yyyy");
            size2 = g.MeasureString(text, this.m_fontCaption).ToSize();
            this.m_rcYear.X = (num + size.Width) - size2.Width;
            this.m_rcYear.Y = num2;
            this.m_rcYear.Width = size2.Width;
            this.m_rcYear.Height = size2.Height;
            g.FillRectangle(this.m_brushBack, this.m_rcLeftButton);
            g.DrawRectangle(this.m_penFrame, this.m_rcLeftButton);
            g.FillPolygon(this.m_brushFrame, this.m_leftArrowPoints);
            g.FillRectangle(this.m_brushBack, this.m_rcRightButton);
            g.DrawRectangle(this.m_penFrame, this.m_rcRightButton);
            g.FillPolygon(this.m_brushFrame, this.m_rightArrowPoints);
        }

        private void DrawCurSelection(Graphics g)
        {
            int dayIndex = this.GetDayIndex(this.m_curSel);
            Point dayCellPosition = this.GetDayCellPosition(dayIndex);
            this.m_graphics.FillRectangle(this.m_brushSelBack, (int) (this.m_scaleFactor * (dayCellPosition.X - 5)), (int) (this.m_scaleFactor * dayCellPosition.Y), (int) (this.m_scaleFactor * Const.DaysCell.Width), (int) (this.m_scaleFactor * Const.DaysCell.Height));
            if (this.m_curSel.Day < 10)
            {
                dayCellPosition.X += 4;
            }
            this.m_graphics.DrawString(this.m_curSel.Day.ToString(), this.GetBoldedDate(this.m_curSel) ? this.m_fontCaption : this.Font, this.m_brushSelText, (float) (this.m_scaleFactor * dayCellPosition.X), (float) (this.m_scaleFactor * dayCellPosition.Y));
        }

        private void DrawDay(Graphics g, DateTime day, bool selected)
        {
            int dayIndex = this.GetDayIndex(day);
            Point dayCellPosition = this.GetDayCellPosition(dayIndex);
            g.FillRectangle(selected ? this.m_brushSelBack : this.m_brushBack, (int) (this.m_scaleFactor * (dayCellPosition.X - 5)), (int) (this.m_scaleFactor * dayCellPosition.Y), (int) (Const.DaysCell.Width * this.m_scaleFactor), (int) (Const.DaysCell.Height * this.m_scaleFactor));
            if (day.Day < 10)
            {
                dayCellPosition.X += 4;
            }
            g.DrawString(day.Day.ToString(), this.GetBoldedDate(day) ? this.m_fontCaption : this.Font, selected ? this.m_brushSelText : this.m_brushCur, (float) (dayCellPosition.X * this.m_scaleFactor), (float) (dayCellPosition.Y * this.m_scaleFactor));
        }

        private void DrawDays(Graphics g)
        {
            if ((this.m_curSel.Month != this.m_curMonth) || (this.m_curSel.Year != this.m_curYear))
            {
                this.CalculateDays();
                this.m_curMonth = this.m_curSel.Month;
                this.m_curYear = this.m_curSel.Year;
            }
            Point daysGrid = Const.DaysGrid;
            daysGrid.X += this.m_showWeekNumbers ? 0x18 : 0;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    DateTime date = this.m_days[(i * 7) + j];
                    int num = (date.Day < 10) ? 4 : 0;
                    g.DrawString(date.Day.ToString(), this.GetBoldedDate(date) ? this.m_fontCaption : this.Font, (date.Month == this.m_curMonth) ? this.m_brushCur : this.m_brushOther, (float) ((daysGrid.X + num) * this.m_scaleFactor), (float) (daysGrid.Y * this.m_scaleFactor));
                    daysGrid.X += Const.DaysCell.Width;
                }
                daysGrid.X = Const.DaysGrid.X + (this.m_showWeekNumbers ? 0x18 : 0);
                daysGrid.Y += Const.DaysCell.Height + 1;
            }
        }

        private void DrawDaysOfWeek(Graphics g)
        {
            string weekDayNames = this.GetWeekDayNames();
            Point point = new Point((Const.DaysGrid.X + 3) + (this.m_showWeekNumbers ? 0x18 : 0), 0x1c);
            foreach (char ch in weekDayNames)
            {
                g.DrawString(ch.ToString(), this.m_fontCaption, this.m_brushCaptionBack, (float) (this.m_scaleFactor * point.X), (float) (this.m_scaleFactor * point.Y));
                point.X += Const.DaysCell.Width;
            }
            g.DrawLine(this.m_penFrame, (int) (this.m_scaleFactor * (Const.DaysGrid.X + (this.m_showWeekNumbers ? 0x18 : 0))), (int) ((this.m_scaleFactor * Const.DaysGrid.Y) - 1), (int) (this.m_scaleFactor * (base.Width - Const.DaysGrid.X)), (int) (this.m_scaleFactor * (Const.DaysGrid.Y - 1)));
        }

        private void DrawHoverSelection(Graphics g, DateTime date, bool draw)
        {
            int dayIndex = this.GetDayIndex(date);
            if ((dayIndex >= 0) && (dayIndex < this.m_days.Length))
            {
                Point dayCellPosition = this.GetDayCellPosition(dayIndex);
                g.DrawRectangle(draw ? this.m_penHoverBox : this.m_penBack, (int) (this.m_scaleFactor * (dayCellPosition.X - 5)), (int) (this.m_scaleFactor * dayCellPosition.Y), (int) (this.m_scaleFactor * Const.DaysCell.Width), (int) (this.m_scaleFactor * Const.DaysCell.Height));
            }
        }

        private void DrawNumbersOfWeek(Graphics g)
        {
            if (this.m_showWeekNumbers)
            {
                if ((this.m_curSel.Month != this.m_curMonth) || (this.m_curSel.Year != this.m_curYear))
                {
                    this.CalculateDays();
                    this.m_curMonth = this.m_curSel.Month;
                    this.m_curYear = this.m_curSel.Year;
                }
                Point weeksGrid = Const.WeeksGrid;
                for (int i = 0; i < 6; i++)
                {
                    DateTime time = this.m_days[(i * 7) + 6];
                    int num3 = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(time, this.CalendarWeekRule, this.GetFirstDayOfWeek());
                    int num = (num3 < 10) ? 4 : 0;
                    g.DrawString(num3.ToString(), this.Font, (time.Month == this.m_curMonth) ? this.m_brushCur : this.m_brushOther, (float) (this.m_scaleFactor * (weeksGrid.X + num)), (float) (this.m_scaleFactor * weeksGrid.Y));
                    weeksGrid.Y += Const.DaysCell.Height + 1;
                }
                g.DrawLine(this.m_penFrame, (int) (this.m_scaleFactor * 0x18), (int) (this.m_scaleFactor * (Const.WeeksGrid.Y + 3)), (int) (this.m_scaleFactor * 0x18), (int) (this.m_scaleFactor * (Const.BottomLabelsPos.Y - 3)));
            }
        }

        private void DrawTodaySelection(Graphics g)
        {
            if (this.m_showTodayBorder)
            {
                int dayIndex = this.GetDayIndex(this.TodayDate);
                if (((dayIndex >= 0) && (dayIndex < this.m_days.Length)) && (this.TodayDate.Month == this.m_curSel.Month))
                {
                    Point dayCellPosition = this.GetDayCellPosition(dayIndex);
                    g.DrawRectangle(this.m_penFrame, (int) (this.m_scaleFactor * (dayCellPosition.X - 5)), (int) (this.m_scaleFactor * dayCellPosition.Y), (int) (this.m_scaleFactor * Const.DaysCell.Width), (int) (this.m_scaleFactor * Const.DaysCell.Height));
                    g.DrawRectangle(this.m_penFrame, (int) (this.m_scaleFactor * (dayCellPosition.X - 4)), (int) (this.m_scaleFactor * (dayCellPosition.Y + 1)), (int) (this.m_scaleFactor * (Const.DaysCell.Width - 2)), (int) (this.m_scaleFactor * (Const.DaysCell.Height - 2)));
                }
            }
        }

        private bool GetBoldedDate(DateTime date)
        {
            if (this.m_boldedDates != null)
            {
                foreach (DateTime time in this.m_boldedDates)
                {
                    if (date == time)
                    {
                        return true;
                    }
                }
            }
            if (this.m_monthlyBoldedDates != null)
            {
                foreach (DateTime time2 in this.m_monthlyBoldedDates)
                {
                    if (date.Day == time2.Day)
                    {
                        return true;
                    }
                }
            }
            if (this.m_annualyBoldedDates != null)
            {
                foreach (DateTime time3 in this.m_annualyBoldedDates)
                {
                    if ((date.Day == time3.Day) && (date.Month == time3.Month))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Point GetDayCellPosition(int index)
        {
            return new Point((Const.DaysGrid.X + (this.m_showWeekNumbers ? 0x18 : 0)) + ((index % 7) * Const.DaysCell.Width), Const.DaysGrid.Y + ((index / 7) * (Const.DaysCell.Height + 1)));
        }

        private int GetDayIndex(DateTime date)
        {
            return (int) date.Subtract(this.m_firstDate).TotalDays;
        }

        private int GetDayIndex(int x, int y)
        {
            Rectangle rectangle = new Rectangle(0, this.m_scaleFactor * Const.DaysGrid.Y, (this.m_scaleFactor * 7) * Const.DaysCell.Width, this.m_scaleFactor * Const.BottomLabelsPos.Y);
            if (!rectangle.Contains(x - (this.m_showWeekNumbers ? (this.m_scaleFactor * 0x18) : 0), y))
            {
                return -1;
            }
            return (((x - (this.m_showWeekNumbers ? (this.m_scaleFactor * 0x18) : 0)) / (this.m_scaleFactor * Const.DaysCell.Width)) + (((y - (this.m_scaleFactor * Const.DaysGrid.Y)) / (this.m_scaleFactor * (Const.DaysCell.Height + 1))) * 7));
        }

        private DayOfWeek GetFirstDayOfWeek()
        {
            if (this.m_firstDayOfWeek == Resco.Controls.DetailView.DetailViewInternal.Day.Default)
            {
                return DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;
            }
            return (DayOfWeek) this.m_firstDayOfWeek;
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
                    daysOfWeek = daysOfWeek + DateTimeFormatInfo.CurrentInfo.GetDayName((DayOfWeek) j).Substring(startIndex, 1);
                }
            }
            int num3 = daysOfWeek.Length / 7;
            string str2 = "";
            for (int i = 0; i < daysOfWeek.Length; i++)
            {
                str2 = str2 + daysOfWeek[(i + ((int)(this.GetFirstDayOfWeek()) * num3)) % daysOfWeek.Length];
            }
            return str2;
        }

        public void Hide()
        {
            this.m_doHide = true;
            base.Hide();
            if (this.m_DateTime != null)
            {
                this.m_DateTime.Focus();
            }
            this.m_doHide = false;
        }

        private void InitMonthContextMenu()
        {
            this.m_monthMenu = new ContextMenu();
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
        }

        protected override void OnKeyUp(KeyEventArgs e)
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

                case Keys.Return:
                    if (this.m_doCloseUp)
                    {
                        this.Hide();
                    }
                    break;
            }
            if (e.KeyCode == KeyPreviousMonth)
            {
                if (Imports.IsSmartphone)
                {
                    this.UpdateCurSel(this.m_curSel.AddMonths(-1));
                }
            }
            else if (e.KeyCode == KeyNextMonth)
            {
                if (Imports.IsSmartphone)
                {
                    this.UpdateCurSel(this.m_curSel.AddMonths(1));
                }
            }
            else if (e.KeyCode == KeyYearSelection)
            {
                if (Imports.IsSmartphone)
                {
                    this.DisplayYearUpDown(0, 0);
                }
            }
            else if (e.KeyCode == KeyTodayDate)
            {
                this.UpdateCurSel(this.TodayDate);
            }
            else if (e.KeyCode == KeyNoneDate)
            {
                this.UpdateCurSel(this.TodayDate, true);
            }
            if (num != 0)
            {
                this.m_isNone = false;
                DateTime newDate = this.m_curSel.AddDays((double) num);
                if (this.m_curSel.Month != newDate.Month)
                {
                    this.UpdateCurSel(newDate);
                }
                else
                {
                    Graphics g = base.CreateGraphics();
                    this.DrawDay(g, this.m_curSel, false);
                    this.DrawDay(g, newDate, true);
                    g.Dispose();
                    this.m_curSel = newDate;
                    this.UpdateHoverCell(this.GetDayIndex(this.m_curSel));
                    if (this.ValueChanged != null)
                    {
                        this.ValueChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                if (((this.m_yearUpDown == null) || !this.m_yearUpDown.Focused) && this.m_doCloseUp)
                {
                    this.Close();
                }
                if (!this.m_doHide && (this.m_DateTime != null))
                {
                    this.m_DateTime._OnLostFocus(e);
                }
                else
                {
                    base.OnLostFocus(e);
                }
            }
        }

        private void OnMonthMenuClick(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                DateTime newDate = DateTime.Parse(string.Format("{0}, {1} {2}", item.Text, this.m_curSel.Day, this.m_curSel.Year));
                this.UpdateCurSel(newDate);
            }
        }

        private void OnMonthMenuPopup(object sender, EventArgs e)
        {
            foreach (MenuItem item in this.m_monthMenu.MenuItems)
            {
                item.Checked = false;
            }
            if ((this.m_curMonth > 0) && (this.m_curMonth <= 12))
            {
                this.m_monthMenu.MenuItems[this.m_curMonth - 1].Checked = true;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.m_yearUpDown.Visible && !this.m_yearUpDown.Bounds.Contains(e.X, e.Y))
            {
                this.OnYearUpDownValueChanged(null, EventArgs.Empty);
                this.m_doCloseUp = false;
                this.m_yearUpDown.Hide();
                base.Focus();
                Application.DoEvents();
                this.m_doCloseUp = true;
            }
            if (this.m_rcLeftButton.Contains(e.X, e.Y))
            {
                this.UpdateCurSel(this.m_curSel.AddMonths(-1));
            }
            else if (this.m_rcRightButton.Contains(e.X, e.Y))
            {
                this.UpdateCurSel(this.m_curSel.AddMonths(1));
            }
            else if (this.m_rcMonth.Contains(e.X, e.Y))
            {
                this.DisplayMonthMenu(e.X, e.Y);
            }
            else if (this.m_rcYear.Contains(e.X, e.Y))
            {
                this.DisplayYearUpDown(e.X, e.Y);
            }
            else if (e.Y >= (this.m_scaleFactor * Const.BottomLabelsPos.Y))
            {
                if (this.m_showNone && (e.Y >= (this.m_scaleFactor * ((Const.BottomLabelsPos.Y + 12) + 5))))
                {
                    this.UpdateCurSel(this.TodayDate, true);
                    if (this.m_doCloseUp)
                    {
                        this.Hide();
                    }
                }
                else
                {
                    this.UpdateCurSel(this.TodayDate);
                    if (this.m_doCloseUp)
                    {
                        this.Hide();
                    }
                }
            }
            else
            {
                this.m_captureMouse = true;
                this.UpdateHoverCell(e.X, e.Y);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.m_captureMouse)
            {
                this.UpdateHoverCell(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.m_captureMouse)
            {
                this.m_captureMouse = false;
                int dayIndex = this.GetDayIndex(this.m_hoverSel);
                if ((dayIndex >= 0) && (dayIndex < this.m_days.Length))
                {
                    this.UpdateCurSel(this.m_hoverSel);
                    if (this.m_doCloseUp)
                    {
                        this.Hide();
                    }
                }
                else
                {
                    this.UpdateCurSel(this.m_curSel);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.CreateMemoryBitmap();
            this.CalculateFirstDate();
            this.m_graphics.Clear(this.BackColor);
            this.DrawCaption(this.m_graphics);
            this.DrawDaysOfWeek(this.m_graphics);
            this.DrawDays(this.m_graphics);
            this.DrawNumbersOfWeek(this.m_graphics);
            if (!this.m_isNone)
            {
                this.DrawCurSelection(this.m_graphics);
                this.DrawHoverSelection(this.m_graphics, this.m_hoverSel, true);
            }
            this.DrawTodaySelection(this.m_graphics);
            this.DrawBottomLabels(this.m_graphics);
            this.m_graphics.DrawRectangle(this.m_penFrame, 0, 0, base.Width - 1, base.Height - 1);
            e.Graphics.DrawImage(this.m_bmp, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            this.SetSize();
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
                this.m_yearUpDown.Hide();
                this.m_doCloseUp = doCloseUp;
                base.Focus();
                if (!this.m_yearUpDownNumericUpDown)
                {
                    Imports.SetInputMode(this.m_yearUpDown, Imports.InputMode.Text);
                }
            }
        }

        private void OnYearUpDownLostFocus(object sender, EventArgs e)
        {
            if ((Environment.OSVersion.Platform == PlatformID.WinCE) && (!this.Focused && this.m_doCloseUp))
            {
                this.Close();
            }
        }

        private void OnYearUpDownValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.m_yearUpDown.Visible)
                {
                    DateTime newDate = new DateTime(Convert.ToInt32(this.m_yearUpDownNumericUpDown ? ((NumericUpDown) this.m_yearUpDown).Value : Convert.ToInt32(this.m_yearUpDown.Text)), this.m_curSel.Month, this.m_curSel.Day);
                    this.UpdateCurSel(newDate);
                }
            }
            catch
            {
                if (this.m_yearUpDownNumericUpDown)
                {
                    ((NumericUpDown) this.m_yearUpDown).Value = this.m_curSel.Year;
                }
                else
                {
                    this.m_yearUpDown.Text = Convert.ToString(this.m_curSel.Year);
                }
            }
        }

        public void RemoveAllAnnuallyBoldedDates()
        {
            this.m_annualyBoldedDates.Clear();
        }

        public void RemoveAllBoldedDates()
        {
            this.m_boldedDates.Clear();
        }

        public void RemoveAllMonthlyBoldedDates()
        {
            this.m_monthlyBoldedDates.Clear();
        }

        public void RemoveAnnuallyBoldedDate(DateTime date)
        {
            int count = this.m_annualyBoldedDates.Count;
            for (int i = 0; i < count; i++)
            {
                DateTime time = (DateTime) this.m_annualyBoldedDates[i];
                if ((date.Day == time.Day) && (date.Month == time.Month))
                {
                    this.m_annualyBoldedDates.RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveBoldedDate(DateTime date)
        {
            for (int i = 0; i < this.m_boldedDates.Count; i++)
            {
                DateTime time = (DateTime) this.m_boldedDates[i];
                if (((date.Day == time.Day) && (date.Month == time.Month)) && (date.Year == time.Year))
                {
                    this.m_boldedDates.RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveMonthlyBoldedDate(DateTime date)
        {
            int count = this.m_monthlyBoldedDates.Count;
            for (int i = 0; i < count; i++)
            {
                DateTime time = (DateTime) this.m_monthlyBoldedDates[i];
                if (date.Day == time.Day)
                {
                    this.m_monthlyBoldedDates.RemoveAt(i);
                    return;
                }
            }
        }

        private void SetSize()
        {
            int num = 0xa4 + (this.m_showWeekNumbers ? 0x18 : 0);
            int num2 = ((Const.BottomLabelsPos.Y + (this.m_showToday ? 12 : 0)) + (this.m_showNone ? 0x11 : 0)) + 5;
            if ((this.Site != null) && this.Site.DesignMode)
            {
                this.m_scaleFactor = ((base.ClientSize.Width > num) || (base.ClientSize.Height > num2)) ? 2 : 1;
            }
            base.ClientSize = new System.Drawing.Size(this.m_scaleFactor * num, this.m_scaleFactor * num2);
        }

        protected virtual bool ShouldSerializeAnnuallyBoldedDates()
        {
            return (this.m_annualyBoldedDates.Count > 0);
        }

        protected virtual bool ShouldSerializeBoldedDates()
        {
            return (this.m_boldedDates.Count > 0);
        }

        protected virtual bool ShouldSerializeCalendarWeekRule()
        {
            if (this.m_calendarWeekRule == DateTimeFormatInfo.CurrentInfo.CalendarWeekRule)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeFirstDayOfWeek()
        {
            if (this.m_firstDayOfWeek == Resco.Controls.DetailView.DetailViewInternal.Day.Default)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeMonthlyBoldedDates()
        {
            return (this.m_monthlyBoldedDates.Count > 0);
        }

        protected virtual bool ShouldSerializeSize()
        {
            return false;
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

        public void UpdateBoldedDates()
        {
            base.Invalidate();
        }

        private void UpdateCurSel(DateTime newDate)
        {
            this.UpdateCurSel(newDate, false);
        }

        private void UpdateCurSel(DateTime newDate, bool isNone)
        {
            bool flag = !isNone && ((this.m_curSel != newDate) || this.m_isNone);
            bool flag2 = isNone && !this.m_isNone;
            this.m_isNone = isNone;
            if ((newDate >= this.MinDate) && (newDate <= this.MaxDate))
            {
                this.m_curSel = newDate;
            }
            else if (this.m_yearUpDownNumericUpDown)
            {
                ((NumericUpDown) this.m_yearUpDown).Value = this.m_curSel.Year;
            }
            else
            {
                this.m_yearUpDown.Text = Convert.ToString(this.m_curSel.Year);
            }
            this.m_hoverSel = this.m_curSel;
            base.Invalidate();
            if (flag && (this.ValueChanged != null))
            {
                this.ValueChanged(this, EventArgs.Empty);
            }
            if (flag2 && (this.NoneSelected != null))
            {
                this.NoneSelected(this, EventArgs.Empty);
            }
        }

        private void UpdateHoverCell(int newIndex)
        {
            if ((newIndex < 0) || (newIndex >= this.m_days.Length))
            {
                Graphics g = base.CreateGraphics();
                this.DrawHoverSelection(g, this.m_hoverSel, false);
                this.DrawTodaySelection(g);
                g.Dispose();
                this.m_hoverSel = DateTime.MinValue;
            }
            else if (this.m_hoverSel != this.m_days[newIndex])
            {
                Graphics graphics2 = base.CreateGraphics();
                this.DrawHoverSelection(graphics2, this.m_hoverSel, false);
                this.DrawHoverSelection(graphics2, this.m_days[newIndex], true);
                this.DrawTodaySelection(graphics2);
                graphics2.Dispose();
                this.m_hoverSel = this.m_days[newIndex];
            }
        }

        private void UpdateHoverCell(int x, int y)
        {
            int dayIndex = this.GetDayIndex(x, y);
            this.UpdateHoverCell(dayIndex);
        }

        public DateTime[] AnnuallyBoldedDates
        {
            get
            {
                DateTime[] timeArray = new DateTime[this.m_annualyBoldedDates.Count];
                for (int i = 0; i < this.m_annualyBoldedDates.Count; i++)
                {
                    timeArray[i] = (DateTime) this.m_annualyBoldedDates[i];
                }
                return timeArray;
            }
            set
            {
                this.m_annualyBoldedDates.Clear();
                if ((value != null) && (value.Length > 0))
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        this.m_annualyBoldedDates.Add(value[i]);
                    }
                }
            }
        }

        public override Color BackColor
        {
            get
            {
                if (this.m_brushBack == null)
                {
                    return SystemColors.Window;
                }
                return this.m_brushBack.Color;
            }
            set
            {
                if (value != this.m_brushBack.Color)
                {
                    this.m_brushBack.Color = value;
                    base.Invalidate();
                }
            }
        }

        public DateTime[] BoldedDates
        {
            get
            {
                DateTime[] timeArray = new DateTime[this.m_boldedDates.Count];
                for (int i = 0; i < this.m_boldedDates.Count; i++)
                {
                    timeArray[i] = (DateTime) this.m_boldedDates[i];
                }
                return timeArray;
            }
            set
            {
                this.m_boldedDates.Clear();
                if ((value != null) && (value.Length > 0))
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        this.m_boldedDates.Add(value[i]);
                    }
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
                    base.Invalidate();
                }
            }
        }

        public Resco.Controls.DetailView.DetailViewInternal.Day FirstDayOfWeek
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
                    this.CalculateDays();
                    base.Invalidate();
                }
            }
        }

        public override bool Focused
        {
            get
            {
                return (base.Focused || ((this.m_yearUpDown != null) && this.m_yearUpDown.Focused));
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
                    if (value == null)
                    {
                        this.m_fontCaption = new System.Drawing.Font(base.Font.Name, base.Font.Size, FontStyle.Bold);
                    }
                    else
                    {
                        this.m_fontCaption = new System.Drawing.Font(value.Name, value.Size, FontStyle.Bold);
                    }
                    base.Invalidate();
                }
            }
        }

        public override Color ForeColor
        {
            get
            {
                if (this.m_brushCur == null)
                {
                    return SystemColors.WindowText;
                }
                return this.m_brushCur.Color;
            }
            set
            {
                if (value != this.m_brushCur.Color)
                {
                    this.m_brushCur.Color = value;
                    base.Invalidate();
                }
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
                if (this.m_showNone && (this.m_isNone != value))
                {
                    this.UpdateCurSel(this.m_today, value);
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
                    if (value > DateTimePickerEx.MaxDateTime)
                    {
                        throw new ArgumentException("DateTimePickerMaxDate", DateTimePickerEx.MaxDateTime.ToString("G"));
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
                    if (value < DateTimePickerEx.MinDateTime)
                    {
                        throw new ArgumentException("DateTimePickerMinDate", DateTimePickerEx.MinDateTime.ToString("G"));
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
                DateTime[] timeArray = new DateTime[this.m_monthlyBoldedDates.Count];
                for (int i = 0; i < this.m_monthlyBoldedDates.Count; i++)
                {
                    timeArray[i] = (DateTime) this.m_monthlyBoldedDates[i];
                }
                return timeArray;
            }
            set
            {
                this.m_monthlyBoldedDates.Clear();
                if ((value != null) && (value.Length > 0))
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        this.m_monthlyBoldedDates.Add(value[i]);
                    }
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
                    base.Invalidate();
                }
            }
        }

        public bool ShowNone
        {
            get
            {
                return this.m_showNone;
            }
            set
            {
                if (value != this.m_showNone)
                {
                    if (!this.m_showNone && this.IsNone)
                    {
                        this.IsNone = false;
                    }
                    this.m_showNone = value;
                    this.SetSize();
                    base.Invalidate();
                }
            }
        }

        public bool ShowToday
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
                    this.SetSize();
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
                    base.Invalidate();
                }
            }
        }

        public bool ShowWeekNumbers
        {
            get
            {
                return this.m_showWeekNumbers;
            }
            set
            {
                if (value != this.m_showWeekNumbers)
                {
                    this.m_showWeekNumbers = value;
                    this.SetSize();
                    base.Invalidate();
                }
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return base.Size;
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
                    this.m_brushCaptionBack.Color = value;
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
                    this.m_brushCaptionText.Color = value;
                    base.Invalidate();
                }
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
                    this.m_brushOther.Color = value;
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
                if ((value != this.m_curSel) || this.IsNone)
                {
                    this.UpdateCurSel(value);
                }
            }
        }

        private class Const
        {
            public static Size ArrowButtonOffset = new Size(6, 6);
            public static Size ArrowButtonSize = new Size(20, 15);
            public static Size ArrowPointsOffset = new Size(13, 9);
            public static Size ArrowPointsSize = new Size(5, 10);
            public const int BottomLabelHeight = 12;
            public static Point BottomLabelsPos = new Point(6, 0x87);
            public const int BottomLabelsSpace = 5;
            public const int CaptionHeight = 0x1c;
            public const int ControlWidth = 0xa4;
            public static Size DaysCell = new Size(0x17, 14);
            public static Point DaysGrid = new Point(6, 0x2b);
            public const string FontName = "Tahoma";
            public const int FontSize = 8;
            public const int NoneLabelHeight = 12;
            public const int NumCols = 7;
            public const int NumRows = 6;
            public static Point WeeksGrid = new Point(6, 0x2b);
            public const int WeekWidth = 0x18;
        }
    }
}

