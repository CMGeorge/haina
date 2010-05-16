namespace Resco.Controls.CommonControls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class VerticalToolbarRenderer : AbstractToolbarRenderer
    {
        public VerticalToolbarRenderer()
        {
            base.m_DockType = ToolbarDockType.Vertical;
        }

        public VerticalToolbarRenderer(Control aParent) : base(aParent)
        {
            base.m_DockType = ToolbarDockType.Vertical;
        }

        public override Rectangle Draw(Graphics gr, Rectangle aRect, int aScrollBarValue)
        {
            return this.DrawCentered(gr, aRect, aScrollBarValue);
        }

        public Rectangle DrawCentered(Graphics gr, Rectangle aRect, int aScrollBarValue)
        {
            Rectangle rectangle;
            bool aFocused = false;
            Rectangle rectangle2 = new Rectangle(0, 0, aRect.Width, 0);
            Point point = new Point(aRect.Location.X, aRect.Location.Y - aScrollBarValue);
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
                    rectangle = new Rectangle(point.X, point.Y, itemSize.Width, itemSize.Height);
                    item.ClientRectangle = rectangle;
                    point = new Point(rectangle.X, rectangle.Bottom);
                    rectangle2.Height += rectangle.Height;
                }
            }
            int offsetY = 0;
            switch (base.m_Alignment)
            {
                case Alignment.MiddleLeft:
                case Alignment.MiddleCenter:
                case Alignment.MiddleRight:
                    offsetY = (aRect.Height - rectangle2.Height) / 2;
                    break;

                case Alignment.BottomLeft:
                case Alignment.BottomCenter:
                case Alignment.BottomRight:
                    offsetY = aRect.Height - rectangle2.Height;
                    break;
            }
            if (offsetY < 0)
            {
                offsetY = 0;
            }
            for (int j = 0; j < count; j++)
            {
                ToolbarItem anItem = base.m_ToolbarItems[j];
                if (anItem.Visible)
                {
                    rectangle = this.HorizontalAlignItem(anItem.ClientRectangle, offsetY, aRect);
                    anItem.ClientRectangle = rectangle;
                    if (((rectangle.Y <= aRect.Bottom) && (rectangle.Y >= aRect.Y)) || ((rectangle.Bottom >= aRect.Y) && (rectangle.Bottom <= aRect.Bottom)))
                    {
                        base.DrawToolbarItem(gr, rectangle, anItem, aFocused);
                    }
                }
            }
            return rectangle2;
        }

        public Rectangle DrawVerticalToolbar(Graphics gr, Rectangle aRect, int aScrollBarValue)
        {
            bool aFocused = false;
            Rectangle rectangle2 = new Rectangle(0, 0, aRect.Width, 0);
            Point point = new Point(aRect.Location.X, aRect.Location.Y - aScrollBarValue);
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
                    Rectangle rectangle = new Rectangle(point.X, point.Y, itemSize.Width, itemSize.Height);
                    item.ClientRectangle = rectangle;
                    if (((rectangle.Y <= aRect.Bottom) && (rectangle.Y >= aRect.Y)) || ((rectangle.Bottom >= aRect.Y) && (rectangle.Bottom <= aRect.Bottom)))
                    {
                        base.DrawToolbarItem(gr, rectangle, item, aFocused);
                    }
                    point = new Point(rectangle.X, rectangle.Bottom);
                    rectangle2.Height += rectangle.Height;
                }
            }
            return rectangle2;
        }

        private Rectangle HorizontalAlignItem(Rectangle aClientRectangle, int offsetY, Rectangle aCtrlClientRect)
        {
            Rectangle rectangle = aClientRectangle;
            int num = 0;
            switch (base.m_Alignment)
            {
                case Alignment.TopCenter:
                case Alignment.MiddleCenter:
                case Alignment.BottomCenter:
                    num = (aCtrlClientRect.Width - aClientRectangle.Width) / 2;
                    break;

                case Alignment.TopRight:
                case Alignment.MiddleRight:
                case Alignment.BottomRight:
                    num = aCtrlClientRect.Width - aClientRectangle.Width;
                    break;
            }
            rectangle.Location = new Point(rectangle.Location.X + num, rectangle.Location.Y + offsetY);
            return rectangle;
        }
    }
}

