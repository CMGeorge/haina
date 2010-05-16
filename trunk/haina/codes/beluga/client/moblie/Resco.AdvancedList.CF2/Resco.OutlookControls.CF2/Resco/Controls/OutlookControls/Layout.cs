namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections;
    using System.Drawing;

    internal abstract class Layout
    {
        protected int _cellCountX;
        protected int _cellCountY;
        protected ArrayList _cellsBounds = new ArrayList();
        protected int _fontHeight;
        protected int _fontWidth;
        protected int _height;
        protected Rectangle _innerBounds;
        protected bool _showGrid;
        protected int _width;
        protected int _x;
        protected int _y;

        public Layout(int x, int y, int width, int height, int cellCountX, int cellCountY)
        {
            this._x = x;
            this._y = y;
            this._width = width;
            this._height = height;
            this._cellCountX = cellCountX;
            this._cellCountY = cellCountY;
        }

        protected Rectangle CalculateCellBounds(int index)
        {
            Rectangle rectangle = this.CalculateInnerBounds();
            if (this._showGrid)
            {
                rectangle.Width--;
                rectangle.Height--;
            }
            int num = index % this._cellCountX;
            int num2 = index / this._cellCountX;
            int num3 = rectangle.Width / this._cellCountX;
            int num4 = rectangle.Height / this._cellCountY;
            int num5 = rectangle.Width % this._cellCountX;
            int num6 = rectangle.Height % this._cellCountY;
            int num7 = ((num5 > num) ? 1 : 0) + (this._showGrid ? 1 : 0);
            int num8 = ((num6 > num2) ? 1 : 0) + (this._showGrid ? 1 : 0);
            int num9 = Math.Min(num5, num);
            int num10 = Math.Min(num6, num2);
            int num11 = rectangle.X + (num * num3);
            int num12 = rectangle.Y + (num2 * num4);
            return new Rectangle(num11 + num9, num12 + num10, num3 + num7, num4 + num8);
        }

        protected abstract Rectangle CalculateInnerBounds();
        public void FontChange(Graphics g, Font font)
        {
            SizeF ef = g.MeasureString("00", font);
            this._fontWidth = (int) Math.Ceiling((double) ef.Width);
            this._fontHeight = (int) Math.Ceiling((double) ef.Height);
            this.Refresh();
        }

        public Rectangle GetCellBounds(int index)
        {
            return (Rectangle) this._cellsBounds[index];
        }

        protected virtual void Refresh()
        {
            this.Resize(this._x, this._y, this._width, this._height);
        }

        public virtual void Resize(int x, int y, int width, int height)
        {
            this._x = x;
            this._y = y;
            this._width = width;
            this._height = height;
            this._innerBounds = this.CalculateInnerBounds();
            this._cellsBounds.Clear();
            for (int i = 0; i < (this._cellCountX * this._cellCountY); i++)
            {
                this._cellsBounds.Add(this.CalculateCellBounds(i));
            }
        }

        public int CellCountX
        {
            get
            {
                return this._cellCountX;
            }
        }

        public int CellCountY
        {
            get
            {
                return this._cellCountY;
            }
        }

        public int Height
        {
            get
            {
                return this._height;
            }
        }

        public Rectangle InnerBounds
        {
            get
            {
                return this._innerBounds;
            }
        }

        public bool ShowGrid
        {
            get
            {
                return this._showGrid;
            }
            set
            {
                this._showGrid = value;
                this.Refresh();
            }
        }

        public int Width
        {
            get
            {
                return this._width;
            }
        }

        public int X
        {
            get
            {
                return this._x;
            }
        }

        public int Y
        {
            get
            {
                return this._y;
            }
        }
    }
}

