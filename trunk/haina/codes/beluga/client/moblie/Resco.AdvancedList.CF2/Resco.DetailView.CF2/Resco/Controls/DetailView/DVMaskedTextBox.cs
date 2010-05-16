namespace Resco.Controls.DetailView
{
    using Resco.Controls.MaskedTextBox;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DVMaskedTextBox : Control
    {
        internal static int BorderSize = 1;
        private Resco.Controls.DetailView.ItemBorder m_ItemBorder = Resco.Controls.DetailView.ItemBorder.Underline;
        private Pen m_Pen = new Pen(Color.Gray, (float) BorderSize);
        private Resco.Controls.MaskedTextBox.MaskedTextBox m_TextBox = new Resco.Controls.MaskedTextBox.MaskedTextBox();

        public event EventHandler ValueChanged;

        internal DVMaskedTextBox()
        {
            this.m_TextBox.Text = "";
            this.m_TextBox.BorderStyle = BorderStyle.None;
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
            this.m_TextBox.Focus();
        }

        private Rectangle GetBounds()
        {
            if (this.MaskedTextBox.BorderStyle == BorderStyle.None)
            {
                return new Rectangle(2 * BorderSize, BorderSize, base.Width - (4 * BorderSize), base.Height - (2 * BorderSize));
            }
            return new Rectangle(0, 0, base.Width, base.Height);
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
            e.Graphics.Clear(this.MaskedTextBox.BackColor);
            switch (this.ItemBorder)
            {
                case Resco.Controls.DetailView.ItemBorder.None:
                    return;

                case Resco.Controls.DetailView.ItemBorder.Flat:
                    e.Graphics.DrawRectangle(this.m_Pen, 0, 0, base.Width - BorderSize, base.Height - BorderSize);
                    return;
            }
            e.Graphics.DrawLine(this.m_Pen, 0, base.Height - BorderSize, base.Width, base.Height - BorderSize);
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
            this.m_TextBox.Bounds = this.GetBounds();
            base.Update();
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

        public bool Focused
        {
            get
            {
                if (!this.m_TextBox.Focused)
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

        public Resco.Controls.MaskedTextBox.MaskedTextBox MaskedTextBox
        {
            get
            {
                return this.m_TextBox;
            }
        }

        public override string Text
        {
            get
            {
                return this.m_TextBox.Text;
            }
            set
            {
                this.m_TextBox.Text = value;
            }
        }
    }
}

