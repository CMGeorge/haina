namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    public class ItemCheckBox : Item
    {
        public static int CheckHeight = 15;
        public static int CheckWidth = 15;
        private bool m_bActive;
        private RescoCheckBoxLocation m_checkBoxLocation;
        private ImageAttributes m_ia;
        private bool m_QuickChange;
        private bool m_ThreeState;

        public ItemCheckBox()
        {
            this.m_ThreeState = true;
            this.m_checkBoxLocation = RescoCheckBoxLocation.Left;
            this.m_ia = new ImageAttributes();
            Color white = Color.White;
            this.m_ia.SetColorKey(white, white);
        }

        public ItemCheckBox(Item toCopy) : base(toCopy)
        {
            this.m_ThreeState = true;
            this.m_checkBoxLocation = RescoCheckBoxLocation.Left;
            if (toCopy is ItemCheckBox)
            {
                this.ThreeState = ((ItemCheckBox) toCopy).ThreeState;
                this.CheckState = ((ItemCheckBox) toCopy).CheckState;
                this.QuickChange = ((ItemCheckBox) toCopy).QuickChange;
            }
            base.m_Text = toCopy.Text;
            this.m_ia = new ImageAttributes();
            Color pixel = ((Bitmap) Resco.Controls.DetailView.DetailView.BmpCheckTrue).GetPixel(0, 0);
            this.m_ia.SetColorKey(pixel, pixel);
        }

        public ItemCheckBox(string Label)
        {
            this.m_ThreeState = true;
            this.m_checkBoxLocation = RescoCheckBoxLocation.Left;
            this.Label = Label;
            this.m_ia = new ImageAttributes();
            Color pixel = ((Bitmap) Resco.Controls.DetailView.DetailView.BmpCheckTrue).GetPixel(0, 0);
            this.m_ia.SetColorKey(pixel, pixel);
        }

        public override void ClearContents()
        {
            this.SetValue(null, true);
        }

        protected override void Click(int yOffset, int parentWidth)
        {
            base.Parent.Focus();
            if (this.Enabled)
            {
                if (!this.m_bActive)
                {
                    this.OnGotFocus(this, new ItemEventArgs(this, 0, base.Name));
                    base.Parent.Invalidate();
                }
                if (this.m_bActive || this.QuickChange)
                {
                    switch (this.CheckState)
                    {
                        case System.Windows.Forms.CheckState.Unchecked:
                            this.Value = true;
                            break;

                        case System.Windows.Forms.CheckState.Checked:
                            this.Value = this.m_ThreeState ? null : ((object) false);
                            break;

                        case System.Windows.Forms.CheckState.Indeterminate:
                            this.Value = false;
                            break;
                    }
                }
                this.m_bActive = true;
                base.Click(yOffset, parentWidth);
            }
        }

        public override Item Clone()
        {
            ItemCheckBox box = new ItemCheckBox(this);
            box.Value = this.Value;
            return box;
        }

        protected override void Draw(Graphics gr, int yOffset, int parentWidth)
        {
            ItemBorder border = base.m_border;
            base.Draw(gr, yOffset, parentWidth);
            if (border == ItemBorder.Flat)
            {
                base.m_border = ItemBorder.Flat;
            }
        }

        protected override void DrawItemTextArea(Graphics gr, ref Rectangle textBounds)
        {
            int x = textBounds.X - DVTextBox.BorderSize;
            int y = textBounds.Y;
            if (this.CheckBoxLocation == RescoCheckBoxLocation.Left)
            {
                textBounds.X = (x + CheckWidth) + 2;
            }
            else
            {
                x = (textBounds.Right - CheckWidth) + DVTextBox.BorderSize;
            }
            textBounds.Width -= CheckWidth + 2;
            base.DrawItemTextArea(gr, ref textBounds);
            if (this.m_bActive)
            {
                gr.FillRectangle(new SolidBrush(Color.White), x, y, CheckWidth, CheckHeight);
                gr.DrawRectangle(new Pen(Color.Black), x, y, CheckWidth, CheckHeight);
            }
            if (base.ItemBorder == ItemBorder.Flat)
            {
                if (!this.m_bActive)
                {
                    gr.DrawRectangle(new Pen((base.m_bHasError && (base.ErrorBackground == ErrorBackground.Border)) ? base.ErrorColor : Color.Gray), x, y, CheckWidth, CheckHeight);
                }
                base.m_border = ItemBorder.None;
            }
            Image image = this.GetImage();
            if (image != null)
            {
                Rectangle destRect = new Rectangle(x, y, CheckWidth, CheckHeight);
                gr.DrawImage(image, destRect, 0, 0, 15, 15, GraphicsUnit.Pixel, this.m_ia);
            }
        }

        protected override string FormatValue(object value)
        {
            return this.Text;
        }

        protected override int GetActiveWidth()
        {
            return ((base.GetActiveWidth() - CheckWidth) + 2);
        }

        private Image GetImage()
        {
            switch (this.CheckState)
            {
                case System.Windows.Forms.CheckState.Checked:
                    return Resco.Controls.DetailView.DetailView.BmpCheckTrue;

                case System.Windows.Forms.CheckState.Indeterminate:
                    return Resco.Controls.DetailView.DetailView.BmpCheckNull;
            }
            return null;
        }

        protected override int GetMinimumHeight()
        {
            return (CheckWidth + 1);
        }

        protected internal override bool HandleKey(Keys key)
        {
            bool flag;
            if ((key == Keys.Right) || (key == Keys.Return))
            {
                flag = true;
            }
            else if (key == Keys.Left)
            {
                flag = false;
            }
            else
            {
                return false;
            }
            switch (this.CheckState)
            {
                case System.Windows.Forms.CheckState.Unchecked:
                    if (!this.m_ThreeState || flag)
                    {
                        this.Value = true;
                        break;
                    }
                    this.Value = null;
                    break;

                case System.Windows.Forms.CheckState.Checked:
                    if (!this.m_ThreeState || !flag)
                    {
                        this.Value = false;
                        break;
                    }
                    this.Value = null;
                    break;

                case System.Windows.Forms.CheckState.Indeterminate:
                    this.Value = !flag;
                    break;
            }
            return true;
        }

        protected override void Hide()
        {
            this.m_bActive = false;
            base.Hide();
        }

        protected override void OnClick(int yOffset, int parentWidth, bool useClickVisualize)
        {
            base.OnClick(yOffset, parentWidth, false);
        }

        protected override object Parse(string text)
        {
            return this.Value;
        }

        public bool ShouldSerializeCheckBoxLocation()
        {
            return (this.m_checkBoxLocation != RescoCheckBoxLocation.Left);
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
            return "CheckBox";
        }

        public RescoCheckBoxLocation CheckBoxLocation
        {
            get
            {
                return this.m_checkBoxLocation;
            }
            set
            {
                if (this.m_checkBoxLocation != value)
                {
                    this.m_checkBoxLocation = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(false)]
        public bool Checked
        {
            get
            {
                return Convert.ToBoolean(base.Value);
            }
            set
            {
                base.Value = value;
            }
        }

        [DefaultValue(2), Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public System.Windows.Forms.CheckState CheckState
        {
            get
            {
                if (this.Value == null)
                {
                    return System.Windows.Forms.CheckState.Indeterminate;
                }
                if (Convert.ToBoolean(base.Value))
                {
                    return System.Windows.Forms.CheckState.Checked;
                }
                return System.Windows.Forms.CheckState.Unchecked;
            }
            set
            {
                switch (value)
                {
                    case System.Windows.Forms.CheckState.Unchecked:
                        this.Value = false;
                        return;

                    case System.Windows.Forms.CheckState.Checked:
                        this.Value = true;
                        return;

                    case System.Windows.Forms.CheckState.Indeterminate:
                        this.Value = null;
                        return;
                }
            }
        }

        internal override bool Focused
        {
            get
            {
                return this.m_bActive;
            }
        }

        [DefaultValue(false)]
        public bool QuickChange
        {
            get
            {
                return this.m_QuickChange;
            }
            set
            {
                this.m_QuickChange = value;
            }
        }

        [DefaultValue("")]
        public override string Text
        {
            get
            {
                return base.m_Text;
            }
            set
            {
                if (base.m_Text != value)
                {
                    base.m_Text = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(true)]
        public bool ThreeState
        {
            get
            {
                return this.m_ThreeState;
            }
            set
            {
                this.m_ThreeState = value;
                if (!value && (this.Value == null))
                {
                    this.Value = false;
                }
            }
        }
    }
}

