namespace Resco.Controls.ScrollBar
{
    using System;
    using System.Drawing;

    public class DrawEventArgs : EventArgs
    {
        internal Rectangle _bounds;
        internal System.Drawing.Graphics _graphics;
        internal ScrollBarElementState _state;

        public DrawEventArgs(System.Drawing.Graphics graphics, Rectangle bounds, ScrollBarElementState state)
        {
            this._graphics = graphics;
            this._bounds = bounds;
            this._state = state;
        }

        public Rectangle Bounds
        {
            get
            {
                return this._bounds;
            }
        }

        public System.Drawing.Graphics Graphics
        {
            get
            {
                return this._graphics;
            }
        }

        public ScrollBarElementState State
        {
            get
            {
                return this._state;
            }
        }
    }
}

