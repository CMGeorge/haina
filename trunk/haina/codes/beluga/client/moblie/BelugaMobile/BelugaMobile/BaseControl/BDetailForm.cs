using System;
using System.Collections.Generic;
using System.Text;

namespace BelugaMobile.BaseControl
{
    public  class BDetailForm:BForm
    {
        private System.Windows.Forms.Panel pnl_ToolBar;
        private System.Windows.Forms.Panel pnl_Contents;
        private System.Windows.Forms.Panel pnl_top;

        private void InitializeComponent()
        {
            this.pnl_top = new System.Windows.Forms.Panel();
            this.pnl_ToolBar = new System.Windows.Forms.Panel();
            this.pnl_Contents = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnl_top
            // 
            this.pnl_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_top.Location = new System.Drawing.Point(0, 0);
            this.pnl_top.Name = "pnl_top";
            this.pnl_top.Size = new System.Drawing.Size(240, 27);
            // 
            // pnl_ToolBar
            // 
            this.pnl_ToolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_ToolBar.Location = new System.Drawing.Point(0, 27);
            this.pnl_ToolBar.Name = "pnl_ToolBar";
            this.pnl_ToolBar.Size = new System.Drawing.Size(240, 31);
            // 
            // pnl_Contents
            // 
            this.pnl_Contents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Contents.Location = new System.Drawing.Point(0, 58);
            this.pnl_Contents.Name = "pnl_Contents";
            this.pnl_Contents.Size = new System.Drawing.Size(240, 236);
            // 
            // BDetailForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.pnl_Contents);
            this.Controls.Add(this.pnl_ToolBar);
            this.Controls.Add(this.pnl_top);
            this.Name = "BDetailForm";
            this.ResumeLayout(false);

        }
    }
}
