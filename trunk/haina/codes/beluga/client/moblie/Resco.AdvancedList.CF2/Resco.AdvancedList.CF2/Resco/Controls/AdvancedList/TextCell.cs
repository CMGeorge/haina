namespace Resco.Controls.AdvancedList
{
    using Resco.Controls.AdvancedList.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TextCell : Cell
    {
        private Font m_Font;
        private string m_format;
        private bool m_restrictSelectedTextToStart;
        private Color m_selectedBackColor;
        private Color m_selectedForeColor;
        private string m_selectedText;
        private Resco.Controls.AdvancedList.Alignment m_TextAlignment;

        public TextCell()
        {
            this.m_Font = new Font("Tahoma", 8f, FontStyle.Regular);
            this.m_TextAlignment = Resco.Controls.AdvancedList.Alignment.TopLeft;
            this.m_format = null;
            this.m_selectedForeColor = SystemColors.HighlightText;
            this.m_selectedBackColor = SystemColors.Highlight;
            this.m_selectedText = "";
            this.m_restrictSelectedTextToStart = false;
        }

        public TextCell(TextCell tc) : base(tc)
        {
            this.m_Font = new Font(tc.m_Font.Name, tc.m_Font.Size, tc.m_Font.Style);
            this.m_TextAlignment = tc.m_TextAlignment;
            this.m_format = tc.m_format;
            this.m_selectedForeColor = tc.m_selectedForeColor;
            this.m_selectedBackColor = tc.m_selectedBackColor;
            this.m_selectedText = tc.m_selectedText;
            this.m_restrictSelectedTextToStart = tc.m_restrictSelectedTextToStart;
        }

        public override Cell Clone()
        {
            return new TextCell(this);
        }

        protected bool DrawAlignedText(Graphics gr, string text, Brush foreBrush, Rectangle layoutRectangle)
        {
            int num;
            int num2;
            HorizontalAlignment center;
            VerticalAlignment middle;
            if (((text == null) || (text.Length == 0)) || (((num = layoutRectangle.Width) < 1) || ((num2 = layoutRectangle.Height) < 1)))
            {
                return true;
            }
            Font font = this.m_Font;
            switch (this.m_TextAlignment)
            {
                case Resco.Controls.AdvancedList.Alignment.MiddleCenter:
                    center = HorizontalAlignment.Center;
                    middle = VerticalAlignment.Middle;
                    break;

                case Resco.Controls.AdvancedList.Alignment.MiddleRight:
                    center = HorizontalAlignment.Right;
                    middle = VerticalAlignment.Middle;
                    break;

                case Resco.Controls.AdvancedList.Alignment.MiddleLeft:
                    center = HorizontalAlignment.Left;
                    middle = VerticalAlignment.Middle;
                    break;

                case Resco.Controls.AdvancedList.Alignment.BottomCenter:
                    center = HorizontalAlignment.Center;
                    middle = VerticalAlignment.Bottom;
                    break;

                case Resco.Controls.AdvancedList.Alignment.BottomRight:
                    center = HorizontalAlignment.Right;
                    middle = VerticalAlignment.Bottom;
                    break;

                case Resco.Controls.AdvancedList.Alignment.BottomLeft:
                    center = HorizontalAlignment.Left;
                    middle = VerticalAlignment.Bottom;
                    break;

                case Resco.Controls.AdvancedList.Alignment.TopCenter:
                    center = HorizontalAlignment.Center;
                    middle = VerticalAlignment.Top;
                    break;

                case Resco.Controls.AdvancedList.Alignment.TopRight:
                    center = HorizontalAlignment.Right;
                    middle = VerticalAlignment.Top;
                    break;

                default:
                    center = HorizontalAlignment.Left;
                    middle = VerticalAlignment.Top;
                    break;
            }
            int num3 = -1;
            if (this.m_selectedText != "")
            {
                num3 = text.ToLower().IndexOf(this.m_selectedText.ToLower());
            }
            Brush brush = Resco.Controls.AdvancedList.AdvancedList.GetBrush(this.SelectedForeColor);
            Brush brush2 = Resco.Controls.AdvancedList.AdvancedList.GetBrush(this.SelectedBackColor);
            bool flag = true;
            WrapTextData data = Utility.WrapText(gr, text, this.m_Font, layoutRectangle.Width);
            if (data == null)
            {
                return true;
            }
            int length = data.Lines.Length;
            int lineHeight = data.LineHeight;
            int height = data.Height;
            if (num2 < data.Height)
            {
                length = num2 / lineHeight;
                if (length < 1)
                {
                    if (num2 <= (lineHeight / 2))
                    {
                        return false;
                    }
                    length = 1;
                }
                height = length * lineHeight;
                flag = data.Lines.Length <= length;
            }
            int y = layoutRectangle.Y;
            if (middle != VerticalAlignment.Top)
            {
                int num8 = num2 - height;
                if (middle == VerticalAlignment.Middle)
                {
                    num8 /= 2;
                }
                y += num8;
            }
            int x = layoutRectangle.X;
            int num10 = 0;
            int index = 0;
            if (y < 0)
            {
                int num12 = -y / lineHeight;
                y += num12 * lineHeight;
                index += num12;
            }
            int num13 = base.Parent.CalculateClientRect().Height;
            int num14 = -1;
            int num15 = 0;
            int num17 = this.m_selectedText.Length;
            while (index < length)
            {
                string str2;
                int num18;
                DrawLineData data2 = data.Lines[index];
                if (!flag && (index == (length - 1)))
                {
                    int cutLength = data2.CutLength;
                    if ((data2.Index + cutLength) > text.Length)
                    {
                        cutLength = data2.Length;
                    }
                    str2 = text.Substring(data2.Index, cutLength);
                    num18 = (cutLength == data2.Length) ? data2.Width : num;
                    if ((base.Parent != null) && (base.Parent.ToolTipType == ToolTipType.Dots))
                    {
                        int width = (int) Math.Ceiling((double) Utility.MeasureString(gr, "...", font).Width);
                        int num21 = num - width;
                        num18 = (int) Math.Ceiling((double) Utility.MeasureString(gr, str2, font).Width);
                        while ((str2.Length > 0) && (num18 >= num21))
                        {
                            str2 = str2.Remove(str2.Length - 1, 1);
                            num18 = (int) Math.Ceiling((double) Utility.MeasureString(gr, str2, font).Width);
                        }
                        int num22 = 0;
                        if (center != HorizontalAlignment.Left)
                        {
                            num22 = num - (num18 + width);
                            if (center == HorizontalAlignment.Center)
                            {
                                num22 /= 2;
                            }
                        }
                        int num23 = (x + num22) + num18;
                        this.DrawTextLine(gr, "...", font, foreBrush, num23, y, width, lineHeight, 0);
                        base.Parent.AddTooltipArea(new Rectangle(num23, y, (num - num23) + 2, lineHeight + 2), text);
                    }
                }
                else
                {
                    str2 = text.Substring(data2.Index, data2.Length);
                    num18 = data2.Width;
                }
                if (center != HorizontalAlignment.Left)
                {
                    num10 = num - num18;
                    if (center == HorizontalAlignment.Center)
                    {
                        num10 /= 2;
                    }
                }
                if ((((num3 > 0) && !this.m_restrictSelectedTextToStart) || (num3 == 0)) && (((num3 + (num15 = num17)) > (num14 = data2.Index)) && (num3 < (num14 + str2.Length))))
                {
                    string str;
                    int num16;
                    int num24 = num3 - num14;
                    if (num24 > 0)
                    {
                        str = str2.Substring(0, num24);
                        this.DrawTextLine(gr, str, font, foreBrush, x + num10, y, num18, lineHeight, data2.Index);
                        num16 = (int) Utility.MeasureString(gr, str, font).Width;
                        if ((num24 + num15) < str2.Length)
                        {
                            str = str2.Substring(num24, num15);
                            int num25 = (int) Utility.MeasureString(gr, str, font).Width;
                            gr.FillRectangle(brush2, (x + num10) + num16, y, num25, lineHeight);
                            this.DrawTextLine(gr, str, font, brush, (x + num10) + num16, y, num18 - num16, lineHeight, data2.Index);
                            num16 += num25;
                            str = str2.Substring(num24 + num15, str2.Length - (num24 + num15));
                            this.DrawTextLine(gr, str, font, foreBrush, (x + num10) + num16, y, num18 - num16, lineHeight, data2.Index);
                        }
                        else
                        {
                            str = str2.Substring(num24, (num14 + str2.Length) - num3);
                            gr.FillRectangle(brush2, (x + num10) + num16, y, num18 - num16, lineHeight);
                            this.DrawTextLine(gr, str, font, brush, (x + num10) + num16, y, num18 - num16, lineHeight, data2.Index);
                        }
                    }
                    else
                    {
                        if ((num24 + num15) < str2.Length)
                        {
                            str = str2.Substring(0, num24 + num15);
                        }
                        else
                        {
                            str = str2;
                        }
                        num16 = (int) Utility.MeasureString(gr, str, font).Width;
                        gr.FillRectangle(brush2, x + num10, y, num16, lineHeight);
                        this.DrawTextLine(gr, str, font, brush, x + num10, y, num18, lineHeight, data2.Index);
                        if ((num24 + num15) < str2.Length)
                        {
                            str = str2.Substring(num15 + num24, str2.Length - (num15 + num24));
                            this.DrawTextLine(gr, str, font, foreBrush, (x + num10) + num16, y, num18 - num16, lineHeight, data2.Index);
                        }
                    }
                }
                else
                {
                    this.DrawTextLine(gr, str2, font, foreBrush, x + num10, y, num18, lineHeight, data2.Index);
                }
                y += lineHeight;
                data2 = null;
                if (y > num13)
                {
                    break;
                }
                index++;
            }
            data = null;
            return flag;
        }

        protected override void DrawContent(Graphics gr, Rectangle drawbounds, object data)
        {
            string text = this.GetText(data);
            Rectangle layoutRectangle = drawbounds;
            layoutRectangle.X += 2;
            layoutRectangle.Y++;
            layoutRectangle.Width -= 3;
            layoutRectangle.Height--;
            Brush foreBrush = Resco.Controls.AdvancedList.AdvancedList.GetBrush(this.GetColor(ColorCategory.Foreground));
            if (!this.DrawAlignedText(gr, text, foreBrush, layoutRectangle))
            {
                this.DrawToolTip(gr, foreBrush, layoutRectangle, text);
            }
        }

        protected virtual void DrawTextLine(Graphics gr, string line, Font font, Brush brush, int x, int y, int width, int height, int textIndex)
        {
            gr.DrawString(line, font, brush, (float) x, (float) y);
        }

        protected virtual void DrawToolTip(Graphics gr, Brush foreBrush, Rectangle layoutRectangle, string text)
        {
            bool flag = (base.Parent != null) ? base.Parent.RightToLeft : false;
            int dx = !flag ? layoutRectangle.Right : layoutRectangle.Left;
            int bottom = layoutRectangle.Bottom;
            switch (((base.Parent != null) ? base.Parent.ToolTipType : ToolTipType.Triangle))
            {
                case ToolTipType.Dots:
                    return;
            }
            Point point = Resco.Controls.AdvancedList.AdvancedList.point1;
            Point point2 = Resco.Controls.AdvancedList.AdvancedList.point2;
            Point point3 = Resco.Controls.AdvancedList.AdvancedList.point3;
            if (flag)
            {
                point3.X *= -1;
            }
            point.Offset(dx, bottom);
            point2.Offset(dx, bottom);
            point3.Offset(dx, bottom);
            Point[] points = new Point[] { point, point2, point3 };
            gr.FillPolygon(foreBrush, points);
            base.Parent.AddTooltipArea(new Rectangle(dx - (!flag ? Resco.Controls.AdvancedList.AdvancedList.TooltipWidth : 0), bottom - Resco.Controls.AdvancedList.AdvancedList.TooltipWidth, Resco.Controls.AdvancedList.AdvancedList.TooltipWidth + 2, Resco.Controls.AdvancedList.AdvancedList.TooltipWidth + 2), text);
        }

        public override int GetAutoHeight(Row r, int index, RowSpecificCellProperties preRscp)
        {
            int num = 0;
            if (!base.AutoHeight)
            {
                return base.Bounds.Height;
            }
            RowSpecificCellProperties properties = (preRscp == null) ? base.GetRowSpecificProperties(r) : preRscp;
            if (properties.CachedAutoHeight >= 0)
            {
                return properties.CachedAutoHeight;
            }
            string text = this.GetText(base[r, index]);
            if (((text == null) || (text.Length == 0)) || !properties.Visible.Value)
            {
                return 0;
            }
            int gridWidth = 0;
            Resco.Controls.AdvancedList.AdvancedList parent = base.Parent;
            if (parent != null)
            {
                gridWidth = parent.CalculateClientRect().Width - (parent.ScrollbarVisible ? parent.ScrollbarWidth : 0);
            }
            if (gridWidth != 0)
            {
                Rectangle rectangle = base.CalculateCellWidth(gridWidth);
                rectangle.Width -= 3;
                if (rectangle.Width > base.Parent.ScrollbarWidth)
                {
                    Graphics gr = base.Graphics;
                    bool flag = false;
                    if ((gr == null) && (base.Parent != null))
                    {
                        gr = base.Parent.CreateGraphics();
                        flag = true;
                    }
                    WrapTextData data = Utility.WrapText(gr, text, this.m_Font, rectangle.Width);
                    if ((gr != null) && flag)
                    {
                        gr.Dispose();
                    }
                    num = (data != null) ? (data.Height + 2) : 0;
                }
            }
            properties.CachedAutoHeight = num;
            return num;
        }

        protected virtual string GetText(object data)
        {
            if (this.m_format == null)
            {
                if (data == null)
                {
                    return "";
                }
                if (data is string)
                {
                    return (data as string);
                }
                return data.ToString();
            }
            return string.Format(this.m_format, data);
        }

        protected virtual bool ShouldSerializeAlignment()
        {
            return (this.m_TextAlignment != Resco.Controls.AdvancedList.Alignment.TopLeft);
        }

        [DefaultValue(10)]
        public virtual Resco.Controls.AdvancedList.Alignment Alignment
        {
            get
            {
                return this.m_TextAlignment;
            }
            set
            {
                if (this.m_TextAlignment != value)
                {
                    this.m_TextAlignment = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue("{0}")]
        public virtual string FormatString
        {
            get
            {
                if (this.m_format == null)
                {
                    return "{0}";
                }
                return this.m_format;
            }
            set
            {
                if (value == "{0}")
                {
                    value = null;
                }
                if (this.m_format != value)
                {
                    this.m_format = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public bool RestrictSelectedTextToStart
        {
            get
            {
                return this.m_restrictSelectedTextToStart;
            }
            set
            {
                this.m_restrictSelectedTextToStart = value;
                base.OnChanged(this, GridEventArgsType.Repaint, null);
            }
        }

        [DefaultValue("Highlight")]
        public Color SelectedBackColor
        {
            get
            {
                return this.m_selectedBackColor;
            }
            set
            {
                if (this.m_selectedBackColor != value)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.Highlight;
                    }
                    this.m_selectedBackColor = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue("HighlightText")]
        public Color SelectedForeColor
        {
            get
            {
                return this.m_selectedForeColor;
            }
            set
            {
                if (this.m_selectedForeColor != value)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.HighlightText;
                    }
                    this.m_selectedForeColor = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public string SelectedText
        {
            get
            {
                return this.m_selectedText;
            }
            set
            {
                if (!(this.m_selectedText == value))
                {
                    this.m_selectedText = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue("Tahoma, 8pt")]
        public virtual Font TextFont
        {
            get
            {
                return this.m_Font;
            }
            set
            {
                if (this.m_Font != value)
                {
                    this.m_Font = value;
                    base.OnChanged(this, base.AutoHeight ? GridEventArgsType.Refresh : GridEventArgsType.Repaint, new Resco.Controls.AdvancedList.AdvancedList.RefreshData(base.Owner));
                }
            }
        }

        private enum VerticalAlignment
        {
            Top,
            Middle,
            Bottom
        }
    }
}

