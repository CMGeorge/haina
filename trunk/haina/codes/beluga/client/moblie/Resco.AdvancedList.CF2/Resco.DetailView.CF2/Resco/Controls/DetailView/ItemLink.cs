namespace Resco.Controls.DetailView
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ItemLink : ItemTextBox
    {
        public static int LinkWidth = 0x10;
        private SolidBrush m_BlackBrush;
        private Pen m_BlackPen;
        private bool m_ButtonLike;
        private bool m_Clicking;
        private bool m_Editable;
        private SolidBrush m_GrayBrush;
        private Pen m_GrayPen;
        private RescoLinkLocation m_linkLocation;
        private Pen m_WhitePen;

        public ItemLink()
        {
            this.m_BlackBrush = new SolidBrush(Color.Black);
            this.m_BlackPen = new Pen(Color.Black);
            this.m_WhitePen = new Pen(Color.White);
            this.m_GrayPen = new Pen(Color.Gray);
            this.m_GrayBrush = new SolidBrush(SystemColors.Control);
            this.m_Clicking = false;
        }

        public ItemLink(Item toCopy) : base(toCopy)
        {
            if (toCopy is ItemLink)
            {
                this.ButtonLike = ((ItemLink) toCopy).ButtonLike;
                this.m_Editable = ((ItemLink) toCopy).m_Editable;
            }
            this.m_BlackBrush = new SolidBrush(Color.Black);
            this.m_BlackPen = new Pen(Color.Black);
            this.m_WhitePen = new Pen(Color.White);
            this.m_GrayPen = new Pen(Color.Gray);
            this.m_GrayBrush = new SolidBrush(SystemColors.Control);
            this.m_Clicking = false;
        }

        protected override void Click(int yOffset, int parentWidth)
        {
            if ((base.Parent != null) && this.Enabled)
            {
                if (!this.m_Clicking)
                {
                    this.m_Clicking = true;
                    base.Parent.Invalidate();
                    this.OnGotFocus(this, new ItemEventArgs());
                }
                else if (this.ButtonLike)
                {
                    this.OnClicked(this, new ItemEventArgs(this, 0, base.Name));
                }
                else
                {
                    Point lastMousePosition = base.Parent.LastMousePosition;
                    if (this.GetActivePartBoundsLink(yOffset).Contains(base.Parent.LastMousePosition.X, base.Parent.LastMousePosition.Y))
                    {
                        this.Hide();
                        this.OnClicked(this, new ItemEventArgs(this, 0, base.Name));
                    }
                    else if (this.m_Editable)
                    {
                        if (base.EditControl != null)
                        {
                            base.EditControl.Focus();
                            this.OnGotFocus(this, new ItemEventArgs(this, 0, base.Name));
                        }
                        else
                        {
                            base.DisableEvents = true;
                            DVTextBox control = base.Parent.GetControl(typeof(DVTextBox)) as DVTextBox;
                            if (control != null)
                            {
                                control.TextBox.Font = base.TextFont;
                                control.TextBox.ReadOnly = base.ReadOnly;
                                control.TextBox.Text = this.Text;
                                control.TextBox.ScrollBars = base.ScrollBars;
                                if (control.TextBox.Multiline != base.MultiLine)
                                {
                                    control.TextBox.Multiline = base.MultiLine;
                                }
                                if (control.TextBox.TextAlign != base.TextAlign)
                                {
                                    control.TextBox.TextAlign = base.TextAlign;
                                }
                                control.ItemBorder = base.ItemBorder;
                                control.TextBox.BorderStyle = base.Border ? BorderStyle.FixedSingle : BorderStyle.None;
                                control.TextBox.MaxLength = base.MaxLength;
                                control.TextBox.PasswordChar = base.PasswordChar;
                                if (control.TextBox.WordWrap != base.WordWrap)
                                {
                                    control.TextBox.WordWrap = base.WordWrap;
                                }
                                if (base.SelectionStart < 0)
                                {
                                    control.TextBox.SelectionStart = control.Text.Length;
                                }
                                else
                                {
                                    control.TextBox.SelectionStart = base.SelectionStart;
                                    control.TextBox.SelectionLength = base.SelectionLength;
                                }
                                base.EditControl = control;
                                base.EditControl.ValueChanged += new EventHandler(this.OnValueChanged);
                                base.EditControl.Bounds = this.GetActivePartBounds(yOffset);
                                base.DisableEvents = false;
                                this.OnGotFocus(this, new ItemEventArgs(this, 0, base.Name));
                                base.DisableEvents = true;
                                if (base.EditControl != null)
                                {
                                    base.EditControl.Show();
                                    base.EditControl.Focus();
                                }
                            }
                            base.DisableEvents = false;
                        }
                    }
                }
            }
        }

        public override Item Clone()
        {
            ItemLink link = new ItemLink(this);
            link.Value = this.Value;
            link.Text = this.Text;
            return link;
        }

        protected override void Draw(Graphics gr, int yOffset, int parentWidth)
        {
            base.Draw(gr, yOffset, parentWidth);
            if (this.ButtonLike)
            {
                this.DrawButton(gr, this.GetActivePartBoundsLink(yOffset));
            }
            else
            {
                this.DrawLink(gr, this.GetActivePartBoundsLink(yOffset));
            }
        }

        protected virtual void DrawButton(Graphics gr, Rectangle rect)
        {
            Pen whitePen;
            Pen pen2;
            Pen grayPen;
            if (!this.m_Clicking)
            {
                grayPen = this.m_GrayPen;
                whitePen = this.m_WhitePen;
                pen2 = this.m_GrayPen;
            }
            else
            {
                pen2 = whitePen = grayPen = this.m_BlackPen;
            }
            gr.DrawLine(whitePen, rect.Left, rect.Top, rect.Right - 2, rect.Top);
            gr.DrawLine(whitePen, rect.Left, rect.Top, rect.Left, rect.Bottom - 2);
            gr.DrawLine(pen2, rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom - 1);
            gr.DrawLine(pen2, rect.Left, rect.Bottom - 1, rect.Right - 1, rect.Bottom - 1);
            gr.DrawRectangle(grayPen, (int) (rect.Left - 1), (int) (rect.Top - 1), (int) (rect.Width + 1), (int) (rect.Height + 1));
        }

        protected override void DrawItemTextArea(Graphics gr, ref Rectangle textBounds)
        {
            if (!this.ButtonLike)
            {
                textBounds.Width -= LinkWidth + 1;
                if (this.m_linkLocation == RescoLinkLocation.Left)
                {
                    textBounds.X += LinkWidth + 1;
                }
            }
            base.DrawItemTextArea(gr, ref textBounds);
        }

        protected virtual void DrawLink(Graphics gr, Rectangle rect)
        {
            if (this.m_Clicking)
            {
                gr.DrawRectangle(this.m_BlackPen, rect);
            }
            rect.X++;
            rect.Y++;
            rect.Width--;
            rect.Height--;
            if ((base.TextBackColor != Color.Transparent) || !base.Parent.UseGradient)
            {
                gr.FillRectangle(base.m_TextBackBrush, rect);
            }
            else if (base.Parent.UseGradient)
            {
                Rectangle srcrc = base.Parent.CalculateClientRect();
                GradientFill.Fill(gr, rect, srcrc, base.Parent.GradientBackColor);
            }
            int num = LinkWidth / 4;
            int x = (rect.X + num) - 1;
            int y = (rect.Y + rect.Height) - 5;
            gr.FillRectangle(this.m_BlackBrush, x, y, 2, 2);
            x += num;
            gr.FillRectangle(this.m_BlackBrush, x, y, 2, 2);
            x += num;
            gr.FillRectangle(this.m_BlackBrush, x, y, 2, 2);
        }

        public void Edit()
        {
            if ((this.Visible && this.Enabled) && (this.Editable && (base.EditControl == null)))
            {
                base.Activate((base.Parent != null) ? base.Parent.UseClickVisualize : false);
            }
        }

        protected override string FormatValue(object value)
        {
            return this.Text;
        }

        protected override Rectangle GetActivePartBounds(int yOffset)
        {
            Rectangle activePartBounds = base.GetActivePartBounds(yOffset);
            activePartBounds.Width -= LinkWidth + 1;
            if (this.m_linkLocation == RescoLinkLocation.Left)
            {
                activePartBounds.X += LinkWidth + 1;
            }
            return activePartBounds;
        }

        protected Rectangle GetActivePartBoundsLink(int yOffset)
        {
            if (this.ButtonLike)
            {
                return base.GetActivePartBounds(yOffset);
            }
            Point itemXWidth = base.Parent.GetItemXWidth(this);
            int num = base.InternalLabelWidth + base.Parent.SeparatorWidth;
            if (base.Style == RescoItemStyle.LabelRight)
            {
                int num2 = itemXWidth.X + Resco.Controls.DetailView.DetailView.ErrorSpacer;
                if (this.LinkLocation == RescoLinkLocation.Right)
                {
                    num2 = (itemXWidth.X + itemXWidth.Y) - ((num + Resco.Controls.DetailView.DetailView.HorizontalSpacer) + LinkWidth);
                }
                return new Rectangle(base.Parent.CalculateClientRect().X + num2, yOffset, LinkWidth, this.Height - 1);
            }
            int num3 = itemXWidth.X + Resco.Controls.DetailView.DetailView.HorizontalSpacer;
            if (this.LinkLocation == RescoLinkLocation.Right)
            {
                num3 = (itemXWidth.X + itemXWidth.Y) - (Resco.Controls.DetailView.DetailView.ErrorSpacer + LinkWidth);
            }
            else if (base.Style == RescoItemStyle.LabelLeft)
            {
                num3 = (itemXWidth.X + num) + Resco.Controls.DetailView.DetailView.HorizontalSpacer;
            }
            return new Rectangle(base.Parent.CalculateClientRect().X + num3, yOffset + this.LabelHeight, LinkWidth, this.Height - 1);
        }

        protected override int GetActiveWidth()
        {
            int activeWidth = base.GetActiveWidth();
            if (!this.ButtonLike)
            {
                activeWidth -= LinkWidth + 1;
            }
            return activeWidth;
        }

        protected internal override bool HandleKey(Keys key)
        {
            if (this.m_Clicking && (((key == Keys.Right) && !this.m_Editable) || (key == Keys.Return)))
            {
                this.OnClicked(this, new ItemEventArgs());
                return true;
            }
            if ((this.m_Clicking && (key == Keys.Right)) && this.m_Editable)
            {
                this.Edit();
                return true;
            }
            return base.HandleKey(key);
        }

        protected override void Hide()
        {
            this.m_Clicking = false;
            base.Hide();
        }

        protected override void MouseDown(int yOffset, int parentWidth, MouseEventArgs e)
        {
            if (this.Enabled)
            {
                this.m_Clicking = true;
                base.Parent.Invalidate();
            }
        }

        protected override void MouseUp(int yOffset, int parentWidth, MouseEventArgs e)
        {
            if (this.Enabled)
            {
                this.m_Clicking = false;
                base.Parent.Invalidate();
            }
        }

        protected override object Parse(string text)
        {
            return this.Value;
        }

        public bool ShouldSerializeLinkLocation()
        {
            return (this.m_linkLocation != RescoLinkLocation.Right);
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
            return "Link";
        }

        internal override void UpdateWidth(int parentWidth)
        {
            if ((this.Control != null) && (base.Parent != null))
            {
                Point itemXWidth = base.Parent.GetItemXWidth(this);
                if ((base.Style == RescoItemStyle.LabelLeft) || (base.Style == RescoItemStyle.LabelRight))
                {
                    int num = base.InternalLabelWidth + base.Parent.SeparatorWidth;
                    int num2 = (itemXWidth.Y < num) ? itemXWidth.Y : num;
                    parentWidth = ((itemXWidth.Y - num2) - Resco.Controls.DetailView.DetailView.ErrorSpacer) - LinkWidth;
                    if (base.Style == RescoItemStyle.LabelLeft)
                    {
                        this.Control.Left = (itemXWidth.X + num2) + ((this.LinkLocation == RescoLinkLocation.Right) ? 0 : (LinkWidth + 1));
                    }
                }
                else
                {
                    parentWidth = (itemXWidth.Y - (2 * Resco.Controls.DetailView.DetailView.HorizontalSpacer)) - LinkWidth;
                }
                this.Control.Size = new Size(parentWidth, this.Control.Height);
            }
        }

        [DefaultValue(false)]
        public bool ButtonLike
        {
            get
            {
                return this.m_ButtonLike;
            }
            set
            {
                if (this.m_ButtonLike != value)
                {
                    this.m_ButtonLike = value;
                    this.OnPropertyChanged();
                }
            }
        }

        [DefaultValue(false)]
        public bool Editable
        {
            get
            {
                return this.m_Editable;
            }
            set
            {
                if (this.m_Editable != value)
                {
                    this.m_Editable = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public RescoLinkLocation LinkLocation
        {
            get
            {
                return this.m_linkLocation;
            }
            set
            {
                if (this.m_linkLocation != value)
                {
                    this.m_linkLocation = value;
                    this.OnPropertyChanged();
                }
            }
        }
    }
}

