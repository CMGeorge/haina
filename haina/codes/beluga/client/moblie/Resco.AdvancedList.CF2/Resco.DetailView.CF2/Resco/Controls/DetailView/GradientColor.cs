namespace Resco.Controls.DetailView
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class GradientColor
    {
        private Color m_endColor;
        private Resco.Controls.DetailView.FillDirection m_fillDirection;
        private Color m_startColor;

        public event EventHandler PropertyChanged;

        public GradientColor()
        {
            this.m_startColor = this.m_endColor = Color.White;
            this.m_fillDirection = Resco.Controls.DetailView.FillDirection.Vertical;
        }

        public GradientColor(Color startColor, Color endColor)
        {
            this.m_startColor = startColor;
            this.m_endColor = endColor;
            this.m_fillDirection = Resco.Controls.DetailView.FillDirection.Vertical;
        }

        public GradientColor(Color startColor, Color endColor, Resco.Controls.DetailView.FillDirection fillDirection)
        {
            this.m_startColor = startColor;
            this.m_endColor = endColor;
            this.m_fillDirection = fillDirection;
        }

        public void DrawGradient(Graphics gr, Rectangle rc)
        {
            GradientFill.Fill(gr, rc, rc, this);
        }

        protected virtual void OnPropertyChanged(EventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        protected virtual bool ShouldSerializeEndColor()
        {
            return (this.m_endColor.ToArgb() != Color.White.ToArgb());
        }

        protected virtual bool ShouldSerializeFillDirection()
        {
            return (this.m_fillDirection != Resco.Controls.DetailView.FillDirection.Vertical);
        }

        protected virtual bool ShouldSerializeStartColor()
        {
            return (this.m_startColor.ToArgb() != Color.White.ToArgb());
        }

        public Color EndColor
        {
            get
            {
                return this.m_endColor;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = Color.White;
                }
                this.m_endColor = value;
                this.OnPropertyChanged(EventArgs.Empty);
            }
        }

        public Resco.Controls.DetailView.FillDirection FillDirection
        {
            get
            {
                return this.m_fillDirection;
            }
            set
            {
                this.m_fillDirection = value;
                this.OnPropertyChanged(EventArgs.Empty);
            }
        }

        public Color StartColor
        {
            get
            {
                return this.m_startColor;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = Color.White;
                }
                this.m_startColor = value;
                this.OnPropertyChanged(EventArgs.Empty);
            }
        }
    }
}

