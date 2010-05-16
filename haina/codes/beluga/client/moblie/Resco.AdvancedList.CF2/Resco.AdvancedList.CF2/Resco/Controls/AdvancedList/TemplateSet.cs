namespace Resco.Controls.AdvancedList
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class TemplateSet : CollectionBase
    {
        private Resco.Controls.AdvancedList.AdvancedList m_Parent;

        internal event GridEventHandler Changed;

        public int Add(RowTemplate value)
        {
            return base.List.Add(value);
        }

        public virtual TemplateSet Clone()
        {
            TemplateSet set = new TemplateSet();
            foreach (RowTemplate template in base.List)
            {
                set.Add(template);
            }
            return set;
        }

        public bool Contains(RowTemplate value)
        {
            return base.List.Contains(value);
        }

        public int IndexOf(RowTemplate value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, RowTemplate value)
        {
            base.List.Insert(index, value);
        }

        private void OnChange(object sender, GridEventArgsType e, object oParam)
        {
            if (this.Changed != null)
            {
                this.Changed(sender, e, oParam);
            }
        }

        protected override void OnClear()
        {
            foreach (RowTemplate template in base.List)
            {
                template.Parent = null;
                template.CellTemplates.m_Owner = null;
                template.Changed -= new GridEventHandler(this.OnChange);
            }
            base.OnClear();
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            if (this.Changed != null)
            {
                this.Changed(this, GridEventArgsType.Refresh, null);
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            RowTemplate template = (RowTemplate) value;
            template.Parent = this.Parent;
            template.Width = (this.Parent == null) ? 0 : this.Parent.CalculateClientRect().Width;
            template.CellTemplates.m_Owner = template;
            template.Changed += new GridEventHandler(this.OnChange);
            if (this.Changed != null)
            {
                this.Changed(this, GridEventArgsType.Refresh, new Resco.Controls.AdvancedList.AdvancedList.RefreshData(true));
            }
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            if (value.GetType() != typeof(RowTemplate))
            {
                throw new ArgumentException("value must be of type RowTemplate.");
            }
            RowTemplate template = (RowTemplate) value;
            template.Parent = null;
            template.Changed -= new GridEventHandler(this.OnChange);
            if (this.Changed != null)
            {
                this.Changed(this, GridEventArgsType.Refresh, new Resco.Controls.AdvancedList.AdvancedList.RefreshData(true));
            }
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            RowTemplate template = (RowTemplate) newValue;
            template.Parent = this.Parent;
            template.Width = (this.Parent == null) ? 0 : this.Parent.CalculateClientRect().Width;
            template.Changed += new GridEventHandler(this.OnChange);
            RowTemplate template2 = (RowTemplate) oldValue;
            template2.Parent = null;
            template2.Changed -= new GridEventHandler(this.OnChange);
            if (this.Changed != null)
            {
                this.Changed(this, GridEventArgsType.Refresh, new Resco.Controls.AdvancedList.AdvancedList.RefreshData(true));
            }
        }

        public void Remove(RowTemplate value)
        {
            base.List.Remove(value);
        }

        public RowTemplate this[string name]
        {
            get
            {
                for (int i = 0; i < base.List.Count; i++)
                {
                    RowTemplate template = (RowTemplate) base.InnerList[i];
                    if (template.Name == name)
                    {
                        return template;
                    }
                }
                return null;
            }
        }

        public RowTemplate this[int index]
        {
            get
            {
                if ((index >= 0) && (index < base.Count))
                {
                    return (RowTemplate) base.InnerList[index];
                }
                return null;
            }
            set
            {
                base.List[index] = value;
            }
        }

        protected internal Resco.Controls.AdvancedList.AdvancedList Parent
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
                    foreach (RowTemplate template in base.InnerList)
                    {
                        template.Parent = this.m_Parent;
                        template.Width = (this.Parent == null) ? 0 : this.Parent.CalculateClientRect().Width;
                    }
                }
            }
        }
    }
}

