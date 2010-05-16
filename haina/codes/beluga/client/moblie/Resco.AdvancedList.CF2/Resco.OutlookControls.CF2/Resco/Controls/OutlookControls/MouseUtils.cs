namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class MouseUtils
    {
        internal const uint GN_CONTEXTMENU = 0x3e8;
        internal const uint SHRG_RETURNCMD = 1;

        internal static bool IsContextMenu(MouseEventArgs e, IntPtr handle)
        {
            if (e.Button != MouseButtons.Left)
            {
                return false;
            }
            SHRGINFO structure = new SHRGINFO();
            structure.cbSize = (uint) Marshal.SizeOf(structure);
            structure.hwndClient = handle;
            structure.x = e.X;
            structure.y = e.Y;
            structure.dwFlags = 1;
            return (SHRecognizeGesture(structure) == 0x3e8);
        }

        [DllImport("aygshell.dll", SetLastError=true)]
        internal static extern uint SHRecognizeGesture(SHRGINFO shrg);

        [StructLayout(LayoutKind.Sequential)]
        internal class SHRGINFO
        {
            public uint cbSize;
            public IntPtr hwndClient = IntPtr.Zero;
            public int x;
            public int y;
            public uint dwFlags;
        }
    }
}

