namespace BelugaMobile.BInfo
{
    partial class HostInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HostInfo));
            this.pcB_DisPlay = new System.Windows.Forms.PictureBox();
            this.pnl_disPlay = new System.Windows.Forms.Panel();
            this.pnl_Name = new System.Windows.Forms.Panel();
            this.lb_Name = new System.Windows.Forms.Label();
            this.ltb_Name = new BelugaMobile.BaseControl.lable_Text_Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lable_Text_Button1 = new BelugaMobile.BaseControl.lable_Text_Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnl_disPlay.SuspendLayout();
            this.pnl_Name.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.pnl_disPlay.Controls.Add(this.panel1);
            this.pnl_disPlay.Controls.Add(this.pnl_Name);
            this.pnl_disPlay.Controls.Add(this.pcB_DisPlay);
            this.pnl_disPlay.Location = new System.Drawing.Point(2, 51);
            this.pnl_disPlay.Name = "pnl_disPlay";
            this.pnl_disPlay.Size = new System.Drawing.Size(236, 83);
            // 
            // pnl_Name
            // 
            this.pnl_Name.Controls.Add(this.ltb_Name);
            this.pnl_Name.Controls.Add(this.lb_Name);
            this.pnl_Name.Location = new System.Drawing.Point(81, 3);
            this.pnl_Name.Name = "pnl_Name";
            this.pnl_Name.Size = new System.Drawing.Size(158, 36);
            // 
            // lb_Name
            // 
            this.lb_Name.Dock = System.Windows.Forms.DockStyle.Left;
            this.lb_Name.Location = new System.Drawing.Point(0, 0);
            this.lb_Name.Name = "lb_Name";
            this.lb_Name.Size = new System.Drawing.Size(36, 36);
            this.lb_Name.Text = "        姓名:";
            this.lb_Name.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ltb_Name
            // 
            this.ltb_Name.Dock = System.Windows.Forms.DockStyle.Right;
            this.ltb_Name.Location = new System.Drawing.Point(33, 0);
            this.ltb_Name.Name = "ltb_Name";
            this.ltb_Name.Size = new System.Drawing.Size(125, 36);
            this.ltb_Name.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lable_Text_Button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(81, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(158, 36);
            // 
            // lable_Text_Button1
            // 
            this.lable_Text_Button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.lable_Text_Button1.Location = new System.Drawing.Point(33, 0);
            this.lable_Text_Button1.Name = "lable_Text_Button1";
            this.lable_Text_Button1.Size = new System.Drawing.Size(125, 36);
            this.lable_Text_Button1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 36);
            this.label1.Text = "        姓名:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // HostInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.pnl_disPlay);
            this.Name = "HostInfo";
            this.Text = "机主信息";
            this.pnl_disPlay.ResumeLayout(false);
            this.pnl_Name.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pcB_DisPlay;
        private System.Windows.Forms.Panel pnl_disPlay;
        public System.Windows.Forms.Panel pnl_Name;
        public System.Windows.Forms.Label lb_Name;
        private BelugaMobile.BaseControl.lable_Text_Button ltb_Name;
        public System.Windows.Forms.Panel panel1;
        private BelugaMobile.BaseControl.lable_Text_Button lable_Text_Button1;
        public System.Windows.Forms.Label label1;
    }
}