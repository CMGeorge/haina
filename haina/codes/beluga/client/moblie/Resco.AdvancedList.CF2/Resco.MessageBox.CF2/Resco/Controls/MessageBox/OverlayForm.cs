namespace Resco.Controls.MessageBox
{
    using Resco.CoreDraw;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class OverlayForm : Form
    {
        protected bool m_bFirstPaint;

        protected virtual Rectangle CalcClientRect()
        {
            return base.ClientRectangle;
        }

        private void GetRects(List<Rectangle> r, Rectangle c, int b)
        {
            r.Add(new Rectangle(c.X - b, c.Y, b, c.Height));
            r.Add(new Rectangle(c.Right, c.Y, b, c.Height));
            r.Add(new Rectangle(c.X, c.Y - b, c.Width, b));
            r.Add(new Rectangle(c.X, c.Bottom, c.Width, b));
            int width = b / 2;
            r.Add(new Rectangle(c.X - width, c.Y - width, width, width));
            r.Add(new Rectangle(c.Right, c.Y - width, width, width));
            r.Add(new Rectangle(c.X - width, c.Bottom, width, width));
            r.Add(new Rectangle(c.Right, c.Bottom, width, width));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle c = this.CalcClientRect();
            if (this.m_bFirstPaint)
            {
                int b = 2 * ((int) (g.DpiY / 96f));
                List<Rectangle> r = new List<Rectangle>();
                r.Add(base.Bounds);
                this.GetRects(r, c, b);
                Color[] colors = new Color[] { Color.FromArgb(-1610612736), Color.FromArgb(0x50000000), Color.FromArgb(0x50000000), Color.FromArgb(0x50000000), Color.FromArgb(0x50000000), Color.FromArgb(0x50000000), Color.FromArgb(0x50000000), Color.FromArgb(0x50000000), Color.FromArgb(0x50000000) };
                ExtendDraw.BlendRects(g, base.Bounds, r.ToArray(), colors);
                this.m_bFirstPaint = false;
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    g.FillRectangle(brush, base.Bounds);
                }
            }
            int width = c.Width;
            int height = c.Height;
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                using (Graphics graphics2 = Graphics.FromImage(bitmap))
                {
                    int x = c.X;
                    int y = c.Y;
                    ExtendDraw.BitBlt(graphics2, new Rectangle(0, 0, width, height), g, x, y);
                    Point offset = new Point(-x, -y);
                    this.Render(graphics2, offset);
                    ExtendDraw.BitBlt(g, new Rectangle(x, y, width, height), graphics2, 0, 0);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected virtual void Render(Graphics g, Point offset)
        {
        }
    }
}

