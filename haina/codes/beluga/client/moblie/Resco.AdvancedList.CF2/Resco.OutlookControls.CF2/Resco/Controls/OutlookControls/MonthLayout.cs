namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;

    internal class MonthLayout : Layout
    {
        private CalendarLayout _parent;

        public MonthLayout(CalendarLayout parent, int x, int y, int width, int height, int cellCountX, int cellCountY) : base(x, y, width, height, cellCountX, cellCountY)
        {
            this._parent = parent;
            this.Refresh();
        }

        protected override Rectangle CalculateInnerBounds()
        {
            int x = base._x + this._parent.WeekNumbersWidth;
            int width = base._width - this._parent.WeekNumbersWidth;
            int y = (base._y + this._parent.MonthCaptionActualHeight) + this._parent.WeekLettersHeight;
            return new Rectangle(x, y, width, base._height - (this._parent.MonthCaptionActualHeight + this._parent.WeekLettersHeight));
        }

        public Rectangle MonthCaptionBounds
        {
            get
            {
                return new Rectangle(base._x, base._y, base._width, this._parent.MonthCaptionActualHeight);
            }
        }
    }
}

