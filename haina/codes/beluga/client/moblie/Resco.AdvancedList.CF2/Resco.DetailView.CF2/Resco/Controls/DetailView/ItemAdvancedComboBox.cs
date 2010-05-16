namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public class ItemAdvancedComboBox : Item
    {
        private CurrencyManager cm;
        private System.Windows.Forms.Control EditControl;
        internal static int ItemACArrowHeight = 4;
        private static Type m_advancedComboBoxType = null;
        private int m_alternateTemplateIndex;
        private int m_arrowBoxWidth;
        private static Assembly m_assembly = null;
        private static string m_assemblyName = "Resco.AdvancedComboBox.CF2.dll";
        private bool m_bStringData;
        private object m_dataSource;
        private string m_displayMember;
        private bool m_fullScreenList;
        private ArrayList m_Items;
        private bool m_listGridLines;
        private int m_SelectedIndex;
        private int m_selectedTemplateIndex;
        private int m_templateIndex;
        private IList m_templates;
        private static Type m_templateSetType = null;
        private int m_textBoxTemplateIndex;
        private bool m_touchScrolling;
        private string m_valueMember;

        public ItemAdvancedComboBox()
        {
            this.m_SelectedIndex = -1;
            this.m_displayMember = "";
            this.m_valueMember = "";
            this.m_alternateTemplateIndex = -1;
            this.m_arrowBoxWidth = 13;
            this.GetTypes();
            ConstructorInfo info = m_templateSetType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);
            this.m_templates = (IList) info.Invoke(new object[0]);
            this.m_Items = new ArrayList();
        }

        public ItemAdvancedComboBox(Item toCopy) : base(toCopy)
        {
            this.m_SelectedIndex = -1;
            this.m_displayMember = "";
            this.m_valueMember = "";
            this.m_alternateTemplateIndex = -1;
            this.m_arrowBoxWidth = 13;
            this.GetTypes();
            if (toCopy is ItemAdvancedComboBox)
            {
                ItemAdvancedComboBox box = (ItemAdvancedComboBox) toCopy;
                this.TemplateIndex = box.TemplateIndex;
                this.AlternateTemplateIndex = box.AlternateTemplateIndex;
                this.SelectedTemplateIndex = box.SelectedTemplateIndex;
                this.TextBoxTemplateIndex = box.TextBoxTemplateIndex;
                this.ArrowBoxWidth = box.ArrowBoxWidth;
                this.m_templates = (IList) base.InvokeMethod(box.Templates, "Clone", new object[0]);
            }
            this.m_Items = new ArrayList();
            base.m_Text = "";
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
                    System.Windows.Forms.Control control = base.Parent.GetControl(m_advancedComboBoxType);
                    if (control != null)
                    {
                        base.InvokeMethod(control, "BeginUpdate", new object[0]);
                        control.Font = base.TextFont;
                        control.ForeColor = base.GetTextForeColor();
                        control.BackColor = base.GetTextBackColor();
                        control.Enabled = true;
                        base.InvokeSetProperty(control, "ArrowBoxWidth", this.ArrowBoxWidth);
                        base.InvokeSetProperty(control, "ListGridLines", this.ListGridLines);
                        base.InvokeSetProperty(control, "FullScreenList", this.FullScreenList);
                        base.InvokeSetProperty(control, "Templates", this.Templates);
                        base.InvokeSetProperty(control, "AlternateTemplateIndex", this.AlternateTemplateIndex);
                        base.InvokeSetProperty(control, "TemplateIndex", this.TemplateIndex);
                        base.InvokeSetProperty(control, "SelectedTemplateIndex", this.SelectedTemplateIndex);
                        base.InvokeSetProperty(control, "TextBoxTemplateIndex", this.TextBoxTemplateIndex);
                        base.InvokeSetProperty(control, "TouchScrolling", this.TouchScrolling);
                        if ((this.DataManager != null) && (this.DataManager.Count == 0))
                        {
                            base.InvokeSetProperty(control, "ValueMember", "");
                            base.InvokeSetProperty(control, "DisplayMember", "");
                            base.InvokeSetProperty(control, "DataSource", new object[0]);
                        }
                        else
                        {
                            base.InvokeSetProperty(control, "ValueMember", this.ValueMember);
                            base.InvokeSetProperty(control, "DisplayMember", this.DisplayMember);
                            base.InvokeSetProperty(control, "DataSource", this.DataSource);
                        }
                        base.InvokeSetProperty(control, "SelectedIndex", this.SelectedIndex);
                        control.Text = this.Text;
                        base.InvokeAddEvent(control, "SelectedValueChanged", new EventHandler(this.OnControlChanged));
                        base.InvokeMethod(control, "EndUpdate", new object[0]);
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
                                base.InvokeSetProperty(this.EditControl, "DroppedDown", true);
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
            textBounds.Width -= this.m_arrowBoxWidth;
            base.DrawItemTextArea(gr, ref textBounds);
            int num = this.m_arrowBoxWidth / 2;
            int itemACArrowHeight = ItemACArrowHeight;
            Rectangle rectangle = new Rectangle(textBounds.Right + DVTextBox.BorderSize, textBounds.Y, this.m_arrowBoxWidth, textBounds.Height);
            if ((num % 2) == 1)
            {
                num++;
            }
            Point[] points = new Point[] { new Point(rectangle.Left + ((this.m_arrowBoxWidth - num) / 2), rectangle.Y + ((rectangle.Height - itemACArrowHeight) / 2)), new Point(rectangle.Left + (this.m_arrowBoxWidth / 2), rectangle.Y + ((rectangle.Height + itemACArrowHeight) / 2)), new Point((rectangle.Left + ((this.m_arrowBoxWidth + num) / 2)) + 1, rectangle.Y + ((rectangle.Height - itemACArrowHeight) / 2)) };
            gr.FillPolygon(base.GetTextForeBrush(), points);
        }

        private bool DropDownClicked(int parentWidth, int yOffset)
        {
            Point lastMousePosition = base.Parent.LastMousePosition;
            Rectangle rectangle = new Rectangle();
            Rectangle activePartBounds = this.GetActivePartBounds(yOffset);
            rectangle.Y = activePartBounds.Y;
            rectangle.Width = 4 * Resco.Controls.DetailView.DetailView.ComboSize;
            rectangle.Height = this.Height - 1;
            rectangle.X = (activePartBounds.X + activePartBounds.Width) - rectangle.Width;
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

        private void GetTypes()
        {
            if (m_advancedComboBoxType == null)
            {
                m_advancedComboBoxType = Type.GetType("Resco.Controls.AdvancedComboBox.AdvancedComboBox");
                if (m_advancedComboBoxType == null)
                {
                    m_assembly = Assembly.LoadFrom(m_assemblyName);
                    if (m_assembly == null)
                    {
                        throw new MissingMethodException("Unable to load '" + m_assemblyName + "' assembly.");
                    }
                    m_advancedComboBoxType = m_assembly.GetType("Resco.Controls.AdvancedComboBox.AdvancedComboBox");
                }
                if (m_advancedComboBoxType == null)
                {
                    throw new MissingMethodException("Unable to get 'Resco.Controls.AdvancedComboBox.AdvancedComboBox' type. You have to reference Resco AdvancedComboBox control to use ItemAdvancedComboBox type.");
                }
            }
            if (m_templateSetType == null)
            {
                m_templateSetType = Type.GetType("Resco.Controls.AdvancedComboBox.TemplateSet");
                if (m_templateSetType == null)
                {
                    if (m_assembly == null)
                    {
                        m_assembly = Assembly.LoadFrom(m_assemblyName);
                    }
                    if (m_assembly == null)
                    {
                        throw new MissingMethodException("Unable to load '" + m_assemblyName + "' assembly.");
                    }
                    m_templateSetType = m_assembly.GetType("Resco.Controls.AdvancedComboBox.TemplateSet");
                }
                if (m_templateSetType == null)
                {
                    throw new MissingMethodException("Unable to get 'Resco.Controls.AdvancedComboBox.AdvancedComboBox' type. You have to reference Resco AdvancedComboBox control to use this Item type.");
                }
            }
        }

        protected internal override bool HandleKey(Keys key)
        {
            if (key == Keys.Left)
            {
                this.SelectedIndex--;
                return true;
            }
            if (key == Keys.Right)
            {
                this.SelectedIndex++;
                return true;
            }
            return false;
        }

        protected internal override bool HandleKeyUp(Keys key)
        {
            if ((key == Keys.Return) && !this.DroppedDown)
            {
                this.DroppedDown = true;
                return true;
            }
            return false;
        }

        protected override void Hide()
        {
            if (this.EditControl != null)
            {
                base.InvokeRemoveEvent(this.EditControl, "SelectedValueChanged", new EventHandler(this.OnControlChanged));
                int num = (int) base.InvokeGetProperty(this.EditControl, "SelectedIndex");
                string text = this.EditControl.Text;
                this.SelectedIndex = num;
                this.Text = text;
                base.Hide();
                this.EditControl.Hide();
                base.InvokeSetProperty(this.EditControl, "DataSource", null);
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
            int num = (int) base.InvokeGetProperty(this.EditControl, "SelectedIndex");
            string text = this.EditControl.Text;
            if (num == -1)
            {
                this.Text = text;
            }
            else
            {
                this.SelectedIndex = num;
            }
            if (this.EditControl != null)
            {
                if (this.SelectedIndex != num)
                {
                    base.InvokeSetProperty(this.EditControl, "SelectedIndex", this.SelectedIndex);
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

        protected internal override void ScaleItem(float fx, float fy)
        {
            base.ScaleItem(fx, fy);
            base.SetProperty("ArrowBoxWidth", (int) (this.m_arrowBoxWidth * fx));
            base.InvokeMethod(this.Templates, "Scale", new object[] { fx, fy });
        }

        public void SetFocus(bool dropDown)
        {
            base.SetFocus();
            if (this.EditControl != null)
            {
                base.InvokeSetProperty(this.EditControl, "DroppedDown", dropDown);
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
            return "AdvancedComboBox";
        }

        protected virtual void UpdatePosition(object value, string bindingMember)
        {
            CurrencyManager dataManager = this.DataManager;
            int num = -1;
            int num2 = (this.EditControl != null) ? -1 : -2;
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
                    if (((this.EditControl != null) && (((IList) base.InvokeGetProperty(this.EditControl, "Items")).Count > 0)) && (((int) base.InvokeGetProperty(this.EditControl, "SelectedIndex")) == -1))
                    {
                        base.InvokeSetProperty(this.EditControl, "SelectedIndex", num);
                    }
                }
                else if ((num2 >= -1) && (this.EditControl != null))
                {
                    base.InvokeMethod(this.EditControl, "BeginUpdate", new object[0]);
                    base.InvokeSetProperty(this.EditControl, "SelectedIndex", num2);
                    base.InvokeMethod(this.EditControl, "EndUpdate", new object[0]);
                }
            }
        }

        [DefaultValue(-1)]
        public int AlternateTemplateIndex
        {
            get
            {
                return this.m_alternateTemplateIndex;
            }
            set
            {
                if (value != this.m_alternateTemplateIndex)
                {
                    this.m_alternateTemplateIndex = value;
                    if (this.EditControl != null)
                    {
                        base.InvokeSetProperty(this.EditControl, "AlternateTemplateIndex", this.m_alternateTemplateIndex);
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(13)]
        public int ArrowBoxWidth
        {
            get
            {
                return this.m_arrowBoxWidth;
            }
            set
            {
                if (value != this.m_arrowBoxWidth)
                {
                    this.m_arrowBoxWidth = value;
                    if (this.EditControl != null)
                    {
                        base.InvokeSetProperty(this.EditControl, "ArrowBoxWidth", this.m_arrowBoxWidth);
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
                        base.InvokeSetProperty(this.EditControl, "DataSource", this.DataSource);
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
                        base.InvokeSetProperty(this.EditControl, "DisplayMember", this.m_displayMember);
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

        [Resco.Controls.DetailView.Design.Browsable(false), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
        public bool DroppedDown
        {
            get
            {
                return ((this.EditControl != null) && ((bool) base.InvokeGetProperty(this.EditControl, "DroppedDown")));
            }
            set
            {
                if (this.EditControl != null)
                {
                    base.InvokeSetProperty(this.EditControl, "DroppedDown", value);
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

        [DefaultValue(false)]
        public bool FullScreenList
        {
            get
            {
                return this.m_fullScreenList;
            }
            set
            {
                if (value != this.m_fullScreenList)
                {
                    this.m_fullScreenList = value;
                    if (this.EditControl != null)
                    {
                        base.InvokeSetProperty(this.EditControl, "FullScreenList", this.m_fullScreenList);
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue((string) null), Resco.Controls.DetailView.Design.Browsable(false)]
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

        [DefaultValue(false)]
        public bool ListGridLines
        {
            get
            {
                return this.m_listGridLines;
            }
            set
            {
                if (value != this.m_listGridLines)
                {
                    this.m_listGridLines = value;
                    if (this.EditControl != null)
                    {
                        base.InvokeSetProperty(this.EditControl, "ListGridLines", this.m_listGridLines);
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
                        if (((IList) base.InvokeGetProperty(this.EditControl, "Items")).Count > 0)
                        {
                            base.InvokeSetProperty(this.EditControl, "SelectedIndex", this.m_SelectedIndex);
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

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden), DefaultValue((string) null), Resco.Controls.DetailView.Design.Browsable(false)]
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
                    this.EditValue = Resco.Controls.DetailView.DetailView.GetBoundItem(this.DataManager, value, this.ValueMember);
                }
                else
                {
                    this.EditValue = value;
                }
            }
        }

        [DefaultValue(0)]
        public int SelectedTemplateIndex
        {
            get
            {
                return this.m_selectedTemplateIndex;
            }
            set
            {
                if (value != this.m_selectedTemplateIndex)
                {
                    this.m_selectedTemplateIndex = value;
                    if (this.EditControl != null)
                    {
                        base.InvokeSetProperty(this.EditControl, "SelectedTemplateIndex", this.m_selectedTemplateIndex);
                    }
                    this.OnPropertyChanged();
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

        [DefaultValue(0)]
        public int TemplateIndex
        {
            get
            {
                return this.m_templateIndex;
            }
            set
            {
                if (value != this.m_templateIndex)
                {
                    this.m_templateIndex = value;
                    if (this.EditControl != null)
                    {
                        base.InvokeSetProperty(this.EditControl, "TemplateIndex", this.m_templateIndex);
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Content)]
        public IList Templates
        {
            get
            {
                return this.m_templates;
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

        [DefaultValue(0)]
        public int TextBoxTemplateIndex
        {
            get
            {
                return this.m_textBoxTemplateIndex;
            }
            set
            {
                if (value != this.m_textBoxTemplateIndex)
                {
                    this.m_textBoxTemplateIndex = value;
                    if (this.EditControl != null)
                    {
                        base.InvokeSetProperty(this.EditControl, "TextBoxTemplateIndex", this.m_textBoxTemplateIndex);
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(false)]
        public bool TouchScrolling
        {
            get
            {
                return this.m_touchScrolling;
            }
            set
            {
                if (value != this.m_touchScrolling)
                {
                    this.m_touchScrolling = value;
                    if (this.EditControl != null)
                    {
                        base.InvokeSetProperty(this.EditControl, "TouchScrolling", this.m_touchScrolling);
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        [Resco.Controls.DetailView.Design.Browsable(false), DefaultValue((string) null), Resco.Controls.DetailView.Design.DesignerSerializationVisibility(Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)]
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
                        base.InvokeSetProperty(this.EditControl, "ValueMember", this.m_valueMember);
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

