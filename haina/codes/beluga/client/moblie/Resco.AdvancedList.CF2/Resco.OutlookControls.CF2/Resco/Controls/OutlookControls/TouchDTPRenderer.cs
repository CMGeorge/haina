namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class TouchDTPRenderer
    {
        private int KMargin;
        private int KRowHeight;
        internal float KSeparatorPenWidth;
        private static Color KWindowFrameColor = Color.FromArgb(0x1f, 0x25, 0x2c);
        private Size KWindowFrameRoundSize;
        private Point[] m_arrowPoints;
        protected Bitmap m_BmpCusror;
        protected Bitmap m_BmpShadowMask;
        private Bitmap m_BmpSkinBuffer;
        protected Bitmap m_BmpWindowFrame;
        private Color m_ColorActualItemForeColor;
        private Color m_ColorCalendarFrameColor;
        private Color m_ColorDefaultText;
        private Color m_ColorInvertedText;
        private Point m_CursorLocation;
        private Color m_CursorTransparentKeyColor;
        private Color m_DayInfoBackColor;
        private Color m_DayInfoForeColor;
        private int m_DayInfoHeight;
        private string m_DayInfoText;
        private Font m_DayInfoTextFont;
        private Control m_Parent;
        internal Color m_ParentTransparentKeyColor;
        private Rectangle m_RectCursor;
        protected RollerItemCollection m_RollerItems;
        private bool m_ShowDayInfo;

        public TouchDTPRenderer()
        {
            this.m_RollerItems = new RollerItemCollection();
            this.KWindowFrameRoundSize = new Size(20, 20);
            this.m_ColorDefaultText = Color.FromArgb(0x33, 0x33, 0x33);
            this.m_ColorInvertedText = Color.White;
            this.m_ColorCalendarFrameColor = KWindowFrameColor;
            this.m_ColorActualItemForeColor = Color.FromArgb(0x21, 0x58, 0xff);
            this.m_CursorTransparentKeyColor = Color.Magenta;
            this.m_ParentTransparentKeyColor = Color.Pink;
            this.KMargin = 7;
            this.KSeparatorPenWidth = 1f;
            this.m_CursorLocation = new Point(0, 0);
            this.m_ShowDayInfo = true;
            this.m_DayInfoText = "Monday";
            this.m_DayInfoTextFont = new Font("Tahoma", 11f, FontStyle.Bold);
            this.m_DayInfoForeColor = Color.White;
            this.m_DayInfoHeight = 30;
            this.m_DayInfoBackColor = this.m_ColorCalendarFrameColor;
        }

        public TouchDTPRenderer(Control aParent)
        {
            this.m_RollerItems = new RollerItemCollection();
            this.KWindowFrameRoundSize = new Size(20, 20);
            this.m_ColorDefaultText = Color.FromArgb(0x33, 0x33, 0x33);
            this.m_ColorInvertedText = Color.White;
            this.m_ColorCalendarFrameColor = KWindowFrameColor;
            this.m_ColorActualItemForeColor = Color.FromArgb(0x21, 0x58, 0xff);
            this.m_CursorTransparentKeyColor = Color.Magenta;
            this.m_ParentTransparentKeyColor = Color.Pink;
            this.KMargin = 7;
            this.KSeparatorPenWidth = 1f;
            this.m_CursorLocation = new Point(0, 0);
            this.m_ShowDayInfo = true;
            this.m_DayInfoText = "Monday";
            this.m_DayInfoTextFont = new Font("Tahoma", 11f, FontStyle.Bold);
            this.m_DayInfoForeColor = Color.White;
            this.m_DayInfoHeight = 30;
            this.m_Parent = aParent;
        }

        private void CreateArrowPoints(Rectangle aRect)
        {
            if (this.m_arrowPoints == null)
            {
                this.m_arrowPoints = new Point[3];
                if (Const.DropDownWidth <= 13)
                {
                    Const.DropArrowSize = new Size(7, 4);
                }
                this.m_arrowPoints[0].X = aRect.Width - (((int) Roller.m_ScaleFactor.Width) * (Const.DropArrowSize.Width + this.KMargin));
                this.m_arrowPoints[0].Y = (aRect.Height + (((int) Roller.m_ScaleFactor.Height) * (Const.DropArrowSize.Height - 1))) / 2;
                this.m_arrowPoints[1].X = (this.m_arrowPoints[0].X + (((int) Roller.m_ScaleFactor.Width) * Const.DropArrowSize.Width)) + ((((int) Roller.m_ScaleFactor.Width) == 2) ? 1 : 0);
                this.m_arrowPoints[1].Y = this.m_arrowPoints[0].Y;
                this.m_arrowPoints[2].X = this.m_arrowPoints[0].X + ((((int) Roller.m_ScaleFactor.Width) * Const.DropArrowSize.Width) / 2);
                this.m_arrowPoints[2].Y = this.m_arrowPoints[0].Y - (((int) Roller.m_ScaleFactor.Height) * Const.DropArrowSize.Height);
            }
        }

        public void DisposePreparedBitmaps()
        {
            if (this.m_BmpShadowMask != null)
            {
                this.m_BmpShadowMask.Dispose();
                this.m_BmpShadowMask = null;
            }
            if (this.m_BmpCusror != null)
            {
                this.m_BmpCusror.Dispose();
                this.m_BmpCusror = null;
            }
            if (this.m_BmpWindowFrame != null)
            {
                this.m_BmpWindowFrame.Dispose();
                this.m_BmpWindowFrame = null;
            }
        }

        public void Draw(Graphics gr, Rectangle aClientRect)
        {
            this.QuickDraw(gr, aClientRect);
        }

        private Rectangle DrawCursor(Graphics gr, Rectangle aRect)
        {
            this.m_CursorLocation = new Point(aRect.X, aRect.Y + ((aRect.Height - this.KRowHeight) / 2));
            this.InitDrawCursor(gr, aRect, this.m_CursorLocation);
            return new Rectangle(this.m_CursorLocation.X, this.m_CursorLocation.Y, aRect.Width, this.KRowHeight);
        }

        private int DrawDayInfo(Graphics gr)
        {
            if (!this.m_ShowDayInfo)
            {
                return 0;
            }
            Rectangle aRect = new Rectangle(0, 0, this.Width, this.DayInfoHeight);
            Rectangle layoutRectangle = aRect;
            layoutRectangle.Inflate(this.KWindowFrameRoundSize.Width / 2, 0);
            using (SolidBrush brush = new SolidBrush(this.m_DayInfoForeColor))
            {
                if (!string.Empty.Equals(this.m_DayInfoText))
                {
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    gr.DrawString(this.m_DayInfoText, this.m_DayInfoTextFont, brush, layoutRectangle, format);
                }
            }
            this.DrawTriangle(gr, aRect);
            return aRect.Height;
        }

        private int DrawDayInfoText(Graphics gr)
        {
            if (!this.m_ShowDayInfo)
            {
                return 0;
            }
            Rectangle rectangle = new Rectangle(0, 0, this.Width, this.DayInfoHeight);
            Rectangle layoutRectangle = rectangle;
            layoutRectangle.Inflate(this.KWindowFrameRoundSize.Width / 2, 0);
            using (SolidBrush brush = new SolidBrush(this.m_DayInfoForeColor))
            {
                if (!string.Empty.Equals(this.m_DayInfoText))
                {
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    gr.DrawString(this.m_DayInfoText, this.m_DayInfoTextFont, brush, layoutRectangle, format);
                }
            }
            return rectangle.Height;
        }

        private int DrawDayInfoTriangle(Graphics gr)
        {
            if (!this.m_ShowDayInfo)
            {
                return 0;
            }
            Rectangle aRect = new Rectangle(0, 0, this.Width, this.DayInfoHeight);
            this.DrawTriangle(gr, aRect);
            return aRect.Height;
        }

        protected void DrawImage(Graphics gr, Image image, Rectangle anImgDestRect, bool anAutoTransparent)
        {
            if (anAutoTransparent)
            {
                ImageAttributes imageAttr = new ImageAttributes();
                Color magenta = Color.Magenta;
                imageAttr.SetColorKey(magenta, magenta);
                gr.DrawImage(image, anImgDestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            }
            else
            {
                Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
                gr.DrawImage(image, anImgDestRect, srcRect, GraphicsUnit.Pixel);
            }
        }

        private void DrawLinesBackground(Graphics gr, Rectangle rect, int i)
        {
            Color color = ((i % 2) == 1) ? Color.LightBlue : Color.LightGray;
            using (SolidBrush brush = new SolidBrush(color))
            {
                gr.FillRectangle(brush, rect);
            }
        }

        private void DrawRoller(Graphics gr, Roller roller, Color aColorDefaultText)
        {
            List<DateTimePickerRow> textFromTo = roller.GetTextFromTo(roller.SelectedIndex - 2, 6);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            this.KRowHeight = roller.RowHeight;
            int num = roller.ScrollbarOffset % this.KRowHeight;
            if (num > roller.RowHeight)
            {
                num = 0;
            }
            using (SolidBrush brush2 = new SolidBrush(aColorDefaultText))
            {
                using (SolidBrush brush3 = new SolidBrush(Color.Gray))
                {
                    using (SolidBrush brush4 = new SolidBrush(this.m_ColorActualItemForeColor))
                    {
                        for (int i = 0; i < textFromTo.Count; i++)
                        {
                            SolidBrush brush;
                            string text = textFromTo[i].Text;
                            Rectangle layoutRectangle = new Rectangle(roller.ClientRectangle.X, ((roller.ClientRectangle.Y + roller.TextOffsetY) + (i * this.KRowHeight)) - num, roller.Width, this.KRowHeight);
                            if (textFromTo[i].Enabled)
                            {
                                if (textFromTo[i].IsDefault)
                                {
                                    brush = brush4;
                                }
                                else
                                {
                                    brush = brush2;
                                }
                            }
                            else
                            {
                                brush = brush3;
                            }
                            if (!string.Empty.Equals(text))
                            {
                                gr.DrawString(text, roller.TextFont, brush, layoutRectangle, format);
                            }
                        }
                    }
                }
            }
        }

        private Rectangle DrawRollers(Graphics gr, int anOffsetY)
        {
            return this.DrawRollers(gr, anOffsetY, true);
        }

        private Rectangle DrawRollers(Graphics gr, int anOffsetY, bool anEnableTextDrawing)
        {
            if ((this.m_RollerItems == null) || (this.m_RollerItems.Count == 0))
            {
                return Rectangle.Empty;
            }
            Rectangle clientRectangle = this.m_RollerItems[0].ClientRectangle;
            clientRectangle.X--;
            clientRectangle.Width = 0;
            for (int i = 0; i < this.m_RollerItems.Count; i++)
            {
                clientRectangle.Width += this.m_RollerItems[i].ClientRectangle.Width + ((int) this.KSeparatorPenWidth);
            }
            this.DrawShadowMask(gr, clientRectangle);
            if (anEnableTextDrawing)
            {
                using (Region region = new Region(clientRectangle))
                {
                    Region clip = gr.Clip;
                    gr.Clip = region;
                    this.DrawRollersText(gr, this.m_ColorDefaultText);
                    gr.ResetClip();
                    gr.Clip = clip;
                }
            }
            return clientRectangle;
        }

        private void DrawRollersText(Graphics gr, Color aColorDefaultText)
        {
            if ((this.m_RollerItems != null) && (this.m_RollerItems.Count != 0))
            {
                for (int i = 0; i < this.m_RollerItems.Count; i++)
                {
                    Roller roller = this.m_RollerItems[i];
                    this.DrawRoller(gr, roller, aColorDefaultText);
                }
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
        }

        private void DrawShadowMask(Graphics gr, Rectangle aRect)
        {
            this.InitDrawShadowMask(gr, aRect, aRect.Location);
        }

        protected void DrawText(Graphics gr, Roller anItem, Rectangle aClientRect)
        {
        }

        private void DrawTriangle(Graphics gr, Rectangle aRect)
        {
            this.CreateArrowPoints(aRect);
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                gr.FillPolygon(brush, this.m_arrowPoints);
            }
        }

        private void DrawVertical3DLines(Graphics gr, Roller roller)
        {
            float width = 2f;
            int num2 = roller.ClientRectangle.X + ((int) (width / 2f));
            int y = roller.ClientRectangle.Y;
            int num4 = num2;
            int bottom = roller.ClientRectangle.Bottom;
            using (Pen pen = new Pen(Color.Black, width))
            {
                gr.DrawLine(pen, num2, y, num4, bottom);
                num2 = roller.ClientRectangle.Right - ((int) (width / 2f));
                num4 = num2;
                gr.DrawLine(pen, num2, y, num4, bottom);
            }
        }

        private void DrawWindowFrame(Graphics gr, Rectangle aRect)
        {
            if (this.m_BmpWindowFrame == null)
            {
                this.InitWindowFrame(aRect);
            }
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorKey(Color.Magenta, Color.Magenta);
            Rectangle destRect = new Rectangle(0, aRect.Y, this.m_BmpWindowFrame.Width, this.m_BmpWindowFrame.Height);
            gr.DrawImage(this.m_BmpWindowFrame, destRect, 0, 0, this.m_BmpWindowFrame.Width, this.m_BmpWindowFrame.Height, GraphicsUnit.Pixel, imageAttr);
        }

        protected void GetLocationByAlignment(Alignment anAlignment, Rectangle aClientRect, SizeF aControlSize, out int aTextLeft, out int aTextTop)
        {
            int num = 2;
            int num2 = 2;
            switch (anAlignment)
            {
                case Alignment.TopLeft:
                    aTextTop = 1;
                    aTextLeft = 1;
                    return;

                case Alignment.TopCenter:
                    aTextTop = 1;
                    aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) / 2;
                    return;

                case Alignment.TopRight:
                    aTextTop = 1;
                    aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) - num;
                    return;

                case Alignment.MiddleLeft:
                    aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) / 2;
                    aTextLeft = 1;
                    return;

                case Alignment.MiddleCenter:
                    aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) / 2;
                    aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) / 2;
                    return;

                case Alignment.MiddleRight:
                    aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) / 2;
                    aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) - num;
                    return;

                case Alignment.BottomLeft:
                    aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) - num2;
                    aTextLeft = 1;
                    return;

                case Alignment.BottomCenter:
                    aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) - num2;
                    aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) / 2;
                    return;

                case Alignment.BottomRight:
                    aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) - 2;
                    aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) - num;
                    return;
            }
            aTextLeft = (aClientRect.Width - ((int) aControlSize.Width)) / 2;
            aTextTop = (aClientRect.Height - ((int) aControlSize.Height)) / 2;
        }

        private void InitCusror(Rectangle aRect)
        {
            if (this.m_BmpCusror != null)
            {
                this.m_BmpCusror.Dispose();
                this.m_BmpCusror = null;
            }
            this.m_BmpCusror = new Bitmap(aRect.Width, this.KRowHeight);
            using (Graphics graphics = Graphics.FromImage(this.m_BmpCusror))
            {
                graphics.Clear(this.m_CursorTransparentKeyColor);
                this.InitDrawCursor(graphics, aRect, new Point(0, 0));
            }
        }

        private void InitDrawCursor(Graphics gr, Rectangle aRect, Point anOffset)
        {
            Rectangle rectangle = new Rectangle(this.m_CursorLocation.X, this.m_CursorLocation.Y, aRect.Width, this.KRowHeight);
            using (Pen pen = new Pen(Color.Gray))
            {
                for (int i = 0; i < this.m_RollerItems.Count; i++)
                {
                    Rectangle clientRectangle = this.m_RollerItems[i].ClientRectangle;
                    clientRectangle.Inflate(-4, -4);
                    clientRectangle.Y = anOffset.Y;
                    clientRectangle.Height = rectangle.Height;
                    DrawRoundedRect(gr, pen, this.m_ColorCalendarFrameColor, clientRectangle, new Size(15, 0x12));
                }
            }
        }

        private void InitDrawShadowMask(Graphics gr, Rectangle aRect, Point anOffset)
        {
            GradientColor color = new GradientColor();
            color.FillDirection = FillDirection.Vertical;
            color.StartColor = Color.FromArgb(0x9c, 0x9c, 0x9c);
            color.EndColor = Color.White;
            int height = aRect.Height / 3;
            Rectangle rc = new Rectangle(anOffset.X, anOffset.Y, aRect.Width, height);
            Rectangle rectangle2 = new Rectangle(anOffset.X, (anOffset.Y + aRect.Height) - height, aRect.Width, height);
            Rectangle rect = new Rectangle(anOffset.X, rc.Bottom, aRect.Width, rectangle2.Top - rc.Bottom);
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                gr.FillRectangle(brush, rect);
            }
            color.DrawGradient(gr, rc);
            color.StartColor = Color.White;
            color.EndColor = Color.FromArgb(0x9c, 0x9c, 0x9c);
            color.DrawGradient(gr, rectangle2);
        }

        private void InitShadowMask(Rectangle aRect)
        {
            if (this.m_BmpShadowMask != null)
            {
                this.m_BmpShadowMask.Dispose();
                this.m_BmpShadowMask = null;
            }
            this.m_BmpShadowMask = new Bitmap(aRect.Width, aRect.Height);
            using (Graphics graphics = Graphics.FromImage(this.m_BmpShadowMask))
            {
                this.InitDrawShadowMask(graphics, aRect, new Point(0, 0));
            }
        }

        private void InitWindowFrame(Rectangle aRectEmpty)
        {
            if (this.m_BmpWindowFrame != null)
            {
                this.m_BmpWindowFrame.Dispose();
                this.m_BmpWindowFrame = null;
            }
            Rectangle rc = aRectEmpty;
            rc.X = 0;
            rc.Y = 0;
            rc.Width = this.m_Parent.ClientRectangle.Width;
            this.m_BmpWindowFrame = new Bitmap(rc.Width, rc.Height);
            using (Graphics graphics = Graphics.FromImage(this.m_BmpWindowFrame))
            {
                if (this.m_ShowDayInfo)
                {
                    graphics.Clear(this.m_ColorCalendarFrameColor);
                }
                else
                {
                    graphics.Clear(Color.Magenta);
                }
                using (new SolidBrush(Color.Magenta))
                {
                    if (!this.m_ShowDayInfo)
                    {
                        using (Pen pen = new Pen(this.m_ColorCalendarFrameColor))
                        {
                            DrawRoundedRect(graphics, pen, this.m_ColorCalendarFrameColor, rc, this.KWindowFrameRoundSize);
                        }
                    }
                    using (Pen pen2 = new Pen(Color.Magenta))
                    {
                        for (int i = 0; i < this.m_RollerItems.Count; i++)
                        {
                            Rectangle clientRectangle = this.m_RollerItems[i].ClientRectangle;
                            clientRectangle.Y = 0;
                            clientRectangle.Inflate(-2, (int) (-this.KMargin * Roller.m_ScaleFactor.Height));
                            DrawRoundedRect(graphics, pen2, Color.Magenta, clientRectangle, new Size(10, 10));
                        }
                    }
                }
            }
        }

        private bool IsDesignMode()
        {
            return ((this.m_Parent.Site != null) && this.m_Parent.Site.DesignMode);
        }

        private void PrepareSkin(Rectangle aClientRect)
        {
            if (this.m_BmpSkinBuffer == null)
            {
                if (this.m_BmpSkinBuffer != null)
                {
                    this.m_BmpSkinBuffer.Dispose();
                    this.m_BmpSkinBuffer = null;
                }
                this.m_BmpSkinBuffer = new Bitmap(aClientRect.Width, aClientRect.Height);
                using (Graphics graphics = Graphics.FromImage(this.m_BmpSkinBuffer))
                {
                    graphics.Clear(this.m_ParentTransparentKeyColor);
                    if (this.m_ShowDayInfo)
                    {
                        using (Pen pen = new Pen(this.m_ColorCalendarFrameColor))
                        {
                            DrawRoundedRect(graphics, pen, this.m_ColorCalendarFrameColor, aClientRect, this.KWindowFrameRoundSize);
                        }
                    }
                    int anOffsetY = this.DrawDayInfoTriangle(graphics);
                    Rectangle rectangle = this.DrawRollers(graphics, anOffsetY, false);
                    if (!Rectangle.Empty.Equals(rectangle))
                    {
                        this.DrawWindowFrame(graphics, rectangle);
                        this.KRowHeight = this.m_RollerItems[0].RowHeight;
                        this.m_RectCursor = this.DrawCursor(graphics, rectangle);
                    }
                }
            }
        }

        private void QuickDraw(Graphics gr, Rectangle aClientRect)
        {
            this.PrepareSkin(aClientRect);
            gr.DrawImage(this.m_BmpSkinBuffer, 0, 0);
            this.DrawDayInfoText(gr);
            Rectangle clientRectangle = this.m_RollerItems[0].ClientRectangle;
            clientRectangle.Inflate(-2, (int) (-this.KMargin * Roller.m_ScaleFactor.Height));
            clientRectangle.Width = 0;
            for (int i = 0; i < this.m_RollerItems.Count; i++)
            {
                clientRectangle.Width += this.m_RollerItems[i].ClientRectangle.Width + ((int) this.KSeparatorPenWidth);
            }
            if (!Rectangle.Empty.Equals(this.m_RectCursor))
            {
                using (Region region = new Region(this.m_RectCursor))
                {
                    Region clip = gr.Clip;
                    gr.Clip = region;
                    this.DrawRollersText(gr, this.m_ColorInvertedText);
                    gr.ResetClip();
                    using (Region region3 = new Region(clientRectangle))
                    {
                        region3.Xor(region);
                        gr.Clip = region3;
                        this.DrawRollersText(gr, this.m_ColorDefaultText);
                        gr.ResetClip();
                    }
                    gr.Clip = clip;
                }
            }
        }

        public Color ColorActualItemForeColor
        {
            get
            {
                return this.m_ColorActualItemForeColor;
            }
            set
            {
                this.m_ColorActualItemForeColor = value;
            }
        }

        public Color ColorCalendarFrameColor
        {
            get
            {
                return this.m_ColorCalendarFrameColor;
            }
            set
            {
                this.m_ColorCalendarFrameColor = value;
                this.m_DayInfoBackColor = this.m_ColorCalendarFrameColor;
            }
        }

        public Color ColorDefaultText
        {
            get
            {
                return this.m_ColorDefaultText;
            }
            set
            {
                this.m_ColorDefaultText = value;
            }
        }

        public Color ColorInvertedText
        {
            get
            {
                return this.m_ColorInvertedText;
            }
            set
            {
                this.m_ColorInvertedText = value;
            }
        }

        public static Color ColorWindowFrame
        {
            get
            {
                return KWindowFrameColor;
            }
            set
            {
                KWindowFrameColor = value;
            }
        }

        public int DayInfoHeight
        {
            get
            {
                return (int) (this.m_DayInfoHeight * Roller.m_ScaleFactor.Height);
            }
            set
            {
                this.m_DayInfoHeight = (int) (((float) value) / Roller.m_ScaleFactor.Height);
            }
        }

        public string DayInfoText
        {
            get
            {
                return this.m_DayInfoText;
            }
            set
            {
                this.m_DayInfoText = value;
            }
        }

        private int Height
        {
            get
            {
                return this.m_Parent.Height;
            }
        }

        public int Margin
        {
            get
            {
                return this.KMargin;
            }
            set
            {
                this.KMargin = value;
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

        public RollerItemCollection RollerItems
        {
            get
            {
                return this.m_RollerItems;
            }
            set
            {
                this.m_RollerItems = value;
            }
        }

        public bool ShowDayInfo
        {
            get
            {
                return this.m_ShowDayInfo;
            }
            set
            {
                this.m_ShowDayInfo = value;
            }
        }

        private int Width
        {
            get
            {
                return this.m_Parent.Width;
            }
        }

        private class Const
        {
            public static Size DropArrowSize = new Size(14, 8);
            public static int DropDownWidth = 20;
        }
    }
}

