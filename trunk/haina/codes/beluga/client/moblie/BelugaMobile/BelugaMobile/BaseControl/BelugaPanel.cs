using System;
using System.Collections.Generic;
using System.Text;
using BelugaMobile.Db;

namespace BelugaMobile.Control
{
    class BelugaPanel : GPanel
    {
        public System.Windows.Forms.Panel secondpanel;
        public System.Windows.Forms.Panel toppanel;
        public System.Windows.Forms.Button left;
        public System.Windows.Forms.Label title;
        public System.Windows.Forms.Button right;

        public void InitializeComponent()
        {
            this.secondpanel = new System.Windows.Forms.Panel();
            this.toppanel = new System.Windows.Forms.Panel();
            this.left = new System.Windows.Forms.Button();
            this.right = new System.Windows.Forms.Button();
            this.title = new System.Windows.Forms.Label();
            this.toppanel.SuspendLayout();
            this.SuspendLayout();
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
            // left
            // 
            this.left.Location = new System.Drawing.Point(4, 4);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(54, 20);
            this.left.TabIndex = 0;
            this.left.Text = "left";
            // 
            // right
            // 
            this.right.Location = new System.Drawing.Point(179, 4);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(58, 20);
            this.right.TabIndex = 1;
            this.right.Text = "right";
            // 
            // title
            // 
            this.title.Location = new System.Drawing.Point(65, 4);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(108, 20);
            this.title.Text = "title";
            this.title.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            this.Location = new System.Drawing.Point(0, 0);
            this.Size = new System.Drawing.Size(240, 259);
            this.Controls.Add(toppanel);
            this.Controls.Add(secondpanel);
        }

        public virtual void ShowPanel(BelugaDb db)
        {
        }
    }
}
