namespace Resco.Controls.AdvancedComboBox.Design
{
    using System;
    using System.ComponentModel;

    internal class RTImageListSite : ISite, IServiceProvider
    {
        private IComponent m_Component;
        private string m_Name;

        public RTImageListSite(IComponent c, string n)
        {
            this.m_Component = c;
            this.m_Name = n;
        }

        public virtual object GetService(Type serviceType)
        {
            return null;
        }

        public virtual IComponent Component
        {
            get
            {
                return this.m_Component;
            }
        }

        public virtual IContainer Container
        {
            get
            {
                return null;
            }
        }

        public virtual bool DesignMode
        {
            get
            {
                return false;
            }
        }

        public virtual string Name
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
    }
}

