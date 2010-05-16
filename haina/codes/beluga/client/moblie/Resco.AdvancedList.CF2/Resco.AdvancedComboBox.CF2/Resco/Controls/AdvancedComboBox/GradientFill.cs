namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;

    internal sealed class GradientFill
    {
        private static Hashtable m_brushes = new Hashtable();

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
                if (Environment.OSVersion.Platform == PlatformID.WinCE)
                {
                    gr.FillRectangle(GetBrush(startColor), rc);
                    return true;
                }
                try
                {
                    try
                    {
                        Assembly.Load("Resco.AdvancedComboBox.Design, Version=" + GetAssemblyVersion() + ", Culture=neutral, PublicKeyToken=7444f602060105f9").GetType("Resco.Controls.AdvancedComboBox.Design.AdvancedComboBoxDesigner").GetMethod("DesignerGradientFill", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(Graphics), typeof(Rectangle), typeof(Color), typeof(Color), typeof(Color), typeof(Color), typeof(FillDirection) }, null).Invoke(null, new object[] { gr, rc, startColor, middleColor1, middleColor2, endColor, fillDirection });
                    }
                    catch (Exception)
                    {
                        gr.FillRectangle(GetBrush(startColor), rc);
                    }
                    return flag2;
                }
                finally
                {
                    flag2 = true;
                }
            }
            return flag2;
        }

        private static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private static Brush GetBrush(Color c)
        {
            if (m_brushes[c] == null)
            {
                m_brushes[c] = new SolidBrush(c);
            }
            return (m_brushes[c] as SolidBrush);
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

