namespace Resco.Controls.AdvancedTree
{
    using System;

    internal enum TreeEventArgsType
    {
        AfterCollapse = 0x36,
        AfterExpand = 0x34,
        BeforeCollapse = 0x35,
        BeforeExpand = 0x33,
        Empty = 0,
        NodeChange = 0x13,
        Refresh = 5,
        Repaint = 1,
        Resize = 3,
        VScroll = 9
    }
}

