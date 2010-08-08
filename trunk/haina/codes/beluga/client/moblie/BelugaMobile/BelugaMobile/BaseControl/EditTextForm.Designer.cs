namespace BelugaMobile.BaseControl
{
    partial class EditTextForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditTextForm));
            this.panel = new BelugaMobile.BaseControl.BasePanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Edit_textBox = new System.Windows.Forms.TextBox();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BackgroundImage = null;
            this.panel.Controls.Add(this.pictureBox2);
            this.panel.Controls.Add(this.pictureBox1);
            this.panel.Controls.Add(this.Edit_textBox);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(200, 200);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(143, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(54, 30);
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(45, 30);
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Edit_textBox
            // 
            this.Edit_textBox.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Edit_textBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Edit_textBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Edit_textBox.Location = new System.Drawing.Point(0, 40);
            this.Edit_textBox.Multiline = true;
            this.Edit_textBox.Name = "Edit_textBox";
            this.Edit_textBox.Size = new System.Drawing.Size(200, 160);
            this.Edit_textBox.TabIndex = 0;
            this.Edit_textBox.TextChanged += new System.EventHandler(this.Edit_textBox_TextChanged);
            // 
            // EditTextForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(200, 200);
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "EditTextForm";
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BaseControl.BasePanel panel;
        private System.Windows.Forms.TextBox Edit_textBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}