namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections;
    using System.Drawing;

    internal class CalendarLayout : Layout
    {
        private Rectangle _leftArrowBounds;
        private Point[] _leftArrowTriangle;
        private int _monthCaptionHeight;
        private ArrayList _monthLayouts;
        private Rectangle _rightArrowBounds;
        private Point[] _rightArrowTriangle;
        private bool _showDaysGrid;
        private bool _showMonthCaption;
        private bool _showNoneBar;
        private bool _showTodayBar;
        private bool _showWeekLetters;
        private bool _showWeekNumbers;

        public CalendarLayout() : base(0, 0, 160, 160, 1, 1)
        {
            this._monthLayouts = new ArrayList();
            this._showMonthCaption = true;
            this._showTodayBar = true;
            this._showWeekLetters = true;
            this._leftArrowTriangle = new Point[3];
            this._rightArrowTriangle = new Point[3];
            this._monthCaptionHeight = 0x1c;
            this.SetDimension(base._cellCountX, base._cellCountY);
        }

        private void CalculateArrowButtons()
        {
            int num = (int) Math.Round((double) (this.MonthCaptionActualHeight * 0.5357));
            int width = (int) Math.Round((double) (num * 1.33));
            int x = (this.MonthCaptionActualHeight - num) / 2;
            if (num < 10)
            {
                int num4 = Math.Min((int) (10 - num), (int) (2 * x));
                num += num4;
                num = Math.Min(num, this.MonthCaptionActualHeight - 1);
                x -= (int) Math.Ceiling(((double) num4) / 2.0);
                width = (int) Math.Round((double) (num * 1.33));
            }
            this._leftArrowBounds = new Rectangle(x, x, width, num);
            this._rightArrowBounds = new Rectangle(((base._width - 1) - x) - width, x, width, num);
            int num5 = (int) Math.Round((double) (width * 0.25));
            int num6 = 2 * num5;
            int num7 = (width - num5) / 2;
            int num8 = (int) Math.Ceiling(((double) (num - num6)) / 2.0);
            this._leftArrowTriangle[0].X = x + num7;
            this._leftArrowTriangle[0].Y = (x + num8) + (num6 / 2);
            this._leftArrowTriangle[1].X = this._leftArrowTriangle[0].X + num5;
            this._leftArrowTriangle[1].Y = x + num8;
            this._leftArrowTriangle[2].X = this._leftArrowTriangle[1].X;
            this._leftArrowTriangle[2].Y = this._leftArrowTriangle[1].Y + num6;
            this._rightArrowTriangle = (Point[]) this._leftArrowTriangle.Clone();
            this._rightArrowTriangle[0].X = base._width - this._leftArrowTriangle[0].X;
            this._rightArrowTriangle[1].X = base._width - this._leftArrowTriangle[1].X;
            this._rightArrowTriangle[2].X = this._rightArrowTriangle[1].X;
        }

        protected override Rectangle CalculateInnerBounds()
        {
            int width = base._width;
            int x = base._x;
            int height = base._height - (this.NoneBarHeight + this.TodayBarHeight);
            return new Rectangle(x, base._y, width, height);
        }

        public MonthLayout GetMonthLayout(int index)
        {
            return (MonthLayout) this._monthLayouts[index];
        }

        public override void Resize(int x, int y, int width, int height)
        {
            base.Resize(x, y, width, height);
            for (int i = 0; i < (base._cellCountX * base._cellCountY); i++)
            {
                Rectangle rectangle = base.CalculateCellBounds(i);
                ((Layout) this._monthLayouts[i]).Resize(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }
            this.CalculateArrowButtons();
        }

        public void SetDimension(int cellCountX, int cellCountY)
        {
            base._cellCountX = cellCountX;
            base._cellCountY = cellCountY;
            this._monthLayouts.Clear();
            for (int i = 0; i < (base._cellCountX * base._cellCountY); i++)
            {
                Rectangle rectangle = base.CalculateCellBounds(i);
                this._monthLayouts.Add(new MonthLayout(this, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 7, 6));
                ((Layout) this._monthLayouts[i]).ShowGrid = this._showDaysGrid;
            }
            this.Refresh();
        }

        public Rectangle LeftArrowBounds
        {
            get
            {
                return this._leftArrowBounds;
            }
        }

        public Point[] LeftArrowTriangle
        {
            get
            {
                return this._leftArrowTriangle;
            }
        }

        public int MonthCaptionActualHeight
        {
            get
            {
                if (!this._showMonthCaption)
                {
                    return 0;
                }
                if (this._monthCaptionHeight >= 0)
                {
                    return this._monthCaptionHeight;
                }
                return base._fontHeight;
            }
        }

        public int MonthCaptionHeight
        {
            get
            {
                return this._monthCaptionHeight;
            }
            set
            {
                this._monthCaptionHeight = value;
                this.Refresh();
            }
        }

        public int NoneBarHeight
        {
            get
            {
                if (!this.ShowNoneBar)
                {
                    return 0;
                }
                return base._fontHeight;
            }
        }

        public Point NoneBarPos
        {
            get
            {
                return new Point(6, base.InnerBounds.Height + this.TodayBarHeight);
            }
        }

        public Rectangle RightArrowBounds
        {
            get
            {
                return this._rightArrowBounds;
            }
        }

        public Point[] RightArrowTriangle
        {
            get
            {
                return this._rightArrowTriangle;
            }
        }

        public bool ShowDaysGrid
        {
            get
            {
                return this._showDaysGrid;
            }
            set
            {
                foreach (MonthLayout layout in this._monthLayouts)
                {
                    layout.ShowGrid = value;
                }
                this._showDaysGrid = value;
                this.Refresh();
            }
        }

        public bool ShowMonthCaption
        {
            get
            {
                return this._showMonthCaption;
            }
            set
            {
                this._showMonthCaption = value;
                this.Refresh();
            }
        }

        public bool ShowNoneBar
        {
            get
            {
                return this._showNoneBar;
            }
            set
            {
                this._showNoneBar = value;
                this.Refresh();
            }
        }

        public bool ShowTodayBar
        {
            get
            {
                return this._showTodayBar;
            }
            set
            {
                this._showTodayBar = value;
                this.Refresh();
            }
        }

        public bool ShowWeekLetters
        {
            get
            {
                return this._showWeekLetters;
            }
            set
            {
                this._showWeekLetters = value;
                this.Refresh();
            }
        }

        public bool ShowWeekNumbers
        {
            get
            {
                return this._showWeekNumbers;
            }
            set
            {
                this._showWeekNumbers = value;
                this.Refresh();
            }
        }

        public int TodayBarHeight
        {
            get
            {
                if (!this.ShowTodayBar)
                {
                    return 0;
                }
                return base._fontHeight;
            }
        }

        public Point TodayBarPos
        {
            get
            {
                return new Point(6, base.InnerBounds.Height);
            }
        }

        public int WeekLettersHeight
        {
            get
            {
                if (!this._showWeekLetters)
                {
                    return 0;
                }
                return (base._fontHeight - 1);
            }
        }

        public int WeekNumbersWidth
        {
            get
            {
                if (!this._showWeekNumbers)
                {
                    return 0;
                }
                return (base._fontWidth + 4);
            }
        }
    }
}

