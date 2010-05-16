namespace Resco.Controls.ScrollBar
{
    using System;
    using System.Drawing;

    public class DrawTrackEventArgs : DrawEventArgs
    {
        internal ScrollBarTrackType _track;
        internal Rectangle _trackRect;
        internal Rectangle _visibleRect;

        public DrawTrackEventArgs(Graphics graphics, Rectangle bounds, ScrollBarElementState state, Rectangle visibleRect, Rectangle trackRect, ScrollBarTrackType track) : base(graphics, bounds, state)
        {
            this._visibleRect = visibleRect;
            this._trackRect = trackRect;
            this._track = track;
        }

        public Rectangle TrackRectangle
        {
            get
            {
                return this._trackRect;
            }
        }

        public ScrollBarTrackType TrackType
        {
            get
            {
                return this._track;
            }
        }

        public Rectangle VisibleRectangle
        {
            get
            {
                return this._visibleRect;
            }
        }
    }
}

