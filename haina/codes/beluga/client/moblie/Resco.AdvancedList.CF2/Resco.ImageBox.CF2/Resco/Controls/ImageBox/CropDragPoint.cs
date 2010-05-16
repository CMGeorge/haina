namespace Resco.Controls.ImageBox
{
    using System;

    [Flags]
    internal enum CropDragPoint
    {
        Bottom = 8,
        Left = 1,
        Middle = 0x10,
        None = 0,
        Right = 2,
        Top = 4
    }
}

