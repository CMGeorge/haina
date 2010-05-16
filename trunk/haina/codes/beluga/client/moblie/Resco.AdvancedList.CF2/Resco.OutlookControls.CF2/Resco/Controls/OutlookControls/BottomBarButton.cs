namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;

    internal class BottomBarButton
    {
        public Color BackColor = Color.FromArgb(0x1f, 0x25, 0x2c);
        public Color BackColorSelected = Color.DarkGray;
        internal Rectangle ClientRectangle = new Rectangle(0, 0, 30, 10);
        public Color ForeColor = Color.White;
        public Color ForeColorSelected = Color.White;
        public bool Pressed;
        public string Text = "Today";
        public Resco.Controls.OutlookControls.ButtonType Type;
        public bool Visible = true;

        public BottomBarButton(string aText)
        {
            this.Text = aText;
        }
    }
}

