namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;

    public class DayCellEventArgs : EventArgs
    {
        internal DateTime _day;
        internal bool _inactive;
        internal bool _selected;
        public Color BackColor;
        public System.Drawing.Font Font;
        public Color ForeColor;
        public System.Drawing.Image Image;
        public Alignment ImageAlignment;
        public bool ImageAutoResize;
        public bool ImageAutoTransparent;
        public Color ImageTransparentColor;
        public string Text;
        public Alignment TextAlignment;

        public DateTime Day
        {
            get
            {
                return this._day;
            }
        }

        public bool Inactive
        {
            get
            {
                return this._inactive;
            }
        }

        public bool Selected
        {
            get
            {
                return this._selected;
            }
        }
    }
}

