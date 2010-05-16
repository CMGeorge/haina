namespace Resco.Controls.DetailView.DetailViewInternal
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class TextBoxEx : TextBox
    {
        private Imports.WindowProcCallback m_delegate;
        private GCHandle m_delegateHandle;
        private int m_Height = 0x15;
        private Keys m_lastKeyDown;
        private IntPtr m_wndproc = IntPtr.Zero;
        private IntPtr m_wndprocReal = IntPtr.Zero;

        internal TextBoxEx()
        {
            this.m_Height = base.Height;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public bool Focus()
        {
            bool flag = base.Focus();
            base.ScrollToCaret();
            return flag;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                if (this.m_wndproc == IntPtr.Zero)
                {
                    this.m_delegate = new Imports.WindowProcCallback(this.WindowProc);
                    this.m_wndproc = Marshal.GetFunctionPointerForDelegate(this.m_delegate);
                }
                if (!this.m_delegateHandle.IsAllocated)
                {
                    this.m_delegateHandle = GCHandle.Alloc(this.m_delegate, GCHandleType.Pinned);
                }
                this.m_wndprocReal = Imports.SetWindowLong(base.Handle, -4, this.m_wndproc);
                this.SetSize(this.Width, this.Height);
            }
            base.OnHandleCreated(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.Multiline && Imports.IsSmartphone)
            {
                if (e.KeyCode == Keys.Up)
                {
                    base.SelectionStart = 0;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    base.SelectionStart = this.Text.Length;
                }
            }
            base.OnKeyDown(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if ((this.Multiline && Imports.IsSmartphone) && (((this.m_lastKeyDown == Keys.Left) || (this.m_lastKeyDown == Keys.Right)) || (this.m_lastKeyDown == Keys.Return)))
            {
                this.m_lastKeyDown = Keys.None;
            }
            else
            {
                base.OnLostFocus(e);
            }
        }

        private void SetSize(int w, int h)
        {
            try
            {
                Imports.SetWindowPos(base.Handle, IntPtr.Zero, 0, 0, w, h, 6);
                IntPtr parent = Imports.GetParent(base.Handle);
                if ((base.Parent != null) && (parent != base.Parent.Handle))
                {
                    Imports.SetWindowPos(parent, IntPtr.Zero, 0, 0, w, h, 6);
                }
            }
            catch
            {
                base.Size = new System.Drawing.Size(w, h);
            }
        }

        protected virtual int WindowProc(IntPtr hwnd, int msg, int wParam, int lParam)
        {
            if (msg == 2)
            {
                Imports.SetWindowLong(base.Handle, -4, this.m_wndprocReal);
                if (this.m_delegateHandle.IsAllocated)
                {
                    this.m_delegateHandle.Free();
                }
            }
            if ((msg == 0x102) && (wParam == 9))
            {
                this.OnKeyPress(new KeyPressEventArgs((char) wParam));
            }
            if (((msg == 0x100) || (msg == 0x101)) && Imports.IsSmartphone)
            {
                if (msg == 0x100)
                {
                    this.m_lastKeyDown = (Keys) wParam;
                }
                if ((wParam == 40L) || (wParam == 0x26L))
                {
                    if (msg == 0x100)
                    {
                        this.OnKeyDown(new KeyEventArgs((Keys) wParam));
                    }
                    else if (msg == 0x101)
                    {
                        this.OnKeyUp(new KeyEventArgs((Keys) wParam));
                    }
                    return 1;
                }
            }
            if (msg == 5)
            {
                this.OnResize(new EventArgs());
            }
            return Imports.CallWindowProc(this.m_wndprocReal, hwnd, msg, wParam, lParam);
        }

        public System.Windows.Forms.BorderStyle BorderStyle
        {
            get
            {
                if (Imports.IsSmartphone)
                {
                    return System.Windows.Forms.BorderStyle.FixedSingle;
                }
                return base.BorderStyle;
            }
            set
            {
                base.BorderStyle = value;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(base.Left, base.Top, this.Width, this.Height);
            }
            set
            {
                base.Location = value.Location;
                this.Size = value.Size;
            }
        }

        public int Height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                if (this.m_Height != value)
                {
                    this.m_Height = value;
                    this.SetSize(this.Width, value);
                }
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size(this.Width, this.Height);
            }
            set
            {
                base.Width = value.Width;
                this.m_Height = value.Height;
                this.SetSize(value.Width, value.Height);
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                if (value != base.Text)
                {
                    bool flag = false;
                    if (base.SelectionStart == this.Text.Length)
                    {
                        flag = true;
                    }
                    base.Text = value;
                    if (flag && (value != null))
                    {
                        base.SelectionStart = value.Length;
                    }
                }
            }
        }

        public int Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                if (base.Width != value)
                {
                    base.Width = value;
                    this.SetSize(value, this.Height);
                }
            }
        }
    }
}

