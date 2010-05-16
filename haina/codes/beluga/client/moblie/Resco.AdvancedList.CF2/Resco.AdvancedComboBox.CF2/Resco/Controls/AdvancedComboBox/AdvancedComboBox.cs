namespace Resco.Controls.AdvancedComboBox
{
    using Resco.Controls.AdvancedComboBox.Design;
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

    public class AdvancedComboBox : UserControl
    {
        private DesignTimeCallback _designTimeCallback;
        public static int DefaultTooltipWidth = 8;
        private List<Rectangle> m_alButtons = new List<Rectangle>();
        private List<Rectangle> m_alLinks = new List<Rectangle>();
        private int m_alternateTemplateIndex;
        private List<TooltipArea> m_alTooltips = new List<TooltipArea>();
        private int m_arrowBoxWidth = 13;
        private int m_arrowHeight = 4;
        private bool m_autoBind;
        private bool m_autoHideDropDownList = true;
        private Bitmap m_backBuffer;
        private SolidBrush m_BackColor;
        private bool m_bButtonPressed;
        private bool m_bDelayLoad;
        private bool m_bDrawingTextBox;
        private bool m_bForceRedraw;
        private bool m_bFullScreenList;
        private bool m_bIsChange = true;
        private bool m_bKeyNavigation;
        internal bool m_bManualDataLoading;
        private Pen m_BorderPen = new Pen(Color.Black);
        private int m_borderWidth = 1;
        private Resco.Controls.AdvancedComboBox.Mapping m_boundMap = Resco.Controls.AdvancedComboBox.Mapping.Empty;
        private SolidBrush m_brushKey;
        private bool m_bSetListBoundsInEndUpdate;
        private bool m_bShowingToolTip;
        private int m_cachedItemsHeight;
        internal Color m_colorKey;
        private IDataConnector m_connector;
        private Conversion m_conversion;
        private object m_dataSource;
        private Resco.Controls.AdvancedComboBox.DataConnector m_dbConnector;
        private string m_displayMember;
        private bool m_doubleBuffered = true;
        private int m_emptyListHeight = 100;
        private bool m_enableHTCGSensor;
        private bool m_enableHTCNavSensor;
        private bool m_enableHTCNavSensorNavigation;
        private Graphics m_grBackBuffer;
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
        private int m_iExpectedItems;
        private int m_iInsertIndex = -1;
        private List<ImageList> m_imageLists;
        private ImageAttributes m_imgAttr;
        private CurrencyManager m_innerListManager;
        private int m_iNoRedraw;
        private int m_iSelectedCellIndex;
        private ItemCollection m_items;
        private int m_iUpdateCounter = 1;
        private AdvancedList m_list;
        private int m_listHeight = -1;
        private CurrencyManager m_listManager;
        private Resco.Controls.AdvancedComboBox.Mapping m_mapLast;
        private int m_maxListHeight = -1;
        private Point m_MousePosition = new Point(0, 0);
        private int m_nItemsInserted;
        private int m_nItemsLoaded;
        private bool m_rightToLeft;
        private HTCNavSensorRotatedEventArgs m_rotatedEventArgs = new HTCNavSensorRotatedEventArgs(0.0, 0.0);
        private float m_scaleFactorX = 1f;
        private float m_scaleFactorY = 1f;
        private int m_selectedItemIndex;
        private int m_selectedTemplateIndex;
        private int m_templateIndex;
        internal Resco.Controls.AdvancedComboBox.ListItem m_textBoxItem;
        private Rectangle m_textBoxTemplateArea;
        private int m_textBoxTemplateIndex;
        private Timer m_Timer;
        private Resco.Controls.AdvancedComboBox.ToolTip m_ToolTip;
        private TemplateSet m_tsCurrent;
        private TemplateSet m_tsDefault;
        private string m_valueMember;
        private OnChangeDelegate OnChangeHandler;
        private OnItemRemovedDelegate OnItemRemovedHandler;
        internal static Point point1;
        internal static Point point2;
        internal static Point point3;
        private static Hashtable sBrushes = null;
        private static Pen sPen = new Pen(Color.Black);
        public static int TooltipWidth = DefaultTooltipWidth;
        internal static Color TransparentColor = Color.Transparent;

        public event ButtonEventHandler ButtonClick;

        public event CellEventHandler CellClick;

        public event CellEnteredMainEventHandler CellEntered;

        public event CustomizeCellEventHandler CustomizeCell;

        public event DataLoadedEventHandler DataLoaded;

        public event EventHandler DisplayMemberChanged;

        public event EventHandler DropDown;

        public event EventHandler DropDownClosed;

        public event DropDownHandler DropDownClosing;

        public event DropDownHandler DroppingDown;

        public event HTCGyroDirectionHandler HTCGyroDirection;

        public event HTCNavSensorRotatedHandler HTCNavSensorRotated;

        public event HTCOrientationChangedHandler HTCOrientationChanged;

        public event ItemAddingEventHandler ItemAdding;

        public event ItemEnteredEventHandler ItemEntered;

        public event LinkEventHandler LinkClick;

        public event EventHandler Scroll
        {
            add
            {
                this.m_list.Scroll += value;
            }
            remove
            {
                this.m_list.Scroll -= value;
            }
        }

        public event EventHandler SelectedIndexChanged;

        public event EventHandler SelectedValueChanged;

        public event ValidateDataEventHandler ValidateData;

        public event EventHandler ValueMemberChanged;

        static AdvancedComboBox()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(Resco.Controls.AdvancedComboBox.AdvancedComboBox), "");
            //}
        }

        public AdvancedComboBox()
        {
            this.OnChangeHandler = new OnChangeDelegate(this.OnChangeSafe);
            this.OnItemRemovedHandler = new OnItemRemovedDelegate(this.OnItemRemovedSafe);
            base.AutoScroll = false;
            base.Size = new Size(100, 0x12);
            this.m_dbConnector = new Resco.Controls.AdvancedComboBox.DataConnector();
            this.m_connector = this.m_dbConnector;
            this.m_bKeyNavigation = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_list = new AdvancedList(this);
            this.m_list.Capture = false;
            this.m_list.Visible = false;
            this.m_list.TouchScrolling = false;
            this.m_list.BorderStyle = this.BorderStyle;//.set_BorderStyle(this.BorderStyle);
            this.m_list.Size = new Size(0, 0);
            this.m_list.ItemSelect += new ItemEventHandler(this.OnItemSelect);
            this.m_list.Changed += new ComboBoxEventHandler(this.OnChange);
            this.SelectedIndexChanged = null;
            this.SelectedValueChanged = null;
            this.ValueMemberChanged = null;
            this.DisplayMemberChanged = null;
            this.m_items = new ItemCollection(this);
            this.m_items.Changed += new ComboBoxEventHandler(this.OnChange);
            this.m_iExpectedItems = -1;
            this.m_nItemsLoaded = 0;
            this.m_nItemsInserted = 0;
            this.m_selectedItemIndex = -1;
            this.m_iSelectedCellIndex = -1;
            this.m_valueMember = "";
            this.m_displayMember = "";
            this.m_tsCurrent = new TemplateSet();
            this.m_tsCurrent.Parent = this;
            this.m_tsCurrent.Changed += new ComboBoxEventHandler(this.OnChange);
            this.m_textBoxTemplateIndex = 0;
            this.m_templateIndex = 0;
            this.m_selectedTemplateIndex = 0;
            this.m_alternateTemplateIndex = -1;
            this.m_grBackBuffer = null;
            this.m_backBuffer = null;
            this.m_colorKey = Color.FromArgb(0xff, 0, 0xff);
            this.m_brushKey = new SolidBrush(this.m_colorKey);
            this.m_imgAttr = new ImageAttributes();
            this.m_imgAttr.SetColorKey(this.m_colorKey, this.m_colorKey);
            this.UpdateDoubleBuffering();
            base.BackColor = SystemColors.ControlLight;
            this.m_BackColor = new SolidBrush(this.BackColor);
            this.m_Timer = new Timer();
            this.m_Timer.Enabled = false;
            this.m_Timer.Interval = 500;
            this.m_Timer.Tick += new EventHandler(this.OnTimerTick);
            this.m_bShowingToolTip = false;
            point1 = new Point(0, 0);
            point2 = new Point(0, -TooltipWidth);
            point3 = new Point(-TooltipWidth, 0);
            this.CalculateTextBoxArea();
        }

        internal void AddButtonArea(Rectangle bounds)
        {
            if (this.m_bDrawingTextBox)
            {
                if (!this.m_alButtons.Contains(bounds))
                {
                    this.m_alButtons.Add(bounds);
                }
            }
            else
            {
                this.m_list.AddButtonArea(bounds);
            }
        }

        internal bool AddItemManually()
        {
            ItemAddingEventArgs e = new ItemAddingEventArgs(this.Items.Count);
            this.OnItemAdding(e);
            if ((e.ListItem != null) && !e.Cancel)
            {
                this.Items.Add(e.ListItem);
            }
            return e.Cancel;
        }

        internal void AddLinkArea(Rectangle bounds)
        {
            if (this.m_bDrawingTextBox)
            {
                if (!this.m_alLinks.Contains(bounds))
                {
                    this.m_alLinks.Add(bounds);
                }
            }
            else
            {
                this.m_list.AddLinkArea(bounds);
            }
        }

        public void AddTooltipArea(Rectangle bounds, string text)
        {
            if (this.m_bDrawingTextBox)
            {
                this.m_alTooltips.Add(new TooltipArea(bounds, text));
            }
            else
            {
                this.m_list.AddTooltipArea(bounds, text);
            }
        }

        public void BeginUpdate()
        {
            this.m_iUpdateCounter++;
        }

        private void BindInnerList()
        {
            if (this.m_listManager != null)
            {
                this.m_innerListManager = this.m_listManager;
            }
            else if (this.m_autoBind && (this.BindingContext != null))
            {
                this.m_innerListManager = (CurrencyManager) this.BindingContext[this.Items];
            }
            else
            {
                this.m_innerListManager = null;
            }
            if (this.m_innerListManager != null)
            {
                this.m_innerListManager.PositionChanged += new EventHandler(this.m_innerListManager_PositionChanged);
                this.SelectedIndex = this.m_innerListManager.Position;
            }
        }

        private void BindTo(object dataSource)
        {
            if (base.BindingContext != null)
            {
                if (((dataSource != null) && !(dataSource is IList)) && !(dataSource is IListSource))
                {
                    throw new ArgumentException("Not an IList or IListSource");
                }
                this.RemoveActiveHandlers();
                this.m_dataSource = dataSource;
                if (this.m_dataSource != null)
                {
                    this.m_listManager = (CurrencyManager) base.BindingContext[dataSource, null];
                    this.m_listManager.Refresh();
                }
                else
                {
                    this.m_listManager = null;
                }
                this.UnbindInnerList();
                this.ReloadDataSource();
                this.BindInnerList();
                this.SetActiveHandlers();
            }
            else
            {
                this.m_dataSource = dataSource;
            }
        }

        protected virtual void CalculateTextBoxArea()
        {
            int width = (base.ClientSize.Width - this.m_arrowBoxWidth) + this.m_borderWidth;
            int x = this.RightToLeft ? (this.m_arrowBoxWidth - this.m_borderWidth) : 0;
            if (width < 0)
            {
                width = 0;
            }
            this.m_textBoxTemplateArea = new Rectangle(x, 0, width, base.ClientSize.Height);
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

        public void Close()
        {
            this.Close(-1, -1);
        }

        internal void Close(int ii, int ci)
        {
            DropDownEventArgs e = new DropDownEventArgs(false, ii, ci);
            this.OnDropDownClosing(this, e);
            if (!e.Cancel)
            {
                this.ForceRedraw();
                this.m_list.Close();
                this.OnDropDownClosed(this, EventArgs.Empty);
            }
        }

        public void CloseConnector()
        {
            if (this.m_connector.IsOpen)
            {
                this.m_connector.Close();
            }
        }

        private void DeleteItem(int index)
        {
            if (this.Items != null)
            {
                this.Items.RemoveAt(index);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (sBrushes != null)
            {
                sBrushes.Clear();
            }
            sBrushes = null;
            if (disposing && (this.m_list != null))
            {
                this.m_list.Dispose();
                this.m_list = null;
            }
            this.m_items = null;
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
            if (this.m_dbConnector != null)
            {
                this.m_dbConnector.Dispose();
                this.m_dbConnector = null;
            }
            this.ButtonClick = null;
            this.CellClick = null;
            this.LinkClick = null;
            this.DropDown = null;
            this.DropDownClosed = null;
            this.SelectedIndexChanged = null;
            this.SelectedValueChanged = null;
            this.ValueMemberChanged = null;
            this.DisplayMemberChanged = null;
            Utility.Dispose();
            Links.Dispose();
            Resco.Controls.AdvancedComboBox.Mapping.DisposeEmptyMapping();
            ImageCache.GlobalCache.Clear();
            GC.Collect();
            base.Dispose(disposing);
        }

        internal bool DoDelayLoad()
        {
            bool flag = true;
            if (this.DbConnector.IsOpen)
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
            else if (!this.m_bManualDataLoading && ((this.m_dataSource == null) || (this.m_nItemsLoaded >= this.m_listManager.List.Count)))
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
                    for (height = this.m_list.ClientRectangle.Height; height > 0; height -= this.Items[this.Items.Count - 1].GetHeight(this.Templates))
                    {
                        this.m_bManualDataLoading = !this.AddItemManually();
                        if (!this.m_bManualDataLoading)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    height = this.m_list.ClientRectangle.Height;
                    while (this.m_nItemsLoaded < this.m_listManager.List.Count)
                    {
                        Resco.Controls.AdvancedComboBox.ListItem item = new BoundItem(this.TemplateIndex, this.SelectedTemplateIndex, this.AlternateTemplateIndex, this.TextBoxTemplateIndex, this.m_listManager.List[this.m_nItemsLoaded], this.m_boundMap as PropertyMapping);
                        this.m_nItemsInserted = this.Items.Count;
                        if (this.InsertItem(item, this.m_nItemsInserted) != this.m_nItemsInserted)
                        {
                            height -= this.Items[this.m_nItemsInserted].GetHeight(this.Templates);
                            if (height < 0)
                            {
                                this.m_nItemsLoaded++;
                                break;
                            }
                        }
                        this.m_nItemsLoaded++;
                    }
                }
                if (this.m_nItemsLoaded >= this.ExpectedItems)
                {
                    this.ExpectedItems = -1;
                }
                this.EndUpdate();
                Cursor.Current = Cursors.Default;
                this.OnDataLoaded(new DataLoadedEventArgs(!flag));
            }
            if (!flag)
            {
                this.ExpectedItems = -1;
            }
            return flag;
        }

        protected virtual void DrawArrowBox(Graphics gr)
        {
            SolidBrush brush = GetBrush(this.ForeColor);
            SolidBrush brush2 = GetBrush(this.BackColor);
            int num = this.m_arrowBoxWidth / 2;
            Rectangle arrowBoxRectangle = this.GetArrowBoxRectangle();
            if ((num % 2) == 1)
            {
                num++;
            }
            Point[] points = new Point[] { new Point(arrowBoxRectangle.Left + ((this.m_arrowBoxWidth - num) / 2), (base.ClientSize.Height - this.m_arrowHeight) / 2), new Point(arrowBoxRectangle.Left + (this.m_arrowBoxWidth / 2), (base.ClientSize.Height + this.m_arrowHeight) / 2), new Point((arrowBoxRectangle.Left + ((this.m_arrowBoxWidth + num) / 2)) + 1, (base.ClientSize.Height - this.m_arrowHeight) / 2) };
            if (this.m_list.Visible)
            {
                gr.FillRectangle(brush, arrowBoxRectangle);
            }
            else
            {
                brush2 = brush;
            }
            gr.FillPolygon(brush2, points);
        }

        protected virtual void DrawTextBox(Graphics gr)
        {
            int width = base.ClientSize.Width;
            int height = base.ClientSize.Height;
            GetBrush(this.ForeColor);
            gr.FillRectangle(this.m_BackColor, base.ClientRectangle);
            if (this.m_arrowBoxWidth > 0)
            {
                this.DrawArrowBox(gr);
            }
            this.ResetCache();
            if (this.SelectedItem != null)
            {
                Region clip = gr.Clip;
                gr.Clip = new Region(this.m_textBoxTemplateArea);
                this.m_bDrawingTextBox = true;
                this.TextBoxTemplate.Draw(gr, this.m_textBoxTemplateArea, this.m_textBoxItem);
                this.m_bDrawingTextBox = false;
                gr.Clip = clip;
            }
            this.m_bIsChange = false;
        }

        public bool EndUpdate()
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
                if (this.m_bSetListBoundsInEndUpdate)
                {
                    this.SetListBounds(true);
                    this.m_bSetListBoundsInEndUpdate = false;
                }
            }
            return true;
        }

        internal void ForceRedraw()
        {
            this.m_bIsChange = true;
            base.Invalidate();
        }

        private Rectangle GetArrowBoxRectangle()
        {
            return new Rectangle(this.RightToLeft ? -this.m_borderWidth : ((base.ClientSize.Width - this.m_arrowBoxWidth) + this.m_borderWidth), 0, this.m_arrowBoxWidth, base.ClientSize.Height);
        }

        private Rectangle GetBoundsInTopLevelControl(Control c)
        {
            Rectangle bounds = c.Bounds;
            for (Control control = c.Parent; (control != null) && (control.Parent != null); control = control.Parent)
            {
                bounds.Offset(control.Bounds.Left, control.Bounds.Top);
            }
            return bounds;
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
            Rectangle arrowBoxRectangle = this.GetArrowBoxRectangle();
            CellEventArgs args = null;
            if (arrowBoxRectangle.Contains(pt))
            {
                return null;
            }
            if (this.m_textBoxItem != null)
            {
                ItemTemplate textBoxTemplate = this.TextBoxTemplate;
                int ci = textBoxTemplate.GetCellClick(pt.X, pt.Y, this.m_textBoxItem);
                Cell c = null;
                if (ci >= 0)
                {
                    c = textBoxTemplate[ci];
                }
                if ((this.m_selectedItemIndex >= 0) && (this.m_selectedItemIndex < this.m_items.Count))
                {
                    args = new CellEventArgs(this.m_items[this.m_selectedItemIndex], c, -1, ci, 0);
                }
            }
            return args;
        }

        internal int GetClientSizeHeight()
        {
            if (this.m_bDrawingTextBox)
            {
                return base.ClientSize.Height;
            }
            return this.List.ClientSize.Height;
        }

        internal int GetClientSizeWidth()
        {
            if (this.m_bDrawingTextBox)
            {
                return base.ClientSize.Width;
            }
            return this.List.ClientSize.Width;
        }

        public string GetItemText(object item)
        {
            for (int i = 0; i < this.m_items.Count; i++)
            {
                if ((this.m_items[i] == item) || ((this.m_items[i] is BoundItem) && (((BoundItem) this.m_items[i]).Data == item)))
                {
                    if ((this.m_displayMember != null) && !(this.m_displayMember == string.Empty))
                    {
                        return this.m_items[i][this.m_displayMember].ToString();
                    }
                    return this.m_items[i].ToString();
                }
            }
            return null;
        }

        public static Pen GetPen(Color c)
        {
            sPen.Color = c;
            return sPen;
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
                Resco.Controls.AdvancedComboBox.ListItem item = new BoundItem(this.TemplateIndex, this.SelectedTemplateIndex, this.AlternateTemplateIndex, this.TextBoxTemplateIndex, this.m_listManager.List[index], this.m_boundMap as PropertyMapping);
                this.InsertItem(item, index);
            }
            else
            {
                this.ReloadDataSource();
            }
        }

        public int InsertItem(Resco.Controls.AdvancedComboBox.ListItem item, int insertIndex)
        {
            ValidateDataArgs e = new ValidateDataArgs(item, insertIndex);
            if (this.ValidateData != null)
            {
                this.ValidateData(this, e);
            }
            if (!e.Skip)
            {
                insertIndex = e.InsertIndex;
                if (insertIndex < 0)
                {
                    this.Items.Add(e.ListItem);
                    return insertIndex;
                }
                this.Items.Insert(insertIndex++, e.ListItem);
            }
            return insertIndex;
        }

        public Resco.Controls.AdvancedComboBox.Mapping LoadData()
        {
            return this.LoadData(-1);
        }

        public Resco.Controls.AdvancedComboBox.Mapping LoadData(int iInsertIndex)
        {
            if (this.DataConnector == null)
            {
                return null;
            }
            try
            {
                this.CloseConnector();
                if (!this.DataConnector.Open())
                {
                    return null;
                }
                this.m_mapLast = this.DataConnector.Mapping;
                this.BeginUpdate();
                this.m_iInsertIndex = iInsertIndex;
                if (this.LoadDataChunk(this.DelayLoad && (iInsertIndex <= this.Items.Count)))
                {
                    this.CloseConnector();
                }
                else if (this.LoadDataChunk(this.DelayLoad && (iInsertIndex <= this.Items.Count)))
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

        internal bool LoadDataChunk(bool bDelay)
        {
            int height = base.Height;
            while (this.DataConnector.MoveNext())
            {
                Resco.Controls.AdvancedComboBox.ListItem item = new Resco.Controls.AdvancedComboBox.ListItem(this.TemplateIndex, this.SelectedTemplateIndex, this.AlternateTemplateIndex, this.TextBoxTemplateIndex, this.DataConnector.Current, this.m_mapLast);
                this.m_iInsertIndex = this.InsertItem(item, this.m_iInsertIndex);
                height -= item.GetHeight(this.Templates);
                if (bDelay && (height < 0))
                {
                    return false;
                }
            }
            return true;
        }

        public void LoadDataManually()
        {
            if (this.ItemAdding == null)
            {
                throw new InvalidOperationException("ItemAdding event does not have a handler.");
            }
            if (!this.DelayLoad)
            {
                this.BeginUpdate();
                while (!this.AddItemManually())
                {
                }
                this.EndUpdate();
            }
            else
            {
                this.m_bManualDataLoading = true;
                this.OnChange(null, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
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
                            if (name == "AdvancedComboBox")
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
                    this.ReadAdvancedComboBox(reader);
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
            this.SelectedIndex = this.m_innerListManager.Position;
        }

        public bool NextSelectableCell()
        {
            if (this.m_textBoxItem != null)
            {
                int index;
                ItemTemplate.CellCollection cellTemplates = this.TextBoxTemplate.CellTemplates;
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

        protected override void OnBindingContextChanged(EventArgs e)
        {
            this.BindTo(this.m_dataSource);
            base.OnBindingContextChanged(e);
        }

        protected internal virtual void OnButton(ButtonCell c, Resco.Controls.AdvancedComboBox.ListItem item, Point index, int yOffset)
        {
            if (this.ButtonClick != null)
            {
                this.ButtonClick(this, new CellEventArgs(item, c, index.X, index.Y, yOffset));
            }
        }

        protected internal virtual void OnCellClick(CellEventArgs cea)
        {
            if (this.CellClick != null)
            {
                this.CellClick(this, cea);
            }
        }

        protected internal virtual void OnCellEntered(CellEnteredMainEventArgs e, Resco.Controls.AdvancedComboBox.AdvancedComboBox parent)
        {
            object constantData;
            switch (e.Cell.CellSource.SourceType)
            {
                case CellSourceType.Constant:
                    constantData = e.Cell.CellSource.ConstantData;
                    break;

                case CellSourceType.ColumnIndex:
                    constantData = e.ListItem[e.Cell.CellSource.ColumnIndex];
                    break;

                case CellSourceType.ColumnName:
                    constantData = e.ListItem[e.Cell.CellSource.ColumnName];
                    break;

                case CellSourceType.DisplayMember:
                    if (e.Cell.CellSource.DisplayMember)
                    {
                        constantData = e.ListItem[parent.DisplayMember];
                    }
                    else
                    {
                        constantData = null;
                    }
                    break;

                default:
                    constantData = null;
                    break;
            }
            e.Cell._FireCellEntered(new CellEnteredEventArgs(e.Cell, e.ListItem, constantData));
            if (this.CellEntered != null)
            {
                this.CellEntered(this, e);
            }
        }

        private void OnChange(object sender, ComboBoxEventArgsType e, ComboBoxArgs args)
        {
            if (this.InvokeRequired && (this.OnChangeHandler != null))
            {
                base.Invoke(this.OnChangeHandler, new object[] { sender, e, args });
            }
            else
            {
                this.OnChangeSafe(sender, e, args);
            }
        }

        private void OnChangeSafe(object sender, ComboBoxEventArgsType e, ComboBoxArgs args)
        {
            if (this.m_iUpdateCounter <= 0)
            {
                if ((args.UpdateRange & ComboBoxUpdateRange.Box) != ((ComboBoxUpdateRange) 0))
                {
                    switch (e)
                    {
                        case ComboBoxEventArgsType.Repaint:
                            this.m_bIsChange = true;
                            break;

                        case ComboBoxEventArgsType.Refresh:
                        {
                            ComboBoxRefreshArgs args2 = args as ComboBoxRefreshArgs;
                            if (args2 != null)
                            {
                                if (((this.m_textBoxItem != null) && args2.ResetBounds) && ((args2.Template == null) || (args2.Template == this.TextBoxTemplate)))
                                {
                                    this.m_textBoxItem.ResetCachedBounds();
                                }
                                this.CalculateTextBoxArea();
                                this.m_bIsChange = true;
                            }
                            break;
                        }
                    }
                    base.Invalidate();
                }
                if (((args.UpdateRange & ComboBoxUpdateRange.List) != ((ComboBoxUpdateRange) 0)) && (this.m_list != null))
                {
                    this.m_list.OnListChange(sender, e, args);
                }
            }
        }

        internal void OnClear(object sender)
        {
            this.m_textBoxItem = null;
            this.m_selectedItemIndex = -1;
            this.m_iSelectedCellIndex = -1;
            this.m_list.OnClear(sender);
        }

        protected override void OnClick(EventArgs e)
        {
            int ci = -1;
            base.OnClick(e);
            this.m_Timer.Enabled = false;
            if (!this.m_bShowingToolTip)
            {
                Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                CellEventArgs cellAtPoint = this.GetCellAtPoint(pt);
                if (cellAtPoint != null)
                {
                    if (this.CheckForLink(pt))
                    {
                        LinkCell c = cellAtPoint.Cell as LinkCell;
                        if (c != null)
                        {
                            this.OnLink(c, cellAtPoint.ListItem, new Point(cellAtPoint.ItemIndex, cellAtPoint.CellIndex), 0, this.m_textBoxTemplateArea.Width);
                        }
                        return;
                    }
                    if (this.CheckForButton(pt))
                    {
                        return;
                    }
                    this.OnCellClick(cellAtPoint);
                    if ((cellAtPoint.CellIndex >= 0) && cellAtPoint.Cell.Selectable)
                    {
                        try
                        {
                            this.SelectedCell = cellAtPoint.Cell;
                        }
                        catch
                        {
                        }
                        this.OnCellEntered(new CellEnteredMainEventArgs(cellAtPoint.Cell, cellAtPoint.CellIndex, cellAtPoint.ListItem), this);
                    }
                    else
                    {
                        this.SelectedCell = null;
                    }
                    ci = cellAtPoint.CellIndex;
                }
                if (!this.m_list.Visible)
                {
                    this.Popup(this.m_selectedItemIndex, ci);
                }
                else
                {
                    this.Close(this.m_selectedItemIndex, ci);
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

        private void OnDisplayMemberChanged(object sender, EventArgs e)
        {
            if (this.DisplayMemberChanged != null)
            {
                this.DisplayMemberChanged(sender, e);
            }
        }

        private void OnDropDown(object sender, EventArgs e)
        {
            if (this.DropDown != null)
            {
                this.DropDown(sender, e);
            }
        }

        private void OnDropDownClosed(object sender, EventArgs e)
        {
            if (this.DropDownClosed != null)
            {
                this.DropDownClosed(sender, e);
            }
        }

        private void OnDropDownClosing(object sender, DropDownEventArgs e)
        {
            if (this.DropDownClosing != null)
            {
                this.DropDownClosing(sender, e);
            }
        }

        private void OnDroppingDown(object sender, DropDownEventArgs e)
        {
            if (this.DroppingDown != null)
            {
                this.DroppingDown(sender, e);
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
                    if (hTCGyroSensorNavigation && (this.SelectedIndex < (this.Items.Count - 1)))
                    {
                        this.SelectedIndex++;
                    }
                    this.m_gyroScrolling = true;
                    this.m_HTCGytoEventArgs.Direction |= HTCDirection.Down;
                }
                if (num2 < -this.m_HTCGyroSensitivity.Y)
                {
                    if (hTCGyroSensorNavigation && (this.SelectedIndex > 0))
                    {
                        this.SelectedIndex--;
                    }
                    this.m_gyroScrolling = true;
                    this.m_HTCGytoEventArgs.Direction |= HTCDirection.Up;
                }
                if (num3 > this.m_HTCGyroSensitivity.X)
                {
                    if (hTCGyroSensorNavigation)
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
                    if (hTCGyroSensorNavigation)
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
                    if ((e.RadialDelta > 0.0) && (this.SelectedIndex < (this.Items.Count - 1)))
                    {
                        this.SelectedIndex++;
                    }
                    else if (this.SelectedIndex > 0)
                    {
                        this.SelectedIndex--;
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

        internal void OnItemAdded(Resco.Controls.AdvancedComboBox.ListItem item, int index)
        {
            this.List.OnItemAdded(item, index);
            this.SetListBounds(true);
            if (item.Selected)
            {
                this.SelectedIndex = item.Index;
                this.m_list.EnsureVisible(item);
            }
            else
            {
                this.m_selectedItemIndex = this.m_list.SelectedItemIndex;
            }
            this.OnChange(index, ComboBoxEventArgsType.Repaint, new ComboBoxArgs(ComboBoxUpdateRange.Box));
        }

        protected virtual void OnItemAdding(ItemAddingEventArgs e)
        {
            if (this.ItemAdding != null)
            {
                this.ItemAdding(this, e);
            }
        }

        protected internal virtual void OnItemEntered(ItemEnteredEventArgs ieea)
        {
            if (this.ItemEntered != null)
            {
                this.ItemEntered(this, ieea);
            }
        }

        internal void OnItemRemoved(Resco.Controls.AdvancedComboBox.ListItem item, int index)
        {
            if (this.InvokeRequired && (this.OnItemRemovedHandler != null))
            {
                base.Invoke(this.OnItemRemovedHandler, new object[] { item, index });
            }
            else
            {
                this.OnItemRemovedSafe(item, index);
            }
        }

        internal void OnItemRemovedSafe(Resco.Controls.AdvancedComboBox.ListItem item, int index)
        {
            this.List.OnItemRemoved(item, index);
            this.SetListBounds(true);
            if (item.Selected)
            {
                this.SelectedIndex = -1;
            }
            else if (index < this.m_selectedItemIndex)
            {
                this.m_selectedItemIndex = this.m_list.SelectedItemIndex;
            }
            this.OnChange(index, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Box);
        }

        private void OnItemSelect(object sender, ItemEventArgs e)
        {
            if (this.m_selectedItemIndex != e.ItemIndex)
            {
                this.m_selectedItemIndex = e.ItemIndex;
                if ((this.m_selectedItemIndex >= 0) && (this.m_selectedItemIndex < this.m_items.Count))
                {
                    this.m_textBoxItem = this.m_items[this.m_selectedItemIndex].Clone();
                }
                else
                {
                    this.m_textBoxItem = null;
                }
                if ((this.Items != null) && (this.m_innerListManager != null))
                {
                    this.m_innerListManager.PositionChanged -= new EventHandler(this.m_innerListManager_PositionChanged);
                    this.m_innerListManager.Position = this.m_selectedItemIndex;
                    this.m_innerListManager.PositionChanged += new EventHandler(this.m_innerListManager_PositionChanged);
                }
                this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(false, ComboBoxUpdateRange.All));
                this.OnSelectedIndexChanged(this, EventArgs.Empty);
                this.OnSelectedValueChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.KeyNavigation)
            {
                if (this.m_list.Visible)
                {
                    this.m_list.HandleKeyDown(e);
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Prior:
                            if ((this.SelectedIndex != 0) && (this.Items.Count > 0))
                            {
                                this.SelectedIndex = 0;
                                e.Handled = true;
                            }
                            goto Label_01F3;

                        case Keys.PageDown:
                            if (this.SelectedIndex != (this.Items.Count - 1))
                            {
                                this.SelectedIndex = this.Items.Count - 1;
                                e.Handled = true;
                            }
                            goto Label_01F3;

                        case Keys.End:
                        case Keys.Home:
                            goto Label_01F3;

                        case Keys.Left:
                            if (!this.RightToLeft)
                            {
                                e.Handled = this.PreviousSelectableCell();
                            }
                            else
                            {
                                e.Handled = this.NextSelectableCell();
                            }
                            goto Label_01F3;

                        case Keys.Up:
                            if (this.SelectedIndex <= 0)
                            {
                                if (base.TopLevelControl != null)
                                {
                                    base.TopLevelControl.SelectNextControl(this, false, true, true, true);
                                }
                            }
                            else
                            {
                                this.SelectedIndex--;
                                e.Handled = true;
                            }
                            goto Label_01F3;

                        case Keys.Right:
                            if (!this.RightToLeft)
                            {
                                e.Handled = this.NextSelectableCell();
                            }
                            else
                            {
                                e.Handled = this.PreviousSelectableCell();
                            }
                            goto Label_01F3;

                        case Keys.Down:
                            if (this.SelectedIndex >= (this.Items.Count - 1))
                            {
                                if (base.TopLevelControl != null)
                                {
                                    base.TopLevelControl.SelectNextControl(this, true, true, true, true);
                                }
                            }
                            else
                            {
                                this.SelectedIndex++;
                                e.Handled = true;
                            }
                            goto Label_01F3;

                        case Keys.Return:
                            if ((this.Items != null) && (this.SelectedCell != null))
                            {
                                ItemTemplate.CellCollection cellTemplates = this.TextBoxTemplate.CellTemplates;
                                if (this.SelectedCell is ButtonCell)
                                {
                                    this.m_textBoxItem.PressedButtonIndex = this.m_iSelectedCellIndex;
                                    this.OnChange(this, ComboBoxEventArgsType.Repaint, new ComboBoxArgs(ComboBoxUpdateRange.Box));
                                }
                            }
                            goto Label_01F3;
                    }
                }
            }
        Label_01F3:
            base.OnKeyDown(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!this.m_list.Visible)
            {
                base.OnKeyPress(e);
            }
            else
            {
                this.m_list.HandleKeyPress(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (this.KeyNavigation)
            {
                if (!this.m_list.Visible)
                {
                    if (e.KeyCode == Keys.Return)
                    {
                        bool flag = true;
                        int iCellIndex = -1;
                        if ((this.Items != null) && (this.SelectedCell != null))
                        {
                            ItemTemplate.CellCollection cellTemplates = this.TextBoxTemplate.CellTemplates;
                            if (this.SelectedCell.Selectable && !(this.SelectedCell is ButtonCell))
                            {
                                iCellIndex = cellTemplates.IndexOf(this.SelectedCell);
                                Resco.Controls.AdvancedComboBox.ListItem i = this.Items[this.SelectedIndex];
                                this.OnCellEntered(new CellEnteredMainEventArgs(this.SelectedCell, iCellIndex, i), this);
                                flag = false;
                            }
                            if (this.SelectedCell is ButtonCell)
                            {
                                int pressedButtonIndex = this.m_textBoxItem.PressedButtonIndex;
                                this.m_textBoxItem.PressedButtonIndex = -1;
                                this.OnChange(this, ComboBoxEventArgsType.Repaint, new ComboBoxArgs(ComboBoxUpdateRange.Box));
                                this.OnButton((ButtonCell) this.SelectedCell, this.m_textBoxItem, new Point(-1, pressedButtonIndex), 0);
                                flag = false;
                            }
                        }
                        if (flag)
                        {
                            this.Popup(this.SelectedIndex, iCellIndex);
                        }
                        e.Handled = true;
                    }
                }
                else
                {
                    this.m_list.HandleKeyUp(e);
                }
            }
            base.OnKeyUp(e);
        }

        protected internal virtual void OnLink(LinkCell c, Resco.Controls.AdvancedComboBox.ListItem item, Point index, int yOffset, int width)
        {
            LinkEventArgs e = new LinkEventArgs(item, c, index.X, index.Y, yOffset);
            if (this.LinkClick != null)
            {
                this.LinkClick(this, e);
            }
            Links.AddLink(e.Target);
            this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
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
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                    return;

                case ListChangedType.ItemMoved:
                    try
                    {
                        this.BeginUpdate();
                        this.DeleteItem(args.OldIndex);
                        this.InsertItem(args.NewIndex);
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                    return;

                case ListChangedType.ItemChanged:
                    if ((args.NewIndex >= 0) && (args.NewIndex < this.Items.Count))
                    {
                        this.Items[args.NewIndex].ResetCachedBounds();
                    }
                    if ((args.NewIndex >= this.m_list.TopItemIndex) && (args.NewIndex <= this.Items.LastDrawnItem))
                    {
                        this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                        return;
                    }
                    return;

                case ListChangedType.PropertyDescriptorAdded:
                case ListChangedType.PropertyDescriptorDeleted:
                case ListChangedType.PropertyDescriptorChanged:
                    this.BindTo(this.m_dataSource);
                    return;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if ((this.m_list != null) && !this.m_list.Focused)
            {
                base.OnLostFocus(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.m_MousePosition.X = e.X;
            this.m_MousePosition.Y = e.Y;
            Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
            CellEventArgs cellAtPoint = this.GetCellAtPoint(pt);
            if ((cellAtPoint != null) && this.CheckForButton(pt))
            {
                if (cellAtPoint.Cell != null)
                {
                    this.m_bButtonPressed = true;
                    this.m_textBoxItem.PressedButtonIndex = cellAtPoint.CellIndex;
                    this.OnChange(this, ComboBoxEventArgsType.Repaint, new ComboBoxArgs(ComboBoxUpdateRange.Box));
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
                if (this.m_bButtonPressed && (this.m_textBoxItem != null))
                {
                    int pressedButtonIndex = this.m_textBoxItem.PressedButtonIndex;
                    this.m_textBoxItem.PressedButtonIndex = -1;
                    this.m_bButtonPressed = false;
                    Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    CellEventArgs cellAtPoint = this.GetCellAtPoint(pt);
                    ButtonCell c = cellAtPoint.Cell as ButtonCell;
                    if (((cellAtPoint != null) && (c != null)) && (cellAtPoint.CellIndex == pressedButtonIndex))
                    {
                        this.OnButton(c, cellAtPoint.ListItem, new Point(-1, cellAtPoint.CellIndex), 0);
                    }
                    this.OnChange(this, ComboBoxEventArgsType.Repaint, new ComboBoxArgs(ComboBoxUpdateRange.Box));
                }
                if (this.m_bShowingToolTip)
                {
                    if (this.m_ToolTip != null)
                    {
                        this.m_ToolTip.Visible = false;
                    }
                    this.m_bShowingToolTip = false;
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if ((this.m_iUpdateCounter == 0) && (this.m_iNoRedraw <= 0))
            {
                this.Redraw(e.Graphics);
            }
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (this.Items != null)
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
            if (this.m_doubleBuffered && (this.m_grBackBuffer != null))
            {
                this.UpdateDoubleBuffering();
            }
            this.SetListBounds(false);
            this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true));
            base.OnResize(e);
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(sender, e);
            }
        }

        private void OnSelectedValueChanged(object sender, EventArgs e)
        {
            if (this.SelectedValueChanged != null)
            {
                this.SelectedValueChanged(sender, e);
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

        private void OnValueMemberChanged(object sender, EventArgs e)
        {
            if (this.ValueMemberChanged != null)
            {
                this.ValueMemberChanged(sender, e);
            }
        }

        public void Popup()
        {
            this.Popup(-1, -1);
        }

        internal void Popup(int ii, int ci)
        {
            DropDownEventArgs e = new DropDownEventArgs(false, ii, ci);
            this.OnDroppingDown(this, e);
            if (!e.Cancel && (this.m_list != null))
            {
                bool bRecalculateHeight = false;
                if (this.m_list.Parent == null)
                {
                    Graphics graphics = base.CreateGraphics();
                    if (graphics != null)
                    {
                        int num = (int) (graphics.DpiX / 96f);
                        this.m_list.Scale(new SizeF((float) num, (float) num));
                        base.TopLevelControl.Controls.Add(this.m_list);
                        base.TopLevelControl.Resize += new EventHandler(this.TopLevelControl_Resize);
                        bRecalculateHeight = true;
                        graphics.Dispose();
                    }
                }
                this.SetListBounds(bRecalculateHeight);
                this.ForceRedraw();
                this.m_list.Popup();
                this.OnDropDown(this, EventArgs.Empty);
            }
        }

        public bool PreviousSelectableCell()
        {
            if (this.m_textBoxItem != null)
            {
                int index;
                ItemTemplate.CellCollection cellTemplates = this.TextBoxTemplate.CellTemplates;
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

        internal void RaiseOnKeyDown(KeyEventArgs e)
        {
            this.OnKeyDown(e);
        }

        internal void RaiseOnKeyPress(KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        internal void RaiseOnKeyUp(KeyEventArgs e)
        {
            this.OnKeyUp(e);
        }

        private void ReadAdvancedComboBox(XmlReader reader)
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
                    goto Label_01D1;
                }
                return;
            Label_0051:
                try
                {
                    int num;
                    string name = reader.Name;
                    if (name == null)
                    {
                        goto Label_01D1;
                    }
                    if (BigHas.methodxxx.TryGetValue(name,out num))//if (<PrivateImplementationDetails>{611E01EE-A6E8-4C9D-9981-A03ED43F828B}.$$method0x6000383-1.TryGetValue(name, ref num))
                    {
                        switch (num)
                        {
                            case 0:
                                goto Label_01D1;

                            case 1:
                                if (!reader.IsEmptyElement)
                                {
                                    this.ReadConnector(reader);
                                }
                                goto Label_01D1;

                            case 2:
                                this.Templates.Add(this.ReadItemTemplate(reader));
                                goto Label_01D1;

                            case 3:
                                goto Label_0112;

                            case 4:
                                if ((reader.HasAttributes && (reader.GetAttribute("Name") != null)) && (reader.GetAttribute("Value") != null))
                                {
                                    this.m_conversion.SetProperty(this, reader.GetAttribute("Name"), reader.GetAttribute("Value"));
                                }
                                goto Label_01D1;
                        }
                    }
                    goto Label_01B4;
                Label_0112:
                    this.BeginUpdate();
                    foreach (ItemTemplate template in this.Templates)
                    {
                        template.Width = base.ClientSize.Width;
                    }
                    this.EndUpdate();
                    return;
                Label_01B4:
                    this.m_conversion.SetProperty(this, reader.Name, reader.ReadString());
                }
                catch
                {
                }
            Label_01D1:
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
                    typeName = "Resco.Controls.AdvancedComboBox.Cell";
                }
                if (!typeName.StartsWith("Resco.Controls.AdvancedComboBox.") && typeName.StartsWith("Resco.Controls."))
                {
                    typeName = typeName.Insert(typeName.LastIndexOf('.'), ".AdvancedComboBox");
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

        private ItemTemplate ReadItemTemplate(XmlReader reader)
        {
            ItemTemplate o = new ItemTemplate();
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
                    if (!(str2 == "ItemTemplate"))
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

        protected virtual void Redraw(Graphics gr)
        {
            if (this.m_doubleBuffered)
            {
                if (this.m_bIsChange)
                {
                    this.DrawTextBox(this.m_grBackBuffer);
                }
                gr.DrawImage(this.m_backBuffer, 0, 0);
            }
            else
            {
                this.DrawTextBox(gr);
            }
        }

        public Resco.Controls.AdvancedComboBox.Mapping Reload()
        {
            int iInsertIndex = -1;
            if (this.m_mapLast != null)
            {
                this.BeginUpdate();
                iInsertIndex = this.Items.RemoveByMapping(this.m_mapLast);
            }
            Resco.Controls.AdvancedComboBox.Mapping mapping = this.LoadData(iInsertIndex);
            this.EndUpdate();
            return mapping;
        }

        private void ReloadDataSource()
        {
            if (this.m_boundMap != Resco.Controls.AdvancedComboBox.Mapping.Empty)
            {
                this.Items.RemoveByMapping(this.m_boundMap);
            }
            if (this.m_listManager != null)
            {
                this.m_boundMap = new PropertyMapping(this.m_listManager.GetItemProperties());
                if (!this.DelayLoad)
                {
                    int insertIndex = 0;
                    for (int i = 0; i < this.m_listManager.List.Count; i++)
                    {
                        Resco.Controls.AdvancedComboBox.ListItem item = new BoundItem(this.TemplateIndex, this.SelectedTemplateIndex, this.AlternateTemplateIndex, this.TextBoxTemplateIndex, this.m_listManager.List[i], this.m_boundMap as PropertyMapping);
                        int num3 = this.InsertItem(item, insertIndex);
                        if (num3 != insertIndex)
                        {
                            insertIndex = num3;
                        }
                    }
                }
                else
                {
                    this.m_nItemsLoaded = 0;
                }
            }
            else
            {
                this.m_boundMap = Resco.Controls.AdvancedComboBox.Mapping.Empty;
            }
        }

        private void RemoveActiveHandlers()
        {
            if ((this.m_listManager != null) && (this.m_listManager.List is IBindingList))
            {
                ((IBindingList) this.m_listManager.List).ListChanged -= new ListChangedEventHandler(this.OnListChanged);
            }
        }

        private void ResetCache()
        {
            this.m_alLinks.Clear();
            this.m_alButtons.Clear();
            this.m_alTooltips.Clear();
        }

        public void ResetImageCache()
        {
            ImageCache.GlobalCache.Clear();
            this.OnChange(this, ComboBoxEventArgsType.Refresh, ComboBoxArgs.Default);
        }

        public void ResetImageCache(ImageList il)
        {
            ImageCache.GlobalCache.Clear(il);
            this.OnChange(this, ComboBoxEventArgsType.Refresh, ComboBoxArgs.Default);
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
                this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
                if (!this.m_bForceRedraw)
                {
                    base.Update();
                }
                this.m_bForceRedraw = false;
            }
        }

        public void SaveXml(string fileName)
        {
            ACBXmlSerializer.SaveXml(fileName, this);
        }

        public void SaveXml(XmlWriter writer)
        {
            ACBXmlSerializer.SaveXml(writer, this);
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.m_scaleFactorX = factor.Width;
            this.m_scaleFactorY = factor.Height;
            if ((this.m_scaleFactorX != 1.0) || (this.m_scaleFactorY != 1.0))
            {
                this.m_tsCurrent.Scale(this.m_scaleFactorX, this.m_scaleFactorY);
                this.DefaultTemplates.Scale(this.m_scaleFactorX, this.m_scaleFactorY);
                if (this.m_items != null)
                {
                    this.m_items.ResetCachedBounds(null);
                }
                sPen.Width *= this.m_scaleFactorX;
                this.m_borderWidth = (int) (this.m_borderWidth * this.m_scaleFactorX);
                this.m_arrowBoxWidth = (int) (this.m_arrowBoxWidth * this.m_scaleFactorX);
                this.m_arrowHeight = (int) (this.m_arrowHeight * this.m_scaleFactorY);
                TooltipWidth = (int) (TooltipWidth * this.m_scaleFactorX);
                this.CalculateTextBoxArea();
                this.m_maxListHeight = (int) (this.m_maxListHeight * this.m_scaleFactorX);
                this.m_emptyListHeight = (int) (this.m_emptyListHeight * this.m_scaleFactorX);
            }
            base.ScaleControl(factor, specified);
        }

        private void SetActiveHandlers()
        {
            if ((this.m_listManager != null) && (this.m_listManager.List is IBindingList))
            {
                ((IBindingList) this.m_listManager.List).ListChanged += new ListChangedEventHandler(this.OnListChanged);
            }
        }

        protected virtual void SetListBounds(bool bRecalculateHeight)
        {
            if (((this.m_list != null) && (this.m_list.Parent != null)) && (base.TopLevelControl != null))
            {
                if (this.m_iUpdateCounter > 0)
                {
                    this.m_bSetListBoundsInEndUpdate = true;
                }
                else if (this.m_bFullScreenList)
                {
                    this.m_list.Bounds = this.m_list.Parent.ClientRectangle;
                }
                else
                {
                    int num5;
                    int cachedItemsHeight = 0;
                    int num2 = 0;
                    int expectedItems = (this.Items != null) ? this.Items.Count : 0;
                    Rectangle boundsInTopLevelControl = this.GetBoundsInTopLevelControl(this);
                    int num3 = (this.m_list.Parent.ClientSize.Height - boundsInTopLevelControl.Bottom) - this.m_borderWidth;
                    int num4 = boundsInTopLevelControl.Top + this.m_borderWidth;
                    num2 = (num3 > num4) ? num3 : num4;
                    if (this.DelayLoad && (this.ExpectedItems > expectedItems))
                    {
                        expectedItems = this.ExpectedItems;
                    }
                    if ((expectedItems == 0) || (this.m_listHeight >= 0))
                    {
                        cachedItemsHeight = (this.m_listHeight >= 0) ? this.m_listHeight : this.m_emptyListHeight;
                    }
                    else if (bRecalculateHeight)
                    {
                        for (int i = 0; (i < expectedItems) && (cachedItemsHeight < this.m_list.Parent.ClientSize.Height); i++)
                        {
                            if ((this.Items != null) && (i < this.Items.Count))
                            {
                                Resco.Controls.AdvancedComboBox.ListItem item = this.Items[i];
                                cachedItemsHeight += item.GetTemplate(this.Templates).GetHeight(item);
                            }
                            else
                            {
                                cachedItemsHeight += this.DefaultTemplates[0].Height;
                            }
                            if (this.ListGridLines)
                            {
                                cachedItemsHeight++;
                            }
                        }
                        this.m_cachedItemsHeight = cachedItemsHeight;
                    }
                    else
                    {
                        cachedItemsHeight = this.m_cachedItemsHeight;
                    }
                    if ((this.m_maxListHeight > 0) && (cachedItemsHeight > this.m_maxListHeight))
                    {
                        cachedItemsHeight = this.m_maxListHeight;
                    }
                    if (cachedItemsHeight > num2)
                    {
                        cachedItemsHeight = num2;
                    }
                    if ((cachedItemsHeight <= num3) || (num3 > num4))
                    {
                        num5 = boundsInTopLevelControl.Bottom - this.m_borderWidth;
                    }
                    else
                    {
                        num5 = (boundsInTopLevelControl.Top - cachedItemsHeight) - this.m_borderWidth;
                    }
                    this.m_list.Bounds = new Rectangle(boundsInTopLevelControl.Left, num5, boundsInTopLevelControl.Width, cachedItemsHeight + (2 * this.m_borderWidth));
                }
            }
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
            return (this.m_list.TouchScrollDirection != Resco.Controls.AdvancedComboBox.TouchScrollDirection.Inverse);
        }

        public void SuspendRedraw()
        {
            this.m_iNoRedraw++;
        }

        private void TopLevelControl_Resize(object sender, EventArgs e)
        {
            this.SetListBounds(false);
        }

        private void UnbindInnerList()
        {
            if (this.m_innerListManager != null)
            {
                this.m_innerListManager.PositionChanged -= new EventHandler(this.m_innerListManager_PositionChanged);
            }
            this.m_innerListManager = null;
        }

        private void UpdateDoubleBuffering()
        {
            GC.Collect();
            int width = 1;
            int height = 1;
            try
            {
                width = (base.Width < 1) ? 1 : base.Width;
                height = (base.Height < 1) ? 1 : base.Height;
            }
            catch
            {
            }
            if ((this.m_backBuffer != null) && ((this.m_backBuffer.Width != width) || (this.m_backBuffer.Height != height)))
            {
                this.m_backBuffer.Dispose();
                this.m_backBuffer = null;
                if (this.m_grBackBuffer != null)
                {
                    this.m_grBackBuffer.Dispose();
                }
                this.m_grBackBuffer = null;
            }
            if (this.m_doubleBuffered && (this.m_backBuffer == null))
            {
                this.m_backBuffer = new Bitmap(width, height);
                this.m_grBackBuffer = Graphics.FromImage(this.m_backBuffer);
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

        [DefaultValue(13)]
        public int ArrowBoxWidth
        {
            get
            {
                return this.m_arrowBoxWidth;
            }
            set
            {
                if (this.m_arrowBoxWidth != value)
                {
                    this.m_arrowBoxWidth = value;
                    this.OnChange(this, ComboBoxEventArgsType.Refresh, ComboBoxArgs.Box);
                }
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
                    this.UnbindInnerList();
                    this.m_autoBind = value;
                    this.BindInnerList();
                }
            }
        }

        [DefaultValue(false)]
        public bool AutoHideDropDownList
        {
            get
            {
                return this.m_autoHideDropDownList;
            }
            set
            {
                if (this.m_autoHideDropDownList != value)
                {
                    this.m_autoHideDropDownList = value;
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

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                this.m_BackColor = new SolidBrush(value);
                this.m_list.BackColor = value;
                this.OnChange(this, ComboBoxEventArgsType.Repaint, ComboBoxArgs.Default);
            }
        }

        [DefaultValue("FixedSingle")]
        public System.Windows.Forms.BorderStyle BorderStyle
        {
            get
            {
                return base.BorderStyle;
            }
            set
            {
                if (base.BorderStyle != value)
                {
                    base.BorderStyle=value;//.set_BorderStyle(value);
                    if (base.BorderStyle == System.Windows.Forms.BorderStyle.None)
                    {
                        this.m_borderWidth = 0;
                    }
                    else
                    {
                        this.m_borderWidth = (int) this.m_scaleFactorX;
                    }
                    if (this.m_list != null)
                    {
                        this.m_list.BorderStyle=value;//.set_BorderStyle(value);
                    }
                    this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(ComboBoxUpdateRange.All));
                }
            }
        }

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
                    this.m_nItemsLoaded = 0;
                    this.m_nItemsInserted = 0;
                    this.BindTo(value);
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public Resco.Controls.AdvancedComboBox.DataConnector DbConnector
        {
            get
            {
                return this.m_dbConnector;
            }
        }

        internal TemplateSet DefaultTemplates
        {
            get
            {
                if (this.m_tsDefault == null)
                {
                    TextCell cell = new TextCell();
                    ItemTemplate template = new ItemTemplate();
                    cell.Bounds = new Rectangle(0, 0, -1, 0x10);
                    cell.CellSource.DisplayMember = true;
                    cell.ForeColor = Color.Black;
                    cell.BackColor = Color.Transparent;
                    template.Height = 0x10;
                    template.CellTemplates.Add(cell);
                    TextCell cell2 = new TextCell();
                    ItemTemplate template2 = new ItemTemplate();
                    cell2.Bounds = new Rectangle(0, 0, -1, 0x10);
                    cell2.CellSource.DisplayMember = true;
                    cell2.ForeColor = Color.White;
                    cell2.BackColor = Color.Navy;
                    template2.Height = 0x10;
                    template2.CellTemplates.Add(cell2);
                    this.m_tsDefault = new TemplateSet();
                    this.m_tsDefault.Add(template);
                    this.m_tsDefault.Add(template2);
                    this.m_tsDefault.Parent = this;
                    this.m_tsDefault.Changed += new ComboBoxEventHandler(this.OnChange);
                }
                return this.m_tsDefault;
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
                    if (!this.m_bDelayLoad && this.DbConnector.IsOpen)
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

        [DefaultValue("")]
        public string DisplayMember
        {
            get
            {
                return this.m_displayMember;
            }
            set
            {
                if (this.m_displayMember != value)
                {
                    this.m_displayMember = value;
                    this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true));
                    this.OnDisplayMemberChanged(this, EventArgs.Empty);
                }
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
                    this.UpdateDoubleBuffering();
                    this.m_list.DoubleBuffered = value;
                    base.Invalidate();
                }
            }
        }

        public bool DroppedDown
        {
            get
            {
                return this.m_list.Visible;
            }
            set
            {
                if (value && !this.DroppedDown)
                {
                    this.Popup();
                }
                else if (!value && this.DroppedDown)
                {
                    this.Close();
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

        public int ExpectedItems
        {
            get
            {
                return this.m_iExpectedItems;
            }
            set
            {
                if (this.m_iExpectedItems != value)
                {
                    this.m_iExpectedItems = value;
                    this.OnChange(this, ComboBoxEventArgsType.Refresh, ComboBoxArgs.Default);
                }
            }
        }

        public override bool Focused
        {
            get
            {
                if (!base.Focused)
                {
                    return this.m_list.Focused;
                }
                return true;
            }
        }

        [DefaultValue(false)]
        public bool FullScreenList
        {
            get
            {
                return this.m_bFullScreenList;
            }
            set
            {
                if (this.m_bFullScreenList != value)
                {
                    this.m_bFullScreenList = value;
                    if (this.m_bFullScreenList)
                    {
                        this.m_list.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
                    }
                    else
                    {
                        this.m_list.Anchor = AnchorStyles.None;
                    }
                    this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxArgs(ComboBoxUpdateRange.List));
                }
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

        [Resco.Controls.AdvancedComboBox.Design.Browsable(false), Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden)]
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

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedComboBox.Design.Browsable(false)]
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

        public ItemCollection Items
        {
            get
            {
                return this.m_items;
            }
        }

        [DefaultValue(true)]
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

        internal AdvancedList List
        {
            get
            {
                return this.m_list;
            }
        }

        [DefaultValue("DarkGray")]
        public Color ListGridColor
        {
            get
            {
                return this.m_list.GridColor;
            }
            set
            {
                this.m_list.GridColor = value;
            }
        }

        [DefaultValue(false)]
        public bool ListGridLines
        {
            get
            {
                return this.m_list.GridLines;
            }
            set
            {
                this.m_list.GridLines = value;
            }
        }

        [DefaultValue(-1)]
        public int ListHeight
        {
            get
            {
                return this.m_listHeight;
            }
            set
            {
                if (this.m_listHeight != value)
                {
                    this.m_listHeight = value;
                    this.SetListBounds(false);
                }
            }
        }

        [DefaultValue(-1)]
        public int MaxListHeight
        {
            get
            {
                return this.m_maxListHeight;
            }
            set
            {
                if (this.m_maxListHeight != value)
                {
                    this.m_maxListHeight = value;
                    this.SetListBounds(false);
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
                    this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true));
                }
            }
        }

        [DefaultValue(false)]
        public bool ScrollbarVisible
        {
            get
            {
                return this.m_list.ScrollbarVisible;
            }
        }

        [DefaultValue(13)]
        public int ScrollbarWidth
        {
            get
            {
                return this.m_list.ScrollbarWidth;
            }
            set
            {
                this.m_list.ScrollbarWidth = value;
            }
        }

        public Cell SelectedCell
        {
            get
            {
                if (this.DroppedDown)
                {
                    return this.m_list.SelectedCell;
                }
                if (this.m_textBoxItem != null)
                {
                    ItemTemplate.CellCollection cellTemplates = this.TextBoxTemplate.CellTemplates;
                    if ((this.m_iSelectedCellIndex >= 0) && (this.m_iSelectedCellIndex < cellTemplates.Count))
                    {
                        return cellTemplates[this.m_iSelectedCellIndex];
                    }
                }
                return null;
            }
            set
            {
                if (this.DroppedDown)
                {
                    this.m_list.SelectedCell = value;
                }
                else if (value == null)
                {
                    if (this.m_iSelectedCellIndex != -1)
                    {
                        this.m_iSelectedCellIndex = -1;
                        this.OnChange(this, ComboBoxEventArgsType.Repaint, new ComboBoxArgs(ComboBoxUpdateRange.Box));
                    }
                }
                else
                {
                    if (this.m_textBoxItem == null)
                    {
                        throw new ArgumentException("The cell is not contained in item's ItemTemplate's CellCollection.", "SelectedCell");
                    }
                    ItemTemplate.CellCollection cellTemplates = this.TextBoxTemplate.CellTemplates;
                    if ((this.m_iSelectedCellIndex == -1) || (cellTemplates[this.m_iSelectedCellIndex] != value))
                    {
                        if (!cellTemplates.Contains(value))
                        {
                            throw new ArgumentException("The cell is not contained in item's ItemTemplate's CellCollection.", "SelectedCell");
                        }
                        if (!value.Selectable)
                        {
                            throw new ArgumentException("The cell is not selectable!", "SelectedCell");
                        }
                        this.m_iSelectedCellIndex = cellTemplates.IndexOf(value);
                        this.OnChange(this, ComboBoxEventArgsType.Repaint, new ComboBoxArgs(ComboBoxUpdateRange.Box));
                    }
                }
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.m_selectedItemIndex;
            }
            set
            {
                if (this.m_selectedItemIndex != value)
                {
                    if (this.DelayLoad)
                    {
                        while (value >= this.Items.Count)
                        {
                            if (!this.DoDelayLoad())
                            {
                                break;
                            }
                        }
                    }
                    if ((value < -1) || (value >= this.m_items.Count))
                    {
                        throw new ArgumentOutOfRangeException("SelectedIndex");
                    }
                    this.m_list.SelectedItemIndex = value;
                    if (value < 0)
                    {
                        this.OnItemSelect(this, new ItemEventArgs(null, -1, 0));
                    }
                }
            }
        }

        public object SelectedItem
        {
            get
            {
                if ((this.m_selectedItemIndex < 0) || (this.m_selectedItemIndex >= this.m_items.Count))
                {
                    return null;
                }
                BoundItem item = this.m_items[this.m_selectedItemIndex] as BoundItem;
                if (item != null)
                {
                    return item.Data;
                }
                return this.m_items[this.m_selectedItemIndex];
            }
            set
            {
                if (value == null)
                {
                    this.SelectedIndex = -1;
                }
                else
                {
                    for (int i = 0; i < this.m_items.Count; i++)
                    {
                        if ((this.m_items[i] == value) || ((this.m_items[i] is BoundItem) && (((BoundItem) this.m_items[i]).Data == value)))
                        {
                            this.SelectedIndex = i;
                            return;
                        }
                    }
                }
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

        public object SelectedValue
        {
            get
            {
                if (this.m_selectedItemIndex < 0)
                {
                    return null;
                }
                if ((this.m_valueMember != null) && !(this.m_valueMember == string.Empty))
                {
                    return this.m_items[this.m_selectedItemIndex][this.m_valueMember];
                }
                return this.SelectedItem.ToString();
            }
        }

        [DefaultValue(true)]
        public bool ShowScrollbar
        {
            get
            {
                return this.m_list.ShowScrollbar;
            }
            set
            {
                this.m_list.ShowScrollbar = value;
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

        [Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedComboBox.Design.DesignerSerializationVisibility.Content)]
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
                    this.m_tsCurrent.Changed -= new ComboBoxEventHandler(this.OnChange);
                    this.m_tsCurrent.Parent = null;
                    this.m_tsCurrent = value;
                    this.m_tsCurrent.Parent = this;
                    this.m_tsCurrent.Changed += new ComboBoxEventHandler(this.OnChange);
                    this.OnChange(this, ComboBoxEventArgsType.Refresh, new ComboBoxRefreshArgs(true));
                }
            }
        }

        private ItemTemplate TextBoxTemplate
        {
            get
            {
                int textBoxTemplateIndex = this.m_textBoxTemplateIndex;
                if (this.m_textBoxItem.TextBoxTemplateIndex >= 0)
                {
                    textBoxTemplateIndex = this.m_textBoxItem.TextBoxTemplateIndex;
                }
                if ((textBoxTemplateIndex >= 0) && (textBoxTemplateIndex < this.m_tsCurrent.Count))
                {
                    return this.m_tsCurrent[textBoxTemplateIndex];
                }
                return this.DefaultTemplates[1];
            }
        }

        [DefaultValue(0)]
        public int TextBoxTemplateIndex
        {
            get
            {
                return this.m_textBoxTemplateIndex;
            }
            set
            {
                this.m_textBoxTemplateIndex = value;
            }
        }

        public Resco.Controls.AdvancedComboBox.TouchScrollDirection TouchScrollDirection
        {
            get
            {
                return this.m_list.TouchScrollDirection;
            }
            set
            {
                this.m_list.TouchScrollDirection = value;
            }
        }

        [DefaultValue(false)]
        public bool TouchScrolling
        {
            get
            {
                return this.m_list.TouchScrolling;
            }
            set
            {
                this.m_list.TouchScrolling = value;
            }
        }

        [DefaultValue(8)]
        public int TouchSensitivity
        {
            get
            {
                return this.m_list.TouchSensitivity;
            }
            set
            {
                this.m_list.TouchSensitivity = value;
            }
        }

        [DefaultValue("")]
        public string ValueMember
        {
            get
            {
                return this.m_valueMember;
            }
            set
            {
                if (this.m_valueMember != value)
                {
                    this.m_valueMember = value;
                    this.OnValueMemberChanged(this, EventArgs.Empty);
                    this.OnSelectedValueChanged(this, EventArgs.Empty);
                }
            }
        }

        public delegate void DesignTimeCallback(object o, object o2);

        internal delegate void OnChangeDelegate(object sender, ComboBoxEventArgsType e, ComboBoxArgs args);

        private delegate void OnItemRemovedDelegate(Resco.Controls.AdvancedComboBox.ListItem item, int index);

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

