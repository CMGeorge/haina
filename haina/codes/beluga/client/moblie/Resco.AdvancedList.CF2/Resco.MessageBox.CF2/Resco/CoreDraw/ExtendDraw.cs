namespace Resco.CoreDraw
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    internal static class ExtendDraw
    {
        internal static readonly uint DT_CALCRECT = 0x400;
        internal static readonly uint DT_LEFT = 0;
        internal static readonly uint DT_NOPREFIX = 0x800;
        internal static readonly uint DT_TOP = 0;
        internal static readonly uint DT_WORDBREAK = 0x10;
        private const int SRCCOPY = 0xcc0020;

        public static void BitBlt(Graphics g, Rectangle dest, Graphics source, int srcX, int srcY)
        {
            IntPtr hdc = g.GetHdc();
            try
            {
                IntPtr hdcSrc = source.GetHdc();
                try
                {
                    BitBlt(hdc, dest.X, dest.Y, dest.Width, dest.Height, hdcSrc, srcX, srcY, 0xcc0020);
                }
                finally
                {
                    source.ReleaseHdc(hdcSrc);
                }
            }
            finally
            {
                g.ReleaseHdc(hdc);
            }
        }

        [DllImport("coredll.dll")]
        private static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
        public static unsafe void BlendRect(Graphics g, Rectangle rect, Color c)
        {
            PixelFormat format = PixelFormat.Format16bppRgb565;
            using (Bitmap bitmap = new Bitmap(rect.Width, rect.Height, format))
            {
                Rectangle dest = new Rectangle(0, 0, rect.Width, rect.Height);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    BitBlt(graphics, dest, g, rect.X, rect.Y);
                }
                BitmapData bitmapdata = bitmap.LockBits(dest, ImageLockMode.ReadWrite, format);
                int num = (bitmapdata.Height * bitmapdata.Stride) / 2;
                short* numPtr = (short*) bitmapdata.Scan0;
                uint a = c.A;
                ushort pixel = (ushort) ((((c.R & 0xf8) << 8) | ((c.G & 0xfc) << 3)) | (c.B >> 3));
                for (int i = 0; i < num; i++)
                {
                    numPtr[i] = (short) MakeAlpha16_16(a, pixel, (ushort) numPtr[i]);
                }
                bitmap.UnlockBits(bitmapdata);
                g.DrawImage(bitmap, rect.X, rect.Y);
            }
        }

        public static unsafe void BlendRects(Graphics g, Rectangle rect, Rectangle[] rects, Color[] colors)
        {
            PixelFormat format = PixelFormat.Format16bppRgb565;
            using (Bitmap bitmap = new Bitmap(rect.Width, rect.Height, format))
            {
                Rectangle dest = new Rectangle(0, 0, rect.Width, rect.Height);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    BitBlt(graphics, dest, g, rect.X, rect.Y);
                }
                BitmapData bitmapdata = bitmap.LockBits(dest, ImageLockMode.ReadWrite, format);
                int num = bitmapdata.Stride / 2;
                for (int i = 0; i < rects.Length; i++)
                {
                    Color color = colors[i];
                    Rectangle rectangle2 = rects[i];
                    short* numPtr = (short*) bitmapdata.Scan0;
                    numPtr += (num * rectangle2.Y) + rectangle2.X;
                    int width = rectangle2.Width;
                    uint a = color.A;
                    ushort pixel = (ushort) ((((color.R & 0xf8) << 8) | ((color.G & 0xfc) << 3)) | (color.B >> 3));
                    for (int j = 0; j < rectangle2.Height; j++)
                    {
                        for (int k = 0; k < width; k++)
                        {
                            numPtr[k] = (short) MakeAlpha16_16(a, pixel, (ushort) numPtr[k]);
                        }
                        numPtr += num;
                    }
                }
                bitmap.UnlockBits(bitmapdata);
                g.DrawImage(bitmap, rect.X, rect.Y);
            }
        }

        [DllImport("coredll.dll")]
        private static extern int DeleteObject(IntPtr HGDIOBJ);
        public static void DrawImage(Graphics g, Image image, int x, int y, Color transparent)
        {
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorKey(transparent, transparent);
            Rectangle destRect = new Rectangle(x, y, image.Width, image.Height);
            g.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
        }

        public static void DrawRoundedRectangle(Graphics g, Pen p, Brush fillBrush, Rectangle rc, int rad)
        {
            Point[] points = new Point[8];
            int width = rad;
            int num2 = rad;
            int num3 = 0;
            int num4 = 0;
            points[0].X = rc.Left + (width / 2);
            points[0].Y = rc.Top + 1;
            points[1].X = rc.Right - (num2 / 2);
            points[1].Y = rc.Top + 1;
            points[2].X = rc.Right;
            points[2].Y = rc.Top + (num2 / 2);
            points[3].X = rc.Right;
            points[3].Y = rc.Bottom - (num3 / 2);
            points[4].X = rc.Right - (num3 / 2);
            points[4].Y = rc.Bottom;
            points[5].X = rc.Left + (num4 / 2);
            points[5].Y = rc.Bottom;
            points[6].X = rc.Left + 1;
            points[6].Y = rc.Bottom - (num4 / 2);
            points[7].X = rc.Left + 1;
            points[7].Y = rc.Top + (width / 2);
            g.DrawLine(p, points[0].X, rc.Top, points[1].X, rc.Top);
            if (num2 > 1)
            {
                g.FillEllipse(fillBrush, rc.Right - num2, rc.Top, num2, num2);
                g.DrawEllipse(p, rc.Right - num2, rc.Top, num2, num2);
            }
            g.DrawLine(p, rc.Right, points[2].Y, rc.Right, points[3].Y);
            if (num3 > 1)
            {
                g.FillEllipse(fillBrush, rc.Right - num3, rc.Bottom - num3, num3, num3);
                g.DrawEllipse(p, rc.Right - num3, rc.Bottom - num3, num3, num3);
            }
            g.DrawLine(p, points[4].X, rc.Bottom, points[5].X, rc.Bottom);
            if (num4 > 1)
            {
                g.FillEllipse(fillBrush, rc.Left, rc.Bottom - num4, num4, num4);
                g.DrawEllipse(p, rc.Left, rc.Bottom - num4, num4, num4);
            }
            g.DrawLine(p, rc.Left, points[6].Y, rc.Left, points[7].Y);
            if (width > 1)
            {
                g.FillEllipse(fillBrush, rc.Left, rc.Top, width, width);
                g.DrawEllipse(p, rc.Left, rc.Top, width, width);
            }
            g.FillPolygon(fillBrush, points);
        }

        [DllImport("coredll.dll")]
        internal static extern int DrawText(IntPtr hDC, string lpString, int nCount, ref RECT lpRect, uint uFormat);
        public static void FillRoundedRectangle(Graphics g, Brush fillBrush, Rectangle rc, int rad, int corners)
        {
            Point[] points = new Point[8];
            int width = ((corners & 1) != 0) ? rad : 0;
            int num2 = ((corners & 2) != 0) ? rad : 0;
            int num3 = ((corners & 4) != 0) ? rad : 0;
            int num4 = ((corners & 8) != 0) ? rad : 0;
            rc.Width--;
            rc.Height--;
            points[0].X = rc.Left + (width / 2);
            points[0].Y = rc.Top;
            points[1].X = rc.Right - (num2 / 2);
            points[1].Y = rc.Top;
            points[2].X = rc.Right + 1;
            points[2].Y = rc.Top + (num2 / 2);
            points[3].X = rc.Right + 1;
            points[3].Y = rc.Bottom - (num3 / 2);
            points[4].X = rc.Right - (num3 / 2);
            points[4].Y = rc.Bottom + 1;
            points[5].X = rc.Left + (num4 / 2);
            points[5].Y = rc.Bottom + 1;
            points[6].X = rc.Left;
            points[6].Y = rc.Bottom - (num4 / 2);
            points[7].X = rc.Left;
            points[7].Y = rc.Top + (width / 2);
            if (num2 > 1)
            {
                g.FillEllipse(fillBrush, rc.Right - num2, rc.Top, num2, num2);
            }
            if (num3 > 1)
            {
                g.FillEllipse(fillBrush, rc.Right - num3, rc.Bottom - num3, num3, num3);
            }
            if (num4 > 1)
            {
                g.FillEllipse(fillBrush, rc.Left, rc.Bottom - num4, num4, num4);
            }
            if (width > 1)
            {
                g.FillEllipse(fillBrush, rc.Left, rc.Top, width, width);
            }
            g.FillPolygon(fillBrush, points);
        }

        private static uint MakeAlpha16_16(uint opacity, ushort pixel, ushort backpixel)
        {
            uint num = opacity;
            uint num2 = pixel;
            uint num3 = backpixel;
            uint num4 = num >> 3;
            uint num5 = 0xf81f;
            num = num3;
            num &= num5;
            uint num6 = num2;
            num6 &= num5;
            num6 -= num;
            uint num7 = num4;
            num7 *= num6;
            num6 = num7 >> 5;
            num += num6;
            num &= num5;
            num5 = 0x7e0;
            num3 &= num5;
            num2 &= num5;
            num2 -= num3;
            num4 *= num2;
            num2 = num4 >> 5;
            num2 += num3;
            num2 &= num5;
            return (num | num2);
        }

        public static Size MeasureString(Graphics g, string text, Font font, int maxWidth)
        {
            Size size;
            IntPtr zero = IntPtr.Zero;
            IntPtr hobj = IntPtr.Zero;
            try
            {
                RECT rect;
                zero = g.GetHdc();
                hobj = font.ToHfont();
                rect.Left = 0;
                rect.Top = 0;
                rect.Right = maxWidth;
                rect.Bottom = 0x3e8;
                IntPtr ptr3 = SelectObject(zero, hobj);
                DrawText(zero, text, text.Length, ref rect, (((DT_LEFT | DT_TOP) | DT_CALCRECT) | DT_NOPREFIX) | DT_WORDBREAK);
                SelectObject(zero, ptr3);
                size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            }
            finally
            {
                if (hobj != IntPtr.Zero)
                {
                    DeleteObject(hobj);
                }
                if (zero != IntPtr.Zero)
                {
                    g.ReleaseHdc(zero);
                }
            }
            return size;
        }

        [DllImport("coredll.dll")]
        private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hobj);

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public static implicit operator ExtendDraw.RECT(Rectangle rc)
            {
                ExtendDraw.RECT rect;
                rect.Left = rc.Left;
                rect.Right = rc.Right;
                rect.Top = rc.Top;
                rect.Bottom = rc.Bottom;
                return rect;
            }
        }
    }
}

