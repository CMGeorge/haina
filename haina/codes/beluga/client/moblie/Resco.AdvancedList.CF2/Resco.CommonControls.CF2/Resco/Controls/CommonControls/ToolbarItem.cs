namespace Resco.Controls.CommonControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class ToolbarItem : Component
    {
        private bool m_AutoTransparent;
        private Color m_BackColor;
        private Rectangle m_ClientRectangle;
        private Size m_CustomSize;
        private bool m_Enabled;
        private Color m_FocusedColor;
        private System.Drawing.Font m_Font;
        private Color m_ForeColor;
        private Alignment m_ImageAlignment;
        private Bitmap m_ImageDefault;
        private Bitmap m_ImagePressed;
        private ToolbarItemSizeType m_ItemSizeType;
        private string m_Name;
        private bool m_Pressed;
        private bool m_StretchImage;
        private object m_Tag;
        private string m_Text;
        private Alignment m_TextAlignment;
        private StringFormat m_TextFormat;
        private ToolbarItemBehaviorType m_ToolbarItemBehavior;
        internal static int m_VgaOffset = 1;
        private bool m_Visible;

        internal event RefreshRequiredEventHandler RefreshRequired;

        public ToolbarItem()
        {
            this.m_Visible = true;
            this.m_Enabled = true;
            this.m_Pressed = false;
            this.m_Name = string.Empty;
            this.m_Text = string.Empty;
            this.Tag = null;
            this.m_Font = new System.Drawing.Font("Arial", 10f, FontStyle.Regular);
            this.m_TextFormat = new StringFormat();
            this.m_TextFormat.Alignment = StringAlignment.Center;
            this.m_TextFormat.LineAlignment = StringAlignment.Center;
            this.m_TextAlignment = Alignment.MiddleCenter;
            this.m_ImageAlignment = Alignment.MiddleCenter;
            this.m_ForeColor = Color.Black;
            this.m_BackColor = Color.White;
            this.m_FocusedColor = Color.Black;
            this.m_StretchImage = false;
            this.m_AutoTransparent = false;
            this.m_ToolbarItemBehavior = ToolbarItemBehaviorType.KeepSelectedAfterClick;
        }

        public ToolbarItem(string aName) : this()
        {
            this.m_Name = aName;
        }

        private void Invalidate()
        {
            this.OnRefreshRequired();
        }

        protected virtual void OnRefreshRequired()
        {
            if (this.RefreshRequired != null)
            {
                this.RefreshRequired(this, new ToolbarItemEventArgs(this));
            }
        }

        private void OnTextAlignmentChanged()
        {
            switch (this.m_TextAlignment)
            {
                case Alignment.TopLeft:
                    this.m_TextFormat.LineAlignment = StringAlignment.Near;
                    this.m_TextFormat.Alignment = StringAlignment.Near;
                    return;

                case Alignment.TopCenter:
                    this.m_TextFormat.LineAlignment = StringAlignment.Near;
                    this.m_TextFormat.Alignment = StringAlignment.Center;
                    return;

                case Alignment.TopRight:
                    this.m_TextFormat.LineAlignment = StringAlignment.Near;
                    this.m_TextFormat.Alignment = StringAlignment.Far;
                    return;

                case Alignment.MiddleLeft:
                    this.m_TextFormat.LineAlignment = StringAlignment.Center;
                    this.m_TextFormat.Alignment = StringAlignment.Near;
                    return;

                case Alignment.MiddleCenter:
                    this.m_TextFormat.LineAlignment = StringAlignment.Center;
                    this.m_TextFormat.Alignment = StringAlignment.Center;
                    return;

                case Alignment.MiddleRight:
                    this.m_TextFormat.LineAlignment = StringAlignment.Center;
                    this.m_TextFormat.Alignment = StringAlignment.Far;
                    return;

                case Alignment.BottomLeft:
                    this.m_TextFormat.LineAlignment = StringAlignment.Far;
                    this.m_TextFormat.Alignment = StringAlignment.Near;
                    return;

                case Alignment.BottomCenter:
                    this.m_TextFormat.LineAlignment = StringAlignment.Far;
                    this.m_TextFormat.Alignment = StringAlignment.Center;
                    return;

                case Alignment.BottomRight:
                    this.m_TextFormat.LineAlignment = StringAlignment.Far;
                    this.m_TextFormat.Alignment = StringAlignment.Far;
                    return;
            }
        }

        protected virtual bool ShouldSerializeAutoTransparent()
        {
            return this.m_AutoTransparent;
        }

        protected virtual bool ShouldSerializeBackColor()
        {
            return !this.m_BackColor.Equals(Color.White);
        }

        protected virtual bool ShouldSerializeCustomSize()
        {
            return !Size.Empty.Equals(this.m_CustomSize);
        }

        protected virtual bool ShouldSerializeFocusedColor()
        {
            return !this.m_FocusedColor.Equals(Color.Black);
        }

        protected virtual bool ShouldSerializeForeColor()
        {
            return !this.m_ForeColor.Equals(Color.Black);
        }

        protected virtual bool ShouldSerializeImageAlignment()
        {
            return (this.m_ImageAlignment != Alignment.MiddleCenter);
        }

        protected virtual bool ShouldSerializeImageDefault()
        {
            return (this.m_ImageDefault != null);
        }

        protected virtual bool ShouldSerializeImagePressed()
        {
            return (this.m_ImagePressed != null);
        }

        protected virtual bool ShouldSerializeItemSizeType()
        {
            return (this.m_ItemSizeType != ToolbarItemSizeType.ByImage);
        }

        protected virtual bool ShouldSerializeName()
        {
            return !string.Empty.Equals(this.m_Name);
        }

        protected virtual bool ShouldSerializeTag()
        {
            return (this.m_Tag != null);
        }

        protected virtual bool ShouldSerializeText()
        {
            return !string.Empty.Equals(this.m_Text);
        }

        protected virtual bool ShouldSerializeTextAlignment()
        {
            return (this.m_TextAlignment != Alignment.MiddleCenter);
        }

        protected virtual bool ShouldSerializeToolbarItemBehavior()
        {
            return (this.m_ToolbarItemBehavior != ToolbarItemBehaviorType.KeepSelectedAfterClick);
        }

        public bool AutoTransparent
        {
            get
            {
                return this.m_AutoTransparent;
            }
            set
            {
                if (this.m_AutoTransparent != value)
                {
                    this.m_AutoTransparent = value;
                    this.Invalidate();
                }
            }
        }

        public Color BackColor
        {
            get
            {
                return this.m_BackColor;
            }
            set
            {
                this.m_BackColor = value;
                this.Invalidate();
            }
        }

        internal Rectangle ClientRectangle
        {
            get
            {
                return this.m_ClientRectangle;
            }
            set
            {
                this.m_ClientRectangle = value;
            }
        }

        public Size CustomSize
        {
            get
            {
                return this.m_CustomSize;
            }
            set
            {
                this.m_CustomSize = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.m_Enabled;
            }
            set
            {
                if (this.m_Enabled != value)
                {
                    this.m_Enabled = value;
                }
            }
        }

        public Color FocusedColor
        {
            get
            {
                return this.m_FocusedColor;
            }
            set
            {
                this.m_FocusedColor = value;
                this.Invalidate();
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this.m_Font;
            }
            set
            {
                this.m_Font = value;
                this.Invalidate();
            }
        }

        public Color ForeColor
        {
            get
            {
                return this.m_ForeColor;
            }
            set
            {
                this.m_ForeColor = value;
                this.Invalidate();
            }
        }

        public Alignment ImageAlignment
        {
            get
            {
                return this.m_ImageAlignment;
            }
            set
            {
                if (this.m_ImageAlignment != value)
                {
                    this.m_ImageAlignment = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue((string) null)]
        public Bitmap ImageDefault
        {
            get
            {
                return this.m_ImageDefault;
            }
            set
            {
                this.m_ImageDefault = value;
                this.Invalidate();
            }
        }

        [DefaultValue((string) null)]
        public Bitmap ImagePressed
        {
            get
            {
                return this.m_ImagePressed;
            }
            set
            {
                this.m_ImagePressed = value;
            }
        }

        public ToolbarItemSizeType ItemSizeType
        {
            get
            {
                return this.m_ItemSizeType;
            }
            set
            {
                this.m_ItemSizeType = value;
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

        internal bool Pressed
        {
            get
            {
                return this.m_Pressed;
            }
            set
            {
                if (((this.m_Pressed != value) && (this.m_ToolbarItemBehavior != ToolbarItemBehaviorType.Separator)) && this.m_Enabled)
                {
                    this.m_Pressed = value;
                }
            }
        }

        public bool StretchImage
        {
            get
            {
                return this.m_StretchImage;
            }
            set
            {
                if (this.m_StretchImage != value)
                {
                    this.m_StretchImage = value;
                    this.Invalidate();
                }
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
                if (this.m_Text != value)
                {
                    this.m_Text = value;
                    this.Invalidate();
                }
            }
        }

        public Alignment TextAlignment
        {
            get
            {
                return this.m_TextAlignment;
            }
            set
            {
                if (this.m_TextAlignment != value)
                {
                    this.m_TextAlignment = value;
                    this.OnTextAlignmentChanged();
                    this.Invalidate();
                }
            }
        }

        public ToolbarItemBehaviorType ToolbarItemBehavior
        {
            get
            {
                return this.m_ToolbarItemBehavior;
            }
            set
            {
                this.m_ToolbarItemBehavior = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this.m_Visible;
            }
            set
            {
                if (this.m_Visible != value)
                {
                    this.m_Visible = value;
                    this.Invalidate();
                }
            }
        }
    }
}

