namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;

    public class TooltipEventArgs : EventArgs
    {
        internal DateTime _day;
        public Color BackColor;
        public System.Drawing.Font Font;
        public Color ForeColor;
        public string Text;

        public DateTime Day
        {
            get
            {
                return this._day;
            }
        }
    }
}

