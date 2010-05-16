namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class LinkCell : TextCell
    {
        private Color m_acolor;
        private bool m_bActive;
        private string m_linkFormat;
        private Color m_vcolor;

        public LinkCell()
        {
            this.m_acolor = Color.Yellow;
            this.m_vcolor = Color.Gray;
            this.m_linkFormat = "{0}";
            base.TextFont = new Font("Tahoma", 8f, FontStyle.Underline);
            base.ForeColor = Color.Blue;
        }

        public LinkCell(LinkCell cell) : base(cell)
        {
            this.m_acolor = Color.Yellow;
            this.m_vcolor = Color.Gray;
            this.m_linkFormat = "{0}";
            base.TextFont = new Font("Tahoma", 8f, FontStyle.Underline);
            base.ForeColor = cell.ForeColor;
            this.m_acolor = cell.m_acolor;
            this.m_vcolor = cell.m_vcolor;
            this.m_linkFormat = cell.m_linkFormat;
        }

        public override Cell Clone()
        {
            return new LinkCell(this);
        }

        protected internal virtual void DrawActiveLink(Graphics gr, LinkEventArgs lea, int w)
        {
            this.m_bActive = false;
        }

        protected override void DrawTextLine(Graphics gr, string line, Font font, Brush brush, int x, int y, int width, int height, int textIndex)
        {
            if ((line != null) && (line.Length > 0))
            {
                base.Parent.AddLinkArea(new Rectangle(x, y, width, height));
            }
            base.DrawTextLine(gr, line, font, brush, x, y, width, height, textIndex);
        }

        protected override Color GetColor(ColorCategory c)
        {
            if (c == ColorCategory.Foreground)
            {
                Color transparentColor = Resco.Controls.AdvancedComboBox.AdvancedComboBox.TransparentColor;
                if (this.m_bActive)
                {
                    transparentColor = this.ActiveColor;
                }
                else if (Links.IsVisited(this.GetLink(base.CurrentData)))
                {
                    transparentColor = this.VisitedColor;
                }
                if (transparentColor != Resco.Controls.AdvancedComboBox.AdvancedComboBox.TransparentColor)
                {
                    return transparentColor;
                }
            }
            return base.GetColor(c);
        }

        protected internal virtual string GetLink(object data)
        {
            if ((data != null) && (data != DBNull.Value))
            {
                return string.Format(this.m_linkFormat, data);
            }
            return "";
        }

        [DefaultValue("Yellow")]
        public Color ActiveColor
        {
            get
            {
                return this.m_acolor;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = Color.Yellow;
                }
                if (value != this.m_acolor)
                {
                    this.m_acolor = value;
                    base.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                }
            }
        }

        [DefaultValue("Blue")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = Color.Blue;
                }
                base.ForeColor = value;
            }
        }

        [DefaultValue("Tahoma, 8pt, style=Underline")]
        public override Font TextFont
        {
            get
            {
                return base.TextFont;
            }
            set
            {
                base.TextFont = value;
            }
        }

        [DefaultValue("{0}")]
        public virtual string UrlFormatString
        {
            get
            {
                return this.m_linkFormat;
            }
            set
            {
                this.m_linkFormat = value;
            }
        }

        [DefaultValue("Gray")]
        public Color VisitedColor
        {
            get
            {
                return this.m_vcolor;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = Color.Gray;
                }
                if (this.m_vcolor != value)
                {
                    this.m_vcolor = value;
                    base.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                }
            }
        }
    }
}

