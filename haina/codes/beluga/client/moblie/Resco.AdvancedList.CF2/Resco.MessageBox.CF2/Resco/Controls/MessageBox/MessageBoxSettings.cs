namespace Resco.Controls.MessageBox
{
    using System;
    using System.Drawing;

    public class MessageBoxSettings
    {
        private Color background;
        private Color foreground;
        private Color lineColor;
        private Font textFont;
        private Color titleBackground;
        private Font titleFont;
        private Color titleForeground;

        public Color Background
        {
            get
            {
                return this.background;
            }
            set
            {
                this.background = value;
            }
        }

        public Color Foreground
        {
            get
            {
                return this.foreground;
            }
            set
            {
                this.foreground = value;
            }
        }

        public Color LineColor
        {
            get
            {
                return this.lineColor;
            }
            set
            {
                this.lineColor = value;
            }
        }

        public Font TextFont
        {
            get
            {
                return this.textFont;
            }
            set
            {
                this.textFont = value;
            }
        }

        public Color TitleBackground
        {
            get
            {
                return this.titleBackground;
            }
            set
            {
                this.titleBackground = value;
            }
        }

        public Font TitleFont
        {
            get
            {
                return this.titleFont;
            }
            set
            {
                this.titleFont = value;
            }
        }

        public Color TitleForeground
        {
            get
            {
                return this.titleForeground;
            }
            set
            {
                this.titleForeground = value;
            }
        }
    }
}

