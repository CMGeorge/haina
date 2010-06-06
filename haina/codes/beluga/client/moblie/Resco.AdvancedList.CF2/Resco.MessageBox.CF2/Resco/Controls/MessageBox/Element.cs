namespace Resco.Controls.MessageBox
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class Element
    {
        private Color backColor;
        private System.Drawing.Font font;
        private Color foreColor;
        private Rectangle m_bounds = new Rectangle(0, 0, 100, 100);
        protected Control m_parent;
        protected int m_scale;

        public Element(Control parent, int scale)
        {
            this.m_parent = parent;
            this.m_scale = scale;
            this.ForeColor = Color.Black;
            this.BackColor = Color.White;
            this.Font = new System.Drawing.Font("Tahoma", 10f, FontStyle.Regular);
        }

        public void Invalidate()
        {
            this.m_parent.Invalidate(this.Bounds);
        }

        public virtual void OnResize()
        {
        }

        public virtual Color BackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                this.backColor = value;
            }
        }

        public int Bottom
        {
            get
            {
                return this.m_bounds.Bottom;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return this.m_bounds;
            }
            set
            {
                this.m_bounds = value;
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this.font;
            }
            set
            {
                this.font = value;
            }
        }

        public virtual Color ForeColor
        {
            get
            {
                return this.foreColor;
            }
            set
            {
                this.foreColor = value;
            }
        }

        public int Height
        {
            get
            {
                return this.m_bounds.Height;
            }
            set
            {
                this.m_bounds.Height = value;
            }
        }

        public int Left
        {
            get
            {
                return this.m_bounds.X;
            }
            set
            {
                this.m_bounds.X = value;
            }
        }

        public Point Location
        {
            get
            {
                return this.m_bounds.Location;
            }
        }

        public int Right
        {
            get
            {
                return this.m_bounds.Right;
            }
        }

        public int Top
        {
            get
            {
                return this.m_bounds.Y;
            }
            set
            {
                this.m_bounds.Y = value;
            }
        }

        public int Width
        {
            get
            {
                return this.m_bounds.Width;
            }
            set
            {
                this.m_bounds.Width = value;
            }
        }
    }
}

