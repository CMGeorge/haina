namespace Resco.Controls.DetailView.Design
{
    using System;

    //[AttributeUsage(0x7fff)]
    [AttributeUsage(AttributeTargets.Property)]
    public class BrowsableAttribute : Attribute
    {
        public static readonly BrowsableAttribute Default = Yes;
        private bool m_browsable = true;
        public static readonly BrowsableAttribute No = new BrowsableAttribute(false);
        public static readonly BrowsableAttribute Yes = new BrowsableAttribute(true);

        public BrowsableAttribute(bool browsable)
        {
            this.m_browsable = browsable;
        }

        public override bool Equals(object o)
        {
            if (o == this)
            {
                return true;
            }
            BrowsableAttribute attribute = o as BrowsableAttribute;
            return ((attribute != null) && (attribute.Browsable == this.m_browsable));
        }

        public override int GetHashCode()
        {
            return this.m_browsable.GetHashCode();
        }

        public bool Browsable
        {
            get
            {
                return this.m_browsable;
            }
        }
    }
}

