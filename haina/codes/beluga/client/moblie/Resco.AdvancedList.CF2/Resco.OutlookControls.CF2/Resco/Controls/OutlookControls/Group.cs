namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class Group
    {
        private Color m_ActiveColor = SystemColors.Window;
        private Rectangle m_backgroundImageRect = new Rectangle(0, 0, 1, 1);
        private bool m_bAutoTransparent;
        private SolidBrush m_brushShortcutsBack;
        private GradientColor m_gradientBackColor = new GradientColor();
        private bool m_groupVisible = true;
        internal ImageAttributes m_ia = new ImageAttributes();
        private int m_ImageIndex = -1;
        private string m_Name = "";
        private GroupsCollection m_Parent;
        private ShortcutsCollection m_shortcuts = new ShortcutsCollection();
        private object m_Tag;
        private string m_Text = "";
        private Color m_transparentColor = Color.Transparent;
        private bool m_useGradient;

        public event EventHandler Invalidating;

        public event Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler SelectedShortcutIndexChanged;

        public event Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler ShortcutEntered;

        public Group()
        {
            this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
            this.m_useGradient = false;
            this.m_shortcuts.InsertCompleteEvent += new InsertCompleteEventHandler(this.OnShortcutsUpdateInsert);
            this.m_shortcuts.RemoveEvent += new RemoveEventHandler(this.OnShortcutsUpdateRemove);
            this.m_shortcuts.SetCompleteEvent += new SetCompleteEventHandler(this.OnShortcutsUpdateSet);
            this.m_shortcuts.Invalidating += new EventHandler(this.OnInvalidating);
            this.m_shortcuts.SelectedShortcutIndexChanged += new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnSelectedShortcutIndexChanged);
            this.m_shortcuts.ShortcutEntered += new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnShortcutEntered);
        }

        private void CreateGdiObjects()
        {
            if ((this.m_brushShortcutsBack == null) || (this.m_brushShortcutsBack.Color != this.ShortcutsBackColor))
            {
                this.m_brushShortcutsBack = OutlookShortcutBar.GetBrush(this.ShortcutsBackColor);
            }
        }

        private void DisableEvents()
        {
            if (this.m_gradientBackColor != null)
            {
                this.m_gradientBackColor.PropertyChanged -= new EventHandler(this.m_gradientBackColor_PropertyChanged);
            }
            if (this.m_shortcuts != null)
            {
                this.m_shortcuts.InsertCompleteEvent -= new InsertCompleteEventHandler(this.OnShortcutsUpdateInsert);
                this.m_shortcuts.RemoveEvent -= new RemoveEventHandler(this.OnShortcutsUpdateRemove);
                this.m_shortcuts.SetCompleteEvent -= new SetCompleteEventHandler(this.OnShortcutsUpdateSet);
                this.m_shortcuts.Invalidating -= new EventHandler(this.OnInvalidating);
                this.m_shortcuts.SelectedShortcutIndexChanged -= new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnSelectedShortcutIndexChanged);
                this.m_shortcuts.ShortcutEntered -= new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnShortcutEntered);
            }
        }

        internal void Dispose()
        {
            this.m_Parent = null;
            this.DisableEvents();
            if (this.m_shortcuts != null)
            {
                this.m_shortcuts.Dispose();
                this.m_shortcuts = null;
            }
        }

        internal void Draw(Graphics gr, Rectangle rect, Pen frame, Image backgroundImage)
        {
            if (this.m_groupVisible)
            {
                this.CreateGdiObjects();
                if (!this.m_useGradient)
                {
                    gr.FillRectangle(this.m_brushShortcutsBack, rect);
                }
                else
                {
                    this.m_gradientBackColor.DrawGradient(gr, rect);
                }
                if (backgroundImage != null)
                {
                    this.m_backgroundImageRect.Width = backgroundImage.Width;
                    this.m_backgroundImageRect.Height = backgroundImage.Height;
                    gr.DrawImage(backgroundImage, rect, this.m_backgroundImageRect, GraphicsUnit.Pixel);
                }
                this.m_shortcuts.Draw(gr, rect, frame);
            }
        }

        public void Invalidate()
        {
            if (this.Invalidating != null)
            {
                this.Invalidating(this, new EventArgs());
            }
        }

        private void m_gradientBackColor_PropertyChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        internal void MouseDown(Point p, Rectangle rect)
        {
            if (this.m_groupVisible)
            {
                this.m_shortcuts.MouseDown(p, rect);
            }
        }

        internal void MouseMove(Point p, Rectangle rect)
        {
            if (this.m_groupVisible)
            {
                this.m_shortcuts.MouseMove(p, rect);
            }
        }

        internal void MouseUp(Point p, Rectangle rect)
        {
            if (this.m_groupVisible)
            {
                this.m_shortcuts.MouseUp(p, rect);
            }
        }

        private void OnDetachImageList(object sender, EventArgs e)
        {
            this.ShortcutsImageList = null;
        }

        private void OnInvalidating(object sender, EventArgs e)
        {
            if (this.Invalidating != null)
            {
                this.Invalidating(sender, e);
            }
        }

        private void OnSelectedShortcutIndexChanged(object sender, SelectedIndexChangedEventArgs e)
        {
            if (this.SelectedShortcutIndexChanged != null)
            {
                this.SelectedShortcutIndexChanged(sender, e);
            }
        }

        private void OnShortcutEntered(object sender, SelectedIndexChangedEventArgs e)
        {
            if (this.ShortcutEntered != null)
            {
                this.ShortcutEntered(sender, e);
            }
        }

        private void OnShortcutsUpdateInsert(object sender, InsertEventArgs e)
        {
            this.Invalidate();
        }

        private void OnShortcutsUpdateRemove(object sender, RemoveEventArgs e)
        {
            if ((this.m_shortcuts.SelectedIndex > -1) && (this.m_shortcuts.Count == 0))
            {
                this.m_shortcuts.SelectedIndex = -1;
            }
        }

        private void OnShortcutsUpdateSet(object sender, SetEventArgs e)
        {
            this.Invalidate();
        }

        protected virtual bool ShouldSerializeGradientBackColor()
        {
            return (((this.m_gradientBackColor.StartColor.ToArgb() != Color.White.ToArgb()) | (this.m_gradientBackColor.EndColor.ToArgb() != Color.White.ToArgb())) | (this.m_gradientBackColor.FillDirection != FillDirection.Vertical));
        }

        protected virtual bool ShouldSerializeShortcutsTextStyle()
        {
            if (this.m_shortcuts.TextStyle == TextStyle.Bottom)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeShortcutsViewStyle()
        {
            if (this.m_shortcuts.ViewStyle == ViewStyle.List)
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            if (!(this.Name == "") && (this.Name != null))
            {
                return this.Name;
            }
            return "Group";
        }

        public bool AutoTransparent
        {
            get
            {
                return this.m_bAutoTransparent;
            }
            set
            {
                if (this.m_bAutoTransparent != value)
                {
                    this.m_bAutoTransparent = value;
                    this.Invalidate();
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
                this.Invalidate();
            }
        }

        public int ImageIndex
        {
            get
            {
                return this.m_ImageIndex;
            }
            set
            {
                this.m_ImageIndex = value;
                this.Invalidate();
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }

        public GroupsCollection Parent
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

        internal int ScrollBarHeight
        {
            get
            {
                return this.m_shortcuts.ScrollBarHeight;
            }
            set
            {
                this.m_shortcuts.ScrollBarHeight = value;
            }
        }

        internal int ScrollBarWidth
        {
            get
            {
                return this.m_shortcuts.ScrollBarWidth;
            }
            set
            {
                this.m_shortcuts.ScrollBarWidth = value;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.m_shortcuts.SelectedIndex;
            }
            set
            {
                this.m_shortcuts.SelectedIndex = value;
            }
        }

        public Color SelShortcutsBackColor
        {
            get
            {
                return this.m_shortcuts.SelBackColor;
            }
            set
            {
                if (this.m_shortcuts.SelBackColor != value)
                {
                    this.m_shortcuts.SelBackColor = value;
                    this.m_shortcuts.RefreshShortcutsOnNextDraw();
                }
            }
        }

        public Color SelShortcutsForeColor
        {
            get
            {
                return this.m_shortcuts.SelForeColor;
            }
            set
            {
                if (this.m_shortcuts.SelForeColor != value)
                {
                    this.m_shortcuts.SelForeColor = value;
                    this.m_shortcuts.RefreshShortcutsOnNextDraw();
                }
            }
        }

        public ShortcutsCollection Shortcuts
        {
            get
            {
                return this.m_shortcuts;
            }
        }

        public Color ShortcutsBackColor
        {
            get
            {
                return this.m_shortcuts.BackColor;
            }
            set
            {
                if (this.m_shortcuts.BackColor != value)
                {
                    this.m_shortcuts.BackColor = value;
                    this.m_shortcuts.RefreshShortcutsOnNextDraw();
                }
            }
        }

        public Font ShortcutsFont
        {
            get
            {
                return this.m_shortcuts.Font;
            }
            set
            {
                this.m_shortcuts.Font = value;
            }
        }

        public Color ShortcutsForeColor
        {
            get
            {
                return this.m_shortcuts.ForeColor;
            }
            set
            {
                if (this.m_shortcuts.ForeColor != value)
                {
                    this.m_shortcuts.ForeColor = value;
                    this.m_shortcuts.RefreshShortcutsOnNextDraw();
                }
            }
        }

        public ImageList ShortcutsImageList
        {
            get
            {
                return this.m_shortcuts.ImageList;
            }
            set
            {
                if (value != this.m_shortcuts.ImageList)
                {
                    EventHandler handler = new EventHandler(this.OnDetachImageList);
                    if (this.m_shortcuts.ImageList != null)
                    {
                        this.m_shortcuts.ImageList.Disposed -= handler;
                    }
                    this.m_shortcuts.ImageList = value;
                    if (value != null)
                    {
                        value.Disposed += handler;
                    }
                }
            }
        }

        public TextStyle ShortcutsTextStyle
        {
            get
            {
                return this.m_shortcuts.TextStyle;
            }
            set
            {
                this.m_shortcuts.TextStyle = value;
            }
        }

        public ViewStyle ShortcutsViewStyle
        {
            get
            {
                return this.m_shortcuts.ViewStyle;
            }
            set
            {
                this.m_shortcuts.ViewStyle = value;
            }
        }

        public int ShortcutsWidth
        {
            get
            {
                return this.m_shortcuts.Width;
            }
            set
            {
                this.m_shortcuts.Width = value;
                this.Invalidate();
            }
        }

        public object Tag
        {
            get
            {
                return this.m_Tag;
            }
            set
            {
                this.m_Tag = value;
            }
        }

        public string Text
        {
            get
            {
                return this.m_Text;
            }
            set
            {
                this.m_Text = value;
                this.Invalidate();
            }
        }

        public Color TransparentColor
        {
            get
            {
                return this.m_transparentColor;
            }
            set
            {
                if (this.m_transparentColor != value)
                {
                    if (value.IsEmpty)
                    {
                        value = Color.Transparent;
                    }
                    this.m_transparentColor = value;
                    this.m_ia.ClearColorKey();
                    if (!(this.m_transparentColor == OutlookShortcutBar.TransparentColor))
                    {
                        this.m_ia.SetColorKey(this.m_transparentColor, this.m_transparentColor);
                    }
                    this.Invalidate();
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
                    this.Invalidate();
                }
            }
        }

        public bool Visible
        {
            get
            {
                return this.m_groupVisible;
            }
            set
            {
                if (this.m_groupVisible != value)
                {
                    this.m_groupVisible = value;
                }
            }
        }
    }
}

