using System;

using System.Collections.Generic;
using System.Text;

namespace BelugaMobile.BaseControl
{
   public class IntegratedForm:BaseForm
    {


       private System.Windows.Forms.Panel pnl_Contents;
       private System.Windows.Forms.Panel pnl_ToolBar;
       private Resco.Controls.CommonControls.ToolbarControl main_Toolbar;
       private Resco.Controls.CommonControls.ToolbarItem tbi_contact;
       private Resco.Controls.CommonControls.ToolbarItem tbi_profile;
       private Resco.Controls.CommonControls.ToolbarItem tbi_news;
       private Resco.Controls.CommonControls.ToolbarItem tbi_msg;
       private Resco.Controls.CommonControls.ToolbarItem tbi_app;
       private System.Windows.Forms.Panel pnl_top;

       public IntegratedForm()
       {
           InitializeComponent();
       }

       private void InitializeComponent()
       {
           System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IntegratedForm));
           this.pnl_Contents = new System.Windows.Forms.Panel();
           this.main_Toolbar = new Resco.Controls.CommonControls.ToolbarControl();
           this.tbi_contact = new Resco.Controls.CommonControls.ToolbarItem();
           this.tbi_profile = new Resco.Controls.CommonControls.ToolbarItem();
           this.tbi_news = new Resco.Controls.CommonControls.ToolbarItem();
           this.tbi_msg = new Resco.Controls.CommonControls.ToolbarItem();
           this.tbi_app = new Resco.Controls.CommonControls.ToolbarItem();
           this.pnl_ToolBar = new System.Windows.Forms.Panel();
           this.pnl_top = new System.Windows.Forms.Panel();
           this.SuspendLayout();
           // 
           // pnl_Contents
           // 
           this.pnl_Contents.Location = new System.Drawing.Point(0, 60);
           this.pnl_Contents.Name = "pnl_Contents";
           this.pnl_Contents.Size = new System.Drawing.Size(240, 200);
           // 
           // main_Toolbar
           // 
           this.main_Toolbar.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("main_Toolbar.BackgroundImage")));
           this.main_Toolbar.BackgroundImageVGA = ((System.Drawing.Bitmap)(resources.GetObject("main_Toolbar.BackgroundImageVGA")));
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
           // 
           // tbi_contact
           // 
           this.tbi_contact.CustomSize = new System.Drawing.Size(0, 0);
           this.tbi_contact.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_contact.ImageDefault")));
           this.tbi_contact.ImageDefaultVGA = ((System.Drawing.Bitmap)(resources.GetObject("tbi_contact.ImageDefaultVGA")));
           this.tbi_contact.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_contact.ImagePressed")));
           this.tbi_contact.ImagePressedVGA = ((System.Drawing.Bitmap)(resources.GetObject("tbi_contact.ImagePressedVGA")));
           this.tbi_contact.Name = "tbi_contact";
           this.tbi_contact.StretchImage = true;
           // 
           // tbi_profile
           // 
           this.tbi_profile.CustomSize = new System.Drawing.Size(0, 0);
           this.tbi_profile.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_profile.ImageDefault")));
           this.tbi_profile.ImageDefaultVGA = ((System.Drawing.Bitmap)(resources.GetObject("tbi_profile.ImageDefaultVGA")));
           this.tbi_profile.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_profile.ImagePressed")));
           this.tbi_profile.ImagePressedVGA = ((System.Drawing.Bitmap)(resources.GetObject("tbi_profile.ImagePressedVGA")));
           this.tbi_profile.Name = "tbi_profile";
           this.tbi_profile.StretchImage = true;
           // 
           // tbi_news
           // 
           this.tbi_news.CustomSize = new System.Drawing.Size(0, 0);
           this.tbi_news.ImageAlignment = Resco.Controls.CommonControls.Alignment.MiddleLeft;
           this.tbi_news.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_news.ImageDefault")));
           this.tbi_news.ImageDefaultVGA = ((System.Drawing.Bitmap)(resources.GetObject("tbi_news.ImageDefaultVGA")));
           this.tbi_news.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_news.ImagePressed")));
           this.tbi_news.ImagePressedVGA = ((System.Drawing.Bitmap)(resources.GetObject("tbi_news.ImagePressedVGA")));
           this.tbi_news.Name = "tbi_news";
           this.tbi_news.StretchImage = true;
           // 
           // tbi_msg
           // 
           this.tbi_msg.CustomSize = new System.Drawing.Size(0, 0);
           this.tbi_msg.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_msg.ImageDefault")));
           this.tbi_msg.ImageDefaultVGA = ((System.Drawing.Bitmap)(resources.GetObject("tbi_msg.ImageDefaultVGA")));
           this.tbi_msg.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_msg.ImagePressed")));
           this.tbi_msg.ImagePressedVGA = ((System.Drawing.Bitmap)(resources.GetObject("tbi_msg.ImagePressedVGA")));
           this.tbi_msg.Name = "tbi_msg";
           this.tbi_msg.StretchImage = true;
           // 
           // tbi_app
           // 
           this.tbi_app.CustomSize = new System.Drawing.Size(0, 0);
           this.tbi_app.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_app.ImageDefault")));
           this.tbi_app.ImageDefaultVGA = ((System.Drawing.Bitmap)(resources.GetObject("tbi_app.ImageDefaultVGA")));
           this.tbi_app.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_app.ImagePressed")));
           this.tbi_app.ImagePressedVGA = ((System.Drawing.Bitmap)(resources.GetObject("tbi_app.ImagePressedVGA")));
           this.tbi_app.Name = "tbi_app";
           this.tbi_app.StretchImage = true;
           // 
           // pnl_ToolBar
           // 
           this.pnl_ToolBar.Dock = System.Windows.Forms.DockStyle.Top;
           this.pnl_ToolBar.Location = new System.Drawing.Point(0, 35);
           this.pnl_ToolBar.Name = "pnl_ToolBar";
           this.pnl_ToolBar.Size = new System.Drawing.Size(240, 25);
           // 
           // pnl_top
           // 
           this.pnl_top.Dock = System.Windows.Forms.DockStyle.Top;
           this.pnl_top.Location = new System.Drawing.Point(0, 0);
           this.pnl_top.Name = "pnl_top";
           this.pnl_top.Size = new System.Drawing.Size(240, 35);
           // 
           // IntegratedForm
           // 
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
           this.ClientSize = new System.Drawing.Size(240, 294);
           this.Controls.Add(this.main_Toolbar);
           this.Controls.Add(this.pnl_Contents);
           this.Controls.Add(this.pnl_ToolBar);
           this.Controls.Add(this.pnl_top);
           this.Name = "IntegratedForm";
           this.ResumeLayout(false);

       }
   }
}
