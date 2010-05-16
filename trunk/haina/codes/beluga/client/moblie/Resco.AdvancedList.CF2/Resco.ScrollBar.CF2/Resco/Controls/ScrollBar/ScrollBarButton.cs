namespace Resco.Controls.ScrollBar
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;

    public class ScrollBarButton : Component
    {
        private System.Drawing.Color _arrowColor;
        private System.Drawing.Image _arrowImage;
        private ScrollBarButtonArrowImageLayout _arrowImageLayout;
        private System.Drawing.Color _arrowImageTransparentColor;
        private bool _borderClosed;
        private System.Drawing.Color _borderColor;
        private ScrollBarBorderStyle _borderStyle;
        private System.Drawing.Color _color;
        private static ScrollBarButton _defaultDisabled = new ScrollBarButton(ScrollBarButtonDefaults.Disabled);
        private static ScrollBarButton _defaultHighlight = new ScrollBarButton(ScrollBarButtonDefaults.Highlight);
        private static ScrollBarButton _defaultNormal = new ScrollBarButton(ScrollBarButtonDefaults.Normal);
        private Resco.Controls.ScrollBar.GradientColor _gradientColor;
        private System.Drawing.Image _image;
        private ImageAttributes _imageAttributes;

        internal event EventHandler PropertyChanged;

        public ScrollBarButton() : this(ScrollBarButtonDefaults.Normal)
        {
        }

        public ScrollBarButton(ScrollBarButtonDefaults type)
        {
            this._imageAttributes = new ImageAttributes();
            this._imageAttributes.ClearColorKey();
            switch (type)
            {
                case ScrollBarButtonDefaults.Normal:
                    this._color = SystemColors.ScrollBar;
                    this._arrowColor = SystemColors.ControlText;
                    break;

                case ScrollBarButtonDefaults.Highlight:
                    this._color = SystemColors.ControlText;
                    this._arrowColor = SystemColors.HighlightText;
                    break;

                case ScrollBarButtonDefaults.Disabled:
                    this._color = SystemColors.ScrollBar;
                    this._arrowColor = SystemColors.GrayText;
                    break;
            }
            this._borderStyle = ScrollBarBorderStyle.Solid;
            this._borderColor = SystemColors.ControlText;
            this._borderClosed = false;
            this._gradientColor = new Resco.Controls.ScrollBar.GradientColor(FillDirection.Horizontal);
            this._gradientColor.PropertyChanged += new EventHandler(this.GradientColor_PropertyChanged);
            this._image = null;
            this._arrowImage = null;
            this._arrowImageLayout = ScrollBarButtonArrowImageLayout.Center;
            this._arrowImageTransparentColor = System.Drawing.Color.Transparent;
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
                if (this._arrowImage != null)
                {
                    this._arrowImage.Dispose();
                }
            }
            this._gradientColor = null;
            this._image = null;
            this._arrowImage = null;
            base.Dispose(disposing);
        }

        private void DoDrawArrow(Graphics gr, Rectangle drawRect, ScrollBarArrow type)
        {
            if (this._arrowImage != null)
            {
                Rectangle rectangle;
                if (this._arrowImageLayout == ScrollBarButtonArrowImageLayout.Center)
                {
                    rectangle = new Rectangle(drawRect.Left + ((drawRect.Width - this._arrowImage.Width) / 2), drawRect.Top + ((drawRect.Height - this._arrowImage.Height) / 2), this._arrowImage.Width, this._arrowImage.Height);
                }
                else
                {
                    rectangle = drawRect;
                }
                gr.DrawImage(this._arrowImage, rectangle, 0, 0, this._arrowImage.Width, this._arrowImage.Height, GraphicsUnit.Pixel, this._imageAttributes);
            }
            else
            {
                drawRect.Inflate(-1, -1);
                if (((drawRect.Width > 0) && (drawRect.Height > 0)) && (this._arrowColor != System.Drawing.Color.Transparent))
                {
                    int num = 0;
                    int left = drawRect.Left;
                    int top = drawRect.Top;
                    int num4 = 0;
                    int num5 = 0;
                    switch (type)
                    {
                        case ScrollBarArrow.Left:
                        case ScrollBarArrow.Right:
                            num = (drawRect.Width < 5) ? drawRect.Width : 5;
                            left += ((drawRect.Width - num) / 2) + ((type == ScrollBarArrow.Right) ? num : 0);
                            top += drawRect.Height / 2;
                            num4 = (type == ScrollBarArrow.Left) ? 1 : -1;
                            num5 = -1;
                            break;

                        case ScrollBarArrow.Up:
                        case ScrollBarArrow.Down:
                            num = (drawRect.Height < 5) ? drawRect.Height : 5;
                            left += drawRect.Width / 2;
                            top += ((drawRect.Height - num) / 2) + ((type == ScrollBarArrow.Down) ? (num - 1) : 0);
                            num4 = (type == ScrollBarArrow.Down) ? 1 : -1;
                            num5 = (type == ScrollBarArrow.Down) ? -1 : 1;
                            break;
                    }
                    int num6 = (type == ScrollBarArrow.Up) ? 2 : ((type == ScrollBarArrow.Down) ? -2 : 0);
                    int num7 = ((type == ScrollBarArrow.Left) || (type == ScrollBarArrow.Right)) ? 2 : 0;
                    int num8 = num6;
                    int num9 = num7;
                    Pen pen = Resco.Controls.ScrollBar.ScrollBar.GetPen(this._arrowColor);
                    Resco.Controls.ScrollBar.ScrollBar.DrawPixel(gr, this._arrowColor, left, top);
                    for (int i = 1; i < num; i++)
                    {
                        left += num4;
                        top += num5;
                        gr.DrawLine(pen, left, top, left + num6, top + num7);
                        num6 += num8;
                        num7 += num9;
                    }
                }
            }
        }

        public void Draw(Graphics gr, Rectangle drawRect, ScrollBarArrow type, System.Drawing.Color parentColor)
        {
            if (this._borderStyle != ScrollBarBorderStyle.None)
            {
                Resco.Controls.ScrollBar.ScrollBar.BorderSide all = Resco.Controls.ScrollBar.ScrollBar.BorderSide.All;
                if (!this._borderClosed)
                {
                    switch (type)
                    {
                        case ScrollBarArrow.Left:
                            all &= ~Resco.Controls.ScrollBar.ScrollBar.BorderSide.Right;
                            break;

                        case ScrollBarArrow.Right:
                            all &= ~Resco.Controls.ScrollBar.ScrollBar.BorderSide.Left;
                            break;

                        case ScrollBarArrow.Up:
                            all &= ~Resco.Controls.ScrollBar.ScrollBar.BorderSide.Down;
                            break;

                        case ScrollBarArrow.Down:
                            all &= ~Resco.Controls.ScrollBar.ScrollBar.BorderSide.Up;
                            break;
                    }
                }
                Resco.Controls.ScrollBar.ScrollBar.DoDrawBorder(gr, this._borderStyle, this._borderColor, drawRect, all, parentColor);
                drawRect.Inflate(-1, -1);
                if (all != Resco.Controls.ScrollBar.ScrollBar.BorderSide.All)
                {
                    if ((type == ScrollBarArrow.Left) || (type == ScrollBarArrow.Right))
                    {
                        drawRect.Width++;
                        if (type == ScrollBarArrow.Right)
                        {
                            drawRect.X--;
                        }
                    }
                    else
                    {
                        drawRect.Height++;
                        if (type == ScrollBarArrow.Down)
                        {
                            drawRect.Y--;
                        }
                    }
                }
            }
            if ((drawRect.Width > 0) && (drawRect.Height > 0))
            {
                if (this._image != null)
                {
                    gr.DrawImage(this._image, drawRect, new Rectangle(0, 0, this._image.Width, this._image.Height), GraphicsUnit.Pixel);
                }
                else if (this._gradientColor.CanDraw())
                {
                    this._gradientColor.DrawGradient(gr, drawRect);
                }
                else
                {
                    gr.FillRectangle(Resco.Controls.ScrollBar.ScrollBar.GetBrush(this._color), drawRect);
                }
                Region clip = gr.Clip;
                Region helperRegion = Resco.Controls.ScrollBar.ScrollBar.GetHelperRegion();
                helperRegion.MakeEmpty();
                helperRegion.Union(drawRect);
                gr.Clip = helperRegion;
                this.DoDrawArrow(gr, drawRect, type);
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

        protected virtual bool ShouldSerializeArrowImageLayout()
        {
            return (this._arrowImageLayout != ScrollBarButtonArrowImageLayout.Center);
        }

        protected virtual bool ShouldSerializeBorderStyle()
        {
            return (this._borderStyle != ScrollBarBorderStyle.Solid);
        }

        protected virtual bool ShouldSerializeGradientColor()
        {
            return Resco.Controls.ScrollBar.ScrollBar.ShouldSerializeGradientColor(this._gradientColor);
        }

        public System.Drawing.Color ArrowColor
        {
            get
            {
                return this._arrowColor;
            }
            set
            {
                if (this._arrowColor != value)
                {
                    this._arrowColor = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public System.Drawing.Image ArrowImage
        {
            get
            {
                return this._arrowImage;
            }
            set
            {
                if (this._arrowImage != value)
                {
                    this._arrowImage = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public ScrollBarButtonArrowImageLayout ArrowImageLayout
        {
            get
            {
                return this._arrowImageLayout;
            }
            set
            {
                if (this._arrowImageLayout != value)
                {
                    this._arrowImageLayout = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public System.Drawing.Color ArrowImageTransparentColor
        {
            get
            {
                return this._arrowImageTransparentColor;
            }
            set
            {
                if (this._arrowImageTransparentColor != value)
                {
                    this._arrowImageTransparentColor = value;
                    if (this._arrowImageTransparentColor == System.Drawing.Color.Transparent)
                    {
                        this._imageAttributes.ClearColorKey();
                    }
                    else
                    {
                        this._imageAttributes.SetColorKey(this._arrowImageTransparentColor, this._arrowImageTransparentColor);
                    }
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public bool BorderClosed
        {
            get
            {
                return this._borderClosed;
            }
            set
            {
                if (this._borderClosed != value)
                {
                    this._borderClosed = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
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

        public static ScrollBarButton Default
        {
            get
            {
                return _defaultNormal;
            }
        }

        public static ScrollBarButton DefaultDisabled
        {
            get
            {
                return _defaultDisabled;
            }
        }

        public static ScrollBarButton DefaultHighlight
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

        public enum ScrollBarButtonArrowImageLayout
        {
            Center,
            Stretch
        }

        public enum ScrollBarButtonDefaults
        {
            Normal,
            Highlight,
            Disabled
        }
    }
}

