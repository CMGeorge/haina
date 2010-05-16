namespace Resco.Controls.DetailView
{
    using System;
    using System.Drawing;

    internal sealed class GradientFill
    {
        public static void DrawVistaGradient(Graphics gr, Color aColor, Rectangle aRect, FillDirection aDir)
        {
            Rectangle dstrc = aRect;
            if (aDir == FillDirection.Vertical)
            {
                dstrc.Height /= 2;
            }
            else
            {
                dstrc.Width /= 2;
            }
            Rectangle rectangle2 = dstrc;
            if (aDir == FillDirection.Vertical)
            {
                rectangle2.Location = new Point(aRect.X, aRect.Y + (aRect.Height / 2));
            }
            else
            {
                rectangle2.Location = new Point(aRect.X + (aRect.Width / 2), aRect.Y);
            }
            Color colorA = GetColorA(aColor);
            Color colorB = GetColorB(aColor);
            Color colorC = GetColorC(aColor);
            Color colorD = GetColorD(aColor);
            GradientColor gradientColor = new GradientColor(colorA, colorB, aDir);
            Fill(gr, dstrc, dstrc, gradientColor);
            GradientColor color6 = new GradientColor(colorC, colorD, aDir);
            Fill(gr, rectangle2, rectangle2, color6);
        }

        public static bool Fill(Graphics gr, Rectangle dstrc, Rectangle srcrc, GradientColor gradientColor)
        {
            Color startColor = gradientColor.StartColor;
            Color endColor = gradientColor.EndColor;
            if (gradientColor.FillDirection == FillDirection.Horizontal)
            {
                if ((dstrc.X != srcrc.X) || (dstrc.Width != srcrc.Width))
                {
                    float num = ((float) (dstrc.Left - srcrc.X)) / ((float) srcrc.Width);
                    float num2 = ((float) (dstrc.Right - srcrc.X)) / ((float) srcrc.Width);
                    int num3 = endColor.R - startColor.R;
                    int num4 = endColor.G - startColor.G;
                    int num5 = endColor.B - startColor.B;
                    startColor = Color.FromArgb((byte) (gradientColor.StartColor.R + (num3 * num)), (byte) (gradientColor.StartColor.G + (num4 * num)), (byte) (gradientColor.StartColor.B + (num5 * num)));
                    endColor = Color.FromArgb((byte) (gradientColor.StartColor.R + (num3 * num2)), (byte) (gradientColor.StartColor.G + (num4 * num2)), (byte) (gradientColor.StartColor.B + (num5 * num2)));
                }
            }
            else if ((dstrc.Y != srcrc.Y) || (dstrc.Height != srcrc.Height))
            {
                float num6 = ((float) (dstrc.Top - srcrc.Y)) / ((float) srcrc.Height);
                float num7 = ((float) (dstrc.Bottom - srcrc.Y)) / ((float) srcrc.Height);
                int num8 = endColor.R - startColor.R;
                int num9 = endColor.G - startColor.G;
                int num10 = endColor.B - startColor.B;
                startColor = Color.FromArgb((byte) (gradientColor.StartColor.R + (num8 * num6)), (byte) (gradientColor.StartColor.G + (num9 * num6)), (byte) (gradientColor.StartColor.B + (num10 * num6)));
                endColor = Color.FromArgb((byte) (gradientColor.StartColor.R + (num8 * num7)), (byte) (gradientColor.StartColor.G + (num9 * num7)), (byte) (gradientColor.StartColor.B + (num10 * num7)));
            }
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
            {
                return FillManaged(gr, dstrc, startColor, endColor, gradientColor.FillDirection);
            }
            Win32Helper.TRIVERTEX[] pVertex = new Win32Helper.TRIVERTEX[] { new Win32Helper.TRIVERTEX(dstrc.X, dstrc.Y, startColor), new Win32Helper.TRIVERTEX(dstrc.Right, dstrc.Bottom, endColor) };
            Win32Helper.GRADIENT_RECT[] pMesh = new Win32Helper.GRADIENT_RECT[] { new Win32Helper.GRADIENT_RECT(0, 1) };
            IntPtr hdc = gr.GetHdc();
            bool flag = false;
            try
            {
                flag = Win32Helper.GradientFill(hdc, pVertex, (uint) pVertex.Length, pMesh, (uint) pMesh.Length, (uint) gradientColor.FillDirection);
                gr.ReleaseHdc(hdc);
            }
            catch (Exception)
            {
                gr.ReleaseHdc(hdc);
                flag = FillManaged(gr, dstrc, startColor, endColor, gradientColor.FillDirection);
            }
            return flag;
        }

        public static bool FillManaged(Graphics gr, Rectangle rc, Color startColor, Color endColor, FillDirection fillDirection)
        {
            if (fillDirection == FillDirection.Horizontal)
            {
                int num = endColor.R - startColor.R;
                int num2 = endColor.G - startColor.G;
                int num3 = endColor.B - startColor.B;
                int width = rc.Width;
                int right = rc.Right;
                int top = rc.Top;
                int num6 = rc.Bottom - 1;
                if (top == num6)
                {
                    int num7 = 0;
                    int left = rc.Left;
                    while (num7 < width)
                    {
                        int red = startColor.R + ((num7 * num) / width);
                        int green = startColor.G + ((num7 * num2) / width);
                        int blue = startColor.B + ((num7 * num3) / width);
                        Color c = Color.FromArgb(red, green, blue);
                        Resco.Controls.DetailView.DetailView.DrawPixel(gr, c, left, top);
                        left++;
                        num7++;
                    }
                }
                else
                {
                    int num12 = 0;
                    int num13 = rc.Left;
                    while (num12 < width)
                    {
                        int num14 = startColor.R + ((num12 * num) / width);
                        int num15 = startColor.G + ((num12 * num2) / width);
                        int num16 = startColor.B + ((num12 * num3) / width);
                        Color color = Color.FromArgb(num14, num15, num16);
                        gr.DrawLine(Resco.Controls.DetailView.DetailView.GetPen(color), num13, top, num13, num6);
                        num13++;
                        num12++;
                    }
                }
            }
            else if (fillDirection == FillDirection.Vertical)
            {
                int num17 = endColor.R - startColor.R;
                int num18 = endColor.G - startColor.G;
                int num19 = endColor.B - startColor.B;
                int height = rc.Height;
                int x = rc.Left;
                int num22 = rc.Right - 1;
                int bottom = rc.Bottom;
                if (x == num22)
                {
                    int num23 = 0;
                    for (int i = rc.Top; num23 < height; i++)
                    {
                        int num25 = startColor.R + ((num23 * num17) / height);
                        int num26 = startColor.G + ((num23 * num18) / height);
                        int num27 = startColor.B + ((num23 * num19) / height);
                        Color color3 = Color.FromArgb(num25, num26, num27);
                        Resco.Controls.DetailView.DetailView.DrawPixel(gr, color3, x, i);
                        num23++;
                    }
                }
                else
                {
                    int num28 = 0;
                    for (int j = rc.Top; num28 < height; j++)
                    {
                        int num30 = startColor.R + ((num28 * num17) / height);
                        int num31 = startColor.G + ((num28 * num18) / height);
                        int num32 = startColor.B + ((num28 * num19) / height);
                        Color color4 = Color.FromArgb(num30, num31, num32);
                        gr.DrawLine(Resco.Controls.DetailView.DetailView.GetPen(color4), x, j, num22, j);
                        num28++;
                    }
                }
            }
            return true;
        }

        private static Color GetColorA(Color aColor)
        {
            int red = (int) (((double) (0xde + aColor.R)) / 2.0);
            int green = (int) (((double) (0xde + aColor.G)) / 2.0);
            int blue = (int) (((double) (0xde + aColor.B)) / 2.0);
            return Color.FromArgb(red, green, blue);
        }

        private static Color GetColorB(Color aColor)
        {
            int red = (int) (((double) (0x43 + aColor.R)) / 2.0);
            int green = (int) (((double) (0x43 + aColor.G)) / 2.0);
            int blue = (int) (((double) (0x43 + aColor.B)) / 2.0);
            return Color.FromArgb(red, green, blue);
        }

        private static Color GetColorC(Color aColor)
        {
            int red = (int) (((double) aColor.R) / 2.0);
            int green = (int) (((double) aColor.G) / 2.0);
            int blue = (int) (((double) aColor.B) / 2.0);
            return Color.FromArgb(red, green, blue);
        }

        private static Color GetColorD(Color aColor)
        {
            int red = (int) (((double) (0x65 + aColor.R)) / 2.0);
            int green = (int) (((double) (0x6a + aColor.G)) / 2.0);
            int blue = (int) (((double) (0x6d + aColor.B)) / 2.0);
            return Color.FromArgb(red, green, blue);
        }
    }
}

