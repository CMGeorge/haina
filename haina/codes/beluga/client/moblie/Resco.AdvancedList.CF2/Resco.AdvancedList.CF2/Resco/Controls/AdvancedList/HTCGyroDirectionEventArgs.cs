namespace Resco.Controls.AdvancedList
{
    using System;

    public class HTCGyroDirectionEventArgs : EventArgs
    {
        private HTCDirection m_direction;

        public HTCGyroDirectionEventArgs(HTCDirection dir)
        {
            this.m_direction = dir;
        }

        public HTCDirection Direction
        {
            get
            {
                return this.m_direction;
            }
            internal set
            {
                this.m_direction = value;
            }
        }
    }
}

