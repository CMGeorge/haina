namespace Resco.Controls.ImageBox
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class CompactImage : IDisposable
    {
        private int _depth;
        private bool _disposed = false;
        private System.Drawing.Size _fullSize;
        private IntPtr _handle = IntPtr.Zero;
        private int _loadedFrame;
        private int _numberOfFrames;
        private Resco.Controls.ImageBox.Native.ProgressFnc _progressFunc;
        private GCHandle _progressFuncHandle;
        private System.Drawing.Size _size;

        public event EventHandler Changed;

        public event EventHandler<ProgressEventArgs> Progress;

        public CompactImage()
        {
            this._progressFunc = new Resco.Controls.ImageBox.Native.ProgressFnc(this.ProgressFnc);
        }

        public void AdjustBrightness(sbyte change)
        {
            this.AdjustColors(change, 0, 0, 0, 0, false);
        }

        internal void AdjustColors(int brightness, int contrast, int red, int green, int blue, bool invert)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("CompactImage");
            }
            if (this._handle != IntPtr.Zero)
            {
                Native.RIL_AdjustColors(this._handle, brightness, contrast, red, green, blue, invert ? 1 : 0);
                this.OnChanged();
            }
        }

        public void AdjustContrast(sbyte change)
        {
            this.AdjustColors(0, change, 0, 0, 0, false);
        }

        public void AdjustGamma(sbyte change)
        {
            this.AdjustGamma(change, change, change);
        }

        public void AdjustGamma(sbyte redChange, sbyte greenChange, sbyte blueChange)
        {
            this.AdjustColors(0, 0, redChange, greenChange, blueChange, false);
        }

        internal float CalculateZoom(Control control, DrawArgs drawArgs)
        {
            if (this._handle != IntPtr.Zero)
            {
                return Native.RIL_CalculateZoom(this._handle, control.Handle, control.Size, drawArgs);
            }
            return 1f;
        }

        public void Crop(Rectangle rect)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("CompactImage");
            }
            if (this._handle != IntPtr.Zero)
            {
                if (((rect.X < 0) || (rect.Y < 0)) || ((rect.Right > this._size.Width) || (rect.Bottom > this._size.Height)))
                {
                    throw new ArgumentException("Invalid crop rectangle defined.");
                }
                Native.RIL_Crop(this._handle, rect, out this._size);
                this.OnChanged();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (this._handle != IntPtr.Zero)
                {
                    Native.RIL_Close(this._handle);
                }
                this._disposed = true;
            }
        }

        internal void Draw(Control control, Graphics graphics, DrawArgs drawArgs)
        {
            if (this._handle != IntPtr.Zero)
            {
                IntPtr hdc = graphics.GetHdc();
                Native.RIL_Draw(this._handle, control.Handle, control.Size, hdc, drawArgs);
                graphics.ReleaseHdc(hdc);
            }
        }

        ~CompactImage()
        {
            this.Dispose(false);
        }

        public void Invert()
        {
            this.AdjustColors(0, 0, 0, 0, 0, true);
        }

        public void Load(string path)
        {
            this.Load(path, Color.Empty, LoadOptions.None, System.Drawing.Size.Empty);
        }

        public void Load(Uri url)
        {
            this.Load(url, Color.Empty, LoadOptions.None, System.Drawing.Size.Empty);
        }

        public void Load(Stream stream, ImageFormat imageFormat)
        {
            this.Load(stream, imageFormat, Color.Empty, LoadOptions.None, System.Drawing.Size.Empty);
        }

        public void Load(string path, Color backgroundColor, LoadOptions loadOptions, System.Drawing.Size maxSize)
        {
            this.Load(path, 0, backgroundColor, loadOptions, maxSize);
        }

        public void Load(Uri url, Color backgroundColor, LoadOptions loadOptions, System.Drawing.Size maxSize)
        {
            this.Load(url, 0, backgroundColor, loadOptions, maxSize);
        }

        public void Load(Stream stream, ImageFormat imageFormat, Color backgroundColor, LoadOptions loadOptions, System.Drawing.Size maxSize)
        {
            this.Load(stream, imageFormat, 0, backgroundColor, loadOptions, maxSize);
        }

        public void Load(string path, int frame, Color backgroundColor, LoadOptions loadOptions, System.Drawing.Size maxSize)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("CompactImage");
            }
            if (this._handle != IntPtr.Zero)
            {
                Native.RIL_Close(this._handle);
            }
            Native.RLoadOut @out = Native.RIL_Load(path, null, null, frame, backgroundColor, loadOptions, maxSize, this._progressFunc);
            this._handle = @out.Handle;
            this._fullSize = (System.Drawing.Size) @out.FullSize;
            this._size = (System.Drawing.Size) @out.Size;
            this._depth = @out.Depth;
            this._numberOfFrames = @out.NumberOfFrames;
            this._loadedFrame = frame;
            this.OnChanged();
        }

        public void Load(Uri url, int frame, Color backgroundColor, LoadOptions loadOptions, System.Drawing.Size maxSize)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("CompactImage");
            }
            if ((string.Compare(url.Scheme, "HTTP", true) != 0) && (string.Compare(url.Scheme, "FILE", true) != 0))
            {
                throw new Exception(string.Format("Schema '{0}' is not supported. Only HTTP and FILE schemas are supported.", url.Scheme));
            }
            if (this._handle != IntPtr.Zero)
            {
                Native.RIL_Close(this._handle);
            }
            Native.RLoadOut @out = Native.RIL_Load(url.ToString(), null, null, frame, backgroundColor, loadOptions, maxSize, this._progressFunc);
            this._handle = @out.Handle;
            this._fullSize = (System.Drawing.Size) @out.FullSize;
            this._size = (System.Drawing.Size) @out.Size;
            this._depth = @out.Depth;
            this._numberOfFrames = @out.NumberOfFrames;
            this._loadedFrame = frame;
            this.OnChanged();
        }

        public void Load(Stream stream, ImageFormat imageFormat, int frame, Color backgroundColor, LoadOptions loadOptions, System.Drawing.Size maxSize)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("CompactImage");
            }
            if (this._handle != IntPtr.Zero)
            {
                Native.RIL_Close(this._handle);
            }
            Native.RLoadOut @out = Native.RIL_Load(null, stream, imageFormat.ToString(), frame, backgroundColor, loadOptions, maxSize, this._progressFunc);
            this._handle = @out.Handle;
            this._fullSize = (System.Drawing.Size) @out.FullSize;
            this._size = (System.Drawing.Size) @out.Size;
            this._depth = @out.Depth;
            this._numberOfFrames = @out.NumberOfFrames;
            this._loadedFrame = frame;
            this.OnChanged();
        }

        private void OnChanged()
        {
            if (this.Changed != null)
            {
                this.Changed(this, EventArgs.Empty);
            }
        }

        private bool OnProgress(float progress)
        {
            if (this.Progress == null)
            {
                return false;
            }
            ProgressEventArgs args = new ProgressEventArgs();
            args.Progress = progress;
            args.Cancel = false;
            this.Progress.Invoke(this, args);
            return args.Cancel;
        }

        private int ProgressFnc(float progress)
        {
            if (!this.OnProgress(progress))
            {
                return 1;
            }
            return 0;
        }

        public void Resize(float factor)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("CompactImage");
            }
            if (this._handle != IntPtr.Zero)
            {
                if (factor < 0.0)
                {
                    throw new ArgumentException("The resize factor must be larger then 0.");
                }
                Native.RIL_Resize(this._handle, factor, out this._size);
                this.OnChanged();
            }
        }

        public void Rotate(Rotation rotation)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("CompactImage");
            }
            if (this._handle != IntPtr.Zero)
            {
                Native.RIL_Rotate(this._handle, (int) rotation, out this._size);
                this.OnChanged();
            }
        }

        public void Save(string path, int quality)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("CompactImage");
            }
            if (this._handle != IntPtr.Zero)
            {
                Native.RIL_Save(this._handle, path, quality);
            }
        }

        public void Save(Stream stream, ImageFormat imageFormat, int quality)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("CompactImage");
            }
            if (this._handle != IntPtr.Zero)
            {
                Native.RIL_Save(this._handle, stream, imageFormat.ToString(), quality);
            }
        }

        public int Depth
        {
            get
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("CompactImage");
                }
                return this._depth;
            }
        }

        public System.Drawing.Size FullSize
        {
            get
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("CompactImage");
                }
                return this._fullSize;
            }
        }

        public int LoadedFrame
        {
            get
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("CompactImage");
                }
                return this._loadedFrame;
            }
        }

        public int NumberOfFrames
        {
            get
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("CompactImage");
                }
                return this._numberOfFrames;
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("CompactImage");
                }
                return this._size;
            }
        }
    }
}

