namespace Resco.Controls.CommonControls
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class ToolbarItemCollection : CollectionBase
    {
        internal event RefreshRequiredEventHandler RefreshRequired;

        public void Add(ToolbarItem anItem)
        {
            anItem.RefreshRequired += new RefreshRequiredEventHandler(this.RefreshToolbarItemRequired);
            base.List.Add(anItem);
        }

        public void Insert(int index, ToolbarItem anItem)
        {
            anItem.RefreshRequired += new RefreshRequiredEventHandler(this.RefreshToolbarItemRequired);
            base.List.Insert(index, anItem);
        }

        protected virtual void OnRefreshRequired(ToolbarItemEventArgs anArgs)
        {
            if (this.RefreshRequired != null)
            {
                this.RefreshRequired(this, anArgs);
            }
        }

        private void RefreshToolbarItemRequired(object sender, ToolbarItemEventArgs e)
        {
            this.OnRefreshRequired(e);
        }

        public void Remove(ToolbarItem anItem)
        {
            int index = base.List.IndexOf(anItem);
            if (index >= 0)
            {
                this.Remove(index);
            }
        }

        public void Remove(int anIndex)
        {
            if ((anIndex > (base.Count - 1)) || (anIndex < 0))
            {
                throw new IndexOutOfRangeException();
            }
            if ((base.List[anIndex] != null) && (base.List[anIndex] is ToolbarItem))
            {
                ToolbarItem item = base.List[anIndex] as ToolbarItem;
                if (item != null)
                {
                    item.RefreshRequired -= new RefreshRequiredEventHandler(this.RefreshToolbarItemRequired);
                }
            }
            base.List.RemoveAt(anIndex);
        }

        public ToolbarItem this[int index]
        {
            get
            {
                return (ToolbarItem) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

