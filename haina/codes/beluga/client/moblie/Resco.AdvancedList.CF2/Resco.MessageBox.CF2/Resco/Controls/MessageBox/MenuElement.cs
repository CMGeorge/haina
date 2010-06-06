namespace Resco.Controls.MessageBox
{
    using Resco.CoreDraw;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class MenuElement : Element
    {
        private Color lineColor;
        private int m_itemHeight;
        private Resco.Controls.MessageBox.MenuItem[] m_items;
        private int m_mouseDown;
        private int m_selected;

        public event Action<int> ButtonClicked;

        public MenuElement(Control parent, int scale) : base(parent, scale)
        {
        }

        public bool HandleMouseDown(int x, int y)
        {
            int index = this.ItemFromPoint(x, y);
            if (index == -1)
            {
                return false;
            }
            if (this.m_items[index].Enabled)
            {
                this.m_selected = index;
                this.m_mouseDown = index;
                base.Invalidate();
            }
            return true;
        }

        public void HandleMouseMove(int x, int y)
        {
            int num2 = (this.ItemFromPoint(x, y) == this.m_mouseDown) ? this.m_mouseDown : -1;
            if (this.m_selected != num2)
            {
                this.m_selected = num2;
                base.Invalidate();
            }
        }

        public void HandleMouseUp(int x, int y)
        {
            if (this.m_selected >= 0)
            {
                if (this.ButtonClicked != null)
                {
                    this.ButtonClicked.Invoke(this.m_selected);
                }
                this.m_selected = -1;
                base.Invalidate();
            }
        }

        private int ItemFromPoint(int x, int y)
        {
            x -= base.Left;
            y -= base.Top;
            if ((x >= 0) && (x < base.Width))
            {
                int num = y / this.m_itemHeight;
                if ((num >= 0) && (num < this.m_items.Length))
                {
                    return num;
                }
            }
            return -1;
        }

        public void Render(Graphics g, Point offset)
        {
            using (Brush brush = new SolidBrush(this.BackColor))
            {
                using (Brush brush2 = new SolidBrush(this.ForeColor))
                {
                    Rectangle bounds = base.Bounds;
                    bounds.X += offset.X;
                    bounds.Y += offset.Y;
                    bounds.Height = this.m_itemHeight;
                    int num = 0;
                    int num2 = this.m_items.Length - 1;
                    foreach (Resco.Controls.MessageBox.MenuItem item in this.m_items)
                    {
                        bool flag = num == this.m_selected;
                        if (num == num2)
                        {
                            ExtendDraw.FillRoundedRectangle(g, flag ? brush2 : brush, bounds, base.m_scale * 5, 12);
                        }
                        else
                        {
                            g.FillRectangle(flag ? brush2 : brush, bounds);
                        }
                        if (item.Image != null)
                        {
                            int num3 = (bounds.Height - item.Image.Height) / 2;
                            ExtendDraw.DrawImage(g, item.Image, num3 + bounds.Left, num3 + bounds.Top, item.Image.GetPixel(0, 0));
                        }
                        StringFormat format = new StringFormat();
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;
                        g.DrawString(item.Text, base.Font, flag ? brush : brush2, bounds, format);
                        if (!item.Enabled)
                        {
                            Rectangle rect = bounds;
                            rect.Inflate(-2 * base.m_scale, -2 * base.m_scale);
                            ExtendDraw.BlendRect(g, rect, Color.FromArgb(-2130706433));
                        }
                        if (num > 0)
                        {
                            using (Pen pen = new Pen(this.LineColor))
                            {
                                g.DrawLine(pen, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
                            }
                        }
                        num++;
                        bounds.Y += this.m_itemHeight;
                    }
                }
            }
        }

        public Resco.Controls.MessageBox.MenuItem[] Items
        {
            get
            {
                return this.m_items;
            }
            set
            {
                this.m_items = value;
                this.m_itemHeight = 0x23 * base.m_scale;
                base.Height = (this.m_items.Length * this.m_itemHeight) + 1;
                this.m_selected = -1;
            }
        }

        public Color LineColor
        {
            get
            {
                return this.lineColor;
            }
            set
            {
                this.lineColor = value;
            }
        }
    }
}

