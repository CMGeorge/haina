namespace Resco.Controls.AdvancedList
{
    using Resco.Controls.AdvancedList.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ButtonCell : Cell
    {
        private bool m_autoResizeImage;
        private bool m_bAutoTransparent;
        private Color m_bColor;
        private bool m_bPressed;
        private ButtonType m_buttonStyle;
        private Image m_defaultImage;
        private Image m_defaultImageVGA;
        private EventHandler m_DetachImageList;
        private Font m_font;
        private ImageAttributes m_ia;
        private Alignment m_imageAlignment;
        private System.Windows.Forms.ImageList m_ImageList;
        private Point m_imagePosition;
        private Color m_pBgColor;
        private Color m_pBorderColor;
        private Color m_pForeColor;
        private Image m_pressedImage;
        private Image m_pressedImageVGA;
        private string m_text;
        private Alignment m_textAlignment;
        private Point m_textPosition;
        private int m_touchMargin;
        private Color m_transparentColor;

        public ButtonCell()
        {
            this.m_bColor = SystemColors.ControlDark;
            this.m_pForeColor = SystemColors.HighlightText;
            this.m_pBgColor = SystemColors.ControlDark;
            this.m_pBorderColor = SystemColors.ControlDarkDark;
            this.m_text = "";
            this.m_DetachImageList = new EventHandler(this.OnDetachImageList);
            this.m_buttonStyle = ButtonType.ImageButton;
            this.m_font = new Font("Tahoma", 8f, FontStyle.Regular);
            this.m_textAlignment = Alignment.MiddleCenter;
            this.m_textPosition = new Point(-1, -1);
            this.m_imageAlignment = Alignment.MiddleCenter;
            this.m_imagePosition = new Point(-1, -1);
            this.m_ia = new ImageAttributes();
            this.m_transparentColor = Resco.Controls.AdvancedList.AdvancedList.TransparentColor;
            this.m_bAutoTransparent = false;
            this.m_autoResizeImage = false;
            base.Selectable = true;
        }

        public ButtonCell(ButtonCell bc) : base(bc)
        {
            this.m_bColor = SystemColors.ControlDark;
            this.m_pForeColor = SystemColors.HighlightText;
            this.m_pBgColor = SystemColors.ControlDark;
            this.m_pBorderColor = SystemColors.ControlDarkDark;
            this.m_text = "";
            this.m_DetachImageList = new EventHandler(this.OnDetachImageList);
            this.m_buttonStyle = bc.m_buttonStyle;
            this.m_font = new Font(bc.m_font.Name, bc.m_font.Size, bc.m_font.Style);
            this.m_text = bc.m_text;
            this.m_textAlignment = bc.m_textAlignment;
            this.m_textPosition = bc.m_textPosition;
            this.m_imageAlignment = bc.m_imageAlignment;
            this.m_imagePosition = bc.m_imagePosition;
            this.m_autoResizeImage = bc.m_autoResizeImage;
            this.m_ia = new ImageAttributes();
            this.m_bColor = bc.m_bColor;
            this.m_pBgColor = bc.m_pBgColor;
            this.m_pBorderColor = bc.m_pBorderColor;
            this.m_pForeColor = bc.m_pForeColor;
            this.m_transparentColor = bc.m_transparentColor;
            this.m_bAutoTransparent = bc.m_bAutoTransparent;
            this.m_touchMargin = bc.m_touchMargin;
            this.m_bPressed = bc.m_bPressed;
            this.m_defaultImage = bc.m_defaultImage;
            this.m_defaultImageVGA = bc.m_defaultImageVGA;
            this.m_pressedImage = bc.m_pressedImage;
            this.m_pressedImageVGA = bc.m_pressedImageVGA;
            base.Selectable = bc.Selectable;
        }

        private Color BackgroundImageColor(Image image)
        {
            Bitmap bitmap = image as Bitmap;
            if (bitmap == null)
            {
                bitmap = new Bitmap(image);
            }
            Color pixel = bitmap.GetPixel(0, 0);
            if (bitmap != image)
            {
                bitmap.Dispose();
            }
            return pixel;
        }

        public override Cell Clone()
        {
            return new ButtonCell(this);
        }

        private void DrawButtonText(Graphics gr, Rectangle drawbounds)
        {
            if (((this.m_text != null) && (this.m_text != string.Empty)) && ((drawbounds.Width > 0) && (drawbounds.Height > 0)))
            {
                SizeF ef = gr.MeasureString(this.m_text, this.m_font);
                RectangleF layoutRectangle = this.GetDrawRectangle(this.m_textPosition, this.m_textAlignment, (float) ((int) (ef.Width + 1f)), (float) ((int) ef.Height), drawbounds);
                Color foreColor = this.Pressed ? this.m_pForeColor : base.ForeColor;
                if (foreColor == Color.Transparent)
                {
                    foreColor = base.Owner.ForeColor;
                }
                Region clip = gr.Clip;
                using (Region region2 = new Region(drawbounds))
                {
                    region2.Intersect(clip);
                    gr.Clip = region2;
                    gr.DrawString(this.m_text, this.m_font, new SolidBrush(foreColor), layoutRectangle);
                    gr.Clip = clip;
                }
            }
        }

        protected override void DrawContent(Graphics gr, Rectangle drawbounds, object data)
        {
            Color color = this.Pressed ? this.m_pBgColor : this.BackColor;
            Color color2 = this.Pressed ? this.m_pBorderColor : this.m_bColor;
            drawbounds.Width--;
            drawbounds.Height--;
            drawbounds.Inflate(-this.m_touchMargin, -this.m_touchMargin);
            base.Parent.AddButtonArea(drawbounds);
            drawbounds.Inflate(this.m_touchMargin, this.m_touchMargin);
            if (this.m_buttonStyle == ButtonType.ImageButton)
            {
                if (color != Resco.Controls.AdvancedList.AdvancedList.TransparentColor)
                {
                    using (SolidBrush brush = new SolidBrush(color))
                    {
                        gr.FillRectangle(brush, drawbounds);
                    }
                }
                if (!(color2 != Resco.Controls.AdvancedList.AdvancedList.TransparentColor))
                {
                    goto Label_00E2;
                }
                using (Pen pen = new Pen(color2))
                {
                    gr.DrawRectangle(pen, drawbounds);
                    goto Label_00E2;
                }
            }
            this.DrawVistaBackground(gr, drawbounds, color, color2);
        Label_00E2:
            this.DrawImage(gr, drawbounds, data);
            drawbounds.Inflate(-2, -2);
            this.DrawButtonText(gr, drawbounds);
        }

        private void DrawImage(Graphics gr, Rectangle drawbounds, object data)
        {
            Size size;
            Image image = this.GetImage(gr, out size, data);
            if (image != null)
            {
                Rectangle rectangle;
                if (this.m_bAutoTransparent)
                {
                    Color colorLow = this.BackgroundImageColor(image);
                    this.m_ia.SetColorKey(colorLow, colorLow);
                }
                Region clip = gr.Clip;
                Region region2 = new Region(drawbounds);
                region2.Intersect(clip);
                gr.Clip = region2;
                if (!this.m_autoResizeImage)
                {
                    RectangleF ef = this.GetDrawRectangle(this.m_imagePosition, this.m_imageAlignment, (float) size.Width, (float) size.Height, drawbounds);
                    rectangle = new Rectangle((int) ef.X, (int) ef.Y, (int) ef.Width, (int) ef.Height);
                }
                else
                {
                    rectangle = drawbounds;
                }
                gr.DrawImage(image, rectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, this.m_ia);
                gr.Clip = clip;
            }
        }

        protected override void DrawSelection(Graphics gr, Rectangle drawbounds)
        {
            if (!this.Pressed)
            {
                Rectangle rect = new Rectangle(drawbounds.X + 1, drawbounds.Y + 1, drawbounds.Width - 3, drawbounds.Height - 3);
                gr.DrawRectangle(new Pen(this.m_bColor), rect);
            }
        }

        private void DrawVistaBackground(Graphics gr, Rectangle drawbounds, Color bgc, Color bc)
        {
            GradientFill.DrawVistaGradient(gr, bgc, drawbounds, FillDirection.Vertical);
            using (Pen pen = new Pen(bc))
            {
                gr.DrawLine(pen, drawbounds.X, drawbounds.Y - 1, drawbounds.Right - 1, drawbounds.Y - 1);
                gr.DrawLine(pen, drawbounds.X, drawbounds.Bottom, drawbounds.Right - 1, drawbounds.Bottom);
                gr.DrawLine(pen, drawbounds.X - 1, drawbounds.Y, drawbounds.X - 1, drawbounds.Bottom - 1);
                gr.DrawLine(pen, drawbounds.Right, drawbounds.Y, drawbounds.Right, drawbounds.Bottom - 1);
            }
        }

        private RectangleF GetDrawRectangle(Point pos, Alignment align, float objWidth, float objHeight, Rectangle drawbounds)
        {
            RectangleF ef = new RectangleF((float) drawbounds.X, (float) drawbounds.Y, objWidth, objHeight);
            if ((pos.X != -1) || (pos.Y != -1))
            {
                ef.X += pos.X;
                ef.Y += pos.Y;
            }
            else
            {
                switch (align)
                {
                    case Alignment.MiddleCenter:
                        ef.X += (drawbounds.Width - objWidth) / 2f;
                        ef.Y += (drawbounds.Height - objHeight) / 2f;
                        goto Label_01FB;

                    case Alignment.MiddleRight:
                        ef.X += drawbounds.Width - objWidth;
                        ef.Y += (drawbounds.Height - objHeight) / 2f;
                        goto Label_01FB;

                    case Alignment.MiddleLeft:
                        ef.Y += (drawbounds.Height - objHeight) / 2f;
                        goto Label_01FB;

                    case (Alignment.MiddleLeft | Alignment.MiddleRight):
                    case (Alignment.BottomLeft | Alignment.MiddleRight):
                        goto Label_01FB;

                    case Alignment.BottomCenter:
                        ef.X += (drawbounds.Width - objWidth) / 2f;
                        ef.Y += drawbounds.Height - objHeight;
                        goto Label_01FB;

                    case Alignment.BottomRight:
                        ef.X += drawbounds.Width - objWidth;
                        ef.Y += drawbounds.Height - objHeight;
                        goto Label_01FB;

                    case Alignment.BottomLeft:
                        ef.Y += drawbounds.Height - objHeight;
                        goto Label_01FB;

                    case Alignment.TopCenter:
                        ef.X += (drawbounds.Width - objWidth) / 2f;
                        goto Label_01FB;

                    case Alignment.TopRight:
                        ef.X += drawbounds.Width - objWidth;
                        goto Label_01FB;
                }
            }
        Label_01FB:
            if (this.Pressed)
            {
                ef.X++;
                ef.Y++;
            }
            return ef;
        }

        private Image GetImage(Graphics gr, out Size scaledSize, object data)
        {
            Image pressedImageVGA = null;
            bool flag = gr.DpiX == 192f;
            SizeF ef = new SizeF(1f, 1f);
            if (this.m_ImageList != null)
            {
                int num = -1;
                try
                {
                    num = Convert.ToInt32(data);
                }
                catch (Exception)
                {
                }
                if ((num >= 0) && (num < this.ImageList.Images.Count))
                {
                    pressedImageVGA = ImageCache.GlobalCache[this.ImageList, num];
                }
            }
            else if (this.Pressed)
            {
                if (flag)
                {
                    pressedImageVGA = this.m_pressedImageVGA;
                    if (pressedImageVGA == null)
                    {
                        pressedImageVGA = this.m_pressedImage;
                        ef.Width *= 2f;
                        ef.Height *= 2f;
                    }
                }
                else
                {
                    pressedImageVGA = this.m_pressedImage;
                    if (pressedImageVGA == null)
                    {
                        pressedImageVGA = this.m_pressedImageVGA;
                        ef.Width *= 0.5f;
                        ef.Height *= 0.5f;
                    }
                }
            }
            else if (flag)
            {
                pressedImageVGA = this.m_defaultImageVGA;
                if ((pressedImageVGA == null) && (this.m_defaultImage != null))
                {
                    pressedImageVGA = this.m_defaultImage;
                    ef.Width *= 2f;
                    ef.Height *= 2f;
                }
            }
            else
            {
                pressedImageVGA = this.m_defaultImage;
                if ((pressedImageVGA == null) && (this.m_defaultImageVGA != null))
                {
                    pressedImageVGA = this.m_defaultImageVGA;
                    ef.Width *= 0.5f;
                    ef.Height *= 0.5f;
                }
            }
            if (pressedImageVGA != null)
            {
                scaledSize = new Size((int) (pressedImageVGA.Width * ef.Width), (int) (pressedImageVGA.Height * ef.Height));
                return pressedImageVGA;
            }
            scaledSize = new Size(0, 0);
            return pressedImageVGA;
        }

        private void OnDetachImageList(object sender, EventArgs e)
        {
            this.ImageList = null;
        }

        internal override void Scale(float fx, float fy)
        {
            base.Scale(fx, fy);
            if (this.m_textPosition.X > 0)
            {
                this.m_textPosition.X = (int) (this.m_textPosition.X * fx);
            }
            if (this.m_textPosition.Y > 0)
            {
                this.m_textPosition.Y = (int) (this.m_textPosition.Y * fy);
            }
            if (this.m_imagePosition.X > 0)
            {
                this.m_imagePosition.X = (int) (this.m_imagePosition.X * fx);
            }
            if (this.m_imagePosition.Y > 0)
            {
                this.m_imagePosition.Y = (int) (this.m_imagePosition.Y * fy);
            }
            this.m_touchMargin = (int) (this.m_touchMargin * fx);
        }

        protected virtual bool ShouldSerializeButtonStyle()
        {
            return (this.m_buttonStyle != ButtonType.ImageButton);
        }

        protected virtual bool ShouldSerializeImageAlignment()
        {
            return (this.m_imageAlignment != Alignment.MiddleCenter);
        }

        protected virtual bool ShouldSerializeImageList()
        {
            return (this.m_ImageList != null);
        }

        protected virtual bool ShouldSerializeTextAlignment()
        {
            return (this.m_textAlignment != Alignment.MiddleCenter);
        }

        [DefaultValue(false)]
        public bool AutoResizeImage
        {
            get
            {
                return this.m_autoResizeImage;
            }
            set
            {
                if (this.m_autoResizeImage != value)
                {
                    this.m_autoResizeImage = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue(false)]
        public bool AutoTransparent
        {
            get
            {
                return this.m_bAutoTransparent;
            }
            set
            {
                if (this.m_bAutoTransparent != value)
                {
                    this.m_bAutoTransparent = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue("ControlDark")]
        public Color BorderColor
        {
            get
            {
                return this.m_bColor;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = SystemColors.ControlDark;
                }
                if (!(value == this.m_bColor))
                {
                    this.m_bColor = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue(0)]
        public ButtonType ButtonStyle
        {
            get
            {
                return this.m_buttonStyle;
            }
            set
            {
                this.m_buttonStyle = value;
            }
        }

        [DefaultValue(0)]
        public Alignment ImageAlignment
        {
            get
            {
                return this.m_imageAlignment;
            }
            set
            {
                if (this.m_imageAlignment != value)
                {
                    this.m_imageAlignment = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue((string) null)]
        public Image ImageDefault
        {
            get
            {
                return this.m_defaultImage;
            }
            set
            {
                if (value != this.m_defaultImage)
                {
                    this.m_defaultImage = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue((string) null)]
        public Image ImageDefaultVGA
        {
            get
            {
                return this.m_defaultImageVGA;
            }
            set
            {
                if (value != this.m_defaultImageVGA)
                {
                    this.m_defaultImageVGA = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue(-1)]
        public int ImageIndex
        {
            get
            {
                int num = -1;
                if (base.CellSource.SourceType == CellSourceType.Constant)
                {
                    try
                    {
                        num = Convert.ToInt32(base.CellSource.ConstantData);
                    }
                    catch (Exception)
                    {
                    }
                }
                return num;
            }
            set
            {
                if (value >= 0)
                {
                    base.CellSource.ConstantData = Convert.ToString(value);
                }
            }
        }

        [DefaultValue((string) null)]
        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                return this.m_ImageList;
            }
            set
            {
                if (this.m_ImageList != null)
                {
                    ImageCache.GlobalCache.Clear(this.m_ImageList);
                }
                if (value != this.m_ImageList)
                {
                    if (((this.m_ImageList != null) && (this.m_ImageList.Site != null)) && this.m_ImageList.Site.DesignMode)
                    {
                        this.m_ImageList.Disposed -= this.m_DetachImageList;
                    }
                    this.m_ImageList = value;
                    if (((this.m_ImageList != null) && (this.m_ImageList.Site != null)) && this.m_ImageList.Site.DesignMode)
                    {
                        value.Disposed += this.m_DetachImageList;
                    }
                }
                base.OnChanged(this, GridEventArgsType.Repaint, null);
            }
        }

        [DefaultValue("-1,-1")]
        public Point ImagePosition
        {
            get
            {
                return this.m_imagePosition;
            }
            set
            {
                if (this.m_imagePosition != value)
                {
                    this.m_imagePosition = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue((string) null)]
        public Image ImagePressed
        {
            get
            {
                return this.m_pressedImage;
            }
            set
            {
                if (value != this.m_pressedImage)
                {
                    this.m_pressedImage = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue((string) null)]
        public Image ImagePressedVGA
        {
            get
            {
                return this.m_pressedImageVGA;
            }
            set
            {
                if (value != this.m_pressedImageVGA)
                {
                    this.m_pressedImageVGA = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        protected internal bool Pressed
        {
            get
            {
                return this.m_bPressed;
            }
            set
            {
                this.m_bPressed = value;
            }
        }

        [DefaultValue("ControlDark")]
        public Color PressedBackColor
        {
            get
            {
                return this.m_pBgColor;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = SystemColors.ControlDark;
                }
                if (!(value == this.m_pBgColor))
                {
                    this.m_pBgColor = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue("ControlDarkDark")]
        public Color PressedBorderColor
        {
            get
            {
                return this.m_pBorderColor;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = SystemColors.ControlDarkDark;
                }
                if (!(value == this.m_pBorderColor))
                {
                    this.m_pBorderColor = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue("HighlightText")]
        public Color PressedForeColor
        {
            get
            {
                return this.m_pForeColor;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = SystemColors.Highlight;
                }
                if (value != this.m_pForeColor)
                {
                    this.m_pForeColor = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue("")]
        public string Text
        {
            get
            {
                return this.m_text;
            }
            set
            {
                if (this.m_text != value)
                {
                    this.m_text = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue(0)]
        public Alignment TextAlignment
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
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue("Tahoma, 8pt")]
        public virtual Font TextFont
        {
            get
            {
                return this.m_font;
            }
            set
            {
                if (this.m_font != value)
                {
                    this.m_font = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue("-1,-1")]
        public Point TextPosition
        {
            get
            {
                return this.m_textPosition;
            }
            set
            {
                if (this.m_textPosition != value)
                {
                    this.m_textPosition = value;
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue("0")]
        public int TouchMargin
        {
            get
            {
                return this.m_touchMargin;
            }
            set
            {
                if (this.m_touchMargin != value)
                {
                    this.m_touchMargin = value;
                    if ((base.Parent != null) && (base.Parent.DataRows != null))
                    {
                        base.Parent.DataRows.ResetCachedBounds(base.Owner);
                    }
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue("Transparent")]
        public Color TransparentColor
        {
            get
            {
                return this.m_transparentColor;
            }
            set
            {
                if (this.m_transparentColor != value)
                {
                    if (value.IsEmpty)
                    {
                        value = Color.Transparent;
                    }
                    this.m_transparentColor = value;
                    this.m_ia.ClearColorKey();
                    if (!(this.m_transparentColor == Resco.Controls.AdvancedList.AdvancedList.TransparentColor))
                    {
                        this.m_ia.SetColorKey(this.m_transparentColor, this.m_transparentColor);
                    }
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        public enum ButtonType
        {
            ImageButton,
            VistaStyleImageButton
        }
    }
}

