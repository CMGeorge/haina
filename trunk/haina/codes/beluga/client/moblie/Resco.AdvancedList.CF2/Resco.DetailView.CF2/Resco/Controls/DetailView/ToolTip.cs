namespace Resco.Controls.DetailView
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class ToolTip : Control
    {
        public ToolTip()
        {
            base.Visible = false;
            base.BackColor = SystemColors.Info;
            base.ForeColor = Color.Black;
            base.Text = "";
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
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(base.ForeColor), clientRectangle);
        }

        public void Show(Point pt)
        {
            int num = base.Parent.Width - 20;
            Graphics graphics = base.CreateGraphics();
            string[] strArray = this.Text.Split(new char[] { '\n' });
            int num2 = 6;
            int num3 = 8;
            foreach (string str in strArray)
            {
                SizeF ef = graphics.MeasureString(str, this.Font);
                num3 = ((int) Math.Min((float) num, Math.Max(ef.Width, (float) num3))) + 8;
                num2 += (int) ef.Height;
                int startIndex = 0;
                while (ef.Width > num)
                {
                    int length = 0;
                    do
                    {
                        length++;
                    }
                    while (graphics.MeasureString(str.Substring(startIndex, length), this.Font).Width < num);
                    int num6 = (startIndex + length) - 1;
                    while (num6 > (startIndex + 1))
                    {
                        if (char.IsWhiteSpace(str[num6]))
                        {
                            break;
                        }
                        num6--;
                    }
                    if (num6 == (startIndex + 1))
                    {
                        startIndex += length;
                        this.Text = this.Text.Insert(startIndex, " ");
                    }
                    else
                    {
                        startIndex = num6;
                    }
                    ef = graphics.MeasureString(str.Substring(startIndex), this.Font);
                    num2 += (int) ef.Height;
                }
            }
            pt.Y -= num2;
            if (pt.Y < 0)
            {
                pt.Y = 0;
            }
            pt.X -= num3 / 2;
            if ((pt.X + num3) > num)
            {
                pt.X -= (pt.X + num3) - num;
            }
            if (pt.X <= 0)
            {
                pt.X = 10;
            }
            graphics.Dispose();
            base.Height = num2;
            base.Width = num3;
            base.Location = pt;
            base.Visible = true;
            base.BringToFront();
        }
    }
}

