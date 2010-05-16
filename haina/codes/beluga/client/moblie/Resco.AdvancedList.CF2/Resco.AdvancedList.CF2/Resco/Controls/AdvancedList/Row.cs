namespace Resco.Controls.AdvancedList
{
    using Resco.Controls.AdvancedList.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class Row : ICollection, IEnumerable, ITypedList
    {
        protected internal int ActualHeight;
        private bool m_bActive;
        private bool m_bSelected;
        private Resco.Controls.AdvancedList.Mapping m_htMap;
        protected int m_iActiveTemplate;
        protected int m_iAlternateTemplate;
        protected internal int m_index;
        protected int m_iSelectedTemplate;
        protected int m_iTemplate;
        private object[] m_oData;
        private RowCollection m_pParent;
        protected int m_pressedButtonIndex;
        private bool m_recalculationNeeded;
        private Hashtable m_rowSpecificCellProperties;
        private object m_tag;

        internal event RowChangedEventHandler RowChanged;

        public Row() : this(0, 0, Resco.Controls.AdvancedList.Mapping.Empty)
        {
        }

        public Row(Resco.Controls.AdvancedList.Mapping fieldNames) : this(0, 0, fieldNames)
        {
        }

        public Row(Row toCopy) : this(toCopy.TemplateIndex, toCopy.SelectedTemplateIndex, toCopy, toCopy.FieldNames)
        {
        }

        public Row(int fieldCount) : this(0, 0, fieldCount)
        {
        }

        protected Row(int templateIndex, int selectedTemplateIndex) : this(templateIndex, selectedTemplateIndex, -1, -1)
        {
        }

        public Row(int templateIndex, int selectedTemplateIndex, Resco.Controls.AdvancedList.Mapping fieldNames) : this(templateIndex, selectedTemplateIndex, -1, -1)
        {
            this.m_htMap = fieldNames;
            this.m_oData = new object[fieldNames.FieldCount];
        }

        public Row(int templateIndex, int selectedTemplateIndex, ICollection dataList) : this(templateIndex, selectedTemplateIndex, dataList, Resco.Controls.AdvancedList.Mapping.Empty)
        {
        }

        public Row(int templateIndex, int selectedTemplateIndex, int fieldCount) : this(templateIndex, selectedTemplateIndex, -1, -1)
        {
            this.m_oData = new object[fieldCount];
            this.m_htMap = Resco.Controls.AdvancedList.Mapping.Empty;
        }

        public Row(int templateIndex, int selectedTemplateIndex, ICollection dataList, Resco.Controls.AdvancedList.Mapping fieldNames) : this(templateIndex, selectedTemplateIndex, -1, -1)
        {
            int num = Math.Max(fieldNames.FieldCount, dataList.Count);
            this.m_htMap = fieldNames;
            this.m_oData = new object[num];
            dataList.CopyTo(this.m_oData, 0);
        }

        protected Row(int templateIndex, int selectedTemplateIndex, int activeTempalteIndex, int alternateTemplateIndex)
        {
            this.ActualHeight = -1;
            this.m_recalculationNeeded = true;
            this.m_index = -1;
            this.m_pressedButtonIndex = -1;
            this.m_iTemplate = templateIndex;
            this.m_iSelectedTemplate = selectedTemplateIndex;
            this.m_iActiveTemplate = activeTempalteIndex;
            this.m_iAlternateTemplate = alternateTemplateIndex;
            this.m_htMap = null;
            this.m_oData = null;
            this.m_rowSpecificCellProperties = new Hashtable();
        }

        public Row(int templateIndex, int selectedTemplateIndex, int activeTempalteIndex, int alternateTempalteIndex, ICollection dataList) : this(templateIndex, selectedTemplateIndex, activeTempalteIndex, alternateTempalteIndex, dataList, Resco.Controls.AdvancedList.Mapping.Empty)
        {
        }

        public Row(int templateIndex, int selectedTemplateIndex, int activeTempalteIndex, int alternateTempalteIndex, ICollection dataList, Resco.Controls.AdvancedList.Mapping fieldNames) : this(templateIndex, selectedTemplateIndex, activeTempalteIndex, alternateTempalteIndex)
        {
            int num = Math.Max(fieldNames.FieldCount, dataList.Count);
            this.m_htMap = fieldNames;
            this.m_oData = new object[num];
            dataList.CopyTo(this.m_oData, 0);
        }

        public Row(int templateIndex, int selectedTemplateIndex, int activeTempalteIndex, int alternateTempalteIndex, string tag, ICollection dataList) : this(templateIndex, selectedTemplateIndex, activeTempalteIndex, alternateTempalteIndex, dataList, Resco.Controls.AdvancedList.Mapping.Empty)
        {
            this.m_tag = tag;
        }

        internal void ChangeSelection(bool bNewSelectedValue, bool bNewActiveValue, bool bHandleStateChange)
        {
            int currentTemplateIndex = this.CurrentTemplateIndex;
            bool bSelected = this.m_bSelected;
            bool bActive = this.m_bActive;
            this.m_bSelected = bNewSelectedValue;
            this.m_bActive = bNewActiveValue;
            if ((this.RowChanged != null) && ((bSelected != this.m_bSelected) || (bActive != this.m_bActive)))
            {
                if (bHandleStateChange)
                {
                    this.RowChanged(this, RowEventArgsType.Selection, bSelected);
                }
                if (currentTemplateIndex != this.CurrentTemplateIndex)
                {
                    this.RowChanged(this, RowEventArgsType.TemplateIndex, currentTemplateIndex);
                }
            }
        }

        public virtual void CopyTo(Array array, int index)
        {
            this.m_oData.CopyTo(array, index);
        }

        public virtual void Delete()
        {
            if (this.m_pParent != null)
            {
                this.m_pParent.Remove(this);
            }
        }

        public virtual void GetData(object[] data)
        {
            if (data.Length < this.FieldCount)
            {
                throw new ArgumentException("Invalid field count");
            }
            this.m_oData.CopyTo(data, 0);
        }

        public virtual IEnumerator GetEnumerator()
        {
            return this.m_oData.GetEnumerator();
        }

        internal int GetHeight(TemplateSet ts)
        {
            int currentTemplateIndex = this.CurrentTemplateIndex;
            if ((currentTemplateIndex >= 0) && (currentTemplateIndex < ts.Count))
            {
                return ts[currentTemplateIndex].GetHeight(this);
            }
            return RowTemplate.Default.Height;
        }

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            if ((listAccessors != null) && (listAccessors.Length >= 1))
            {
                return ListBindingHelper.GetListItemProperties(listAccessors[0].PropertyType);
            }
            if (this.FieldNames != null)
            {
                return this.FieldNames.GetRowPropertyDescriptors();
            }
            return null;
        }

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return base.GetType().ToString();
        }

        internal RowTemplate GetTemplate(TemplateSet ts)
        {
            int currentTemplateIndex = this.CurrentTemplateIndex;
            if ((currentTemplateIndex >= 0) && (currentTemplateIndex < ts.Count))
            {
                return ts[currentTemplateIndex];
            }
            return RowTemplate.Default;
        }

        protected virtual void OnChanged()
        {
            this.ResetCachedBounds();
            if (this.RowChanged != null)
            {
                this.RowChanged(this, RowEventArgsType.Empty, null);
            }
        }

        internal void ResetCachedBounds()
        {
            foreach (Resco.Controls.AdvancedList.RowSpecificCellProperties properties in this.m_rowSpecificCellProperties.Values)
            {
                properties.ResetCachedBounds();
            }
            this.m_recalculationNeeded = true;
        }

        public virtual void SetData(ICollection data)
        {
            if (data.Count != this.m_oData.Length)
            {
                this.m_oData = new object[data.Count];
            }
            data.CopyTo(this.m_oData, 0);
            this.OnChanged();
        }

        public virtual void SetData(IDataRecord reader)
        {
            if (reader.FieldCount != this.FieldCount)
            {
                this.m_oData = new object[reader.FieldCount];
            }
            reader.GetValues(this.m_oData);
            this.OnChanged();
        }

        public override string ToString()
        {
            string[] stringData = this.StringData;
            if ((stringData != null) && (stringData.Length >= 1))
            {
                return string.Join(",", stringData);
            }
            return "";
        }

        public void Update()
        {
            this.OnChanged();
        }

        [DefaultValue(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedList.Design.Browsable(false)]
        protected internal virtual bool Active
        {
            get
            {
                return this.m_bActive;
            }
            set
            {
                bool bNewSelectedValue = !value ? this.m_bSelected : true;
                this.ChangeSelection(bNewSelectedValue, value, true);
            }
        }

        [DefaultValue(-1)]
        public virtual int ActiveTemplateIndex
        {
            get
            {
                return this.m_iActiveTemplate;
            }
            set
            {
                if (this.m_iActiveTemplate != value)
                {
                    int iActiveTemplate = this.m_iActiveTemplate;
                    this.m_iActiveTemplate = value;
                    if (this.Active && (this.RowChanged != null))
                    {
                        this.RowChanged(this, RowEventArgsType.TemplateIndex, iActiveTemplate);
                    }
                }
            }
        }

        [DefaultValue(-1)]
        public virtual int AlternateTemplateIndex
        {
            get
            {
                return this.m_iAlternateTemplate;
            }
            set
            {
                if (this.m_iAlternateTemplate != value)
                {
                    int iAlternateTemplate = this.m_iAlternateTemplate;
                    this.m_iAlternateTemplate = value;
                    if (this.RowChanged != null)
                    {
                        this.RowChanged(this, RowEventArgsType.TemplateIndex, iAlternateTemplate);
                    }
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public virtual int CurrentTemplateIndex
        {
            get
            {
                int templateIndex = this.TemplateIndex;
                if (this.Selected)
                {
                    if ((this.ActiveTemplateIndex >= 0) && this.m_bActive)
                    {
                        return this.ActiveTemplateIndex;
                    }
                    return this.SelectedTemplateIndex;
                }
                if (((this.AlternateTemplateIndex >= 0) && (this.Index > 0)) && ((this.Index % 2) == 1))
                {
                    templateIndex = this.AlternateTemplateIndex;
                }
                return templateIndex;
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedList.Design.Browsable(false)]
        public virtual int FieldCount
        {
            get
            {
                return this.m_oData.Length;
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedList.Design.Browsable(false)]
        public Resco.Controls.AdvancedList.Mapping FieldNames
        {
            get
            {
                return this.m_htMap;
            }
            set
            {
                if (value == null)
                {
                    value = Resco.Controls.AdvancedList.Mapping.Empty;
                }
                if (this.m_htMap != value)
                {
                    this.m_htMap = value;
                    if (this.RowChanged != null)
                    {
                        this.RowChanged(this, RowEventArgsType.Empty, null);
                    }
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), DefaultValue((string) null), Resco.Controls.AdvancedList.Design.Browsable(false)]
        public int Height
        {
            get
            {
                if (this.Parent == null)
                {
                    return 0;
                }
                TemplateSet templates = this.Parent.Parent.Templates;
                int currentTemplateIndex = this.CurrentTemplateIndex;
                if ((currentTemplateIndex >= 0) && (currentTemplateIndex < templates.Count))
                {
                    return templates[currentTemplateIndex].GetHeight(this);
                }
                return RowTemplate.Default.Height;
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public virtual int Index
        {
            get
            {
                return this.m_index;
            }
        }

        public virtual object this[int index]
        {
            get
            {
                if ((index >= 0) && (index < this.m_oData.Length))
                {
                    return this.m_oData[index];
                }
                return null;
            }
            set
            {
                if (((index >= 0) && (index < this.m_oData.Length)) && (this.m_oData[index] != value))
                {
                    this.m_oData[index] = value;
                    this.OnChanged();
                }
            }
        }

        public virtual object this[string name]
        {
            get
            {
                int ordinal = this.FieldNames.GetOrdinal(name);
                if ((ordinal >= 0) && (ordinal < this.m_oData.Length))
                {
                    return this.m_oData[ordinal];
                }
                return null;
            }
            set
            {
                int ordinal = this.FieldNames.GetOrdinal(name);
                if ((ordinal >= this.m_oData.Length) && (ordinal < this.FieldNames.FieldCount))
                {
                    object[] array = new object[this.FieldNames.FieldCount];
                    this.m_oData.CopyTo(array, 0);
                    this.m_oData = array;
                }
                if ((ordinal >= 0) && (ordinal < this.m_oData.Length))
                {
                    this.m_oData[ordinal] = value;
                    this.OnChanged();
                }
            }
        }

        protected internal RowCollection Parent
        {
            get
            {
                return this.m_pParent;
            }
            set
            {
                if (this.m_pParent != value)
                {
                    this.m_pParent = value;
                    this.RowChanged = (this.m_pParent == null) ? null : new RowChangedEventHandler(this.m_pParent.OnRowChange);
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), DefaultValue(-1), Resco.Controls.AdvancedList.Design.Browsable(false)]
        internal int PressedButtonIndex
        {
            get
            {
                return this.m_pressedButtonIndex;
            }
            set
            {
                this.m_pressedButtonIndex = value;
            }
        }

        internal bool RecalculationNeeded
        {
            get
            {
                return this.m_recalculationNeeded;
            }
            set
            {
                this.m_recalculationNeeded = value;
            }
        }

        internal Hashtable RowSpecificCellProperties
        {
            get
            {
                return this.m_rowSpecificCellProperties;
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedList.Design.Browsable(false), DefaultValue(false)]
        public virtual bool Selected
        {
            get
            {
                return this.m_bSelected;
            }
            set
            {
                this.ChangeSelection(value, value, true);
            }
        }

        [DefaultValue(0)]
        public virtual int SelectedTemplateIndex
        {
            get
            {
                return this.m_iSelectedTemplate;
            }
            set
            {
                if (this.m_iSelectedTemplate != value)
                {
                    int iSelectedTemplate = this.m_iSelectedTemplate;
                    this.m_iSelectedTemplate = value;
                    if (this.Selected && (this.RowChanged != null))
                    {
                        this.RowChanged(this, RowEventArgsType.TemplateIndex, iSelectedTemplate);
                    }
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Content)]
        public string[] StringData
        {
            get
            {
                if (this.m_oData == null)
                {
                    return null;
                }
                int length = this.m_oData.Length;
                string[] strArray = new string[length];
                for (int i = 0; i < length; i++)
                {
                    strArray[i] = Convert.ToString(this.m_oData[i]);
                }
                return strArray;
            }
            set
            {
                if (this.m_oData != null)
                {
                    this.m_oData = value;
                    this.FieldNames = Resco.Controls.AdvancedList.Mapping.Empty;
                    this.OnChanged();
                }
            }
        }

        int ICollection.Count
        {
            get
            {
                return this.FieldCount;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(true), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Visible)]
        public object Tag
        {
            get
            {
                return this.m_tag;
            }
            set
            {
                this.m_tag = value;
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), DefaultValue((string) null), Resco.Controls.AdvancedList.Design.Browsable(false)]
        public RowTemplate Template
        {
            get
            {
                if (this.Parent == null)
                {
                    return null;
                }
                TemplateSet templates = this.Parent.Parent.Templates;
                int currentTemplateIndex = this.CurrentTemplateIndex;
                if ((currentTemplateIndex >= 0) && (currentTemplateIndex < templates.Count))
                {
                    return templates[currentTemplateIndex];
                }
                return RowTemplate.Default;
            }
        }

        [DefaultValue(0)]
        public virtual int TemplateIndex
        {
            get
            {
                return this.m_iTemplate;
            }
            set
            {
                if (this.m_iTemplate != value)
                {
                    int iTemplate = this.m_iTemplate;
                    this.m_iTemplate = value;
                    if (!this.Selected && (this.RowChanged != null))
                    {
                        this.RowChanged(this, RowEventArgsType.TemplateIndex, iTemplate);
                    }
                }
            }
        }
    }
}

