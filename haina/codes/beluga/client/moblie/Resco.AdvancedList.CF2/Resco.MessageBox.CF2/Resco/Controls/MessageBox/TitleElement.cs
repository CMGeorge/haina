namespace Resco.Controls.MessageBox
{
    using Resco.CoreDraw;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class TitleElement : Element
    {
        private string text;
        private bool m_bMultiLine;
        private const int MARGIN = 20;

        public TitleElement(Control parent, int scale) : base(parent, scale)
        {
        }

        private Rectangle GetTextBounds(Graphics g)
        {
            Rectangle bounds = base.Bounds;
            bounds.Inflate(-20 * base.m_scale, 0);
            if (this.m_bMultiLine)
            {
                int height = ExtendDraw.MeasureString(g, this.Text, base.Font, bounds.Width).Height;
                if (height < bounds.Height)
                {
                    bounds.Y += (bounds.Height - height) / 2;
                    bounds.Height = height;
                }
            }
            return bounds;
        }

        public override void OnResize()
        {
            if (this.m_bMultiLine)
            {
                using (Graphics graphics = base.m_parent.CreateGraphics())
                {
                    int num = ExtendDraw.MeasureString(graphics, this.Text, base.Font, base.Width - (40 * base.m_scale)).Height + (0x36 * base.m_scale);
                    if (num > (100 * base.m_scale))
                    {
                        num = 100 * base.m_scale;
                    }
                    base.Height = num;
                    return;
                }
            }
            base.Height = 0x1b * base.m_scale;
        }

        public void Render(Graphics g, Point offset)
        {
            Rectangle bounds = base.Bounds;
            bounds.X += offset.X;
            bounds.Y += offset.Y;
            using (Brush brush = new SolidBrush(this.ForeColor))
            {
                using (Brush brush2 = new SolidBrush(this.BackColor))
                {
                    Rectangle rc = bounds;
                    ExtendDraw.FillRoundedRectangle(g, brush, rc, base.m_scale * 5, 3);
                    rc.Inflate(-base.m_scale, -base.m_scale);
                    ExtendDraw.FillRoundedRectangle(g, brush2, rc, base.m_scale * 5, 3);
                }
                Region clip = g.Clip;
                bounds = this.GetTextBounds(g);
                bounds.X += offset.X;
                bounds.Y += offset.Y;
                g.Clip = new Region(bounds);
                StringFormat format = new StringFormat();
                if (!this.m_bMultiLine)
                {
                    SizeF ef = g.MeasureString(this.Text, base.Font);
                    if (bounds.Width <= ef.Width)
                    {
                        bounds.Width = (int) ef.Width;
                    }
                    format.LineAlignment = StringAlignment.Center;
                }
                format.Alignment = StringAlignment.Center;
                g.DrawString(this.Text, base.Font, brush, bounds, format);
                g.Clip = clip;
            }
        }

        public void SetText(string text, bool bMultiLine)
        {
            this.Text = text;
            this.m_bMultiLine = bMultiLine;
            this.OnResize();
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
    }
}

