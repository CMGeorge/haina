namespace Resco.Controls.DetailView
{
    using Resco.Controls.MaskedTextBox;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class ItemMaskedTextBox : Item
    {
        internal DVMaskedTextBox EditControl;
        private bool m_beepOnError;
        private bool m_Border;
        private string m_mask;
        private char m_PasswordChar;
        private char m_promptChar;
        private bool m_ReadOnly;
        private bool m_resetOnPrompt;
        private bool m_resetOnSpace;
        private MaskFormat m_textMaskFormat;

        public ItemMaskedTextBox()
        {
            this.PasswordChar = '\0';
            this.ReadOnly = false;
            this.Border = false;
            this.BeepOnError = false;
            this.Mask = "";
            this.PromptChar = '_';
            this.ResetOnPrompt = true;
            this.ResetOnSpace = true;
            this.TextMaskFormat = MaskFormat.IncludeLiterals;
        }

        public ItemMaskedTextBox(Item toCopy) : base(toCopy)
        {
            if (toCopy is ItemMaskedTextBox)
            {
                this.Border = ((ItemMaskedTextBox) toCopy).Border;
                this.PasswordChar = ((ItemMaskedTextBox) toCopy).PasswordChar;
                this.ReadOnly = ((ItemMaskedTextBox) toCopy).ReadOnly;
            }
        }

        public ItemMaskedTextBox(string Label)
        {
            this.Label = Label;
            this.PasswordChar = '\0';
            this.ReadOnly = false;
            this.Border = false;
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
                    DVMaskedTextBox control = base.Parent.GetControl(typeof(DVMaskedTextBox)) as DVMaskedTextBox;
                    if (control != null)
                    {
                        control.MaskedTextBox.Font = base.TextFont;
                        control.MaskedTextBox.ReadOnly = this.ReadOnly;
                        if (control.MaskedTextBox.TextAlign != base.TextAlign)
                        {
                            control.MaskedTextBox.TextAlign = base.TextAlign;
                        }
                        control.ItemBorder = base.ItemBorder;
                        control.MaskedTextBox.BorderStyle = this.Border ? BorderStyle.FixedSingle : BorderStyle.None;
                        control.MaskedTextBox.PasswordChar = this.PasswordChar;
                        control.MaskedTextBox.BeepOnError = this.BeepOnError;
                        control.MaskedTextBox.Mask = this.Mask;
                        control.MaskedTextBox.PromptChar = this.PromptChar;
                        control.MaskedTextBox.ResetOnPrompt = this.ResetOnPrompt;
                        control.MaskedTextBox.ResetOnSpace = this.ResetOnSpace;
                        control.MaskedTextBox.TextMaskFormat = this.TextMaskFormat;
                        this.EditControl = control;
                        this.EditControl.ValueChanged += new EventHandler(this.OnValueChanged);
                        this.EditControl.Bounds = this.GetActivePartBounds(yOffset);
                        control.MaskedTextBox.Text = this.Text;
                        base.DisableEvents = false;
                        this.OnGotFocus(this, new ItemEventArgs(this, 0, base.Name));
                        base.DisableEvents = true;
                        if (this.EditControl != null)
                        {
                            this.EditControl.Show();
                            this.EditControl.Focus();
                        }
                    }
                    base.DisableEvents = false;
                    base.Click(yOffset, parentWidth);
                }
            }
        }

        public override Item Clone()
        {
            ItemMaskedTextBox box = new ItemMaskedTextBox(this);
            box.Value = this.Value;
            box.Text = this.Text;
            return box;
        }

        protected override void Hide()
        {
            if (this.EditControl != null)
            {
                this.Text = this.EditControl.Text;
                this.EditControl.ValueChanged -= new EventHandler(this.OnValueChanged);
                this.EditControl.Hide();
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

        internal void OnValueChanged(object sender, EventArgs e)
        {
            if (this.EditControl != null)
            {
                this.Text = this.EditControl.Text;
            }
        }

        protected virtual bool ShouldSerializePasswordChar()
        {
            return (this.m_PasswordChar != '\0');
        }

        protected virtual bool ShouldSerializePromptChar()
        {
            return (this.m_promptChar != '_');
        }

        protected virtual bool ShouldSerializeTextMaskFormat()
        {
            return (this.TextMaskFormat != MaskFormat.IncludeLiterals);
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
            return "MaskedTextBox";
        }

        protected override void UpdateControl(object value)
        {
            if (this.EditControl != null)
            {
                this.EditControl.Text = this.Text;
            }
        }

        [DefaultValue(false)]
        public bool BeepOnError
        {
            get
            {
                return this.m_beepOnError;
            }
            set
            {
                if (this.EditControl != null)
                {
                    this.EditControl.MaskedTextBox.BeepOnError = value;
                }
                this.m_beepOnError = value;
            }
        }

        [DefaultValue(false)]
        public bool Border
        {
            get
            {
                return this.m_Border;
            }
            set
            {
                if (this.EditControl != null)
                {
                    this.EditControl.MaskedTextBox.BorderStyle = value ? BorderStyle.FixedSingle : BorderStyle.None;
                }
                this.m_Border = value;
            }
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.EditControl;
            }
        }

        [DefaultValue("")]
        public string Mask
        {
            get
            {
                return this.m_mask;
            }
            set
            {
                if (this.EditControl != null)
                {
                    this.EditControl.MaskedTextBox.Mask = value;
                }
                this.m_mask = value;
            }
        }

        public char PasswordChar
        {
            get
            {
                return this.m_PasswordChar;
            }
            set
            {
                if (this.m_PasswordChar != value)
                {
                    this.m_PasswordChar = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.MaskedTextBox.PasswordChar = value;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue("_")]
        public char PromptChar
        {
            get
            {
                return this.m_promptChar;
            }
            set
            {
                if (this.EditControl != null)
                {
                    this.EditControl.MaskedTextBox.PromptChar = value;
                }
                this.m_promptChar = value;
            }
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get
            {
                return this.m_ReadOnly;
            }
            set
            {
                if (this.m_ReadOnly != value)
                {
                    this.m_ReadOnly = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.MaskedTextBox.ReadOnly = value;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(true)]
        public bool ResetOnPrompt
        {
            get
            {
                return this.m_resetOnPrompt;
            }
            set
            {
                if (this.EditControl != null)
                {
                    this.EditControl.MaskedTextBox.ResetOnPrompt = value;
                }
                this.m_resetOnPrompt = value;
            }
        }

        [DefaultValue(true)]
        public bool ResetOnSpace
        {
            get
            {
                return this.m_resetOnSpace;
            }
            set
            {
                if (this.EditControl != null)
                {
                    this.EditControl.MaskedTextBox.ResetOnSpace = value;
                }
                this.m_resetOnSpace = value;
            }
        }

        public MaskFormat TextMaskFormat
        {
            get
            {
                return this.m_textMaskFormat;
            }
            set
            {
                if (this.EditControl != null)
                {
                    this.EditControl.MaskedTextBox.TextMaskFormat = value;
                }
                this.m_textMaskFormat = value;
            }
        }
    }
}

