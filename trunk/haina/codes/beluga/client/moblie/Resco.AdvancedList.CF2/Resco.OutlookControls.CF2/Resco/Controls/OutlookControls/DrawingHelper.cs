namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal static class DrawingHelper
    {
        private const byte AC_SRC_ALPHA = 0;
        private const byte AC_SRC_OVER = 0;
        private const int SRCCOPY = 0xcc0020;

        public static void AlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, BlendFunction blendFunction)
        {
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                AlphaBlendCE(hdcDest, nXOriginDest, nYOriginDest, nWidthDest, nHeightDest, hdcSrc, nXOriginSrc, nYOriginSrc, nWidthSrc, nHeightSrc, blendFunction);
            }
        }

        [DllImport("coredll.dll", EntryPoint="AlphaBlend")]
        private static extern int AlphaBlendCE(IntPtr hdcDest, int xDest, int yDest, int cxDest, int cyDest, IntPtr hdcSrc, int xSrc, int ySrc, int cxSrc, int cySrc, BlendFunction blendFunction);
        [DllImport("gdi32.dll", EntryPoint="GdiAlphaBlend")]
        public static extern bool AlphaBlendWin(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, BlendFunction blendFunction);
        [DllImport("coredll.dll")]
        public static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
        public static int CloneImage(IntPtr hdcDest, int nWidth, int nHeight, IntPtr hdcSrc)
        {
            return BitBlt(hdcDest, 0, 0, nWidth, nHeight, hdcSrc, 0, 0, 0xcc0020);
        }

        [DllImport("gdi32.dll", SetLastError=true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll", SetLastError=true)]
        private static extern bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll", SetLastError=true)]
        private static extern bool DeleteObject(IntPtr hObject);
        public static void DrawAlpha(Graphics graphics, Bitmap image, byte transparency, int x, int y)
        {
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                using (Graphics graphics2 = Graphics.FromImage(image))
                {
                    IntPtr hdc = graphics.GetHdc();
                    IntPtr hdcSrc = graphics2.GetHdc();
                    BlendFunction blendFunction = new BlendFunction(0, 0, transparency, 0);
                    AlphaBlend(hdc, x, y, image.Width, image.Height, hdcSrc, 0, 0, image.Width, image.Height, blendFunction);
                    graphics.ReleaseHdc(hdc);
                    graphics2.ReleaseHdc(hdcSrc);
                    return;
                }
            }
            TransparentBlt(graphics, image, x, y, transparency);
        }

        public static Size GetStringRectangle(Form sender, string source, Font font)
        {
            Size size;
            Graphics graphics = sender.CreateGraphics();
            try
            {
                size = new Size((int) graphics.MeasureString(source, font).Width, (int) graphics.MeasureString(source, font).Height);
            }
            catch
            {
                size = new Size(20, 20);
            }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                }
            }
            return size;
        }

        public static Bitmap ResizeBitmap(Bitmap bitmap, Size size)
        {
            if ((bitmap.Width == size.Width) && (bitmap.Height == size.Height))
            {
                return bitmap;
            }
            Bitmap image = new Bitmap(size.Width, size.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.DrawImage(bitmap, new Rectangle(0, 0, size.Width, size.Height), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
            graphics.Dispose();
            return image;
        }

        [DllImport("gdi32.dll", SetLastError=true)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        public static void TransparentBlt(Graphics gr, Bitmap aBmp, int aX, int aY, byte bAlpha)
        {
            IntPtr hdc = gr.GetHdc();
            IntPtr ptr2 = CreateCompatibleDC(hdc);
            IntPtr hgdiobj = SelectObject(ptr2, aBmp.GetHbitmap());
            AlphaBlendWin(hdc, aX, aY, aBmp.Width, aBmp.Height, ptr2, 0, 0, aBmp.Width, aBmp.Height, new BlendFunction(0, 0, bAlpha, 0));
            DeleteObject(SelectObject(ptr2, hgdiobj));
            DeleteDC(ptr2);
            gr.ReleaseHdc(hdc);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BlendFunction
        {
            private byte BlendOp;
            private byte BlendFlags;
            private byte SourceConstantAlpha;
            private byte AlphaFormat;
            public BlendFunction(byte op, byte flags, byte alpha, byte format)
            {
                this.BlendOp = op;
                this.BlendFlags = flags;
                this.SourceConstantAlpha = alpha;
                this.AlphaFormat = format;
            }
        }
    }
}

