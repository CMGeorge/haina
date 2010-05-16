namespace Resco.Controls.ScrollBar
{
    using System;
    using System.Drawing;

    public class DrawArrowButtonEventArgs : DrawEventArgs
    {
        internal ScrollBarArrow _type;

        public DrawArrowButtonEventArgs(Graphics graphics, Rectangle bounds, ScrollBarElementState state, ScrollBarArrow type) : base(graphics, bounds, state)
        {
            this._type = type;
        }

        public ScrollBarArrow Type
        {
            get
            {
                return this._type;
            }
        }
    }
}

