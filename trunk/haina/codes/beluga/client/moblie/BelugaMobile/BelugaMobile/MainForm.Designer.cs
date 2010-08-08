namespace BelugaMobile
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Resco.Controls.CommonControls.ToolbarControl main_Toolbar;
        private Resco.Controls.CommonControls.ToolbarItem tbi_contact;
        private Resco.Controls.CommonControls.ToolbarItem tbi_profile;
        private Resco.Controls.CommonControls.ToolbarItem tbi_news;
        private Resco.Controls.CommonControls.ToolbarItem tbi_msg;
        private Resco.Controls.CommonControls.ToolbarItem tbi_app;
        private ContactPanel contactpanel;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.contactpanel = new ContactPanel();
            this.main_Toolbar = new Resco.Controls.CommonControls.ToolbarControl();
            this.tbi_contact = new Resco.Controls.CommonControls.ToolbarItem();
            this.tbi_profile = new Resco.Controls.CommonControls.ToolbarItem();
            this.tbi_news = new Resco.Controls.CommonControls.ToolbarItem();
            this.tbi_msg = new Resco.Controls.CommonControls.ToolbarItem();
            this.tbi_app = new Resco.Controls.CommonControls.ToolbarItem();
            this.SuspendLayout();

            // 
            // main_Toolbar
            // 
            this.main_Toolbar.Items.Add(this.tbi_contact);
            this.main_Toolbar.Items.Add(this.tbi_profile);
            this.main_Toolbar.Items.Add(this.tbi_news);
            this.main_Toolbar.Items.Add(this.tbi_msg);
            this.main_Toolbar.Items.Add(this.tbi_app);
            this.main_Toolbar.Location = new System.Drawing.Point(0, 259);
            this.main_Toolbar.Name = "main_Toolbar";
            this.main_Toolbar.SelectedIndex = -1;
            this.main_Toolbar.Size = new System.Drawing.Size(240, 35);
            this.main_Toolbar.StretchBackgroundImage = true;
            this.main_Toolbar.TabIndex = 0;
            this.main_Toolbar.SelectionChanged += new System.EventHandler(this.Toolbar_SelectIndexChanged);
            // 
            // tbi_contact
            // 
            this.tbi_contact.CustomSize = new System.Drawing.Size(0, 0);
            this.tbi_contact.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_contact.ImageDefault")));
            this.tbi_contact.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_contact.ImagePressed")));
            this.tbi_contact.Name = "tbi_contact";
            this.tbi_contact.StretchImage = true;
            // 
            // tbi_profile
            // 
            this.tbi_profile.CustomSize = new System.Drawing.Size(0, 0);
            this.tbi_profile.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_profile.ImageDefault")));
            this.tbi_profile.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_profile.ImagePressed")));
            this.tbi_profile.Name = "tbi_profile";
            this.tbi_profile.StretchImage = true;
            // 
            // tbi_news
            // 
            this.tbi_news.CustomSize = new System.Drawing.Size(0, 0);
            this.tbi_news.ImageAlignment = Resco.Controls.CommonControls.Alignment.MiddleLeft;
            this.tbi_news.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_news.ImageDefault")));
            this.tbi_news.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_news.ImagePressed")));
            this.tbi_news.Name = "tbi_news";
            this.tbi_news.StretchImage = true;
            // 
            // tbi_msg
            // 
            this.tbi_msg.CustomSize = new System.Drawing.Size(0, 0);
            this.tbi_msg.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_msg.ImageDefault")));
            this.tbi_msg.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_msg.ImagePressed")));
            this.tbi_msg.Name = "tbi_msg";
            this.tbi_msg.StretchImage = true;
            // 
            // tbi_app
            // 
            this.tbi_app.CustomSize = new System.Drawing.Size(0, 0);
            this.tbi_app.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_app.ImageDefault")));
            this.tbi_app.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_app.ImagePressed")));
            this.tbi_app.Name = "tbi_app";
            this.tbi_app.StretchImage = true;
            //
            // contactpanel
            //
            contactpanel.InitContactPanel();
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.main_Toolbar);
            this.Controls.Add(contactpanel);
            this.Name = "MainForm";
            this.Text = "Sihus";
            this.Load += new System.EventHandler(this.mainform_Load);
            this.Closed += new System.EventHandler(this.mainform_Close);
            this.ResumeLayout(false);

        }
        #endregion
    }
}

