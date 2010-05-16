namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class NodeCollection : CollectionBase
    {
        internal int LastDrawnNode;
        internal int LastDrawnNodeOffset;
        private Resco.Controls.AdvancedTree.AdvancedTree m_AdvancedTree = null;
        private bool m_bManualDataLoading = true;
        private Node m_parentNode;

        internal event TreeChangedEventHandler Changed;

        internal NodeCollection(Node parent)
        {
            this.m_parentNode = parent;
        }

        public int Add(Node node)
        {
            if (node.ParentCollection != null)
            {
                throw new ApplicationException("A single Node instance cannot be contained in multiple collections");
            }
            return ((IList) this).Add(node);
        }

        internal int CalculateNodesHeight()
        {
            int num = 0;
            foreach (Node node in base.List)
            {
                NodeTemplate template = this.GetTemplate(node);
                if (this.AdvancedTree.GridLines)
                {
                    num += template.GetHeight(node) + 1;
                }
                else
                {
                    num += template.GetHeight(node);
                }
                if (node.IsExpanded)
                {
                    num += node.Nodes.CalculateNodesHeight();
                }
            }
            return num;
        }

        public bool Contains(Node value)
        {
            return ((IList) this).Contains(value);
        }

        internal int Draw(Graphics gr, TemplateSet ts, int width, int ymax, int ymin, int iNode, int iNodeOffset, ref bool resetScrollbar)
        {
            int yOffset = iNodeOffset;
            int count = base.List.Count;
            this.LastDrawnNodeOffset = yOffset;
            this.LastDrawnNode = iNode;
            while (this.LastDrawnNode < count)
            {
                Node node = null;
                if ((this.LastDrawnNode >= 0) && (this.LastDrawnNode < base.InnerList.Count))
                {
                    node = base.InnerList[this.LastDrawnNode] as Node;
                }
                if (node != null)
                {
                    NodeTemplate template = node.GetTemplate(ts);
                    int height = 0;
                    int num4 = -1;
                    int xOffset = node.Level * this.m_AdvancedTree.NodeIndent;
                    if (template.CustomizeCells(node))
                    {
                        num4 = template.GetHeight(node);
                        node.ResetCachedBounds(null);
                    }
                    height = template.GetHeight(node);
                    if ((num4 >= 0) && (height != num4))
                    {
                        resetScrollbar = true;
                    }
                    if ((yOffset + height) >= ymin)
                    {
                        template.Draw(gr, xOffset, yOffset, node, width, -1);
                    }
                    this.LastDrawnNodeOffset = yOffset;
                    yOffset += height;
                    if (this.AdvancedTree.GridLines)
                    {
                        if (yOffset >= 0)
                        {
                            gr.DrawLine(this.m_AdvancedTree.m_penBorder, 0, yOffset, this.AdvancedTree.Width, yOffset);
                        }
                        yOffset++;
                    }
                    if (node.IsExpanded)
                    {
                        yOffset = node.Nodes.Draw(gr, ts, width, ymax, ymin, 0, yOffset, ref resetScrollbar);
                    }
                    if (yOffset > ymax)
                    {
                        return yOffset;
                    }
                }
                this.LastDrawnNode++;
            }
            this.LastDrawnNodeOffset = yOffset;
            return yOffset;
        }

        internal int GetHeight(int i, TemplateSet ts)
        {
            return this.GetHeight(i, ts, false);
        }

        internal int GetHeight(int i, TemplateSet ts, bool subNodes)
        {
            if ((i < 0) || (i >= base.List.Count))
            {
                return 0;
            }
            Node node = (Node) base.InnerList[i];
            NodeTemplate template = ts[node.CurrentTemplateIndex];
            if (template == null)
            {
                return 0;
            }
            int height = template.GetHeight(node);
            if (subNodes && node.IsExpanded)
            {
                height += node.Nodes.CalculateNodesHeight();
            }
            if (this.AdvancedTree.GridLines)
            {
                return (height + 1);
            }
            return height;
        }

        internal Point GetNodeClick(int iNode, int iNodeOffset, int pos_x, int pos_y, out int yOffset, out int yOut, out Node node)
        {
            int num = 0;
            int num2 = iNodeOffset;
            yOffset = 0;
            yOut = num2;
            node = null;
            if (num2 <= pos_y)
            {
                for (int i = iNode; i < base.List.Count; i++)
                {
                    Node r = (Node) base.InnerList[i];
                    NodeTemplate template = this.GetTemplate(r);
                    int height = template.GetHeight(r);
                    if (this.AdvancedTree.GridLines)
                    {
                        num2 += height + 1;
                    }
                    else
                    {
                        num2 += height;
                    }
                    if (num2 > pos_y)
                    {
                        bool flag = (this.AdvancedTree != null) && this.AdvancedTree.RightToLeft;
                        yOffset = num2 - (this.AdvancedTree.GridLines ? (height + 1) : height);
                        num = (r.Level * this.AdvancedTree.NodeIndent) + (this.AdvancedTree.ShowPlusMinus ? this.AdvancedTree.PlusMinusSize.Width : 0);
                        if (flag)
                        {
                            num *= -1;
                        }
                        int y = template.GetCellClick(pos_x - num, pos_y - yOffset, r);
                        node = r;
                        return new Point(i, y);
                    }
                    if (r.IsExpanded)
                    {
                        Point point = r.Nodes.GetNodeClick(0, num2, pos_x, pos_y, out yOffset, out num2, out node);
                        if ((point.X != -1) || (point.Y != -1))
                        {
                            return point;
                        }
                    }
                    yOut = num2;
                }
            }
            return new Point(-1, -1);
        }

        internal NodeTemplate GetTemplate(Node r)
        {
            if (this.AdvancedTree == null)
            {
                return null;
            }
            TemplateSet templates = this.AdvancedTree.Templates;
            int count = templates.Count;
            int currentTemplateIndex = r.CurrentTemplateIndex;
            if ((currentTemplateIndex >= 0) && (currentTemplateIndex < count))
            {
                return templates[currentTemplateIndex];
            }
            return NodeTemplate.Default;
        }

        internal NodeTemplate GetTemplate(Node r, int ix)
        {
            TemplateSet templates = this.AdvancedTree.Templates;
            int count = templates.Count;
            if ((ix >= 0) && (ix < count))
            {
                return templates[ix];
            }
            return NodeTemplate.Default;
        }

        public int IndexOf(Node node)
        {
            return ((IList) this).IndexOf(node);
        }

        public void Insert(int index, Node node)
        {
            if (node.ParentCollection != null)
            {
                throw new ApplicationException("A single Node instance cannot be contained in multiple collections.");
            }
            ((IList) this).Insert(index, node);
        }

        internal void OnChanged(object sender, TreeChangedEventArgs e)
        {
            if (this.Changed != null)
            {
                this.Changed(sender, e);
            }
        }

        protected override void OnClear()
        {
            if ((this.AdvancedTree != null) && (this.AdvancedTree.Site != null))
            {
                base.OnClear();
            }
            else
            {
                foreach (Node node in base.List)
                {
                    if (node.Selected)
                    {
                        node.Selected = false;
                    }
                    node.Nodes.Clear();
                    node.SetParentCollection(null);
                }
                base.OnClear();
            }
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            if (this.Parent == null)
            {
                this.OnChanged(this, new TreeRefreshEventArgs(false));
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);
            Node n = value as Node;
            if (n != null)
            {
                n.SetParentCollection(this);
                if (n.Selected)
                {
                    this.SetSelected(n);
                }
                if (this.AdvancedTree != null)
                {
                    this.AdvancedTree.OnNodeAdded(n, index);
                }
            }
        }

        internal void OnNodeChanged(object sender, NodeEventArgsType e, object oParam)
        {
            object obj2 = oParam;
            Node r = (Node) sender;
            NodeTemplate template = this.GetTemplate(r);
            switch (e)
            {
                case NodeEventArgsType.Selection:
                    if (!((bool) oParam))
                    {
                        this.SetSelected(null);
                        return;
                    }
                    this.SetSelected(r);
                    return;

                case NodeEventArgsType.TemplateIndex:
                {
                    int offset = 0;
                    NodeTemplate template2 = this.GetTemplate(r, (int) oParam);
                    offset -= template2.GetHeight(r);
                    r.ResetCachedBounds(null);
                    offset += template.GetHeight(r);
                    this.OnChanged(base.List.IndexOf(r), new TreeResizeEventArgs(offset));
                    return;
                }
                case NodeEventArgsType.BeforeExpand:
                    this.OnChanged(this, new TreeChangedEventArgs(TreeEventArgsType.BeforeExpand, oParam));
                    return;

                case NodeEventArgsType.AfterExpand:
                    this.OnChanged(this, new TreeRefreshEventArgs(true));
                    this.OnChanged(this, new TreeChangedEventArgs(TreeEventArgsType.AfterExpand, oParam));
                    return;

                case NodeEventArgsType.BeforeCollapse:
                    this.OnChanged(this, new TreeChangedEventArgs(TreeEventArgsType.BeforeCollapse, oParam));
                    return;

                case NodeEventArgsType.AfterCollapse:
                    this.OnChanged(this, new TreeRefreshEventArgs(true));
                    this.OnChanged(this, new TreeChangedEventArgs(TreeEventArgsType.AfterCollapse, oParam));
                    return;
            }
            if ((template != null) && template.AutoHeight)
            {
                int actualHeight = r.ActualHeight;
                r.ResetCachedBounds(null);
                int height = template.GetHeight(r);
                if (height != actualHeight)
                {
                    this.OnChanged(base.List.IndexOf(r), new TreeResizeEventArgs(height - actualHeight));
                    return;
                }
            }
            this.OnChanged(base.List.IndexOf(sender), new TreeChangedEventArgs(TreeEventArgsType.NodeChange, obj2));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);
            Node node = value as Node;
            if (node != null)
            {
                if (this.AdvancedTree != null)
                {
                    this.AdvancedTree.OnNodeRemoved(node, index);
                }
                node.SetParentCollection(null);
                if (node.Selected)
                {
                    this.SetSelected(null);
                }
            }
        }

        public void Remove(Node node)
        {
            ((IList) this).Remove(node);
        }

        internal void ResetCachedBounds(NodeTemplate nt)
        {
            foreach (Node node in base.List)
            {
                node.ResetCachedBounds(nt);
            }
        }

        internal void ResetSelected()
        {
            for (int i = 0; i < base.List.Count; i++)
            {
                ((Node) base.InnerList[i]).Nodes.ResetSelected();
                ((Node) base.InnerList[i]).Selected = false;
            }
            this.m_AdvancedTree.m_countSelected = 0;
            this.m_AdvancedTree.m_nodeSelected = null;
        }

        internal void SetAdvancedTree(Resco.Controls.AdvancedTree.AdvancedTree at)
        {
            if (this.m_AdvancedTree != null)
            {
                this.Changed = (TreeChangedEventHandler) Delegate.Remove(this.Changed, new TreeChangedEventHandler(this.m_AdvancedTree.OnChanged));
            }
            this.m_AdvancedTree = at;
            if (this.m_AdvancedTree != null)
            {
                foreach (Node node in base.List)
                {
                    node.Nodes.SetAdvancedTree(this.m_AdvancedTree);
                }
                this.Changed = (TreeChangedEventHandler) Delegate.Combine(this.Changed, new TreeChangedEventHandler(this.m_AdvancedTree.OnChanged));
            }
        }

        internal void SetSelected(Node n)
        {
            if ((this.m_AdvancedTree.m_nodeSelected != null) && !this.m_AdvancedTree.MultiSelect)
            {
                this.m_AdvancedTree.m_nodeSelected.Selected = false;
            }
            if ((this.m_AdvancedTree.m_nodeSelected == null) || (this.m_AdvancedTree.m_nodeSelected != n))
            {
                this.m_AdvancedTree.m_nodeSelected = n;
                if (n != null)
                {
                    if (this.m_AdvancedTree.MultiSelect)
                    {
                        this.m_AdvancedTree.m_countSelected++;
                    }
                    else
                    {
                        this.m_AdvancedTree.m_countSelected = 1;
                    }
                }
                else if (this.m_AdvancedTree.m_countSelected > 0)
                {
                    this.m_AdvancedTree.m_countSelected--;
                }
            }
        }

        public Resco.Controls.AdvancedTree.AdvancedTree AdvancedTree
        {
            get
            {
                return this.m_AdvancedTree;
            }
        }

        public Node this[int index]
        {
            get
            {
                return (Node) base.InnerList[index];
            }
            set
            {
                base.RemoveAt(index);
                this.Insert(index, value);
            }
        }

        internal bool ManualDataLoading
        {
            get
            {
                return this.m_bManualDataLoading;
            }
            set
            {
                this.m_bManualDataLoading = value;
                if (value && (this.Parent != null))
                {
                    this.Parent.ParentCollection.ManualDataLoading = value;
                }
            }
        }

        public Node Parent
        {
            get
            {
                return this.m_parentNode;
            }
        }
    }
}

