namespace Resco.Controls.CommonControls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class HorizontalToolbarRenderer : AbstractToolbarRenderer
    {
        public HorizontalToolbarRenderer()
        {
            base.m_DockType = ToolbarDockType.Horizontal;
        }

        public HorizontalToolbarRenderer(Control aParent) : base(aParent)
        {
            base.m_DockType = ToolbarDockType.Horizontal;
        }

        public override Rectangle Draw(Graphics gr, Rectangle aRect, int aScrollBarValue)
        {
            return this.DrawCentered(gr, aRect, aScrollBarValue);
        }

        public Rectangle DrawCentered(Graphics gr, Rectangle aRect, int aScrollBarValue)
        {
            Rectangle rectangle;
            bool aFocused = false;
            Rectangle rectangle2 = new Rectangle(0, 0, 0, aRect.Height);
            Point point = new Point(aRect.Location.X - aScrollBarValue, aRect.Location.Y);
            int count = base.m_ToolbarItems.Count;
            for (int i = 0; i < count; i++)
            {
                ToolbarItem anItem = base.m_ToolbarItems[i];
                if (anItem.Visible)
                {
                    Size itemSize;
                    if (anItem.ItemSizeType == ToolbarItemSizeType.ByImage)
                    {
                        itemSize = base.GetItemSize(anItem);
                        itemSize = new Size((int) (itemSize.Width * ToolbarControl.m_ScaleFactor.Width), (int) (itemSize.Height * ToolbarControl.m_ScaleFactor.Height));
                    }
                    else if (anItem.ItemSizeType == ToolbarItemSizeType.ByImageWithoutScaling)
                    {
                        itemSize = base.GetItemSize(anItem);
                    }
                    else
                    {
                        itemSize = anItem.CustomSize;
                        itemSize = new Size((int) (itemSize.Width * ToolbarControl.m_ScaleFactor.Width), (int) (itemSize.Height * ToolbarControl.m_ScaleFactor.Height));
                    }
                    rectangle = new Rectangle(point.X, point.Y, itemSize.Width, itemSize.Height);
                    anItem.ClientRectangle = rectangle;
                    point = new Point(rectangle.Right, rectangle.Y);
                    rectangle2.Width += rectangle.Width;
                }
            }
            int offsetX = 0;
            switch (base.m_Alignment)
            {
                case Alignment.TopCenter:
                case Alignment.MiddleCenter:
                case Alignment.BottomCenter:
                    offsetX = (aRect.Width - rectangle2.Width) / 2;
                    if (rectangle2.Width > base.Width)
                    {
                        offsetX += base.MarginAtBegin;
                    }
                    break;

                case Alignment.TopRight:
                case Alignment.MiddleRight:
                case Alignment.BottomRight:
                    offsetX = aRect.Width - rectangle2.Width;
                    break;
            }
            if (offsetX < 0)
            {
                offsetX = 0;
            }
            for (int j = 0; j < count; j++)
            {
                ToolbarItem item2 = base.m_ToolbarItems[j];
                if (item2.Visible)
                {
                    rectangle = this.VerticalAlignItem(item2.ClientRectangle, offsetX, aRect);
                    item2.ClientRectangle = rectangle;
                    if (((rectangle.X <= aRect.Right) && (rectangle.X >= aRect.X)) || ((rectangle.Right >= aRect.X) && (rectangle.Right <= aRect.Right)))
                    {
                        base.DrawToolbarItem(gr, rectangle, item2, aFocused);
                    }
                }
            }
            return rectangle2;
        }

        public Rectangle DrawHorizontalToolbar(Graphics gr, Rectangle aRect, int aScrollBarValue)
        {
            bool aFocused = false;
            Rectangle rectangle2 = new Rectangle(0, 0, 0, aRect.Height);
            int count = base.m_ToolbarItems.Count;
            for (int i = 0; i < count; i++)
            {
                ToolbarItem item = base.m_ToolbarItems[i];
                if (item.Visible)
                {
                    Size itemSize;
                    if (base.m_FocusedItem != null)
                    {
                        aFocused = base.m_FocusedItem.Equals(item);
                    }
                    if (item.ItemSizeType == ToolbarItemSizeType.ByImage)
                    {
                        itemSize = base.GetItemSize(item);
                        itemSize = new Size((int) (itemSize.Width * ToolbarControl.m_ScaleFactor.Width), (int) (itemSize.Height * ToolbarControl.m_ScaleFactor.Height));
                    }
                    else if (item.ItemSizeType == ToolbarItemSizeType.ByImageWithoutScaling)
                    {
                        itemSize = base.GetItemSize(item);
                    }
                    else
                    {
                        itemSize = item.CustomSize;
                        itemSize = new Size((int) (itemSize.Width * ToolbarControl.m_ScaleFactor.Width), (int) (itemSize.Height * ToolbarControl.m_ScaleFactor.Height));
                    }
                    if (itemSize.Width > base.Width)
                    {
                        aScrollBarValue -= base.MarginAtBegin;
                    }
                    Point point = new Point(aRect.Location.X - aScrollBarValue, aRect.Location.Y);
                    Rectangle rectangle = new Rectangle(point.X, point.Y, itemSize.Width, itemSize.Height);
                    item.ClientRectangle = rectangle;
                    if (((rectangle.X <= aRect.Right) && (rectangle.X >= aRect.X)) || ((rectangle.Right >= aRect.X) && (rectangle.Right <= aRect.Right)))
                    {
                        base.DrawToolbarItem(gr, rectangle, item, aFocused);
                    }
                    point = new Point(rectangle.Right, rectangle.Y);
                    rectangle2.Width += rectangle.Width;
                }
            }
            return rectangle2;
        }

        private Rectangle VerticalAlignItem(Rectangle aClientRectangle, int offsetX, Rectangle aCtrlClientRect)
        {
            Rectangle rectangle = aClientRectangle;
            int num = 0;
            switch (base.m_Alignment)
            {
                case Alignment.MiddleLeft:
                case Alignment.MiddleCenter:
                case Alignment.MiddleRight:
                    num = (aCtrlClientRect.Height - aClientRectangle.Height) / 2;
                    break;

                case Alignment.BottomLeft:
                case Alignment.BottomCenter:
                case Alignment.BottomRight:
                    num = aCtrlClientRect.Height - aClientRectangle.Height;
                    break;
            }
            rectangle.Location = new Point(rectangle.Location.X + offsetX, rectangle.Location.Y + num);
            return rectangle;
        }
    }
}

