namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class HTCNavSensor : IDisposable
    {
        private const int HTCNavOpenAPI = 1;
        private Control myControl;
        private WndProcHandler myHandler;
        private int myLastTick;
        private IntPtr myOldWndProc;
        private const int WM_HTCNAV = 0x4c8;

        public event NavSensorMoveHandler Rotated;

        public HTCNavSensor(Control control)
        {
            this.myHandler = new WndProcHandler(this.WndProc);
            this.myControl = control;
            HTCNavOpen(control.Handle, 1);
            HTCNavSetMode(control.Handle, HTCAPIMode.Gesture);
            this.myOldWndProc = SetWindowLong(control.Handle, WindowLong.GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(this.myHandler));
        }

        [DllImport("coredll")]
        private static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int Msg, int wParam, int lParam);
        public void Dispose()
        {
            if ((this.myOldWndProc != IntPtr.Zero) && (this.myControl != null))
            {
                SetWindowLong(this.myControl.Handle, WindowLong.GWL_WNDPROC, this.myOldWndProc);
                this.myOldWndProc = IntPtr.Zero;
            }
            HTCNavClose(1);
            this.myControl = null;
            this.Rotated = null;
        }

        [DllImport("HTCAPI")]
        private static extern int HTCNavClose(int api);
        [DllImport("HTCAPI")]
        private static extern int HTCNavOpen(IntPtr hWnd, int api);
        [DllImport("HTCAPI")]
        private static extern int HTCNavSetMode(IntPtr hWnd, HTCAPIMode mode);
        [DllImport("coredll")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, WindowLong windowLong, IntPtr newLong);
        private int WndProc(IntPtr hWnd, int message, int wParam, int lParam)
        {
            try
            {
                if (message == 0x4c8)
                {
                    int num = Environment.TickCount - this.myLastTick;
                    this.myLastTick = Environment.TickCount;
                    int num2 = 0x5dc;
                    if (num > num2)
                    {
                        num = 0;
                    }
                    bool flag = (wParam & 0x100) == 0;
                    int num3 = wParam & 0xff;
                    double rotationsPerSecond = num3 * 0.038910505836575876;
                    if (!flag)
                    {
                        rotationsPerSecond = -rotationsPerSecond;
                    }
                    double radialDelta = (rotationsPerSecond * num) / 1000.0;
                    if (this.Rotated != null)
                    {
                        this.Rotated(rotationsPerSecond, radialDelta);
                    }
                }
            }
            catch (Exception)
            {
            }
            return CallWindowProc(this.myOldWndProc, hWnd, message, wParam, lParam);
        }

        private enum HTCAPIMode : uint
        {
            Gesture = 4
        }

        private enum WindowLong
        {
            GWL_WNDPROC = -4
        }

        private delegate int WndProcHandler(IntPtr hwnd, int message, int wParam, int lParam);
    }
}

