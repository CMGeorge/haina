namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using TouchScrolling;

    public class ShortcutsCollection : CollectionBase
    {
        internal static Point[] m_Arrow = new Point[3];
        private ImageAttributes m_attrDisabled = new ImageAttributes();
        private Bitmap m_Background;
        private bool m_bAutoScaleEnabled = true;
        private bool m_bDoInvalidate = true;
        private Bitmap m_bmpDisabled;
        private SolidBrush m_brushDimmedScrollbar;
        private SolidBrush m_brushSelShortcutsBack;
        private SolidBrush m_brushSelShortcutsFore;
        private SolidBrush m_brushShortcutsBack;
        private SolidBrush m_brushShortcutsFore;
        private Rectangle m_ClientRectangle;
        internal const int m_DefaultSpacing = 5;
        private Color m_DimmedScrollbarColor = SystemColors.GrayText;
        private bool m_downArrowDown;
        private System.Drawing.Font m_Font = new System.Drawing.Font("Tahoma", 8f, FontStyle.Regular);
        private Graphics m_grDisabled;
        private System.Windows.Forms.ImageList m_ImageList;
        private bool m_indexChanged;
        private string m_Name = "Shortcuts";
        private bool m_OldNeedScrollbarState;
        private static UserControl m_ParentControl;
        private Pen m_penDisbled;
        private int m_pushedIndex = -1;
        internal static SizeF m_scaleFactor = new SizeF(1f, 1f);
        private int m_ScrollBarHeight = 15;
        private int m_ScrollBarWidth = 15;
        private int m_selectedIndex = -1;
        private int m_selectingIndex = -1;
        private Color m_selShortcutsBackColor = SystemColors.Highlight;
        private Color m_selShortcutsForeColor = SystemColors.HighlightText;
        private Color m_shortcutsBackColor = SystemColors.Window;
        private Color m_shortcutsForeColor = SystemColors.WindowText;
        internal static int m_Spacing = 5;
        private Resco.Controls.OutlookControls.TextStyle m_textStyle;
        private int m_topLine;
        private int m_TouchBgrdHeight;
        private OsbTouchTool m_TouchNavigatorTool;
        private bool m_upArrowDown;
        private static SizeF m_userScaleFactor = new SizeF(1f, 1f);
        private Resco.Controls.OutlookControls.ViewStyle m_viewStyle = Resco.Controls.OutlookControls.ViewStyle.List;
        private int m_VScrollShift;
        private int m_Width = 80;
        internal Color TransparentColor = Color.Transparent;

        internal static  event Resco.Controls.OutlookControls.GestureDetectedHandler GestureDetected;

        public event InsertCompleteEventHandler InsertCompleteEvent;

        public event InsertEventHandler InsertEvent;

        public event EventHandler Invalidating;

        public event RemoveCompleteEventHandler RemoveCompleteEvent;

        public event RemoveEventHandler RemoveEvent;

        public event Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler SelectedShortcutIndexChanged;

        public event SetCompleteEventHandler SetCompleteEvent;

        public event SetEventHandler SetEvent;

        public event Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler ShortcutEntered;

        public ShortcutsCollection()
        {
            this.m_attrDisabled.SetColorKey(Color.Pink, Color.Pink);
            this.InitTouchTool();
        }

        public int Add(Resco.Controls.OutlookControls.Shortcut value)
        {
            return base.List.Add(value);
        }

        public bool Contains(Resco.Controls.OutlookControls.Shortcut value)
        {
            return base.List.Contains(value);
        }

        private void CreateDisabledBitmap(int width, int height)
        {
            bool flag = false;
            if (((this.m_bmpDisabled == null) || (this.m_bmpDisabled.Width != width)) || (this.m_bmpDisabled.Height != height))
            {
                if (this.m_bmpDisabled != null)
                {
                    this.m_bmpDisabled.Dispose();
                    this.m_bmpDisabled = null;
                }
                this.m_bmpDisabled = new Bitmap(width, height);
                this.m_grDisabled = Graphics.FromImage(this.m_bmpDisabled);
                flag = true;
            }
            if (this.m_grDisabled == null)
            {
                this.m_grDisabled = Graphics.FromImage(this.m_bmpDisabled);
                flag = true;
            }
            if ((this.m_penDisbled == null) || (this.m_penDisbled.Color != this.m_shortcutsBackColor))
            {
                if (this.m_penDisbled != null)
                {
                    this.m_penDisbled.Dispose();
                    this.m_penDisbled = null;
                }
                this.m_penDisbled = new Pen(this.m_shortcutsBackColor);
                flag = true;
            }
            if (flag)
            {
                this.m_grDisabled.Clear(Color.Pink);
                this.m_bmpDisabled.SetPixel(0, 0, Color.Gray);
                for (int i = 2; i < (width + height); i += 2)
                {
                    int num2 = (i < width) ? i : width;
                    int num3 = (i < width) ? 0 : (i - width);
                    int num4 = (i < height) ? 0 : (i - height);
                    int num5 = (i < height) ? i : height;
                    this.m_grDisabled.DrawLine(this.m_penDisbled, num2, num3, num4, num5);
                }
            }
        }

        private void CreateGdiObjects()
        {
            if ((this.m_brushShortcutsFore == null) || (this.m_brushShortcutsFore.Color != this.ForeColor))
            {
                this.m_brushShortcutsFore = OutlookShortcutBar.GetBrush(this.ForeColor);
            }
            if ((this.m_brushShortcutsBack == null) || (this.m_brushShortcutsBack.Color != this.BackColor))
            {
                this.m_brushShortcutsBack = OutlookShortcutBar.GetBrush(this.BackColor);
            }
            if ((this.m_brushSelShortcutsBack == null) || (this.m_brushSelShortcutsBack.Color != this.SelBackColor))
            {
                this.m_brushSelShortcutsBack = OutlookShortcutBar.GetBrush(this.SelBackColor);
            }
            if ((this.m_brushSelShortcutsFore == null) || (this.m_brushSelShortcutsFore.Color != this.SelForeColor))
            {
                this.m_brushSelShortcutsFore = OutlookShortcutBar.GetBrush(this.SelForeColor);
            }
            if ((this.m_brushDimmedScrollbar == null) || (this.m_brushDimmedScrollbar.Color != this.DimmedScrollbarColor))
            {
                this.m_brushDimmedScrollbar = OutlookShortcutBar.GetBrush(this.DimmedScrollbarColor);
            }
        }

        private void DeinitTouchTool()
        {
            if (this.m_TouchNavigatorTool != null)
            {
                this.m_TouchNavigatorTool.GestureDetected -= new OsbTouchTool.GestureDetectedHandler(this.TouchNavigatorTool_GestureDetected);
                this.m_TouchNavigatorTool.MouseMoveDetected -= new OsbTouchTool.MouseMoveDetectedHandler(this.TouchNavigatorTool_MouseMoveDetected);
                OsbTouchTool.ParentControl = null;
                this.m_TouchNavigatorTool.Dispose();
                this.m_TouchNavigatorTool = null;
            }
        }

        internal void Dispose()
        {
            ParentControl = null;
            if (this.m_Background != null)
            {
                this.m_Background.Dispose();
                this.m_Background = null;
            }
            if (this.m_bmpDisabled != null)
            {
                this.m_bmpDisabled.Dispose();
                this.m_bmpDisabled = null;
            }
            if (this.m_Font != null)
            {
                this.m_Font.Dispose();
                this.m_Font = null;
            }
            if (this.m_grDisabled != null)
            {
                this.m_grDisabled.Dispose();
                this.m_grDisabled = null;
            }
            if (this.m_ImageList != null)
            {
                this.m_ImageList.Dispose();
                this.m_ImageList = null;
            }
            if (this.m_penDisbled != null)
            {
                this.m_penDisbled.Dispose();
                this.m_penDisbled = null;
            }
            for (int i = 0; i < base.Count; i++)
            {
                Resco.Controls.OutlookControls.Shortcut shortcut = this[i];
                if ((shortcut != null) && (shortcut != null))
                {
                    shortcut.Dispose();
                    shortcut = null;
                }
            }
            base.Clear();
            this.DeinitTouchTool();
        }

        internal void Draw(Graphics gr, Rectangle rect, Pen frame)
        {
            bool flag;
            this.ResizeTouchBackground(rect);
            this.PrepareTouchBackground(gr, rect, frame, out flag);
            if (flag)
            {
                this.ShowArrows(gr, rect, frame);
            }
        }

        internal void Draw2(Graphics gr, Rectangle rect, Pen frame)
        {
            bool flag;
            Rectangle destRect = rect;
            this.ResizeTouchBackground(rect);
            this.PrepareTouchBackground2(gr, rect, frame, out flag);
            ImageAttributes imageAttr = new ImageAttributes();
            this.GetTransparentColor(this.m_Background);
            imageAttr.SetColorKey(this.TransparentColor, this.TransparentColor);
            gr.DrawImage(this.m_Background, destRect, 0, this.m_VScrollShift, rect.Width, rect.Height, GraphicsUnit.Pixel, imageAttr);
            if (flag)
            {
                this.ShowArrows(gr, rect, frame);
            }
        }

        internal Size DrawAlignedText(Graphics gr, string text, TextAlignment textAlignment, Brush foreBrush, Rectangle layoutRectangle)
        {
            int num;
            int num2;
            HorizontalAlignment center;
            VerticalAlignment middle;
            if (((text == null) || ((num = layoutRectangle.Width) < 1)) || ((num2 = layoutRectangle.Height) < 1))
            {
                return new Size(0, 0);
            }
            bool flag = true;
            System.Drawing.Font font = this.m_Font;
            switch (textAlignment)
            {
                case TextAlignment.MiddleCenter:
                    center = HorizontalAlignment.Center;
                    middle = VerticalAlignment.Middle;
                    break;

                case TextAlignment.MiddleRight:
                    center = HorizontalAlignment.Right;
                    middle = VerticalAlignment.Middle;
                    break;

                case TextAlignment.MiddleLeft:
                    center = HorizontalAlignment.Left;
                    middle = VerticalAlignment.Middle;
                    break;

                case TextAlignment.BottomCenter:
                    center = HorizontalAlignment.Center;
                    middle = VerticalAlignment.Bottom;
                    break;

                case TextAlignment.BottomRight:
                    center = HorizontalAlignment.Right;
                    middle = VerticalAlignment.Bottom;
                    break;

                case TextAlignment.BottomLeft:
                    center = HorizontalAlignment.Left;
                    middle = VerticalAlignment.Bottom;
                    break;

                case TextAlignment.TopCenter:
                    center = HorizontalAlignment.Center;
                    middle = VerticalAlignment.Top;
                    break;

                case TextAlignment.TopRight:
                    center = HorizontalAlignment.Right;
                    middle = VerticalAlignment.Top;
                    break;

                default:
                    center = HorizontalAlignment.Left;
                    middle = VerticalAlignment.Top;
                    break;
            }
            SizeF ef = gr.MeasureString(text, font);
            float width = ef.Width;
            if (width <= num)
            {
                if (text.IndexOf('\n') == -1)
                {
                    float num4 = ef.Height;
                    float num5 = layoutRectangle.Y;
                    if (middle != VerticalAlignment.Top)
                    {
                        float num6 = num2 - num4;
                        if (middle == VerticalAlignment.Middle)
                        {
                            num6 /= 2f;
                        }
                        num5 += num6;
                    }
                    float num7 = layoutRectangle.X;
                    if (center != HorizontalAlignment.Left)
                    {
                        float num8 = num - width;
                        if (center == HorizontalAlignment.Center)
                        {
                            num8 /= 2f;
                        }
                        num7 += num8;
                    }
                    gr.DrawString(text, font, foreBrush, num7, num5);
                    return ef.ToSize();
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
                }
                float num12 = layoutRectangle.Y;
                float num13 = num12;
                if (middle != VerticalAlignment.Top)
                {
                    float num14 = num2 - (num11 * num9);
                    if (middle == VerticalAlignment.Middle)
                    {
                        num14 /= 2f;
                    }
                    num12 += num14;
                }
                float num15 = layoutRectangle.X;
                float num16 = 0f;
                for (int k = 0; k < num9; k++)
                {
                    string str = strArray[k];
                    if (center != HorizontalAlignment.Left)
                    {
                        ef = gr.MeasureString(str, font);
                        num16 = num - ef.Width;
                        if (center == HorizontalAlignment.Center)
                        {
                            num16 /= 2f;
                        }
                    }
                    else
                    {
                        ef = gr.MeasureString(str, font);
                    }
                    gr.DrawString(str, font, foreBrush, num15 + num16, num12);
                    num12 += num11;
                }
                return new Size(num, (int) (num12 - num13));
            }
            string[] strArray2 = text.Replace("\r", "").Split(new char[] { '\n' });
            int length = strArray2.Length;
            if ((length > 1) && (strArray2[length - 1] == ""))
            {
                length--;
            }
            float num20 = ef.Height / ((float) length);
            int num21 = (int) Math.Floor((double) (((float) num2) / num20));
            if (num21 < 1)
            {
                num21 = 1;
            }
            string[] strArray3 = new string[num21];
            float[] numArray = new float[num21];
            int index = 0;
            num21--;
            for (int i = 0; i < length; i++)
            {
                string str2 = strArray2[i];
                float num24 = gr.MeasureString(str2, font).Width;
                if (num24 <= num)
                {
                    strArray3[index] = str2;
                    numArray[index] = num24;
                    if ((++index <= num21) || (i >= (length - 1)))
                    {
                        continue;
                    }
                    break;
                }
                int num25 = str2.Length;
                int num26 = (int) (((float) (num25 * num)) / num24);
                if (num26 < 1)
                {
                    strArray3[index] = str2.Substring(0, 1);
                    numArray[index] = num;
                    if (flag && (num25 > 1))
                    {
                        strArray2[i] = str2.Substring(1, num25 - 1);
                        i--;
                    }
                }
                else
                {
                    int startIndex = 0;
                    bool flag2 = !flag && (center == HorizontalAlignment.Right);
                    if (flag2)
                    {
                        startIndex = num25 - num26;
                    }
                    StringBuilder builder = new StringBuilder(str2.Substring(startIndex, num26));
                    ef = gr.MeasureString(builder.ToString(), font);
                    float num28 = ef.Width;
                    if (num28 > num)
                    {
                        do
                        {
                            num26--;
                            if (num26 <= 0)
                            {
                                break;
                            }
                            builder.Remove(flag2 ? 0 : num26, 1);
                            num28 = gr.MeasureString(builder.ToString(), font).Width;
                        }
                        while (num28 > num);
                    }
                    else
                    {
                        do
                        {
                            num28 = ef.Width;
                            num26++;
                            if (num26 >= num25)
                            {
                                break;
                            }
                            if (flag2)
                            {
                                builder.Insert(0, str2.Substring(num25 - num26, 1));
                            }
                            else
                            {
                                builder.Append(str2[num26 - 1]);
                            }
                            ef = gr.MeasureString(builder.ToString(), font);
                        }
                        while (ef.Width <= num);
                        if (num26 != num25)
                        {
                            builder.Remove(flag2 ? 0 : (num26 - 1), 1);
                        }
                    }
                    int count = builder.Length;
                    if (flag && (index < num21))
                    {
                        if (count < num25)
                        {
                            if (char.IsWhiteSpace(str2[count]))
                            {
                                count++;
                            }
                            else
                            {
                                int num30 = str2.LastIndexOfAny(new char[] { ' ', '\t' }, count - 1, count);
                                if (num30 > 0)
                                {
                                    builder.Remove(num30, count - num30);
                                    count = num30 + 1;
                                }
                            }
                        }
                        if (count < num25)
                        {
                            strArray2[i] = str2.Substring(count, num25 - count);
                            num28 = gr.MeasureString(builder.ToString(), font).Width;
                            i--;
                        }
                    }
                    strArray3[index] = builder.ToString();
                    numArray[index] = num28;
                }
                if ((++index > num21) && (i < (length - 1)))
                {
                    break;
                }
            }
            float y = layoutRectangle.Y;
            if (middle != VerticalAlignment.Top)
            {
                float num32 = num2 - (num20 * index);
                if (middle == VerticalAlignment.Middle)
                {
                    num32 /= 2f;
                }
                y += num32;
            }
            float num33 = y;
            float x = layoutRectangle.X;
            float num35 = 0f;
            for (int j = 0; j < index; j++)
            {
                string s = strArray3[j];
                if (center != HorizontalAlignment.Left)
                {
                    num35 = num - numArray[j];
                    if (center == HorizontalAlignment.Center)
                    {
                        num35 /= 2f;
                    }
                }
                gr.DrawString(s, font, foreBrush, x + num35, y);
                y += num20;
            }
            return new Size(num, (int) (y - num33));
        }

        private void DrawBackground(Graphics gr, Rectangle rect, Pen frame, out bool aNeedScrollbar)
        {
            this.DrawShortcuts(gr, rect, frame, out aNeedScrollbar);
        }

        internal void DrawImage(Graphics gr, Image image, Rectangle dest, ImageAttributes ia, bool bAutoTransparent)
        {
            if (bAutoTransparent)
            {
                Bitmap bitmap = new Bitmap(image);
                Color pixel = bitmap.GetPixel(0, 0);
                ia.SetColorKey(pixel, pixel);
                bitmap.Dispose();
                bitmap = null;
            }
            if (this.m_bAutoScaleEnabled)
            {
                Rectangle rectangle = new Rectangle((int) (((image.Width * m_userScaleFactor.Width) - dest.Width) / 2f), (int) (((image.Height * m_userScaleFactor.Height) - dest.Height) / 2f), dest.Width, dest.Height);
                gr.DrawImage(image, dest, (int) (((float) rectangle.X) / m_userScaleFactor.Width), (int) (((float) rectangle.Y) / m_userScaleFactor.Height), (int) (((float) rectangle.Width) / m_userScaleFactor.Width), (int) (((float) rectangle.Height) / m_userScaleFactor.Height), GraphicsUnit.Pixel, ia);
            }
            else
            {
                Rectangle rectangle2 = new Rectangle(0, 0, image.Width, image.Height);
                int x = dest.X + ((dest.Width - image.Width) / 2);
                int y = dest.Y + ((dest.Height - image.Height) / 2);
                Rectangle destRect = new Rectangle(x, y, image.Width, image.Height);
                gr.DrawImage(image, destRect, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel, ia);
            }
            image.Dispose();
            image = null;
        }

        internal void DrawShortcuts(Graphics gr, Rectangle rect, Pen frame, out bool aNeedScrollbar)
        {
            this.CreateGdiObjects();
            int x = rect.X;
            int y = rect.Y + m_Spacing;
            int width = (this.m_viewStyle == Resco.Controls.OutlookControls.ViewStyle.List) ? this.m_Width : rect.Width;
            if (width > rect.Width)
            {
                width = rect.Width;
            }
            int num4 = (int) ((this.Font.Size * 2f) * m_scaleFactor.Height);
            int height = (int) ((((this.m_ImageList != null) ? (this.m_ImageList.ImageSize.Height * m_userScaleFactor.Height) : 0f) + ((this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Bottom) ? ((float) (num4 * 2)) : ((float) 0))) + (4f * m_scaleFactor.Height));
            if (((this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Right) || (this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Left)) && (height < num4))
            {
                height = num4;
            }
            int num6 = 0;
            int num7 = ((base.List.Count + (rect.Width / width)) - 1) / (rect.Width / width);
            int num1 = rect.Height / height;
            this.m_indexChanged = false;
            this.CreateDisabledBitmap(width, height);
            foreach (Resco.Controls.OutlookControls.Shortcut shortcut in base.List)
            {
                Rectangle rectangle;
                if (this.IndexOf(shortcut) == this.m_selectingIndex)
                {
                    rectangle = new Rectangle(x + m_Spacing, y, width - (2 * m_Spacing), height);
                    gr.FillRectangle(this.m_brushSelShortcutsBack, rectangle);
                }
                int num8 = (this.m_ImageList != null) ? ((int) (this.m_ImageList.ImageSize.Width * m_userScaleFactor.Width)) : 0;
                int num9 = (this.m_ImageList != null) ? ((int) (this.m_ImageList.ImageSize.Height * m_userScaleFactor.Height)) : 0;
                if (this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Bottom)
                {
                    Rectangle dest = new Rectangle(x + m_Spacing, y + m_Spacing, width - (2 * m_Spacing), num9);
                    if (((this.m_ImageList != null) && (shortcut.ImageIndex >= 0)) && (shortcut.ImageIndex < this.m_ImageList.Images.Count))
                    {
                        this.DrawImage(gr, this.m_ImageList.Images[shortcut.ImageIndex], dest, shortcut.m_ia, shortcut.AutoTransparent);
                    }
                    rectangle = new Rectangle((x + ((int) (2f * m_scaleFactor.Width))) + m_Spacing, (y + dest.Height) + m_Spacing, (width - ((int) (4f * m_scaleFactor.Width))) - (2 * m_Spacing), (height - dest.Height) - ((int) (5f * m_scaleFactor.Height)));
                    this.DrawAlignedText(gr, shortcut.Text, TextAlignment.MiddleCenter, (this.IndexOf(shortcut) == this.m_selectingIndex) ? this.m_brushSelShortcutsFore : this.m_brushShortcutsFore, rectangle);
                }
                else if (this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Right)
                {
                    Rectangle rectangle3 = new Rectangle(x + (2 * m_Spacing), y + ((height - num9) / 2), num8, num9);
                    if (((this.m_ImageList != null) && (shortcut.ImageIndex >= 0)) && (shortcut.ImageIndex < this.m_ImageList.Images.Count))
                    {
                        this.DrawImage(gr, this.m_ImageList.Images[shortcut.ImageIndex], rectangle3, shortcut.m_ia, shortcut.AutoTransparent);
                    }
                    rectangle = new Rectangle(((x + ((int) (2f * m_scaleFactor.Width))) + (2 * m_Spacing)) + rectangle3.Width, y, ((width - ((int) (4f * m_scaleFactor.Width))) - (4 * m_Spacing)) - rectangle3.Width, height);
                    this.DrawAlignedText(gr, shortcut.Text, TextAlignment.MiddleLeft, (this.IndexOf(shortcut) == this.m_selectingIndex) ? this.m_brushSelShortcutsFore : this.m_brushShortcutsFore, rectangle);
                }
                else
                {
                    int num10 = x + width;
                    Rectangle rectangle4 = new Rectangle((num10 - (2 * m_Spacing)) - num8, y + ((height - num9) / 2), num8, num9);
                    if (((this.m_ImageList != null) && (shortcut.ImageIndex >= 0)) && (shortcut.ImageIndex < this.m_ImageList.Images.Count))
                    {
                        this.DrawImage(gr, this.m_ImageList.Images[shortcut.ImageIndex], rectangle4, shortcut.m_ia, shortcut.AutoTransparent);
                    }
                    int num11 = gr.MeasureString(shortcut.Text, this.Font).ToSize().Width;
                    int num12 = (((num10 - num11) - ((int) (2f * m_scaleFactor.Width))) - (2 * m_Spacing)) - rectangle4.Width;
                    if (num12 < (x + (2 * m_Spacing)))
                    {
                        num12 = x + (2 * m_Spacing);
                    }
                    rectangle = new Rectangle(num12, y, ((width - ((int) (4f * m_scaleFactor.Width))) - (4 * m_Spacing)) - rectangle4.Width, height);
                    this.DrawAlignedText(gr, shortcut.Text, TextAlignment.MiddleLeft, (this.IndexOf(shortcut) == this.m_selectingIndex) ? this.m_brushSelShortcutsFore : this.m_brushShortcutsFore, rectangle);
                }
                if (!shortcut.Enabled)
                {
                    gr.DrawImage(this.m_bmpDisabled, new Rectangle(x, y, width, height), 0, 0, width, height, GraphicsUnit.Pixel, this.m_attrDisabled);
                }
                x += width;
                if ((x + width) > rect.Width)
                {
                    x = rect.X;
                    num6++;
                    y += height;
                }
            }
            if (((num7 * height) > rect.Height) || (this.m_VScrollShift > 0))
            {
                aNeedScrollbar = true;
            }
            else
            {
                aNeedScrollbar = false;
            }
        }

        private void EnsureVisibleSelectedShortcut()
        {
            if ((this.m_ClientRectangle.Width != 0) && (this.m_ClientRectangle.Height != 0))
            {
                int width = (this.m_viewStyle == Resco.Controls.OutlookControls.ViewStyle.List) ? this.m_Width : this.m_ClientRectangle.Width;
                if (width > this.m_ClientRectangle.Width)
                {
                    width = this.m_ClientRectangle.Width;
                }
                int num2 = (int) ((this.Font.Size * 2f) * m_scaleFactor.Height);
                int num3 = (int) ((((this.m_ImageList != null) ? (this.m_ImageList.ImageSize.Height * m_userScaleFactor.Height) : 0f) + ((this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Bottom) ? ((float) (num2 * 2)) : ((float) 0))) + (4f * m_scaleFactor.Height));
                int num4 = this.m_ClientRectangle.Height / num3;
                int num5 = num4;
                if (this.m_indexChanged && (this.m_selectingIndex >= 0))
                {
                    int num6 = this.m_ClientRectangle.Width / width;
                    int num7 = this.m_VScrollShift / num3;
                    if ((this.m_VScrollShift % num3) > 0)
                    {
                        num7++;
                        num5 = num4 - 1;
                    }
                    int num8 = this.m_selectingIndex / num6;
                    if (num8 < num7)
                    {
                        num7 = num8;
                        this.m_VScrollShift = num7 * num3;
                    }
                    else if (num8 > ((num7 + num5) - 1))
                    {
                        num7 = (num8 - num4) + 1;
                        this.m_VScrollShift = num7 * num3;
                    }
                }
            }
        }

        private Color GetTransparentColor(Bitmap image)
        {
            return image.GetPixel(0, 0);
        }

        public int IndexOf(Resco.Controls.OutlookControls.Shortcut value)
        {
            return base.List.IndexOf(value);
        }

        private void InitTouchTool()
        {
            if (this.m_TouchNavigatorTool == null)
            {
                this.m_TouchNavigatorTool = new OsbTouchTool(m_ParentControl);
                this.m_TouchNavigatorTool.GestureDetected += new OsbTouchTool.GestureDetectedHandler(this.TouchNavigatorTool_GestureDetected);
                this.m_TouchNavigatorTool.MouseMoveDetected += new OsbTouchTool.MouseMoveDetectedHandler(this.TouchNavigatorTool_MouseMoveDetected);
            }
            else
            {
                OsbTouchTool.ParentControl = m_ParentControl;
            }
        }

        public void Insert(int index, Resco.Controls.OutlookControls.Shortcut value)
        {
            base.List.Insert(index, value);
        }

        public void Invalidate()
        {
            if (this.Invalidating != null)
            {
                this.Invalidating(this, new EventArgs());
            }
        }

        private bool IsAbleToScroollDown()
        {
            return (this.m_VScrollShift < (this.m_TouchBgrdHeight - this.m_ClientRectangle.Height));
        }

        private bool IsAbleToScroollUp()
        {
            return (this.m_VScrollShift > 0);
        }

        internal void MouseDown(Point p, Rectangle rect)
        {
            this.m_TouchNavigatorTool.MouseDown(p.X, p.Y);
            int width = (this.m_viewStyle == Resco.Controls.OutlookControls.ViewStyle.List) ? this.m_Width : rect.Width;
            if (width > rect.Width)
            {
                width = rect.Width;
            }
            int num2 = (int) ((this.Font.Size * 2f) * m_scaleFactor.Height);
            int num3 = (int) ((((this.m_ImageList != null) ? (this.m_ImageList.ImageSize.Height * m_userScaleFactor.Height) : 0f) + ((this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Bottom) ? ((float) (num2 * 2)) : ((float) 0))) + (4f * m_scaleFactor.Height));
            if (((this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Right) || (this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Left)) && (num3 < num2))
            {
                num3 = num2;
            }
            this.m_upArrowDown = false;
            this.m_downArrowDown = false;
            int num4 = rect.Width / width;
            int num5 = ((base.List.Count % num4) == 0) ? (base.List.Count / num4) : ((base.List.Count / num4) + 1);
            if ((num5 * num3) > rect.Height)
            {
                Rectangle rectangle = new Rectangle((rect.X + rect.Width) - ((int) (this.m_ScrollBarWidth * m_scaleFactor.Width)), 0, (int) (this.m_ScrollBarWidth * m_scaleFactor.Width), (int) (this.m_ScrollBarHeight * m_scaleFactor.Height));
                if (rectangle.Contains(p.X, p.Y))
                {
                    this.m_upArrowDown = true;
                    return;
                }
                rectangle.Y = rect.Height - ((int) (this.m_ScrollBarHeight * m_scaleFactor.Height));
                if (rectangle.Contains(p.X, p.Y))
                {
                    this.m_downArrowDown = true;
                    return;
                }
            }
            int num6 = -1;
            if (p.X <= (num4 * width))
            {
                int num7 = p.X / width;
                int num8 = ((p.Y - m_Spacing) + this.m_VScrollShift) / num3;
                if (num8 < 0)
                {
                    num8 = 0;
                }
                int num9 = (num8 * num4) + num7;
                num6 = (num9 > (base.List.Count - 1)) ? -1 : num9;
                if (((num6 >= 0) && (num6 < base.Count)) && !this[num6].Enabled)
                {
                    num6 = -1;
                }
            }
            this.m_selectingIndex = (num6 == -1) ? this.m_selectingIndex : num6;
            this.m_pushedIndex = this.m_selectingIndex;
        }

        internal void MouseMove(Point p, Rectangle rect)
        {
            this.m_TouchNavigatorTool.MouseMove(p.X, p.Y);
            int width = (this.m_viewStyle == Resco.Controls.OutlookControls.ViewStyle.List) ? this.m_Width : rect.Width;
            if (width > rect.Width)
            {
                width = rect.Width;
            }
            int num2 = (int) ((this.Font.Size * 2f) * m_scaleFactor.Height);
            int num3 = (int) ((((this.m_ImageList != null) ? (this.m_ImageList.ImageSize.Height * m_userScaleFactor.Height) : 0f) + ((this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Bottom) ? ((float) (num2 * 2)) : ((float) 0))) + (4f * m_scaleFactor.Height));
            if (((this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Right) || (this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Left)) && (num3 < num2))
            {
                num3 = num2;
            }
            this.m_upArrowDown = false;
            this.m_downArrowDown = false;
            int num4 = rect.Width / width;
            int num5 = ((base.List.Count % num4) == 0) ? (base.List.Count / num4) : ((base.List.Count / num4) + 1);
            if ((num5 * num3) > rect.Height)
            {
                Rectangle rectangle = new Rectangle((rect.X + rect.Width) - ((int) (this.m_ScrollBarWidth * m_scaleFactor.Width)), 0, (int) (this.m_ScrollBarWidth * m_scaleFactor.Width), (int) (this.m_ScrollBarHeight * m_scaleFactor.Height));
                if (rectangle.Contains(p.X, p.Y))
                {
                    this.m_upArrowDown = true;
                    return;
                }
                rectangle.Y = rect.Height - ((int) (this.m_ScrollBarHeight * m_scaleFactor.Height));
                if (rectangle.Contains(p.X, p.Y))
                {
                    this.m_downArrowDown = true;
                    return;
                }
            }
            int num6 = -1;
            if (p.X <= (num4 * width))
            {
                int num7 = p.X / width;
                int num8 = ((p.Y - m_Spacing) + this.m_VScrollShift) / num3;
                if (num8 < 0)
                {
                    num8 = 0;
                }
                int num9 = (num8 * num4) + num7;
                num6 = (num9 > (base.List.Count - 1)) ? -1 : num9;
                if (((num6 >= 0) && (num6 < base.Count)) && !this[num6].Enabled)
                {
                    num6 = -1;
                }
            }
            this.m_selectingIndex = (num6 == -1) ? this.m_selectingIndex : num6;
        }

        internal void MouseUp(Point p, Rectangle rect)
        {
            bool flag = this.m_TouchNavigatorTool.MouseUp(p.X, p.Y);
            int width = (this.m_viewStyle == Resco.Controls.OutlookControls.ViewStyle.List) ? this.m_Width : rect.Width;
            if (width > rect.Width)
            {
                width = rect.Width;
            }
            int num2 = (int) ((this.Font.Size * 2f) * m_scaleFactor.Height);
            int num3 = (int) ((((this.m_ImageList != null) ? (this.m_ImageList.ImageSize.Height * m_userScaleFactor.Height) : 0f) + ((this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Bottom) ? ((float) (num2 * 2)) : ((float) 0))) + (4f * m_scaleFactor.Height));
            if (((this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Right) || (this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Left)) && (num3 < num2))
            {
                num3 = num2;
            }
            int num4 = rect.Width / width;
            int num5 = ((base.List.Count % num4) == 0) ? (base.List.Count / num4) : ((base.List.Count / num4) + 1);
            if ((num5 * num3) > rect.Height)
            {
                Rectangle rectangle = new Rectangle((rect.X + rect.Width) - ((int) (this.m_ScrollBarWidth * m_scaleFactor.Width)), 0, (int) (this.m_ScrollBarWidth * m_scaleFactor.Width), (int) (this.m_ScrollBarHeight * m_scaleFactor.Height));
                if (rectangle.Contains(p.X, p.Y) && this.m_upArrowDown)
                {
                    int num6 = this.m_VScrollShift / num3;
                    num6 -= (num6 == 0) ? 0 : 1;
                    this.m_VScrollShift = num6 * num3;
                    this.m_upArrowDown = false;
                    return;
                }
                rectangle.Y = rect.Height - ((int) (this.m_ScrollBarHeight * m_scaleFactor.Height));
                if (rectangle.Contains(p.X, p.Y) && this.m_downArrowDown)
                {
                    int num7 = this.m_VScrollShift / num3;
                    num7 += (num7 >= (num5 - (rect.Height / num3))) ? 0 : 1;
                    this.m_VScrollShift = num7 * num3;
                    this.m_downArrowDown = false;
                    return;
                }
            }
            this.m_upArrowDown = false;
            this.m_downArrowDown = false;
            if (!flag)
            {
                int shortcutIndex = -1;
                if (p.X <= (num4 * width))
                {
                    int num9 = p.X / width;
                    int num1 = this.m_VScrollShift % num3;
                    int num10 = ((p.Y - m_Spacing) + this.m_VScrollShift) / num3;
                    if (num10 < 0)
                    {
                        num10 = 0;
                    }
                    int num11 = (num10 * num4) + num9;
                    shortcutIndex = (num11 > (base.List.Count - 1)) ? -1 : num11;
                    if (((shortcutIndex >= 0) && (shortcutIndex < base.Count)) && !this[shortcutIndex].Enabled)
                    {
                        shortcutIndex = -1;
                    }
                }
                this.m_bDoInvalidate = false;
                this.SelectedIndex = (shortcutIndex == -1) ? this.m_selectedIndex : shortcutIndex;
                if ((shortcutIndex >= 0) && (this.ShortcutEntered != null))
                {
                    this.ShortcutEntered(this, new SelectedIndexChangedEventArgs(-1, shortcutIndex, SelectedIndexChangedEventArgs.SelectionType.ByMouse));
                }
            }
        }

        protected override void OnClear()
        {
            base.OnClear();
        }

        protected override void OnInsert(int index, object value)
        {
            if (!typeof(Resco.Controls.OutlookControls.Shortcut).IsInstanceOfType(value))
            {
                throw new ArgumentException("Value must be of type Shortcut.");
            }
            if (this.InsertEvent != null)
            {
                this.InsertEvent(this, new InsertEventArgs(index, value));
            }
            base.OnInsert(index, value);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            if (this.InsertCompleteEvent != null)
            {
                this.InsertCompleteEvent(this, new InsertEventArgs(index, value));
            }
            ((Resco.Controls.OutlookControls.Shortcut) value).Invalidating += new EventHandler(this.OnInvalidating);
            ((Resco.Controls.OutlookControls.Shortcut) value).Parent = this;
            base.OnInsertComplete(index, value);
        }

        private void OnInvalidating(object sender, EventArgs e)
        {
            this.RefreshShortcutsOnNextDraw();
            if (this.Invalidating != null)
            {
                this.Invalidating(sender, e);
            }
        }

        private void OnMouseGestureDetected(OsbTouchTool.GestureType gestureType)
        {
            if (GestureDetected != null)
            {
                Resco.Controls.OutlookControls.GestureEventArgs e = new Resco.Controls.OutlookControls.GestureEventArgs(gestureType);
                GestureDetected(this, e);
            }
        }

        protected override void OnRemove(int index, object value)
        {
            if (!typeof(Resco.Controls.OutlookControls.Shortcut).IsInstanceOfType(value))
            {
                throw new ArgumentException("Value must be of type Shortcut.");
            }
            if (this.RemoveEvent != null)
            {
                this.RemoveEvent(this, new RemoveEventArgs(index, value));
            }
            base.OnRemove(index, value);
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            if (this.RemoveCompleteEvent != null)
            {
                this.RemoveCompleteEvent(this, new RemoveEventArgs(index, value));
            }
            ((Resco.Controls.OutlookControls.Shortcut) value).Invalidating -= new EventHandler(this.OnInvalidating);
            base.OnRemoveComplete(index, value);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (!typeof(Resco.Controls.OutlookControls.Shortcut).IsInstanceOfType(newValue))
            {
                throw new ArgumentException("Value must be of type Shortcut.");
            }
            if (this.SetEvent != null)
            {
                this.SetEvent(this, new SetEventArgs(index, oldValue, newValue));
            }
            base.OnSet(index, oldValue, newValue);
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (this.SetCompleteEvent != null)
            {
                this.SetCompleteEvent(this, new SetEventArgs(index, oldValue, newValue));
            }
            base.OnSetComplete(index, oldValue, newValue);
        }

        protected override void OnValidate(object value)
        {
            if (!typeof(Resco.Controls.OutlookControls.Shortcut).IsInstanceOfType(value))
            {
                throw new ArgumentException("Value must be of type Shortcut.");
            }
        }

        private void PrepareTouchBackground(Graphics gr, Rectangle rect, Pen frame, out bool aNeedScrollbar)
        {
            int num = this.TouchBgrdHeight(rect);
            if (this.m_TouchBgrdHeight != num)
            {
                this.ResetTouchBgrd();
            }
            this.m_TouchBgrdHeight = num;
            if (this.m_Background == null)
            {
                this.m_ClientRectangle = rect;
                this.m_Background = new Bitmap(rect.Width, this.m_TouchBgrdHeight);
                this.m_indexChanged = true;
            }
            Graphics.FromImage(this.m_Background);
            Rectangle rectangle = rect;
            Region region = new Region(rect);
            gr.Clip = region;
            rectangle.Location = new Point(rect.X, rect.Y - this.m_VScrollShift);
            this.DrawBackground(gr, rectangle, frame, out aNeedScrollbar);
            gr.ResetClip();
            region.Dispose();
            region = null;
            this.m_OldNeedScrollbarState = aNeedScrollbar;
        }

        private void PrepareTouchBackground2(Graphics gr, Rectangle rect, Pen frame, out bool aNeedScrollbar)
        {
            int num = this.TouchBgrdHeight(rect);
            if (this.m_TouchBgrdHeight != num)
            {
                this.ResetTouchBgrd();
            }
            this.m_TouchBgrdHeight = num;
            if (this.m_Background == null)
            {
                this.m_ClientRectangle = rect;
                this.m_Background = new Bitmap(rect.Width, this.m_TouchBgrdHeight);
                this.m_indexChanged = true;
            }
            if (this.m_indexChanged)
            {
                Graphics graphics = Graphics.FromImage(this.m_Background);
                graphics.Clear(this.TransparentColor);
                Rectangle rectangle = rect;
                rectangle.Location = new Point(0, 0);
                this.DrawBackground(graphics, rectangle, frame, out aNeedScrollbar);
                this.m_OldNeedScrollbarState = aNeedScrollbar;
            }
            else
            {
                aNeedScrollbar = this.m_OldNeedScrollbarState;
            }
        }

        internal void RefreshShortcutsOnNextDraw()
        {
            if (this.m_Background != null)
            {
                this.m_Background.Dispose();
                this.m_Background = null;
            }
        }

        public void Remove(Resco.Controls.OutlookControls.Shortcut value)
        {
            base.List.Remove(value);
        }

        private void ResetTouchBgrd()
        {
            this.RefreshShortcutsOnNextDraw();
            this.m_VScrollShift = 0;
        }

        private void ResizeTouchBackground(Rectangle rect)
        {
            if ((this.m_Background != null) && ((rect.Width != this.m_ClientRectangle.Width) || (rect.Height != this.m_ClientRectangle.Height)))
            {
                this.ResetTouchBgrd();
            }
        }

        private Rectangle ShowArrows(Graphics gr, Rectangle rect, Pen frame)
        {
            Point[] array = new Point[3];
            m_Arrow.CopyTo(array, 0);
            Rectangle rectangle = new Rectangle((rect.X + rect.Width) - ((int) (this.m_ScrollBarWidth * m_scaleFactor.Width)), rect.Y - 1, (int) (this.m_ScrollBarWidth * m_scaleFactor.Width), (int) (this.m_ScrollBarHeight * m_scaleFactor.Height));
            int num = 10;
            int num2 = 5;
            int num3 = (((rectangle.Width - num) / 2) - m_Arrow[0].X) + 1;
            int num4 = (((rectangle.Height - num2) / 2) - m_Arrow[0].Y) + 1;
            array[0].Offset((rectangle.X - ((int) (1f * m_scaleFactor.Width))) + num3, (rectangle.Y + ((int) (4f * m_scaleFactor.Height))) + num4);
            array[1].Offset((rectangle.X + ((int) (1f * m_scaleFactor.Width))) + num3, (rectangle.Y + ((int) (4f * m_scaleFactor.Height))) + num4);
            array[2].Offset(rectangle.X + num3, (rectangle.Y - ((int) (7f * m_scaleFactor.Height))) + num4);
            gr.FillRectangle(this.m_upArrowDown ? this.m_brushShortcutsFore : this.m_brushShortcutsBack, rectangle);
            gr.DrawRectangle(frame, rectangle);
            if (this.IsAbleToScroollUp())
            {
                gr.FillPolygon(this.m_upArrowDown ? this.m_brushShortcutsBack : this.m_brushShortcutsFore, array);
            }
            else
            {
                gr.FillPolygon(this.m_brushDimmedScrollbar, array);
            }
            Point[] pointArray2 = new Point[3];
            m_Arrow.CopyTo(pointArray2, 0);
            rectangle.Y = (rect.Y + rect.Height) - ((int) (this.m_ScrollBarHeight * m_scaleFactor.Height));
            pointArray2[0].Offset(rectangle.X + num3, rectangle.Y + num4);
            pointArray2[1].Offset(rectangle.X + num3, rectangle.Y + num4);
            pointArray2[2].Offset(rectangle.X + num3, rectangle.Y + num4);
            gr.FillRectangle(this.m_downArrowDown ? this.m_brushShortcutsFore : this.m_brushShortcutsBack, rectangle);
            gr.DrawRectangle(frame, rectangle);
            if (this.IsAbleToScroollDown())
            {
                gr.FillPolygon(this.m_downArrowDown ? this.m_brushShortcutsBack : this.m_brushShortcutsFore, pointArray2);
                return rect;
            }
            gr.FillPolygon(this.m_brushDimmedScrollbar, pointArray2);
            return rect;
        }

        private int TouchBgrdHeight(Rectangle rect)
        {
            if (base.List.Count == 0)
            {
                return rect.Height;
            }
            int width = (this.m_viewStyle == Resco.Controls.OutlookControls.ViewStyle.List) ? this.m_Width : rect.Width;
            if (width > rect.Width)
            {
                width = rect.Width;
            }
            int num2 = (int) ((this.Font.Size * 2f) * m_scaleFactor.Height);
            int num3 = (int) ((((this.m_ImageList != null) ? (this.m_ImageList.ImageSize.Height * m_userScaleFactor.Height) : 0f) + ((this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Bottom) ? ((float) (num2 * 2)) : ((float) 0))) + (15f * m_scaleFactor.Height));
            if (((this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Right) || (this.m_textStyle == Resco.Controls.OutlookControls.TextStyle.Left)) && (num3 < num2))
            {
                num3 = num2;
            }
            int num4 = ((base.List.Count - 1) / (rect.Width / width)) + 1;
            return (num4 * num3);
        }

        private void TouchNavigatorTool_GestureDetected(object sender, OsbTouchTool.GestureEventArgs e)
        {
            switch (e.Gesture)
            {
                case OsbTouchTool.GestureType.Up:
                case OsbTouchTool.GestureType.Down:
                    break;

                default:
                    this.OnMouseGestureDetected(e.Gesture);
                    break;
            }
        }

        private void TouchNavigatorTool_MouseMoveDetected(object sender, OsbTouchTool.MouseMoveEventArgs e)
        {
            this.VScrollShift += e.MoveY;
            m_ParentControl.Refresh();
        }

        public Resco.Controls.OutlookControls.Shortcut[] All
        {
            get
            {
                Resco.Controls.OutlookControls.Shortcut[] array = new Resco.Controls.OutlookControls.Shortcut[base.List.Count];
                base.List.CopyTo(array, 0);
                return array;
            }
            set
            {
                base.List.Clear();
                foreach (Resco.Controls.OutlookControls.Shortcut shortcut in value)
                {
                    this.Add(shortcut);
                }
            }
        }

        public bool AutoScaleEnabled
        {
            get
            {
                return this.m_bAutoScaleEnabled;
            }
            set
            {
                this.m_bAutoScaleEnabled = value;
            }
        }

        public Color BackColor
        {
            get
            {
                return this.m_shortcutsBackColor;
            }
            set
            {
                if (value != this.m_shortcutsBackColor)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.Window;
                    }
                    this.m_shortcutsBackColor = value;
                    this.Invalidate();
                }
            }
        }

        public Color DimmedScrollbarColor
        {
            get
            {
                return this.m_DimmedScrollbarColor;
            }
            set
            {
                if (value != this.m_DimmedScrollbarColor)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.GrayText;
                    }
                    this.m_DimmedScrollbarColor = value;
                    this.Invalidate();
                }
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this.m_Font;
            }
            set
            {
                if (value != this.m_Font)
                {
                    this.m_Font = value;
                    this.Invalidate();
                }
            }
        }

        public Color ForeColor
        {
            get
            {
                return this.m_shortcutsForeColor;
            }
            set
            {
                if (value != this.m_shortcutsForeColor)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.WindowText;
                    }
                    this.m_shortcutsForeColor = value;
                    this.Invalidate();
                }
            }
        }

        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                return this.m_ImageList;
            }
            set
            {
                this.m_ImageList = value;
                this.Invalidate();
            }
        }

        public Resco.Controls.OutlookControls.Shortcut this[string name]
        {
            get
            {
                int num = 0;
                foreach (Resco.Controls.OutlookControls.Shortcut shortcut in base.List)
                {
                    if (shortcut.Name == name)
                    {
                        return shortcut;
                    }
                    num++;
                }
                throw new ArgumentOutOfRangeException();
            }
            set
            {
                int num = 0;
                foreach (Resco.Controls.OutlookControls.Shortcut shortcut in base.List)
                {
                    if (shortcut.Name == name)
                    {
                        base.List[num] = value;
                        return;
                    }
                    num++;
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        public Resco.Controls.OutlookControls.Shortcut this[int index]
        {
            get
            {
                if ((index < 0) || (index >= base.List.Count))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return (Resco.Controls.OutlookControls.Shortcut) base.List[index];
            }
            set
            {
                if ((index < 0) || (index >= base.List.Count))
                {
                    throw new ArgumentOutOfRangeException();
                }
                base.List[index] = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }

        internal static UserControl ParentControl
        {
            get
            {
                return m_ParentControl;
            }
            set
            {
                m_ParentControl = value;
                OsbTouchTool.ParentControl = value;
            }
        }

        internal int ScrollBarHeight
        {
            get
            {
                return this.m_ScrollBarHeight;
            }
            set
            {
                if (value != this.m_ScrollBarHeight)
                {
                    this.m_ScrollBarHeight = value;
                    this.Invalidate();
                }
            }
        }

        internal int ScrollBarWidth
        {
            get
            {
                return this.m_ScrollBarWidth;
            }
            set
            {
                if (value != this.m_ScrollBarWidth)
                {
                    this.m_ScrollBarWidth = value;
                    this.Invalidate();
                }
            }
        }

        public Color SelBackColor
        {
            get
            {
                return this.m_selShortcutsBackColor;
            }
            set
            {
                if (value != this.m_selShortcutsBackColor)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.Highlight;
                    }
                    this.m_selShortcutsBackColor = value;
                    this.Invalidate();
                }
            }
        }

        public int SelectedIndex
        {
            get
            {
                if (this.m_selectingIndex < 0)
                {
                    return this.m_selectedIndex;
                }
                return this.m_selectingIndex;
            }
            set
            {
                this.m_selectingIndex = value;
                this.m_pushedIndex = -1;
                if (this.m_selectedIndex != value)
                {
                    this.m_selectedIndex = value;
                    this.m_indexChanged = true;
                    this.EnsureVisibleSelectedShortcut();
                    if (this.m_bDoInvalidate)
                    {
                        this.Invalidate();
                    }
                    if (this.SelectedShortcutIndexChanged != null)
                    {
                        this.SelectedShortcutIndexChanged(this, new SelectedIndexChangedEventArgs(-1, this.m_selectedIndex));
                    }
                }
                this.m_bDoInvalidate = true;
            }
        }

        public Color SelForeColor
        {
            get
            {
                return this.m_selShortcutsForeColor;
            }
            set
            {
                if (value != this.m_selShortcutsForeColor)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.HighlightText;
                    }
                    this.m_selShortcutsForeColor = value;
                    this.Invalidate();
                }
            }
        }

        public Resco.Controls.OutlookControls.TextStyle TextStyle
        {
            get
            {
                return this.m_textStyle;
            }
            set
            {
                if (value != this.m_textStyle)
                {
                    this.m_textStyle = value;
                    this.Invalidate();
                }
            }
        }

        public static SizeF UserScaleFactor
        {
            get
            {
                return m_userScaleFactor;
            }
            set
            {
                m_userScaleFactor = value;
            }
        }

        public Resco.Controls.OutlookControls.ViewStyle ViewStyle
        {
            get
            {
                return this.m_viewStyle;
            }
            set
            {
                if (value != this.m_viewStyle)
                {
                    this.m_viewStyle = value;
                    this.Invalidate();
                }
            }
        }

        private int VScrollShift
        {
            get
            {
                return this.m_VScrollShift;
            }
            set
            {
                int num = value;
                if ((num < 0) || (this.m_TouchBgrdHeight < this.m_ClientRectangle.Height))
                {
                    this.m_VScrollShift = 0;
                }
                else if (num > (this.m_TouchBgrdHeight - this.m_ClientRectangle.Height))
                {
                    this.m_VScrollShift = this.m_TouchBgrdHeight - this.m_ClientRectangle.Height;
                }
                else
                {
                    this.m_VScrollShift = num;
                }
            }
        }

        public int Width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                if (this.m_Width != value)
                {
                    this.m_Width = value;
                    if (this.m_bDoInvalidate)
                    {
                        this.Invalidate();
                    }
                    this.m_bDoInvalidate = true;
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

