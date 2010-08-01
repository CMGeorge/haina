using System.Drawing;
namespace BelugaMobile.BInfo
{
    partial class ContactInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContactInfo));
            this.pnl_disPlay = new System.Windows.Forms.Panel();
            this.lab_sig = new System.Windows.Forms.Label();
            this.lab_name = new System.Windows.Forms.Label();
            this.pcB_DisPlay = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new Resco.Controls.CommonControls.TabControl();
            this.advancedList1 = new Resco.Controls.AdvancedList.AdvancedList();
            this.pnl_disPlay.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_disPlay
            // 
            this.pnl_disPlay.BackColor = System.Drawing.Color.White;
            this.pnl_disPlay.Controls.Add(this.lab_sig);
            this.pnl_disPlay.Controls.Add(this.lab_name);
            this.pnl_disPlay.Controls.Add(this.pcB_DisPlay);
            this.pnl_disPlay.Location = new System.Drawing.Point(1, 43);
            this.pnl_disPlay.Name = "pnl_disPlay";
            this.pnl_disPlay.Size = new System.Drawing.Size(239, 83);
            // 
            // lab_sig
            // 
            this.lab_sig.Location = new System.Drawing.Point(94, 34);
            this.lab_sig.Name = "lab_sig";
            this.lab_sig.Size = new System.Drawing.Size(142, 20);
            this.lab_sig.Text = "签名";
            // 
            // lab_name
            // 
            this.lab_name.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lab_name.Location = new System.Drawing.Point(94, 12);
            this.lab_name.Name = "lab_name";
            this.lab_name.Size = new System.Drawing.Size(53, 18);
            this.lab_name.Text = "姓名";
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
            // tabControl1
            // 
            this.tabControl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabControl1.BackgroundImage")));
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 126);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Size = new System.Drawing.Size(240, 168);
            this.tabControl1.TabIndex = 5;
            this.tabControl1.Text = "tabControl1";
            // 
            // advancedList1
            // 
            this.advancedList1.GridColor = System.Drawing.Color.Empty;
            this.advancedList1.Location = new System.Drawing.Point(0, 0);
            this.advancedList1.Name = "advancedList1";
            this.advancedList1.Size = new System.Drawing.Size(150, 150);
            this.advancedList1.TabIndex = 0;
            // 
            // ContactInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pnl_disPlay);
            this.Name = "ContactInfo";
            this.Text = "联系人信息";
            this.pnl_disPlay.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public void loadTabControl() 
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BelugaMobile.BaseControl.IntegratedForm));
            this.tabControl1.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("main_Toolbar.BackgroundImage")));
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 94);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Size = new System.Drawing.Size(240, 200);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Text = "tabControl1";

            this.tabPage1 = new Resco.Controls.CommonControls.TabPage();
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.TabItem.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_app.ImageDefault")));
            this.tabPage1.TabItem.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_app.ImagePressed")));
            //this.tabPage1.Size = new System.Drawing.Size(100, 100);

            
            this.tabPage1.BackColor = Color.Red;
            this.tabControl1.TabPages.Add(tabPage1);


            this.tabPage2 = new Resco.Controls.CommonControls.TabPage();
            this.tabPage2.Name = "tabPage1";
            this.tabPage2.TabItem.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_app.ImageDefault")));
            this.tabPage2.TabItem.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_app.ImagePressed")));
            //this.tabPage1.Size = new System.Drawing.Size(100, 100);
            this.tabPage2.BackColor = Color.Blue;
            this.tabControl1.TabPages.Add(tabPage2);


            this.tabPage3 = new Resco.Controls.CommonControls.TabPage();
            this.tabPage3.Name = "tabPage1";
            this.tabPage3.BackColor = Color.Aquamarine;
            this.tabPage3.TabItem.ImageDefault = ((System.Drawing.Bitmap)(resources.GetObject("tbi_app.ImageDefault")));
            this.tabPage3.TabItem.ImagePressed = ((System.Drawing.Bitmap)(resources.GetObject("tbi_app.ImagePressed")));
            //this.tabPage1.Size = new System.Drawing.Size(100, 100);

            this.tabControl1.TabPages.Add(tabPage3);
            this.tabPage1.Controls.Add(advancedList1);

            this.advancedList1.GridColor = System.Drawing.Color.Empty;
            this.advancedList1.Location = new System.Drawing.Point(3, 132);
            this.advancedList1.Name = "advancedList1";
            this.advancedList1.Size = new System.Drawing.Size(157, 95);
            this.advancedList1.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        #endregion

        private System.Windows.Forms.Panel pnl_disPlay;
        private System.Windows.Forms.PictureBox pcB_DisPlay;
        private Resco.Controls.CommonControls.TabControl tabControl1;
        private System.Windows.Forms.Label lab_name;
        private System.Windows.Forms.Label lab_sig;
        private Resco.Controls.CommonControls.TabPage tabPage1;
        private Resco.Controls.CommonControls.TabPage tabPage2;
        private Resco.Controls.CommonControls.TabPage tabPage3;
        private Resco.Controls.AdvancedList.AdvancedList advancedList1;
    }
}