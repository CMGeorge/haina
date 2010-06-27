using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BelugaMobile.BaseControl
{
    public delegate void sendTextHandler(string text);
    public partial class EditTextForm : Form
    {
        public EditTextForm()
        {
            InitializeComponent();
        }
        public event sendTextHandler sendText;
        private void Edit_textBox_TextChanged(object sender, EventArgs e)
        {
            if (sendText != null)
            {
                sendText(this.Edit_textBox.Text);
            }
        }
    }
}