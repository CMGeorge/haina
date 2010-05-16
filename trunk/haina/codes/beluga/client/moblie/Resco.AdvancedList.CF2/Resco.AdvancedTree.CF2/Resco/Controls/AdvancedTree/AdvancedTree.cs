namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    public class AdvancedTree : UserControl
    {
        private DesignTimeCallback _designTimeCallback;
        private Rectangle backRect = new Rectangle(0, 0, 100, 100);
        private bool bIsInScrollChange;
        private Node m_activeNode;
        private List<Rectangle> m_alButtons = new List<Rectangle>();
        private List<Rectangle> m_alLinks = new List<Rectangle>();
        private List<TooltipArea> m_alTooltips = new List<TooltipArea>();
        private SolidBrush m_BackColor;
        private bool m_bDelayLoad;
        private bool m_bDrawGrid = true;
        private bool m_bEnableTouchScrolling;
        private bool m_bGradientChanged;
        private bool m_bIsChange = true;
        private bool m_bKeyNavigation;
        private bool m_bMultiSelect;
        private SolidBrush m_brushKey;
        private bool m_bShowFooter;
        private bool m_bShowHeader;
        private bool m_bShowingToolTip;
        private bool m_bShowPlusMinus = true;
        private bool m_bShowScrollbar = true;
        private bool m_bTouchScrolling;
        private bool m_bTreeNodeLines = false;
        internal Color m_colorKey;
        private System.Windows.Forms.ContextMenu m_ContextMenu;
        private Conversion m_conversion;
        internal int m_countSelected;
        private GradientColor m_gradientBackColor;
        private int m_iActualHeight;
        private int m_iActualNode;
        private int m_iActualNodeOffset;
        private int m_iLevelIndentation = -1;
        private ImageAttributes m_imgAttr;
        private int m_iNoRedraw;
        private int m_iScrollChange;
        private int m_iScrollWidth;
        private int m_iUpdate = 1;
        private int m_iVScrollPrevValue;
        private Resco.Controls.AdvancedTree.SelectionMode m_mode;
        private Point m_MousePosition = new Point(0, 0);
        private NodeCollection m_ncNodes = new NodeCollection(null);
        internal Node m_nodeSelected;
        internal Pen m_penBorder = new Pen(Color.DarkGray);
        private Node m_pressedButtonNode;
        private Resco.Controls.AdvancedTree.Header m_rFooter;
        private Resco.Controls.AdvancedTree.Header m_rHeader;
        private bool m_rightToLeft;
        private float m_scaleFactorY;
        private Size m_szPlusMinusMargin = new Size(3, 3);
        private Size m_szPlusMinusSize = new Size(15, 15);
        private System.Windows.Forms.Timer m_Timer;
        private Resco.Controls.AdvancedTree.ToolTip m_ToolTip;
        private int m_TouchAutoScrollDiff;
        private System.Windows.Forms.Timer m_TouchScrollingTimer;
        private int m_touchSensitivity;
        private uint m_TouchTime0;
        private uint m_TouchTime1;
        private TemplateSet m_tsCurrent;
        private bool m_useGradient;
        private ScrollbarWrapper m_vScroll;
        private Control m_vScrollBarResco;
        private bool m_vScrollVisible;
        private int m_vScrollWidth;
        internal static Point point1;
        internal static Point point2;
        internal static Point point3;
        private static Hashtable sBrushes = new Hashtable();
        private static int ScrollBottomOffset = -1;
        private static int ScrollSmallChange = 0x10;
        private static Hashtable sPens = new Hashtable();
        private static Bitmap sPixel = null;
        internal const PlatformID SYMBIAN_OS = ((PlatformID) 0xc0);
        public static int TooltipWidth = 8;
        internal static Color TransparentColor = Color.Transparent;

        public event EventHandler ActiveNodeChanged;

        public event NodeEventHandler AfterCollapse;

        public event NodeEventHandler AfterExpand;

        public event NodeEventHandler AfterNodeSelect;

        public event NodeCancelEventHandler BeforeCollapse;

        public event NodeCancelEventHandler BeforeExpand;

        public event ButtonEventHandler ButtonClick;

        public event CellEventHandler CellClick;

        public event CustomizeCellEventHandler CustomizeCell;

        public event CellEventHandler FooterClick;

        public event CellEventHandler HeaderClick;

        public event LinkEventHandler LinkClick;

        public event NodeAddingEventHandler NodeAdding;

        public event NodeEnteredEventHandler NodeEntered;

        public event EventHandler Scroll;

        static AdvancedTree()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(Resco.Controls.AdvancedTree.AdvancedTree), "");
            //}
        }

        public AdvancedTree()
        {
            this.m_ncNodes.SetAdvancedTree(this);
            this.m_ncNodes.ManualDataLoading = false;
            this.m_tsCurrent = new TemplateSet();
            this.m_tsCurrent.Parent = this;
            this.m_tsCurrent.Changed += new TreeChangedEventHandler(this.OnChanged);
            BackBufferManager.AddRef();
            this.m_colorKey = Color.FromArgb(0xff, 0, 0xff);
            this.m_brushKey = new SolidBrush(this.m_colorKey);
            this.m_imgAttr = new ImageAttributes();
            this.m_imgAttr.SetColorKey(this.m_colorKey, this.m_colorKey);
            base.BackColor = SystemColors.ControlLightLight;
            this.m_BackColor = new SolidBrush(this.BackColor);
            this.m_vScrollWidth = 0;
            this.m_iScrollWidth = 13;
            this.m_iActualNode = 0;
            this.m_iActualNodeOffset = 0;
            this.m_iActualHeight = 0;
            this.m_iVScrollPrevValue = 0;
            using (Graphics graphics = base.CreateGraphics())
            {
                point1 = new Point(0, 0);
                point2 = new Point(0, -((int) (TooltipWidth * (graphics.DpiY / 96f))));
                point3 = new Point(-((int) (TooltipWidth * (graphics.DpiX / 96f))), 0);
                this.m_ToolTip = null;
                this.m_scaleFactorY = graphics.DpiY / 96f;
            }
            this.m_Timer = new System.Windows.Forms.Timer();
            this.m_Timer.Enabled = false;
            this.m_Timer.Interval = 500;
            this.m_Timer.Tick += new EventHandler(this.OnTimerTick);
            this.m_bShowingToolTip = false;
            this.m_TouchScrollingTimer = new System.Windows.Forms.Timer();
            this.m_TouchScrollingTimer.Enabled = false;
            this.m_TouchScrollingTimer.Interval = 50;
            this.m_TouchScrollingTimer.Tick += new EventHandler(this.OnTouchScrollingTimerTick);
            this.m_bTouchScrolling = false;
            this.m_bEnableTouchScrolling = false;
            this.m_TouchAutoScrollDiff = 0;
            this.m_touchSensitivity = 8;
            this.m_rHeader = new Resco.Controls.AdvancedTree.Header();
            this.m_rHeader.SetParentCollection(this.m_ncNodes);
            this.m_rFooter = new Resco.Controls.AdvancedTree.Header();
            this.m_rFooter.SetParentCollection(this.m_ncNodes);
            this.m_gradientBackColor = new GradientColor();
            this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
            this.m_useGradient = false;
            NodeTemplate.Default.Parent = this;
        }

        internal void AddButtonArea(Rectangle bounds)
        {
            this.m_alButtons.Add(bounds);
        }

        internal void AddLinkArea(Rectangle bounds)
        {
            this.m_alLinks.Add(bounds);
        }

        private bool AddNodeManually(NodeCollection ncNodes)
        {
            int count = ncNodes.Count;
            NodeAddingEventArgs e = new NodeAddingEventArgs(ncNodes.Parent, count);
            this.OnNodeAdding(e);
            if ((e.Node != null) && !e.Cancel)
            {
                ncNodes.Add(e.Node);
                while (!this.AddNodeManually(e.Node.Nodes))
                {
                }
            }
            return e.Cancel;
        }

        private bool AddNodeManuallyDelay(NodeCollection ncNodes)
        {
            int count = ncNodes.Count;
            NodeAddingEventArgs e = new NodeAddingEventArgs(ncNodes.Parent, count);
            this.OnNodeAdding(e);
            if ((e.Node != null) && !e.Cancel)
            {
                ncNodes.Add(e.Node);
            }
            return e.Cancel;
        }

        public void AddTooltipArea(Rectangle bounds, string text)
        {
            this.m_alTooltips.Add(new TooltipArea(bounds, text));
        }

        [Obsolete("This method is obsolete, use EndUpdate instead.")]
        public void BeginInit()
        {
            this.BeginUpdate();
        }

        public void BeginUpdate()
        {
            this.m_iUpdate++;
        }

        private void CalculateFirstNode(int iOffset)
        {
            int gridLinesWidth = this.GridLinesWidth;
            this.m_iActualNodeOffset += iOffset;
            if (iOffset <= 0)
            {
                for (int i = this.m_iActualNode; i < this.m_ncNodes.Count; i++)
                {
                    int num2 = this.m_ncNodes.GetHeight(i, this.m_tsCurrent, true);
                    if (Math.Abs(this.m_iActualNodeOffset) < num2)
                    {
                        this.m_iActualNode = i;
                        return;
                    }
                    this.m_iActualNodeOffset += num2;
                }
            }
            else
            {
                for (int j = this.m_iActualNode; j >= 0; j--)
                {
                    if (this.m_iActualNodeOffset > 0)
                    {
                        if (j == 0)
                        {
                            this.EnsureVisible(this.m_ncNodes, 0);
                            return;
                        }
                        int num4 = this.m_ncNodes.GetHeight(j - 1, this.m_tsCurrent, true);
                        this.m_iActualNodeOffset -= num4;
                    }
                    else
                    {
                        int num5 = this.m_ncNodes.GetHeight(j, this.m_tsCurrent, true);
                        if (Math.Abs(this.m_iActualNodeOffset) < num5)
                        {
                            this.m_iActualNode = j;
                            return;
                        }
                    }
                }
            }
        }

        private bool CheckForButton(Point p)
        {
            using (List<Rectangle>.Enumerator enumerator = this.m_alButtons.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Contains(p.X, p.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckForLink(Point p)
        {
            using (List<Rectangle>.Enumerator enumerator = this.m_alLinks.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Contains(p.X, p.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private string CheckForTooltip(Point p)
        {
            foreach (TooltipArea area in this.m_alTooltips)
            {
                if (area.Bounds.Contains(p.X, p.Y))
                {
                    return area.Text;
                }
            }
            return null;
        }

        public void CollapseAll()
        {
            this.SuspendRedraw();
            foreach (Node node in this.m_ncNodes)
            {
                node.CollapseAll();
            }
            this.ResumeRedraw();
        }

        private void CollapseNavigationNode()
        {
            if (this.m_activeNode != null)
            {
                if (this.m_activeNode.IsExpanded)
                {
                    this.m_activeNode.Collapse();
                }
                else
                {
                    Node parent = this.m_activeNode.ParentCollection.Parent;
                    if (parent != null)
                    {
                        this.SelectedNode = parent;
                    }
                    else if (this.m_ncNodes.Count > 0)
                    {
                        this.SelectedNode = this.m_ncNodes[0];
                    }
                }
            }
        }

        private int CustomizeHeaderFooter(NodeTemplate t, Node n, ref bool bResetScrollbar)
        {
            int num2 = -1;
            if (t.CustomizeCells(n))
            {
                num2 = t.GetHeight(n);
                n.ResetCachedBounds(null);
            }
            int height = t.GetHeight(n);
            if ((num2 >= 0) && (height != num2))
            {
                bResetScrollbar = true;
            }
            return height;
        }

        protected override void Dispose(bool disposing)
        {
            if (this.m_TouchScrollingTimer != null)
            {
                this.m_TouchScrollingTimer.Enabled = false;
                this.m_TouchScrollingTimer.Dispose();
            }
            this.m_TouchScrollingTimer = null;
            BackBufferManager.Release();
            if (this.m_ncNodes != null)
            {
                this.m_ncNodes.SetAdvancedTree(null);
            }
            this.m_ncNodes = null;
            if (this.m_tsCurrent != null)
            {
                this.m_tsCurrent.Changed -= new TreeChangedEventHandler(this.OnChanged);
                this.m_tsCurrent.Parent = null;
            }
            this.m_tsCurrent = null;
            if (this.m_alLinks != null)
            {
                this.m_alLinks.Clear();
            }
            this.m_alLinks = null;
            if (this.m_alTooltips != null)
            {
                this.m_alTooltips.Clear();
            }
            this.m_alTooltips = null;
            if (this.m_alButtons != null)
            {
                this.m_alButtons.Clear();
            }
            this.m_alButtons = null;
            Utility.Dispose();
            Resco.Controls.AdvancedTree.Mapping.DisposeEmptyMapping();
            NodeTemplate.DisposeDefaultNodeTemplate();
            ImageCache.GlobalCache.Clear();
            if (sBrushes != null)
            {
                sBrushes.Clear();
            }
            sBrushes = null;
            if (sPens != null)
            {
                sPens.Clear();
            }
            sPens = null;
            if (sPixel != null)
            {
                sPixel.Dispose();
            }
            sPixel = null;
            if (this.m_rHeader != null)
            {
                this.m_rHeader.SetParentCollection(null);
            }
            this.m_rHeader = null;
            if (this.m_rFooter != null)
            {
                this.m_rFooter.SetParentCollection(null);
            }
            this.m_rFooter = null;
            if (this.m_gradientBackColor != null)
            {
                this.m_gradientBackColor.PropertyChanged -= new EventHandler(this.m_gradientBackColor_PropertyChanged);
            }
            this.m_gradientBackColor = null;
            base.Dispose(disposing);
        }

        internal void DoDelayLoad()
        {
            if (this.m_ncNodes.ManualDataLoading)
            {
                Cursor.Current = Cursors.WaitCursor;
                this.BeginUpdate();
                int height = base.ClientRectangle.Height;
                int iActualNode = this.m_iActualNode;
                if (this.DoDelayLoadRecursive(this.m_ncNodes, iActualNode, height) > 0)
                {
                    this.m_ncNodes.ManualDataLoading = false;
                }
                this.EndUpdate();
                Cursor.Current = Cursors.Default;
            }
        }

        internal int DoDelayLoadRecursive(NodeCollection col, int startIndex, int h)
        {
            if (col.ManualDataLoading)
            {
                while (h > 0)
                {
                    if (col.Count <= startIndex)
                    {
                        col.ManualDataLoading = !this.AddNodeManuallyDelay(col);
                        if (col.ManualDataLoading && (col.Count > startIndex))
                        {
                            h -= col[startIndex].GetHeight(this.Templates);
                            if (this.GridLines)
                            {
                                h--;
                            }
                        }
                    }
                    if (!col.ManualDataLoading || (col.Count <= startIndex))
                    {
                        return h;
                    }
                    if (col[startIndex].IsExpanded)
                    {
                        h = this.DoDelayLoadRecursive(col[startIndex].Nodes, 0, h);
                    }
                    startIndex++;
                }
                return h;
            }
            return h;
        }

        internal static void DrawPixel(Graphics gr, Color c, int x, int y)
        {
            if (sPixel == null)
            {
                sPixel = new Bitmap(1, 1);
            }
            sPixel.SetPixel(0, 0, c);
            gr.DrawImage(sPixel, x, y);
        }

        [Obsolete("This method is obsolete, use EndUpdate instead.")]
        public void EndInit()
        {
            this.EndUpdate();
        }

        public bool EndUpdate()
        {
            if (this.m_iUpdate > 0)
            {
                this.m_iUpdate--;
            }
            else
            {
                return false;
            }
            if (this.m_iUpdate == 0)
            {
                this.OnChanged(this, new TreeRefreshEventArgs(true));
            }
            return true;
        }

        public int EnsureVisible(Node node)
        {
            return this.EnsureVisible(node, false);
        }

        public int EnsureVisible(Node node, bool bTop)
        {
            for (Node node2 = node; node2 != null; node2 = node2.ParentCollection.Parent)
            {
                if (node2.ParentCollection == null)
                {
                    break;
                }
                if (node2.ParentCollection.Parent != null)
                {
                    node2.ParentCollection.Parent.Expand();
                }
            }
            int headerHeight = this.HeaderHeight;
            if (!bTop && this.IsVisible(node))
            {
                bool bContinue = true;
                return this.GetVisibleOffset(this.m_iActualNodeOffset, this.m_ncNodes, this.m_iActualNode, node, out bContinue);
            }
            int offset = 0;
            if (!bTop && !this.IsOverTop(node))
            {
                offset = ((base.Height - this.HeaderHeight) - this.FooterHeight) - node.GetHeight(this.Templates);
                if (offset < 0)
                {
                    offset = 0;
                }
            }
            int index = node.ParentCollection.IndexOf(node);
            return this.SetScrollPos(node.ParentCollection, index, offset);
        }

        public int EnsureVisible(NodeCollection coll, int ix)
        {
            return this.EnsureVisible(coll, ix, false);
        }

        public int EnsureVisible(NodeCollection coll, int ix, bool bTop)
        {
            if ((ix >= 0) && (coll.Count != 0))
            {
                return this.EnsureVisible(coll[ix], bTop);
            }
            return 0;
        }

        public void ExpandAll()
        {
            this.SuspendRedraw();
            foreach (Node node in this.m_ncNodes)
            {
                node.ExpandAll();
            }
            this.ResumeRedraw();
        }

        private void ExpandNavigationNode()
        {
            if (this.m_activeNode != null)
            {
                if (this.m_activeNode.IsExpanded)
                {
                    if (this.m_activeNode.Nodes.Count > 0)
                    {
                        this.SelectedNode = this.m_activeNode.Nodes[0];
                    }
                }
                else
                {
                    this.m_activeNode.Expand();
                }
            }
        }

        private int FindVisible(NodeCollection coll, int y, int nIndex, int max)
        {
            coll.LastDrawnNode = nIndex;
            while (coll.LastDrawnNode < coll.Count)
            {
                Node node = coll[coll.LastDrawnNode];
                int height = node.GetTemplate(this.m_tsCurrent).GetHeight(node);
                y += height;
                if (this.m_bDrawGrid)
                {
                    y++;
                }
                if (node.IsExpanded)
                {
                    y += this.FindVisible(node.Nodes, y, 0, max);
                }
                if (y > max)
                {
                    return y;
                }
                coll.LastDrawnNode++;
            }
            return y;
        }

        public static SolidBrush GetBrush(Color c)
        {
            if (sBrushes == null)
            {
                sBrushes = new Hashtable();
            }
            SolidBrush brush = sBrushes[c] as SolidBrush;
            if (brush == null)
            {
                brush = new SolidBrush(c);
                sBrushes[c] = brush;
            }
            return brush;
        }

        public CellEventArgs GetCellAtPoint(Point pt)
        {
            int num;
            Node rHeader = null;
            if (pt.Y < this.HeaderHeight)
            {
                rHeader = this.m_rHeader;
                NodeTemplate template = rHeader.GetTemplate(this.m_tsCurrent);
                int ri = -1;
                int ci = template.GetCellClick(pt.X, pt.Y, rHeader);
                return new CellEventArgs(rHeader, (ci >= 0) ? template[ci] : null, ri, ci, 0);
            }
            if (pt.Y > (num = base.Height - this.FooterHeight))
            {
                rHeader = this.m_rFooter;
                NodeTemplate template2 = rHeader.GetTemplate(this.m_tsCurrent);
                int num4 = -2;
                int num5 = template2.GetCellClick(pt.X, pt.Y - num, rHeader);
                return new CellEventArgs(rHeader, (num5 >= 0) ? template2[num5] : null, num4, num5, num);
            }
            int iNodeOffset = this.m_iActualNodeOffset + this.HeaderHeight;
            Point point = this.m_ncNodes.GetNodeClick(this.m_iActualNode, iNodeOffset, pt.X, pt.Y, out num, out iNodeOffset, out rHeader);
            if (rHeader != null)
            {
                return new CellEventArgs(rHeader, (point.Y >= 0) ? rHeader.GetTemplate(this.m_tsCurrent)[point.Y] : null, point.X, point.Y, num);
            }
            return null;
        }

        public Rectangle GetCellBounds(Node node, int cellIndex)
        {
            int nodeOffset = this.GetNodeOffset(node);
            if (((nodeOffset < 0) || (nodeOffset == 0x7fffffff)) || (nodeOffset > (base.Height - this.FooterHeight)))
            {
                return Rectangle.Empty;
            }
            NodeTemplate template = node.GetTemplate(this.m_tsCurrent);
            int height = template.GetHeight(node);
            if ((nodeOffset + height) < 0)
            {
                return Rectangle.Empty;
            }
            if (cellIndex < 0)
            {
                return new Rectangle((node.Level * this.NodeIndent) + this.PlusMinusSize.Width, nodeOffset, base.Width - this.m_vScrollWidth, template.Height);
            }
            if (cellIndex >= template.CellTemplates.Count)
            {
                return Rectangle.Empty;
            }
            Cell cell = template.CellTemplates[cellIndex];
            return cell.CalculateBounds((node.Level * this.NodeIndent) + this.PlusMinusSize.Width, nodeOffset, node, base.Width - this.m_vScrollWidth, null);
        }

        public Node GetNextNode(Node node)
        {
            if ((node != null) && (node.ParentCollection != null))
            {
                if (node.IsExpanded && (node.Nodes.Count > 0))
                {
                    return node.Nodes[0];
                }
                Node node2 = node;
                while (node != null)
                {
                    int num = node.ParentCollection.IndexOf(node) + 1;
                    if (num < node.ParentCollection.Count)
                    {
                        return node.ParentCollection[num];
                    }
                    node = node.ParentCollection.Parent;
                }
                return node2;
            }
            if (this.m_ncNodes.Count <= 0)
            {
                return null;
            }
            return this.m_ncNodes[0];
        }

        public int GetNodeCount(bool includeSubTrees)
        {
            int count = this.m_ncNodes.Count;
            if (includeSubTrees)
            {
                foreach (Node node in this.m_ncNodes)
                {
                    count += node.GetNodeCount(true);
                }
            }
            return count;
        }

        private int GetNodeOffset(Node node)
        {
            if (node == null)
            {
                return -2147483648;
            }
            if (this.GetTopMostParentIndex(node) < this.m_iActualNode)
            {
                return -2147483648;
            }
            int num2 = this.m_iActualNodeOffset + this.HeaderHeight;
            int num3 = base.Height - this.FooterHeight;
            for (Node node2 = this.m_ncNodes[this.m_iActualNode]; node2 != null; node2 = this.GetNextNode(node2))
            {
                if (node2 == node)
                {
                    return num2;
                }
                int height = node2.GetTemplate(this.m_tsCurrent).GetHeight(node2);
                num2 += height;
                if (this.GridLines)
                {
                    num2++;
                }
                if (num2 > num3)
                {
                    return 0x7fffffff;
                }
            }
            return num2;
        }

        public static Pen GetPen(Color c)
        {
            if (sPens == null)
            {
                sPens = new Hashtable();
            }
            Pen pen = sPens[c] as Pen;
            if (pen == null)
            {
                pen = new Pen(c);
                sPens[c] = pen;
            }
            return pen;
        }

        public Node GetPrevNode(Node node)
        {
            if ((node != null) && (node.ParentCollection != null))
            {
                if ((this.m_ncNodes.Count <= 0) || (node != this.m_ncNodes[0]))
                {
                    int num = node.ParentCollection.IndexOf(node) - 1;
                    if (num < 0)
                    {
                        return node.ParentCollection.Parent;
                    }
                    node = node.ParentCollection[num];
                    while (node.IsExpanded)
                    {
                        if (node.Nodes.Count <= 0)
                        {
                            return node;
                        }
                        node = node.Nodes[node.Nodes.Count - 1];
                    }
                }
                return node;
            }
            if (this.m_ncNodes.Count <= 0)
            {
                return null;
            }
            return this.m_ncNodes[this.m_ncNodes.Count - 1];
        }

        private Node GetSelectedNode(NodeCollection col)
        {
            foreach (Node node in col)
            {
                if (node.Selected)
                {
                    return node;
                }
                Node selectedNode = this.GetSelectedNode(node.Nodes);
                if (selectedNode != null)
                {
                    return selectedNode;
                }
            }
            return null;
        }

        private int GetSelectedNodes(Node[] nodes, NodeCollection col, int i, int c)
        {
            foreach (Node node in col)
            {
                if (node.Selected)
                {
                    nodes[i++] = node;
                }
                if (i >= c)
                {
                    return i;
                }
                i = this.GetSelectedNodes(nodes, node.Nodes, i, c);
                if (i >= c)
                {
                    return i;
                }
            }
            return i;
        }

        private uint GetTickCount()
        {
            return (uint) Environment.TickCount;
        }

        private int GetTopMostParentIndex(Node node)
        {
            Node parent = node;
            while (parent != null)
            {
                if (parent.ParentCollection == null)
                {
                    return -1;
                }
                if (parent.ParentCollection.Parent != null)
                {
                    parent = parent.ParentCollection.Parent;
                }
                else
                {
                    return parent.ParentCollection.IndexOf(parent);
                }
            }
            return -1;
        }

        private int GetVisibleOffset(int offset, NodeCollection coll, int index, Node node, out bool bContinue)
        {
            int num = offset;
            bContinue = true;
            for (int i = index; i < coll.Count; i++)
            {
                if (!bContinue || (coll[i] == node))
                {
                    bContinue = false;
                    return num;
                }
                num += coll.GetHeight(i, this.m_tsCurrent);
                if (coll[i].IsExpanded)
                {
                    num += this.GetVisibleOffset(0, coll[i].Nodes, 0, node, out bContinue);
                }
            }
            return num;
        }

        private bool IsOverTop(Node node)
        {
            int topMostParentIndex = this.GetTopMostParentIndex(node);
            if (topMostParentIndex < this.m_iActualNode)
            {
                return false;
            }
            if ((topMostParentIndex == this.m_iActualNode) && (node == this.m_ncNodes[this.m_iActualNode]))
            {
                return true;
            }
            bool bContinue = true;
            return (this.GetVisibleOffset(this.m_iActualNodeOffset, this.m_ncNodes, this.m_iActualNode, node, out bContinue) <= 0);
        }

        public bool IsVisible(Node node)
        {
            int num;
            int topMostParentIndex = this.GetTopMostParentIndex(node);
            if (topMostParentIndex < this.m_iActualNode)
            {
                return false;
            }
            if ((topMostParentIndex == this.m_iActualNode) && (node == this.m_ncNodes[this.m_iActualNode]))
            {
                return (this.m_iActualNodeOffset == 0);
            }
            topMostParentIndex = node.ParentCollection.IndexOf(node);
            if ((node.ParentCollection.LastDrawnNode == 0) || (node.ParentCollection.LastDrawnNode < topMostParentIndex))
            {
                num = base.Height - this.FooterHeight;
                int y = this.m_iActualNodeOffset + this.HeaderHeight;
                this.FindVisible(this.m_ncNodes, y, this.m_iActualNode, num);
            }
            bool bContinue = true;
            num = (base.Height - this.HeaderHeight) - this.FooterHeight;
            int num4 = this.GetVisibleOffset(this.m_iActualNodeOffset, this.m_ncNodes, this.m_iActualNode, node, out bContinue);
            int height = node.GetHeight(this.Templates);
            int lastDrawnNodeOffset = node.ParentCollection.LastDrawnNodeOffset;
            node.GetHeight(this.Templates);
            return (((topMostParentIndex <= node.ParentCollection.LastDrawnNode) && (num4 > 0)) && ((num4 + height) < num));
        }

        public bool IsVisible(NodeCollection coll, int ix)
        {
            return (((ix >= 0) && (coll.Count != 0)) || this.IsVisible(coll[ix]));
        }

        public void LoadDataManually()
        {
            if (this.NodeAdding == null)
            {
                InvalidOperationException exception = new InvalidOperationException("NodeAdding event does not have a handler.");
                throw exception;
            }
            if (!this.DelayLoad)
            {
                this.BeginUpdate();
                while (!this.AddNodeManually(this.m_ncNodes))
                {
                }
                this.EndUpdate();
            }
            else
            {
                this.m_ncNodes.ManualDataLoading = true;
                this.OnChanged(null, TreeRepaintEventArgs.Empty);
            }
        }

        public void LoadXml(string location)
        {
            this.LoadXml(location, null);
        }

        public void LoadXml(XmlReader reader)
        {
            try
            {
                this.BeginUpdate();
                this.m_conversion = new Conversion(this.Site);
                Conversion.DesignTimeCallback = this._designTimeCallback;
                while (reader.Read())
                {
                    string name = reader.Name;
                    if (name != null)
                    {
                        if (!(name == "ImageList"))
                        {
                            if (name == "AdvancedTree")
                            {
                                goto Label_0055;
                            }
                        }
                        else
                        {
                            this.ReadImageList(reader);
                        }
                    }
                    continue;
                Label_0055:
                    this.ReadAdvancedTree(reader);
                }
                this.m_conversion = null;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public void LoadXml(string location, DesignTimeCallback dtc)
        {
            XmlTextReader reader = null;
            this._designTimeCallback = dtc;
            try
            {
                reader = new XmlTextReader(location);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                this.LoadXml(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                reader = null;
                this._designTimeCallback = null;
            }
        }

        private void m_gradientBackColor_PropertyChanged(object sender, EventArgs e)
        {
            this.m_bGradientChanged = true;
            this.OnChanged(this, new TreeRepaintEventArgs());
        }

        private void m_vScroll_ValueChanged(object sender, EventArgs e)
        {
            this.bIsInScrollChange = true;
            this.OnChanged(this, new TreeChangedEventArgs(TreeEventArgsType.VScroll, this.m_iVScrollPrevValue - this.m_vScroll.Value));
            this.OnScroll();
            if (this.DelayLoad && (this.m_vScroll.Value > (this.m_vScroll.Maximum - (2 * this.m_vScroll.LargeChange))))
            {
                this.DoDelayLoad();
            }
            this.bIsInScrollChange = false;
        }

        protected virtual void OnActiveNodeChanged(EventArgs e)
        {
            if (this.ActiveNodeChanged != null)
            {
                this.ActiveNodeChanged(this, e);
            }
        }

        protected virtual void OnAfterCollapse(NodeEventArgs e)
        {
            if (this.AfterCollapse != null)
            {
                this.AfterCollapse(this, e);
            }
        }

        protected virtual void OnAfterExpand(NodeEventArgs e)
        {
            if (this.AfterExpand != null)
            {
                this.AfterExpand(this, e);
            }
        }

        protected void OnAfterNodeSelect(NodeEventArgs e)
        {
            bool flag = false;
            if (e.Node.Selected && (this.m_activeNode != e.Node))
            {
                flag = true;
                this.m_activeNode = e.Node;
            }
            else if (!e.Node.Selected && (this.m_activeNode == e.Node))
            {
                flag = true;
                this.m_activeNode = null;
            }
            e.Offset = this.EnsureVisible(e.Node, false);
            if (this.AfterNodeSelect != null)
            {
                this.AfterNodeSelect(this, e);
            }
            if (flag)
            {
                this.OnActiveNodeChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnBeforeCollapse(NodeCancelEventArgs e)
        {
            if (this.BeforeCollapse != null)
            {
                this.BeforeCollapse(this, e);
            }
        }

        protected virtual void OnBeforeExpand(NodeCancelEventArgs e)
        {
            if (this.BeforeExpand != null)
            {
                this.BeforeExpand(this, e);
            }
        }

        protected virtual void OnButton(ButtonCell c, Node n, Point index, int yOffset)
        {
            CellEventArgs e = new CellEventArgs(n, c, index.X, index.Y, yOffset);
            if (this.ButtonClick != null)
            {
                this.ButtonClick(this, e);
            }
        }

        internal virtual void OnChanged(object sender, TreeChangedEventArgs e)
        {
            this.m_bIsChange = true;
            if (this.m_iUpdate <= 0)
            {
                bool flag = false;
                int iOffset = 0;
                bool flag2 = false;
                switch (e.Type)
                {
                    case TreeEventArgsType.Resize:
                    {
                        TreeResizeEventArgs args2 = (TreeResizeEventArgs) e;
                        this.m_iActualHeight += args2.Offset;
                        if ((sender is int) && (((int) sender) < this.m_iActualNode))
                        {
                            this.m_iVScrollPrevValue += args2.Offset;
                            this.m_vScroll.Value += args2.Offset;
                        }
                        if (this.SetVScrollBar(this.m_iActualHeight))
                        {
                            flag = true;
                            iOffset = this.m_iVScrollPrevValue - this.m_vScroll.Value;
                            flag2 = true;
                        }
                        this.m_ncNodes.LastDrawnNode = 0;
                        break;
                    }
                    case TreeEventArgsType.Refresh:
                        if (this.m_rHeader != null)
                        {
                            this.m_rHeader.ResetCachedBounds(null);
                        }
                        if (this.m_rFooter != null)
                        {
                            this.m_rFooter.ResetCachedBounds(null);
                        }
                        if (this.m_ncNodes != null)
                        {
                            TreeRefreshEventArgs args = (TreeRefreshEventArgs) e;
                            if (args.ResetBounds)
                            {
                                this.m_ncNodes.ResetCachedBounds(args.Template);
                            }
                            this.m_iActualHeight = this.m_ncNodes.CalculateNodesHeight();
                            if (this.SetVScrollBar(this.m_iActualHeight))
                            {
                                this.m_ncNodes.ResetCachedBounds(args.Template);
                                this.m_iActualHeight = this.m_ncNodes.CalculateNodesHeight();
                                this.SetVScrollBar(this.m_iActualHeight);
                            }
                        }
                        if (this.m_iScrollChange == 0)
                        {
                            this.m_ncNodes.LastDrawnNode = 0;
                        }
                        this.SetScrollPos(this.m_ncNodes, this.m_iActualNode, this.m_iActualNodeOffset);
                        break;

                    case TreeEventArgsType.VScroll:
                        flag = true;
                        iOffset = (int) e.Param;
                        break;

                    case TreeEventArgsType.BeforeExpand:
                        this.OnBeforeExpand(e.Param as NodeCancelEventArgs);
                        break;

                    case TreeEventArgsType.AfterExpand:
                        this.OnAfterExpand(e.Param as NodeEventArgs);
                        break;

                    case TreeEventArgsType.BeforeCollapse:
                        this.OnBeforeCollapse(e.Param as NodeCancelEventArgs);
                        break;

                    case TreeEventArgsType.AfterCollapse:
                        this.OnAfterCollapse(e.Param as NodeEventArgs);
                        break;
                }
                if (flag)
                {
                    this.CalculateFirstNode(iOffset);
                    this.m_iVScrollPrevValue = this.m_vScroll.Value;
                }
                if (flag2)
                {
                    iOffset = 0;
                }
                this.SetRedrawBackBuffer(iOffset);
                base.Invalidate();
            }
        }

        protected override void OnClick(EventArgs e)
        {
            this.m_Timer.Enabled = false;
            if (!this.m_bShowingToolTip && !this.m_bTouchScrolling)
            {
                base.OnClick(e);
                Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                CellEventArgs cellAtPoint = this.GetCellAtPoint(pt);
                if (cellAtPoint != null)
                {
                    if (this.ShowPlusMinus)
                    {
                        int x = !this.RightToLeft ? (cellAtPoint.Node.Level * this.NodeIndent) : ((base.Width - (cellAtPoint.Node.Level * this.NodeIndent)) - this.PlusMinusSize.Width);
                        Rectangle rectangle = new Rectangle(x, cellAtPoint.Offset, this.PlusMinusSize.Width, this.PlusMinusSize.Height);
                        if (rectangle.Contains(pt))
                        {
                            if (cellAtPoint.Node.IsExpanded)
                            {
                                cellAtPoint.Node.Collapse();
                                return;
                            }
                            cellAtPoint.Node.Expand();
                            return;
                        }
                    }
                    if (this.CheckForLink(pt))
                    {
                        LinkCell c = cellAtPoint.Cell as LinkCell;
                        if (c != null)
                        {
                            this.OnLink(c, cellAtPoint.Node, new Point(cellAtPoint.NodeIndex, cellAtPoint.CellIndex), cellAtPoint.Offset);
                        }
                    }
                    else if (!this.CheckForButton(pt))
                    {
                        switch (cellAtPoint.NodeIndex)
                        {
                            case -2:
                                if (this.FooterClick != null)
                                {
                                    this.FooterClick(this, cellAtPoint);
                                }
                                return;

                            case -1:
                                if (this.HeaderClick != null)
                                {
                                    this.HeaderClick(this, cellAtPoint);
                                }
                                return;
                        }
                        int num2 = !this.RightToLeft ? ((cellAtPoint.Node.Level * this.NodeIndent) + this.NodeIndent) : ((base.Width - (cellAtPoint.Node.Level * this.NodeIndent)) - this.PlusMinusSize.Width);
                        if ((!this.RightToLeft && (pt.X > num2)) || (this.RightToLeft && (pt.X < num2)))
                        {
                            Node node = cellAtPoint.Node;
                            int currentTemplateIndex = node.CurrentTemplateIndex;
                            this.m_activeNode = node;
                            if ((this.m_mode != Resco.Controls.AdvancedTree.SelectionMode.NoSelect) && ((this.m_mode == Resco.Controls.AdvancedTree.SelectionMode.SelectDeselect) || !node.Selected))
                            {
                                this.SuspendRedraw();
                                node.Selected = !node.Selected;
                                this.ResumeRedraw();
                                this.OnAfterNodeSelect(new NodeEventArgs(cellAtPoint.Node, cellAtPoint.NodeIndex, cellAtPoint.Offset));
                            }
                            if (((currentTemplateIndex == node.CurrentTemplateIndex) && (cellAtPoint.CellIndex >= 0)) && (this.CellClick != null))
                            {
                                this.CellClick(this, cellAtPoint);
                            }
                            if ((this.m_mode == Resco.Controls.AdvancedTree.SelectionMode.SelectOnly) || ((this.m_mode == Resco.Controls.AdvancedTree.SelectionMode.SelectDeselect) && !node.Selected))
                            {
                                int iNodeIndex = (this.m_activeNode != null) ? this.m_activeNode.ParentCollection.IndexOf(this.m_activeNode) : -1;
                                this.OnNodeEntered(new NodeEnteredEventArgs(this.m_activeNode, iNodeIndex));
                            }
                        }
                    }
                }
            }
        }

        protected internal virtual void OnCustomizeCell(CustomizeCellEventArgs e)
        {
            if (this.CustomizeCell != null)
            {
                this.CustomizeCell(this, e);
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
            CellEventArgs cellAtPoint = this.GetCellAtPoint(pt);
            if ((cellAtPoint != null) && this.ShowPlusMinus)
            {
                int x = !this.RightToLeft ? ((cellAtPoint.Node.Level * this.NodeIndent) + this.PlusMinusSize.Width) : 0;
                Rectangle rectangle = new Rectangle(x, cellAtPoint.Offset, base.Width - ((cellAtPoint.Node.Level * this.NodeIndent) + this.PlusMinusSize.Width), cellAtPoint.Node.Height);
                if (rectangle.Contains(pt))
                {
                    if (cellAtPoint.Node.IsExpanded)
                    {
                        cellAtPoint.Node.Collapse();
                        return;
                    }
                    cellAtPoint.Node.Expand();
                    return;
                }
            }
            base.OnDoubleClick(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            this.m_TouchScrollingTimer.Enabled = false;
            if ((this.KeyNavigation && (this.SelectionMode == Resco.Controls.AdvancedTree.SelectionMode.SelectOnly)) && !this.MultiSelect)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        if (!this.RightToLeft)
                        {
                            this.CollapseNavigationNode();
                            break;
                        }
                        this.ExpandNavigationNode();
                        break;

                    case Keys.Up:
                        this.SelectedNode = this.GetPrevNode(this.m_activeNode);
                        break;

                    case Keys.Right:
                        if (!this.RightToLeft)
                        {
                            this.ExpandNavigationNode();
                            break;
                        }
                        this.CollapseNavigationNode();
                        break;

                    case Keys.Down:
                        this.SelectedNode = this.GetNextNode(this.m_activeNode);
                        break;
                }
            }
            base.OnKeyDown(e);
        }

        protected virtual void OnLink(LinkCell c, Node n, Point index, int yOffset)
        {
            LinkEventArgs lea = new LinkEventArgs(n, c, index.X, index.Y, yOffset);
            this.RedrawBackBuffer(false);
            c.DrawActiveLink(BackBufferManager.GetBackBufferGraphics(base.Width, base.Height), lea, base.Width - this.m_vScrollWidth);
            this.Refresh();
            Thread.Sleep(50);
            if (this.LinkClick != null)
            {
                this.LinkClick(this, lea);
            }
            Links.AddLink(lea.Target);
            this.OnChanged(this, TreeRepaintEventArgs.Empty);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.m_MousePosition.X = e.X;
            this.m_MousePosition.Y = e.Y;
            if (this.m_bEnableTouchScrolling && (this.m_vScroll != null))
            {
                this.m_TouchScrollingTimer.Enabled = false;
                this.m_MousePosition.X = e.X;
                this.m_MousePosition.Y = e.Y;
                this.m_TouchAutoScrollDiff = 0;
                this.m_TouchTime0 = this.GetTickCount();
                this.m_TouchTime1 = this.m_TouchTime0;
            }
            Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
            CellEventArgs cellAtPoint = this.GetCellAtPoint(pt);
            if ((cellAtPoint != null) && this.CheckForButton(pt))
            {
                if (cellAtPoint.Cell != null)
                {
                    this.m_pressedButtonNode = cellAtPoint.Node;
                    cellAtPoint.Node.PressedButtonIndex = cellAtPoint.CellIndex;
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
            else
            {
                try
                {
                    if (this.CheckForTooltip(this.m_MousePosition) != null)
                    {
                        this.m_Timer.Enabled = true;
                    }
                    else if ((this.m_ContextMenu != null) && ((e.Button == MouseButtons.Right) || ContextMenuSupport.RecognizeGesture(base.Handle, e.X, e.Y)))
                    {
                        this.ContextMenu.Show(this, new Point(e.X, e.Y));
                    }
                    else if (Environment.OSVersion.Platform == ((PlatformID) 0xc0))
                    {
                        this.m_Timer.Enabled = true;
                    }
                }
                catch
                {
                    this.m_Timer.Enabled = true;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (((this.m_bEnableTouchScrolling && (this.m_vScroll != null)) && (e.Button == MouseButtons.Left)) && (e.Y != this.m_MousePosition.Y))
            {
                int num = e.Y - this.m_MousePosition.Y;
                this.m_TouchAutoScrollDiff += num;
                if ((this.GetTickCount() - this.m_TouchTime1) > 100)
                {
                    this.m_TouchAutoScrollDiff = 0;
                    this.m_TouchTime0 = this.GetTickCount();
                    this.m_TouchTime1 = this.m_TouchTime0;
                }
                else
                {
                    this.m_TouchTime1 = this.GetTickCount();
                }
                if (!this.m_bShowingToolTip && (this.m_bTouchScrolling || (Math.Abs(this.m_TouchAutoScrollDiff) >= ((int) (this.m_touchSensitivity * this.m_scaleFactorY)))))
                {
                    this.m_Timer.Enabled = false;
                    this.m_bTouchScrolling = true;
                    this.m_MousePosition.X = e.X;
                    this.m_MousePosition.Y = e.Y;
                    int num2 = (this.m_vScroll.Maximum - this.m_vScroll.LargeChange) + 1;
                    int minimum = this.m_vScroll.Value - num;
                    if (minimum < this.m_vScroll.Minimum)
                    {
                        minimum = this.m_vScroll.Minimum;
                    }
                    if (minimum > num2)
                    {
                        minimum = num2;
                    }
                    this.m_vScroll.Value = minimum;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            try
            {
                base.OnMouseUp(e);
                if (this.m_Timer != null)
                {
                    this.m_Timer.Enabled = false;
                }
                if (this.m_pressedButtonNode != null)
                {
                    Node pressedButtonNode = this.m_pressedButtonNode;
                    int pressedButtonIndex = pressedButtonNode.PressedButtonIndex;
                    pressedButtonNode.PressedButtonIndex = -1;
                    this.m_pressedButtonNode = null;
                    if (!this.m_bTouchScrolling)
                    {
                        Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                        CellEventArgs cellAtPoint = this.GetCellAtPoint(pt);
                        if (((cellAtPoint != null) && (cellAtPoint.Cell is ButtonCell)) && ((cellAtPoint.Node == pressedButtonNode) && (cellAtPoint.CellIndex == pressedButtonIndex)))
                        {
                            this.OnButton((ButtonCell) cellAtPoint.Cell, cellAtPoint.Node, new Point(cellAtPoint.NodeIndex, cellAtPoint.CellIndex), 0);
                        }
                    }
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
                if (this.m_bShowingToolTip)
                {
                    this.m_ToolTip.Visible = false;
                    this.m_bShowingToolTip = false;
                }
                else if ((this.m_bEnableTouchScrolling && (this.m_vScroll != null)) && (e.Button == MouseButtons.Left))
                {
                    if (!this.m_bTouchScrolling || ((this.GetTickCount() - this.m_TouchTime1) > 100))
                    {
                        this.m_TouchAutoScrollDiff = 0;
                    }
                    uint num2 = (this.GetTickCount() - this.m_TouchTime0) / 50;
                    if (num2 > 0)
                    {
                        this.m_TouchAutoScrollDiff = (int) (this.m_TouchAutoScrollDiff / num2);
                    }
                    this.m_TouchScrollingTimer.Enabled = true;
                    this.m_bTouchScrolling = false;
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        internal void OnNodeAdded(Node node, int index)
        {
            int offset = 0;
            bool flag = true;
            NodeCollection parentCollection = node.ParentCollection;
            while (parentCollection.Parent != null)
            {
                if (parentCollection.Parent.IsExpanded)
                {
                    parentCollection = parentCollection.Parent.ParentCollection;
                }
                else
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                offset = node.GetTemplate(this.Templates).GetHeight(node);
                if (this.m_bDrawGrid)
                {
                    offset++;
                }
                if (node.IsExpanded)
                {
                    offset += node.Nodes.CalculateNodesHeight();
                }
            }
            if ((index < this.m_iActualNode) && (node.ParentCollection == this.m_ncNodes))
            {
                this.m_iActualNode++;
            }
            this.OnChanged(index, new TreeResizeEventArgs(offset));
        }

        protected virtual void OnNodeAdding(NodeAddingEventArgs e)
        {
            if (this.NodeAdding != null)
            {
                this.NodeAdding(this, e);
            }
        }

        protected virtual void OnNodeEntered(NodeEnteredEventArgs e)
        {
            if (this.NodeEntered != null)
            {
                this.NodeEntered(this, e);
            }
        }

        internal void OnNodeRemoved(Node node, int index)
        {
            int height = 0;
            bool flag = true;
            NodeCollection parentCollection = node.ParentCollection;
            while (parentCollection.Parent != null)
            {
                if (parentCollection.Parent.IsExpanded)
                {
                    parentCollection = parentCollection.Parent.ParentCollection;
                }
                else
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                height = node.GetTemplate(this.Templates).GetHeight(node);
                if (this.m_bDrawGrid)
                {
                    height++;
                }
                if (node.IsExpanded)
                {
                    height += node.Nodes.CalculateNodesHeight();
                }
            }
            if (node.ParentCollection == this.m_ncNodes)
            {
                if (index == this.m_iActualNode)
                {
                    this.EnsureVisible(this.m_ncNodes, this.m_iActualNode);
                }
                if (index < this.m_iActualNode)
                {
                    this.m_iActualNode--;
                }
            }
            this.OnChanged(index, new TreeResizeEventArgs(-height));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            bool isValid = BackBufferManager.IsValid(this);
            if ((this.m_bIsChange || !isValid) && (((this.m_iUpdate == 0) && (this.m_iNoRedraw <= 0)) && (!this.RedrawBackBuffer(isValid) && this.DelayLoad)))
            {
                this.DoDelayLoad();
            }
            e.Graphics.DrawImage(BackBufferManager.GetBackBufferImage(base.Width, base.Height), 0, 0);
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (this.m_ncNodes != null)
            {
                this.EndUpdate();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.m_vScroll != null)
            {
                this.m_vScroll.Bounds = new Rectangle(base.Width - this.m_iScrollWidth, 0, this.m_iScrollWidth, base.Height);
            }
            this.SetVScrollBar(this.m_iActualHeight);
            this.OnChanged(this, new TreeRefreshEventArgs(true));
            if (this.m_iUpdate == 0)
            {
                this.SetRedrawBackBuffer(0);
                this.m_ncNodes.LastDrawnNode = -1;
            }
            base.Invalidate();
        }

        protected virtual void OnScroll()
        {
            if (this.Scroll != null)
            {
                this.Scroll(this, EventArgs.Empty);
            }
        }

        private void OnScrollResize(object sender, EventArgs e)
        {
            if (sender == this.m_vScroll)
            {
                this.ScrollbarWidth = this.m_vScroll.Width;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            try
            {
                this.m_Timer.Enabled = false;
                Point mousePosition = this.m_MousePosition;
                string str = this.CheckForTooltip(mousePosition);
                if (str != null)
                {
                    if (this.m_ToolTip == null)
                    {
                        this.m_ToolTip = new Resco.Controls.AdvancedTree.ToolTip();
                        base.Controls.Add(this.m_ToolTip);
                    }
                    this.m_ToolTip.Text = str;
                    this.m_ToolTip.Show(mousePosition);
                    this.m_bShowingToolTip = true;
                }
                else if (this.ContextMenu != null)
                {
                    this.ContextMenu.Show(this, this.m_MousePosition);
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private void OnTouchScrollingTimerTick(object sender, EventArgs e)
        {
            int num = (this.m_vScroll.Maximum - this.m_vScroll.LargeChange) + 1;
            int minimum = this.m_vScroll.Value - this.m_TouchAutoScrollDiff;
            if (minimum < this.m_vScroll.Minimum)
            {
                minimum = this.m_vScroll.Minimum;
            }
            if (minimum > num)
            {
                minimum = num;
            }
            this.m_vScroll.Value = minimum;
            if (this.m_TouchAutoScrollDiff < 0)
            {
                this.m_TouchAutoScrollDiff += (Math.Abs(this.m_TouchAutoScrollDiff) / 10) + 1;
                if (this.m_TouchAutoScrollDiff > 0)
                {
                    this.m_TouchAutoScrollDiff = 0;
                    this.m_TouchScrollingTimer.Enabled = false;
                }
            }
            else if (this.m_TouchAutoScrollDiff > 0)
            {
                this.m_TouchAutoScrollDiff -= (Math.Abs(this.m_TouchAutoScrollDiff) / 10) + 1;
                if (this.m_TouchAutoScrollDiff < 0)
                {
                    this.m_TouchAutoScrollDiff = 0;
                    this.m_TouchScrollingTimer.Enabled = false;
                }
            }
            else
            {
                this.m_TouchAutoScrollDiff = 0;
                this.m_TouchScrollingTimer.Enabled = false;
            }
        }

        private void ReadAdvancedTree(XmlReader reader)
        {
            try
            {
                if (reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                    {
                        try
                        {
                            this.m_conversion.SetProperty(this, reader.Name, reader.Value);
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    reader.MoveToElement();
                }
                this.Templates.Clear();
                if (!reader.IsEmptyElement)
                {
                    goto Label_01D9;
                }
                return;
            Label_0051:
                try
                {
                    int num;
                    string name = reader.Name;
                    if (name == null)
                    {
                        goto Label_01D9;
                    }
                    if (BigHas.methodxxx.TryGetValue(name,out num)) //if (<PrivateImplementationDetails>{6AB0F983-3A18-4A24-A179-D81AD869DE22}.$$method0x60000d6-1.TryGetValue(name, ref num))
                    {
                        switch (num)
                        {
                            case 0:
                                goto Label_01D9;

                            case 1:
                                this.Templates.Add(this.ReadNodeTemplate(reader));
                                goto Label_01D9;

                            case 2:
                                return;

                            case 3:
                                goto Label_0131;

                            case 4:
                                goto Label_014C;

                            case 5:
                                goto Label_0155;

                            case 6:
                                goto Label_016D;

                            case 7:
                                goto Label_0176;
                        }
                    }
                    goto Label_01BC;
                Label_0131:
                    this.Header.TemplateIndex = Convert.ToInt32(reader.ReadString());
                    goto Label_01D9;
                Label_014C:
                    this.ReadHeader(reader);
                    goto Label_01D9;
                Label_0155:
                    this.Footer.TemplateIndex = Convert.ToInt32(reader.ReadString());
                    goto Label_01D9;
                Label_016D:
                    this.ReadFooter(reader);
                    goto Label_01D9;
                Label_0176:
                    if ((reader.HasAttributes && (reader.GetAttribute("Name") != null)) && (reader.GetAttribute("Value") != null))
                    {
                        this.m_conversion.SetProperty(this, reader.GetAttribute("Name"), reader.GetAttribute("Value"));
                    }
                    goto Label_01D9;
                Label_01BC:
                    this.m_conversion.SetProperty(this, reader.Name, reader.ReadString());
                }
                catch
                {
                }
            Label_01D9:
                if (reader.Read())
                {
                    goto Label_0051;
                }
            }
            catch
            {
            }
        }

        private Cell ReadCell(XmlReader reader)
        {
            Cell o = null;
            try
            {
                string str = reader["Name"];
                string typeName = reader["Type"];
                string sRect = reader["Bounds"];
                if (typeName == null)
                {
                    typeName = "Resco.Controls.AdvancedTree.Cell";
                }
                if (!typeName.StartsWith("Resco.Controls.AdvancedTree.") && typeName.StartsWith("Resco.Controls."))
                {
                    typeName = typeName.Insert(typeName.LastIndexOf('.'), ".AdvancedTree");
                }
                ConstructorInfo info = Type.GetType(typeName).GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);
                if (info != null)
                {
                    o = (Cell) info.Invoke(new object[0]);
                }
                if (this._designTimeCallback != null)
                {
                    this._designTimeCallback(o, null);
                }
                o.Name = str;
                if (sRect != null)
                {
                    o.Bounds = Conversion.RectangleFromString(sRect);
                }
                if (!reader.IsEmptyElement)
                {
                    goto Label_012C;
                }
                return o;
            Label_00C6:
                try
                {
                    string str4;
                    if (((str4 = reader.Name) == null) || (str4 == ""))
                    {
                        goto Label_012C;
                    }
                    if (!(str4 == "Cell"))
                    {
                        if (str4 == "Property")
                        {
                            goto Label_0105;
                        }
                        goto Label_012C;
                    }
                    return o;
                Label_0105:
                    this.m_conversion.SetProperty(o, reader["Name"], reader["Value"]);
                }
                catch
                {
                }
            Label_012C:
                if (reader.Read())
                {
                    goto Label_00C6;
                }
            }
            catch
            {
                try
                {
                    if (((reader.Name == "Cell") && reader.IsStartElement()) && !reader.IsEmptyElement)
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "Cell")
                            {
                                goto Label_017C;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        Label_017C:
            if (o != null)
            {
                return o;
            }
            return new Cell();
        }

        private void ReadFooter(XmlReader reader)
        {
            this.Footer.StringData = Conversion.StringDataFromString(reader.ReadString());
            string[] stringData = this.Footer.StringData;
            if ((stringData.Length == 1) && (stringData[0] == ""))
            {
                while (reader.Name != "Footer")
                {
                    string str;
                    if (((str = reader.Name) != null) && (str == "Property"))
                    {
                        if (reader.HasAttributes && (reader["Name"] == "StringData"))
                        {
                            this.Footer.StringData = Conversion.StringDataFromString(reader["Value"]);
                        }
                        else if ((reader.HasAttributes && (reader.GetAttribute("Name") != null)) && (reader.GetAttribute("Value") != null))
                        {
                            this.m_conversion.SetProperty(this.Footer, reader.GetAttribute("Name"), reader.GetAttribute("Value"));
                        }
                    }
                    reader.Read();
                }
            }
        }

        private void ReadHeader(XmlReader reader)
        {
            this.Header.StringData = Conversion.StringDataFromString(reader.ReadString());
            string[] stringData = this.Header.StringData;
            if ((stringData.Length == 1) && (stringData[0] == ""))
            {
                while (reader.Name != "Header")
                {
                    string str;
                    if (((str = reader.Name) != null) && (str == "Property"))
                    {
                        if (reader.HasAttributes && (reader["Name"] == "StringData"))
                        {
                            this.Header.StringData = Conversion.StringDataFromString(reader["Value"]);
                        }
                        else if ((reader.HasAttributes && (reader.GetAttribute("Name") != null)) && (reader.GetAttribute("Value") != null))
                        {
                            this.m_conversion.SetProperty(this.Header, reader.GetAttribute("Name"), reader.GetAttribute("Value"));
                        }
                    }
                    reader.Read();
                }
            }
        }

        private void ReadImageList(XmlReader reader)
        {
            try
            {
                string sName = reader["Name"];
                string sSize = reader["ImageSize"];
                ImageList imageList = this.m_conversion.GetImageList(sName);
                if (imageList != null)
                {
                    try
                    {
                        Size size = Conversion.SizeFromString(sSize);
                        imageList.ImageSize = size;
                    }
                    catch
                    {
                        imageList.ImageSize = new Size(0x10, 0x10);
                    }
                    if (imageList.Images.Count > 0)
                    {
                        PropertyDescriptor descriptor = TypeDescriptor.GetProperties(imageList)["Images"];
                        if (descriptor != null)
                        {
                            ((IList) descriptor.GetValue(imageList)).Clear();
                        }
                    }
                    if (!reader.IsEmptyElement)
                    {
                        byte[] array = new byte[0x3e8];
                        while (reader.Read())
                        {
                            if (reader.Name == "Data")
                            {
                                try
                                {
                                    MemoryStream stream;
                                    if (reader is XmlTextReader)
                                    {
                                        int num;
                                        stream = new MemoryStream();
                                        while ((num = ((XmlTextReader) reader).ReadBase64(array, 0, 0x3e8)) > 0)
                                        {
                                            stream.Write(array, 0, num);
                                        }
                                        stream.Position = 0L;
                                    }
                                    else
                                    {
                                        stream = new MemoryStream(Convert.FromBase64String(reader.ReadString()));
                                    }
                                    Bitmap bitmap = new Bitmap(stream);
                                    PropertyDescriptor descriptor2 = TypeDescriptor.GetProperties(imageList)["Images"];
                                    if (descriptor2 != null)
                                    {
                                        ((IList) descriptor2.GetValue(imageList)).Add(bitmap);
                                    }
                                    else
                                    {
                                        imageList.Images.Add(bitmap);
                                    }
                                }
                                catch
                                {
                                }
                                reader.ReadEndElement();
                            }
                            if ((reader.NodeType == XmlNodeType.EndElement) && (reader.Name == "ImageList"))
                            {
                                return;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private NodeTemplate ReadNodeTemplate(XmlReader reader)
        {
            NodeTemplate o = new NodeTemplate();
            if (this._designTimeCallback != null)
            {
                this._designTimeCallback(o, null);
            }
            o.Name = reader["Name"];
            try
            {
                string str = reader["Height"];
                if (str != null)
                {
                    o.Height = Convert.ToInt32(str);
                }
            }
            catch
            {
            }
            if (!reader.IsEmptyElement)
            {
            Label_011E:
                if (!reader.Read())
                {
                    return o;
                }
                try
                {
                    string str2;
                    if (((str2 = reader.Name) == null) || (str2 == ""))
                    {
                        goto Label_011E;
                    }
                    if (!(str2 == "NodeTemplate"))
                    {
                        if (str2 == "Cell")
                        {
                            goto Label_00A6;
                        }
                        if (str2 == "Property")
                        {
                            goto Label_00BB;
                        }
                        goto Label_0101;
                    }
                    return o;
                Label_00A6:
                    o.CellTemplates.Add(this.ReadCell(reader));
                    goto Label_011E;
                Label_00BB:
                    if ((reader.HasAttributes && (reader["Name"] != null)) && (reader["Value"] != null))
                    {
                        this.m_conversion.SetProperty(o, reader["Name"], reader["Value"]);
                    }
                    goto Label_011E;
                Label_0101:
                    this.m_conversion.SetProperty(o, reader.Name, reader.ReadString());
                }
                catch
                {
                }
                goto Label_011E;
            }
            return o;
        }

        protected virtual bool RedrawBackBuffer(bool isValid)
        {
            int lastDrawnNode;
            int num13;
            Graphics backBufferGraphics = BackBufferManager.GetBackBufferGraphics(base.Width, base.Height);
            Bitmap backBufferImage = BackBufferManager.GetBackBufferImage(base.Width, base.Height);
            Graphics tempGraphics = BackBufferManager.GetTempGraphics(base.Width, base.Height);
            Bitmap tempImage = BackBufferManager.GetTempImage(base.Width, base.Height);
            bool bResetScrollbar = false;
            this.backRect.Width = base.Width;
            this.backRect.Height = base.Height;
            if ((this.m_useGradient && (this.m_gradientBackColor.FillDirection == FillDirection.Vertical)) && (!BackBufferManager.IsValidGradient(this) || this.m_bGradientChanged))
            {
                this.m_gradientBackColor.DrawGradient(BackBufferManager.GetGradientGraphics(base.Width, base.Height), this.backRect);
                int red = (this.m_gradientBackColor.EndColor.R + this.m_gradientBackColor.StartColor.R) / 2;
                int green = (this.m_gradientBackColor.EndColor.G + this.m_gradientBackColor.StartColor.G) / 2;
                int blue = (this.m_gradientBackColor.EndColor.B + this.m_gradientBackColor.StartColor.B) / 2;
                this.m_colorKey = Color.FromArgb(red, green, blue);
                this.m_brushKey.Color = this.m_colorKey;
                this.m_imgAttr.SetColorKey(this.m_colorKey, this.m_colorKey);
                this.m_bGradientChanged = false;
            }
            int width = base.Width;
            int height = base.Height;
            bool flag2 = true;
            int iScrollChange = this.m_iScrollChange;
            this.m_iScrollChange = 0;
            int num7 = 0;
            int num8 = 0;
            NodeTemplate t = null;
            NodeTemplate template2 = null;
            if (this.m_bShowHeader)
            {
                t = this.m_tsCurrent[this.m_rHeader.TemplateIndex];
                if (t != null)
                {
                    num7 = this.CustomizeHeaderFooter(t, this.m_rHeader, ref bResetScrollbar);
                }
            }
            if (this.m_bShowFooter)
            {
                template2 = this.m_tsCurrent[this.m_rFooter.TemplateIndex];
                if (template2 != null)
                {
                    num8 = this.CustomizeHeaderFooter(template2, this.m_rFooter, ref bResetScrollbar);
                }
            }
            int ymax = height - num8;
            if ((Math.Abs(iScrollChange) > (ymax - num7)) || !isValid)
            {
                iScrollChange = 0;
            }
            int num10 = width - this.m_vScrollWidth;
            int y = 0;
            this.ResetCache(iScrollChange);
            int num14 = 0;
            if (iScrollChange < 0)
            {
                int num15 = height + iScrollChange;
                lastDrawnNode = this.m_ncNodes.LastDrawnNode;
                num13 = this.m_ncNodes.LastDrawnNodeOffset + iScrollChange;
                Rectangle srcRect = new Rectangle(0, -iScrollChange, width, num15);
                Rectangle destRect = new Rectangle(0, 0, width, num15);
                if (this.m_useGradient && (this.m_gradientBackColor.FillDirection == FillDirection.Vertical))
                {
                    backBufferGraphics.DrawImage(tempImage, destRect, srcRect, GraphicsUnit.Pixel);
                    tempGraphics.DrawImage(backBufferImage, 0, 0);
                }
                else
                {
                    tempGraphics.DrawImage(backBufferImage, 0, 0);
                    backBufferGraphics.DrawImage(tempImage, destRect, srcRect, GraphicsUnit.Pixel);
                }
                num14 = num15 - num8;
            }
            else
            {
                lastDrawnNode = this.m_iActualNode;
                num13 = this.m_iActualNodeOffset + num7;
                if (iScrollChange > 0)
                {
                    int num16 = height - iScrollChange;
                    Rectangle rectangle3 = new Rectangle(0, 0, width, num16);
                    Rectangle rectangle4 = new Rectangle(0, iScrollChange, width, num16);
                    if (this.m_useGradient && (this.m_gradientBackColor.FillDirection == FillDirection.Vertical))
                    {
                        backBufferGraphics.DrawImage(tempImage, rectangle4, rectangle3, GraphicsUnit.Pixel);
                        tempGraphics.DrawImage(backBufferImage, 0, 0);
                    }
                    else
                    {
                        tempGraphics.DrawImage(backBufferImage, 0, 0);
                        backBufferGraphics.DrawImage(tempImage, rectangle4, rectangle3, GraphicsUnit.Pixel);
                    }
                    ymax = iScrollChange + num7;
                }
            }
            if (this.m_useGradient && (this.m_gradientBackColor.FillDirection != FillDirection.Vertical))
            {
                this.m_gradientBackColor.DrawGradient(backBufferGraphics, new Rectangle(0, num14, width, ymax - num14));
            }
            Graphics gr = backBufferGraphics;
            if (this.m_useGradient && (this.m_gradientBackColor.FillDirection == FillDirection.Vertical))
            {
                gr = tempGraphics;
            }
            y = this.m_ncNodes.Draw(gr, this.m_tsCurrent, num10, ymax, num14, lastDrawnNode, num13, ref bResetScrollbar);
            if (y < ymax)
            {
                if (!this.m_useGradient)
                {
                    backBufferGraphics.FillRectangle(this.m_BackColor, 0, y, width, ymax - y);
                }
                else if (this.m_gradientBackColor.FillDirection != FillDirection.Vertical)
                {
                    this.m_gradientBackColor.DrawGradient(backBufferGraphics, new Rectangle(0, y, width, ymax - y));
                }
                else if (this.m_gradientBackColor.FillDirection == FillDirection.Vertical)
                {
                    gr.FillRectangle(this.m_brushKey, 0, y, width, ymax - y);
                }
                flag2 = false;
            }
            if (this.m_bShowHeader && (t != null))
            {
                gr.Clip = new Region(new Rectangle(0, 0, width, num7));
                t.Draw(backBufferGraphics, -this.PlusMinusSize.Width, 0, this.m_rHeader, num10, -1);
                gr.ResetClip();
            }
            if (this.m_bShowFooter && (template2 != null))
            {
                template2.Draw(backBufferGraphics, -this.PlusMinusSize.Width, height - num8, this.m_rFooter, num10, -1);
            }
            if (this.m_useGradient && (this.m_gradientBackColor.FillDirection == FillDirection.Vertical))
            {
                backBufferGraphics.DrawImage(BackBufferManager.GetGradientImage(base.Width, base.Height), 0, 0);
                backBufferGraphics.DrawImage(tempImage, this.backRect, 0, 0, this.backRect.Width, this.backRect.Height, GraphicsUnit.Pixel, this.m_imgAttr);
            }
            if (bResetScrollbar)
            {
                this.OnChanged(this, new TreeRefreshEventArgs(true));
                return flag2;
            }
            this.m_bIsChange = false;
            return flag2;
        }

        private void ResetCache(int iScroll)
        {
            if (iScroll == 0)
            {
                this.m_alLinks.Clear();
                this.m_alButtons.Clear();
                this.m_alTooltips.Clear();
            }
            else
            {
                int headerHeight = this.HeaderHeight;
                int num2 = base.Height - this.FooterHeight;
                for (int i = this.m_alLinks.Count - 1; i >= 0; i--)
                {
                    Rectangle rectangle = this.m_alLinks[i];
                    rectangle.Y += iScroll;
                    if ((rectangle.Bottom < headerHeight) || (rectangle.Top > num2))
                    {
                        this.m_alLinks.RemoveAt(i);
                    }
                    else
                    {
                        this.m_alLinks[i]=rectangle;
                    }
                }
                for (int j = this.m_alButtons.Count - 1; j >= 0; j--)
                {
                    Rectangle rectangle2 = this.m_alButtons[j];
                    rectangle2.Y += iScroll;
                    if ((rectangle2.Bottom < headerHeight) || (rectangle2.Top > num2))
                    {
                        this.m_alButtons.RemoveAt(j);
                    }
                    else
                    {
                        this.m_alButtons[j] = rectangle2;
                    }
                }
                for (int k = this.m_alTooltips.Count - 1; k >= 0; k--)
                {
                    Rectangle bounds = this.m_alTooltips[k].Bounds;
                    bounds.Y += iScroll;
                    if ((bounds.Bottom < headerHeight) || (bounds.Top > num2))
                    {
                        this.m_alTooltips.RemoveAt(k);
                    }
                    else
                    {
                        this.m_alTooltips[k].Bounds = bounds;
                    }
                }
            }
        }

        public void ResumeRedraw()
        {
            if (this.m_iNoRedraw > 0)
            {
                this.m_iNoRedraw--;
            }
            if (this.m_iNoRedraw == 0)
            {
                this.OnChanged(this, TreeRepaintEventArgs.Empty);
                base.Invalidate();
            }
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            float width = factor.Width;
            float height = factor.Height;
            this.BeginUpdate();
            if ((width != 1.0) || (height != 1.0))
            {
                this.ScrollbarWidth = (int) (this.ScrollbarWidth * width);
                this.PlusMinusSize = new Size((int) (this.PlusMinusSize.Width * width), (int) (this.PlusMinusSize.Height * height));
                this.PlusMinusMargin = new Size((int) (this.PlusMinusMargin.Width * width), (int) (this.PlusMinusMargin.Height * height));
                if (this.LevelIndentation >= 0)
                {
                    this.LevelIndentation = (int) (this.LevelIndentation * width);
                }
                this.TouchSensitivity = (int) (this.TouchSensitivity * height);
                if (this.m_rFooter != null)
                {
                    this.m_rFooter.ResetCachedBounds(null);
                }
                if (this.m_rHeader != null)
                {
                    this.m_rHeader.ResetCachedBounds(null);
                }
                if (this.m_ncNodes != null)
                {
                    this.m_ncNodes.ResetCachedBounds(null);
                }
                foreach (NodeTemplate template in this.Templates)
                {
                    template.Scale(width, height);
                }
            }
            base.ScaleControl(factor, specified);
            this.EndUpdate();
        }

        private void SetRedrawBackBuffer(int iScroll)
        {
            this.m_bIsChange = true;
            if ((iScroll == 0) && !this.bIsInScrollChange)
            {
                this.m_iScrollChange = 0;
            }
            else
            {
                this.m_iScrollChange += iScroll;
            }
        }

        public int SetScrollPos(NodeCollection coll, int ix, int offset)
        {
            if (this.m_vScroll == null)
            {
                bool bContinue = true;
                if ((coll.Count < 0) && (coll.Parent != null))
                {
                    ix = coll.Parent.Nodes.IndexOf(coll.Parent);
                    coll = coll.Parent.ParentCollection;
                }
                offset = 0;
                if (coll.Count > 0)
                {
                    offset = this.GetVisibleOffset(0, this.m_ncNodes, 0, coll[ix], out bContinue);
                }
                return offset;
            }
            int headerHeight = this.HeaderHeight;
            Node parent = null;
            NodeCollection parentCollection = null;
            NodeCollection nodes2 = coll;
            while (((ix > 0) && (offset > 0)) || (nodes2 != this.m_ncNodes))
            {
                if (ix > 0)
                {
                    ix--;
                    offset -= nodes2.GetHeight(ix, this.m_tsCurrent, true);
                }
                else
                {
                    parent = nodes2.Parent;
                    if (parent != null)
                    {
                        parentCollection = nodes2.Parent.ParentCollection;
                        ix = parentCollection.IndexOf(parent);
                        nodes2 = parentCollection;
                        offset -= nodes2.GetHeight(ix, this.m_tsCurrent);
                    }
                }
            }
            if ((ix == 0) && (offset > 0))
            {
                offset = 0;
            }
            this.m_iActualNode = ix;
            this.m_iActualNodeOffset = offset;
            int num2 = -offset;
            int num3 = this.m_vScroll.Maximum - ((base.Height - this.HeaderHeight) - this.FooterHeight);
            if (num3 < 0)
            {
                num3 = 0;
            }
            for (int i = 0; i < this.m_iActualNode; i++)
            {
                int num5 = num2 + this.m_ncNodes.GetHeight(i, this.m_tsCurrent);
                if (num5 > (num3 + 1))
                {
                    this.m_iActualNode = i;
                    for (int j = this.m_iActualNode; j < ix; j++)
                    {
                        headerHeight += this.m_ncNodes.GetHeight(j, this.m_tsCurrent);
                    }
                    this.m_iActualNodeOffset += (num3 > 0) ? (num2 - num3) : 0;
                    headerHeight += (num3 > 0) ? (num2 - num3) : 0;
                    num2 = num3;
                    break;
                }
                num2 += this.m_ncNodes.GetHeight(i, this.m_tsCurrent, true);
            }
            this.m_iVScrollPrevValue = num2;
            this.m_vScroll.Value = num2;
            this.OnChanged(this, new TreeChangedEventArgs(TreeEventArgsType.Empty, null));
            return headerHeight;
        }

        private bool SetVScrollBar(int height)
        {
            bool vScrollBarVisible = false;
            int num = this.HeaderHeight + this.FooterHeight;
            int num2 = base.Height - num;
            if (num2 <= 0)
            {
                if (this.m_vScroll == null)
                {
                    return false;
                }
                vScrollBarVisible = this.VScrollBarVisible;
                this.m_vScrollWidth = 0;
                this.m_vScroll.Hide();
                this.m_vScroll.Value = 0;
                if (this.m_iVScrollPrevValue == this.m_vScroll.Value)
                {
                    return vScrollBarVisible;
                }
                return true;
            }
            if (this.m_vScroll == null)
            {
                this.m_vScroll = new ScrollbarWrapper((this.m_vScrollBarResco == null) ? new System.Windows.Forms.VScrollBar() : this.m_vScrollBarResco, ScrollOrientation.VerticalScroll);
                if (!this.RightToLeft)
                {
                    this.m_vScroll.Bounds = new Rectangle(base.Width - this.m_iScrollWidth, 0, this.m_iScrollWidth, base.Height);
                }
                else
                {
                    this.m_vScroll.Bounds = new Rectangle(0, 0, this.m_iScrollWidth, base.Height);
                }
                this.m_vScroll.Minimum = 0;
                this.m_vScroll.Maximum = Math.Max(0, height + ScrollBottomOffset);
                this.m_vScroll.SmallChange = (num2 < ScrollSmallChange) ? num2 : ScrollSmallChange;
                this.m_vScroll.LargeChange = num2;
                this.m_vScroll.Value = 0;
                this.m_vScroll.ValueChanged += new EventHandler(this.m_vScroll_ValueChanged);
                this.m_vScroll.Resize += new EventHandler(this.OnScrollResize);
                this.VScrollBarVisible = false;
                this.m_vScroll.Attach(this);
                if (num2 >= height)
                {
                    return false;
                }
                this.VScrollBarVisible = this.ShowScrollbar;
                this.m_vScrollWidth = this.ShowScrollbar ? this.m_iScrollWidth : 0;
                return this.ShowScrollbar;
            }
            if (!this.RightToLeft)
            {
                this.m_vScroll.Left = base.Width - this.m_iScrollWidth;
            }
            else
            {
                this.m_vScroll.Left = 0;
            }
            if (num2 < ScrollSmallChange)
            {
                this.m_vScroll.SmallChange = num2;
            }
            if ((this.m_vScroll.Value + num2) > height)
            {
                this.m_vScroll.Value = Math.Max(0, height - num2);
            }
            this.m_vScroll.Maximum = Math.Max(0, height + ScrollBottomOffset);
            if (num2 != this.m_vScroll.LargeChange)
            {
                this.m_vScroll.LargeChange = num2;
            }
            if (height > num2)
            {
                vScrollBarVisible = !(!this.VScrollBarVisible ^ this.ShowScrollbar);
                if (vScrollBarVisible)
                {
                    if (this.ShowScrollbar)
                    {
                        this.m_vScrollWidth = this.m_iScrollWidth;
                        this.VScrollBarVisible = true;
                    }
                    else
                    {
                        this.m_vScrollWidth = 0;
                        this.VScrollBarVisible = false;
                    }
                }
            }
            else
            {
                vScrollBarVisible = this.VScrollBarVisible;
                if (vScrollBarVisible)
                {
                    this.m_vScrollWidth = 0;
                    this.VScrollBarVisible = false;
                }
            }
            if (this.m_iVScrollPrevValue == this.m_vScroll.Value)
            {
                return vScrollBarVisible;
            }
            return true;
        }

        protected virtual bool ShouldSerializeFooter()
        {
            return this.m_bShowFooter;
        }

        protected virtual bool ShouldSerializeGradientBackColor()
        {
            return ((this.m_gradientBackColor.StartColor.ToArgb() != SystemColors.ControlLightLight.ToArgb()) | (this.m_gradientBackColor.EndColor.ToArgb() != SystemColors.ControlLightLight.ToArgb()));
        }

        protected virtual bool ShouldSerializeHeader()
        {
            return this.m_bShowHeader;
        }

        protected virtual bool ShouldSerializePlusMinusMargin()
        {
            if (this.m_szPlusMinusMargin.Width == 3)
            {
                return (this.m_szPlusMinusMargin.Height != 3);
            }
            return true;
        }

        protected virtual bool ShouldSerializePlusMinusSize()
        {
            if (this.m_szPlusMinusSize.Width == 15)
            {
                return (this.m_szPlusMinusSize.Height != 15);
            }
            return true;
        }

        protected virtual bool ShouldSerializeSelectionMode()
        {
            return (this.m_mode != Resco.Controls.AdvancedTree.SelectionMode.SelectOnly);
        }

        public void SuspendRedraw()
        {
            this.m_iNoRedraw++;
        }

        public Node ActiveNode
        {
            get
            {
                return this.m_activeNode;
            }
            set
            {
                if (value != this.m_activeNode)
                {
                    if (value == null)
                    {
                        this.SelectedNode = null;
                    }
                    else
                    {
                        this.SelectedNode = value;
                        this.OnActiveNodeChanged(EventArgs.Empty);
                    }
                }
            }
        }

        protected internal Graphics BackBuffer
        {
            get
            {
                return BackBufferManager.GetBackBufferGraphics(base.Width, base.Height);
            }
        }

        public Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                this.m_BackColor = new SolidBrush(value);
                this.OnChanged(this, new TreeRepaintEventArgs());
            }
        }

        public System.Windows.Forms.ContextMenu ContextMenu
        {
            get
            {
                return this.m_ContextMenu;
            }
            set
            {
                this.m_ContextMenu = value;
            }
        }

        public bool DelayLoad
        {
            get
            {
                return this.m_bDelayLoad;
            }
            set
            {
                if (this.m_bDelayLoad != value)
                {
                    this.m_bDelayLoad = value;
                }
            }
        }

        public Resco.Controls.AdvancedTree.Header Footer
        {
            get
            {
                return this.m_rFooter;
            }
            set
            {
                if (value == null)
                {
                    value = new Resco.Controls.AdvancedTree.Header();
                }
                if (this.m_rFooter != value)
                {
                    this.m_rFooter.SetParentCollection(null);
                    this.m_rFooter = value;
                    this.m_rFooter.SetParentCollection(this.m_ncNodes);
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

        public int FooterHeight
        {
            get
            {
                if (this.m_bShowFooter)
                {
                    NodeTemplate template = this.m_tsCurrent[this.m_rFooter.TemplateIndex];
                    if (template != null)
                    {
                        return template.GetHeight(this.m_rFooter);
                    }
                }
                return 0;
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
                this.OnChanged(this, new TreeRepaintEventArgs());
            }
        }

        public Color GridColor
        {
            get
            {
                return this.m_penBorder.Color;
            }
            set
            {
                if (this.m_penBorder.Color != value)
                {
                    if (value.IsEmpty)
                    {
                        value = Color.DarkGray;
                    }
                    this.m_penBorder.Color = value;
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

        public bool GridLines
        {
            get
            {
                return this.m_bDrawGrid;
            }
            set
            {
                if (this.m_bDrawGrid != value)
                {
                    this.m_bDrawGrid = value;
                    this.OnChanged(this, new TreeRefreshEventArgs(true));
                }
            }
        }

        protected virtual int GridLinesWidth
        {
            get
            {
                if (!this.m_bDrawGrid)
                {
                    return 0;
                }
                return 1;
            }
        }

        public Resco.Controls.AdvancedTree.Header Header
        {
            get
            {
                return this.m_rHeader;
            }
            set
            {
                if (value == null)
                {
                    value = new Resco.Controls.AdvancedTree.Header();
                }
                if (this.m_rHeader != value)
                {
                    this.m_rHeader.SetParentCollection(null);
                    this.m_rHeader = value;
                    this.m_rHeader.SetParentCollection(this.m_ncNodes);
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

        public int HeaderHeight
        {
            get
            {
                if (this.m_bShowHeader)
                {
                    NodeTemplate template = this.m_tsCurrent[this.m_rHeader.TemplateIndex];
                    if (template != null)
                    {
                        return template.GetHeight(this.m_rHeader);
                    }
                }
                return 0;
            }
        }

        public bool KeyNavigation
        {
            get
            {
                return this.m_bKeyNavigation;
            }
            set
            {
                this.m_bKeyNavigation = value;
            }
        }

        public int LevelIndentation
        {
            get
            {
                return this.m_iLevelIndentation;
            }
            set
            {
                if (this.m_iLevelIndentation != value)
                {
                    this.m_iLevelIndentation = value;
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

        public bool MultiSelect
        {
            get
            {
                return this.m_bMultiSelect;
            }
            set
            {
                if (value != this.m_bMultiSelect)
                {
                    this.m_bMultiSelect = value;
                    this.m_ncNodes.ResetSelected();
                }
            }
        }

        protected internal int NodeIndent
        {
            get
            {
                if (this.m_iLevelIndentation >= 0)
                {
                    return this.m_iLevelIndentation;
                }
                return this.PlusMinusSize.Width;
            }
        }

        public NodeCollection Nodes
        {
            get
            {
                return this.m_ncNodes;
            }
        }

        public Size PlusMinusMargin
        {
            get
            {
                return this.m_szPlusMinusMargin;
            }
            set
            {
                if (this.m_szPlusMinusMargin != value)
                {
                    this.m_szPlusMinusMargin = value;
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

        public Size PlusMinusSize
        {
            get
            {
                return this.m_szPlusMinusSize;
            }
            set
            {
                if (this.m_szPlusMinusSize != value)
                {
                    this.m_szPlusMinusSize = value;
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

        public bool RightToLeft
        {
            get
            {
                return this.m_rightToLeft;
            }
            set
            {
                if (this.m_rightToLeft != value)
                {
                    this.m_rightToLeft = value;
                    this.OnChanged(this, new TreeRefreshEventArgs(true));
                }
            }
        }

        public Control ScrollBar
        {
            get
            {
                return this.m_vScrollBarResco;
            }
            set
            {
                if (this.m_vScrollBarResco != value)
                {
                    this.m_vScrollBarResco = value;
                    if (this.m_vScroll != null)
                    {
                        this.m_vScroll.Detach();
                        this.m_vScroll.ValueChanged -= new EventHandler(this.m_vScroll_ValueChanged);
                        this.m_vScroll.Resize -= new EventHandler(this.OnScrollResize);
                    }
                    this.m_vScroll = null;
                    this.OnChanged(this, new TreeRefreshEventArgs(true));
                }
            }
        }

        public bool ScrollbarVisible
        {
            get
            {
                return (this.ShowScrollbar && (this.m_vScrollWidth > 0));
            }
        }

        public int ScrollbarWidth
        {
            get
            {
                return this.m_iScrollWidth;
            }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                if (this.m_iScrollWidth != value)
                {
                    this.m_iScrollWidth = value;
                    if (this.m_vScrollWidth != 0)
                    {
                        this.m_vScrollWidth = value;
                        this.m_vScroll.Width = value;
                        this.m_vScroll.Left = base.Width - value;
                        this.OnChanged(this, TreeRepaintEventArgs.Empty);
                    }
                }
            }
        }

        public int SelectedCount
        {
            get
            {
                return this.m_countSelected;
            }
        }

        public Node SelectedNode
        {
            get
            {
                if (this.SelectedCount > 0)
                {
                    return this.GetSelectedNode(this.m_ncNodes);
                }
                return null;
            }
            set
            {
                bool flag = false;
                int i = -1;
                if (value != null)
                {
                    i = value.ParentCollection.IndexOf(value);
                }
                if (i >= 0)
                {
                    this.SuspendRedraw();
                    if (!value.Selected)
                    {
                        value.Selected = true;
                        flag = true;
                    }
                    int yoff = this.EnsureVisible(value, false);
                    this.ResumeRedraw();
                    if (flag)
                    {
                        this.OnAfterNodeSelect(new NodeEventArgs(value, i, yoff));
                    }
                    else
                    {
                        this.m_activeNode = value;
                    }
                }
                else if ((!this.m_bMultiSelect && (this.m_activeNode != null)) && this.m_activeNode.Selected)
                {
                    i = -1;
                    if (this.m_activeNode != null)
                    {
                        i = this.m_activeNode.ParentCollection.IndexOf(this.m_activeNode);
                    }
                    if (i < 0)
                    {
                        this.m_activeNode = null;
                    }
                    else
                    {
                        this.SuspendRedraw();
                        this.m_activeNode.Selected = false;
                        this.OnAfterNodeSelect(new NodeEventArgs(this.m_activeNode, i, -1));
                        this.ResumeRedraw();
                    }
                }
            }
        }

        public Node[] SelectedNodes
        {
            get
            {
                int selectedCount = this.SelectedCount;
                Node[] nodes = new Node[selectedCount];
                this.GetSelectedNodes(nodes, this.m_ncNodes, 0, selectedCount);
                return nodes;
            }
        }

        public Resco.Controls.AdvancedTree.SelectionMode SelectionMode
        {
            get
            {
                return this.m_mode;
            }
            set
            {
                this.m_mode = value;
            }
        }

        public bool ShowFooter
        {
            get
            {
                return this.m_bShowFooter;
            }
            set
            {
                if (this.m_bShowFooter != value)
                {
                    this.m_bShowFooter = value;
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

        public bool ShowHeader
        {
            get
            {
                return this.m_bShowHeader;
            }
            set
            {
                if (this.m_bShowHeader != value)
                {
                    this.m_bShowHeader = value;
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

        public bool ShowPlusMinus
        {
            get
            {
                return this.m_bShowPlusMinus;
            }
            set
            {
                if (this.m_bShowPlusMinus != value)
                {
                    this.m_bShowPlusMinus = value;
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }

        [DefaultValue(true)]
        public bool ShowScrollbar
        {
            get
            {
                return this.m_bShowScrollbar;
            }
            set
            {
                if (this.m_bShowScrollbar != value)
                {
                    this.m_bShowScrollbar = value;
                    this.OnChanged(this, new TreeRefreshEventArgs(true));
                }
            }
        }

        public TemplateSet Templates
        {
            get
            {
                return this.m_tsCurrent;
            }
            set
            {
                if (this.m_tsCurrent != value)
                {
                    this.m_tsCurrent.Changed -= new TreeChangedEventHandler(this.OnChanged);
                    this.m_tsCurrent = value;
                    this.m_tsCurrent.Parent = this;
                    this.m_tsCurrent.Changed += new TreeChangedEventHandler(this.OnChanged);
                    this.OnChanged(this, new TreeRefreshEventArgs(true));
                }
            }
        }

        [DefaultValue(false)]
        public bool TouchScrolling
        {
            get
            {
                return this.m_bEnableTouchScrolling;
            }
            set
            {
                this.m_bEnableTouchScrolling = value;
            }
        }

        [DefaultValue(8)]
        public int TouchSensitivity
        {
            get
            {
                return this.m_touchSensitivity;
            }
            set
            {
                this.m_touchSensitivity = value;
            }
        }

        public bool TreeNodeLines
        {
            get
            {
                return this.m_bTreeNodeLines;
            }
            set
            {
                if (this.m_bTreeNodeLines != value)
                {
                    this.m_bTreeNodeLines = value;
                    this.OnChanged(this, new TreeRepaintEventArgs());
                }
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
                    this.OnChanged(this, new TreeRepaintEventArgs());
                }
            }
        }

        protected System.Windows.Forms.VScrollBar VScrollBar
        {
            get
            {
                if (this.m_vScroll == null)
                {
                    return null;
                }
                return (System.Windows.Forms.VScrollBar) this.m_vScroll;
            }
        }

        private bool VScrollBarVisible
        {
            get
            {
                return this.m_vScrollVisible;
            }
            set
            {
                this.m_vScrollVisible = value;
                this.m_vScroll.Visible = value;
            }
        }

        public delegate void DesignTimeCallback(object o, object o2);

        internal class TooltipArea
        {
            public Rectangle Bounds;
            public string Text;

            public TooltipArea(Rectangle bounds, string text)
            {
                this.Text = text;
                this.Bounds = bounds;
            }
        }
    }

    internal class BigHas
    {
        // Fields
        internal static Dictionary<string, int> methodxxx;
    }
}

