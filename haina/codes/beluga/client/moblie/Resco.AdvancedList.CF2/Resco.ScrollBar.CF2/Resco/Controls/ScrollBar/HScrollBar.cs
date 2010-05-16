namespace Resco.Controls.ScrollBar
{
    using System;

    public class HScrollBar : Resco.Controls.ScrollBar.ScrollBar
    {
        public HScrollBar() : base(Resco.Controls.ScrollBar.ScrollBar.ScrollBarOrientation.Horizontal)
        {
        }

        private HScrollBarExtensionLocation ConvertExtensionLocation(ScrollBarExtensionLocation location)
        {
            if (location != ScrollBarExtensionLocation.LeftTop)
            {
                return HScrollBarExtensionLocation.Bottom;
            }
            return HScrollBarExtensionLocation.Top;
        }

        protected virtual bool ShouldSerializeExtensionLocation()
        {
            return (base.ExtensionLocationInControl != ScrollBarExtensionLocation.LeftTop);
        }

        protected virtual bool ShouldSerializeLeftButton()
        {
            return (base.LeftUpButton != null);
        }

        protected virtual bool ShouldSerializeLeftButtonDisabled()
        {
            return (base.LeftUpButtonDisabled != null);
        }

        protected virtual bool ShouldSerializeLeftButtonHighlight()
        {
            return (base.LeftUpButtonHighlight != null);
        }

        protected virtual bool ShouldSerializeRightButton()
        {
            return (base.RightDownButton != null);
        }

        protected virtual bool ShouldSerializeRightButtonDisabled()
        {
            return (base.RightDownButtonDisabled != null);
        }

        protected virtual bool ShouldSerializeRightButtonHighlight()
        {
            return (base.RightDownButtonHighlight != null);
        }

        public int ExtensionHeight
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

        public HScrollBarExtensionLocation ExtensionLocation
        {
            get
            {
                return this.ConvertExtensionLocation(base.ExtensionLocationInControl);
            }
            set
            {
                if (this.ConvertExtensionLocation(base.ExtensionLocationInControl) != value)
                {
                    base.ExtensionLocationInControl = (value == HScrollBarExtensionLocation.Top) ? ScrollBarExtensionLocation.LeftTop : ScrollBarExtensionLocation.RightBottom;
                }
            }
        }

        public ScrollBarButton LeftButton
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

        public ScrollBarButton LeftButtonDisabled
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

        public ScrollBarButton LeftButtonHighlight
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

        public int LeftButtonWidth
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

        public int MaximumThumbWidth
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

        public int MinimumThumbWidth
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

        public ScrollBarButton RightButton
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

        public ScrollBarButton RightButtonDisabled
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

        public ScrollBarButton RightButtonHighlight
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

        public int RightButtonWidth
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
    }
}

