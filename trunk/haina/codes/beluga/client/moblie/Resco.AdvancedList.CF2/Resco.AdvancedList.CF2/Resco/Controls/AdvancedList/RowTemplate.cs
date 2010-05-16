namespace Resco.Controls.AdvancedList
{
    using Resco.Controls.AdvancedList.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class RowTemplate : Component
    {
        internal bool AutoHeight;
        protected internal Row CurrentRow;
        internal bool HasCustomizedCells;
        private Color m_BackColor;
        private Rectangle m_bgRect = new Rectangle();
        private CellCollection m_CellTemplates;
        private Color m_ForeColor;
        private GradientColor m_gradientBackColor;
        private int m_iHeight;
        private int m_iWidth;
        private string m_sName = "";
        protected internal Resco.Controls.AdvancedList.AdvancedList Parent;
        private static RowTemplate sDefault;

        internal event GridEventHandler Changed;

        public RowTemplate()
        {
            this.m_CellTemplates = new CellCollection(this);
            this.m_BackColor = SystemColors.ControlLightLight;
            this.m_ForeColor = SystemColors.ControlText;
            this.m_gradientBackColor = new GradientColor();
            this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
            this.m_iHeight = 0x10;
        }

        public virtual RowTemplate Clone()
        {
            RowTemplate template = new RowTemplate();
            foreach (Cell cell in this.m_CellTemplates)
            {
                template.CellTemplates.Add(cell.Clone());
            }
            template.BackColor = this.BackColor;
            template.ForeColor = this.ForeColor;
            template.GradientBackColor = new GradientColor(this.GradientBackColor.StartColor, this.GradientBackColor.MiddleColor1, this.GradientBackColor.MiddleColor2, this.GradientBackColor.EndColor, this.GradientBackColor.FillDirection);
            template.GradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
            template.Height = this.Height;
            template.Name = this.Name;
            return template;
        }

        internal bool CustomizeCells(Row row)
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
                    object data = cell[row, i];
                    CustomizeCellEventArgs e = new CustomizeCellEventArgs(cell, data, row);
                    this.OnCustomizeCell(e);
                    RowSpecificCellProperties rowSpecificProperties = cell.GetRowSpecificProperties(row);
                    bool flag2 = rowSpecificProperties.Visible.Value;
                    rowSpecificProperties.Selectable = new bool?(e.Cell.Selectable);
                    rowSpecificProperties.Visible = new bool?(e.Cell.Visible);
                    rowSpecificProperties.CachedAutoHeight = e.Cell.GetAutoHeight(e.DataRow, i, rowSpecificProperties);
                    rowSpecificProperties.Cea = e;
                    if (e.Cell.Visible != flag2)
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }

        internal static void DisposeDefaultRowTemplate()
        {
            sDefault = null;
        }

        protected internal virtual void Draw(Graphics gr, int xOffset, int yOffset, Row row, int width, int height)
        {
            Color colorKey;
            this.CurrentRow = row;
            if (height == -1)
            {
                height = this.GetHeight(row);
            }
            if (this.m_BackColor == Color.Transparent)
            {
                if (this.Parent.BackgroundType == BackgroundType.btGradient)
                {
                    colorKey = this.Parent.m_colorKey;
                }
                else
                {
                    colorKey = this.Parent.BackColor;
                }
            }
            else
            {
                colorKey = this.m_BackColor;
            }
            if (((this.GradientBackColor != null) && !this.GradientBackColor.CanDraw()) && (this.Parent != null))
            {
                if ((this.Parent.BackgroundType == BackgroundType.btSolid) || (this.m_BackColor != Color.Transparent))
                {
                    gr.FillRectangle(Resco.Controls.AdvancedList.AdvancedList.GetBrush(colorKey), xOffset, yOffset, width, height);
                }
            }
            else
            {
                this.m_bgRect.X = xOffset;
                this.m_bgRect.Y = yOffset;
                this.m_bgRect.Width = width;
                this.m_bgRect.Height = height;
                this.m_gradientBackColor.DrawGradient(gr, this.m_bgRect);
            }
            int count = this.m_CellTemplates.Count;
            for (int i = 0; i < count; i++)
            {
                Cell cell = this.m_CellTemplates[i];
                RowSpecificCellProperties rscp = null;
                object data = null;
                if (cell.CustomizeCell && (this.Parent != null))
                {
                    rscp = cell.GetRowSpecificProperties(row);
                    if (rscp.Cea != null)
                    {
                        cell = rscp.Cea.Cell;
                        data = rscp.Cea.Data;
                    }
                }
                else
                {
                    data = cell[row, i];
                }
                if (cell is ButtonCell)
                {
                    ((ButtonCell) cell).Pressed = row.PressedButtonIndex == i;
                }
                if ((cell != null) && cell.Visible)
                {
                    cell.Draw(gr, xOffset, yOffset, width, data, rscp);
                }
                if (rscp != null)
                {
                    rscp.Cea = null;
                }
            }
        }

        internal int GetCellClick(int pos_x, int pos_y, Row row)
        {
            int num = this.m_CellTemplates.Count - 1;
            while (num >= 0)
            {
                RowSpecificCellProperties rowSpecificProperties = this.m_CellTemplates[num].GetRowSpecificProperties(row);
                if ((rowSpecificProperties.Visible.Value && rowSpecificProperties.CachedBounds.HasValue) && rowSpecificProperties.CachedBounds.Value.Contains(pos_x, pos_y))
                {
                    return num;
                }
                num--;
            }
            return num;
        }

        public int GetHeight(Row row)
        {
            if (row.RecalculationNeeded)
            {
                row.RecalculationNeeded = false;
                if (!this.AutoHeight)
                {
                    row.ActualHeight = this.m_iHeight;
                    return this.m_iHeight;
                }
                int iHeight = this.m_iHeight;
                for (int i = 0; i < this.CellTemplates.Count; i++)
                {
                    Cell cell = this.CellTemplates[i];
                    if (cell.AutoHeight)
                    {
                        int num3 = ((cell.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom) ? cell.m_anchorSizeBottom : 0;
                        iHeight = Math.Max(iHeight, (cell.GetAutoHeight(row, i, null) + cell.Bounds.Top) + num3);
                    }
                }
                row.ActualHeight = iHeight;
            }
            return row.ActualHeight;
        }

        private void m_gradientBackColor_PropertyChanged(object sender, EventArgs e)
        {
            this.OnChange(this, GridEventArgsType.Repaint, null);
        }

        protected internal virtual void OnCellEntered(CellEnteredMainEventArgs e)
        {
            Cell cell = e.Cell;
            Cell cell1 = this.CellTemplates[e.CellIndex];
        }

        internal virtual void OnChange(object sender, GridEventArgsType e, object oParam)
        {
            if (this.Changed != null)
            {
                this.Changed(sender, e, oParam);
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
            this.OnChange(this, GridEventArgsType.Refresh, new Resco.Controls.AdvancedList.AdvancedList.RefreshData(this));
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
            bool flag = this.m_gradientBackColor.StartColor != Color.Transparent;
            bool flag2 = this.m_gradientBackColor.EndColor != Color.Transparent;
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
                    this.OnChange(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Content)]
        public CellCollection CellTemplates
        {
            get
            {
                return this.m_CellTemplates;
            }
        }

        public static RowTemplate Default
        {
            get
            {
                if (sDefault == null)
                {
                    sDefault = new RowTemplate();
                }
                return sDefault;
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
                    this.OnChange(this, GridEventArgsType.Repaint, null);
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
                this.OnChange(this, GridEventArgsType.Repaint, null);
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
                    this.OnChange(this, GridEventArgsType.Refresh, new Resco.Controls.AdvancedList.AdvancedList.RefreshData(this));
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
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

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
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

        [DefaultValue(""), Resco.Controls.AdvancedList.Design.Browsable(false)]
        public string Name
        {
            get
            {
                if (this.Site != null)
                {
                    return this.Site.Name;
                }
                return this.m_sName;
            }
            set
            {
                try
                {
                    if ((this.Site != null) && (this.Site.Name != value))
                    {
                        this.Site.Name = value;
                    }
                }
                catch (Exception exception)
                {
                    if ((this.Site != null) && (this.Site.Name != value))
                    {
                        value = this.Site.Name;
                    }
                    throw exception;
                }
                finally
                {
                    this.m_sName = value;
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false)]
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
            internal RowTemplate m_Owner;

            public CellCollection(RowTemplate rt)
            {
                this.m_Owner = rt;
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
                if ((this.m_Owner != null) && (this.m_Owner.Parent != null))
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
                if (this.m_Owner != null)
                {
                    this.m_Owner.AutoHeight = false;
                    if (this.m_Owner.Parent != null)
                    {
                        this.m_Owner.Parent.ResumeRedraw();
                    }
                }
            }

            protected override void OnInsert(int index, object value)
            {
                RowTemplate owner = ((Cell) value).Owner;
                if ((owner != null) && (owner != this.m_Owner))
                {
                    throw new ArgumentException("Cell is already part of other RowTemplate");
                }
                if (this.m_Owner.Parent != null)
                {
                    this.m_Owner.Parent.SuspendRedraw();
                }
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
                    this.m_Owner.Parent.ResumeRedraw();
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
                    this.m_Owner.Parent.ResumeRedraw();
                }
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                RowTemplate owner = ((Cell) newValue).Owner;
                if ((owner != null) && (owner != this.m_Owner))
                {
                    throw new ArgumentException("Cell is already part of other RowTemplate");
                }
                if (this.m_Owner.Parent != null)
                {
                    this.m_Owner.Parent.SuspendRedraw();
                }
            }

            protected override void OnSetComplete(int index, object oldValue, object newValue)
            {
                Cell cell = (Cell) newValue;
                cell.Owner = this.m_Owner;
                cell.Bounds = cell.Bounds;
                Cell cell2 = (Cell) oldValue;
                cell2.Owner = null;
                if ((cell.AutoHeight != cell2.AutoHeight) || (cell.CustomizeCell != cell2.CustomizeCell))
                {
                    this.m_Owner.RefreshProperties();
                }
                if (this.m_Owner.Parent != null)
                {
                    this.m_Owner.Parent.ResumeRedraw();
                }
            }

            public void Remove(Cell value)
            {
                base.List.Remove(value);
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

