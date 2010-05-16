namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class TemplateSet : CollectionBase
    {
        private Resco.Controls.AdvancedTree.AdvancedTree m_Parent;

        internal event TreeChangedEventHandler Changed;

        public int Add(NodeTemplate value)
        {
            return base.List.Add(value);
        }

        public virtual TemplateSet Clone()
        {
            TemplateSet set = new TemplateSet();
            foreach (NodeTemplate template in base.List)
            {
                set.Add(template);
            }
            return set;
        }

        public bool Contains(NodeTemplate value)
        {
            return base.List.Contains(value);
        }

        public int IndexOf(NodeTemplate value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, NodeTemplate value)
        {
            base.List.Insert(index, value);
        }

        internal virtual void OnChanged(object sender, TreeChangedEventArgs e)
        {
            if (this.Changed != null)
            {
                this.Changed(sender, e);
            }
        }

        protected override void OnClear()
        {
            foreach (NodeTemplate template in base.List)
            {
                template.Parent = null;
                template.Changed -= new TreeChangedEventHandler(this.OnChanged);
            }
            base.OnClear();
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            this.OnChanged(this, new TreeRefreshEventArgs(true));
        }

        protected override void OnInsertComplete(int index, object value)
        {
            NodeTemplate template = (NodeTemplate) value;
            template.Parent = this.Parent;
            template.Width = (this.Parent == null) ? 0 : this.Parent.Width;
            template.Changed += new TreeChangedEventHandler(this.OnChanged);
            this.OnChanged(this, new TreeRefreshEventArgs(true));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            if (value.GetType() != typeof(NodeTemplate))
            {
                throw new ArgumentException("value must be of type NodeTemplate.");
            }
            NodeTemplate template = (NodeTemplate) value;
            template.Parent = null;
            template.Changed -= new TreeChangedEventHandler(this.OnChanged);
            this.OnChanged(this, new TreeRefreshEventArgs(true));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            NodeTemplate template = (NodeTemplate) newValue;
            template.Parent = this.Parent;
            template.Width = (this.Parent == null) ? 0 : this.Parent.Width;
            template.Changed += new TreeChangedEventHandler(this.OnChanged);
            NodeTemplate template2 = (NodeTemplate) oldValue;
            template2.Parent = null;
            template2.Changed -= new TreeChangedEventHandler(this.OnChanged);
            this.OnChanged(this, new TreeRefreshEventArgs(null));
        }

        public void Remove(NodeTemplate value)
        {
            base.List.Remove(value);
        }

        public NodeTemplate this[string name]
        {
            get
            {
                for (int i = 0; i < base.List.Count; i++)
                {
                    NodeTemplate template = (NodeTemplate) base.InnerList[i];
                    if (template.Name == name)
                    {
                        return template;
                    }
                }
                return null;
            }
        }

        public NodeTemplate this[int index]
        {
            get
            {
                if ((index >= 0) && (index < base.Count))
                {
                    return (NodeTemplate) base.InnerList[index];
                }
                return NodeTemplate.Default;
            }
            set
            {
                base.List[index] = value;
            }
        }

        protected internal Resco.Controls.AdvancedTree.AdvancedTree Parent
        {
            get
            {
                return this.m_Parent;
            }
            set
            {
                if (this.m_Parent != value)
                {
                    this.m_Parent = value;
                    foreach (NodeTemplate template in base.InnerList)
                    {
                        template.Parent = this.m_Parent;
                        if (this.m_Parent != null)
                        {
                            template.Width = this.m_Parent.Width;
                        }
                    }
                }
            }
        }
    }
}

