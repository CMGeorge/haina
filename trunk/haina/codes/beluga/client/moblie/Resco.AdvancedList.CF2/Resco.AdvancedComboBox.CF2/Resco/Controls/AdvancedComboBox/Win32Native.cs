namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static class Win32Native
    {
        internal const int CB_GETDROPPEDSTATE = 0x157;
        internal const int CB_SHOWDROPDOWN = 0x14f;
        internal const int CBN_DROPDOWN = 7;
        private const uint EM_GETINPUTMODE = 0xdd;
        private const uint EM_SETINPUTMODE = 0xde;
        internal const int ES_NUMBER = 0x2000;
        private const int GW_CHILD = 5;
        internal const int GWL_EXSTYLE = -20;
        internal const int GWL_STYLE = -16;
        internal const int GWL_WNDPROC = -4;
        internal const int SWP_NOMOVE = 2;
        internal const int SWP_NOZORDER = 4;
        internal const int WM_COMMAND = 0x111;
        internal const int WM_DESTROY = 2;
        internal const int WM_KEYDOWN = 0x100;
        internal const int WM_KEYUP = 0x101;
        internal const int WM_LBUTTONDOWN = 0x201;
        internal const int WM_LBUTTONUP = 0x202;
        internal const int WM_SIZE = 5;

        [DllImport("coredll.dll")]
        internal static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, int msg, int wParam, int lParam);
        [DllImport("COREDLL.DLL", CharSet=CharSet.Unicode, SetLastError=true)]
        internal static extern IntPtr GetCapture();
        [DllImport("coredll.dll")]
        internal static extern int GetLastError();
        [DllImport("coredll.dll")]
        internal static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("coredll.dll")]
        public static extern int ReleaseCapture(int hwnd);
        [DllImport("COREDLL.DLL", CharSet=CharSet.Unicode, SetLastError=true)]
        internal static extern int SendMessageW(IntPtr hwnd, uint wMsg, uint wParam, uint lParam);
        public static void SendMouseDown(IntPtr handle, int x, int y)
        {
            int num = (y << 0x10) | x;
            uint wParam = 1;
            SendMessageW(handle, 0x201, wParam, (uint) num);
        }

        public static void SendMouseUp(IntPtr handle, int x, int y)
        {
            int num = (y << 0x10) | x;
            uint wParam = 1;
            SendMessageW(handle, 0x202, wParam, (uint) num);
        }

        [DllImport("coredll.dll")]
        public static extern int SetCapture(int hwnd);
        [DllImport("coredll.dll")]
        internal static extern IntPtr SetWindowLong(IntPtr hwnd, int nIndex, IntPtr dwNewLong);
        [DllImport("coredll.dll")]
        internal static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
        [DllImport("Coredll.dll", EntryPoint="SystemParametersInfoW", CharSet=CharSet.Unicode)]
        internal static extern int SystemParametersInfo4Strings(uint uiAction, uint uiParam, StringBuilder pvParam, uint fWinIni);

        internal delegate int WindowProcCallback(IntPtr hwnd, int msg, int wParam, int lParam);
    }
}

