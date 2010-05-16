namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    public class ButtonCell : Cell
    {
        private bool m_autoResizeImage;
        private bool m_bAutoTransparent;
        private Color m_bColor;
        private bool m_bPressed;
        private ButtonType m_buttonStyle;
        private Image m_defaultImage;
        private Image m_defaultImageVGA;
        private Font m_font;
        private ImageAttributes m_ia;
        private Alignment m_imageAlignment;
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
            this.m_buttonStyle = ButtonType.ImageButton;
            this.m_font = new Font("Tahoma", 8f, FontStyle.Regular);
            this.m_textAlignment = Alignment.MiddleCenter;
            this.m_textPosition = new Point(-1, -1);
            this.m_imageAlignment = Alignment.MiddleCenter;
            this.m_imagePosition = new Point(-1, -1);
            this.m_ia = new ImageAttributes();
            this.m_transparentColor = Resco.Controls.AdvancedTree.AdvancedTree.TransparentColor;
            this.m_bAutoTransparent = false;
            this.m_autoResizeImage = false;
        }

        public ButtonCell(ButtonCell bc) : base(bc)
        {
            this.m_bColor = SystemColors.ControlDark;
            this.m_pForeColor = SystemColors.HighlightText;
            this.m_pBgColor = SystemColors.ControlDark;
            this.m_pBorderColor = SystemColors.ControlDarkDark;
            this.m_text = "";
            this.m_buttonStyle = bc.m_buttonStyle;
            this.m_font = new Font(bc.m_font.Name, bc.m_font.Size, bc.m_font.Style);
            this.m_text = bc.m_text;
            this.m_textAlignment = bc.m_textAlignment;
            this.m_textPosition = bc.m_textPosition;
            this.m_imageAlignment = bc.m_imageAlignment;
            this.m_imagePosition = bc.m_imagePosition;
            this.m_autoResizeImage = bc.m_autoResizeImage;
            this.m_defaultImage = bc.m_defaultImage;
            this.m_defaultImageVGA = bc.m_defaultImageVGA;
            this.m_pressedImage = bc.m_pressedImage;
            this.m_pressedImageVGA = bc.m_pressedImageVGA;
            this.m_ia = new ImageAttributes();
            this.m_bColor = bc.m_bColor;
            this.m_pBgColor = bc.m_pBgColor;
            this.m_pBorderColor = bc.m_pBorderColor;
            this.m_pForeColor = bc.m_pForeColor;
            this.m_transparentColor = bc.m_transparentColor;
            this.m_bAutoTransparent = bc.m_bAutoTransparent;
            this.m_touchMargin = bc.m_touchMargin;
        }

        private Color BackgroundImageColor(Image image)
        {
            Color pixel;
            using (Bitmap bitmap = new Bitmap(image))
            {
                pixel = bitmap.GetPixel(0, 0);
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
                RectangleF layoutRectangle = this.GetDrawRectangle(this.m_textPosition, this.m_textAlignment, (int) (ef.Width + 1f), (int) ef.Height, drawbounds);
                Color foreColor = this.Pressed ? this.m_pForeColor : base.ForeColor;
                if (foreColor == Color.Transparent)
                {
                    foreColor = base.Owner.ForeColor;
                }
                Region clip = gr.Clip;
                gr.Clip = new Region(drawbounds);
                gr.DrawString(this.m_text, this.m_font, new SolidBrush(foreColor), layoutRectangle);
                gr.Clip = clip;
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
                if (color != Resco.Controls.AdvancedTree.AdvancedTree.TransparentColor)
                {
                    gr.FillRectangle(new SolidBrush(color), drawbounds);
                }
                if (color2 != Resco.Controls.AdvancedTree.AdvancedTree.TransparentColor)
                {
                    gr.DrawRectangle(new Pen(color2), drawbounds);
                }
            }
            else
            {
                this.DrawVistaBackground(gr, drawbounds, color, color2);
            }
            this.DrawImage(gr, drawbounds);
            drawbounds.Inflate(-2, -2);
            this.DrawButtonText(gr, drawbounds);
        }

        private void DrawImage(Graphics gr, Rectangle drawbounds)
        {
            Size size;
            Image image = this.GetImage(gr, out size);
            if (image != null)
            {
                Rectangle rectangle;
                if (this.m_bAutoTransparent)
                {
                    Color colorLow = this.BackgroundImageColor(image);
                    this.m_ia.SetColorKey(colorLow, colorLow);
                }
                Region clip = gr.Clip;
                gr.Clip = new Region(drawbounds);
                if (!this.m_autoResizeImage)
                {
                    rectangle = this.GetDrawRectangle(this.m_imagePosition, this.m_imageAlignment, size.Width, size.Height, drawbounds);
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

        private Rectangle GetDrawRectangle(Point pos, Alignment align, int objWidth, int objHeight, Rectangle drawbounds)
        {
            Rectangle rectangle = new Rectangle(drawbounds.X, drawbounds.Y, objWidth, objHeight);
            if ((pos.X != -1) || (pos.Y != -1))
            {
                rectangle.X += pos.X;
                rectangle.Y += pos.Y;
            }
            else
            {
                switch (align)
                {
                    case Alignment.MiddleCenter:
                        rectangle.X += (drawbounds.Width - objWidth) / 2;
                        rectangle.Y += (drawbounds.Height - objHeight) / 2;
                        goto Label_01D0;

                    case Alignment.MiddleRight:
                        rectangle.X += drawbounds.Width - objWidth;
                        rectangle.Y += (drawbounds.Height - objHeight) / 2;
                        goto Label_01D0;

                    case Alignment.MiddleLeft:
                        rectangle.Y += (drawbounds.Height - objHeight) / 2;
                        goto Label_01D0;

                    case (Alignment.MiddleLeft | Alignment.MiddleRight):
                    case (Alignment.BottomLeft | Alignment.MiddleRight):
                        goto Label_01D0;

                    case Alignment.BottomCenter:
                        rectangle.X += (drawbounds.Width - objWidth) / 2;
                        rectangle.Y += drawbounds.Height - objHeight;
                        goto Label_01D0;

                    case Alignment.BottomRight:
                        rectangle.X += drawbounds.Width - objWidth;
                        rectangle.Y += drawbounds.Height - objHeight;
                        goto Label_01D0;

                    case Alignment.BottomLeft:
                        rectangle.Y += drawbounds.Height - objHeight;
                        goto Label_01D0;

                    case Alignment.TopCenter:
                        rectangle.X += (drawbounds.Width - objWidth) / 2;
                        goto Label_01D0;

                    case Alignment.TopRight:
                        rectangle.X += drawbounds.Width - objWidth;
                        goto Label_01D0;
                }
            }
        Label_01D0:
            if (this.Pressed)
            {
                rectangle.X++;
                rectangle.Y++;
            }
            return rectangle;
        }

        private Image GetImage(Graphics gr, out Size scaledSize)
        {
            Image pressedImageVGA = null;
            bool flag = gr.DpiX == 192f;
            SizeF ef = new SizeF(1f, 1f);
            if (this.Pressed)
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

        protected virtual bool ShouldSerializeTextAlignment()
        {
            return (this.m_textAlignment != Alignment.MiddleCenter);
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

        internal bool Pressed
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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
                    if (!(this.m_transparentColor == Resco.Controls.AdvancedTree.AdvancedTree.TransparentColor))
                    {
                        this.m_ia.SetColorKey(this.m_transparentColor, this.m_transparentColor);
                    }
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
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

