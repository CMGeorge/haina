namespace Resco.Controls.DetailView.Design
{
    using Resco.Controls.DetailView;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    internal class CustomPropertyDescriptor : PropertyDescriptor
    {
        private AttributeCollection m_attributes;
        private static Hashtable m_browsableAttributes = new Hashtable();
        private object m_defaultValue;
        private static Hashtable m_defaultValueAttributes = new Hashtable();
        private static readonly object m_noValue = new object();
        private PropertyDescriptor m_propertyDescriptor;
        private MethodInfo m_shouldSerializeMethod;

        static CustomPropertyDescriptor()
        {
            m_defaultValueAttributes.Add("Resco.Controls.DetailView.DetailView.BorderStyle", new DefaultValueAttribute(BorderStyle.None));
            m_defaultValueAttributes.Add("Resco.Controls.DetailView.DetailView.AutoScroll", new DefaultValueAttribute(false));
            m_defaultValueAttributes.Add("Resco.Controls.DetailView.DetailView.AutoScrollMargin", new DefaultValueAttribute("0, 0"));
            m_defaultValueAttributes.Add("Resco.Controls.DetailView.DetailView.Size", new DefaultValueAttribute("200, 200"));
            m_defaultValueAttributes.Add("Resco.Controls.DetailView.DetailView.ForeColor", new DefaultValueAttribute("ControlText"));
            m_defaultValueAttributes.Add("Resco.Controls.DetailView.DetailView.Visible", new DefaultValueAttribute(true));
            m_defaultValueAttributes.Add("Resco.Controls.DetailView.DetailView.Enabled", new DefaultValueAttribute(true));
            m_defaultValueAttributes.Add("Resco.Controls.DetailView.DetailView.Font", new DefaultValueAttribute("Tahoma, 9pt"));
            m_defaultValueAttributes.Add("Resco.Controls.DetailView.DetailView.TabStop", new DefaultValueAttribute(true));
            m_defaultValueAttributes.Add("Resco.Controls.DetailView.DetailView.Anchor", new DefaultValueAttribute(AnchorStyles.Left | AnchorStyles.Top));
            m_defaultValueAttributes.Add("Resco.Controls.DetailView.DetailView.Dock", new DefaultValueAttribute(DockStyle.None));
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.BorderStyle", Resco.Controls.DetailView.Design.BrowsableAttribute.Yes);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.BindingContext", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.AutoScaleDimensions", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.AutoScaleMode", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.AutoValidate", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.AutoScrollPosition", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.Left", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.Top", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.Bounds", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.ClientSize", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.Width", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.Height", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.Parent", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.DetailView.DetailView.Capture", Resco.Controls.DetailView.Design.BrowsableAttribute.No);
        }

        public CustomPropertyDescriptor(PropertyDescriptor propertyDescriptor) : base(propertyDescriptor.Name, GetCustomAttributes(propertyDescriptor))
        {
            this.m_propertyDescriptor = propertyDescriptor;
            this.m_attributes = base.CreateAttributeCollection();
        }

        public override bool CanResetValue(object component)
        {
            return this.m_propertyDescriptor.CanResetValue(component);
        }

        public static Attribute[] GetCustomAttributes(PropertyDescriptor propertyDescriptor)
        {
            object[] customAttributes = propertyDescriptor.ComponentType.GetProperty(propertyDescriptor.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance).GetCustomAttributes(true);
            List<Attribute> list = new List<Attribute>();
            foreach (Attribute attribute in customAttributes)
            {
                list.Add(attribute);
            }
            string str = propertyDescriptor.ComponentType.FullName + "." + propertyDescriptor.Name;
            if (propertyDescriptor.ComponentType == typeof(Resco.Controls.DetailView.DetailView))
            {
                Attribute attribute2 = m_defaultValueAttributes[str] as Attribute;
                if (attribute2 != null)
                {
                    list.Add(attribute2);
                }
                attribute2 = m_browsableAttributes[str] as Attribute;
                if (attribute2 != null)
                {
                    list.Add(attribute2);
                }
            }
            return list.ToArray();
        }

        public override object GetValue(object component)
        {
            return this.m_propertyDescriptor.GetValue(component);
        }

        public override void ResetValue(object component)
        {
            this.m_propertyDescriptor.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            this.m_propertyDescriptor.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            bool flag = false;
            if (this.m_propertyDescriptor.IsReadOnly)
            {
                if (this.ShouldSerializeMethodValue != null)
                {
                    try
                    {
                        flag = (bool) this.ShouldSerializeMethodValue.Invoke(component, null);
                    }
                    catch
                    {
                    }
                    return flag;
                }
                flag = this.Attributes.Contains(Resco.Controls.DetailView.Design.DesignerSerializationVisibilityAttribute.Content);
                if (!flag)
                {
                    foreach (Attribute attribute in this.Attributes)
                    {
                        if (attribute.ToString().IndexOf("DesignerSerializationVisibilityAttribute") >= 0)
                        {
                            PropertyInfo property = attribute.GetType().GetProperty("Visibility");
                            if (property != null)
                            {
                                return (property.GetValue(attribute, new object[0]).ToString() == "Content");
                            }
                        }
                    }
                }
                return flag;
            }
            if (this.DefaultValue != m_noValue)
            {
                return !object.Equals(this.DefaultValue, this.m_propertyDescriptor.GetValue(component));
            }
            if (this.ShouldSerializeMethodValue == null)
            {
                return true;
            }
            try
            {
                flag = (bool) this.ShouldSerializeMethodValue.Invoke(component, null);
            }
            catch
            {
            }
            return flag;
        }

        public AttributeCollection Attributes
        {
            get
            {
                return this.m_attributes;
            }
        }

        public bool Browsable
        {
            get
            {
                foreach (Attribute attribute in this.Attributes)
                {
                    if (attribute is Resco.Controls.DetailView.Design.BrowsableAttribute)
                    {
                        return ((Resco.Controls.DetailView.Design.BrowsableAttribute) attribute).Browsable;
                    }
                    if (attribute.ToString().IndexOf("BrowsableAttribute") >= 0)
                    {
                        PropertyInfo property = attribute.GetType().GetProperty("Browsable");
                        if (property != null)
                        {
                            return (bool) property.GetValue(attribute, null);
                        }
                    }
                }
                return true;
            }
        }

        public override Type ComponentType
        {
            get
            {
                return this.m_propertyDescriptor.ComponentType;
            }
        }

        public object DefaultValue
        {
            get
            {
                if (this.m_defaultValue == null)
                {
                    Attribute attribute = this.Attributes[typeof(DefaultValueAttribute)];
                    if (attribute != null)
                    {
                        this.m_defaultValue = ((DefaultValueAttribute) attribute).Value;
                        if ((this.PropertyType == typeof(Color)) && (this.m_defaultValue is string))
                        {
                            this.m_defaultValue = DVConversion.ColorFromString(this.m_defaultValue as string);
                        }
                        if ((this.PropertyType == typeof(Rectangle)) && (this.m_defaultValue is string))
                        {
                            this.m_defaultValue = DVConversion.RectangleFromString(this.m_defaultValue as string);
                        }
                        if ((this.PropertyType == typeof(Size)) && (this.m_defaultValue is string))
                        {
                            this.m_defaultValue = DVConversion.SizeFromString(this.m_defaultValue as string);
                        }
                        if ((this.PropertyType == typeof(Point)) && (this.m_defaultValue is string))
                        {
                            this.m_defaultValue = DVConversion.PointFromString(this.m_defaultValue as string);
                        }
                        if ((this.PropertyType == typeof(Font)) && (this.m_defaultValue is string))
                        {
                            this.m_defaultValue = DVConversion.FontFromString(this.m_defaultValue as string);
                        }
                        if ((this.PropertyType == typeof(DateTime)) && (this.m_defaultValue is string))
                        {
                            this.m_defaultValue = DVConversion.DateTimeFromString(this.m_defaultValue as string);
                        }
                        if ((this.PropertyType == typeof(decimal)) && (this.m_defaultValue is int))
                        {
                            this.m_defaultValue = Convert.ToDecimal(this.m_defaultValue);
                        }
                    }
                    else
                    {
                        this.m_defaultValue = m_noValue;
                    }
                }
                return this.m_defaultValue;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return this.m_propertyDescriptor.IsReadOnly;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.m_propertyDescriptor.PropertyType;
            }
        }

        public Resco.Controls.DetailView.Design.DesignerSerializationVisibility SerializationVisibility
        {
            get
            {
                foreach (Attribute attribute in this.Attributes)
                {
                    if (attribute is Resco.Controls.DetailView.Design.DesignerSerializationVisibilityAttribute)
                    {
                        return ((Resco.Controls.DetailView.Design.DesignerSerializationVisibilityAttribute) attribute).Visibility;
                    }
                    if (attribute.ToString().IndexOf("DesignerSerializationVisibilityAttribute") >= 0)
                    {
                        PropertyInfo property = attribute.GetType().GetProperty("Visibility");
                        if (property != null)
                        {
                            return (Resco.Controls.DetailView.Design.DesignerSerializationVisibility) property.GetValue(attribute, null);
                        }
                    }
                }
                return Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Visible;
            }
        }

        private MethodInfo ShouldSerializeMethodValue
        {
            get
            {
                if (this.m_shouldSerializeMethod == null)
                {
                    try
                    {
                        this.m_shouldSerializeMethod = MemberDescriptor.FindMethod(this.ComponentType, "ShouldSerialize" + this.m_propertyDescriptor.Name, new Type[0], typeof(bool), false);
                    }
                    catch
                    {
                    }
                }
                return this.m_shouldSerializeMethod;
            }
        }
    }
}

