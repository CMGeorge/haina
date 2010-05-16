namespace Resco.Controls.DetailView
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class PageCollection : ICollection, IEnumerable
    {
        private Page[] InnerList;

        public PageCollection(Page[] pages)
        {
            this.InnerList = pages;
        }

        public bool Contains(Page page)
        {
            return (this.IndexOf(page) >= 0);
        }

        public void CopyTo(Array array, int index)
        {
            this.InnerList.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }

        public Page GetItemPage(Item item)
        {
            foreach (Page page in this.InnerList)
            {
                if (page.IndexOf(item) >= 0)
                {
                    return page;
                }
            }
            return null;
        }

        public int IndexOf(Page page)
        {
            for (int i = 0; i < this.InnerList.Length; i++)
            {
                if (this.InnerList[i] == page)
                {
                    return i;
                }
            }
            return -1;
        }

        public int Count
        {
            get
            {
                return this.InnerList.Length;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public Page this[int i]
        {
            get
            {
                return this.InnerList[i];
            }
        }

        public object SyncRoot
        {
            get
            {
                return this.InnerList.SyncRoot;
            }
        }
    }
}

