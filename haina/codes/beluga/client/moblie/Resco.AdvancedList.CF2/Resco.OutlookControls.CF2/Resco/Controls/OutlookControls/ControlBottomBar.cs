namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal class ControlBottomBar
    {
        private System.Drawing.Size KWindowFrameRoundSize = new System.Drawing.Size(10, 10);
        private Color m_BackColor = SystemColors.Control;
        private Bitmap m_bmpOffscreen;
        private Color m_BorderColor = Color.FromArgb(0x1f, 0x25, 0x2c);
        private List<BottomBarButton> m_Buttons = new List<BottomBarButton>();
        private int m_ClickedButtonIndex = -1;
        private System.Drawing.Font m_Font;
        private Graphics m_GxOff;
        private int m_Height;
        private int m_KMargin = 3;
        private Point m_Location;
        private IParentControl m_Parent;
        private bool m_Visible;
        private int m_Width;

        public event ButtonClickEventHandler ButtonClick;

        public ControlBottomBar(IParentControl aParent)
        {
            this.m_Parent = aParent;
            this.m_Font = new System.Drawing.Font(FontFamily.GenericSerif, 10f, FontStyle.Bold);
            BottomBarButton button = new BottomBarButton("Today");
            button.Type = Resco.Controls.OutlookControls.ButtonType.Today;
            this.m_Buttons.Add(button);
            BottomBarButton button2 = new BottomBarButton("None");
            button2.Type = Resco.Controls.OutlookControls.ButtonType.None;
            this.m_Buttons.Add(button2);
        }

        private void ClearSelectionForButtons()
        {
            for (int i = 0; i < this.m_Buttons.Count; i++)
            {
                this.m_Buttons[i].Pressed = false;
            }
        }

        protected void Dispose(bool disposing)
        {
            if (disposing && (this.m_Font != null))
            {
                this.m_Font.Dispose();
                this.m_Font = null;
            }
        }

        private void DrawButton(Graphics gr, BottomBarButton aButton)
        {
            Color color = aButton.Pressed ? aButton.ForeColorSelected : aButton.ForeColor;
            if (aButton.Pressed)
            {
                Color color2 = aButton.Pressed ? aButton.BackColorSelected : aButton.BackColor;
                Rectangle clientRectangle = aButton.ClientRectangle;
                clientRectangle.Width--;
                clientRectangle.Height--;
                using (Pen pen = new Pen(color2))
                {
                    TouchDTPRenderer.DrawRoundedRect(gr, pen, color2, clientRectangle, this.KWindowFrameRoundSize);
                }
            }
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            using (SolidBrush brush = new SolidBrush(color))
            {
                gr.DrawString(aButton.Text, this.Font, brush, aButton.ClientRectangle, format);
            }
        }

        public void DrawControl(Graphics gr)
        {
            Region region = new Region(this.ClientRectangle);
            gr.Clip = region;
            Rectangle clientRectangle = this.ClientRectangle;
            int num = 0;
            for (int i = 0; i < this.m_Buttons.Count; i++)
            {
                if (this.m_Buttons[i].Visible)
                {
                    num++;
                }
            }
            int x = this.m_Location.X + this.KMargin;
            int y = this.m_Location.Y + this.KMargin;
            int width = ((this.Width - this.KMargin) / num) - this.KMargin;
            int height = this.Height - (this.KMargin * 2);
            int num7 = 0;
            for (int j = 0; j < this.m_Buttons.Count; j++)
            {
                BottomBarButton aButton = this.m_Buttons[j];
                if (aButton.Visible)
                {
                    this.m_Buttons[j].ClientRectangle = new Rectangle(x, y, width, height);
                    this.DrawButton(gr, aButton);
                    num7++;
                    if (num7 == num)
                    {
                        x += this.Width - (this.KMargin + width);
                    }
                    else
                    {
                        x += this.KMargin + width;
                    }
                }
            }
            gr.ResetClip();
            region.Dispose();
            region = null;
        }

        private int GetSelectedButtonsIndex(MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            for (int i = 0; i < this.m_Buttons.Count; i++)
            {
                BottomBarButton button = this.m_Buttons[i];
                if (button.Visible && button.ClientRectangle.Contains(x, y))
                {
                    return i;
                }
            }
            return -1;
        }

        public void MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.m_ClickedButtonIndex = this.GetSelectedButtonsIndex(e);
                for (int i = 0; i < this.m_Buttons.Count; i++)
                {
                    this.m_Buttons[i].Pressed = this.m_ClickedButtonIndex == i;
                }
                this.Refresh();
            }
        }

        public void MouseMove(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (this.m_ClickedButtonIndex >= 0))
            {
                if (this.GetSelectedButtonsIndex(e) != this.m_ClickedButtonIndex)
                {
                    this.ClearSelectionForButtons();
                }
                else
                {
                    this.m_Buttons[this.m_ClickedButtonIndex].Pressed = true;
                }
                this.Refresh();
            }
        }

        public void MouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if ((this.m_ClickedButtonIndex >= 0) && (this.GetSelectedButtonsIndex(e) == this.m_ClickedButtonIndex))
                {
                    this.OnButtonPressed(this.m_Buttons[this.m_ClickedButtonIndex]);
                }
                this.m_ClickedButtonIndex = -1;
                this.ClearSelectionForButtons();
                this.Refresh();
            }
        }

        private void OnButtonPressed(BottomBarButton aButton)
        {
            if (this.ButtonClick != null)
            {
                this.ButtonClick(this, aButton);
            }
        }

        private void Refresh()
        {
            if (this.m_Parent != null)
            {
                this.m_Parent.ForceRefresh();
            }
        }

        internal void SetButtonsBackColor(Color aColor)
        {
            for (int i = 0; i < this.m_Buttons.Count; i++)
            {
                this.m_Buttons[i].BackColor = aColor;
            }
        }

        internal void SetButtonsBackColorSelectedr(Color aColor)
        {
            for (int i = 0; i < this.m_Buttons.Count; i++)
            {
                this.m_Buttons[i].BackColorSelected = aColor;
            }
        }

        internal void SetButtonsForeColor(Color aColor)
        {
            for (int i = 0; i < this.m_Buttons.Count; i++)
            {
                this.m_Buttons[i].ForeColor = aColor;
            }
        }

        internal void SetButtonsForeColorSelected(Color aColor)
        {
            for (int i = 0; i < this.m_Buttons.Count; i++)
            {
                this.m_Buttons[i].ForeColorSelected = aColor;
            }
        }

        public Color BackColor
        {
            get
            {
                return this.m_BackColor;
            }
            set
            {
                this.m_BackColor = value;
            }
        }

        public Color BorderColor
        {
            get
            {
                return this.m_BorderColor;
            }
            set
            {
                this.m_BorderColor = value;
            }
        }

        public Rectangle ClientRectangle
        {
            get
            {
                return new Rectangle(this.m_Location.X, this.m_Location.Y, this.Width, this.Height);
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this.m_Font;
            }
            set
            {
                this.m_Font = value;
            }
        }

        public int Height
        {
            get
            {
                return (int) (this.m_Height * Roller.m_ScaleFactor.Height);
            }
            set
            {
                this.m_Height = (int) (((float) value) / Roller.m_ScaleFactor.Height);
            }
        }

        internal int KMargin
        {
            get
            {
                return (int) (this.m_KMargin * Roller.m_ScaleFactor.Width);
            }
            set
            {
                this.m_KMargin = (int) (((float) value) / Roller.m_ScaleFactor.Width);
            }
        }

        public Point Location
        {
            get
            {
                return this.m_Location;
            }
            set
            {
                this.m_Location = value;
            }
        }

        public BottomBarButton NoneButton
        {
            get
            {
                if ((this.m_Buttons != null) && (this.m_Buttons.Count != 0))
                {
                    return this.m_Buttons[1];
                }
                return null;
            }
            set
            {
                if ((this.m_Buttons != null) && (this.m_Buttons.Count != 0))
                {
                    this.m_Buttons[1]= value;
                }
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size(this.Width, this.Height);
            }
            set
            {
                this.Width = value.Width;
                this.Height = value.Height;
            }
        }

        public BottomBarButton TodayButton
        {
            get
            {
                if ((this.m_Buttons != null) && (this.m_Buttons.Count != 0))
                {
                    return this.m_Buttons[0];
                }
                return null;
            }
            set
            {
                if ((this.m_Buttons != null) && (this.m_Buttons.Count != 0))
                {
                    this.m_Buttons[0]=value;
                }
            }
        }

        public bool Visible
        {
            get
            {
                return this.m_Visible;
            }
            set
            {
                this.m_Visible = value;
            }
        }

        public int Width
        {
            get
            {
                return (int) (this.m_Width * Roller.m_ScaleFactor.Height);
            }
            set
            {
                this.m_Width = (int) (((float) value) / Roller.m_ScaleFactor.Height);
            }
        }

        public delegate void ButtonClickEventHandler(object sender, BottomBarButton aButton);
    }
}

