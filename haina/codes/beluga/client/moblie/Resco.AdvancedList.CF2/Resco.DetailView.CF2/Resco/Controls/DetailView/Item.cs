namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Forms;

    public class Item : IComponent, IDisposable
    {
        public static Font DefaultFont = new Font("Tahoma", 8f, FontStyle.Regular);
        internal bool DisableEvents;
        internal bool DisableRefresh;
        public static int ItemRoundedCornerSize = 6;
        public static int ItemSpacing = 0;
        private bool m_AutoHeight;
        private bool m_bFocusing;
        internal bool m_bHasError;
        private bool m_bHiding;
        private bool m_bLabelToolTip;
        internal Resco.Controls.DetailView.ItemBorder m_border;
        private bool m_bValidating;
        private bool m_bVisible;
        private string m_dataMember;
        private bool m_Enabled;
        private Resco.Controls.DetailView.ErrorBackground m_ErrorBackground;
        private string m_ErrorMessage;
        private string m_Label;
        private HorizontalAlignment m_LabelAlignment;
        private bool m_LabelAutoHeight;
        internal SolidBrush m_LabelBackBrush;
        private Color m_LabelBackColor;
        private Font m_LabelFont;
        internal SolidBrush m_LabelForeBrush;
        private Color m_LabelForeColor;
        private int m_LabelHeight;
        private bool m_LabelTooLong;
        private int m_LabelWidth;
        private VerticalAlignment m_lineAlign;
        private string m_Name;
        private bool m_NewLine;
        private Resco.Controls.DetailView.DetailView m_Parent;
        private Pen m_Pen;
        private Color m_PreviousColor;
        private SolidBrush m_RedBrush;
        private Color m_RedColor;
        private RoundedCornerStyles m_roundedCorner;
        private ISite m_site;
        private Size m_Size;
        private RescoItemStyle m_Style;
        private object m_Tag;
        protected string m_Text;
        private HorizontalAlignment m_TextAlign;
        internal SolidBrush m_TextBackBrush;
        private Color m_TextBackColor;
        private Font m_TextFont;
        private SolidBrush m_TextForeBrush;
        private Color m_TextForeColor;
        protected object m_Value;

        public event ItemEventHandler Changed;

        public event ItemEventHandler Clicked;

        public event EventHandler Disposed;

        public event ItemEventHandler GotFocus;

        public event ItemEventHandler LabelClicked;

        public event ItemEventHandler LostFocus;

        public event ItemValidatingEventHandler Validating;

        public Item()
        {
            this.m_border = Resco.Controls.DetailView.ItemBorder.Underline;
            this.m_dataMember = "";
            this.m_bVisible = true;
            this.m_LabelWidth = -1;
            this.m_Name = "";
            this.m_Parent = null;
            this.Changed = null;
            this.DisableEvents = false;
            this.DisableRefresh = false;
            this.Tag = null;
            this.m_Style = RescoItemStyle.LabelLeft;
            this.m_lineAlign = VerticalAlignment.Top;
            this.m_roundedCorner = RoundedCornerStyles.None;
            this.Label = "";
            this.LabelToolTip = false;
            this.m_LabelTooLong = false;
            this.Enabled = true;
            this.EditValue = null;
            this.m_Size = new Size(-1, 0x10);
            this.m_NewLine = true;
            this.m_LabelHeight = 0x10;
            this.m_LabelAutoHeight = false;
            this.ErrorMessage = null;
            this.m_LabelAlignment = HorizontalAlignment.Right;
            this.m_LabelFont = DefaultFont;
            this.m_TextFont = DefaultFont;
            this.m_LabelBackColor = Color.Transparent;
            this.m_LabelForeColor = Color.Black;
            this.m_TextBackColor = Color.Transparent;
            this.m_PreviousColor = Color.Transparent;
            this.m_TextForeColor = Color.Black;
            this.m_TextAlign = HorizontalAlignment.Left;
            this.m_LabelBackBrush = new SolidBrush(Color.Transparent);
            this.m_LabelForeBrush = new SolidBrush(Color.Black);
            this.m_TextBackBrush = new SolidBrush(Color.Transparent);
            this.m_TextForeBrush = new SolidBrush(Color.Black);
            this.m_Pen = new Pen(Color.Gray);
            this.m_RedColor = Color.Red;
            this.m_RedBrush = new SolidBrush(this.m_RedColor);
            this.m_ErrorBackground = Resco.Controls.DetailView.ErrorBackground.Background;
            this.m_Text = "";
        }

        public Item(Item toCopy)
        {
            this.m_border = Resco.Controls.DetailView.ItemBorder.Underline;
            this.m_dataMember = "";
            this.m_bVisible = true;
            this.m_LabelWidth = -1;
            this.m_Name = "";
            this.m_Parent = null;
            this.Changed = null;
            this.DisableEvents = false;
            this.DisableRefresh = false;
            this.Tag = null;
            this.Style = toCopy.Style;
            this.m_lineAlign = toCopy.LineAlign;
            this.m_roundedCorner = toCopy.RoundedCorner;
            this.Label = toCopy.Label;
            this.Enabled = toCopy.Enabled;
            this.m_Size = toCopy.m_Size;
            this.m_NewLine = toCopy.m_NewLine;
            this.m_AutoHeight = toCopy.AutoHeight;
            this.m_LabelHeight = toCopy.LabelHeight;
            this.m_LabelWidth = toCopy.m_LabelWidth;
            this.m_LabelAutoHeight = toCopy.m_LabelAutoHeight;
            this.ErrorMessage = null;
            this.m_LabelAlignment = toCopy.LabelAlignment;
            this.m_LabelFont = toCopy.LabelFont;
            this.m_TextFont = toCopy.TextFont;
            this.m_LabelBackColor = toCopy.LabelBackColor;
            this.m_LabelForeColor = toCopy.LabelForeColor;
            this.m_TextBackColor = toCopy.TextBackColor;
            this.m_PreviousColor = this.m_TextBackColor;
            this.m_TextForeColor = toCopy.TextForeColor;
            this.m_TextAlign = toCopy.TextAlign;
            this.m_RedColor = toCopy.m_RedColor;
            this.m_ErrorBackground = toCopy.ErrorBackground;
            this.m_LabelBackBrush = new SolidBrush(this.m_LabelBackColor);
            this.m_LabelForeBrush = new SolidBrush(this.m_LabelForeColor);
            this.m_TextBackBrush = new SolidBrush(this.m_TextBackColor);
            this.m_TextForeBrush = new SolidBrush(this.m_TextForeColor);
            this.m_Pen = new Pen(Color.Gray);
            this.m_RedBrush = new SolidBrush(this.m_RedColor);
            this.m_dataMember = toCopy.DataMember;
            this.m_Name = toCopy.Name;
            this.m_border = toCopy.m_border;
        }

        internal void _Click(int yOffset, int parentWidth, bool useClickVisualize)
        {
            this.m_bFocusing = true;
            this.OnClick(yOffset + ItemSpacing, parentWidth, useClickVisualize);
            this.m_bFocusing = false;
        }

        internal void _Draw(Graphics gr, int yOffset, int parentWidth)
        {
            if (this.Visible)
            {
                this.Draw(gr, yOffset + ItemSpacing, parentWidth);
            }
        }

        internal void _Hide()
        {
            if (!this.m_bHiding)
            {
                try
                {
                    this.m_bHiding = true;
                    this.Hide();
                }
                finally
                {
                    this.m_bHiding = false;
                }
            }
        }

        internal void _LabelClick(int yOffset, int parentWidth, bool useClickVisualize)
        {
            this.OnLabelClick(yOffset + ItemSpacing, parentWidth, useClickVisualize);
        }

        internal virtual Size _MeasureTextSize(Graphics gr, Font font, string text, int width, bool wrap, bool label)
        {
            return this.MeasureTextSize(gr, font, text, width, wrap);
        }

        internal void _MouseDown(int yOffset, int parentWidth, MouseEventArgs e)
        {
            this.MouseDown(yOffset + ItemSpacing, parentWidth, e);
        }

        internal void _MouseUp(int yOffset, int parentWidth, MouseEventArgs e)
        {
            this.MouseUp(yOffset + ItemSpacing, parentWidth, e);
        }

        internal void _MoveTop(int offset)
        {
            this.MoveTop(offset);
        }

        internal void Activate(bool useClickVisualize)
        {
            if (this.Parent != null)
            {
                Rectangle rectangle = this.Parent.CalculateClientRect();
                int num = -this.Parent.ScrollOffset;
                int num2 = 0;
                for (int i = 0; i < this.Parent.CurrentPage.Count; i++)
                {
                    Item item = this.Parent.CurrentPage[i];
                    if (item == this)
                    {
                        break;
                    }
                    int itemHeight = item.ItemHeight;
                    if ((i == 0) || item.NewLine)
                    {
                        num2 = itemHeight;
                    }
                    if ((i == (this.Parent.CurrentPage.Count - 1)) || this.Parent.CurrentPage[i + 1].NewLine)
                    {
                        num += num2;
                    }
                }
                if ((num + this.ItemHeight) > (rectangle.Height - this.Parent.PagerHeight))
                {
                    num = this.Parent.ScrollTo(num + this.Parent.ScrollOffset);
                }
                if (num < 0)
                {
                    num = this.Parent.ScrollTo(num + this.Parent.ScrollOffset);
                }
                num += this.Parent.HasPages ? ((this.Parent.PagesLocation == RescoPagesLocation.Bottom) ? 0 : this.Parent.PagerHeight) : 0;
                this._Click(num + rectangle.Y, this.Parent.UsableWidth, useClickVisualize);
            }
        }

        internal virtual bool CheckForToolTip(int x, int y, int itemX, int itemWidth, bool bShow)
        {
            if (this.Parent != null)
            {
                Rectangle rectangle = this.Parent.CalculateClientRect();
                if (((this.ErrorMessage != null) && (this.ErrorMessage != "")) && (((this.m_Style == RescoItemStyle.LabelRight) && ((x - rectangle.X) < (itemX + Resco.Controls.DetailView.DetailView.ErrorSpacer))) || ((this.m_Style != RescoItemStyle.LabelRight) && ((x - rectangle.X) > ((itemX + itemWidth) - Resco.Controls.DetailView.DetailView.ErrorSpacer)))))
                {
                    this.Parent.ShowToolTip(this.ErrorMessage, x, y, bShow);
                    return true;
                }
                if (this.m_LabelTooLong)
                {
                    Rectangle labelBounds = this.GetLabelBounds(0);
                    if ((x > (labelBounds.Right - Resco.Controls.DetailView.DetailView.TooltipWidth)) && (x < labelBounds.Right))
                    {
                        this.Parent.ShowToolTip(this.Label, x, y, bShow);
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual void ClearContents()
        {
            this.SetText("", true);
            this.SetValue(null, true);
        }

        protected virtual void Click(int yOffset, int parentWidth)
        {
            this.OnClicked(this, new ItemEventArgs(this, 0, this.Name));
        }

        public virtual Item Clone()
        {
            Item item = new Item(this);
            item.Value = this.Value;
            item.Text = this.Text;
            return item;
        }

        protected void DefaultDraw(Graphics gr, int yOffset, int parentWidth)
        {
            Resco.Controls.DetailView.DetailView parent = this.Parent;
            if (parent != null)
            {
                int width;
                Rectangle rectangle2;
                Rectangle rectangle3;
                int num6;
                int num7;
                int num8;
                int num9;
                Rectangle rectangle = parent.CalculateClientRect();
                int internalLabelWidth = this.InternalLabelWidth;
                int separatorWidth = parent.SeparatorWidth;
                int height = this.m_Size.Height;
                int labelHeight = 0;
                Point itemXWidth = this.Parent.GetItemXWidth(this);
                int itemLeft = rectangle.X + itemXWidth.X;
                int y = itemXWidth.Y;
                if (this.m_Style == RescoItemStyle.LabelLeft)
                {
                    rectangle2 = new Rectangle(itemLeft + Resco.Controls.DetailView.DetailView.HorizontalSpacer, yOffset, (y < internalLabelWidth) ? y : internalLabelWidth, height);
                    num8 = yOffset;
                    width = y - (((Resco.Controls.DetailView.DetailView.HorizontalSpacer + rectangle2.Width) + separatorWidth) + Resco.Controls.DetailView.DetailView.ErrorSpacer);
                    if (width < 0)
                    {
                        num6 = itemLeft + rectangle2.Width;
                        num7 = num6;
                    }
                    else
                    {
                        num6 = rectangle2.Right + separatorWidth;
                        num7 = num6 + width;
                    }
                    width = num7 - num6;
                    rectangle3 = new Rectangle(num6 + DVTextBox.BorderSize, num8, width - (2 * DVTextBox.BorderSize), height - 1);
                    num9 = (num8 + height) - 1;
                }
                else if (this.m_Style == RescoItemStyle.LabelRight)
                {
                    int num12 = y - internalLabelWidth;
                    rectangle2 = new Rectangle((itemLeft + ((num12 < 0) ? 0 : num12)) - Resco.Controls.DetailView.DetailView.HorizontalSpacer, yOffset, (num12 < 0) ? y : internalLabelWidth, height);
                    num6 = itemLeft + Resco.Controls.DetailView.DetailView.ErrorSpacer;
                    num8 = yOffset;
                    width = y - (((Resco.Controls.DetailView.DetailView.HorizontalSpacer + rectangle2.Width) + separatorWidth) + Resco.Controls.DetailView.DetailView.ErrorSpacer);
                    if (width < 0)
                    {
                        num7 = num6;
                    }
                    else
                    {
                        num7 = num6 + width;
                    }
                    width = num7 - num6;
                    rectangle3 = new Rectangle(num6 + DVTextBox.BorderSize, num8, width - (2 * DVTextBox.BorderSize), height - 1);
                    num9 = (num8 + height) - 1;
                }
                else
                {
                    labelHeight = this.m_LabelHeight;
                    rectangle2 = new Rectangle(itemLeft + Resco.Controls.DetailView.DetailView.HorizontalSpacer, yOffset, y - (Resco.Controls.DetailView.DetailView.ErrorSpacer + Resco.Controls.DetailView.DetailView.HorizontalSpacer), labelHeight);
                    if (rectangle2.Width < 0)
                    {
                        rectangle2.Width = 0;
                    }
                    num8 = yOffset + labelHeight;
                    num6 = itemLeft + Resco.Controls.DetailView.DetailView.HorizontalSpacer;
                    width = rectangle2.Width;
                    num7 = num6 + width;
                    rectangle3 = new Rectangle(num6 + DVTextBox.BorderSize, num8, width - (2 * DVTextBox.BorderSize), height - 1);
                    num9 = (num8 + height) - 1;
                }
                this.DrawBackground(gr, itemLeft, yOffset, y, height + labelHeight, num6, num8, width, height);
                this.DrawItemLabelArea(gr, rectangle2);
                this.DrawItemTextArea(gr, ref rectangle3);
                if (this.m_bHasError && (this.ErrorBackground == Resco.Controls.DetailView.ErrorBackground.Border))
                {
                    this.m_Pen.Color = this.ErrorColor;
                }
                else
                {
                    this.m_Pen.Color = Color.Gray;
                }
                if (this.m_border == Resco.Controls.DetailView.ItemBorder.Underline)
                {
                    gr.DrawLine(this.m_Pen, num6, num9, num7, num9);
                }
                else if (this.m_border == Resco.Controls.DetailView.ItemBorder.Flat)
                {
                    gr.DrawRectangle(this.m_Pen, num6, num8, width, height - 1);
                }
            }
        }

        public void Dispose()
        {
            if (this.Disposed != null)
            {
                this.Disposed(this, EventArgs.Empty);
            }
        }

        protected virtual void Draw(Graphics gr, int yOffset, int parentWidth)
        {
            this.DefaultDraw(gr, yOffset, parentWidth);
        }

        protected void DrawAlignmentString(Graphics gr, string text, Font font, Brush foreBrush, Rectangle layoutRectangle, HorizontalAlignment alignment, VerticalAlignment valign)
        {
            this.DrawAlignmentString(gr, text, font, foreBrush, layoutRectangle, alignment, valign, false);
        }

        protected bool DrawAlignmentString(Graphics gr, string text, Font font, Brush foreBrush, Rectangle layoutRectangle, HorizontalAlignment alignment, VerticalAlignment valign, bool wrap)
        {
            int num;
            int num2;
            if (((text == null) || (text.Length == 0)) || (((num = layoutRectangle.Width) < 7) || ((num2 = layoutRectangle.Height) < 3)))
            {
                return true;
            }
            layoutRectangle.X += DVTextBox.BorderSize;
            num -= 2 * DVTextBox.BorderSize;
            layoutRectangle.Y += DVTextBox.BorderSize;
            num2 -= 2 * DVTextBox.BorderSize;
            SizeF ef = gr.MeasureString(text, font);
            float width = ef.Width;
            bool flag = true;
            if (width <= num)
            {
                if (text.IndexOf('\n') == -1)
                {
                    float num4 = ef.Height;
                    float num5 = layoutRectangle.Y;
                    if (valign != VerticalAlignment.Top)
                    {
                        float num6 = Math.Max((float) 0f, (float) (num2 - num4));
                        if (valign == VerticalAlignment.Middle)
                        {
                            num6 /= 2f;
                        }
                        num5 += num6;
                    }
                    float num7 = layoutRectangle.X;
                    if (alignment != HorizontalAlignment.Left)
                    {
                        float num8 = num - width;
                        if (alignment == HorizontalAlignment.Center)
                        {
                            num8 /= 2f;
                        }
                        num7 += num8;
                    }
                    gr.DrawString(text, font, foreBrush, num7, num5);
                    return flag;
                }
                string[] strArray = text.Replace("\r", "").Split(new char[] { '\n' });
                int num9 = strArray.Length;
                if ((num9 > 1) && (strArray[num9 - 1] == ""))
                {
                    num9--;
                }
                float height = ef.Height;
                float num11 = height / ((float) num9);
                if (height > num2)
                {
                    num9 = (int) Math.Floor((double) (((float) num2) / num11));
                    if (num9 < 1)
                    {
                        num9 = 1;
                    }
                    flag = false;
                }
                float num12 = layoutRectangle.Y;
                if (valign != VerticalAlignment.Top)
                {
                    float num13 = num2 - (num11 * num9);
                    if (valign == VerticalAlignment.Middle)
                    {
                        num13 /= 2f;
                    }
                    num12 += num13;
                }
                float num14 = layoutRectangle.X;
                float num15 = 0f;
                for (int k = 0; k < num9; k++)
                {
                    string str = strArray[k];
                    if (alignment != HorizontalAlignment.Left)
                    {
                        ef = gr.MeasureString(str, font);
                        num15 = num - ef.Width;
                        if (alignment == HorizontalAlignment.Center)
                        {
                            num15 /= 2f;
                        }
                    }
                    gr.DrawString(str, font, foreBrush, num14 + num15, num12);
                    num12 += num11;
                }
                return flag;
            }
            string[] strArray2 = text.Replace("\r", "").Split(new char[] { '\n' });
            int length = strArray2.Length;
            if ((length > 1) && (strArray2[length - 1] == ""))
            {
                length--;
            }
            float num19 = ef.Height / ((float) length);
            int num20 = (int) Math.Floor((double) (((float) num2) / num19));
            if (num20 < 1)
            {
                num20 = 1;
            }
            string[] strArray3 = new string[num20];
            float[] numArray = new float[num20];
            int index = 0;
            num20--;
            for (int i = 0; i < length; i++)
            {
                string str2 = strArray2[i];
                float num23 = gr.MeasureString(str2, font).Width;
                if (num23 <= num)
                {
                    strArray3[index] = str2;
                    numArray[index] = num23;
                    if ((++index <= num20) || (i >= (length - 1)))
                    {
                        continue;
                    }
                    flag = false;
                    break;
                }
                flag = false;
                int num24 = str2.Length;
                int num25 = (int) (((float) (num24 * num)) / num23);
                if (num25 < 1)
                {
                    strArray3[index] = str2.Substring(0, 1);
                    numArray[index] = num;
                    if (wrap && (num24 > 1))
                    {
                        flag = true;
                        strArray2[i] = str2.Substring(1, num24 - 1);
                        i--;
                    }
                }
                else
                {
                    int startIndex = 0;
                    bool flag2 = !wrap && (alignment == HorizontalAlignment.Right);
                    if (flag2)
                    {
                        startIndex = num24 - num25;
                    }
                    StringBuilder builder = new StringBuilder(str2.Substring(startIndex, num25));
                    ef = gr.MeasureString(builder.ToString(), font);
                    float num27 = ef.Width;
                    if (num27 > num)
                    {
                        do
                        {
                            num25--;
                            if (num25 <= 0)
                            {
                                break;
                            }
                            builder.Remove(flag2 ? 0 : num25, 1);
                            num27 = gr.MeasureString(builder.ToString(), font).Width;
                        }
                        while (num27 > num);
                    }
                    else
                    {
                        do
                        {
                            num27 = ef.Width;
                            num25++;
                            if (num25 >= num24)
                            {
                                break;
                            }
                            if (flag2)
                            {
                                builder.Insert(0, str2.Substring(num24 - num25, 1));
                            }
                            else
                            {
                                builder.Append(str2[num25 - 1]);
                            }
                            ef = gr.MeasureString(builder.ToString(), font);
                        }
                        while (ef.Width <= num);
                        if (num25 != num24)
                        {
                            builder.Remove(flag2 ? 0 : (num25 - 1), 1);
                        }
                    }
                    int count = builder.Length;
                    if (wrap && (index < num20))
                    {
                        flag = true;
                        if (count < num24)
                        {
                            if (char.IsWhiteSpace(str2[count]))
                            {
                                count++;
                            }
                            else
                            {
                                int num29 = str2.LastIndexOfAny(new char[] { ' ', '\t' }, count - 1, count);
                                if (num29 > 0)
                                {
                                    builder.Remove(num29, count - num29);
                                    count = num29 + 1;
                                }
                            }
                        }
                        if (count < num24)
                        {
                            strArray2[i] = str2.Substring(count, num24 - count);
                            num27 = gr.MeasureString(builder.ToString(), font).Width;
                            i--;
                        }
                    }
                    strArray3[index] = builder.ToString();
                    numArray[index] = num27;
                }
                if ((++index > num20) && (i < (length - 1)))
                {
                    flag = false;
                    break;
                }
            }
            float y = layoutRectangle.Y;
            if (valign != VerticalAlignment.Top)
            {
                float num31 = num2 - (num19 * index);
                if (valign == VerticalAlignment.Middle)
                {
                    num31 /= 2f;
                }
                y += num31;
            }
            float x = layoutRectangle.X;
            float num33 = 0f;
            for (int j = 0; j < index; j++)
            {
                string s = strArray3[j];
                if (alignment != HorizontalAlignment.Left)
                {
                    num33 = num - numArray[j];
                    if (alignment == HorizontalAlignment.Center)
                    {
                        num33 /= 2f;
                    }
                }
                gr.DrawString(s, font, foreBrush, x + num33, y);
                y += num19;
            }
            return flag;
        }

        protected virtual void DrawBackground(Graphics gr, int x, int y, int textWidth, int labelWidth, int separWidth, int height)
        {
            switch (this.Style)
            {
                case RescoItemStyle.LabelTop:
                    this.DrawBackground(gr, x, y, this.m_Parent.UsableWidth, this.Height, x + Resco.Controls.DetailView.DetailView.HorizontalSpacer, y + this.LabelHeight, textWidth, height);
                    return;

                case RescoItemStyle.LabelRight:
                    this.DrawBackground(gr, x, y, this.m_Parent.UsableWidth, height, x + Resco.Controls.DetailView.DetailView.ErrorSpacer, y, textWidth, height);
                    return;
            }
            this.DrawBackground(gr, x, y, this.m_Parent.UsableWidth, height, (x + labelWidth) + separWidth, y, textWidth, height);
        }

        protected virtual void DrawBackground(Graphics gr, int itemLeft, int itemTop, int itemWidth, int itemHeight, int textLeft, int textTop, int textWidth, int textHeight)
        {
            if ((this.m_LabelBackColor != Color.Transparent) || !this.Parent.UseGradient)
            {
                this.DrawRoundedRectangle(gr, this.m_LabelBackBrush, itemLeft, itemTop, itemWidth, itemHeight, this.RoundedCorner);
            }
            if (!this.m_bHasError)
            {
                if ((this.m_TextBackColor != Color.Transparent) || !this.Parent.UseGradient)
                {
                    gr.FillRectangle(this.m_TextBackBrush, textLeft, textTop, textWidth + 1, textHeight);
                }
            }
            else
            {
                if (this.m_ErrorBackground == Resco.Controls.DetailView.ErrorBackground.Background)
                {
                    gr.FillRectangle(this.m_RedBrush, textLeft, textTop, textWidth + 1, textHeight);
                }
                else if ((this.m_TextBackColor != Color.Transparent) || !this.Parent.UseGradient)
                {
                    gr.FillRectangle(this.m_TextBackBrush, textLeft, textTop, textWidth + 1, textHeight);
                }
                int x = ((this.m_Style == RescoItemStyle.LabelRight) ? itemLeft : (textLeft + textWidth)) + ((Resco.Controls.DetailView.DetailView.ErrorSpacer / 2) - 1);
                gr.FillRectangle(this.m_RedBrush, x, textTop + 2, 2, Resco.Controls.DetailView.DetailView.ErrorSpacer);
                gr.FillRectangle(this.m_RedBrush, x, (textTop + 4) + Resco.Controls.DetailView.DetailView.ErrorSpacer, 2, 2);
            }
        }

        protected virtual void DrawItemLabelArea(Graphics gr, Rectangle labelBounds)
        {
            if (!this.DrawAlignmentString(gr, this.Label, this.LabelFont, this.m_LabelForeBrush, labelBounds, this.LabelAlignment, this.LineAlign, true) && this.LabelToolTip)
            {
                int right = labelBounds.Right;
                int bottom = labelBounds.Bottom;
                Point[] points = new Point[] { new Point(right, bottom - Resco.Controls.DetailView.DetailView.TooltipWidth), new Point(right - Resco.Controls.DetailView.DetailView.TooltipWidth, bottom), new Point(right, bottom) };
                gr.FillPolygon(this.GetTextForeBrush(), points);
                this.m_LabelTooLong = true;
            }
            else
            {
                this.m_LabelTooLong = false;
            }
        }

        protected virtual void DrawItemTextArea(Graphics gr, ref Rectangle textBounds)
        {
            this.DrawAlignmentString(gr, this.Text, this.TextFont, this.GetTextForeBrush(), textBounds, this.TextAlign, this.LineAlign, true);
        }

        protected void DrawRoundedRectangle(Graphics gr, Brush brush, int x, int y, int width, int height, RoundedCornerStyles corners)
        {
            if (corners == RoundedCornerStyles.None)
            {
                gr.FillRectangle(brush, x, y, width, height);
            }
            else
            {
                int num = ItemRoundedCornerSize * 2;
                Rectangle rect = new Rectangle(x, y, width, ItemRoundedCornerSize);
                if ((corners & RoundedCornerStyles.TopLeft) == RoundedCornerStyles.TopLeft)
                {
                    Rectangle rectangle2 = new Rectangle(x, y, num - 1, num - 1);
                    gr.FillEllipse(brush, rectangle2);
                    rect.X += ItemRoundedCornerSize;
                    rect.Width -= ItemRoundedCornerSize;
                }
                if ((corners & RoundedCornerStyles.TopRight) == RoundedCornerStyles.TopRight)
                {
                    Rectangle rectangle3 = new Rectangle((x + width) - num, y, num - 1, num - 1);
                    gr.FillEllipse(brush, rectangle3);
                    rect.Width -= ItemRoundedCornerSize;
                }
                gr.FillRectangle(brush, rect);
                gr.FillRectangle(brush, x, y + ItemRoundedCornerSize, width, height - num);
                rect = new Rectangle(x, (y + height) - ItemRoundedCornerSize, width, ItemRoundedCornerSize);
                if ((corners & RoundedCornerStyles.BottomLeft) == RoundedCornerStyles.BottomLeft)
                {
                    Rectangle rectangle4 = new Rectangle(x, (y + height) - num, num - 1, num - 1);
                    gr.FillEllipse(brush, rectangle4);
                    rect.X += ItemRoundedCornerSize;
                    rect.Width -= ItemRoundedCornerSize;
                }
                if ((corners & RoundedCornerStyles.BottomRight) == RoundedCornerStyles.BottomRight)
                {
                    Rectangle rectangle5 = new Rectangle((x + width) - num, (y + height) - num, num - 1, num - 1);
                    gr.FillEllipse(brush, rectangle5);
                    rect.Width -= ItemRoundedCornerSize;
                }
                gr.FillRectangle(brush, rect);
            }
        }

        protected virtual string FormatValue(object value)
        {
            return Convert.ToString(value);
        }

        protected virtual Rectangle GetActivePartBounds(int yOffset)
        {
            Resco.Controls.DetailView.DetailView parent = this.Parent;
            if (parent == null)
            {
                return Rectangle.Empty;
            }
            Rectangle rectangle = parent.CalculateClientRect();
            Point itemXWidth = parent.GetItemXWidth(this);
            int num = this.InternalLabelWidth + parent.SeparatorWidth;
            if (this.m_Style == RescoItemStyle.LabelLeft)
            {
                int num2 = Resco.Controls.DetailView.DetailView.HorizontalSpacer + ((itemXWidth.Y < num) ? itemXWidth.Y : num);
                int num3 = ((itemXWidth.Y - num2) - Resco.Controls.DetailView.DetailView.ErrorSpacer) + 1;
                return new Rectangle((rectangle.X + itemXWidth.X) + num2, yOffset, (num3 < 0) ? 0 : num3, this.Height);
            }
            if (this.m_Style == RescoItemStyle.LabelRight)
            {
                int num4 = Resco.Controls.DetailView.DetailView.HorizontalSpacer + ((itemXWidth.Y < num) ? itemXWidth.Y : num);
                int num5 = ((itemXWidth.Y - num4) - Resco.Controls.DetailView.DetailView.ErrorSpacer) + 1;
                return new Rectangle((rectangle.X + itemXWidth.X) + Resco.Controls.DetailView.DetailView.ErrorSpacer, yOffset, (num5 < 0) ? 0 : num5, this.Height);
            }
            int horizontalSpacer = Resco.Controls.DetailView.DetailView.HorizontalSpacer;
            return new Rectangle((rectangle.X + itemXWidth.X) + horizontalSpacer, yOffset + this.LabelHeight, (itemXWidth.Y - (horizontalSpacer + Resco.Controls.DetailView.DetailView.ErrorSpacer)) + 1, this.Height);
        }

        protected virtual int GetActiveWidth()
        {
            if (this.Parent == null)
            {
                return 0;
            }
            Point itemXWidth = this.Parent.GetItemXWidth(this);
            int horizontalSpacer = Resco.Controls.DetailView.DetailView.HorizontalSpacer;
            if ((this.m_Style == RescoItemStyle.LabelLeft) || (this.m_Style == RescoItemStyle.LabelRight))
            {
                int num2 = this.InternalLabelWidth + this.Parent.SeparatorWidth;
                horizontalSpacer = (itemXWidth.Y < num2) ? itemXWidth.Y : num2;
            }
            return (((itemXWidth.Y - horizontalSpacer) - Resco.Controls.DetailView.DetailView.ErrorSpacer) + 1);
        }

        private void GetAutoHeight()
        {
            if (this.Parent != null)
            {
                using (Graphics graphics = this.Parent.CreateGraphics())
                {
                    string text = ((this.m_Text == null) || (this.m_Text == "")) ? "0" : this.m_Text;
                    int num = this._MeasureTextSize(graphics, this.m_TextFont, text, this.GetActiveWidth(), true, false).Height + 1;
                    int minimumHeight = this.GetMinimumHeight();
                    this.InternalHeight(false, (num > minimumHeight) ? num : minimumHeight);
                }
            }
        }

        private void GetLabelAutoHeight()
        {
            try
            {
                if (this.Parent != null)
                {
                    using (Graphics graphics = this.Parent.CreateGraphics())
                    {
                        int width = (this.Parent.CalculateClientRect().Width - (this.Parent.m_bScrollVisible ? this.Parent.m_VScrollBarWidth : 0)) - (Resco.Controls.DetailView.DetailView.ErrorSpacer + Resco.Controls.DetailView.DetailView.HorizontalSpacer);
                        Size size = this._MeasureTextSize(graphics, this.m_LabelFont, this.m_Label, width, true, true);
                        this.InternalLabeHeigth(false, size.Height);
                    }
                }
            }
            catch
            {
            }
        }

        protected virtual Rectangle GetLabelBounds(int yOffset)
        {
            Resco.Controls.DetailView.DetailView parent = this.Parent;
            if (parent == null)
            {
                return Rectangle.Empty;
            }
            Rectangle rectangle = parent.CalculateClientRect();
            int internalLabelWidth = this.InternalLabelWidth;
            int separatorWidth = parent.SeparatorWidth;
            int height = this.m_Size.Height;
            Point itemXWidth = parent.GetItemXWidth(this);
            int num3 = rectangle.X + itemXWidth.X;
            int y = itemXWidth.Y;
            if (this.m_Style == RescoItemStyle.LabelLeft)
            {
                return new Rectangle(num3 + Resco.Controls.DetailView.DetailView.HorizontalSpacer, yOffset, (y < internalLabelWidth) ? y : internalLabelWidth, height);
            }
            if (this.m_Style == RescoItemStyle.LabelRight)
            {
                int num5 = y - internalLabelWidth;
                return new Rectangle((num3 + ((num5 < 0) ? 0 : num5)) - Resco.Controls.DetailView.DetailView.HorizontalSpacer, yOffset, (num5 < 0) ? y : internalLabelWidth, height);
            }
            height = this.m_LabelHeight;
            Rectangle rectangle2 = new Rectangle(num3 + Resco.Controls.DetailView.DetailView.HorizontalSpacer, yOffset, y - (Resco.Controls.DetailView.DetailView.ErrorSpacer + Resco.Controls.DetailView.DetailView.HorizontalSpacer), height);
            if (rectangle2.Width < 0)
            {
                rectangle2.Width = 0;
            }
            return rectangle2;
        }

        protected virtual int GetMinimumHeight()
        {
            return 0;
        }

        protected Color GetTextBackColor()
        {
            if (this.Parent == null)
            {
                return this.m_TextBackColor;
            }
            if (!(this.TextBackColor == Color.Transparent))
            {
                return this.TextBackColor;
            }
            return this.Parent.BackColor;
        }

        protected Brush GetTextForeBrush()
        {
            this.m_TextForeBrush.Color = this.GetTextForeColor();
            return this.m_TextForeBrush;
        }

        protected Color GetTextForeColor()
        {
            if (this.Parent == null)
            {
                return this.m_TextForeColor;
            }
            if (!this.Enabled)
            {
                return this.Parent.DisabledForeColor;
            }
            if (!(this.TextForeColor == Color.Transparent))
            {
                return this.TextForeColor;
            }
            return this.Parent.ForeColor;
        }

        protected internal virtual bool HandleKey(Keys key)
        {
            return false;
        }

        protected internal virtual bool HandleKeyUp(Keys key)
        {
            return false;
        }

        protected virtual void Hide()
        {
            this.DisableRefresh = false;
            this.OnLostFocus(this, new ItemEventArgs());
        }

        private void InternalHeight(bool invalidate, int value)
        {
            int dif = value - this.m_Size.Height;
            if (dif != 0)
            {
                this.m_Size.Height = value;
                if (this.Control != null)
                {
                    this.Control.Height = this.m_Size.Height;
                }
                if (this.Parent != null)
                {
                    this.Parent.ItemResized(invalidate, dif);
                }
            }
        }

        private void InternalLabeHeigth(bool invalidate, int value)
        {
            int dif = value - this.m_LabelHeight;
            if (dif != 0)
            {
                this.m_LabelHeight = value;
                if (this.Parent != null)
                {
                    this.Parent.ItemResized(invalidate, dif);
                }
            }
        }

        protected void InvokeAddEvent(object obj, string eventName, Delegate handler)
        {
            obj.GetType().GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance).AddEventHandler(obj, handler);
        }

        protected object InvokeGetProperty(object obj, string name)
        {
            return obj.GetType().GetProperty(name, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance).GetValue(obj, null);
        }

        protected object InvokeMethod(object obj, string name, object[] parameters)
        {
            return obj.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.Instance).Invoke(obj, parameters);
        }

        protected void InvokeRemoveEvent(object obj, string eventName, Delegate handler)
        {
            obj.GetType().GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance).RemoveEventHandler(obj, handler);
        }

        protected void InvokeSetProperty(object obj, string name, object value)
        {
            obj.GetType().GetProperty(name, BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance).SetValue(obj, value, null);
        }

        protected virtual void LabelClick(int yOffset, int parentWidth)
        {
            this.OnLabelClicked(this, new ItemEventArgs(this, 0, this.Name));
        }

        public Size MeasureTextSize(Graphics gr, Font font, string text, int width, bool wrap)
        {
            SizeF ef;
            Size size = new Size(width, 0);
            size.Width -= 2 * DVTextBox.BorderSize;
            size.Height = 2 * DVTextBox.BorderSize;
            width = size.Width;
            if (text.EndsWith("\n"))
            {
                ef = gr.MeasureString(text + "0", font);
            }
            else
            {
                ef = gr.MeasureString(text, font);
            }
            if (ef.Width <= width)
            {
                size.Width = (int) Math.Ceiling((double) ef.Width);
                size.Height += (int) Math.Ceiling((double) ef.Height);
                return size;
            }
            string[] strArray = text.Replace("\r", "").Split(new char[] { '\n' });
            int length = strArray.Length;
            for (int i = 0; i < length; i++)
            {
                float num8;
                string str = strArray[i];
                if (str == "")
                {
                    ef = gr.MeasureString("0", font);
                }
                else
                {
                    ef = gr.MeasureString(str, font);
                }
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

        protected virtual void MouseDown(int yOffset, int parentWidth, MouseEventArgs e)
        {
        }

        protected virtual void MouseUp(int yOffset, int parentWidth, MouseEventArgs e)
        {
        }

        protected virtual void MoveTop(int offset)
        {
        }

        public virtual void OnChanged(object sender, ItemEventArgs e)
        {
            if (!this.DisableEvents)
            {
                e.item = this;
                e.Name = this.Name;
                if (this.Changed != null)
                {
                    this.Changed(sender, e);
                }
            }
        }

        protected virtual void OnClick(int yOffset, int parentWidth, bool useClickVisualize)
        {
            if (((this.Parent != null) && useClickVisualize) && (base.GetType() != typeof(Item)))
            {
                using (Graphics graphics = this.Parent.CreateGraphics())
                {
                    float width = graphics.DpiX / 96f;
                    using (Pen pen = new Pen(Color.Blue, width))
                    {
                        Rectangle activePartBounds = this.GetActivePartBounds(yOffset);
                        activePartBounds.Inflate((int) (-2f * width), (int) (-2f * width));
                        graphics.DrawRectangle(pen, activePartBounds);
                    }
                }
            }
            this.Click(yOffset, parentWidth);
        }

        public virtual void OnClicked(object sender, ItemEventArgs e)
        {
            if (!this.DisableEvents)
            {
                e.item = this;
                e.Name = this.Name;
                if (this.Clicked != null)
                {
                    this.Clicked(sender, e);
                }
            }
        }

        public virtual void OnGotFocus(object sender, ItemEventArgs e)
        {
            if (!this.DisableEvents)
            {
                e.item = this;
                e.Name = this.Name;
                if (this.GotFocus != null)
                {
                    this.GotFocus(sender, e);
                }
            }
        }

        protected virtual void OnLabelClick(int yOffset, int parentWidth, bool useClickVisualize)
        {
            this.LabelClick(yOffset, parentWidth);
        }

        public virtual void OnLabelClicked(object sender, ItemEventArgs e)
        {
            if (!this.DisableEvents)
            {
                e.item = this;
                e.Name = this.Name;
                if (this.LabelClicked != null)
                {
                    this.LabelClicked(sender, e);
                }
            }
        }

        public virtual void OnLostFocus(object sender, ItemEventArgs e)
        {
            if (!this.DisableEvents)
            {
                e.item = this;
                e.Name = this.Name;
                if (this.LostFocus != null)
                {
                    this.LostFocus(sender, e);
                }
            }
        }

        protected virtual void OnPropertyChanged()
        {
            if ((this.Parent != null) && this.Parent.AutoRefresh)
            {
                this.Parent.Invalidate();
            }
        }

        protected virtual void OnValidating(object sender, ValidatingEventArgs e)
        {
            if (!this.DisableEvents)
            {
                this.m_bValidating = true;
                if (this.Validating != null)
                {
                    this.Validating(sender, e);
                }
                this.m_bValidating = false;
            }
        }

        internal void ParentChangeBackColor(Color backcolor)
        {
            if (this.m_LabelBackColor == Color.Transparent)
            {
                this.m_LabelBackBrush.Color = backcolor;
            }
            if (this.m_TextBackColor == Color.Transparent)
            {
                this.m_TextBackBrush.Color = backcolor;
            }
        }

        internal void ParentChangeForeColor(Color forecolor)
        {
            if (this.m_LabelForeColor == Color.Transparent)
            {
                this.m_LabelForeBrush.Color = forecolor;
            }
            if (this.m_TextForeColor == Color.Transparent)
            {
                this.m_TextForeBrush.Color = forecolor;
            }
        }

        protected virtual object Parse(string text)
        {
            object editValue;
            if (text == null)
            {
                return null;
            }
            Type conversionType = (this.Parent != null) ? this.Parent.GetBoundItemType(this.m_dataMember) : typeof(object);
            if ((conversionType != typeof(string)) && (text == ""))
            {
                return null;
            }
            try
            {
                editValue = Convert.ChangeType(text, conversionType, CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                try
                {
                    editValue = Convert.ChangeType(text, conversionType, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    editValue = this.EditValue;
                }
            }
            return editValue;
        }

        protected internal virtual void ScaleItem(float fx, float fy)
        {
            if (this.m_LabelWidth != -1)
            {
                this.SetProperty("LabelWidth", (int) (this.m_LabelWidth * fx));
            }
            if (this.m_LabelHeight != -1)
            {
                this.SetProperty("LabelHeight", (int) (this.m_LabelHeight * fy));
            }
            int num = (this.m_Size.Width < 0) ? -1 : ((int) (this.m_Size.Width * fx));
            int num2 = (int) (this.m_Size.Height * fy);
            this.m_Size.Width = num;
            this.SetProperty("Height", num2);
        }

        public void SetFocus()
        {
            this.Parent.SelectedItem = this;
        }

        internal virtual void SetParent(Resco.Controls.DetailView.DetailView o)
        {
            this.m_Parent = o;
        }

        protected void SetProperty(string name, object value)
        {
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this).Find(name, false);
            if (descriptor != null)
            {
                descriptor.SetValue(this, value);
            }
        }

        protected virtual void SetText(string text, bool validated)
        {
            if (this.m_Text != text)
            {
                if (!validated)
                {
                    ValidatingEventArgs e = new ValidatingEventArgs(this, this.Parse(text), text, true);
                    this.OnValidating(this, e);
                    if (e.Cancel)
                    {
                        this.UpdateControl(this.EditValue);
                        return;
                    }
                    this.m_Text = e.NewText;
                    this.SetValue(e.NewValue, true);
                }
                else
                {
                    this.m_Text = text;
                }
                this.OnPropertyChanged();
            }
        }

        protected virtual void SetValue(object value, bool validated)
        {
            if ((this.m_Value != value) && ((this.m_Value == null) || !this.m_Value.Equals(value)))
            {
                if (!validated)
                {
                    ValidatingEventArgs e = new ValidatingEventArgs(this, value, this.FormatValue(value), false);
                    this.OnValidating(this, e);
                    if (e.Cancel)
                    {
                        this.UpdateControl(this.EditValue);
                        return;
                    }
                    this.m_Value = e.NewValue;
                    this.SetText(e.NewText, true);
                }
                else
                {
                    this.m_Value = value;
                }
                this.UpdateControl(this.EditValue);
                if (this.Parent != null)
                {
                    this.Parent.UpdateData(this);
                    if (!this.DisableRefresh && this.Parent.AutoRefresh)
                    {
                        this.Parent.Invalidate();
                    }
                }
                this.OnPropertyChanged();
                this.OnChanged(this, new ItemEventArgs());
            }
        }

        protected virtual bool ShouldSerializeErrorBackground()
        {
            return (this.m_ErrorBackground != Resco.Controls.DetailView.ErrorBackground.Background);
        }

        protected virtual bool ShouldSerializeItemBorder()
        {
            return (this.m_border != Resco.Controls.DetailView.ItemBorder.Underline);
        }

        protected bool ShouldSerializeLabelHeight()
        {
            return (((this.Style == RescoItemStyle.LabelTop) && !this.LabelAutoHeight) && (this.m_LabelHeight != 0x10));
        }

        protected virtual bool ShouldSerializeLineAlign()
        {
            return (this.m_lineAlign != VerticalAlignment.Top);
        }

        protected virtual bool ShouldSerializeRoundedCorner()
        {
            return (this.m_roundedCorner != RoundedCornerStyles.None);
        }

        protected virtual bool ShouldSerializeStyle()
        {
            return (this.m_Style != RescoItemStyle.LabelLeft);
        }

        protected virtual bool ShouldSerializeTag()
        {
            return (this.m_Tag != null);
        }

        public override string ToString()
        {
            if (!(this.Name == "") && (this.Name != null))
            {
                return this.Name;
            }
            if (this.Site != null)
            {
                return this.Site.Name;
            }
            return "Item";
        }

        protected virtual void UpdateControl(object value)
        {
        }

        internal virtual void UpdateWidth(int parentWidth)
        {
            if ((this.Control != null) && (this.Parent != null))
            {
                Rectangle activePartBounds = this.GetActivePartBounds(0);
                this.Control.Size = new Size(activePartBounds.Width, this.Control.Height);
            }
        }

        [DefaultValue(false)]
        public bool AutoHeight
        {
            get
            {
                return this.m_AutoHeight;
            }
            set
            {
                if (this.m_AutoHeight != value)
                {
                    this.m_AutoHeight = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.DetailView.Design.Browsable(false)]
        public virtual System.Windows.Forms.Control Control
        {
            get
            {
                return null;
            }
        }

        [DefaultValue("")]
        public string DataMember
        {
            get
            {
                return this.m_dataMember;
            }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                if (this.m_dataMember != value)
                {
                    this.m_dataMember = value;
                    if (this.Parent != null)
                    {
                        this.Parent.UpdateControl();
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(false), Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public bool DisabledEvents
        {
            get
            {
                return this.DisableEvents;
            }
            set
            {
                this.DisableEvents = value;
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.DetailView.Design.Browsable(false), DefaultValue((string) null)]
        public virtual object EditValue
        {
            get
            {
                return this.Value;
            }
            set
            {
                this.Value = value;
            }
        }

        [DefaultValue(true)]
        public virtual bool Enabled
        {
            get
            {
                return this.m_Enabled;
            }
            set
            {
                this.m_Enabled = value;
                if (this.Control != null)
                {
                    this.Control.Enabled = value;
                }
            }
        }

        public Resco.Controls.DetailView.ErrorBackground ErrorBackground
        {
            get
            {
                return this.m_ErrorBackground;
            }
            set
            {
                this.m_ErrorBackground = value;
                this.OnPropertyChanged();
            }
        }

        [DefaultValue("Red")]
        public Color ErrorColor
        {
            get
            {
                return this.m_RedColor;
            }
            set
            {
                this.m_RedColor = value;
                if (value == Color.Transparent)
                {
                    this.m_RedBrush = new SolidBrush(Color.Red);
                }
                else
                {
                    this.m_RedBrush = new SolidBrush(value);
                }
                this.OnPropertyChanged();
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue((string) null), Resco.Controls.DetailView.Design.Browsable(false)]
        public string ErrorMessage
        {
            get
            {
                return this.m_ErrorMessage;
            }
            set
            {
                if (this.m_ErrorMessage != value)
                {
                    this.m_ErrorMessage = value;
                    if (this.Parent != null)
                    {
                        this.Parent.UpdateError(this);
                    }
                    if ((value != null) && (value != ""))
                    {
                        this.m_bHasError = true;
                        this.m_PreviousColor = this.TextBackColor;
                        if (this.Parent != null)
                        {
                            this.Parent.Invalidate();
                        }
                    }
                    else
                    {
                        this.m_bHasError = false;
                        this.TextBackColor = this.m_PreviousColor;
                    }
                }
            }
        }

        internal virtual bool Focused
        {
            get
            {
                return this.m_bFocusing;
            }
        }

        [DefaultValue(0x10)]
        public virtual int Height
        {
            get
            {
                if (this.m_AutoHeight)
                {
                    this.GetAutoHeight();
                }
                return this.m_Size.Height;
            }
            set
            {
                if (!this.m_AutoHeight)
                {
                    this.InternalHeight(true, value);
                }
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue(-1), Resco.Controls.DetailView.Design.Browsable(false)]
        public int Index
        {
            get
            {
                if (this.m_Parent != null)
                {
                    return this.m_Parent.Items.IndexOf(this);
                }
                return -1;
            }
        }

        protected internal int InternalLabelWidth
        {
            get
            {
                if (this.m_LabelWidth >= 0)
                {
                    return this.m_LabelWidth;
                }
                return this.Parent.LabelWidth;
            }
        }

        public Resco.Controls.DetailView.ItemBorder ItemBorder
        {
            get
            {
                return this.m_border;
            }
            set
            {
                if (this.m_border != value)
                {
                    this.m_border = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public int ItemHeight
        {
            get
            {
                return (ItemSpacing + (this.m_bVisible ? (this.LabelHeight + this.Height) : 0));
            }
        }

        [DefaultValue(-1)]
        public virtual int ItemWidth
        {
            get
            {
                return this.m_Size.Width;
            }
            set
            {
                if (this.m_Size.Width != value)
                {
                    this.m_Size.Width = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue("")]
        public virtual string Label
        {
            get
            {
                return this.m_Label;
            }
            set
            {
                this.m_Label = value;
                this.OnPropertyChanged();
            }
        }

        [DefaultValue(1)]
        public virtual HorizontalAlignment LabelAlignment
        {
            get
            {
                return this.m_LabelAlignment;
            }
            set
            {
                this.m_LabelAlignment = value;
                this.OnPropertyChanged();
            }
        }

        [DefaultValue(false)]
        public bool LabelAutoHeight
        {
            get
            {
                return this.m_LabelAutoHeight;
            }
            set
            {
                if (this.m_LabelAutoHeight != value)
                {
                    this.m_LabelAutoHeight = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue("Transparent")]
        public Color LabelBackColor
        {
            get
            {
                return this.m_LabelBackColor;
            }
            set
            {
                this.m_LabelBackColor = value;
                if ((value == Color.Transparent) && (this.Parent != null))
                {
                    this.m_LabelBackBrush.Color = this.Parent.BackColor;
                }
                else
                {
                    this.m_LabelBackBrush.Color = value;
                }
                this.OnPropertyChanged();
            }
        }

        [DefaultValue("Tahoma, 8pt")]
        public Font LabelFont
        {
            get
            {
                return this.m_LabelFont;
            }
            set
            {
                this.m_LabelFont = value;
                this.OnPropertyChanged();
            }
        }

        [DefaultValue("Black")]
        public Color LabelForeColor
        {
            get
            {
                return this.m_LabelForeColor;
            }
            set
            {
                this.m_LabelForeColor = value;
                if ((value == Color.Transparent) && (this.Parent != null))
                {
                    this.m_LabelForeBrush.Color = this.Parent.ForeColor;
                }
                else
                {
                    this.m_LabelForeBrush.Color = value;
                }
                this.OnPropertyChanged();
            }
        }

        public virtual int LabelHeight
        {
            get
            {
                if (this.m_Style != RescoItemStyle.LabelTop)
                {
                    return 0;
                }
                if (this.m_LabelAutoHeight)
                {
                    this.GetLabelAutoHeight();
                }
                return this.m_LabelHeight;
            }
            set
            {
                if (this.m_Style == RescoItemStyle.LabelTop)
                {
                    if (!this.m_LabelAutoHeight)
                    {
                        this.InternalLabeHeigth(true, value);
                    }
                }
                else
                {
                    this.m_LabelHeight = value;
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(true), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Visible), DefaultValue(false)]
        public bool LabelToolTip
        {
            get
            {
                return this.m_bLabelToolTip;
            }
            set
            {
                if (this.m_bLabelToolTip != value)
                {
                    this.m_bLabelToolTip = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(-1)]
        public virtual int LabelWidth
        {
            get
            {
                if ((this.m_Style != RescoItemStyle.LabelLeft) && (this.m_Style != RescoItemStyle.LabelRight))
                {
                    return -1;
                }
                return this.m_LabelWidth;
            }
            set
            {
                if (this.m_LabelWidth != value)
                {
                    this.m_LabelWidth = value;
                    if (((this.m_Style == RescoItemStyle.LabelLeft) || (this.m_Style == RescoItemStyle.LabelRight)) && (this.Parent != null))
                    {
                        this.UpdateWidth(this.Parent.UsableWidth);
                        this.OnPropertyChanged();
                    }
                }
            }
        }

        [DefaultValue(0)]
        public VerticalAlignment LineAlign
        {
            get
            {
                return this.m_lineAlign;
            }
            set
            {
                if (this.m_lineAlign != value)
                {
                    this.m_lineAlign = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false)]
        public string Name
        {
            get
            {
                if (this.Site != null)
                {
                    return this.Site.Name;
                }
                return this.m_Name;
            }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                if (this.m_Name != value)
                {
                    try
                    {
                        if ((this.Site != null) && (this.Site.Name != value))
                        {
                            this.Site.Name = value;
                        }
                    }
                    catch (Exception exception)
                    {
                        if ((this.Site != null) && (this.Site.Name != value))
                        {
                            value = this.Site.Name;
                        }
                        throw exception;
                    }
                    finally
                    {
                        if (this.m_Parent != null)
                        {
                            this.m_Parent.Items.ChangeName(this, this.m_Name, value);
                        }
                        this.m_Name = value;
                    }
                }
            }
        }

        [DefaultValue(true)]
        public virtual bool NewLine
        {
            get
            {
                return this.m_NewLine;
            }
            set
            {
                if (this.m_NewLine != value)
                {
                    this.m_NewLine = value;
                    if (this.Parent != null)
                    {
                        this.Parent.ItemResized(true, 0);
                    }
                }
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue((string) null), Resco.Controls.DetailView.Design.Browsable(false)]
        public Resco.Controls.DetailView.DetailView Parent
        {
            get
            {
                return this.m_Parent;
            }
        }

        [DefaultValue(0)]
        public RoundedCornerStyles RoundedCorner
        {
            get
            {
                return this.m_roundedCorner;
            }
            set
            {
                if (value != this.m_roundedCorner)
                {
                    this.m_roundedCorner = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public ISite Site
        {
            get
            {
                return this.m_site;
            }
            set
            {
                this.m_site = value;
            }
        }

        [DefaultValue(1)]
        public RescoItemStyle Style
        {
            get
            {
                return this.m_Style;
            }
            set
            {
                if (value != this.m_Style)
                {
                    this.m_Style = value;
                    if (this.Parent != null)
                    {
                        this.Parent.ItemResized(this.Parent.AutoRefresh, 0);
                    }
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(true), DefaultValue((string) null), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Visible)]
        public object Tag
        {
            get
            {
                return this.m_Tag;
            }
            set
            {
                this.m_Tag = value;
            }
        }

        [DefaultValue("")]
        public virtual string Text
        {
            get
            {
                return this.m_Text;
            }
            set
            {
                if (!this.m_bValidating)
                {
                    this.SetText(value, false);
                }
            }
        }

        [DefaultValue(0)]
        public HorizontalAlignment TextAlign
        {
            get
            {
                return this.m_TextAlign;
            }
            set
            {
                if (this.m_TextAlign != value)
                {
                    this.m_TextAlign = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue("Transparent")]
        public Color TextBackColor
        {
            get
            {
                return this.m_TextBackColor;
            }
            set
            {
                this.m_TextBackColor = value;
                this.m_PreviousColor = value;
                if ((value == Color.Transparent) && (this.Parent != null))
                {
                    this.m_TextBackBrush.Color = this.Parent.BackColor;
                }
                else
                {
                    this.m_TextBackBrush.Color = value;
                }
                this.OnPropertyChanged();
            }
        }

        [DefaultValue("Tahoma, 8pt")]
        public Font TextFont
        {
            get
            {
                return this.m_TextFont;
            }
            set
            {
                this.m_TextFont = value;
                this.OnPropertyChanged();
            }
        }

        [DefaultValue("Black")]
        public Color TextForeColor
        {
            get
            {
                return this.m_TextForeColor;
            }
            set
            {
                this.m_TextForeColor = value;
                if ((value == Color.Transparent) && (this.Parent != null))
                {
                    this.m_TextForeBrush.Color = this.Parent.ForeColor;
                }
                else
                {
                    this.m_TextForeBrush.Color = value;
                }
                this.OnPropertyChanged();
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue((string) null)]
        public virtual object Value
        {
            get
            {
                return this.m_Value;
            }
            set
            {
                if (!this.m_bValidating)
                {
                    this.SetValue(value, false);
                }
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Visible), DefaultValue(true), Resco.Controls.DetailView.Design.Browsable(true)]
        public virtual bool Visible
        {
            get
            {
                return this.m_bVisible;
            }
            set
            {
                if (this.m_bVisible != value)
                {
                    int itemHeight = this.ItemHeight;
                    this.m_bVisible = value;
                    if (this.Parent != null)
                    {
                        this.Parent.ItemResized(this.ItemHeight - itemHeight);
                    }
                }
            }
        }
    }
}

