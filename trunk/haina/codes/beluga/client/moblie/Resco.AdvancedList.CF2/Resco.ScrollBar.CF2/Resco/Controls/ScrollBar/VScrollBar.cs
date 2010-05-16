namespace Resco.Controls.ScrollBar
{
    using System;

    public class VScrollBar : Resco.Controls.ScrollBar.ScrollBar
    {
        public VScrollBar() : base(Resco.Controls.ScrollBar.ScrollBar.ScrollBarOrientation.Vertical)
        {
        }

        private VScrollBarExtensionLocation ConvertExtensionLocation(ScrollBarExtensionLocation location)
        {
            if (location != ScrollBarExtensionLocation.LeftTop)
            {
                return VScrollBarExtensionLocation.Right;
            }
            return VScrollBarExtensionLocation.Left;
        }

        protected virtual bool ShouldSerializeDownButton()
        {
            return (base.RightDownButton != null);
        }

        protected virtual bool ShouldSerializeDownButtonDisabled()
        {
            return (base.RightDownButtonDisabled != null);
        }

        protected virtual bool ShouldSerializeDownButtonHighlight()
        {
            return (base.RightDownButtonHighlight != null);
        }

        protected virtual bool ShouldSerializeExtensionLocation()
        {
            return (base.ExtensionLocationInControl != ScrollBarExtensionLocation.LeftTop);
        }

        protected virtual bool ShouldSerializeUpButton()
        {
            return (base.LeftUpButton != null);
        }

        protected virtual bool ShouldSerializeUpButtonDisabled()
        {
            return (base.LeftUpButtonDisabled != null);
        }

        protected virtual bool ShouldSerializeUpButtonHighlight()
        {
            return (base.LeftUpButtonHighlight != null);
        }

        public ScrollBarButton DownButton
        {
            get
            {
                return base.RightDownButton;
            }
            set
            {
                base.RightDownButton = value;
            }
        }

        public ScrollBarButton DownButtonDisabled
        {
            get
            {
                return base.RightDownButtonDisabled;
            }
            set
            {
                base.RightDownButtonDisabled = value;
            }
        }

        public int DownButtonHeight
        {
            get
            {
                return base.RightDownButtonSize;
            }
            set
            {
                base.RightDownButtonSize = value;
            }
        }

        public ScrollBarButton DownButtonHighlight
        {
            get
            {
                return base.RightDownButtonHighlight;
            }
            set
            {
                base.RightDownButtonHighlight = value;
            }
        }

        public VScrollBarExtensionLocation ExtensionLocation
        {
            get
            {
                return this.ConvertExtensionLocation(base.ExtensionLocationInControl);
            }
            set
            {
                if (this.ConvertExtensionLocation(base.ExtensionLocationInControl) != value)
                {
                    base.ExtensionLocationInControl = (value == VScrollBarExtensionLocation.Left) ? ScrollBarExtensionLocation.LeftTop : ScrollBarExtensionLocation.RightBottom;
                }
            }
        }

        public int ExtensionWidth
        {
            get
            {
                return base.ExtensionSize;
            }
            set
            {
                base.ExtensionSize = value;
            }
        }

        public int MaximumThumbHeight
        {
            get
            {
                return base.MaximumThumbSize;
            }
            set
            {
                base.MaximumThumbSize = value;
            }
        }

        public int MinimumThumbHeight
        {
            get
            {
                return base.MinimumThumbSize;
            }
            set
            {
                base.MinimumThumbSize = value;
            }
        }

        public ScrollBarButton UpButton
        {
            get
            {
                return base.LeftUpButton;
            }
            set
            {
                base.LeftUpButton = value;
            }
        }

        public ScrollBarButton UpButtonDisabled
        {
            get
            {
                return base.LeftUpButtonDisabled;
            }
            set
            {
                base.LeftUpButtonDisabled = value;
            }
        }

        public int UpButtonHeight
        {
            get
            {
                return base.LeftUpButtonSize;
            }
            set
            {
                base.LeftUpButtonSize = value;
            }
        }

        public ScrollBarButton UpButtonHighlight
        {
            get
            {
                return base.LeftUpButtonHighlight;
            }
            set
            {
                base.LeftUpButtonHighlight = value;
            }
        }
    }
}

