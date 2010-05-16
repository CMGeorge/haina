namespace Resco.Controls.CommonControls
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ToolBar : UserControl
    {
        private ToolbarControl m_ToolbarControl;

        public event EventHandler ItemEntered;

        public ToolBar(Resco.Controls.CommonControls.TabControl aParent)
        {
            if (aParent == null)
            {
                throw new UnauthorizedAccessException("Error: This control is a part of the TabControl. Please use ToolbarControl in your application.");
            }
            base.Height = 0x1a;
            this.Dock = DockStyle.Top;
            this.m_ToolbarControl = new ToolbarControl();
            this.m_ToolbarControl.Height = 0x1a;
            this.m_ToolbarControl.Dock = DockStyle.Fill;
            this.m_ToolbarControl.SelectionChanged += new EventHandler(this.ToolbarControl_SelectionChanged);
            this.m_ToolbarControl.ItemEntered += new EventHandler(this.ToolbarControl_ItemEntered);
            base.Controls.Add(this.m_ToolbarControl);
        }

        public void InvokeOnClick(Point pt)
        {
            pt.Y -= base.Location.Y;
            this.m_ToolbarControl.Click(pt);
        }

        private void OnItemEntered()
        {
            if (this.ItemEntered != null)
            {
                this.ItemEntered(this, EventArgs.Empty);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.m_ToolbarControl != null)
            {
                this.m_ToolbarControl.Dock = DockStyle.Fill;
                this.m_ToolbarControl.Invalidate();
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            this.m_ToolbarControl.Refresh();
        }

        protected virtual bool ShouldSerializeArrowsTransparency()
        {
            return (this.m_ToolbarControl.ArrowsTransparency != 150);
        }

        protected virtual bool ShouldSerializeBackgroundImage()
        {
            return (this.BackgroundImage != null);
        }

        protected virtual bool ShouldSerializeBmpArrowNext()
        {
            return (this.BmpArrowNext != null);
        }

        protected virtual bool ShouldSerializeBmpArrowPrevious()
        {
            return (this.BmpArrowPrevious != null);
        }

        protected virtual bool ShouldSerializeItemsAlignment()
        {
            return (this.m_ToolbarControl.ItemsAlignment != Alignment.TopCenter);
        }

        private void ToolbarControl_ItemEntered(object sender, EventArgs e)
        {
            this.OnItemEntered();
        }

        private void ToolbarControl_SelectionChanged(object sender, EventArgs e)
        {
            Resco.Controls.CommonControls.TabControl parent = base.Parent as Resco.Controls.CommonControls.TabControl;
            parent.SelectedIndex = this.m_ToolbarControl.SelectedIndex;
        }

        public byte ArrowsTransparency
        {
            get
            {
                return this.m_ToolbarControl.ArrowsTransparency;
            }
            set
            {
                this.m_ToolbarControl.ArrowsTransparency = value;
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
                if (this.m_ToolbarControl != null)
                {
                    this.m_ToolbarControl.BackColor = value;
                }
                base.BackColor = value;
            }
        }

        public Bitmap BackgroundImage
        {
            get
            {
                return this.m_ToolbarControl.BackgroundImage;
            }
            set
            {
                this.m_ToolbarControl.BackgroundImage = value;
            }
        }

        public Bitmap BmpArrowNext
        {
            get
            {
                return this.m_ToolbarControl.BmpArrowNext;
            }
            set
            {
                this.m_ToolbarControl.BmpArrowNext = value;
            }
        }

        public Bitmap BmpArrowPrevious
        {
            get
            {
                return this.m_ToolbarControl.BmpArrowPrevious;
            }
            set
            {
                this.m_ToolbarControl.BmpArrowPrevious = value;
            }
        }

        public ToolbarDockType DockType
        {
            get
            {
                return this.m_ToolbarControl.DockType;
            }
            set
            {
                this.m_ToolbarControl.DockType = value;
            }
        }

        public bool EnableArrowsTransparency
        {
            get
            {
                return this.m_ToolbarControl.EnableArrowsTransparency;
            }
            set
            {
                this.m_ToolbarControl.EnableArrowsTransparency = value;
            }
        }

        public bool EnableTouchScrolling
        {
            get
            {
                return this.m_ToolbarControl.EnableTouchScrolling;
            }
            set
            {
                this.m_ToolbarControl.EnableTouchScrolling = value;
            }
        }

        public ToolbarItemCollection Items
        {
            get
            {
                return this.m_ToolbarControl.Items;
            }
            set
            {
                this.m_ToolbarControl.Items = value;
                base.Invalidate();
            }
        }

        public Alignment ItemsAlignment
        {
            get
            {
                return this.m_ToolbarControl.ItemsAlignment;
            }
            set
            {
                this.m_ToolbarControl.ItemsAlignment = value;
                base.Invalidate();
            }
        }

        public int MarginAtBegin
        {
            get
            {
                return this.m_ToolbarControl.MarginAtBegin;
            }
            set
            {
                this.m_ToolbarControl.MarginAtBegin = value;
            }
        }

        public int MarginAtEnd
        {
            get
            {
                return this.m_ToolbarControl.MarginAtEnd;
            }
            set
            {
                this.m_ToolbarControl.MarginAtEnd = value;
            }
        }

        public bool SelectBeforeScrolling
        {
            get
            {
                return this.m_ToolbarControl.SelectBeforeScrolling;
            }
            set
            {
                this.m_ToolbarControl.SelectBeforeScrolling = value;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.m_ToolbarControl.SelectedIndex;
            }
            set
            {
                this.m_ToolbarControl.SelectedIndex = value;
            }
        }

        public bool StretchBackgroundImage
        {
            get
            {
                return this.m_ToolbarControl.StretchBackgroundImage;
            }
            set
            {
                this.m_ToolbarControl.StretchBackgroundImage = value;
            }
        }

        public int TouchSensitivity
        {
            get
            {
                return this.m_ToolbarControl.TouchSensitivity;
            }
            set
            {
                this.m_ToolbarControl.TouchSensitivity = value;
            }
        }
    }
}

