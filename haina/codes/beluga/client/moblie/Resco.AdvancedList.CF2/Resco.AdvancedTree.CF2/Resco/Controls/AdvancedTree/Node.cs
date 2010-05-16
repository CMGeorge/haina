namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class Node : IEnumerable
    {
        internal int ActualHeight;
        private bool m_bExpanded;
        private bool m_bHidePlusMinus;
        private bool m_bSelected;
        private Resco.Controls.AdvancedTree.Mapping m_htMap;
        protected int m_iSelectedTemplate;
        protected int m_iTemplate;
        private NodeCollection m_ncNodes;
        private NodeCollection m_ncParentCollection;
        private Hashtable m_nodeSpecificCellProperties;
        private object[] m_oData;
        private object m_oTag;
        protected int m_pressedButtonIndex;
        private bool m_recalculationNeeded;

        internal event NodeChangedEventHandler NodeChanged;

        public Node() : this(0, 0, Resco.Controls.AdvancedTree.Mapping.Empty)
        {
        }

        public Node(Resco.Controls.AdvancedTree.Mapping fieldNames) : this(0, 0, fieldNames)
        {
        }

        public Node(Node toCopy) : this(toCopy.TemplateIndex, toCopy.SelectedTemplateIndex, toCopy.m_oData, toCopy.FieldNames)
        {
            this.m_oTag = toCopy.m_oTag;
        }

        public Node(int fieldCount) : this(0, 0, fieldCount)
        {
        }

        protected Node(int templateIndex, int selectedTemplateIndex)
        {
            this.m_pressedButtonIndex = -1;
            this.ActualHeight = -1;
            this.m_recalculationNeeded = true;
            this.m_iTemplate = templateIndex;
            this.m_iSelectedTemplate = selectedTemplateIndex;
            this.m_htMap = null;
            this.m_oData = new object[0];
            this.m_ncNodes = new NodeCollection(this);
            this.m_bExpanded = false;
            this.m_nodeSpecificCellProperties = new Hashtable();
        }

        public Node(int templateIndex, int selectedTemplateIndex, Resco.Controls.AdvancedTree.Mapping fieldNames) : this(templateIndex, selectedTemplateIndex)
        {
            this.m_htMap = fieldNames;
            this.m_oData = new object[fieldNames.FieldCount];
        }

        public Node(int templateIndex, int selectedTemplateIndex, ICollection dataList) : this(templateIndex, selectedTemplateIndex, dataList, Resco.Controls.AdvancedTree.Mapping.Empty)
        {
        }

        public Node(int templateIndex, int selectedTemplateIndex, int fieldCount) : this(templateIndex, selectedTemplateIndex)
        {
            this.m_oData = new object[fieldCount];
            this.m_htMap = Resco.Controls.AdvancedTree.Mapping.Empty;
        }

        public Node(int templateIndex, int selectedTemplateIndex, ICollection dataList, Resco.Controls.AdvancedTree.Mapping fieldNames) : this(templateIndex, selectedTemplateIndex)
        {
            int num = Math.Max(fieldNames.FieldCount, dataList.Count);
            this.m_htMap = fieldNames;
            this.m_oData = new object[num];
            dataList.CopyTo(this.m_oData, 0);
        }

        public virtual void Collapse()
        {
            if (this.m_bExpanded)
            {
                int index = this.ParentCollection.IndexOf(this);
                NodeCancelEventArgs oParam = new NodeCancelEventArgs(this, index, 0);
                this.OnNodeChanged(NodeEventArgsType.BeforeCollapse, oParam);
                if (!oParam.Cancel)
                {
                    this.m_bExpanded = false;
                    this.Nodes.LastDrawnNode = 0;
                    NodeEventArgs args2 = new NodeEventArgs(this, index, 0);
                    this.OnNodeChanged(NodeEventArgsType.AfterCollapse, args2);
                }
            }
        }

        public virtual void CollapseAll()
        {
            if ((this.ParentCollection != null) && (this.ParentCollection.AdvancedTree != null))
            {
                this.ParentCollection.AdvancedTree.SuspendRedraw();
            }
            this.CollapseAllEx();
            if ((this.ParentCollection != null) && (this.ParentCollection.AdvancedTree != null))
            {
                this.ParentCollection.AdvancedTree.ResumeRedraw();
            }
        }

        private void CollapseAllEx()
        {
            this.Collapse();
            foreach (Node node in this.Nodes)
            {
                node.CollapseAllEx();
            }
        }

        public virtual void CopyTo(Array array, int index)
        {
            this.m_oData.CopyTo(array, index);
        }

        private void DoDelayLoad()
        {
            if (this.Nodes.ManualDataLoading)
            {
                this.ParentCollection.ManualDataLoading = this.Nodes.ManualDataLoading;
                this.ParentCollection.AdvancedTree.DoDelayLoad();
            }
        }

        public virtual void Expand()
        {
            if (!this.m_bExpanded)
            {
                int index = this.ParentCollection.IndexOf(this);
                NodeCancelEventArgs oParam = new NodeCancelEventArgs(this, index, 0);
                this.OnNodeChanged(NodeEventArgsType.BeforeExpand, oParam);
                if (!oParam.Cancel)
                {
                    this.m_bExpanded = true;
                    if ((this.ParentCollection.AdvancedTree != null) && this.ParentCollection.AdvancedTree.DelayLoad)
                    {
                        this.DoDelayLoad();
                    }
                    NodeEventArgs args2 = new NodeEventArgs(this, index, 0);
                    this.OnNodeChanged(NodeEventArgsType.AfterExpand, args2);
                }
            }
        }

        public virtual void ExpandAll()
        {
            if ((this.ParentCollection != null) && (this.ParentCollection.AdvancedTree != null))
            {
                this.ParentCollection.AdvancedTree.SuspendRedraw();
            }
            this.ExpandAllEx();
            if ((this.ParentCollection != null) && (this.ParentCollection.AdvancedTree != null))
            {
                this.ParentCollection.AdvancedTree.ResumeRedraw();
            }
        }

        private void ExpandAllEx()
        {
            this.Expand();
            foreach (Node node in this.Nodes)
            {
                node.ExpandAllEx();
            }
        }

        public virtual void GetData(object[] data)
        {
            if (data.Length < this.m_oData.Length)
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
            return NodeTemplate.Default.Height;
        }

        public virtual int GetNodeCount(bool includeSubTrees)
        {
            int count = this.Nodes.Count;
            if (includeSubTrees)
            {
                foreach (Node node in this.m_ncNodes)
                {
                    count += node.GetNodeCount(true);
                }
            }
            return count;
        }

        public NodeTemplate GetTemplate(TemplateSet ts)
        {
            int currentTemplateIndex = this.CurrentTemplateIndex;
            if ((currentTemplateIndex >= 0) && (currentTemplateIndex < ts.Count))
            {
                return ts[currentTemplateIndex];
            }
            return NodeTemplate.Default;
        }

        internal void OnNodeChanged(NodeEventArgsType e, object oParam)
        {
            if (this.NodeChanged != null)
            {
                this.NodeChanged(this, e, oParam);
            }
        }

        public virtual void Remove()
        {
            if (this.m_ncParentCollection != null)
            {
                this.m_ncParentCollection.Remove(this);
            }
        }

        internal void ResetCachedBounds(NodeTemplate nt)
        {
            if ((nt == null) || (nt == this.Template))
            {
                foreach (Resco.Controls.AdvancedTree.NodeSpecificCellProperties properties in this.m_nodeSpecificCellProperties.Values)
                {
                    properties.ResetCachedBounds();
                }
                this.m_recalculationNeeded = true;
            }
            if (this.Nodes != null)
            {
                this.Nodes.ResetCachedBounds(nt);
            }
        }

        public virtual void SetData(ICollection data)
        {
            if (data.Count != this.m_oData.Length)
            {
                this.m_oData = new object[data.Count];
            }
            data.CopyTo(this.m_oData, 0);
            this.OnNodeChanged(NodeEventArgsType.Empty, null);
        }

        public virtual void SetData(IDataRecord reader)
        {
            if (reader.FieldCount != this.FieldCount)
            {
                this.m_oData = new object[reader.FieldCount];
            }
            reader.GetValues(this.m_oData);
            this.OnNodeChanged(NodeEventArgsType.Empty, null);
        }

        internal void SetParentCollection(NodeCollection nc)
        {
            if (this.m_ncParentCollection != nc)
            {
                this.m_ncParentCollection = nc;
                if (nc != null)
                {
                    this.m_ncNodes.SetAdvancedTree(nc.AdvancedTree);
                }
                else
                {
                    this.m_ncNodes.SetAdvancedTree(null);
                }
                this.NodeChanged = (this.m_ncParentCollection == null) ? null : new NodeChangedEventHandler(this.m_ncParentCollection.OnNodeChanged);
            }
        }

        protected virtual bool ShouldSerializeSelectedTemplateIndex()
        {
            return (this.m_iSelectedTemplate != 0);
        }

        protected virtual bool ShouldSerializeTemplateIndex()
        {
            return (this.m_iTemplate != 0);
        }

        public virtual void Toggle()
        {
            if (this.IsExpanded)
            {
                this.Collapse();
            }
            else
            {
                this.Expand();
            }
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
            this.OnNodeChanged(NodeEventArgsType.Empty, null);
        }

        private int Count
        {
            get
            {
                return this.FieldCount;
            }
        }

        public virtual int CurrentTemplateIndex
        {
            get
            {
                if (!this.Selected)
                {
                    return this.TemplateIndex;
                }
                return this.SelectedTemplateIndex;
            }
        }

        public virtual int FieldCount
        {
            get
            {
                return this.m_oData.Length;
            }
        }

        public Resco.Controls.AdvancedTree.Mapping FieldNames
        {
            get
            {
                return this.m_htMap;
            }
            set
            {
                if (value == null)
                {
                    value = Resco.Controls.AdvancedTree.Mapping.Empty;
                }
                if (this.m_htMap != value)
                {
                    this.m_htMap = value;
                    this.OnNodeChanged(NodeEventArgsType.Empty, null);
                }
            }
        }

        public int Height
        {
            get
            {
                if (this.ParentCollection == null)
                {
                    return 0;
                }
                TemplateSet templates = this.ParentCollection.AdvancedTree.Templates;
                int currentTemplateIndex = this.CurrentTemplateIndex;
                if ((currentTemplateIndex >= 0) && (currentTemplateIndex < templates.Count))
                {
                    return templates[currentTemplateIndex].GetHeight(this);
                }
                return NodeTemplate.Default.Height;
            }
        }

        public virtual bool HidePlusMinus
        {
            get
            {
                return this.m_bHidePlusMinus;
            }
            set
            {
                if (this.m_bHidePlusMinus != value)
                {
                    this.m_bHidePlusMinus = value;
                    this.OnNodeChanged(NodeEventArgsType.Empty, null);
                }
            }
        }

        public virtual bool IsExpanded
        {
            get
            {
                return this.m_bExpanded;
            }
            set
            {
                this.m_bExpanded = value;
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
                    this.OnNodeChanged(NodeEventArgsType.Empty, null);
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
                    this.OnNodeChanged(NodeEventArgsType.Empty, null);
                }
            }
        }

        public virtual int Level
        {
            get
            {
                int num = 0;
                NodeCollection parentCollection = this.ParentCollection;
                while (parentCollection != null)
                {
                    if ((parentCollection != null) && (parentCollection.Parent != null))
                    {
                        num++;
                        parentCollection = parentCollection.Parent.ParentCollection;
                    }
                    else
                    {
                        parentCollection = null;
                    }
                }
                return num;
            }
        }

        public virtual NodeCollection Nodes
        {
            get
            {
                return this.m_ncNodes;
            }
        }

        internal Hashtable NodeSpecificCellProperties
        {
            get
            {
                return this.m_nodeSpecificCellProperties;
            }
        }

        public Node Parent
        {
            get
            {
                if (this.m_ncParentCollection == null)
                {
                    return null;
                }
                return this.m_ncParentCollection.Parent;
            }
        }

        public NodeCollection ParentCollection
        {
            get
            {
                return this.m_ncParentCollection;
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
                    if (currentTemplateIndex < 0)
                    {
                        currentTemplateIndex = -2147483648;
                    }
                    this.m_bSelected = value;
                    if (this.NodeChanged != null)
                    {
                        this.NodeChanged(this, NodeEventArgsType.Selection, this.m_bSelected);
                        if (currentTemplateIndex != this.CurrentTemplateIndex)
                        {
                            this.NodeChanged(this, NodeEventArgsType.TemplateIndex, currentTemplateIndex);
                        }
                    }
                }
            }
        }

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
                    this.m_iSelectedTemplate = value;
                }
            }
        }

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
                }
            }
        }

        public object Tag
        {
            get
            {
                return this.m_oTag;
            }
            set
            {
                this.m_oTag = value;
            }
        }

        public NodeTemplate Template
        {
            get
            {
                if (this.ParentCollection == null)
                {
                    return null;
                }
                TemplateSet templates = this.ParentCollection.AdvancedTree.Templates;
                int currentTemplateIndex = this.CurrentTemplateIndex;
                if ((currentTemplateIndex >= 0) && (currentTemplateIndex < templates.Count))
                {
                    return templates[currentTemplateIndex];
                }
                return NodeTemplate.Default;
            }
        }

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
                        this.OnNodeChanged(NodeEventArgsType.TemplateIndex, iTemplate);
                    }
                }
            }
        }
    }
}

