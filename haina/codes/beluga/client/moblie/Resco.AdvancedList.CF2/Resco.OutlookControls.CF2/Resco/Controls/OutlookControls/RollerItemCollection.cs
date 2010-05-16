namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class RollerItemCollection : CollectionBase
    {
        internal IParentControl m_Parent;

        internal event RefreshRequiredEventHandler RefreshRequired;

        public void Add(Roller anItem)
        {
            anItem.RefreshRequired += new RefreshRequiredEventHandler(this.RefreshToolbarItemRequired);
            if (this.m_Parent != null)
            {
                anItem.m_Parent = this.m_Parent;
            }
            base.List.Add(anItem);
        }

        public void Insert(int index, Roller anItem)
        {
            anItem.RefreshRequired += new RefreshRequiredEventHandler(this.RefreshToolbarItemRequired);
            if (this.m_Parent != null)
            {
                anItem.m_Parent = this.m_Parent;
            }
            base.List.Insert(index, anItem);
        }

        protected virtual void OnRefreshRequired(RollerItemEventArgs anArgs)
        {
            if (this.RefreshRequired != null)
            {
                this.RefreshRequired(this, anArgs);
            }
        }

        private void RefreshToolbarItemRequired(object sender, RollerItemEventArgs e)
        {
            this.OnRefreshRequired(e);
        }

        public void Remove(Roller anItem)
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
            if ((base.List[anIndex] != null) && (base.List[anIndex] is Roller))
            {
                Roller roller = base.List[anIndex] as Roller;
                if (roller != null)
                {
                    roller.RefreshRequired -= new RefreshRequiredEventHandler(this.RefreshToolbarItemRequired);
                    roller.m_Parent = null;
                }
            }
            base.List.RemoveAt(anIndex);
        }

        public Roller this[int index]
        {
            get
            {
                return (Roller) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

