namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    public class ItemComboBox : Item
    {
        private CurrencyManager cm;
        private DVComboBox EditControl;
        private bool m_bStringData;
        private object m_dataSource;
        private string m_displayMember;
        private RescoComboBoxStyle m_DropDownStyle;
        private ImageAttributes m_ImageAttributes;
        private ArrayList m_Items;
        private int m_MaxLength;
        private int m_SelectedIndex;
        private string m_valueMember;

        public ItemComboBox()
        {
            this.m_displayMember = "";
            this.m_valueMember = "";
            this.m_SelectedIndex = -1;
            this.DropDownStyle = RescoComboBoxStyle.DropDown;
            this.m_Items = new ArrayList();
            base.m_Text = "";
            this.m_ImageAttributes = new ImageAttributes();
            this.m_ImageAttributes.SetColorKey(Color.White, Color.White);
        }

        public ItemComboBox(Item toCopy) : base(toCopy)
        {
            this.m_displayMember = "";
            this.m_valueMember = "";
            this.m_SelectedIndex = -1;
            if (toCopy is ItemComboBox)
            {
                this.DropDownStyle = ((ItemComboBox) toCopy).DropDownStyle;
            }
            else
            {
                this.DropDownStyle = RescoComboBoxStyle.DropDown;
            }
            this.m_Items = new ArrayList();
            base.m_Text = "";
            this.m_ImageAttributes = new ImageAttributes();
            this.m_ImageAttributes.SetColorKey(Color.White, Color.White);
        }

        public ItemComboBox(string Label, RescoComboBoxStyle ComboBoxStyle)
        {
            this.m_displayMember = "";
            this.m_valueMember = "";
            this.m_SelectedIndex = -1;
            this.Label = Label;
            this.m_Items = new ArrayList();
            this.DropDownStyle = ComboBoxStyle;
            base.m_Text = "";
            this.m_ImageAttributes = new ImageAttributes();
            this.m_ImageAttributes.SetColorKey(Color.White, Color.White);
        }

        public int Add(object value)
        {
            int num = this.m_Items.Add(value);
            if (this.DataManager != null)
            {
                this.DataManager.Refresh();
            }
            return num;
        }

        public void Clear()
        {
            this.m_Items.Clear();
            if (this.DataManager != null)
            {
                this.DataManager.Refresh();
            }
            this.SelectedIndex = -1;
            this.EditValue = null;
            this.Text = "";
        }

        public override void ClearContents()
        {
            this.SelectedIndex = -1;
            this.EditValue = null;
            this.Text = "";
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
                    DVComboBox control = base.Parent.GetControl(typeof(DVComboBox)) as DVComboBox;
                    if (control != null)
                    {
                        control.ComboBox.BeginInit();
                        control.Font = base.TextFont;
                        control.ComboBox.DropDownStyle = this.DropDownStyle;
                        control.Enabled = true;
                        if ((this.DataManager != null) && (this.DataManager.Count == 0))
                        {
                            control.ComboBox.ValueMember = "";
                            control.ComboBox.DisplayMember = "";
                            control.ComboBox.DataSource = new object[0];
                        }
                        else
                        {
                            control.ComboBox.ValueMember = this.ValueMember;
                            control.ComboBox.DisplayMember = this.DisplayMember;
                            control.ComboBox.DataSource = this.DataSource;
                        }
                        control.ComboBox.SelectedIndex = this.SelectedIndex;
                        control.Text = this.Text;
                        control.ComboBox.MaxLength = this.MaxLength;
                        control.ValueChanged = (EventHandler) Delegate.Combine(control.ValueChanged, new EventHandler(this.OnControlChanged));
                        control.ComboBox.EndInit();
                        this.EditControl = control;
                        control.Bounds = this.GetActivePartBounds(yOffset);
                        base.DisableEvents = false;
                        this.OnGotFocus(this, new ItemEventArgs(this, 0, base.Name));
                        base.DisableEvents = true;
                        if (this.EditControl != null)
                        {
                            this.EditControl.Show();
                            if (this.DropDownClicked(parentWidth, yOffset))
                            {
                                this.EditControl.ComboBox.DroppedDown = true;
                            }
                        }
                    }
                    base.DisableEvents = false;
                    base.Click(yOffset, parentWidth);
                }
            }
        }

        public override Item Clone()
        {
            ItemComboBox box = new ItemComboBox(this);
            if (this.m_bStringData)
            {
                box.StringData = this.StringData;
            }
            else
            {
                box.Items = this.Items;
            }
            box.SelectedIndex = this.SelectedIndex;
            return box;
        }

        protected override void DrawItemTextArea(Graphics gr, ref Rectangle textBounds)
        {
            textBounds.Width -= 3 * Resco.Controls.DetailView.DetailView.ComboSize;
            base.DrawItemTextArea(gr, ref textBounds);
            Image bmpComboBox = Resco.Controls.DetailView.DetailView.BmpComboBox;
            Rectangle comboRectangle = Resco.Controls.DetailView.DetailView.ComboRectangle;
            comboRectangle.Offset(textBounds.Right, textBounds.Y);
            gr.DrawImage(bmpComboBox, comboRectangle, 0, 0, bmpComboBox.Width, bmpComboBox.Height, GraphicsUnit.Pixel, this.m_ImageAttributes);
        }

        private bool DropDownClicked(int parentWidth, int yOffset)
        {
            Point lastMousePosition = base.Parent.LastMousePosition;
            Rectangle rectangle = new Rectangle();
            Rectangle activePartBounds = this.GetActivePartBounds(yOffset);
            if (this.DropDownStyle == RescoComboBoxStyle.DropDownList)
            {
                rectangle = activePartBounds;
            }
            else
            {
                rectangle.Y = activePartBounds.Y;
                rectangle.Width = 4 * Resco.Controls.DetailView.DetailView.ComboSize;
                rectangle.Height = this.Height - 1;
                rectangle.X = (activePartBounds.X + activePartBounds.Width) - rectangle.Width;
            }
            return rectangle.Contains(lastMousePosition.X, lastMousePosition.Y);
        }

        protected override string FormatValue(object value)
        {
            int position = this.GetPosition(value, this.ValueMember);
            if (position == -1)
            {
                return base.FormatValue(value);
            }
            if (this.DataManager != null)
            {
                return Convert.ToString(Resco.Controls.DetailView.DetailView.GetBoundItem(this.DataManager, this.DataManager.List[position], this.DisplayMember));
            }
            return Convert.ToString(this.m_Items[position]);
        }

        protected virtual int GetPosition(object value, string bindingMember)
        {
            CurrencyManager dataManager = this.DataManager;
            int index = -1;
            if ((dataManager != null) && (value != null))
            {
                if (bindingMember == null)
                {
                    bindingMember = "";
                }
                PropertyDescriptor descriptor = dataManager.GetItemProperties().Find(bindingMember, true);
                for (int i = 0; i < dataManager.List.Count; i++)
                {
                    object obj2;
                    if (descriptor != null)
                    {
                        obj2 = descriptor.GetValue(dataManager.List[i]);
                    }
                    else
                    {
                        obj2 = dataManager.List[i];
                    }
                    if (value.Equals(obj2))
                    {
                        index = i;
                        break;
                    }
                }
            }
            if (dataManager == null)
            {
                index = this.m_Items.IndexOf(value);
            }
            return index;
        }

        protected internal override bool HandleKey(Keys key)
        {
            if (key == Keys.Return)
            {
                return true;
            }
            if ((key != Keys.Up) && (key != Keys.Down))
            {
                return false;
            }
            return this.DroppedDown;
        }

        protected internal override bool HandleKeyUp(Keys key)
        {
            if ((key == Keys.Return) && !this.DroppedDown)
            {
                return true;
            }
            if ((key != Keys.Up) && (key != Keys.Down))
            {
                return false;
            }
            return this.DroppedDown;
        }

        protected override void Hide()
        {
            if (this.EditControl != null)
            {
                this.EditControl.ValueChanged = (EventHandler) Delegate.Remove(this.EditControl.ValueChanged, new EventHandler(this.OnControlChanged));
                int selectedIndex = this.EditControl.ComboBox.SelectedIndex;
                string text = this.EditControl.Text;
                this.SelectedIndex = selectedIndex;
                this.Text = text;
                base.Hide();
                this.EditControl.Hide();
            }
            else
            {
                base.Hide();
            }
            this.EditControl = null;
        }

        public int IndexOf(object value)
        {
            return this.m_Items.IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            this.m_Items.Insert(index, value);
            if (this.DataManager != null)
            {
                this.DataManager.Refresh();
            }
        }

        protected override void MoveTop(int offset)
        {
            if ((this.EditControl != null) && (offset != 0))
            {
                this.EditControl.Top += offset;
            }
        }

        private void OnControlChanged(object sender, EventArgs e)
        {
            int selectedIndex = this.EditControl.ComboBox.SelectedIndex;
            string text = this.EditControl.Text;
            if (selectedIndex == -1)
            {
                this.Text = text;
            }
            else
            {
                this.SelectedIndex = selectedIndex;
            }
            if (this.EditControl != null)
            {
                if (this.SelectedIndex != selectedIndex)
                {
                    this.EditControl.ComboBox.SelectedIndex = this.SelectedIndex;
                }
                if (this.Text != this.EditControl.Text)
                {
                    this.EditControl.Text = this.Text;
                }
            }
        }

        protected override object Parse(string text)
        {
            int position = this.GetPosition(text, this.DisplayMember);
            if (position == -1)
            {
                return base.Parse(text);
            }
            if (this.DataManager != null)
            {
                return Resco.Controls.DetailView.DetailView.GetBoundItem(this.DataManager, this.DataManager.List[position], this.ValueMember);
            }
            return this.m_Items[position];
        }

        public void Remove(object value)
        {
            this.m_Items.Remove(value);
            if (this.DataManager != null)
            {
                this.DataManager.Refresh();
            }
        }

        public void SetFocus(bool dropDown)
        {
            base.SetFocus();
            if (this.EditControl != null)
            {
                this.EditControl.ComboBox.DroppedDown = dropDown;
            }
        }

        internal override void SetParent(Resco.Controls.DetailView.DetailView o)
        {
            this.cm = null;
            if (o == null)
            {
                base.SetParent(o);
            }
            else
            {
                int selectedIndex = this.SelectedIndex;
                if (selectedIndex != -1)
                {
                    this.m_SelectedIndex = -1;
                }
                base.SetParent(o);
                this.SelectedIndex = selectedIndex;
            }
        }

        protected override void SetText(string text, bool validated)
        {
            if (base.m_Text != text)
            {
                if (!validated)
                {
                    ValidatingEventArgs e = new ValidatingEventArgs(this, this.Parse(text), text, false);
                    this.OnValidating(this, e);
                    if (!e.Cancel)
                    {
                        base.m_Text = e.NewText;
                        this.UpdatePosition(base.m_Text, this.DisplayMember);
                        if (this.SelectedIndex == -1)
                        {
                            base.m_Value = e.NewValue;
                        }
                        if (base.Parent != null)
                        {
                            base.Parent.UpdateData(this);
                        }
                        if (((base.Parent != null) && !base.DisableRefresh) && base.Parent.AutoRefresh)
                        {
                            base.Parent.Invalidate();
                        }
                        this.OnChanged(this, new ItemEventArgs());
                    }
                }
                if (((this.EditControl != null) && (this.SelectedIndex == -1)) && (this.EditControl.Text != base.m_Text))
                {
                    this.EditControl.Text = base.m_Text;
                }
            }
        }

        protected override void SetValue(object value, bool validated)
        {
            if ((base.m_Value != value) && ((base.m_Value == null) || !base.m_Value.Equals(value)))
            {
                if (!validated)
                {
                    ValidatingEventArgs e = new ValidatingEventArgs(this, value, this.FormatValue(value), false);
                    this.OnValidating(this, e);
                    if (!e.Cancel)
                    {
                        base.m_Value = e.NewValue;
                        this.UpdatePosition(base.m_Value, this.ValueMember);
                        if (this.SelectedIndex == -1)
                        {
                            base.m_Text = e.NewText;
                        }
                        if (base.Parent != null)
                        {
                            base.Parent.UpdateData(this);
                        }
                        if (((base.Parent != null) && !base.DisableRefresh) && base.Parent.AutoRefresh)
                        {
                            base.Parent.Invalidate();
                        }
                        this.OnChanged(this, new ItemEventArgs());
                    }
                }
                if (((this.EditControl != null) && (this.SelectedIndex == -1)) && (this.EditControl.Text != base.m_Text))
                {
                    this.EditControl.Text = base.m_Text;
                }
            }
        }

        protected virtual bool ShouldSerializeDropDownStyle()
        {
            return (this.m_DropDownStyle != RescoComboBoxStyle.DropDown);
        }

        protected bool ShouldSerializeStringData()
        {
            return ((this.m_bStringData && (this.m_Items != null)) && (this.m_Items.Count > 0));
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
            return "ComboBox";
        }

        protected virtual void UpdatePosition(object value, string bindingMember)
        {
            CurrencyManager dataManager = this.DataManager;
            int num = -1;
            int num2 = this.ControlVisible ? -1 : -2;
            if ((dataManager != null) && (value != null))
            {
                if (bindingMember == null)
                {
                    bindingMember = "";
                }
                PropertyDescriptor descriptor = dataManager.GetItemProperties().Find(bindingMember, true);
                for (int i = 0; i < dataManager.List.Count; i++)
                {
                    object obj2;
                    if (descriptor != null)
                    {
                        obj2 = descriptor.GetValue(dataManager.List[i]);
                    }
                    else
                    {
                        obj2 = dataManager.List[i];
                    }
                    if (value.Equals(obj2))
                    {
                        num = i;
                        break;
                    }
                    if (((num2 == -1) && (value is string)) && ((obj2 is string) && ((string) obj2).ToUpper().StartsWith(((string) value).ToUpper())))
                    {
                        num2 = i;
                    }
                }
            }
            if (dataManager == null)
            {
                this.m_SelectedIndex = this.m_Items.IndexOf(value);
            }
            else
            {
                this.m_SelectedIndex = num;
            }
            if (dataManager != null)
            {
                if (num >= 0)
                {
                    dataManager.Position = num;
                    if (((this.EditControl != null) && (this.EditControl.ComboBox.Items.Count > 0)) && (this.EditControl.ComboBox.SelectedIndex == -1))
                    {
                        this.EditControl.ComboBox.SelectedIndex = num;
                    }
                }
                else if ((num2 >= -1) && (this.EditControl != null))
                {
                    this.EditControl.ComboBox.BeginInit();
                    this.EditControl.ComboBox.SelectedIndex = num2;
                    this.EditControl.ComboBox.EndInit();
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

        protected bool ControlVisible
        {
            get
            {
                return (this.EditControl != null);
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.DetailView.Design.Browsable(false)]
        public CurrencyManager DataManager
        {
            get
            {
                if (((this.cm == null) && (base.Parent != null)) && ((this.DataSource != null) && (base.Parent.BindingContext != null)))
                {
                    this.cm = (CurrencyManager) base.Parent.BindingContext[this.DataSource];
                    if (this.m_SelectedIndex != -1)
                    {
                        if (this.m_SelectedIndex < this.cm.Count)
                        {
                            this.cm.Position = this.m_SelectedIndex;
                        }
                        else
                        {
                            this.m_SelectedIndex = this.cm.Position;
                        }
                    }
                }
                return this.cm;
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public object DataSource
        {
            get
            {
                if (this.m_dataSource == null)
                {
                    return this.m_Items;
                }
                return this.m_dataSource;
            }
            set
            {
                if (this.m_dataSource != value)
                {
                    base.m_Value = this.Value;
                    this.m_dataSource = value;
                    this.cm = null;
                    if (this.EditControl != null)
                    {
                        this.EditControl.ComboBox.DataSource = this.DataSource;
                    }
                    if (value == null)
                    {
                        this.Value = null;
                        base.m_Text = "";
                    }
                    else if (this.Value != base.m_Value)
                    {
                        this.Value = base.m_Value;
                    }
                    else
                    {
                        this.UpdatePosition(base.m_Value, this.m_valueMember);
                    }
                }
            }
        }

        [DefaultValue("")]
        public string DisplayMember
        {
            get
            {
                return this.m_displayMember;
            }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                if (value != this.m_displayMember)
                {
                    base.m_Text = this.Text;
                    this.m_displayMember = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.ComboBox.DisplayMember = this.m_displayMember;
                    }
                    if (this.Text != base.m_Text)
                    {
                        this.Text = base.m_Text;
                    }
                    else
                    {
                        this.UpdatePosition(base.m_Text, this.m_displayMember);
                    }
                }
            }
        }

        [DefaultValue(0)]
        public RescoComboBoxStyle DropDownStyle
        {
            get
            {
                return this.m_DropDownStyle;
            }
            set
            {
                this.m_DropDownStyle = value;
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.DetailView.Design.Browsable(false)]
        public bool DroppedDown
        {
            get
            {
                return ((this.EditControl != null) && this.EditControl.ComboBox.DroppedDown);
            }
            set
            {
                if (this.EditControl != null)
                {
                    this.EditControl.ComboBox.DroppedDown = value;
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

        [Resco.Controls.DetailView.Design.Browsable(false), DefaultValue((string) null), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public object[] Items
        {
            get
            {
                if (this.m_Items != null)
                {
                    return (object[]) this.m_Items.ToArray(typeof(object));
                }
                return null;
            }
            set
            {
                this.m_Items.Clear();
                if (value != null)
                {
                    this.m_bStringData = true;
                    foreach (object obj2 in value)
                    {
                        if (obj2 != null)
                        {
                            this.m_Items.Add(obj2);
                            this.m_bStringData = this.m_bStringData && (obj2 is string);
                        }
                    }
                }
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
                        this.EditControl.ComboBox.MaxLength = value;
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(-1)]
        public int SelectedIndex
        {
            get
            {
                return this.m_SelectedIndex;
            }
            set
            {
                if (value != this.m_SelectedIndex)
                {
                    CurrencyManager dataManager = this.DataManager;
                    if (dataManager == null)
                    {
                        this.m_SelectedIndex = value;
                    }
                    else if ((value < 0) || (value >= dataManager.Count))
                    {
                        this.m_SelectedIndex = -1;
                    }
                    else
                    {
                        this.m_SelectedIndex = value;
                        dataManager.Position = value;
                    }
                    if (base.Parent != null)
                    {
                        base.Parent.UpdateData(this);
                    }
                    if (this.EditControl != null)
                    {
                        if (this.EditControl.ComboBox.Items.Count > 0)
                        {
                            this.EditControl.ComboBox.SelectedIndex = this.m_SelectedIndex;
                        }
                        if (this.m_SelectedIndex == -1)
                        {
                            this.EditControl.Text = base.m_Text;
                        }
                    }
                    this.OnChanged(this, new ItemEventArgs());
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue((string) null)]
        public object SelectedItem
        {
            get
            {
                if (this.SelectedIndex >= 0)
                {
                    if ((this.DataManager != null) && (this.SelectedIndex < this.DataManager.List.Count))
                    {
                        return this.DataManager.List[this.SelectedIndex];
                    }
                    if ((this.SelectedIndex >= 0) && (this.SelectedIndex < this.m_Items.Count))
                    {
                        return this.m_Items[this.SelectedIndex];
                    }
                }
                return null;
            }
            set
            {
                if (((this.ValueMember != "") && (this.DataManager != null)) && (this.DataManager.Position > -1))
                {
                    this.Value = Resco.Controls.DetailView.DetailView.GetBoundItem(this.DataManager, value, this.ValueMember);
                }
                else
                {
                    this.Value = value;
                }
            }
        }

        [DefaultValue((string) null)]
        public string[] StringData
        {
            get
            {
                if (this.m_bStringData && (this.m_Items != null))
                {
                    return (string[]) this.m_Items.ToArray(typeof(string));
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.m_Items.Clear();
                    string[] strArray = value;
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        object obj2 = strArray[i];
                        if (obj2 != null)
                        {
                            this.m_Items.Add(obj2);
                        }
                    }
                    this.m_bStringData = true;
                }
                else
                {
                    this.m_bStringData = false;
                }
            }
        }

        [DefaultValue("")]
        public override string Text
        {
            get
            {
                if (this.SelectedIndex == -1)
                {
                    return base.m_Text;
                }
                if ((this.DataManager != null) && (this.DataManager.Position > -1))
                {
                    return string.Format("{0}", Resco.Controls.DetailView.DetailView.GetBoundItem(this.DataManager, this.DataManager.Current, this.DisplayMember));
                }
                if ((this.SelectedIndex >= 0) && (this.SelectedIndex < this.m_Items.Count))
                {
                    return string.Format("{0}", this.m_Items[this.SelectedIndex]);
                }
                return "";
            }
            set
            {
                base.m_Text = this.Text;
                base.Text = value;
            }
        }

        [DefaultValue(""), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.DetailView.Design.Browsable(false)]
        public override object Value
        {
            get
            {
                if (this.SelectedIndex == -1)
                {
                    return base.m_Value;
                }
                if ((this.DataManager != null) && (this.DataManager.Position > -1))
                {
                    return Resco.Controls.DetailView.DetailView.GetBoundItem(this.DataManager, this.DataManager.Current, this.ValueMember);
                }
                if ((this.SelectedIndex >= 0) && (this.SelectedIndex < this.m_Items.Count))
                {
                    return this.m_Items[this.SelectedIndex];
                }
                return null;
            }
            set
            {
                base.m_Value = this.Value;
                base.Value = value;
            }
        }

        [DefaultValue("")]
        public string ValueMember
        {
            get
            {
                return this.m_valueMember;
            }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                if (value != this.m_valueMember)
                {
                    if (this.SelectedIndex != -1)
                    {
                        base.m_Value = this.Value;
                    }
                    this.m_valueMember = value;
                    if (this.EditControl != null)
                    {
                        this.EditControl.ComboBox.ValueMember = this.m_valueMember;
                    }
                    if (this.Value != base.m_Value)
                    {
                        this.Value = base.m_Value;
                    }
                    else
                    {
                        this.UpdatePosition(base.m_Value, this.m_valueMember);
                    }
                }
            }
        }
    }
}

