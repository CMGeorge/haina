namespace Resco.Controls.DetailView
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public sealed class DetailViewItems : CollectionBase
    {
        private Hashtable m_htItemsByName = new Hashtable();
        private string m_Name = "";
        private Resco.Controls.DetailView.DetailView m_Parent;

        internal event DetailViewEventHandler Changed;

        internal DetailViewItems()
        {
        }

        public int Add(Item value)
        {
            return base.List.Add(value);
        }

        internal int CalculateItemsHeight()
        {
            int num = 0;
            int currentPageIndex = this.Parent.CurrentPageIndex;
            int num3 = 0;
            for (int i = 0; i < base.List.Count; i++)
            {
                Item item = (Item) base.List[i];
                if (!(item is ItemPageBreak) && (currentPageIndex == 0))
                {
                    int itemHeight = item.ItemHeight;
                    if ((i == 0) || item.NewLine)
                    {
                        num3 = itemHeight;
                    }
                    if (itemHeight > num3)
                    {
                        num3 = itemHeight;
                    }
                    if ((i == (base.List.Count - 1)) || ((Item) base.List[i + 1]).NewLine)
                    {
                        num += num3;
                    }
                }
                else if (item is ItemPageBreak)
                {
                    if (currentPageIndex == 0)
                    {
                        return num;
                    }
                    currentPageIndex--;
                }
            }
            return num;
        }

        internal void ChangeName(Item i, string oldName, string newName)
        {
            if (((Item) this.m_htItemsByName[oldName]) != null)
            {
                this.m_htItemsByName.Remove(oldName);
            }
            this.m_htItemsByName[newName] = i;
        }

        public bool Contains(Item value)
        {
            return base.List.Contains(value);
        }

        internal void Draw(Graphics gr, int xOffset, int yOffset, int ctrlWidth, int ctrlHeight)
        {
            int num = yOffset;
            int pagerHeight = 0;
            Resco.Controls.DetailView.DetailView parent = this.Parent;
            Rectangle rectangle = parent.CalculateClientRect();
            int currentPageIndex = parent.CurrentPageIndex;
            while (parent.HasPages && !parent.Pages[currentPageIndex].PagingItem.Visible)
            {
                currentPageIndex++;
                if (currentPageIndex >= parent.PageCount)
                {
                    currentPageIndex = 0;
                }
                if (currentPageIndex == parent.CurrentPageIndex)
                {
                    return;
                }
            }
            parent.CurrentPageIndex = currentPageIndex;
            IList currentPage = parent.CurrentPage;
            if (this.Parent.HasPages && (parent.PagesLocation == RescoPagesLocation.Top))
            {
                num += this.Parent.PagerHeight;
                ctrlHeight += this.Parent.PagerHeight;
                pagerHeight = this.Parent.PagerHeight;
            }
            int num4 = 0;
            for (int i = 0; i < currentPage.Count; i++)
            {
                Item item = (Item) currentPage[i];
                if (item != null)
                {
                    if (num > (ctrlHeight + rectangle.Y))
                    {
                        break;
                    }
                    int itemHeight = item.ItemHeight;
                    if ((i == 0) || item.NewLine)
                    {
                        num4 = itemHeight;
                    }
                    if ((num + itemHeight) >= pagerHeight)
                    {
                        item._Draw(gr, num, ctrlWidth);
                    }
                    if (itemHeight > num4)
                    {
                        num4 = itemHeight;
                    }
                    if ((i == (currentPage.Count - 1)) || ((Item) currentPage[i + 1]).NewLine)
                    {
                        num += num4;
                    }
                }
            }
            if (this.Parent.HasPages)
            {
                this.Parent.CurrentPage.PagingItem._Draw(gr, num, ctrlWidth);
            }
        }

        public int IndexOf(Item value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, Item value)
        {
            base.List.Insert(index, value);
        }

        internal void MouseUpDown(int yOffset, MouseEventArgs e, int ctrlWidth, int ctrlHeight, bool down)
        {
            if (this.Parent != null)
            {
                Rectangle rectangle = this.Parent.CalculateClientRect();
                int num = yOffset;
                Page currentPage = this.Parent.CurrentPage;
                int num2 = 0;
                for (int i = 0; i < currentPage.Count; i++)
                {
                    Item item = currentPage[i];
                    if (!(item is ItemPageBreak) && item.Visible)
                    {
                        if (num > (rectangle.Y + ctrlHeight))
                        {
                            return;
                        }
                        int itemHeight = item.ItemHeight;
                        if ((i == 0) || item.NewLine)
                        {
                            num2 = itemHeight;
                        }
                        Point itemXWidth = this.Parent.GetItemXWidth(item);
                        if ((((num + item.LabelHeight) < e.Y) && (((num + item.Height) + item.LabelHeight) > e.Y)) && ((itemXWidth.X < (e.X - rectangle.X)) && ((itemXWidth.X + itemXWidth.Y) > (e.X - rectangle.X))))
                        {
                            if (item.CheckForToolTip(e.X, e.Y, itemXWidth.X, itemXWidth.Y, down))
                            {
                                return;
                            }
                            if (e.Y <= (rectangle.Y + ctrlHeight))
                            {
                                if (down)
                                {
                                    item._MouseDown(num, ctrlWidth, e);
                                    return;
                                }
                                item._MouseUp(num, ctrlWidth, e);
                                return;
                            }
                        }
                        if (itemHeight > num2)
                        {
                            num2 = itemHeight;
                        }
                        if ((i == (currentPage.Count - 1)) || currentPage[i + 1].NewLine)
                        {
                            num += num2;
                        }
                    }
                }
            }
        }

        protected override void OnClear()
        {
            if (this.Parent != null)
            {
                this.Parent.SuspendRedraw();
            }
            if (this.Changed != null)
            {
                foreach (Item item in base.List)
                {
                    this.Changed(item, DetailViewEventArgsType.ItemRemove, 0);
                    item.SetParent(null);
                }
            }
            this.m_htItemsByName.Clear();
            base.OnClear();
        }

        protected override void OnClearComplete()
        {
            if (this.Parent != null)
            {
                this.Parent.ResumeRedraw();
            }
            base.OnClearComplete();
        }

        protected override void OnInsert(int index, object value)
        {
            if (!typeof(Item).IsInstanceOfType(value))
            {
                throw new ArgumentException("Value must be of type Item.");
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            ((Item) value).SetParent(this.Parent);
            ((Item) value).ParentChangeBackColor(this.Parent.BackColor);
            ((Item) value).ParentChangeForeColor(this.Parent.ForeColor);
            if (((Item) value).Name != "")
            {
                this.m_htItemsByName[((Item) value).Name] = value;
            }
            if (this.Changed != null)
            {
                this.Changed(value, DetailViewEventArgsType.ItemAdd, ((Item) base.InnerList[index]).ItemHeight);
            }
            base.OnInsertComplete(index, value);
        }

        protected override void OnRemove(int index, object value)
        {
            if (!typeof(Item).IsInstanceOfType(value))
            {
                throw new ArgumentException("Value must be of type Item.");
            }
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            this.m_htItemsByName.Remove(((Item) value).Name);
            base.OnRemoveComplete(index, value);
            if (this.Changed != null)
            {
                this.Changed(value, DetailViewEventArgsType.ItemRemove, ((Item) value).ItemHeight);
            }
            ((Item) value).SetParent(null);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (!typeof(Item).IsInstanceOfType(newValue))
            {
                throw new ArgumentException("Value must be of type Item.");
            }
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (this.Changed != null)
            {
                this.Changed(oldValue, DetailViewEventArgsType.ItemRemove, ((Item) oldValue).ItemHeight);
            }
            ((Item) oldValue).SetParent(null);
            if (((Item) oldValue).Name != "")
            {
                this.m_htItemsByName.Remove(((Item) oldValue).Name);
            }
            if (((Item) newValue).Name != "")
            {
                this.m_htItemsByName[((Item) newValue).Name] = newValue;
            }
            base.OnSetComplete(index, oldValue, newValue);
            ((Item) newValue).SetParent(this.Parent);
            ((Item) newValue).ParentChangeBackColor(this.Parent.BackColor);
            ((Item) newValue).ParentChangeForeColor(this.Parent.ForeColor);
            if (this.Changed != null)
            {
                this.Changed(newValue, DetailViewEventArgsType.ItemAdd, ((Item) newValue).ItemHeight);
            }
        }

        protected override void OnValidate(object value)
        {
            if (!typeof(Item).IsInstanceOfType(value))
            {
                throw new ArgumentException("Value must be of type Item.");
            }
        }

        public void Remove(Item value)
        {
            base.List.Remove(value);
        }

        public Item[] All
        {
            get
            {
                Item[] array = new Item[base.List.Count];
                base.List.CopyTo(array, 0);
                return array;
            }
            set
            {
                base.List.Clear();
                foreach (Item item in value)
                {
                    this.Add(item);
                }
            }
        }

        public Item this[string name]
        {
            get
            {
                return (this.m_htItemsByName[name] as Item);
            }
            set
            {
                int num = 0;
                foreach (Item item in base.List)
                {
                    if (item.Name == name)
                    {
                        base.List[num] = value;
                        break;
                    }
                    num++;
                }
            }
        }

        public Item this[int index]
        {
            get
            {
                if ((index >= 0) && (index < base.List.Count))
                {
                    return (Item) base.InnerList[index];
                }
                return null;
            }
            set
            {
                if ((index >= 0) && (index < base.List.Count))
                {
                    base.List[index] = value;
                }
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }

        internal Resco.Controls.DetailView.DetailView Parent
        {
            get
            {
                return this.m_Parent;
            }
            set
            {
                this.m_Parent = value;
            }
        }
    }
}

