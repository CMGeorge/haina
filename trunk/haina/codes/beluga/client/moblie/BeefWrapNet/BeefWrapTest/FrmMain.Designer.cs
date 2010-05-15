namespace BeefWrap
{
    partial class FrmMain
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
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.btnTestSync = new System.Windows.Forms.Button();
            this.listBoxResult = new System.Windows.Forms.ListBox();
            this.btnTestASync = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDownLoad = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnUpFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItemExit);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Text = "退出";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // btnTestSync
            // 
            this.btnTestSync.Location = new System.Drawing.Point(3, 3);
            this.btnTestSync.Name = "btnTestSync";
            this.btnTestSync.Size = new System.Drawing.Size(100, 20);
            this.btnTestSync.TabIndex = 0;
            this.btnTestSync.Text = "测试同步请求";
            this.btnTestSync.Click += new System.EventHandler(this.btnTestSync_Click);
            // 
            // listBoxResult
            // 
            this.listBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxResult.Location = new System.Drawing.Point(0, 63);
            this.listBoxResult.Name = "listBoxResult";
            this.listBoxResult.Size = new System.Drawing.Size(240, 198);
            this.listBoxResult.TabIndex = 1;
            // 
            // btnTestASync
            // 
            this.btnTestASync.Location = new System.Drawing.Point(109, 3);
            this.btnTestASync.Name = "btnTestASync";
            this.btnTestASync.Size = new System.Drawing.Size(100, 20);
            this.btnTestASync.TabIndex = 2;
            this.btnTestASync.Text = "测试异步请求";
            this.btnTestASync.Click += new System.EventHandler(this.btnTestASync_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(157, 29);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(72, 20);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "清空";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDownLoad
            // 
            this.btnDownLoad.Location = new System.Drawing.Point(3, 29);
            this.btnDownLoad.Name = "btnDownLoad";
            this.btnDownLoad.Size = new System.Drawing.Size(72, 20);
            this.btnDownLoad.TabIndex = 4;
            this.btnDownLoad.Text = "下载文件";
            this.btnDownLoad.Click += new System.EventHandler(this.btnDownLoad_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 261);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(240, 7);
            // 
            // btnUpFile
            // 
            this.btnUpFile.Location = new System.Drawing.Point(79, 29);
            this.btnUpFile.Name = "btnUpFile";
            this.btnUpFile.Size = new System.Drawing.Size(72, 20);
            this.btnUpFile.TabIndex = 5;
            this.btnUpFile.Text = "上传文件";
            this.btnUpFile.Click += new System.EventHandler(this.btnUpFile_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.btnUpFile);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnDownLoad);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnTestASync);
            this.Controls.Add(this.listBoxResult);
            this.Controls.Add(this.btnTestSync);
            this.Menu = this.mainMenu1;
            this.Name = "FrmMain";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTestSync;
        private System.Windows.Forms.ListBox listBoxResult;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.Button btnTestASync;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDownLoad;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnUpFile;
    }
}

