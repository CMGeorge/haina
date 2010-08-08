namespace BelugaMobile.BaseControl
{
    partial class DesignUsedForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.contactpanel = new System.Windows.Forms.Panel();
            this.secondpanel = new System.Windows.Forms.Panel();
            this.toppanel = new System.Windows.Forms.Panel();
            this.title = new System.Windows.Forms.Label();
            this.right = new System.Windows.Forms.Button();
            this.left = new System.Windows.Forms.Button();
            this.contactpanel.SuspendLayout();
            this.toppanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contactpanel
            // 
            this.contactpanel.Controls.Add(this.secondpanel);
            this.contactpanel.Controls.Add(this.toppanel);
            this.contactpanel.Location = new System.Drawing.Point(0, 0);
            this.contactpanel.Name = "contactpanel";
            this.contactpanel.Size = new System.Drawing.Size(240, 259);
            // 
            // secondpanel
            // 
            this.secondpanel.BackColor = System.Drawing.Color.LightGray;
            this.secondpanel.Location = new System.Drawing.Point(0, 30);
            this.secondpanel.Name = "secondpanel";
            this.secondpanel.Size = new System.Drawing.Size(240, 25);
            // 
            // toppanel
            // 
            this.toppanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.toppanel.Controls.Add(this.title);
            this.toppanel.Controls.Add(this.right);
            this.toppanel.Controls.Add(this.left);
            this.toppanel.Location = new System.Drawing.Point(0, 0);
            this.toppanel.Name = "toppanel";
            this.toppanel.Size = new System.Drawing.Size(240, 30);
            // 
            // title
            // 
            this.title.Location = new System.Drawing.Point(65, 4);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(108, 20);
            this.title.Text = "title";
            this.title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // right
            // 
            this.right.Location = new System.Drawing.Point(179, 4);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(58, 20);
            this.right.TabIndex = 1;
            this.right.Text = "right";
            // 
            // left
            // 
            this.left.Location = new System.Drawing.Point(4, 4);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(54, 20);
            this.left.TabIndex = 0;
            this.left.Text = "left";
            // 
            // DesignUsedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.contactpanel);
            this.Menu = this.mainMenu1;
            this.Name = "DesignUsedForm";
            this.Text = "DesignUsedForm";
            this.contactpanel.ResumeLayout(false);
            this.toppanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel contactpanel;
        private System.Windows.Forms.Panel secondpanel;
        private System.Windows.Forms.Panel toppanel;
        private System.Windows.Forms.Button left;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Button right;
    }
}