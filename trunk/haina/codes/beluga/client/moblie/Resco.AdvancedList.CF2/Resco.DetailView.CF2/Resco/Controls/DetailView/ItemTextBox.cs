namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ItemTextBox : Item
    {
        internal DVTextBox EditControl;
        private bool m_Border;
        private string m_displayFormat;
        private string m_displayText;
        private int m_MaxLength;
        private bool m_MultiLine;
        private char m_PasswordChar;
        private bool m_ReadOnly;
        private System.Windows.Forms.ScrollBars m_ScrollBars;
        private string m_SelectedText;
        private int m_SelectionLength;
        private int m_SelectionStart;
        private bool m_TextTooLong;
        private bool m_WordWrap;

        public ItemTextBox()
        {
            this.m_displayText = "";
            this.m_displayFormat = "";
            this.MaxLength = 0;
            this.MultiLine = false;
            this.PasswordChar = '\0';
            this.WordWrap = false;
            this.ReadOnly = false;
            this.m_SelectedText = "";
            this.m_SelectionLength = 0;
            this.m_SelectionStart = -1;
            this.Border = false;
            this.m_TextTooLong = false;
        }

        public ItemTextBox(Item toCopy) : base(toCopy)
        {
            this.m_displayText = "";
            this.m_displayFormat = "";
            if (toCopy is ItemTextBox)
            {
                this.Border = ((ItemTextBox) toCopy).Border;
                this.MaxLength = ((ItemTextBox) toCopy).MaxLength;
                this.MultiLine = ((ItemTextBox) toCopy).MultiLine;
                this.PasswordChar = ((ItemTextBox) toCopy).PasswordChar;
                this.WordWrap = ((ItemTextBox) toCopy).WordWrap;
                this.ReadOnly = ((ItemTextBox) toCopy).ReadOnly;
                this.DisplayFormat = ((ItemTextBox) toCopy).DisplayFormat;
                this.ScrollBars = ((ItemTextBox) toCopy).ScrollBars;
            }
            this.m_SelectedText = "";
            this.m_SelectionLength = 0;
            this.m_SelectionStart = -1;
        }

        public ItemTextBox(string Label)
        {
            this.m_displayText = "";
            this.m_displayFormat = "";
            this.Label = Label;
            this.MaxLength = 0;
            this.MultiLine = false;
            this.PasswordChar = '\0';
            this.WordWrap = false;
            this.ReadOnly = false;
            this.m_SelectedText = "";
            this.m_SelectionLength = 0;
            this.m_SelectionStart = -1;
            this.Border = false;
        }

        internal override Size _MeasureTextSize(Graphics gr, Font font, string text, int width, bool wrap, bool label)
        {
            if (this.m_Border)
            {
                width -= 4 * DVTextBox.BorderSize;
            }
            else
            {
                width -= 2 * DVTextBox.BorderSize;
            }
            Size size = base.MeasureTextSize(gr, font, text, width, label || (this.m_MultiLine ? this.m_WordWrap : false));
            if (this.m_Border)
            {
                size.Height += 4 * DVTextBox.BorderSize;
                return size;
            }
            size.Height += 2 * DVTextBox.BorderSize;
            return size;
        }

        internal override bool CheckForToolTip(int x, int y, int itemX, int itemWidth, bool bShow)
        {
            if (base.CheckForToolTip(x, y, itemX, itemWidth, bShow))
            {
                return true;
            }
            if ((base.Parent != null) && this.m_TextTooLong)
            {
                Rectangle activePartBounds = this.GetActivePartBounds(0);
                activePartBounds.Width -= 2 * DVTextBox.BorderSize;
                if ((x > (activePartBounds.Right - Resco.Controls.DetailView.DetailView.TooltipWidth)) && (x < activePartBounds.Right))
                {
                    base.Parent.ShowToolTip(this.m_displayText, x, y, bShow);
                    return true;
                }
            }
            return false;
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
                    DVTextBox control = base.Parent.GetControl(typeof(DVTextBox)) as DVTextBox;
                    if (control != null)
                    {
                        control.LineAlign = base.LineAlign;
                        control.TextBox.Font = base.TextFont;
                        control.TextBox.ReadOnly = this.ReadOnly;
                        control.TextBox.Text = this.Text;
                        control.TextBox.ScrollBars = this.ScrollBars;
                        if (control.TextBox.Multiline != this.MultiLine)
                        {
                            control.TextBox.Multiline = this.MultiLine;
                        }
                        if (control.TextBox.TextAlign != base.TextAlign)
                        {
                            control.TextBox.TextAlign = base.TextAlign;
                        }
                        control.ItemBorder = base.ItemBorder;
                        control.BorderStyle = this.Border ? BorderStyle.FixedSingle : BorderStyle.None;
                        control.TextBox.MaxLength = this.MaxLength;
                        control.TextBox.PasswordChar = this.PasswordChar;
                        if (control.TextBox.WordWrap != this.WordWrap)
                        {
                            control.TextBox.WordWrap = this.WordWrap;
                        }
                        if (this.SelectionStart < 0)
                        {
                            control.TextBox.SelectionStart = control.Text.Length;
                        }
                        else
                        {
                            control.TextBox.SelectionStart = this.SelectionStart;
                            control.TextBox.SelectionLength = this.SelectionLength;
                        }
                        this.EditControl = control;
                        this.EditControl.ValueChanged += new EventHandler(this.OnValueChanged);
                        this.EditControl.Bounds = this.GetActivePartBounds(yOffset);
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
            ItemTextBox box = new ItemTextBox(this);
            box.Value = this.Value;
            box.Text = this.Text;
            return box;
        }

        protected override void DrawItemTextArea(Graphics gr, ref Rectangle textBounds)
        {
            if (!base.DrawAlignmentString(gr, this.m_displayText, base.TextFont, base.GetTextForeBrush(), textBounds, base.TextAlign, base.LineAlign, this.m_MultiLine ? this.m_WordWrap : false) && Resco.Controls.DetailView.DetailView.ShowTextTooLong)
            {
                int right = textBounds.Right;
                int bottom = textBounds.Bottom;
                Point[] points = new Point[] { new Point(right, bottom - Resco.Controls.DetailView.DetailView.TooltipWidth), new Point(right - Resco.Controls.DetailView.DetailView.TooltipWidth, bottom), new Point(right, bottom) };
                gr.FillPolygon(base.GetTextForeBrush(), points);
                this.m_TextTooLong = true;
            }
            else
            {
                this.m_TextTooLong = false;
            }
        }

        protected internal override bool HandleKey(Keys key)
        {
            int num;
            return ((((key == Keys.Up) && (this.EditControl != null)) && (((num = this.EditControl.Text.IndexOf(Convert.ToChar(13))) > 0) && (num < (this.EditControl.TextBox.SelectionStart + this.EditControl.TextBox.SelectionLength)))) || (((key == Keys.Down) && (this.EditControl != null)) && (((num = this.EditControl.Text.LastIndexOf(Convert.ToChar(13))) > 0) && (num > (this.EditControl.TextBox.SelectionStart + this.EditControl.TextBox.SelectionLength)))));
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

        protected override void OnPropertyChanged()
        {
            if (this.m_displayFormat == "")
            {
                this.m_displayText = this.Text;
            }
            else
            {
                this.m_displayText = string.Format(this.m_displayFormat, this.Value);
            }
            if (this.m_displayText == null)
            {
                this.m_displayText = "";
            }
            if ((this.PasswordChar != '\0') && !this.MultiLine)
            {
                this.m_displayText = new string(this.PasswordChar, this.m_displayText.Length);
            }
            base.OnPropertyChanged();
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
            return "TextBox";
        }

        protected override void UpdateControl(object value)
        {
            if (this.EditControl != null)
            {
                this.EditControl.Text = this.Text;
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
                    this.EditControl.TextBox.BorderStyle = value ? BorderStyle.FixedSingle : BorderStyle.None;
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
        public string DisplayFormat
        {
            get
            {
                return this.m_displayFormat;
            }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                if (this.m_displayFormat != value)
                {
                    this.m_displayFormat = value;
                    this.OnPropertyChanged();
                }
            }
        }

        protected virtual string DisplayText
        {
            get
            {
                return this.m_displayText;
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

        [DefaultValue(0)]
        public int MaxLength
        {
            get
            {
                return this.m_MaxLength;
            }
            set
            {
                if (this.m_MaxLength != value)
                {
                    this.m_MaxLength = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.TextBox.MaxLength = value;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(false)]
        public bool MultiLine
        {
            get
            {
                return this.m_MultiLine;
            }
            set
            {
                if (this.m_MultiLine != value)
                {
                    this.m_MultiLine = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.TextBox.Multiline = value;
                    }
                    this.OnPropertyChanged();
                }
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
                        this.EditControl.TextBox.PasswordChar = value;
                    }
                    this.OnPropertyChanged();
                }
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
                        this.EditControl.TextBox.ReadOnly = value;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(0)]
        public System.Windows.Forms.ScrollBars ScrollBars
        {
            get
            {
                return this.m_ScrollBars;
            }
            set
            {
                this.m_ScrollBars = value;
                if (this.EditControl != null)
                {
                    this.EditControl.TextBox.ScrollBars = value;
                }
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue(""), Resco.Controls.DetailView.Design.Browsable(false)]
        public string SelectedText
        {
            get
            {
                if (this.EditControl != null)
                {
                    this.m_SelectedText = this.EditControl.TextBox.SelectedText;
                }
                return this.m_SelectedText;
            }
            set
            {
                if (this.EditControl != null)
                {
                    this.EditControl.TextBox.SelectedText = value;
                    this.m_SelectionStart = this.EditControl.TextBox.SelectionStart;
                    this.m_SelectionLength = this.EditControl.TextBox.SelectionLength;
                    this.EditControl.TextBox.ScrollToCaret();
                }
                else
                {
                    this.m_SelectionStart = this.Text.IndexOf(value);
                    this.m_SelectionLength = value.Length;
                }
                this.m_SelectedText = value;
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), DefaultValue(0), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public int SelectionLength
        {
            get
            {
                if (this.EditControl != null)
                {
                    this.m_SelectionLength = this.EditControl.TextBox.SelectionLength;
                }
                return this.m_SelectionLength;
            }
            set
            {
                if (this.EditControl != null)
                {
                    this.EditControl.TextBox.SelectionLength = value;
                    this.m_SelectionStart = this.EditControl.TextBox.SelectionStart;
                    this.m_SelectedText = this.EditControl.TextBox.SelectedText;
                    this.EditControl.TextBox.ScrollToCaret();
                }
                else if (this.m_SelectionStart >= 0)
                {
                    if ((this.m_SelectionLength + value) > this.Text.Length)
                    {
                        this.m_SelectionLength = this.Text.Length - value;
                    }
                    this.m_SelectedText = this.Text.Substring(this.m_SelectionStart, value);
                }
                this.m_SelectionLength = value;
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), DefaultValue(-1), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public int SelectionStart
        {
            get
            {
                if (this.EditControl != null)
                {
                    this.m_SelectionStart = this.EditControl.TextBox.SelectionStart;
                }
                return this.m_SelectionStart;
            }
            set
            {
                if (this.EditControl != null)
                {
                    this.EditControl.TextBox.SelectionStart = value;
                    this.m_SelectionLength = this.EditControl.TextBox.SelectionLength;
                    this.m_SelectedText = this.EditControl.TextBox.SelectedText;
                    this.EditControl.TextBox.ScrollToCaret();
                }
                else if (this.m_SelectionStart >= 0)
                {
                    if ((this.m_SelectionLength + value) > this.Text.Length)
                    {
                        this.m_SelectionLength = this.Text.Length - value;
                    }
                    this.m_SelectedText = this.Text.Substring(value, this.m_SelectionLength);
                }
                this.m_SelectionStart = value;
            }
        }

        [DefaultValue(false)]
        public bool WordWrap
        {
            get
            {
                return this.m_WordWrap;
            }
            set
            {
                if (this.m_WordWrap != value)
                {
                    this.m_WordWrap = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.TextBox.WordWrap = value;
                    }
                    this.OnPropertyChanged();
                }
            }
        }
    }
}

