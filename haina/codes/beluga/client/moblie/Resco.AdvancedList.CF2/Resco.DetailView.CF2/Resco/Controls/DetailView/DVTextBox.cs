namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.DetailViewInternal;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DVTextBox : Control
    {
        internal static int BorderSize = 1;
        private Pen m_borderPen = new Pen(SystemColors.WindowFrame, (float) BorderSize);
        private System.Windows.Forms.BorderStyle m_borderStyle;
        private Resco.Controls.DetailView.ItemBorder m_ItemBorder = Resco.Controls.DetailView.ItemBorder.Underline;
        private VerticalAlignment m_lineAlign;
        private Pen m_Pen = new Pen(Color.Gray, (float) BorderSize);
        private TextBoxEx m_TextBox = new TextBoxEx();

        public event EventHandler ValueChanged;

        internal DVTextBox()
        {
            this.m_TextBox.Text = "";
            this.m_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m_TextBox.Multiline = false;
            this.m_TextBox.WordWrap = false;
            this.m_TextBox.Bounds = this.GetBounds();
            this.m_TextBox.TextChanged += new EventHandler(this.OnTextBoxTextChanged);
            this.m_TextBox.KeyDown += new KeyEventHandler(this.m_TextBox_KeyDown);
            this.m_TextBox.KeyPress += new KeyPressEventHandler(this.m_TextBox_KeyPress);
            this.m_TextBox.KeyUp += new KeyEventHandler(this.m_TextBox_KeyUp);
            this.m_TextBox.LostFocus += new EventHandler(this.m_TextBox_LostFocus);
            base.Controls.Add(this.m_TextBox);
            this.Text = "";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.m_TextBox != null))
            {
                this.m_TextBox.Dispose();
                this.m_TextBox = null;
            }
            base.Dispose(disposing);
        }

        public void Focus()
        {
            if (this.m_TextBox != null)
            {
                this.m_TextBox.Focus();
            }
        }

        private Rectangle GetBounds()
        {
            Size clientSize = base.ClientSize;
            SizeF ef = new SizeF((float) clientSize.Width, (float) clientSize.Height);
            using (Graphics graphics = base.CreateGraphics())
            {
                ef = graphics.MeasureString((this.m_TextBox.Text != "") ? this.m_TextBox.Text : "0", this.m_TextBox.Font);
            }
            Rectangle rectangle = new Rectangle(2 * BorderSize, BorderSize, base.Width - (4 * BorderSize), (base.Height - (2 * BorderSize)) - 1);
            switch ((this.m_TextBox.Multiline ? VerticalAlignment.Top : this.m_lineAlign))
            {
                case VerticalAlignment.Top:
                    return rectangle;

                case VerticalAlignment.Middle:
                    rectangle.Y += Math.Max(0, (int) ((rectangle.Height - ef.Height) / 2f));
                    rectangle.Height = Math.Min(rectangle.Height, (int) ef.Height);
                    return rectangle;

                case VerticalAlignment.Bottom:
                    rectangle.Y += Math.Max(0, rectangle.Height - ((int) ef.Height));
                    rectangle.Height = Math.Min(rectangle.Height, (int) ef.Height);
                    return rectangle;
            }
            return rectangle;
        }

        private void m_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        private void m_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void m_TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        private void m_TextBox_LostFocus(object sender, EventArgs e)
        {
            base.OnLostFocus(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear((this.m_TextBox != null) ? this.m_TextBox.BackColor : this.BackColor);
            int x = BorderSize / 2;
            switch (this.ItemBorder)
            {
                case Resco.Controls.DetailView.ItemBorder.None:
                    break;

                case Resco.Controls.DetailView.ItemBorder.Flat:
                    e.Graphics.DrawRectangle(this.m_Pen, 0, 0, base.Width - 1, base.Height - 1);
                    break;

                default:
                    e.Graphics.DrawLine(this.m_Pen, 0, (base.Height - 1) + x, base.Width, (base.Height - 1) + x);
                    break;
            }
            switch (this.BorderStyle)
            {
                case System.Windows.Forms.BorderStyle.None:
                    break;

                case System.Windows.Forms.BorderStyle.FixedSingle:
                    e.Graphics.DrawRectangle(this.m_borderPen, x, x, (base.Width - BorderSize) - x, (base.Height - BorderSize) - x);
                    break;

                default:
                    return;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            this.Update();
            base.OnResize(e);
        }

        protected virtual void OnTextBoxTextChanged(object sender, EventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(sender, e);
            }
        }

        public void Update()
        {
            if (this.m_TextBox != null)
            {
                this.m_TextBox.Bounds = this.GetBounds();
            }
            base.Update();
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
                if (this.m_TextBox != null)
                {
                    this.m_TextBox.BackColor = value;
                }
            }
        }

        public System.Windows.Forms.BorderStyle BorderStyle
        {
            get
            {
                return this.m_borderStyle;
            }
            set
            {
                if (this.m_borderStyle != value)
                {
                    this.m_borderStyle = value;
                    this.Update();
                }
            }
        }

        public override System.Windows.Forms.ContextMenu ContextMenu
        {
            get
            {
                if (this.m_TextBox != null)
                {
                    return this.m_TextBox.ContextMenu;
                }
                return base.ContextMenu;
            }
            set
            {
                if (this.m_TextBox != null)
                {
                    this.m_TextBox.ContextMenu = value;
                }
                base.ContextMenu = value;
            }
        }

        public override bool Focused
        {
            get
            {
                if (!((this.m_TextBox != null) ? this.m_TextBox.Focused : false))
                {
                    return base.Focused;
                }
                return true;
            }
        }

        public Resco.Controls.DetailView.ItemBorder ItemBorder
        {
            get
            {
                return this.m_ItemBorder;
            }
            set
            {
                if (this.m_ItemBorder != value)
                {
                    this.m_ItemBorder = value;
                    this.Update();
                }
            }
        }

        public VerticalAlignment LineAlign
        {
            get
            {
                return this.m_lineAlign;
            }
            set
            {
                if (this.m_lineAlign != value)
                {
                    this.m_lineAlign = value;
                    this.Update();
                }
            }
        }

        public override string Text
        {
            get
            {
                if (this.m_TextBox == null)
                {
                    return "";
                }
                return this.m_TextBox.Text;
            }
            set
            {
                if (this.m_TextBox != null)
                {
                    this.m_TextBox.Text = value;
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

