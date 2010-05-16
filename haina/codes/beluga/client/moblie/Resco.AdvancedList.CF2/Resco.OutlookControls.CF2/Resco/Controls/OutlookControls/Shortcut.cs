namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;

    public class Shortcut
    {
        private bool m_bAutoTransparent;
        private bool m_Enabled = true;
        internal ImageAttributes m_ia = new ImageAttributes();
        private int m_ImageIndex = -1;
        private string m_Name = "";
        private ShortcutsCollection m_Parent;
        private object m_Tag;
        private string m_Text = "";
        private Color m_transparentColor = Color.Transparent;

        public event EventHandler Invalidating;

        internal void Dispose()
        {
            this.m_Parent = null;
        }

        public void Invalidate()
        {
            if (this.Invalidating != null)
            {
                this.Invalidating(this, new EventArgs());
            }
        }

        public override string ToString()
        {
            if (!(this.Name == "") && (this.Name != null))
            {
                return this.Name;
            }
            return "Shortcut";
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
                    this.Invalidate();
                }
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

        public ShortcutsCollection Parent
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
    }
}

