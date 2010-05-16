namespace Resco.Controls.AdvancedList
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Reflection;

    public class TableMapping : Resco.Controls.AdvancedList.Mapping
    {
        private System.Data.DataTable m_table;

        public TableMapping(System.Data.DataTable table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            this.m_table = table;
        }

        public override void AddColumns(string[] names)
        {
            foreach (string str in names)
            {
                this.m_table.Columns.Add(str);
            }
        }

        public override string GetName(int i)
        {
            if ((i >= 0) && (i < this.FieldCount))
            {
                return this.m_table.Columns[i].ColumnName;
            }
            return null;
        }

        public override int GetOrdinal(string name)
        {
            return this.m_table.Columns.IndexOf(name);
        }

        internal override PropertyDescriptorCollection GetPropertyDescriptors()
        {
            return ((ITypedList) this.m_table.DefaultView).GetItemProperties(null);
        }

        public System.Data.DataTable DataTable
        {
            get
            {
                return this.m_table;
            }
        }

        public override int FieldCount
        {
            get
            {
                return this.m_table.Columns.Count;
            }
        }

        public override string this[int i]
        {
            get
            {
                return this.m_table.Columns[i].ColumnName;
            }
        }

        public override string[] Names
        {
            get
            {
                string[] strArray = new string[this.FieldCount];
                int num = 0;
                foreach (DataColumn column in this.m_table.Columns)
                {
                    strArray[num++] = column.ColumnName;
                }
                return strArray;
            }
        }
    }
}

