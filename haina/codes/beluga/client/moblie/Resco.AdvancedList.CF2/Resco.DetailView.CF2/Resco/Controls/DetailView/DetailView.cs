namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.Design;
    using Resco.Controls.DetailView.DetailViewInternal;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;

    public class DetailView : UserControl
    {
        private DesignTimeCallback _designTimeCallback;
        private static float _DPIx = 96f;
        private static float _DPIy = 96f;
        private int _pagerHeight = -1;
        public static Size ArrowImageSize = new Size(15, 15);
        public static int ComboButtonWidth = 0x16;
        internal static Rectangle ComboRectangle = new Rectangle(ComboSize / 4, ComboSize * 2, (2 * ComboSize) - 1, ComboSize);
        internal static int ComboSize = 4;
        private IContainer components;
        public static int DefaultLabelWidth = 50;
        public static int DefaultScrollBarWidth = 13;
        public static int DefaultTabArrowsHeight = 0x10;
        public static int DefaultTabStripHeight = 0x18;
        public static int ErrorSpacer = 8;
        public static int HorizontalSpacer = 8;
        private bool InUpdate;
        internal Item InUpdateItem;
        internal static bool IsEventRunning = false;
        private RescoArrowStyle m_ArrowStyle;
        private bool m_AutoRefresh;
        private SolidBrush m_BackColor;
        private bool m_bEnableTouchScrolling;
        private BindingManagerBase m_bmb;
        internal bool m_bScrollVisible;
        private bool m_bTouchScrolling;
        private System.Windows.Forms.ContextMenu m_ContextMenu;
        private Hashtable m_Controls = new Hashtable(10);
        private DVConversion m_Conversion;
        private object m_dataSource;
        private Color m_disabledForeColor;
        private bool m_doubleBuffered = true;
        private SizeF m_dpiFactor = new SizeF(1f, 1f);
        private bool m_enableDesignTimeCustomItems;
        private GradientColor m_gradientBackColor;
        private int m_iActualMaximumValue;
        private int m_iCurrentPage;
        private int m_iNavigationIndex;
        private int m_iPages;
        private int m_iPrevValue;
        private int m_iSuspendRedraw;
        private DetailViewItems m_ItemCollection = new DetailViewItems();
        private List<_ItemBoundsHelper> m_itemList;
        private bool m_KeyNavigation;
        private bool m_keyUpEvent;
        private int m_LabelWidth;
        private Point m_LastMousePosition;
        internal bool m_LeftArrowClicked;
        private Item m_nextNavItem;
        private PageCollection m_pages;
        private RescoPagesLocation m_pagesLocation;
        internal bool m_PagesOverWidth;
        private bool m_pagesRightToLeft;
        private RescoPageStyle m_PagesStyle;
        internal bool m_RightArrowClicked;
        private SizeF m_scaleFactor = new SizeF(1f, 1f);
        internal Item m_SelectedItem;
        private int m_SeparatorWidth;
        private static Pen m_sPen;
        private static Bitmap m_sPixel;
        private bool m_splitArrows;
        internal int m_StartDrawPage;
        private Resco.Controls.DetailView.ToolTip m_ToolTip;
        private int m_TouchAutoScrollDiffX;
        private int m_TouchAutoScrollDiffY;
        private bool m_touchPage;
        private Resco.Controls.DetailView.TouchScrollDirection m_touchPagesDirection;
        private Resco.Controls.DetailView.TouchScrollDirection m_touchScrollDirection;
        private Timer m_TouchScrollingTimer;
        private int m_touchSensitivity;
        private uint m_TouchTime0;
        private uint m_TouchTime1;
        private bool m_useClickVisualize;
        private bool m_useGradient;
        private bool m_useNextNavItem;
        private ScrollbarWrapper m_vScroll = new ScrollbarWrapper(new System.Windows.Forms.VScrollBar(), ScrollOrientation.VerticalScroll);
        private Control m_vScrollBarResco = null;
        internal int m_VScrollBarWidth = 13;
        public static bool ShowTextTooLong = true;
        private static string[] sImageNames = new string[] { "e_up1.gif", "e_down1.gif", "e_left1.gif", "e_right1.gif", "d_up1.gif", "d_down1.gif", "d_left1.gif", "d_right1.gif", "tu_up.gif", "tu_down.gif", "tu_left.gif", "tu_right.gif", "tc_up.gif", "tc_down.gif", "tc_left.gif", "tc_right.gif" };
        public static Image[] sImages;
        public static Size TabImageSize = new Size(12, 12);
        public static int TooltipWidth = 8;
        public static int UpDownButtonWidth = 0x1a;

        public event ItemEventHandler ItemChanged;

        public event ItemEventHandler ItemClick;

        public event ItemEventHandler ItemGotFocus;

        public event ItemEventHandler ItemLabelClick;

        public event ItemEventHandler ItemLostFocus;

        public event ItemValidatingEventHandler ItemValidating;

        public event EventHandler PageChanged;

        public event PageChangingEventHandler PageChanging;

        public event EventHandler Scroll;

        public event TouchGestureEventHandler TouchGesture;

        static DetailView()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(Resco.Controls.DetailView.DetailView), "");
            //}
            sImages = new Image[0x13];
        }

        public DetailView()
        {
            this.m_ItemCollection.Parent = this;
            this.AutoRefresh = true;
            base.Tag = null;
            Graphics graphics = null;
            try
            {
                graphics = base.CreateGraphics();
                this.m_dpiFactor = new SizeF(graphics.DpiX / 96f, graphics.DpiY / 96f);
                ScaleSettings(graphics.DpiX, graphics.DpiY);
            }
            catch
            {
            }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                }
                graphics = null;
            }
            this.m_LastMousePosition = new Point(0, 0);
            this.m_iCurrentPage = 0;
            this.m_PagesStyle = RescoPageStyle.Arrows;
            this.m_ArrowStyle = RescoArrowStyle.LeftRight;
            this.m_pagesRightToLeft = false;
            this.m_splitArrows = false;
            this.m_pagesLocation = RescoPagesLocation.Bottom;
            this.m_StartDrawPage = 0;
            this.m_PagesOverWidth = false;
            base.BackColor = Color.White;
            this.m_disabledForeColor = SystemColors.GrayText;
            this.m_LabelWidth = DefaultLabelWidth;
            this.m_SeparatorWidth = HorizontalSpacer;
            this.m_BackColor = new SolidBrush(this.BackColor);
            this.m_gradientBackColor = new GradientColor();
            this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
            this.m_useGradient = false;
            BackBufferManager.AddRef();
            Rectangle rectangle = this.CalculateClientRect();
            this.m_VScrollBarWidth = DefaultScrollBarWidth;
            this.m_vScroll.Bounds = new Rectangle(rectangle.Width - this.m_VScrollBarWidth, 0, this.m_VScrollBarWidth, rectangle.Height);
            this.m_vScroll.Minimum = 0;
            this.m_vScroll.Maximum = 0;
            this.m_iActualMaximumValue = 0;
            this.m_vScroll.SmallChange = 20;
            this.m_vScroll.LargeChange = base.Height;
            this.m_vScroll.Value = 0;
            this.m_vScroll.Visible = false;
            this.m_iPrevValue = 0;
            this.m_vScroll.Attach(this);
            this.m_vScroll.ValueChanged += new EventHandler(this.OnValueChanged);
            this.m_vScroll.Resize += new EventHandler(this.OnScrollResize);
            this.SelectedItem = null;
            this.m_ToolTip = null;
            this.m_LeftArrowClicked = false;
            this.m_RightArrowClicked = false;
            this.m_ItemCollection.Changed += new DetailViewEventHandler(this.OnItemsChanged);
            this.GetControl(typeof(DVComboBox));
            this.m_TouchScrollingTimer = new Timer();
            this.m_TouchScrollingTimer.Enabled = false;
            this.m_TouchScrollingTimer.Interval = 50;
            this.m_TouchScrollingTimer.Tick += new EventHandler(this.OnTouchScrollingTimerTick);
            this.m_bTouchScrolling = false;
            this.m_bEnableTouchScrolling = false;
            this.m_TouchAutoScrollDiffX = 0;
            this.m_TouchAutoScrollDiffY = 0;
            this.m_touchSensitivity = 0x10;
            this.m_touchPage = false;
            this.m_touchScrollDirection = Resco.Controls.DetailView.TouchScrollDirection.Inverse;
            this.m_touchPagesDirection = Resco.Controls.DetailView.TouchScrollDirection.Inverse;
            this.InitializeComponent();
        }

        private void c_KeyDown(object sender, KeyEventArgs e)
        {
            this.OnKeyDown(e);
        }

        private void c_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        private void c_KeyUp(object sender, KeyEventArgs e)
        {
            this.OnKeyUp(e);
        }

        private void c_LostFocus(object sender, EventArgs e)
        {
            if (!this.Focused)
            {
                this.SelectedItem = null;
                this.OnLostFocus(e);
            }
        }

        protected internal virtual Rectangle CalculateClientRect()
        {
            return base.ClientRectangle;
        }

        public void ClearContents()
        {
            this.ClearContents(false);
        }

        public void ClearContents(bool dataBind)
        {
            this.SelectedItem = null;
            foreach (Item item in this.m_ItemCollection)
            {
                if (((item.GetType() == typeof(ItemPageBreak)) && (item.DataMember == "")) || ((dataBind || (item.GetType() == typeof(Item))) && (item.DataMember == "")))
                {
                    continue;
                }
                item.ClearContents();
            }
        }

        public void ClearErrorMessages()
        {
            bool autoRefresh = this.AutoRefresh;
            this.AutoRefresh = false;
            foreach (Item item in this.Items)
            {
                item.ErrorMessage = null;
            }
            this.AutoRefresh = autoRefresh;
            base.Invalidate();
        }

        private static object ConvertWithNullable(object value, Type T)
        {
            if (!IsNullable(T))
            {
                return value;
            }
            if (value == null)
            {
                return null;
            }
            Type underlyingType = Nullable.GetUnderlyingType(T);
            if (value.GetType() == underlyingType)
            {
                return value;
            }
            try
            {
                return Convert.ChangeType(value, underlyingType, CultureInfo.CurrentCulture);
            }
            catch
            {
                return Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.SuspendRedraw();
            if (this.m_TouchScrollingTimer != null)
            {
                this.m_TouchScrollingTimer.Dispose();
            }
            this.m_TouchScrollingTimer = null;
            BackBufferManager.Release();
            this.m_gradientBackColor = null;
            this.m_pages = null;
            if (this.m_ItemCollection != null)
            {
                this.m_ItemCollection.Clear();
            }
            this.m_ItemCollection = null;
            if (this.m_itemList != null)
            {
                this.m_itemList.Clear();
            }
            this.m_itemList = null;
            if (disposing)
            {
                foreach (Control control in this.m_Controls.Values)
                {
                    control.Dispose();
                }
                this.m_Controls.Clear();
                if (this.m_vScroll != null)
                {
                    this.m_vScroll.Detach();
                }
                this.m_vScroll = null;
            }
            if (m_sPen != null)
            {
                m_sPen.Dispose();
            }
            m_sPen = null;
            if (m_sPixel != null)
            {
                m_sPixel.Dispose();
            }
            m_sPixel = null;
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        internal static void DrawPixel(Graphics gr, Color c, int x, int y)
        {
            if (m_sPixel == null)
            {
                m_sPixel = new Bitmap(1, 1);
            }
            m_sPixel.SetPixel(0, 0, c);
            gr.DrawImage(m_sPixel, x, y);
        }

        public int EnsureVisible(Item item, bool bTop)
        {
            if (item != null)
            {
                if (!(item is ItemPageBreak))
                {
                    int index = this.m_ItemCollection.IndexOf(item);
                    if ((this.CurrentPage.FirstIndex > index) || ((index - this.CurrentPage.FirstIndex) >= this.CurrentPage.Count))
                    {
                        for (int j = this.PageCount - 1; j >= 0; j--)
                        {
                            if (this.Pages[j].FirstIndex <= index)
                            {
                                this.Pages[j].Show();
                                if (this.Pages[j] != this.CurrentPage)
                                {
                                    return -1;
                                }
                                break;
                            }
                        }
                    }
                    int num3 = 0;
                    for (int i = this.CurrentPage.FirstIndex; i < index; i++)
                    {
                        num3 += this.m_ItemCollection[i].ItemHeight;
                    }
                    if (num3 < this.m_vScroll.Value)
                    {
                        this.m_vScroll.Value = num3;
                    }
                    else if ((num3 + item.ItemHeight) > (base.Height - this.PagerHeight))
                    {
                        if (bTop)
                        {
                            this.m_vScroll.Value = Math.Min(num3, this.m_vScroll.Maximum - this.m_vScroll.LargeChange);
                        }
                        else
                        {
                            this.m_vScroll.Value = Math.Min((int) ((num3 + item.ItemHeight) - this.m_vScroll.LargeChange), (int) (this.m_vScroll.Maximum - this.m_vScroll.LargeChange));
                        }
                    }
                    return (num3 - this.m_vScroll.Value);
                }
                foreach (Page page in this.Pages)
                {
                    if (page.PagingItem == item)
                    {
                        page.Show();
                        break;
                    }
                }
            }
            return -1;
        }

        internal static Image GetArrow(bool isRight, bool isLeftRight, bool isDC, bool isTab)
        {
            int index = 0;
            if (isRight)
            {
                index |= 1;
            }
            if (isLeftRight)
            {
                index |= 2;
            }
            if (isDC)
            {
                index |= 4;
            }
            if (isTab)
            {
                index |= 8;
            }
            Image image = sImages[index];
            if (image == null)
            {
                image = LoadBitmap("Resco.Controls.DetailView.Bitmaps." + sImageNames[index]);
                sImages[index] = image;
            }
            return image;
        }

        internal static object GetBoundItem(BindingManagerBase bmb, object item, string strField)
        {
            if (((item != null) && (strField != null)) && (strField.Length > 0))
            {
                try
                {
                    PropertyDescriptor descriptor;
                    if (bmb != null)
                    {
                        descriptor = bmb.GetItemProperties().Find(strField, true);
                    }
                    else
                    {
                        descriptor = TypeDescriptor.GetProperties(item).Find(strField, true);
                    }
                    if (descriptor != null)
                    {
                        item = descriptor.GetValue(item);
                    }
                }
                catch (Exception)
                {
                }
            }
            return item;
        }

        internal Type GetBoundItemType(string member)
        {
            if (((member != null) && (member.Length != 0)) && (((this.m_bmb != null) && (this.m_bmb.Position >= 0)) && (this.m_bmb.Current != null)))
            {
                try
                {
                    PropertyDescriptor descriptor = this.m_bmb.GetItemProperties().Find(member, true);
                    if (descriptor != null)
                    {
                        return descriptor.PropertyType;
                    }
                }
                catch (Exception)
                {
                }
            }
            return typeof(object);
        }

        public Control GetControl(Type controlType)
        {
            Control control = this.m_Controls[controlType] as Control;
            if (control != null)
            {
                return control;
            }
            try
            {
                ConstructorInfo info = controlType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);
                if (info != null)
                {
                    control = (Control) info.Invoke(new object[0]);
                }
                if (control == null)
                {
                    return null;
                }
                control.Visible = false;
                control.KeyDown += new KeyEventHandler(this.c_KeyDown);
                control.KeyPress += new KeyPressEventHandler(this.c_KeyPress);
                control.KeyUp += new KeyEventHandler(this.c_KeyUp);
                control.LostFocus += new EventHandler(this.c_LostFocus);
                base.Controls.Add(control);
                this.m_Controls.Add(controlType, control);
                control.Scale(this.m_scaleFactor);
                return control;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ItemEventArgs GetItemAtPoint(Point pt)
        {
            int num = base.Height - this.PagerHeight;
            if (this.HasPages && (((this.PagesLocation == RescoPagesLocation.Bottom) && (pt.Y > num)) || ((this.PagesLocation == RescoPagesLocation.Top) && (pt.Y < this.PagerHeight))))
            {
                return new ItemEventArgs(this.CurrentPage.PagingItem, this.CurrentPage.PagingItem.Index, this.CurrentPage.PagingItem.Name);
            }
            if (this.HasPages && (this.PagesLocation == RescoPagesLocation.Top))
            {
                pt.Y -= this.PagerHeight;
            }
            int num2 = -this.m_vScroll.Value;
            int num3 = 0;
            for (int i = 0; i < this.CurrentPage.Count; i++)
            {
                Item item = this.CurrentPage[i];
                if (item.Visible)
                {
                    if ((i != 0) && item.NewLine)
                    {
                        num2 += num3;
                        num3 = 0;
                    }
                    num3 = Math.Max(num3, item.ItemHeight);
                    if (num2 > num)
                    {
                        break;
                    }
                    Point itemXWidth = this.GetItemXWidth(item);
                    if (((num2 < pt.Y) && ((num2 + item.ItemHeight) >= pt.Y)) && ((pt.X >= itemXWidth.X) && (pt.X < (itemXWidth.X + itemXWidth.Y))))
                    {
                        return new ItemEventArgs(item, item.Index, item.Name);
                    }
                }
            }
            return null;
        }

        internal Point GetItemXWidth(Item item)
        {
            if (this.m_itemList == null)
            {
                this.m_itemList = new List<_ItemBoundsHelper>();
            }
            if (this.m_itemList != null)
            {
                this.m_itemList.Clear();
            }
            int index = this.Items.IndexOf(item);
            int x = 0;
            int y = 0;
            int num5 = 0;
            int num6 = 0;
            Item item2 = item;
            int i = index;
            while ((i > 0) && !item2.NewLine)
            {
                if (this.Items[i - 1] is ItemPageBreak)
                {
                    break;
                }
                i--;
                item2 = this.Items[i];
            }
            int itemWidth = item2.ItemWidth;
            this.m_itemList.Add(new _ItemBoundsHelper(i, itemWidth));
            if (itemWidth < 0)
            {
                num6++;
            }
            else
            {
                num5 += itemWidth;
            }
            i++;
            while ((i < this.Items.Count) && !(item2 = this.Items[i]).NewLine)
            {
                itemWidth = item2.ItemWidth;
                this.m_itemList.Add(new _ItemBoundsHelper(i, itemWidth));
                if (itemWidth < 0)
                {
                    num6++;
                }
                else
                {
                    num5 += itemWidth;
                }
                i++;
            }
            int num8 = (num6 == 0) ? 0 : ((this.UsableWidth - num5) / num6);
            foreach (_ItemBoundsHelper helper in this.m_itemList)
            {
                y = (helper.width < 0) ? num8 : helper.width;
                if (helper.index == index)
                {
                    break;
                }
                x += y;
            }
            if ((x + y) > this.UsableWidth)
            {
                y = this.UsableWidth - x;
            }
            return new Point(x, y);
        }

        internal static Pen GetPen(Color color)
        {
            if (m_sPen == null)
            {
                return (m_sPen = new Pen(color));
            }
            m_sPen.Color = color;
            return m_sPen;
        }

        public void HideActiveControl()
        {
            this.SelectedItem = null;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            base.AutoScaleMode = AutoScaleMode.Inherit;//.set_AutoScaleMode(3);
        }

        public void InvokeMouseDown(MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }

        private static bool IsNullable(Type t)
        {
            if (!t.IsGenericType)
            {
                return false;
            }
            return t.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        internal void ItemResized(int dif)
        {
            this.ItemResized(true, dif);
        }

        internal void ItemResized(bool invaldiate, int dif)
        {
            if (this.m_iSuspendRedraw <= 0)
            {
                this.m_iActualMaximumValue = this.m_ItemCollection.CalculateItemsHeight();
                this.SetVScrollBar(this.m_iActualMaximumValue);
                if (invaldiate)
                {
                    base.Invalidate();
                }
            }
        }

        private static Bitmap LoadBitmap(string name)
        {
            Stream manifestResourceStream = null;
            manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            if (manifestResourceStream == null)
            {
                throw new NullReferenceException("Unable to load resource: " + name);
            }
            return new Bitmap(manifestResourceStream);
        }

        public void LoadXml(string location)
        {
            XmlTextReader reader = null;
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
            }
        }

        public void LoadXml(XmlReader reader)
        {
            try
            {
                this.m_Conversion = new DVConversion();
                DVConversion.DesignTimeCallback = this._designTimeCallback;
                base.SuspendLayout();
                while (reader.Read())
                {
                    string str2;
                    if (((str2 = reader.Name) != null) && (str2 == "DetailView"))
                    {
                        this.ReadDetailView(reader);
                    }
                }
            }
            finally
            {
                base.ResumeLayout();
                this.m_Conversion = null;
                DVConversion.DesignTimeCallback = null;
            }
        }

        public void LoadXml(string location, DesignTimeCallback dtc)
        {
            XmlTextReader reader = null;
            this._designTimeCallback = dtc;
            if (this._designTimeCallback != null)
            {
                this._designTimeCallback(4, this);
            }
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
            base.Invalidate();
        }

        public void NextPage()
        {
            Page page;
            int pageIndex = this.CurrentPage.PageIndex;
            do
            {
                page = null;
                pageIndex += this.PagesRightToLeft ? -1 : 1;
                if ((pageIndex < 0) || (pageIndex >= this.Pages.Count))
                {
                    break;
                }
                page = this.Pages[pageIndex];
            }
            while (((page != null) && (page.PagingItem != null)) && !page.PagingItem.Visible);
            if (page != null)
            {
                this.HideActiveControl();
                if (this.SelectedItem == null)
                {
                    this.CurrentPage = page;
                }
            }
        }

        private void OnBindingChanged(object sender, EventArgs e)
        {
            this.UpdateControl();
        }

        protected override void OnBindingContextChanged(EventArgs e)
        {
            if ((this.BindingContext != null) && (this.DataSource != null))
            {
                this.m_bmb = this.BindingContext[this.DataSource];
                this.m_bmb.CurrentChanged += new EventHandler(this.OnBindingChanged);
            }
            base.OnBindingContextChanged(e);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (!this.m_bTouchScrolling && ((this.m_ToolTip == null) || !this.m_ToolTip.Visible))
            {
                Rectangle rectangle = this.CalculateClientRect();
                int num = -this.m_vScroll.Value;
                int usableWidth = this.UsableWidth;
                int num3 = rectangle.Height - this.PagerHeight;
                int x = this.LastMousePosition.X;
                int y = this.LastMousePosition.Y;
                if ((this.PagesLocation == RescoPagesLocation.Top) && ((y - rectangle.Y) < this.PagerHeight))
                {
                    this.SelectedItem = null;
                    if ((y - rectangle.Y) >= 0)
                    {
                        this.CurrentPage.PagingItem._Click(x, usableWidth, this.UseClickVisualize);
                    }
                }
                else if ((this.PagesLocation == RescoPagesLocation.Bottom) && ((y - rectangle.Y) > num3))
                {
                    this.SelectedItem = null;
                    if ((y - rectangle.Y) < rectangle.Height)
                    {
                        this.CurrentPage.PagingItem._Click(x, usableWidth, this.UseClickVisualize);
                    }
                }
                else
                {
                    int yOffset = num + rectangle.Y;
                    yOffset += this.HasPages ? ((this.PagesLocation == RescoPagesLocation.Bottom) ? 0 : this.PagerHeight) : 0;
                    bool flag = false;
                    int num7 = 0;
                    for (int i = 0; i < this.CurrentPage.Count; i++)
                    {
                        Item item = this.CurrentPage[i];
                        if (!item.Visible)
                        {
                            continue;
                        }
                        int itemHeight = item.ItemHeight;
                        if ((i == 0) || item.NewLine)
                        {
                            num7 = itemHeight;
                        }
                        Point itemXWidth = this.GetItemXWidth(item);
                        int num10 = (itemXWidth.Y < item.InternalLabelWidth) ? itemXWidth.Y : item.InternalLabelWidth;
                        if (item.Style == RescoItemStyle.LabelTop)
                        {
                            if ((((yOffset + item.LabelHeight) < y) && (((yOffset + item.Height) + item.LabelHeight) > y)) && ((itemXWidth.X < (x - rectangle.X)) && ((itemXWidth.X + itemXWidth.Y) > (x - rectangle.X))))
                            {
                                flag = true;
                                goto Label_03CF;
                            }
                            if (((yOffset >= y) || ((yOffset + item.LabelHeight) <= y)) || ((itemXWidth.X >= (x - rectangle.X)) || ((itemXWidth.X + itemXWidth.Y) <= (x - rectangle.X))))
                            {
                                goto Label_03CF;
                            }
                            item._LabelClick(yOffset, usableWidth, this.UseClickVisualize);
                            break;
                        }
                        if (item.Style == RescoItemStyle.LabelRight)
                        {
                            if (((yOffset < y) && ((yOffset + item.ItemHeight) > y)) && ((itemXWidth.X < (x - rectangle.X)) && (((itemXWidth.X + itemXWidth.Y) - num10) > (x - rectangle.X))))
                            {
                                flag = true;
                                goto Label_03CF;
                            }
                            if (((yOffset >= y) || ((yOffset + item.ItemHeight) <= y)) || (((itemXWidth.X + itemXWidth.Y) <= (x - rectangle.X)) || (((itemXWidth.X + itemXWidth.Y) - num10) >= (x - rectangle.X))))
                            {
                                goto Label_03CF;
                            }
                            item._LabelClick(yOffset, usableWidth, this.UseClickVisualize);
                            break;
                        }
                        if (((yOffset < y) && ((yOffset + item.ItemHeight) > y)) && (((itemXWidth.X + num10) < (x - rectangle.X)) && ((itemXWidth.X + itemXWidth.Y) > (x - rectangle.X))))
                        {
                            flag = true;
                        }
                        else if (((yOffset < y) && ((yOffset + item.ItemHeight) > y)) && ((itemXWidth.X < (x - rectangle.X)) && ((itemXWidth.X + num10) > (x - rectangle.X))))
                        {
                            item._LabelClick(yOffset, usableWidth, this.UseClickVisualize);
                            break;
                        }
                    Label_03CF:
                        if (flag)
                        {
                            if (!item.CheckForToolTip(x, y, itemXWidth.X, itemXWidth.Y, false))
                            {
                                int num11 = -1;
                                int num12 = -1;
                                if (this.m_SelectedItem != item)
                                {
                                    num11 = this.m_vScroll.Value;
                                    this.SelectedItem = null;
                                    num12 = this.m_vScroll.Value;
                                    this.m_SelectedItem = item;
                                }
                                item._Click(yOffset, usableWidth, this.UseClickVisualize);
                                if ((this.SelectedItem != null) && (num11 > -1))
                                {
                                    this.SelectedItem._MoveTop(num11 - num12);
                                }
                            }
                            break;
                        }
                        if (itemHeight > num7)
                        {
                            num7 = itemHeight;
                        }
                        if ((i == (this.CurrentPage.Count - 1)) || this.CurrentPage[i + 1].NewLine)
                        {
                            yOffset += num7;
                        }
                        if (yOffset > y)
                        {
                            break;
                        }
                    }
                    if (!flag)
                    {
                        this.SelectedItem = null;
                    }
                }
            }
        }

        protected virtual void OnItemChanged(object sender, ItemEventArgs e)
        {
            IsEventRunning = true;
            e.Index = this.Items.IndexOf(e.item);
            if (this.ItemChanged != null)
            {
                this.ItemChanged(this, e);
            }
            IsEventRunning = false;
        }

        protected virtual void OnItemClick(object sender, ItemEventArgs e)
        {
            IsEventRunning = true;
            e.Index = this.Items.IndexOf(e.item);
            if (this.ItemClick != null)
            {
                this.ItemClick(this, e);
            }
            IsEventRunning = false;
        }

        protected virtual void OnItemGotFocus(object sender, ItemEventArgs e)
        {
            IsEventRunning = true;
            e.Index = this.Items.IndexOf(e.item);
            if (this.ItemGotFocus != null)
            {
                this.ItemGotFocus(this, e);
            }
            IsEventRunning = false;
        }

        protected virtual void OnItemLabelClick(object sender, ItemEventArgs e)
        {
            IsEventRunning = true;
            e.Index = this.Items.IndexOf(e.item);
            if (this.ItemLabelClick != null)
            {
                this.ItemLabelClick(this, e);
            }
            IsEventRunning = false;
        }

        protected virtual void OnItemLostFocus(object sender, ItemEventArgs e)
        {
            IsEventRunning = true;
            e.Index = this.Items.IndexOf(e.item);
            if (this.ItemLostFocus != null)
            {
                this.ItemLostFocus(this, e);
            }
            IsEventRunning = false;
        }

        private void OnItemsChanged(object sender, DetailViewEventArgsType e, object oParam)
        {
            switch (e)
            {
                case DetailViewEventArgsType.ItemAdd:
                    this.m_pages = null;
                    if (sender is ItemPageBreak)
                    {
                        this.m_iPages++;
                        break;
                    }
                    ((Item) sender).Clicked += new ItemEventHandler(this.OnItemClick);
                    ((Item) sender).LabelClicked += new ItemEventHandler(this.OnItemLabelClick);
                    ((Item) sender).Changed += new ItemEventHandler(this.OnItemChanged);
                    ((Item) sender).GotFocus += new ItemEventHandler(this.OnItemGotFocus);
                    ((Item) sender).LostFocus += new ItemEventHandler(this.OnItemLostFocus);
                    ((Item) sender).Validating += new ItemValidatingEventHandler(this.OnItemValidating);
                    if (this.m_iSuspendRedraw <= 0)
                    {
                        this.m_iActualMaximumValue = this.m_ItemCollection.CalculateItemsHeight();
                        this.SetVScrollBar(this.m_iActualMaximumValue);
                    }
                    break;

                case DetailViewEventArgsType.ItemRemove:
                    this.m_pages = null;
                    if (sender is ItemPageBreak)
                    {
                        this.m_iPages--;
                        break;
                    }
                    ((Item) sender).Clicked -= new ItemEventHandler(this.OnItemClick);
                    ((Item) sender).LabelClicked -= new ItemEventHandler(this.OnItemLabelClick);
                    ((Item) sender).Changed -= new ItemEventHandler(this.OnItemChanged);
                    ((Item) sender).GotFocus -= new ItemEventHandler(this.OnItemGotFocus);
                    ((Item) sender).LostFocus -= new ItemEventHandler(this.OnItemLostFocus);
                    ((Item) sender).Validating -= new ItemValidatingEventHandler(this.OnItemValidating);
                    if (this.m_iSuspendRedraw <= 0)
                    {
                        this.m_iActualMaximumValue = this.m_ItemCollection.CalculateItemsHeight();
                        this.SetVScrollBar(this.m_iActualMaximumValue);
                    }
                    break;
            }
            if (this.m_iSuspendRedraw <= 0)
            {
                base.Invalidate();
            }
        }

        protected virtual void OnItemValidating(object sender, ValidatingEventArgs e)
        {
            IsEventRunning = true;
            if (this.ItemValidating != null)
            {
                this.ItemValidating(this, e);
            }
            IsEventRunning = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((this.KeyNavigation && (this.SelectedItem != null)) && (this.m_ItemCollection.Count > 0))
            {
                if ((this.m_SelectedItem != null) && this.m_SelectedItem.HandleKey(e.KeyCode))
                {
                    return;
                }
                int num = 0;
                if (e.KeyCode == Keys.Up)
                {
                    num = -1;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    num = 1;
                }
                if (num != 0)
                {
                    e.Handled = true;
                    this.m_iNavigationIndex = this.m_SelectedItem.Index;
                    this.m_iNavigationIndex += num;
                    bool flag = false;
                    while ((this.m_iNavigationIndex >= 0) && (this.m_iNavigationIndex < this.m_ItemCollection.Count))
                    {
                        Item item = this.m_ItemCollection[this.m_iNavigationIndex];
                        Page itemPage = this.Pages.GetItemPage(item);
                        if ((!flag && (itemPage != null)) && (itemPage != this.CurrentPage))
                        {
                            itemPage = this.CurrentPage;
                            item = (num > 0) ? itemPage[0] : itemPage[itemPage.Count - 1];
                            this.m_iNavigationIndex = item.Index;
                        }
                        flag = true;
                        if (((((item.GetType() != typeof(Item)) && item.Enabled) && (item.Visible && (item.GetType() != typeof(ItemPageBreak)))) && (itemPage != null)) && ((itemPage.PagingItem != null) ? itemPage.PagingItem.Visible : true))
                        {
                            this.m_nextNavItem = item;
                            this.m_useNextNavItem = true;
                            return;
                        }
                        this.m_iNavigationIndex += num;
                    }
                    this.m_nextNavItem = null;
                    this.m_useNextNavItem = true;
                    return;
                }
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (!this.m_keyUpEvent)
            {
                this.m_keyUpEvent = true;
                if (!this.KeyNavigation || (this.m_ItemCollection.Count <= 0))
                {
                    goto Label_0252;
                }
                if ((this.m_SelectedItem != null) && this.m_SelectedItem.HandleKeyUp(e.KeyCode))
                {
                    this.m_keyUpEvent = false;
                    return;
                }
                if (this.SelectedItem != null)
                {
                    goto Label_020C;
                }
                if (e.KeyCode != Keys.Up)
                {
                    if (e.KeyCode == Keys.Down)
                    {
                        for (int j = this.CurrentPage[0].Index; (j >= 0) && (j < this.m_ItemCollection.Count); j++)
                        {
                            Item item2 = this.m_ItemCollection[j];
                            Page itemPage = this.Pages.GetItemPage(item2);
                            if (((((item2.GetType() != typeof(Item)) && item2.Enabled) && (item2.Visible && (item2.GetType() != typeof(ItemPageBreak)))) && (itemPage != null)) && ((itemPage.PagingItem != null) ? itemPage.PagingItem.Visible : true))
                            {
                                this.SelectedItem = item2;
                                break;
                            }
                        }
                        this.m_keyUpEvent = false;
                        return;
                    }
                    if (e.KeyCode == Keys.Left)
                    {
                        this.PreviousPage();
                        this.m_keyUpEvent = false;
                        return;
                    }
                    if (e.KeyCode == Keys.Right)
                    {
                        this.NextPage();
                        this.m_keyUpEvent = false;
                        return;
                    }
                    goto Label_020C;
                }
                for (int i = this.CurrentPage[this.CurrentPage.Count - 1].Index; (i >= 0) && (i < this.m_ItemCollection.Count); i--)
                {
                    Item item = this.m_ItemCollection[i];
                    Page page = this.Pages.GetItemPage(item);
                    if (((((item.GetType() != typeof(Item)) && item.Enabled) && (item.Visible && (item.GetType() != typeof(ItemPageBreak)))) && (page != null)) && ((page.PagingItem != null) ? page.PagingItem.Visible : true))
                    {
                        this.SelectedItem = item;
                        break;
                    }
                }
                this.m_keyUpEvent = false;
            }
            return;
        Label_020C:
            if (this.m_useNextNavItem && ((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down)))
            {
                this.m_useNextNavItem = false;
                this.SelectedItem = this.m_nextNavItem;
                if (this.m_nextNavItem != null)
                {
                    this.m_keyUpEvent = false;
                    return;
                }
                this.m_keyUpEvent = false;
            }
        Label_0252:
            this.m_keyUpEvent = false;
            base.OnKeyUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.m_LastMousePosition.X = e.X;
            this.m_LastMousePosition.Y = e.Y;
            Rectangle rectangle = this.CalculateClientRect();
            int yOffset = -this.m_vScroll.Value + rectangle.Y;
            yOffset += (this.PagesLocation == RescoPagesLocation.Bottom) ? 0 : this.PagerHeight;
            int ctrlHeight = base.Height - (((this.PageCount == 0) || (this.PagesLocation == RescoPagesLocation.Top)) ? 0 : this.PagerHeight);
            this.m_ItemCollection.MouseUpDown(yOffset, e, this.UsableWidth, ctrlHeight, true);
            if (this.m_bEnableTouchScrolling)
            {
                this.m_TouchScrollingTimer.Enabled = false;
                this.m_TouchAutoScrollDiffX = 0;
                this.m_TouchAutoScrollDiffY = 0;
                this.m_touchPage = false;
                this.m_TouchTime0 = this.TickCount;
                this.m_TouchTime1 = this.m_TouchTime0;
            }
            if ((this.m_ToolTip == null) || !this.m_ToolTip.Visible)
            {
                try
                {
                    if ((this.m_ContextMenu != null) && ((e.Button == MouseButtons.Right) || ContextMenuSupport.RecognizeGesture(base.Handle, e.X, e.Y)))
                    {
                        this.m_ContextMenu.Show(this, this.m_LastMousePosition);
                    }
                }
                catch
                {
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if ((this.m_bEnableTouchScrolling && ((this.Site == null) || !this.Site.DesignMode)) && ((e.Y != this.m_LastMousePosition.Y) || (e.X != this.m_LastMousePosition.X)))
            {
                int num = e.Y - this.m_LastMousePosition.Y;
                int num2 = e.X - this.m_LastMousePosition.X;
                this.m_LastMousePosition.X = e.X;
                this.m_LastMousePosition.Y = e.Y;
                if ((this.m_ToolTip == null) || !this.m_ToolTip.Visible)
                {
                    this.m_TouchAutoScrollDiffY += num;
                    this.m_TouchAutoScrollDiffX += num2;
                    if ((this.TickCount - this.m_TouchTime1) > 100)
                    {
                        this.m_TouchAutoScrollDiffX = 0;
                        this.m_TouchAutoScrollDiffY = 0;
                        this.m_TouchTime0 = this.TickCount;
                        this.m_TouchTime1 = this.m_TouchTime0;
                    }
                    else
                    {
                        this.m_TouchTime1 = this.TickCount;
                    }
                    float width = this.m_dpiFactor.Width;
                    float height = this.m_dpiFactor.Height;
                    if ((this.m_bTouchScrolling || (Math.Abs(this.m_TouchAutoScrollDiffY) >= ((int) (this.m_touchSensitivity * width)))) || (this.m_bTouchScrolling || (Math.Abs(this.m_TouchAutoScrollDiffX) >= ((int) (this.m_touchSensitivity * height)))))
                    {
                        if (Math.Abs(this.m_TouchAutoScrollDiffX) > Math.Abs(this.m_TouchAutoScrollDiffY))
                        {
                            this.m_touchPage = true;
                        }
                        else
                        {
                            this.m_touchPage = false;
                        }
                        this.m_bTouchScrolling = true;
                        if ((this.m_vScroll != null) && !this.m_touchPage)
                        {
                            TouchGestureEventArgs args = new TouchGestureEventArgs();
                            args.Type = TouchGestureType.Scroll;
                            args.Difference = num;
                            args.Direction = (this.m_TouchAutoScrollDiffY > 0) ? Resco.Controls.DetailView.TouchGesture.Down : Resco.Controls.DetailView.TouchGesture.Up;
                            this.OnTouchGesture(args);
                            if (!args.Handled)
                            {
                                int num5 = this.m_vScroll.Value - ((this.m_touchScrollDirection == Resco.Controls.DetailView.TouchScrollDirection.Inverse) ? num : -num);
                                this.m_vScroll.Value = Math.Min(Math.Max(num5, this.m_vScroll.Minimum), this.m_vScroll.Maximum - this.m_vScroll.LargeChange);
                            }
                        }
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            try
            {
                this.m_LastMousePosition.X = e.X;
                this.m_LastMousePosition.Y = e.Y;
                Rectangle rectangle = this.CalculateClientRect();
                if ((this.m_ToolTip != null) && this.m_ToolTip.Visible)
                {
                    this.m_ToolTip.Visible = false;
                }
                else if (this.m_bEnableTouchScrolling && this.m_bTouchScrolling)
                {
                    if ((this.TickCount - this.m_TouchTime1) > 100)
                    {
                        this.m_TouchAutoScrollDiffY = 0;
                    }
                    if (this.m_touchPage)
                    {
                        this.m_touchPage = false;
                        if (this.m_TouchAutoScrollDiffX > 0)
                        {
                            TouchGestureEventArgs right = TouchGestureEventArgs.Right;
                            right.Difference = this.m_TouchAutoScrollDiffX;
                            this.OnTouchGesture(right);
                            if (!right.Handled)
                            {
                                if (this.m_touchPagesDirection == Resco.Controls.DetailView.TouchScrollDirection.Inverse)
                                {
                                    this.PreviousPage();
                                }
                                else
                                {
                                    this.NextPage();
                                }
                            }
                        }
                        else
                        {
                            TouchGestureEventArgs left = TouchGestureEventArgs.Left;
                            left.Difference = this.m_TouchAutoScrollDiffX;
                            this.OnTouchGesture(left);
                            if (!left.Handled)
                            {
                                if (this.m_touchPagesDirection == Resco.Controls.DetailView.TouchScrollDirection.Inverse)
                                {
                                    this.NextPage();
                                }
                                else
                                {
                                    this.PreviousPage();
                                }
                            }
                        }
                    }
                    else
                    {
                        uint num = (this.TickCount - this.m_TouchTime0) / 50;
                        if (num > 0)
                        {
                            this.m_TouchAutoScrollDiffY = (int) (this.m_TouchAutoScrollDiffY / num);
                        }
                        TouchGestureEventArgs args3 = new TouchGestureEventArgs();
                        args3.Type = TouchGestureType.Gesture;
                        args3.Difference = this.m_TouchAutoScrollDiffY;
                        args3.Direction = (this.m_TouchAutoScrollDiffY > 0) ? Resco.Controls.DetailView.TouchGesture.Down : Resco.Controls.DetailView.TouchGesture.Up;
                        this.OnTouchGesture(args3);
                        if (!args3.Handled)
                        {
                            this.m_TouchScrollingTimer.Enabled = true;
                        }
                    }
                    this.m_bTouchScrolling = false;
                }
                int yOffset = -this.m_vScroll.Value + rectangle.Y;
                yOffset += (this.PagesLocation == RescoPagesLocation.Bottom) ? 0 : this.PagerHeight;
                int ctrlHeight = base.Height - (((this.PageCount == 0) || (this.PagesLocation == RescoPagesLocation.Top)) ? 0 : this.PagerHeight);
                this.m_ItemCollection.MouseUpDown(yOffset, e, this.UsableWidth, ctrlHeight, false);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        protected virtual void OnPageChanged(EventArgs e)
        {
            if (this.PageChanged != null)
            {
                this.PageChanged(this, e);
            }
        }

        protected virtual void OnPageChanging(PageChangeEventArgs e)
        {
            if (this.PageChanging != null)
            {
                this.PageChanging(this, e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.m_iSuspendRedraw <= 0)
            {
                Rectangle clientRect = this.CalculateClientRect();
                if (this.m_doubleBuffered)
                {
                    this.Redraw(BackBufferManager.GetBackBufferGraphics(base.ClientRectangle.Width, base.ClientRectangle.Height), clientRect);
                    e.Graphics.DrawImage(BackBufferManager.GetBackBufferImage(base.ClientRectangle.Width, base.ClientRectangle.Height), clientRect, clientRect, GraphicsUnit.Pixel);
                }
                else
                {
                    e.Graphics.Clip = new Region(clientRect);
                    this.Redraw(e.Graphics, clientRect);
                    e.Graphics.ResetClip();
                }
            }
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if ((this.m_vScroll != null) && !base.Controls.Contains((Control) this.m_vScroll))
            {
                this.m_vScroll.Visible = this.m_bScrollVisible;
                base.Controls.Add((Control) this.m_vScroll);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            try
            {
                int num = (this.PagesLocation == RescoPagesLocation.Bottom) ? 0 : this.PagerHeight;
                if (this.m_vScroll != null)
                {
                    Rectangle rectangle = this.CalculateClientRect();
                    this.m_vScroll.Bounds = new Rectangle((rectangle.X + rectangle.Width) - this.m_VScrollBarWidth, rectangle.Y + num, this.m_VScrollBarWidth, rectangle.Height);
                }
                this.SetVScrollBar(this.m_iActualMaximumValue);
                int usableWidth = this.UsableWidth;
                this.ResizeControls();
                this.EnsureVisible(this.SelectedItem, false);
            }
            catch (ObjectDisposedException)
            {
            }
            base.Invalidate();
        }

        private void OnScrollResize(object sender, EventArgs e)
        {
            this.ScrollbarWidth = this.m_vScroll.Width;
        }

        protected virtual void OnTouchGesture(TouchGestureEventArgs e)
        {
            if (this.TouchGesture != null)
            {
                this.TouchGesture(this, e);
            }
        }

        private void OnTouchScrollingTimerTick(object sender, EventArgs e)
        {
            if (this.m_vScroll != null)
            {
                this.m_vScroll.Value -= (this.m_touchScrollDirection == Resco.Controls.DetailView.TouchScrollDirection.Inverse) ? this.m_TouchAutoScrollDiffY : -this.m_TouchAutoScrollDiffY;
            }
            if (this.m_TouchAutoScrollDiffY < 0)
            {
                this.m_TouchAutoScrollDiffY += (Math.Abs(this.m_TouchAutoScrollDiffY) / 10) + 1;
                if ((this.m_TouchAutoScrollDiffY > 0) || (this.m_vScroll.Value > (this.m_vScroll.Maximum - this.m_vScroll.LargeChange)))
                {
                    this.m_TouchAutoScrollDiffY = 0;
                    this.m_TouchScrollingTimer.Enabled = false;
                }
            }
            else if (this.m_TouchAutoScrollDiffY > 0)
            {
                this.m_TouchAutoScrollDiffY -= (Math.Abs(this.m_TouchAutoScrollDiffY) / 10) + 1;
                if ((this.m_TouchAutoScrollDiffY < 0) || (this.m_vScroll.Value <= this.m_vScroll.Minimum))
                {
                    this.m_TouchAutoScrollDiffY = 0;
                    this.m_TouchScrollingTimer.Enabled = false;
                }
            }
            else
            {
                this.m_TouchAutoScrollDiffY = 0;
                this.m_TouchScrollingTimer.Enabled = false;
            }
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (this.SelectedItem != null)
            {
                this.SelectedItem._MoveTop(this.m_iPrevValue - this.m_vScroll.Value);
            }
            base.Invalidate();
            this.m_iPrevValue = this.m_vScroll.Value;
            if (this.Scroll != null)
            {
                this.Scroll(this, e);
            }
        }

        public void PreviousPage()
        {
            Page page;
            int pageIndex = this.CurrentPage.PageIndex;
            do
            {
                page = null;
                pageIndex += this.PagesRightToLeft ? 1 : -1;
                if ((pageIndex < 0) || (pageIndex >= this.Pages.Count))
                {
                    break;
                }
                page = this.Pages[pageIndex];
            }
            while (((page != null) && (page.PagingItem != null)) && !page.PagingItem.Visible);
            if (page != null)
            {
                this.HideActiveControl();
                if (this.SelectedItem == null)
                {
                    this.CurrentPage = page;
                }
            }
        }

        private void ReadCollection(XmlReader reader, object obj, PropertyDescriptor pd)
        {
            IList list = null;
            if (!pd.IsReadOnly)
            {
                ConstructorInfo info = this.ReadGetType(reader).GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);
                if (info != null)
                {
                    list = (IList) info.Invoke(new object[0]);
                }
                pd.SetValue(obj, list);
            }
            else
            {
                list = (IList) pd.GetValue(obj);
            }
            while (reader.Read())
            {
                try
                {
                    object obj2;
                    string str;
                    if (((str = reader.Name) == null) || (str == ""))
                    {
                        continue;
                    }
                    if (!(str == "Collection"))
                    {
                        if (str == "Object")
                        {
                            goto Label_008A;
                        }
                        continue;
                    }
                    break;
                Label_008A:
                    obj2 = this.ReadObject(reader);
                    list.Add(obj2);
                    continue;
                }
                catch
                {
                    continue;
                }
            }
        }

        private void ReadDetailView(XmlReader reader)
        {
            this.SuspendRedraw();
            if (this.m_vScroll != null)
            {
                this.m_vScroll.Visible = false;
            }
            Graphics graphics = base.CreateGraphics();
            float dpiX = graphics.DpiX;
            float dpiY = graphics.DpiY;
            if (this._designTimeCallback != null)
            {
                Size size = (Size) this._designTimeCallback(3, null);
                dpiX = size.Width;
                dpiY = size.Height;
            }
            int num3 = (int) dpiX;
            int num4 = (int) dpiY;
            graphics.Dispose();
            graphics = null;
            try
            {
                SizeF ef = new SizeF(dpiX / 96f, dpiY / 96f);
                if (ef.Width != 1f)
                {
                    this.SeparatorWidth = (int) (((float) this.SeparatorWidth) / ef.Width);
                    this.LabelWidth = (int) (((float) this.LabelWidth) / ef.Width);
                    this.ScrollbarWidth = (int) (((float) this.ScrollbarWidth) / ef.Width);
                }
                if ((ef.Height != 1f) && (this.PagerHeight >= 0))
                {
                    this.PagerHeight = (int) (((float) this.PagerHeight) / ef.Height);
                }
                if (reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                    {
                        try
                        {
                            if (reader.Name.ToLower() == "dpix")
                            {
                                num3 = Convert.ToInt32(reader.Value);
                            }
                            if (reader.Name.ToLower() == "dpiy")
                            {
                                num4 = Convert.ToInt32(reader.Value);
                            }
                            else
                            {
                                this.m_Conversion.SetProperty(this, reader.Name, reader.Value);
                            }
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    reader.MoveToElement();
                }
                this.Items.Clear();
                if (!reader.IsEmptyElement)
                {
                    goto Label_0273;
                }
                return;
            Label_019A:
                try
                {
                    SizeF ef2;
                    string str;
                    if (((str = reader.Name) != null) && (str != ""))
                    {
                        if (!(str == "Item"))
                        {
                            if (str == "DetailView")
                            {
                                goto Label_01FF;
                            }
                            if (str == "Property")
                            {
                                goto Label_0232;
                            }
                            goto Label_0256;
                        }
                        Item item = this.ReadItem(reader);
                        if (item != null)
                        {
                            this.Items.Add(item);
                        }
                    }
                    goto Label_0273;
                Label_01FF:
                    ef2 = new SizeF(dpiX / ((float) num3), dpiY / ((float) num4));
                    base.Scale(ef2);
                    base.Invalidate();
                    this.ResumeRedraw();
                    this.SetVScrollBar(this.m_iActualMaximumValue);
                    return;
                Label_0232:
                    this.m_Conversion.SetProperty(this, reader["Name"], reader["Value"]);
                    goto Label_0273;
                Label_0256:
                    this.m_Conversion.SetProperty(this, reader.Name, reader.ReadString());
                }
                catch (Exception)
                {
                }
            Label_0273:
                if (reader.Read())
                {
                    goto Label_019A;
                }
            }
            catch (Exception)
            {
            }
            this.ResumeRedraw();
        }

        private Type ReadGetType(XmlReader reader)
        {
            Type type = Type.GetType(reader["Type"]);
            if (type == null)
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.LoadFrom(reader["Assembly"] + ".cf2.dll");
                }
                catch
                {
                    try
                    {
                        assembly = Assembly.LoadFrom(reader["Assembly"] + ".cf3.dll");
                    }
                    catch
                    {
                        try
                        {
                            assembly = Assembly.LoadFrom(reader["Assembly"] + ".dll");
                        }
                        catch
                        {
                        }
                    }
                }
                if (assembly != null)
                {
                    type = assembly.GetType(reader["Type"]);
                }
            }
            return type;
        }

        private Item ReadItem(XmlReader reader)
        {
            Item o = null;
            try
            {
                string str = reader["Name"];
                string typeName = reader["Type"];
                if (typeName == null)
                {
                    typeName = "Resco.Controls.DetailView.Item";
                }
                if (!typeName.StartsWith("Resco.Controls.DetailView.") && typeName.StartsWith("Resco.Controls."))
                {
                    typeName = typeName.Insert(typeName.LastIndexOf('.'), ".DetailView");
                }
                ConstructorInfo info = Type.GetType(typeName).GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);
                if (info != null)
                {
                    o = (Item) info.Invoke(new object[0]);
                }
                o.Name = str;
                if (!reader.IsEmptyElement)
                {
                    goto Label_0147;
                }
                return o;
            Label_0096:
                try
                {
                    string str3;
                    if (((str3 = reader.Name) == null) || (str3 == ""))
                    {
                        goto Label_0147;
                    }
                    if (!(str3 == "Item"))
                    {
                        if (str3 == "Property")
                        {
                            goto Label_00FF;
                        }
                        if (str3 == "Collection")
                        {
                            goto Label_0123;
                        }
                        goto Label_0147;
                    }
                    if (this._designTimeCallback != null)
                    {
                        this._designTimeCallback(1, o);
                    }
                    return o;
                Label_00FF:
                    this.m_Conversion.SetProperty(o, reader["Name"], reader["Value"]);
                    goto Label_0147;
                Label_0123:
                    this.ReadCollection(reader, o, TypeDescriptor.GetProperties(o).Find(reader["Name"], false));
                }
                catch
                {
                }
            Label_0147:
                if (reader.Read())
                {
                    goto Label_0096;
                }
            }
            catch
            {
                try
                {
                    if (((reader.Name == "Item") && reader.IsStartElement()) && !reader.IsEmptyElement)
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "Item")
                            {
                                goto Label_019A;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        Label_019A:
            o = (o == null) ? new Item() : o;
            if (this._designTimeCallback != null)
            {
                this._designTimeCallback(1, o);
            }
            return o;
        }

        private object ReadObject(XmlReader reader)
        {
            object o = null;
            ConstructorInfo info = this.ReadGetType(reader).GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);
            if (info != null)
            {
                o = info.Invoke(new object[0]);
            }
            while (reader.Read())
            {
                try
                {
                    string str;
                    if (((str = reader.Name) == null) || (str == ""))
                    {
                        continue;
                    }
                    if (!(str == "Object"))
                    {
                        if (str == "Property")
                        {
                            goto Label_0098;
                        }
                        if (str == "Collection")
                        {
                            goto Label_00BC;
                        }
                        continue;
                    }
                    if (this._designTimeCallback != null)
                    {
                        this._designTimeCallback(2, o);
                    }
                    return o;
                Label_0098:
                    this.m_Conversion.SetProperty(o, reader["Name"], reader["Value"]);
                    continue;
                Label_00BC:
                    this.ReadCollection(reader, o, TypeDescriptor.GetProperties(o).Find(reader["Name"], false));
                    continue;
                }
                catch
                {
                    continue;
                }
            }
            if (this._designTimeCallback != null)
            {
                this._designTimeCallback(2, o);
            }
            return o;
        }

        private void Redraw(Graphics gr, Rectangle clientRect)
        {
            int num = -this.m_vScroll.Value;
            int x = clientRect.X;
            int y = clientRect.Y;
            int width = clientRect.Width;
            int height = clientRect.Height;
            if (!this.m_useGradient)
            {
                gr.FillRectangle(this.m_BackColor, x, y, width, height);
            }
            else
            {
                int pagerHeight = 0;
                int num7 = height;
                FillDirection fillDirection = this.m_gradientBackColor.FillDirection;
                if (this.HasPages)
                {
                    if (this.PagesLocation == RescoPagesLocation.Top)
                    {
                        pagerHeight = this.PagerHeight;
                    }
                    num7 -= this.PagerHeight;
                }
                Rectangle dstrc = new Rectangle(x, y + pagerHeight, width, num7);
                Rectangle srcrc = new Rectangle(x, y, width, height);
                GradientFill.Fill(gr, dstrc, srcrc, this.m_gradientBackColor);
            }
            this.m_ItemCollection.Draw(gr, x, y + num, width - (this.m_bScrollVisible ? this.m_VScrollBarWidth : 0), height - ((this.PageCount == 0) ? 0 : this.PagerHeight));
        }

        internal void ResizeControls()
        {
            if (this.m_SelectedItem != null)
            {
                this.m_SelectedItem.UpdateWidth(this.UsableWidth);
            }
        }

        public void ResumeRedraw()
        {
            if (this.m_iSuspendRedraw > 0)
            {
                this.m_iSuspendRedraw--;
            }
            if (this.m_iSuspendRedraw <= 0)
            {
                this.ItemResized(true, 0);
            }
        }

        public void SaveXml(string fileName)
        {
            DVXmlSerializer.SaveXml(fileName, this);
        }

        public void SaveXml(XmlWriter writer)
        {
            DVXmlSerializer.SaveXml(writer, this);
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.m_scaleFactor = factor;
            float width = factor.Width;
            float height = factor.Height;
            if (width != 1.0)
            {
                this.SeparatorWidth = (int) (this.SeparatorWidth * width);
                this.LabelWidth = (int) (this.LabelWidth * width);
                if (this.m_vScrollBarResco == null)
                {
                    this.ScrollbarWidth = (int) (this.ScrollbarWidth * width);
                }
            }
            if ((height != 1.0) && (this._pagerHeight > 0))
            {
                this._pagerHeight = (int) (this._pagerHeight * height);
            }
            if ((width != 1.0) || (height != 1.0))
            {
                foreach (Item item in this.m_ItemCollection)
                {
                    item.ScaleItem(width, height);
                }
                this.m_iActualMaximumValue = this.m_ItemCollection.CalculateItemsHeight();
            }
            base.ScaleControl(factor, specified);
            this.SetVScrollBar(this.m_iActualMaximumValue);
        }

        private static void ScaleSettings(float dpix, float dpiy)
        {
            if ((dpix != _DPIx) || (dpiy != _DPIy))
            {
                float num = dpix / _DPIx;
                float num2 = dpiy / _DPIy;
                TooltipWidth = (int) (num * TooltipWidth);
                ErrorSpacer = (int) (num * ErrorSpacer);
                UpDownButtonWidth = (int) (num * UpDownButtonWidth);
                TabImageSize.Width = (int) (num * TabImageSize.Width);
                TabImageSize.Height = (int) (num2 * TabImageSize.Height);
                ArrowImageSize.Width = (int) (num * ArrowImageSize.Width);
                ArrowImageSize.Height = (int) (num2 * ArrowImageSize.Height);
                Item.ItemRoundedCornerSize = (int) (num * Item.ItemRoundedCornerSize);
                ItemLink.LinkWidth = (int) (num * ItemLink.LinkWidth);
                ItemCheckBox.CheckWidth = (int) (num * ItemCheckBox.CheckWidth);
                ItemCheckBox.CheckHeight = (int) (num2 * ItemCheckBox.CheckHeight);
                ItemDateTime.DTWidth = (int) (num * 90f);
                ItemAdvancedComboBox.ItemACArrowHeight = (int) (num * ItemAdvancedComboBox.ItemACArrowHeight);
                DefaultTabArrowsHeight = (int) (num2 * DefaultTabArrowsHeight);
                DefaultTabStripHeight = (int) (num2 * DefaultTabStripHeight);
                DateTimePickerEx.Const.DropArrowSize.Width = (int) (num * DateTimePickerEx.Const.DropArrowSize.Width);
                DateTimePickerEx.Const.DropArrowSize.Height = (int) (num2 * DateTimePickerEx.Const.DropArrowSize.Height);
                DateTimePickerEx.Const.DropDownWidth = (int) (num * DateTimePickerEx.Const.DropDownWidth);
                if (num > 1f)
                {
                    DVTextBox.BorderSize = 2;
                }
                else
                {
                    DVTextBox.BorderSize = 1;
                }
                ComboSize = (int) (num * ComboSize);
                ComboRectangle = new Rectangle(ComboSize / 4, ComboSize * 2, (2 * ComboSize) - 1, ComboSize);
                _DPIx = dpix;
                _DPIy = dpiy;
            }
        }

        public int ScrollTo(int y)
        {
            int num = y;
            if (y > (this.m_vScroll.Maximum - this.m_vScroll.LargeChange))
            {
                y = this.m_vScroll.Maximum - this.m_vScroll.LargeChange;
            }
            if (y < 0)
            {
                y = 0;
            }
            int num1 = this.m_vScroll.Value;
            this.m_vScroll.Value = y;
            return (num - y);
        }

        internal static void SetBoundItem(BindingManagerBase bmb, object item, string strField, object value)
        {
            PropertyDescriptor descriptor = null;
            if (((item != null) && (strField != null)) && (strField.Length > 0))
            {
                try
                {
                    if (bmb != null)
                    {
                        descriptor = bmb.GetItemProperties().Find(strField, true);
                    }
                    else
                    {
                        descriptor = TypeDescriptor.GetProperties(item).Find(strField, true);
                    }
                    if (descriptor != null)
                    {
                        descriptor.SetValue(item, ConvertWithNullable(value, descriptor.PropertyType));
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        IConvertible convertible = value as IConvertible;
                        if ((convertible != null) && (descriptor != null))
                        {
                            descriptor.SetValue(item, convertible.ToType(descriptor.PropertyType, CultureInfo.CurrentCulture));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        internal void SetVScrollBar(int height)
        {
            try
            {
                if (this.m_vScroll == null)
                {
                    if (this.m_vScrollBarResco == null)
                    {
                        this.m_vScroll = new ScrollbarWrapper(new System.Windows.Forms.VScrollBar(), ScrollOrientation.VerticalScroll);
                    }
                    else
                    {
                        this.m_vScroll = new ScrollbarWrapper(this.m_vScrollBarResco, ScrollOrientation.VerticalScroll);
                    }
                    this.m_vScroll.TabStop = false;
                    this.m_vScroll.Attach(this);
                    this.m_vScroll.ValueChanged += new EventHandler(this.OnValueChanged);
                    this.m_vScroll.Resize += new EventHandler(this.OnScrollResize);
                    this.m_vScroll.BringToFront();
                }
                Rectangle rectangle = this.CalculateClientRect();
                Rectangle rectangle2 = new Rectangle();
                rectangle2.Width = this.m_VScrollBarWidth;
                rectangle2.Height = rectangle.Height - ((this.PageCount == 0) ? 0 : this.PagerHeight);
                rectangle2.X = (rectangle.X + rectangle.Width) - this.m_VScrollBarWidth;
                if (this.PagesLocation == RescoPagesLocation.Top)
                {
                    rectangle2.Y = rectangle.Y + this.PagerHeight;
                }
                else
                {
                    rectangle2.Y = rectangle.Y;
                }
                this.m_vScroll.Bounds = rectangle2;
                this.m_iPrevValue = this.m_vScroll.Value;
                this.m_vScroll.Maximum = height;
                this.m_vScroll.SmallChange = 20;
                int num = rectangle.Height - ((this.PageCount == 0) ? 0 : this.PagerHeight);
                this.m_vScroll.LargeChange = (num > 0) ? num : 0;
                if (this.m_vScroll.Maximum > this.m_vScroll.LargeChange)
                {
                    if (!this.m_bScrollVisible)
                    {
                        this.m_bScrollVisible = true;
                        this.m_vScroll.Show();
                        this.ResizeControls();
                    }
                    if (this.m_iPrevValue != this.m_vScroll.Value)
                    {
                        this.OnValueChanged(this, new EventArgs());
                    }
                }
                else
                {
                    if (this.m_bScrollVisible)
                    {
                        this.m_bScrollVisible = false;
                        this.m_vScroll.Hide();
                        this.ResizeControls();
                    }
                    this.m_vScroll.Value = 0;
                    this.OnValueChanged(this, new EventArgs());
                }
            }
            catch (Exception)
            {
            }
        }

        protected virtual bool ShouldSerializeArrowStyle()
        {
            return (this.m_ArrowStyle != RescoArrowStyle.LeftRight);
        }

        protected virtual bool ShouldSerializeGradientBackColor()
        {
            return (((this.m_gradientBackColor.StartColor.ToArgb() != Color.White.ToArgb()) | (this.m_gradientBackColor.EndColor.ToArgb() != Color.White.ToArgb())) | (this.m_gradientBackColor.FillDirection != FillDirection.Vertical));
        }

        internal bool ShouldSerializePagerHeight()
        {
            return (this._pagerHeight >= 0);
        }

        protected virtual bool ShouldSerializePagesLocation()
        {
            return (this.m_pagesLocation != RescoPagesLocation.Bottom);
        }

        protected virtual bool ShouldSerializePagingStyle()
        {
            return (this.m_PagesStyle != RescoPageStyle.Arrows);
        }

        protected virtual bool ShouldSerializeTouchPagesDirection()
        {
            return (this.m_touchPagesDirection != Resco.Controls.DetailView.TouchScrollDirection.Inverse);
        }

        protected virtual bool ShouldSerializeTouchScrollDirection()
        {
            return (this.m_touchScrollDirection != Resco.Controls.DetailView.TouchScrollDirection.Inverse);
        }

        internal void ShowToolTip(string text, int x, int y, bool bShow)
        {
            if (bShow && (this.m_ToolTip == null))
            {
                this.m_ToolTip = this.GetControl(typeof(Resco.Controls.DetailView.ToolTip)) as Resco.Controls.DetailView.ToolTip;
            }
            if (this.m_ToolTip != null)
            {
                this.m_ToolTip.Text = text;
                if (bShow)
                {
                    this.m_ToolTip.Show(new Point(x, y));
                }
            }
        }

        public void SuspendRedraw()
        {
            this.m_iSuspendRedraw++;
        }

        public void UpdateControl()
        {
            if (!this.InUpdate)
            {
                if ((this.m_bmb == null) || (this.m_bmb.Position < 0))
                {
                    this.ClearContents(true);
                }
                else
                {
                    this.InUpdate = true;
                    ItemBindingException next = null;
                    try
                    {
                        object current = this.m_bmb.Current;
                        int length = 0;
                        DataRow row = null;
                        DataColumn[] columnsInError = null;
                        if (current is DataRowView)
                        {
                            row = ((DataRowView) current).Row;
                            columnsInError = row.GetColumnsInError();
                            length = columnsInError.Length;
                        }
                        if (length > 0)
                        {
                            foreach (Item item in this.Items)
                            {
                                if (this.InUpdateItem != item)
                                {
                                    string dataMember = item.DataMember;
                                    if (dataMember.Length != 0)
                                    {
                                        object obj3 = GetBoundItem(this.m_bmb, current, dataMember);
                                        try
                                        {
                                            item.Value = (obj3 == DBNull.Value) ? null : obj3;
                                        }
                                        catch (Exception exception2)
                                        {
                                            next = new ItemBindingException(item, obj3, exception2, next);
                                        }
                                        for (int i = 0; i < length; i++)
                                        {
                                            if (columnsInError[i].ColumnName == dataMember)
                                            {
                                                item.ErrorMessage = row.GetColumnError(columnsInError[i]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Item item2 in this.Items)
                            {
                                if (this.InUpdateItem != item2)
                                {
                                    string strField = item2.DataMember;
                                    if (strField.Length != 0)
                                    {
                                        object obj4 = GetBoundItem(this.m_bmb, current, strField);
                                        try
                                        {
                                            item2.Value = (obj4 == DBNull.Value) ? null : obj4;
                                            continue;
                                        }
                                        catch (Exception exception3)
                                        {
                                            next = new ItemBindingException(item2, obj4, exception3, next);
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                        if (next != null)
                        {
                            throw next;
                        }
                    }
                    finally
                    {
                        this.InUpdate = false;
                        base.Invalidate();
                    }
                }
            }
        }

        internal void UpdateData(Item itemChanged)
        {
            if (((!this.InUpdate && (this.InUpdateItem != itemChanged)) && ((this.m_bmb != null) && (this.m_bmb.Position >= 0))) && (itemChanged.DataMember != ""))
            {
                this.InUpdateItem = itemChanged;
                try
                {
                    object obj2 = itemChanged.Value;
                    if ((obj2 == null) && (this.CurrentTable != null))
                    {
                        obj2 = DBNull.Value;
                    }
                    SetBoundItem(this.m_bmb, this.m_bmb.Current, itemChanged.DataMember, obj2);
                }
                finally
                {
                    this.InUpdateItem = null;
                }
            }
        }

        internal void UpdateError(Item itemChanged)
        {
            if (this.InUpdateItem != itemChanged)
            {
                DataRow currentRow = this.CurrentRow;
                if (currentRow != null)
                {
                    DataTable currentTable = this.CurrentTable;
                    if ((currentTable != null) && ((this.m_bmb != null) && (this.m_bmb.Position >= 0)))
                    {
                        int index = currentTable.Columns.IndexOf(itemChanged.DataMember);
                        if (index >= 0)
                        {
                            this.InUpdateItem = itemChanged;
                            try
                            {
                                string errorMessage = itemChanged.ErrorMessage;
                                currentRow.SetColumnError(index, errorMessage);
                            }
                            finally
                            {
                                this.InUpdateItem = null;
                            }
                        }
                    }
                }
            }
        }

        public RescoArrowStyle ArrowStyle
        {
            get
            {
                return this.m_ArrowStyle;
            }
            set
            {
                if (this.m_ArrowStyle != value)
                {
                    this.m_ArrowStyle = value;
                    base.Invalidate();
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), DefaultValue(true)]
        public bool AutoRefresh
        {
            get
            {
                return this.m_AutoRefresh;
            }
            set
            {
                this.m_AutoRefresh = value;
            }
        }

        [DefaultValue("White")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                if (value != base.BackColor)
                {
                    this.m_BackColor = new SolidBrush(value);
                    base.BackColor = value;
                    foreach (Item item in this.Items)
                    {
                        item.ParentChangeBackColor(value);
                    }
                }
            }
        }

        internal static Image BmpCheckNull
        {
            get
            {
                Image image = sImages[0x11];
                if (image == null)
                {
                    image = LoadBitmap("Resco.Controls.DetailView.Bitmaps.cb_null.bmp");
                    sImages[0x11] = image;
                }
                return image;
            }
        }

        internal static Image BmpCheckTrue
        {
            get
            {
                Image image = sImages[0x10];
                if (image == null)
                {
                    image = LoadBitmap("Resco.Controls.DetailView.Bitmaps.cb_check.bmp");
                    sImages[0x10] = image;
                }
                return image;
            }
        }

        internal static Image BmpComboBox
        {
            get
            {
                Image image = sImages[0x12];
                if (image == null)
                {
                    image = new Bitmap((2 * ComboSize) - 1, ComboSize);
                    Graphics graphics = Graphics.FromImage(image);
                    graphics.Clear(Color.White);
                    Point[] points = new Point[] { new Point(0, 0), new Point(ComboSize - 1, ComboSize - 1), new Point((2 * ComboSize) - 2, 0) };
                    graphics.FillPolygon(new SolidBrush(Color.Black), points);
                    graphics.Dispose();
                    sImages[0x12] = image;
                }
                return image;
            }
        }

        [DefaultValue(false)]
        public bool Border
        {
            get
            {
                return (base.BorderStyle != BorderStyle.None);
            }
            set
            {
                base.BorderStyle=(value ? BorderStyle.FixedSingle : BorderStyle.None);
            }
        }

        public override System.Windows.Forms.ContextMenu ContextMenu
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

        [DefaultValue(0), Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public Page CurrentPage
        {
            get
            {
                PageCollection pages = this.Pages;
                if (pages == null)
                {
                    return null;
                }
                if (this.CurrentPageIndex < 0)
                {
                    this.m_iCurrentPage = 0;
                }
                else if (this.CurrentPageIndex >= pages.Count)
                {
                    this.m_iCurrentPage = pages.Count - 1;
                }
                return pages[this.CurrentPageIndex];
            }
            set
            {
                if (value != null)
                {
                    this.CurrentPageIndex = value.PageIndex;
                }
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue(0), Resco.Controls.DetailView.Design.Browsable(false)]
        public int CurrentPageIndex
        {
            get
            {
                return this.m_iCurrentPage;
            }
            set
            {
                if (((value != this.m_iCurrentPage) && (value >= 0)) && (value < this.PageCount))
                {
                    PageChangeEventArgs e = new PageChangeEventArgs(this.m_iCurrentPage, value);
                    this.OnPageChanging(e);
                    if (!e.Cancel)
                    {
                        bool flag = this.SelectedItem == this.CurrentPage.PagingItem;
                        this.m_iCurrentPage = value;
                        if (flag)
                        {
                            this.SelectedItem = this.CurrentPage.PagingItem;
                        }
                        this.m_iActualMaximumValue = this.m_ItemCollection.CalculateItemsHeight();
                        this.SetVScrollBar(this.m_iActualMaximumValue);
                        base.Invalidate();
                        this.OnPageChanged(EventArgs.Empty);
                    }
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public DataRow CurrentRow
        {
            get
            {
                if (((this.m_bmb != null) && (this.m_bmb.Position >= 0)) && (this.m_bmb.Current is DataRowView))
                {
                    return ((DataRowView) this.m_bmb.Current).Row;
                }
                return null;
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public DataTable CurrentTable
        {
            get
            {
                if (this.m_dataSource is DataTable)
                {
                    return (DataTable) this.m_dataSource;
                }
                if (this.m_dataSource is DataView)
                {
                    return ((DataView) this.m_dataSource).Table;
                }
                return null;
            }
        }

        [DefaultValue((string) null), Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public object DataSource
        {
            get
            {
                return this.m_dataSource;
            }
            set
            {
                if (this.m_dataSource != value)
                {
                    if (value == null)
                    {
                        if (this.m_bmb != null)
                        {
                            this.m_bmb.CurrentChanged -= new EventHandler(this.OnBindingChanged);
                            this.m_bmb.CurrentItemChanged+=(new EventHandler(this.OnBindingChanged));
                            this.m_bmb = null;
                        }
                        this.ClearContents(true);
                    }
                    else if (this.BindingContext != null)
                    {
                        this.m_bmb = this.BindingContext[value];
                        this.m_bmb.CurrentChanged += new EventHandler(this.OnBindingChanged);
                        this.m_bmb.CurrentItemChanged += new EventHandler(this.OnBindingChanged);//.add_CurrentItemChanged(new EventHandler(this.OnBindingChanged));
                    }
                    this.m_dataSource = value;
                    this.UpdateControl();
                }
            }
        }

        [DefaultValue("GrayText")]
        public Color DisabledForeColor
        {
            get
            {
                return this.m_disabledForeColor;
            }
            set
            {
                if (value != this.m_disabledForeColor)
                {
                    this.m_disabledForeColor = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(true)]
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
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool EnableDesignTimeCustomItems
        {
            get
            {
                return this.m_enableDesignTimeCustomItems;
            }
            set
            {
                this.m_enableDesignTimeCustomItems = value;
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), DefaultValue((string) null)]
        public IList ErrorMessageItems
        {
            get
            {
                ArrayList list = new ArrayList();
                if (this.Items != null)
                {
                    foreach (Item item in this.Items)
                    {
                        if ((item.ErrorMessage != null) && (item.ErrorMessage != ""))
                        {
                            list.Add(item);
                        }
                    }
                }
                return list;
            }
        }

        public bool Focused
        {
            get
            {
                return (base.Focused || ((this.SelectedItem != null) && this.SelectedItem.Focused));
            }
        }

        [DefaultValue("ControlText")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                if (value != base.ForeColor)
                {
                    base.ForeColor = value;
                    foreach (Item item in this.Items)
                    {
                        item.ParentChangeForeColor(value);
                    }
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
                    this.m_gradientBackColor = null;
                    this.m_gradientBackColor = value;
                    this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
                }
                base.Invalidate();
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false)]
        public bool HasPages
        {
            get
            {
                return (this.m_iPages > 0);
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Content)]
        public virtual DetailViewItems Items
        {
            get
            {
                return this.m_ItemCollection;
            }
        }

        [DefaultValue(false)]
        public bool KeyNavigation
        {
            get
            {
                return this.m_KeyNavigation;
            }
            set
            {
                this.m_KeyNavigation = value;
            }
        }

        [DefaultValue(50)]
        public virtual int LabelWidth
        {
            get
            {
                return this.m_LabelWidth;
            }
            set
            {
                if (value > 0)
                {
                    this.m_LabelWidth = value;
                }
                else
                {
                    this.m_LabelWidth = 0;
                }
                base.Invalidate();
                base.Update();
            }
        }

        [DefaultValue((string) null), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.DetailView.Design.Browsable(false)]
        internal Point LastMousePosition
        {
            get
            {
                return this.m_LastMousePosition;
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.DetailView.Design.Browsable(false)]
        public int PageCount
        {
            get
            {
                if (this.Pages == null)
                {
                    return 0;
                }
                int count = this.Pages.Count;
                if ((count <= 1) && (this.Pages[0].PagingItem == null))
                {
                    return 0;
                }
                return count;
            }
        }

        public int PagerHeight
        {
            get
            {
                if ((this.CurrentPage == null) || (this.CurrentPage.PagingItem == null))
                {
                    return -1;
                }
                if (this._pagerHeight >= 0)
                {
                    return this._pagerHeight;
                }
                switch (this.m_PagesStyle)
                {
                    case RescoPageStyle.TabStrip:
                        return DefaultTabStripHeight;
                }
                return DefaultTabArrowsHeight;
            }
            set
            {
                if ((value == DefaultTabStripHeight) && (this.m_PagesStyle == RescoPageStyle.TabStrip))
                {
                    value = -1;
                }
                else if ((value == DefaultTabArrowsHeight) && ((this.m_PagesStyle == RescoPageStyle.Arrows) || (this.m_PagesStyle == RescoPageStyle.Dots)))
                {
                    value = -1;
                }
                this._pagerHeight = value;
                this.HideActiveControl();
                this.SetVScrollBar(this.m_iActualMaximumValue);
                base.Invalidate();
            }
        }

        [DefaultValue((string) null), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public PageCollection Pages
        {
            get
            {
                if (this.m_ItemCollection == null)
                {
                    return null;
                }
                if (this.m_pages == null)
                {
                    int iFirst = 0;
                    int count = this.m_ItemCollection.Count;
                    ArrayList list = new ArrayList();
                    for (int i = 0; i < count; i++)
                    {
                        if (this.m_ItemCollection[i] is ItemPageBreak)
                        {
                            list.Add(new Page(this, iFirst, i));
                            iFirst = i + 1;
                        }
                    }
                    if (list.Count < 1)
                    {
                        list.Add(new Page(this, 0, -1));
                    }
                    this.m_pages = new PageCollection((Page[]) list.ToArray(typeof(Page)));
                    if (this.CurrentPageIndex > this.m_pages.Count)
                    {
                        this.CurrentPageIndex = this.m_pages.Count - 1;
                    }
                    if (this.m_StartDrawPage > this.m_pages.Count)
                    {
                        this.m_StartDrawPage = this.m_pages.Count - 1;
                    }
                }
                return this.m_pages;
            }
        }

        public RescoPagesLocation PagesLocation
        {
            get
            {
                return this.m_pagesLocation;
            }
            set
            {
                if (this.m_pagesLocation != value)
                {
                    this.m_pagesLocation = value;
                    this.SetVScrollBar(this.m_iActualMaximumValue);
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(false)]
        public bool PagesRightToLeft
        {
            get
            {
                return this.m_pagesRightToLeft;
            }
            set
            {
                if (value != this.m_pagesRightToLeft)
                {
                    this.m_pagesRightToLeft = value;
                    base.Invalidate();
                }
            }
        }

        public RescoPageStyle PagingStyle
        {
            get
            {
                return this.m_PagesStyle;
            }
            set
            {
                if (this.m_PagesStyle != value)
                {
                    this.m_PagesStyle = value;
                    this.HideActiveControl();
                    this.SetVScrollBar(this.m_iActualMaximumValue);
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(13)]
        public int ScrollbarWidth
        {
            get
            {
                return this.m_VScrollBarWidth;
            }
            set
            {
                if (this.m_VScrollBarWidth != value)
                {
                    this.m_VScrollBarWidth = value;
                    this.m_vScroll.Width = value;
                    this.m_vScroll.Left = (this.CalculateClientRect().Y + this.CalculateClientRect().Width) - value;
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false)]
        public int ScrollOffset
        {
            get
            {
                if (this.m_bScrollVisible && (this.m_vScroll != null))
                {
                    return this.m_vScroll.Value;
                }
                return 0;
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false)]
        public bool ScrollVisible
        {
            get
            {
                return this.m_bScrollVisible;
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), DefaultValue((string) null)]
        public Item SelectedItem
        {
            get
            {
                return this.m_SelectedItem;
            }
            set
            {
                if (this.m_SelectedItem != value)
                {
                    if (this.m_SelectedItem != null)
                    {
                        bool focused = this.m_SelectedItem.Focused;
                        this.m_SelectedItem._Hide();
                        if (focused)
                        {
                            base.Focus();
                        }
                        base.Invalidate();
                    }
                    this.m_SelectedItem = value;
                    if (this.m_SelectedItem != null)
                    {
                        if (!this.CurrentPage.Contains(this.m_SelectedItem))
                        {
                            int pageCount = this.PageCount;
                            for (int i = 0; i < pageCount; i++)
                            {
                                if (this.Pages[i].Contains(this.m_SelectedItem))
                                {
                                    this.CurrentPageIndex = i;
                                    break;
                                }
                            }
                        }
                        this.m_SelectedItem.Activate(this.UseClickVisualize);
                    }
                }
            }
        }

        [DefaultValue(8)]
        public virtual int SeparatorWidth
        {
            get
            {
                return this.m_SeparatorWidth;
            }
            set
            {
                this.m_SeparatorWidth = value;
                base.Invalidate();
            }
        }

        [DefaultValue(false)]
        public bool SplitArrows
        {
            get
            {
                return this.m_splitArrows;
            }
            set
            {
                if (value != this.m_splitArrows)
                {
                    this.m_splitArrows = value;
                    base.Invalidate();
                }
            }
        }

        private uint TickCount
        {
            get
            {
                try
                {
                    return (uint) Environment.TickCount;
                }
                catch
                {
                    return (uint) (DateTime.Now.Ticks / 0x2710L);
                }
            }
        }

        public Resco.Controls.DetailView.TouchScrollDirection TouchPagesDirection
        {
            get
            {
                return this.m_touchPagesDirection;
            }
            set
            {
                this.m_touchPagesDirection = value;
            }
        }

        public Resco.Controls.DetailView.TouchScrollDirection TouchScrollDirection
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

        [DefaultValue(0x10)]
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

        [Resco.Controls.DetailView.Design.Browsable(false)]
        public int UsableWidth
        {
            get
            {
                return (this.CalculateClientRect().Width - (this.m_bScrollVisible ? this.m_VScrollBarWidth : 0));
            }
        }

        [DefaultValue(false)]
        public bool UseClickVisualize
        {
            get
            {
                return this.m_useClickVisualize;
            }
            set
            {
                if (this.m_useClickVisualize != value)
                {
                    this.m_useClickVisualize = value;
                }
            }
        }

        [DefaultValue(false)]
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
                    base.Invalidate();
                }
            }
        }

        [DefaultValue((string) null)]
        public Control VScrollBar
        {
            get
            {
                return this.m_vScrollBarResco;
            }
            set
            {
                if (this.m_vScrollBarResco != value)
                {
                    this.ScrollTo(0);
                    this.m_vScrollBarResco = value;
                    if (this.m_vScroll != null)
                    {
                        this.m_vScroll.Detach();
                        this.m_vScroll.ValueChanged -= new EventHandler(this.OnValueChanged);
                        this.m_vScroll.Resize -= new EventHandler(this.OnScrollResize);
                    }
                    this.m_vScroll = null;
                    this.SetVScrollBar(this.m_iActualMaximumValue);
                }
            }
        }

        internal class _ItemBoundsHelper
        {
            public int index;
            public int width;

            public _ItemBoundsHelper(int i, int w)
            {
                this.index = i;
                this.width = w;
            }
        }

        public delegate object DesignTimeCallback(int mode, object o);

        public class ItemBindingException : Exception
        {
            public object BoundValue;
            public Resco.Controls.DetailView.Item Item;
            public Resco.Controls.DetailView.DetailView.ItemBindingException Next;

            internal ItemBindingException(Resco.Controls.DetailView.Item item, object value, Exception ex, Resco.Controls.DetailView.DetailView.ItemBindingException next) : base("Error binding item " + item.Name, ex)
            {
                this.Next = next;
                this.BoundValue = value;
            }
        }
    }
}

