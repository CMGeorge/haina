namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Threading;
    using System.Windows.Forms;

    public class ItemPageBreak : Item
    {
        private ImageAttributes _ia;
        private Pen m_BlackPen;
        private bool m_bVisible;
        private SolidBrush m_GrayBrush;
        private RescoPageNumberStyle m_PageNumberStyle;
        private int m_RightArrow;
        private bool m_ShowPageText;
        private int m_TextHeight;
        private int m_TextWidth;
        private SolidBrush m_WhiteBrush;

        public ItemPageBreak()
        {
            this.m_WhiteBrush = new SolidBrush(Color.White);
            this.m_GrayBrush = new SolidBrush(SystemColors.Control);
            this.m_BlackPen = new Pen(Color.Black);
            this.m_PageNumberStyle = RescoPageNumberStyle.CurrentOfAll;
            this.m_ShowPageText = true;
            this.m_RightArrow = 0;
            this.m_bVisible = true;
        }

        public ItemPageBreak(Item toCopy) : base(toCopy)
        {
            if (toCopy is ItemPageBreak)
            {
                this.PageNumberStyle = ((ItemPageBreak) toCopy).PageNumberStyle;
                this.ShowPageText = ((ItemPageBreak) toCopy).ShowPageText;
            }
            this.m_WhiteBrush = new SolidBrush(Color.White);
            this.m_GrayBrush = new SolidBrush(SystemColors.Control);
            this.m_BlackPen = new Pen(Color.Black);
            this.m_RightArrow = 0;
        }

        private bool CheckStartDrawPage(int index, int dir)
        {
            PageCollection pages = base.Parent.Pages;
            int pageCount = base.Parent.PageCount;
            do
            {
                index += dir;
            }
            while (((index < pageCount) && (index >= 0)) && !pages[index].PagingItem.Visible);
            return ((index >= 0) && (index != pageCount));
        }

        protected override void Click(int yOffset, int parentWidth)
        {
            Rectangle rectangle;
            bool pagesRightToLeft;
            Point lastMousePosition;
            int num2;
            PageCollection pages;
            int pageCount;
            int num4;
            int left = yOffset;
            if (base.Parent != null)
            {
                rectangle = base.Parent.CalculateClientRect();
                pagesRightToLeft = this.PagesRightToLeft;
                switch (this.PageStyle)
                {
                    case RescoPageStyle.TabStrip:
                        lastMousePosition = base.Parent.LastMousePosition;
                        lastMousePosition.X -= rectangle.X;
                        lastMousePosition.Y -= rectangle.Y;
                        num2 = pagesRightToLeft ? rectangle.Width : 0;
                        pages = base.Parent.Pages;
                        pageCount = base.Parent.PageCount;
                        num4 = 0;
                        goto Label_06FA;

                    case RescoPageStyle.Dots:
                        this.ClickDots(left, parentWidth);
                        return;
                }
                if (((base.Parent.CurrentPageIndex != 0) && (left > this.m_RightArrow)) && (left < (this.m_RightArrow + Resco.Controls.DetailView.DetailView.ArrowImageSize.Width)))
                {
                    Resco.Controls.DetailView.DetailView parent = base.Parent;
                    parent.CurrentPageIndex--;
                }
                if (base.Parent.CurrentPageIndex != (base.Parent.PageCount - 1))
                {
                    if (pagesRightToLeft)
                    {
                        if ((left - rectangle.X) < Resco.Controls.DetailView.DetailView.ArrowImageSize.Width)
                        {
                            Resco.Controls.DetailView.DetailView view2 = base.Parent;
                            view2.CurrentPageIndex++;
                            return;
                        }
                        return;
                    }
                    if ((left - rectangle.X) <= (rectangle.Width - Resco.Controls.DetailView.DetailView.ArrowImageSize.Width))
                    {
                        return;
                    }
                    Resco.Controls.DetailView.DetailView view3 = base.Parent;
                    view3.CurrentPageIndex++;
                }
            }
            return;
        Label_01AE:
            if ((((lastMousePosition.X > (pagesRightToLeft ? Resco.Controls.DetailView.DetailView.TabImageSize.Width : (rectangle.Width - (2 * Resco.Controls.DetailView.DetailView.TabImageSize.Width)))) && (lastMousePosition.X < (pagesRightToLeft ? (2 * Resco.Controls.DetailView.DetailView.TabImageSize.Width) : (rectangle.Width - Resco.Controls.DetailView.DetailView.TabImageSize.Width)))) && (lastMousePosition.Y > ((base.Parent.PagesLocation == RescoPagesLocation.Bottom) ? (rectangle.Height - (2 * Resco.Controls.DetailView.DetailView.TabImageSize.Height)) : 0))) && (lastMousePosition.Y < ((base.Parent.PagesLocation == RescoPagesLocation.Bottom) ? (rectangle.Height - Resco.Controls.DetailView.DetailView.TabImageSize.Height) : Resco.Controls.DetailView.DetailView.TabImageSize.Height)))
            {
                if ((base.Parent.m_StartDrawPage > 0) && this.CheckStartDrawPage(num4, -1))
                {
                    if (pagesRightToLeft)
                    {
                        base.Parent.m_RightArrowClicked = true;
                    }
                    else
                    {
                        base.Parent.m_LeftArrowClicked = true;
                    }
                    base.Parent.Refresh();
                    Thread.Sleep(50);
                    if (pagesRightToLeft)
                    {
                        base.Parent.m_RightArrowClicked = false;
                    }
                    else
                    {
                        base.Parent.m_LeftArrowClicked = false;
                    }
                    this.UpdateStartDrawPage(num4, -1);
                    base.Parent.Refresh();
                }
                return;
            }
            if ((((lastMousePosition.X > (pagesRightToLeft ? 0 : (rectangle.Width - Resco.Controls.DetailView.DetailView.TabImageSize.Width))) && (lastMousePosition.X < (pagesRightToLeft ? Resco.Controls.DetailView.DetailView.TabImageSize.Width : rectangle.Width))) && (lastMousePosition.Y > ((base.Parent.PagesLocation == RescoPagesLocation.Bottom) ? (rectangle.Height - (2 * Resco.Controls.DetailView.DetailView.TabImageSize.Height)) : 0))) && (lastMousePosition.Y < ((base.Parent.PagesLocation == RescoPagesLocation.Bottom) ? (rectangle.Height - Resco.Controls.DetailView.DetailView.TabImageSize.Height) : Resco.Controls.DetailView.DetailView.TabImageSize.Height)))
            {
                if ((base.Parent.m_StartDrawPage < (base.Parent.PageCount - 1)) && this.CheckStartDrawPage(num4, 1))
                {
                    if (pagesRightToLeft)
                    {
                        base.Parent.m_LeftArrowClicked = true;
                    }
                    else
                    {
                        base.Parent.m_RightArrowClicked = true;
                    }
                    base.Parent.Refresh();
                    Thread.Sleep(50);
                    if (pagesRightToLeft)
                    {
                        base.Parent.m_LeftArrowClicked = false;
                    }
                    else
                    {
                        base.Parent.m_RightArrowClicked = false;
                    }
                    this.UpdateStartDrawPage(num4, 1);
                    base.Parent.Refresh();
                }
                return;
            }
        Label_0633:
            if (pagesRightToLeft)
            {
                if ((lastMousePosition.X > (num2 - (pages[num4].PagingItem.m_TextWidth + 10))) && (lastMousePosition.X < num2))
                {
                    base.Parent.CurrentPageIndex = num4;
                    return;
                }
                num2 -= pages[num4].PagingItem.m_TextWidth + 10;
                if (num2 < 0)
                {
                    return;
                }
            }
            else
            {
                if ((lastMousePosition.X > num2) && (lastMousePosition.X < ((num2 + pages[num4].PagingItem.m_TextWidth) + 10)))
                {
                    base.Parent.CurrentPageIndex = num4;
                    return;
                }
                num2 += pages[num4].PagingItem.m_TextWidth + 10;
                if (num2 > rectangle.Width)
                {
                    return;
                }
            }
        Label_06F4:
            num4++;
        Label_06FA:
            if (num4 < pageCount)
            {
                if ((num4 < base.Parent.m_StartDrawPage) || !pages[num4].PagingItem.Visible)
                {
                    goto Label_06F4;
                }
                if (!base.Parent.m_PagesOverWidth)
                {
                    goto Label_0633;
                }
                switch (this.ArrowStyle)
                {
                    case RescoArrowStyle.LeftRight:
                        goto Label_01AE;

                    case RescoArrowStyle.UpDown:
                        if ((((lastMousePosition.X <= (pagesRightToLeft ? 0 : (rectangle.Width - Resco.Controls.DetailView.DetailView.TabImageSize.Width))) || (lastMousePosition.X >= (pagesRightToLeft ? Resco.Controls.DetailView.DetailView.TabImageSize.Width : rectangle.Width))) || (lastMousePosition.Y <= ((base.Parent.PagesLocation == RescoPagesLocation.Bottom) ? (rectangle.Height - (2 * Resco.Controls.DetailView.DetailView.TabImageSize.Height)) : 0))) || (lastMousePosition.Y >= ((base.Parent.PagesLocation == RescoPagesLocation.Bottom) ? (rectangle.Height - Resco.Controls.DetailView.DetailView.TabImageSize.Height) : Resco.Controls.DetailView.DetailView.TabImageSize.Height)))
                        {
                            if ((((lastMousePosition.X > (pagesRightToLeft ? 0 : (rectangle.Width - Resco.Controls.DetailView.DetailView.TabImageSize.Width))) && (lastMousePosition.X < (pagesRightToLeft ? Resco.Controls.DetailView.DetailView.TabImageSize.Width : rectangle.Width))) && (lastMousePosition.Y > ((base.Parent.PagesLocation == RescoPagesLocation.Bottom) ? (rectangle.Height - Resco.Controls.DetailView.DetailView.TabImageSize.Height) : Resco.Controls.DetailView.DetailView.TabImageSize.Height))) && (lastMousePosition.Y < ((base.Parent.PagesLocation == RescoPagesLocation.Bottom) ? rectangle.Height : (2 * Resco.Controls.DetailView.DetailView.TabImageSize.Height))))
                            {
                                if ((base.Parent.m_StartDrawPage < (base.Parent.PageCount - 1)) && this.CheckStartDrawPage(num4, 1))
                                {
                                    base.Parent.m_RightArrowClicked = true;
                                    base.Parent.Refresh();
                                    Thread.Sleep(50);
                                    base.Parent.m_RightArrowClicked = false;
                                    this.UpdateStartDrawPage(num4, 1);
                                    base.Parent.Refresh();
                                }
                                return;
                            }
                            goto Label_0633;
                        }
                        if ((base.Parent.m_StartDrawPage > 0) && this.CheckStartDrawPage(num4, -1))
                        {
                            base.Parent.m_LeftArrowClicked = true;
                            base.Parent.Refresh();
                            Thread.Sleep(50);
                            base.Parent.m_LeftArrowClicked = false;
                            this.UpdateStartDrawPage(num4, -1);
                            base.Parent.Refresh();
                        }
                        return;
                }
                goto Label_01AE;
            }
        }

        private void ClickDots(int left, int width)
        {
            Resco.Controls.DetailView.DetailView parent = base.Parent;
            Rectangle rectangle = parent.CalculateClientRect();
            PageCollection pages = parent.Pages;
            int height = this.Height;
            bool pagesRightToLeft = this.PagesRightToLeft;
            int pageCount = parent.PageCount;
            int num3 = 0;
            foreach (Page page in pages)
            {
                if (page.PagingItem.Visible)
                {
                    num3++;
                }
            }
            int num4 = height / 4;
            int num5 = (rectangle.X + ((width - (num3 * height)) / 2)) + num4;
            for (int i = 0; i < pageCount; i++)
            {
                int num6 = pagesRightToLeft ? ((pageCount - i) - 1) : i;
                if (pages[num6].PagingItem.Visible)
                {
                    if ((left > num5) && (left < (num5 + height)))
                    {
                        parent.CurrentPageIndex = num6;
                        return;
                    }
                    num5 += height;
                }
            }
        }

        public override Item Clone()
        {
            ItemPageBreak @break = new ItemPageBreak(this);
            @break.Text = this.Text;
            return @break;
        }

        protected override void Draw(Graphics gr, int yOffset, int parentWidth)
        {
            Resco.Controls.DetailView.DetailView parent = base.Parent;
            if (parent != null)
            {
                Rectangle rectangle = parent.CalculateClientRect();
                int height = this.Height;
                if (height > 0)
                {
                    yOffset = rectangle.Y + ((parent.PagesLocation == RescoPagesLocation.Top) ? 0 : (rectangle.Height - height));
                    if ((base.LabelBackColor != Color.Transparent) || !parent.UseGradient)
                    {
                        gr.FillRectangle(base.m_LabelBackBrush, rectangle.X, yOffset, rectangle.Width, height);
                    }
                    else if (parent.UseGradient)
                    {
                        Rectangle dstrc = new Rectangle(rectangle.X, yOffset, rectangle.Width, height);
                        Rectangle srcrc = rectangle;
                        GradientFill.Fill(gr, dstrc, srcrc, base.Parent.GradientBackColor);
                    }
                    switch (this.PageStyle)
                    {
                        case RescoPageStyle.TabStrip:
                            this.DrawTabStrip(gr, rectangle.X, yOffset, rectangle.Width, height);
                            return;

                        case RescoPageStyle.Dots:
                            this.DrawDots(gr, rectangle.X, yOffset, rectangle.Width, height);
                            return;
                    }
                    this.DrawArrows(gr, rectangle.X, yOffset, rectangle.Width, height);
                }
            }
        }

        private void DrawArrows(Graphics gr, int left, int top, int width, int height)
        {
            Image image;
            Image image2;
            Resco.Controls.DetailView.DetailView parent = base.Parent;
            Font labelFont = base.LabelFont;
            string text = null;
            int currentPageIndex = parent.CurrentPageIndex;
            int pageCount = parent.PageCount;
            int y = top + ((height - Resco.Controls.DetailView.DetailView.ArrowImageSize.Height) / 2);
            int num6 = top;
            bool pagesRightToLeft = this.PagesRightToLeft;
            bool splitArrows = this.SplitArrows;
            int num7 = left;
            if (parent.PagesLocation == RescoPagesLocation.Top)
            {
                gr.DrawLine(this.m_BlackPen, left, (top + height) - 1, left + width, (top + height) - 1);
            }
            else
            {
                gr.DrawLine(this.m_BlackPen, left, top - 1, left + width, top - 1);
            }
            left += pagesRightToLeft ? 0 : (width - Resco.Controls.DetailView.DetailView.ArrowImageSize.Width);
            bool isLeftRight = this.ArrowStyle == RescoArrowStyle.LeftRight;
            if (pagesRightToLeft)
            {
                image = Resco.Controls.DetailView.DetailView.GetArrow(false, isLeftRight, currentPageIndex == (pageCount - 1), false);
                image2 = Resco.Controls.DetailView.DetailView.GetArrow(true, isLeftRight, currentPageIndex == 0, false);
            }
            else
            {
                image = Resco.Controls.DetailView.DetailView.GetArrow(true, isLeftRight, currentPageIndex == (pageCount - 1), false);
                image2 = Resco.Controls.DetailView.DetailView.GetArrow(false, isLeftRight, currentPageIndex == 0, false);
            }
            Rectangle destRect = new Rectangle(left, y, Resco.Controls.DetailView.DetailView.ArrowImageSize.Width, Resco.Controls.DetailView.DetailView.ArrowImageSize.Height);
            if (this._ia == null)
            {
                Color pixel = ((Bitmap) image).GetPixel(0, 0);
                this._ia = new ImageAttributes();
                this._ia.SetColorKey(pixel, pixel);
            }
            gr.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, this._ia);
            left += pagesRightToLeft ? 10 : -10;
            switch (this.PageNumberStyle)
            {
                case RescoPageNumberStyle.None:
                    break;

                case RescoPageNumberStyle.Current:
                    text = string.Format("{0}", currentPageIndex + 1);
                    break;

                default:
                    if (pagesRightToLeft)
                    {
                        text = string.Format("{1}/{0}", currentPageIndex + 1, pageCount);
                    }
                    else
                    {
                        text = string.Format("{0}/{1}", currentPageIndex + 1, pageCount);
                    }
                    break;
            }
            int num2 = left;
            int num = 0;
            if (text != null)
            {
                SizeF ef = gr.MeasureString(text, labelFont);
                num = Convert.ToInt32(Math.Ceiling((double) ef.Width));
                if (splitArrows)
                {
                    left = num7 + ((width - num) / 2);
                }
                else
                {
                    left += num * (pagesRightToLeft ? 1 : -1);
                }
                height = Convert.ToInt32(Math.Ceiling((double) ef.Height));
                num6 += (this.Height - height) / 2;
                gr.DrawString(text, labelFont, base.m_LabelForeBrush, (float) left, (float) num6);
                if (!splitArrows)
                {
                    left += pagesRightToLeft ? (num + 10) : -10;
                }
                num2 = left;
            }
            if (splitArrows)
            {
                left = num7 + (pagesRightToLeft ? (width - Resco.Controls.DetailView.DetailView.ArrowImageSize.Width) : Resco.Controls.DetailView.DetailView.ArrowImageSize.Width);
            }
            left += pagesRightToLeft ? 0 : -Resco.Controls.DetailView.DetailView.ArrowImageSize.Width;
            destRect.X = left;
            gr.DrawImage(image2, destRect, 0, 0, image2.Width, image2.Height, GraphicsUnit.Pixel, this._ia);
            this.m_RightArrow = left;
            if (this.ShowPageText)
            {
                text = this.Text;
                labelFont = base.TextFont;
                if (pagesRightToLeft)
                {
                    if (splitArrows)
                    {
                        base.DrawAlignmentString(gr, text, labelFont, base.GetTextForeBrush(), new Rectangle((num7 + num2) + num, num6, ((width - num2) - num) - Resco.Controls.DetailView.DetailView.ArrowImageSize.Width, height), HorizontalAlignment.Right, VerticalAlignment.Top);
                    }
                    else
                    {
                        base.DrawAlignmentString(gr, text, labelFont, base.GetTextForeBrush(), new Rectangle((num7 + left) + 8, num6, (width - left) - 8, height), HorizontalAlignment.Right, VerticalAlignment.Top);
                    }
                }
                else if (splitArrows)
                {
                    gr.DrawString(text, labelFont, base.GetTextForeBrush(), new Rectangle((num7 + 8) + Resco.Controls.DetailView.DetailView.ArrowImageSize.Width, num6, Math.Max(0, (num2 - 8) - Resco.Controls.DetailView.DetailView.ArrowImageSize.Width), height));
                }
                else
                {
                    gr.DrawString(text, labelFont, base.GetTextForeBrush(), new Rectangle(num7 + 8, num6, Math.Max(8, left - 8), height));
                }
            }
        }

        private void DrawDots(Graphics gr, int left, int top, int width, int height)
        {
            Resco.Controls.DetailView.DetailView parent = base.Parent;
            PageCollection pages = parent.Pages;
            bool pagesRightToLeft = this.PagesRightToLeft;
            int currentPageIndex = parent.CurrentPageIndex;
            int pageCount = parent.PageCount;
            int num2 = 0;
            foreach (Page page in pages)
            {
                if (page.PagingItem.Visible)
                {
                    num2++;
                }
            }
            int num3 = height / 2;
            int num4 = height / 4;
            int x = (left + ((width - (num2 * height)) / 2)) + num4;
            int y = top + num4;
            for (int i = 0; i < pageCount; i++)
            {
                int num7 = pagesRightToLeft ? ((pageCount - i) - 1) : i;
                if (pages[num7].PagingItem.Visible)
                {
                    if (parent.CurrentPageIndex == num7)
                    {
                        gr.FillEllipse(base.GetTextForeBrush(), x, y, num3, num3);
                    }
                    else
                    {
                        gr.FillEllipse((base.TextBackColor == Color.Transparent) ? this.m_GrayBrush : base.m_TextBackBrush, x, y, num3, num3);
                    }
                    x += height;
                }
            }
        }

        private void DrawTabStrip(Graphics gr, int left, int top, int width, int height)
        {
            Resco.Controls.DetailView.DetailView parent = base.Parent;
            PageCollection pages = parent.Pages;
            int num = (parent.PagesLocation == RescoPagesLocation.Top) ? height : 0;
            int pageCount = parent.PageCount;
            bool pagesRightToLeft = this.PagesRightToLeft;
            int num3 = left;
            if (parent.PagesLocation == RescoPagesLocation.Top)
            {
                top--;
            }
            left += pagesRightToLeft ? width : 0;
            int num4 = 0;
            for (int i = 0; i < pageCount; i++)
            {
                if (pages[i].PagingItem.Visible)
                {
                    string text = pages[i].Text;
                    ItemPageBreak pagingItem = pages[i].PagingItem;
                    SizeF ef = gr.MeasureString(text, pagingItem.TextFont);
                    pagingItem.m_TextWidth = Convert.ToInt32(Math.Ceiling((double) ef.Width));
                    pagingItem.m_TextHeight = Convert.ToInt32(Math.Ceiling((double) ef.Height));
                    num4 += pagingItem.m_TextWidth + 10;
                    parent.m_PagesOverWidth = (parent.m_StartDrawPage != 0) || (num4 >= width);
                    if (i >= parent.m_StartDrawPage)
                    {
                        int x = left - (pagesRightToLeft ? (pagingItem.m_TextWidth + 10) : 0);
                        if (parent.CurrentPageIndex == i)
                        {
                            if ((base.TextBackColor != Color.Transparent) || !base.Parent.UseGradient)
                            {
                                gr.FillRectangle(base.m_TextBackBrush, x, top, pagingItem.m_TextWidth + 10, height);
                            }
                        }
                        else
                        {
                            gr.FillRectangle(this.m_GrayBrush, x, top, pagingItem.m_TextWidth + 10, height);
                        }
                        if (parent.CurrentPageIndex != i)
                        {
                            gr.DrawLine(this.m_BlackPen, x, top + num, ((x + pagingItem.m_TextWidth) + 10) - 1, top + num);
                        }
                        gr.DrawLine(this.m_BlackPen, x + (pagesRightToLeft ? 0 : ((pagingItem.m_TextWidth + 10) - 1)), top, x + (pagesRightToLeft ? 0 : ((pagingItem.m_TextWidth + 10) - 1)), top + height);
                        gr.DrawString(text, pagingItem.TextFont, pagingItem.GetTextForeBrush(), new Rectangle(x + 5, top + ((height - pagingItem.m_TextHeight) / 2), pagingItem.m_TextWidth, pagingItem.m_TextHeight));
                        if (pagesRightToLeft)
                        {
                            left -= pagingItem.m_TextWidth + 10;
                            if (left < 0)
                            {
                                break;
                            }
                        }
                        else
                        {
                            left += pagingItem.m_TextWidth + 10;
                            if (left > width)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            if (pagesRightToLeft)
            {
                if (left > 0)
                {
                    gr.DrawLine(this.m_BlackPen, num3, top + num, left, top + num);
                }
            }
            else if (left < width)
            {
                gr.DrawLine(this.m_BlackPen, left, top + num, num3 + width, top + num);
            }
            if (parent.m_PagesOverWidth)
            {
                bool isLeftRight = this.ArrowStyle == RescoArrowStyle.LeftRight;
                Image image = Resco.Controls.DetailView.DetailView.GetArrow(true, isLeftRight, parent.m_RightArrowClicked, true);
                Image image2 = Resco.Controls.DetailView.DetailView.GetArrow(false, isLeftRight, parent.m_LeftArrowClicked, true);
                if (isLeftRight)
                {
                    Rectangle destRect = new Rectangle(num3 + (pagesRightToLeft ? 0 : (width - (Resco.Controls.DetailView.DetailView.TabImageSize.Width * 2))), top, Resco.Controls.DetailView.DetailView.TabImageSize.Width, Resco.Controls.DetailView.DetailView.TabImageSize.Height);
                    Rectangle srcRect = new Rectangle(0, 0, image2.Width, image2.Height);
                    gr.DrawImage(image2, destRect, srcRect, GraphicsUnit.Pixel);
                    destRect.X = num3 + (pagesRightToLeft ? Resco.Controls.DetailView.DetailView.TabImageSize.Width : (width - Resco.Controls.DetailView.DetailView.TabImageSize.Width));
                    srcRect = new Rectangle(0, 0, image.Width, image.Height);
                    gr.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                }
                else
                {
                    Rectangle rectangle3 = new Rectangle(num3 + (pagesRightToLeft ? 0 : (width - Resco.Controls.DetailView.DetailView.TabImageSize.Width)), top, Resco.Controls.DetailView.DetailView.TabImageSize.Width, Resco.Controls.DetailView.DetailView.TabImageSize.Height);
                    Rectangle rectangle4 = new Rectangle(0, 0, image2.Width, image2.Height);
                    gr.DrawImage(image2, rectangle3, rectangle4, GraphicsUnit.Pixel);
                    rectangle3.Y = top + Resco.Controls.DetailView.DetailView.TabImageSize.Height;
                    rectangle4 = new Rectangle(0, 0, image.Width, image.Height);
                    gr.DrawImage(image, rectangle3, rectangle4, GraphicsUnit.Pixel);
                }
            }
        }

        protected internal override bool HandleKey(Keys key)
        {
            if (key == Keys.Left)
            {
                Resco.Controls.DetailView.DetailView parent = base.Parent;
                parent.CurrentPageIndex--;
                return true;
            }
            if (key == Keys.Right)
            {
                Resco.Controls.DetailView.DetailView view2 = base.Parent;
                view2.CurrentPageIndex++;
                return true;
            }
            return false;
        }

        protected override void OnClick(int yOffset, int parentWidth, bool useClickVisualize)
        {
            base.OnClick(yOffset, parentWidth, false);
        }

        protected internal override void ScaleItem(float fx, float fy)
        {
        }

        protected virtual bool ShouldSerializePageNumberStyle()
        {
            return (this.m_PageNumberStyle != RescoPageNumberStyle.CurrentOfAll);
        }

        public override string ToString()
        {
            if (!(base.Name == "") && (base.Name != null))
            {
                return base.Name;
            }
            if (base.Site != null)
            {
                return base.Site.Name;
            }
            return "PageBreak";
        }

        private void UpdateStartDrawPage(int index, int dir)
        {
            PageCollection pages = base.Parent.Pages;
            int pageCount = base.Parent.PageCount;
            base.Parent.m_StartDrawPage = index;
            do
            {
                Resco.Controls.DetailView.DetailView parent = base.Parent;
                parent.m_StartDrawPage += dir;
                index += dir;
            }
            while (((index < pageCount) && (index > 0)) && !pages[index].PagingItem.Visible);
        }

        private RescoArrowStyle ArrowStyle
        {
            get
            {
                if (base.Parent != null)
                {
                    return base.Parent.ArrowStyle;
                }
                return RescoArrowStyle.LeftRight;
            }
        }

        [DefaultValue(true), Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public override int Height
        {
            get
            {
                if (base.Parent != null)
                {
                    return base.Parent.PagerHeight;
                }
                return base.Height;
            }
            set
            {
                if (base.Parent != null)
                {
                    base.Parent.PagerHeight = value;
                }
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue(""), Resco.Controls.DetailView.Design.Browsable(false)]
        public override string Label
        {
            get
            {
                return base.Label;
            }
            set
            {
                base.Label = value;
            }
        }

        [DefaultValue(1), Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public override HorizontalAlignment LabelAlignment
        {
            get
            {
                return base.LabelAlignment;
            }
            set
            {
                base.LabelAlignment = value;
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.DetailView.Design.Browsable(false)]
        public VerticalAlignment LineAlign
        {
            get
            {
                return base.LineAlign;
            }
            set
            {
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue(true)]
        public override bool NewLine
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        [DefaultValue(2)]
        public RescoPageNumberStyle PageNumberStyle
        {
            get
            {
                return this.m_PageNumberStyle;
            }
            set
            {
                this.m_PageNumberStyle = value;
            }
        }

        private bool PagesRightToLeft
        {
            get
            {
                return ((base.Parent != null) && base.Parent.PagesRightToLeft);
            }
        }

        private RescoPageStyle PageStyle
        {
            get
            {
                if (base.Parent != null)
                {
                    return base.Parent.PagingStyle;
                }
                return RescoPageStyle.Arrows;
            }
        }

        [DefaultValue(true)]
        public bool ShowPageText
        {
            get
            {
                return this.m_ShowPageText;
            }
            set
            {
                this.m_ShowPageText = value;
            }
        }

        private bool SplitArrows
        {
            get
            {
                return ((base.Parent != null) && base.Parent.SplitArrows);
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.DetailView.Design.Browsable(false), DefaultValue(1)]
        public RescoItemStyle Style
        {
            get
            {
                return base.Style;
            }
            set
            {
                base.Style = value;
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.DetailView.Design.Browsable(false)]
        public HorizontalAlignment TextAlign
        {
            get
            {
                return base.TextAlign;
            }
            set
            {
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), DefaultValue(true), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public override bool Visible
        {
            get
            {
                return this.m_bVisible;
            }
            set
            {
                if (this.m_bVisible != value)
                {
                    this.m_bVisible = value;
                    if (base.Parent != null)
                    {
                        base.Parent.Invalidate();
                    }
                }
            }
        }
    }
}

