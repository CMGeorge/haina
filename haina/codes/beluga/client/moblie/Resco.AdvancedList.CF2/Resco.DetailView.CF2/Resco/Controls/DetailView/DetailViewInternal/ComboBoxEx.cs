namespace Resco.Controls.DetailView.DetailViewInternal
{
    using Resco.Controls.DetailView;
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ComboBoxEx : ComboBox
    {
        private readonly int ButtonWidth = Resco.Controls.DetailView.DetailView.ComboButtonWidth;
        private bool m_bDoLastText;
        private bool m_bInChange;
        private Imports.WindowProcCallback m_delegate;
        private GCHandle m_delegateHandle;
        private RescoComboBoxStyle m_DropDownStyle = RescoComboBoxStyle.DropDownList;
        private bool m_IsInit;
        private string m_lastDropDownText = "";
        private Keys m_lastKeyDown;
        private int m_maxLength;
        private TextBoxEx m_TextBox;
        private IntPtr m_wndproc = IntPtr.Zero;
        private IntPtr m_wndprocReal = IntPtr.Zero;

        public event EventHandler Changed;

        public event EventHandler DropDown;

        public event EventHandler DropDownClosed;

        internal ComboBoxEx()
        {
            if (Imports.IsSmartphone)
            {
                this.m_TextBox = new TextBoxEx();
                this.m_TextBox.Text = "";
                this.m_TextBox.BorderStyle = BorderStyle.None;
                this.m_TextBox.Multiline = false;
                this.m_TextBox.Hide();
                this.m_TextBox.KeyDown += new KeyEventHandler(this.m_TextBox_KeyDown);
                this.m_TextBox.KeyPress += new KeyPressEventHandler(this.m_TextBox_KeyPress);
                this.m_TextBox.KeyUp += new KeyEventHandler(this.m_TextBox_KeyUp);
                this.m_TextBox.LostFocus += new EventHandler(this.m_TextBox_LostFocus);
                this.m_TextBox.TextChanged += new EventHandler(this.m_TextBox_TextChanged);
            }
        }

        public void BeginInit()
        {
            this.m_IsInit = true;
            base.BeginUpdate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.m_TextBox != null))
            {
                this.m_TextBox.Dispose();
            }
            this.m_TextBox = null;
            base.Dispose(disposing);
        }

        public void EndInit()
        {
            this.m_IsInit = false;
            base.EndUpdate();
        }

        public bool Focus()
        {
            if (Imports.IsSmartphone && (this.DropDownStyle == RescoComboBoxStyle.DropDown))
            {
                return this.m_TextBox.Focus();
            }
            return base.Focus();
        }

        public void Hide()
        {
            if (Imports.IsSmartphone && (this.m_DropDownStyle == RescoComboBoxStyle.DropDown))
            {
                this.m_TextBox.Visible = false;
            }
            base.Visible = false;
        }

        private void m_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Return) && Imports.IsSmartphone)
            {
                Imports.SendMessageW(base.Handle, 0x100, 13, 0);
            }
            base.OnKeyDown(e);
        }

        private void m_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void m_TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                this.DroppedDown = !this.DroppedDown;
            }
            base.OnKeyUp(e);
        }

        private void m_TextBox_LostFocus(object sender, EventArgs e)
        {
            if (!this.Focused)
            {
                this.OnLostFocus(e);
            }
        }

        private void m_TextBox_TextChanged(object sender, EventArgs e)
        {
            if (!this.m_IsInit && !this.m_bInChange)
            {
                string text;
                this.m_bInChange = true;
                if (!Imports.IsSmartphone)
                {
                    text = this.Text;
                }
                else
                {
                    text = this.m_TextBox.Text;
                }
                if (this.SelectedIndex < 0)
                {
                    this.SelectedIndex = -1;
                    base.Text = text;
                }
                bool isSmartphone = Imports.IsSmartphone;
                this.m_bInChange = false;
                if (this.Changed != null)
                {
                    this.Changed(this, EventArgs.Empty);
                }
            }
        }

        protected internal virtual void OnDropDown(EventArgs e)
        {
            if (this.DropDown != null)
            {
                this.DropDown(this, e);
            }
        }

        protected internal virtual void OnDropDownClosed(EventArgs e)
        {
            if (this.DropDownClosed != null)
            {
                this.DropDownClosed(this, e);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if ((Imports.IsSmartphone && (this.DropDownStyle == RescoComboBoxStyle.DropDown)) && (this.SelectedIndex >= 0))
            {
                this.m_TextBox.Text = base.Text;
                this.m_TextBox.SelectionStart = 0;
                this.m_TextBox.SelectionLength = base.Text.Length;
                this.m_TextBox.Focus();
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                this.DroppedDown = !this.DroppedDown;
            }
            if ((e.KeyData == Keys.Return) && !this.DroppedDown)
            {
                this.Focus();
            }
            base.OnKeyUp(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                if (Imports.IsSmartphone && (this.m_lastKeyDown == Keys.Return))
                {
                    this.m_lastKeyDown = Keys.None;
                }
                else if (!this.Focused)
                {
                    base.OnLostFocus(e);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if ((base.Parent != null) && !base.Parent.Controls.Contains(this.m_TextBox))
            {
                base.Parent.Controls.Add(this.m_TextBox);
            }
            base.OnParentChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (Imports.IsSmartphone)
            {
                Rectangle rectangle;
                if (this.m_TextBox.BorderStyle == BorderStyle.None)
                {
                    rectangle = new Rectangle(2 * DVTextBox.BorderSize, 2 * DVTextBox.BorderSize, base.Bounds.Width - (4 * DVTextBox.BorderSize), base.Bounds.Height - (4 * DVTextBox.BorderSize));
                }
                else
                {
                    rectangle = new Rectangle(0, 0, base.Bounds.Width, base.Bounds.Height);
                }
                rectangle.Width -= this.ButtonWidth;
                this.m_TextBox.Bounds = rectangle;
            }
            base.OnResize(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (!this.m_IsInit && !this.m_bInChange)
            {
                this.m_bInChange = true;
                base.OnSelectedIndexChanged(e);
                if ((Imports.IsSmartphone && (this.DropDownStyle == RescoComboBoxStyle.DropDown)) && (this.SelectedIndex != -1))
                {
                    this.m_TextBox.Text = base.Text;
                    this.m_TextBox.SelectionStart = 0;
                    this.m_TextBox.SelectionLength = this.m_TextBox.Text.Length;
                    if (!this.DroppedDown)
                    {
                        this.m_TextBox.Focus();
                    }
                }
                this.m_bInChange = false;
                if (this.Changed != null)
                {
                    this.Changed(this, EventArgs.Empty);
                }
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!Imports.IsSmartphone)
            {
                this.m_TextBox_TextChanged(this, e);
            }
            base.OnTextChanged(e);
        }

        public void Show()
        {
            base.Visible = true;
            if (!Imports.IsSmartphone)
            {
                if (this.SelectedIndex == -1)
                {
                    string text = this.Text;
                    this.SelectedIndex = -1;
                    this.Text = text;
                }
                this.Focus();
            }
            else if (this.DropDownStyle == RescoComboBoxStyle.DropDown)
            {
                this.m_TextBox.BringToFront();
                this.m_TextBox.Show();
                this.m_TextBox.Focus();
                this.m_TextBox.SelectionStart = this.m_TextBox.Text.Length;
            }
            else
            {
                base.Focus();
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
            if ((msg == 0x100) || (msg == 0x101))
            {
                if ((msg == 0x100) && Imports.IsSmartphone)
                {
                    this.m_lastKeyDown = (Keys) wParam;
                }
                if ((((wParam == 40L) || (wParam == 0x26L)) && Imports.IsSmartphone) || ((wParam == 13L) && !Imports.IsSmartphone))
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

        public virtual RescoComboBoxStyle DropDownStyle
        {
            get
            {
                return this.m_DropDownStyle;
            }
            set
            {
                if (this.m_DropDownStyle != value)
                {
                    this.m_DropDownStyle = value;
                    if (!Imports.IsSmartphone)
                    {
                        switch (value)
                        {
                            case RescoComboBoxStyle.DropDownList:
                                base.DropDownStyle = ComboBoxStyle.DropDownList;
                                return;
                        }
                        base.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                }
            }
        }

        public bool DroppedDown
        {
            get
            {
                try
                {
                    return (Imports.SendMessageW(base.Handle, 0x157, 0, 0) > 0);
                }
                catch
                {
                    PropertyInfo property = base.GetType().BaseType.GetProperty("DroppedDown", BindingFlags.Public | BindingFlags.Instance);
                    return ((property != null) && Convert.ToBoolean(property.GetValue(this, new object[0])));
                }
            }
            set
            {
                try
                {
                    Imports.SendMessageW(base.Handle, 0x14f, (uint)(value ? 1 : 0), 0);
                }
                catch
                {
                    PropertyInfo property = base.GetType().BaseType.GetProperty("DroppedDown", BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        property.SetValue(this, value, new object[0]);
                    }
                    else
                    {
                        value = false;
                    }
                }
                if (Imports.IsSmartphone)
                {
                    if (value)
                    {
                        base.Focus();
                    }
                    else if (this.DropDownStyle == RescoComboBoxStyle.DropDown)
                    {
                        this.m_TextBox.Focus();
                    }
                }
            }
        }

        public bool Focused
        {
            get
            {
                if (!Imports.IsSmartphone)
                {
                    return base.Focused;
                }
                return (base.Focused || ((this.m_TextBox != null) && this.m_TextBox.Focused));
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                if (Imports.IsSmartphone)
                {
                    this.m_TextBox.Font = value;
                }
            }
        }

        public int MaxLength
        {
            get
            {
                if (!Imports.IsSmartphone)
                {
                    return this.m_maxLength;
                }
                return this.m_TextBox.MaxLength;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (!Imports.IsSmartphone)
                {
                    this.m_maxLength = value;
                    try
                    {
                        Imports.SendMessageW(base.Handle, 0x141, (uint) value, 0);
                    }
                    catch
                    {
                        PropertyInfo property = base.GetType().BaseType.GetProperty("MaxLength", BindingFlags.Public | BindingFlags.Instance);
                        if (property != null)
                        {
                            property.SetValue(this, value, new object[0]);
                        }
                    }
                }
                else
                {
                    this.m_TextBox.MaxLength = value;
                }
            }
        }

        public override string Text
        {
            get
            {
                if (!Imports.IsSmartphone || (this.DropDownStyle != RescoComboBoxStyle.DropDown))
                {
                    return base.Text;
                }
                if (this.m_TextBox == null)
                {
                    return base.Text;
                }
                return this.m_TextBox.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                }
                if (Imports.IsSmartphone && (this.DropDownStyle == RescoComboBoxStyle.DropDown))
                {
                    this.m_TextBox.Text = value;
                    if (this.m_TextBox.Visible)
                    {
                        this.m_TextBox.Focus();
                    }
                    this.m_TextBox.SelectionStart = this.m_TextBox.Text.Length;
                }
            }
        }

        public TextBoxEx TextBox
        {
            get
            {
                return this.m_TextBox;
            }
        }
    }
}

