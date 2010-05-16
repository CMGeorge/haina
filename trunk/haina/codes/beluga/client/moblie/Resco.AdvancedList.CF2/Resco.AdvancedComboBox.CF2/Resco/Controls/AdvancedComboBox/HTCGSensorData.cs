namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct HTCGSensorData
    {
        public short TiltX;
        public short TiltY;
        public short TiltZ;
        public short Unknown1;
        public int AngleY;
        public int AngleX;
        public int ScreenOrientation;
    }
}

