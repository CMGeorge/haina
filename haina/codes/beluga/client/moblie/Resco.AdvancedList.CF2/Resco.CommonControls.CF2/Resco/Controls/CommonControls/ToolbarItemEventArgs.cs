namespace Resco.Controls.CommonControls
{
    using System;

    public class ToolbarItemEventArgs : EventArgs
    {
        private ToolbarItem m_Item;

        public ToolbarItemEventArgs(ToolbarItem aItem)
        {
            this.m_Item = aItem;
        }

        public ToolbarItem Item
        {
            get
            {
                return this.m_Item;
            }
        }
    }
}

