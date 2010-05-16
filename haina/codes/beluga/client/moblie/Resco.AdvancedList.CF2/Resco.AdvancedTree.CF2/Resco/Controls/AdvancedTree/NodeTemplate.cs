namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class NodeTemplate
    {
        internal bool AutoHeight;
        protected internal Node CurrentNode;
        internal bool HasCustomizedCells;
        private Color m_BackColor;
        private Rectangle m_bgRect = new Rectangle();
        private CellCollection m_CellTemplates;
        private Color m_ForeColor;
        private GradientColor m_gradientBackColor;
        private int m_iHeight;
        private int m_iWidth;
        private Resco.Controls.AdvancedTree.AdvancedTree m_Parent;
        private string m_sName;
        private bool m_useGradient;
        private bool m_useTemplateBackground;
        private static NodeTemplate s_Default;

        internal event TreeChangedEventHandler Changed;

        public NodeTemplate()
        {
            this.m_CellTemplates = new CellCollection(this);
            this.m_BackColor = SystemColors.ControlLightLight;
            this.m_ForeColor = SystemColors.ControlText;
            this.m_gradientBackColor = new GradientColor();
            this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
            this.m_useGradient = false;
            this.m_useTemplateBackground = false;
            this.m_iHeight = 0x10;
            this.m_sName = "";
        }

        public virtual NodeTemplate Clone()
        {
            NodeTemplate template = new NodeTemplate();
            foreach (Cell cell in this.m_CellTemplates)
            {
                template.CellTemplates.Add(cell.Clone());
            }
            template.BackColor = this.BackColor;
            template.ForeColor = this.ForeColor;
            template.GradientBackColor = new GradientColor(this.GradientBackColor.StartColor, this.GradientBackColor.EndColor, this.GradientBackColor.FillDirection);
            template.UseGradient = this.UseGradient;
            template.UseTemplateBackground = this.UseTemplateBackground;
            template.Height = this.Height;
            template.Name = this.Name;
            return template;
        }

        internal bool CustomizeCells(Node node)
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
                    object data = cell[node, i];
                    CustomizeCellEventArgs e = new CustomizeCellEventArgs(cell, data, node);
                    this.OnCustomizeCell(e);
                    NodeSpecificCellProperties nodeSpecificProperties = cell.GetNodeSpecificProperties(node);
                    bool flag2 = nodeSpecificProperties.Visible.HasValue ? nodeSpecificProperties.Visible.Value : cell.Visible;
                    nodeSpecificProperties.Visible = new bool?(e.Cell.Visible);
                    nodeSpecificProperties.CachedAutoHeight = e.Cell.GetAutoHeight(e.Node, i, nodeSpecificProperties);
                    nodeSpecificProperties.Cea = e;
                    if (e.Cell.Visible != flag2)
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }

        internal static void DisposeDefaultNodeTemplate()
        {
            s_Default = null;
        }

        protected internal virtual void Draw(Graphics gr, int xOffset, int yOffset, Node node, int width, int height)
        {
            bool rightToLeft;
            int num;
            Color colorKey;
            if (this.Parent != null)
            {
                rightToLeft = this.Parent.RightToLeft;
                num = this.Parent.ScrollbarVisible ? this.Parent.ScrollbarWidth : 0;
            }
            else
            {
                rightToLeft = false;
                num = 0;
            }
            this.CurrentNode = node;
            if (height == -1)
            {
                height = this.GetHeight(node);
            }
            if (this.m_BackColor == Color.Transparent)
            {
                if (this.Parent.UseGradient)
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
            int x = !rightToLeft ? xOffset : ((width + num) - (xOffset + this.Parent.PlusMinusSize.Width));
            if (((this.Parent != null) && (!this.Parent.UseGradient || (this.Parent.GradientBackColor.FillDirection == FillDirection.Vertical))) && !this.m_useTemplateBackground)
            {
                Brush brush = Resco.Controls.AdvancedTree.AdvancedTree.GetBrush(colorKey);
                if (!rightToLeft)
                {
                    gr.FillRectangle(brush, 0, yOffset, xOffset + this.Parent.PlusMinusSize.Width, height);
                }
                else
                {
                    gr.FillRectangle(brush, x, yOffset, xOffset + this.Parent.PlusMinusSize.Width, height);
                }
            }
            else if (this.m_useTemplateBackground)
            {
                this.DrawBackground(gr, 0, yOffset, width, height, colorKey);
            }
            if (((this.Parent != null) && this.Parent.ShowPlusMinus) && !node.HidePlusMinus)
            {
                int num3 = this.Parent.PlusMinusSize.Width - (2 * this.Parent.PlusMinusMargin.Width);
                int num4 = this.Parent.PlusMinusSize.Height - (2 * this.Parent.PlusMinusMargin.Height);
                int num5 = num3 / 2;
                int num6 = num4 / 2;
                gr.DrawRectangle(Resco.Controls.AdvancedTree.AdvancedTree.GetPen(Color.Gray), (int) (x + this.Parent.PlusMinusMargin.Width), (int) (this.Parent.PlusMinusMargin.Height + yOffset), (int) (num3 - 1), (int) (num4 - 1));
                int num7 = (this.Parent.PlusMinusMargin.Width + x) + num5;
                int num8 = (this.Parent.PlusMinusMargin.Height + yOffset) + num6;
                gr.DrawLine(Resco.Controls.AdvancedTree.AdvancedTree.GetPen(Color.Black), num7 - (num5 / 2), num8, num7 + (num5 / 2), num8);
                if (!node.IsExpanded)
                {
                    gr.DrawLine(Resco.Controls.AdvancedTree.AdvancedTree.GetPen(Color.Black), num7, num8 - (num6 / 2), num7, num8 + (num6 / 2));
                }
            }
            if ((this.Parent != null) && this.Parent.TreeNodeLines)
            {
                int num9 = this.Parent.PlusMinusSize.Width - (2 * this.Parent.PlusMinusMargin.Width);
                int num10 = this.Parent.PlusMinusSize.Height - (2 * this.Parent.PlusMinusMargin.Height);
                int num11 = num9 / 2;
                int num12 = num10 / 2;
                int num13 = (this.Parent.PlusMinusMargin.Width + x) + num11;
                int num14 = (this.Parent.PlusMinusMargin.Height + yOffset) + num12;
                bool flag2 = node.ParentCollection.IndexOf(node) == (node.ParentCollection.Count - 1);
                if (node.HidePlusMinus)
                {
                    gr.DrawLine(Resco.Controls.AdvancedTree.AdvancedTree.GetPen(Color.Gray), num13, num14, x + (this.Parent.PlusMinusSize.Width * (!rightToLeft ? 1 : -1)), num14);
                    gr.DrawLine(Resco.Controls.AdvancedTree.AdvancedTree.GetPen(Color.Gray), num13, yOffset, num13, flag2 ? num14 : (yOffset + this.Height));
                }
                else
                {
                    gr.DrawLine(Resco.Controls.AdvancedTree.AdvancedTree.GetPen(Color.Gray), x + (!rightToLeft ? (this.Parent.PlusMinusSize.Width - this.Parent.PlusMinusMargin.Width) : 0), num14, x + (!rightToLeft ? this.Parent.PlusMinusSize.Width : this.Parent.PlusMinusMargin.Width), num14);
                    gr.DrawLine(Resco.Controls.AdvancedTree.AdvancedTree.GetPen(Color.Gray), num13, yOffset, num13, yOffset + this.Parent.PlusMinusMargin.Height);
                    if (!flag2)
                    {
                        gr.DrawLine(Resco.Controls.AdvancedTree.AdvancedTree.GetPen(Color.Gray), num13, (yOffset + this.Parent.PlusMinusSize.Height) - this.Parent.PlusMinusMargin.Height, num13, yOffset + this.Height);
                    }
                }
                int num15 = x + ((!rightToLeft ? -1 : 1) * this.Parent.PlusMinusSize.Width);
                for (Node node2 = node.Parent; node2 != null; node2 = node2.Parent)
                {
                    if (node2.ParentCollection.IndexOf(node2) != (node2.ParentCollection.Count - 1))
                    {
                        num13 = (num15 + this.Parent.PlusMinusMargin.Width) + ((this.Parent.PlusMinusSize.Width - (2 * this.Parent.PlusMinusMargin.Width)) / 2);
                        gr.DrawLine(Resco.Controls.AdvancedTree.AdvancedTree.GetPen(Color.Gray), num13, yOffset, num13, yOffset + this.Height);
                    }
                    num15 += (!rightToLeft ? -1 : 1) * this.Parent.PlusMinusSize.Width;
                }
            }
            if (this.Parent != null)
            {
                xOffset += this.Parent.PlusMinusSize.Width;
            }
            x = !rightToLeft ? xOffset : (num - xOffset);
            if (!this.m_useTemplateBackground)
            {
                this.DrawBackground(gr, x, yOffset, width, height, colorKey);
            }
            int count = this.m_CellTemplates.Count;
            for (int i = 0; i < count; i++)
            {
                Cell cell = this.m_CellTemplates[i];
                NodeSpecificCellProperties nscp = null;
                object data = null;
                if (cell.CustomizeCell && (this.Parent != null))
                {
                    nscp = cell.GetNodeSpecificProperties(node);
                    if (nscp.Cea != null)
                    {
                        cell = nscp.Cea.Cell;
                        data = nscp.Cea.Data;
                    }
                }
                else
                {
                    data = cell[node, i];
                }
                if (cell is ButtonCell)
                {
                    ((ButtonCell) cell).Pressed = node.PressedButtonIndex == i;
                }
                if ((cell != null) && cell.Visible)
                {
                    cell.Draw(gr, xOffset, yOffset, width, data, nscp);
                }
                if (nscp != null)
                {
                    nscp.Cea = null;
                }
            }
        }

        private void DrawBackground(Graphics gr, int x, int y, int width, int height, Color backColor)
        {
            if (!this.m_useGradient)
            {
                if (!this.Parent.UseGradient || (this.m_BackColor != Color.Transparent))
                {
                    gr.FillRectangle(Resco.Controls.AdvancedTree.AdvancedTree.GetBrush(backColor), x, y, width, height);
                }
            }
            else
            {
                this.m_bgRect.X = x;
                this.m_bgRect.Y = y;
                this.m_bgRect.Width = width;
                this.m_bgRect.Height = height;
                this.m_gradientBackColor.DrawGradient(gr, this.m_bgRect);
            }
        }

        internal int GetCellClick(int pos_x, int pos_y, Node node)
        {
            int num = this.m_CellTemplates.Count - 1;
            while (num >= 0)
            {
                NodeSpecificCellProperties nodeSpecificProperties = this.m_CellTemplates[num].GetNodeSpecificProperties(node);
                if ((nodeSpecificProperties.Visible.Value && nodeSpecificProperties.CachedBounds.HasValue) && nodeSpecificProperties.CachedBounds.Value.Contains(pos_x, pos_y))
                {
                    return num;
                }
                num--;
            }
            return num;
        }

        public int GetHeight(Node node)
        {
            if (node.RecalculationNeeded)
            {
                node.RecalculationNeeded = false;
                if (!this.AutoHeight)
                {
                    node.ActualHeight = this.m_iHeight;
                    return this.m_iHeight;
                }
                int iHeight = this.m_iHeight;
                for (int i = 0; i < this.CellTemplates.Count; i++)
                {
                    Cell cell = this.CellTemplates[i];
                    if (cell.AutoHeight)
                    {
                        int num3 = ((cell.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom) ? cell.m_anchorSizeBottom : 0;
                        iHeight = Math.Max(iHeight, (cell.GetAutoHeight(node, i, null) + cell.Bounds.Top) + num3);
                    }
                }
                node.ActualHeight = iHeight;
            }
            return node.ActualHeight;
        }

        private void m_gradientBackColor_PropertyChanged(object sender, EventArgs e)
        {
            this.OnChanged(this, TreeRepaintEventArgs.Empty);
        }

        internal virtual void OnChanged(object sender, TreeChangedEventArgs e)
        {
            if (this.Changed != null)
            {
                this.Changed(sender, e);
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
            this.OnChanged(this, new TreeRefreshEventArgs(this));
            return true;
        }

        public void Scale(float fx, float fy)
        {
            this.Height = (int) (fy * this.Height);
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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

        public CellCollection CellTemplates
        {
            get
            {
                return this.m_CellTemplates;
            }
        }

        public static NodeTemplate Default
        {
            get
            {
                if (s_Default == null)
                {
                    s_Default = new NodeTemplate();
                }
                return s_Default;
            }
        }

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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
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
                this.OnChanged(this, TreeRepaintEventArgs.Empty);
            }
        }

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
                    this.OnChanged(this, new TreeRefreshEventArgs(this));
                }
            }
        }

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

        public Resco.Controls.AdvancedTree.AdvancedTree Parent
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
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

        public bool UseTemplateBackground
        {
            get
            {
                return this.m_useTemplateBackground;
            }
            set
            {
                if (this.m_useTemplateBackground != value)
                {
                    this.m_useTemplateBackground = value;
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

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
    }
}

