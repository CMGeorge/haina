namespace Resco.Controls.MaskedTextBox
{
    using Resco.Controls.DetailView;
    using Resco.Controls.DetailView.DetailViewInternal;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class MaskedTextBox : TextBox
    {
        public static string CantConvertToType = "cannot be converted to type";
        private bool m_beepOnError;
        private int m_caret;
        private Imports.WindowProcCallback m_delegate;
        private GCHandle m_delegateHandle;
        private IFormatProvider m_formatProvider;
        private bool m_hidePromptOnLeave;
        private bool m_internalTextSet;
        private Resco.Controls.MaskedTextBox.MaskedTextProvider m_provider;
        private Type m_validatingType;
        private IntPtr m_wndproc;
        private IntPtr m_wndprocReal;
        public static string MaskInputIncompleteMsg = "Mask input is not complete.";
        public static string TypeValidationSucceeded = "Type validation succeeded.";

        public event MaskInputRejectedEventHandler MaskInputRejected;

        public event TypeValidationEventHandler TypeValidationCompleted;

        static MaskedTextBox()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(Resco.Controls.MaskedTextBox.MaskedTextBox), "");
            //}
        }

        public MaskedTextBox()
        {
            this.m_wndproc = IntPtr.Zero;
            this.m_wndprocReal = IntPtr.Zero;
            this.m_provider = new Resco.Controls.MaskedTextBox.MaskedTextProvider();
            this.m_provider.TextChanged += new EventHandler(this.MaskProviderTextChanged);
            this.m_caret = 0;
            this.m_internalTextSet = false;
            this.m_beepOnError = false;
            this.m_hidePromptOnLeave = false;
            base.Multiline = false;
            this.m_validatingType = null;
            base.Validating += new CancelEventHandler(this.DoValidating);
        }

        public MaskedTextBox(Resco.Controls.MaskedTextBox.MaskedTextProvider maskedTextProvider)
        {
            this.m_wndproc = IntPtr.Zero;
            this.m_wndprocReal = IntPtr.Zero;
            this.m_provider = maskedTextProvider;
            this.m_provider.TextChanged += new EventHandler(this.MaskProviderTextChanged);
            this.m_caret = 0;
            this.m_internalTextSet = false;
            this.m_beepOnError = false;
            this.m_hidePromptOnLeave = false;
            base.Multiline = false;
            this.m_validatingType = null;
        }

        public MaskedTextBox(string mask)
        {
            this.m_wndproc = IntPtr.Zero;
            this.m_wndprocReal = IntPtr.Zero;
            this.m_provider.Mask = mask;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_provider != null)
                {
                    this.m_provider.TextChanged -= new EventHandler(this.MaskProviderTextChanged);
                }
                this.m_provider = null;
            }
            base.Dispose(disposing);
        }

        private void DoValidating(object sender, CancelEventArgs e)
        {
            this.OnValidating(e);
        }

        private void MaskProviderTextChanged(object sender, EventArgs e)
        {
            this.m_internalTextSet = true;
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                Imports.HideCaret(base.Handle);
            }
            base.Text = (this.HidePromptOnLeave && !this.Focused) ? this.m_provider.ToString(false, true) : this.m_provider.ToDisplayString();
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                Imports.ShowCaret(base.Handle);
            }
            this.m_internalTextSet = false;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            this.MaskProviderTextChanged(this, EventArgs.Empty);
            this.SetSelection();
            base.OnGotFocus(e);
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
            }
            base.OnHandleCreated(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((this.Mask == "") || base.ReadOnly)
            {
                base.OnKeyDown(e);
            }
            else
            {
                int num4;
                int selectionStart = base.SelectionStart;
                int selectionLength = base.SelectionLength;
                int position = (selectionStart + base.SelectionLength) - 1;
                if (this.m_provider.Format[selectionStart] != this.PromptChar)
                {
                    selectionStart = this.m_provider.FindEditPositionFrom(-1, true);
                    selectionLength = 1;
                }
                switch (e.KeyData)
                {
                    case Keys.End:
                        base.SelectionStart = this.m_provider.FindEditPositionFrom(base.MaxLength, false);
                        base.SelectionLength = 1;
                        this.m_caret = base.SelectionStart;
                        e.Handled = true;
                        return;

                    case Keys.Home:
                        base.SelectionStart = this.m_provider.FindEditPositionFrom(-1, true);
                        base.SelectionLength = 1;
                        this.m_caret = base.SelectionStart;
                        e.Handled = true;
                        return;

                    case Keys.Left:
                    case Keys.Up:
                        num4 = this.m_provider.FindEditPositionFrom(selectionStart, false);
                        if (num4 != selectionStart)
                        {
                            base.SelectionStart = num4;
                            base.SelectionLength = 1;
                        }
                        this.m_caret = num4;
                        e.Handled = true;
                        return;

                    case Keys.Right:
                    case Keys.Down:
                        num4 = this.m_provider.FindEditPositionFrom(selectionStart, true);
                        if (num4 != selectionStart)
                        {
                            base.SelectionStart = num4;
                            base.SelectionLength = 1;
                        }
                        this.m_caret = num4;
                        e.Handled = true;
                        return;

                    case Keys.Delete:
                        this.m_provider.RemoveAt(selectionStart, selectionLength);
                        base.SelectionStart = selectionStart;
                        base.SelectionLength = 1;
                        this.m_caret = selectionStart;
                        e.Handled = true;
                        return;

                    case (Keys.Shift | Keys.End):
                        if (selectionStart >= this.m_caret)
                        {
                            if (selectionStart == this.m_caret)
                            {
                                num4 = this.m_provider.FindEditPositionFrom(base.MaxLength, false);
                                base.SelectionLength = selectionLength + (num4 - position);
                            }
                        }
                        else
                        {
                            num4 = this.m_provider.FindEditPositionFrom(base.MaxLength, false);
                            base.SelectionStart = this.m_caret;
                            base.SelectionLength = (num4 - this.m_caret) + 1;
                        }
                        e.Handled = true;
                        return;

                    case (Keys.Shift | Keys.Home):
                        if ((selectionStart > this.m_caret) || (selectionLength > 1))
                        {
                            num4 = this.m_provider.FindEditPositionFrom(-1, true);
                            base.SelectionStart = num4;
                            base.SelectionLength = (this.m_caret - num4) + 1;
                        }
                        else
                        {
                            num4 = this.m_provider.FindEditPositionFrom(-1, true);
                            base.SelectionStart -= selectionStart - num4;
                            base.SelectionLength = selectionLength + (selectionStart - num4);
                        }
                        e.Handled = true;
                        return;

                    case (Keys.Shift | Keys.Left):
                    case (Keys.Shift | Keys.Up):
                        if ((selectionStart >= this.m_caret) && ((selectionStart != this.m_caret) || (selectionLength > 1)))
                        {
                            base.SelectionLength = selectionLength - (position - this.m_provider.FindEditPositionFrom(position, false));
                            break;
                        }
                        num4 = this.m_provider.FindEditPositionFrom(selectionStart, false);
                        base.SelectionStart -= selectionStart - num4;
                        base.SelectionLength = selectionLength + (selectionStart - num4);
                        break;

                    case (Keys.Shift | Keys.Right):
                    case (Keys.Shift | Keys.Down):
                        if (selectionStart >= this.m_caret)
                        {
                            if (selectionStart == this.m_caret)
                            {
                                num4 = this.m_provider.FindEditPositionFrom(position, true);
                                base.SelectionLength = selectionLength + (num4 - position);
                            }
                        }
                        else
                        {
                            num4 = this.m_provider.FindEditPositionFrom(selectionStart, true);
                            base.SelectionStart += num4 - selectionStart;
                            base.SelectionLength = selectionLength - (num4 - selectionStart);
                        }
                        e.Handled = true;
                        return;

                    case (Keys.Shift | Keys.Insert):
                    case (Keys.Control | Keys.V):
                        selectionLength = this.Paste(selectionStart);
                        if (selectionLength > 0)
                        {
                            base.SelectionStart = selectionStart;
                            base.SelectionLength = selectionLength;
                        }
                        e.Handled = true;
                        return;

                    default:
                        base.OnKeyDown(e);
                        return;
                }
                e.Handled = true;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if ((this.Mask == "") || base.ReadOnly)
            {
                base.OnKeyPress(e);
            }
            else
            {
                int selectionStart = base.SelectionStart;
                int selectionLength = base.SelectionLength;
                if (e.KeyChar == '\b')
                {
                    base.OnKeyPress(e);
                    this.m_provider.RemoveAt(selectionStart, selectionLength);
                    int num3 = this.m_provider.FindEditPositionFrom(selectionStart, false);
                    base.SelectionStart = num3;
                    base.SelectionLength = 1;
                    this.m_caret = num3;
                    e.Handled = true;
                }
                else
                {
                    if (this.m_provider.Format[selectionStart] != this.PromptChar)
                    {
                        selectionStart = this.m_provider.FindEditPositionFrom(-1, true);
                        selectionLength = 1;
                    }
                    if (this.m_provider.Replace(e.KeyChar, selectionStart, selectionLength))
                    {
                        base.OnKeyPress(e);
                        selectionStart = this.m_provider.FindEditPositionFrom(selectionStart, true);
                        base.SelectionStart = selectionStart;
                        this.m_caret = selectionStart;
                        base.SelectionLength = 1;
                    }
                    else if (this.BeepOnError)
                    {
                        Imports.MessageBeep(Imports.BeepAlert.Exclamation);
                        this.OnMaskInputRejected(new MaskInputRejectedEventArgs(selectionStart));
                    }
                    e.Handled = true;
                }
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            this.MaskProviderTextChanged(this, EventArgs.Empty);
            base.OnLostFocus(e);
        }

        private void OnMaskInputRejected(MaskInputRejectedEventArgs e)
        {
            if (this.MaskInputRejected != null)
            {
                this.MaskInputRejected(this, e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.SetSelection();
            base.OnMouseUp(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (this.Mask == "")
            {
                this.m_provider.Text = base.Text;
            }
            this.m_internalTextSet = false;
            base.OnTextChanged(e);
        }

        protected virtual void OnTypeValidationCompleted(TypeValidationEventArgs e)
        {
            if (this.TypeValidationCompleted != null)
            {
                this.TypeValidationCompleted(this, e);
            }
        }

        protected virtual void OnValidating(CancelEventArgs e)
        {
            this.PerformTypeValidation(e);
        }

        private object ParseString(string value, Type targetType, IFormatProvider formatProvider)
        {
            object obj2;
            try
            {
                MethodInfo info = targetType.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string), typeof(NumberStyles), typeof(IFormatProvider) }, null);
                if (info != null)
                {
                    return info.Invoke(null, new object[] { value, NumberStyles.Any, formatProvider });
                }
                info = targetType.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string), typeof(IFormatProvider) }, null);
                if (info != null)
                {
                    return info.Invoke(null, new object[] { value, formatProvider });
                }
                info = targetType.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null);
                if (info != null)
                {
                    return info.Invoke(null, new object[] { value });
                }
                obj2 = null;
            }
            catch (TargetInvocationException exception)
            {
                throw new FormatException(exception.InnerException.Message, exception.InnerException);
            }
            return obj2;
        }

        private int Paste(int position)
        {
            string data = (string) Clipboard.GetDataObject().GetData(DataFormats.Text);
            return this.m_provider.InsertAt(data, position);
        }

        private object PerformTypeValidation(CancelEventArgs e)
        {
            object returnValue = null;
            if (this.m_validatingType != null)
            {
                string message = null;
                if ((this.Mask.Length > 0) && !this.MaskedTextProvider.MaskCompleted)
                {
                    message = MaskInputIncompleteMsg;
                }
                else
                {
                    try
                    {
                        returnValue = this.ValidateType(this.MaskedTextProvider.ToString(false, this.m_provider.IncludeLiterals), this.m_validatingType, this.m_formatProvider);
                    }
                    catch (Exception innerException)
                    {
                        if (innerException.InnerException != null)
                        {
                            innerException = innerException.InnerException;
                        }
                        message = innerException.GetType().ToString() + ": " + innerException.Message;
                    }
                }
                bool isValidInput = false;
                if (message == null)
                {
                    isValidInput = true;
                    message = TypeValidationSucceeded;
                }
                TypeValidationEventArgs args = new TypeValidationEventArgs(this.m_validatingType, isValidInput, returnValue, message);
                this.OnTypeValidationCompleted(args);
                if (e != null)
                {
                    e.Cancel = args.Cancel;
                }
            }
            return returnValue;
        }

        private void SetSelection()
        {
            if (this.Mask != "")
            {
                int selectionStart = base.SelectionStart;
                int num2 = selectionStart;
                int selectionLength = base.SelectionLength;
                if ((selectionStart == base.MaxLength) || (this.m_provider.Format[selectionStart] != this.PromptChar))
                {
                    if (this.m_provider.FindEditPositionFrom(selectionStart, true) == selectionStart)
                    {
                        selectionStart = this.m_provider.FindEditPositionFrom(selectionStart, false);
                    }
                    else
                    {
                        selectionStart = this.m_provider.FindEditPositionFrom(selectionStart, true);
                    }
                    base.SelectionStart = selectionStart;
                }
                if (selectionLength < 1)
                {
                    base.SelectionLength = 1;
                }
                else if (this.m_provider.Format[(num2 + selectionLength) - 1] != this.PromptChar)
                {
                    selectionLength += this.m_provider.FindEditPositionFrom(selectionStart + selectionLength, true) - (selectionStart + selectionLength);
                    base.SelectionLength = selectionLength;
                }
                this.m_caret = selectionStart;
            }
        }

        protected virtual bool ShouldSerializeText()
        {
            if (this.m_provider == null)
            {
                return false;
            }
            return (this.m_provider.ToString(true, true) != this.m_provider.Format);
        }

        protected virtual bool ShouldSerializeTextMaskFormat()
        {
            return (this.TextMaskFormat != MaskFormat.IncludeLiterals);
        }

        public object ValidateText()
        {
            return this.PerformTypeValidation(null);
        }

        private object ValidateType(string value, Type targetType, IFormatProvider formatProvider)
        {
            object obj2 = this.ParseString(value, targetType, formatProvider);
            if (obj2 != null)
            {
                return obj2;
            }
            if (value == null)
            {
                throw new FormatException(string.Format("'{0}' " + CantConvertToType + " '{1}'.", value, targetType));
            }
            return Convert.ChangeType(value, targetType, formatProvider);
        }

        private int WindowProc(IntPtr hwnd, int msg, int wParam, int lParam)
        {
            switch (msg)
            {
                case 0x100:
                case 0x101:
                    if (!Imports.IsSmartphone || ((wParam != 40L) && (wParam != 0x26L)))
                    {
                        goto Label_016F;
                    }
                    if (msg != 0x100)
                    {
                        if (msg == 0x101)
                        {
                            this.OnKeyUp(new KeyEventArgs((Keys) wParam));
                        }
                        break;
                    }
                    this.OnKeyDown(new KeyEventArgs((Keys) wParam));
                    break;

                case 0x102:
                    if (wParam == 9)
                    {
                        this.OnKeyPress(new KeyPressEventArgs((char) wParam));
                    }
                    goto Label_016F;

                case 2:
                    Imports.SetWindowLong(base.Handle, -4, this.m_wndprocReal);
                    if (this.m_delegateHandle.IsAllocated)
                    {
                        this.m_delegateHandle.Free();
                    }
                    goto Label_016F;

                case 0x200:
                    this.OnMouseMove(new MouseEventArgs(((wParam & 1) > 0) ? MouseButtons.Left : MouseButtons.None, 1, lParam & 0xffff, (lParam >> 0x10) & 0xffff, 0));
                    goto Label_016F;

                case 0x201:
                {
                    int num = Imports.CallWindowProc(this.m_wndprocReal, hwnd, msg, wParam, lParam);
                    this.OnMouseDown(new MouseEventArgs(((wParam & 1) > 0) ? MouseButtons.Left : MouseButtons.None, 1, lParam & 0xffff, (lParam >> 0x10) & 0xffff, 0));
                    return num;
                }
                case 0x202:
                    this.OnMouseUp(new MouseEventArgs(((wParam & 1) > 0) ? MouseButtons.Left : MouseButtons.None, 1, lParam & 0xffff, (lParam >> 0x10) & 0xffff, 0));
                    goto Label_016F;

                default:
                    goto Label_016F;
            }
            return 1;
        Label_016F:
            return Imports.CallWindowProc(this.m_wndprocReal, hwnd, msg, wParam, lParam);
        }

        public bool AcceptsReturn
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public bool AcceptsTab
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public bool BeepOnError
        {
            get
            {
                return this.m_beepOnError;
            }
            set
            {
                this.m_beepOnError = value;
            }
        }

        public IFormatProvider FormatProvider
        {
            get
            {
                return this.m_formatProvider;
            }
            set
            {
                this.m_formatProvider = value;
            }
        }

        public bool HidePromptOnLeave
        {
            get
            {
                return this.m_hidePromptOnLeave;
            }
            set
            {
                if (this.m_hidePromptOnLeave != value)
                {
                    this.m_hidePromptOnLeave = value;
                    if (!this.Focused)
                    {
                        this.MaskProviderTextChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public string Mask
        {
            get
            {
                if (this.m_provider == null)
                {
                    return "";
                }
                return this.m_provider.Mask;
            }
            set
            {
                this.m_provider.Mask = value;
                base.MaxLength = (this.m_provider.Format.Length > 0) ? this.m_provider.Format.Length : 0x7fff;
            }
        }

        public bool MaskCompleted
        {
            get
            {
                if (this.m_provider == null)
                {
                    return false;
                }
                return this.m_provider.MaskCompleted;
            }
        }

        public Resco.Controls.MaskedTextBox.MaskedTextProvider MaskedTextProvider
        {
            get
            {
                return this.m_provider;
            }
        }

        public override int MaxLength
        {
            get
            {
                return base.MaxLength;
            }
            set
            {
                if (this.m_provider.Format.Length == 0)
                {
                    base.MaxLength = value;
                }
            }
        }

        public override bool Multiline
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public char PromptChar
        {
            get
            {
                if (this.m_provider == null)
                {
                    return '_';
                }
                return this.m_provider.PromptChar;
            }
            set
            {
                this.m_provider.PromptChar = value;
            }
        }

        public bool ResetOnPrompt
        {
            get
            {
                return ((this.m_provider == null) || this.m_provider.ResetOnPrompt);
            }
            set
            {
                this.m_provider.ResetOnPrompt = value;
            }
        }

        public bool ResetOnSpace
        {
            get
            {
                return ((this.m_provider == null) || this.m_provider.ResetOnSpace);
            }
            set
            {
                this.m_provider.ResetOnSpace = value;
            }
        }

        public override string Text
        {
            get
            {
                if ((this.m_provider != null) && !this.m_internalTextSet)
                {
                    return this.m_provider.Text;
                }
                return base.Text;
            }
            set
            {
                if (this.m_provider.Text != value)
                {
                    this.m_provider.Text = value;
                }
            }
        }

        public MaskFormat TextMaskFormat
        {
            get
            {
                if (this.m_provider != null)
                {
                    return ((this.m_provider.IncludeLiterals ? MaskFormat.IncludeLiterals : MaskFormat.ExcludePromptAndLiterals) | (this.m_provider.IncludePrompt ? MaskFormat.IncludePrompt : MaskFormat.ExcludePromptAndLiterals));
                }
                return MaskFormat.IncludeLiterals;
            }
            set
            {
                this.m_provider.IncludeLiterals = (value & MaskFormat.IncludeLiterals) == MaskFormat.IncludeLiterals;
                this.m_provider.IncludePrompt = (value & MaskFormat.IncludePrompt) == MaskFormat.IncludePrompt;
            }
        }

        public Type ValidatingType
        {
            get
            {
                return this.m_validatingType;
            }
            set
            {
                if (this.m_validatingType != value)
                {
                    this.m_validatingType = value;
                }
            }
        }

        public bool WordWrap
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
    }
}

