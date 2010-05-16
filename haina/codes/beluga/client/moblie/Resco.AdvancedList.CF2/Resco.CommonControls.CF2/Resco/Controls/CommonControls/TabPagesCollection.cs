namespace Resco.Controls.CommonControls
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    public class TabPagesCollection : IList, ICollection, IEnumerable
    {
        private ArrayList m_list;
        private Resco.Controls.CommonControls.TabControl m_parent;
        private ToolbarItemCollection m_ToolbarItemCollection;

        public TabPagesCollection(Resco.Controls.CommonControls.TabControl parent, ToolbarItemCollection aToolbarItemCollection)
        {
            this.m_parent = parent;
            this.m_list = new ArrayList();
            this.m_ToolbarItemCollection = aToolbarItemCollection;
        }

        public void Add(Resco.Controls.CommonControls.TabPage page)
        {
            this.Insert(this.Count, page);
        }

        public void Clear()
        {
            while (this.Count > 0)
            {
                this.RemoveAt(0);
            }
        }

        public bool Contains(Resco.Controls.CommonControls.TabPage page)
        {
            return this.m_list.Contains(page);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_list.GetEnumerator();
        }

        public int IndexOf(Resco.Controls.CommonControls.TabPage page)
        {
            return this.m_list.IndexOf(page);
        }

        public void Insert(int index, Resco.Controls.CommonControls.TabPage page)
        {
            this.m_list.Insert(index, page);
            page.Dock = DockStyle.Fill;
            page.VisiblePanel = false;
            this.m_parent.Controls.Add(page);
            this.m_ToolbarItemCollection.Insert(index, page.TabItem);
            this.m_parent.OnChanged();
        }

        public void Remove(Resco.Controls.CommonControls.TabPage page)
        {
            this.m_parent.Controls.Remove(page);
            this.m_list.Remove(page);
            this.m_ToolbarItemCollection.Remove(page.TabItem);
            this.m_parent.OnChanged();
        }

        public void RemoveAt(int index)
        {
            this.Remove((Resco.Controls.CommonControls.TabPage) this.m_list[index]);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.m_list.CopyTo(array, index);
        }

        int IList.Add(object value)
        {
            this.Add((Resco.Controls.CommonControls.TabPage) value);
            return this.IndexOf((Resco.Controls.CommonControls.TabPage) value);
        }

        bool IList.Contains(object value)
        {
            return this.Contains((Resco.Controls.CommonControls.TabPage) value);
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOf((Resco.Controls.CommonControls.TabPage) value);
        }

        void IList.Insert(int index, object value)
        {
            this.Insert(index, (Resco.Controls.CommonControls.TabPage) value);
        }

        void IList.Remove(object value)
        {
            this.Remove((Resco.Controls.CommonControls.TabPage) value);
        }

        public int Count
        {
            get
            {
                return this.m_list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Resco.Controls.CommonControls.TabPage this[int index]
        {
            get
            {
                return (Resco.Controls.CommonControls.TabPage) this.m_list[index];
            }
            set
            {
                this.m_list[index] = value;
                this.m_parent.OnChanged();
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

        bool IList.IsFixedSize
        {
            get
            {
                return false;
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
                this[index] = (Resco.Controls.CommonControls.TabPage) value;
            }
        }
    }
}

