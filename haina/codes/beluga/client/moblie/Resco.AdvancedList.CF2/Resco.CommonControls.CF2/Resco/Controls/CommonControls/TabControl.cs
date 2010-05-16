namespace Resco.Controls.CommonControls
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;

    public class TabControl : Control
    {
        private DesignTimeCallback _designTimeCallback;
        private bool m_bInChange;
        private TabControlConversion m_Conversion;
        private TabsDockType m_DockType = TabsDockType.Top;
        private bool m_Initialization;
        private int m_selectedIndex;
        private TabPagesCollection m_tabPages;
        private Resco.Controls.CommonControls.ToolBar m_toolBar;

        public event EventHandler ItemEntered;

        public event EventHandler SelectedIndexChanged;

        static TabControl()
        {
            if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            {
                //RescoLicenseMessage.ShowEvaluationMessage(typeof(Resco.Controls.CommonControls.TabControl), "");
            }
        }

        public TabControl()
        {
            this.m_toolBar = new Resco.Controls.CommonControls.ToolBar(this);
            this.m_toolBar.ItemEntered += new EventHandler(this.ToolBar_ItemEntered);
            base.Controls.Add(this.m_toolBar);
            this.m_tabPages = new TabPagesCollection(this, this.m_toolBar.Items);
            this.m_Initialization = true;
        }

        public Rectangle GetClientArea()
        {
            return this.m_toolBar.Bounds;
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
                while (reader.Read())
                {
                    string str2;
                    if (((str2 = reader.Name) != null) && (str2 == "TabControl"))
                    {
                        this.ReadTabControl(reader);
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

        internal void OnChanged()
        {
            if (!this.m_bInChange)
            {
                this.m_bInChange = true;
                base.Invalidate();
                this.m_toolBar.Refresh();
                this.m_bInChange = false;
            }
        }

        private void OnDockTypeChanged()
        {
            if (this.m_DockType == TabsDockType.Top)
            {
                this.m_toolBar.Dock = DockStyle.Top;
                this.m_toolBar.DockType = ToolbarDockType.Horizontal;
                this.m_toolBar.Width = base.Width;
            }
            else if (this.m_DockType == TabsDockType.Bottom)
            {
                this.m_toolBar.Dock = DockStyle.Bottom;
                this.m_toolBar.DockType = ToolbarDockType.Horizontal;
                this.m_toolBar.Width = base.Width;
            }
            else if (this.m_DockType == TabsDockType.Left)
            {
                this.m_toolBar.Dock = DockStyle.Left;
                this.m_toolBar.DockType = ToolbarDockType.Vertical;
                this.m_toolBar.Height = base.Height;
            }
            else
            {
                this.m_toolBar.Dock = DockStyle.Right;
                this.m_toolBar.DockType = ToolbarDockType.Vertical;
                this.m_toolBar.Height = base.Height;
            }
            base.Invalidate();
        }

        private void OnItemEntered()
        {
            if (this.ItemEntered != null)
            {
                this.ItemEntered(this, EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.m_Initialization)
            {
                this.m_Initialization = false;
                int selectedIndex = this.m_selectedIndex;
                this.SetSelectedIndexWithoutEvent(ref selectedIndex);
                this.m_toolBar.SelectedIndex = this.m_selectedIndex;
            }
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Size size = base.Size;
            if (this.m_toolBar != null)
            {
                this.OnDockTypeChanged();
                this.m_toolBar.Invalidate();
                Size size2 = this.m_toolBar.Size;
            }
            if (this.SelectedTab != null)
            {
                this.SelectedTab.Dock = DockStyle.Fill;
                Size size3 = this.SelectedTab.Size;
            }
        }

        internal void OnSelectedIndexChanged(EventArgs e)
        {
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(this, e);
            }
        }

        private void ReadImage(Resco.Controls.CommonControls.TabControl aControl, XmlReader reader)
        {
            try
            {
                if (reader.Name == "Image")
                {
                    Bitmap bitmap = TabControlConversion.ImageFromString(reader["Data"]);
                    this.m_Conversion.SetProperty(aControl, reader["Name"], bitmap);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ReadImage(Resco.Controls.CommonControls.TabPage anItem, XmlReader reader)
        {
            try
            {
                if (reader.Name == "Image")
                {
                    Bitmap bitmap = TabControlConversion.ImageFromString(reader["Data"]);
                    this.m_Conversion.SetProperty(anItem, reader["Name"], bitmap);
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

        private void ReadTabControl(XmlReader reader)
        {
            this.m_Conversion = new TabControlConversion(this.Site, this._designTimeCallback);
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
                    this._designTimeCallback(this.m_tabPages, null);
                }
                this.m_tabPages.Clear();
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
                        if (BigHas.methodxxx.TryGetValue(name, out num))//if (<PrivateImplementationDetails>{5BFC582E-3A05-4130-8F3B-BAA3770C919A}.$$method0x6000126-1.TryGetValue(name, ref num))
                        {
                            switch (num)
                            {
                                case 0:
                                    goto Label_016F;

                                case 1:
                                    this.m_tabPages.Add(this.ReadTabPage(reader));
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

        private Resco.Controls.CommonControls.TabPage ReadTabPage(XmlReader reader)
        {
            Resco.Controls.CommonControls.TabPage o = null;
            try
            {
                string str = reader["Name"];
                o = new Resco.Controls.CommonControls.TabPage();
                if (this._designTimeCallback != null)
                {
                    this._designTimeCallback(o, null);
                }
                o.Name = str;
                if (!reader.IsEmptyElement)
                {
                    goto Label_00F1;
                }
                return o;
            Label_0042:
                try
                {
                    string str2;
                    if (((str2 = reader.Name) == null) || (str2 == ""))
                    {
                        goto Label_00F1;
                    }
                    if (!(str2 == "TabPage"))
                    {
                        if (str2 == "Image")
                        {
                            goto Label_00B1;
                        }
                        if (str2 == "TabItem")
                        {
                            goto Label_00BB;
                        }
                        if (str2 == "Property")
                        {
                            goto Label_00CA;
                        }
                        goto Label_00F1;
                    }
                    if (this._designTimeCallback != null)
                    {
                        this._designTimeCallback(o, null);
                    }
                    return o;
                Label_00B1:
                    this.ReadImage(o, reader);
                    goto Label_00F1;
                Label_00BB:
                    o.TabItem = this.ReadToolbarItem(reader);
                    goto Label_00F1;
                Label_00CA:
                    this.m_Conversion.SetProperty(o, reader["Name"], reader["Value"]);
                }
                catch
                {
                }
            Label_00F1:
                if (reader.Read())
                {
                    goto Label_0042;
                }
            }
            catch
            {
                try
                {
                    if (((reader.Name == "TabPage") && reader.IsStartElement()) && !reader.IsEmptyElement)
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "TabPage")
                            {
                                goto Label_0144;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        Label_0144:
            if (o != null)
            {
                return o;
            }
            return new Resco.Controls.CommonControls.TabPage();
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
                    goto Label_00B7;
                }
                return o;
            Label_003F:
                try
                {
                    string str2;
                    if (((str2 = reader.Name) == null) || (str2 == ""))
                    {
                        goto Label_00B7;
                    }
                    if (!(str2 == "TabItem"))
                    {
                        if (str2 == "Image")
                        {
                            goto Label_0086;
                        }
                        if (str2 == "Property")
                        {
                            goto Label_0090;
                        }
                        goto Label_00B7;
                    }
                    return o;
                Label_0086:
                    this.ReadImage(o, reader);
                    goto Label_00B7;
                Label_0090:
                    this.m_Conversion.SetProperty(o, reader["Name"], reader["Value"]);
                }
                catch
                {
                }
            Label_00B7:
                if (reader.Read())
                {
                    goto Label_003F;
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
                                goto Label_0107;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        Label_0107:
            if (o != null)
            {
                return o;
            }
            return new ToolbarItem();
        }

        internal void SetSelectedIndexWithoutEvent(ref int value)
        {
            if (this.m_Initialization)
            {
                this.m_selectedIndex = value;
            }
            else
            {
                if ((this.m_tabPages.Count > 0) && (value < 0))
                {
                    value = 0;
                }
                if (value >= this.m_tabPages.Count)
                {
                    value = this.m_tabPages.Count - 1;
                }
                if ((this.m_selectedIndex >= 0) && (this.m_selectedIndex < this.m_tabPages.Count))
                {
                    this.m_tabPages[this.m_selectedIndex].VisiblePanel = false;
                }
                if ((value >= 0) && (value < this.m_tabPages.Count))
                {
                    this.m_tabPages[value].VisiblePanel = true;
                    this.m_tabPages[value].BringToFront();
                }
                this.m_selectedIndex = value;
            }
        }

        protected virtual bool ShouldSerializeArrowsTransparency()
        {
            return (this.m_toolBar.ArrowsTransparency != 150);
        }

        protected virtual bool ShouldSerializeBackgroundImage()
        {
            return (this.m_toolBar.BackgroundImage != null);
        }

        protected virtual bool ShouldSerializeBmpArrowNext()
        {
            return (this.m_toolBar.BmpArrowNext != null);
        }

        protected virtual bool ShouldSerializeBmpArrowPrevious()
        {
            return (this.m_toolBar.BmpArrowPrevious != null);
        }

        protected virtual bool ShouldSerializeDockType()
        {
            return (this.m_DockType != TabsDockType.Top);
        }

        protected virtual bool ShouldSerializeEnableArrowsTransparency()
        {
            return !this.EnableArrowsTransparency;
        }

        protected virtual bool ShouldSerializeItemsAlignment()
        {
            return (this.m_toolBar.ItemsAlignment != Alignment.TopCenter);
        }

        protected virtual bool ShouldSerializeMarginAtBegin()
        {
            return (this.MarginAtBegin != 0);
        }

        protected virtual bool ShouldSerializeMarginAtEnd()
        {
            return (this.MarginAtEnd != 0);
        }

        protected virtual bool ShouldSerializeTabPages()
        {
            return ((this.m_tabPages != null) && (this.m_tabPages.Count != 0));
        }

        private void ToolBar_ItemEntered(object sender, EventArgs e)
        {
            this.OnItemEntered();
        }

        public byte ArrowsTransparency
        {
            get
            {
                return this.m_toolBar.ArrowsTransparency;
            }
            set
            {
                this.m_toolBar.ArrowsTransparency = value;
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
                if (this.m_toolBar != null)
                {
                    this.m_toolBar.BackColor = value;
                }
                base.BackColor = value;
            }
        }

        public Bitmap BackgroundImage
        {
            get
            {
                return this.m_toolBar.BackgroundImage;
            }
            set
            {
                this.m_toolBar.BackgroundImage = value;
            }
        }

        public Bitmap BmpArrowNext
        {
            get
            {
                return this.m_toolBar.BmpArrowNext;
            }
            set
            {
                this.m_toolBar.BmpArrowNext = value;
            }
        }

        public Bitmap BmpArrowPrevious
        {
            get
            {
                return this.m_toolBar.BmpArrowPrevious;
            }
            set
            {
                this.m_toolBar.BmpArrowPrevious = value;
            }
        }

        public TabsDockType DockType
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
                return this.m_toolBar.EnableArrowsTransparency;
            }
            set
            {
                this.m_toolBar.EnableArrowsTransparency = value;
                base.Invalidate();
            }
        }

        public bool EnableTouchScrolling
        {
            get
            {
                return this.m_toolBar.EnableTouchScrolling;
            }
            set
            {
                this.m_toolBar.EnableTouchScrolling = value;
            }
        }

        internal ToolbarItemCollection Items
        {
            get
            {
                return this.m_toolBar.Items;
            }
            set
            {
                this.m_toolBar.Items = value;
            }
        }

        public Alignment ItemsAlignment
        {
            get
            {
                return this.m_toolBar.ItemsAlignment;
            }
            set
            {
                this.m_toolBar.ItemsAlignment = value;
                base.Invalidate();
            }
        }

        public int MarginAtBegin
        {
            get
            {
                return this.m_toolBar.MarginAtBegin;
            }
            set
            {
                this.m_toolBar.MarginAtBegin = value;
            }
        }

        public int MarginAtEnd
        {
            get
            {
                return this.m_toolBar.MarginAtEnd;
            }
            set
            {
                this.m_toolBar.MarginAtEnd = value;
            }
        }

        public bool SelectBeforeScrolling
        {
            get
            {
                return this.m_toolBar.SelectBeforeScrolling;
            }
            set
            {
                this.m_toolBar.SelectBeforeScrolling = value;
            }
        }

        public int SelectedIndex
        {
            get
            {
                if ((this.m_tabPages.Count > 0) && (this.m_selectedIndex < 0))
                {
                    this.m_selectedIndex = 0;
                }
                return this.m_selectedIndex;
            }
            set
            {
                if (this.m_Initialization)
                {
                    this.m_selectedIndex = value;
                }
                else
                {
                    int selectedIndex = this.m_selectedIndex;
                    this.SetSelectedIndexWithoutEvent(ref value);
                    if (selectedIndex != this.m_selectedIndex)
                    {
                        this.m_toolBar.SelectedIndex = this.m_selectedIndex;
                        this.OnSelectedIndexChanged(EventArgs.Empty);
                        this.OnChanged();
                    }
                }
            }
        }

        public Resco.Controls.CommonControls.TabPage SelectedTab
        {
            get
            {
                if (((this.m_tabPages != null) && (this.m_selectedIndex >= 0)) && (this.m_selectedIndex < this.m_tabPages.Count))
                {
                    return this.m_tabPages[this.m_selectedIndex];
                }
                return null;
            }
            set
            {
                this.SelectedIndex = this.m_tabPages.IndexOf(value);
            }
        }

        public TabPagesCollection TabPages
        {
            get
            {
                return this.m_tabPages;
            }
        }

        public Size ToolbarSize
        {
            get
            {
                return this.m_toolBar.Size;
            }
            set
            {
                this.m_toolBar.Size = value;
            }
        }

        public int TouchSensitivity
        {
            get
            {
                return this.m_toolBar.TouchSensitivity;
            }
            set
            {
                this.m_toolBar.TouchSensitivity = value;
            }
        }

        public delegate void DesignTimeCallback(object o, object o2);
    }
}

