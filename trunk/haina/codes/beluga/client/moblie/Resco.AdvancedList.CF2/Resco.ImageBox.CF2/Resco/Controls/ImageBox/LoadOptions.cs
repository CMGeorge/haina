namespace Resco.Controls.ImageBox
{
    using System;

    [Flags]
    public enum LoadOptions
    {
        BestQuality = 0x800,
        FillMaxSize = 0x20,
        FitMaxSize = 0x10,
        FullSize = 0x40,
        None = 0
    }
}

