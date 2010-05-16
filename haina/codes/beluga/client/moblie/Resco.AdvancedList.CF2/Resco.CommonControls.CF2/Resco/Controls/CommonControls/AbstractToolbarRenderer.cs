namespace Resco.Controls.CommonControls
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal abstract class AbstractToolbarRenderer
    {
        private Size KSelectionRoundSize;
        protected Resco.Controls.CommonControls.Alignment m_Alignment;
        protected ToolbarDockType m_DockType;
        protected ToolbarItem m_FocusedItem;
        private int m_MarginAtBegin;
        private int m_MarginAtEnd;
        private Control m_Parent;
        protected ToolbarItemCollection m_ToolbarItems;

        public AbstractToolbarRenderer()
        {
            this.m_ToolbarItems = new ToolbarItemCollection();
            this.KSelectionRoundSize = new Size(20, 20);
        }

        public AbstractToolbarRenderer(Control aParent)
        {
            this.m_ToolbarItems = new ToolbarItemCollection();
            this.KSelectionRoundSize = new Size(20, 20);
            this.m_Parent = aParent;
        }

        protected Color BackgroundImageColor(Bitmap bmp)
        {
            return bmp.GetPixel(0, 0);
        }

        protected Color BackgroundImageColor(Image image)
        {
            using (Bitmap bitmap = new Bitmap(image))
            {
                return bitmap.GetPixel(0, 0);
            }
        }

        public abstract Rectangle Draw(Graphics gr, Rectangle aRect, int aScrollBarValue);
        protected void DrawImage(Graphics gr, Image image, Rectangle anImgDestRect, bool anAutoTransparent)
        {
            if (anAutoTransparent)
            {
                ImageAttributes imageAttr = new ImageAttributes();
                Color colorLow = this.BackgroundImageColor(image);
                imageAttr.SetColorKey(colorLow, colorLow);
                gr.DrawImage(image, anImgDestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            }
            else
            {
                Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
                gr.DrawImage(image, anImgDestRect, srcRect, GraphicsUnit.Pixel);
            }
        }

        public static void DrawRoundedRect(Graphics g, Pen p, Color backColor, Rectangle rc, Size size)
        {
            Point[] points = new Point[8];
            points[0].X = rc.Left + (size.Width / 2);
            points[0].Y = rc.Top + 1;
            points[1].X = rc.Right - (size.Width / 2);
            points[1].Y = rc.Top + 1;
            points[2].X = rc.Right;
            points[2].Y = rc.Top + (size.Height / 2);
            points[3].X = rc.Right;
            points[3].Y = rc.Bottom - (size.Height / 2);
            points[4].X = rc.Right - (size.Width / 2);
            points[4].Y = rc.Bottom;
            points[5].X = rc.Left + (size.Width / 2);
            points[5].Y = rc.Bottom;
            points[6].X = rc.Left + 1;
            points[6].Y = rc.Bottom - (size.Height / 2);
            points[7].X = rc.Left + 1;
            points[7].Y = rc.Top + (size.Height / 2);
            Brush brush = new SolidBrush(backColor);
            g.DrawLine(p, rc.Left + (size.Width / 2), rc.Top, rc.Right - (size.Width / 2), rc.Top);
            g.FillEllipse(brush, rc.Right - size.Width, rc.Top, size.Width, size.Height);
            g.DrawEllipse(p, rc.Right - size.Width, rc.Top, size.Width, size.Height);
            g.DrawLine(p, rc.Right, rc.Top + (size.Height / 2), rc.Right, rc.Bottom - (size.Height / 2));
            g.FillEllipse(brush, rc.Right - size.Width, rc.Bottom - size.Height, size.Width, size.Height);
            g.DrawEllipse(p, rc.Right - size.Width, rc.Bottom - size.Height, size.Width, size.Height);
            g.DrawLine(p, rc.Right - (size.Width / 2), rc.Bottom, rc.Left + (size.Width / 2), rc.Bottom);
            g.FillEllipse(brush, rc.Left, rc.Bottom - size.Height, size.Width, size.Height);
            g.DrawEllipse(p, rc.Left, rc.Bottom - size.Height, size.Width, size.Height);
            g.DrawLine(p, rc.Left, rc.Bottom - (size.Height / 2), rc.Left, rc.Top + (size.Height / 2));
            g.FillEllipse(brush, rc.Left, rc.Top, size.Width, size.Height);
            g.DrawEllipse(p, rc.Left, rc.Top, size.Width, size.Height);
            g.FillPolygon(brush, points);
            brush.Dispose();
            brush = null;
        }

        protected void DrawText(Graphics gr, ToolbarItem anItem, Rectangle aClientRect)
        {
            if ((anItem.Text != null) && (anItem.Text.Length != 0))
            {
                int num4;
                int num5;
                SolidBrush brush = new SolidBrush(anItem.ForeColor);
                string[] strArray = anItem.Text.Replace(@"\n", @"\").Split(new char[] { '\\' });
                SizeF ef = new SizeF(0f, 0f);
                float width = 0f;
                float height = 0f;
                for (int i = 0; i < strArray.Length; i++)
                {
                    ef = gr.MeasureString(strArray[i], anItem.Font);
                    if (width < ef.Width)
                    {
                        width = ef.Width;
                    }
                    if (height < ef.Height)
                    {
                        height = ef.Height;
                    }
                }
                SizeF textSize = new SizeF(width, height * strArray.Length);
                this.GetTextLocation(anItem, aClientRect, textSize, out num4, out num5);
                Rectangle layoutRectangle = new Rectangle(aClientRect.X + num4, aClientRect.Y + num5, ((int) textSize.Width) + 1, ((int) textSize.Height) + 1);
                layoutRectangle.Height = (int) height;
                Region region = new Region(aClientRect);
                gr.Clip = region;
                StringFormat stringFormat = this.GetStringFormat(anItem.TextAlignment);
                for (int j = 0; j < strArray.Length; j++)
                {
                    gr.DrawString(strArray[j], anItem.Font, brush, layoutRectangle, stringFormat);
                    layoutRectangle.Y += (int) height;
                }
                gr.ResetClip();
                region.Dispose();
                region = null;
                brush.Dispose();
                brush = null;
            }
        }

        private Rectangle DrawToolbarImage(Graphics gr, ToolbarItem anItem, Image image, Rectangle aRect)
        {
            Rectangle imageDestRect = this.GetImageDestRect(anItem, image);
            imageDestRect.X += aRect.X;
            imageDestRect.Y += aRect.Y;
            this.DrawImage(gr, image, imageDestRect, anItem.AutoTransparent);
            return imageDestRect;
        }

        protected void DrawToolbarItem(Graphics gr, Rectangle aRect, ToolbarItem anItem, bool aFocused)
        {
            if (anItem.BackColor != Color.Transparent)
            {
                Rectangle rect = aRect;
                using (SolidBrush brush = new SolidBrush(anItem.BackColor))
                {
                    gr.FillRectangle(brush, rect);
                }
            }
            if ((anItem.ImagePressed == null) && anItem.Pressed)
            {
                Rectangle rc = aRect;
                rc.Inflate(-1, -1);
                using (Pen pen = new Pen(anItem.FocusedColor))
                {
                    DrawRoundedRect(gr, pen, anItem.FocusedColor, rc, this.KSelectionRoundSize);
                }
            }
            if ((anItem.ImagePressed != null) && anItem.Pressed)
            {
                this.DrawToolbarImage(gr, anItem, anItem.ImagePressed, aRect);
            }
            else if (anItem.ImageDefault != null)
            {
                this.DrawToolbarImage(gr, anItem, anItem.ImageDefault, aRect);
            }
            this.DrawText(gr, anItem, aRect);
        }

        private Rectangle GetImageDestRect(ToolbarItem anItem, Image anImage)
        {
            int num;
            int num2;
            if (anItem.StretchImage)
            {
                return new Rectangle(0, 0, anItem.ClientRectangle.Width, anItem.ClientRectangle.Height);
            }
            Size anImgSize = anImage.Size;
            if (anItem.ItemSizeType != ToolbarItemSizeType.ByImageWithoutScaling)
            {
                anImgSize.Height = (int) (anImgSize.Height * ToolbarControl.m_ScaleFactor.Height);
                anImgSize.Width = (int) (anImgSize.Width * ToolbarControl.m_ScaleFactor.Width);
            }
            this.GetImageLocation(anItem.ImageAlignment, anItem.ClientRectangle, anImgSize, out num, out num2);
            return new Rectangle(num, num2, anImgSize.Width, anImgSize.Height);
        }

        protected void GetImageLocation(Resco.Controls.CommonControls.Alignment anAlignment, Rectangle aClientRect, Size anImgSize, out int imageLeft, out int imageTop)
        {
            this.GetLocationByAlignment(anAlignment, aClientRect, new SizeF((float) anImgSize.Width, (float) anImgSize.Height), out imageLeft, out imageTop);
        }

        protected Size GetItemSize(ToolbarItem anItem)
        {
            Size empty = Size.Empty;
            if ((anItem.ImagePressed != null) && anItem.Pressed)
            {
                return anItem.ImagePressed.Size;
            }
            if (anItem.ImageDefault != null)
            {
                empty = anItem.ImageDefault.Size;
            }
            return empty;
        }

        protected void GetLocationByAlignment(Resco.Controls.CommonControls.Alignment anAlignment, Rectangle aClientRect, SizeF aControlSize, out int aTextLeft, out int aTextTop)
        {
            int num = 2;
            int num2 = 2;
            switch (anAlignment)
            {
                case Resco.Controls.CommonControls.Alignment.TopLeft:
                    aTextTop = 1;
                    aTextLeft = 1;
                    return;

                case Resco.Controls.CommonControls.Alignment.TopCenter:
                    aTextTop = 1;
                    aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) / 2;
                    return;

                case Resco.Controls.CommonControls.Alignment.TopRight:
                    aTextTop = 1;
                    aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) - num;
                    return;

                case Resco.Controls.CommonControls.Alignment.MiddleLeft:
                    aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) / 2;
                    aTextLeft = 1;
                    return;

                case Resco.Controls.CommonControls.Alignment.MiddleCenter:
                    aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) / 2;
                    aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) / 2;
                    return;

                case Resco.Controls.CommonControls.Alignment.MiddleRight:
                    aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) / 2;
                    aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) - num;
                    return;

                case Resco.Controls.CommonControls.Alignment.BottomLeft:
                    aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) - num2;
                    aTextLeft = 1;
                    return;

                case Resco.Controls.CommonControls.Alignment.BottomCenter:
                    aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) - num2;
                    aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) / 2;
                    return;

                case Resco.Controls.CommonControls.Alignment.BottomRight:
                    aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) - 2;
                    aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) - num;
                    return;
            }
            aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) / 2;
            aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) / 2;
        }

        private StringFormat GetStringFormat(Resco.Controls.CommonControls.Alignment anAlignment)
        {
            StringFormat format = new StringFormat();
            switch (anAlignment)
            {
                case Resco.Controls.CommonControls.Alignment.TopLeft:
                    format.LineAlignment = StringAlignment.Near;
                    format.Alignment = StringAlignment.Near;
                    return format;

                case Resco.Controls.CommonControls.Alignment.TopCenter:
                    format.LineAlignment = StringAlignment.Near;
                    format.Alignment = StringAlignment.Center;
                    return format;

                case Resco.Controls.CommonControls.Alignment.TopRight:
                    format.LineAlignment = StringAlignment.Near;
                    format.Alignment = StringAlignment.Far;
                    return format;

                case Resco.Controls.CommonControls.Alignment.MiddleLeft:
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Near;
                    return format;

                case Resco.Controls.CommonControls.Alignment.MiddleCenter:
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Center;
                    return format;

                case Resco.Controls.CommonControls.Alignment.MiddleRight:
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Far;
                    return format;

                case Resco.Controls.CommonControls.Alignment.BottomLeft:
                    format.LineAlignment = StringAlignment.Far;
                    format.Alignment = StringAlignment.Near;
                    return format;

                case Resco.Controls.CommonControls.Alignment.BottomCenter:
                    format.LineAlignment = StringAlignment.Far;
                    format.Alignment = StringAlignment.Center;
                    return format;

                case Resco.Controls.CommonControls.Alignment.BottomRight:
                    format.LineAlignment = StringAlignment.Far;
                    format.Alignment = StringAlignment.Far;
                    return format;
            }
            return format;
        }

        protected void GetTextLocation(ToolbarItem anItem, Rectangle aClientRect, SizeF textSize, out int aTextLeft, out int aTextTop)
        {
            this.GetLocationByAlignment(anItem.TextAlignment, aClientRect, textSize, out aTextLeft, out aTextTop);
        }

        private bool IsDesignMode()
        {
            return ((this.m_Parent.Site != null) && this.m_Parent.Site.DesignMode);
        }

        public Resco.Controls.CommonControls.Alignment Alignment
        {
            get
            {
                return this.m_Alignment;
            }
            set
            {
                this.m_Alignment = value;
            }
        }

        protected int Height
        {
            get
            {
                return this.m_Parent.Height;
            }
        }

        public int MarginAtBegin
        {
            get
            {
                return this.m_MarginAtBegin;
            }
            set
            {
                this.m_MarginAtBegin = value;
            }
        }

        public int MarginAtEnd
        {
            get
            {
                return this.m_MarginAtEnd;
            }
            set
            {
                this.m_MarginAtEnd = value;
            }
        }

        public Control Parent
        {
            get
            {
                return this.m_Parent;
            }
            set
            {
                this.m_Parent = value;
            }
        }

        public ToolbarItemCollection ToolbarItems
        {
            get
            {
                return this.m_ToolbarItems;
            }
            set
            {
                this.m_ToolbarItems = value;
            }
        }

        protected int Width
        {
            get
            {
                return this.m_Parent.Width;
            }
        }
    }
}

