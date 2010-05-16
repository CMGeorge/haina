namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class HTCNavSensorRotatedEventArgs : EventArgs
    {
        private double m_radialDelta;
        private double m_rotationsPerSecond;

        public HTCNavSensorRotatedEventArgs(double rotationsPerSecond, double radialDelta)
        {
            this.m_rotationsPerSecond = rotationsPerSecond;
            this.m_radialDelta = radialDelta;
        }

        public double RadialDelta
        {
            get
            {
                return this.m_radialDelta;
            }
            internal set
            {
                this.m_radialDelta = value;
            }
        }

        public double RotationsPerSecond
        {
            get
            {
                return this.m_rotationsPerSecond;
            }
            internal set
            {
                this.m_rotationsPerSecond = value;
            }
        }
    }
}

