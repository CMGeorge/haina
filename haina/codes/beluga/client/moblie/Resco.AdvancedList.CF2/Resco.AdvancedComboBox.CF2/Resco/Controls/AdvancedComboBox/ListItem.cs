namespace Resco.Controls.AdvancedComboBox
{
    using Resco.Controls.AdvancedComboBox.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class ListItem : ICollection, IEnumerable
    {
        protected internal int ActualHeight;
        internal ItemTemplate LastTemplate;
        private bool m_bSelected;
        private Resco.Controls.AdvancedComboBox.Mapping m_htMap;
        protected int m_iAlternateTemplate;
        protected internal int m_index;
        protected int m_iSelectedTemplate;
        protected int m_iTemplate;
        private Hashtable m_itemSpecificCellProperties;
        protected int m_iTextBoxTemplate;
        private object[] m_oData;
        private ItemCollection m_parent;
        protected int m_pressedButtonIndex;
        private bool m_recalculationNeeded;
        private object m_tag;

        internal event ItemChangedEventHandler ItemChanged;

        public ListItem() : this(0, 0, -1, -1, Resco.Controls.AdvancedComboBox.Mapping.Empty)
        {
        }

        public ListItem(ListItem toCopy) : this(toCopy.TemplateIndex, toCopy.SelectedTemplateIndex, toCopy.AlternateTemplateIndex, toCopy.TextBoxTemplateIndex, toCopy, toCopy.FieldNames)
        {
        }

        public ListItem(Resco.Controls.AdvancedComboBox.Mapping fieldNames) : this(0, 0, -1, -1, fieldNames)
        {
        }

        public ListItem(int fieldCount) : this(0, 0, -1, -1, fieldCount)
        {
        }

        protected ListItem(int ti, int sti, int ati, int tbti)
        {
            this.m_index = -1;
            this.m_pressedButtonIndex = -1;
            this.ActualHeight = -1;
            this.m_recalculationNeeded = true;
            this.m_iTemplate = ti;
            this.m_iSelectedTemplate = sti;
            this.m_iAlternateTemplate = ati;
            this.m_iTextBoxTemplate = tbti;
            this.m_htMap = null;
            this.m_oData = null;
            this.m_itemSpecificCellProperties = new Hashtable();
        }

        public ListItem(int ti, int sti, int ati, int tbti, Resco.Controls.AdvancedComboBox.Mapping fieldNames) : this(ti, sti, ati, tbti)
        {
            this.m_htMap = fieldNames;
            this.m_oData = new object[fieldNames.FieldCount];
        }

        public ListItem(int ti, int sti, int ati, int tbti, ICollection dataList) : this(ti, sti, ati, tbti, dataList, Resco.Controls.AdvancedComboBox.Mapping.Empty)
        {
        }

        public ListItem(int ti, int sti, int ati, int tbti, int fieldCount) : this(ti, sti, ati, tbti)
        {
            this.m_oData = new object[fieldCount];
            this.m_htMap = Resco.Controls.AdvancedComboBox.Mapping.Empty;
        }

        public ListItem(int ti, int sti, int ati, int tbti, ICollection dataList, Resco.Controls.AdvancedComboBox.Mapping fieldNames) : this(ti, sti, ati, tbti)
        {
            int num = Math.Max(fieldNames.FieldCount, dataList.Count);
            this.m_htMap = fieldNames;
            this.m_oData = new object[num];
            dataList.CopyTo(this.m_oData, 0);
        }

        public ListItem(int ti, int sti, int ati, int tbti, string tag, ICollection dataList) : this(ti, sti, ati, tbti, dataList, Resco.Controls.AdvancedComboBox.Mapping.Empty)
        {
            this.m_tag = tag;
        }

        public virtual ListItem Clone()
        {
            return new ListItem(this);
        }

        public virtual void CopyTo(Array array, int index)
        {
            this.m_oData.CopyTo(array, index);
        }

        public virtual void Delete()
        {
            if (this.m_parent != null)
            {
                this.m_parent.Remove(this);
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
            if ((currentTemplateIndex < 0) || (currentTemplateIndex >= ts.Count))
            {
                if ((this.m_parent != null) && (this.m_parent.Parent != null))
                {
                    return this.m_parent.Parent.DefaultTemplates[this.Selected ? 1 : 0].Height;
                }
                return -1;
            }
            return ts[currentTemplateIndex].GetHeight(this);
        }

        internal ItemTemplate GetTemplate(TemplateSet ts)
        {
            int currentTemplateIndex = this.CurrentTemplateIndex;
            if ((currentTemplateIndex < 0) || (currentTemplateIndex >= ts.Count))
            {
                if ((this.m_parent != null) && (this.m_parent.Parent != null))
                {
                    return this.m_parent.Parent.DefaultTemplates[this.Selected ? 1 : 0];
                }
                return null;
            }
            return ts[currentTemplateIndex];
        }

        protected virtual void OnItemChanged(object sender, ItemEventArgsType e, ComboBoxArgs args)
        {
            this.ResetCachedBounds();
            if (this.ItemChanged != null)
            {
                this.ItemChanged(sender, e, args);
            }
        }

        internal void ResetCachedBounds()
        {
            foreach (Resco.Controls.AdvancedComboBox.ItemSpecificCellProperties properties in this.m_itemSpecificCellProperties.Values)
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
            this.OnItemChanged(this, ItemEventArgsType.Empty, ComboBoxArgs.Default);
        }

        public virtual void SetData(IDataRecord reader)
        {
            if (reader.FieldCount != this.FieldCount)
            {
                this.m_oData = new object[reader.FieldCount];
            }
            reader.GetValues(this.m_oData);
            this.OnItemChanged(this, ItemEventArgsType.Empty, ComboBoxArgs.Default);
        }

        public override string ToString()
        {
            string[] stringData = this.StringData;
            if ((stringData != null) && (stringData.Length > 0))
            {
                return string.Join(",", stringData);
            }
            return "";
        }

        public void Update()
        {
            this.OnItemChanged(this, ItemEventArgsType.Empty, ComboBoxArgs.Default);
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
                    this.OnItemChanged(this, ItemEventArgsType.TemplateIndex, new ComboBoxIndexChangedArgs(iAlternateTemplate));
                }
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedComboBox.Design.Browsable(false)]
        public virtual int CurrentTemplateIndex
        {
            get
            {
                if (this.Selected)
                {
                    return this.SelectedTemplateIndex;
                }
                if (((this.AlternateTemplateIndex >= 0) && (this.Index > 0)) && ((this.Index % 2) == 1))
                {
                    return this.AlternateTemplateIndex;
                }
                return this.TemplateIndex;
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.Browsable(false), Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden)]
        public virtual int FieldCount
        {
            get
            {
                return this.m_oData.Length;
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedComboBox.Design.Browsable(false)]
        public Resco.Controls.AdvancedComboBox.Mapping FieldNames
        {
            get
            {
                return this.m_htMap;
            }
            set
            {
                if (value == null)
                {
                    value = Resco.Controls.AdvancedComboBox.Mapping.Empty;
                }
                if (this.m_htMap != value)
                {
                    this.m_htMap = value;
                    if (this.ItemChanged != null)
                    {
                        this.ItemChanged(this, ItemEventArgsType.Empty, ComboBoxArgs.Default);
                    }
                }
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden), DefaultValue((string) null), Resco.Controls.AdvancedComboBox.Design.Browsable(false)]
        public int Height
        {
            get
            {
                if ((this.Parent == null) || (this.Parent.Parent == null))
                {
                    return 0;
                }
                TemplateSet templates = this.Parent.Parent.Templates;
                int currentTemplateIndex = this.CurrentTemplateIndex;
                if ((currentTemplateIndex < 0) || (currentTemplateIndex >= templates.Count))
                {
                    return this.m_parent.Parent.DefaultTemplates[this.Selected ? 1 : 0].Height;
                }
                return templates[currentTemplateIndex].GetHeight(this);
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.Browsable(false), Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden)]
        public int Index
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
                if (((index >= 0) || (index < this.m_oData.Length)) && (this.m_oData[index] != value))
                {
                    this.m_oData[index] = value;
                    this.OnItemChanged(this, ItemEventArgsType.Empty, ComboBoxArgs.Default);
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
                    this.OnItemChanged(this, ItemEventArgsType.Empty, ComboBoxArgs.Default);
                }
            }
        }

        internal Hashtable ItemSpecificCellProperties
        {
            get
            {
                return this.m_itemSpecificCellProperties;
            }
        }

        protected internal ItemCollection Parent
        {
            get
            {
                return this.m_parent;
            }
            set
            {
                if (this.m_parent != value)
                {
                    this.m_parent = value;
                    this.ItemChanged = (this.m_parent == null) ? null : new ItemChangedEventHandler(this.m_parent.OnItemChange);
                }
            }
        }

        [DefaultValue(-1)]
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

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedComboBox.Design.Browsable(false), DefaultValue(false)]
        public virtual bool Selected
        {
            get
            {
                return this.m_bSelected;
            }
            set
            {
                if (this.m_bSelected != value)
                {
                    int currentTemplateIndex = this.CurrentTemplateIndex;
                    this.m_bSelected = value;
                    this.OnItemChanged(this, ItemEventArgsType.Selection, null);
                    if (currentTemplateIndex != this.CurrentTemplateIndex)
                    {
                        this.OnItemChanged(this, ItemEventArgsType.TemplateIndex, new ComboBoxIndexChangedArgs(currentTemplateIndex));
                    }
                }
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
                    if (this.Selected)
                    {
                        this.OnItemChanged(this, ItemEventArgsType.TemplateIndex, new ComboBoxIndexChangedArgs(iSelectedTemplate));
                    }
                }
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Content)]
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
                    this.FieldNames = Resco.Controls.AdvancedComboBox.Mapping.Empty;
                    this.OnItemChanged(this, ItemEventArgsType.Empty, ComboBoxArgs.Default);
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

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Visible), Resco.Controls.AdvancedComboBox.Design.Browsable(true)]
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

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedComboBox.Design.Browsable(false), DefaultValue((string) null)]
        public ItemTemplate Template
        {
            get
            {
                if ((this.Parent == null) || (this.Parent.Parent == null))
                {
                    return null;
                }
                TemplateSet templates = this.Parent.Parent.Templates;
                int currentTemplateIndex = this.CurrentTemplateIndex;
                if ((currentTemplateIndex < 0) || (currentTemplateIndex >= templates.Count))
                {
                    return this.m_parent.Parent.DefaultTemplates[this.Selected ? 1 : 0];
                }
                return templates[currentTemplateIndex];
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
                    if (!this.Selected)
                    {
                        this.OnItemChanged(this, ItemEventArgsType.TemplateIndex, new ComboBoxIndexChangedArgs(iTemplate));
                    }
                }
            }
        }

        [DefaultValue(-1)]
        public virtual int TextBoxTemplateIndex
        {
            get
            {
                return this.m_iTextBoxTemplate;
            }
            set
            {
                if (this.m_iTextBoxTemplate != value)
                {
                    int iTextBoxTemplate = this.m_iTextBoxTemplate;
                    this.m_iTextBoxTemplate = value;
                    this.OnItemChanged(this, ItemEventArgsType.TemplateIndex, new ComboBoxIndexChangedArgs(iTextBoxTemplate));
                }
            }
        }
    }
}

