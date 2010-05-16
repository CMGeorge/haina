namespace Resco.Controls.ImageBox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ImageBox : UserControl
    {
        private SolidBrush _backColorBrush;
        private SolidBrush _cropBrush;
        private CropDragPoint _cropDragPoint;
        private Pen _cropPen;
        private static int _cropRectSize;
        private DrawArgs _drawArgs = new DrawArgs();
        private Resco.Controls.ImageBox.DrawingMode _drawgingMode;
        private HScrollBar _hScrollBar;
        private Resco.Controls.ImageBox.CompactImage _image;
        private Size _imageSize = Size.Empty;
        private MouseAction _mouseAction;
        private Point _mousePreviousPos = Point.Empty;
        private Resco.Controls.ImageBox.PenMode _penMode;
        private Resco.Controls.ImageBox.ScrollBarMode _scrollBarMode;
        private int _scrollBarWidth = 13;
        private int _suspendRedraw;
        private Size _visibleSize = Size.Empty;
        private VScrollBar _vScrollBar;

        static ImageBox()
        {
            int num;
            _cropRectSize = 5;
            try
            {
                Native.RIL_ColorToCOLORREF(Color.Black);
                num = (int) ((((double) GetDeviceCaps(IntPtr.Zero, 0x58)) / 96.0) + 0.5);
            }
            catch
            {
                num = 1;
            }
            _cropRectSize = 5 * num;
        }

        public ImageBox()
        {
            this._drawgingMode = this._drawArgs.DrawingMode;
            this._backColorBrush = new SolidBrush(this.BackColor);
            this._cropPen = new Pen(Color.WhiteSmoke);
            this._cropBrush = new SolidBrush(Color.WhiteSmoke);
            this._vScrollBar = new VScrollBar();
            this._vScrollBar.Visible = false;
            this._vScrollBar.Bounds = new Rectangle(base.Width - this._scrollBarWidth, 0, this._scrollBarWidth, (base.Height - this._scrollBarWidth) + 1);
            this._vScrollBar.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            this._vScrollBar.ValueChanged += new EventHandler(this._vScrollBar_ValueChanged);
            base.Controls.Add(this._vScrollBar);
            this._hScrollBar = new HScrollBar();
            this._hScrollBar.Visible = false;
            this._hScrollBar.Bounds = new Rectangle(0, base.Height - this._scrollBarWidth, base.Width, this._scrollBarWidth);
            this._hScrollBar.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this._hScrollBar.ValueChanged += new EventHandler(this._hScrollBar_ValueChanged);
            base.Controls.Add(this._hScrollBar);
            this.UpdateScrollBars();
            this._drawArgs.BackgroundColor = this.BackColor;
        }

        private void _hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            int x = -this._hScrollBar.Value;
            if (x > 0)
            {
                x = 0;
            }
            else if (x < (this._visibleSize.Width - this._imageSize.Width))
            {
                x = this._visibleSize.Width - this._imageSize.Width;
            }
            if (this._drawArgs.Origin.X != x)
            {
                this.SuspendRedraw();
                this._drawArgs.OffsetCropBounds(x - this._drawArgs.Origin.X, 0);
                this._drawArgs.Origin = new Point(x, this._drawArgs.Origin.Y);
                this.ResumeRedraw();
            }
        }

        private void _image_Changed(object sender, EventArgs e)
        {
            this.SuspendRedraw();
            if (this._image == null)
            {
                this._imageSize = new Size(0, 0);
            }
            else
            {
                if (this._drawArgs.Zoom <= 0.0)
                {
                    this._drawArgs.Zoom = this._image.CalculateZoom(this, this._drawArgs);
                }
                else
                {
                    this._drawArgs.Zoom = this._image.CalculateZoom(this, this._drawArgs);
                }
                if ((this._drawArgs.Rotation == Resco.Controls.ImageBox.Rotation.Left90) || (this._drawArgs.Rotation == Resco.Controls.ImageBox.Rotation.Left270))
                {
                    this._imageSize = new Size((int) Math.Ceiling((double) (this._drawArgs.Zoom * this._image.Size.Height)), (int) Math.Ceiling((double) (this._drawArgs.Zoom * this._image.Size.Width)));
                }
                else
                {
                    this._imageSize = new Size((int) Math.Ceiling((double) (this._drawArgs.Zoom * this._image.Size.Width)), (int) Math.Ceiling((double) (this._drawArgs.Zoom * this._image.Size.Height)));
                }
            }
            this.UpdateScrollBars();
            this.ResumeRedraw();
        }

        private void _vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            int y = -this._vScrollBar.Value;
            if (y > 0)
            {
                y = 0;
            }
            else if (y < (this._visibleSize.Height - this._imageSize.Height))
            {
                y = this._visibleSize.Height - this._imageSize.Height;
            }
            if (this._drawArgs.Origin.Y != y)
            {
                this.SuspendRedraw();
                this._drawArgs.OffsetCropBounds(0, y - this._drawArgs.Origin.Y);
                this._drawArgs.Origin = new Point(this._drawArgs.Origin.X, y);
                this.ResumeRedraw();
            }
        }

        private void ApplyDrawArgs()
        {
            if (this._drawArgs.Rotation != Resco.Controls.ImageBox.Rotation.None)
            {
                this._image.Rotate(this._drawArgs.Rotation);
            }
            if ((((this._drawArgs.Brightness != 0) || (this._drawArgs.Contrast != 0)) || (this._drawArgs.Invert || (this._drawArgs.GammaRed != 0))) || ((this._drawArgs.GammaGreen != 0) || (this._drawArgs.GammaBlue != 0)))
            {
                this._image.AdjustColors(this._drawArgs.Brightness, this._drawArgs.Contrast, this._drawArgs.GammaRed, this._drawArgs.GammaGreen, this._drawArgs.GammaBlue, this._drawArgs.Invert);
            }
        }

        private void CalculateVisibleSize()
        {
            if (this._scrollBarMode == Resco.Controls.ImageBox.ScrollBarMode.Hidden)
            {
                this._visibleSize = base.ClientSize;
            }
            else if (this._scrollBarMode == Resco.Controls.ImageBox.ScrollBarMode.Visible)
            {
                this._visibleSize = new Size(base.ClientSize.Width - this._scrollBarWidth, base.ClientSize.Height - this._scrollBarWidth);
            }
            else
            {
                bool flag;
                this._visibleSize = base.ClientSize;
                if (this._visibleSize.Width < this._imageSize.Width)
                {
                    this._visibleSize.Height -= this._scrollBarWidth;
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                if (this._visibleSize.Height < this._imageSize.Height)
                {
                    this._visibleSize.Width -= this._scrollBarWidth;
                    if (!flag && (this._visibleSize.Width < this._imageSize.Width))
                    {
                        this._visibleSize.Height -= this._scrollBarWidth;
                    }
                }
            }
        }

        public void Crop()
        {
            this.Crop(this.CropBounds);
        }

        public void Crop(Rectangle cropBounds)
        {
            this.SuspendRedraw();
            this._image.Crop(cropBounds);
            this._drawArgs.Origin = new Point(0, 0);
            this._drawArgs.CropBounds = Rectangle.Empty;
            this._image_Changed(this, EventArgs.Empty);
            this.ResumeRedraw();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (this._image != null)
                {
                    this._image.Dispose();
                }
                if (this._backColorBrush != null)
                {
                    this._backColorBrush.Dispose();
                }
                if (this._cropPen != null)
                {
                    this._cropPen.Dispose();
                }
                if (this._cropBrush != null)
                {
                    this._cropBrush.Dispose();
                }
            }
        }

        [DllImport("coredll.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public void LoadImage(string path)
        {
            this.LoadImage(path, 0);
        }

        public void LoadImage(Uri url)
        {
            this.LoadImage(url, 0);
        }

        public void LoadImage(Stream stream, ImageFormat imageFormat)
        {
            this.LoadImage(stream, imageFormat, 0);
        }

        public void LoadImage(string path, int frame)
        {
            Resco.Controls.ImageBox.CompactImage image = new Resco.Controls.ImageBox.CompactImage();
            image.Load(path, 0, Color.Empty, LoadOptions.None, Size.Empty);
            this.CompactImage = image;
        }

        public void LoadImage(Uri url, int frame)
        {
            Resco.Controls.ImageBox.CompactImage image = new Resco.Controls.ImageBox.CompactImage();
            image.Load(url, 0, Color.Empty, LoadOptions.None, Size.Empty);
            this.CompactImage = image;
        }

        public void LoadImage(Stream stream, ImageFormat imageFormat, int frame)
        {
            Resco.Controls.ImageBox.CompactImage image = new Resco.Controls.ImageBox.CompactImage();
            image.Load(stream, imageFormat, 0, Color.Empty, LoadOptions.None, Size.Empty);
            this.CompactImage = image;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if ((this._penMode == Resco.Controls.ImageBox.PenMode.None) || (this._image == null))
            {
                base.OnMouseDown(e);
            }
            else
            {
                if (this._penMode == Resco.Controls.ImageBox.PenMode.Move)
                {
                    if ((this._visibleSize.Width < this._imageSize.Width) || (this._visibleSize.Height < this._imageSize.Height))
                    {
                        this._mouseAction = MouseAction.MoveImage;
                    }
                }
                else if (this._penMode == Resco.Controls.ImageBox.PenMode.Crop)
                {
                    Rectangle cropBounds = this._drawArgs.CropBounds;
                    if (Math.Abs((int) (cropBounds.Left - e.X)) <= _cropRectSize)
                    {
                        this._cropDragPoint = CropDragPoint.Left;
                    }
                    else if (Math.Abs((int) (cropBounds.Right - e.X)) <= _cropRectSize)
                    {
                        this._cropDragPoint = CropDragPoint.Right;
                    }
                    else if (Math.Abs((int) ((cropBounds.Left + (cropBounds.Width / 2)) - e.X)) <= _cropRectSize)
                    {
                        this._cropDragPoint = CropDragPoint.Middle;
                    }
                    else
                    {
                        this._cropDragPoint = CropDragPoint.None;
                    }
                    if (this._cropDragPoint != CropDragPoint.None)
                    {
                        if (Math.Abs((int) (cropBounds.Top - e.Y)) <= _cropRectSize)
                        {
                            this._cropDragPoint |= CropDragPoint.Top;
                        }
                        else if (Math.Abs((int) (cropBounds.Bottom - e.Y)) <= _cropRectSize)
                        {
                            this._cropDragPoint |= CropDragPoint.Bottom;
                        }
                        else if (Math.Abs((int) ((cropBounds.Top + (cropBounds.Height / 2)) - e.Y)) <= _cropRectSize)
                        {
                            this._cropDragPoint |= CropDragPoint.Middle;
                        }
                        else
                        {
                            this._cropDragPoint = CropDragPoint.None;
                        }
                    }
                    if (((this._cropDragPoint == CropDragPoint.None) && (Math.Abs((int) ((cropBounds.Left + (cropBounds.Width / 2)) - e.X)) <= _cropRectSize)) && (Math.Abs((int) ((cropBounds.Top + (cropBounds.Height / 2)) - e.Y)) <= _cropRectSize))
                    {
                        this._cropDragPoint = CropDragPoint.Middle;
                    }
                    if (this._cropDragPoint != CropDragPoint.None)
                    {
                        this._mouseAction = MouseAction.DragCrop;
                    }
                    else if ((this._visibleSize.Width < this._imageSize.Width) || (this._visibleSize.Height < this._imageSize.Height))
                    {
                        this._mouseAction = MouseAction.MoveImage;
                    }
                }
                if (this._mouseAction != MouseAction.None)
                {
                    this._mousePreviousPos = new Point(e.X, e.Y);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((this._mouseAction == MouseAction.None) || (this._image == null))
            {
                base.OnMouseMove(e);
            }
            else
            {
                this.SuspendRedraw();
                if (this._mouseAction == MouseAction.MoveImage)
                {
                    Point origin = this.Origin;
                    if (this._visibleSize.Width < this._imageSize.Width)
                    {
                        origin.X += e.X - this._mousePreviousPos.X;
                    }
                    if (this._visibleSize.Height < this._imageSize.Height)
                    {
                        origin.Y += e.Y - this._mousePreviousPos.Y;
                    }
                    this.Origin = origin;
                }
                else if (this._mouseAction == MouseAction.DragCrop)
                {
                    int x;
                    int y;
                    Rectangle cropBounds = this._drawArgs.CropBounds;
                    int num = this._mousePreviousPos.X - e.X;
                    int num2 = this._mousePreviousPos.Y - e.Y;
                    if (this._visibleSize.Width > this._imageSize.Width)
                    {
                        x = (this._visibleSize.Width - this._imageSize.Width) / 2;
                    }
                    else
                    {
                        x = this.Origin.X;
                    }
                    if (this._visibleSize.Height > this._imageSize.Height)
                    {
                        y = (this._visibleSize.Height - this._imageSize.Height) / 2;
                    }
                    else
                    {
                        y = this.Origin.Y;
                    }
                    if ((this._cropDragPoint & CropDragPoint.Left) != CropDragPoint.None)
                    {
                        if (num > (cropBounds.X - x))
                        {
                            num = cropBounds.X - x;
                        }
                        if (-num > cropBounds.Width)
                        {
                            num = -cropBounds.Width;
                        }
                        cropBounds.X -= num;
                        cropBounds.Width += num;
                    }
                    if ((this._cropDragPoint & CropDragPoint.Right) != CropDragPoint.None)
                    {
                        cropBounds.Width -= num;
                        if (cropBounds.Width < 0)
                        {
                            cropBounds.Width = 0;
                        }
                        else if (cropBounds.Right > (this._imageSize.Width + x))
                        {
                            cropBounds.Width = (this._imageSize.Width - cropBounds.X) + x;
                        }
                    }
                    if ((this._cropDragPoint & CropDragPoint.Top) != CropDragPoint.None)
                    {
                        if (num2 > (cropBounds.Y - y))
                        {
                            num2 = cropBounds.Y - y;
                        }
                        if (-num2 > cropBounds.Height)
                        {
                            num2 = -cropBounds.Height;
                        }
                        cropBounds.Y -= num2;
                        cropBounds.Height += num2;
                    }
                    if ((this._cropDragPoint & CropDragPoint.Bottom) != CropDragPoint.None)
                    {
                        cropBounds.Height -= num2;
                        if (cropBounds.Height < 0)
                        {
                            cropBounds.Height = 0;
                        }
                        else if (cropBounds.Bottom > (this._imageSize.Height + y))
                        {
                            cropBounds.Height = (this._imageSize.Height - cropBounds.Y) + y;
                        }
                    }
                    if (this._cropDragPoint == CropDragPoint.Middle)
                    {
                        cropBounds.X -= num;
                        if (cropBounds.X < x)
                        {
                            cropBounds.X = x;
                        }
                        else if ((cropBounds.Right - x) > this._imageSize.Width)
                        {
                            cropBounds.X = (this._imageSize.Width - cropBounds.Width) + x;
                        }
                        cropBounds.Y -= num2;
                        if (cropBounds.Y < y)
                        {
                            cropBounds.Y = y;
                        }
                        else if ((cropBounds.Bottom - y) > this._imageSize.Height)
                        {
                            cropBounds.Y = (this._imageSize.Height - cropBounds.Height) + y;
                        }
                    }
                    this._drawArgs.CropBounds = cropBounds;
                }
                this._mousePreviousPos = new Point(e.X, e.Y);
                this.ResumeRedraw();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if ((this._mouseAction == MouseAction.None) || (this._image == null))
            {
                base.OnMouseUp(e);
            }
            else
            {
                this.SuspendRedraw();
                this._mouseAction = MouseAction.None;
                this._mousePreviousPos = Point.Empty;
                this._cropDragPoint = CropDragPoint.None;
                this.ResumeRedraw();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this._suspendRedraw <= 0)
            {
                if (this._image == null)
                {
                    e.Graphics.FillRectangle(this._backColorBrush, e.ClipRectangle);
                }
                else
                {
                    this._drawArgs.DrawingMode = (this._mouseAction != MouseAction.None) ? Resco.Controls.ImageBox.DrawingMode.Fast : this._drawgingMode;
                    this._image.Draw(this, e.Graphics, this._drawArgs);
                }
                if (((this._penMode == Resco.Controls.ImageBox.PenMode.Crop) && (this._drawArgs.CropBounds != Rectangle.Empty)) && (this._image != null))
                {
                    Rectangle cropBounds = this._drawArgs.CropBounds;
                    e.Graphics.DrawRectangle(this._cropPen, cropBounds);
                    if (this._mouseAction == MouseAction.None)
                    {
                        e.Graphics.FillRectangle(this._cropBrush, new Rectangle(cropBounds.Left - _cropRectSize, cropBounds.Top - _cropRectSize, (2 * _cropRectSize) - 1, (2 * _cropRectSize) - 1));
                        e.Graphics.FillRectangle(this._cropBrush, new Rectangle((cropBounds.Left + (cropBounds.Width / 2)) - _cropRectSize, cropBounds.Y - _cropRectSize, (2 * _cropRectSize) - 1, (2 * _cropRectSize) - 1));
                        e.Graphics.FillRectangle(this._cropBrush, new Rectangle(cropBounds.Right - _cropRectSize, cropBounds.Top - _cropRectSize, (2 * _cropRectSize) - 1, (2 * _cropRectSize) - 1));
                        e.Graphics.FillRectangle(this._cropBrush, new Rectangle(cropBounds.Right - _cropRectSize, (cropBounds.Top + (cropBounds.Height / 2)) - _cropRectSize, (2 * _cropRectSize) - 1, (2 * _cropRectSize) - 1));
                        e.Graphics.FillRectangle(this._cropBrush, new Rectangle(cropBounds.Right - _cropRectSize, cropBounds.Bottom - _cropRectSize, (2 * _cropRectSize) - 1, (2 * _cropRectSize) - 1));
                        e.Graphics.FillRectangle(this._cropBrush, new Rectangle((cropBounds.Left + (cropBounds.Width / 2)) - _cropRectSize, cropBounds.Bottom - _cropRectSize, (2 * _cropRectSize) - 1, (2 * _cropRectSize) - 1));
                        e.Graphics.FillRectangle(this._cropBrush, new Rectangle(cropBounds.Left - _cropRectSize, cropBounds.Bottom - _cropRectSize, (2 * _cropRectSize) - 1, (2 * _cropRectSize) - 1));
                        e.Graphics.FillRectangle(this._cropBrush, new Rectangle(cropBounds.Left - _cropRectSize, (cropBounds.Top + (cropBounds.Height / 2)) - _cropRectSize, (2 * _cropRectSize) - 1, (2 * _cropRectSize) - 1));
                        int num = cropBounds.X + (cropBounds.Width / 2);
                        int num2 = cropBounds.Y + (cropBounds.Height / 2);
                        e.Graphics.FillRectangle(this._cropBrush, new Rectangle(num - _cropRectSize, num2 - _cropRectSize, (2 * _cropRectSize) - 1, (2 * _cropRectSize) - 1));
                    }
                }
                base.OnPaint(e);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.SuspendRedraw();
            this._image_Changed(this, EventArgs.Empty);
            this.ResumeRedraw();
        }

        public void ResumeRedraw()
        {
            if (this._suspendRedraw > 0)
            {
                this._suspendRedraw--;
                if (this._suspendRedraw == 0)
                {
                    base.Invalidate();
                }
            }
        }

        public void SaveImage(string path, int quality)
        {
            if (this._image == null)
            {
                throw new Exception();
            }
            this.ApplyDrawArgs();
            this._image.Save(path, quality);
        }

        public void SaveImage(Stream stream, ImageFormat imageFormat, int quality)
        {
            if (this._image == null)
            {
                throw new Exception();
            }
            this.ApplyDrawArgs();
            this._image.Save(stream, imageFormat, quality);
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this._scrollBarWidth = (int) (this._scrollBarWidth * factor.Width);
            base.ScaleControl(factor, specified);
        }

        public void SuspendRedraw()
        {
            this._suspendRedraw++;
        }

        private void UpdateScrollBars()
        {
            bool flag;
            bool flag2;
            this.CalculateVisibleSize();
            if (this._scrollBarMode == Resco.Controls.ImageBox.ScrollBarMode.Hidden)
            {
                flag2 = false;
                flag = false;
            }
            else if (this._scrollBarMode == Resco.Controls.ImageBox.ScrollBarMode.Visible)
            {
                flag2 = true;
                flag = true;
            }
            else if (this._image != null)
            {
                flag2 = this._visibleSize.Width < this._imageSize.Width;
                flag = this._visibleSize.Height < this._imageSize.Height;
            }
            else
            {
                flag2 = false;
                flag = false;
            }
            this._hScrollBar.Visible = flag2;
            if (flag2)
            {
                this._hScrollBar.Bounds = new Rectangle(0, base.Height - this._scrollBarWidth, base.Width, this._scrollBarWidth);
                this._hScrollBar.LargeChange = Math.Max(10, (this._imageSize.Width - this._visibleSize.Width) / 10);
                this._hScrollBar.SmallChange = Math.Max(1, this._hScrollBar.LargeChange / 2);
                this._hScrollBar.Minimum = 0;
                this._hScrollBar.Maximum = ((this._imageSize.Width - this._visibleSize.Width) + this._hScrollBar.LargeChange) - 1;
            }
            this._vScrollBar.Visible = flag;
            if (flag)
            {
                if (flag2)
                {
                    this._vScrollBar.Bounds = new Rectangle(base.Width - this._scrollBarWidth, 0, this._scrollBarWidth, (base.Height - this._scrollBarWidth) + 1);
                }
                else
                {
                    this._vScrollBar.Bounds = new Rectangle(base.Width - this._scrollBarWidth, 0, this._scrollBarWidth, base.Height);
                }
                this._vScrollBar.LargeChange = Math.Max(10, (this._imageSize.Height - this._visibleSize.Height) / 10);
                this._vScrollBar.SmallChange = Math.Max(1, this._vScrollBar.LargeChange / 2);
                this._vScrollBar.Minimum = 0;
                this._vScrollBar.Maximum = ((this._imageSize.Height - this._visibleSize.Height) + this._vScrollBar.LargeChange) - 1;
            }
            this._drawArgs.Margins = new Rectangle(0, 0, flag ? this._scrollBarWidth : 0, flag2 ? this._scrollBarWidth : 0);
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                this._drawArgs.BackgroundColor = value;
                this._backColorBrush.Dispose();
                this._backColorBrush = new SolidBrush(value);
            }
        }

        [DefaultValue(0)]
        public sbyte Brightness
        {
            get
            {
                return this._drawArgs.Brightness;
            }
            set
            {
                if (this._drawArgs.Brightness != value)
                {
                    this.SuspendRedraw();
                    this._drawArgs.Brightness = value;
                    this.ResumeRedraw();
                }
            }
        }

        public Resco.Controls.ImageBox.CompactImage CompactImage
        {
            get
            {
                return this._image;
            }
            set
            {
                if (this._image != null)
                {
                    this._image.Changed -= new EventHandler(this._image_Changed);
                    this._image.Dispose();
                }
                this._drawArgs.Origin = new Point(0, 0);
                this._drawArgs.CropBounds = Rectangle.Empty;
                this._image = value;
                if (this._image != null)
                {
                    this._image.Changed += new EventHandler(this._image_Changed);
                }
                this._image_Changed(this._image, EventArgs.Empty);
            }
        }

        [DefaultValue(0)]
        public sbyte Contrast
        {
            get
            {
                return this._drawArgs.Contrast;
            }
            set
            {
                if (this._drawArgs.Contrast != value)
                {
                    this.SuspendRedraw();
                    this._drawArgs.Contrast = value;
                    this.ResumeRedraw();
                }
            }
        }

        public Rectangle CropBounds
        {
            get
            {
                if (this._drawArgs.CropBounds == Rectangle.Empty)
                {
                    return Rectangle.Empty;
                }
                int x = this._drawArgs.CropBounds.X;
                int y = this._drawArgs.CropBounds.Y;
                int width = this._drawArgs.CropBounds.Width;
                int height = this._drawArgs.CropBounds.Height;
                if (this._visibleSize.Width > this._imageSize.Width)
                {
                    x -= (this._visibleSize.Width - this._imageSize.Width) / 2;
                }
                else
                {
                    x -= this.Origin.X;
                }
                if (this._visibleSize.Height > this._imageSize.Height)
                {
                    y -= (this._visibleSize.Height - this._imageSize.Height) / 2;
                }
                else
                {
                    y -= this.Origin.Y;
                }
                if (this._drawArgs.Zoom != 1f)
                {
                    x = (int) Math.Ceiling((double) (((float) x) / this._drawArgs.Zoom));
                    y = (int) Math.Ceiling((double) (((float) y) / this._drawArgs.Zoom));
                    width = (int) Math.Ceiling((double) (((float) width) / this._drawArgs.Zoom));
                    height = (int) Math.Ceiling((double) (((float) height) / this._drawArgs.Zoom));
                }
                if (this.Rotation != Resco.Controls.ImageBox.Rotation.None)
                {
                    int num5;
                    if (this.Rotation == Resco.Controls.ImageBox.Rotation.Left270)
                    {
                        num5 = x;
                        x = y;
                        y = this._image.Size.Height - (num5 + width);
                    }
                    else if (this.Rotation == Resco.Controls.ImageBox.Rotation.Left180)
                    {
                        x = this._image.Size.Width - (x + width);
                        y = this._image.Size.Height - (y + height);
                    }
                    else
                    {
                        num5 = x;
                        x = this._image.Size.Width - (y + height);
                        y = num5;
                    }
                    if ((this.Rotation == Resco.Controls.ImageBox.Rotation.Left270) || (this.Rotation == Resco.Controls.ImageBox.Rotation.Left90))
                    {
                        num5 = width;
                        width = height;
                        height = num5;
                    }
                }
                return new Rectangle(x, y, width, height);
            }
            set
            {
                Rectangle empty;
                if ((value.Width == 0) || (value.Height == 0))
                {
                    empty = Rectangle.Empty;
                }
                else
                {
                    int x = (value.X < 0) ? 0 : value.X;
                    int y = (value.Y < 0) ? 0 : value.Y;
                    int width = (value.Right > this._image.Size.Width) ? (this._image.Size.Width - value.X) : value.Width;
                    int height = (value.Bottom > this._image.Size.Height) ? (this._image.Size.Height - value.Y) : value.Height;
                    if (this.Rotation != Resco.Controls.ImageBox.Rotation.None)
                    {
                        int num5;
                        if (this.Rotation == Resco.Controls.ImageBox.Rotation.Left270)
                        {
                            num5 = x;
                            x = this._image.Size.Height - (y + height);
                            y = num5;
                        }
                        else if (this.Rotation == Resco.Controls.ImageBox.Rotation.Left180)
                        {
                            x = this._image.Size.Width - (x + width);
                            y = this._image.Size.Height - (y + height);
                        }
                        else
                        {
                            num5 = x;
                            x = y;
                            y = this._image.Size.Width - (x + width);
                        }
                        if ((this.Rotation == Resco.Controls.ImageBox.Rotation.Left270) || (this.Rotation == Resco.Controls.ImageBox.Rotation.Left90))
                        {
                            num5 = width;
                            width = height;
                            height = num5;
                        }
                    }
                    if (this._drawArgs.Zoom != 1f)
                    {
                        x = (int) Math.Ceiling((double) (this._drawArgs.Zoom * x));
                        y = (int) Math.Ceiling((double) (this._drawArgs.Zoom * y));
                        width = (int) Math.Ceiling((double) (this._drawArgs.Zoom * width));
                        height = (int) Math.Ceiling((double) (this._drawArgs.Zoom * height));
                    }
                    if (this._visibleSize.Width > this._imageSize.Width)
                    {
                        x += (this._visibleSize.Width - this._imageSize.Width) / 2;
                    }
                    else
                    {
                        x += this.Origin.X;
                    }
                    if (this._visibleSize.Height > this._imageSize.Height)
                    {
                        y += (this._visibleSize.Height - this._imageSize.Height) / 2;
                    }
                    else
                    {
                        y += this.Origin.Y;
                    }
                    empty = new Rectangle(x, y, width, height);
                }
                if (this._drawArgs.CropBounds != empty)
                {
                    this.SuspendRedraw();
                    this._drawArgs.CropBounds = empty;
                    this.ResumeRedraw();
                }
            }
        }

        public Resco.Controls.ImageBox.DrawingMode DrawingMode
        {
            get
            {
                return this._drawgingMode;
            }
            set
            {
                if (this._drawgingMode != value)
                {
                    this.SuspendRedraw();
                    this._drawgingMode = value;
                    this.ResumeRedraw();
                }
            }
        }

        [DefaultValue(0)]
        public sbyte Gamma
        {
            set
            {
                this.SuspendRedraw();
                this._drawArgs.Gamma = value;
                this.ResumeRedraw();
            }
        }

        [DefaultValue(0)]
        public sbyte GammaBlue
        {
            get
            {
                return this._drawArgs.GammaBlue;
            }
            set
            {
                if (this._drawArgs.GammaBlue != value)
                {
                    this.SuspendRedraw();
                    this._drawArgs.GammaBlue = value;
                    this.ResumeRedraw();
                }
            }
        }

        [DefaultValue(0)]
        public sbyte GammaGreen
        {
            get
            {
                return this._drawArgs.GammaGreen;
            }
            set
            {
                if (this._drawArgs.GammaGreen != value)
                {
                    this.SuspendRedraw();
                    this._drawArgs.GammaGreen = value;
                    this.ResumeRedraw();
                }
            }
        }

        [DefaultValue(0)]
        public sbyte GammaRed
        {
            get
            {
                return this._drawArgs.GammaRed;
            }
            set
            {
                if (this._drawArgs.GammaRed != value)
                {
                    this.SuspendRedraw();
                    this._drawArgs.GammaRed = value;
                    this.ResumeRedraw();
                }
            }
        }

        [DefaultValue(false)]
        public bool Invert
        {
            get
            {
                return this._drawArgs.Invert;
            }
            set
            {
                if (this._drawArgs.Invert != value)
                {
                    this.SuspendRedraw();
                    this._drawArgs.Invert = value;
                    this.ResumeRedraw();
                }
            }
        }

        public Point Origin
        {
            get
            {
                return this._drawArgs.Origin;
            }
            set
            {
                if ((this._image != null) && (this._drawArgs.Origin != value))
                {
                    int x;
                    int y;
                    if (value.X > 0)
                    {
                        x = 0;
                    }
                    else if (value.X < (this._visibleSize.Width - this._imageSize.Width))
                    {
                        x = this._visibleSize.Width - this._imageSize.Width;
                    }
                    else
                    {
                        x = value.X;
                    }
                    if (value.Y > 0)
                    {
                        y = 0;
                    }
                    else if (value.Y < (this._visibleSize.Height - this._imageSize.Height))
                    {
                        y = this._visibleSize.Height - this._imageSize.Height;
                    }
                    else
                    {
                        y = value.Y;
                    }
                    if ((this._drawArgs.Origin.X != x) || (this._drawArgs.Origin.Y != y))
                    {
                        this.SuspendRedraw();
                        if (this._drawArgs.Origin.X != x)
                        {
                            this._drawArgs.OffsetCropBounds(x - this._drawArgs.Origin.X, 0);
                            this._drawArgs.Origin = new Point(x, this._drawArgs.Origin.Y);
                            if (this._hScrollBar.Visible)
                            {
                                this._hScrollBar.Value = -x;
                            }
                        }
                        if (this._drawArgs.Origin.Y != y)
                        {
                            this._drawArgs.OffsetCropBounds(0, y - this._drawArgs.Origin.Y);
                            this._drawArgs.Origin = new Point(this._drawArgs.Origin.X, y);
                            if (this._vScrollBar.Visible)
                            {
                                this._vScrollBar.Value = -y;
                            }
                        }
                        this.ResumeRedraw();
                    }
                }
            }
        }

        public Resco.Controls.ImageBox.PenMode PenMode
        {
            get
            {
                return this._penMode;
            }
            set
            {
                if (this._penMode != value)
                {
                    this.SuspendRedraw();
                    this._penMode = value;
                    if (this._penMode != Resco.Controls.ImageBox.PenMode.Crop)
                    {
                        this._drawArgs.CropBounds = Rectangle.Empty;
                    }
                    this.ResumeRedraw();
                }
            }
        }

        [DefaultValue(0)]
        public Resco.Controls.ImageBox.Rotation Rotation
        {
            get
            {
                return this._drawArgs.Rotation;
            }
            set
            {
                Rectangle rectangle = (this._penMode == Resco.Controls.ImageBox.PenMode.Crop) ? this.CropBounds : Rectangle.Empty;
                if (this._drawArgs.Rotation != value)
                {
                    this.SuspendRedraw();
                    this._drawArgs.Rotation = value;
                    this._drawArgs.Origin = new Point(0, 0);
                    this._image_Changed(this._image, EventArgs.Empty);
                    if (this._penMode == Resco.Controls.ImageBox.PenMode.Crop)
                    {
                        this.CropBounds = rectangle;
                    }
                    this.ResumeRedraw();
                }
            }
        }

        public Resco.Controls.ImageBox.ScrollBarMode ScrollBarMode
        {
            get
            {
                return this._scrollBarMode;
            }
            set
            {
                if (this._scrollBarMode != value)
                {
                    this.SuspendRedraw();
                    this._scrollBarMode = value;
                    this.UpdateScrollBars();
                    this.ResumeRedraw();
                }
            }
        }

        public int ScrollBarWidth
        {
            get
            {
                return this._scrollBarWidth;
            }
            set
            {
                if (this._scrollBarWidth != value)
                {
                    this.SuspendRedraw();
                    this._scrollBarWidth = value;
                    this._vScrollBar.Bounds = new Rectangle(base.Width - this._scrollBarWidth, 0, this._scrollBarWidth, (base.Height - this._scrollBarWidth) + 1);
                    this._hScrollBar.Bounds = new Rectangle(0, base.Height - this._scrollBarWidth, base.Width, this._scrollBarWidth);
                    this.UpdateScrollBars();
                    this.ResumeRedraw();
                }
            }
        }

        [DefaultValue((float) 1f)]
        public float Zoom
        {
            get
            {
                return this._drawArgs.Zoom;
            }
            set
            {
                if (this._drawArgs.Zoom != value)
                {
                    if (this._image == null)
                    {
                        this._drawArgs.Zoom = value;
                    }
                    else
                    {
                        Rectangle rectangle = (this._penMode == Resco.Controls.ImageBox.PenMode.Crop) ? this.CropBounds : Rectangle.Empty;
                        float zoom = this._drawArgs.Zoom;
                        this._drawArgs.Zoom = value;
                        if (this._drawArgs.Zoom <= 0.0)
                        {
                            this._drawArgs.Zoom = this._image.CalculateZoom(this, this._drawArgs);
                        }
                        else
                        {
                            this._drawArgs.Zoom = this._image.CalculateZoom(this, this._drawArgs);
                        }
                        if (this._drawArgs.Zoom != zoom)
                        {
                            this.SuspendRedraw();
                            int num2 = (int) ((this._image.Size.Width * (zoom - this._drawArgs.Zoom)) / 2f);
                            int num3 = (int) ((this._image.Size.Height * (zoom - this._drawArgs.Zoom)) / 2f);
                            if ((num2 != 0) || (num3 != 0))
                            {
                                this.Origin = new Point(this._drawArgs.Origin.X + num2, this._drawArgs.Origin.Y + num3);
                            }
                            this._image_Changed(this._image, EventArgs.Empty);
                            if (this._penMode == Resco.Controls.ImageBox.PenMode.Crop)
                            {
                                this.CropBounds = rectangle;
                            }
                            this.ResumeRedraw();
                        }
                    }
                }
            }
        }
    }
}

