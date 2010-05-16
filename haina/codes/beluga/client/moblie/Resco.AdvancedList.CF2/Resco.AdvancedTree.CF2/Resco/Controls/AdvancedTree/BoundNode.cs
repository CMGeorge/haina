namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Reflection;

    public class BoundNode : Node
    {
        private object m_data;
        private PropertyDescriptorCollection m_properties;

        public BoundNode(System.Data.DataRow row) : this(0, 0, row, new TableMapping(row.Table))
        {
        }

        public BoundNode(int templateIndex, int selectedTemplateIndex, System.Data.DataRow row) : this(templateIndex, selectedTemplateIndex, row, new TableMapping(row.Table))
        {
        }

        public BoundNode(int templateIndex, int selectedTemplateIndex, System.Data.DataRow row, Resco.Controls.AdvancedTree.Mapping fieldNames) : base(templateIndex, selectedTemplateIndex, fieldNames)
        {
            this.m_data = row;
            this.m_properties = ((ITypedList) row.Table.DefaultView).GetItemProperties(null);
            try
            {
                int index = row.Table.Rows.IndexOf(row);
                row.Table.DefaultView[index].PropertyChanged+=(new PropertyChangedEventHandler(this.OnBindPropertyChanged));
            }
            catch
            {
            }
        }

        public BoundNode(int templateIndex, int selectedTemplateIndex, object data, PropertyMapping propertyMapping) : base(templateIndex, selectedTemplateIndex, 0)
        {
            base.FieldNames = propertyMapping;
            this.m_data = data;
            this.m_properties = propertyMapping.GetPropertyDescriptors();
            INotifyPropertyChanged changed = this.m_data as INotifyPropertyChanged;
            if (changed != null)
            {
                changed.PropertyChanged+=(new PropertyChangedEventHandler(this.OnBindPropertyChanged));
            }
        }

        public override void CopyTo(Array array, int index)
        {
            if (this.m_data is System.Data.DataRow)
            {
                ((System.Data.DataRow) this.m_data).ItemArray.CopyTo(array, index);
            }
            int fieldCount = this.FieldCount;
            object[] objArray = new object[fieldCount];
            for (int i = 0; i < fieldCount; i++)
            {
                objArray[i] = this.m_properties[i].GetValue(this.m_data);
            }
            objArray.CopyTo(array, index);
        }

        public override void GetData(object[] data)
        {
            if (this.m_data is System.Data.DataRow)
            {
                ((System.Data.DataRow) this.m_data).ItemArray.CopyTo(data, 0);
            }
            else
            {
                int fieldCount = this.FieldCount;
                if (data.Length != fieldCount)
                {
                    throw new ArgumentException("Invalid field count");
                }
                for (int i = 0; i < fieldCount; i++)
                {
                    data[i] = this.m_properties[i].GetValue(this.m_data);
                }
            }
        }

        public override IEnumerator GetEnumerator()
        {
            if (this.m_data is System.Data.DataRow)
            {
                return ((System.Data.DataRow) this.m_data).ItemArray.GetEnumerator();
            }
            int fieldCount = this.FieldCount;
            object[] objArray = new object[fieldCount];
            for (int i = 0; i < fieldCount; i++)
            {
                objArray[i] = this.m_properties[i].GetValue(this.m_data);
            }
            return objArray.GetEnumerator();
        }

        private void OnBindPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnNodeChanged(NodeEventArgsType.Empty, null);
        }

        public override void SetData(ICollection data)
        {
            if (!(this.m_data is System.Data.DataRow))
            {
                throw new NotSupportedException();
            }
            if (data.Count != this.FieldCount)
            {
                throw new ArgumentException("Invalid field count");
            }
            object[] array = new object[data.Count];
            data.CopyTo(array, 0);
            (this.m_data as System.Data.DataRow).ItemArray = array;
            base.OnNodeChanged(NodeEventArgsType.Empty, null);
        }

        public override void SetData(IDataRecord reader)
        {
            if (!(this.m_data is System.Data.DataRow))
            {
                throw new NotSupportedException();
            }
            if (reader.FieldCount != this.FieldCount)
            {
                throw new ArgumentException("Invalid field count");
            }
            object[] values = new object[reader.FieldCount];
            reader.GetValues(values);
            (this.m_data as System.Data.DataRow).ItemArray = values;
            base.OnNodeChanged(NodeEventArgsType.Empty, null);
        }

        public object Data
        {
            get
            {
                return this.m_data;
            }
        }

        public System.Data.DataRow DataRow
        {
            get
            {
                if (this.m_data is System.Data.DataRow)
                {
                    return (System.Data.DataRow) this.m_data;
                }
                if (this.m_data is DataRowView)
                {
                    return ((DataRowView) this.m_data).Row;
                }
                return null;
            }
        }

        public override int FieldCount
        {
            get
            {
                return this.m_properties.Count;
            }
        }

        public override object this[string name]
        {
            get
            {
                try
                {
                    if (this.m_data is System.Data.DataRow)
                    {
                        return ((System.Data.DataRow) this.m_data)[name];
                    }
                    return this.m_properties[name].GetValue(this.m_data);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    if (this[name] != value)
                    {
                        if (this.m_data is System.Data.DataRow)
                        {
                            ((System.Data.DataRow) this.m_data)[name] = value;
                        }
                        else
                        {
                            this.m_properties[name].SetValue(this.m_data, value);
                        }
                        base.OnNodeChanged(NodeEventArgsType.Empty, null);
                    }
                }
                catch
                {
                }
            }
        }

        public override object this[int index]
        {
            get
            {
                try
                {
                    if (this.m_data is System.Data.DataRow)
                    {
                        return ((System.Data.DataRow) this.m_data)[index];
                    }
                    return this.m_properties[index].GetValue(this.m_data);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    if (this[index] != value)
                    {
                        if (this.m_data is System.Data.DataRow)
                        {
                            ((System.Data.DataRow) this.m_data)[index] = value;
                        }
                        else
                        {
                            this.m_properties[index].SetValue(this.m_data, value);
                        }
                        base.OnNodeChanged(NodeEventArgsType.Empty, null);
                    }
                }
                catch
                {
                }
            }
        }
    }
}

