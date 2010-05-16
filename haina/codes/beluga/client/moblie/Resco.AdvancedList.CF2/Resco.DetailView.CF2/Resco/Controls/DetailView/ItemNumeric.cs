namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.Design;
    using Resco.Controls.NumericUpDown;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class ItemNumeric : Item
    {
        private Resco.Controls.NumericUpDown.NumericUpDown EditControl;
        private bool m_AllowNoneValue;
        private int m_decimals;
        private string m_displayFormat;
        private decimal m_increment;
        private decimal m_maximum;
        private decimal m_minimum;
        private string m_NoneText;
        private bool m_showArrows;
        private UpDownAlignment m_upDownAlign;
        private Resco.Controls.NumericUpDown.UpDownStyle m_upDownStyle;
        private int m_upDownWidth;

        public ItemNumeric()
        {
            this.m_displayFormat = "{0}";
            this.m_maximum = 100M;
            this.m_increment = 1M;
            this.Value = 0;
            this.m_showArrows = true;
            this.m_upDownAlign = UpDownAlignment.Right;
            this.m_upDownStyle = Resco.Controls.NumericUpDown.UpDownStyle.UpDown;
            this.m_upDownWidth = 12;
            this.m_AllowNoneValue = false;
            this.m_NoneText = "None";
        }

        public ItemNumeric(Item toCopy) : base(toCopy)
        {
            this.m_displayFormat = "{0}";
            this.m_maximum = 100M;
            this.m_increment = 1M;
            if (toCopy is ItemNumeric)
            {
                this.DecimalPlaces = ((ItemNumeric) toCopy).DecimalPlaces;
                this.Minimum = ((ItemNumeric) toCopy).Minimum;
                this.Maximum = ((ItemNumeric) toCopy).Maximum;
                this.Increment = ((ItemNumeric) toCopy).Increment;
                this.DisplayFormat = ((ItemNumeric) toCopy).DisplayFormat;
                this.ShowArrows = ((ItemNumeric) toCopy).ShowArrows;
                this.UpDownAlign = ((ItemNumeric) toCopy).UpDownAlign;
                this.UpDownStyle = ((ItemNumeric) toCopy).UpDownStyle;
                this.UpDownWidth = ((ItemNumeric) toCopy).UpDownWidth;
            }
        }

        protected override void Click(int yOffset, int parentWidth)
        {
            if (this.Enabled)
            {
                if (this.EditControl != null)
                {
                    this.EditControl.Focus();
                    this.OnGotFocus(this, new ItemEventArgs(this, 0, base.Name));
                }
                else
                {
                    base.DisableEvents = true;
                    Resco.Controls.DetailView.DetailView parent = base.Parent;
                    if (parent != null)
                    {
                        Resco.Controls.NumericUpDown.NumericUpDown control = parent.GetControl(typeof(Resco.Controls.NumericUpDown.NumericUpDown)) as Resco.Controls.NumericUpDown.NumericUpDown;
                        if (control != null)
                        {
                            control.DecimalPlaces = this.DecimalPlaces;
                            control.Font = base.TextFont;
                            control.Minimum = this.Minimum;
                            control.Maximum = this.Maximum;
                            control.Increment = this.Increment;
                            control.TextAlign = base.TextAlign;
                            control.UpDownWidth = this.UpDownWidth;
                            control.UpDownAlign = this.ShowArrows ? this.UpDownAlign : UpDownAlignment.None;
                            control.UpDownStyle = this.UpDownStyle;
                            control.NoneText = this.NoneText;
                            control.AllowNoneValue = this.AllowNoneValue;
                            if (this.Value == null)
                            {
                                control.NoneValue = true;
                            }
                            else
                            {
                                control.NoneValue = false;
                            }
                            control.Value = this.NumericValue;
                            control.ValueChanged += new EventHandler(this.OnValueChanged);
                            this.EditControl = control;
                            this.EditControl.Bounds = this.GetActivePartBounds(yOffset);
                            base.DisableEvents = false;
                            this.OnGotFocus(this, new ItemEventArgs(this, 0, base.Name));
                            base.DisableEvents = true;
                            if (this.EditControl != null)
                            {
                                this.EditControl.BringToFront();
                                this.EditControl.Show();
                                this.EditControl.Focus();
                            }
                        }
                        base.DisableEvents = false;
                        base.Click(yOffset, parentWidth);
                    }
                }
            }
        }

        public override Item Clone()
        {
            ItemNumeric numeric = new ItemNumeric(this);
            numeric.Value = this.Value;
            return numeric;
        }

        protected override string FormatValue(object value)
        {
            string displayFormat = this.m_displayFormat;
            if ((this.m_displayFormat == "{0}") && (this.m_decimals > 0))
            {
                displayFormat = "{0:0.";
                for (int i = 0; i < this.m_decimals; i++)
                {
                    displayFormat = displayFormat + "0";
                }
                displayFormat = displayFormat + "}";
            }
            return string.Format(displayFormat, value);
        }

        protected internal override bool HandleKey(Keys key)
        {
            if (key == Keys.Left)
            {
                decimal num = this.NumericValue - this.Increment;
                if (num >= this.Minimum)
                {
                    this.NumericValue = num;
                }
                return true;
            }
            if (key != Keys.Right)
            {
                return base.HandleKey(key);
            }
            decimal num2 = this.NumericValue + this.Increment;
            if (num2 <= this.Maximum)
            {
                this.NumericValue = num2;
            }
            return true;
        }

        protected override void Hide()
        {
            if (this.EditControl != null)
            {
                this.EditControl.UpdateValue();
                if (this.EditControl.NoneValue)
                {
                    this.Value = null;
                }
                else
                {
                    this.NumericValue = this.EditControl.Value;
                }
                this.EditControl.ValueChanged -= new EventHandler(this.OnValueChanged);
                this.EditControl.Visible = false;
            }
            base.Hide();
            this.EditControl = null;
        }

        protected override void MoveTop(int offset)
        {
            if (this.EditControl != null)
            {
                this.EditControl.Top += offset;
            }
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (this.EditControl != null)
            {
                if (this.AllowNoneValue && this.EditControl.NoneValue)
                {
                    this.Value = null;
                }
                else
                {
                    this.NumericValue = this.EditControl.Value;
                }
            }
        }

        protected internal override void ScaleItem(float fx, float fy)
        {
            base.ScaleItem(fx, fy);
            base.SetProperty("UpDownWidth", (int) (this.m_upDownWidth * fx));
        }

        protected virtual bool ShouldSerializeUpDownAlign()
        {
            return (this.m_upDownAlign != UpDownAlignment.Right);
        }

        protected virtual bool ShouldSerializeUpDownStyle()
        {
            return (this.m_upDownStyle != Resco.Controls.NumericUpDown.UpDownStyle.UpDown);
        }

        public override string ToString()
        {
            if (!(base.Name == "") && (base.Name != null))
            {
                return base.Name;
            }
            if (base.Site != null)
            {
                return base.Site.Name;
            }
            return "Numeric";
        }

        protected override void UpdateControl(object value)
        {
            if (this.EditControl != null)
            {
                if (this.Value == null)
                {
                    this.EditControl.NoneValue = true;
                }
                else
                {
                    this.EditControl.NoneValue = false;
                }
                this.EditControl.Value = this.NumericValue;
            }
        }

        [DefaultValue(false)]
        public bool AllowNoneValue
        {
            get
            {
                return this.m_AllowNoneValue;
            }
            set
            {
                if (this.m_AllowNoneValue != value)
                {
                    this.m_AllowNoneValue = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.AllowNoneValue = value;
                    }
                    if (!value)
                    {
                        this.Value = this.m_minimum;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.EditControl;
            }
        }

        [DefaultValue(0)]
        public int DecimalPlaces
        {
            get
            {
                return this.m_decimals;
            }
            set
            {
                if (this.m_decimals != value)
                {
                    this.m_decimals = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.DecimalPlaces = this.m_decimals;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue("{0}")]
        public string DisplayFormat
        {
            get
            {
                return this.m_displayFormat;
            }
            set
            {
                if ((value == null) || (value == ""))
                {
                    value = "{0}";
                }
                if (this.m_displayFormat != value)
                {
                    this.m_displayFormat = value;
                    this.OnPropertyChanged();
                }
            }
        }

        internal override bool Focused
        {
            get
            {
                if (this.EditControl != null)
                {
                    return this.EditControl.Focused;
                }
                return base.Focused;
            }
        }

        [DefaultValue(1)]
        public decimal Increment
        {
            get
            {
                return this.m_increment;
            }
            set
            {
                if (this.m_increment != value)
                {
                    this.m_increment = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.Increment = this.m_increment;
                    }
                }
            }
        }

        [DefaultValue(100)]
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
                    this.m_maximum = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.Maximum = this.m_maximum;
                    }
                    if (this.NumericValue > this.m_maximum)
                    {
                        this.NumericValue = this.m_maximum;
                    }
                    if (this.Minimum > this.m_maximum)
                    {
                        this.Minimum = this.m_maximum;
                    }
                }
            }
        }

        [DefaultValue(0)]
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
                    this.m_minimum = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.Minimum = this.m_minimum;
                    }
                    if (this.NumericValue < this.m_minimum)
                    {
                        this.NumericValue = this.m_minimum;
                    }
                    if (this.Maximum < this.m_minimum)
                    {
                        this.Maximum = this.m_minimum;
                    }
                }
            }
        }

        [DefaultValue("None")]
        public string NoneText
        {
            get
            {
                return this.m_NoneText;
            }
            set
            {
                if (this.m_NoneText != value)
                {
                    this.m_NoneText = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(0)]
        public decimal NumericValue
        {
            get
            {
                object obj2 = this.Value;
                if (obj2 == null)
                {
                    return 0M;
                }
                try
                {
                    return Convert.ToDecimal(obj2);
                }
                catch (Exception)
                {
                    return 0M;
                }
            }
            set
            {
                this.Value = value;
            }
        }

        [DefaultValue(true)]
        public bool ShowArrows
        {
            get
            {
                return this.m_showArrows;
            }
            set
            {
                if (this.m_showArrows != value)
                {
                    this.m_showArrows = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.UpDownAlign = this.m_showArrows ? this.m_upDownAlign : UpDownAlignment.None;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(""), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.DetailView.Design.Browsable(false)]
        public override string Text
        {
            get
            {
                if (this.Value == null)
                {
                    return this.m_NoneText;
                }
                return this.FormatValue(this.NumericValue);
            }
            set
            {
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
                    if (this.EditControl != null)
                    {
                        this.EditControl.UpDownAlign = this.m_showArrows ? this.m_upDownAlign : UpDownAlignment.None;
                    }
                    this.OnPropertyChanged();
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
                    if (this.EditControl != null)
                    {
                        this.EditControl.UpDownStyle = value;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(12)]
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
                    if (this.EditControl != null)
                    {
                        this.EditControl.UpDownWidth = value;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        public override object Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                decimal minimum;
                if (value == null)
                {
                    if (!this.m_AllowNoneValue)
                    {
                        if ((this.Minimum > 0M) || (this.Maximum < 0M))
                        {
                            minimum = this.Minimum;
                        }
                        else
                        {
                            minimum = 0M;
                        }
                    }
                    else
                    {
                        minimum = this.Minimum;
                    }
                }
                else
                {
                    minimum = Convert.ToDecimal(value);
                }
                if ((minimum < this.Minimum) || (minimum > this.Maximum))
                {
                    throw new ArgumentOutOfRangeException("Must be between Minimum and Maximum!");
                }
                if (this.m_AllowNoneValue && (value == null))
                {
                    base.Value = null;
                }
                else
                {
                    base.Value = minimum;
                }
            }
        }
    }
}

