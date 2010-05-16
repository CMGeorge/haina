namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class GradientColor
    {
        private Color m_endColor;
        private Resco.Controls.OutlookControls.FillDirection m_fillDirection;
        private Color m_startColor;

        public event EventHandler PropertyChanged;

        public GradientColor()
        {
            this.m_startColor = this.m_endColor = Color.Transparent;
            this.m_fillDirection = Resco.Controls.OutlookControls.FillDirection.Horizontal;
        }

        public GradientColor(Resco.Controls.OutlookControls.FillDirection fillDirection) : this()
        {
            this.m_fillDirection = fillDirection;
        }

        public GradientColor(Color startColor, Color endColor)
        {
            this.m_startColor = startColor;
            this.m_endColor = endColor;
            this.m_fillDirection = Resco.Controls.OutlookControls.FillDirection.Horizontal;
        }

        public GradientColor(Color startColor, Color endColor, Resco.Controls.OutlookControls.FillDirection fillDirection)
        {
            this.m_startColor = startColor;
            this.m_endColor = endColor;
            this.m_fillDirection = fillDirection;
        }

        public bool CanDraw()
        {
            return ((this.StartColor != Color.Transparent) && (this.EndColor != Color.Transparent));
        }

        public void DrawGradient(Graphics gr, Rectangle rc)
        {
            GradientFill.Fill(gr, rc, this.StartColor, Color.Transparent, Color.Transparent, this.EndColor, this.FillDirection);
        }

        public void DrawVistaGradient(Graphics gr, Rectangle rc)
        {
            GradientFill.DrawVistaGradient(gr, this.StartColor, rc, this.FillDirection);
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
            return (this.m_endColor != Color.Transparent);
        }

        protected virtual bool ShouldSerializeFillDirection()
        {
            return (this.m_fillDirection != Resco.Controls.OutlookControls.FillDirection.Horizontal);
        }

        protected virtual bool ShouldSerializeStartColor()
        {
            return (this.m_startColor != Color.Transparent);
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
                    value = Color.Transparent;
                }
                this.m_endColor = value;
                this.OnPropertyChanged(EventArgs.Empty);
            }
        }

        public Resco.Controls.OutlookControls.FillDirection FillDirection
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
                    value = Color.Transparent;
                }
                this.m_startColor = value;
                this.OnPropertyChanged(EventArgs.Empty);
            }
        }
    }
}

