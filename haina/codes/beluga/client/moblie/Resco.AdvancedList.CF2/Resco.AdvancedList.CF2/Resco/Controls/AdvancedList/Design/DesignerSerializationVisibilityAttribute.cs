namespace Resco.Controls.AdvancedList.Design
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class DesignerSerializationVisibilityAttribute : Attribute
    {
        public static readonly DesignerSerializationVisibilityAttribute Content = new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content);
        public static readonly DesignerSerializationVisibilityAttribute Default = Visible;
        public static readonly DesignerSerializationVisibilityAttribute Hidden = new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden);
        private DesignerSerializationVisibility m_visibility;
        public static readonly DesignerSerializationVisibilityAttribute Visible = new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible);

        public DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility visiblity)
        {
            this.m_visibility = visiblity;
        }

        public override bool Equals(object o)
        {
            if (o == this)
            {
                return true;
            }
            DesignerSerializationVisibilityAttribute attribute = o as DesignerSerializationVisibilityAttribute;
            return ((attribute != null) && (attribute.Visibility == this.m_visibility));
        }

        public override int GetHashCode()
        {
            return this.m_visibility.GetHashCode();
        }

        public DesignerSerializationVisibility Visibility
        {
            get
            {
                return this.m_visibility;
            }
        }
    }
}

