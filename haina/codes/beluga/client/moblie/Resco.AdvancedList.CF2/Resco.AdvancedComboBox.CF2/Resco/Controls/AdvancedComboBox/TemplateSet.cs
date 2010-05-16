namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class TemplateSet : CollectionBase
    {
        private Resco.Controls.AdvancedComboBox.AdvancedComboBox m_parent;

        internal event ComboBoxEventHandler Changed;

        public int Add(ItemTemplate value)
        {
            return base.List.Add(value);
        }

        public virtual TemplateSet Clone()
        {
            TemplateSet set = new TemplateSet();
            foreach (ItemTemplate template in base.List)
            {
                set.Add(template);
            }
            return set;
        }

        public bool Contains(ItemTemplate value)
        {
            return base.List.Contains(value);
        }

        public int IndexOf(ItemTemplate value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, ItemTemplate value)
        {
            base.List.Insert(index, value);
        }

        private void OnChange(object sender, ComboBoxEventArgsType e, ComboBoxArgs args)
        {
            if (this.Changed != null)
            {
                this.Changed(sender, e, args);
            }
        }

        protected override void OnClear()
        {
            foreach (ItemTemplate template in base.List)
            {
                template.Parent = null;
                template.CellTemplates.m_Owner = null;
                template.Changed -= new ComboBoxEventHandler(this.OnChange);
            }
            base.OnClear();
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            this.OnChange(this, ComboBoxEventArgsType.Refresh, ComboBoxArgs.Default);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            ItemTemplate it = (ItemTemplate) value;
            it.Parent = this.Parent;
            it.Width = (this.Parent == null) ? 0 : this.Parent.ClientSize.Width;
            it.CellTemplates.m_Owner = it;
            it.Changed += new ComboBoxEventHandler(this.OnChange);
            this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(it));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ItemTemplate template = value as ItemTemplate;
            if (template == null)
            {
                throw new ArgumentException("value must be of type ItemTemplate.");
            }
            template.Parent = null;
            template.Changed -= new ComboBoxEventHandler(this.OnChange);
            this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(false));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            ItemTemplate template = (ItemTemplate) newValue;
            ItemTemplate template2 = (ItemTemplate) oldValue;
            template.Parent = this.Parent;
            template.Width = (this.Parent == null) ? 0 : this.Parent.ClientSize.Width;
            template.Changed += new ComboBoxEventHandler(this.OnChange);
            template2.Parent = null;
            template2.Changed -= new ComboBoxEventHandler(this.OnChange);
            this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true));
        }

        public void Remove(ItemTemplate value)
        {
            base.List.Remove(value);
        }

        public void Scale(float fx, float fy)
        {
            foreach (ItemTemplate template in this)
            {
                template.Scale(fx, fy);
            }
        }

        public ItemTemplate this[string name]
        {
            get
            {
                for (int i = 0; i < base.List.Count; i++)
                {
                    ItemTemplate template = (ItemTemplate) base.InnerList[i];
                    if (template.Name == name)
                    {
                        return template;
                    }
                }
                return null;
            }
        }

        public ItemTemplate this[int index]
        {
            get
            {
                if ((index >= 0) && (index < base.Count))
                {
                    return (ItemTemplate) base.InnerList[index];
                }
                return null;
            }
            set
            {
                base.List[index] = value;
            }
        }

        protected internal Resco.Controls.AdvancedComboBox.AdvancedComboBox Parent
        {
            get
            {
                return this.m_parent;
            }
            set
            {
                if (this.m_parent != value)
                {
                    int num = (value == null) ? 0 : value.ClientSize.Width;
                    this.m_parent = value;
                    foreach (ItemTemplate template in base.InnerList)
                    {
                        template.Parent = this.m_parent;
                        template.Width = num;
                    }
                }
            }
        }
    }
}

