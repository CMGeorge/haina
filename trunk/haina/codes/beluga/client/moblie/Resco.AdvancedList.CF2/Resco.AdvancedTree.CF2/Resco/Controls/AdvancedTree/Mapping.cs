namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Reflection;

    public class Mapping
    {
        private PropertyDescriptorCollection m_cache;
        private Hashtable m_htMap;
        private string[] m_names;
        private static Resco.Controls.AdvancedTree.Mapping s_Empty;

        protected Mapping()
        {
            this.m_htMap = null;
            this.m_names = null;
        }

        public Mapping(IDataRecord reader)
        {
            int fieldCount = reader.FieldCount;
            this.m_htMap = new Hashtable(fieldCount);
            this.m_names = new string[fieldCount];
            for (int i = 0; i < fieldCount; i++)
            {
                string name = reader.GetName(i);
                this.m_names[i] = name;
                try
                {
                    this.m_htMap.Add(name, i);
                }
                catch (Exception exception)
                {
                    throw new ArgumentException("Ambigous column name: " + name, exception);
                }
            }
        }

        public Mapping(string[] names)
        {
            this.m_htMap = new Hashtable(names.Length);
            this.m_names = names;
            for (int i = 0; i < this.m_names.Length; i++)
            {
                try
                {
                    this.m_htMap.Add(this.m_names[i], i);
                }
                catch (Exception exception)
                {
                    throw new ArgumentException("Ambigous column name: " + this.m_names[i], exception);
                }
            }
        }

        public virtual void AddColumns(string[] names)
        {
            if ((this == Empty) || (this.m_names == null))
            {
                throw new NotSupportedException("Cann't change Mapping.Empty");
            }
            string[] array = new string[this.m_names.Length + names.Length];
            this.m_names.CopyTo(array, 0);
            names.CopyTo(array, this.m_names.Length);
            Hashtable hashtable = new Hashtable(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    hashtable.Add(array[i], i);
                }
                catch (Exception exception)
                {
                    throw new ArgumentException("Ambigous column name: " + array[i], exception);
                }
            }
            this.m_htMap = hashtable;
            this.m_names = array;
            this.m_cache = null;
        }

        internal static void DisposeEmptyMapping()
        {
            s_Empty = null;
        }

        public virtual string GetName(int i)
        {
            if (((this.m_names != null) && (i >= 0)) && (i < this.m_names.Length))
            {
                return this.m_names[i];
            }
            return null;
        }

        public virtual int GetOrdinal(string name)
        {
            if (this.m_htMap != null)
            {
                object obj2 = this.m_htMap[name];
                if (obj2 != null)
                {
                    return (int) obj2;
                }
            }
            return -1;
        }

        internal virtual PropertyDescriptorCollection GetPropertyDescriptors()
        {
            if (this.m_cache == null)
            {
                PropertyDescriptor[] properties = new PropertyDescriptor[this.m_names.Length];
                for (int i = 0; i < this.m_names.Length; i++)
                {
                    properties[i] = new NodeDescriptor(this.m_names[i]);
                }
                this.m_cache = new PropertyDescriptorCollection(properties);
            }
            return this.m_cache;
        }

        public static Resco.Controls.AdvancedTree.Mapping Empty
        {
            get
            {
                if (s_Empty == null)
                {
                    s_Empty = new Resco.Controls.AdvancedTree.Mapping();
                }
                return s_Empty;
            }
        }

        public virtual int FieldCount
        {
            get
            {
                if (this.m_names != null)
                {
                    return this.m_names.Length;
                }
                return 0;
            }
        }

        public virtual string this[int i]
        {
            get
            {
                return this.m_names[i];
            }
        }

        public virtual string[] Names
        {
            get
            {
                string[] array = new string[this.FieldCount];
                if (this.m_names != null)
                {
                    this.m_names.CopyTo(array, 0);
                }
                return array;
            }
        }

        private class NodeDescriptor : PropertyDescriptor
        {
            public NodeDescriptor(string name) : base(name, null)
            {
            }

            public override bool CanResetValue(object component)
            {
                return (component is Node);
            }

            public override object GetValue(object component)
            {
                return ((Node) component)[this.Name];
            }

            public override void ResetValue(object component)
            {
                ((Node) component)[this.Name] = null;
            }

            public override void SetValue(object component, object value)
            {
                ((Node) component)[this.Name] = value;
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get
                {
                    return typeof(Node);
                }
            }

            public override bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return typeof(object);
                }
            }
        }
    }
}

