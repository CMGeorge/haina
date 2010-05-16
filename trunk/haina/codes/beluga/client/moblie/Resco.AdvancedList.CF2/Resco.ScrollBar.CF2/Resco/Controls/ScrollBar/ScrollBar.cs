namespace Resco.Controls.ScrollBar
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ScrollBar : Control
    {
        private ScrollBarArrowButtonsLayout _arrowButtonsLayout;
        private Bitmap _backBufferBitmap;
        private Graphics _backBufferGraphics;
        private Bitmap _backgroundBufferBitmap;
        private Graphics _backgroundBufferGraphics;
        private BackgroundValid _backgroundValid;
        internal static Pen _cachedPen;
        internal static SolidBrush _cachedSolidBrush;
        private ScrollBarHitRegion _clickAction;
        private ScrollBarExtensionBase _extension;
        private ScrollBarExtensionLocation _extensionLocation;
        private int _extensionSize;
        private bool _extensionVisible;
        internal static Bitmap _fakePixel;
        internal static Region _helperRegion;
        private ScrollBarTrack _highTrack;
        private ScrollBarTrack _highTrackDisabled;
        private ScrollBarTrack _highTrackHighlight;
        private int _largeChange;
        private ScrollBarButton _leftUpButton;
        private ScrollBarButton _leftUpButtonDisabled;
        private ScrollBarButton _leftUpButtonHighlight;
        private int _leftUpButtonSize;
        private ScrollBarTrack _lowTrack;
        private ScrollBarTrack _lowTrackDisabled;
        private ScrollBarTrack _lowTrackHighlight;
        private int _maximum;
        private int _maximumThumbSize;
        private int _minimum;
        private int _minimumThumbSize;
        private Timer _mouseTimer;
        private int _mouseX;
        private int _mouseY;
        private ScrollBarButton _rightDownButton;
        private ScrollBarButton _rightDownButtonDisabled;
        private ScrollBarButton _rightDownButtonHighlight;
        private int _rightDownButtonSize;
        private ScrollBarOrientation _scrollBarOrientation;
        private int _smallChange;
        private ScrollBarThumb _thumb;
        private ScrollBarThumb _thumbDisabled;
        private ScrollBarThumb _thumbHighlight;
        private MarginPadding _thumbMargins;
        private int _thumbPos;
        private ScrollBarTrackClickBehavior _trackClickBehavior;
        private int _value;

        public event DrawArrowButtonHandler DrawArrowButton;

        public event DrawThumbHandler DrawThumb;

        public event DrawTrackHandler DrawTrack;

        public event EventHandler MaximumReached;

        public event EventHandler MinimumReached;

        public event EventHandler ValueChanged;

        static ScrollBar()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(Resco.Controls.ScrollBar.ScrollBar), "");
            //}
        }

        protected ScrollBar(ScrollBarOrientation orientation)
        {
            this._scrollBarOrientation = orientation;
            this._mouseTimer = new Timer();
            this._mouseTimer.Tick += new EventHandler(this.MouseTimerTick);
            this._mouseTimer.Interval = 100;
            this._mouseTimer.Enabled = false;
            this._clickAction = ScrollBarHitRegion.None;
            this._minimum = 0;
            this._maximum = 100;
            this._largeChange = 10;
            this._smallChange = 1;
            this._value = 0;
            this._arrowButtonsLayout = ScrollBarArrowButtonsLayout.Edges;
            this._leftUpButton = null;
            this._leftUpButtonHighlight = null;
            this._leftUpButtonDisabled = null;
            this._leftUpButtonSize = 0x13;
            this._rightDownButton = null;
            this._rightDownButtonHighlight = null;
            this._rightDownButtonDisabled = null;
            this._rightDownButtonSize = 0x13;
            this._lowTrack = null;
            this._lowTrackHighlight = null;
            this._lowTrackDisabled = null;
            this._highTrack = null;
            this._highTrackHighlight = null;
            this._highTrackDisabled = null;
            this._trackClickBehavior = ScrollBarTrackClickBehavior.Scroll;
            this._thumb = null;
            this._thumbHighlight = null;
            this._thumbDisabled = null;
            this._minimumThumbSize = 11;
            this._maximumThumbSize = 0;
            this._thumbMargins = new MarginPadding();
            this._extension = null;
            this._extensionSize = 13;
            this._extensionLocation = ScrollBarExtensionLocation.LeftTop;
            this._extensionVisible = true;
            this._backgroundBufferBitmap = null;
            this._backgroundBufferGraphics = null;
            this._backgroundValid = BackgroundValid.Invalid;
            this._backBufferBitmap = null;
            this._backBufferGraphics = null;
            this.DrawTrack = null;
            this.DrawArrowButton = null;
            this.DrawThumb = null;
            this.ValueChanged = null;
            this.MinimumReached = null;
            this.MaximumReached = null;
        }

        internal static bool CreateBackBuffer(ref Graphics graphics, ref Bitmap bitmap, Size reqSize)
        {
            bool flag = true;
            if (((bitmap == null) || (bitmap.Width != reqSize.Width)) || (bitmap.Height != reqSize.Height))
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                    if (graphics != null)
                    {
                        graphics.Dispose();
                        graphics = null;
                    }
                }
                if ((reqSize.Width > 0) && (reqSize.Height > 0))
                {
                    bitmap = new Bitmap(reqSize.Width, reqSize.Height);
                }
                flag = false;
            }
            if (graphics == null)
            {
                if (bitmap != null)
                {
                    graphics = Graphics.FromImage(bitmap);
                }
                return flag;
            }
            if (bitmap == null)
            {
                graphics.Dispose();
                graphics = null;
            }
            return flag;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._leftUpButton != null)
                {
                    this._leftUpButton.PropertyChanged -= new EventHandler(this.LeftUpButton_PropertyChanged);
                }
                if (this._leftUpButtonHighlight != null)
                {
                    this._leftUpButtonHighlight.PropertyChanged -= new EventHandler(this.LeftUpButton_PropertyChanged);
                }
                if (this._leftUpButtonDisabled != null)
                {
                    this._leftUpButtonDisabled.PropertyChanged -= new EventHandler(this.LeftUpButton_PropertyChanged);
                }
                if (this._rightDownButton != null)
                {
                    this._rightDownButton.PropertyChanged -= new EventHandler(this.RightDownButton_PropertyChanged);
                }
                if (this._rightDownButtonHighlight != null)
                {
                    this._rightDownButtonHighlight.PropertyChanged -= new EventHandler(this.RightDownButton_PropertyChanged);
                }
                if (this._rightDownButtonDisabled != null)
                {
                    this._rightDownButtonDisabled.PropertyChanged -= new EventHandler(this.RightDownButton_PropertyChanged);
                }
                if (this._lowTrack != null)
                {
                    this._lowTrack.PropertyChanged -= new EventHandler(this.LowTrack_PropertyChanged);
                }
                if (this._lowTrackHighlight != null)
                {
                    this._lowTrackHighlight.PropertyChanged -= new EventHandler(this.LowTrack_PropertyChanged);
                }
                if (this._lowTrackDisabled != null)
                {
                    this._lowTrackDisabled.PropertyChanged -= new EventHandler(this.LowTrack_PropertyChanged);
                }
                if (this._highTrack != null)
                {
                    this._highTrack.PropertyChanged -= new EventHandler(this.HighTrack_PropertyChanged);
                }
                if (this._highTrackHighlight != null)
                {
                    this._highTrackHighlight.PropertyChanged -= new EventHandler(this.HighTrack_PropertyChanged);
                }
                if (this._highTrackDisabled != null)
                {
                    this._highTrackDisabled.PropertyChanged -= new EventHandler(this.HighTrack_PropertyChanged);
                }
                if (this._thumb != null)
                {
                    this._thumb.PropertyChanged -= new EventHandler(this.Thumb_PropertyChanged);
                }
                if (this._thumbHighlight != null)
                {
                    this._thumbHighlight.PropertyChanged -= new EventHandler(this.Thumb_PropertyChanged);
                }
                if (this._thumbDisabled != null)
                {
                    this._thumbDisabled.PropertyChanged -= new EventHandler(this.Thumb_PropertyChanged);
                }
                if (this._extension != null)
                {
                    this._extension.PropertyChanged -= new ScrollBarExtensionBase.PropertyChangedHandler(this.Extension_PropertyChanged);
                    this.ValueChanged = (EventHandler) Delegate.Remove(this.ValueChanged, new EventHandler(this._extension.ScrollBar_ValueChanged));
                    this._extension.Parent = null;
                }
                if (this._mouseTimer != null)
                {
                    this._mouseTimer.Enabled = false;
                    this._mouseTimer.Dispose();
                }
                if (_cachedPen != null)
                {
                    _cachedPen.Dispose();
                }
                if (_cachedSolidBrush != null)
                {
                    _cachedSolidBrush.Dispose();
                }
                if (_fakePixel != null)
                {
                    _fakePixel.Dispose();
                }
                if (_helperRegion != null)
                {
                    _helperRegion.Dispose();
                }
                if (this._backgroundBufferGraphics != null)
                {
                    this._backgroundBufferGraphics.Dispose();
                }
                if (this._backgroundBufferBitmap != null)
                {
                    this._backgroundBufferBitmap.Dispose();
                }
                if (this._backBufferGraphics != null)
                {
                    this._backBufferGraphics.Dispose();
                }
                if (this._backBufferBitmap != null)
                {
                    this._backBufferBitmap.Dispose();
                }
            }
            this._leftUpButton = null;
            this._leftUpButtonHighlight = null;
            this._leftUpButtonDisabled = null;
            this._rightDownButton = null;
            this._rightDownButtonHighlight = null;
            this._rightDownButtonDisabled = null;
            this._lowTrack = null;
            this._lowTrackHighlight = null;
            this._lowTrackDisabled = null;
            this._highTrack = null;
            this._highTrackHighlight = null;
            this._highTrackDisabled = null;
            this._thumb = null;
            this._thumbHighlight = null;
            this._thumbDisabled = null;
            this._mouseTimer = null;
            _cachedPen = null;
            _cachedSolidBrush = null;
            _fakePixel = null;
            _helperRegion = null;
            this._backgroundBufferBitmap = null;
            this._backgroundBufferGraphics = null;
            this._backBufferGraphics = null;
            this._backBufferBitmap = null;
            base.Dispose(disposing);
        }

        private void DoDrawArrowButton(Graphics gr, Rectangle drawRect, ScrollBarArrow type, ScrollBarElementState state)
        {
            ScrollBarButton leftUpButton = null;
            if ((type == ScrollBarArrow.Left) || (type == ScrollBarArrow.Up))
            {
                switch (state)
                {
                    case ScrollBarElementState.Normal:
                        leftUpButton = this.LeftUpButton;
                        goto Label_006B;

                    case ScrollBarElementState.Pressed:
                        leftUpButton = this.LeftUpButtonHighlight;
                        goto Label_006B;

                    case ScrollBarElementState.Disabled:
                        leftUpButton = this.LeftUpButtonDisabled;
                        goto Label_006B;
                }
            }
            else
            {
                switch (state)
                {
                    case ScrollBarElementState.Normal:
                        leftUpButton = this.RightDownButton;
                        goto Label_006B;

                    case ScrollBarElementState.Pressed:
                        leftUpButton = this.RightDownButtonHighlight;
                        goto Label_006B;

                    case ScrollBarElementState.Disabled:
                        leftUpButton = this.RightDownButtonDisabled;
                        goto Label_006B;
                }
            }
        Label_006B:
            if (leftUpButton == null)
            {
                switch (state)
                {
                    case ScrollBarElementState.Normal:
                        leftUpButton = ScrollBarButton.Default;
                        break;

                    case ScrollBarElementState.Pressed:
                        leftUpButton = ScrollBarButton.DefaultHighlight;
                        break;

                    case ScrollBarElementState.Disabled:
                        leftUpButton = ScrollBarButton.DefaultDisabled;
                        break;
                }
            }
            Color window = SystemColors.Window;
            if (base.Parent != null)
            {
                window = base.Parent.BackColor;
            }
            leftUpButton.Draw(gr, drawRect, type, window);
        }

        private void DoDrawArrowsAndTrack(PaintEventArgs e, Rectangle leftUpArrow, Rectangle rightDownArrow, Rectangle track, Rectangle lowTrack, Rectangle lowTrackVisible, Rectangle highTrack, Rectangle highTrackVisible, Rectangle thumb)
        {
            PaintEventArgs args = null;
            Graphics backgroundBuffer = this.GetBackgroundBuffer();
            BackgroundValid valid = this._backgroundValid;
            if (backgroundBuffer != null)
            {
                if (this._backgroundValid != BackgroundValid.Valid)
                {
                    args = new PaintEventArgs(backgroundBuffer, new Rectangle(0, 0, this._backgroundBufferBitmap.Width, this._backgroundBufferBitmap.Height));
                    if (((this._extension != null) && this._extensionVisible) && (this._extensionLocation == ScrollBarExtensionLocation.LeftTop))
                    {
                        int extensionSize = this.ExtensionSize;
                        if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                        {
                            leftUpArrow.Y -= extensionSize;
                            rightDownArrow.Y -= extensionSize;
                            track.Y -= extensionSize;
                            lowTrack.Y -= extensionSize;
                            lowTrackVisible.Y -= extensionSize;
                            highTrack.Y -= extensionSize;
                            highTrackVisible.Y -= extensionSize;
                        }
                        else
                        {
                            leftUpArrow.X -= extensionSize;
                            rightDownArrow.X -= extensionSize;
                            track.X -= extensionSize;
                            lowTrack.X -= extensionSize;
                            lowTrackVisible.X -= extensionSize;
                            highTrack.X -= extensionSize;
                            highTrackVisible.X -= extensionSize;
                        }
                    }
                }
            }
            else
            {
                args = e;
            }
            if (valid != BackgroundValid.Valid)
            {
                if ((((valid & BackgroundValid.LeftUpArrowInvalid) == BackgroundValid.LeftUpArrowInvalid) && (leftUpArrow.Width > 0)) && (leftUpArrow.Height > 0))
                {
                    this.OnDrawArrowButton(new DrawArrowButtonEventArgs(args.Graphics, leftUpArrow, base.Enabled ? ((this._clickAction == ScrollBarHitRegion.LeftUpArrow) ? ScrollBarElementState.Pressed : ScrollBarElementState.Normal) : ScrollBarElementState.Disabled, (this._scrollBarOrientation == ScrollBarOrientation.Horizontal) ? ScrollBarArrow.Left : ScrollBarArrow.Up));
                }
                if ((((valid & BackgroundValid.RightDownArrowInvalid) == BackgroundValid.RightDownArrowInvalid) && (rightDownArrow.Width > 0)) && (rightDownArrow.Height > 0))
                {
                    this.OnDrawArrowButton(new DrawArrowButtonEventArgs(args.Graphics, rightDownArrow, base.Enabled ? ((this._clickAction == ScrollBarHitRegion.RightDownArrow) ? ScrollBarElementState.Pressed : ScrollBarElementState.Normal) : ScrollBarElementState.Disabled, (this._scrollBarOrientation == ScrollBarOrientation.Horizontal) ? ScrollBarArrow.Right : ScrollBarArrow.Down));
                }
                if ((valid & BackgroundValid.LowTrackInvalid) == BackgroundValid.LowTrackInvalid)
                {
                    this.OnDrawTrack(new DrawTrackEventArgs(args.Graphics, lowTrack, base.Enabled ? ((this._clickAction == ScrollBarHitRegion.TrackLow) ? ScrollBarElementState.Pressed : ScrollBarElementState.Normal) : ScrollBarElementState.Disabled, lowTrackVisible, track, ScrollBarTrackType.Low));
                }
                if ((valid & BackgroundValid.HighTrackInvalid) == BackgroundValid.HighTrackInvalid)
                {
                    this.OnDrawTrack(new DrawTrackEventArgs(args.Graphics, highTrack, base.Enabled ? ((this._clickAction == ScrollBarHitRegion.TrackHigh) ? ScrollBarElementState.Pressed : ScrollBarElementState.Normal) : ScrollBarElementState.Disabled, highTrackVisible, track, ScrollBarTrackType.High));
                }
                this._backgroundValid = (args != e) ? BackgroundValid.Valid : BackgroundValid.Invalid;
            }
            if (args != e)
            {
                int x = 0;
                int y = 0;
                if (((this._extension != null) && this._extensionVisible) && (this._extensionLocation == ScrollBarExtensionLocation.LeftTop))
                {
                    if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                    {
                        y += this.ExtensionSize;
                    }
                    else if (this._scrollBarOrientation == ScrollBarOrientation.Vertical)
                    {
                        x += this.ExtensionSize;
                    }
                }
                e.Graphics.DrawImage(this._backgroundBufferBitmap, x, y);
            }
        }

        internal static void DoDrawBorder(Graphics gr, ScrollBarBorderStyle style, Color c, Rectangle rect, BorderSide sides, Color parentColor)
        {
            if (((style != ScrollBarBorderStyle.None) && (rect.Width > 0)) && ((rect.Height > 0) && (sides != BorderSide.None)))
            {
                Pen pen = GetPen(c);
                if ((rect.Width == 1) && (rect.Height == 1))
                {
                    DrawPixel(gr, c, rect.Left, rect.Top);
                }
                else if (rect.Width == 1)
                {
                    if ((sides & (BorderSide.Right | BorderSide.Left)) != BorderSide.None)
                    {
                        gr.DrawLine(pen, rect.Left, rect.Top, rect.Left, rect.Bottom - 1);
                    }
                }
                else if (rect.Height == 1)
                {
                    if ((sides & (BorderSide.Down | BorderSide.Up)) != BorderSide.None)
                    {
                        gr.DrawLine(pen, rect.Left, rect.Top, rect.Right - 1, rect.Top);
                    }
                }
                else
                {
                    int left = rect.Left;
                    int top = rect.Top;
                    int num3 = rect.Right - 1;
                    int num4 = rect.Bottom - 1;
                    if ((style == ScrollBarBorderStyle.Solid) || ((style == ScrollBarBorderStyle.Rounded) && ((rect.Width == 2) || (rect.Height == 2))))
                    {
                        if ((sides & BorderSide.Left) == BorderSide.Left)
                        {
                            gr.DrawLine(pen, left, top, left, num4);
                        }
                        if ((sides & BorderSide.Up) == BorderSide.Up)
                        {
                            gr.DrawLine(pen, left, top, num3, top);
                        }
                        if ((sides & BorderSide.Right) == BorderSide.Right)
                        {
                            gr.DrawLine(pen, num3, top, num3, num4);
                        }
                        if ((sides & BorderSide.Down) == BorderSide.Down)
                        {
                            gr.DrawLine(pen, left, num4, num3, num4);
                        }
                    }
                    else
                    {
                        if (rect.Width == 3)
                        {
                            if ((sides & BorderSide.Up) == BorderSide.Up)
                            {
                                if ((sides & BorderSide.Left) == BorderSide.None)
                                {
                                    gr.DrawLine(pen, left, top, left + 1, top);
                                }
                                else if ((sides & BorderSide.Right) == BorderSide.None)
                                {
                                    gr.DrawLine(pen, left + 1, top, num3, top);
                                }
                                else
                                {
                                    DrawPixel(gr, c, left + 1, top);
                                }
                            }
                            if ((sides & BorderSide.Down) == BorderSide.Down)
                            {
                                if ((sides & BorderSide.Left) == BorderSide.None)
                                {
                                    gr.DrawLine(pen, left, num4, left + 1, num4);
                                }
                                else if ((sides & BorderSide.Right) == BorderSide.None)
                                {
                                    gr.DrawLine(pen, left + 1, num4, num3, num4);
                                }
                                else
                                {
                                    DrawPixel(gr, c, left + 1, num4);
                                }
                            }
                        }
                        else
                        {
                            if ((sides & BorderSide.Up) == BorderSide.Up)
                            {
                                gr.DrawLine(pen, left + (((sides & BorderSide.Left) == BorderSide.Left) ? 1 : 0), top, num3 - (((sides & BorderSide.Right) == BorderSide.Right) ? 1 : 0), top);
                            }
                            if ((sides & BorderSide.Down) == BorderSide.Down)
                            {
                                gr.DrawLine(pen, left + (((sides & BorderSide.Left) == BorderSide.Left) ? 1 : 0), num4, num3 - (((sides & BorderSide.Right) == BorderSide.Right) ? 1 : 0), num4);
                            }
                        }
                        if (rect.Height == 3)
                        {
                            if ((sides & BorderSide.Left) == BorderSide.Left)
                            {
                                if ((sides & BorderSide.Up) == BorderSide.None)
                                {
                                    gr.DrawLine(pen, left, top, left, top + 1);
                                }
                                else if ((sides & BorderSide.Down) == BorderSide.None)
                                {
                                    gr.DrawLine(pen, left, top + 1, left, num4);
                                }
                                else
                                {
                                    DrawPixel(gr, c, left, top + 1);
                                }
                            }
                            if ((sides & BorderSide.Right) == BorderSide.Right)
                            {
                                if ((sides & BorderSide.Up) == BorderSide.None)
                                {
                                    gr.DrawLine(pen, num3, top, num3, top + 1);
                                }
                                else if ((sides & BorderSide.Down) == BorderSide.None)
                                {
                                    gr.DrawLine(pen, num3, top + 1, num3, num4);
                                }
                                else
                                {
                                    DrawPixel(gr, c, num3, top + 1);
                                }
                            }
                        }
                        else
                        {
                            if ((sides & BorderSide.Left) == BorderSide.Left)
                            {
                                gr.DrawLine(pen, left, top + (((sides & BorderSide.Up) == BorderSide.Up) ? 1 : 0), left, num4 - (((sides & BorderSide.Down) == BorderSide.Down) ? 1 : 0));
                            }
                            if ((sides & BorderSide.Right) == BorderSide.Right)
                            {
                                gr.DrawLine(pen, num3, top + (((sides & BorderSide.Up) == BorderSide.Up) ? 1 : 0), num3, num4 - (((sides & BorderSide.Down) == BorderSide.Down) ? 1 : 0));
                            }
                        }
                        if (parentColor != Color.Transparent)
                        {
                            if ((sides & (BorderSide.Up | BorderSide.Left)) == (BorderSide.Up | BorderSide.Left))
                            {
                                DrawPixel(gr, parentColor, left, top);
                            }
                            if ((sides & (BorderSide.Right | BorderSide.Up)) == (BorderSide.Right | BorderSide.Up))
                            {
                                DrawPixel(gr, parentColor, num3, top);
                            }
                            if ((sides & (BorderSide.Down | BorderSide.Left)) == (BorderSide.Down | BorderSide.Left))
                            {
                                DrawPixel(gr, parentColor, left, num4);
                            }
                            if ((sides & (BorderSide.Down | BorderSide.Right)) == (BorderSide.Down | BorderSide.Right))
                            {
                                DrawPixel(gr, parentColor, num3, num4);
                            }
                        }
                    }
                }
            }
        }

        private void DoDrawLine(Graphics gr, Pen pen, int x1, int y1, int x2, int y2)
        {
            gr.DrawLine(pen, x1, y1, x2, y2);
        }

        private void DoDrawThumb(Graphics gr, Rectangle drawRect, ScrollBarElementState state)
        {
            ScrollBarThumb thumbHighlight = null;
            switch (state)
            {
                case ScrollBarElementState.Normal:
                    thumbHighlight = this.Thumb;
                    break;

                case ScrollBarElementState.Pressed:
                    thumbHighlight = this.ThumbHighlight;
                    break;

                case ScrollBarElementState.Disabled:
                    thumbHighlight = this.ThumbDisabled;
                    break;
            }
            if (thumbHighlight == null)
            {
                switch (state)
                {
                    case ScrollBarElementState.Normal:
                        thumbHighlight = ScrollBarThumb.Default;
                        break;

                    case ScrollBarElementState.Pressed:
                        thumbHighlight = ScrollBarThumb.DefaultHighlight;
                        break;
                }
            }
            if (thumbHighlight != null)
            {
                thumbHighlight.Draw(gr, drawRect, this._scrollBarOrientation);
            }
        }

        private void DoDrawTrack(Graphics gr, Rectangle drawRect, Rectangle visibleRect, Rectangle trackRect, ScrollBarElementState state, ScrollBarTrackType type)
        {
            ScrollBarTrack defaultHighlight = null;
            switch (state)
            {
                case ScrollBarElementState.Normal:
                    defaultHighlight = (type == ScrollBarTrackType.Low) ? this.LowTrack : this.HighTrack;
                    break;

                case ScrollBarElementState.Pressed:
                    defaultHighlight = (type == ScrollBarTrackType.Low) ? this.LowTrackHighlight : this.HighTrackHighlight;
                    break;

                case ScrollBarElementState.Disabled:
                    defaultHighlight = (type == ScrollBarTrackType.Low) ? this.LowTrackDisabled : this.HighTrackDisabled;
                    break;
            }
            if (defaultHighlight == null)
            {
                switch (state)
                {
                    case ScrollBarElementState.Normal:
                        defaultHighlight = ScrollBarTrack.Default;
                        break;

                    case ScrollBarElementState.Pressed:
                        defaultHighlight = ScrollBarTrack.DefaultHighlight;
                        break;

                    case ScrollBarElementState.Disabled:
                        defaultHighlight = ScrollBarTrack.DefaultDisabled;
                        break;
                }
            }
            Color window = SystemColors.Window;
            if (base.Parent != null)
            {
                window = base.Parent.BackColor;
            }
            defaultHighlight.Draw(gr, drawRect, visibleRect, trackRect, type, this._scrollBarOrientation, window);
        }

        internal static void DrawPixel(Graphics gr, Color c, int x, int y)
        {
            if (_fakePixel == null)
            {
                _fakePixel = new Bitmap(1, 1);
            }
            _fakePixel.SetPixel(0, 0, c);
            gr.DrawImage(_fakePixel, x, y);
        }

        private void Extension_NeedChangeValue(object sender, ScrollBarExtensionBase.ValueIndexConversionEventArgs e)
        {
            this.Value = e.Parameter;
        }

        private void Extension_PropertyChanged(object sender, ScrollBarExtensionBase.PropertyChangedEventArgs e)
        {
            this.HandleChange(ScrollBarChangeType.ExtensionPropertyChanged);
        }

        private Graphics GetBackBuffer()
        {
            CreateBackBuffer(ref this._backBufferGraphics, ref this._backBufferBitmap, base.Size);
            return this._backBufferGraphics;
        }

        private Graphics GetBackgroundBuffer()
        {
            Size reqSize = base.Size;
            if ((this._extension != null) && this._extensionVisible)
            {
                if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                {
                    reqSize.Height -= this.ExtensionSize;
                }
                else
                {
                    reqSize.Width -= this.ExtensionSize;
                }
            }
            if (!CreateBackBuffer(ref this._backgroundBufferGraphics, ref this._backgroundBufferBitmap, reqSize))
            {
                this._backgroundValid = BackgroundValid.Invalid;
            }
            return this._backgroundBufferGraphics;
        }

        internal static SolidBrush GetBrush(Color c)
        {
            if (_cachedSolidBrush == null)
            {
                _cachedSolidBrush = new SolidBrush(c);
            }
            else
            {
                _cachedSolidBrush.Color = c;
            }
            return _cachedSolidBrush;
        }

        private void GetElementRectangles(out Rectangle leftUpArrow, out Rectangle rightBottomArrow, out Rectangle track, out Rectangle lowTrack, out Rectangle lowTrackVisible, out Rectangle highTrack, out Rectangle highTrackVisible, out Rectangle thumb)
        {
            int num3;
            leftUpArrow = Rectangle.Empty;
            rightBottomArrow = Rectangle.Empty;
            lowTrack = Rectangle.Empty;
            lowTrackVisible = Rectangle.Empty;
            highTrack = Rectangle.Empty;
            highTrackVisible = Rectangle.Empty;
            track = Rectangle.Empty;
            thumb = Rectangle.Empty;
            Rectangle scrollBarRect = this.GetScrollBarRect();
            int width = this._leftUpButtonSize;
            int num2 = this._rightDownButtonSize;
            if (this._arrowButtonsLayout != ScrollBarArrowButtonsLayout.Hidden)
            {
                if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                {
                    num3 = scrollBarRect.Width - (width + num2);
                    if (num3 < 0)
                    {
                        width += num3 / 2;
                        num2 = scrollBarRect.Width - width;
                    }
                    int left = scrollBarRect.Left;
                    int x = scrollBarRect.Right - num2;
                    if (this._arrowButtonsLayout == ScrollBarArrowButtonsLayout.Beginning)
                    {
                        x = left + width;
                    }
                    else if (this._arrowButtonsLayout == ScrollBarArrowButtonsLayout.End)
                    {
                        left = x - width;
                    }
                    leftUpArrow = new Rectangle(left, scrollBarRect.Top, width, scrollBarRect.Height);
                    rightBottomArrow = new Rectangle(x, scrollBarRect.Top, num2, scrollBarRect.Height);
                    if (num3 < 0)
                    {
                        return;
                    }
                    track = new Rectangle(leftUpArrow.Right, scrollBarRect.Top, num3, scrollBarRect.Height);
                    if (this._arrowButtonsLayout == ScrollBarArrowButtonsLayout.Beginning)
                    {
                        track.X = rightBottomArrow.Right;
                    }
                    else if (this._arrowButtonsLayout == ScrollBarArrowButtonsLayout.End)
                    {
                        track.X = scrollBarRect.Left;
                    }
                }
                else
                {
                    num3 = scrollBarRect.Height - (width + num2);
                    if (num3 < 0)
                    {
                        width += num3 / 2;
                        num2 = scrollBarRect.Height - width;
                    }
                    int top = scrollBarRect.Top;
                    int y = scrollBarRect.Bottom - num2;
                    if (this._arrowButtonsLayout == ScrollBarArrowButtonsLayout.Beginning)
                    {
                        y = top + width;
                    }
                    else if (this._arrowButtonsLayout == ScrollBarArrowButtonsLayout.End)
                    {
                        top = y - width;
                    }
                    leftUpArrow = new Rectangle(scrollBarRect.Left, top, scrollBarRect.Width, width);
                    rightBottomArrow = new Rectangle(scrollBarRect.Left, y, scrollBarRect.Width, num2);
                    if (num3 < 0)
                    {
                        return;
                    }
                    track = new Rectangle(scrollBarRect.Left, leftUpArrow.Bottom, scrollBarRect.Width, num3);
                    if (this._arrowButtonsLayout == ScrollBarArrowButtonsLayout.Beginning)
                    {
                        track.Y = rightBottomArrow.Bottom;
                    }
                    else if (this._arrowButtonsLayout == ScrollBarArrowButtonsLayout.End)
                    {
                        track.Y = scrollBarRect.Top;
                    }
                }
            }
            else
            {
                track = scrollBarRect;
                num3 = (this._scrollBarOrientation == ScrollBarOrientation.Horizontal) ? track.Width : track.Height;
            }
            int num8 = num3 / 2;
            int largeChange = this.LargeChange;
            if ((num3 >= this.MinimumThumbSize) && (largeChange > 0))
            {
                int num10 = (this.Maximum - this.Minimum) + 1;
                int minimumThumbSize = (largeChange * num3) / num10;
                if (minimumThumbSize < this.MinimumThumbSize)
                {
                    minimumThumbSize = this.MinimumThumbSize;
                }
                if ((this.MaximumThumbSize > 0) && (minimumThumbSize > this.MaximumThumbSize))
                {
                    minimumThumbSize = this.MaximumThumbSize;
                }
                if (minimumThumbSize > num3)
                {
                    minimumThumbSize = num3;
                }
                int num12 = 0;
                int num13 = num10 - largeChange;
                if (num13 > 0)
                {
                    if (this._clickAction == ScrollBarHitRegion.Thumb)
                    {
                        num12 = this._thumbPos;
                    }
                    else
                    {
                        num12 = ((num3 - minimumThumbSize) * this.Value) / num13;
                    }
                }
                if ((num12 + minimumThumbSize) > num3)
                {
                    num12 = num3 - minimumThumbSize;
                    if (this._clickAction == ScrollBarHitRegion.Thumb)
                    {
                        this._thumbPos = num12;
                    }
                }
                if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                {
                    thumb = new Rectangle(track.Left + num12, track.Top, minimumThumbSize, track.Height);
                }
                else
                {
                    thumb = new Rectangle(track.Left, track.Top + num12, track.Width, minimumThumbSize);
                }
                num8 = num12 + (minimumThumbSize / 2);
            }
            if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
            {
                lowTrack = new Rectangle(track.Left, track.Top, num8, track.Height);
                lowTrackVisible = new Rectangle(track.Left, track.Top, lowTrack.Width - (thumb.Width / 2), track.Height);
                highTrack = new Rectangle(lowTrack.Right, track.Top, num3 - lowTrack.Width, track.Height);
                highTrackVisible = new Rectangle(lowTrackVisible.Right + thumb.Width, track.Top, (num3 - lowTrackVisible.Width) - thumb.Width, track.Height);
            }
            else
            {
                lowTrack = new Rectangle(track.Left, track.Top, track.Width, num8);
                lowTrackVisible = new Rectangle(track.Left, track.Top, track.Width, lowTrack.Height - (thumb.Height / 2));
                highTrack = new Rectangle(track.Left, lowTrack.Bottom, track.Width, num3 - lowTrack.Height);
                highTrackVisible = new Rectangle(track.Left, lowTrackVisible.Bottom + thumb.Height, track.Width, (num3 - lowTrackVisible.Height) - thumb.Height);
            }
        }

        private Rectangle GetExtensionRect()
        {
            Rectangle rectangle = new Rectangle(0, 0, base.Width, base.Height);
            if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
            {
                rectangle.Height = this.ExtensionSize;
                if (this._extensionLocation == ScrollBarExtensionLocation.RightBottom)
                {
                    rectangle.Y = base.Height - rectangle.Height;
                }
                return rectangle;
            }
            if (this._scrollBarOrientation == ScrollBarOrientation.Vertical)
            {
                rectangle.Width = this.ExtensionSize;
                if (this._extensionLocation == ScrollBarExtensionLocation.RightBottom)
                {
                    rectangle.X = base.Width - rectangle.Width;
                }
            }
            return rectangle;
        }

        internal static Region GetHelperRegion()
        {
            if (_helperRegion == null)
            {
                _helperRegion = new Region();
            }
            return _helperRegion;
        }

        internal static Pen GetPen(Color c)
        {
            if (_cachedPen == null)
            {
                _cachedPen = new Pen(c);
            }
            else
            {
                _cachedPen.Color = c;
            }
            return _cachedPen;
        }

        private Rectangle GetScrollBarRect()
        {
            Rectangle rectangle = new Rectangle(0, 0, base.Width, base.Height);
            if ((this._extension != null) && this._extensionVisible)
            {
                int extensionSize = this.ExtensionSize;
                if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                {
                    rectangle.Height -= extensionSize;
                    if (this._extensionLocation == ScrollBarExtensionLocation.LeftTop)
                    {
                        rectangle.Y += extensionSize;
                    }
                    return rectangle;
                }
                if (this._scrollBarOrientation == ScrollBarOrientation.Vertical)
                {
                    rectangle.Width -= extensionSize;
                    if (this._extensionLocation == ScrollBarExtensionLocation.LeftTop)
                    {
                        rectangle.X += extensionSize;
                    }
                }
            }
            return rectangle;
        }

        protected void HandleChange(ScrollBarChangeType type)
        {
            switch (type)
            {
                case ScrollBarChangeType.LowTrackPropertyChanged:
                    this._backgroundValid |= BackgroundValid.LowTrackInvalid;
                    break;

                case ScrollBarChangeType.HighTrackPropertyChanged:
                    this._backgroundValid |= BackgroundValid.HighTrackInvalid;
                    break;

                case ScrollBarChangeType.NeedFullRedraw:
                    this._backgroundValid |= BackgroundValid.Invalid;
                    break;

                case ScrollBarChangeType.ScrollBarPropertyChanged:
                    this._backgroundValid |= BackgroundValid.HighTrackInvalid | BackgroundValid.LowTrackInvalid;
                    break;

                case ScrollBarChangeType.LeftUpArrowPropertyChanged:
                    this._backgroundValid |= BackgroundValid.LeftUpArrowInvalid;
                    break;

                case ScrollBarChangeType.RightDownArrowPropertyChanged:
                    this._backgroundValid |= BackgroundValid.RightDownArrowInvalid;
                    break;
            }
            base.Invalidate();
        }

        private void HighTrack_PropertyChanged(object sender, EventArgs e)
        {
            this.HandleChange(ScrollBarChangeType.HighTrackPropertyChanged);
        }

        private ScrollBarHitRegion HitTest(int x, int y, out int thumbPos)
        {
            Rectangle rectangle;
            Rectangle rectangle2;
            Rectangle rectangle3;
            Rectangle rectangle4;
            Rectangle rectangle5;
            Rectangle rectangle6;
            Rectangle rectangle7;
            Rectangle rectangle8;
            thumbPos = 0;
            if (((this._extension != null) && this._extensionVisible) && this.GetExtensionRect().Contains(x, y))
            {
                return ScrollBarHitRegion.Extension;
            }
            this.GetElementRectangles(out rectangle, out rectangle2, out rectangle3, out rectangle4, out rectangle5, out rectangle6, out rectangle7, out rectangle8);
            if (rectangle.Contains(x, y))
            {
                return ScrollBarHitRegion.LeftUpArrow;
            }
            if (rectangle2.Contains(x, y))
            {
                return ScrollBarHitRegion.RightDownArrow;
            }
            if (rectangle8.Contains(x, y))
            {
                if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                {
                    thumbPos = rectangle8.Left - rectangle4.Left;
                }
                else if (this._scrollBarOrientation == ScrollBarOrientation.Vertical)
                {
                    thumbPos = rectangle8.Top - rectangle4.Top;
                }
                return ScrollBarHitRegion.Thumb;
            }
            if (rectangle5.Contains(x, y))
            {
                return ScrollBarHitRegion.TrackLow;
            }
            if (rectangle7.Contains(x, y))
            {
                return ScrollBarHitRegion.TrackHigh;
            }
            return ScrollBarHitRegion.None;
        }

        private void LeftUpButton_PropertyChanged(object sender, EventArgs e)
        {
            this.HandleChange(ScrollBarChangeType.LeftUpArrowPropertyChanged);
        }

        private void LowTrack_PropertyChanged(object sender, EventArgs e)
        {
            this.HandleChange(ScrollBarChangeType.LowTrackPropertyChanged);
        }

        private void MouseTimerTick(object sender, EventArgs e)
        {
            switch (this._clickAction)
            {
                case ScrollBarHitRegion.LeftUpArrow:
                    this.Value -= this.SmallChange;
                    return;

                case ScrollBarHitRegion.RightDownArrow:
                    this.Value += this.SmallChange;
                    return;

                case ScrollBarHitRegion.TrackLow:
                    this.Value -= this.LargeChange;
                    return;

                case ScrollBarHitRegion.TrackHigh:
                    this.Value += this.LargeChange;
                    return;
            }
        }

        protected internal virtual void OnDrawArrowButton(DrawArrowButtonEventArgs e)
        {
            if (this.DrawArrowButton != null)
            {
                this.DrawArrowButton(this, e);
            }
            else
            {
                this.DoDrawArrowButton(e.Graphics, e.Bounds, e.Type, e.State);
            }
        }

        protected internal virtual void OnDrawThumb(DrawEventArgs e)
        {
            if (this.DrawThumb != null)
            {
                this.DrawThumb(this, e);
            }
            else
            {
                this.DoDrawThumb(e.Graphics, e.Bounds, e.State);
            }
        }

        protected internal virtual void OnDrawTrack(DrawTrackEventArgs e)
        {
            if (this.DrawTrack != null)
            {
                this.DrawTrack(this, e);
            }
            else
            {
                this.DoDrawTrack(e.Graphics, e.Bounds, e.VisibleRectangle, e.TrackRectangle, e.State, e.TrackType);
            }
        }

        protected internal virtual void OnMaximumReached(EventArgs e)
        {
            if (this.MaximumReached != null)
            {
                this.MaximumReached(this, e);
            }
        }

        protected internal virtual void OnMinimumReached(EventArgs e)
        {
            if (this.MinimumReached != null)
            {
                this.MinimumReached(this, e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this._clickAction = this.HitTest(e.X, e.Y, out this._thumbPos);
            if (((this._clickAction == ScrollBarHitRegion.TrackLow) || (this._clickAction == ScrollBarHitRegion.TrackHigh)) && (this._trackClickBehavior == ScrollBarTrackClickBehavior.Jump))
            {
                this._mouseX = base.Left - 1;
                this._mouseY = base.Top - 1;
                this._clickAction = ScrollBarHitRegion.Thumb;
                this.SetValueToMousePosition(e.X, e.Y);
                this._backgroundValid |= BackgroundValid.HighTrackInvalid | BackgroundValid.LowTrackInvalid;
                this._mouseX = e.X;
                this._mouseY = e.Y;
            }
            else
            {
                switch (this._clickAction)
                {
                    case ScrollBarHitRegion.LeftUpArrow:
                        this.Value -= this.SmallChange;
                        this._mouseTimer.Enabled = true;
                        this._backgroundValid |= BackgroundValid.LeftUpArrowInvalid;
                        goto Label_01B7;

                    case ScrollBarHitRegion.RightDownArrow:
                        this.Value += this.SmallChange;
                        this._mouseTimer.Enabled = true;
                        this._backgroundValid |= BackgroundValid.RightDownArrowInvalid;
                        goto Label_01B7;

                    case ScrollBarHitRegion.TrackLow:
                        this.Value -= this.LargeChange;
                        this._mouseTimer.Enabled = true;
                        this._backgroundValid |= BackgroundValid.LowTrackInvalid;
                        goto Label_01B7;

                    case ScrollBarHitRegion.TrackHigh:
                        this.Value += this.LargeChange;
                        this._mouseTimer.Enabled = true;
                        this._backgroundValid |= BackgroundValid.HighTrackInvalid;
                        goto Label_01B7;

                    case ScrollBarHitRegion.Thumb:
                        this._mouseX = e.X;
                        this._mouseY = e.Y;
                        goto Label_01B7;

                    case ScrollBarHitRegion.Extension:
                        if (this._extension != null)
                        {
                            this._extension.Mouse(ScrollBarExtensionBase.MouseAction.Down, e);
                        }
                        goto Label_01B7;
                }
            }
        Label_01B7:
            base.OnMouseDown(e);
            base.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this._clickAction == ScrollBarHitRegion.Thumb)
            {
                this.SetValueToMousePosition(e.X, e.Y);
                this._mouseX = e.X;
                this._mouseY = e.Y;
                base.Invalidate();
            }
            else if ((this._extension != null) && (this._clickAction == ScrollBarHitRegion.Extension))
            {
                this._extension.Mouse(ScrollBarExtensionBase.MouseAction.Move, e);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this._mouseTimer.Enabled = false;
            switch (this._clickAction)
            {
                case ScrollBarHitRegion.LeftUpArrow:
                    this._backgroundValid |= BackgroundValid.LeftUpArrowInvalid;
                    break;

                case ScrollBarHitRegion.RightDownArrow:
                    this._backgroundValid |= BackgroundValid.RightDownArrowInvalid;
                    break;

                case ScrollBarHitRegion.TrackLow:
                    this._backgroundValid |= BackgroundValid.LowTrackInvalid;
                    break;

                case ScrollBarHitRegion.TrackHigh:
                    this._backgroundValid |= BackgroundValid.HighTrackInvalid;
                    break;

                case ScrollBarHitRegion.Extension:
                    if (this._extension != null)
                    {
                        this._extension.Mouse(ScrollBarExtensionBase.MouseAction.Up, e);
                    }
                    break;
            }
            this._clickAction = ScrollBarHitRegion.None;
            base.OnMouseUp(e);
            base.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            PaintEventArgs args = null;
            Rectangle rectangle;
            Rectangle rectangle2;
            Rectangle rectangle3;
            Rectangle rectangle4;
            Rectangle rectangle5;
            Rectangle rectangle6;
            Rectangle rectangle7;
            Rectangle rectangle8;
            Graphics backBuffer = this.GetBackBuffer();
            if (backBuffer != null)
            {
                args = new PaintEventArgs(backBuffer, new Rectangle(0, 0, base.Width, base.Height));
            }
            else
            {
                args = e;
            }
            this.GetElementRectangles(out rectangle, out rectangle2, out rectangle3, out rectangle4, out rectangle5, out rectangle6, out rectangle7, out rectangle8);
            this.DoDrawArrowsAndTrack(args, rectangle, rectangle2, rectangle3, rectangle4, rectangle5, rectangle6, rectangle7, rectangle8);
            if (base.Enabled)
            {
                rectangle8.Offset(this._thumbMargins.Left, this._thumbMargins.Top);
                rectangle8.Width -= this._thumbMargins.Left + this._thumbMargins.Right;
                rectangle8.Height -= this._thumbMargins.Top + this._thumbMargins.Bottom;
                this.OnDrawThumb(new DrawEventArgs(args.Graphics, rectangle8, (this._clickAction == ScrollBarHitRegion.Thumb) ? ScrollBarElementState.Pressed : ScrollBarElementState.Normal));
            }
            if ((this._extension != null) && this._extensionVisible)
            {
                Color window = SystemColors.Window;
                if (base.Parent != null)
                {
                    window = base.Parent.BackColor;
                }
                this._extension.Draw(args.Graphics, window);
            }
            if (args != e)
            {
                e.Graphics.DrawImage(this._backBufferBitmap, 0, 0);
            }
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if ((this._extension != null) && this._extensionVisible)
            {
                this._extension.Bounds = this.GetExtensionRect();
            }
        }

        protected internal virtual void OnValueChanged(EventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        private void RightDownButton_PropertyChanged(object sender, EventArgs e)
        {
            this.HandleChange(ScrollBarChangeType.RightDownArrowPropertyChanged);
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            float width = factor.Width;
            if (width != 1f)
            {
                if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                {
                    this.LeftUpButtonSize = (int) (this.LeftUpButtonSize * width);
                    this.RightDownButtonSize = (int) (this.RightDownButtonSize * width);
                    this.MinimumThumbSize = (int) (this.MinimumThumbSize * width);
                    this.MaximumThumbSize = (int) (this.MaximumThumbSize * width);
                }
                else if (this._scrollBarOrientation == ScrollBarOrientation.Vertical)
                {
                    this._extensionSize = (int) (this._extensionSize * width);
                }
                this.ThumbMargins = new MarginPadding((int) (this.ThumbMargins.Left * width), this.ThumbMargins.Top, (int) (this.ThumbMargins.Right * width), this.ThumbMargins.Bottom);
            }
            float height = factor.Height;
            if (height != 1f)
            {
                if (this._scrollBarOrientation == ScrollBarOrientation.Vertical)
                {
                    this.LeftUpButtonSize = (int) (this.LeftUpButtonSize * height);
                    this.RightDownButtonSize = (int) (this.RightDownButtonSize * height);
                    this.MinimumThumbSize = (int) (this.MinimumThumbSize * height);
                    this.MaximumThumbSize = (int) (this.MaximumThumbSize * height);
                }
                else if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                {
                    this._extensionSize = (int) (this._extensionSize * height);
                }
                this.ThumbMargins = new MarginPadding(this.ThumbMargins.Left, (int) (this.ThumbMargins.Top * height), this.ThumbMargins.Right, (int) (this.ThumbMargins.Bottom * height));
            }
            base.ScaleControl(factor, specified);
        }

        private void SetupExtension(int oldSize, int newSize, ScrollBarExtensionLocation oldLocation, ScrollBarExtensionLocation newLocation)
        {
            Rectangle rectangle = new Rectangle(base.Location.X, base.Location.Y, base.Width, base.Height);
            if (newLocation != oldLocation)
            {
                if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                {
                    rectangle.Y += (newLocation == ScrollBarExtensionLocation.LeftTop) ? -oldSize : oldSize;
                }
                else
                {
                    rectangle.X += (newLocation == ScrollBarExtensionLocation.LeftTop) ? -oldSize : oldSize;
                }
            }
            if (newSize != oldSize)
            {
                int num = newSize - oldSize;
                if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                {
                    rectangle.Height += num;
                    if (newLocation == ScrollBarExtensionLocation.LeftTop)
                    {
                        rectangle.Y -= num;
                    }
                }
                else
                {
                    rectangle.Width += num;
                    if (newLocation == ScrollBarExtensionLocation.LeftTop)
                    {
                        rectangle.X -= num;
                    }
                }
            }
            base.Bounds = rectangle;
        }

        private void SetValueToMousePosition(int x, int y)
        {
            Rectangle rectangle;
            Rectangle rectangle2;
            Rectangle rectangle3;
            Rectangle rectangle4;
            Rectangle rectangle5;
            Rectangle rectangle6;
            Rectangle rectangle7;
            Rectangle rectangle8;
            this.GetElementRectangles(out rectangle, out rectangle2, out rectangle3, out rectangle4, out rectangle5, out rectangle6, out rectangle7, out rectangle8);
            if (rectangle3.Contains(x, y))
            {
                if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                {
                    this._thumbPos += x - this._mouseX;
                }
                else if (this._scrollBarOrientation == ScrollBarOrientation.Vertical)
                {
                    this._thumbPos += y - this._mouseY;
                }
                if (this._thumbPos < 0)
                {
                    this._thumbPos = 0;
                }
            }
            if (this._arrowButtonsLayout == ScrollBarArrowButtonsLayout.Edges)
            {
                if (rectangle.Contains(x, y))
                {
                    this.Value = this.Minimum;
                    this._thumbPos = 0;
                }
                else if (rectangle2.Contains(x, y))
                {
                    this.Value = (this.Maximum - this.LargeChange) + 1;
                    if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                    {
                        this._thumbPos = rectangle3.Width - rectangle8.Width;
                    }
                    else
                    {
                        this._thumbPos = rectangle3.Height - rectangle8.Height;
                    }
                }
            }
            if (rectangle3.Contains(x, y))
            {
                int largeChange = this.LargeChange;
                int num2 = (this.Maximum - this.Minimum) + 1;
                int height = 1;
                int num4 = num2 - largeChange;
                int width = 1;
                if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                {
                    width = rectangle3.Width;
                    height = rectangle8.Width;
                }
                else
                {
                    width = rectangle3.Height;
                    height = rectangle8.Height;
                }
                if (!rectangle3.Contains(this._mouseX, this._mouseY))
                {
                    if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                    {
                        this._thumbPos = (x - rectangle3.Left) - (rectangle8.Width / 2);
                    }
                    else
                    {
                        this._thumbPos = (y - rectangle3.Top) - (rectangle8.Height / 2);
                    }
                    if (this._thumbPos < 0)
                    {
                        this._thumbPos = 0;
                    }
                }
                int num6 = width - height;
                if (num6 > 0)
                {
                    int num7 = (num6 / num4) / 2;
                    this.Value = ((this._thumbPos + num7) * num4) / num6;
                }
                else
                {
                    this.Value = 0;
                }
            }
        }

        protected virtual bool ShouldSerializeArrowButtonsLayout()
        {
            return (this._arrowButtonsLayout != ScrollBarArrowButtonsLayout.Edges);
        }

        protected virtual bool ShouldSerializeExtension()
        {
            return (this.Extension != null);
        }

        internal static bool ShouldSerializeGradientColor(GradientColor gc)
        {
            bool flag = gc.StartColor != Color.Transparent;
            bool flag2 = gc.EndColor != Color.Transparent;
            bool flag3 = gc.MiddleColor1 != Color.Transparent;
            bool flag4 = gc.MiddleColor2 != Color.Transparent;
            bool flag5 = (gc.MiddleColor1Offset != 50) || (gc.MiddleColor2Offset != 50);
            if (((!flag && !flag2) && (!flag3 && !flag4)) && !flag5)
            {
                return (gc.FillDirection != FillDirection.Horizontal);
            }
            return true;
        }

        protected virtual bool ShouldSerializeHighTrack()
        {
            return (this.HighTrack != null);
        }

        protected virtual bool ShouldSerializeHighTrackDisabled()
        {
            return (this.HighTrackDisabled != null);
        }

        protected virtual bool ShouldSerializeHighTrackHighlight()
        {
            return (this.HighTrackHighlight != null);
        }

        protected virtual bool ShouldSerializeLowTrack()
        {
            return (this.LowTrack != null);
        }

        protected virtual bool ShouldSerializeLowTrackDisabled()
        {
            return (this.LowTrackDisabled != null);
        }

        protected virtual bool ShouldSerializeLowTrackHighlight()
        {
            return (this.LowTrackHighlight != null);
        }

        protected virtual bool ShouldSerializeThumb()
        {
            return (this.Thumb != null);
        }

        protected virtual bool ShouldSerializeThumbDisabled()
        {
            return (this.ThumbDisabled != null);
        }

        protected virtual bool ShouldSerializeThumbHighlight()
        {
            return (this.ThumbHighlight != null);
        }

        public bool ShouldSerializeThumbMargins()
        {
            return (this._thumbMargins != MarginPadding.Default);
        }

        protected virtual bool ShouldSerializeTrackClickBehavior()
        {
            return (this._trackClickBehavior != ScrollBarTrackClickBehavior.Scroll);
        }

        private void Thumb_PropertyChanged(object sender, EventArgs e)
        {
            this.HandleChange(ScrollBarChangeType.ThumbPropertyChanged);
        }

        public override string ToString()
        {
            return string.Format("Value = {0} Min = {1} Max = {2}", this.Value, this.Minimum, this.Maximum);
        }

        public ScrollBarArrowButtonsLayout ArrowButtonsLayout
        {
            get
            {
                return this._arrowButtonsLayout;
            }
            set
            {
                if (this._arrowButtonsLayout != value)
                {
                    this._arrowButtonsLayout = value;
                    this.HandleChange(ScrollBarChangeType.NeedFullRedraw);
                }
            }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
            }
        }

        public ScrollBarExtensionBase Extension
        {
            get
            {
                return this._extension;
            }
            set
            {
                if (this._extension != value)
                {
                    base.SuspendLayout();
                    if ((value == null) && this._extensionVisible)
                    {
                        this.SetupExtension(this.ExtensionSize, 0, this.ExtensionLocationInControl, this.ExtensionLocationInControl);
                    }
                    if (this._extension != null)
                    {
                        this._extension.PropertyChanged -= new ScrollBarExtensionBase.PropertyChangedHandler(this.Extension_PropertyChanged);
                        this._extension.NeedChangeValue -= new ScrollBarExtensionBase.NeedChangeValueHandler(this.Extension_NeedChangeValue);
                        this.ValueChanged = (EventHandler) Delegate.Remove(this.ValueChanged, new EventHandler(this._extension.ScrollBar_ValueChanged));
                    }
                    else if (this._extensionVisible)
                    {
                        this.SetupExtension(0, this.ExtensionSize, this.ExtensionLocationInControl, this.ExtensionLocationInControl);
                    }
                    this._extension = value;
                    if (this._extension != null)
                    {
                        this._extension.Parent = this;
                        this._extension.PropertyChanged += new ScrollBarExtensionBase.PropertyChangedHandler(this.Extension_PropertyChanged);
                        this._extension.NeedChangeValue += new ScrollBarExtensionBase.NeedChangeValueHandler(this.Extension_NeedChangeValue);
                        this.ValueChanged = (EventHandler) Delegate.Combine(this.ValueChanged, new EventHandler(this._extension.ScrollBar_ValueChanged));
                        this._extension.Bounds = this.GetExtensionRect();
                        if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                        {
                            this._extension.Location = (this._extensionLocation == ScrollBarExtensionLocation.LeftTop) ? ScrollBarExtensionBase.ComponentLocation.Top : ScrollBarExtensionBase.ComponentLocation.Bottom;
                        }
                        else
                        {
                            this._extension.Location = (this._extensionLocation == ScrollBarExtensionLocation.LeftTop) ? ScrollBarExtensionBase.ComponentLocation.Left : ScrollBarExtensionBase.ComponentLocation.Right;
                        }
                    }
                    this.HandleChange(ScrollBarChangeType.ExtensionPropertyChanged);
                    base.ResumeLayout();
                }
            }
        }

        protected ScrollBarExtensionLocation ExtensionLocationInControl
        {
            get
            {
                return this._extensionLocation;
            }
            set
            {
                if (this._extensionLocation != value)
                {
                    base.SuspendLayout();
                    if (this._extension != null)
                    {
                        this.SetupExtension(this.ExtensionSize, this.ExtensionSize, this._extensionLocation, value);
                    }
                    this._extensionLocation = value;
                    if (this._extension != null)
                    {
                        this._extension.Bounds = this.GetExtensionRect();
                        if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                        {
                            this._extension.Location = (this._extensionLocation == ScrollBarExtensionLocation.LeftTop) ? ScrollBarExtensionBase.ComponentLocation.Top : ScrollBarExtensionBase.ComponentLocation.Bottom;
                        }
                        else
                        {
                            this._extension.Location = (this._extensionLocation == ScrollBarExtensionLocation.LeftTop) ? ScrollBarExtensionBase.ComponentLocation.Left : ScrollBarExtensionBase.ComponentLocation.Right;
                        }
                    }
                    this.HandleChange(ScrollBarChangeType.ExtensionPropertyChanged);
                    base.ResumeLayout();
                }
            }
        }

        protected int ExtensionSize
        {
            get
            {
                int height = this._extensionSize;
                if (this._extensionVisible)
                {
                    if (this._scrollBarOrientation == ScrollBarOrientation.Horizontal)
                    {
                        if (base.Height < height)
                        {
                            height = base.Height;
                        }
                        return height;
                    }
                    if (base.Width < height)
                    {
                        height = base.Width;
                    }
                }
                return height;
            }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                if (this._extensionSize != value)
                {
                    base.SuspendLayout();
                    if (this._extension != null)
                    {
                        this.SetupExtension(this._extensionSize, value, this.ExtensionLocationInControl, this.ExtensionLocationInControl);
                    }
                    this._extensionSize = value;
                    if (this._extension != null)
                    {
                        this._extension.Bounds = this.GetExtensionRect();
                    }
                    this.HandleChange(ScrollBarChangeType.ExtensionPropertyChanged);
                    base.ResumeLayout();
                }
            }
        }

        public bool ExtensionVisible
        {
            get
            {
                return this._extensionVisible;
            }
            set
            {
                if (this._extensionVisible != value)
                {
                    if (this._extension != null)
                    {
                        if (!value)
                        {
                            this.SetupExtension(this.ExtensionSize, 0, this.ExtensionLocationInControl, this.ExtensionLocationInControl);
                        }
                        else
                        {
                            this.SetupExtension(0, this.ExtensionSize, this.ExtensionLocationInControl, this.ExtensionLocationInControl);
                            this._extension.Bounds = this.GetExtensionRect();
                        }
                    }
                    this._extensionVisible = value;
                    if (this._extensionVisible)
                    {
                        this._extension.ScrollBar_ValueChanged(this, EventArgs.Empty);
                    }
                    this.HandleChange(ScrollBarChangeType.NeedFullRedraw);
                }
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
            }
        }

        public ScrollBarTrack HighTrack
        {
            get
            {
                return this._highTrack;
            }
            set
            {
                if (this._highTrack != value)
                {
                    if (this._highTrack != null)
                    {
                        this._highTrack.PropertyChanged -= new EventHandler(this.HighTrack_PropertyChanged);
                    }
                    this._highTrack = value;
                    if (this._highTrack != null)
                    {
                        this._highTrack.PropertyChanged += new EventHandler(this.HighTrack_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.HighTrackPropertyChanged);
                }
            }
        }

        public ScrollBarTrack HighTrackDisabled
        {
            get
            {
                return this._highTrackDisabled;
            }
            set
            {
                if (this._highTrackDisabled != value)
                {
                    if (this._highTrackDisabled != null)
                    {
                        this._highTrackDisabled.PropertyChanged -= new EventHandler(this.HighTrack_PropertyChanged);
                    }
                    this._highTrackDisabled = value;
                    if (this._highTrackDisabled != null)
                    {
                        this._highTrackDisabled.PropertyChanged += new EventHandler(this.HighTrack_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.HighTrackPropertyChanged);
                }
            }
        }

        public ScrollBarTrack HighTrackHighlight
        {
            get
            {
                return this._highTrackHighlight;
            }
            set
            {
                if (this._highTrackHighlight != value)
                {
                    if (this._highTrackHighlight != null)
                    {
                        this._highTrackHighlight.PropertyChanged -= new EventHandler(this.HighTrack_PropertyChanged);
                    }
                    this._highTrackHighlight = value;
                    if (this._highTrackHighlight != null)
                    {
                        this._highTrackHighlight.PropertyChanged += new EventHandler(this.HighTrack_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.HighTrackPropertyChanged);
                }
            }
        }

        public int LargeChange
        {
            get
            {
                int num = (this._maximum - this._minimum) + 1;
                if (this._largeChange >= num)
                {
                    return num;
                }
                return this._largeChange;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (this._largeChange != value)
                {
                    this._largeChange = value;
                    this.HandleChange(ScrollBarChangeType.ScrollBarPropertyChanged);
                }
            }
        }

        protected ScrollBarButton LeftUpButton
        {
            get
            {
                return this._leftUpButton;
            }
            set
            {
                if (this._leftUpButton != value)
                {
                    if (this._leftUpButton != null)
                    {
                        this._leftUpButton.PropertyChanged -= new EventHandler(this.LeftUpButton_PropertyChanged);
                    }
                    this._leftUpButton = value;
                    if (this._leftUpButton != null)
                    {
                        this._leftUpButton.PropertyChanged += new EventHandler(this.LeftUpButton_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.LeftUpArrowPropertyChanged);
                }
            }
        }

        protected ScrollBarButton LeftUpButtonDisabled
        {
            get
            {
                return this._leftUpButtonDisabled;
            }
            set
            {
                if (this._leftUpButtonDisabled != value)
                {
                    if (this._leftUpButtonDisabled != null)
                    {
                        this._leftUpButtonDisabled.PropertyChanged -= new EventHandler(this.LeftUpButton_PropertyChanged);
                    }
                    this._leftUpButtonDisabled = value;
                    if (this._leftUpButtonDisabled != null)
                    {
                        this._leftUpButtonDisabled.PropertyChanged += new EventHandler(this.LeftUpButton_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.LeftUpArrowPropertyChanged);
                }
            }
        }

        protected ScrollBarButton LeftUpButtonHighlight
        {
            get
            {
                return this._leftUpButtonHighlight;
            }
            set
            {
                if (this._leftUpButtonHighlight != value)
                {
                    if (this._leftUpButtonHighlight != null)
                    {
                        this._leftUpButtonHighlight.PropertyChanged -= new EventHandler(this.LeftUpButton_PropertyChanged);
                    }
                    this._leftUpButtonHighlight = value;
                    if (this._leftUpButtonHighlight != null)
                    {
                        this._leftUpButtonHighlight.PropertyChanged += new EventHandler(this.LeftUpButton_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.LeftUpArrowPropertyChanged);
                }
            }
        }

        protected int LeftUpButtonSize
        {
            get
            {
                return this._leftUpButtonSize;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (this._leftUpButtonSize != value)
                {
                    this._leftUpButtonSize = value;
                    this.HandleChange(ScrollBarChangeType.NeedFullRedraw);
                }
            }
        }

        public ScrollBarTrack LowTrack
        {
            get
            {
                return this._lowTrack;
            }
            set
            {
                if (this._lowTrack != value)
                {
                    if (this._lowTrack != null)
                    {
                        this._lowTrack.PropertyChanged -= new EventHandler(this.LowTrack_PropertyChanged);
                    }
                    this._lowTrack = value;
                    if (this._lowTrack != null)
                    {
                        this._lowTrack.PropertyChanged += new EventHandler(this.LowTrack_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.LowTrackPropertyChanged);
                }
            }
        }

        public ScrollBarTrack LowTrackDisabled
        {
            get
            {
                return this._lowTrackDisabled;
            }
            set
            {
                if (this._lowTrackDisabled != value)
                {
                    if (this._lowTrackDisabled != null)
                    {
                        this._lowTrackDisabled.PropertyChanged -= new EventHandler(this.LowTrack_PropertyChanged);
                    }
                    this._lowTrackDisabled = value;
                    if (this._lowTrackDisabled != null)
                    {
                        this._lowTrackDisabled.PropertyChanged += new EventHandler(this.LowTrack_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.LowTrackPropertyChanged);
                }
            }
        }

        public ScrollBarTrack LowTrackHighlight
        {
            get
            {
                return this._lowTrackHighlight;
            }
            set
            {
                if (this._lowTrackHighlight != value)
                {
                    if (this._lowTrackHighlight != null)
                    {
                        this._lowTrackHighlight.PropertyChanged -= new EventHandler(this.LowTrack_PropertyChanged);
                    }
                    this._lowTrackHighlight = value;
                    if (this._lowTrackHighlight != null)
                    {
                        this._lowTrackHighlight.PropertyChanged += new EventHandler(this.LowTrack_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.LowTrackPropertyChanged);
                }
            }
        }

        public int Maximum
        {
            get
            {
                return this._maximum;
            }
            set
            {
                if (value < this._minimum)
                {
                    this._minimum = value;
                }
                if (value < this._value)
                {
                    this._value = value;
                }
                if (this._maximum != value)
                {
                    this._maximum = value;
                    this.HandleChange(ScrollBarChangeType.ScrollBarPropertyChanged);
                }
            }
        }

        protected int MaximumThumbSize
        {
            get
            {
                return this._maximumThumbSize;
            }
            set
            {
                if (value < 4)
                {
                    value = 0;
                }
                if ((value > 0) && (value < this._minimumThumbSize))
                {
                    this._minimumThumbSize = value;
                }
                if (this._maximumThumbSize != value)
                {
                    this._maximumThumbSize = value;
                    this.HandleChange(ScrollBarChangeType.ThumbPropertyChanged);
                }
            }
        }

        public int Minimum
        {
            get
            {
                return this._minimum;
            }
            set
            {
                if (value > this._maximum)
                {
                    this._maximum = value;
                }
                if (value > this._value)
                {
                    this._value = value;
                }
                if (this._minimum != value)
                {
                    this._minimum = value;
                    this.HandleChange(ScrollBarChangeType.ScrollBarPropertyChanged);
                }
            }
        }

        protected int MinimumThumbSize
        {
            get
            {
                return this._minimumThumbSize;
            }
            set
            {
                if (value < 4)
                {
                    value = 4;
                }
                if ((this._maximumThumbSize > 0) && (value > this._maximumThumbSize))
                {
                    this._maximumThumbSize = value;
                }
                if (this._minimumThumbSize != value)
                {
                    this._minimumThumbSize = value;
                    this.HandleChange(ScrollBarChangeType.ThumbPropertyChanged);
                }
            }
        }

        protected ScrollBarButton RightDownButton
        {
            get
            {
                return this._rightDownButton;
            }
            set
            {
                if (this._rightDownButton != value)
                {
                    if (this._rightDownButton != null)
                    {
                        this._rightDownButton.PropertyChanged -= new EventHandler(this.RightDownButton_PropertyChanged);
                    }
                    this._rightDownButton = value;
                    if (this._rightDownButton != null)
                    {
                        this._rightDownButton.PropertyChanged += new EventHandler(this.RightDownButton_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.RightDownArrowPropertyChanged);
                }
            }
        }

        protected ScrollBarButton RightDownButtonDisabled
        {
            get
            {
                return this._rightDownButtonDisabled;
            }
            set
            {
                if (this._rightDownButtonDisabled != value)
                {
                    if (this._rightDownButtonDisabled != null)
                    {
                        this._rightDownButtonDisabled.PropertyChanged -= new EventHandler(this.RightDownButton_PropertyChanged);
                    }
                    this._rightDownButtonDisabled = value;
                    if (this._rightDownButtonDisabled != null)
                    {
                        this._rightDownButtonDisabled.PropertyChanged += new EventHandler(this.RightDownButton_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.RightDownArrowPropertyChanged);
                }
            }
        }

        protected ScrollBarButton RightDownButtonHighlight
        {
            get
            {
                return this._rightDownButtonHighlight;
            }
            set
            {
                if (this._rightDownButtonHighlight != value)
                {
                    if (this._rightDownButtonHighlight != null)
                    {
                        this._rightDownButtonHighlight.PropertyChanged -= new EventHandler(this.RightDownButton_PropertyChanged);
                    }
                    this._rightDownButtonHighlight = value;
                    if (this._rightDownButtonHighlight != null)
                    {
                        this._rightDownButtonHighlight.PropertyChanged += new EventHandler(this.RightDownButton_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.RightDownArrowPropertyChanged);
                }
            }
        }

        protected int RightDownButtonSize
        {
            get
            {
                return this._rightDownButtonSize;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (this._rightDownButtonSize != value)
                {
                    this._rightDownButtonSize = value;
                    this.HandleChange(ScrollBarChangeType.NeedFullRedraw);
                }
            }
        }

        public int SmallChange
        {
            get
            {
                if (this._smallChange >= this.LargeChange)
                {
                    return this.LargeChange;
                }
                return this._smallChange;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (this._smallChange != value)
                {
                    this._smallChange = value;
                    this.HandleChange(ScrollBarChangeType.ScrollBarPropertySmallChangeChanged);
                }
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
            }
        }

        public ScrollBarThumb Thumb
        {
            get
            {
                return this._thumb;
            }
            set
            {
                if (this._thumb != value)
                {
                    if (this._thumb != null)
                    {
                        this._thumb.PropertyChanged -= new EventHandler(this.Thumb_PropertyChanged);
                    }
                    this._thumb = value;
                    if (this._thumb != null)
                    {
                        this._thumb.PropertyChanged += new EventHandler(this.Thumb_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.ThumbPropertyChanged);
                }
            }
        }

        public ScrollBarThumb ThumbDisabled
        {
            get
            {
                return this._thumbDisabled;
            }
            set
            {
                if (this._thumbDisabled != value)
                {
                    if (this._thumbDisabled != null)
                    {
                        this._thumbDisabled.PropertyChanged -= new EventHandler(this.Thumb_PropertyChanged);
                    }
                    this._thumbDisabled = value;
                    if (this._thumbDisabled != null)
                    {
                        this._thumbDisabled.PropertyChanged += new EventHandler(this.Thumb_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.ThumbPropertyChanged);
                }
            }
        }

        public ScrollBarThumb ThumbHighlight
        {
            get
            {
                return this._thumbHighlight;
            }
            set
            {
                if (this._thumbHighlight != value)
                {
                    if (this._thumbHighlight != null)
                    {
                        this._thumbHighlight.PropertyChanged -= new EventHandler(this.Thumb_PropertyChanged);
                    }
                    this._thumbHighlight = value;
                    if (this._thumbHighlight != null)
                    {
                        this._thumbHighlight.PropertyChanged += new EventHandler(this.Thumb_PropertyChanged);
                    }
                    this.HandleChange(ScrollBarChangeType.ThumbPropertyChanged);
                }
            }
        }

        public MarginPadding ThumbMargins
        {
            get
            {
                return this._thumbMargins;
            }
            set
            {
                bool flag = false;
                int left = value.Left;
                if (left < 0)
                {
                    left = 0;
                    flag = true;
                }
                int right = value.Right;
                if (right < 0)
                {
                    right = 0;
                    flag = true;
                }
                int top = value.Top;
                if (top < 0)
                {
                    top = 0;
                    flag = true;
                }
                int bottom = value.Bottom;
                if (bottom < 0)
                {
                    bottom = 0;
                    flag = true;
                }
                if (flag)
                {
                    value = new MarginPadding(left, top, right, bottom);
                }
                if (this._thumbMargins != value)
                {
                    this._thumbMargins = value;
                    this.HandleChange(ScrollBarChangeType.ThumbPropertyChanged);
                }
            }
        }

        public ScrollBarTrackClickBehavior TrackClickBehavior
        {
            get
            {
                return this._trackClickBehavior;
            }
            set
            {
                if (this._trackClickBehavior != value)
                {
                    this._trackClickBehavior = value;
                }
            }
        }

        public int Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if (value > ((this._maximum - this.LargeChange) + 1))
                {
                    value = (this._maximum - this.LargeChange) + 1;
                }
                if (value < this._minimum)
                {
                    value = this._minimum;
                }
                if (value != this._value)
                {
                    this._value = value;
                    this.OnValueChanged(EventArgs.Empty);
                    if (this._value == this.Minimum)
                    {
                        this.OnMinimumReached(EventArgs.Empty);
                    }
                    if (this._value >= ((this.Maximum - this.LargeChange) + 1))
                    {
                        this.OnMaximumReached(EventArgs.Empty);
                    }
                    this.HandleChange(ScrollBarChangeType.ScrollBarPropertyChanged);
                }
            }
        }

        private enum BackgroundValid
        {
            HighTrackInvalid = 8,
            Invalid = 15,
            LeftUpArrowInvalid = 1,
            LowTrackInvalid = 4,
            RightDownArrowInvalid = 2,
            Valid = 0
        }

        internal enum BorderSide
        {
            All = 15,
            Down = 8,
            Left = 1,
            None = 0,
            Right = 4,
            Up = 2
        }

        public delegate void DrawArrowButtonHandler(object sender, DrawArrowButtonEventArgs e);

        public delegate void DrawThumbHandler(object sender, DrawEventArgs e);

        public delegate void DrawTrackHandler(object sender, DrawTrackEventArgs e);

        protected enum ScrollBarChangeType
        {
            ExtensionPropertyChanged = 0x80,
            HighTrackPropertyChanged = 0x20,
            LeftUpArrowPropertyChanged = 4,
            LowTrackPropertyChanged = 0x10,
            NeedFullRedraw = 0x10000,
            RightDownArrowPropertyChanged = 8,
            ScrollBarPropertyChanged = 1,
            ScrollBarPropertySmallChangeChanged = 2,
            ThumbPropertyChanged = 0x40
        }

        private enum ScrollBarHitRegion
        {
            None,
            LeftUpArrow,
            RightDownArrow,
            TrackLow,
            TrackHigh,
            Thumb,
            Extension
        }

        public enum ScrollBarOrientation
        {
            Horizontal,
            Vertical
        }
    }
}

