namespace Resco.Controls.OutlookControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class ControlToolTip : UserControl
    {
        private IContainer components;
        private const int KMargin = 200;
        private Label label1;

        public ControlToolTip()
        {
            this.InitializeComponent();
            this.BackColor = SystemColors.Info;
            this.ForeColor = SystemColors.InfoText;
            base.Hide();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        internal Size GetSize(string aText)
        {
            Size size = new Size();
            Graphics graphics = base.CreateGraphics();
            SizeF ef = graphics.MeasureString(aText, this.label1.Font);
            int num = (int) (ef.Width / 200f);
            size.Height = (((int) ef.Height) * (num + 1)) + 4;
            size.Width = (num > 0) ? 0xcc : (((int) ef.Width) + 10);
            graphics.Dispose();
            graphics = null;
            return size;
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            base.SuspendLayout();
            this.label1.Dock = DockStyle.Fill;
            this.label1.Font = new Font("Tahoma", 8f, FontStyle.Regular);
            this.label1.Location = new Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x53, 0x10);
            this.label1.Text = "label1";
            base.AutoScaleDimensions=(new SizeF(96f, 96f));
            base.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackColor = SystemColors.Info;
            base.BorderStyle=(BorderStyle.FixedSingle);
            base.Controls.Add(this.label1);
            base.Name = "ControlToolTip";
            base.Size = new Size(0x53, 0x10);
            base.ResumeLayout(false);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.Hide();
            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.Hide();
            base.OnMouseDown(e);
        }

        public void ShowText(string aText)
        {
            this.ShowText(aText, true, false);
        }

        public void ShowText(string aText, bool anAutoHeight, bool anAutoWidth)
        {
            this.label1.Text = aText;
            Graphics graphics = base.CreateGraphics();
            SizeF ef = graphics.MeasureString(aText, this.label1.Font);
            int num = (int) (ef.Width / 200f);
            if (anAutoHeight)
            {
                base.Height = (((int) ef.Height) * (num + 1)) + 4;
            }
            if (anAutoWidth)
            {
                base.Width = (num > 0) ? 0xcc : (((int) ef.Width) + 10);
            }
            base.BringToFront();
            base.Show();
            graphics.Dispose();
            graphics = null;
        }
    }
}

