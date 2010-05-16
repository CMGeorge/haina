namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal class ListBoxEx : UserControl
    {
        private SolidBrush m_backSelectedBrush;
        private Bitmap m_bmp;
        private SolidBrush m_foreBrush;
        private SolidBrush m_foreSelectedBrush;
        private Graphics m_graphics;
        private ArrayList m_Items = new ArrayList();
        private int m_selectedIndex = -1;
        private VScrollBar m_vScrollBar = new VScrollBar();

        public event EventHandler SelectedIndexChanged;

        public ListBoxEx()
        {
            this.m_vScrollBar.Visible = false;
            this.m_vScrollBar.ValueChanged += new EventHandler(this.m_vScrollBar_ValueChanged);
            this.m_vScrollBar.Value = 0;
            base.Controls.Add(this.m_vScrollBar);
            this.m_foreBrush = new SolidBrush(this.ForeColor);
            this.m_foreSelectedBrush = new SolidBrush(SystemColors.HighlightText);
            this.m_backSelectedBrush = new SolidBrush(SystemColors.Highlight);
            //base.set_BorderStyle(BorderStyle.FixedSingle);
            base.BorderStyle = (BorderStyle.FixedSingle);
        }

        public void Clear()
        {
            this.m_Items.Clear();
            this.m_selectedIndex = -1;
        }

        public void EnsureVisible(int index)
        {
            int height = base.CreateGraphics().MeasureString("0", this.Font).ToSize().Height;
            int num2 = base.ClientSize.Height / height;
            if (index >= (this.m_vScrollBar.Value + num2))
            {
                this.m_vScrollBar.Value += index - ((this.m_vScrollBar.Value + num2) - 1);
            }
            if (index < this.m_vScrollBar.Value)
            {
                this.m_vScrollBar.Value += index - this.m_vScrollBar.Value;
            }
            base.Invalidate();
        }

        private void m_vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                this.SelectedIndex--;
            }
            if (e.KeyCode == Keys.Down)
            {
                this.SelectedIndex++;
            }
            this.EnsureVisible(this.SelectedIndex);
            base.OnKeyDown(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int height = base.CreateGraphics().MeasureString("0", this.Font).ToSize().Height;
            int num2 = (e.Y / height) + this.m_vScrollBar.Value;
            this.SelectedIndex = num2;
            base.OnMouseDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (((this.m_bmp == null) || (this.m_bmp.Width != base.Width)) || (this.m_bmp.Height != base.Height))
            {
                this.m_bmp = new Bitmap(base.ClientSize.Width, base.ClientSize.Height);
                this.m_graphics = Graphics.FromImage(this.m_bmp);
            }
            if (this.m_foreBrush.Color != this.ForeColor)
            {
                this.m_foreBrush.Color = this.ForeColor;
            }
            this.m_graphics.Clear(this.BackColor);
            int height = this.m_graphics.MeasureString("0", this.Font).ToSize().Height;
            int num2 = base.ClientSize.Height / height;
            int count = this.m_Items.Count;
            if (num2 < count)
            {
                this.m_vScrollBar.Width = (int) ((13f * base.CreateGraphics().DpiX) / 96f);
                this.m_vScrollBar.Height = base.ClientSize.Height;
                this.m_vScrollBar.Top = 0;
                this.m_vScrollBar.Left = base.ClientSize.Width - this.m_vScrollBar.Width;
                this.m_vScrollBar.Minimum = 0;
                this.m_vScrollBar.Maximum = count - 1;
                this.m_vScrollBar.SmallChange = 1;
                this.m_vScrollBar.LargeChange = num2;
                this.m_vScrollBar.Visible = true;
            }
            else
            {
                this.m_vScrollBar.Visible = false;
                this.m_vScrollBar.Value = 0;
            }
            int y = 0;
            for (int i = this.m_vScrollBar.Value; (i < ((this.m_vScrollBar.Value + num2) + 1)) && (i < this.m_Items.Count); i++)
            {
                object obj2 = this.m_Items[i];
                if (this.m_selectedIndex == i)
                {
                    this.m_graphics.FillRectangle(this.m_backSelectedBrush, 0, y, base.ClientSize.Width - 1, height);
                }
                this.m_graphics.DrawString(obj2.ToString(), this.Font, (this.m_selectedIndex == i) ? this.m_foreSelectedBrush : this.m_foreBrush, 3f, (float) y);
                y += height;
            }
            e.Graphics.DrawImage(this.m_bmp, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected virtual void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(sender, e);
            }
        }

        public ArrayList Items
        {
            get
            {
                return this.m_Items;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.m_selectedIndex;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value >= this.m_Items.Count)
                {
                    value = this.m_Items.Count - 1;
                }
                if (this.m_selectedIndex != value)
                {
                    this.m_selectedIndex = value;
                    this.OnSelectedIndexChanged(this, new EventArgs());
                    base.Invalidate();
                }
            }
        }
    }
}

