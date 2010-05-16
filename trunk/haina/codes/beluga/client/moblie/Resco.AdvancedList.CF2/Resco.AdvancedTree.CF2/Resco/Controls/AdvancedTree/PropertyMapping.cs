namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public class PropertyMapping : Resco.Controls.AdvancedTree.Mapping
    {
        private PropertyDescriptorCollection m_pdc;

        public PropertyMapping(PropertyDescriptorCollection descriptors)
        {
            if (descriptors == null)
            {
                throw new ArgumentNullException("pdc");
            }
            this.m_pdc = descriptors;
        }

        public PropertyMapping(Type type)
        {
            this.m_pdc = TypeDescriptor.GetProperties(type);
        }

        public override void AddColumns(string[] names)
        {
            throw new NotSupportedException("Cannot add columns to PropertyMapping");
        }

        public override string GetName(int i)
        {
            if ((i >= 0) && (i < this.FieldCount))
            {
                return this.m_pdc[i].Name;
            }
            return null;
        }

        public override int GetOrdinal(string name)
        {
            return this.m_pdc.IndexOf(this.m_pdc[name]);
        }

        internal override PropertyDescriptorCollection GetPropertyDescriptors()
        {
            return this.m_pdc;
        }

        public override int FieldCount
        {
            get
            {
                return this.m_pdc.Count;
            }
        }

        public override string this[int i]
        {
            get
            {
                return this.m_pdc[i].Name;
            }
        }

        public override string[] Names
        {
            get
            {
                string[] strArray = new string[this.FieldCount];
                int num = 0;
                foreach (PropertyDescriptor descriptor in this.m_pdc)
                {
                    strArray[num++] = descriptor.Name;
                }
                return strArray;
            }
        }
    }
}

