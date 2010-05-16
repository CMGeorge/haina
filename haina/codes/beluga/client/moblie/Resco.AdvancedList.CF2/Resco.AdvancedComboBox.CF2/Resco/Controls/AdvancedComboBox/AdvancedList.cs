namespace Resco.Controls.AdvancedComboBox
{
    using Resco.Controls.AdvancedComboBox.Design;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class AdvancedList : UserControl
    {
        private Rectangle backRect = new Rectangle(0, 0, 100, 100);
        private bool bIsInScrollChange;
        private List<Rectangle> m_alButtons = new List<Rectangle>();
        private List<Rectangle> m_alLinks = new List<Rectangle>();
        private List<TooltipArea> m_alTooltips = new List<TooltipArea>();
        private Bitmap m_BackBuffer_Image;
        private SolidBrush m_BackColor;
        internal bool m_bDrawGrid;
        private bool m_bEnableTouchScrolling;
        private bool m_bForceRedraw;
        private bool m_bIsChange = true;
        private Pen m_BorderPen = new Pen(Color.Black);
        private SolidBrush m_brushKey;
        private bool m_bSendMouseEventToOwner;
        private bool m_bShowingContextMenu;
        private bool m_bShowingToolTip;
        private bool m_bShowScrollbar = true;
        private bool m_bTouchScrolling;
        internal Color m_colorKey;
        private System.Windows.Forms.ContextMenu m_ContextMenu;
        private Win32Native.WindowProcCallback m_delegate;
        private GCHandle m_dlgLock;
        private bool m_doubleBuffered = true;
        private bool m_enableHTCGSensor;
        private bool m_enableHTCNavSensor;
        private bool m_enableHTCNavSensorNavigation;
        private long m_gcCollect = DateTime.Now.Ticks;
        private GradientColor m_gradientBackColor;
        private Graphics m_grBackBuffer;
        private Graphics m_grGradient;
        private Graphics m_grTmp;
        private bool m_gyroScrolling;
        private double m_HTCActualRadial;
        private Timer m_HTCGyroNavigationTimer;
        private int m_HTCGyroNavRepeatDelay = 500;
        private int m_HTCGyroNavRepeatRate = 50;
        private Point m_HTCGyroOffset = new Point(0, 0);
        private Point m_HTCGyroSensitivity = new Point(200, 200);
        private HTCGSensor m_HTCGyroSensor;
        private bool m_HTCGyroSensorNavigation;
        private Point m_HTCGyroStatus = new Point(0, 0);
        private HTCGyroDirectionEventArgs m_HTCGytoEventArgs = new HTCGyroDirectionEventArgs(HTCDirection.None);
        private HTCNavSensor m_HTCNavSensor;
        private Timer m_HTCNavTimer;
        private Resco.Controls.AdvancedComboBox.HTCScreenOrientation m_HTCScreenOrientation = Resco.Controls.AdvancedComboBox.HTCScreenOrientation.Portrait;
        private int m_iActualItemIndex;
        private int m_iDocumentHeight;
        private ImageAttributes m_imgAttr;
        private Bitmap m_imgGradient;
        private int m_iNoRedraw;
        private int m_iScrollChange;
        private int m_iScrollWidth;
        private int m_iSelectedCellIndex;
        private int m_iTopmostItemOffset;
        private int m_iUpdateCounter = 1;
        private int m_iVScrollPrevValue;
        private Point m_MousePosition = new Point(0, 0);
        private Resco.Controls.AdvancedComboBox.AdvancedComboBox m_owner;
        private int m_pressedButtonItemIndex = -1;
        private HTCNavSensorRotatedEventArgs m_rotatedEventArgs = new HTCNavSensorRotatedEventArgs(0.0, 0.0);
        private IntPtr m_scrollWnd = IntPtr.Zero;
        private Timer m_Timer;
        private Bitmap m_tmpImage;
        private Resco.Controls.AdvancedComboBox.ToolTip m_ToolTip;
        private int m_TouchAutoScrollDiff;
        private Resco.Controls.AdvancedComboBox.TouchScrollDirection m_touchScrollDirection;
        private Timer m_TouchScrollingTimer;
        private int m_touchSensitivity;
        private uint m_TouchTime0;
        private uint m_TouchTime1;
        private bool m_useGradient;
        private System.Windows.Forms.VScrollBar m_vScroll;
        private bool m_vScrollVisible;
        private int m_vScrollWidth;
        private IntPtr m_wndproc = IntPtr.Zero;
        private IntPtr m_wndprocReal = IntPtr.Zero;
        public static int ScrollBottomOffset = -1;
        private static int ScrollSmallChange = 0x10;

        internal event ComboBoxEventHandler Changed;

        public event HTCGyroDirectionHandler HTCGyroDirection;

        public event HTCNavSensorRotatedHandler HTCNavSensorRotated;

        public event HTCOrientationChangedHandler HTCOrientationChanged;

        internal event ItemEnteredEventHandler ItemEntered;

        internal event ItemEventHandler ItemSelect;

        internal event EventHandler Scroll;

        internal AdvancedList(Resco.Controls.AdvancedComboBox.AdvancedComboBox owner)
        {
            this.m_owner = owner;
            base.Visible = false;
            this.AutoScroll = false;
            this.m_grBackBuffer = null;
            this.m_BackBuffer_Image = null;
            this.m_grTmp = null;
            this.m_tmpImage = null;
            this.m_colorKey = Color.FromArgb(0xff, 0, 0xff);
            this.m_brushKey = new SolidBrush(this.m_colorKey);
            this.m_imgAttr = new ImageAttributes();
            this.m_imgAttr.SetColorKey(this.m_colorKey, this.m_colorKey);
            this.UpdateDoubleBuffering();
            base.BackColor = SystemColors.ControlLight;
            this.m_BackColor = new SolidBrush(this.BackColor);
            this.m_vScrollWidth = 0;
            this.m_iScrollWidth = 13;
            this.ItemSelect = null;
            this.Scroll = null;
            this.m_iActualItemIndex = 0;
            this.m_iTopmostItemOffset = 0;
            this.m_iDocumentHeight = 0;
            this.m_iVScrollPrevValue = 0;
            this.m_ToolTip = null;
            this.m_Timer = new Timer();
            this.m_Timer.Enabled = false;
            this.m_Timer.Interval = 500;
            this.m_Timer.Tick += new EventHandler(this.OnTimerTick);
            this.m_bShowingToolTip = false;
            this.m_iSelectedCellIndex = -1;
            this.m_TouchScrollingTimer = new Timer();
            this.m_TouchScrollingTimer.Enabled = false;
            this.m_TouchScrollingTimer.Interval = 50;
            this.m_TouchScrollingTimer.Tick += new EventHandler(this.OnTouchScrollingTimerTick);
            this.m_bTouchScrolling = false;
            this.m_bEnableTouchScrolling = false;
            this.m_TouchAutoScrollDiff = 0;
            this.m_touchSensitivity = 8;
            this.m_touchScrollDirection = Resco.Controls.AdvancedComboBox.TouchScrollDirection.Inverse;
            this.m_gradientBackColor = new GradientColor(FillDirection.Horizontal);
            this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
            this.m_useGradient = false;
            this.m_imgGradient = null;
            this.m_grGradient = null;
        }

        internal void ActivateLink(int itemIndex, int cellIndex)
        {
            int itemOffset = this.GetItemOffset(itemIndex);
            switch (itemOffset)
            {
                case -2147483648:
                case 0x7fffffff:
                    return;
            }
            Resco.Controls.AdvancedComboBox.ListItem item = this.m_owner.Items[itemIndex];
            ItemTemplate template = item.GetTemplate(this.m_owner.Templates);
            if ((cellIndex >= 0) && (cellIndex < template.CellTemplates.Count))
            {
                LinkCell c = template.CellTemplates[cellIndex] as LinkCell;
                if (c != null)
                {
                    this.m_owner.OnLink(c, item, new Point(itemIndex, cellIndex), itemOffset, base.ClientSize.Width - this.ClientScrollbarWidth);
                }
            }
        }

        internal void AddButtonArea(Rectangle bounds)
        {
            if (!this.m_alButtons.Contains(bounds))
            {
                this.m_alButtons.Add(bounds);
            }
        }

        internal void AddLinkArea(Rectangle bounds)
        {
            if (!this.m_alLinks.Contains(bounds))
            {
                this.m_alLinks.Add(bounds);
            }
        }

        internal void AddTooltipArea(Rectangle bounds, string text)
        {
            this.m_alTooltips.Add(new TooltipArea(bounds, text));
        }

        internal void BeginUpdate()
        {
            this.m_iUpdateCounter++;
        }

        private void CalculateFirstItem(int iOffset)
        {
            int gridLinesWidth = this.GridLinesWidth;
            this.m_iTopmostItemOffset += iOffset;
            if (iOffset <= 0)
            {
                int iTopmostItemOffset = this.m_iTopmostItemOffset;
                for (int i = this.m_iActualItemIndex; i < this.m_owner.Items.Count; i++)
                {
                    int height = this.m_owner.Items.GetHeight(i, this.m_owner.Templates);
                    if (Math.Abs(iTopmostItemOffset) < height)
                    {
                        this.m_iActualItemIndex = i;
                        this.m_iTopmostItemOffset = iTopmostItemOffset;
                        return;
                    }
                    iTopmostItemOffset += height;
                }
            }
            else
            {
                for (int j = this.m_iActualItemIndex; j >= 0; j--)
                {
                    if (this.m_iTopmostItemOffset > 0)
                    {
                        if (j == 0)
                        {
                            this.EnsureVisible(0);
                            return;
                        }
                        int num5 = this.m_owner.Items.GetHeight(j - 1, this.m_owner.Templates);
                        this.m_iTopmostItemOffset -= num5;
                    }
                    else
                    {
                        int num6 = this.m_owner.Items.GetHeight(j, this.m_owner.Templates);
                        if (Math.Abs(this.m_iTopmostItemOffset) < num6)
                        {
                            this.m_iActualItemIndex = j;
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

        internal void Close()
        {
            base.Visible = false;
            base.Capture = false;
            this.m_owner.Focus();
        }

        protected override void Dispose(bool disposing)
        {
            if (this.m_TouchScrollingTimer != null)
            {
                this.m_TouchScrollingTimer.Dispose();
                this.m_TouchScrollingTimer = null;
            }
            if (this.m_BackBuffer_Image != null)
            {
                this.m_BackBuffer_Image.Dispose();
                this.m_BackBuffer_Image = null;
            }
            if (this.m_grBackBuffer != null)
            {
                this.m_grBackBuffer.Dispose();
                this.m_grBackBuffer = null;
            }
            if (this.m_tmpImage != null)
            {
                this.m_tmpImage.Dispose();
                this.m_tmpImage = null;
            }
            if (this.m_grTmp != null)
            {
                this.m_grTmp.Dispose();
                this.m_grTmp = null;
            }
            if (this.m_imgGradient != null)
            {
                this.m_imgGradient.Dispose();
                this.m_imgGradient = null;
            }
            if (this.m_grGradient != null)
            {
                this.m_grGradient.Dispose();
                this.m_grGradient = null;
            }
            this.m_gradientBackColor = null;
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
            this.UnhackScrollbar();
            base.Dispose(disposing);
        }

        internal bool EndUpdate()
        {
            if (this.m_iUpdateCounter > 0)
            {
                this.m_iUpdateCounter--;
            }
            else
            {
                return false;
            }
            if (this.m_iUpdateCounter == 0)
            {
                this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true));
            }
            return true;
        }

        public int EnsureVisible(Resco.Controls.AdvancedComboBox.ListItem item)
        {
            int index = this.m_owner.Items.IndexOf(item);
            return this.EnsureVisible(index, false);
        }

        public int EnsureVisible(int ix)
        {
            return this.EnsureVisible(ix, false);
        }

        public int EnsureVisible(Resco.Controls.AdvancedComboBox.ListItem item, bool bTop)
        {
            int index = this.m_owner.Items.IndexOf(item);
            return this.EnsureVisible(index, bTop);
        }

        public int EnsureVisible(int ix, bool bTop)
        {
            return this.EnsureVisible(ix, -1, bTop);
        }

        public int EnsureVisible(int ix, int cx, bool bTop)
        {
            if (ix < 0)
            {
                return 0;
            }
            int num = 0;
            if (!bTop && this.IsVisible(ix))
            {
                for (int i = 0; i < ix; i++)
                {
                    num += this.m_owner.Items.GetHeight(i, this.m_owner.Templates);
                }
                return num;
            }
            int offset = 0;
            if (!bTop && (ix > this.m_iActualItemIndex))
            {
                offset = base.ClientSize.Height - this.m_owner.Items[ix].GetHeight(this.m_owner.Templates);
                if (offset < 0)
                {
                    offset = 0;
                }
            }
            return this.SetScrollPos(ix, cx, offset);
        }

        internal CellEventArgs GetCellAtPoint(Point pt)
        {
            int num;
            Resco.Controls.AdvancedComboBox.ListItem i = null;
            Point point = this.m_owner.Items.GetItemClick(this.m_iActualItemIndex, this.m_iTopmostItemOffset, pt.X, pt.Y, out num);
            if (point.X >= 0)
            {
                i = this.m_owner.Items[point.X];
            }
            if (i != null)
            {
                return new CellEventArgs(i, (point.Y >= 0) ? i.GetTemplate(this.m_owner.Templates)[point.Y] : null, point.X, point.Y, num);
            }
            return null;
        }

        private int GetItemOffset(int itemIndex)
        {
            if ((itemIndex < 0) || (itemIndex >= this.m_owner.Items.Count))
            {
                return -2147483648;
            }
            if (itemIndex < this.m_iActualItemIndex)
            {
                return -2147483648;
            }
            if (itemIndex == this.m_iActualItemIndex)
            {
                return this.m_iTopmostItemOffset;
            }
            int iTopmostItemOffset = this.m_iTopmostItemOffset;
            int height = base.Height;
            for (int i = this.m_iActualItemIndex; i < itemIndex; i++)
            {
                Resco.Controls.AdvancedComboBox.ListItem item = this.m_owner.Items[i];
                int num4 = item.GetTemplate(this.m_owner.Templates).GetHeight(item);
                iTopmostItemOffset += num4;
                if (this.m_bDrawGrid)
                {
                    iTopmostItemOffset++;
                }
                if (iTopmostItemOffset > height)
                {
                    return 0x7fffffff;
                }
            }
            return iTopmostItemOffset;
        }

        [DllImport("coredll.dll")]
        private static extern uint GetTickCount();
        private uint GetTicks()
        {
            try
            {
                return GetTickCount();
            }
            catch
            {
                return (uint) (DateTime.Now.Ticks / 0x2710L);
            }
        }

        private void HackScrollbar()
        {
            if (this.m_wndproc == IntPtr.Zero)
            {
                this.m_delegate = new Win32Native.WindowProcCallback(this.ScrollBarWindowProc);
                this.m_wndproc = Marshal.GetFunctionPointerForDelegate(this.m_delegate);
                this.m_dlgLock = GCHandle.Alloc(this.m_delegate, GCHandleType.Weak);
            }
            this.m_scrollWnd = this.m_vScroll.Handle;
            this.m_wndprocReal = Win32Native.SetWindowLong(this.m_scrollWnd, -4, this.m_wndproc);
        }

        private void HandleContextMenu(Point pos)
        {
            if (this.ContextMenu != null)
            {
                CellEventArgs cellAtPoint = this.GetCellAtPoint(pos);
                this.HandleItemSelection(cellAtPoint, true);
                this.m_bShowingContextMenu = true;
                this.ContextMenu.Show(this, pos);
            }
        }

        private void HandleItemSelection(CellEventArgs cea, bool bDisableItemEntered)
        {
            Resco.Controls.AdvancedComboBox.ListItem listItem = cea.ListItem;
            int currentTemplateIndex = listItem.CurrentTemplateIndex;
            this.SelectedItem = listItem;
            if (currentTemplateIndex == listItem.CurrentTemplateIndex)
            {
                if (cea.CellIndex >= 0)
                {
                    this.m_owner.OnCellClick(cea);
                }
                this.m_owner.OnItemEntered(new ItemEnteredEventArgs(cea.ListItem, cea.ItemIndex));
                if (((cea.CellIndex >= 0) && cea.Cell.Selectable) && (listItem != null))
                {
                    this.SelectedCell = cea.Cell;
                    this.m_owner.OnCellEntered(new CellEnteredMainEventArgs(cea.Cell, cea.CellIndex, cea.ListItem), this.m_owner);
                }
                else
                {
                    this.SelectedCell = null;
                }
            }
            this.m_owner.Close(cea.ItemIndex, cea.CellIndex);
        }

        internal void HandleKeyDown(KeyEventArgs e)
        {
            int selectedItemIndex = this.SelectedItemIndex;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (!this.m_owner.RightToLeft)
                    {
                        this.PreviousSelectableCell();
                        break;
                    }
                    this.NextSelectableCell();
                    break;

                case Keys.Up:
                    if (this.SelectedItemIndex > 0)
                    {
                        this.SelectedItemIndex--;
                    }
                    break;

                case Keys.Right:
                    if (!this.m_owner.RightToLeft)
                    {
                        this.NextSelectableCell();
                        break;
                    }
                    this.PreviousSelectableCell();
                    break;

                case Keys.Down:
                    this.SelectedItemIndex++;
                    break;

                case Keys.Return:
                    if ((this.m_owner.Items != null) && (this.SelectedCell != null))
                    {
                        ItemTemplate.CellCollection cellTemplates = this.SelectedItem.GetTemplate(this.m_owner.Templates).CellTemplates;
                        if ((this.SelectedCell is ButtonCell) && this.SelectedCell.Selectable)
                        {
                            this.m_pressedButtonItemIndex = this.SelectedItemIndex;
                            this.SelectedItem.PressedButtonIndex = this.m_iSelectedCellIndex;
                            this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.List);
                        }
                    }
                    break;
            }
            base.OnKeyDown(e);
        }

        internal void HandleKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        internal void HandleKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                bool flag = true;
                if (!(this.SelectedCell is ButtonCell))
                {
                    this.m_owner.OnItemEntered(new ItemEnteredEventArgs(this.SelectedItem, this.SelectedItemIndex));
                }
                if ((this.m_owner.Items != null) && (this.SelectedCell != null))
                {
                    ItemTemplate.CellCollection cellTemplates = this.SelectedItem.GetTemplate(this.m_owner.Templates).CellTemplates;
                    if (this.SelectedCell.Selectable && !(this.SelectedCell is ButtonCell))
                    {
                        int index = cellTemplates.IndexOf(this.SelectedCell);
                        this.m_owner.OnCellEntered(new CellEnteredMainEventArgs(this.SelectedCell, index, this.SelectedItem), this.m_owner);
                        flag = false;
                    }
                    if (this.SelectedCell is ButtonCell)
                    {
                        int pressedButtonIndex = this.SelectedItem.PressedButtonIndex;
                        this.m_pressedButtonItemIndex = -1;
                        this.SelectedItem.PressedButtonIndex = -1;
                        this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.List);
                        this.m_owner.OnButton((ButtonCell) this.SelectedCell, this.SelectedItem, new Point(this.SelectedItemIndex, pressedButtonIndex), 0);
                        flag = false;
                    }
                }
                if (flag)
                {
                    this.m_owner.Close(this.SelectedItemIndex, -1);
                }
            }
            base.OnKeyUp(e);
        }

        private void IniHTCNavSensor()
        {
            if (((this.Site == null) || !this.Site.DesignMode) && ((this.m_HTCNavSensor == null) && (base.TopLevelControl != null)))
            {
                this.m_HTCNavSensor = new HTCNavSensor(base.TopLevelControl);
                this.m_HTCNavSensor.Rotated += new NavSensorMoveHandler(this.m_HTCNavSensor_Rotated);
            }
        }

        private void InitHTCGSensor()
        {
            if ((this.Site == null) || !this.Site.DesignMode)
            {
                if (this.m_HTCGyroSensor == null)
                {
                    this.m_HTCGyroSensor = new HTCGSensor();
                }
                if (this.m_HTCGyroNavigationTimer == null)
                {
                    this.m_HTCGyroNavigationTimer = new Timer();
                    this.m_HTCGyroNavigationTimer.Enabled = false;
                    this.m_HTCGyroNavigationTimer.Interval = 50;
                    this.m_HTCGyroNavigationTimer.Tick += new EventHandler(this.OnGyroNavigationTimerTick);
                }
            }
        }

        public bool IsVisible(Resco.Controls.AdvancedComboBox.ListItem item)
        {
            return this.IsVisible(this.m_owner.Items.IndexOf(item));
        }

        public bool IsVisible(int ix)
        {
            if (ix < this.m_iActualItemIndex)
            {
                return false;
            }
            if (ix == this.m_iActualItemIndex)
            {
                return (this.m_iTopmostItemOffset == 0);
            }
            if (this.m_owner.Items.LastDrawnItem == 0)
            {
                int iTopmostItemOffset = this.m_iTopmostItemOffset;
                int height = base.Height;
                this.m_owner.Items.LastDrawnItem = this.m_iActualItemIndex;
                while (this.m_owner.Items.LastDrawnItem < this.m_owner.Items.Count)
                {
                    Resco.Controls.AdvancedComboBox.ListItem item = this.m_owner.Items[this.m_owner.Items.LastDrawnItem];
                    int num3 = item.GetTemplate(this.m_owner.Templates).GetHeight(item);
                    iTopmostItemOffset += num3;
                    if (this.m_bDrawGrid)
                    {
                        iTopmostItemOffset++;
                    }
                    if (iTopmostItemOffset > height)
                    {
                        break;
                    }
                    ItemCollection items = this.m_owner.Items;
                    items.LastDrawnItem++;
                }
            }
            return (ix < this.m_owner.Items.LastDrawnItem);
        }

        private void m_gradientBackColor_PropertyChanged(object sender, EventArgs e)
        {
            this.m_imgGradient = null;
            this.m_grGradient = null;
            this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.List);
        }

        private void m_HTCNavSensor_Rotated(double rotationsPerSecond, double radialDelta)
        {
            if (this.m_enableHTCNavSensor)
            {
                this.m_rotatedEventArgs.RotationsPerSecond = rotationsPerSecond;
                this.m_rotatedEventArgs.RadialDelta = radialDelta;
                this.OnHTCNavSensorRotated(this.m_rotatedEventArgs);
            }
        }

        private void m_HTCNavTimer_Tick(object sender, EventArgs e)
        {
            if (this.m_enableHTCNavSensor && (base.TopLevelControl != null))
            {
                try
                {
                    this.IniHTCNavSensor();
                }
                catch
                {
                    this.m_enableHTCNavSensor = false;
                }
                if (this.m_HTCNavTimer != null)
                {
                    this.m_HTCNavTimer.Enabled = false;
                }
            }
        }

        private void m_vScroll_ValueChanged(object sender, EventArgs e)
        {
            this.bIsInScrollChange = true;
            this.OnChange(this, ComboBoxEventArgsType.VScroll, new ComboBoxScrollArgs(this.m_iVScrollPrevValue - this.m_vScroll.Value));
            this.OnScroll();
            if (this.m_owner.DelayLoad && (this.m_vScroll.Value > (this.m_vScroll.Maximum - (2 * this.m_vScroll.LargeChange))))
            {
                this.m_owner.DoDelayLoad();
            }
            this.bIsInScrollChange = false;
        }

        public bool NextSelectableCell()
        {
            if (this.SelectedItemIndex >= 0)
            {
                int index;
                ItemTemplate.CellCollection cellTemplates = this.SelectedItem.GetTemplate(this.m_owner.Templates).CellTemplates;
                if (this.SelectedCell != null)
                {
                    index = cellTemplates.IndexOf(this.SelectedCell);
                }
                else
                {
                    index = -1;
                }
                while (++index < cellTemplates.Count)
                {
                    if (cellTemplates[index].Selectable)
                    {
                        this.SelectedCell = cellTemplates[index];
                        return true;
                    }
                }
                this.SelectedCell = null;
            }
            return false;
        }

        internal void OnAutoHeightChanged(int nItemIndex, int iDiff)
        {
            this.m_bIsChange = true;
            if (this.m_iUpdateCounter <= 0)
            {
                bool flag = false;
                int iOffset = 0;
                bool flag2 = false;
                this.m_iDocumentHeight += iDiff;
                if (nItemIndex < this.m_iActualItemIndex)
                {
                    this.m_iVScrollPrevValue += iDiff;
                    this.m_vScroll.Value += iDiff;
                }
                if (this.SetVScrollBar(this.DocumentHeight))
                {
                    flag = true;
                    iOffset = this.m_iVScrollPrevValue - this.m_vScroll.Value;
                    flag2 = true;
                }
                if (flag)
                {
                    this.CalculateFirstItem(iOffset);
                    this.m_iVScrollPrevValue = this.m_vScroll.Value;
                }
                if (flag2)
                {
                    iOffset = 0;
                }
                this.SetRedraw(iOffset);
                base.Invalidate();
            }
        }

        private void OnChange(object sender, ComboBoxEventArgsType e, ComboBoxArgs args)
        {
            if (this.Changed != null)
            {
                this.Changed(sender, e, args);
            }
        }

        internal void OnClear(object sender)
        {
            this.m_iActualItemIndex = 0;
            this.m_iTopmostItemOffset = 0;
            this.m_iVScrollPrevValue = 0;
            if (this.m_vScroll != null)
            {
                this.m_vScrollWidth = 0;
                this.VScrollBarVisible = false;
            }
            this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true, ComboBoxUpdateRange.List));
        }

        protected override void OnClick(EventArgs e)
        {
            if (!this.m_bSendMouseEventToOwner)
            {
                this.m_Timer.Enabled = false;
                if ((!this.m_bShowingToolTip && !this.m_bTouchScrolling) && !this.m_bShowingContextMenu)
                {
                    base.OnClick(e);
                    Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    CellEventArgs cellAtPoint = this.GetCellAtPoint(pt);
                    if (cellAtPoint != null)
                    {
                        if (cellAtPoint.Cell != null)
                        {
                            if (this.CheckForLink(pt))
                            {
                                LinkCell c = cellAtPoint.Cell as LinkCell;
                                if (c != null)
                                {
                                    this.m_owner.OnLink(c, cellAtPoint.ListItem, new Point(cellAtPoint.ItemIndex, cellAtPoint.CellIndex), cellAtPoint.Offset, base.ClientSize.Width - this.ClientScrollbarWidth);
                                    this.m_bIsChange = true;
                                }
                                return;
                            }
                            if (this.CheckForButton(pt))
                            {
                                return;
                            }
                        }
                        this.HandleItemSelection(cellAtPoint, false);
                    }
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (this.m_owner.AutoHideDropDownList)
            {
                base.Capture = true;
            }
            base.OnGotFocus(e);
        }

        private void OnGyroNavigationTimerTick(object sender, EventArgs e)
        {
            if (this.m_HTCGyroSensor != null)
            {
                HTCGSensorData rawSensorData = this.m_HTCGyroSensor.GetRawSensorData();
                this.m_HTCGyroStatus.X = rawSensorData.TiltX;
                this.m_HTCGyroStatus.Y = rawSensorData.TiltY;
                int hTCScreenOrientation = (int) this.m_HTCScreenOrientation;
                this.m_HTCScreenOrientation = (Resco.Controls.AdvancedComboBox.HTCScreenOrientation) rawSensorData.ScreenOrientation;
                if (hTCScreenOrientation != rawSensorData.ScreenOrientation)
                {
                    this.OnHTCOrientationChanged(EventArgs.Empty);
                }
                int num2 = 0;
                int num3 = 0;
                switch (Utility.ScreenOrientation)
                {
                    case 1:
                        num2 = -(rawSensorData.TiltY - this.m_HTCGyroOffset.Y);
                        num3 = -(rawSensorData.TiltX - this.m_HTCGyroOffset.X);
                        break;

                    case 2:
                        num2 = rawSensorData.TiltX - this.m_HTCGyroOffset.X;
                        num3 = rawSensorData.TiltY - this.m_HTCGyroOffset.Y;
                        break;

                    case 4:
                        num2 = -(rawSensorData.TiltX - this.m_HTCGyroOffset.X);
                        num3 = -(rawSensorData.TiltY - this.m_HTCGyroOffset.Y);
                        break;

                    default:
                        num2 = rawSensorData.TiltY - this.m_HTCGyroOffset.Y;
                        num3 = rawSensorData.TiltX - this.m_HTCGyroOffset.X;
                        break;
                }
                bool hTCGyroSensorNavigation = this.m_HTCGyroSensorNavigation;
                this.m_HTCGytoEventArgs.Direction = HTCDirection.None;
                bool gyroScrolling = this.m_gyroScrolling;
                this.m_gyroScrolling = false;
                if (num2 > this.m_HTCGyroSensitivity.Y)
                {
                    if (hTCGyroSensorNavigation)
                    {
                        this.SelectedItemIndex++;
                    }
                    this.m_gyroScrolling = true;
                    this.m_HTCGytoEventArgs.Direction |= HTCDirection.Down;
                }
                if (num2 < -this.m_HTCGyroSensitivity.Y)
                {
                    if (hTCGyroSensorNavigation && (this.SelectedItemIndex > 0))
                    {
                        this.SelectedItemIndex--;
                    }
                    this.m_gyroScrolling = true;
                    this.m_HTCGytoEventArgs.Direction |= HTCDirection.Up;
                }
                if (num3 > this.m_HTCGyroSensitivity.X)
                {
                    if (hTCGyroSensorNavigation)
                    {
                        if (this.m_owner.RightToLeft)
                        {
                            this.PreviousSelectableCell();
                        }
                        else
                        {
                            this.NextSelectableCell();
                        }
                    }
                    this.m_gyroScrolling = true;
                    this.m_HTCGytoEventArgs.Direction |= HTCDirection.Right;
                }
                if (num3 < -this.m_HTCGyroSensitivity.X)
                {
                    if (hTCGyroSensorNavigation)
                    {
                        if (this.m_owner.RightToLeft)
                        {
                            this.NextSelectableCell();
                        }
                        else
                        {
                            this.PreviousSelectableCell();
                        }
                    }
                    this.m_gyroScrolling = true;
                    this.m_HTCGytoEventArgs.Direction |= HTCDirection.Left;
                }
                this.OnHTCGyroDirection(this.m_HTCGytoEventArgs);
                if (this.m_gyroScrolling)
                {
                    if (!gyroScrolling)
                    {
                        this.m_HTCGyroNavigationTimer.Interval = this.m_HTCGyroNavRepeatDelay;
                    }
                    else
                    {
                        this.m_HTCGyroNavigationTimer.Interval = this.m_HTCGyroNavRepeatRate;
                    }
                }
                else
                {
                    this.m_HTCGyroNavigationTimer.Interval = 50;
                }
            }
        }

        protected virtual void OnHTCGyroDirection(HTCGyroDirectionEventArgs e)
        {
            if (this.HTCGyroDirection != null)
            {
                this.HTCGyroDirection(this, e);
            }
        }

        protected virtual void OnHTCNavSensorRotated(HTCNavSensorRotatedEventArgs e)
        {
            if (this.m_enableHTCNavSensorNavigation)
            {
                this.m_HTCActualRadial += e.RadialDelta;
                if (Math.Abs(this.m_HTCActualRadial) > 0.1)
                {
                    if (e.RadialDelta > 0.0)
                    {
                        this.SelectedItemIndex++;
                    }
                    else if (this.SelectedItemIndex > 0)
                    {
                        this.SelectedItemIndex--;
                    }
                }
                this.m_HTCActualRadial = this.m_HTCActualRadial % 0.1;
            }
            if (this.HTCNavSensorRotated != null)
            {
                this.HTCNavSensorRotated(this, e);
            }
        }

        protected virtual void OnHTCOrientationChanged(EventArgs e)
        {
            if (this.HTCOrientationChanged != null)
            {
                this.HTCOrientationChanged(this, e);
            }
        }

        internal int OnItemAdded(Resco.Controls.AdvancedComboBox.ListItem item, int index)
        {
            int height = item.GetTemplate(this.m_owner.Templates).GetHeight(item);
            if (this.m_bDrawGrid)
            {
                height++;
            }
            if (index < this.m_iActualItemIndex)
            {
                this.m_iActualItemIndex++;
            }
            this.OnChange(this, ComboBoxEventArgsType.Resize, new ComboBoxScrollArgs(height));
            return height;
        }

        internal int OnItemRemoved(Resco.Controls.AdvancedComboBox.ListItem item, int index)
        {
            int d = -item.GetTemplate(this.m_owner.Templates).GetHeight(item);
            if (this.m_bDrawGrid)
            {
                d--;
            }
            if (index == this.m_iActualItemIndex)
            {
                this.EnsureVisible(this.m_iActualItemIndex);
            }
            if (index < this.m_iActualItemIndex)
            {
                this.m_iActualItemIndex--;
            }
            this.OnChange(this, ComboBoxEventArgsType.Resize, new ComboBoxScrollArgs(d));
            return d;
        }

        protected void OnItemSelect(Resco.Controls.AdvancedComboBox.ListItem item, int index)
        {
            int yoff = this.EnsureVisible(index, false);
            if ((this.ItemSelect != null) && item.Selected)
            {
                this.ItemSelect(this, new ItemEventArgs(item, index, yoff));
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            this.m_TouchScrollingTimer.Enabled = false;
            this.m_owner.RaiseOnKeyDown(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            this.m_owner.RaiseOnKeyPress(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            this.m_owner.RaiseOnKeyUp(e);
        }

        internal void OnListChange(object sender, ComboBoxEventArgsType e, ComboBoxArgs args)
        {
            this.m_bIsChange = true;
            if (this.m_iUpdateCounter <= 0)
            {
                bool flag = false;
                int iOffset = 0;
                bool flag2 = false;
                switch (e)
                {
                    case ComboBoxEventArgsType.Resize:
                    {
                        ComboBoxScrollArgs args3 = args as ComboBoxScrollArgs;
                        this.m_iDocumentHeight += args3.Dif;
                        if ((args3.Index < this.m_iActualItemIndex) && (this.m_vScroll != null))
                        {
                            this.m_iVScrollPrevValue += args3.Dif;
                            this.m_vScroll.Value += args3.Dif;
                        }
                        if (this.SetVScrollBar(this.DocumentHeight))
                        {
                            flag = true;
                            iOffset = this.m_iVScrollPrevValue - this.m_vScroll.Value;
                            flag2 = true;
                        }
                        this.m_owner.Items.LastDrawnItem = 0;
                        break;
                    }
                    case ComboBoxEventArgsType.Refresh:
                    {
                        ComboBoxRefreshArgs args2 = args as ComboBoxRefreshArgs;
                        if (this.m_owner.Items != null)
                        {
                            if ((args2 != null) && args2.ResetBounds)
                            {
                                this.m_owner.Items.ResetCachedBounds(args2.Template);
                            }
                            this.m_iDocumentHeight = this.m_owner.Items.CalculateRowsHeight();
                            if (this.SetVScrollBar(this.DocumentHeight))
                            {
                                this.m_owner.Items.ResetCachedBounds((args2 == null) ? null : args2.Template);
                                this.m_iDocumentHeight = this.m_owner.Items.CalculateRowsHeight();
                                this.SetVScrollBar(this.DocumentHeight);
                            }
                        }
                        if (this.m_iScrollChange == 0)
                        {
                            this.m_owner.Items.LastDrawnItem = 0;
                        }
                        this.SetScrollPos(this.m_iActualItemIndex, -1, this.m_iTopmostItemOffset);
                        break;
                    }
                    case ComboBoxEventArgsType.VScroll:
                    {
                        ComboBoxScrollArgs args4 = args as ComboBoxScrollArgs;
                        flag = true;
                        iOffset = args4.Dif;
                        break;
                    }
                    default:
                        this.SetVScrollBarBounds();
                        break;
                }
                if (flag)
                {
                    this.CalculateFirstItem(iOffset);
                    this.m_iVScrollPrevValue = this.m_vScroll.Value;
                }
                if (flag2)
                {
                    iOffset = 0;
                }
                this.SetRedraw(iOffset);
                base.Invalidate();
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.Capture = false;
            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            Point pt = base.PointToScreen(p);
            if (!base.RectangleToScreen(base.ClientRectangle).Contains(pt))
            {
                if (base.TopLevelControl.RectangleToScreen(base.TopLevelControl.ClientRectangle).Contains(pt))
                {
                    Point point3 = this.m_owner.PointToClient(pt);
                    if (this.m_owner.ClientRectangle.Contains(point3))
                    {
                        Win32Native.SendMouseDown(this.m_owner.Handle, point3.X, point3.Y);
                        this.m_bSendMouseEventToOwner = true;
                    }
                    else
                    {
                        this.m_owner.Close(-1, -1);
                    }
                }
            }
            else if (this.ScrollbarVisible && this.m_vScroll.RectangleToScreen(this.m_vScroll.ClientRectangle).Contains(pt))
            {
                pt = this.m_vScroll.PointToClient(pt);
                base.Capture = false;
                Win32Native.SendMouseDown(this.m_vScroll.Handle, pt.X, pt.Y);
            }
            else
            {
                base.OnMouseDown(e);
                this.m_bShowingContextMenu = false;
                this.m_MousePosition.X = e.X;
                this.m_MousePosition.Y = e.Y;
                if (this.m_bEnableTouchScrolling && (e.Button == MouseButtons.Left))
                {
                    this.m_TouchScrollingTimer.Enabled = false;
                    this.m_TouchAutoScrollDiff = 0;
                    this.m_TouchTime0 = this.GetTicks();
                    this.m_TouchTime1 = this.m_TouchTime0;
                }
                Point point4 = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                CellEventArgs cellAtPoint = this.GetCellAtPoint(point4);
                if ((cellAtPoint != null) && this.CheckForButton(point4))
                {
                    if (cellAtPoint.Cell != null)
                    {
                        this.m_pressedButtonItemIndex = cellAtPoint.ItemIndex;
                        cellAtPoint.ListItem.PressedButtonIndex = cellAtPoint.CellIndex;
                        this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.List);
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
                    }
                    catch
                    {
                        this.m_Timer.Enabled = true;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if ((this.m_bEnableTouchScrolling && (e.Button == MouseButtons.Left)) && (e.Y != this.m_MousePosition.Y))
            {
                int num = e.Y - this.m_MousePosition.Y;
                this.m_TouchAutoScrollDiff += num;
                if ((this.GetTicks() - this.m_TouchTime1) > 100)
                {
                    this.m_TouchAutoScrollDiff = 0;
                    this.m_TouchTime0 = this.GetTicks();
                    this.m_TouchTime1 = this.m_TouchTime0;
                }
                else
                {
                    this.m_TouchTime1 = this.GetTicks();
                }
                float num2 = (this.m_grBackBuffer == null) ? 1f : (this.m_grBackBuffer.DpiY / 96f);
                if (!this.m_bShowingToolTip && (this.m_bTouchScrolling || (Math.Abs(num) >= ((int) (this.m_touchSensitivity * num2)))))
                {
                    this.m_Timer.Enabled = false;
                    this.m_bTouchScrolling = true;
                    this.m_MousePosition.X = e.X;
                    this.m_MousePosition.Y = e.Y;
                    if (this.m_vScroll != null)
                    {
                        int num3 = this.m_vScroll.Value;
                        int num4 = (this.m_vScroll.Maximum - this.m_vScroll.LargeChange) + 1;
                        if (this.m_touchScrollDirection == Resco.Controls.AdvancedComboBox.TouchScrollDirection.Inverse)
                        {
                            num3 -= num;
                        }
                        else
                        {
                            num3 += num;
                        }
                        if (num3 < 0)
                        {
                            this.m_vScroll.Value = 0;
                        }
                        else if (num3 > num4)
                        {
                            this.m_vScroll.Value = num4;
                        }
                        else
                        {
                            this.m_vScroll.Value = num3;
                        }
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.m_bSendMouseEventToOwner)
            {
                Point p = new Point(e.X, e.Y);
                Point pt = this.m_owner.PointToClient(base.PointToScreen(p));
                if (this.m_owner.ClientRectangle.Contains(pt))
                {
                    Win32Native.SendMouseUp(this.m_owner.Handle, pt.X, pt.Y);
                }
                this.m_bSendMouseEventToOwner = false;
            }
            else
            {
                try
                {
                    base.OnMouseUp(e);
                    this.m_bShowingContextMenu = false;
                    if (this.m_Timer != null)
                    {
                        this.m_Timer.Enabled = false;
                    }
                    if (this.m_pressedButtonItemIndex >= 0)
                    {
                        Resco.Controls.AdvancedComboBox.ListItem item = this.m_owner.Items[this.m_pressedButtonItemIndex];
                        int pressedButtonItemIndex = this.m_pressedButtonItemIndex;
                        int pressedButtonIndex = item.PressedButtonIndex;
                        item.PressedButtonIndex = -1;
                        this.m_pressedButtonItemIndex = -1;
                        if (!this.m_bTouchScrolling)
                        {
                            Point point3 = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                            CellEventArgs cellAtPoint = this.GetCellAtPoint(point3);
                            if (((cellAtPoint != null) && (cellAtPoint.Cell is ButtonCell)) && ((cellAtPoint.ItemIndex == pressedButtonItemIndex) && (cellAtPoint.CellIndex == pressedButtonIndex)))
                            {
                                this.m_owner.OnButton((ButtonCell) cellAtPoint.Cell, cellAtPoint.ListItem, new Point(cellAtPoint.ItemIndex, cellAtPoint.CellIndex), 0);
                            }
                        }
                        this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.List);
                    }
                    if (this.m_bShowingToolTip)
                    {
                        if (this.m_ToolTip != null)
                        {
                            this.m_ToolTip.Visible = false;
                        }
                        this.m_bShowingToolTip = false;
                    }
                    else if (this.m_bEnableTouchScrolling && this.m_bTouchScrolling)
                    {
                        if ((this.GetTicks() - this.m_TouchTime1) > 100)
                        {
                            this.m_TouchAutoScrollDiff = 0;
                        }
                        uint num3 = (this.GetTicks() - this.m_TouchTime0) / 50;
                        if (num3 > 0)
                        {
                            this.m_TouchAutoScrollDiff = (int) (this.m_TouchAutoScrollDiff / num3);
                        }
                        this.m_TouchScrollingTimer.Enabled = true;
                        this.m_bTouchScrolling = false;
                    }
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if ((this.m_iUpdateCounter == 0) && (this.m_iNoRedraw <= 0))
            {
                if (!this.Redraw(e.Graphics) && this.m_owner.DelayLoad)
                {
                    this.m_owner.DoDelayLoad();
                }
                if (this.m_bIsChange && ((DateTime.Now.Ticks - this.m_gcCollect) > 0x11e1a300L))
                {
                    this.m_gcCollect = DateTime.Now.Ticks;
                    GC.Collect();
                }
            }
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (this.m_owner.Items != null)
            {
                this.EndUpdate();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            if (this.m_doubleBuffered)
            {
                if (this.m_grBackBuffer == null)
                {
                    return;
                }
                this.UpdateDoubleBuffering();
            }
            if (this.m_vScroll != null)
            {
                this.SetVScrollBarBounds();
            }
            this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true, ComboBoxUpdateRange.List));
            if (this.m_iUpdateCounter == 0)
            {
                this.SetRedraw(0);
                this.m_owner.Items.LastDrawnItem = -1;
            }
            base.OnResize(e);
            base.Invalidate();
        }

        protected virtual void OnScroll()
        {
            if (this.Scroll != null)
            {
                this.Scroll(this, EventArgs.Empty);
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
                        this.m_ToolTip = new Resco.Controls.AdvancedComboBox.ToolTip();
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
            if (this.m_vScroll != null)
            {
                int num = this.m_vScroll.Value;
                int num2 = (this.m_vScroll.Maximum - this.m_vScroll.LargeChange) + 1;
                if (this.m_touchScrollDirection == Resco.Controls.AdvancedComboBox.TouchScrollDirection.Inverse)
                {
                    num -= this.m_TouchAutoScrollDiff;
                }
                else
                {
                    num += this.m_TouchAutoScrollDiff;
                }
                if (num < 0)
                {
                    num = 0;
                }
                else if (num > num2)
                {
                    num = num2;
                }
                if (this.m_vScroll.Value == num)
                {
                    this.m_TouchAutoScrollDiff = 0;
                    this.m_TouchScrollingTimer.Enabled = false;
                    return;
                }
                this.m_vScroll.Value = num;
            }
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
                this.m_TouchScrollingTimer.Enabled = false;
            }
        }

        internal void Popup()
        {
            base.BringToFront();
            this.EnsureVisible(this.SelectedItem);
            base.Visible = true;
            if (this.m_owner.AutoHideDropDownList)
            {
                base.Capture = true;
            }
            base.Focus();
        }

        public bool PreviousSelectableCell()
        {
            if (this.SelectedItemIndex >= 0)
            {
                int index;
                ItemTemplate.CellCollection cellTemplates = this.SelectedItem.GetTemplate(this.m_owner.Templates).CellTemplates;
                if (this.SelectedCell != null)
                {
                    index = cellTemplates.IndexOf(this.SelectedCell);
                }
                else
                {
                    index = -1;
                }
                if (index == -1)
                {
                    if (cellTemplates.Count == 0)
                    {
                        return false;
                    }
                    index = cellTemplates.Count - 1;
                    if (cellTemplates[index].Selectable)
                    {
                        this.SelectedCell = cellTemplates[index];
                        return true;
                    }
                }
                while (--index > -1)
                {
                    if (cellTemplates[index].Selectable)
                    {
                        this.SelectedCell = cellTemplates[index];
                        return true;
                    }
                }
                this.SelectedCell = null;
            }
            return false;
        }

        protected virtual bool Redraw(Graphics gr)
        {
            bool flag = true;
            if (this.m_doubleBuffered)
            {
                if (this.m_bIsChange)
                {
                    flag = this.RedrawBackBuffer();
                }
                gr.DrawImage(this.m_BackBuffer_Image, 0, 0);
                return flag;
            }
            return this.RedrawToDisplay(gr);
        }

        protected virtual bool RedrawBackBuffer()
        {
            int lastDrawnItem;
            int iTopmostItemOffset;
            bool resetScrollbar = false;
            this.backRect.Width = this.m_BackBuffer_Image.Width;
            this.backRect.Height = this.m_BackBuffer_Image.Height;
            if ((this.m_useGradient && (this.m_imgGradient == null)) && (this.m_gradientBackColor.FillDirection == FillDirection.Vertical))
            {
                this.m_imgGradient = new Bitmap(this.m_BackBuffer_Image.Width, this.m_BackBuffer_Image.Height);
                this.m_grGradient = Graphics.FromImage(this.m_imgGradient);
                this.m_gradientBackColor.DrawGradient(this.m_grGradient, this.backRect);
                int argb = (this.m_gradientBackColor.EndColor.ToArgb() + this.m_gradientBackColor.StartColor.ToArgb()) / 2;
                this.m_colorKey = Color.FromArgb(argb);
                this.m_brushKey.Color = this.m_colorKey;
                this.m_imgAttr.SetColorKey(this.m_colorKey, this.m_colorKey);
            }
            int width = base.ClientSize.Width;
            int height = base.ClientSize.Height;
            bool flag2 = true;
            int iScrollChange = this.m_iScrollChange;
            this.m_iScrollChange = 0;
            int ymax = height;
            if (Math.Abs(iScrollChange) > ymax)
            {
                iScrollChange = 0;
            }
            int num6 = width - this.ClientScrollbarWidth;
            int y = 0;
            this.ResetCache(iScrollChange);
            if ((iScrollChange < 0) && (this.m_owner.Items.LastDrawnItem > 0))
            {
                int num10 = height + iScrollChange;
                lastDrawnItem = this.m_owner.Items.LastDrawnItem;
                iTopmostItemOffset = this.m_owner.Items.LastDrawnItemOffset + iScrollChange;
                Rectangle srcRect = new Rectangle(0, -iScrollChange, width, num10);
                Rectangle destRect = new Rectangle(0, 0, width, num10);
                if (this.m_useGradient && (this.m_gradientBackColor.FillDirection == FillDirection.Vertical))
                {
                    this.m_grBackBuffer.DrawImage(this.m_tmpImage, destRect, srcRect, GraphicsUnit.Pixel);
                    this.m_grTmp.DrawImage(this.m_BackBuffer_Image, 0, 0);
                }
                else
                {
                    this.m_grTmp.DrawImage(this.m_BackBuffer_Image, 0, 0);
                    this.m_grBackBuffer.DrawImage(this.m_tmpImage, destRect, srcRect, GraphicsUnit.Pixel);
                }
            }
            else
            {
                lastDrawnItem = this.m_iActualItemIndex;
                iTopmostItemOffset = this.m_iTopmostItemOffset;
                if (iScrollChange > 0)
                {
                    int num11 = height - iScrollChange;
                    Rectangle rectangle3 = new Rectangle(0, 0, width, num11);
                    Rectangle rectangle4 = new Rectangle(0, iScrollChange, width, num11);
                    if (this.m_useGradient && (this.m_gradientBackColor.FillDirection == FillDirection.Vertical))
                    {
                        this.m_grBackBuffer.DrawImage(this.m_tmpImage, rectangle4, rectangle3, GraphicsUnit.Pixel);
                        this.m_grTmp.DrawImage(this.m_BackBuffer_Image, 0, 0);
                    }
                    else
                    {
                        this.m_grTmp.DrawImage(this.m_BackBuffer_Image, 0, 0);
                        this.m_grBackBuffer.DrawImage(this.m_tmpImage, rectangle4, rectangle3, GraphicsUnit.Pixel);
                    }
                    ymax = iScrollChange;
                }
            }
            if (this.m_useGradient && (this.m_gradientBackColor.FillDirection != FillDirection.Vertical))
            {
                this.m_gradientBackColor.DrawGradient(this.m_grBackBuffer, new Rectangle(0, iTopmostItemOffset, width, ymax - iTopmostItemOffset));
            }
            Graphics grBackBuffer = this.m_grBackBuffer;
            if (this.m_useGradient && (this.m_gradientBackColor.FillDirection == FillDirection.Vertical))
            {
                grBackBuffer = this.m_grTmp;
            }
            grBackBuffer.Clip = new Region(new Rectangle(0, iTopmostItemOffset, num6, ymax - iTopmostItemOffset));
            y = this.m_owner.Items.Draw(grBackBuffer, this.m_owner.Templates, num6, ymax, lastDrawnItem, iTopmostItemOffset, ref resetScrollbar);
            grBackBuffer.ResetClip();
            if (y <= ymax)
            {
                if (!this.m_useGradient)
                {
                    this.RedrawBackground(grBackBuffer, new Rectangle(0, y, width, ymax - y));
                }
                else if (this.m_gradientBackColor.FillDirection != FillDirection.Vertical)
                {
                    this.m_gradientBackColor.DrawGradient(this.m_grBackBuffer, new Rectangle(0, y, width, ymax - y));
                }
                else if (this.m_gradientBackColor.FillDirection == FillDirection.Vertical)
                {
                    grBackBuffer.FillRectangle(this.m_brushKey, 0, y, width, ymax - y);
                }
                flag2 = false;
            }
            if (this.m_useGradient && (this.m_gradientBackColor.FillDirection == FillDirection.Vertical))
            {
                this.m_grBackBuffer.DrawImage(this.m_imgGradient, 0, 0);
                this.m_grBackBuffer.DrawImage(this.m_tmpImage, this.backRect, 0, 0, this.backRect.Width, this.backRect.Height, GraphicsUnit.Pixel, this.m_imgAttr);
            }
            if (resetScrollbar)
            {
                this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true));
                return flag2;
            }
            this.m_bIsChange = false;
            return flag2;
        }

        protected virtual void RedrawBackground(Graphics g, Rectangle rect)
        {
            g.FillRectangle(this.m_BackColor, rect.X, rect.Y, rect.Width, rect.Height);
        }

        protected virtual bool RedrawToDisplay(Graphics gr)
        {
            bool flag = true;
            bool resetScrollbar = false;
            int iScrollChange = this.m_iScrollChange;
            this.m_iScrollChange = 0;
            int height = base.Height;
            if (Math.Abs(iScrollChange) > height)
            {
                iScrollChange = 0;
            }
            int width = base.ClientSize.Width - this.ClientScrollbarWidth;
            int y = 0;
            int iActualItemIndex = this.m_iActualItemIndex;
            int iTopmostItemOffset = this.m_iTopmostItemOffset;
            this.backRect.Width = base.ClientSize.Width;
            this.backRect.Height = base.ClientSize.Height;
            this.ResetCache(iScrollChange);
            if (this.m_useGradient)
            {
                this.m_gradientBackColor.DrawGradient(gr, new Rectangle(0, y, base.ClientSize.Width, height - y));
            }
            y = this.m_owner.Items.Draw(gr, this.m_owner.Templates, width, height, iActualItemIndex, iTopmostItemOffset, ref resetScrollbar);
            if (y <= height)
            {
                if (!this.m_useGradient)
                {
                    this.RedrawBackground(gr, new Rectangle(0, y, base.ClientSize.Width, base.Height));
                }
                flag = false;
            }
            if (resetScrollbar)
            {
                this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true));
            }
            this.m_bIsChange = false;
            return flag;
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
                int height = base.Height;
                for (int i = this.m_alLinks.Count - 1; i >= 0; i--)
                {
                    Rectangle rectangle = this.m_alLinks[i];//.get_Item(i);
                    rectangle.Y += iScroll;
                    if ((rectangle.Bottom < 0) || (rectangle.Top > height))
                    {
                        this.m_alLinks.RemoveAt(i);
                    }
                    else
                    {
                        this.m_alLinks[i]=rectangle;//.set_Item(i, rectangle);
                    }
                }
                for (int j = this.m_alButtons.Count - 1; j >= 0; j--)
                {
                    Rectangle rectangle2 = this.m_alButtons[j];//.get_Item(j);
                    rectangle2.Y += iScroll;
                    if ((rectangle2.Bottom < 0) || (rectangle2.Top > height))
                    {
                        this.m_alButtons.RemoveAt(j);
                    }
                    else
                    {
                        this.m_alButtons[j]=rectangle2;//.set_Item(j, rectangle2);
                    }
                }
                for (int k = this.m_alTooltips.Count - 1; k >= 0; k--)
                {
                    Rectangle bounds = this.m_alTooltips[k].Bounds;
                    bounds.Y += iScroll;
                    if ((bounds.Bottom < 0) || (bounds.Top > height))
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

        public void ResumeRedraw(bool bForceRedraw)
        {
            this.m_bForceRedraw = bForceRedraw;
            if (this.m_iNoRedraw > 0)
            {
                this.m_iNoRedraw--;
            }
            if (this.m_iNoRedraw == 0)
            {
                this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.List);
                if (this.m_bForceRedraw)
                {
                    base.Update();
                }
                this.m_bForceRedraw = false;
            }
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            if (factor.Width != 1.0)
            {
                this.m_iScrollWidth = (int) (this.m_iScrollWidth * factor.Width);
            }
            base.ScaleControl(factor, specified);
        }

        private bool ScrollbarEnabled()
        {
            return (((this.m_vScroll != null) && this.VScrollBarVisible) && (this.m_vScroll.Maximum > this.m_vScroll.Minimum));
        }

        protected virtual int ScrollBarWindowProc(IntPtr hwnd, int msg, int wParam, int lParam)
        {
            if (msg == 2)
            {
                this.UnhackScrollbar();
            }
            int num = Win32Native.CallWindowProc(this.m_wndprocReal, hwnd, msg, wParam, lParam);
            if ((msg == 0x202) && this.m_owner.AutoHideDropDownList)
            {
                base.Capture = true;
            }
            return num;
        }

        private void SetRedraw(int iScroll)
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

        public int SetScrollPos(int ix, int cx, int offset)
        {
            if (this.m_vScroll == null)
            {
                int num = 0;
                for (int k = 0; k < ix; k++)
                {
                    num += this.m_owner.Items.GetHeight(k, this.m_owner.Templates);
                }
                return num;
            }
            int num3 = 0;
            int num4 = this.m_vScroll.Maximum - base.ClientSize.Height;
            if (num4 < 0)
            {
                num4 = 0;
            }
            for (int i = 0; i < ix; i++)
            {
                num3 += this.m_owner.Items.GetHeight(i, this.m_owner.Templates);
            }
            int num6 = (cx >= 0) ? this.m_owner.Items[ix].Template[cx].Bounds.Top : 0;
            int num7 = (num3 + num6) - offset;
            if (num7 > num4)
            {
                num7 = num4;
            }
            else if (num7 < 0)
            {
                num7 = 0;
            }
            int num8 = 0;
            for (int j = 0; j < this.m_owner.Items.Count; j++)
            {
                int height = this.m_owner.Items.GetHeight(j, this.m_owner.Templates);
                if ((num8 + height) > num7)
                {
                    this.m_iActualItemIndex = j;
                    this.m_iTopmostItemOffset = num8 - num7;
                    break;
                }
                num8 += height;
            }
            this.m_iVScrollPrevValue = num7;
            this.m_vScroll.Value = num7;
            this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.List);
            return (num3 - num7);
        }

        private bool SetVScrollBar(int height)
        {
            bool vScrollBarVisible = false;
            int num = base.ClientSize.Height;
            if (num <= 0)
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
                if ((num >= height) || (height < 100))
                {
                    return false;
                }
                this.m_vScroll = new System.Windows.Forms.VScrollBar();
                this.SetVScrollBarBounds();
                this.m_vScroll.Minimum = 0;
                this.m_vScroll.Maximum = Math.Max(0, height + ScrollBottomOffset);
                this.m_vScroll.LargeChange = num;
                this.m_vScroll.SmallChange = (num < ScrollSmallChange) ? num : ScrollSmallChange;
                this.m_vScroll.Value = 0;
                this.m_vScroll.ValueChanged += new EventHandler(this.m_vScroll_ValueChanged);
                base.Controls.Add(this.m_vScroll);
                this.VScrollBarVisible = this.ShowScrollbar;
                this.m_vScrollWidth = this.ShowScrollbar ? this.m_iScrollWidth : 0;
                this.HackScrollbar();
                return this.ShowScrollbar;
            }
            this.SetVScrollBarBounds();
            if (num < ScrollSmallChange)
            {
                this.m_vScroll.SmallChange = num;
            }
            if ((this.m_vScroll.Value + num) > height)
            {
                this.m_vScroll.Value = Math.Max(0, height - num);
            }
            this.m_vScroll.Maximum = Math.Max(0, height + ScrollBottomOffset);
            if (num != this.m_vScroll.LargeChange)
            {
                this.m_vScroll.LargeChange = num;
            }
            if (height > num)
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

        private void SetVScrollBarBounds()
        {
            int num = (base.Width - base.ClientSize.Width) / 2;
            if (this.m_vScroll != null)
            {
                if (!this.m_owner.RightToLeft)
                {
                    this.m_vScroll.Bounds = new Rectangle((base.ClientSize.Width - this.m_vScrollWidth) + num, -num, this.m_vScrollWidth, base.Height);
                }
                else
                {
                    this.m_vScroll.Bounds = new Rectangle(-num, -num, this.m_vScrollWidth, base.Height);
                }
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

        protected bool ShouldSerializeGyroSensorCalibration()
        {
            if ((this.m_HTCGyroOffset.X == 0) && (this.m_HTCGyroOffset.Y == 0))
            {
                return false;
            }
            return true;
        }

        protected bool ShouldSerializeGyroSensorSensitivity()
        {
            if ((this.m_HTCGyroSensitivity.X == 200) && (this.m_HTCGyroSensitivity.X == 200))
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeTouchScrollDirection()
        {
            return (this.m_touchScrollDirection != Resco.Controls.AdvancedComboBox.TouchScrollDirection.Inverse);
        }

        public void SuspendRedraw()
        {
            this.m_iNoRedraw++;
        }

        private void UnhackScrollbar()
        {
            if (this.m_scrollWnd != IntPtr.Zero)
            {
                Win32Native.SetWindowLong(this.m_scrollWnd, -4, this.m_wndprocReal);
                this.m_scrollWnd = IntPtr.Zero;
                this.m_wndprocReal = IntPtr.Zero;
                this.m_wndproc = IntPtr.Zero;
                this.m_delegate = null;
                this.m_dlgLock.Free();
            }
        }

        private void UpdateDoubleBuffering()
        {
            GC.Collect();
            int width = 1;
            int height = 1;
            try
            {
                width = (base.ClientSize.Width < 1) ? 1 : base.ClientSize.Width;
                height = (base.ClientSize.Height < 1) ? 1 : base.ClientSize.Height;
            }
            catch
            {
            }
            if ((this.m_BackBuffer_Image != null) && ((this.m_BackBuffer_Image.Width != width) || (this.m_BackBuffer_Image.Height != height)))
            {
                this.m_BackBuffer_Image.Dispose();
                this.m_BackBuffer_Image = null;
                if (this.m_grBackBuffer != null)
                {
                    this.m_grBackBuffer.Dispose();
                }
                this.m_grBackBuffer = null;
            }
            if ((this.m_tmpImage != null) && ((this.m_tmpImage.Width != width) || (this.m_tmpImage.Height != height)))
            {
                this.m_tmpImage.Dispose();
                this.m_tmpImage = null;
                if (this.m_grTmp != null)
                {
                    this.m_grTmp.Dispose();
                }
                this.m_grTmp = null;
            }
            if ((this.m_imgGradient != null) && ((this.m_imgGradient.Width != width) || (this.m_imgGradient.Height != height)))
            {
                this.m_imgGradient.Dispose();
                if (this.m_grGradient != null)
                {
                    this.m_grGradient.Dispose();
                }
                this.m_imgGradient = null;
                this.m_grGradient = null;
            }
            if (this.m_doubleBuffered)
            {
                if (this.m_BackBuffer_Image == null)
                {
                    this.m_BackBuffer_Image = new Bitmap(width, height);
                    this.m_grBackBuffer = Graphics.FromImage(this.m_BackBuffer_Image);
                }
                if (this.m_tmpImage == null)
                {
                    this.m_tmpImage = new Bitmap(width, height);
                    this.m_grTmp = Graphics.FromImage(this.m_tmpImage);
                }
            }
        }

        protected internal Graphics BackBuffer
        {
            get
            {
                return this.m_grBackBuffer;
            }
        }

        internal Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                this.m_BackColor = new SolidBrush(value);
            }
        }

        internal int ClientScrollbarWidth
        {
            get
            {
                if (this.m_vScrollVisible)
                {
                    return (this.m_vScrollWidth - ((base.Width - base.ClientSize.Width) / 2));
                }
                return 0;
            }
        }

        internal System.Windows.Forms.ContextMenu ContextMenu
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

        private int DocumentHeight
        {
            get
            {
                if (!this.m_owner.DelayLoad || (this.m_owner.ExpectedItems < 0))
                {
                    return this.m_iDocumentHeight;
                }
                ItemTemplate template = this.m_owner.Templates[this.m_owner.TemplateIndex];
                if (template == null)
                {
                    template = this.m_owner.DefaultTemplates[0];
                }
                if (template == null)
                {
                    return this.m_iDocumentHeight;
                }
                return ((template.Height + (this.GridLines ? 1 : 0)) * this.m_owner.ExpectedItems);
            }
        }

        internal bool DoubleBuffered
        {
            get
            {
                return this.m_doubleBuffered;
            }
            set
            {
                if (this.m_doubleBuffered != value)
                {
                    this.m_doubleBuffered = value;
                    this.UpdateDoubleBuffering();
                    if (this.m_iUpdateCounter == 0)
                    {
                        this.SetRedraw(0);
                        this.m_owner.Items.LastDrawnItem = -1;
                    }
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(false)]
        public bool EnableHTCGyroSensor
        {
            get
            {
                return this.m_enableHTCGSensor;
            }
            set
            {
                if (this.m_enableHTCGSensor != value)
                {
                    try
                    {
                        this.InitHTCGSensor();
                        this.m_enableHTCGSensor = value;
                        if (this.m_HTCGyroNavigationTimer != null)
                        {
                            this.m_HTCGyroNavigationTimer.Enabled = value;
                        }
                    }
                    catch
                    {
                        this.m_enableHTCGSensor = false;
                        if (this.m_HTCGyroNavigationTimer != null)
                        {
                            this.m_HTCGyroNavigationTimer.Enabled = false;
                        }
                    }
                }
            }
        }

        [DefaultValue(false)]
        public bool EnableHTCNavSensor
        {
            get
            {
                return this.m_enableHTCNavSensor;
            }
            set
            {
                if (value)
                {
                    if (this.m_HTCNavTimer == null)
                    {
                        this.m_HTCNavTimer = new Timer();
                        this.m_HTCNavTimer.Tick += new EventHandler(this.m_HTCNavTimer_Tick);
                        this.m_HTCNavTimer.Interval = 200;
                    }
                    this.m_HTCNavTimer.Enabled = true;
                    this.m_enableHTCNavSensor = true;
                }
                else
                {
                    if (this.m_HTCNavTimer != null)
                    {
                        this.m_HTCNavTimer.Enabled = false;
                    }
                    if (this.m_HTCNavSensor != null)
                    {
                        this.m_HTCNavSensor.Dispose();
                        this.m_HTCNavSensor = null;
                    }
                    this.m_enableHTCNavSensor = false;
                }
            }
        }

        internal GradientColor GradientBackColor
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
                this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.List);
            }
        }

        internal Color GridColor
        {
            get
            {
                return this.m_owner.Items.m_penBorder.Color;
            }
            set
            {
                if (this.m_owner.Items.m_penBorder.Color != value)
                {
                    if (value.IsEmpty)
                    {
                        value = Color.DarkGray;
                    }
                    this.m_owner.Items.m_penBorder.Color = value;
                    this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.List);
                }
            }
        }

        internal bool GridLines
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
                    this.OnChange(this, ComboBoxEventArgsType.Refresh, ComboBoxArgs.List);
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

        [DefaultValue(500)]
        public int GyroNavigationRepeatDelay
        {
            get
            {
                return this.m_HTCGyroNavRepeatDelay;
            }
            set
            {
                this.m_HTCGyroNavRepeatDelay = value;
            }
        }

        [DefaultValue(50)]
        public int GyroNavigationRepeatRate
        {
            get
            {
                return this.m_HTCGyroNavRepeatRate;
            }
            set
            {
                this.m_HTCGyroNavRepeatRate = value;
            }
        }

        public Point GyroSensorCalibration
        {
            get
            {
                return this.m_HTCGyroOffset;
            }
            set
            {
                this.m_HTCGyroOffset = value;
            }
        }

        [DefaultValue(false)]
        public bool GyroSensorNavigation
        {
            get
            {
                return this.m_HTCGyroSensorNavigation;
            }
            set
            {
                this.m_HTCGyroSensorNavigation = value;
            }
        }

        public Point GyroSensorSensitivity
        {
            get
            {
                return this.m_HTCGyroSensitivity;
            }
            set
            {
                if (value.X < 0)
                {
                    value.X = -value.X;
                }
                if (value.Y < 0)
                {
                    value.Y = -value.Y;
                }
                this.m_HTCGyroSensitivity = value;
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedComboBox.Design.Browsable(false)]
        public Point GyroSensorStatus
        {
            get
            {
                return this.m_HTCGyroStatus;
            }
            set
            {
                this.m_HTCGyroStatus = value;
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.Browsable(false), Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden)]
        public Resco.Controls.AdvancedComboBox.HTCScreenOrientation HTCScreenOrientation
        {
            get
            {
                return this.m_HTCScreenOrientation;
            }
        }

        [DefaultValue(false)]
        public bool NavSensorNavigation
        {
            get
            {
                return this.m_enableHTCNavSensorNavigation;
            }
            set
            {
                this.m_enableHTCNavSensorNavigation = value;
            }
        }

        internal bool ScrollbarVisible
        {
            get
            {
                return ((this.ShowScrollbar && (this.m_vScrollWidth > 0)) && (this.m_vScroll != null));
            }
        }

        internal int ScrollbarWidth
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
                        this.m_vScroll.Left = base.ClientSize.Width - value;
                        this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true, ComboBoxUpdateRange.List));
                    }
                }
            }
        }

        internal Cell SelectedCell
        {
            get
            {
                if (this.SelectedItem != null)
                {
                    ItemTemplate.CellCollection cellTemplates = this.SelectedItem.GetTemplate(this.m_owner.Templates).CellTemplates;
                    if ((this.m_iSelectedCellIndex >= 0) && (this.m_iSelectedCellIndex < cellTemplates.Count))
                    {
                        return cellTemplates[this.m_iSelectedCellIndex];
                    }
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    if (this.m_iSelectedCellIndex != -1)
                    {
                        this.m_iSelectedCellIndex = -1;
                        this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true, ComboBoxUpdateRange.List));
                    }
                }
                else
                {
                    if (this.SelectedItem == null)
                    {
                        throw new ArgumentException("The cell is not contained in currently selected item's ItemTemplate's CellCollection.", "SelectedCell");
                    }
                    ItemTemplate.CellCollection cellTemplates = this.SelectedItem.GetTemplate(this.m_owner.Templates).CellTemplates;
                    if ((this.m_iSelectedCellIndex == -1) || (cellTemplates[this.m_iSelectedCellIndex] != value))
                    {
                        if (!cellTemplates.Contains(value))
                        {
                            throw new ArgumentException("The cell is not contained in currently selected item's ItemTemplate's CellCollection.", "SelectedCell");
                        }
                        if (!value.Selectable)
                        {
                            throw new ArgumentException("The cell is not selectable!", "SelectedCell");
                        }
                        this.m_iSelectedCellIndex = cellTemplates.IndexOf(value);
                        this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true, ComboBoxUpdateRange.List));
                    }
                }
            }
        }

        internal Resco.Controls.AdvancedComboBox.ListItem SelectedItem
        {
            get
            {
                return this.m_owner.Items.SelectedItem;
            }
            set
            {
                if (this.m_owner.Items.SelectedItem != value)
                {
                    int index = -1;
                    if (value != null)
                    {
                        index = value.Index;
                    }
                    this.SuspendRedraw();
                    this.m_owner.Items.SelectedItem = value;
                    this.ResumeRedraw(true);
                    base.Update();
                    if (value != null)
                    {
                        this.OnItemSelect(value, index);
                    }
                }
            }
        }

        [Resco.Controls.AdvancedComboBox.Design.Browsable(false), Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden)]
        internal int SelectedItemIndex
        {
            get
            {
                if (this.m_owner.Items.SelectedItem != null)
                {
                    return this.m_owner.Items.SelectedItem.Index;
                }
                return -1;
            }
            set
            {
                if (value != this.SelectedItemIndex)
                {
                    this.SelectedCell = null;
                    if (value < 0)
                    {
                        this.SelectedItem = null;
                    }
                    else if (value < this.m_owner.Items.Count)
                    {
                        this.SelectedItem = this.m_owner.Items[value];
                    }
                }
            }
        }

        internal bool ShowScrollbar
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
                    this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true, ComboBoxUpdateRange.List));
                }
            }
        }

        internal int TopItemIndex
        {
            get
            {
                return this.m_iActualItemIndex;
            }
        }

        internal int TopItemOffset
        {
            get
            {
                return this.m_iTopmostItemOffset;
            }
        }

        internal Resco.Controls.AdvancedComboBox.TouchScrollDirection TouchScrollDirection
        {
            get
            {
                return this.m_touchScrollDirection;
            }
            set
            {
                this.m_touchScrollDirection = value;
            }
        }

        internal bool TouchScrolling
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

        internal int TouchSensitivity
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

        internal bool UseGradient
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
                    this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.List);
                }
            }
        }

        protected System.Windows.Forms.VScrollBar VScrollBar
        {
            get
            {
                return this.m_vScroll;
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
}

