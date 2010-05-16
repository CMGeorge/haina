namespace Resco.Controls.ScrollBar
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class ScrollBarTrack : Component
    {
        private System.Drawing.Color _borderColor;
        private ScrollBarBorderStyle _borderStyle;
        private System.Drawing.Color _color;
        private static ScrollBarTrack _defaultDisabled = new ScrollBarTrack(ScrollBarTrackDefaults.Disabled);
        private static ScrollBarTrack _defaultHighlight = new ScrollBarTrack(ScrollBarTrackDefaults.Highlight);
        private static ScrollBarTrack _defaultNormal = new ScrollBarTrack(ScrollBarTrackDefaults.Normal);
        private Resco.Controls.ScrollBar.GradientColor _gradientColor;
        private System.Drawing.Image _image;
        private ScrollBarTrackLayout _trackLayout;

        internal event EventHandler PropertyChanged;

        public ScrollBarTrack() : this(ScrollBarTrackDefaults.Normal)
        {
        }

        public ScrollBarTrack(ScrollBarTrackDefaults type)
        {
            switch (type)
            {
                case ScrollBarTrackDefaults.Normal:
                    this._color = SystemColors.Window;
                    break;

                case ScrollBarTrackDefaults.Highlight:
                    this._color = SystemColors.ControlText;
                    break;

                case ScrollBarTrackDefaults.Disabled:
                    this._color = SystemColors.Window;
                    break;
            }
            this._borderStyle = ScrollBarBorderStyle.Solid;
            this._borderColor = SystemColors.ControlText;
            this._gradientColor = new Resco.Controls.ScrollBar.GradientColor(FillDirection.Horizontal);
            this._gradientColor.PropertyChanged += new EventHandler(this.GradientColor_PropertyChanged);
            this._image = null;
            this._trackLayout = ScrollBarTrackLayout.FixedToTrack;
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
            }
            this._gradientColor = null;
            this._image = null;
            base.Dispose(disposing);
        }

        public void Draw(Graphics gr, Rectangle drawRect, Rectangle visibleRect, Rectangle trackRect, ScrollBarTrackType type, Resco.Controls.ScrollBar.ScrollBar.ScrollBarOrientation orientation, System.Drawing.Color parentColor)
        {
            if (this._borderStyle != ScrollBarBorderStyle.None)
            {
                Resco.Controls.ScrollBar.ScrollBar.BorderSide all = Resco.Controls.ScrollBar.ScrollBar.BorderSide.All;
                if (orientation == Resco.Controls.ScrollBar.ScrollBar.ScrollBarOrientation.Horizontal)
                {
                    //all &= ~((type == ScrollBarTrackType.Low) ? 4 : 1);
                    int temp = (int)all;
                    temp&= ~((type == ScrollBarTrackType.Low) ? 4 : 1);
                    all = (Resco.Controls.ScrollBar.ScrollBar.BorderSide)temp;
                }
                else
                {
                    //all &= ~((type == ScrollBarTrackType.Low) ? 8 : 2);
                    int temp = (int)all;
                    temp &= ~((type == ScrollBarTrackType.Low) ?  8 : 2);
                    all = (Resco.Controls.ScrollBar.ScrollBar.BorderSide)temp;
                }
                Resco.Controls.ScrollBar.ScrollBar.DoDrawBorder(gr, this._borderStyle, this._borderColor, drawRect, all, parentColor);
                drawRect.Inflate(-1, -1);
                if (orientation == Resco.Controls.ScrollBar.ScrollBar.ScrollBarOrientation.Horizontal)
                {
                    drawRect.Width++;
                    if (type == ScrollBarTrackType.High)
                    {
                        drawRect.X--;
                    }
                }
                else
                {
                    drawRect.Height++;
                    if (type == ScrollBarTrackType.High)
                    {
                        drawRect.Y--;
                    }
                }
            }
            if ((this._image != null) || this._gradientColor.CanDraw())
            {
                Rectangle destRect = drawRect;
                Region clip = null;
                if (this._trackLayout == ScrollBarTrackLayout.FixedToTrack)
                {
                    destRect = trackRect;
                    clip = gr.Clip;
                    Region helperRegion = Resco.Controls.ScrollBar.ScrollBar.GetHelperRegion();
                    helperRegion.MakeEmpty();
                    helperRegion.Union(drawRect);
                    gr.Clip = helperRegion;
                }
                if (this._image != null)
                {
                    gr.DrawImage(this._image, destRect, new Rectangle(0, 0, this._image.Width, this._image.Height), GraphicsUnit.Pixel);
                }
                else
                {
                    this._gradientColor.DrawGradient(gr, destRect);
                }
                if (this._trackLayout == ScrollBarTrackLayout.FixedToTrack)
                {
                    gr.Clip = clip;
                }
            }
            else
            {
                gr.FillRectangle(Resco.Controls.ScrollBar.ScrollBar.GetBrush(this._color), drawRect);
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

        protected virtual bool ShouldSerializeTrackLayout()
        {
            return (this._trackLayout != ScrollBarTrackLayout.FixedToTrack);
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

        public static ScrollBarTrack Default
        {
            get
            {
                return _defaultNormal;
            }
        }

        public static ScrollBarTrack DefaultDisabled
        {
            get
            {
                return _defaultDisabled;
            }
        }

        public static ScrollBarTrack DefaultHighlight
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

        public ScrollBarTrackLayout TrackLayout
        {
            get
            {
                return this._trackLayout;
            }
            set
            {
                if (this._trackLayout != value)
                {
                    this._trackLayout = value;
                    this.OnPropertyChanged(EventArgs.Empty);
                }
            }
        }

        public enum ScrollBarTrackDefaults
        {
            Normal,
            Highlight,
            Disabled
        }

        public enum ScrollBarTrackLayout
        {
            Stretched,
            FixedToTrack
        }
    }
}

