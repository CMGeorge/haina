namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    internal sealed class Win32Helper
    {
        [DllImport("coredll.dll", SetLastError=true)]
        public static extern bool GradientFill(IntPtr hdc, TRIVERTEX[] pVertex, uint dwNumVertex, GRADIENT_RECT[] pMesh, uint dwNumMesh, uint dwMode);

        [StructLayout(LayoutKind.Sequential)]
        public struct GRADIENT_RECT
        {
            public uint UpperLeft;
            public uint LowerRight;
            public GRADIENT_RECT(uint ul, uint lr)
            {
                this.UpperLeft = ul;
                this.LowerRight = lr;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TRIVERTEX
        {
            public int x;
            public int y;
            public ushort Red;
            public ushort Green;
            public ushort Blue;
            public ushort Alpha;
            public TRIVERTEX(int x, int y, Color color) : this(x, y, color.R, color.G, color.B, color.A)
            {
            }

            public TRIVERTEX(int x, int y, ushort red, ushort green, ushort blue, ushort alpha)
            {
                this.x = x;
                this.y = y;
                this.Red = (ushort) (red << 8);
                this.Green = (ushort) (green << 8);
                this.Blue = (ushort) (blue << 8);
                this.Alpha = (ushort) (alpha << 8);
            }
        }
    }
}

