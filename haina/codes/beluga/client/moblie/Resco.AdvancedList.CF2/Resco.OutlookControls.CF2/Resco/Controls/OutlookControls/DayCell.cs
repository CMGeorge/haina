namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;

    public class DayCell
    {
        private Color m_backColor;
        private DateTime m_date;
        private System.Drawing.Font m_Font;
        private Color m_foreColor;
        private System.Drawing.Image m_Image;
        private Alignment m_imageAlignment;
        private bool m_imageAutoResize;
        private bool m_imageAutoTransparent;
        private Color m_imageTransparentColor;
        private Color m_selBackColor;
        private Color m_selForeColor;
        private object m_Tag;
        private string m_Text;
        private Alignment m_textAlignment;
        private Color m_tooltipBackColor;
        private System.Drawing.Font m_tooltipFont;
        private Color m_tooltipForeColor;
        private string m_tooltipText;
        private BoldedDateType m_type;

        public DayCell(DateTime date) : this(date, BoldedDateType.Nonrecurrent)
        {
        }

        public DayCell(DateTime date, BoldedDateType type)
        {
            this.m_foreColor = Color.Transparent;
            this.m_backColor = Color.Transparent;
            this.m_selForeColor = Color.Transparent;
            this.m_selBackColor = Color.Transparent;
            this.m_imageAlignment = Alignment.BottomRight;
            this.m_imageTransparentColor = Color.Empty;
            this.m_tooltipBackColor = Color.Transparent;
            this.m_tooltipForeColor = Color.Transparent;
            this.m_date = date;
            this.m_type = type;
        }

        public Color BackColor
        {
            get
            {
                return this.m_backColor;
            }
            set
            {
                this.m_backColor = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return this.m_date;
            }
            set
            {
                this.m_date = value;
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this.m_Font;
            }
            set
            {
                this.m_Font = value;
            }
        }

        public Color ForeColor
        {
            get
            {
                return this.m_foreColor;
            }
            set
            {
                this.m_foreColor = value;
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this.m_Image;
            }
            set
            {
                this.m_Image = value;
            }
        }

        public Alignment ImageAlignment
        {
            get
            {
                return this.m_imageAlignment;
            }
            set
            {
                this.m_imageAlignment = value;
            }
        }

        public bool ImageAutoResize
        {
            get
            {
                return this.m_imageAutoResize;
            }
            set
            {
                this.m_imageAutoResize = value;
            }
        }

        public bool ImageAutoTransparent
        {
            get
            {
                return this.m_imageAutoTransparent;
            }
            set
            {
                this.m_imageAutoTransparent = value;
            }
        }

        public Color ImageTransparentColor
        {
            get
            {
                return this.m_imageTransparentColor;
            }
            set
            {
                this.m_imageTransparentColor = value;
            }
        }

        public Color SelBackColor
        {
            get
            {
                return this.m_selBackColor;
            }
            set
            {
                this.m_selBackColor = value;
            }
        }

        public Color SelForeColor
        {
            get
            {
                return this.m_selForeColor;
            }
            set
            {
                this.m_selForeColor = value;
            }
        }

        public object Tag
        {
            get
            {
                return this.m_Tag;
            }
            set
            {
                this.m_Tag = value;
            }
        }

        public string Text
        {
            get
            {
                return this.m_Text;
            }
            set
            {
                this.m_Text = value;
            }
        }

        public Alignment TextAlignment
        {
            get
            {
                return this.m_textAlignment;
            }
            set
            {
                this.m_textAlignment = value;
            }
        }

        public Color TooltipBackColor
        {
            get
            {
                return this.m_tooltipBackColor;
            }
            set
            {
                this.m_tooltipBackColor = value;
            }
        }

        public System.Drawing.Font TooltipFont
        {
            get
            {
                return this.m_tooltipFont;
            }
            set
            {
                this.m_tooltipFont = value;
            }
        }

        public Color TooltipForeColor
        {
            get
            {
                return this.m_tooltipForeColor;
            }
            set
            {
                this.m_tooltipForeColor = value;
            }
        }

        public string TooltipText
        {
            get
            {
                return this.m_tooltipText;
            }
            set
            {
                this.m_tooltipText = value;
            }
        }

        public BoldedDateType Type
        {
            get
            {
                return this.m_type;
            }
            set
            {
                this.m_type = value;
            }
        }
    }
}

