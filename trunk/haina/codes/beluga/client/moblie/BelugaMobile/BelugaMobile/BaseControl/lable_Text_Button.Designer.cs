namespace BelugaMobile.BaseControl
{
    partial class lable_Text_Button
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(lable_Text_Button));
            this.pnl_Full = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.lable = new System.Windows.Forms.Label();
            this.pnl_Full.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_Full
            // 
            this.pnl_Full.Controls.Add(this.lable);
            this.pnl_Full.Controls.Add(this.pictureBox);
            this.pnl_Full.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Full.Location = new System.Drawing.Point(0, 0);
            this.pnl_Full.Name = "pnl_Full";
            this.pnl_Full.Size = new System.Drawing.Size(150, 26);
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(126, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(24, 26);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            // 
            // lable
            // 
            this.lable.Dock = System.Windows.Forms.DockStyle.Left;
            this.lable.Location = new System.Drawing.Point(0, 0);
            this.lable.Name = "lable";
            this.lable.Size = new System.Drawing.Size(127, 26);
            // 
            // lable_Text_Button
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pnl_Full);
            this.Name = "lable_Text_Button";
            this.Size = new System.Drawing.Size(150, 26);
            this.pnl_Full.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel pnl_Full;
        public System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label lable;
    }
}
