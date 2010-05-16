namespace Resco.Controls.CommonControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;
    using TouchScrolling;
    using System.Collections.Generic;

    internal class BigHas
    {
        // Fields
        internal static Dictionary<string, int> methodxxx;
    }
    public class ToolbarControl : UserControl
    {
        private Bitmap _backBufferBitmap;
        private Graphics _backBufferGraphics;
        internal static Pen _cachedPen;
        private DesignTimeCallback _designTimeCallback;
        internal static Bitmap _fakePixel;
        private IContainer components;
        private byte m_ArrowsTransparency = 150;
        private Bitmap m_bmpArrowNext;
        private Bitmap m_bmpArrowNextVGA;
        private Bitmap m_bmpArrowPrevious;
        private Bitmap m_bmpArrowPreviousVGA;
        private Bitmap m_bmpBackground;
        private ToolbarConversion m_Conversion;
        private ToolbarDockType m_DockType;
        private bool m_EnableArrowsTransparency = true;
        private int m_FocusedItemIndex;
        private bool m_IsDisposed;
        private Alignment m_ItemsAlignment = Alignment.TopCenter;
        private AbstractToolbarRenderer m_Renderer;
        internal static SizeF m_ScaleFactor;
        private int m_ScrollBarValue;
        private bool m_SelectBeforeScrolling = true;
        private int m_SelectedIndex = -1;
        private bool m_StretchBackgroundImage;
        private ToolbarItemCollection m_ToolbarItems = new ToolbarItemCollection();
        private int m_TouchBgrdHeight;
        private int m_TouchBgrdWidth;
        private TouchTool m_TouchNavigatorTool;
        private bool m_TouchScrollNeeded = true;

        public event EventHandler FocusChanged;

        public event EventHandler ItemEntered;

        public event EventHandler SelectionChanged;

        static ToolbarControl()
        {
            if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            {
                //RescoLicenseMessage.ShowEvaluationMessage(typeof(ToolbarControl), "");
            }
        }

        public ToolbarControl()
        {
            this.InitializeComponent();
            this.m_Renderer = new HorizontalToolbarRenderer(this);
            this.EnableEvents();
            this.InitTouchTool();
            float num = base.CurrentAutoScaleDimensions.Height / 96f;
            ToolbarItem.m_VgaOffset = (num > 1f) ? 2 : 1;
        }

        public void Click(Point pt)
        {
            this.MouseClickDown(pt);
        }

        private Graphics CreateBackBuffer()
        {
            if (((this._backBufferBitmap == null) || (this._backBufferBitmap.Width != base.Width)) || (this._backBufferBitmap.Height != base.Height))
            {
                if (this._backBufferBitmap != null)
                {
                    this._backBufferBitmap.Dispose();
                    this._backBufferBitmap = null;
                    if (this._backBufferGraphics != null)
                    {
                        this._backBufferGraphics.Dispose();
                        this._backBufferGraphics = null;
                    }
                }
                this._backBufferBitmap = new Bitmap(base.Width, base.Height);
            }
            if (this._backBufferGraphics == null)
            {
                this._backBufferGraphics = Graphics.FromImage(this._backBufferBitmap);
            }
            else if (this._backBufferGraphics == null)
            {
                this._backBufferGraphics.Dispose();
                this._backBufferGraphics = null;
            }
            return this._backBufferGraphics;
        }

        private void DeinitTouchTool()
        {
            if (this.m_TouchNavigatorTool != null)
            {
                this.m_TouchNavigatorTool.MouseMoveDetected -= new TouchTool.MouseMoveDetectedHandler(this.TouchNavigatorTool_MouseMoveDetected);
                this.m_TouchNavigatorTool.ParentControl = null;
                this.m_TouchNavigatorTool.Dispose();
                this.m_TouchNavigatorTool = null;
            }
        }

        private void DisableEvents()
        {
            base.KeyDown -= new KeyEventHandler(this.ToolbarControl_KeyDown);
            this.Items.RefreshRequired -= new RefreshRequiredEventHandler(this.ToolbarItems_RefreshRequired);
        }

        protected override void Dispose(bool disposing)
        {
            this.m_IsDisposed = true;
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
                if (this._backBufferGraphics != null)
                {
                    this._backBufferGraphics.Dispose();
                }
                if (this._backBufferBitmap != null)
                {
                    this._backBufferBitmap.Dispose();
                }
                if (_cachedPen != null)
                {
                    _cachedPen.Dispose();
                }
                if (_fakePixel != null)
                {
                    _fakePixel.Dispose();
                }
            }
            this.components = null;
            this._backBufferGraphics = null;
            this._backBufferBitmap = null;
            _cachedPen = null;
            _fakePixel = null;
            base.Dispose(disposing);
        }

        private void Draw(Graphics gr, Rectangle aRect, int aScrollBarValue)
        {
            this.m_Renderer.ToolbarItems = this.m_ToolbarItems;
            this.m_Renderer.Alignment = this.m_ItemsAlignment;
            Rectangle rectangle = this.m_Renderer.Draw(gr, aRect, aScrollBarValue);
            this.m_TouchBgrdWidth = rectangle.Width;
            this.m_TouchBgrdHeight = rectangle.Height;
            this.UpdateTouchScrollTool();
        }

        private void DrawBackgroundImage(Graphics gr)
        {
            if (this.m_bmpBackground != null)
            {
                if (this.m_StretchBackgroundImage)
                {
                    Rectangle srcRect = new Rectangle(0, 0, this.m_bmpBackground.Width, this.m_bmpBackground.Height);
                    gr.DrawImage(this.m_bmpBackground, base.ClientRectangle, srcRect, GraphicsUnit.Pixel);
                }
                else
                {
                    TextureBrush brush = new TextureBrush(this.m_bmpBackground);
                    gr.FillRectangle(brush, base.ClientRectangle);
                    brush.Dispose();
                    brush = null;
                }
            }
        }

        internal static void DrawPixel(Graphics gr, Color c, int x, int y)
        {
            if (_fakePixel == null)
            {
                _fakePixel = new Bitmap(1, 1);
            }
            _fakePixel.SetPixel(0, 0, c);
            gr.DrawImage(_fakePixel, x, y);
        }

        private void DrawScrollbar(Graphics gr)
        {
            if ((this.m_bmpArrowPrevious != null) && this.IsAbleScroollToPrevious())
            {
                Bitmap bmpArrowPrevious = this.m_bmpArrowPrevious;
                if (!m_ScaleFactor.Equals(new SizeF(1f, 1f)))
                {
                    if (this.m_bmpArrowPreviousVGA == null)
                    {
                        Size size = new Size((int) (this.m_bmpArrowPrevious.Width * m_ScaleFactor.Width), (int) (this.m_bmpArrowPrevious.Height * m_ScaleFactor.Height));
                        this.m_bmpArrowPreviousVGA = DrawingHelper.ResizeBitmap(this.m_bmpArrowPrevious, size);
                    }
                    bmpArrowPrevious = this.m_bmpArrowPreviousVGA;
                }
                if ((Environment.OSVersion.Platform == PlatformID.WinCE) && this.m_EnableArrowsTransparency)
                {
                    DrawingHelper.DrawAlpha(gr, bmpArrowPrevious, this.m_ArrowsTransparency, 0, 0);
                }
                else
                {
                    gr.DrawImage(bmpArrowPrevious, 0, 0);
                }
            }
            if ((this.m_bmpArrowNext != null) && this.IsAbleScroollToNext())
            {
                int x = 0;
                int y = 0;
                Bitmap bmpArrowNext = this.m_bmpArrowNext;
                if (!m_ScaleFactor.Equals(new SizeF(1f, 1f)))
                {
                    if (this.m_bmpArrowNextVGA == null)
                    {
                        Size size2 = new Size((int) (this.m_bmpArrowNext.Width * m_ScaleFactor.Width), (int) (this.m_bmpArrowNext.Height * m_ScaleFactor.Height));
                        this.m_bmpArrowNextVGA = DrawingHelper.ResizeBitmap(this.m_bmpArrowNext, size2);
                    }
                    bmpArrowNext = this.m_bmpArrowNextVGA;
                }
                if (this.m_DockType == ToolbarDockType.Horizontal)
                {
                    x = base.Width - bmpArrowNext.Width;
                }
                else
                {
                    y = base.Height - bmpArrowNext.Height;
                }
                if (this.m_EnableArrowsTransparency)
                {
                    DrawingHelper.DrawAlpha(gr, bmpArrowNext, this.m_ArrowsTransparency, x, y);
                }
                else
                {
                    gr.DrawImage(bmpArrowNext, x, y);
                }
            }
        }

        private void DrawToolbarControl(Graphics gr, Rectangle aRect)
        {
            if (this.m_bmpBackground == null)
            {
                gr.Clear(this.BackColor);
            }
            if (this.m_ToolbarItems != null)
            {
                this.Draw(gr, aRect, this.m_ScrollBarValue);
            }
        }

        private void EnableEvents()
        {
            base.KeyDown += new KeyEventHandler(this.ToolbarControl_KeyDown);
            this.Items.RefreshRequired += new RefreshRequiredEventHandler(this.ToolbarItems_RefreshRequired);
        }

        private void EnsureVisibleSelectedItem()
        {
            if ((this.m_SelectedIndex >= 0) && (this.m_SelectedIndex < this.m_ToolbarItems.Count))
            {
                ToolbarItem item = this.m_ToolbarItems[this.m_SelectedIndex];
                Rectangle clientRectangle = item.ClientRectangle;
                if (this.m_DockType == ToolbarDockType.Horizontal)
                {
                    clientRectangle.X += this.m_ScrollBarValue - this.MarginAtBeginEx;
                    if ((clientRectangle.X < this.m_ScrollBarValue) || (clientRectangle.Right > (this.m_ScrollBarValue + base.Width)))
                    {
                        if (clientRectangle.Right > (this.m_ScrollBarValue + base.Width))
                        {
                            clientRectangle.X = ((clientRectangle.Right - base.Width) + this.MarginAtBeginEx) + this.MarginAtEnd;
                        }
                        this.OnScrollShiftHorizontal(clientRectangle.X);
                    }
                }
                else
                {
                    clientRectangle.Y += this.m_ScrollBarValue - this.MarginAtBeginEx;
                    if ((clientRectangle.Y < this.m_ScrollBarValue) || (clientRectangle.Bottom > (this.m_ScrollBarValue + base.Height)))
                    {
                        if (clientRectangle.Bottom > (this.m_ScrollBarValue + base.Height))
                        {
                            clientRectangle.Y -= (((clientRectangle.Right - this.m_ScrollBarValue) - base.Height) + this.MarginAtBeginEx) + this.MarginAtEnd;
                        }
                        this.OnScrollShiftVertical(clientRectangle.Y);
                    }
                }
            }
        }

        internal static Pen GetPen(Color c)
        {
            if (_cachedPen == null)
            {
                _cachedPen = new Pen(c);
            }
            else
            {
                _cachedPen.Color = c;
            }
            return _cachedPen;
        }

        private void HandleHorizontalBarKeys(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    this.SelectPreviouseItem();
                    return;

                case Keys.Up:
                    base.Parent.SelectNextControl(this, false, true, false, true);
                    return;

                case Keys.Right:
                    this.SelectNextItem();
                    return;

                case Keys.Down:
                    base.Parent.SelectNextControl(this, true, true, false, true);
                    return;
            }
        }

        private void HandleVerticalBarKeys(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    base.Parent.SelectNextControl(this, false, true, false, true);
                    return;

                case Keys.Up:
                    this.SelectPreviouseItem();
                    return;

                case Keys.Right:
                    base.Parent.SelectNextControl(this, true, true, false, true);
                    return;

                case Keys.Down:
                    this.SelectNextItem();
                    return;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            base.AutoScaleMode = AutoScaleMode.Dpi;// .set_AutoScaleMode(2);
        }

        private void InitTouchTool()
        {
            if (this.m_TouchNavigatorTool == null)
            {
                this.m_TouchNavigatorTool = new TouchTool(this);
                this.m_TouchNavigatorTool.MouseMoveDetected += new TouchTool.MouseMoveDetectedHandler(this.TouchNavigatorTool_MouseMoveDetected);
            }
        }

        private bool IsAbleScroollToNext()
        {
            if (this.m_DockType == ToolbarDockType.Horizontal)
            {
                return (this.m_ScrollBarValue < ((this.m_TouchBgrdWidth - base.Width) + this.MarginAtEnd));
            }
            return (this.m_ScrollBarValue < ((this.m_TouchBgrdHeight - base.Height) + this.MarginAtEnd));
        }

        private bool IsAbleScroollToPrevious()
        {
            return (this.m_ScrollBarValue > -this.MarginAtBeginEx);
        }

        public void ItemEnter(int anItemIndex)
        {
            this.SelectedIndex = anItemIndex;
            this.OnItemEntered();
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
                base.SuspendLayout();
                this.BackgroundImage = null;
                this.BmpArrowNext = null;
                this.BmpArrowPrevious = null;
                while (reader.Read())
                {
                    string str2;
                    if (((str2 = reader.Name) != null) && (str2 == "ToolbarControl"))
                    {
                        this.ReadToolbarControl(reader);
                    }
                }
            }
            finally
            {
                base.ResumeLayout();
                base.Invalidate();
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

        internal void MouseClickDown(Point pt)
        {
            if ((this.m_bmpArrowPrevious != null) && this.IsAbleScroollToPrevious())
            {
                Bitmap bmpArrowPrevious = this.m_bmpArrowPrevious;
                if (!m_ScaleFactor.Equals(new SizeF(1f, 1f)))
                {
                    bmpArrowPrevious = this.m_bmpArrowPreviousVGA;
                }
                if (bmpArrowPrevious != null)
                {
                    Rectangle rectangle = new Rectangle(0, 0, bmpArrowPrevious.Width, bmpArrowPrevious.Height);
                    if (rectangle.Contains(pt))
                    {
                        this.m_ScrollBarValue -= (this.m_DockType == ToolbarDockType.Horizontal) ? base.Width : base.Height;
                        if (this.m_ScrollBarValue < -this.MarginAtBeginEx)
                        {
                            this.m_ScrollBarValue = -this.MarginAtBeginEx;
                        }
                        this.Refresh();
                        return;
                    }
                }
            }
            if ((this.m_bmpArrowNext != null) && this.IsAbleScroollToNext())
            {
                Bitmap bmpArrowNext = this.m_bmpArrowNext;
                if (!m_ScaleFactor.Equals(new SizeF(1f, 1f)))
                {
                    bmpArrowNext = this.m_bmpArrowNextVGA;
                }
                if (bmpArrowNext != null)
                {
                    int x = 0;
                    int y = 0;
                    if (this.m_DockType == ToolbarDockType.Horizontal)
                    {
                        x = base.Width - bmpArrowNext.Width;
                    }
                    else
                    {
                        y = base.Height - bmpArrowNext.Height;
                    }
                    Rectangle rectangle2 = new Rectangle(x, y, bmpArrowNext.Width, bmpArrowNext.Height);
                    if (rectangle2.Contains(pt))
                    {
                        int num3 = (this.m_DockType == ToolbarDockType.Horizontal) ? base.Width : base.Height;
                        this.m_ScrollBarValue += num3;
                        if (this.m_ScrollBarValue > ((this.m_TouchBgrdWidth - num3) + this.MarginAtEnd))
                        {
                            this.m_ScrollBarValue = (this.m_TouchBgrdWidth - num3) + this.MarginAtEnd;
                        }
                        this.Refresh();
                        return;
                    }
                }
            }
            ToolbarItem item = null;
            int num4 = -1;
            int count = this.m_ToolbarItems.Count;
            for (int i = 0; i < count; i++)
            {
                ToolbarItem item2 = this.m_ToolbarItems[i];
                if ((item2 != null) && item2.Visible)
                {
                    item2.Pressed = false;
                    if ((item == null) && item2.ClientRectangle.Contains(pt))
                    {
                        item = item2;
                        num4 = i;
                    }
                }
            }
            if ((((num4 >= 0) && (num4 < this.m_ToolbarItems.Count)) && ((num4 != this.m_SelectedIndex) && (item.ToolbarItemBehavior != ToolbarItemBehaviorType.Separator))) && item.Enabled)
            {
                item.Pressed = true;
                this.SelectedIndex = num4;
            }
            else if ((this.m_SelectedIndex >= 0) && (this.m_SelectedIndex < this.m_ToolbarItems.Count))
            {
                this.m_ToolbarItems[this.m_SelectedIndex].Pressed = true;
            }
        }

        internal void MouseClickDown(MouseEventArgs e)
        {
            this.MouseClickDown(new Point(e.X, e.Y));
        }

        internal void MouseClickUp(MouseEventArgs e)
        {
            if ((this.m_SelectedIndex >= 0) && (this.m_SelectedIndex < this.m_ToolbarItems.Count))
            {
                ToolbarItem item = this.m_ToolbarItems[this.m_SelectedIndex];
                Point pt = new Point(e.X, e.Y);
                if (item != null)
                {
                    if (item.ClientRectangle.Contains(pt))
                    {
                        this.OnItemEntered();
                    }
                    if (item.ToolbarItemBehavior == ToolbarItemBehaviorType.UnselectAfterClick)
                    {
                        this.m_SelectedIndex = -1;
                        item.Pressed = false;
                        base.Invalidate();
                    }
                }
            }
        }

        private void OnDockTypeChanged()
        {
            if (this.m_DockType == ToolbarDockType.Horizontal)
            {
                this.m_Renderer = new HorizontalToolbarRenderer(this);
            }
            else
            {
                this.m_Renderer = new VerticalToolbarRenderer(this);
            }
            base.Invalidate();
        }

        private void OnFocusChanged()
        {
            if (this.FocusChanged != null)
            {
                this.FocusChanged(this, EventArgs.Empty);
            }
        }

        private void OnItemEntered()
        {
            if (this.ItemEntered != null)
            {
                this.ItemEntered(this, EventArgs.Empty);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                base.Capture = true;
                if (this.m_SelectBeforeScrolling)
                {
                    this.MouseClickDown(e);
                }
                if (this.m_TouchScrollNeeded)
                {
                    this.m_TouchNavigatorTool.MouseDown(e.X, e.Y);
                }
                this.Refresh();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && this.m_TouchScrollNeeded)
            {
                this.m_TouchNavigatorTool.MouseMove(e.X, e.Y);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                base.Capture = false;
                bool flag = false;
                if (this.m_TouchScrollNeeded)
                {
                    flag = this.m_TouchNavigatorTool.MouseUp(e.X, e.Y);
                }
                if (!flag)
                {
                    if (!this.m_SelectBeforeScrolling)
                    {
                        this.MouseClickDown(e);
                    }
                    this.MouseClickUp(e);
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gr = this.CreateBackBuffer();
            if ((gr == null) || ((this.Site != null) && this.Site.DesignMode))
            {
                gr = e.Graphics;
            }
            this.DrawBackgroundImage(gr);
            this.DrawToolbarControl(gr, base.ClientRectangle);
            this.DrawScrollbar(gr);
            if (gr != e.Graphics)
            {
                e.Graphics.DrawImage(this._backBufferBitmap, 0, 0);
            }
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        private void OnScrollShiftHorizontal(int value)
        {
            int num = value;
            if ((num < -this.MarginAtBeginEx) || (this.m_TouchBgrdWidth < base.ClientRectangle.Width))
            {
                this.m_ScrollBarValue = -this.MarginAtBeginEx;
            }
            else if (num > ((this.m_TouchBgrdWidth - base.ClientRectangle.Width) + this.MarginAtEnd))
            {
                this.m_ScrollBarValue = (this.m_TouchBgrdWidth - base.ClientRectangle.Width) + this.MarginAtEnd;
            }
            else
            {
                this.m_ScrollBarValue = num;
            }
        }

        private void OnScrollShiftVertical(int value)
        {
            int num = value;
            if ((num < -this.MarginAtBeginEx) || (this.m_TouchBgrdHeight < base.ClientRectangle.Height))
            {
                this.m_ScrollBarValue = -this.MarginAtBeginEx;
            }
            else if (num > ((this.m_TouchBgrdHeight - base.ClientRectangle.Height) + this.MarginAtEnd))
            {
                this.m_ScrollBarValue = (this.m_TouchBgrdHeight - base.ClientRectangle.Height) + this.MarginAtEnd;
            }
            else
            {
                this.m_ScrollBarValue = num;
            }
        }

        private void OnSelectedIndexChanged()
        {
            int count = this.m_ToolbarItems.Count;
            for (int i = 0; i < count; i++)
            {
                ToolbarItem item = this.m_ToolbarItems[i];
                if (i == this.m_SelectedIndex)
                {
                    item.Pressed = true;
                }
                else
                {
                    item.Pressed = false;
                }
            }
            this.EnsureVisibleSelectedItem();
            this.Refresh();
            this.OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, EventArgs.Empty);
            }
        }

        private void ReadImage(ToolbarControl aControl, XmlReader reader)
        {
            try
            {
                if (reader.Name == "Image")
                {
                    Bitmap bitmap = ToolbarConversion.ImageFromString(reader["Data"]);
                    this.m_Conversion.SetProperty(aControl, reader["Name"], bitmap);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ReadImage(ToolbarItem anItem, XmlReader reader)
        {
            try
            {
                if (reader.Name == "Image")
                {
                    Bitmap bitmap = ToolbarConversion.ImageFromString(reader["Data"]);
                    this.m_Conversion.SetProperty(anItem, reader["Name"], bitmap);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ReadToolbarControl(XmlReader reader)
        {
            this.m_Conversion = new ToolbarConversion(this.Site, this._designTimeCallback);
            try
            {
                if (reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                    {
                        try
                        {
                            this.m_Conversion.SetProperty(this, reader.Name, reader.Value);
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
                    this._designTimeCallback(this.m_ToolbarItems, null);
                }
                this.m_ToolbarItems.Clear();
                if (!reader.IsEmptyElement)
                {
                    goto Label_016F;
                }
                return;
            Label_0082:
                try
                {
                    string name = reader.Name;
                    if (name != null)
                    {
                        int num;
                       if (BigHas.methodxxx.TryGetValue(name, out num))// if (<PrivateImplementationDetails>{5BFC582E-3A05-4130-8F3B-BAA3770C919A}.$$method0x60000b9-1.TryGetValue(name, ref num))
                        {
                            switch (num)
                            {
                                case 0:
                                    goto Label_016F;

                                case 1:
                                    this.m_ToolbarItems.Add(this.ReadToolbarItem(reader));
                                    goto Label_016F;

                                case 2:
                                    this.ReadImage(this, reader);
                                    goto Label_016F;

                                case 3:
                                    return;

                                case 4:
                                    this.m_Conversion.SetProperty(this, reader["Name"], reader["Value"]);
                                    goto Label_016F;
                            }
                        }
                        this.m_Conversion.SetProperty(this, reader.Name, reader.ReadString());
                    }
                }
                catch
                {
                }
            Label_016F:
                if (reader.Read())
                {
                    goto Label_0082;
                }
                base.Update();
            }
            catch
            {
            }
            finally
            {
                this.m_Conversion = null;
            }
        }

        private ToolbarItem ReadToolbarItem(XmlReader reader)
        {
            ToolbarItem o = null;
            try
            {
                string str = reader["Name"];
                o = new ToolbarItem();
                if (this._designTimeCallback != null)
                {
                    this._designTimeCallback(o, null);
                }
                o.Name = str;
                if (!reader.IsEmptyElement)
                {
                    goto Label_00CF;
                }
                return o;
            Label_0042:
                try
                {
                    string str2;
                    if (((str2 = reader.Name) == null) || (str2 == ""))
                    {
                        goto Label_00CF;
                    }
                    if (!(str2 == "ToolbarItem"))
                    {
                        if (str2 == "Image")
                        {
                            goto Label_009E;
                        }
                        if (str2 == "Property")
                        {
                            goto Label_00A8;
                        }
                        goto Label_00CF;
                    }
                    if (this._designTimeCallback != null)
                    {
                        this._designTimeCallback(o, null);
                    }
                    return o;
                Label_009E:
                    this.ReadImage(o, reader);
                    goto Label_00CF;
                Label_00A8:
                    this.m_Conversion.SetProperty(o, reader["Name"], reader["Value"]);
                }
                catch
                {
                }
            Label_00CF:
                if (reader.Read())
                {
                    goto Label_0042;
                }
            }
            catch
            {
                try
                {
                    if (((reader.Name == "ToolbarItem") && reader.IsStartElement()) && !reader.IsEmptyElement)
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "ToolbarItem")
                            {
                                goto Label_0122;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        Label_0122:
            if (o != null)
            {
                return o;
            }
            return new ToolbarItem();
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            m_ScaleFactor = factor;
            base.ScaleControl(factor, specified);
        }

        private void SelectNextItem()
        {
            for (int i = this.SelectedIndex + 1; i < this.m_ToolbarItems.Count; i++)
            {
                if (this.m_ToolbarItems[i].Visible)
                {
                    this.SelectedIndex = i;
                    return;
                }
            }
        }

        private void SelectPreviouseItem()
        {
            for (int i = this.SelectedIndex - 1; i >= 0; i--)
            {
                if (this.m_ToolbarItems[i].Visible)
                {
                    this.SelectedIndex = i;
                    return;
                }
            }
        }

        protected virtual bool ShouldSerializeArrowsTransparency()
        {
            return (this.m_ArrowsTransparency != 150);
        }

        protected virtual bool ShouldSerializeBackgroundImage()
        {
            return (this.m_bmpBackground != null);
        }

        protected virtual bool ShouldSerializeBmpArrowNext()
        {
            return (this.m_bmpArrowNext != null);
        }

        protected virtual bool ShouldSerializeBmpArrowPrevious()
        {
            return (this.m_bmpArrowPrevious != null);
        }

        protected virtual bool ShouldSerializeDockType()
        {
            return (this.m_DockType != ToolbarDockType.Horizontal);
        }

        protected virtual bool ShouldSerializeEnableArrowsTransparency()
        {
            return !this.m_EnableArrowsTransparency;
        }

        protected virtual bool ShouldSerializeItems()
        {
            return ((this.m_ToolbarItems != null) && (this.m_ToolbarItems.Count != 0));
        }

        protected virtual bool ShouldSerializeItemsAlignment()
        {
            return (this.m_ItemsAlignment != Alignment.TopCenter);
        }

        protected virtual bool ShouldSerializeMarginAtBegin()
        {
            return (this.m_Renderer.MarginAtBegin != 0);
        }

        protected virtual bool ShouldSerializeMarginAtEnd()
        {
            return (this.m_Renderer.MarginAtEnd != 0);
        }

        protected virtual bool ShouldSerializeSelectedIndex()
        {
            return true;
        }

        protected virtual bool ShouldSerializeStretchBackgroundImage()
        {
            return this.m_StretchBackgroundImage;
        }

        internal void TestScrolling()
        {
            int num = this.m_TouchBgrdWidth - base.Width;
            while (this.m_ScrollBarValue < num)
            {
                this.Refresh();
                this.m_ScrollBarValue += 5;
            }
        }

        private void ToolbarControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.m_DockType == ToolbarDockType.Vertical)
            {
                this.HandleVerticalBarKeys(e);
            }
            else
            {
                this.HandleHorizontalBarKeys(e);
            }
            if (e.KeyCode == Keys.Return)
            {
                this.OnItemEntered();
            }
        }

        private void ToolbarItems_RefreshRequired(object sender, ToolbarItemEventArgs e)
        {
            base.Invalidate();
        }

        private void TouchNavigatorTool_GestureDetected(object sender, TouchTool.GestureEventArgs e)
        {
            switch (e.Gesture)
            {
            }
        }

        private void TouchNavigatorTool_MouseMoveDetected(object sender, TouchTool.MouseMoveEventArgs e)
        {
            if (this.m_DockType == ToolbarDockType.Horizontal)
            {
                this.ScrollShift += e.MoveX;
            }
            else
            {
                this.ScrollShift += e.MoveY;
            }
            this.Refresh();
        }

        private void UpdateTouchScrollTool()
        {
            if (this.m_TouchNavigatorTool.EnableTouchScrolling)
            {
                if (((this.m_DockType == ToolbarDockType.Horizontal) && (this.m_TouchBgrdWidth <= base.Width)) || ((this.m_DockType == ToolbarDockType.Vertical) && (this.m_TouchBgrdHeight <= base.Height)))
                {
                    this.m_TouchScrollNeeded = false;
                }
                else
                {
                    this.m_TouchScrollNeeded = true;
                }
            }
            else
            {
                this.m_TouchScrollNeeded = false;
            }
        }

        public byte ArrowsTransparency
        {
            get
            {
                return this.m_ArrowsTransparency;
            }
            set
            {
                this.m_ArrowsTransparency = value;
                base.Invalidate();
            }
        }

        public Bitmap BackgroundImage
        {
            get
            {
                return this.m_bmpBackground;
            }
            set
            {
                if (this.m_bmpBackground != value)
                {
                    this.m_bmpBackground = value;
                    base.Invalidate();
                }
            }
        }

        public Bitmap BmpArrowNext
        {
            get
            {
                return this.m_bmpArrowNext;
            }
            set
            {
                this.m_bmpArrowNext = value;
                if (this.m_bmpArrowNextVGA != null)
                {
                    this.m_bmpArrowNextVGA.Dispose();
                    this.m_bmpArrowNextVGA = null;
                }
                base.Invalidate();
            }
        }

        public Bitmap BmpArrowPrevious
        {
            get
            {
                return this.m_bmpArrowPrevious;
            }
            set
            {
                this.m_bmpArrowPrevious = value;
                if (this.m_bmpArrowPreviousVGA != null)
                {
                    this.m_bmpArrowPreviousVGA.Dispose();
                    this.m_bmpArrowPreviousVGA = null;
                }
                base.Invalidate();
            }
        }

        public ToolbarDockType DockType
        {
            get
            {
                return this.m_DockType;
            }
            set
            {
                if (this.m_DockType != value)
                {
                    this.m_DockType = value;
                    this.OnDockTypeChanged();
                }
            }
        }

        public bool EnableArrowsTransparency
        {
            get
            {
                return this.m_EnableArrowsTransparency;
            }
            set
            {
                this.m_EnableArrowsTransparency = value;
                base.Invalidate();
            }
        }

        public bool EnableTouchScrolling
        {
            get
            {
                return ((this.m_TouchNavigatorTool != null) && this.m_TouchNavigatorTool.EnableTouchScrolling);
            }
            set
            {
                if (this.m_TouchNavigatorTool != null)
                {
                    this.m_TouchNavigatorTool.EnableTouchScrolling = value;
                }
            }
        }

        internal bool IsDisposed
        {
            get
            {
                return this.m_IsDisposed;
            }
        }

        public ToolbarItemCollection Items
        {
            get
            {
                return this.m_ToolbarItems;
            }
            set
            {
                this.m_ToolbarItems = value;
            }
        }

        public Alignment ItemsAlignment
        {
            get
            {
                return this.m_ItemsAlignment;
            }
            set
            {
                this.m_ItemsAlignment = value;
                base.Invalidate();
            }
        }

        public int MarginAtBegin
        {
            get
            {
                return this.m_Renderer.MarginAtBegin;
            }
            set
            {
                this.m_Renderer.MarginAtBegin = value;
                this.m_ScrollBarValue = -this.MarginAtBeginEx;
                base.Invalidate();
            }
        }

        internal int MarginAtBeginEx
        {
            get
            {
                if ((this.m_DockType == ToolbarDockType.Horizontal) && (this.m_TouchBgrdWidth < base.Width))
                {
                    return 0;
                }
                if ((this.m_DockType == ToolbarDockType.Vertical) && (this.m_TouchBgrdHeight < base.Height))
                {
                    return 0;
                }
                return this.m_Renderer.MarginAtBegin;
            }
        }

        public int MarginAtEnd
        {
            get
            {
                return this.m_Renderer.MarginAtEnd;
            }
            set
            {
                this.m_Renderer.MarginAtEnd = value;
                base.Invalidate();
            }
        }

        private int ScrollShift
        {
            get
            {
                return this.m_ScrollBarValue;
            }
            set
            {
                if (this.m_DockType == ToolbarDockType.Horizontal)
                {
                    this.OnScrollShiftHorizontal(value);
                }
                else
                {
                    this.OnScrollShiftVertical(value);
                }
            }
        }

        public bool SelectBeforeScrolling
        {
            get
            {
                return this.m_SelectBeforeScrolling;
            }
            set
            {
                this.m_SelectBeforeScrolling = value;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.m_SelectedIndex;
            }
            set
            {
                if (((value >= -1) && (value < this.m_ToolbarItems.Count)) && (this.m_SelectedIndex != value))
                {
                    this.m_SelectedIndex = value;
                    this.OnSelectedIndexChanged();
                }
            }
        }

        public bool StretchBackgroundImage
        {
            get
            {
                return this.m_StretchBackgroundImage;
            }
            set
            {
                this.m_StretchBackgroundImage = value;
                base.Invalidate();
            }
        }

        public int TouchSensitivity
        {
            get
            {
                if (this.m_TouchNavigatorTool != null)
                {
                    return this.m_TouchNavigatorTool.TouchSensitivity;
                }
                return 0;
            }
            set
            {
                if (this.m_TouchNavigatorTool != null)
                {
                    this.m_TouchNavigatorTool.TouchSensitivity = value;
                }
            }
        }

        public delegate void DesignTimeCallback(object o, object o2);
    }
}

