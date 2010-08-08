using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BelugaMobile.Db;
using BelugaMobile.BaseControl;
using System.Data.SQLiteClient;
using System.Data.SQLiteClient.Native;
using System.Runtime.InteropServices;
using BelugaMobile.Control;

namespace BelugaMobile
{
    public partial class MainForm : GForm
    {
        private BelugaDb belugadb;

        public MainForm()
        {
            InitializeComponent();
            belugadb = new BelugaDb();
            belugadb.Open();
        }

        //public static BelugaDb _db = new BelugaDb();

        private void mainform_Load(object sender, EventArgs e)
        {
            //if (!File.Exists(_db.DATA_PATH + @"\sihus.ini"))
            {
                // 进入安装后第一次初始化流程
            }
            //else
            {   // 显示主panel，主panel上显示beluga的5个主要page页
                contactpanel.ShowPanel(belugadb);
            }
        }

        private void Toolbar_SelectIndexChanged(object sender, EventArgs e) // 工具栏按钮变化
        {

        }

        private void mainform_Close(object sender, EventArgs e)
        {
            belugadb.Close();
        }
    }
}