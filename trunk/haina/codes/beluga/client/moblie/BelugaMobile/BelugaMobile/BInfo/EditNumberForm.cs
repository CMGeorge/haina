using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BelugaMobile.BInfo
{
    public partial class EditNumberForm :BaseControl.BaseForm
    {

        static EditNumberForm objInstance = null;
        public static EditNumberForm getInstance()
        {
            if (objInstance == null) objInstance = new EditNumberForm();
            return objInstance;
        }

        public EditNumberForm()
        {
            InitializeComponent();
        }
    }
}