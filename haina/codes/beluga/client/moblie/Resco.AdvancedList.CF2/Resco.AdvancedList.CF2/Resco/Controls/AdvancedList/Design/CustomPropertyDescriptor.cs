namespace Resco.Controls.AdvancedList.Design
{
    using Resco.Controls.AdvancedList;
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
            m_defaultValueAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.BorderStyle", new DefaultValueAttribute(BorderStyle.None));
            m_defaultValueAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.AutoScroll", new DefaultValueAttribute(false));
            m_defaultValueAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.AutoScrollMargin", new DefaultValueAttribute("0, 0"));
            m_defaultValueAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Size", new DefaultValueAttribute("200, 200"));
            m_defaultValueAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.ForeColor", new DefaultValueAttribute("ControlText"));
            m_defaultValueAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Visible", new DefaultValueAttribute(true));
            m_defaultValueAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Enabled", new DefaultValueAttribute(true));
            m_defaultValueAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Font", new DefaultValueAttribute("Tahoma, 9pt"));
            m_defaultValueAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.TabStop", new DefaultValueAttribute(true));
            m_defaultValueAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Anchor", new DefaultValueAttribute(AnchorStyles.Left | AnchorStyles.Top));
            m_defaultValueAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Dock", new DefaultValueAttribute(DockStyle.None));
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.BorderStyle", Resco.Controls.AdvancedList.Design.BrowsableAttribute.Yes);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.BindingContext", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.AutoScaleDimensions", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.AutoScaleMode", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.AutoValidate", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.AutoScrollPosition", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Left", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Top", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Bounds", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.ClientSize", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Width", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Height", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Parent", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
            m_browsableAttributes.Add("Resco.Controls.AdvancedList.AdvancedList.Capture", Resco.Controls.AdvancedList.Design.BrowsableAttribute.No);
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
            if (propertyDescriptor.ComponentType == typeof(Resco.Controls.AdvancedList.AdvancedList))
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
                if (this.ShouldSerializeMethodValue == null)
                {
                    return this.Attributes.Contains(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibilityAttribute.Content);
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
                            this.m_defaultValue = Conversion.ColorFromString(this.m_defaultValue as string);
                        }
                        if ((this.PropertyType == typeof(Rectangle)) && (this.m_defaultValue is string))
                        {
                            this.m_defaultValue = Conversion.RectangleFromString(this.m_defaultValue as string);
                        }
                        if ((this.PropertyType == typeof(Size)) && (this.m_defaultValue is string))
                        {
                            this.m_defaultValue = Conversion.SizeFromString(this.m_defaultValue as string);
                        }
                        if ((this.PropertyType == typeof(Point)) && (this.m_defaultValue is string))
                        {
                            this.m_defaultValue = Conversion.PointFromString(this.m_defaultValue as string);
                        }
                        if ((this.PropertyType == typeof(Font)) && (this.m_defaultValue is string))
                        {
                            this.m_defaultValue = Conversion.FontFromString(this.m_defaultValue as string);
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

        public Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility SerializationVisibility
        {
            get
            {
                Attribute attribute = this.Attributes[typeof(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibilityAttribute)];
                if (attribute != null)
                {
                    return ((Resco.Controls.AdvancedList.Design.DesignerSerializationVisibilityAttribute) attribute).Visibility;
                }
                return Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Visible;
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

