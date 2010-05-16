namespace Resco.Controls.CommonControls
{
    using System;
    using System.Windows.Forms;

    public class TabPage : Panel
    {
        private string m_Name;
        private ToolbarItem m_TabItem;

        public TabPage()
        {
            base.Text = "";
            this.m_TabItem = new ToolbarItem();
        }

        protected virtual bool ShouldSerializeTabItem()
        {
            return (this.m_TabItem != null);
        }

        protected virtual bool ShouldSerializeVisible()
        {
            return true;
        }

        protected virtual bool ShouldSerializeVisiblePanel()
        {
            return true;
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }

        public Resco.Controls.CommonControls.TabControl Owner
        {
            get
            {
                return (base.Parent as Resco.Controls.CommonControls.TabControl);
            }
        }

        public ToolbarItem TabItem
        {
            get
            {
                return this.m_TabItem;
            }
            set
            {
                this.m_TabItem = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this.m_TabItem.Visible;
            }
            set
            {
                this.m_TabItem.Visible = value;
                this.VisiblePanel = false;
            }
        }

        internal bool VisiblePanel
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }
    }
}

