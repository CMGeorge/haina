namespace Resco.Controls.NumericUpDown
{
    using Resco.Controls.DetailView;
    using Resco.Controls.DetailView.DetailViewInternal;
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class NumericUpDown : UserControl
    {
        private int ConstTextBoxBorder = 2;
        private bool m_allowNoneValue = false;
        private int m_decimalPlaces = -1;
        private Color m_focusedColor = Color.Transparent;
        private string m_format = "{0}";
        private string m_formatString = "";
        private string m_formatStringDefault = "{0}";
        private GradientColor m_gradientColors = new GradientColor(Color.LightGray, Color.Black);
        private bool m_hexadecimal = false;
        private decimal m_increment = 1M;
        private decimal m_maximum = 100M;
        private decimal m_minimum = 0M;
        private SpinButton m_minusButton;
        private string m_noneText = "None";
        private bool m_noneValue = false;
        private SpinButton m_plusButton;
        private bool m_readOnly = false;
        private HorizontalAlignment m_textAlign = HorizontalAlignment.Left;
        private TextBox m_textBox;
        private bool m_thousandsSeparator = false;
        private UpDownAlignment m_upDownAlign = UpDownAlignment.Right;
        private Resco.Controls.NumericUpDown.UpDownStyle m_upDownStyle = Resco.Controls.NumericUpDown.UpDownStyle.UpDown;
        private int m_upDownWidth = 12;
        private decimal m_value = 0M;
        private Resco.Controls.NumericUpDown.VisualStyle m_visualStyle = Resco.Controls.NumericUpDown.VisualStyle.Colors;

        public event EventHandler ValueChanged;

        static NumericUpDown()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(Resco.Controls.NumericUpDown.NumericUpDown), "");
            //}
        }

        public NumericUpDown()
        {
            this.m_gradientColors.PropertyChanged += new EventHandler(this.OnGradientColorPropertyChanged);
            this.m_textBox = new TextBox();
            this.m_textBox.BorderStyle = BorderStyle.None;
            this.m_textBox.Text = "0";
            this.m_textBox.KeyDown += new KeyEventHandler(this.OnTextBoxKeyDown);
            this.m_textBox.KeyPress += new KeyPressEventHandler(this.OnTextBoxKeyPress);
            this.m_textBox.KeyUp += new KeyEventHandler(this.OnTextBoxKeyUp);
            this.m_textBox.GotFocus += new EventHandler(this.OnTextBoxGotFocus);
            this.m_textBox.LostFocus += new EventHandler(this.OnTextBoxLostFocus);
            this.m_textBox.Disposed += new EventHandler(this.OnTextBoxDisposed);
            this.m_textBox.Multiline = true;
            this.m_textBox.TextAlign = this.m_textAlign;
            this.m_textBox.Visible = false;
            Imports.SetInputMode(this.m_textBox, Imports.InputMode.Numbers);
            this.m_plusButton = new SpinButton();
            this.m_plusButton.TabStop = false;
            this.m_plusButton.DefaultAdornmentType = AdornmentType.UpArrow;
            this.m_plusButton.AdornmentType = AdornmentType.UpArrow;
            this.m_plusButton.BorderStyle = RescoBorderStyle.Left;
            this.m_plusButton.SpinChanged += new SpinChangedEventHandler(this.OnPlusButtonSpinChanged);
            this.m_minusButton = new SpinButton();
            this.m_minusButton.TabStop = false;
            this.m_minusButton.DefaultAdornmentType = AdornmentType.DownArrow;
            this.m_minusButton.AdornmentType = AdornmentType.DownArrow;
            this.m_minusButton.BorderStyle = RescoBorderStyle.Left;
            this.m_minusButton.SpinChanged += new SpinChangedEventHandler(this.OnMinusButtonSpinChanged);
            base.Controls.Add(this.m_plusButton);
            base.Controls.Add(this.m_minusButton);
            base.Controls.Add(this.m_textBox);
            base.BorderStyle=(BorderStyle.FixedSingle);
            base.BackColor = SystemColors.Window;
            base.Size = new Size(100, 0x16);
            this.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_textBox != null)
                {
                    this.m_textBox.Dispose();
                }
                if (this.m_plusButton != null)
                {
                    this.m_plusButton.Dispose();
                }
                if (this.m_minusButton != null)
                {
                    this.m_minusButton.Dispose();
                }
            }
            this.m_textBox = null;
            this.m_plusButton = null;
            this.m_minusButton = null;
            base.Dispose(disposing);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            try
            {
                this.TextBoxShow();
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private void OnGradientColorPropertyChanged(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if ((this.Site != null) && this.Site.DesignMode)
            {
                base.Controls.Remove(this.m_textBox);
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

        private void OnMinusButtonSpinChanged(object sender, SpinChangedEventArgs e)
        {
            if (!this.Focused)
            {
                this.TextBoxShow();
            }
            this.ParseText(true, false);
            this.Value -= this.m_increment * Convert.ToDecimal(Math.Pow(10.0, (double) (e.IncreaseRate - 1)));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.TextBoxShow();
            base.OnMouseDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(this.ForeColor))
            {
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                switch (this.m_textAlign)
                {
                    case HorizontalAlignment.Right:
                        format.Alignment = StringAlignment.Far;
                        break;

                    case HorizontalAlignment.Center:
                        format.Alignment = StringAlignment.Center;
                        break;

                    default:
                        format.Alignment = StringAlignment.Near;
                        break;
                }
                e.Graphics.DrawString(this.Text, this.Font, brush, this.m_textBox.Bounds, format);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.Focused && !this.ReadOnly)
            {
                e.Graphics.Clear(this.m_textBox.BackColor);
            }
            else if (this.VisualStyle == Resco.Controls.NumericUpDown.VisualStyle.VistaStyle)
            {
                GradientFill.DrawVistaGradient(e.Graphics, this.BackColor, new Rectangle(0, 0, base.Width, base.Height), FillDirection.Vertical);
            }
            else if (this.VisualStyle == Resco.Controls.NumericUpDown.VisualStyle.Gradients)
            {
                GradientFill.Fill(e.Graphics, base.ClientRectangle, base.ClientRectangle, this.m_gradientColors);
            }
            else
            {
                base.OnPaintBackground(e);
            }
        }

        private void OnPlusButtonSpinChanged(object sender, SpinChangedEventArgs e)
        {
            if (!this.Focused)
            {
                this.TextBoxShow();
            }
            this.ParseText(true, false);
            this.Value += this.m_increment * Convert.ToDecimal(Math.Pow(10.0, (double) (e.IncreaseRate - 1)));
        }

        protected override void OnResize(EventArgs e)
        {
            this.PositionControls();
            base.OnResize(e);
        }

        private void OnTextBoxDisposed(object sender, EventArgs e)
        {
            this.m_textBox = null;
        }

        private void OnTextBoxGotFocus(object sender, EventArgs e)
        {
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            this.OnKeyDown(e);
            if (!e.Handled)
            {
                if (e.KeyCode == Keys.Up)
                {
                    this.OnPlusButtonSpinChanged(this, new SpinChangedEventArgs(1));
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    this.OnMinusButtonSpinChanged(this, new SpinChangedEventArgs(1));
                    e.Handled = true;
                }
            }
        }

        private void OnTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            string numberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if ((((e.KeyChar < '0') || (e.KeyChar > '9')) && (!this.m_hexadecimal || (((e.KeyChar < 'a') || (e.KeyChar > 'f')) && ((e.KeyChar < 'A') || (e.KeyChar > 'F'))))) && ((((this.m_hexadecimal || (Convert.ToString(e.KeyChar) != numberDecimalSeparator)) || (this.m_textBox.Text.IndexOf(numberDecimalSeparator) >= 0)) && ((this.m_hexadecimal || (e.KeyChar != '-')) || (this.m_textBox.SelectionStart != 0))) && ((e.KeyChar != ' ') && (e.KeyChar != '\b'))))
            {
                if (e.KeyChar == '\r')
                {
                    this.ParseText(false, true);
                    e.Handled = true;
                }
                else
                {
                    e.Handled = true;
                }
            }
            if (!e.Handled)
            {
                this.OnKeyPress(e);
            }
        }

        private void OnTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            this.OnKeyUp(e);
        }

        private void OnTextBoxLostFocus(object sender, EventArgs e)
        {
            if (!this.Focused)
            {
                this.TextBoxHide();
            }
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, EventArgs.Empty);
            }
        }

        private void ParseText(bool bValueChange, bool allowNull)
        {
            try
            {
                bool flag = this.m_textBox.Text == "";
                if (this.AllowNoneValue && (flag || (this.m_textBox.Text == this.m_noneText)))
                {
                    if (allowNull && (!bValueChange || flag))
                    {
                        this.NoneValue = true;
                    }
                    else
                    {
                        this.NoneValue = false;
                    }
                }
                else
                {
                    if (this.m_hexadecimal)
                    {
                        this.Value = long.Parse(this.m_textBox.Text.Replace(" ", ""), NumberStyles.HexNumber);
                    }
                    else
                    {
                        this.Value = decimal.Parse(this.m_textBox.Text.Replace(" ", ""));
                    }
                    this.NoneValue = false;
                }
            }
            catch
            {
                this.NoneValue = false;
                this.UpdateText();
            }
        }

        private void PositionControls()
        {
            int num;
            Rectangle rectangle;
            Rectangle rectangle2;
            Rectangle rectangle3;
            Size clientSize = base.ClientSize;
            SizeF ef = new SizeF((float) clientSize.Width, (float) clientSize.Height);
            using (Graphics graphics = base.CreateGraphics())
            {
                ef = graphics.MeasureString((this.m_textBox.Text != "") ? this.m_textBox.Text : "0", this.m_textBox.Font);
            }
            bool flag = true;
            switch (this.m_upDownAlign)
            {
                case UpDownAlignment.Left:
                    rectangle = new Rectangle((2 * this.UpDownWidth) + this.ConstTextBoxBorder, (clientSize.Height - ((int) ef.Height)) / 2, (clientSize.Width - (2 * this.UpDownWidth)) - (2 * this.ConstTextBoxBorder), (int) ef.Height);
                    rectangle2 = new Rectangle(0, 0, this.UpDownWidth, clientSize.Height);
                    rectangle3 = new Rectangle(this.UpDownWidth, 0, this.UpDownWidth, clientSize.Height);
                    if (this.m_plusButton != null)
                    {
                        this.m_plusButton.BorderStyle = RescoBorderStyle.Right;
                    }
                    if (this.m_minusButton != null)
                    {
                        this.m_minusButton.BorderStyle = RescoBorderStyle.Right;
                    }
                    break;

                case UpDownAlignment.HorizontalSplit:
                    rectangle = new Rectangle(this.UpDownWidth + this.ConstTextBoxBorder, (clientSize.Height - ((int) ef.Height)) / 2, (clientSize.Width - (2 * this.UpDownWidth)) - (2 * this.ConstTextBoxBorder), (int) ef.Height);
                    rectangle2 = new Rectangle(0, 0, this.UpDownWidth, clientSize.Height);
                    rectangle3 = new Rectangle(clientSize.Width - this.UpDownWidth, 0, this.UpDownWidth, clientSize.Height);
                    if (this.m_plusButton != null)
                    {
                        this.m_plusButton.BorderStyle = RescoBorderStyle.Right;
                    }
                    if (this.m_minusButton != null)
                    {
                        this.m_minusButton.BorderStyle = RescoBorderStyle.Left;
                    }
                    break;

                case UpDownAlignment.Up:
                    num = ((clientSize.Height - this.UpDownWidth) - ((int) ef.Height)) / 2;
                    rectangle = new Rectangle(this.ConstTextBoxBorder, this.UpDownWidth + ((num > 0) ? num : 0), clientSize.Width - (2 * this.ConstTextBoxBorder), (int) ef.Height);
                    rectangle2 = new Rectangle(0, 0, clientSize.Width / 2, this.UpDownWidth);
                    rectangle3 = new Rectangle(clientSize.Width / 2, 0, clientSize.Width / 2, this.UpDownWidth);
                    if (this.m_plusButton != null)
                    {
                        this.m_plusButton.BorderStyle = RescoBorderStyle.Bottom | RescoBorderStyle.Right;
                    }
                    if (this.m_minusButton != null)
                    {
                        this.m_minusButton.BorderStyle = RescoBorderStyle.Bottom;
                    }
                    break;

                case UpDownAlignment.Down:
                    num = ((clientSize.Height - this.UpDownWidth) - ((int) ef.Height)) / 2;
                    rectangle = new Rectangle(this.ConstTextBoxBorder, (num > 0) ? num : (2 * num), clientSize.Width - (2 * this.ConstTextBoxBorder), (int) ef.Height);
                    rectangle2 = new Rectangle(0, clientSize.Height - this.UpDownWidth, clientSize.Width / 2, this.UpDownWidth);
                    rectangle3 = new Rectangle(clientSize.Width / 2, clientSize.Height - this.UpDownWidth, clientSize.Width / 2, this.UpDownWidth);
                    if (this.m_plusButton != null)
                    {
                        this.m_plusButton.BorderStyle = RescoBorderStyle.Top | RescoBorderStyle.Right;
                    }
                    if (this.m_minusButton != null)
                    {
                        this.m_minusButton.BorderStyle = RescoBorderStyle.Top;
                    }
                    break;

                case UpDownAlignment.VerticalSplit:
                    rectangle = new Rectangle(this.ConstTextBoxBorder, (clientSize.Height - ((int) ef.Height)) / 2, clientSize.Width - (2 * this.ConstTextBoxBorder), (int) ef.Height);
                    rectangle2 = new Rectangle(0, 0, clientSize.Width, this.UpDownWidth);
                    rectangle3 = new Rectangle(0, clientSize.Height - this.UpDownWidth, clientSize.Width, this.UpDownWidth);
                    if (this.m_plusButton != null)
                    {
                        this.m_plusButton.BorderStyle = RescoBorderStyle.Bottom;
                    }
                    if (this.m_minusButton != null)
                    {
                        this.m_minusButton.BorderStyle = RescoBorderStyle.Top;
                    }
                    break;

                case UpDownAlignment.None:
                    rectangle = new Rectangle(this.ConstTextBoxBorder, (clientSize.Height - ((int) ef.Height)) / 2, clientSize.Width - (2 * this.ConstTextBoxBorder), (int) ef.Height);
                    rectangle2 = new Rectangle(0, 0, 1, 1);
                    rectangle3 = new Rectangle(0, 0, 1, 1);
                    flag = false;
                    break;

                default:
                    rectangle = new Rectangle(this.ConstTextBoxBorder, (clientSize.Height - ((int) ef.Height)) / 2, (clientSize.Width - (2 * this.UpDownWidth)) - (2 * this.ConstTextBoxBorder), (int) ef.Height);
                    rectangle2 = new Rectangle(clientSize.Width - (2 * this.UpDownWidth), 0, this.UpDownWidth, clientSize.Height);
                    rectangle3 = new Rectangle(clientSize.Width - this.UpDownWidth, 0, this.UpDownWidth, clientSize.Height);
                    if (this.m_plusButton != null)
                    {
                        this.m_plusButton.BorderStyle = RescoBorderStyle.Left;
                    }
                    if (this.m_minusButton != null)
                    {
                        this.m_minusButton.BorderStyle = RescoBorderStyle.Left;
                    }
                    break;
            }
            if (this.m_plusButton != null)
            {
                this.m_plusButton.Visible = flag;
            }
            if (this.m_minusButton != null)
            {
                this.m_minusButton.Visible = flag;
            }
            if (this.m_textBox != null)
            {
                this.m_textBox.Bounds = rectangle;
            }
            if (this.m_upDownStyle == Resco.Controls.NumericUpDown.UpDownStyle.UpDown)
            {
                if (this.m_plusButton != null)
                {
                    this.m_plusButton.Bounds = rectangle2;
                }
                if (this.m_minusButton != null)
                {
                    this.m_minusButton.Bounds = rectangle3;
                }
            }
            else
            {
                if (this.m_plusButton != null)
                {
                    this.m_plusButton.Bounds = rectangle3;
                }
                if (this.m_minusButton != null)
                {
                    this.m_minusButton.Bounds = rectangle2;
                }
                RescoBorderStyle borderStyle = this.m_plusButton.BorderStyle;
                if (this.m_plusButton != null)
                {
                    this.m_plusButton.BorderStyle = this.m_minusButton.BorderStyle;
                }
                if (this.m_minusButton != null)
                {
                    this.m_minusButton.BorderStyle = borderStyle;
                }
            }
            base.Invalidate();
        }

        public override void Refresh()
        {
            this.PositionControls();
            base.Refresh();
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            if (factor.Width != 1f)
            {
                this.m_upDownWidth = (int) (this.m_upDownWidth * factor.Width);
            }
            SizeF ef = base.CurrentAutoScaleDimensions;//.get_CurrentAutoScaleDimensions();
            ContainerControl topLevelControl = base.TopLevelControl as ContainerControl;
            if (topLevelControl != null)
            {
                ef = topLevelControl.CurrentAutoScaleDimensions;//.get_CurrentAutoScaleDimensions();
            }
            if (ef.Width > 96f)
            {
                this.ConstTextBoxBorder = (int) ((this.ConstTextBoxBorder * ef.Width) / 96f);
            }
            base.ScaleControl(factor, specified);
        }

        protected virtual bool ShouldSerializeGradientColors()
        {
            return (((this.m_gradientColors.StartColor != Color.LightGray) | (this.m_gradientColors.EndColor != Color.Black)) | (this.m_gradientColors.FillDirection != FillDirection.Vertical));
        }

        protected virtual bool ShouldSerializeUpDownAlign()
        {
            return (this.m_upDownAlign != UpDownAlignment.Right);
        }

        protected virtual bool ShouldSerializeUpDownStyle()
        {
            return (this.m_upDownStyle != Resco.Controls.NumericUpDown.UpDownStyle.UpDown);
        }

        protected virtual bool ShouldSerializeVisualStyle()
        {
            return (this.m_visualStyle != Resco.Controls.NumericUpDown.VisualStyle.Colors);
        }

        private void TextBoxHide()
        {
            this.ParseText(false, true);
            this.m_textBox.Hide();
            base.Invalidate();
            base.OnLostFocus(EventArgs.Empty);
        }

        private void TextBoxShow()
        {
            if (!this.ReadOnly)
            {
                this.UpdateText();
                this.m_textBox.Show();
                this.m_textBox.Focus();
                this.m_textBox.SelectionStart = 0;
                this.m_textBox.SelectionLength = this.m_textBox.Text.Length;
            }
            else
            {
                base.Focus();
            }
            base.Invalidate();
            base.OnGotFocus(EventArgs.Empty);
        }

        private void UpdateFormat()
        {
            if (this.m_hexadecimal)
            {
                this.m_format = "{0:X}";
            }
            else if (this.m_decimalPlaces < 0)
            {
                if (this.m_thousandsSeparator)
                {
                    this.m_format = "{0:#,0.##########}";
                }
                else
                {
                    this.m_format = "{0}";
                }
            }
            else
            {
                if (this.m_thousandsSeparator)
                {
                    this.m_format = "{0:#,0.";
                }
                else
                {
                    this.m_format = "{0:0.";
                }
                for (int i = 0; i < this.m_decimalPlaces; i++)
                {
                    this.m_format = this.m_format + "0";
                }
                this.m_format = this.m_format + "}";
            }
            int index = this.m_formatStringDefault.IndexOf('{');
            if (index < 0)
            {
                index = 0;
            }
            int num3 = this.m_formatStringDefault.IndexOf('}');
            if (num3 < 0)
            {
                num3 = this.m_formatStringDefault.Length - 1;
            }
            this.m_formatStringDefault = this.m_formatStringDefault.Remove(index, (num3 - index) + 1);
            this.m_formatStringDefault = this.m_formatStringDefault.Insert(index, this.m_format);
            this.UpdateText();
        }

        private void UpdateText()
        {
            if (this.m_textBox != null)
            {
                if (this.m_noneValue)
                {
                    this.m_textBox.Text = this.m_noneText;
                }
                else if (this.m_hexadecimal)
                {
                    this.m_textBox.Text = string.Format(this.m_format, Convert.ToInt64(this.m_value));
                }
                else
                {
                    this.m_textBox.Text = string.Format(this.m_format, this.m_value);
                }
                this.m_textBox.SelectionStart = 0;
                this.m_textBox.SelectionLength = this.m_textBox.Text.Length;
                base.Invalidate();
            }
        }

        public void UpdateValue()
        {
            this.ParseText(false, true);
            base.Invalidate();
        }

        private void ValidateValue()
        {
            if (this.m_value < this.m_minimum)
            {
                this.m_value = this.m_minimum;
            }
            if (this.Value > this.m_maximum)
            {
                this.m_value = this.m_maximum;
            }
        }

        public bool AllowNoneValue
        {
            get
            {
                return this.m_allowNoneValue;
            }
            set
            {
                if (this.m_allowNoneValue != value)
                {
                    this.m_allowNoneValue = value;
                    this.m_noneValue = this.m_allowNoneValue ? this.m_noneValue : false;
                    this.UpdateText();
                }
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
                base.BackColor = value;
                if (this.m_focusedColor == Color.Transparent)
                {
                    this.m_textBox.BackColor = base.BackColor;
                }
            }
        }

        public int DecimalPlaces
        {
            get
            {
                return this.m_decimalPlaces;
            }
            set
            {
                if (this.m_decimalPlaces != value)
                {
                    this.m_decimalPlaces = value;
                    this.UpdateFormat();
                }
            }
        }

        public override bool Focused
        {
            get
            {
                return (base.Focused | ((this.m_textBox != null) ? this.m_textBox.Focused : false));
            }
        }

        public Color FocusedColor
        {
            get
            {
                return this.m_focusedColor;
            }
            set
            {
                if (value == Color.Empty)
                {
                    value = Color.Transparent;
                }
                if (value != this.m_focusedColor)
                {
                    this.m_focusedColor = value;
                    this.m_textBox.BackColor = (this.m_focusedColor == Color.Transparent) ? this.BackColor : this.m_focusedColor;
                }
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                this.m_textBox.Font = value;
                this.Refresh();
            }
        }

        public string FormatString
        {
            get
            {
                return this.m_formatString;
            }
            set
            {
                if (this.m_formatString != value)
                {
                    this.m_formatString = value;
                    this.UpdateFormat();
                }
            }
        }

        public GradientColor GradientColors
        {
            get
            {
                return this.m_gradientColors;
            }
            set
            {
                if (this.m_gradientColors != value)
                {
                    this.m_gradientColors.PropertyChanged -= new EventHandler(this.OnGradientColorPropertyChanged);
                    this.m_gradientColors = null;
                    this.m_gradientColors = value;
                    this.m_gradientColors.PropertyChanged += new EventHandler(this.OnGradientColorPropertyChanged);
                    base.Invalidate();
                }
            }
        }

        public bool Hexadecimal
        {
            get
            {
                return this.m_hexadecimal;
            }
            set
            {
                if (this.m_hexadecimal != value)
                {
                    this.m_hexadecimal = value;
                    this.UpdateFormat();
                }
            }
        }

        public decimal Increment
        {
            get
            {
                return this.m_increment;
            }
            set
            {
                this.m_increment = value;
            }
        }

        public decimal Maximum
        {
            get
            {
                return this.m_maximum;
            }
            set
            {
                if (this.m_maximum != value)
                {
                    if (value < this.m_minimum)
                    {
                        value = this.m_minimum;
                    }
                    this.m_maximum = value;
                    this.ValidateValue();
                }
            }
        }

        public decimal Minimum
        {
            get
            {
                return this.m_minimum;
            }
            set
            {
                if (this.m_minimum != value)
                {
                    if (value > this.m_maximum)
                    {
                        value = this.m_maximum;
                    }
                    this.m_minimum = value;
                    this.ValidateValue();
                }
            }
        }

        public SpinButton MinusButton
        {
            get
            {
                return this.m_minusButton;
            }
        }

        public string NoneText
        {
            get
            {
                return this.m_noneText;
            }
            set
            {
                if (this.m_noneText != value)
                {
                    this.m_noneText = value;
                    this.UpdateText();
                }
            }
        }

        public bool NoneValue
        {
            get
            {
                return this.m_noneValue;
            }
            set
            {
                if (this.m_noneValue != value)
                {
                    this.m_noneValue = this.m_allowNoneValue ? value : false;
                    this.UpdateText();
                    this.OnValueChanged(EventArgs.Empty);
                }
            }
        }

        public SpinButton PlusButton
        {
            get
            {
                return this.m_plusButton;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this.m_readOnly;
            }
            set
            {
                if (this.m_readOnly != value)
                {
                    this.m_readOnly = value;
                    if (this.m_readOnly)
                    {
                        this.TextBoxHide();
                    }
                }
            }
        }

        public override string Text
        {
            get
            {
                string format = (this.m_formatString == "") ? this.m_formatStringDefault : this.m_formatString;
                if (this.m_noneValue)
                {
                    return this.m_noneText;
                }
                if (this.m_hexadecimal)
                {
                    return string.Format(format, Convert.ToInt64(this.m_value));
                }
                return string.Format(format, this.m_value);
            }
            set
            {
            }
        }

        public HorizontalAlignment TextAlign
        {
            get
            {
                return this.m_textAlign;
            }
            set
            {
                if (this.m_textAlign != value)
                {
                    this.m_textAlign = value;
                    if (this.m_textBox != null)
                    {
                        this.m_textBox.TextAlign = value;
                    }
                    base.Invalidate();
                }
            }
        }

        public bool ThousandsSeparator
        {
            get
            {
                return this.m_thousandsSeparator;
            }
            set
            {
                if (this.m_thousandsSeparator != value)
                {
                    this.m_thousandsSeparator = value;
                    this.UpdateFormat();
                }
            }
        }

        public UpDownAlignment UpDownAlign
        {
            get
            {
                return this.m_upDownAlign;
            }
            set
            {
                if (this.m_upDownAlign != value)
                {
                    this.m_upDownAlign = value;
                    this.Refresh();
                }
            }
        }

        public Resco.Controls.NumericUpDown.UpDownStyle UpDownStyle
        {
            get
            {
                return this.m_upDownStyle;
            }
            set
            {
                if (this.m_upDownStyle != value)
                {
                    this.m_upDownStyle = value;
                    this.Refresh();
                }
            }
        }

        public int UpDownWidth
        {
            get
            {
                return this.m_upDownWidth;
            }
            set
            {
                if (this.m_upDownWidth != value)
                {
                    this.m_upDownWidth = value;
                    this.Refresh();
                }
            }
        }

        public decimal Value
        {
            get
            {
                return this.m_value;
            }
            set
            {
                if (this.m_value != value)
                {
                    this.m_value = value;
                    this.ValidateValue();
                    this.UpdateText();
                    this.OnValueChanged(EventArgs.Empty);
                }
            }
        }

        public Resco.Controls.NumericUpDown.VisualStyle VisualStyle
        {
            get
            {
                return this.m_visualStyle;
            }
            set
            {
                if (this.m_visualStyle != value)
                {
                    this.m_visualStyle = value;
                    base.Invalidate();
                }
            }
        }
    }
}

