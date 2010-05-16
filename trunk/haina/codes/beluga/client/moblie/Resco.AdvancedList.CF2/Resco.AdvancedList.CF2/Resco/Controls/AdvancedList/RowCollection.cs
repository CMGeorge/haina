namespace Resco.Controls.AdvancedList
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class RowCollection : CollectionBase, ITypedList
    {
        internal int LastDrawnRow;
        internal int LastDrawnRowOffset;
        internal bool m_bDrawGrid = true;
        private int m_cSelected;
        internal Pen m_penBorder = new Pen(Color.DarkGray);
        private Resco.Controls.AdvancedList.AdvancedList m_pParent;
        private Row m_rActive;

        internal event GridEventHandler Changed;

        internal RowCollection(Resco.Controls.AdvancedList.AdvancedList parent)
        {
            this.m_pParent = parent;
        }

        public int Add(Row row)
        {
            if (row.Parent != null)
            {
                throw new ApplicationException("Row can't be in more collections");
            }
            return base.List.Add(row);
        }

        internal int CalculateRowsHeight()
        {
            int num = 0;
            foreach (Row row in base.List)
            {
                RowTemplate template = this.GetTemplate(row);
                num += template.GetHeight(row);
                if (this.m_bDrawGrid)
                {
                    num++;
                }
            }
            return num;
        }

        public bool Contains(Row value)
        {
            return base.List.Contains(value);
        }

        internal int Draw(Graphics gr, TemplateSet ts, int xOffset, int width, int yOffset, int ymax, int iRow, int iRowOffset, ref bool resetScrollbar)
        {
            int num = yOffset + iRowOffset;
            int count = base.List.Count;
            this.LastDrawnRowOffset = num;
            this.LastDrawnRow = iRow;
            while (this.LastDrawnRow < count)
            {
                Row row = null;
                if ((this.LastDrawnRow >= 0) && (this.LastDrawnRow < base.InnerList.Count))
                {
                    row = base.InnerList[this.LastDrawnRow] as Row;
                }
                if (row != null)
                {
                    RowTemplate template = row.GetTemplate(ts);
                    int height = 0;
                    int num4 = -1;
                    if (template.CustomizeCells(row))
                    {
                        num4 = template.GetHeight(row);
                        row.ResetCachedBounds();
                    }
                    height = template.GetHeight(row);
                    if ((num4 >= 0) && (height != num4))
                    {
                        resetScrollbar = true;
                    }
                    if ((num + height) >= 0)
                    {
                        template.Draw(gr, xOffset, num, row, width, height);
                    }
                    this.LastDrawnRowOffset = num;
                    num += height;
                    if (this.m_bDrawGrid)
                    {
                        gr.DrawLine(this.m_penBorder, xOffset, num, width + xOffset, num);
                        num++;
                    }
                    if (num > ymax)
                    {
                        return num;
                    }
                }
                this.LastDrawnRow++;
            }
            this.LastDrawnRowOffset = num;
            return num;
        }

        internal int GetHeight(int i, TemplateSet ts)
        {
            if ((i < 0) || (i >= base.List.Count))
            {
                return 0;
            }
            Row row = (Row) base.InnerList[i];
            RowTemplate template = ts[row.CurrentTemplateIndex];
            if (template == null)
            {
                template = RowTemplate.Default;
            }
            if (this.m_bDrawGrid)
            {
                return (template.GetHeight(row) + 1);
            }
            return template.GetHeight(row);
        }

        internal Point GetRowClick(int iRow, int iRowOffset, int pos_x, int pos_y, out int yOffset)
        {
            yOffset = 0;
            int num = iRowOffset;
            if (num <= pos_y)
            {
                for (int i = iRow; i < base.List.Count; i++)
                {
                    Row r = (Row) base.InnerList[i];
                    RowTemplate template = this.GetTemplate(r);
                    int height = template.GetHeight(r);
                    yOffset = num;
                    num += this.m_bDrawGrid ? (height + 1) : height;
                    if (num > pos_y)
                    {
                        return new Point(i, template.GetCellClick(pos_x, pos_y - yOffset, r));
                    }
                }
            }
            return new Point(-1, -1);
        }

        internal RowTemplate GetTemplate(Row r)
        {
            TemplateSet templates = this.m_pParent.Templates;
            int count = templates.Count;
            int currentTemplateIndex = r.CurrentTemplateIndex;
            if ((currentTemplateIndex >= 0) && (currentTemplateIndex < count))
            {
                return templates[currentTemplateIndex];
            }
            return RowTemplate.Default;
        }

        internal RowTemplate GetTemplate(Row r, int ix)
        {
            TemplateSet templates = this.m_pParent.Templates;
            int count = templates.Count;
            if ((ix >= 0) && (ix < count))
            {
                return templates[ix];
            }
            return RowTemplate.Default;
        }

        public int IndexOf(Row row)
        {
            return base.List.IndexOf(row);
        }

        public void Insert(int index, Row row)
        {
            if (row.Parent != null)
            {
                throw new ApplicationException("Row can't be in more collections");
            }
            base.List.Insert(index, row);
        }

        internal void OnChanged(object sender, GridEventArgsType e, object oParam)
        {
            if (this.Changed != null)
            {
                this.Changed(sender, e, oParam);
            }
        }

        protected override void OnClear()
        {
            foreach (Row row in base.List)
            {
                row.Parent = null;
            }
            base.OnClear();
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            this.m_cSelected = 0;
            this.m_rActive = null;
            this.LastDrawnRow = 0;
            if (this.Parent != null)
            {
                this.Parent.OnClear(this);
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            Row sender = value as Row;
            base.OnInsertComplete(index, value);
            for (int i = index + 1; i < base.Count; i++)
            {
                Row row1 = this[i];
                row1.m_index++;
            }
            if (sender != null)
            {
                if (sender.Selected)
                {
                    this.OnRowChange(sender, RowEventArgsType.Selection, false);
                }
                sender.Parent = this;
                sender.m_index = index;
                if (this.Parent != null)
                {
                    this.Parent.OnRowAdded(sender, index);
                }
            }
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);
            for (int i = index; i < base.Count; i++)
            {
                Row row1 = this[i];
                row1.m_index--;
            }
            Row row = value as Row;
            if (row != null)
            {
                if (this.Parent != null)
                {
                    this.Parent.OnRowRemoved(row, index);
                }
                row.Parent = null;
                if (row.Selected)
                {
                    if (row.Active)
                    {
                        this.m_rActive = null;
                    }
                    if (this.m_cSelected > 0)
                    {
                        this.m_cSelected--;
                    }
                }
            }
        }

        internal void OnRowChange(object sender, RowEventArgsType e, object oParam)
        {
            object obj2 = oParam;
            Row r = (Row) sender;
            RowTemplate template = this.GetTemplate(r);
            switch (e)
            {
                case RowEventArgsType.Selection:
                {
                    if (!r.Selected)
                    {
                        if (this.m_cSelected > 0)
                        {
                            this.m_cSelected--;
                        }
                        if (r != this.m_rActive)
                        {
                            break;
                        }
                        this.m_rActive = null;
                        return;
                    }
                    bool flag = (bool) oParam;
                    if (this.m_rActive != null)
                    {
                        this.m_rActive.ChangeSelection(this.Parent.MultiSelect, false, false);
                    }
                    if (!flag)
                    {
                        this.m_cSelected = this.m_pParent.MultiSelect ? (this.m_cSelected + 1) : 1;
                    }
                    this.m_rActive = r;
                    return;
                }
                case RowEventArgsType.TemplateIndex:
                {
                    int num = 0;
                    num = -this.GetTemplate(r, (int) oParam).GetHeight(r);
                    r.ResetCachedBounds();
                    num += template.GetHeight(r);
                    this.OnChanged(base.List.IndexOf(r), GridEventArgsType.Resize, num);
                    return;
                }
                default:
                    if (template.AutoHeight)
                    {
                        int actualHeight = r.ActualHeight;
                        r.ResetCachedBounds();
                        int height = template.GetHeight(r);
                        if (height != actualHeight)
                        {
                            this.OnChanged(base.List.IndexOf(r), GridEventArgsType.Resize, height - actualHeight);
                            return;
                        }
                    }
                    this.Changed(base.List.IndexOf(sender), GridEventArgsType.RowChange, obj2);
                    break;
            }
        }

        public void Remove(Row row)
        {
            base.List.Remove(row);
        }

        public int RemoveByMapping(Resco.Controls.AdvancedList.Mapping fieldNames)
        {
            int num = -1;
            for (int i = base.List.Count - 1; i >= 0; i--)
            {
                Row row = (Row) base.InnerList[i];
                if (row.FieldNames == fieldNames)
                {
                    base.List.RemoveAt(i);
                    num = i;
                }
            }
            return num;
        }

        internal void ResetCachedBounds(RowTemplate rt)
        {
            foreach (Row row in base.List)
            {
                if ((rt == null) || (row.Template == rt))
                {
                    row.ResetCachedBounds();
                }
            }
        }

        internal void ResetSelected()
        {
            for (int i = 0; i < base.List.Count; i++)
            {
                ((Row) base.InnerList[i]).Selected = false;
            }
            this.m_cSelected = 0;
            this.m_rActive = null;
        }

        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            int num = (this.Parent.ActiveRowIndex < 0) ? 0 : this.Parent.ActiveRowIndex;
            if (num < base.List.Count)
            {
                Row row = (Row) base.InnerList[num];
                if ((row.FieldNames != null) && ((listAccessors == null) || (listAccessors.Length == 0)))
                {
                    return row.FieldNames.GetPropertyDescriptors();
                }
            }
            return new PropertyDescriptorCollection(null);
        }

        string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
        {
            return string.Empty;
        }

        internal Row ActiveRow
        {
            get
            {
                return this.m_rActive;
            }
        }

        public Row this[int index]
        {
            get
            {
                return (Row) base.InnerList[index];
            }
            set
            {
                base.RemoveAt(index);
                this.Insert(index, value);
            }
        }

        internal Resco.Controls.AdvancedList.AdvancedList Parent
        {
            get
            {
                return this.m_pParent;
            }
            set
            {
                this.m_pParent = value;
            }
        }

        public int SelectedCount
        {
            get
            {
                return this.m_cSelected;
            }
        }
    }
}

