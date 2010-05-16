namespace Resco.Controls.AdvancedComboBox
{
    using Resco.Controls.AdvancedComboBox.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ItemTemplate : Component
    {
        internal bool AutoHeight;
        protected internal Resco.Controls.AdvancedComboBox.ListItem CurrentItem;
        internal bool HasCustomizedCells;
        private Color m_BackColor;
        private Rectangle m_bgRect = new Rectangle();
        private CellCollection m_CellTemplates;
        private Color m_ForeColor;
        private GradientColor m_gradientBackColor;
        private int m_iHeight;
        private int m_iWidth;
        private string m_sName = "";
        private bool m_useGradient;
        protected internal Resco.Controls.AdvancedComboBox.AdvancedComboBox Parent;

        internal event ComboBoxEventHandler Changed;

        public ItemTemplate()
        {
            this.m_CellTemplates = new CellCollection(this);
            this.m_BackColor = SystemColors.ControlLightLight;
            this.m_ForeColor = SystemColors.ControlText;
            this.m_gradientBackColor = new GradientColor();
            this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
            this.m_useGradient = false;
            this.m_iHeight = 0x10;
        }

        public virtual ItemTemplate Clone()
        {
            ItemTemplate template = new ItemTemplate();
            foreach (Cell cell in this.m_CellTemplates)
            {
                template.CellTemplates.Add(cell.Clone());
            }
            template.BackColor = this.BackColor;
            template.ForeColor = this.ForeColor;
            template.GradientBackColor = new GradientColor(this.GradientBackColor.StartColor, this.GradientBackColor.MiddleColor1, this.GradientBackColor.MiddleColor2, this.GradientBackColor.EndColor, this.GradientBackColor.FillDirection);
            template.GradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
            template.UseGradient = this.UseGradient;
            template.Height = this.Height;
            template.Name = this.Name;
            return template;
        }

        internal bool CustomizeCells(Resco.Controls.AdvancedComboBox.ListItem item)
        {
            bool flag = false;
            if (!this.HasCustomizedCells || (this.Parent == null))
            {
                return false;
            }
            int count = this.m_CellTemplates.Count;
            for (int i = 0; i < count; i++)
            {
                Cell cell = this.m_CellTemplates[i];
                if (cell.CustomizeCell)
                {
                    object data = cell[item, i];
                    CustomizeCellEventArgs e = new CustomizeCellEventArgs(cell, data, item);
                    this.OnCustomizeCell(e);
                    ItemSpecificCellProperties itemSpecificProperties = cell.GetItemSpecificProperties(item);
                    bool flag2 = itemSpecificProperties.Visible.HasValue ? itemSpecificProperties.Visible.Value : cell.Visible;
                    itemSpecificProperties.Visible = new bool?(e.Cell.Visible);
                    itemSpecificProperties.CachedAutoHeight = e.Cell.GetAutoHeight(e.DataItem, i, itemSpecificProperties);
                    itemSpecificProperties.Cea = e;
                    if (e.Cell.Visible != flag2)
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }

        protected internal virtual void Draw(Graphics gr, Rectangle rect, Resco.Controls.AdvancedComboBox.ListItem item)
        {
            this.CurrentItem = item;
            int height = this.GetHeight(item);
            if ((this.Parent != null) && (this.m_BackColor != Color.Transparent))
            {
                gr.FillRectangle(Resco.Controls.AdvancedComboBox.AdvancedComboBox.GetBrush(this.m_BackColor), rect.Left, rect.Top, rect.Width, height);
            }
            for (int i = 0; i < this.m_CellTemplates.Count; i++)
            {
                Cell cell = this.m_CellTemplates[i];
                ItemSpecificCellProperties iscp = null;
                object data = null;
                if (cell.CustomizeCell && (this.Parent != null))
                {
                    iscp = cell.GetItemSpecificProperties(item);
                    if (iscp.Cea != null)
                    {
                        cell = iscp.Cea.Cell;
                        data = iscp.Cea.Data;
                    }
                }
                else
                {
                    data = cell[item, i];
                }
                if (cell is ButtonCell)
                {
                    ((ButtonCell) cell).Pressed = item.PressedButtonIndex == i;
                }
                if ((cell != null) && cell.Visible)
                {
                    cell.Draw(gr, 0, rect.Top, rect.Width, data, iscp);
                }
                if (iscp != null)
                {
                    iscp.Cea = null;
                }
            }
        }

        protected internal virtual void Draw(Graphics gr, int xOffset, int yOffset, Resco.Controls.AdvancedComboBox.ListItem item, int width, int height)
        {
            int num = (((this.Parent != null) ? this.Parent.RightToLeft : false) && this.Parent.List.ScrollbarVisible) ? this.Parent.List.ClientScrollbarWidth : 0;
            Color backColor = this.m_BackColor;
            this.CurrentItem = item;
            if (height == -1)
            {
                height = this.GetHeight(item);
            }
            if (backColor == Color.Transparent)
            {
                if (this.Parent.List.UseGradient)
                {
                    backColor = this.Parent.m_colorKey;
                }
                else
                {
                    backColor = this.Parent.BackColor;
                }
            }
            if (!this.m_useGradient && (this.Parent != null))
            {
                if ((this.Parent.DoubleBuffered && (this.Parent.List.GradientBackColor.FillDirection == FillDirection.Vertical)) || (!this.Parent.List.UseGradient || (this.m_BackColor != Color.Transparent)))
                {
                    gr.FillRectangle(Resco.Controls.AdvancedComboBox.AdvancedComboBox.GetBrush(backColor), xOffset + num, yOffset, width, height);
                }
            }
            else
            {
                this.m_bgRect.X = xOffset + num;
                this.m_bgRect.Y = yOffset;
                this.m_bgRect.Width = width;
                this.m_bgRect.Height = height;
                this.m_gradientBackColor.DrawGradient(gr, this.m_bgRect);
            }
            int count = this.m_CellTemplates.Count;
            for (int i = 0; i < count; i++)
            {
                Cell cell = this.m_CellTemplates[i];
                ItemSpecificCellProperties iscp = null;
                object data = null;
                if (cell.CustomizeCell && (this.Parent != null))
                {
                    iscp = cell.GetItemSpecificProperties(item);
                    if (iscp.Cea != null)
                    {
                        cell = iscp.Cea.Cell;
                        data = iscp.Cea.Data;
                    }
                }
                else
                {
                    data = cell[item, i];
                }
                if (cell is ButtonCell)
                {
                    ((ButtonCell) cell).Pressed = item.PressedButtonIndex == i;
                }
                if ((cell != null) && cell.Visible)
                {
                    cell.Draw(gr, xOffset, yOffset, width, data, iscp);
                }
                if (iscp != null)
                {
                    iscp.Cea = null;
                }
            }
        }

        internal int GetCellClick(int pos_x, int pos_y, Resco.Controls.AdvancedComboBox.ListItem item)
        {
            for (int i = this.m_CellTemplates.Count - 1; i >= 0; i--)
            {
                ItemSpecificCellProperties itemSpecificProperties = this.m_CellTemplates[i].GetItemSpecificProperties(item);
                if ((itemSpecificProperties.Visible.Value && itemSpecificProperties.CachedBounds.HasValue) && itemSpecificProperties.CachedBounds.Value.Contains(pos_x, pos_y))
                {
                    return i;
                }
            }
            return -1;
        }

        public int GetHeight(Resco.Controls.AdvancedComboBox.ListItem item)
        {
            if (item.RecalculationNeeded)
            {
                item.RecalculationNeeded = false;
                if (!this.AutoHeight)
                {
                    item.ActualHeight = this.m_iHeight;
                    return this.m_iHeight;
                }
                int iHeight = this.m_iHeight;
                for (int i = 0; i < this.CellTemplates.Count; i++)
                {
                    Cell cell = this.CellTemplates[i];
                    if (cell.AutoHeight)
                    {
                        int num3 = ((cell.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom) ? cell.m_anchorSizeBottom : 0;
                        iHeight = Math.Max(iHeight, (cell.GetAutoHeight(item, i, null) + cell.Bounds.Top) + num3);
                    }
                }
                item.ActualHeight = iHeight;
            }
            return item.ActualHeight;
        }

        private void m_gradientBackColor_PropertyChanged(object sender, EventArgs e)
        {
            this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
        }

        protected internal virtual void OnCellEntered(CellEnteredMainEventArgs e)
        {
        }

        internal virtual void OnChange(object sender, ComboBoxEventArgsType e, ComboBoxArgs args)
        {
            if (this.Changed != null)
            {
                this.Changed(sender, e, args);
            }
        }

        protected virtual void OnCustomizeCell(CustomizeCellEventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.OnCustomizeCell(e);
            }
        }

        internal bool RefreshProperties()
        {
            bool autoHeight = this.AutoHeight;
            bool hasCustomizedCells = this.HasCustomizedCells;
            this.AutoHeight = false;
            this.HasCustomizedCells = false;
            for (int i = 0; i < this.CellTemplates.Count; i++)
            {
                this.AutoHeight = this.AutoHeight || this.CellTemplates[i].AutoHeight;
                this.HasCustomizedCells = this.HasCustomizedCells || this.CellTemplates[i].CustomizeCell;
            }
            if ((autoHeight == this.AutoHeight) && (hasCustomizedCells == this.HasCustomizedCells))
            {
                return false;
            }
            this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(this));
            return true;
        }

        public void Scale(float fx, float fy)
        {
            this.Height = (int) (fy * this.Height);
            this.m_iWidth = (int) (fx * this.Width);
            foreach (Cell cell in this.CellTemplates)
            {
                cell.Scale(fx, fy);
            }
        }

        protected virtual bool ShouldSerializeGradientBackColor()
        {
            bool flag = this.m_gradientBackColor.StartColor.ToArgb() != SystemColors.ControlLightLight.ToArgb();
            bool flag2 = this.m_gradientBackColor.EndColor.ToArgb() != SystemColors.ControlLightLight.ToArgb();
            bool flag3 = this.m_gradientBackColor.MiddleColor1 != Color.Transparent;
            bool flag4 = this.m_gradientBackColor.MiddleColor2 != Color.Transparent;
            return ((((flag | flag2) | flag3) | flag4) | (this.m_gradientBackColor.FillDirection != FillDirection.Horizontal));
        }

        public override string ToString()
        {
            return this.m_sName;
        }

        [DefaultValue("ControlLightLight")]
        public virtual Color BackColor
        {
            get
            {
                return this.m_BackColor;
            }
            set
            {
                if (this.m_BackColor != value)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.ControlLightLight;
                    }
                    this.m_BackColor = value;
                    this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                }
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Content)]
        public CellCollection CellTemplates
        {
            get
            {
                return this.m_CellTemplates;
            }
        }

        [DefaultValue("ControlText")]
        public virtual Color ForeColor
        {
            get
            {
                return this.m_ForeColor;
            }
            set
            {
                if (this.m_ForeColor != value)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.ControlText;
                    }
                    this.m_ForeColor = value;
                    this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                }
            }
        }

        public GradientColor GradientBackColor
        {
            get
            {
                return this.m_gradientBackColor;
            }
            set
            {
                if (this.m_gradientBackColor != value)
                {
                    this.m_gradientBackColor.PropertyChanged -= new EventHandler(this.m_gradientBackColor_PropertyChanged);
                    this.m_gradientBackColor = value;
                    this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
                }
                this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
            }
        }

        [DefaultValue(0x10)]
        public int Height
        {
            get
            {
                return this.m_iHeight;
            }
            set
            {
                if (this.m_iHeight != value)
                {
                    this.m_iHeight = value;
                    this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(this));
                }
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden)]
        public Cell this[string name]
        {
            get
            {
                return this.m_CellTemplates[name];
            }
            set
            {
                this.m_CellTemplates[name] = value;
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden)]
        public Cell this[int index]
        {
            get
            {
                return this.m_CellTemplates[index];
            }
            set
            {
                this.m_CellTemplates[index] = value;
            }
        }

        [DefaultValue("")]
        public string Name
        {
            get
            {
                return this.m_sName;
            }
            set
            {
                this.m_sName = value;
            }
        }

        public bool UseGradient
        {
            get
            {
                return this.m_useGradient;
            }
            set
            {
                if (this.m_useGradient != value)
                {
                    this.m_useGradient = value;
                    this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                }
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.Browsable(false)]
        internal int Width
        {
            get
            {
                return this.m_iWidth;
            }
            set
            {
                if (value != this.m_iWidth)
                {
                    this.m_iWidth = value;
                    foreach (Cell cell in this.CellTemplates)
                    {
                        cell.RecalculateAnchors();
                    }
                }
            }
        }

        public class CellCollection : CollectionBase
        {
            internal ItemTemplate m_Owner;

            public CellCollection(ItemTemplate it)
            {
                this.m_Owner = it;
            }

            public int Add(Cell value)
            {
                return base.List.Add(value);
            }

            public bool Contains(Cell value)
            {
                return base.List.Contains(value);
            }

            public int IndexOf(Cell value)
            {
                return base.List.IndexOf(value);
            }

            public void Insert(int index, Cell value)
            {
                base.List.Insert(index, value);
            }

            protected override void OnClear()
            {
                if (this.m_Owner.Parent != null)
                {
                    this.m_Owner.Parent.SuspendRedraw();
                }
                foreach (Cell cell in base.List)
                {
                    cell.Owner = null;
                }
                base.OnClear();
            }

            protected override void OnClearComplete()
            {
                base.OnClearComplete();
                this.m_Owner.AutoHeight = false;
                if (this.m_Owner.Parent != null)
                {
                    this.m_Owner.Parent.ResumeRedraw(false);
                }
            }

            protected override void OnInsert(int index, object value)
            {
                this.SuspendRedraw(((Cell) value).Owner);
            }

            protected override void OnInsertComplete(int index, object value)
            {
                Cell cell = (Cell) value;
                cell.Owner = this.m_Owner;
                cell.Bounds = cell.Bounds;
                if (cell.AutoHeight || cell.CustomizeCell)
                {
                    this.m_Owner.RefreshProperties();
                }
                if (this.m_Owner.Parent != null)
                {
                    this.m_Owner.Parent.ResumeRedraw(false);
                }
            }

            protected override void OnRemoveComplete(int index, object value)
            {
                Cell cell = (Cell) value;
                cell.Owner = null;
                if ((this.m_Owner.AutoHeight && cell.AutoHeight) || (this.m_Owner.HasCustomizedCells && cell.CustomizeCell))
                {
                    this.m_Owner.RefreshProperties();
                }
                if (this.m_Owner.Parent != null)
                {
                    this.m_Owner.Parent.ResumeRedraw(false);
                }
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                this.SuspendRedraw(((Cell) newValue).Owner);
            }

            protected override void OnSetComplete(int index, object oldValue, object newValue)
            {
                Cell cell = (Cell) newValue;
                Cell cell2 = (Cell) oldValue;
                cell.Owner = this.m_Owner;
                cell.Bounds = cell.Bounds;
                ((Cell) oldValue).Owner = null;
                if ((cell.AutoHeight != cell2.AutoHeight) || (cell.CustomizeCell != cell2.CustomizeCell))
                {
                    this.m_Owner.RefreshProperties();
                }
                if (this.m_Owner.Parent != null)
                {
                    this.m_Owner.Parent.ResumeRedraw(false);
                }
            }

            public void Remove(Cell value)
            {
                base.List.Remove(value);
            }

            private void SuspendRedraw(ItemTemplate it)
            {
                if ((it != null) && (it != this.m_Owner))
                {
                    throw new ArgumentException("Cell is already part of other ItemTemplate");
                }
                if (this.m_Owner.Parent != null)
                {
                    this.m_Owner.Parent.SuspendRedraw();
                }
            }

            public Cell this[string name]
            {
                get
                {
                    for (int i = 0; i < base.List.Count; i++)
                    {
                        Cell cell = (Cell) base.InnerList[i];
                        if (cell.Name == name)
                        {
                            return cell;
                        }
                    }
                    return null;
                }
                set
                {
                    for (int i = 0; i < base.List.Count; i++)
                    {
                        Cell cell = (Cell) base.InnerList[i];
                        if (cell.Name == name)
                        {
                            base.List[i] = value;
                            return;
                        }
                    }
                }
            }

            public Cell this[int i]
            {
                get
                {
                    return (Cell) base.InnerList[i];
                }
                set
                {
                    base.List[i] = value;
                }
            }
        }
    }
}

