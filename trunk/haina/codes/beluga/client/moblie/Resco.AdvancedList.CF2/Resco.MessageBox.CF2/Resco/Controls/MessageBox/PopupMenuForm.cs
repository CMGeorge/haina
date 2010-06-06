namespace Resco.Controls.MessageBox
{
    using System;
    using System.Drawing;
    using System.Media;
    using System.Windows.Forms;

    public class PopupMenuForm : OverlayForm
    {
        private object result;
        private static PopupMenuForm CachedForm;
        protected object m_defaultResult;
        protected MessageBoxIcon m_icon;
        protected MenuElement m_menu;
        private static MessageBoxSettings m_settings;
        protected TitleElement m_title;

        protected PopupMenuForm(object defaultResult)
        {
            base.WindowState = FormWindowState.Maximized;
            this.m_defaultResult = defaultResult;
            int scale = 1;
            using (Graphics graphics = base.CreateGraphics())
            {
                scale = (int) (graphics.DpiY / 96f);
            }
            this.m_title = new TitleElement(this, scale);
            this.m_title.ForeColor = Settings.TitleForeground;
            this.m_title.BackColor = Settings.TitleBackground;
            this.m_title.Font = Settings.TitleFont;
            this.m_menu = new MenuElement(this, scale);
            this.m_menu.ForeColor = Settings.Foreground;
            this.m_menu.BackColor = Settings.Background;
            this.m_menu.LineColor = Settings.LineColor;
            this.m_menu.Font = Settings.TextFont;
            this.m_menu.ButtonClicked += new Action<int>(OnMenuButtonClicked);
        }

        protected override Rectangle CalcClientRect()
        {
            return new Rectangle(this.m_title.Left, this.m_title.Top, this.m_title.Width, this.m_title.Height + this.m_menu.Height);
        }

        protected virtual void OnMenuButtonClicked(int obj)
        {
            this.Result = this.m_menu.Items[obj].Result;
            base.Close();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.m_menu.HandleMouseDown(e.X, e.Y) && !this.m_title.Bounds.Contains(e.X, e.Y))
            {
                int num = 0;
                foreach (Resco.Controls.MessageBox.MenuItem item in this.m_menu.Items)
                {
                    if (((this.m_defaultResult == null) && (item.Result == null)) || ((this.m_defaultResult != null) && this.m_defaultResult.Equals(item.Result)))
                    {
                        this.OnMenuButtonClicked(num);
                        return;
                    }
                    num++;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.m_menu.HandleMouseMove(e.X, e.Y);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.m_menu.HandleMouseUp(e.X, e.Y);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.m_title != null)
            {
                int num = 20;
                int num2 = Screen.PrimaryScreen.Bounds.Width - 40;
                this.m_title.Left = num;
                this.m_title.Width = num2;
                this.m_title.OnResize();
                this.m_menu.Left = num;
                this.m_menu.Width = num2;
                this.m_menu.OnResize();
                int num3 = ((Screen.PrimaryScreen.Bounds.Height - this.m_title.Height) - this.m_menu.Height) / 2;
                this.m_title.Top = num3;
                this.m_menu.Top = this.m_title.Bottom;
            }
        }

        protected static void PlaySound(MessageBoxIcon icon)
        {
            MessageBoxIcon icon2 = icon;
            if (icon2 <= MessageBoxIcon.Question)
            {
                if (icon2 != MessageBoxIcon.Hand)
                {
                    if (icon2 == MessageBoxIcon.Question)
                    {
                        SystemSounds.Question.Play();
                    }
                    return;
                }
            }
            else
            {
                if (icon2 != MessageBoxIcon.Exclamation)
                {
                    if (icon2 == MessageBoxIcon.Asterisk)
                    {
                        SystemSounds.Asterisk.Play();
                    }
                    return;
                }
                SystemSounds.Exclamation.Play();
                return;
            }
            SystemSounds.Hand.Play();
        }

        protected override void Render(Graphics g, Point offset)
        {
            this.m_title.Render(g, offset);
            this.m_menu.Render(g, offset);
            if (this.m_icon != MessageBoxIcon.None)
            {
                PlaySound(this.m_icon);
                this.m_icon = MessageBoxIcon.None;
            }
        }

        public static T Show<T>(MessageBoxIcon icon, bool bMultiLine, string text, T defaultResult, params Resco.Controls.MessageBox.MenuItem[] items)
        {
            T result;
            if (CachedForm == null)
            {
                CachedForm = new PopupMenuForm(defaultResult);
            }
            try
            {
                CachedForm.m_title.SetText(text, bMultiLine);
                CachedForm.m_menu.Items = items;
                CachedForm.m_icon = icon;
                CachedForm.m_bFirstPaint = true;
                CachedForm.ShowDialog();
                result = (T) CachedForm.Result;
            }
            catch
            {
                CachedForm.Hide();
                throw;
            }
            return result;
        }

        public object Result
        {
            get
            {
                return this.result;
            }
            set
            {
                this.result = value;
            }
        }

        public static MessageBoxSettings Settings
        {
            get
            {
                if (m_settings == null)
                {
                    MessageBoxSettings settings = new MessageBoxSettings();
                    settings.Foreground = Color.Black;
                    settings.Background = Color.White;
                    settings.LineColor = Color.Gainsboro;
                    settings.TitleBackground = Color.FromArgb(0x33, 0x33, 0x33);
                    settings.TitleForeground = Color.White;
                    settings.TextFont = new Font("Tahoma", 13f, FontStyle.Regular);
                    settings.TitleFont = new Font("Tahoma", 10f, FontStyle.Regular);
                    m_settings = settings;
                }
                return m_settings;
            }
            set
            {
                m_settings = value;
            }
        }
    }
}

