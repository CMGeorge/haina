using System;

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BelugaMobile.BInfo
{
    public class BInfo : Form
    {
        private System.Windows.Forms.PictureBox pcB_DisPlay;
        private BelugaMobile.BaseControl.lable_Text_Button ltb_UserName;
        public Panel pnl_Full;
        public Label label1;
        public Label label2;
        private PictureBox pcb_mood;
        private TabControl tbc_commuint;
        private TabPage tab_callrecord;
        private TabPage tab_communityRecord;
        private TabPage tab_Host;
        private TabPage tabPage1;
        private System.Windows.Forms.Panel pnl_disPlay;

        public BInfo() 
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BInfo));
            this.pcB_DisPlay = new System.Windows.Forms.PictureBox();
            this.pnl_disPlay = new System.Windows.Forms.Panel();
            this.pnl_Full = new System.Windows.Forms.Panel();
            this.ltb_UserName = new BelugaMobile.BaseControl.lable_Text_Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pcb_mood = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbc_commuint = new System.Windows.Forms.TabControl();
            this.tab_callrecord = new System.Windows.Forms.TabPage();
            this.tab_communityRecord = new System.Windows.Forms.TabPage();
            this.tab_Host = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pnl_disPlay.SuspendLayout();
            this.pnl_Full.SuspendLayout();
            this.tbc_commuint.SuspendLayout();
            this.SuspendLayout();
            // 
            // pcB_DisPlay
            // 
            this.pcB_DisPlay.BackColor = System.Drawing.Color.Black;
            this.pcB_DisPlay.Dock = System.Windows.Forms.DockStyle.Left;
            this.pcB_DisPlay.Image = ((System.Drawing.Image)(resources.GetObject("pcB_DisPlay.Image")));
            this.pcB_DisPlay.Location = new System.Drawing.Point(0, 0);
            this.pcB_DisPlay.Name = "pcB_DisPlay";
            this.pcB_DisPlay.Size = new System.Drawing.Size(75, 83);
            this.pcB_DisPlay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // pnl_disPlay
            // 
            this.pnl_disPlay.BackColor = System.Drawing.Color.White;
            this.pnl_disPlay.Controls.Add(this.label2);
            this.pnl_disPlay.Controls.Add(this.pcb_mood);
            this.pnl_disPlay.Controls.Add(this.pnl_Full);
            this.pnl_disPlay.Controls.Add(this.pcB_DisPlay);
            this.pnl_disPlay.Location = new System.Drawing.Point(1, 43);
            this.pnl_disPlay.Name = "pnl_disPlay";
            this.pnl_disPlay.Size = new System.Drawing.Size(236, 83);
            // 
            // pnl_Full
            // 
            this.pnl_Full.Controls.Add(this.label1);
            this.pnl_Full.Controls.Add(this.ltb_UserName);
            this.pnl_Full.Location = new System.Drawing.Point(81, 3);
            this.pnl_Full.Name = "pnl_Full";
            this.pnl_Full.Size = new System.Drawing.Size(158, 36);
            // 
            // ltb_UserName
            // 
            this.ltb_UserName.LableText = "头像：";
            this.ltb_UserName.Location = new System.Drawing.Point(51, 0);
            this.ltb_UserName.Name = "ltb_UserName";
            this.ltb_UserName.Size = new System.Drawing.Size(104, 36);
            this.ltb_UserName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 36);
            this.label1.Text = "          姓名:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pcb_mood
            // 
            this.pcb_mood.BackColor = System.Drawing.Color.Black;
            this.pcb_mood.Image = ((System.Drawing.Image)(resources.GetObject("pcb_mood.Image")));
            this.pcb_mood.Location = new System.Drawing.Point(200, 45);
            this.pcb_mood.Name = "pcb_mood";
            this.pcb_mood.Size = new System.Drawing.Size(33, 38);
            this.pcb_mood.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(147, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 36);
            this.label2.Text = "          心情:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbc_commuint
            // 
            this.tbc_commuint.Controls.Add(this.tab_callrecord);
            this.tbc_commuint.Controls.Add(this.tab_communityRecord);
            this.tbc_commuint.Controls.Add(this.tab_Host);
            this.tbc_commuint.Controls.Add(this.tabPage1);
            this.tbc_commuint.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbc_commuint.Location = new System.Drawing.Point(0, 127);
            this.tbc_commuint.Name = "tbc_commuint";
            this.tbc_commuint.SelectedIndex = 0;
            this.tbc_commuint.Size = new System.Drawing.Size(240, 167);
            this.tbc_commuint.TabIndex = 1;
            // 
            // tab_callrecord
            // 
            this.tab_callrecord.Location = new System.Drawing.Point(0, 0);
            this.tab_callrecord.Name = "tab_callrecord";
            this.tab_callrecord.Size = new System.Drawing.Size(240, 144);
            this.tab_callrecord.Text = "通讯记录";
            // 
            // tab_communityRecord
            // 
            this.tab_communityRecord.Location = new System.Drawing.Point(0, 0);
            this.tab_communityRecord.Name = "tab_communityRecord";
            this.tab_communityRecord.Size = new System.Drawing.Size(240, 144);
            this.tab_communityRecord.Text = "社区记录";
            // 
            // tab_Host
            // 
            this.tab_Host.Location = new System.Drawing.Point(0, 0);
            this.tab_Host.Name = "tab_Host";
            this.tab_Host.Size = new System.Drawing.Size(240, 144);
            this.tab_Host.Text = "个人资料";
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(240, 144);
            this.tabPage1.Text = "社区";
            // 
            // BInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.tbc_commuint);
            this.Controls.Add(this.pnl_disPlay);
            this.Name = "BInfo";
            this.Text = "编辑个人信息";
            this.pnl_disPlay.ResumeLayout(false);
            this.pnl_Full.ResumeLayout(false);
            this.tbc_commuint.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }
    }
}
