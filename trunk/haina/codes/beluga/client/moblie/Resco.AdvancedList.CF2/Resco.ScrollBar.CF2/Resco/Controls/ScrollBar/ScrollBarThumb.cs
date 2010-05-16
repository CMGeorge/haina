namespace Resco.Controls.ScrollBar
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;

    public class ScrollBarThumb : Component
    {
        private System.Drawing.Color _borderColor;
        private ScrollBarBorderStyle _borderStyle;
        private System.Drawing.Color _color;
        private static ScrollBarThumb _defaultHighlight = new ScrollBarThumb(ScrollBarThumbDefaults.Highlight);
        private static ScrollBarThumb _defaultNormal = new ScrollBarThumb(ScrollBarThumbDefaults.Normal);
        private Resco.Controls.ScrollBar.GradientColor _gradientColor;
        private System.Drawing.Color _gripColor;
        private System.Drawing.Image _gripImage;
        private ImageAttributes _gripImageAttributes;
        private System.Drawing.Color _gripImageTransparentColor;
        private int _gripLines;
        private ScrollBarThumbGripStyle _gripStyle;
        private System.Drawing.Image _image;
        private ImageAttributes _imageAttributes;
        private ScrollBarThumbImageLayout _imageLayout;
        private System.Drawing.Color _imageTransparentColor;

        internal event EventHandler PropertyChanged;

        public ScrollBarThumb() : this(ScrollBarThumbDefaults.Normal)
        {
        }

        public ScrollBarThumb(ScrollBarThumbDefaults type)
        {
            this._imageAttributes = new ImageAttributes();
            this._imageAttributes.ClearColorKey();
            this._gripImageAttributes = new ImageAttributes();
            this._gripImageAttributes.ClearColorKey();
            switch (type)
            {
                case ScrollBarThumbDefaults.Normal:
                    this._color = SystemColors.ScrollBar;
                    this._gripColor = SystemColors.ControlText;
                    break;

                case ScrollBarThumbDefaults.Highlight:
                    this._color = SystemColors.ControlText;
                    this._gripColor = SystemColors.HighlightText;
                    break;
            }
            this._borderStyle = ScrollBarBorderStyle.Solid;
            this._borderColor = SystemColors.ControlText;
            this._gradientColor = new Resco.Controls.ScrollBar.GradientColor(FillDirection.Horizontal);
            this._gradientColor.PropertyChanged += new EventHandler(this.GradientColor_PropertyChanged);
            this._image = null;
            this._imageLayout = ScrollBarThumbImageLayout.Stretch;
            this._imageTransparentColor = System.Drawing.Color.Transparent;
            this._gripStyle = ScrollBarThumbGripStyle.Lines;
            this._gripImage = null;
            this._gripImageTransparentColor = System.Drawing.Color.Transparent;
            this._gripLines = 3;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._gradientColor != null)
                {
                    this._gradientColor.PropertyChanged -= new EventHandler(this.GradientColor_PropertyChanged);
                }
                if (this._image != null)
                {
                    this._image.Dispose();
                }
                if (this._gripImage != null)
                {
                    this._gripImage.Dispose();
                }
            }
            this._gradientColor = null;
            this._image = null;
            this._gripImage = null;
            base.Dispose(disposing);
        }

        private void DoDrawGrip(Graphics gr, Rectangle drawRect, Resco.Controls.ScrollBar.ScrollBar.ScrollBarOrientation orientation)
        {
            if (this._gripStyle != ScrollBarThumbGripStyle.None)
            {
                if (this._gripStyle == ScrollBarThumbGripStyle.Image)
                {
                    if (this._gripImage != null)
                    {
                        Rectangle destRect = new Rectangle(drawRect.Left + ((drawRect.Width - this._gripImage.Width) / 2), drawRect.Top + ((drawRect.Height - this._gripImage.Height) / 2), this._gripImage.Width, this._gripImage.Height);
                        gr.DrawImage(this._gripImage, destRect, 0, 0, this._gripImage.Width, this._gripImage.Height, GraphicsUnit.Pixel, this._gripImageAttributes);
                    }
                }
                else if (this._gripLines > 0)
                {
                    drawRect.Inflate(-1, -1);
                    if ((drawRect.Width > 0) && (drawRect.Height > 0))
                    {
                        int width = (this._gripLines * 2) - 1;
                        if (this._gripStyle == ScrollBarThumbGripStyle.Lines)
                        {
                            int left = drawRect.Left;
                            int top = drawRect.Top;
                            int num4 = 0;
                            int num5 = 0;
                            int num6 = 0;
                            int num7 = 0;
                            if (orientation == Resco.Controls.ScrollBar.ScrollBar.ScrollBarOrientation.Horizontal)
                            {
                                if (width < drawRect.Width)
                                {
                                    left += (drawRect.Width - width) / 2;
                                }
                                else
                                {
                                    width = drawRect.Width;
                                }
                                num4 = 2;
                                num7 = drawRect.Height - 1;
                            }
                            else
                            {
                                if (width < drawRect.Height)
                                {
                                    top += (drawRect.Height - width) / 2;
                                }
                                else
                                {
                                    width = drawRect.Height;
                                }
                                num5 = 2;
                                num6 = drawRect.Width - 1;
                            }
                            Pen pen = Resco.Controls.ScrollBar.ScrollBar.GetPen(this._gripColor);
                            for (int i = 0; i < width; i += 2)
                            {
                                gr.DrawLine(pen, left, top, left + num6, top + num7);
                                left += num4;
                                top += num5;
                            }
                        }
                        else if (this._gripStyle == ScrollBarThumbGripStyle.Dots)
                        {
                            int num9 = 0;
                            int height = 0;
                            int num12 = drawRect.Left;
                            int y = drawRect.Top;
                            if (orientation == Resco.Controls.ScrollBar.ScrollBar.ScrollBarOrientation.Horizontal)
                            {
                                if (width < drawRect.Width)
                                {
                                    num12 += (drawRect.Width - width) / 2;
                                }
                                else
                                {
                                    width = drawRect.Width;
                                }
                                num9 = width;
                                height = drawRect.Height;
                            }
                            else
                            {
                                if (width < drawRect.Height)
                                {
                                    y += (drawRect.Height - width) / 2;
                                }
                                else
                                {
                                    width = drawRect.Height;
                                }
                                num9 = drawRect.Width;
                                height = width;
                            }
                            for (int j = 0; j < height; j += 2)
                            {
                                int x = num12;
                                for (int k = 0; k < num9; k += 2)
                                {
                                    Resco.Controls.ScrollBar.ScrollBar.DrawPixel(gr, this._gripColor, x, y);
                                    x += 2;
                                }
                                y += 2;
                            }
                        }
                    }
                }
            }
        }

        public void Draw(Graphics gr, Rectangle drawRect, Resco.Controls.ScrollBar.ScrollBar.ScrollBarOrientation orientation)
        {
            if (this._borderStyle != ScrollBarBorderStyle.None)
            {
                Resco.Controls.ScrollBar.ScrollBar.DoDrawBorder(gr, this._borderStyle, this._borderColor, drawRect, Resco.Controls.ScrollBar.ScrollBar.BorderSide.All, System.Drawing.Color.Transparent);
                drawRect.Inflate(-1, -1);
            }
            if ((drawRect.Width > 0) && (drawRect.Height > 0))
            {
                Region clip = gr.Clip;
                Region helperRegion = Resco.Controls.ScrollBar.ScrollBar.GetHelperRegion();
                helperRegion.MakeEmpty();
                helperRegion.Union(drawRect);
                gr.Clip = helperRegion;
                if (this._image != null)
                {
                    if (this._imageLayout == ScrollBarThumbImageLayout.Tile)
                    {
                        int width = drawRect.Width;
                        int height = drawRect.Height;
                        Rectangle destRect = new Rectangle(drawRect.Left, drawRect.Top, this._image.Width, this._image.Height);
                        for (int i = 0; i < height; i += this._image.Height)
                        {
                            for (int j = 0; j < width; j += this._image.Width)
                            {
                                gr.DrawImage(this._image, destRect, 0, 0, this._image.Width, this._image.Height, GraphicsUnit.Pixel, this._imageAttributes);
                                destRect.X += this._image.Width;
                            }
                            destRect.X = drawRect.Left;
                            destRect.Y += this._image.Height;
                        }
                    }
                    else
                    {
                        Rectangle rectangle2;
                        if (this._imageLayout == ScrollBarThumbImageLayout.Center)
                        {
                            rectangle2 = new Rectangle(drawRect.Left + ((drawRect.Width - this._image.Width) / 2), drawRect.Top + ((drawRect.Height - this._image.Height) / 2), this._image.Width, this._image.Height);
                        }
                        else
                        {
                            rectangle2 = drawRect;
                        }
                        gr.DrawImage(this._image, rectangle2, 0, 0, this._image.Width, this._image.Height, GraphicsUnit.Pixel, this._imageAttributes);
                    }
                }
                else if (this._gradientColor.CanDraw())
                {
                    this._gradientColor.DrawGradient(gr, drawRect);
                }
                else
                {
                    gr.FillRectangle(Resco.Controls.ScrollBar.ScrollBar.GetBrush(this._color), drawRect);
                }
                this.DoDrawGrip(gr, drawRect, orientation);
                gr.Clip = clip;
            }
        }

        private void GradientColor_PropertyChanged(object sender, EventArgs e)
        {
            this.OnPropertyChanged(EventArgs.Empty);
        }

        protected virtual void OnPropertyChanged(EventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        protected virtual bool ShouldSerializeBorderStyle()
        {
            return (this._borderStyle != ScrollBarBorderStyle.Solid);
        }

        protected virtual bool ShouldSerializeGradientColor()
        {
            return Resco.Controls.ScrollBar.ScrollBar.ShouldSerializeGradientColor(this._gradientColor);
        }

        protected virtual bool ShouldSerializeGripStyle()
        {
            return (this._gripStyle != ScrollBarThumbGripStyle.Lines);
        }

        protected virtual bool ShouldSerializeImageLayout()
        {
            return (this._imageLayout != ScrollBarThumbImageLayout.Stretch);
        }

        public System.Drawing.Color BorderColor
        {
            get
            {
                return this._borderColor;
            }
            set
            {
                if (this._borderColor != value)
                {
                    this._borderColor = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public ScrollBarBorderStyle BorderStyle
        {
            get
            {
                return this._borderStyle;
            }
            set
            {
                if (this._borderStyle != value)
                {
                    this._borderStyle = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this._color;
            }
            set
            {
                if (this._color != value)
                {
                    this._color = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public static ScrollBarThumb Default
        {
            get
            {
                return _defaultNormal;
            }
        }

        public static ScrollBarThumb DefaultHighlight
        {
            get
            {
                return _defaultHighlight;
            }
        }

        public Resco.Controls.ScrollBar.GradientColor GradientColor
        {
            get
            {
                return this._gradientColor;
            }
            set
            {
                if (this._gradientColor != value)
                {
                    this._gradientColor.PropertyChanged -= new EventHandler(this.GradientColor_PropertyChanged);
                    this._gradientColor = value;
                    this._gradientColor.PropertyChanged += new EventHandler(this.GradientColor_PropertyChanged);
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public System.Drawing.Color GripColor
        {
            get
            {
                return this._gripColor;
            }
            set
            {
                if (this._gripColor != value)
                {
                    this._gripColor = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public System.Drawing.Image GripImage
        {
            get
            {
                return this._gripImage;
            }
            set
            {
                if (this._gripImage != value)
                {
                    this._gripImage = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public System.Drawing.Color GripImageTransparentColor
        {
            get
            {
                return this._gripImageTransparentColor;
            }
            set
            {
                if (this._gripImageTransparentColor != value)
                {
                    this._gripImageTransparentColor = value;
                    if (this._gripImageTransparentColor == System.Drawing.Color.Transparent)
                    {
                        this._gripImageAttributes.ClearColorKey();
                    }
                    else
                    {
                        this._gripImageAttributes.SetColorKey(this._gripImageTransparentColor, this._gripImageTransparentColor);
                    }
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public int GripLines
        {
            get
            {
                return this._gripLines;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (this._gripLines != value)
                {
                    this._gripLines = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public ScrollBarThumbGripStyle GripStyle
        {
            get
            {
                return this._gripStyle;
            }
            set
            {
                if (this._gripStyle != value)
                {
                    this._gripStyle = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this._image;
            }
            set
            {
                if (this._image != value)
                {
                    this._image = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public ScrollBarThumbImageLayout ImageLayout
        {
            get
            {
                return this._imageLayout;
            }
            set
            {
                if (this._imageLayout != value)
                {
                    this._imageLayout = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public System.Drawing.Color ImageTransparentColor
        {
            get
            {
                return this._imageTransparentColor;
            }
            set
            {
                if (this._imageTransparentColor != value)
                {
                    this._imageTransparentColor = value;
                    if (this._imageTransparentColor == System.Drawing.Color.Transparent)
                    {
                        this._imageAttributes.ClearColorKey();
                    }
                    else
                    {
                        this._imageAttributes.SetColorKey(this._imageTransparentColor, this._imageTransparentColor);
                    }
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public enum ScrollBarThumbDefaults
        {
            Normal,
            Highlight
        }

        public enum ScrollBarThumbGripStyle
        {
            None,
            Lines,
            Dots,
            Image
        }

        public enum ScrollBarThumbImageLayout
        {
            Center,
            Stretch,
            Tile
        }
    }
}

