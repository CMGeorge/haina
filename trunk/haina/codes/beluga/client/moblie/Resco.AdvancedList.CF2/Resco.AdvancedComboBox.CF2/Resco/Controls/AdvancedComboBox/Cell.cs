namespace Resco.Controls.AdvancedComboBox
{
    using Resco.Controls.AdvancedComboBox.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class Cell : Component
    {
        protected object CurrentData;
        private AnchorStyles m_anchor;
        internal int m_anchorSizeBottom;
        internal int m_anchorSizeLeft;
        internal int m_anchorSizeRight;
        internal int m_anchorSizeTop;
        private Color m_BackColor;
        private BorderType m_Border;
        private Rectangle m_Bounds;
        private bool m_bSelectable;
        private bool m_bVisible;
        private Resco.Controls.AdvancedComboBox.CellSource m_cellSource;
        private bool m_CustomizeCell;
        private Color m_ForeColor;
        private bool m_IsAutoHeight;
        private ItemTemplate m_Owner;
        private SizeF m_scale;
        private string m_sName;

        public event CellEnteredEventHandler CellEntered;

        internal event ComboBoxEventHandler Changed;

        public Cell()
        {
            this.m_anchor = AnchorStyles.Left | AnchorStyles.Top;
            this.m_bVisible = true;
            this.m_sName = "";
            this.Changed = null;
            this.m_anchor = AnchorStyles.Left | AnchorStyles.Top;
            this.m_BackColor = Resco.Controls.AdvancedComboBox.AdvancedComboBox.TransparentColor;
            this.m_ForeColor = Resco.Controls.AdvancedComboBox.AdvancedComboBox.TransparentColor;
            this.m_Bounds = new Rectangle(-1, -1, -1, -1);
            this.m_cellSource = new Resco.Controls.AdvancedComboBox.CellSource();
            this.m_cellSource.Parent = this;
            this.m_Border = BorderType.None;
            this.m_IsAutoHeight = false;
            this.m_CustomizeCell = false;
            this.m_scale = new SizeF(1f, 1f);
        }

        public Cell(Cell cell)
        {
            this.m_anchor = AnchorStyles.Left | AnchorStyles.Top;
            this.m_bVisible = true;
            this.m_sName = "";
            this.Changed = null;
            this.m_anchor = cell.m_anchor;
            this.m_BackColor = cell.m_BackColor;
            this.m_ForeColor = cell.m_ForeColor;
            this.m_Bounds = cell.m_Bounds;
            this.m_cellSource = cell.m_cellSource.Copy();
            this.m_Border = cell.m_Border;
            this.m_CustomizeCell = cell.m_CustomizeCell;
            this.Name = cell.Name;
            this.m_IsAutoHeight = cell.m_IsAutoHeight;
            this.m_bSelectable = cell.m_bSelectable;
            this.m_bVisible = cell.m_bVisible;
            this.m_scale = cell.m_scale;
        }

        internal void _FireCellEntered(CellEnteredEventArgs e)
        {
            this.OnCellEntered(e);
        }

        protected internal virtual Rectangle CalculateBounds(int xOffset, int yOffset, Resco.Controls.AdvancedComboBox.ListItem item, int gridWidth, ItemSpecificCellProperties preRscp)
        {
            Rectangle rectangle;
            ItemSpecificCellProperties itemSpecificProperties = preRscp;
            if (itemSpecificProperties == null)
            {
                itemSpecificProperties = this.GetItemSpecificProperties(item);
            }
            if ((itemSpecificProperties != null) && itemSpecificProperties.CachedBounds.HasValue)
            {
                rectangle = itemSpecificProperties.CachedBounds.Value;
            }
            else
            {
                int cachedAutoHeight = 0;
                if ((itemSpecificProperties != null) && (itemSpecificProperties.CachedAutoHeight >= 0))
                {
                    cachedAutoHeight = itemSpecificProperties.CachedAutoHeight;
                }
                rectangle = this.CalculateCellWidth(gridWidth);
                if (this.AutoHeight && (cachedAutoHeight > this.m_Bounds.Height))
                {
                    rectangle.Height = cachedAutoHeight;
                }
                if ((this.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                {
                    if ((this.Anchor & AnchorStyles.Top) == AnchorStyles.Top)
                    {
                        rectangle.Height = item.ActualHeight - (this.m_anchorSizeTop + this.m_anchorSizeBottom);
                        if (rectangle.Height < 0)
                        {
                            rectangle.Height = 0;
                        }
                    }
                    else
                    {
                        rectangle.Y = item.ActualHeight - (this.m_anchorSizeBottom + rectangle.Height);
                        if (rectangle.Y < 0)
                        {
                            rectangle.Y = 0;
                        }
                    }
                }
                else if (rectangle.Bottom > item.ActualHeight)
                {
                    rectangle.Height = item.ActualHeight - rectangle.Y;
                }
                if ((this.Parent != null) ? this.Parent.RightToLeft : false)
                {
                    rectangle.X = this.Parent.Width - rectangle.Right;
                    xOffset *= -1;
                }
                if (itemSpecificProperties != null)
                {
                    itemSpecificProperties.CachedBounds = new Rectangle?(rectangle);
                }
            }
            rectangle.Offset(xOffset, yOffset);
            return rectangle;
        }

        protected Rectangle CalculateCellWidth(int gridWidth)
        {
            Rectangle bounds = this.Bounds;
            if ((this.Anchor & AnchorStyles.Right) == AnchorStyles.Right)
            {
                if (((this.Anchor & AnchorStyles.Left) == AnchorStyles.Left) || (bounds.Width == -1))
                {
                    bounds.Width = gridWidth - (this.m_anchorSizeLeft + this.m_anchorSizeRight);
                    if (bounds.Width < 0)
                    {
                        bounds.Width = 0;
                    }
                    return bounds;
                }
                bounds.X = gridWidth - (bounds.Width + this.m_anchorSizeRight);
                if (bounds.X < 0)
                {
                    bounds.X = 0;
                }
                if (bounds.Right > gridWidth)
                {
                    bounds.Width = gridWidth - bounds.X;
                }
                return bounds;
            }
            if ((bounds.Width == -1) || (bounds.Right > gridWidth))
            {
                bounds.Width = gridWidth - bounds.X;
            }
            return bounds;
        }

        public virtual Cell Clone()
        {
            return new Cell(this);
        }

        protected internal virtual void Draw(System.Drawing.Graphics gr, int xOffset, int yOffset, int gridWidth, object data, ItemSpecificCellProperties iscp)
        {
            this.CurrentData = data;
            Rectangle drawbounds = this.CalculateBounds(xOffset, yOffset, this.Owner.CurrentItem, gridWidth, iscp);
            if (this.m_bVisible)
            {
                this.DrawBackground(gr, drawbounds);
                this.DrawContent(gr, drawbounds, data);
                this.DrawBorder(gr, drawbounds);
                if (this.Selectable)
                {
                    Cell cell = (iscp != null) ? iscp.OriginalCell : this.Parent.SelectedCell;
                    if ((cell == this) && ((this.Owner.CurrentItem == this.Parent.List.SelectedItem) || (this.Owner.CurrentItem == this.Parent.m_textBoxItem)))
                    {
                        this.DrawSelection(gr, drawbounds);
                    }
                }
            }
            this.CurrentData = null;
        }

        protected virtual void DrawBackground(System.Drawing.Graphics gr, Rectangle drawbounds)
        {
            Color backColor = this.BackColor;
            if (backColor != Resco.Controls.AdvancedComboBox.AdvancedComboBox.TransparentColor)
            {
                gr.FillRectangle(Resco.Controls.AdvancedComboBox.AdvancedComboBox.GetBrush(backColor), drawbounds);
            }
        }

        protected virtual void DrawBorder(System.Drawing.Graphics gr, Rectangle drawbounds)
        {
            Pen pen;
            BorderType border = this.Border;
            switch (border)
            {
                case BorderType.None:
                    return;

                case BorderType.Flat:
                    pen = Resco.Controls.AdvancedComboBox.AdvancedComboBox.GetPen(this.GetColor(ColorCategory.BorderFlat));
                    break;

                case BorderType.Inset:
                    pen = Resco.Controls.AdvancedComboBox.AdvancedComboBox.GetPen(this.GetColor(ColorCategory.BorderShadow));
                    break;

                case BorderType.Raised:
                    pen = Resco.Controls.AdvancedComboBox.AdvancedComboBox.GetPen(this.GetColor(ColorCategory.BorderHighlight));
                    break;

                default:
                    return;
            }
            if ((drawbounds.Height > 0) && (drawbounds.Width > 0))
            {
                gr.DrawLine(pen, drawbounds.Left, drawbounds.Top, drawbounds.Right - 1, drawbounds.Top);
                gr.DrawLine(pen, drawbounds.Left, drawbounds.Top, drawbounds.Left, drawbounds.Bottom - 1);
                if ((drawbounds.Height > 1) && (drawbounds.Width > 1))
                {
                    switch (border)
                    {
                        case BorderType.Inset:
                            pen = Resco.Controls.AdvancedComboBox.AdvancedComboBox.GetPen(this.GetColor(ColorCategory.BorderHighlight));
                            break;

                        case BorderType.Raised:
                            pen = Resco.Controls.AdvancedComboBox.AdvancedComboBox.GetPen(this.GetColor(ColorCategory.BorderShadow));
                            break;
                    }
                    gr.DrawLine(pen, drawbounds.Left, drawbounds.Bottom - 1, drawbounds.Right - 1, drawbounds.Bottom - 1);
                    gr.DrawLine(pen, drawbounds.Right - 1, drawbounds.Bottom - 1, drawbounds.Right - 1, drawbounds.Top);
                }
            }
        }

        protected virtual void DrawContent(System.Drawing.Graphics gr, Rectangle drawbounds, object data)
        {
        }

        protected virtual void DrawSelection(System.Drawing.Graphics gr, Rectangle drawbounds)
        {
            if ((drawbounds.Height > 4) && (drawbounds.Width > 4))
            {
                Rectangle rect = new Rectangle(drawbounds.X + 2, drawbounds.Y + 2, drawbounds.Width - 4, drawbounds.Height - 4);
                gr.DrawRectangle(Resco.Controls.AdvancedComboBox.AdvancedComboBox.GetPen(Color.Gray), rect);
            }
        }

        public virtual int GetAutoHeight(Resco.Controls.AdvancedComboBox.ListItem li, int index, ItemSpecificCellProperties preRscp)
        {
            ItemSpecificCellProperties properties = (preRscp == null) ? this.GetItemSpecificProperties(li) : preRscp;
            properties.CachedAutoHeight = this.m_Bounds.Height;
            return properties.CachedAutoHeight;
        }

        protected virtual Color GetColor(ColorCategory c)
        {
            Color backColor;
            switch (c)
            {
                case ColorCategory.Background:
                    backColor = this.BackColor;
                    if (backColor == Resco.Controls.AdvancedComboBox.AdvancedComboBox.TransparentColor)
                    {
                        backColor = this.m_Owner.BackColor;
                    }
                    return backColor;

                case ColorCategory.Foreground:
                    backColor = this.ForeColor;
                    if (backColor == Resco.Controls.AdvancedComboBox.AdvancedComboBox.TransparentColor)
                    {
                        backColor = this.m_Owner.ForeColor;
                    }
                    return backColor;

                case ColorCategory.BorderFlat:
                    return SystemColors.ControlDark;

                case ColorCategory.BorderHighlight:
                    return SystemColors.ControlLightLight;

                case ColorCategory.BorderShadow:
                    return SystemColors.ControlDarkDark;
            }
            return Resco.Controls.AdvancedComboBox.AdvancedComboBox.TransparentColor;
        }

        internal ItemSpecificCellProperties GetItemSpecificProperties(Resco.Controls.AdvancedComboBox.ListItem li)
        {
            ItemSpecificCellProperties properties = null;
            if (li != null)
            {
                properties = (ItemSpecificCellProperties) li.ItemSpecificCellProperties[this];
                if (properties == null)
                {
                    properties = new ItemSpecificCellProperties(this);
                    li.ItemSpecificCellProperties[this] = properties;
                }
            }
            return properties;
        }

        public bool IsVisible(Resco.Controls.AdvancedComboBox.ListItem li)
        {
            ItemSpecificCellProperties itemSpecificProperties = this.GetItemSpecificProperties(li);
            if (!itemSpecificProperties.Visible.HasValue)
            {
                return this.Visible;
            }
            return itemSpecificProperties.Visible.Value;
        }

        protected virtual void OnCellEntered(CellEnteredEventArgs e)
        {
            if (this.CellEntered != null)
            {
                this.CellEntered(this, e);
            }
        }

        internal void OnChanged(object sender, ComboBoxEventArgsType e, ComboBoxArgs args)
        {
            if (this.Changed != null)
            {
                this.Changed(this, e, args);
            }
        }

        internal void RecalculateAnchors()
        {
            if (this.Owner != null)
            {
                this.m_anchorSizeTop = this.m_Bounds.Top;
                this.m_anchorSizeLeft = this.m_Bounds.Left;
                this.m_anchorSizeRight = (this.m_Bounds.Width == -1) ? 0 : (this.Owner.Width - this.m_Bounds.Right);
                this.m_anchorSizeBottom = this.Owner.Height - this.m_Bounds.Bottom;
            }
        }

        internal virtual void Scale(float fx, float fy)
        {
            this.m_scale.Width = fx;
            this.m_scale.Height = fy;
            this.m_Bounds = new Rectangle((int) (this.m_Bounds.X * fx), (int) (this.m_Bounds.Y * fy), (this.m_Bounds.Width == -1) ? -1 : ((int) (this.m_Bounds.Width * fx)), (this.m_Bounds.Height == -1) ? -1 : ((int) (this.m_Bounds.Height * fy)));
            this.m_anchorSizeLeft = (int) (this.m_anchorSizeLeft * fx);
            this.m_anchorSizeRight = (int) (this.m_anchorSizeRight * fx);
            this.m_anchorSizeTop = (int) (this.m_anchorSizeTop * fy);
            this.m_anchorSizeBottom = (int) (this.m_anchorSizeBottom * fy);
        }

        protected virtual bool ShouldSerializeBorder()
        {
            return (this.m_Border != BorderType.None);
        }

        protected virtual bool ShouldSerializeCellSource()
        {
            if ((this.m_cellSource.SourceType == CellSourceType.ColumnIndex) && (this.m_cellSource.ColumnIndex == -1))
            {
                return false;
            }
            return true;
        }

        [DefaultValue(5)]
        public AnchorStyles Anchor
        {
            get
            {
                return this.m_anchor;
            }
            set
            {
                if (this.m_anchor != value)
                {
                    this.m_anchor = value;
                    this.OnChanged(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(this.Owner));
                }
            }
        }

        [DefaultValue(false)]
        public bool AutoHeight
        {
            get
            {
                return this.m_IsAutoHeight;
            }
            set
            {
                if (this.m_IsAutoHeight != value)
                {
                    bool flag = false;
                    this.m_IsAutoHeight = value;
                    if (this.m_Owner != null)
                    {
                        flag = this.m_Owner.RefreshProperties();
                    }
                    if (!flag)
                    {
                        this.OnChanged(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(this.Owner));
                    }
                }
            }
        }

        [DefaultValue("Transparent")]
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
                        value = Color.Transparent;
                    }
                    this.m_BackColor = value;
                    this.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                }
            }
        }

        [DefaultValue(0)]
        public virtual BorderType Border
        {
            get
            {
                return this.m_Border;
            }
            set
            {
                if (this.m_Border != value)
                {
                    this.m_Border = value;
                    this.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                }
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden), DefaultValue("-1,-1,-1,-1")]
        public Rectangle Bounds
        {
            get
            {
                return this.m_Bounds;
            }
            set
            {
                this.m_Bounds = value;
                if (this.m_Owner != null)
                {
                    if ((this.m_Bounds.X == -1) || (this.m_Bounds.Y == -1))
                    {
                        int index = this.m_Owner.CellTemplates.IndexOf(this);
                        if (index == 0)
                        {
                            if (this.m_Bounds.X == -1)
                            {
                                this.m_Bounds.X = 0;
                            }
                            if (this.m_Bounds.Y == -1)
                            {
                                this.m_Bounds.Y = 0;
                            }
                        }
                        else
                        {
                            Cell cell = this.m_Owner.CellTemplates[index - 1];
                            if (cell.Bounds.Width == -1)
                            {
                                if (this.m_Bounds.X == -1)
                                {
                                    this.m_Bounds.X = 0;
                                }
                                if (this.m_Bounds.Y == -1)
                                {
                                    this.m_Bounds.Y = cell.Bounds.Bottom;
                                }
                                if (this.m_Bounds.Height == -1)
                                {
                                    this.m_Bounds.Height = cell.Bounds.Height;
                                }
                            }
                            else
                            {
                                if (this.m_Bounds.X == -1)
                                {
                                    this.m_Bounds.X = cell.Bounds.Right;
                                }
                                if (this.m_Bounds.Y == -1)
                                {
                                    this.m_Bounds.Y = cell.Bounds.Top;
                                }
                            }
                        }
                    }
                    if (this.m_Bounds.Height == -1)
                    {
                        this.m_Bounds.Height = this.m_Owner.Height - this.m_Bounds.Y;
                    }
                }
                this.RecalculateAnchors();
                this.OnChanged(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(this.Owner));
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Content), DefaultValue("-1")]
        public Resco.Controls.AdvancedComboBox.CellSource CellSource
        {
            get
            {
                return this.m_cellSource;
            }
            set
            {
                if (this.m_cellSource != value)
                {
                    switch (value.SourceType)
                    {
                        case CellSourceType.Constant:
                            this.m_cellSource.ConstantData = value.ConstantData;
                            break;

                        case CellSourceType.ColumnIndex:
                            this.m_cellSource.ColumnIndex = value.ColumnIndex;
                            break;

                        case CellSourceType.ColumnName:
                            this.m_cellSource.ColumnName = value.ColumnName;
                            break;

                        case CellSourceType.DisplayMember:
                            this.m_cellSource.DisplayMember = value.DisplayMember;
                            break;
                    }
                    if (this.AutoHeight)
                    {
                        this.OnChanged(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(this.Owner));
                    }
                    else
                    {
                        this.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                    }
                }
            }
        }

        protected Resco.Controls.AdvancedComboBox.ListItem CurrentItem
        {
            get
            {
                if (this.m_Owner != null)
                {
                    return this.m_Owner.CurrentItem;
                }
                return null;
            }
        }

        [DefaultValue(false)]
        public bool CustomizeCell
        {
            get
            {
                return this.m_CustomizeCell;
            }
            set
            {
                this.m_CustomizeCell = value;
            }
        }

        [DefaultValue("Transparent")]
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
                        value = Color.Transparent;
                    }
                    this.m_ForeColor = value;
                    this.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                }
            }
        }

        protected internal System.Drawing.Graphics Graphics
        {
            get
            {
                if ((this.m_Owner != null) && (this.m_Owner.Parent != null))
                {
                    return this.m_Owner.Parent.BackBuffer;
                }
                return null;
            }
        }

        public object this[Resco.Controls.AdvancedComboBox.ListItem i, int index]
        {
            get
            {
                switch (this.m_cellSource.SourceType)
                {
                    case CellSourceType.Constant:
                        return this.m_cellSource.ConstantData;

                    case CellSourceType.ColumnIndex:
                    {
                        int columnIndex = this.m_cellSource.ColumnIndex;
                        if (columnIndex < 0)
                        {
                            columnIndex = index;
                        }
                        return i[columnIndex];
                    }
                    case CellSourceType.ColumnName:
                        return i[this.m_cellSource.ColumnName];

                    case CellSourceType.DisplayMember:
                    {
                        if (this.Parent == null)
                        {
                            break;
                        }
                        object obj2 = i[this.Parent.DisplayMember];
                        if (obj2 == null)
                        {
                            obj2 = i.ToString();
                        }
                        return obj2;
                    }
                }
                return null;
            }
            set
            {
                switch (this.m_cellSource.SourceType)
                {
                    case CellSourceType.ColumnIndex:
                    {
                        int columnIndex = this.m_cellSource.ColumnIndex;
                        if (columnIndex < 0)
                        {
                            columnIndex = index;
                        }
                        i[columnIndex] = value;
                        return;
                    }
                    case (CellSourceType.ColumnIndex | CellSourceType.Constant):
                        break;

                    case CellSourceType.ColumnName:
                        i[this.m_cellSource.ColumnName] = value;
                        break;

                    default:
                        return;
                }
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.Browsable(false), DefaultValue("-1,-1")]
        public Point Location
        {
            get
            {
                return new Point(this.m_Bounds.X, this.m_Bounds.Y);
            }
            set
            {
                this.Bounds = new Rectangle(value.X, value.Y, this.m_Bounds.Width, this.m_Bounds.Height);
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.Browsable(true), DefaultValue("")]
        public virtual string Name
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

        protected internal ItemTemplate Owner
        {
            get
            {
                return this.m_Owner;
            }
            set
            {
                if (this.m_Owner != value)
                {
                    this.m_Owner = value;
                    if (this.m_Owner == null)
                    {
                        this.Changed = null;
                        this.CellSource.Parent = null;
                    }
                    else
                    {
                        this.Changed = new ComboBoxEventHandler(this.m_Owner.OnChange);
                        this.CellSource.Parent = this;
                        this.RecalculateAnchors();
                    }
                }
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedComboBox.Design.Browsable(false)]
        public Resco.Controls.AdvancedComboBox.AdvancedComboBox Parent
        {
            get
            {
                if (this.m_Owner == null)
                {
                    return null;
                }
                return this.m_Owner.Parent;
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedComboBox.Design.Browsable(false)]
        internal SizeF ScaleSize
        {
            get
            {
                return this.m_scale;
            }
            set
            {
                this.m_scale = value;
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.Browsable(true), DefaultValue(false)]
        public bool Selectable
        {
            get
            {
                return this.m_bSelectable;
            }
            set
            {
                this.m_bSelectable = value;
            }
        }

        [DefaultValue("-1,-1"), Resco.Controls.AdvancedComboBox.Design.Browsable(false)]
        public System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size(this.m_Bounds.Width, this.m_Bounds.Height);
            }
            set
            {
                this.Bounds = new Rectangle(this.m_Bounds.X, this.m_Bounds.Y, value.Width, value.Height);
            }
        }

        [DefaultValue(true), Resco.Controls.AdvancedComboBox.Design.Browsable(true)]
        public bool Visible
        {
            get
            {
                return this.m_bVisible;
            }
            set
            {
                this.m_bVisible = value;
                this.OnChanged(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
            }
        }
    }
}

