namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class ToolTip : UserControl
    {
        private SolidBrush m_brush;
        private GradientColor m_gradientBackColor;
        private bool m_minimizeBox;
        private Pen m_pen;
        private SizeF m_scaleFactor = new SizeF(1f, 1f);
        private Resco.Controls.OutlookControls.ShowLocation m_showLocation;
        private bool m_showTitle;
        private ToolTipStyle m_style;
        private HorizontalAlignment m_textAlignment;
        private Color m_titleBackColor;
        private Font m_titleFont;
        private Color m_titleForeColor;
        private int m_titleHeight;
        private string m_titleText;
        private bool m_useGradient;

        static ToolTip()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(Resco.Controls.OutlookControls.ToolTip), "");
            //}
        }

        public ToolTip()
        {
            base.BackColor = SystemColors.Info;
            base.ForeColor = SystemColors.InfoText;
            //base.set_BorderStyle(BorderStyle.FixedSingle);
            base.BorderStyle = (BorderStyle.FixedSingle);
            base.Text = "";
            this.m_textAlignment = HorizontalAlignment.Left;
            this.m_showLocation = Resco.Controls.OutlookControls.ShowLocation.TopLeft;
            this.m_minimizeBox = false;
            this.m_showTitle = false;
            this.m_titleText = "";
            this.m_titleFont = new Font(this.Font.Name, this.Font.Size, FontStyle.Bold);
            this.m_titleHeight = 0x10;
            this.m_titleBackColor = SystemColors.ActiveCaption;
            this.m_titleForeColor = SystemColors.ActiveCaptionText;
            this.m_gradientBackColor = new GradientColor();
            this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
            this.m_useGradient = false;
            this.m_style = ToolTipStyle.Normal;
        }

        private void ApplyStyle(ToolTipStyle style)
        {
            switch (style)
            {
                case ToolTipStyle.Normal:
                    base.Capture = false;
                    return;

                case ToolTipStyle.Modal:
                case ToolTipStyle.AutoHide:
                    if (base.Visible && !base.Capture)
                    {
                        base.Capture = true;
                    }
                    return;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_pen != null)
                {
                    this.m_pen.Dispose();
                    this.m_pen = null;
                }
                if (this.m_brush != null)
                {
                    this.m_brush.Dispose();
                    this.m_brush = null;
                }
                if (this.m_titleFont != null)
                {
                    this.m_titleFont.Dispose();
                    this.m_titleFont = null;
                }
            }
            base.Dispose(disposing);
        }

        protected bool DrawAlignmentString(Graphics gr, string text, Font font, Brush foreBrush, Rectangle layoutRectangle, HorizontalAlignment alignment, bool wrap)
        {
            int num;
            int num2;
            if (((text == null) || (text.Length == 0)) || (((num = layoutRectangle.Width) < 7) || ((num2 = layoutRectangle.Height) < 3)))
            {
                return true;
            }
            SizeF ef = gr.MeasureString(text, font);
            float width = ef.Width;
            bool flag = true;
            if (width <= num)
            {
                if (text.IndexOf('\n') == -1)
                {
                    float num4 = layoutRectangle.Y;
                    float num5 = layoutRectangle.X;
                    if (alignment != HorizontalAlignment.Left)
                    {
                        float num6 = num - width;
                        if (alignment == HorizontalAlignment.Center)
                        {
                            num6 /= 2f;
                        }
                        num5 += num6;
                    }
                    gr.DrawString(text, font, foreBrush, num5, num4);
                    return flag;
                }
                string[] strArray = text.Replace("\r", "").Split(new char[] { '\n' });
                int num7 = strArray.Length;
                if ((num7 > 1) && (strArray[num7 - 1] == ""))
                {
                    num7--;
                }
                float height = ef.Height;
                float num9 = height / ((float) num7);
                if (height > num2)
                {
                    num7 = (int) Math.Floor((double) (((float) num2) / num9));
                    if (num7 < 1)
                    {
                        num7 = 1;
                    }
                    flag = false;
                }
                float num10 = layoutRectangle.Y;
                float num11 = layoutRectangle.X;
                float num12 = 0f;
                for (int k = 0; k < num7; k++)
                {
                    string str = strArray[k];
                    if (alignment != HorizontalAlignment.Left)
                    {
                        ef = gr.MeasureString(str, font);
                        num12 = num - ef.Width;
                        if (alignment == HorizontalAlignment.Center)
                        {
                            num12 /= 2f;
                        }
                    }
                    gr.DrawString(str, font, foreBrush, num11 + num12, num10);
                    num10 += num9;
                }
                return flag;
            }
            string[] strArray2 = text.Replace("\r", "").Split(new char[] { '\n' });
            int length = strArray2.Length;
            if ((length > 1) && (strArray2[length - 1] == ""))
            {
                length--;
            }
            float num16 = ef.Height / ((float) length);
            int num17 = (int) Math.Floor((double) (((float) num2) / num16));
            if (num17 < 1)
            {
                num17 = 1;
            }
            string[] strArray3 = new string[num17];
            float[] numArray = new float[num17];
            int index = 0;
            num17--;
            for (int i = 0; i < length; i++)
            {
                string str2 = strArray2[i];
                float num20 = gr.MeasureString(str2, font).Width;
                if (num20 <= num)
                {
                    strArray3[index] = str2;
                    numArray[index] = num20;
                    if ((++index <= num17) || (i >= (length - 1)))
                    {
                        continue;
                    }
                    flag = false;
                    break;
                }
                flag = false;
                int num21 = str2.Length;
                int num22 = (int) (((float) (num21 * num)) / num20);
                if (num22 < 1)
                {
                    strArray3[index] = str2.Substring(0, 1);
                    numArray[index] = num;
                    if (wrap && (num21 > 1))
                    {
                        flag = true;
                        strArray2[i] = str2.Substring(1, num21 - 1);
                        i--;
                    }
                }
                else
                {
                    int startIndex = 0;
                    bool flag2 = !wrap && (alignment == HorizontalAlignment.Right);
                    if (flag2)
                    {
                        startIndex = num21 - num22;
                    }
                    StringBuilder builder = new StringBuilder(str2.Substring(startIndex, num22));
                    ef = gr.MeasureString(builder.ToString(), font);
                    float num24 = ef.Width;
                    if (num24 > num)
                    {
                        do
                        {
                            num22--;
                            if (num22 <= 0)
                            {
                                break;
                            }
                            builder.Remove(flag2 ? 0 : num22, 1);
                            num24 = gr.MeasureString(builder.ToString(), font).Width;
                        }
                        while (num24 > num);
                    }
                    else
                    {
                        do
                        {
                            num24 = ef.Width;
                            num22++;
                            if (num22 >= num21)
                            {
                                break;
                            }
                            if (flag2)
                            {
                                builder.Insert(0, str2.Substring(num21 - num22, 1));
                            }
                            else
                            {
                                builder.Append(str2[num22 - 1]);
                            }
                            ef = gr.MeasureString(builder.ToString(), font);
                        }
                        while (ef.Width <= num);
                        if (num22 != num21)
                        {
                            builder.Remove(flag2 ? 0 : (num22 - 1), 1);
                        }
                    }
                    int count = builder.Length;
                    if (wrap && (index < num17))
                    {
                        flag = true;
                        if (count < num21)
                        {
                            if (char.IsWhiteSpace(str2[count]))
                            {
                                count++;
                            }
                            else
                            {
                                int num26 = str2.LastIndexOfAny(new char[] { ' ', '\t' }, count - 1, count);
                                if (num26 > 0)
                                {
                                    builder.Remove(num26, count - num26);
                                    count = num26 + 1;
                                }
                            }
                        }
                        if (count < num21)
                        {
                            strArray2[i] = str2.Substring(count, num21 - count);
                            num24 = gr.MeasureString(builder.ToString(), font).Width;
                            i--;
                        }
                    }
                    strArray3[index] = builder.ToString();
                    numArray[index] = num24;
                }
                if ((++index > num17) && (i < (length - 1)))
                {
                    flag = false;
                    break;
                }
            }
            float y = layoutRectangle.Y;
            float x = layoutRectangle.X;
            float num29 = 0f;
            for (int j = 0; j < index; j++)
            {
                string s = strArray3[j];
                if (alignment != HorizontalAlignment.Left)
                {
                    num29 = num - numArray[j];
                    if (alignment == HorizontalAlignment.Center)
                    {
                        num29 /= 2f;
                    }
                }
                gr.DrawString(s, font, foreBrush, x + num29, y);
                y += num16;
            }
            return flag;
        }

        protected SolidBrush GetBrush(Color c)
        {
            if (this.m_brush == null)
            {
                this.m_brush = new SolidBrush(c);
            }
            else
            {
                this.m_brush.Color = c;
            }
            return this.m_brush;
        }

        protected Pen GetPen(Color c, float width)
        {
            if (this.m_pen == null)
            {
                this.m_pen = new Pen(c, width);
            }
            else
            {
                this.m_pen.Color = c;
                if (this.m_pen.Width != width)
                {
                    this.m_pen.Width = width;
                }
            }
            return this.m_pen;
        }

        public void Hide()
        {
            base.Capture = false;
            base.Hide();
        }

        private void m_gradientBackColor_PropertyChanged(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        protected Size MeasureTextSize(Graphics gr, Font font, string text, int width, bool wrap)
        {
            Size size = new Size(width, 0);
            SizeF ef = gr.MeasureString(text, font);
            if (ef.Width <= width)
            {
                size.Width = (int) Math.Ceiling((double) ef.Width);
                size.Height += (int) Math.Ceiling((double) ef.Height);
                return size;
            }
            string[] strArray = text.Replace("\r", "").Split(new char[] { '\n' });
            int length = strArray.Length;
            if ((length > 1) && (strArray[length - 1] == ""))
            {
                length--;
            }
            for (int i = 0; i < length; i++)
            {
                float num8;
                string str = strArray[i];
                ef = gr.MeasureString(str, font);
                float num4 = ef.Width;
                if (num4 <= width)
                {
                    size.Height += (int) Math.Ceiling((double) ef.Height);
                    continue;
                }
                int num5 = str.Length;
                int num6 = (int) (((float) (num5 * width)) / num4);
                if (num6 < 1)
                {
                    size.Height += (int) Math.Ceiling((double) ef.Height);
                    if (wrap && (num5 > 1))
                    {
                        strArray[i] = str.Substring(1, num5 - 1);
                        i--;
                    }
                    continue;
                }
                int startIndex = 0;
                StringBuilder builder = new StringBuilder(str.Substring(startIndex, num6));
                ef = gr.MeasureString(builder.ToString(), font);
                if (ef.Width > width)
                {
                    do
                    {
                        num6--;
                        if (num6 <= 0)
                        {
                            break;
                        }
                        builder.Remove(num6, 1);
                        ef = gr.MeasureString(builder.ToString(), font);
                    }
                    while (ef.Width > width);
                }
                else
                {
                    do
                    {
                        num8 = ef.Width;
                        num6++;
                        if (num6 >= num5)
                        {
                            break;
                        }
                        builder.Append(str[num6 - 1]);
                        ef = gr.MeasureString(builder.ToString(), font);
                    }
                    while (ef.Width <= width);
                    if (num6 != num5)
                    {
                        builder.Remove(num6 - 1, 1);
                    }
                }
                int count = builder.Length;
                if (wrap)
                {
                    if (count < num5)
                    {
                        if (char.IsWhiteSpace(str[count]))
                        {
                            count++;
                        }
                        else
                        {
                            int num10 = str.LastIndexOfAny(new char[] { ' ', '\t' }, count - 1, count);
                            if (num10 > 0)
                            {
                                builder.Remove(num10, count - num10);
                                count = num10 + 1;
                            }
                        }
                    }
                    if (count < num5)
                    {
                        strArray[i] = str.Substring(count, num5 - count);
                        ef = gr.MeasureString(builder.ToString(), font);
                        num8 = ef.Width;
                        i--;
                    }
                }
                size.Height += (int) Math.Ceiling((double) ef.Height);
            }
            return size;
        }

        protected override void OnClick(EventArgs e)
        {
            if (this.m_minimizeBox)
            {
                Point pt = base.PointToClient(Control.MousePosition);
                int titleHeight = this.m_titleHeight;
                int height = this.m_titleHeight;
                Rectangle rectangle = new Rectangle(base.ClientRectangle.Width - titleHeight, -1, titleHeight, height);
                if (rectangle.Contains(pt))
                {
                    this.Hide();
                }
            }
            base.OnClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if ((this.m_style == ToolTipStyle.AutoHide) && !base.ClientRectangle.Contains(e.X, e.Y))
            {
                this.Hide();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.ApplyStyle(this.m_style);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int width = (int) Math.Ceiling((double) (4f * this.m_scaleFactor.Width));
            int height = (int) Math.Ceiling((double) (2f * this.m_scaleFactor.Height));
            Rectangle clientRectangle = base.ClientRectangle;
            if (this.m_showTitle)
            {
                clientRectangle.X += width;
                clientRectangle.Width -= this.m_minimizeBox ? this.m_titleHeight : 0;
                clientRectangle.Height = this.m_titleHeight;
                e.Graphics.DrawString(this.m_titleText, this.m_titleFont, this.GetBrush(this.m_titleForeColor), clientRectangle);
            }
            clientRectangle = base.ClientRectangle;
            clientRectangle.X += width;
            clientRectangle.Y += height + ((this.m_showTitle || this.m_minimizeBox) ? this.m_titleHeight : 0);
            clientRectangle.Width -= 2 * width;
            clientRectangle.Height -= 2 * height;
            this.DrawAlignmentString(e.Graphics, this.Text, this.Font, new SolidBrush(base.ForeColor), clientRectangle, this.m_textAlignment, true);
            if (this.m_minimizeBox)
            {
                width = this.m_titleHeight;
                height = this.m_titleHeight;
                clientRectangle = new Rectangle(base.ClientRectangle.Width - width, -1, width, height);
                int num3 = (int) (3f * this.m_scaleFactor.Width);
                clientRectangle.Inflate(-num3, -num3);
                Pen pen = this.GetPen(this.m_showTitle ? this.m_titleForeColor : this.ForeColor, 2f * this.m_scaleFactor.Width);
                e.Graphics.DrawLine(pen, (int) (clientRectangle.X + ((int) this.m_scaleFactor.Width)), (int) (clientRectangle.Y + ((int) this.m_scaleFactor.Height)), (int) ((clientRectangle.X + clientRectangle.Width) - ((int) this.m_scaleFactor.Width)), (int) ((clientRectangle.Y + clientRectangle.Height) - ((int) this.m_scaleFactor.Height)));
                e.Graphics.DrawLine(pen, (int) (clientRectangle.X + ((int) this.m_scaleFactor.Width)), (int) ((clientRectangle.Y + clientRectangle.Height) - ((int) this.m_scaleFactor.Height)), (int) ((clientRectangle.X + clientRectangle.Width) - ((int) this.m_scaleFactor.Width)), (int) (clientRectangle.Y + ((int) this.m_scaleFactor.Height)));
            }
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Rectangle empty = Rectangle.Empty;
            bool flag = this.m_showTitle && (this.m_titleBackColor != Color.Transparent);
            if (this.m_useGradient)
            {
                empty = new Rectangle(0, flag ? this.m_titleHeight : 0, base.Width, base.Height - (flag ? this.m_titleHeight : 0));
                this.m_gradientBackColor.DrawGradient(e.Graphics, empty);
            }
            else
            {
                base.OnPaintBackground(e);
            }
            if (flag)
            {
                empty = new Rectangle(0, 0, base.Width, this.m_titleHeight);
                e.Graphics.FillRectangle(this.GetBrush(this.m_titleBackColor), empty);
            }
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            this.m_titleHeight = (int) Math.Ceiling((double) (this.m_titleHeight * factor.Height));
            ContainerControl topLevelControl = base.TopLevelControl as ContainerControl;
            if (topLevelControl != null)
            {
                factor = new SizeF((topLevelControl.AutoScaleDimensions.Width / 96f) * factor.Width, (topLevelControl.AutoScaleDimensions.Height / 96f) * factor.Height);
            }
            this.m_scaleFactor = factor;
        }

        protected virtual bool ShouldSerializeGradientBackColor()
        {
            return (((this.m_gradientBackColor.StartColor.ToArgb() != Color.White.ToArgb()) | (this.m_gradientBackColor.EndColor.ToArgb() != Color.White.ToArgb())) | (this.m_gradientBackColor.FillDirection != FillDirection.Vertical));
        }

        protected virtual bool ShouldSerializeShowLocation()
        {
            return (this.m_showLocation != Resco.Controls.OutlookControls.ShowLocation.TopLeft);
        }

        protected virtual bool ShouldSerializeStyle()
        {
            return (this.m_style != ToolTipStyle.Normal);
        }

        public void Show()
        {
            if (base.Parent != null)
            {
                Point pt = base.Parent.PointToClient(Control.MousePosition);
                this.Show(pt);
            }
        }

        public void Show(Point pt)
        {
            this.Show(pt, this.Text);
        }

        public void Show(string text)
        {
            this.Text = text;
            this.Show();
        }

        public void Show(Point pt, string text)
        {
            this.Text = text;
            if (base.Parent != null)
            {
                int num = (int) Math.Ceiling((double) (4f * this.m_scaleFactor.Width));
                int num2 = (int) Math.Ceiling((double) (2f * this.m_scaleFactor.Height));
                int num3 = (base.BorderStyle == BorderStyle.None) ? 0 : ((int) Math.Ceiling((double) (1f * this.m_scaleFactor.Width)));
                int width = base.Parent.ClientSize.Width;
                int num5 = (width - (2 * num)) - (2 * num3);
                Graphics gr = base.CreateGraphics();
                Size size = this.MeasureTextSize(gr, this.Font, text, num5, true);
                if (gr != null)
                {
                    gr.Dispose();
                }
                gr = null;
                size.Width += (2 * num) + (2 * num3);
                size.Height += ((2 * num2) + (2 * num3)) + ((this.m_showTitle || this.m_minimizeBox) ? this.m_titleHeight : 0);
                base.Size = size;
                switch (this.m_showLocation)
                {
                    case Resco.Controls.OutlookControls.ShowLocation.TopRight:
                        pt.X -= base.Width;
                        break;

                    case Resco.Controls.OutlookControls.ShowLocation.BottomLeft:
                        pt.Y -= base.Height;
                        break;

                    case Resco.Controls.OutlookControls.ShowLocation.BottomRight:
                        pt.X -= base.Width;
                        pt.Y -= base.Height;
                        break;
                }
                if ((pt.X + base.Width) > base.Parent.Width)
                {
                    pt.X -= (pt.X + base.Width) - base.Parent.Width;
                }
                if ((pt.Y + base.Height) > base.Parent.Height)
                {
                    pt.Y -= (pt.Y + base.Height) - base.Parent.Height;
                }
                if (pt.X < 0)
                {
                    pt.X = 0;
                }
                if (pt.Y < 0)
                {
                    pt.Y = 0;
                }
                base.Location = pt;
                base.Visible = true;
                if (width != base.Parent.ClientSize.Width)
                {
                    this.Show(pt, text);
                }
                base.BringToFront();
                base.Capture = true;
            }
        }

        public GradientColor GradientBackColor
        {
            get
            {
                return this.m_gradientBackColor;
            }
            set
            {
                if (this.m_gradientBackColor != value)
                {
                    this.m_gradientBackColor.PropertyChanged -= new EventHandler(this.m_gradientBackColor_PropertyChanged);
                    this.m_gradientBackColor = null;
                    this.m_gradientBackColor = value;
                    this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
                }
                base.Invalidate();
            }
        }

        public bool MinimizeBox
        {
            get
            {
                return this.m_minimizeBox;
            }
            set
            {
                if (this.m_minimizeBox != value)
                {
                    this.m_minimizeBox = value;
                    base.Invalidate();
                }
            }
        }

        public Resco.Controls.OutlookControls.ShowLocation ShowLocation
        {
            get
            {
                return this.m_showLocation;
            }
            set
            {
                if (this.m_showLocation != value)
                {
                    this.m_showLocation = value;
                    base.Invalidate();
                }
            }
        }

        public bool ShowTitle
        {
            get
            {
                return this.m_showTitle;
            }
            set
            {
                if (this.m_showTitle != value)
                {
                    this.m_showTitle = value;
                    base.Invalidate();
                }
            }
        }

        public ToolTipStyle Style
        {
            get
            {
                return this.m_style;
            }
            set
            {
                if (this.m_style != value)
                {
                    this.m_style = value;
                    this.ApplyStyle(value);
                }
            }
        }

        public string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    base.Invalidate();
                }
            }
        }

        public HorizontalAlignment TextAlignment
        {
            get
            {
                return this.m_textAlignment;
            }
            set
            {
                if (this.m_textAlignment != value)
                {
                    this.m_textAlignment = value;
                    base.Invalidate();
                }
            }
        }

        public Color TitleBackColor
        {
            get
            {
                return this.m_titleBackColor;
            }
            set
            {
                if (value == Color.Empty)
                {
                    value = SystemColors.ActiveCaption;
                }
                if (this.m_titleBackColor != value)
                {
                    this.m_titleBackColor = value;
                    base.Invalidate();
                }
            }
        }

        public Font TitleFont
        {
            get
            {
                return this.m_titleFont;
            }
            set
            {
                this.m_titleFont = value;
                base.Invalidate();
            }
        }

        public Color TitleForeColor
        {
            get
            {
                return this.m_titleForeColor;
            }
            set
            {
                if (value == Color.Empty)
                {
                    value = SystemColors.ActiveCaptionText;
                }
                if (this.m_titleForeColor != value)
                {
                    this.m_titleForeColor = value;
                    base.Invalidate();
                }
            }
        }

        public int TitleHeight
        {
            get
            {
                return this.m_titleHeight;
            }
            set
            {
                if (this.m_titleHeight != value)
                {
                    this.m_titleHeight = value;
                    base.Invalidate();
                }
            }
        }

        public string TitleText
        {
            get
            {
                return this.m_titleText;
            }
            set
            {
                if (this.m_titleText != value)
                {
                    this.m_titleText = value;
                    base.Invalidate();
                }
            }
        }

        public bool UseGradient
        {
            get
            {
                return this.m_useGradient;
            }
            set
            {
                if (this.m_useGradient != value)
                {
                    this.m_useGradient = value;
                    base.Invalidate();
                }
            }
        }
    }
}

