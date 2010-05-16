namespace Resco.Controls.OutlookControls
{
    using System;

    public class RollerItemEventArgs : EventArgs
    {
        private Roller m_Item;

        public RollerItemEventArgs(Roller aItem)
        {
            this.m_Item = aItem;
        }

        public Roller Item
        {
            get
            {
                return this.m_Item;
            }
        }
    }
}

