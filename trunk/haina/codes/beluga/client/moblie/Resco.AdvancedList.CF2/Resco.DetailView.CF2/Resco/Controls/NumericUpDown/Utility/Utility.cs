namespace Resco.Controls.NumericUpDown.Utility
{
    using Resco.Controls.NumericUpDown;
    using System;
    using System.Drawing;

    internal class Utility
    {
        private SolidBrush m_brush;
        private Pen m_pen;
        private SizeF m_scaleFactor = new SizeF(1f, 1f);

        internal Utility()
        {
        }

        internal void Dispose()
        {
            if (this.m_brush != null)
            {
                this.m_brush.Dispose();
            }
            this.m_brush = null;
            if (this.m_pen != null)
            {
                this.m_pen.Dispose();
            }
            this.m_pen = null;
        }

        internal void DrawAdornment(Graphics gr, int x, int y, int size, Color color, AdornmentType adornmentType, RescoBorderStyle borderStyle)
        {
            Point[] points = new Point[3];
            switch (borderStyle)
            {
                case RescoBorderStyle.None:
                case RescoBorderStyle.All:
                    break;

                case RescoBorderStyle.Right:
                    x--;
                    break;

                default:
                    x++;
                    break;
            }
            switch (adornmentType)
            {
                case AdornmentType.RightArrow:
                    x -= size / 2;
                    points[0] = new Point(x, y + size);
                    points[1] = new Point(x + size, y);
                    points[2] = new Point(x, y - size);
                    gr.FillPolygon(this.GetBrush(color), points);
                    return;

                case AdornmentType.UpArrow:
                    y += size / 2;
                    points[0] = new Point((x + size) + 1, y);
                    points[1] = new Point(x, (y - size) - 1);
                    points[2] = new Point(x - size, y);
                    gr.FillPolygon(this.GetBrush(color), points);
                    return;

                case AdornmentType.LeftArrow:
                    x += size / 2;
                    points[0] = new Point(x, y - size);
                    points[1] = new Point(x - size, y);
                    points[2] = new Point(x, y + size);
                    gr.FillPolygon(this.GetBrush(color), points);
                    return;

                case AdornmentType.DownArrow:
                    y -= size / 2;
                    points[0] = new Point((x - size) + 1, y);
                    points[1] = new Point(x, y + size);
                    points[2] = new Point(x + size, y);
                    gr.FillPolygon(this.GetBrush(color), points);
                    return;

                case AdornmentType.Plus:
                case AdornmentType.Minus:
                {
                    int height = size / 4;
                    size += (size - height) % 2;
                    int num2 = x - (size / 2);
                    int num3 = y - (size / 2);
                    gr.FillRectangle(this.GetBrush(color), num2, y - (height / 2), size, height);
                    if (adornmentType == AdornmentType.Plus)
                    {
                        gr.FillRectangle(this.GetBrush(color), x - (height / 2), num3, height, size);
                    }
                    break;
                }
                case AdornmentType.Text:
                case AdornmentType.None:
                    break;

                default:
                    return;
            }
        }

        internal void DrawBorder(Graphics gr, Color color, Size size, RescoBorderStyle borderStyle)
        {
            int x = (int) (this.m_scaleFactor.Width / 2f);
            int y = (int) (this.m_scaleFactor.Height / 2f);
            if (borderStyle != RescoBorderStyle.None)
            {
                if ((borderStyle & RescoBorderStyle.All) != RescoBorderStyle.None)
                {
                    gr.DrawRectangle(this.GetPen(color), x, y, (size.Width - 1) - (2 * x), (size.Height - 1) - (2 * y));
                }
                else
                {
                    if ((borderStyle & RescoBorderStyle.Left) != RescoBorderStyle.None)
                    {
                        gr.DrawLine(this.GetPen(color), x, 0, x, size.Height);
                    }
                    if ((borderStyle & RescoBorderStyle.Right) != RescoBorderStyle.None)
                    {
                        gr.DrawLine(this.GetPen(color), size.Width - 1, 0, size.Width - 1, size.Height);
                    }
                    if ((borderStyle & RescoBorderStyle.Top) != RescoBorderStyle.None)
                    {
                        gr.DrawLine(this.GetPen(color), 0, y, size.Width, y);
                    }
                    if ((borderStyle & RescoBorderStyle.Bottom) != RescoBorderStyle.None)
                    {
                        gr.DrawLine(this.GetPen(color), 0, size.Height - 1, size.Width, size.Height - 1);
                    }
                }
            }
        }

        internal Brush GetBrush(Color color)
        {
            if (this.m_brush == null)
            {
                this.m_brush = new SolidBrush(color);
            }
            else if (this.m_brush.Color != color)
            {
                this.m_brush.Color = color;
            }
            return this.m_brush;
        }

        internal Pen GetPen(Color color)
        {
            if (this.m_pen == null)
            {
                this.m_pen = new Pen(color, this.m_scaleFactor.Width);
            }
            else if (this.m_pen.Color != color)
            {
                this.m_pen.Color = color;
            }
            return this.m_pen;
        }

        internal void Scale(SizeF dpi)
        {
            if (dpi.Width == 0f)
            {
                dpi.Width = 96f;
            }
            if (dpi.Height == 0f)
            {
                dpi.Height = 96f;
            }
            this.m_scaleFactor = new SizeF(dpi.Width / 96f, dpi.Height / 96f);
        }
    }
}

