namespace Resco.Controls.DetailView.DetailViewInternal
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    internal class Imports
    {
        internal const int CB_GETDROPPEDSTATE = 0x157;
        internal const int CB_LIMITTEXT = 0x141;
        internal const int CB_SHOWDROPDOWN = 0x14f;
        internal const int CBN_CLOSEUP = 8;
        internal const int CBN_DROPDOWN = 7;
        internal const int CBN_KILLFOCUS = 4;
        internal const int CBN_SELCHANGE = 1;
        internal const int CBN_SELENDCANCEL = 10;
        internal const int EM_GETINPUTMODE = 0xdd;
        internal const int EM_SETINPUTMODE = 0xde;
        internal const int GWL_WNDPROC = -4;
        private static bool m_isSmartphone;
        internal const int MK_LBUTTON = 1;
        private const int SPI_GETPLATFORMTYPE = 0x101;
        internal const int SWP_NOMOVE = 2;
        internal const int SWP_NOZORDER = 4;
        internal const int WM_CHAR = 0x102;
        internal const int WM_COMMAND = 0x111;
        internal const int WM_DESTROY = 2;
        internal const int WM_KEYDOWN = 0x100;
        internal const int WM_KEYUP = 0x101;
        internal const int WM_LBUTTONDOWN = 0x201;
        internal const int WM_LBUTTONUP = 0x202;
        internal const int WM_MOUSEMOVE = 0x200;
        internal const int WM_SIZE = 5;

        static Imports()
        {
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                m_isSmartphone = GetSmartphone();
            }
            else
            {
                m_isSmartphone = false;
            }
        }

        [DllImport("coredll.dll")]
        internal static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, int msg, int wParam, int lParam);
        [DllImport("coredll.dll")]
        internal static extern IntPtr GetParent(IntPtr hWnd);
        internal static bool GetSmartphone()
        {
            StringBuilder pvParam = new StringBuilder(50);
            SystemParametersInfo4Strings(0x101, (uint) pvParam.Capacity, pvParam, 0);
            return (pvParam.ToString().ToLower() == "smartphone");
        }

        [DllImport("coredll.dll")]
        internal static extern bool HideCaret(IntPtr hWnd);
        [DllImport("Coredll.dll")]
        internal static extern void MessageBeep(BeepAlert Flags);
        [DllImport("COREDLL.DLL", CharSet=CharSet.Unicode, SetLastError=true)]
        internal static extern int SendMessageW(IntPtr hwnd, uint wMsg, uint wParam, uint lParam);
        internal static void SetInputMode(Control ctrl, InputMode mode)
        {
            try
            {
                if (Environment.OSVersion.Platform == PlatformID.WinCE)
                {
                    SendMessageW(ctrl.Handle, 0xde, 0, (uint) mode);
                }
            }
            catch
            {
            }
        }

        [DllImport("coredll.dll")]
        internal static extern IntPtr SetWindowLong(IntPtr hwnd, int nIndex, IntPtr dwNewLong);
        [DllImport("coredll.dll")]
        internal static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
        [DllImport("coredll.dll")]
        internal static extern bool ShowCaret(IntPtr hWnd);
        [DllImport("Coredll.dll", EntryPoint="SystemParametersInfoW", CharSet=CharSet.Unicode)]
        internal static extern int SystemParametersInfo4Strings(uint uiAction, uint uiParam, StringBuilder pvParam, uint fWinIni);

        internal static bool IsSmartphone
        {
            get
            {
                return m_isSmartphone;
            }
        }

        internal enum BeepAlert
        {
            Exclamation = 0x30
        }

        internal enum InputMode
        {
            Numbers = 2,
            Text = 3
        }

        internal delegate int WindowProcCallback(IntPtr hwnd, int msg, int wParam, int lParam);
    }
}

