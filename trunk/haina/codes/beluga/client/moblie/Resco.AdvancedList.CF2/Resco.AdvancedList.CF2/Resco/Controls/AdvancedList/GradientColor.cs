namespace Resco.Controls.AdvancedList
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class GradientColor
    {
        private Color m_endColor;
        private Resco.Controls.AdvancedList.FillDirection m_fillDirection;
        private Color m_middleColor1;
        private Color m_middleColor2;
        private Color m_startColor;

        public event EventHandler PropertyChanged;

        public GradientColor()
        {
            this.m_startColor = this.m_endColor = Color.Transparent;
            this.m_middleColor1 = Color.Transparent;
            this.m_middleColor2 = Color.Transparent;
            this.m_fillDirection = Resco.Controls.AdvancedList.FillDirection.Horizontal;
        }

        public GradientColor(Resco.Controls.AdvancedList.FillDirection fillDirection) : this()
        {
            this.m_fillDirection = fillDirection;
        }

        public GradientColor(Color startColor, Color endColor)
        {
            this.m_startColor = startColor;
            this.m_endColor = endColor;
            this.m_middleColor1 = Color.Transparent;
            this.m_middleColor2 = Color.Transparent;
            this.m_fillDirection = Resco.Controls.AdvancedList.FillDirection.Horizontal;
        }

        public GradientColor(Color startColor, Color endColor, Resco.Controls.AdvancedList.FillDirection fillDirection)
        {
            this.m_startColor = startColor;
            this.m_endColor = endColor;
            this.m_middleColor1 = Color.Transparent;
            this.m_middleColor2 = Color.Transparent;
            this.m_fillDirection = fillDirection;
        }

        public GradientColor(Color startColor, Color middleColor1, Color middleColor2, Color endColor, Resco.Controls.AdvancedList.FillDirection fillDirection)
        {
            this.m_startColor = startColor;
            this.m_endColor = endColor;
            this.m_middleColor1 = middleColor1;
            this.m_middleColor2 = middleColor2;
            this.m_fillDirection = fillDirection;
        }

        public bool CanDraw()
        {
            return ((this.StartColor != Color.Transparent) && (this.EndColor != Color.Transparent));
        }

        public void DrawGradient(Graphics gr, Rectangle rc)
        {
            GradientFill.Fill(gr, rc, this.StartColor, this.MiddleColor1, this.MiddleColor2, this.EndColor, this.FillDirection);
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
            return (this.m_fillDirection != Resco.Controls.AdvancedList.FillDirection.Horizontal);
        }

        protected virtual bool ShouldSerializeMiddleColor1()
        {
            return (this.m_middleColor1.ToArgb() != Color.Transparent.ToArgb());
        }

        protected virtual bool ShouldSerializeMiddleColor2()
        {
            return (this.m_middleColor2.ToArgb() != Color.Transparent.ToArgb());
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

        public Resco.Controls.AdvancedList.FillDirection FillDirection
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

        public Color MiddleColor1
        {
            get
            {
                return this.m_middleColor1;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = Color.Transparent;
                }
                this.m_middleColor1 = value;
                this.OnPropertyChanged(EventArgs.Empty);
            }
        }

        public Color MiddleColor2
        {
            get
            {
                return this.m_middleColor2;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = Color.Transparent;
                }
                this.m_middleColor2 = value;
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

