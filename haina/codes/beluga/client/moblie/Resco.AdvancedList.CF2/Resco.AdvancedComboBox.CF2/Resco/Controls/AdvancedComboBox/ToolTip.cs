namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class ToolTip : Control
    {
        private WrapTextData m_wrapper;

        public ToolTip()
        {
            base.Visible = false;
            base.BackColor = SystemColors.Info;
            base.ForeColor = Color.Black;
            base.Text = "";
        }

        protected override void Dispose(bool disposing)
        {
            this.m_wrapper = null;
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle clientRectangle = base.ClientRectangle;
            clientRectangle.Width--;
            clientRectangle.Height--;
            e.Graphics.DrawRectangle(new Pen(Color.Black), clientRectangle);
            clientRectangle.X += 4;
            clientRectangle.Y += 2;
            clientRectangle.Width -= 4;
            clientRectangle.Height -= 2;
            SolidBrush brush = new SolidBrush(base.ForeColor);
            for (int i = 0; i < this.m_wrapper.Lines.Length; i++)
            {
                DrawLineData data = this.m_wrapper.Lines[i];
                string s = this.Text.Substring(data.Index, data.Length);
                e.Graphics.DrawString(s, this.Font, brush, (float) clientRectangle.X, (float) clientRectangle.Y);
                clientRectangle.Y += this.m_wrapper.LineHeight;
                data = null;
            }
            brush.Dispose();
        }

        public void Show(Point pt)
        {
            int num = base.Parent.Width - 20;
            Graphics gr = base.CreateGraphics();
            int num2 = 6;
            int width = 8;
            SizeF ef = gr.MeasureString(this.Text, this.Font);
            width = ((int) Math.Min((float) num, Math.Max(ef.Width, (float) width))) + 8;
            this.m_wrapper = Utility.WrapText(gr, this.Text, this.Font, width);
            width = this.m_wrapper.Width;
            num2 += this.m_wrapper.Height;
            pt.Y -= num2;
            if (pt.Y < 0)
            {
                pt.Y = 0;
            }
            pt.X -= width / 2;
            if ((pt.X + width) > num)
            {
                pt.X -= (pt.X + width) - num;
            }
            if (pt.X <= 0)
            {
                pt.X = 10;
            }
            gr.Dispose();
            base.Height = num2;
            base.Width = width;
            base.Location = pt;
            base.Visible = true;
            base.BringToFront();
        }
    }
}

