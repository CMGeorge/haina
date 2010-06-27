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

        public string TextBoxText
        {
            get
            {
                return this.textBox.Text;
            }
            set
            {
                this.textBox.Text = value;
            }
        }
    }
}
