namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal class ScrollbarWrapper
    {
        private EventHandler m_onResize;
        private EventHandler m_onValueChanged;
        private Control m_scrollBar;
        private Resco.Controls.AdvancedTree.ScrollOrientation m_scrollOrientation;

        public event EventHandler Resize;

        public event EventHandler ValueChanged;

        public ScrollbarWrapper(Control scrollbar, Resco.Controls.AdvancedTree.ScrollOrientation scrollOrientation)
        {
            this.m_scrollBar = scrollbar;
            this.m_scrollOrientation = scrollOrientation;
            this.m_onValueChanged = new EventHandler(this.OnValueChanged);
            this.m_onResize = new EventHandler(this.OnResize);
        }

        public void Attach(Control parent)
        {
            parent.Controls.Add(this.m_scrollBar);
            if (this.m_scrollBar is ScrollBar)
            {
                ((ScrollBar) this.m_scrollBar).ValueChanged += this.m_onValueChanged;
            }
            else
            {
                this.m_scrollBar.GetType().GetEvent("ValueChanged").AddEventHandler(this.m_scrollBar, this.m_onValueChanged);
            }
            this.m_scrollBar.Resize += new EventHandler(this.OnResize);
        }

        public void BringToFront()
        {
            this.m_scrollBar.BringToFront();
        }

        public void Detach()
        {
            if (this.m_scrollBar != null)
            {
                if (this.m_scrollBar is ScrollBar)
                {
                    ((ScrollBar) this.m_scrollBar).ValueChanged -= this.m_onValueChanged;
                }
                else
                {
                    this.m_scrollBar.GetType().GetEvent("ValueChanged").RemoveEventHandler(this.m_scrollBar, this.m_onValueChanged);
                }
                this.m_scrollBar.Resize -= this.m_onResize;
                if (this.m_scrollBar.Parent != null)
                {
                    bool flag = false;
                    if ((this.m_scrollBar.Parent.Site != null) && this.m_scrollBar.Parent.Site.DesignMode)
                    {
                        flag = true;
                    }
                    this.m_scrollBar.Parent.Controls.Remove(this.m_scrollBar);
                    if (!flag)
                    {
                        this.m_scrollBar.Dispose();
                    }
                }
                else
                {
                    this.m_scrollBar.Dispose();
                }
            }
            this.m_scrollBar = null;
        }

        private object GetPropertyValue(string name)
        {
            return this.m_scrollBar.GetType().GetProperty(name).GetValue(this.m_scrollBar, null);
        }

        public void Hide()
        {
            this.m_scrollBar.Hide();
        }

        protected virtual void OnResize(object sender, EventArgs e)
        {
            if (this.Resize != null)
            {
                this.Resize(this, e);
            }
        }

        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        public static explicit operator VScrollBar(ScrollbarWrapper wrapper)
        {
            return (VScrollBar) wrapper.m_scrollBar;
        }

        public static implicit operator Control(ScrollbarWrapper wrapper)
        {
            return wrapper.m_scrollBar;
        }

        private void SetPropertyValue(string name, object value)
        {
            this.m_scrollBar.GetType().GetProperty(name).SetValue(this.m_scrollBar, value, null);
        }

        public void Show()
        {
            this.m_scrollBar.Show();
        }

        public Rectangle Bounds
        {
            get
            {
                return this.m_scrollBar.Bounds;
            }
            set
            {
                this.m_scrollBar.Bounds = value;
            }
        }

        public DockStyle Dock
        {
            get
            {
                return this.m_scrollBar.Dock;
            }
            set
            {
                this.m_scrollBar.Dock = value;
            }
        }

        public int Height
        {
            get
            {
                return this.m_scrollBar.Height;
            }
            set
            {
                this.m_scrollBar.Height = value;
            }
        }

        public int LargeChange
        {
            get
            {
                if (this.m_scrollBar is ScrollBar)
                {
                    return ((ScrollBar) this.m_scrollBar).LargeChange;
                }
                return Convert.ToInt32(this.GetPropertyValue("LargeChange"));
            }
            set
            {
                if (this.m_scrollBar is ScrollBar)
                {
                    ((ScrollBar) this.m_scrollBar).LargeChange = value;
                }
                else
                {
                    this.SetPropertyValue("LargeChange", value);
                }
            }
        }

        public int Left
        {
            get
            {
                return this.m_scrollBar.Left;
            }
            set
            {
                this.m_scrollBar.Left = value;
            }
        }

        public int Maximum
        {
            get
            {
                if (this.m_scrollBar is ScrollBar)
                {
                    return ((ScrollBar) this.m_scrollBar).Maximum;
                }
                return Convert.ToInt32(this.GetPropertyValue("Maximum"));
            }
            set
            {
                if (this.m_scrollBar is ScrollBar)
                {
                    ((ScrollBar) this.m_scrollBar).Maximum = value;
                }
                else
                {
                    this.SetPropertyValue("Maximum", value);
                }
            }
        }

        public int Minimum
        {
            get
            {
                if (this.m_scrollBar is ScrollBar)
                {
                    return ((ScrollBar) this.m_scrollBar).Minimum;
                }
                return Convert.ToInt32(this.GetPropertyValue("Minimum"));
            }
            set
            {
                if (this.m_scrollBar is ScrollBar)
                {
                    ((ScrollBar) this.m_scrollBar).Minimum = value;
                }
                else
                {
                    this.SetPropertyValue("Minimum", value);
                }
            }
        }

        public Resco.Controls.AdvancedTree.ScrollOrientation ScrollOrientation
        {
            get
            {
                return this.m_scrollOrientation;
            }
        }

        public int SmallChange
        {
            get
            {
                if (this.m_scrollBar is ScrollBar)
                {
                    return ((ScrollBar) this.m_scrollBar).SmallChange;
                }
                return Convert.ToInt32(this.GetPropertyValue("SmallChange"));
            }
            set
            {
                if (this.m_scrollBar is ScrollBar)
                {
                    ((ScrollBar) this.m_scrollBar).SmallChange = value;
                }
                else
                {
                    this.SetPropertyValue("SmallChange", value);
                }
            }
        }

        public bool TabStop
        {
            get
            {
                return this.m_scrollBar.TabStop;
            }
            set
            {
                this.m_scrollBar.TabStop = value;
            }
        }

        public int Top
        {
            get
            {
                return this.m_scrollBar.Top;
            }
            set
            {
                this.m_scrollBar.Top = value;
            }
        }

        public int Value
        {
            get
            {
                if (this.m_scrollBar is ScrollBar)
                {
                    return ((ScrollBar) this.m_scrollBar).Value;
                }
                return Convert.ToInt32(this.GetPropertyValue("Value"));
            }
            set
            {
                if (this.m_scrollBar is ScrollBar)
                {
                    ((ScrollBar) this.m_scrollBar).Value = value;
                }
                else
                {
                    this.SetPropertyValue("Value", value);
                }
            }
        }

        public bool Visible
        {
            get
            {
                return this.m_scrollBar.Visible;
            }
            set
            {
                if ((this.m_scrollBar.Site != null) && this.m_scrollBar.Site.DesignMode)
                {
                    value = true;
                }
                this.m_scrollBar.Visible = value;
            }
        }

        public int Width
        {
            get
            {
                return this.m_scrollBar.Width;
            }
            set
            {
                this.m_scrollBar.Width = value;
            }
        }
    }
}

