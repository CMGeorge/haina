namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Runtime.InteropServices;

    internal class ContextMenuSupport
    {
        public static bool RecognizeGesture(IntPtr handle, int x, int y)
        {
            SHRGINFO shrg = new SHRGINFO();
            shrg.cbSize = 20;
            shrg.hwndClient = handle;
            shrg.dwFlags = 3;
            shrg.ptDownx = x;
            shrg.ptDowny = y;
            return (SHRecognizeGesture(ref shrg) == 0x3e8);
        }

        [DllImport("aygshell.dll")]
        internal static extern int SHRecognizeGesture(ref SHRGINFO shrg);

        [StructLayout(LayoutKind.Sequential)]
        internal struct SHRGINFO
        {
            public int cbSize;
            public IntPtr hwndClient;
            public int ptDownx;
            public int ptDowny;
            public int dwFlags;
        }
    }
}

