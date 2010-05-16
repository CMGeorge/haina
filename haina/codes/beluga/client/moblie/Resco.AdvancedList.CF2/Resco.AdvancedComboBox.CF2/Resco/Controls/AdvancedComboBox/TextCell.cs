namespace Resco.Controls.AdvancedComboBox
{
    using Resco.Controls.AdvancedComboBox.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TextCell : Cell
    {
        private Font m_Font;
        private string m_format;
        private Color m_selectedBackColor;
        private Color m_selectedForeColor;
        private string m_selectedText;
        private Resco.Controls.AdvancedComboBox.Alignment m_TextAlignment;

        public TextCell()
        {
            this.m_Font = new Font("Tahoma", 8f, FontStyle.Regular);
            this.m_TextAlignment = Resco.Controls.AdvancedComboBox.Alignment.TopLeft;
            this.m_format = null;
            this.m_selectedForeColor = SystemColors.HighlightText;
            this.m_selectedBackColor = SystemColors.Highlight;
            this.m_selectedText = "";
        }

        public TextCell(TextCell tc) : base(tc)
        {
            this.m_Font = new Font(tc.m_Font.Name, tc.m_Font.Size, tc.m_Font.Style);
            this.m_TextAlignment = tc.m_TextAlignment;
            this.m_format = tc.m_format;
            this.m_selectedForeColor = tc.m_selectedForeColor;
            this.m_selectedBackColor = tc.m_selectedBackColor;
            this.m_selectedText = tc.m_selectedText;
        }

        public override Cell Clone()
        {
            return new TextCell(this);
        }

        internal bool DrawAlignedText(Graphics gr, string text, Brush foreBrush, Rectangle layoutRectangle)
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
                case Resco.Controls.AdvancedComboBox.Alignment.MiddleCenter:
                    center = HorizontalAlignment.Center;
                    middle = VerticalAlignment.Middle;
                    break;

                case Resco.Controls.AdvancedComboBox.Alignment.MiddleRight:
                    center = HorizontalAlignment.Right;
                    middle = VerticalAlignment.Middle;
                    break;

                case Resco.Controls.AdvancedComboBox.Alignment.MiddleLeft:
                    center = HorizontalAlignment.Left;
                    middle = VerticalAlignment.Middle;
                    break;

                case Resco.Controls.AdvancedComboBox.Alignment.BottomCenter:
                    center = HorizontalAlignment.Center;
                    middle = VerticalAlignment.Bottom;
                    break;

                case Resco.Controls.AdvancedComboBox.Alignment.BottomRight:
                    center = HorizontalAlignment.Right;
                    middle = VerticalAlignment.Bottom;
                    break;

                case Resco.Controls.AdvancedComboBox.Alignment.BottomLeft:
                    center = HorizontalAlignment.Left;
                    middle = VerticalAlignment.Bottom;
                    break;

                case Resco.Controls.AdvancedComboBox.Alignment.TopCenter:
                    center = HorizontalAlignment.Center;
                    middle = VerticalAlignment.Top;
                    break;

                case Resco.Controls.AdvancedComboBox.Alignment.TopRight:
                    center = HorizontalAlignment.Right;
                    middle = VerticalAlignment.Top;
                    break;

                default:
                    center = HorizontalAlignment.Left;
                    middle = VerticalAlignment.Top;
                    break;
            }
            int num3 = -1;
            Brush brush = Resco.Controls.AdvancedComboBox.AdvancedComboBox.GetBrush(this.SelectedForeColor);
            Brush brush2 = Resco.Controls.AdvancedComboBox.AdvancedComboBox.GetBrush(this.SelectedBackColor);
            if (this.m_selectedText != "")
            {
                num3 = text.ToLower().IndexOf(this.m_selectedText.ToLower());
            }
            bool flag = true;
            WrapTextData data = Utility.WrapText(gr, text, this.m_Font, layoutRectangle.Width);
            if (data == null)
            {
                return true;
            }
            int length = data.Lines.Length;
            int lineHeight = data.LineHeight;
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
                middle = VerticalAlignment.Top;
                flag = data.Lines.Length <= length;
            }
            int y = layoutRectangle.Y;
            if (middle != VerticalAlignment.Top)
            {
                int num7 = num2 - data.Height;
                if (middle == VerticalAlignment.Middle)
                {
                    num7 /= 2;
                }
                y += num7;
            }
            int x = layoutRectangle.X;
            int num9 = 0;
            int index = 0;
            if (y < 0)
            {
                int num11 = -y / lineHeight;
                y += num11 * lineHeight;
                index += num11;
            }
            int clientSizeHeight = base.Parent.GetClientSizeHeight();
            int num13 = -1;
            int num14 = 0;
            int num16 = this.m_selectedText.Length;
            while (index < length)
            {
                string str2;
                int width;
                DrawLineData data2 = data.Lines[index];
                if (!flag && (index == (length - 1)))
                {
                    int cutLength = data2.CutLength;
                    if ((data2.Index + cutLength) > text.Length)
                    {
                        cutLength = data2.Length;
                    }
                    str2 = text.Substring(data2.Index, cutLength);
                    width = (cutLength == data2.Length) ? data2.Width : num;
                }
                else
                {
                    str2 = text.Substring(data2.Index, data2.Length);
                    width = data2.Width;
                }
                if (center != HorizontalAlignment.Left)
                {
                    num9 = num - width;
                    if (center == HorizontalAlignment.Center)
                    {
                        num9 /= 2;
                    }
                }
                if (((num3 >= 0) && ((num3 + (num14 = num16)) > (num13 = data2.Index))) && (num3 < (num13 + str2.Length)))
                {
                    string str;
                    int num15;
                    int num19 = num3 - num13;
                    if (num19 > 0)
                    {
                        str = str2.Substring(0, num19);
                        this.DrawTextLine(gr, str, font, foreBrush, x + num9, y, width, lineHeight, data2.Index);
                        num15 = (int) gr.MeasureString(str, font).Width;
                        if ((num19 + num14) < str2.Length)
                        {
                            str = str2.Substring(num19, num14);
                            int num20 = (int) gr.MeasureString(str, font).Width;
                            gr.FillRectangle(brush2, (x + num9) + num15, y, num20, lineHeight);
                            this.DrawTextLine(gr, str, font, brush, (x + num9) + num15, y, width - num15, lineHeight, data2.Index);
                            num15 += num20;
                            str = str2.Substring(num19 + num14, str2.Length - (num19 + num14));
                            this.DrawTextLine(gr, str, font, foreBrush, (x + num9) + num15, y, width - num15, lineHeight, data2.Index);
                        }
                        else
                        {
                            str = str2.Substring(num19, (num13 + str2.Length) - num3);
                            gr.FillRectangle(brush2, (x + num9) + num15, y, width - num15, lineHeight);
                            this.DrawTextLine(gr, str, font, brush, (x + num9) + num15, y, width - num15, lineHeight, data2.Index);
                        }
                    }
                    else
                    {
                        if ((num19 + num14) < str2.Length)
                        {
                            str = str2.Substring(0, num19 + num14);
                        }
                        else
                        {
                            str = str2;
                        }
                        num15 = (int) gr.MeasureString(str, font).Width;
                        gr.FillRectangle(brush2, x + num9, y, num15, lineHeight);
                        this.DrawTextLine(gr, str, font, brush, x + num9, y, width, lineHeight, data2.Index);
                        if ((num19 + num14) < str2.Length)
                        {
                            str = str2.Substring(num14 + num19, str2.Length - (num14 + num19));
                            this.DrawTextLine(gr, str, font, foreBrush, (x + num9) + num15, y, width - num15, lineHeight, data2.Index);
                        }
                    }
                }
                else
                {
                    this.DrawTextLine(gr, str2, font, foreBrush, x + num9, y, width, lineHeight, data2.Index);
                }
                y += lineHeight;
                data2 = null;
                if (y > clientSizeHeight)
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
            Brush foreBrush = Resco.Controls.AdvancedComboBox.AdvancedComboBox.GetBrush(this.GetColor(ColorCategory.Foreground));
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
            Point point = Resco.Controls.AdvancedComboBox.AdvancedComboBox.point1;
            Point point2 = Resco.Controls.AdvancedComboBox.AdvancedComboBox.point2;
            Point point3 = Resco.Controls.AdvancedComboBox.AdvancedComboBox.point3;
            if (flag)
            {
                point3.X *= -1;
            }
            int dx = !flag ? layoutRectangle.Right : layoutRectangle.Left;
            int bottom = layoutRectangle.Bottom;
            point.Offset(dx, bottom);
            point2.Offset(dx, bottom);
            point3.Offset(dx, bottom);
            Point[] points = new Point[] { point, point2, point3 };
            gr.FillPolygon(foreBrush, points);
            base.Parent.AddTooltipArea(new Rectangle(dx - (!flag ? Resco.Controls.AdvancedComboBox.AdvancedComboBox.TooltipWidth : 0), bottom - Resco.Controls.AdvancedComboBox.AdvancedComboBox.TooltipWidth, Resco.Controls.AdvancedComboBox.AdvancedComboBox.TooltipWidth + 2, Resco.Controls.AdvancedComboBox.AdvancedComboBox.TooltipWidth + 2), text);
        }

        public override int GetAutoHeight(Resco.Controls.AdvancedComboBox.ListItem item, int index, ItemSpecificCellProperties preRscp)
        {
            int num = 0;
            if (!base.AutoHeight)
            {
                return base.Bounds.Height;
            }
            ItemSpecificCellProperties properties = (preRscp == null) ? base.GetItemSpecificProperties(item) : preRscp;
            if (properties.CachedAutoHeight >= 0)
            {
                return properties.CachedAutoHeight;
            }
            string text = this.GetText(base[item, index]);
            if ((text == null) || (text.Length == 0))
            {
                return 0;
            }
            int gridWidth = 0;
            if (base.Parent != null)
            {
                gridWidth = base.Parent.List.Width - (base.Parent.List.ScrollbarVisible ? base.Parent.List.ClientScrollbarWidth : 0);
            }
            if (gridWidth != 0)
            {
                Rectangle rectangle = base.CalculateCellWidth(gridWidth);
                rectangle.Width -= 3;
                if (rectangle.Width > base.Parent.List.ClientScrollbarWidth)
                {
                    WrapTextData data = Utility.WrapText(base.Graphics, text, this.m_Font, rectangle.Width);
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
            return (this.m_TextAlignment != Resco.Controls.AdvancedComboBox.Alignment.TopLeft);
        }

        [DefaultValue(10)]
        public virtual Resco.Controls.AdvancedComboBox.Alignment Alignment
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
                    base.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
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
                    if (base.AutoHeight)
                    {
                        base.OnChanged(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(base.Owner));
                    }
                    else
                    {
                        base.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                    }
                }
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
                    base.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
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
                    base.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                }
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedComboBox.Design.Browsable(false)]
        public string SelectedText
        {
            get
            {
                return this.m_selectedText;
            }
            set
            {
                if (this.m_selectedText != value)
                {
                    this.m_selectedText = value;
                    base.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
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
                    if (base.AutoHeight)
                    {
                        base.OnChanged(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(base.Owner));
                    }
                    else
                    {
                        base.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                    }
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

