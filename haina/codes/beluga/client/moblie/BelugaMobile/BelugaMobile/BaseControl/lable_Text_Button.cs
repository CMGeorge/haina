using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BelugaMobile.BaseControl
{
    
    public partial class lable_Text_Button : UserControl
    {
        public lable_Text_Button()
        {
            InitializeComponent();
        }

        public string LableText 
        {
            get 
            {
                return this.lable.Text;
            }
            set 
            {
                this.lable.Text = value;
            }
        }

        public override string Text
        {
            get
            {
                return this.lable.Text;
            }
            set
            {
                this.lable.Text = value;
            }
        }


        private void pictureBox_Click(object sender, EventArgs e)
        {
            EditTextForm eidtTxt = new EditTextForm();
            eidtTxt.EditText = LableText;
            eidtTxt.sendText += new sendTextHandler(eidtTxt_sendText);
            eidtTxt.ShowDialog();
        }

        void eidtTxt_sendText(string text)
        {
            LableText = text;
        }
    }
}
