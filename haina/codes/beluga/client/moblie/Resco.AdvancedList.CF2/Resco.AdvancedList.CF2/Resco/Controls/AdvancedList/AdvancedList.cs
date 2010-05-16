namespace Resco.Controls.AdvancedList
{
    using Resco.Controls.AdvancedList.Design;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;

    public class AdvancedList : UserControl
    {
        private DesignTimeCallback _designTimeCallback;
        private Rectangle backRect = new Rectangle(0, 0, 100, 100);
        private bool bIsInScrollChange;
        public static int DefaultTooltipWidth = 8;
        private int m_activeTemplateIndex;
        private List<Rectangle> m_alButtons = new List<Rectangle>();
        private List<Rectangle> m_alLinks = new List<Rectangle>();
        private int m_alternateTemplateIndex;
        private List<TooltipArea> m_alTooltips = new List<TooltipArea>();
        private bool m_autoBind;
        private SolidBrush m_BackColor;
        private bool m_bDelayLoad;
        private bool m_bEnableTouchScrolling;
        private bool m_bFocusOnClick;
        private Image m_bgImage;
        private bool m_bGradientChanged;
        private bool m_bIsChange = true;
        private bool m_bKeyNavigation;
        private bool m_bManualDataLoading;
        private bool m_bMultiSelect;
        private Pen m_BorderPen = new Pen(Color.Black);
        private Resco.Controls.AdvancedList.Mapping m_boundMap = Resco.Controls.AdvancedList.Mapping.Empty;
        private SolidBrush m_brushKey;
        private bool m_bScrollbarOverlap = true;
        private bool m_bShowFooter;
        private bool m_bShowHeader;
        private bool m_bShowingContextMenu;
        private bool m_bShowingToolTip;
        private bool m_bShowScrollbar = true;
        private bool m_bStartingTouchScroll;
        private bool m_bTouchScrolling;
        private int m_buttonCellIndex;
        private int m_buttonRowIndex;
        internal Color m_colorKey;
        private IDataConnector m_connector;
        private System.Windows.Forms.ContextMenu m_ContextMenu;
        private Conversion m_conversion;
        private object m_dataSource;
        private Resco.Controls.AdvancedList.DataConnector m_dbConnector;
        private bool m_doubleBuffered = true;
        private bool m_enableHTCGSensor;
        private bool m_enableHTCNavSensor;
        private bool m_enableHTCNavSensorNavigation;
        private IEnumerator m_enumerator;
        private bool m_enumeratorNeedLoad;
        private GradientColor m_gradientBackColor;
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
        private Resco.Controls.AdvancedList.HTCScreenOrientation m_HTCScreenOrientation = Resco.Controls.AdvancedList.HTCScreenOrientation.Portrait;
        private int m_iActualRowIndex;
        private int m_iDocumentHeight;
        private int m_iExpectedRows;
        private int m_iInsertIndex = -1;
        private List<ImageList> m_imageLists;
        private ImageAttributes m_imgAttr;
        private CurrencyManager m_innerListManager;
        private int m_iNoRedraw;
        private int m_iScrollChange;
        private int m_iScrollWidth;
        private int m_iSelectedCellIndex;
        private int m_iTopmostRowOffset;
        private int m_iUpdate = 1;
        private int m_iVScrollPrevValue;
        private CurrencyManager m_listManager;
        private Resco.Controls.AdvancedList.Mapping m_mapLast;
        private Resco.Controls.AdvancedList.SelectionMode m_mode;
        private Point m_MousePosition = new Point(0, 0);
        private int m_nRowsInserted;
        private int m_nRowsLoaded;
        private int m_pressedButtonRowIndex = -3;
        private RowCollection m_rcRows;
        private Resco.Controls.AdvancedList.HeaderRow m_rFooter;
        private Resco.Controls.AdvancedList.HeaderRow m_rHeader;
        private bool m_rightToLeft;
        private HTCNavSensorRotatedEventArgs m_rotatedEventArgs = new HTCNavSensorRotatedEventArgs(0.0, 0.0);
        private float m_scaleFactorY;
        private int m_selectedTemplateIndex;
        private int m_templateIndex;
        private Timer m_Timer;
        private Resco.Controls.AdvancedList.ToolTip m_ToolTip;
        private Resco.Controls.AdvancedList.ToolTipType m_toolTipType;
        private int m_TouchAutoScrollDiff;
        private Resco.Controls.AdvancedList.TouchScrollDirection m_touchScrollDirection;
        private Timer m_TouchScrollingTimer;
        private int m_touchSensitivity;
        private uint m_TouchTime0;
        private uint m_TouchTime1;
        private TemplateSet m_tsCurrent;
        private ScrollbarWrapper m_vScroll;
        private Control m_vScrollBarResco;
        private bool m_vScrollVisible;
        private int m_vScrollWidth;
        private OnChangeDelegate OnChangeHandler;
        private OnRowRemovedDelegate OnRowRemovedHandler;
        internal static Point point1;
        internal static Point point2;
        internal static Point point3;
        private static Hashtable sBrushes = null;
        public static int ScrollBottomOffset = -1;
        private static int ScrollSmallChange = 0x10;
        private static Pen sPen = null;
        private static Bitmap sPixel = null;
        internal const PlatformID SYMBIAN_OS = ((PlatformID) 0xc0);
        public static int TooltipWidth = 8;
        internal static Color TransparentColor = Color.Transparent;

        public event EventHandler ActiveRowChanged;

        public event ButtonEventHandler ButtonClick;

        public event CellEventHandler CellClick;

        public event CellEnteredMainEventHandler CellEntered;

        public event CustomizeCellEventHandler CustomizeCell;

        public event DataLoadedEventHandler DataLoaded;

        public event CellEventHandler FooterClick;

        public event CellEventHandler HeaderClick;

        public event HTCGyroDirectionHandler HTCGyroDirection;

        public event HTCNavSensorRotatedHandler HTCNavSensorRotated;

        public event HTCOrientationChangedHandler HTCOrientationChanged;

        public event LinkEventHandler LinkClick;

        public event RowAddingEventHandler RowAdding;

        public event RowEnteredEventHandler RowEntered;

        public event RowEventHandler RowSelect;

        public event EventHandler Scroll;

        public event ValidateDataEventHandler ValidateData;

        static AdvancedList()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(Resco.Controls.AdvancedList.AdvancedList), "");
            //}
        }

        public AdvancedList()
        {
            this.OnChangeHandler = new OnChangeDelegate(this.OnChangeSafe);
            this.OnRowRemovedHandler = new OnRowRemovedDelegate(this.OnRowRemovedSafe);
            this.m_templateIndex = 0;
            this.m_selectedTemplateIndex = 0;
            this.m_activeTemplateIndex = -1;
            this.m_alternateTemplateIndex = -1;
            this.m_dbConnector = new Resco.Controls.AdvancedList.DataConnector();
            this.m_connector = this.m_dbConnector;
            this.m_rcRows = new RowCollection(this);
            this.m_rcRows.Changed += new GridEventHandler(this.OnChange);
            this.m_tsCurrent = new TemplateSet();
            this.m_tsCurrent.Parent = this;
            this.m_tsCurrent.Changed += new GridEventHandler(this.OnChange);
            BackBufferManager.AddRef();
            this.m_colorKey = Color.FromArgb(0xff, 0, 0xff);
            this.m_brushKey = new SolidBrush(this.m_colorKey);
            this.m_imgAttr = new ImageAttributes();
            this.m_imgAttr.SetColorKey(this.m_colorKey, this.m_colorKey);
            base.BackColor = SystemColors.ControlDark;
            this.m_BackColor = new SolidBrush(this.BackColor);
            this.m_vScrollWidth = 0;
            this.m_iScrollWidth = 13;
            this.ButtonClick = null;
            this.LinkClick = null;
            this.CellClick = null;
            this.RowSelect = null;
            this.HeaderClick = null;
            this.ActiveRowChanged = null;
            this.Scroll = null;
            this.m_rHeader = new Resco.Controls.AdvancedList.HeaderRow();
            this.m_rHeader.Parent = this.m_rcRows;
            this.m_rFooter = new Resco.Controls.AdvancedList.HeaderRow();
            this.m_rFooter.Parent = this.m_rcRows;
            this.m_iActualRowIndex = 0;
            this.m_iTopmostRowOffset = 0;
            this.m_iDocumentHeight = 0;
            this.m_iExpectedRows = -1;
            this.m_iVScrollPrevValue = 0;
            this.m_toolTipType = Resco.Controls.AdvancedList.ToolTipType.Triangle;
            using (Graphics graphics = base.CreateGraphics())
            {
                TooltipWidth = (int) (DefaultTooltipWidth * (graphics.DpiX / 96f));
                point1 = new Point(0, 0);
                point2 = new Point(0, -TooltipWidth);
                point3 = new Point(-TooltipWidth, 0);
                this.m_ToolTip = null;
                this.m_scaleFactorY = graphics.DpiY / 96f;
            }
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
            this.m_bStartingTouchScroll = false;
            this.m_bEnableTouchScrolling = false;
            this.m_TouchAutoScrollDiff = 0;
            this.m_touchSensitivity = 8;
            this.m_touchScrollDirection = Resco.Controls.AdvancedList.TouchScrollDirection.Inverse;
            this.m_nRowsLoaded = 0;
            this.m_nRowsInserted = 0;
            this.m_bFocusOnClick = false;
            this.m_gradientBackColor = new GradientColor(FillDirection.Horizontal);
            this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
        }

        public void ActivateLink(int rowIndex, int cellIndex)
        {
            int rowOffset = this.GetRowOffset(rowIndex);
            switch (rowOffset)
            {
                case -2147483648:
                case 0x7fffffff:
                    return;
            }
            Row r = this.m_rcRows[rowIndex];
            RowTemplate template = r.GetTemplate(this.m_tsCurrent);
            if ((cellIndex >= 0) && (cellIndex < template.CellTemplates.Count))
            {
                LinkCell c = template.CellTemplates[cellIndex] as LinkCell;
                if (c != null)
                {
                    this.OnLink(c, r, new Point(rowIndex, cellIndex), rowOffset);
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

        private bool AddRowManually()
        {
            RowAddingEventArgs e = new RowAddingEventArgs(this.m_rcRows.Count);
            this.OnRowAdding(e);
            if ((e.Row != null) && !e.Cancel)
            {
                this.m_rcRows.Add(e.Row);
            }
            return e.Cancel;
        }

        public void AddTooltipArea(Rectangle bounds, string text)
        {
            this.m_alTooltips.Add(new TooltipArea(bounds, text));
        }

        [Obsolete("This method is obsolete, use BeginUpdate instead.")]
        public void BeginInit()
        {
            this.BeginUpdate();
        }

        public void BeginUpdate()
        {
            this.m_iUpdate++;
        }

        private void BindTo(object dataSource)
        {
            if ((base.Parent != null) && (base.BindingContext != null))
            {
                if (dataSource == null)
                {
                    this.RemoveActiveHandlers();
                    this.m_listManager = null;
                    this.m_dataSource = null;
                    this.ReloadDataSource();
                    this.RebindInnerList();
                }
                else
                {
                    if ((!(dataSource is IList) && !(dataSource is IListSource)) && !(dataSource is IEnumerable))
                    {
                        throw new ArgumentException("Not an IList, IListSource or IEnumerable");
                    }
                    this.m_iActualRowIndex = 0;
                    this.m_iTopmostRowOffset = 0;
                    this.m_iVScrollPrevValue = 0;
                    this.RemoveActiveHandlers();
                    this.RemoveEnumerator();
                    this.m_dataSource = dataSource;
                    if ((dataSource is IList) || (dataSource is IListSource))
                    {
                        this.m_listManager = (CurrencyManager) base.BindingContext[dataSource, null];
                        this.m_listManager.Refresh();
                    }
                    else
                    {
                        this.m_listManager = null;
                        this.SetEnumerator();
                    }
                    this.ReloadDataSource();
                    this.RebindInnerList();
                    this.SetActiveHandlers();
                }
            }
            else
            {
                this.m_dataSource = dataSource;
            }
        }

        protected internal virtual Rectangle CalculateClientRect()
        {
            return base.ClientRectangle;
        }

        private void CalculateFirstRow(int iOffset)
        {
            int gridLinesWidth = this.GridLinesWidth;
            this.m_iTopmostRowOffset += iOffset;
            if (iOffset <= 0)
            {
                int iTopmostRowOffset = this.m_iTopmostRowOffset;
                for (int i = this.m_iActualRowIndex; i < this.m_rcRows.Count; i++)
                {
                    int height = this.m_rcRows.GetHeight(i, this.m_tsCurrent);
                    if (Math.Abs(iTopmostRowOffset) < height)
                    {
                        this.m_iActualRowIndex = i;
                        this.m_iTopmostRowOffset = iTopmostRowOffset;
                        return;
                    }
                    iTopmostRowOffset += height;
                }
            }
            else
            {
                for (int j = this.m_iActualRowIndex; j >= 0; j--)
                {
                    if (this.m_iTopmostRowOffset > 0)
                    {
                        if (j == 0)
                        {
                            this.EnsureVisible(0);
                            return;
                        }
                        int num5 = this.m_rcRows.GetHeight(j - 1, this.m_tsCurrent);
                        this.m_iTopmostRowOffset -= num5;
                    }
                    else
                    {
                        int num6 = this.m_rcRows.GetHeight(j, this.m_tsCurrent);
                        if (Math.Abs(this.m_iTopmostRowOffset) < num6)
                        {
                            this.m_iActualRowIndex = j;
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

        public void CloseConnector()
        {
            if (this.m_connector.IsOpen)
            {
                this.m_connector.Close();
            }
        }

        private int CustomizeHeaderFooter(RowTemplate t, Row r, ref bool bResetScrollbar)
        {
            int num2 = -1;
            if (t.CustomizeCells(r))
            {
                num2 = t.GetHeight(r);
                r.ResetCachedBounds();
            }
            int height = t.GetHeight(r);
            if ((num2 >= 0) && (height != num2))
            {
                bResetScrollbar = true;
            }
            return height;
        }

        private void DeleteItem(int index)
        {
            if (this.m_rcRows != null)
            {
                this.m_rcRows.RemoveAt(index);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.m_TouchScrollingTimer != null)
            {
                this.m_TouchScrollingTimer.Dispose();
                this.m_TouchScrollingTimer = null;
            }
            if (this.m_dbConnector != null)
            {
                this.m_dbConnector.Dispose();
                this.m_dbConnector = null;
            }
            if (this.m_rcRows != null)
            {
                this.m_rcRows.Parent = null;
            }
            this.m_rcRows = null;
            if (this.m_tsCurrent != null)
            {
                this.m_tsCurrent.Changed -= new GridEventHandler(this.OnChange);
                this.m_tsCurrent.Parent = null;
            }
            this.m_tsCurrent = null;
            BackBufferManager.Release();
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
            Resco.Controls.AdvancedList.Mapping.DisposeEmptyMapping();
            RowTemplate.DisposeDefaultRowTemplate();
            ImageCache.GlobalCache.Clear();
            if (sBrushes != null)
            {
                sBrushes.Clear();
            }
            sBrushes = null;
            this.RemoveActiveHandlers();
            if (this.m_rHeader != null)
            {
                this.m_rHeader.Parent = null;
            }
            this.m_rHeader = null;
            if (this.m_rFooter != null)
            {
                this.m_rFooter.Parent = null;
            }
            this.m_rFooter = null;
            this.OnChangeHandler = null;
            this.OnRowRemovedHandler = null;
            if (this.m_gradientBackColor != null)
            {
                this.m_gradientBackColor.PropertyChanged -= new EventHandler(this.m_gradientBackColor_PropertyChanged);
            }
            this.m_gradientBackColor = null;
            if (sPen != null)
            {
                sPen.Dispose();
            }
            sPen = null;
            if (sPixel != null)
            {
                sPixel.Dispose();
            }
            sPixel = null;
            GC.Collect();
            base.Dispose(disposing);
        }

        private bool DoDelayLoad()
        {
            bool flag = true;
            if (this.m_connector.IsOpen)
            {
                Cursor.Current = Cursors.WaitCursor;
                this.BeginUpdate();
                try
                {
                    if (this.LoadDataChunk(true))
                    {
                        this.CloseConnector();
                        flag = false;
                    }
                }
                catch
                {
                    this.CloseConnector();
                    flag = false;
                }
                this.EndUpdate();
                Cursor.Current = Cursors.Default;
                this.OnDataLoaded(new DataLoadedEventArgs(!flag));
            }
            else
            {
                Rectangle rectangle = this.CalculateClientRect();
                if ((!this.m_bManualDataLoading && (((this.m_enumerator == null) || !(this.m_dataSource is IEnumerable)) || !this.m_enumeratorNeedLoad)) && (((this.m_dataSource == null) || (this.m_listManager == null)) || (this.m_nRowsLoaded >= this.m_listManager.List.Count)))
                {
                    flag = false;
                }
                else
                {
                    int height;
                    Cursor.Current = Cursors.WaitCursor;
                    this.BeginUpdate();
                    if (this.m_bManualDataLoading)
                    {
                        for (height = rectangle.Height; height > 0; height -= this.m_rcRows[this.m_rcRows.Count - 1].GetHeight(this.Templates))
                        {
                            this.m_bManualDataLoading = !this.AddRowManually();
                            if (!this.m_bManualDataLoading)
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    else if (this.m_enumerator != null)
                    {
                        height = rectangle.Height;
                        while (flag = this.m_enumeratorNeedLoad = this.m_enumerator.MoveNext())
                        {
                            if (this.m_boundMap == Resco.Controls.AdvancedList.Mapping.Empty)
                            {
                                this.m_boundMap = new PropertyMapping(this.m_enumerator.Current.GetType());
                            }
                            Row row = new BoundRow(this.TemplateIndex, this.SelectedTemplateIndex, this.m_enumerator.Current, this.m_boundMap as PropertyMapping);
                            row.AlternateTemplateIndex = this.AlternateTemplateIndex;
                            row.ActiveTemplateIndex = this.ActiveTemplateIndex;
                            this.m_nRowsInserted = this.m_rcRows.Count;
                            int num2 = this.InsertRow(row, this.m_nRowsInserted);
                            this.m_nRowsLoaded++;
                            if (num2 != this.m_nRowsInserted)
                            {
                                height -= this.m_rcRows[this.m_nRowsInserted].GetHeight(this.Templates);
                                if (height < 0)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        height = rectangle.Height;
                        while (this.m_nRowsLoaded < this.m_listManager.List.Count)
                        {
                            Row row2 = new BoundRow(this.TemplateIndex, this.SelectedTemplateIndex, this.m_listManager.List[this.m_nRowsLoaded], this.m_boundMap as PropertyMapping);
                            row2.AlternateTemplateIndex = this.AlternateTemplateIndex;
                            row2.ActiveTemplateIndex = this.ActiveTemplateIndex;
                            this.m_nRowsInserted = this.m_rcRows.Count;
                            if (this.InsertRow(row2, this.m_nRowsInserted) != this.m_nRowsInserted)
                            {
                                height -= this.m_rcRows[this.m_nRowsInserted].GetHeight(this.Templates);
                                if (height < 0)
                                {
                                    this.m_nRowsLoaded++;
                                    break;
                                }
                            }
                            this.m_nRowsLoaded++;
                        }
                        if (this.m_listManager.List.Count == this.m_nRowsLoaded)
                        {
                            flag = false;
                        }
                    }
                    if (this.m_nRowsLoaded >= this.m_iExpectedRows)
                    {
                        this.m_iExpectedRows = -1;
                    }
                    this.EndUpdate();
                    Cursor.Current = Cursors.Default;
                    this.OnDataLoaded(new DataLoadedEventArgs(!flag));
                }
            }
            if (!flag)
            {
                this.ExpectedRows = -1;
            }
            return flag;
        }

        protected void DrawBackgroundImage(Graphics gr)
        {
            Rectangle srcRect = new Rectangle(0, 0, this.m_bgImage.Width, this.m_bgImage.Height);
            Rectangle rectangle2 = this.CalculateClientRect();
            Rectangle destRect = new Rectangle(rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height);
            gr.DrawImage(this.m_bgImage, destRect, srcRect, GraphicsUnit.Pixel);
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
                this.OnChange(this, GridEventArgsType.Refresh, new RefreshData(true));
            }
            return true;
        }

        public int EnsureVisible(Row row)
        {
            int index = this.m_rcRows.IndexOf(row);
            return this.EnsureVisible(index, false);
        }

        public int EnsureVisible(int ix)
        {
            return this.EnsureVisible(ix, false);
        }

        public int EnsureVisible(Row row, bool bTop)
        {
            int index = this.m_rcRows.IndexOf(row);
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
            int headerHeight = this.HeaderHeight;
            if (!bTop && this.IsVisible(ix))
            {
                for (int i = 0; i < ix; i++)
                {
                    headerHeight += this.m_rcRows.GetHeight(i, this.m_tsCurrent);
                }
                return headerHeight;
            }
            int offset = 0;
            if (!bTop && (ix > this.m_iActualRowIndex))
            {
                offset = (((this.CalculateClientRect().Height - this.HeaderHeight) - this.FooterHeight) - this.m_rcRows[ix].GetHeight(this.Templates)) - (this.GridLines ? 1 : 0);
                if (offset < 0)
                {
                    offset = 0;
                }
            }
            return this.SetScrollPos(ix, cx, offset);
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
            Row rHeader = null;
            Rectangle rectangle = this.CalculateClientRect();
            pt.Y -= rectangle.Y;
            pt.X -= rectangle.X + (this.RightToLeft ? this.m_vScrollWidth : 0);
            if (pt.Y < this.HeaderHeight)
            {
                rHeader = this.m_rHeader;
                RowTemplate template = rHeader.GetTemplate(this.m_tsCurrent);
                int ri = -1;
                int ci = template.GetCellClick(pt.X, pt.Y, rHeader);
                return new CellEventArgs(rHeader, (ci >= 0) ? template[ci] : null, ri, ci, 0);
            }
            if (pt.Y > (num = rectangle.Height - this.FooterHeight))
            {
                rHeader = this.m_rFooter;
                RowTemplate template2 = rHeader.GetTemplate(this.m_tsCurrent);
                int num4 = -2;
                int num5 = template2.GetCellClick(pt.X, pt.Y - num, rHeader);
                return new CellEventArgs(rHeader, (num5 >= 0) ? template2[num5] : null, num4, num5, num);
            }
            Point point = this.m_rcRows.GetRowClick(this.m_iActualRowIndex, this.m_iTopmostRowOffset + this.HeaderHeight, pt.X, pt.Y, out num);
            if (point.X >= 0)
            {
                rHeader = this.m_rcRows[point.X];
            }
            if (rHeader != null)
            {
                return new CellEventArgs(rHeader, (point.Y >= 0) ? rHeader.GetTemplate(this.m_tsCurrent)[point.Y] : null, point.X, point.Y, num + rectangle.Y);
            }
            return null;
        }

        public Rectangle GetCellBounds(int rowIndex, int cellIndex)
        {
            Rectangle rectangle = this.CalculateClientRect();
            int rowOffset = this.GetRowOffset(rowIndex);
            if (((rowOffset == -2147483648) || (rowOffset == 0x7fffffff)) || (rowOffset > (rectangle.Height - this.FooterHeight)))
            {
                return Rectangle.Empty;
            }
            Row row = this.m_rcRows[rowIndex];
            RowTemplate template = row.GetTemplate(this.m_tsCurrent);
            int height = template.GetHeight(row);
            if ((rowOffset + height) < 0)
            {
                return Rectangle.Empty;
            }
            if ((cellIndex < 0) || (cellIndex >= template.CellTemplates.Count))
            {
                return new Rectangle(rectangle.X, rowOffset + rectangle.Y, rectangle.Width - this.m_vScrollWidth, template.Height);
            }
            Rectangle rectangle2 = template.CellTemplates[cellIndex].CalculateBounds(0, rowOffset, row, rectangle.Width - this.m_vScrollWidth, null);
            if ((rectangle2.Y < this.HeaderHeight) || ((rectangle2.Y + rectangle2.Height) > (rectangle.Height - this.FooterHeight)))
            {
                return Rectangle.Empty;
            }
            rectangle2.X += rectangle.X + (this.RightToLeft ? this.m_vScrollWidth : 0);
            rectangle2.Y += rectangle.Y;
            return rectangle2;
        }

        public static Pen GetPen(Color c)
        {
            if (sPen == null)
            {
                return (sPen = new Pen(c));
            }
            sPen.Color = c;
            return sPen;
        }

        private int GetRowOffset(int rowIndex)
        {
            if ((rowIndex < 0) || (rowIndex >= this.m_rcRows.Count))
            {
                return -2147483648;
            }
            if (rowIndex < this.m_iActualRowIndex)
            {
                return -2147483648;
            }
            if (rowIndex == this.m_iActualRowIndex)
            {
                return (this.m_iTopmostRowOffset + this.HeaderHeight);
            }
            Rectangle rectangle = this.CalculateClientRect();
            int num = this.m_iTopmostRowOffset + this.HeaderHeight;
            int num2 = rectangle.Height - this.FooterHeight;
            for (int i = this.m_iActualRowIndex; i < rowIndex; i++)
            {
                if (num > num2)
                {
                    return 0x7fffffff;
                }
                Row row = this.m_rcRows[i];
                int height = row.GetTemplate(this.m_tsCurrent).GetHeight(row);
                num += height;
                if (this.m_rcRows.m_bDrawGrid)
                {
                    num++;
                }
            }
            return num;
        }

        private uint GetTicks()
        {
            return (uint) Environment.TickCount;
        }

        private void HandleContextMenu(CellEventArgs cea, Point pos)
        {
            if (this.ContextMenu != null)
            {
                if (cea == null)
                {
                    cea = this.GetCellAtPoint(pos);
                }
                this.HandleRowSelection(cea, this.SelectionMode == Resco.Controls.AdvancedList.SelectionMode.SelectDeselect, true);
                this.m_bShowingContextMenu = true;
                this.ContextMenu.Show(this, pos);
            }
        }

        private void HandleRowSelection(CellEventArgs cea, bool bDisableDeselecting, bool bDisableRowEntered)
        {
            if ((cea != null) && (cea.DataRow != null))
            {
                Row dataRow = cea.DataRow;
                int currentTemplateIndex = dataRow.CurrentTemplateIndex;
                if (this.m_mode != Resco.Controls.AdvancedList.SelectionMode.NoSelect)
                {
                    bool active = dataRow.Active;
                    bool selected = dataRow.Selected;
                    this.SuspendRedraw();
                    if (this.m_mode == Resco.Controls.AdvancedList.SelectionMode.SelectDeselect)
                    {
                        if (dataRow.Selected && bDisableDeselecting)
                        {
                            this.ResumeRedraw();
                            return;
                        }
                        dataRow.Selected = !dataRow.Selected;
                    }
                    else if (!dataRow.Selected)
                    {
                        dataRow.Selected = true;
                    }
                    else if (!dataRow.Active)
                    {
                        dataRow.Active = true;
                    }
                    this.ResumeRedraw();
                    this.OnRowSelect(cea.DataRow, cea.RowIndex, cea.Offset, dataRow.Selected != selected, dataRow.Active != active);
                }
                if (((currentTemplateIndex == dataRow.CurrentTemplateIndex) && (cea.CellIndex >= 0)) && (this.CellClick != null))
                {
                    this.CellClick(this, cea);
                }
                if ((!bDisableRowEntered && (this.m_rcRows != null)) && ((this.m_mode == Resco.Controls.AdvancedList.SelectionMode.SelectOnly) || ((this.m_mode == Resco.Controls.AdvancedList.SelectionMode.SelectDeselect) && !dataRow.Selected)))
                {
                    this.OnRowEntered(new RowEnteredEventArgs(cea.DataRow, cea.RowIndex));
                    if ((cea.CellIndex >= 0) && cea.Cell.IsSelectable(cea.DataRow))
                    {
                        try
                        {
                            this.SelectedCell = cea.Cell;
                        }
                        catch
                        {
                        }
                        this.OnCellEntered(new CellEnteredMainEventArgs(cea.Cell, cea.CellIndex, cea.DataRow));
                    }
                    else
                    {
                        this.SelectedCell = null;
                    }
                }
            }
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

        private void InsertItem(int index)
        {
            if (this.m_listManager != null)
            {
                Row row = new BoundRow(this.TemplateIndex, this.SelectedTemplateIndex, this.m_listManager.List[index], this.m_boundMap as PropertyMapping);
                row.ActiveTemplateIndex = this.ActiveTemplateIndex;
                row.AlternateTemplateIndex = this.AlternateTemplateIndex;
                this.InsertRow(row, index);
            }
            else
            {
                this.ReloadDataSource();
            }
        }

        public int InsertRow(Row row, int insertIndex)
        {
            ValidateDataArgs e = new ValidateDataArgs(row, insertIndex);
            this.OnValidateData(e);
            if (!e.Skip)
            {
                insertIndex = e.InsertIndex;
                if (insertIndex < 0)
                {
                    this.m_rcRows.Add(e.DataRow);
                    return insertIndex;
                }
                this.m_rcRows.Insert(insertIndex++, e.DataRow);
            }
            return insertIndex;
        }

        public bool IsVisible(Row row)
        {
            return this.IsVisible(this.m_rcRows.IndexOf(row));
        }

        public bool IsVisible(int ix)
        {
            if (ix < this.m_iActualRowIndex)
            {
                return false;
            }
            if (ix == this.m_iActualRowIndex)
            {
                return (this.m_iTopmostRowOffset == 0);
            }
            if (this.m_rcRows.LastDrawnRow == 0)
            {
                int num = this.m_iTopmostRowOffset + this.HeaderHeight;
                int num2 = this.CalculateClientRect().Height - this.FooterHeight;
                this.m_rcRows.LastDrawnRow = this.m_iActualRowIndex;
                while (this.m_rcRows.LastDrawnRow < this.m_rcRows.Count)
                {
                    Row row = this.m_rcRows[this.m_rcRows.LastDrawnRow];
                    int height = row.GetTemplate(this.m_tsCurrent).GetHeight(row);
                    num += height;
                    if (this.m_rcRows.m_bDrawGrid)
                    {
                        num++;
                    }
                    if (num > num2)
                    {
                        break;
                    }
                    this.m_rcRows.LastDrawnRow++;
                }
            }
            return (ix < this.m_rcRows.LastDrawnRow);
        }

        public Resco.Controls.AdvancedList.Mapping LoadData()
        {
            return this.LoadData(-1);
        }

        public Resco.Controls.AdvancedList.Mapping LoadData(int iInsertIndex)
        {
            if (this.m_connector == null)
            {
                return null;
            }
            try
            {
                this.CloseConnector();
                if (!this.m_connector.Open())
                {
                    return null;
                }
                this.m_mapLast = this.m_connector.Mapping;
                this.BeginUpdate();
                this.m_iInsertIndex = iInsertIndex;
                if (this.LoadDataChunk(this.m_bDelayLoad && (iInsertIndex <= this.m_rcRows.Count)))
                {
                    this.CloseConnector();
                }
                else if (this.LoadDataChunk(this.m_bDelayLoad && (iInsertIndex <= this.m_rcRows.Count)))
                {
                    this.CloseConnector();
                }
            }
            catch (Exception exception)
            {
                this.CloseConnector();
                throw exception;
            }
            finally
            {
                this.EndUpdate();
            }
            return this.m_mapLast;
        }

        private bool LoadDataChunk(bool bDelay)
        {
            int height = this.CalculateClientRect().Height;
            while (this.m_connector.MoveNext())
            {
                Row row = new Row(this.TemplateIndex, this.SelectedTemplateIndex, this.m_connector.Current, this.m_mapLast);
                row.AlternateTemplateIndex = this.AlternateTemplateIndex;
                row.ActiveTemplateIndex = this.ActiveTemplateIndex;
                this.m_iInsertIndex = this.InsertRow(row, this.m_iInsertIndex);
                height -= row.GetHeight(this.Templates);
                if (bDelay && (height < 0))
                {
                    return false;
                }
            }
            return true;
        }

        public void LoadDataManually()
        {
            if (this.RowAdding == null)
            {
                InvalidOperationException exception = new InvalidOperationException("RowAdding event does not have a handler.");
                throw exception;
            }
            if (!this.m_bDelayLoad)
            {
                this.BeginUpdate();
                while (!this.AddRowManually())
                {
                }
                this.EndUpdate();
            }
            else
            {
                this.m_bManualDataLoading = true;
                this.OnChange(null, GridEventArgsType.Repaint, null);
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
                            if (name == "AdvancedList")
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
                    this.ReadAdvancedList(reader);
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
            this.OnChange(this, GridEventArgsType.Repaint, null);
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

        private void m_innerListManager_PositionChanged(object sender, EventArgs e)
        {
            this.ActiveRowIndex = this.m_innerListManager.Position;
        }

        private void m_vScroll_ValueChanged(object sender, EventArgs e)
        {
            bool flag = this.m_iVScrollPrevValue <= this.m_vScroll.Value;
            this.bIsInScrollChange = true;
            this.OnChange(this, GridEventArgsType.VScroll, this.m_iVScrollPrevValue - this.m_vScroll.Value);
            this.OnScroll();
            if ((this.DelayLoad && flag) && (this.m_vScroll.Value > (this.m_vScroll.Maximum - (2 * this.m_vScroll.LargeChange))))
            {
                this.DoDelayLoad();
            }
            this.bIsInScrollChange = false;
        }

        public bool NextSelectableCell()
        {
            if (this.ActiveRowIndex >= 0)
            {
                int index;
                RowTemplate template = this.Templates[this.ActiveRow.CurrentTemplateIndex];
                RowTemplate.CellCollection cellTemplates = template.CellTemplates;
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
                    RowSpecificCellProperties rowSpecificProperties = cellTemplates[index].GetRowSpecificProperties(this.ActiveRow);
                    if (rowSpecificProperties.Selectable.Value && rowSpecificProperties.Visible.Value)
                    {
                        this.SelectedCell = cellTemplates[index];
                        return true;
                    }
                }
                this.SelectedCell = null;
            }
            return false;
        }

        internal void OnAutoHeightChanged(int nRowIndex, int iDiff)
        {
            this.m_bIsChange = true;
            if (this.m_iUpdate <= 0)
            {
                bool flag = false;
                int iOffset = 0;
                bool flag2 = false;
                this.m_iDocumentHeight += iDiff;
                if (nRowIndex < this.m_iActualRowIndex)
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
                    this.CalculateFirstRow(iOffset);
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

        protected override void OnBindingContextChanged(EventArgs e)
        {
            this.DataSource = this.m_dataSource;
            base.OnBindingContextChanged(e);
        }

        protected virtual void OnButton(ButtonCell c, Row r, Point index, int yOffset)
        {
            CellEventArgs e = new CellEventArgs(r, c, index.X, index.Y, yOffset);
            if (this.ButtonClick != null)
            {
                this.ButtonClick(this, e);
            }
        }

        protected virtual void OnCellEntered(CellEnteredMainEventArgs e)
        {
            object constantData;
            switch (e.Cell.CellSource.SourceType)
            {
                case CellSourceType.Constant:
                    constantData = e.Cell.CellSource.ConstantData;
                    break;

                case CellSourceType.ColumnIndex:
                    constantData = e.Row[e.Cell.CellSource.ColumnIndex];
                    break;

                case CellSourceType.ColumnName:
                    constantData = e.Row[e.Cell.CellSource.ColumnName];
                    break;

                default:
                    constantData = null;
                    break;
            }
            e.Cell._FireCellEntered(new CellEnteredEventArgs(e.Cell, e.Row, constantData));
            if (this.CellEntered != null)
            {
                this.CellEntered(this, e);
            }
        }

        private void OnChange(object sender, GridEventArgsType e, object oParam)
        {
            if (this.InvokeRequired && (this.OnChangeHandler != null))
            {
                base.Invoke(this.OnChangeHandler, new object[] { sender, e, oParam });
            }
            else
            {
                this.OnChangeSafe(sender, e, oParam);
            }
        }

        private void OnChangeSafe(object sender, GridEventArgsType e, object oParam)
        {
            this.m_bIsChange = true;
            if (this.m_iUpdate <= 0)
            {
                bool flag = false;
                int iOffset = 0;
                bool flag2 = false;
                switch (e)
                {
                    case GridEventArgsType.Resize:
                    {
                        int num2 = (int) oParam;
                        this.m_iDocumentHeight += num2;
                        if (((sender is int) && (((int) sender) < this.m_iActualRowIndex)) && (this.m_vScroll != null))
                        {
                            this.m_iVScrollPrevValue += num2;
                            this.m_vScroll.Value += num2;
                        }
                        if (this.SetVScrollBar(this.DocumentHeight))
                        {
                            flag = true;
                            iOffset = this.m_iVScrollPrevValue - this.m_vScroll.Value;
                            flag2 = true;
                        }
                        this.m_rcRows.LastDrawnRow = 0;
                        break;
                    }
                    case GridEventArgsType.Refresh:
                    {
                        RefreshData data = oParam as RefreshData;
                        if (this.m_rHeader != null)
                        {
                            this.m_rHeader.ResetCachedBounds();
                        }
                        if (this.m_rFooter != null)
                        {
                            this.m_rFooter.ResetCachedBounds();
                        }
                        if (this.m_rcRows != null)
                        {
                            if ((data != null) && data.ResetBounds)
                            {
                                this.m_rcRows.ResetCachedBounds(data.Template);
                            }
                            this.m_iDocumentHeight = this.m_rcRows.CalculateRowsHeight();
                            if (this.SetVScrollBar(this.DocumentHeight))
                            {
                                this.m_rcRows.ResetCachedBounds((data == null) ? null : data.Template);
                                this.m_iDocumentHeight = this.m_rcRows.CalculateRowsHeight();
                                this.SetVScrollBar(this.DocumentHeight);
                            }
                        }
                        if (this.m_iScrollChange == 0)
                        {
                            this.m_rcRows.LastDrawnRow = 0;
                        }
                        this.SetScrollPos(this.m_iActualRowIndex, -1, this.m_iTopmostRowOffset);
                        break;
                    }
                    case GridEventArgsType.VScroll:
                        flag = true;
                        iOffset = (int) oParam;
                        break;

                    default:
                        this.SetVScrollBarBounds();
                        break;
                }
                if (flag)
                {
                    this.CalculateFirstRow(iOffset);
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

        internal void OnClear(object sender)
        {
            this.m_iActualRowIndex = 0;
            this.m_iTopmostRowOffset = 0;
            this.m_iVScrollPrevValue = 0;
            if (this.m_vScroll != null)
            {
                this.m_vScrollWidth = 0;
                this.VScrollBarVisible = false;
            }
            this.OnChange(sender, GridEventArgsType.Refresh, new RefreshData(true));
        }

        protected override void OnClick(EventArgs e)
        {
            this.m_Timer.Enabled = false;
            if ((!this.m_bShowingToolTip && !this.m_bStartingTouchScroll) && (!this.m_bTouchScrolling && !this.m_bShowingContextMenu))
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
                                this.OnLink(c, cellAtPoint.DataRow, new Point(cellAtPoint.RowIndex, cellAtPoint.CellIndex), cellAtPoint.Offset);
                            }
                            return;
                        }
                        if (this.CheckForButton(pt))
                        {
                            return;
                        }
                    }
                    switch (cellAtPoint.RowIndex)
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
                    this.HandleRowSelection(cellAtPoint, false, false);
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

        protected virtual void OnDataLoaded(DataLoadedEventArgs e)
        {
            if (this.DataLoaded != null)
            {
                this.DataLoaded(this, e);
            }
        }

        private void OnGyroNavigationTimerTick(object sender, EventArgs e)
        {
            if (this.m_HTCGyroSensor != null)
            {
                HTCGSensorData rawSensorData = this.m_HTCGyroSensor.GetRawSensorData();
                this.m_HTCGyroStatus.X = rawSensorData.TiltX;
                this.m_HTCGyroStatus.Y = rawSensorData.TiltY;
                int hTCScreenOrientation = (int) this.m_HTCScreenOrientation;
                this.m_HTCScreenOrientation = (Resco.Controls.AdvancedList.HTCScreenOrientation) rawSensorData.ScreenOrientation;
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
                bool flag = (this.m_HTCGyroSensorNavigation && (this.SelectionMode == Resco.Controls.AdvancedList.SelectionMode.SelectOnly)) && !this.MultiSelect;
                this.m_HTCGytoEventArgs.Direction = HTCDirection.None;
                bool gyroScrolling = this.m_gyroScrolling;
                this.m_gyroScrolling = false;
                if (num2 > this.m_HTCGyroSensitivity.Y)
                {
                    if (flag)
                    {
                        this.ActiveRowIndex++;
                    }
                    this.m_gyroScrolling = true;
                    this.m_HTCGytoEventArgs.Direction |= HTCDirection.Down;
                }
                if (num2 < -this.m_HTCGyroSensitivity.Y)
                {
                    if (flag && (this.ActiveRowIndex > 0))
                    {
                        this.ActiveRowIndex--;
                    }
                    this.m_gyroScrolling = true;
                    this.m_HTCGytoEventArgs.Direction |= HTCDirection.Up;
                }
                if (num3 > this.m_HTCGyroSensitivity.X)
                {
                    if (flag)
                    {
                        if (this.m_rightToLeft)
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
                    if (flag)
                    {
                        if (this.m_rightToLeft)
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
                        this.ActiveRowIndex++;
                    }
                    else if (this.ActiveRowIndex > 0)
                    {
                        this.ActiveRowIndex--;
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            this.m_TouchScrollingTimer.Enabled = false;
            if ((this.KeyNavigation && (this.SelectionMode == Resco.Controls.AdvancedList.SelectionMode.SelectOnly)) || ((this.SelectionMode == Resco.Controls.AdvancedList.SelectionMode.SelectDeselect) && !this.MultiSelect))
            {
                int activeRowIndex = this.ActiveRowIndex;
                switch (e.KeyCode)
                {
                    case Keys.Prior:
                    case Keys.PageDown:
                    case Keys.End:
                    case Keys.Home:
                        return;

                    case Keys.Left:
                        if (!this.m_rightToLeft)
                        {
                            this.PreviousSelectableCell();
                            return;
                        }
                        this.NextSelectableCell();
                        return;

                    case Keys.Up:
                        if (activeRowIndex > 0)
                        {
                            this.ActiveRowIndex--;
                        }
                        return;

                    case Keys.Right:
                        if (!this.m_rightToLeft)
                        {
                            this.NextSelectableCell();
                            return;
                        }
                        this.PreviousSelectableCell();
                        return;

                    case Keys.Down:
                        this.ActiveRowIndex++;
                        return;

                    case Keys.Return:
                        if (((this.m_rcRows != null) && (this.SelectedCell != null)) && ((this.SelectedCell is ButtonCell) && this.SelectedCell.IsSelectable(this.ActiveRow)))
                        {
                            this.m_pressedButtonRowIndex = this.ActiveRowIndex;
                            this.ActiveRow.PressedButtonIndex = this.SelectedCellIndex;
                            this.OnChange(this, GridEventArgsType.Repaint, null);
                        }
                        return;

                    default:
                        return;
                }
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if ((this.KeyNavigation && (this.SelectionMode == Resco.Controls.AdvancedList.SelectionMode.SelectOnly)) && (!this.MultiSelect && (e.KeyCode == Keys.Return)))
            {
                this.OnRowEntered(new RowEnteredEventArgs(this.ActiveRow, this.ActiveRowIndex));
                if ((this.m_rcRows != null) && (this.SelectedCell != null))
                {
                    RowTemplate.CellCollection cellTemplates = this.Templates[this.ActiveRow.CurrentTemplateIndex].CellTemplates;
                    if (this.SelectedCell.IsSelectable(this.ActiveRow))
                    {
                        this.OnCellEntered(new CellEnteredMainEventArgs(this.SelectedCell, cellTemplates.IndexOf(this.SelectedCell), this.ActiveRow));
                    }
                    if (this.SelectedCell is ButtonCell)
                    {
                        int pressedButtonIndex = this.ActiveRow.PressedButtonIndex;
                        this.m_pressedButtonRowIndex = -3;
                        this.ActiveRow.PressedButtonIndex = -1;
                        this.OnChange(this, GridEventArgsType.Repaint, null);
                        this.OnButton((ButtonCell) this.SelectedCell, this.ActiveRow, new Point(this.ActiveRowIndex, pressedButtonIndex), 0);
                    }
                }
            }
        }

        protected virtual void OnLink(LinkCell c, Row r, Point index, int yOffset)
        {
            Rectangle rectangle = this.CalculateClientRect();
            int width = rectangle.Width;
            if (this.m_bScrollbarOverlap || ((!this.ShowHeader || (this.HeaderRow.Template != c.Owner)) && (!this.ShowFooter || (this.FooterRow.Template != c.Owner))))
            {
                width -= this.m_vScrollWidth;
            }
            LinkEventArgs lea = new LinkEventArgs(r, c, index.X, index.Y, yOffset);
            if (this.m_doubleBuffered)
            {
                this.Redraw(BackBufferManager.GetBackBufferGraphics(rectangle.Width, rectangle.Height));
                c.DrawActiveLink(BackBufferManager.GetBackBufferGraphics(rectangle.Width, rectangle.Height), lea, width);
            }
            else
            {
                using (Graphics graphics = base.CreateGraphics())
                {
                    c.DrawActiveLink(graphics, lea, width);
                }
            }
            this.Refresh();
            if (this.LinkClick != null)
            {
                this.LinkClick(this, lea);
            }
            Links.AddLink(lea.Target);
            this.OnChange(this, GridEventArgsType.Repaint, null);
        }

        internal void OnListChanged(object sender, ListChangedEventArgs args)
        {
            switch (args.ListChangedType)
            {
                case ListChangedType.Reset:
                    try
                    {
                        this.BeginUpdate();
                        this.ReloadDataSource();
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                    return;

                case ListChangedType.ItemAdded:
                    try
                    {
                        this.BeginUpdate();
                        this.InsertItem(args.NewIndex);
                        this.ActiveRowIndex = this.m_innerListManager.Position;
                        this.ShiftBoundRowsForCF20(args.NewIndex, this.m_rcRows.Count);
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                    return;

                case ListChangedType.ItemDeleted:
                    try
                    {
                        this.BeginUpdate();
                        this.DeleteItem(args.NewIndex);
                        this.ActiveRowIndex = this.m_innerListManager.Position;
                        this.ShiftBoundRowsForCF20(args.NewIndex, this.m_rcRows.Count);
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                    return;

                case ListChangedType.ItemMoved:
                    try
                    {
                        int oldIndex;
                        int newIndex;
                        this.BeginUpdate();
                        this.DeleteItem(args.OldIndex);
                        this.InsertItem(args.NewIndex);
                        if (args.OldIndex < args.NewIndex)
                        {
                            oldIndex = args.OldIndex;
                            newIndex = args.NewIndex;
                        }
                        else
                        {
                            oldIndex = args.NewIndex;
                            newIndex = args.OldIndex;
                        }
                        this.ShiftBoundRowsForCF20(oldIndex, newIndex);
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                    return;

                case ListChangedType.ItemChanged:
                    if ((args.NewIndex >= 0) && (args.NewIndex < this.m_rcRows.Count))
                    {
                        this.m_rcRows[args.NewIndex].ResetCachedBounds();
                    }
                    if ((args.NewIndex >= this.m_iActualRowIndex) && (args.NewIndex <= this.m_rcRows.LastDrawnRow))
                    {
                        this.OnChange(this, GridEventArgsType.Repaint, null);
                    }
                    return;

                case ListChangedType.PropertyDescriptorAdded:
                case ListChangedType.PropertyDescriptorDeleted:
                case ListChangedType.PropertyDescriptorChanged:
                    this.BindTo(this.m_dataSource);
                    return;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.m_bFocusOnClick)
            {
                base.Focus();
            }
            this.m_bShowingContextMenu = false;
            this.m_MousePosition.X = e.X;
            this.m_MousePosition.Y = e.Y;
            if (this.m_bEnableTouchScrolling && (e.Button == MouseButtons.Left))
            {
                this.m_TouchScrollingTimer.Enabled = false;
                this.m_TouchAutoScrollDiff = 0;
                this.m_TouchTime0 = this.GetTicks();
                this.m_TouchTime1 = this.m_TouchTime0;
                if (this.m_bTouchScrolling)
                {
                    return;
                }
            }
            Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
            CellEventArgs cellAtPoint = this.GetCellAtPoint(pt);
            if (((cellAtPoint != null) && (cellAtPoint.Cell != null)) && this.CheckForButton(pt))
            {
                this.m_pressedButtonRowIndex = cellAtPoint.RowIndex;
                cellAtPoint.DataRow.PressedButtonIndex = cellAtPoint.CellIndex;
                this.OnChange(this, GridEventArgsType.Repaint, null);
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
                        this.HandleContextMenu(cellAtPoint, new Point(e.X, e.Y));
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
            if ((this.m_bEnableTouchScrolling && (e.Button == MouseButtons.Left)) && (e.Y != this.m_MousePosition.Y))
            {
                int num = e.Y - this.m_MousePosition.Y;
                this.m_TouchAutoScrollDiff += num;
                if ((this.GetTicks() - this.m_TouchTime1) > 200)
                {
                    this.m_TouchAutoScrollDiff = 0;
                    this.m_TouchTime0 = this.GetTicks();
                    this.m_TouchTime1 = this.m_TouchTime0;
                }
                else
                {
                    this.m_TouchTime1 = this.GetTicks();
                }
                if (!this.m_bShowingToolTip && (this.m_bStartingTouchScroll || (Math.Abs(num) >= ((int) (this.m_touchSensitivity * this.m_scaleFactorY)))))
                {
                    this.m_Timer.Enabled = false;
                    this.m_bStartingTouchScroll = true;
                    this.m_MousePosition.X = e.X;
                    this.m_MousePosition.Y = e.Y;
                    if (this.m_vScroll != null)
                    {
                        int num2 = this.m_vScroll.Value;
                        int num3 = (this.m_vScroll.Maximum - this.m_vScroll.LargeChange) + 1;
                        if (this.m_touchScrollDirection == Resco.Controls.AdvancedList.TouchScrollDirection.Inverse)
                        {
                            num2 -= num;
                        }
                        else
                        {
                            num2 += num;
                        }
                        if (num2 < 0)
                        {
                            this.m_vScroll.Value = 0;
                        }
                        else if (num2 > num3)
                        {
                            this.m_vScroll.Value = num3;
                        }
                        else
                        {
                            this.m_vScroll.Value = num2;
                        }
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            try
            {
                base.OnMouseUp(e);
                this.m_bShowingContextMenu = false;
                if (this.m_Timer != null)
                {
                    this.m_Timer.Enabled = false;
                }
                if (this.m_pressedButtonRowIndex >= -2)
                {
                    Row rHeader;
                    if (this.m_pressedButtonRowIndex == -1)
                    {
                        rHeader = this.m_rHeader;
                    }
                    else if (this.m_pressedButtonRowIndex == -2)
                    {
                        rHeader = this.m_rFooter;
                    }
                    else
                    {
                        rHeader = this.m_rcRows[this.m_pressedButtonRowIndex];
                    }
                    int pressedButtonRowIndex = this.m_pressedButtonRowIndex;
                    int pressedButtonIndex = rHeader.PressedButtonIndex;
                    rHeader.PressedButtonIndex = -1;
                    this.m_pressedButtonRowIndex = -3;
                    if (!this.m_bStartingTouchScroll && !this.m_bTouchScrolling)
                    {
                        Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                        CellEventArgs cellAtPoint = this.GetCellAtPoint(pt);
                        if (((cellAtPoint != null) && (cellAtPoint.Cell is ButtonCell)) && ((cellAtPoint.RowIndex == pressedButtonRowIndex) && (cellAtPoint.CellIndex == pressedButtonIndex)))
                        {
                            this.OnButton((ButtonCell) cellAtPoint.Cell, cellAtPoint.DataRow, new Point(cellAtPoint.RowIndex, cellAtPoint.CellIndex), cellAtPoint.Offset);
                        }
                    }
                    this.OnChange(this, GridEventArgsType.Repaint, null);
                }
                if (this.m_bShowingToolTip)
                {
                    if (this.m_ToolTip != null)
                    {
                        this.m_ToolTip.Visible = false;
                    }
                    this.m_bShowingToolTip = false;
                }
                else if (this.m_bEnableTouchScrolling)
                {
                    if (this.m_bStartingTouchScroll)
                    {
                        if ((this.GetTicks() - this.m_TouchTime1) > 200)
                        {
                            this.m_TouchAutoScrollDiff = 0;
                        }
                        uint num3 = (this.GetTicks() - this.m_TouchTime0) / 50;
                        if (num3 > 0)
                        {
                            this.m_TouchAutoScrollDiff = (int) (this.m_TouchAutoScrollDiff / num3);
                        }
                        this.m_bStartingTouchScroll = false;
                        this.m_bTouchScrolling = true;
                        this.m_TouchScrollingTimer.Enabled = true;
                        this.OnTouchScrollingTimerTick(this, EventArgs.Empty);
                    }
                    else if (this.m_bTouchScrolling)
                    {
                        this.m_bTouchScrolling = false;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (((this.m_iUpdate == 0) && (this.m_iNoRedraw <= 0)) && (!this.Redraw(e.Graphics) && this.DelayLoad))
            {
                this.DoDelayLoad();
            }
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (this.m_rcRows != null)
            {
                this.EndUpdate();
            }
            if (this.m_dbConnector != null)
            {
                this.m_dbConnector.IsDesignTime = this.Site != null;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            if (this.m_vScroll != null)
            {
                this.SetVScrollBarBounds();
            }
            this.OnChange(this, GridEventArgsType.Refresh, new RefreshData(true));
            if (this.m_iUpdate == 0)
            {
                this.SetRedraw(0);
                this.m_rcRows.LastDrawnRow = -1;
            }
            base.OnResize(e);
            base.Invalidate();
        }

        internal void OnRowAdded(Row row, int index)
        {
            int height = row.GetTemplate(this.Templates).GetHeight(row);
            if (this.m_rcRows.m_bDrawGrid)
            {
                height++;
            }
            if (index < this.m_iActualRowIndex)
            {
                this.m_iActualRowIndex++;
            }
            this.OnChange(index, GridEventArgsType.Resize, height);
        }

        protected virtual void OnRowAdding(RowAddingEventArgs e)
        {
            if (this.RowAdding != null)
            {
                this.RowAdding(this, e);
            }
        }

        protected virtual void OnRowEntered(RowEnteredEventArgs e)
        {
            if (this.RowEntered != null)
            {
                this.RowEntered(this, e);
            }
        }

        internal void OnRowRemoved(Row row, int index)
        {
            if (this.InvokeRequired && (this.OnRowRemovedHandler != null))
            {
                base.Invoke(this.OnRowRemovedHandler, new object[] { row, index });
            }
            else
            {
                this.OnRowRemovedSafe(row, index);
            }
        }

        internal void OnRowRemovedSafe(Row row, int index)
        {
            int height = row.GetTemplate(this.Templates).GetHeight(row);
            if (this.m_rcRows.m_bDrawGrid)
            {
                height++;
            }
            if (index == this.m_iActualRowIndex)
            {
                this.EnsureVisible(this.m_iActualRowIndex);
            }
            if (index < this.m_iActualRowIndex)
            {
                this.m_iActualRowIndex--;
            }
            this.OnChange(index, GridEventArgsType.Resize, -height);
        }

        protected void OnRowSelect(Row row, int index, int yOffset, bool bSelectedChanged, bool bActiveChanged)
        {
            yOffset = this.EnsureVisible(index, false);
            if ((this.RowSelect != null) && bSelectedChanged)
            {
                this.RowSelect(this, new RowEventArgs(row, index, yOffset));
            }
            if (this.m_rcRows != null)
            {
                if ((this.m_innerListManager != null) && (this.ActiveRowIndex != -1))
                {
                    this.m_innerListManager.PositionChanged -= new EventHandler(this.m_innerListManager_PositionChanged);
                    this.m_innerListManager.Position = this.ActiveRowIndex;
                    this.m_innerListManager.PositionChanged += new EventHandler(this.m_innerListManager_PositionChanged);
                }
                if (bActiveChanged && (this.ActiveRowChanged != null))
                {
                    this.ActiveRowChanged(this, EventArgs.Empty);
                }
            }
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
                        this.m_ToolTip = new Resco.Controls.AdvancedList.ToolTip();
                        base.Controls.Add(this.m_ToolTip);
                    }
                    this.m_ToolTip.Text = str;
                    this.m_ToolTip.Show(mousePosition);
                    this.m_bShowingToolTip = true;
                }
                else
                {
                    this.HandleContextMenu(null, this.m_MousePosition);
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
                if (this.m_touchScrollDirection == Resco.Controls.AdvancedList.TouchScrollDirection.Inverse)
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
                    this.m_bTouchScrolling = false;
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
                    this.m_bTouchScrolling = false;
                    this.m_TouchScrollingTimer.Enabled = false;
                }
            }
            else if (this.m_TouchAutoScrollDiff > 0)
            {
                this.m_TouchAutoScrollDiff -= (Math.Abs(this.m_TouchAutoScrollDiff) / 10) + 1;
                if (this.m_TouchAutoScrollDiff < 0)
                {
                    this.m_TouchAutoScrollDiff = 0;
                    this.m_bTouchScrolling = false;
                    this.m_TouchScrollingTimer.Enabled = false;
                }
            }
            else
            {
                this.m_bTouchScrolling = false;
                this.m_TouchScrollingTimer.Enabled = false;
            }
        }

        protected virtual void OnValidateData(ValidateDataArgs e)
        {
            if (this.ValidateData != null)
            {
                this.ValidateData(this, e);
            }
        }

        public bool PreviousSelectableCell()
        {
            if (this.ActiveRowIndex >= 0)
            {
                Cell cell;
                RowSpecificCellProperties rowSpecificProperties;
                int index;
                RowTemplate template = this.Templates[this.ActiveRow.CurrentTemplateIndex];
                RowTemplate.CellCollection cellTemplates = template.CellTemplates;
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
                    cell = cellTemplates[index];
                    rowSpecificProperties = cell.GetRowSpecificProperties(this.ActiveRow);
                    if (rowSpecificProperties.Selectable.Value && rowSpecificProperties.Visible.Value)
                    {
                        this.SelectedCell = cell;
                        return true;
                    }
                }
                while (--index > -1)
                {
                    cell = cellTemplates[index];
                    rowSpecificProperties = cell.GetRowSpecificProperties(this.ActiveRow);
                    if (rowSpecificProperties.Selectable.Value && rowSpecificProperties.Visible.Value)
                    {
                        this.SelectedCell = cell;
                        return true;
                    }
                }
                this.SelectedCell = null;
            }
            return false;
        }

        private void ReadAdvancedList(XmlReader reader)
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
                if (this._designTimeCallback != null)
                {
                    this._designTimeCallback(this.Templates, null);
                }
                this.Templates.Clear();
                if (!reader.IsEmptyElement)
                {
                    goto Label_021A;
                }
                return;
            Label_006B:
                try
                {
                    string name = reader.Name;
                    if (name != null)
                    {
                        int num;
                        if (BigHas.methodxxx.TryGetValue(name, out num)) //(<PrivateImplementationDetails>{DC00B8B3-44E1-455D-9896-5D9179BE50F6}.$$method0x60002bd-1.TryGetValue(name, ref num))
                        {
                            switch (num)
                            {
                                case 0:
                                    goto Label_021A;

                                case 1:
                                    if (!reader.IsEmptyElement)
                                    {
                                        this.ReadConnector(reader);
                                    }
                                    goto Label_021A;

                                case 2:
                                    this.Templates.Add(this.ReadRowTemplate(reader));
                                    goto Label_021A;

                                case 3:
                                    return;

                                case 4:
                                    this.HeaderRow.TemplateIndex = Convert.ToInt32(reader.ReadString());
                                    goto Label_021A;

                                case 5:
                                    this.ReadHeaderRow(reader);
                                    goto Label_021A;

                                case 6:
                                    this.FooterRow.TemplateIndex = Convert.ToInt32(reader.ReadString());
                                    goto Label_021A;

                                case 7:
                                    this.ReadFooterRow(reader);
                                    goto Label_021A;

                                case 8:
                                    if ((reader.HasAttributes && (reader.GetAttribute("Name") != null)) && (reader.GetAttribute("Value") != null))
                                    {
                                        this.m_conversion.SetProperty(this, reader.GetAttribute("Name"), reader.GetAttribute("Value"));
                                    }
                                    goto Label_021A;
                            }
                        }
                        this.m_conversion.SetProperty(this, reader.Name, reader.ReadString());
                    }
                }
                catch
                {
                }
            Label_021A:
                if (reader.Read())
                {
                    goto Label_006B;
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
                    typeName = "Resco.Controls.AdvancedList.Cell";
                }
                if (!typeName.StartsWith("Resco.Controls.AdvancedList.") && typeName.StartsWith("Resco.Controls."))
                {
                    typeName = typeName.Insert(typeName.LastIndexOf('.'), ".AdvancedList");
                }
                ConstructorInfo info = Type.GetType(typeName).GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);
                if (info != null)
                {
                    o = (Cell) info.Invoke(new object[0]);
                }
                if ((this._designTimeCallback != null) && (o != null))
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
                    goto Label_012F;
                }
                return o;
            Label_00C9:
                try
                {
                    string str4;
                    if (((str4 = reader.Name) == null) || (str4 == ""))
                    {
                        goto Label_012F;
                    }
                    if (!(str4 == "Cell"))
                    {
                        if (str4 == "Property")
                        {
                            goto Label_0108;
                        }
                        goto Label_012F;
                    }
                    return o;
                Label_0108:
                    this.m_conversion.SetProperty(o, reader["Name"], reader["Value"]);
                }
                catch
                {
                }
            Label_012F:
                if (reader.Read())
                {
                    goto Label_00C9;
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
                                goto Label_017F;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        Label_017F:
            if (o != null)
            {
                return o;
            }
            return new Cell();
        }

        private void ReadConnector(XmlReader reader)
        {
            while (reader.Read())
            {
                try
                {
                    string str;
                    if (((str = reader.Name) != null) && (str != ""))
                    {
                        if (str == "DbConnector")
                        {
                            break;
                        }
                        this.m_conversion.SetProperty(this.DbConnector, reader.Name, reader.ReadString());
                    }
                    continue;
                }
                catch
                {
                    continue;
                }
            }
        }

        private void ReadFooterRow(XmlReader reader)
        {
            this.FooterRow.StringData = Conversion.StringDataFromString(reader.ReadString());
            string[] stringData = this.FooterRow.StringData;
            if ((stringData.Length == 1) && (stringData[0] == ""))
            {
                while (reader.Name != "FooterRow")
                {
                    string str;
                    if (((str = reader.Name) != null) && (str == "Property"))
                    {
                        if (reader.HasAttributes && (reader["Name"] == "StringData"))
                        {
                            this.FooterRow.StringData = Conversion.StringDataFromString(reader["Value"]);
                        }
                        else if ((reader.HasAttributes && (reader.GetAttribute("Name") != null)) && (reader.GetAttribute("Value") != null))
                        {
                            this.m_conversion.SetProperty(this.FooterRow, reader.GetAttribute("Name"), reader.GetAttribute("Value"));
                        }
                    }
                    reader.Read();
                }
            }
        }

        private void ReadHeaderRow(XmlReader reader)
        {
            this.HeaderRow.StringData = Conversion.StringDataFromString(reader.ReadString());
            string[] stringData = this.HeaderRow.StringData;
            if ((stringData.Length == 1) && (stringData[0] == ""))
            {
                while (reader.Name != "HeaderRow")
                {
                    string str;
                    if (((str = reader.Name) != null) && (str == "Property"))
                    {
                        if (reader.HasAttributes && (reader["Name"] == "StringData"))
                        {
                            this.HeaderRow.StringData = Conversion.StringDataFromString(reader["Value"]);
                        }
                        else if ((reader.HasAttributes && (reader.GetAttribute("Name") != null)) && (reader.GetAttribute("Value") != null))
                        {
                            this.m_conversion.SetProperty(this.HeaderRow, reader.GetAttribute("Name"), reader.GetAttribute("Value"));
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
                string s = reader["ImageHeight"];
                string str4 = reader["ImageWidth"];
                ImageList imageList = this.m_conversion.GetImageList(sName);
                if (imageList != null)
                {
                    Size size;
                    if (((s != null) && (s != "")) && ((str4 != null) && (str4 != "")))
                    {
                        try
                        {
                            int width = int.Parse(str4);
                            int height = int.Parse(s);
                            size = new Size(width, height);
                        }
                        catch
                        {
                            size = new Size(0x10, 0x10);
                        }
                    }
                    else if ((sSize != null) && (sSize != ""))
                    {
                        try
                        {
                            size = Conversion.SizeFromString(sSize);
                        }
                        catch
                        {
                            size = new Size(0x10, 0x10);
                        }
                    }
                    else
                    {
                        size = new Size(0x10, 0x10);
                    }
                    imageList.ImageSize = size;
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
                                        int num3;
                                        stream = new MemoryStream();
                                        while ((num3 = ((XmlTextReader) reader).ReadBase64(array, 0, 0x3e8)) > 0)
                                        {
                                            stream.Write(array, 0, num3);
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

        private RowTemplate ReadRowTemplate(XmlReader reader)
        {
            RowTemplate o = new RowTemplate();
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
                    if (!(str2 == "RowTemplate"))
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
                    if (this._designTimeCallback != null)
                    {
                        this._designTimeCallback(o, null);
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

        private void RebindInnerList()
        {
            if (this.m_innerListManager != null)
            {
                this.m_innerListManager.PositionChanged -= new EventHandler(this.m_innerListManager_PositionChanged);
            }
            if (this.m_listManager != null)
            {
                this.m_innerListManager = this.m_listManager;
            }
            else if (this.m_autoBind && (this.BindingContext != null))
            {
                this.m_innerListManager = (CurrencyManager) this.BindingContext[this.DataRows];
            }
            else
            {
                this.m_innerListManager = null;
            }
            if (this.m_innerListManager != null)
            {
                this.m_innerListManager.PositionChanged += new EventHandler(this.m_innerListManager_PositionChanged);
                this.ActiveRowIndex = this.m_innerListManager.Position;
            }
        }

        protected virtual bool Redraw(Graphics gr)
        {
            bool flag = true;
            if (this.m_doubleBuffered)
            {
                bool isValid = BackBufferManager.IsValid(this);
                if (this.m_bIsChange || !isValid)
                {
                    flag = this.RedrawBackBuffer(isValid);
                }
                Rectangle rectangle = this.CalculateClientRect();
                gr.DrawImage(BackBufferManager.GetBackBufferImage(rectangle.Width, rectangle.Height), rectangle.X, rectangle.Y, new Rectangle(0, 0, rectangle.Width, rectangle.Height), GraphicsUnit.Pixel);
                return flag;
            }
            return this.RedrawToDisplay(gr);
        }

        protected virtual bool RedrawBackBuffer(bool isValid)
        {
            int iActualRowIndex;
            int num10;
            Rectangle rectangle = this.CalculateClientRect();
            Graphics backBufferGraphics = BackBufferManager.GetBackBufferGraphics(rectangle.Width, rectangle.Height);
            Bitmap backBufferImage = BackBufferManager.GetBackBufferImage(rectangle.Width, rectangle.Height);
            Graphics tempGraphics = BackBufferManager.GetTempGraphics(rectangle.Width, rectangle.Height);
            Bitmap tempImage = BackBufferManager.GetTempImage(rectangle.Width, rectangle.Height);
            bool bResetScrollbar = false;
            int width = rectangle.Width;
            int height = rectangle.Height;
            bool flag2 = true;
            int iScrollChange = this.m_iScrollChange;
            int num4 = 0;
            int num5 = 0;
            RowTemplate t = null;
            RowTemplate template2 = null;
            this.m_iScrollChange = 0;
            this.backRect.Width = rectangle.Width;
            this.backRect.Height = rectangle.Height;
            if (this.m_bShowHeader)
            {
                t = this.m_tsCurrent[this.m_rHeader.TemplateIndex];
                if (t != null)
                {
                    num4 = this.CustomizeHeaderFooter(t, this.m_rHeader, ref bResetScrollbar);
                }
            }
            if (this.m_bShowFooter)
            {
                template2 = this.m_tsCurrent[this.m_rFooter.TemplateIndex];
                if (template2 != null)
                {
                    num5 = this.CustomizeHeaderFooter(template2, this.m_rFooter, ref bResetScrollbar);
                }
            }
            int ymax = height - num5;
            if ((Math.Abs(iScrollChange) > (ymax - num4)) || !isValid)
            {
                iScrollChange = 0;
            }
            int num7 = width - this.m_vScrollWidth;
            int y = 0;
            this.ResetCache(iScrollChange);
            if ((this.BackgroundType == Resco.Controls.AdvancedList.BackgroundType.btImage) || ((this.BackgroundType == Resco.Controls.AdvancedList.BackgroundType.btGradient) && (this.m_gradientBackColor.FillDirection == FillDirection.Vertical)))
            {
                iActualRowIndex = this.m_iActualRowIndex;
                num10 = this.m_iTopmostRowOffset + num4;
                if (this.BackgroundType == Resco.Controls.AdvancedList.BackgroundType.btImage)
                {
                    this.DrawBackgroundImage(backBufferGraphics);
                }
                else
                {
                    this.m_gradientBackColor.DrawGradient(backBufferGraphics, new Rectangle(0, y, rectangle.Width, ymax - y));
                }
            }
            else if ((iScrollChange < 0) && (this.m_rcRows.LastDrawnRow > 0))
            {
                int num11 = height + iScrollChange;
                iActualRowIndex = this.m_rcRows.LastDrawnRow;
                num10 = this.m_rcRows.LastDrawnRowOffset + iScrollChange;
                Rectangle srcRect = new Rectangle(0, -iScrollChange, width, num11);
                Rectangle destRect = new Rectangle(0, 0, width, num11);
                tempGraphics.DrawImage(backBufferImage, 0, 0);
                backBufferGraphics.DrawImage(tempImage, destRect, srcRect, GraphicsUnit.Pixel);
            }
            else
            {
                iActualRowIndex = this.m_iActualRowIndex;
                num10 = this.m_iTopmostRowOffset + num4;
                if (iScrollChange > 0)
                {
                    int num12 = height - iScrollChange;
                    Rectangle rectangle4 = new Rectangle(0, 0, width, num12);
                    Rectangle rectangle5 = new Rectangle(0, iScrollChange, width, num12);
                    tempGraphics.DrawImage(backBufferImage, 0, 0);
                    backBufferGraphics.DrawImage(tempImage, rectangle5, rectangle4, GraphicsUnit.Pixel);
                    ymax = iScrollChange + num4;
                }
            }
            if ((this.BackgroundType == Resco.Controls.AdvancedList.BackgroundType.btGradient) && (this.m_gradientBackColor.FillDirection != FillDirection.Vertical))
            {
                this.m_gradientBackColor.DrawGradient(backBufferGraphics, new Rectangle(0, num10, width, ymax - num10));
            }
            Graphics gr = backBufferGraphics;
            using (Region region = new Region(new Rectangle(0, num10, num7, ymax - num10)))
            {
                gr.Clip = region;
                y = this.m_rcRows.Draw(gr, this.m_tsCurrent, 0, num7, 0, ymax, iActualRowIndex, num10, ref bResetScrollbar);
                gr.ResetClip();
            }
            if (y < ymax)
            {
                if (this.BackgroundType == Resco.Controls.AdvancedList.BackgroundType.btSolid)
                {
                    this.RedrawBackground(gr, new Rectangle(0, y, width, ymax - y));
                }
                else if ((this.BackgroundType == Resco.Controls.AdvancedList.BackgroundType.btGradient) && (this.m_gradientBackColor.FillDirection != FillDirection.Vertical))
                {
                    this.m_gradientBackColor.DrawGradient(backBufferGraphics, new Rectangle(0, y, width, ymax - y));
                }
                flag2 = false;
            }
            if (this.m_bShowHeader && (t != null))
            {
                gr.Clip = new Region(new Rectangle(0, 0, this.m_bScrollbarOverlap ? num7 : width, num4));
                t.Draw(gr, 0, 0, this.m_rHeader, this.m_bScrollbarOverlap ? num7 : width, num4);
                gr.ResetClip();
            }
            if (this.m_bShowFooter && (template2 != null))
            {
                template2.Draw(gr, 0, height - num5, this.m_rFooter, this.m_bScrollbarOverlap ? num7 : width, num5);
            }
            if (bResetScrollbar)
            {
                this.OnChange(this, GridEventArgsType.Refresh, new RefreshData(true));
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
            Rectangle rectangle = this.CalculateClientRect();
            bool flag = true;
            using (Region region = new Region(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)))
            {
                gr.Clip = region;
                bool bResetScrollbar = false;
                int iScrollChange = this.m_iScrollChange;
                this.m_iScrollChange = 0;
                int height = 0;
                int num3 = 0;
                RowTemplate t = null;
                RowTemplate template2 = null;
                if (this.m_bShowHeader)
                {
                    t = this.m_tsCurrent[this.m_rHeader.TemplateIndex];
                    if (t != null)
                    {
                        height = this.CustomizeHeaderFooter(t, this.m_rHeader, ref bResetScrollbar);
                    }
                }
                if (this.m_bShowFooter)
                {
                    template2 = this.m_tsCurrent[this.m_rFooter.TemplateIndex];
                    if (template2 != null)
                    {
                        num3 = this.CustomizeHeaderFooter(template2, this.m_rFooter, ref bResetScrollbar);
                    }
                }
                int ymax = rectangle.Height - num3;
                if (Math.Abs(iScrollChange) > (ymax - height))
                {
                    iScrollChange = 0;
                }
                int width = rectangle.Width - this.m_vScrollWidth;
                int y = 0;
                int iActualRowIndex = this.m_iActualRowIndex;
                int iRowOffset = this.m_iTopmostRowOffset + height;
                this.backRect.Width = rectangle.Width;
                this.backRect.Height = rectangle.Height;
                this.ResetCache(iScrollChange);
                if (this.BackgroundType == Resco.Controls.AdvancedList.BackgroundType.btGradient)
                {
                    this.m_gradientBackColor.DrawGradient(gr, new Rectangle(rectangle.X, y + rectangle.Y, rectangle.Width, ymax - y));
                }
                else if (this.BackgroundType == Resco.Controls.AdvancedList.BackgroundType.btImage)
                {
                    this.DrawBackgroundImage(gr);
                }
                int xOffset = rectangle.X + (this.RightToLeft ? this.m_vScrollWidth : 0);
                y = this.m_rcRows.Draw(gr, this.m_tsCurrent, xOffset, width, rectangle.Y, ymax, iActualRowIndex, iRowOffset, ref bResetScrollbar);
                if (y < ymax)
                {
                    if (this.BackgroundType == Resco.Controls.AdvancedList.BackgroundType.btSolid)
                    {
                        this.RedrawBackground(gr, new Rectangle(rectangle.X, y, rectangle.Width, rectangle.Height));
                    }
                    flag = false;
                }
                if (this.m_bShowHeader && (t != null))
                {
                    t.Draw(gr, rectangle.X, rectangle.Y, this.m_rHeader, this.m_bScrollbarOverlap ? width : rectangle.Width, height);
                }
                if (this.m_bShowFooter && (template2 != null))
                {
                    template2.Draw(gr, rectangle.X, (rectangle.Y + rectangle.Height) - num3, this.m_rFooter, this.m_bScrollbarOverlap ? width : rectangle.Width, num3);
                }
                if (bResetScrollbar)
                {
                    this.OnChange(this, GridEventArgsType.Refresh, new RefreshData(true));
                }
                gr.ResetClip();
            }
            return flag;
        }

        public Resco.Controls.AdvancedList.Mapping Reload()
        {
            int iInsertIndex = -1;
            if (this.m_mapLast != null)
            {
                this.BeginUpdate();
                iInsertIndex = this.m_rcRows.RemoveByMapping(this.m_mapLast);
            }
            Resco.Controls.AdvancedList.Mapping mapping = this.LoadData(iInsertIndex);
            this.EndUpdate();
            return mapping;
        }

        private void ReloadDataSource()
        {
            if (this.m_boundMap != Resco.Controls.AdvancedList.Mapping.Empty)
            {
                this.DataRows.RemoveByMapping(this.m_boundMap);
            }
            if (this.m_listManager != null)
            {
                this.m_boundMap = new PropertyMapping(this.m_listManager.GetItemProperties());
                if (!this.m_bDelayLoad)
                {
                    int insertIndex = 0;
                    for (int i = 0; i < this.m_listManager.List.Count; i++)
                    {
                        Row row = new BoundRow(this.TemplateIndex, this.SelectedTemplateIndex, this.m_listManager.List[i], this.m_boundMap as PropertyMapping);
                        row.AlternateTemplateIndex = this.AlternateTemplateIndex;
                        row.ActiveTemplateIndex = this.ActiveTemplateIndex;
                        int num3 = this.InsertRow(row, insertIndex);
                        if (num3 != insertIndex)
                        {
                            insertIndex = num3;
                        }
                    }
                }
                else
                {
                    this.m_nRowsLoaded = 0;
                    this.DoDelayLoad();
                }
            }
            else if (this.m_enumerator != null)
            {
                this.m_boundMap = Resco.Controls.AdvancedList.Mapping.Empty;
                if (!this.m_bDelayLoad)
                {
                    int num4 = 0;
                    while (this.m_enumerator.MoveNext())
                    {
                        if (this.m_boundMap == Resco.Controls.AdvancedList.Mapping.Empty)
                        {
                            this.m_boundMap = new PropertyMapping(this.m_enumerator.Current.GetType());
                        }
                        Row row2 = new BoundRow(this.TemplateIndex, this.SelectedTemplateIndex, this.m_enumerator.Current, this.m_boundMap as PropertyMapping);
                        row2.AlternateTemplateIndex = this.AlternateTemplateIndex;
                        row2.ActiveTemplateIndex = this.ActiveTemplateIndex;
                        int num5 = this.InsertRow(row2, num4);
                        if (num5 != num4)
                        {
                            num4 = num5;
                        }
                    }
                    this.m_enumeratorNeedLoad = false;
                }
                else
                {
                    this.m_nRowsLoaded = 0;
                    this.DoDelayLoad();
                }
            }
            else
            {
                this.m_boundMap = Resco.Controls.AdvancedList.Mapping.Empty;
            }
        }

        private void RemoveActiveHandlers()
        {
            if ((this.m_listManager != null) && (this.m_listManager.List is IBindingList))
            {
                ((IBindingList) this.m_listManager.List).ListChanged -= new ListChangedEventHandler(this.OnListChanged);
            }
        }

        private void RemoveEnumerator()
        {
            if ((this.m_enumerator != null) && (this.m_enumerator is IDisposable))
            {
                ((IDisposable) this.m_enumerator).Dispose();
            }
            this.m_enumerator = null;
            this.m_enumeratorNeedLoad = false;
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
                Rectangle rectangle = this.CalculateClientRect();
                int num = rectangle.Y + this.HeaderHeight;
                int num2 = (rectangle.Y + rectangle.Height) - this.FooterHeight;
                for (int i = this.m_alLinks.Count - 1; i >= 0; i--)
                {
                    Rectangle rectangle2 = this.m_alLinks[i]; //this.m_alLinks.get_Item(i);
                    rectangle2.Y += iScroll;
                    if ((rectangle2.Bottom < num) || (rectangle2.Top > num2))
                    {
                        this.m_alLinks.RemoveAt(i);
                    }
                    else
                    {
                        this.m_alLinks[i]=rectangle2;
                    }
                }
                for (int j = this.m_alButtons.Count - 1; j >= 0; j--)
                {
                    Rectangle rectangle3 = this.m_alButtons[j];
                    rectangle3.Y += iScroll;
                    if ((rectangle3.Bottom < num) || (rectangle3.Top > num2))
                    {
                        this.m_alButtons.RemoveAt(j);
                    }
                    else
                    {
                        this.m_alButtons[j]= rectangle3;
                    }
                }
                for (int k = this.m_alTooltips.Count - 1; k >= 0; k--)
                {
                    Rectangle bounds = this.m_alTooltips[k].Bounds;
                    bounds.Y += iScroll;
                    if ((bounds.Bottom < num) || (bounds.Top > num2))
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

        public void ResetImageCache()
        {
            ImageCache.GlobalCache.Clear();
            this.OnChange(this, GridEventArgsType.Repaint, null);
        }

        public void ResetImageCache(ImageList il)
        {
            ImageCache.GlobalCache.Clear(il);
            this.OnChange(this, GridEventArgsType.Repaint, null);
        }

        public void ResumeRedraw()
        {
            if (this.m_iNoRedraw > 0)
            {
                this.m_iNoRedraw--;
            }
            if (this.m_iNoRedraw == 0)
            {
                this.OnChange(this, GridEventArgsType.Repaint, null);
                base.Invalidate();
            }
        }

        public void SaveXml(string fileName)
        {
            ALXmlSerializer.SaveXml(fileName, this);
        }

        public void SaveXml(XmlWriter writer)
        {
            ALXmlSerializer.SaveXml(writer, this);
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.BeginUpdate();
            float width = factor.Width;
            float height = factor.Height;
            if ((width != 1.0) || (height != 1.0))
            {
                if (this.m_vScrollBarResco == null)
                {
                    this.ScrollbarWidth = (int) (this.ScrollbarWidth * width);
                }
                foreach (RowTemplate template in this.Templates)
                {
                    template.Scale(width, height);
                }
                if (this.m_rFooter != null)
                {
                    this.m_rFooter.ResetCachedBounds();
                }
                if (this.m_rHeader != null)
                {
                    this.m_rHeader.ResetCachedBounds();
                }
                if (this.m_rcRows != null)
                {
                    this.m_rcRows.ResetCachedBounds(null);
                }
            }
            base.ScaleControl(factor, specified);
            this.EndUpdate();
        }

        private void SetActiveHandlers()
        {
            if ((this.m_listManager != null) && (this.m_listManager.List is IBindingList))
            {
                ((IBindingList) this.m_listManager.List).ListChanged += new ListChangedEventHandler(this.OnListChanged);
            }
        }

        private void SetEnumerator()
        {
            if (this.m_dataSource is IEnumerable)
            {
                this.m_enumerator = ((IEnumerable) this.m_dataSource).GetEnumerator();
                this.m_enumeratorNeedLoad = true;
            }
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

        private int SetScrollPos(int offset)
        {
            if (this.m_vScroll == null)
            {
                return 0;
            }
            Rectangle rectangle = this.CalculateClientRect();
            int num = -1;
            int num2 = 0;
            int num3 = ((this.m_iDocumentHeight - rectangle.Height) + this.HeaderHeight) + this.FooterHeight;
            if (offset > num3)
            {
                offset = num3;
            }
            if (offset < 0)
            {
                offset = 0;
            }
            num = 0;
            while (num < this.m_rcRows.Count)
            {
                int height = this.m_rcRows[num].GetHeight(this.m_tsCurrent);
                num2 += height;
                if (num2 > offset)
                {
                    this.m_iActualRowIndex = num;
                    this.m_iTopmostRowOffset = num2 - (height + offset);
                    break;
                }
                num++;
            }
            this.m_iVScrollPrevValue = offset;
            this.m_vScroll.Value = offset;
            this.OnChange(this, GridEventArgsType.Empty, null);
            return num;
        }

        public int SetScrollPos(int ix, int cx, int offset)
        {
            if (this.m_vScroll == null)
            {
                int num = 0;
                for (int k = 0; k < ix; k++)
                {
                    num += this.m_rcRows.GetHeight(k, this.m_tsCurrent);
                }
                return num;
            }
            Rectangle rectangle = this.CalculateClientRect();
            int num3 = 0;
            int num4 = (this.m_vScroll.Maximum - ((rectangle.Height - this.HeaderHeight) - this.FooterHeight)) + 1;
            if (num4 < 0)
            {
                num4 = 0;
            }
            for (int i = 0; i < ix; i++)
            {
                num3 += this.m_rcRows.GetHeight(i, this.m_tsCurrent);
            }
            int num6 = (cx >= 0) ? this.m_rcRows[ix].Template[cx].Bounds.Top : 0;
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
            for (int j = 0; j < this.m_rcRows.Count; j++)
            {
                int height = this.m_rcRows.GetHeight(j, this.m_tsCurrent);
                if ((num8 + height) > num7)
                {
                    this.m_iActualRowIndex = j;
                    this.m_iTopmostRowOffset = num8 - num7;
                    break;
                }
                num8 += height;
            }
            this.m_iVScrollPrevValue = num7;
            this.m_vScroll.Value = num7;
            this.OnChange(this, GridEventArgsType.Empty, null);
            return (num3 - num7);
        }

        private bool SetVScrollBar(int height)
        {
            Rectangle rectangle = this.CalculateClientRect();
            bool vScrollBarVisible = false;
            int num = this.HeaderHeight + this.FooterHeight;
            int num2 = rectangle.Height - num;
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
                this.SetVScrollBarBounds();
                this.m_vScroll.Minimum = 0;
                this.m_vScroll.Maximum = Math.Max(0, height + ScrollBottomOffset);
                this.m_vScroll.LargeChange = num2;
                this.m_vScroll.SmallChange = (num2 < ScrollSmallChange) ? num2 : ScrollSmallChange;
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
            this.SetVScrollBarBounds();
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

        private void SetVScrollBarBounds()
        {
            if (this.m_vScroll != null)
            {
                Rectangle rectangle = this.CalculateClientRect();
                int height = rectangle.Height;
                int y = rectangle.Y;
                if (!this.m_bScrollbarOverlap)
                {
                    y += this.HeaderHeight;
                    height -= y;
                    height -= this.FooterHeight;
                }
                if (!this.RightToLeft)
                {
                    this.m_vScroll.Bounds = new Rectangle((rectangle.X + rectangle.Width) - this.m_iScrollWidth, y, this.m_iScrollWidth, height);
                }
                else
                {
                    this.m_vScroll.Bounds = new Rectangle(rectangle.X, y, this.m_iScrollWidth, height);
                }
            }
        }

        private void ShiftBoundRowsForCF20(int fromIndex, int toIndex)
        {
            for (int i = fromIndex; i < toIndex; i++)
            {
                ((BoundRow) this.m_rcRows[i]).TempSet(this.m_listManager.List[i], this.m_boundMap as PropertyMapping);
            }
        }

        protected virtual bool ShouldSerializeDataSource()
        {
            return (this.m_dataSource != null);
        }

        protected virtual bool ShouldSerializeFooterRow()
        {
            return this.m_bShowFooter;
        }

        protected virtual bool ShouldSerializeGradientBackColor()
        {
            bool flag = this.m_gradientBackColor.StartColor != Color.Transparent;
            bool flag2 = this.m_gradientBackColor.EndColor != Color.Transparent;
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

        protected virtual bool ShouldSerializeHeaderRow()
        {
            return this.m_bShowHeader;
        }

        protected virtual bool ShouldSerializeSelectionMode()
        {
            return (this.m_mode != Resco.Controls.AdvancedList.SelectionMode.SelectOnly);
        }

        protected virtual bool ShouldSerializeToolTipType()
        {
            return (this.m_toolTipType != Resco.Controls.AdvancedList.ToolTipType.Triangle);
        }

        protected virtual bool ShouldSerializeTouchScrollDirection()
        {
            return (this.m_touchScrollDirection != Resco.Controls.AdvancedList.TouchScrollDirection.Inverse);
        }

        public void SuspendRedraw()
        {
            this.m_iNoRedraw++;
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        internal Row ActiveRow
        {
            get
            {
                if (this.m_rcRows != null)
                {
                    return this.m_rcRows.ActiveRow;
                }
                return null;
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public int ActiveRowIndex
        {
            get
            {
                if ((this.m_rcRows != null) && (this.m_rcRows.ActiveRow != null))
                {
                    return this.m_rcRows.ActiveRow.Index;
                }
                return -1;
            }
            set
            {
                if (value != this.ActiveRowIndex)
                {
                    this.SelectedCell = null;
                    if (value == -1)
                    {
                        this.m_rcRows.ActiveRow.Selected = false;
                    }
                    else if (value >= 0)
                    {
                        if (this.DelayLoad)
                        {
                            while (value >= this.m_rcRows.Count)
                            {
                                if (!this.DoDelayLoad())
                                {
                                    break;
                                }
                            }
                        }
                        if (value < this.m_rcRows.Count)
                        {
                            this.SelectedRow = this.m_rcRows[value];
                        }
                    }
                }
            }
        }

        [DefaultValue(-1)]
        public int ActiveTemplateIndex
        {
            get
            {
                return this.m_activeTemplateIndex;
            }
            set
            {
                this.m_activeTemplateIndex = value;
            }
        }

        [DefaultValue(-1)]
        public int AlternateTemplateIndex
        {
            get
            {
                return this.m_alternateTemplateIndex;
            }
            set
            {
                this.m_alternateTemplateIndex = value;
            }
        }

        [DefaultValue(false)]
        public bool AutoBinding
        {
            get
            {
                return this.m_autoBind;
            }
            set
            {
                if (this.m_autoBind != value)
                {
                    this.m_autoBind = value;
                    this.RebindInnerList();
                }
            }
        }

        protected internal Graphics BackBuffer
        {
            get
            {
                Rectangle rectangle = this.CalculateClientRect();
                return BackBufferManager.GetBackBufferGraphics(rectangle.Width, rectangle.Height);
            }
        }

        [DefaultValue("ControlDark")]
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
                this.OnChange(this, GridEventArgsType.Repaint, null);
            }
        }

        [DefaultValue((string) null)]
        public Image BackgroundImage
        {
            get
            {
                return this.m_bgImage;
            }
            set
            {
                if (value != this.m_bgImage)
                {
                    this.m_bgImage = value;
                    this.OnChange(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        internal Resco.Controls.AdvancedList.BackgroundType BackgroundType
        {
            get
            {
                if (this.m_bgImage != null)
                {
                    return Resco.Controls.AdvancedList.BackgroundType.btImage;
                }
                if ((this.GradientBackColor != null) && this.GradientBackColor.CanDraw())
                {
                    return Resco.Controls.AdvancedList.BackgroundType.btGradient;
                }
                return Resco.Controls.AdvancedList.BackgroundType.btSolid;
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false)]
        public bool Border
        {
            get
            {
                return (base.BorderStyle != BorderStyle.None);
            }
            set
            {
                base.BorderStyle = value ? BorderStyle.FixedSingle : BorderStyle.None;
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

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public IDataConnector DataConnector
        {
            get
            {
                return this.m_connector;
            }
            set
            {
                if (this.m_connector != value)
                {
                    this.CloseConnector();
                    this.m_connector = (value == null) ? this.m_dbConnector : value;
                }
            }
        }

        public RowCollection DataRows
        {
            get
            {
                return this.m_rcRows;
            }
        }

        public object DataSource
        {
            get
            {
                return this.m_dataSource;
            }
            set
            {
                try
                {
                    this.BeginUpdate();
                    this.m_nRowsLoaded = 0;
                    this.m_nRowsInserted = 0;
                    this.BindTo(value);
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public Resco.Controls.AdvancedList.DataConnector DbConnector
        {
            get
            {
                return this.m_dbConnector;
            }
        }

        [DefaultValue(false)]
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
                    if (!this.m_bDelayLoad && this.m_connector.IsOpen)
                    {
                        try
                        {
                            this.BeginUpdate();
                            this.LoadDataChunk(false);
                        }
                        catch
                        {
                        }
                        finally
                        {
                            this.CloseConnector();
                            this.EndUpdate();
                        }
                    }
                }
            }
        }

        private int DocumentHeight
        {
            get
            {
                if (!this.DelayLoad || (this.m_iExpectedRows < 0))
                {
                    return this.m_iDocumentHeight;
                }
                RowTemplate template = this.Templates[this.TemplateIndex];
                if (template == null)
                {
                    template = RowTemplate.Default;
                }
                if (template == null)
                {
                    return this.m_iDocumentHeight;
                }
                return ((template.Height + (this.GridLines ? 1 : 0)) * this.m_iExpectedRows);
            }
        }

        public bool DoubleBuffered
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
                    if (value)
                    {
                        BackBufferManager.AddRef();
                    }
                    else
                    {
                        BackBufferManager.Release();
                    }
                    if (this.m_iUpdate == 0)
                    {
                        this.SetRedraw(0);
                        this.m_rcRows.LastDrawnRow = -1;
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

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public int ExpectedRows
        {
            get
            {
                return this.m_iExpectedRows;
            }
            set
            {
                if (this.m_iExpectedRows != value)
                {
                    this.m_iExpectedRows = value;
                    this.OnChange(this, GridEventArgsType.Refresh, null);
                }
            }
        }

        [DefaultValue(false)]
        public bool FocusOnClick
        {
            get
            {
                return this.m_bFocusOnClick;
            }
            set
            {
                this.m_bFocusOnClick = value;
            }
        }

        public int FooterHeight
        {
            get
            {
                if (this.m_bShowFooter)
                {
                    RowTemplate template = this.m_tsCurrent[this.m_rFooter.TemplateIndex];
                    if (template != null)
                    {
                        return template.GetHeight(this.m_rFooter);
                    }
                }
                return 0;
            }
        }

        public Resco.Controls.AdvancedList.HeaderRow FooterRow
        {
            get
            {
                return this.m_rFooter;
            }
            set
            {
                if (value == null)
                {
                    value = new Resco.Controls.AdvancedList.HeaderRow();
                }
                if (this.m_rFooter != value)
                {
                    this.m_rFooter.Parent = null;
                    this.m_rFooter = value;
                    this.m_rFooter.Parent = this.m_rcRows;
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

        [DefaultValue("DarkGray")]
        public Color GridColor
        {
            get
            {
                if (this.m_rcRows != null)
                {
                    return this.m_rcRows.m_penBorder.Color;
                }
                return Color.DarkGray;
            }
            set
            {
                if (this.m_rcRows.m_penBorder.Color != value)
                {
                    if (value.IsEmpty)
                    {
                        value = Color.DarkGray;
                    }
                    this.m_rcRows.m_penBorder.Color = value;
                    this.OnChange(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue(true)]
        public bool GridLines
        {
            get
            {
                if (this.m_rcRows != null)
                {
                    return this.m_rcRows.m_bDrawGrid;
                }
                return true;
            }
            set
            {
                if (this.m_rcRows.m_bDrawGrid != value)
                {
                    this.m_rcRows.m_bDrawGrid = value;
                    this.OnChange(this, GridEventArgsType.Refresh, null);
                }
            }
        }

        protected virtual int GridLinesWidth
        {
            get
            {
                if (!this.m_rcRows.m_bDrawGrid)
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

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
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

        public int HeaderHeight
        {
            get
            {
                if (this.m_bShowHeader)
                {
                    RowTemplate template = this.m_tsCurrent[this.m_rHeader.TemplateIndex];
                    if (template != null)
                    {
                        return template.GetHeight(this.m_rHeader);
                    }
                }
                return 0;
            }
        }

        public Resco.Controls.AdvancedList.HeaderRow HeaderRow
        {
            get
            {
                return this.m_rHeader;
            }
            set
            {
                if (value == null)
                {
                    value = new Resco.Controls.AdvancedList.HeaderRow();
                }
                if (this.m_rHeader != value)
                {
                    this.m_rHeader.Parent = null;
                    this.m_rHeader = value;
                    this.m_rHeader.Parent = this.m_rcRows;
                    this.OnChange(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public Resco.Controls.AdvancedList.HTCScreenOrientation HTCScreenOrientation
        {
            get
            {
                return this.m_HTCScreenOrientation;
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public List<ImageList> ImageLists
        {
            get
            {
                if (this.m_imageLists == null)
                {
                    this.m_imageLists = new List<ImageList>();
                }
                return this.m_imageLists;
            }
            set
            {
                this.m_imageLists = value;
            }
        }

        [DefaultValue(false)]
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

        [DefaultValue(false)]
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
                    this.m_rcRows.ResetSelected();
                }
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

        [DefaultValue(false)]
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
                    this.OnChange(this, GridEventArgsType.Refresh, new RefreshData(true));
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
                    this.ScrollPos = 0;
                    this.m_vScrollBarResco = value;
                    if (this.m_vScroll != null)
                    {
                        this.m_vScroll.Detach();
                        this.m_vScroll.ValueChanged -= new EventHandler(this.m_vScroll_ValueChanged);
                        this.m_vScroll.Resize -= new EventHandler(this.OnScrollResize);
                    }
                    this.m_vScroll = null;
                    this.OnChange(this, GridEventArgsType.Refresh, null);
                }
            }
        }

        [DefaultValue(true)]
        public bool ScrollbarOverlap
        {
            get
            {
                return this.m_bScrollbarOverlap;
            }
            set
            {
                if (this.m_bScrollbarOverlap != value)
                {
                    this.m_bScrollbarOverlap = value;
                    this.OnChange(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false), DefaultValue(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public bool ScrollbarVisible
        {
            get
            {
                return (this.ShowScrollbar && (this.m_vScrollWidth > 0));
            }
        }

        [DefaultValue(13)]
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
                        Rectangle rectangle = this.CalculateClientRect();
                        this.m_vScroll.Left = (rectangle.X + rectangle.Width) - value;
                        this.OnChange(this, GridEventArgsType.Refresh, new RefreshData(true));
                    }
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public int ScrollPos
        {
            get
            {
                if (this.m_vScroll == null)
                {
                    return 0;
                }
                return this.m_vScroll.Value;
            }
            set
            {
                this.SetScrollPos(value);
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public Cell SelectedCell
        {
            get
            {
                if (this.ActiveRow != null)
                {
                    RowTemplate template = this.Templates[this.ActiveRow.CurrentTemplateIndex];
                    RowTemplate.CellCollection cellTemplates = null;
                    if (template != null)
                    {
                        cellTemplates = template.CellTemplates;
                    }
                    if (((cellTemplates != null) && (this.m_iSelectedCellIndex >= 0)) && (this.m_iSelectedCellIndex < cellTemplates.Count))
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
                        this.OnChange(this, GridEventArgsType.Refresh, null);
                    }
                }
                else
                {
                    if (this.ActiveRow == null)
                    {
                        throw new ArgumentException("The cell is not contained in currently selected row's RowTemplate's CellCollection.", "SelectedCell");
                    }
                    RowTemplate.CellCollection cellTemplates = this.Templates[this.ActiveRow.CurrentTemplateIndex].CellTemplates;
                    if ((this.m_iSelectedCellIndex == -1) || (cellTemplates[this.m_iSelectedCellIndex] != value))
                    {
                        if (!cellTemplates.Contains(value))
                        {
                            throw new ArgumentException("The cell is not contained in currently selected row's RowTemplate's CellCollection.", "SelectedCell");
                        }
                        if (!value.IsSelectable(this.ActiveRow))
                        {
                            throw new ArgumentException("The cell is not selectable!", "SelectedCell");
                        }
                        this.m_iSelectedCellIndex = cellTemplates.IndexOf(value);
                        this.OnChange(this, GridEventArgsType.Refresh, null);
                    }
                }
            }
        }

        private int SelectedCellIndex
        {
            get
            {
                return this.m_iSelectedCellIndex;
            }
            set
            {
                this.m_iSelectedCellIndex = value;
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedList.Design.Browsable(false)]
        public Row SelectedRow
        {
            get
            {
                if ((this.m_rcRows != null) && (this.m_rcRows.SelectedCount > 0))
                {
                    foreach (Row row in this.m_rcRows)
                    {
                        if (row.Selected)
                        {
                            return row;
                        }
                    }
                }
                return null;
            }
            set
            {
                bool bSelectedChanged = false;
                int index = this.m_rcRows.IndexOf(value);
                if (index >= 0)
                {
                    bool active = value.Active;
                    this.SuspendRedraw();
                    if (!value.Selected)
                    {
                        value.Selected = true;
                        bSelectedChanged = true;
                    }
                    else if (!value.Active)
                    {
                        value.Active = true;
                    }
                    int yOffset = this.EnsureVisible(index, false);
                    this.ResumeRedraw();
                    if (bSelectedChanged || (active != value.Active))
                    {
                        this.OnRowSelect(value, index, yOffset, bSelectedChanged, value.Active);
                    }
                }
                else if (!this.m_bMultiSelect && (this.ActiveRow != null))
                {
                    this.ActiveRow.Selected = false;
                }
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedList.Design.Browsable(false)]
        public Row[] SelectedRows
        {
            get
            {
                if (this.m_rcRows == null)
                {
                    return null;
                }
                int selectedCount = this.m_rcRows.SelectedCount;
                Row[] rowArray = new Row[selectedCount];
                int num2 = 0;
                foreach (Row row in this.m_rcRows)
                {
                    if (row.Selected)
                    {
                        rowArray[num2++] = row;
                    }
                    if (num2 >= selectedCount)
                    {
                        return rowArray;
                    }
                }
                return rowArray;
            }
        }

        [DefaultValue(0)]
        public int SelectedTemplateIndex
        {
            get
            {
                return this.m_selectedTemplateIndex;
            }
            set
            {
                this.m_selectedTemplateIndex = value;
            }
        }

        [DefaultValue(0)]
        public Resco.Controls.AdvancedList.SelectionMode SelectionMode
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

        [DefaultValue(false)]
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
                    this.OnChange(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        [DefaultValue(false)]
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
                    this.OnChange(this, GridEventArgsType.Repaint, null);
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
                    this.OnChange(this, GridEventArgsType.Refresh, new RefreshData(true));
                }
            }
        }

        [DefaultValue(0)]
        public int TemplateIndex
        {
            get
            {
                return this.m_templateIndex;
            }
            set
            {
                this.m_templateIndex = value;
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Content)]
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
                    this.m_tsCurrent.Changed -= new GridEventHandler(this.OnChange);
                    this.m_tsCurrent = value;
                    this.m_tsCurrent.Parent = this;
                    this.m_tsCurrent.Changed += new GridEventHandler(this.OnChange);
                    this.OnChange(this, GridEventArgsType.Refresh, new RefreshData(true));
                }
            }
        }

        [DefaultValue(0)]
        public Resco.Controls.AdvancedList.ToolTipType ToolTipType
        {
            get
            {
                return this.m_toolTipType;
            }
            set
            {
                if (value != this.m_toolTipType)
                {
                    this.m_toolTipType = value;
                    this.OnChange(this, GridEventArgsType.Repaint, null);
                }
            }
        }

        public int TopRowIndex
        {
            get
            {
                return this.m_iActualRowIndex;
            }
        }

        [Resco.Controls.AdvancedList.Design.Browsable(false)]
        public int TopRowOffset
        {
            get
            {
                return this.m_iTopmostRowOffset;
            }
        }

        public Resco.Controls.AdvancedList.TouchScrollDirection TouchScrollDirection
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

        private delegate void OnChangeDelegate(object sender, GridEventArgsType e, object oParam);

        private delegate void OnRowRemovedDelegate(Row row, int index);

        internal class RefreshData
        {
            public bool ResetBounds;
            public RowTemplate Template;

            public RefreshData() : this(null, false)
            {
            }

            public RefreshData(RowTemplate rt) : this(rt, true)
            {
            }

            public RefreshData(bool bResetBounds) : this(null, bResetBounds)
            {
            }

            private RefreshData(RowTemplate rt, bool bResetBounds)
            {
                this.Template = rt;
                this.ResetBounds = bResetBounds;
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

   internal class BigHas
    {
        // Fields
        internal static Dictionary<string, int> methodxxx;
    }

 

}

