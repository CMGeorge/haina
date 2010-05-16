namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.DetailViewInternal;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class DVComboBox : Control
    {
        private bool m_bCloseUp;
        private bool m_bSelChange;
        private ComboBoxEx m_ComboBox = new ComboBoxEx();
        private Imports.WindowProcCallback m_delegate;
        private GCHandle m_delegateHandle;
        private IntPtr m_wndproc = IntPtr.Zero;
        private IntPtr m_wndprocReal = IntPtr.Zero;
        public EventHandler ValueChanged;

        internal DVComboBox()
        {
            this.m_ComboBox.DropDownStyle = RescoComboBoxStyle.DropDown;
            this.m_ComboBox.LostFocus += new EventHandler(this.m_ComboBox_LostFocus);
            this.m_ComboBox.Changed += new EventHandler(this.OnComboBoxChanged);
            this.m_ComboBox.KeyDown += new KeyEventHandler(this.m_ComboBox_KeyDown);
            this.m_ComboBox.KeyPress += new KeyPressEventHandler(this.m_ComboBox_KeyPress);
            this.m_ComboBox.KeyUp += new KeyEventHandler(this.m_ComboBox_KeyUp);
            base.Controls.Add(this.m_ComboBox);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.m_ComboBox != null))
            {
                this.m_ComboBox.Dispose();
            }
            this.m_ComboBox = null;
            base.Dispose(disposing);
        }

        public bool Focus()
        {
            if (this.m_ComboBox == null)
            {
                return false;
            }
            return this.m_ComboBox.Focus();
        }

        private Rectangle GetBounds()
        {
            return new Rectangle(0, 0, base.Bounds.Width, base.Bounds.Height);
        }

        public void Hide()
        {
            this.m_ComboBox.Hide();
            this.m_ComboBox.DataSource = null;
            base.Visible = false;
        }

        private void m_ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        private void m_ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void m_ComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        private void m_ComboBox_LostFocus(object sender, EventArgs e)
        {
            this.OnLostFocus(e);
        }

        protected virtual void OnComboBoxChanged(object sender, EventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(sender, e);
            }
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
                    this.m_delegateHandle = GCHandle.Alloc(this.m_delegate, GCHandleType.Normal);
                }
                this.m_wndprocReal = Imports.SetWindowLong(base.Handle, -4, this.m_wndproc);
            }
            base.OnHandleCreated(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (!this.Focused)
            {
                base.OnLostFocus(e);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            if (this.m_ComboBox != null)
            {
                this.m_ComboBox.Bounds = this.GetBounds();
                base.Height = this.m_ComboBox.Height;
            }
            base.OnResize(e);
        }

        public void Show()
        {
            this.m_ComboBox.Font = base.Font;
            this.m_ComboBox.Bounds = this.GetBounds();
            base.Height = this.m_ComboBox.Height;
            base.Visible = true;
            this.m_ComboBox.BringToFront();
            this.m_ComboBox.Show();
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
            if (msg == 0x111)
            {
                switch ((wParam >> 0x10))
                {
                    case 4:
                        this.m_bCloseUp = false;
                        this.m_bSelChange = false;
                        break;

                    case 7:
                        if (this.m_ComboBox != null)
                        {
                            this.m_ComboBox.OnDropDown(EventArgs.Empty);
                        }
                        break;

                    case 8:
                        if ((this.m_ComboBox == null) || !this.m_bSelChange)
                        {
                            this.m_bCloseUp = true;
                        }
                        else
                        {
                            this.m_ComboBox.OnDropDownClosed(EventArgs.Empty);
                            this.m_bCloseUp = false;
                        }
                        this.m_bSelChange = false;
                        break;

                    case 10:
                    case 1:
                        if (this.m_bCloseUp && (this.m_ComboBox != null))
                        {
                            this.m_bSelChange = false;
                            this.m_bCloseUp = false;
                            int num2 = Imports.CallWindowProc(this.m_wndprocReal, hwnd, msg, wParam, lParam);
                            this.m_ComboBox.OnDropDownClosed(EventArgs.Empty);
                            return num2;
                        }
                        this.m_bSelChange = true;
                        this.m_bCloseUp = false;
                        break;
                }
            }
            return Imports.CallWindowProc(this.m_wndprocReal, hwnd, msg, wParam, lParam);
        }

        public ComboBoxEx ComboBox
        {
            get
            {
                return this.m_ComboBox;
            }
        }

        public bool Focused
        {
            get
            {
                if (base.Focused)
                {
                    return true;
                }
                if (this.m_ComboBox == null)
                {
                    return false;
                }
                return this.m_ComboBox.Focused;
            }
        }

        public override string Text
        {
            get
            {
                if (this.m_ComboBox == null)
                {
                    return base.Text;
                }
                return this.m_ComboBox.Text;
            }
            set
            {
                if (this.m_ComboBox != null)
                {
                    this.m_ComboBox.Text = value;
                }
            }
        }
    }
}

