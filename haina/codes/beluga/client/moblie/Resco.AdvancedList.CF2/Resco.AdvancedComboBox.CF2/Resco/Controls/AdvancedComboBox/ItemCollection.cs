namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class ItemCollection : CollectionBase, ITypedList
    {
        internal int LastDrawnItem;
        internal int LastDrawnItemOffset;
        private Resco.Controls.AdvancedComboBox.AdvancedComboBox m_parent;
        internal Pen m_penBorder = new Pen(Color.DarkGray);
        private ListItem m_selectedItem;

        internal event ComboBoxEventHandler Changed;

        internal ItemCollection(Resco.Controls.AdvancedComboBox.AdvancedComboBox parent)
        {
            this.m_parent = parent;
        }

        public int Add(ListItem item)
        {
            if (item.Parent != null)
            {
                throw new ApplicationException("ListItem can't be in more collections");
            }
            return base.List.Add(item);
        }

        public int Add(object obj)
        {
            int ti = 0;
            int sti = 0;
            int ati = -1;
            int tbti = -1;
            if (this.Parent != null)
            {
                ti = this.Parent.TemplateIndex;
                sti = this.Parent.SelectedTemplateIndex;
                ati = this.Parent.AlternateTemplateIndex;
                tbti = this.Parent.TextBoxTemplateIndex;
            }
            BoundItem item = new BoundItem(ti, sti, ati, tbti, obj, new PropertyMapping(obj.GetType()));
            return base.List.Add(item);
        }

        internal int CalculateRowsHeight()
        {
            int num = 0;
            foreach (ListItem item in base.List)
            {
                ItemTemplate template = this.GetTemplate(item);
                num += template.GetHeight(item);
                if (this.Parent.List.GridLines)
                {
                    num++;
                }
            }
            return num;
        }

        public bool Contains(ListItem value)
        {
            return base.List.Contains(value);
        }

        internal int Draw(Graphics gr, TemplateSet ts, int width, int ymax, int iItem, int iItemOffset, ref bool resetScrollbar)
        {
            int num = (((this.Parent != null) && this.Parent.RightToLeft) && this.Parent.List.ScrollbarVisible) ? this.Parent.List.ClientScrollbarWidth : 0;
            int yOffset = iItemOffset;
            int count = base.List.Count;
            this.LastDrawnItemOffset = yOffset;
            this.LastDrawnItem = iItem;
            while (this.LastDrawnItem < count)
            {
                ListItem item = null;
                if ((this.LastDrawnItem >= 0) && (this.LastDrawnItem < base.InnerList.Count))
                {
                    item = base.InnerList[this.LastDrawnItem] as ListItem;
                }
                if (item != null)
                {
                    ItemTemplate template = item.GetTemplate(ts);
                    int height = 0;
                    int num5 = -1;
                    if (template.CustomizeCells(item))
                    {
                        num5 = template.GetHeight(item);
                        item.ResetCachedBounds();
                    }
                    height = template.GetHeight(item);
                    if ((num5 >= 0) && (height != num5))
                    {
                        resetScrollbar = true;
                    }
                    if ((yOffset + height) >= 0)
                    {
                        template.Draw(gr, 0, yOffset, item, width, height);
                    }
                    this.LastDrawnItemOffset = yOffset;
                    yOffset += height;
                    if (this.Parent.List.GridLines)
                    {
                        gr.DrawLine(this.m_penBorder, num, yOffset, width + num, yOffset);
                        yOffset++;
                    }
                    if (yOffset > ymax)
                    {
                        return yOffset;
                    }
                }
                this.LastDrawnItem++;
            }
            this.LastDrawnItemOffset = yOffset;
            return yOffset;
        }

        internal int GetHeight(int i, TemplateSet ts)
        {
            if ((i < 0) || (i >= base.List.Count))
            {
                return 0;
            }
            ListItem item = (ListItem) base.InnerList[i];
            int height = this.GetTemplate(item).GetHeight(item);
            if (this.Parent.List.GridLines)
            {
                height++;
            }
            return height;
        }

        internal Point GetItemClick(int iItem, int iItemOffset, int pos_x, int pos_y, out int yOffset)
        {
            int num = iItemOffset;
            yOffset = 0;
            if (num <= pos_y)
            {
                for (int i = iItem; i < base.List.Count; i++)
                {
                    ListItem item = (ListItem) base.InnerList[i];
                    ItemTemplate template = this.GetTemplate(item);
                    int height = template.GetHeight(item);
                    yOffset = num;
                    num += this.Parent.List.GridLines ? (height + 1) : height;
                    if (num > pos_y)
                    {
                        return new Point(i, template.GetCellClick(pos_x, pos_y - yOffset, item));
                    }
                }
            }
            return new Point(-1, -1);
        }

        internal ItemTemplate GetTemplate(ListItem item)
        {
            TemplateSet templates = this.m_parent.Templates;
            int count = templates.Count;
            int currentTemplateIndex = item.CurrentTemplateIndex;
            if ((currentTemplateIndex < 0) || (currentTemplateIndex >= count))
            {
                return this.m_parent.DefaultTemplates[item.Selected ? 1 : 0];
            }
            return templates[currentTemplateIndex];
        }

        internal ItemTemplate GetTemplate(ListItem item, int ix)
        {
            TemplateSet templates = this.m_parent.Templates;
            int count = templates.Count;
            if ((ix < 0) || (ix >= count))
            {
                return this.m_parent.DefaultTemplates[item.Selected ? 1 : 0];
            }
            return templates[ix];
        }

        public int IndexOf(ListItem item)
        {
            return base.List.IndexOf(item);
        }

        public void Insert(int index, ListItem item)
        {
            if (item.Parent != null)
            {
                throw new ApplicationException("ListItem can't be in more collections");
            }
            base.List.Insert(index, item);
        }

        public void Insert(int index, object obj)
        {
            int ti = 0;
            int sti = 0;
            int ati = -1;
            int tbti = -1;
            if (this.Parent != null)
            {
                ti = this.Parent.TemplateIndex;
                sti = this.Parent.SelectedTemplateIndex;
                ati = this.Parent.AlternateTemplateIndex;
                tbti = this.Parent.TextBoxTemplateIndex;
            }
            BoundItem item = new BoundItem(ti, sti, ati, tbti, obj, new PropertyMapping(obj.GetType()));
            base.List.Insert(index, item);
        }

        internal void OnChanged(object sender, ComboBoxEventArgsType e, ComboBoxArgs args)
        {
            if (this.Changed != null)
            {
                this.Changed(sender, e, args);
            }
        }

        protected override void OnClear()
        {
            foreach (ListItem item in base.List)
            {
                item.Parent = null;
            }
            base.OnClear();
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            this.LastDrawnItem = 0;
            this.LastDrawnItemOffset = 0;
            this.m_selectedItem = null;
            if (this.m_parent != null)
            {
                this.m_parent.OnClear(this);
            }
            this.OnChanged(this, ComboBoxEventArgsType.Refresh, ComboBoxArgs.Default);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            ListItem item = value as ListItem;
            base.OnInsertComplete(index, value);
            for (int i = index + 1; i < base.Count; i++)
            {
                ListItem item1 = this[i];
                item1.m_index++;
            }
            if (item != null)
            {
                item.Parent = this;
                item.m_index = index;
                if (this.Parent != null)
                {
                    this.Parent.OnItemAdded(item, index);
                    if (item.Selected)
                    {
                        this.Parent.List.SelectedItem = item;
                    }
                }
            }
        }

        internal void OnItemChange(object sender, ItemEventArgsType e, ComboBoxArgs args)
        {
            ListItem item = (ListItem) sender;
            ItemTemplate template = this.GetTemplate(item);
            switch (e)
            {
                case ItemEventArgsType.Selection:
                    if (this.m_selectedItem != null)
                    {
                        ListItem selectedItem = this.m_selectedItem;
                        this.m_selectedItem = null;
                        if (selectedItem != item)
                        {
                            selectedItem.Selected = false;
                        }
                    }
                    this.m_selectedItem = item;
                    return;

                case ItemEventArgsType.TemplateIndex:
                {
                    ComboBoxIndexChangedArgs args2 = args as ComboBoxIndexChangedArgs;
                    if (args2 == null)
                    {
                        throw new ArgumentException("Wrong argument type. args must be of type ComboBoxIndexChangedArgs.", "args");
                    }
                    int d = -this.GetTemplate(item, args2.OldTemplateIndex).GetHeight(item);
                    item.ResetCachedBounds();
                    d += template.GetHeight(item);
                    this.OnChanged(this, ComboBoxEventArgsType.Resize, new ComboBoxScrollArgs(d, base.List.IndexOf(item)));
                    return;
                }
            }
            if (template.AutoHeight)
            {
                int actualHeight = item.ActualHeight;
                item.ResetCachedBounds();
                int height = template.GetHeight(item);
                if (height != actualHeight)
                {
                    this.OnChanged(this, ComboBoxEventArgsType.Resize, new ComboBoxScrollArgs(height - actualHeight, base.List.IndexOf(item)));
                    return;
                }
            }
            this.OnChanged(this, ComboBoxEventArgsType.ItemChange, new ComboBoxScrollArgs(0, base.List.IndexOf(item)));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);
            for (int i = index; i < base.Count; i++)
            {
                ListItem item1 = this[i];
                item1.m_index--;
            }
            ListItem item = value as ListItem;
            if (item != null)
            {
                if (this.Parent != null)
                {
                    this.Parent.OnItemRemoved(item, index);
                }
                item.Parent = null;
                if (this.m_selectedItem == item)
                {
                    this.m_selectedItem = null;
                }
            }
        }

        public void Remove(ListItem item)
        {
            base.List.Remove(item);
        }

        public int RemoveByMapping(Resco.Controls.AdvancedComboBox.Mapping fieldNames)
        {
            int num = -1;
            for (int i = base.List.Count - 1; i >= 0; i--)
            {
                ListItem item = (ListItem) base.InnerList[i];
                if (item.FieldNames == fieldNames)
                {
                    base.List.RemoveAt(i);
                    num = i;
                }
            }
            return num;
        }

        internal void ResetCachedBounds(ItemTemplate it)
        {
            foreach (ListItem item in base.List)
            {
                if ((it == null) || (item.Template == it))
                {
                    item.ResetCachedBounds();
                }
            }
        }

        internal void ResetSelected()
        {
            if (this.m_selectedItem != null)
            {
                this.m_selectedItem.Selected = false;
            }
        }

        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            int num = (this.m_selectedItem.Index < 0) ? 0 : this.m_selectedItem.Index;
            if (num < base.List.Count)
            {
                ListItem item = (ListItem) base.InnerList[num];
                if ((item.FieldNames != null) && ((listAccessors == null) || (listAccessors.Length == 0)))
                {
                    return item.FieldNames.GetPropertyDescriptors();
                }
            }
            return new PropertyDescriptorCollection(null);
        }

        string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
        {
            return string.Empty;
        }

        public ListItem this[int index]
        {
            get
            {
                return (ListItem) base.InnerList[index];
            }
            set
            {
                base.RemoveAt(index);
                this.Insert(index, value);
            }
        }

        internal Resco.Controls.AdvancedComboBox.AdvancedComboBox Parent
        {
            get
            {
                return this.m_parent;
            }
        }

        public ListItem SelectedItem
        {
            get
            {
                return this.m_selectedItem;
            }
            set
            {
                if (value != this.m_selectedItem)
                {
                    if (this.m_selectedItem != null)
                    {
                        this.m_selectedItem.Selected = false;
                    }
                    if (value != null)
                    {
                        value.Selected = true;
                    }
                    this.m_selectedItem = value;
                }
            }
        }
    }
}

