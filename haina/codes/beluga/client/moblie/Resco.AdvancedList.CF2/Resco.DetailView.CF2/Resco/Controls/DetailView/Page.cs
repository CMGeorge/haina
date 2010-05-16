namespace Resco.Controls.DetailView
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class Page : IList, ICollection, IEnumerable
    {
        private int m_count;
        private int m_iFirst;
        private ItemPageBreak m_Item;
        private string m_Name = "";
        private Resco.Controls.DetailView.DetailView m_parent;

        internal Page(Resco.Controls.DetailView.DetailView parent, int iFirst, int iPageBreak)
        {
            this.m_parent = parent;
            this.m_iFirst = iFirst;
            if (iPageBreak >= 0)
            {
                this.m_Item = (ItemPageBreak) this.m_parent.Items[iPageBreak];
                this.m_count = iPageBreak - iFirst;
            }
            else
            {
                this.m_count = this.m_parent.Items.Count - iFirst;
                this.m_Item = null;
            }
        }

        public bool Contains(object value)
        {
            if (value is Item)
            {
                int index = this.m_parent.Items.IndexOf((Item) value);
                if ((index >= this.m_iFirst) && (index < (this.m_iFirst + this.m_count)))
                {
                    return true;
                }
            }
            return (value == this.m_Item);
        }

        public void CopyTo(Array array, int index)
        {
            for (int i = 0; i < this.m_count; i++)
            {
                array.SetValue(this.m_parent.Items[this.m_iFirst + i], (int) (index + i));
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new PageEnumerator(this);
        }

        public int IndexOf(object value)
        {
            if (value is Item)
            {
                int index = this.m_parent.Items.IndexOf((Item) value);
                if ((index >= this.m_iFirst) && (index < (this.m_iFirst + this.m_count)))
                {
                    return (index - this.m_iFirst);
                }
            }
            return -1;
        }

        public void Show()
        {
            this.m_parent.CurrentPageIndex = this.PageIndex;
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException("Collection is read only");
        }

        void IList.Clear()
        {
            throw new NotSupportedException("Collection is read only");
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException("Collection is read only");
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException("Collection is read only");
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException("Collection is read only");
        }

        public int Count
        {
            get
            {
                return this.m_count;
            }
        }

        public int FirstIndex
        {
            get
            {
                return this.m_iFirst;
            }
        }

        public bool IsCurrent
        {
            get
            {
                return (this == this.m_parent.CurrentPage);
            }
        }

        public bool IsFirst
        {
            get
            {
                return (this.PageIndex == 0);
            }
        }

        public bool IsLast
        {
            get
            {
                return (this.PageIndex == (this.m_parent.Pages.Count - 1));
            }
        }

        public Item this[int index]
        {
            get
            {
                if ((index >= 0) && (index < this.m_count))
                {
                    return this.m_parent.Items[this.m_iFirst + index];
                }
                return null;
            }
        }

        public string Name
        {
            get
            {
                if (this.m_Item != null)
                {
                    return this.m_Item.Name;
                }
                return this.m_Name;
            }
            set
            {
                if (this.m_Item != null)
                {
                    this.m_Item.Name = value;
                }
                else
                {
                    this.m_Name = value;
                }
            }
        }

        public int PageIndex
        {
            get
            {
                return this.m_parent.Pages.IndexOf(this);
            }
        }

        public ItemPageBreak PagingItem
        {
            get
            {
                return this.m_Item;
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
                return this.m_parent.Items;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return true;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotSupportedException("Collection is read only");
            }
        }

        public string Text
        {
            get
            {
                if (this.m_Item != null)
                {
                    return this.m_Item.Text;
                }
                return null;
            }
            set
            {
                if (this.m_Item != null)
                {
                    this.m_Item.Text = value;
                }
            }
        }
    }
}

