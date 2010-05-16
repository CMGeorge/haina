namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Drawing;

    internal sealed class GradientFill
    {
        public static void DrawVistaGradient(Graphics gr, Color aColor, Rectangle aRect, FillDirection aDir)
        {
            Color colorA = GetColorA(aColor);
            Color colorB = GetColorB(aColor);
            Color colorC = GetColorC(aColor);
            Color colorD = GetColorD(aColor);
            Fill(gr, aRect, colorA, colorB, colorC, colorD, aDir);
        }

        public static bool Fill(Graphics gr, Rectangle rc, Color startColor, Color middleColor1, Color middleColor2, Color endColor, FillDirection fillDirection)
        {
            bool flag = (middleColor1 != Color.Transparent) | (middleColor2 != Color.Transparent);
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
            {
                return FillManagedWithMiddle(gr, rc, startColor, middleColor1, middleColor2, endColor, fillDirection);
            }
            int num = 2;
            if (flag)
            {
                num += 2;
            }
            Win32Helper.TRIVERTEX[] pVertex = new Win32Helper.TRIVERTEX[num];
            pVertex[0] = new Win32Helper.TRIVERTEX(rc.X, rc.Y, startColor);
            if (flag)
            {
                if (middleColor1 == Color.Transparent)
                {
                    middleColor1 = middleColor2;
                }
                if (middleColor2 == Color.Transparent)
                {
                    middleColor2 = middleColor1;
                }
                if (fillDirection == FillDirection.Horizontal)
                {
                    pVertex[1] = new Win32Helper.TRIVERTEX(rc.X + (rc.Width / 2), rc.Bottom, middleColor1);
                    pVertex[2] = new Win32Helper.TRIVERTEX(rc.X + (rc.Width / 2), rc.Y, middleColor2);
                }
                else
                {
                    pVertex[1] = new Win32Helper.TRIVERTEX(rc.Right, rc.Y + (rc.Height / 2), middleColor1);
                    pVertex[2] = new Win32Helper.TRIVERTEX(rc.X, rc.Y + (rc.Height / 2), middleColor2);
                }
            }
            pVertex[num - 1] = new Win32Helper.TRIVERTEX(rc.Right, rc.Bottom, endColor);
            Win32Helper.GRADIENT_RECT[] pMesh = null;
            if (flag)
            {
                pMesh = new Win32Helper.GRADIENT_RECT[] { new Win32Helper.GRADIENT_RECT(0, 1), new Win32Helper.GRADIENT_RECT(2, 3) };
            }
            else
            {
                pMesh = new Win32Helper.GRADIENT_RECT[] { new Win32Helper.GRADIENT_RECT(0, 1) };
            }
            IntPtr hdc = gr.GetHdc();
            bool flag2 = false;
            try
            {
                flag2 = Win32Helper.GradientFill(hdc, pVertex, (uint) pVertex.Length, pMesh, (uint) pMesh.Length, (uint) fillDirection);
                gr.ReleaseHdc(hdc);
            }
            catch (Exception)
            {
                gr.ReleaseHdc(hdc);
                flag2 = FillManagedWithMiddle(gr, rc, startColor, middleColor1, middleColor2, endColor, fillDirection);
            }
            return flag2;
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
                        Resco.Controls.AdvancedTree.AdvancedTree.DrawPixel(gr, c, left, top);
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
                        Color color2 = Color.FromArgb(num14, num15, num16);
                        gr.DrawLine(Resco.Controls.AdvancedTree.AdvancedTree.GetPen(color2), num13, top, num13, num6);
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
                        Resco.Controls.AdvancedTree.AdvancedTree.DrawPixel(gr, color3, x, i);
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
                        gr.DrawLine(Resco.Controls.AdvancedTree.AdvancedTree.GetPen(color4), x, j, num22, j);
                        num28++;
                    }
                }
            }
            return true;
        }

        public static bool FillManagedWithMiddle(Graphics gr, Rectangle rc, Color startColor, Color middleColor1, Color middleColor2, Color endColor, FillDirection fillDirection)
        {
            if ((middleColor1 != Color.Transparent) | (middleColor2 != Color.Transparent))
            {
                if (middleColor1 == Color.Transparent)
                {
                    middleColor1 = middleColor2;
                }
                if (middleColor2 == Color.Transparent)
                {
                    middleColor2 = middleColor1;
                }
                if (fillDirection == FillDirection.Horizontal)
                {
                    Rectangle rectangle = new Rectangle(rc.X, rc.Y, rc.Width / 2, rc.Height);
                    FillManaged(gr, rectangle, startColor, middleColor1, fillDirection);
                    Rectangle rectangle2 = new Rectangle(rc.X + (rc.Width / 2), rc.Y, rc.Width / 2, rc.Height);
                    FillManaged(gr, rectangle2, middleColor2, endColor, fillDirection);
                }
                else
                {
                    Rectangle rectangle3 = new Rectangle(rc.X, rc.Y, rc.Width, rc.Height / 2);
                    FillManaged(gr, rectangle3, startColor, middleColor1, fillDirection);
                    Rectangle rectangle4 = new Rectangle(rc.X, rc.Y + (rc.Height / 2), rc.Width, rc.Height / 2);
                    FillManaged(gr, rectangle4, middleColor2, endColor, fillDirection);
                }
            }
            else
            {
                FillManaged(gr, rc, startColor, endColor, fillDirection);
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

