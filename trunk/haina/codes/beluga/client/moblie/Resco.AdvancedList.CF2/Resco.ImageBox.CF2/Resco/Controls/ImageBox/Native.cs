namespace Resco.Controls.ImageBox
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal sealed class Native
    {
        private static readonly Native _initializer = new Native();
        private int _refCount;

        private Native()
        {
            try
            {
                NativeMethods.RIL_Initialize();
            }
            catch
            {
            }
        }

        private void AddRef()
        {
            if (this._refCount++ == 0)
            {
                GC.SuppressFinalize(this);
            }
        }

        ~Native()
        {
            try
            {
                NativeMethods.RIL_Uninitialize();
            }
            catch
            {
            }
        }

        private void ReleaseRef()
        {
            if (this._refCount-- == 1)
            {
                GC.ReRegisterForFinalize(this);
            }
        }

        internal static void RIL_AdjustColors(IntPtr handle, int brightness, int contrast, int red, int green, int blue, int invert)
        {
            int num;
            try
            {
                num = NativeMethods.RIL_AdjustColors(handle, brightness, contrast, red, green, blue, invert);
            }
            catch
            {
                throw new RILException();
            }
            if (num != 0)
            {
                throw new RILException(num);
            }
        }

        internal static float RIL_CalculateZoom(IntPtr handle, IntPtr hwnd, Size size, DrawArgs drawArgs)
        {
            int num;
            try
            {
                num = NativeMethods.RIL_CalculateZoom(handle, hwnd, size, ref drawArgs._DrawArgs);
            }
            catch
            {
                throw new RILException();
            }
            if (num != 0)
            {
                throw new RILException(num);
            }
            return drawArgs._DrawArgs.Zoom;
        }

        internal static void RIL_Close(IntPtr handle)
        {
            try
            {
                NativeMethods.RIL_Close(handle);
                if (handle != IntPtr.Zero)
                {
                    _initializer.ReleaseRef();
                }
            }
            catch
            {
                throw new RILException();
            }
        }

        internal static int RIL_ColorToCOLORREF(Color color)
        {
            return (((color.B << 0x10) | (color.G << 8)) | color.R);
        }

        internal static void RIL_Crop(IntPtr handle, Rectangle rect, out Size size)
        {
            int num;
            SIZE size2;
            try
            {
                num = NativeMethods.RIL_Crop(handle, rect, out size2);
            }
            catch
            {
                throw new RILException();
            }
            if (num != 0)
            {
                throw new RILException(num);
            }
            size = (Size) size2;
        }

        internal static void RIL_Draw(IntPtr handle, IntPtr hwnd, Size size, IntPtr hdc, DrawArgs drawArgs)
        {
            int num;
            try
            {
                num = NativeMethods.RIL_Draw(handle, hwnd, size, hdc, ref drawArgs._DrawArgs);
            }
            catch
            {
                throw new RILException();
            }
            if (num != 0)
            {
                throw new RILException(num);
            }
        }

        internal static RLoadOut RIL_Load(string location, Stream stream, string formatType, int frame, Color backgroundColor, LoadOptions loadOptions, Size maxSize, ProgressFnc progressFunction)
        {
            RLoadOut @out;
            int num;
            try
            {
                @out = new RLoadOut();
                num = NativeMethods.RIL_Load(location, (stream == null) ? null : ((IIOStream) new IOStream(stream)), formatType, frame, RIL_ColorToCOLORREF(backgroundColor), (int) loadOptions, maxSize, progressFunction, out @out);
            }
            catch
            {
                throw new RILException();
            }
            if (num != 0)
            {
                throw new RILException(num);
            }
            if (@out.Handle != IntPtr.Zero)
            {
                _initializer.AddRef();
            }
            return @out;
        }

        internal static void RIL_Resize(IntPtr handle, float factor, out Size size)
        {
            int num;
            SIZE size2;
            try
            {
                num = NativeMethods.RIL_Resize(handle, factor, out size2);
            }
            catch
            {
                throw new RILException();
            }
            if (num != 0)
            {
                throw new RILException(num);
            }
            size = (Size) size2;
        }

        internal static void RIL_Rotate(IntPtr handle, int angle, out Size size)
        {
            int num;
            SIZE size2;
            try
            {
                num = NativeMethods.RIL_Rotate(handle, angle, out size2);
            }
            catch
            {
                throw new RILException();
            }
            if (num != 0)
            {
                throw new RILException(num);
            }
            size = (Size) size2;
        }

        internal static void RIL_Save(IntPtr handle, string location, int quality)
        {
            int num;
            try
            {
                num = NativeMethods.RIL_Save(handle, location, null, null, quality);
            }
            catch
            {
                throw new RILException();
            }
            if (num != 0)
            {
                throw new RILException(num);
            }
        }

        internal static void RIL_Save(IntPtr handle, Stream stream, string formatType, int quality)
        {
            int num;
            try
            {
                num = NativeMethods.RIL_Save(handle, null, new IOStream(stream), formatType, quality);
            }
            catch
            {
                throw new RILException();
            }
            if (num != 0)
            {
                throw new RILException(num);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DRAWARGS
        {
            public Native.RECT Margins;
            public Native.RECT CropBounds;
            public float Zoom;
            public int Rotation;
            public Native.POINT Origin;
            public int BackgroundColor;
            public int DrawingMode;
            public Gamma GammaArgs;
            [StructLayout(LayoutKind.Sequential)]
            internal struct Gamma
            {
                public sbyte Red;
                public sbyte Green;
                public sbyte Blue;
                public sbyte Contrast;
                public sbyte Brightness;
                public int Invert;
            }
        }

        private static class NativeMethods
        {
            [DllImport("Resco.ImageBox.Native.dll")]
            internal static extern int RIL_AdjustColors(IntPtr handle, int brightness, int contrast, int red, int green, int blue, int invert);
            [DllImport("Resco.ImageBox.Native.dll")]
            internal static extern int RIL_CalculateZoom(IntPtr handle, IntPtr hwnd, Native.SIZE size, ref Native.DRAWARGS drawArgs);
            [DllImport("Resco.ImageBox.Native.dll")]
            internal static extern void RIL_Close(IntPtr handle);
            [DllImport("Resco.ImageBox.Native.dll")]
            internal static extern int RIL_Crop(IntPtr handle, Native.RECT rect, out Native.SIZE size);
            [DllImport("Resco.ImageBox.Native.dll")]
            internal static extern int RIL_Draw(IntPtr handle, IntPtr hwnd, Native.SIZE size, IntPtr hdc, ref Native.DRAWARGS drawArgs);
            [DllImport("Resco.ImageBox.Native.dll")]
            internal static extern void RIL_Initialize();
            [DllImport("Resco.ImageBox.Native.dll")]
            internal static extern int RIL_Load(string location, IIOStream stream, string formatType, int frame, int backgroundColor, int loadOptions, Size maxSize, Native.ProgressFnc progressFunction, out Native.RLoadOut loadOut);
            [DllImport("Resco.ImageBox.Native.dll")]
            internal static extern int RIL_Resize(IntPtr handle, float factor, out Native.SIZE size);
            [DllImport("Resco.ImageBox.Native.dll")]
            internal static extern int RIL_Rotate(IntPtr handle, int angle, out Native.SIZE size);
            [DllImport("Resco.ImageBox.Native.dll")]
            internal static extern int RIL_Save(IntPtr handle, string location, IIOStream stream, string formatType, int quality);
            [DllImport("Resco.ImageBox.Native.dll")]
            internal static extern void RIL_Uninitialize();
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            internal int x;
            internal int y;
            internal POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public static implicit operator Native.POINT(Point point)
            {
                return new Native.POINT(point.X, point.Y);
            }

            public static implicit operator Point(Native.POINT point)
            {
                return new Point(point.x, point.y);
            }
        }

        public delegate int ProgressFnc(float progress);

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public static implicit operator Native.RECT(Rectangle rect)
            {
                return new Native.RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }

            public static implicit operator Rectangle(Native.RECT rect)
            {
                return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RLoadOut
        {
            public IntPtr Handle;
            public Native.SIZE FullSize;
            public Native.SIZE Size;
            public int Depth;
            public int NumberOfFrames;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SIZE
        {
            internal int cx;
            internal int cy;
            public SIZE(int cx, int cy)
            {
                this.cx = cx;
                this.cy = cy;
            }

            public static implicit operator Native.SIZE(Size size)
            {
                return new Native.SIZE(size.Width, size.Height);
            }

            public static implicit operator Size(Native.SIZE size)
            {
                return new Size(size.cx, size.cy);
            }
        }
    }
}

