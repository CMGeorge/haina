using System;
using System.Collections.Generic;
using System.Text;
using BelugaMobile.Control;
using Resco.Controls.AdvancedList;
using Resco.Controls.ScrollBar;
using BelugaMobile.Db;

namespace BelugaMobile
{
    class ContactPanel : BelugaPanel
    {
        private Resco.Controls.ScrollBar.VScrollBar vScrollBar1;
        private Resco.Controls.ScrollBar.LetterBar letterBar;
        private Resco.Controls.ScrollBar.ScrollBarThumb scrollBarVThumb;
        private Resco.Controls.ScrollBar.ScrollBarTrack scrollVBarTrack;
        private Resco.Controls.AdvancedList.AdvancedList contacts;
        private Resco.Controls.AdvancedList.RowTemplate RowTemplate;
        private Resco.Controls.AdvancedList.ButtonCell photoCell;
        private Resco.Controls.AdvancedList.TextCell nameCell;
        private Resco.Controls.AdvancedList.ButtonCell phoneCell;
        private Resco.Controls.AdvancedList.ButtonCell msgCell;
        private Resco.Controls.AdvancedList.ButtonCell locationCell;
        private Resco.Controls.AdvancedList.ButtonCell cityCell;

        public void InitContactPanel()
        {
            base.InitializeComponent();
            this.title.Text = "四湖客-联系人";
            this.left.Visible = false;
            this.right.Text = "添加";
            this.right.Click += new EventHandler(rightbutton_Click);

            this.contacts = new Resco.Controls.AdvancedList.AdvancedList();
            this.letterBar = new Resco.Controls.ScrollBar.LetterBar();
            this.vScrollBar1 = new Resco.Controls.ScrollBar.VScrollBar();
            this.scrollVBarTrack = new Resco.Controls.ScrollBar.ScrollBarTrack();
            this.scrollBarVThumb = new Resco.Controls.ScrollBar.ScrollBarThumb();
            this.RowTemplate = new Resco.Controls.AdvancedList.RowTemplate();
            this.nameCell = new Resco.Controls.AdvancedList.TextCell();
            this.photoCell = new Resco.Controls.AdvancedList.ButtonCell();
            this.phoneCell = new Resco.Controls.AdvancedList.ButtonCell();
            this.msgCell = new Resco.Controls.AdvancedList.ButtonCell();
            this.locationCell = new Resco.Controls.AdvancedList.ButtonCell();
            this.cityCell = new Resco.Controls.AdvancedList.ButtonCell();
            this.contacts.SuspendLayout();

            // 
            // RowTemplate
            // 
            this.RowTemplate.BackColor = System.Drawing.Color.Gainsboro;
            this.RowTemplate.CellTemplates.Add(this.photoCell);
            this.RowTemplate.CellTemplates.Add(this.nameCell);
            this.RowTemplate.CellTemplates.Add(this.phoneCell);
            this.RowTemplate.CellTemplates.Add(this.msgCell);
            this.RowTemplate.CellTemplates.Add(this.locationCell);
            this.RowTemplate.CellTemplates.Add(this.cityCell);
            this.RowTemplate.Height = 35;
            this.RowTemplate.Name = "RowTemplate";
            // 
            // nameCell
            // 
            this.nameCell.CellSource.ColumnName = "name";
            this.nameCell.Location = new System.Drawing.Point(35, 5);
            this.nameCell.Name = "name";
            this.nameCell.Size = new System.Drawing.Size(90, 25);
            this.nameCell.TextFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
                       
            // 
            // photoCell
            // 
            this.photoCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(237)))), ((int)(((byte)(221)))));
            this.photoCell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(237)))), ((int)(((byte)(221)))));
            this.photoCell.CellSource.ColumnName = "photo";
            this.photoCell.ImageAlignment = Resco.Controls.AdvancedList.Alignment.TopCenter;
            //this.photoCell.ImageDefault = ((System.Drawing.Image)(resources.GetObject("buttonCell1.ImageDefault")));
            //this.photoCell.ImagePressed = ((System.Drawing.Image)(resources.GetObject("buttonCell1.ImagePressed")));
            this.photoCell.Location = new System.Drawing.Point(4, 5);
            this.photoCell.Name = "photo";
            this.photoCell.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(153)))), ((int)(((byte)(51)))));
            this.photoCell.PressedBorderColor = System.Drawing.Color.Black;
            this.photoCell.PressedForeColor = System.Drawing.Color.White;
            this.photoCell.Selectable = false;
            this.photoCell.Size = new System.Drawing.Size(25, 25);
            this.photoCell.Text = "Photo";
            this.photoCell.TextAlignment = Resco.Controls.AdvancedList.Alignment.BottomCenter;
            // 
            // PhoneCell
            // 
            this.phoneCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(237)))), ((int)(((byte)(221)))));
            this.phoneCell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(237)))), ((int)(((byte)(221)))));
            this.phoneCell.ImageAlignment = Resco.Controls.AdvancedList.Alignment.TopCenter;
            //this.phoneCell.ImageDefault = ((System.Drawing.Image)(resources.GetObject("buttonCell2.ImageDefault")));
            //this.phoneCell.ImagePressed = ((System.Drawing.Image)(resources.GetObject("buttonCell2.ImagePressed")));
            this.phoneCell.Location = new System.Drawing.Point(130, 7);
            this.phoneCell.Name = "PhoneCell";
            this.phoneCell.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(153)))), ((int)(((byte)(51)))));
            this.phoneCell.PressedBorderColor = System.Drawing.Color.Black;
            this.phoneCell.PressedForeColor = System.Drawing.Color.White;
            this.phoneCell.Selectable = false;
            this.phoneCell.Size = new System.Drawing.Size(20, 20);
            this.phoneCell.Text = "Phone";
            this.phoneCell.TextAlignment = Resco.Controls.AdvancedList.Alignment.BottomCenter;
            // 
            // msgCell
            // 
            this.msgCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(237)))), ((int)(((byte)(221)))));
            this.msgCell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(237)))), ((int)(((byte)(221)))));
            this.msgCell.ImageAlignment = Resco.Controls.AdvancedList.Alignment.TopCenter;
            //this.msgCell.ImageDefault = ((System.Drawing.Image)(resources.GetObject("buttonCell3.ImageDefault")));
            //this.msgCell.ImagePressed = ((System.Drawing.Image)(resources.GetObject("buttonCell3.ImagePressed")));
            this.msgCell.Location = new System.Drawing.Point(151, 7);
            this.msgCell.Name = "MsgCell";
            this.msgCell.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(153)))), ((int)(((byte)(51)))));
            this.msgCell.PressedBorderColor = System.Drawing.Color.Black;
            this.msgCell.PressedForeColor = System.Drawing.Color.White;
            this.msgCell.Selectable = false;
            this.msgCell.Size = new System.Drawing.Size(20, 20);
            this.msgCell.Text = "Msg";
            this.msgCell.TextAlignment = Resco.Controls.AdvancedList.Alignment.BottomCenter;
            // 
            // locationCell
            // 
            this.locationCell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.locationCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(237)))), ((int)(((byte)(221)))));
            this.locationCell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(237)))), ((int)(((byte)(221)))));
            this.locationCell.ImageAlignment = Resco.Controls.AdvancedList.Alignment.TopCenter;
            //this.locationCell.ImageDefault = ((System.Drawing.Image)(resources.GetObject("buttonCell4.ImageDefault")));
            //this.locationCell.ImagePressed = ((System.Drawing.Image)(resources.GetObject("buttonCell4.ImagePressed")));
            this.locationCell.Location = new System.Drawing.Point(186, 57);
            this.locationCell.Name = "locationCell";
            this.locationCell.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(153)))), ((int)(((byte)(51)))));
            this.locationCell.PressedBorderColor = System.Drawing.Color.Black;
            this.locationCell.PressedForeColor = System.Drawing.Color.White;
            this.locationCell.Selectable = true;
            this.locationCell.Size = new System.Drawing.Size(50, 45);
            this.locationCell.Text = "Loc";
            this.locationCell.TextAlignment = Resco.Controls.AdvancedList.Alignment.BottomCenter;
            // 
            // cityCell
            // 
            this.cityCell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cityCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(237)))), ((int)(((byte)(221)))));
            this.cityCell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(237)))), ((int)(((byte)(221)))));
            this.cityCell.ImageAlignment = Resco.Controls.AdvancedList.Alignment.TopCenter;
            //this.cityCell.ImageDefault = ((System.Drawing.Image)(resources.GetObject("buttonCell4.ImageDefault")));
            //this.cityCell.ImagePressed = ((System.Drawing.Image)(resources.GetObject("buttonCell4.ImagePressed")));
            this.cityCell.Location = new System.Drawing.Point(186, 57);
            this.cityCell.Name = "cityCell";
            this.cityCell.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(153)))), ((int)(((byte)(51)))));
            this.cityCell.PressedBorderColor = System.Drawing.Color.Black;
            this.cityCell.PressedForeColor = System.Drawing.Color.White;
            this.cityCell.Selectable = true;
            this.cityCell.Size = new System.Drawing.Size(50, 45);
            this.cityCell.Text = "City";
            this.cityCell.TextAlignment = Resco.Controls.AdvancedList.Alignment.BottomCenter;

            this.contacts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.contacts.Controls.Add(this.vScrollBar1);
            this.contacts.DataRows.Clear();
            this.contacts.DataSource = null;
            this.contacts.Location = new System.Drawing.Point(0, 56);
            this.contacts.Name = "ContactList";
            this.contacts.ScrollBar = this.vScrollBar1;
            this.contacts.ScrollbarWidth = 24;
            this.contacts.SelectedTemplateIndex = 1;
            this.contacts.Size = new System.Drawing.Size(240, 202);
            this.contacts.TabIndex = 0;
            this.contacts.Templates.Add(this.RowTemplate);
            this.contacts.TouchScrolling = true;
            this.contacts.ButtonClick += new Resco.Controls.AdvancedList.ButtonEventHandler(this.contacts_ButtonClick);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.ArrowButtonsLayout = Resco.Controls.ScrollBar.ScrollBarArrowButtonsLayout.Hidden;
            this.vScrollBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Extension = this.letterBar;
            this.vScrollBar1.ExtensionVisible = true;
            this.vScrollBar1.ExtensionWidth = 20;
            this.vScrollBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.vScrollBar1.HighTrack = this.scrollVBarTrack;
            this.vScrollBar1.HighTrackDisabled = this.scrollVBarTrack;
            this.vScrollBar1.HighTrackHighlight = this.scrollVBarTrack;
            this.vScrollBar1.LargeChange = 1;
            this.vScrollBar1.Location = new System.Drawing.Point(216, 0);
            this.vScrollBar1.LowTrack = this.scrollVBarTrack;
            this.vScrollBar1.LowTrackDisabled = this.scrollVBarTrack;
            this.vScrollBar1.LowTrackHighlight = this.scrollVBarTrack;
            this.vScrollBar1.Maximum = 0;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(24, 202);
            this.vScrollBar1.TabIndex = 3;
            this.vScrollBar1.TabStop = false;
            this.vScrollBar1.Thumb = this.scrollBarVThumb;
            this.vScrollBar1.ThumbDisabled = this.scrollBarVThumb;
            this.vScrollBar1.ThumbHighlight = this.scrollBarVThumb;
            // 
            // letterBar
            // 
            this.letterBar.BorderStyle = Resco.Controls.ScrollBar.ScrollBarBorderStyle.None;
            this.letterBar.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Regular);
            this.letterBar.GradientColor = new Resco.Controls.ScrollBar.GradientColor(System.Drawing.Color.SkyBlue, System.Drawing.Color.Transparent, System.Drawing.Color.Transparent, System.Drawing.Color.DodgerBlue, 50, 50, Resco.Controls.ScrollBar.FillDirection.Vertical);
            this.letterBar.IndexToValue += new Resco.Controls.ScrollBar.ScrollBarExtensionBase.ValueIndexConversionHandler(this.letterBar_IndexToValue);
            this.letterBar.ValueToIndex += new Resco.Controls.ScrollBar.ScrollBarExtensionBase.ValueIndexConversionHandler(this.letterBar_ValueToIndex);
            // 
            // scrollVBarTrack
            // 
            this.scrollVBarTrack.BorderStyle = Resco.Controls.ScrollBar.ScrollBarBorderStyle.None;
            // 
            // scrollBarVThumb
            // 
            this.scrollBarVThumb.BorderStyle = Resco.Controls.ScrollBar.ScrollBarBorderStyle.None;
            this.scrollBarVThumb.GradientColor = new Resco.Controls.ScrollBar.GradientColor(System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.White, 30, 70, Resco.Controls.ScrollBar.FillDirection.Vertical);
            this.scrollBarVThumb.GripStyle = Resco.Controls.ScrollBar.ScrollBarThumb.ScrollBarThumbGripStyle.None;

            this.Controls.Add(this.contacts);
            this.contacts.ResumeLayout(false);
            this.toppanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        public void rightbutton_Click(object sender, EventArgs e)
        {
        }

        private int letterBar_IndexToValue(object sender, Resco.Controls.ScrollBar.ScrollBarExtensionBase.ValueIndexConversionEventArgs e)
        {
            // get datasource, return if not found
            //DataView dv = alCustomers.DataSource as DataView;
            //if (dv == null)
            //    return -1;

            //// get DataTable
            //DataTable dt = dv.Table as DataTable;
            //if (dt == null)
            //    return -1;

            //// find all rows which starts with the selected letter
            //DataRow[] rows = dt.Select("CompanyName LIKE '" + letterBar.IndexToLetter(e.Parameter) + "%'", "CompanyName");
            //if (rows.Length > 0)
            //{
            //    // get the position of the first row
            //    int position = dv.Find(Convert.ToString(rows[0]["CompanyName"]));

            //    // uncomment this line to select the row
            //    //alCustomers.ActiveRowIndex = position;

            //    // ensure visible the row and do not update scrollbar value
            //    alCustomers.EnsureVisible(position, true);
            //}

            // return -1 to not update ScrollBar.Value
            return -1;
        }

        private int letterBar_ValueToIndex(object sender, Resco.Controls.ScrollBar.ScrollBarExtensionBase.ValueIndexConversionEventArgs e)
        {
            // get the row at the ScrollBar value
            //Resco.Controls.AdvancedList.Row row = alCustomers.DataRows[alCustomers.TopRowIndex];
            //if (row == null)
            //    return -1;

            //string text = Convert.ToString(row["CompanyName"]);
            //if (text.Length > 0)
            //{
            //    return letterBar.LetterToIndex(text[0]);
            //}

            return -1;
        }

        private void contacts_ButtonClick(object sender, Resco.Controls.AdvancedList.CellEventArgs e)
        {
            switch (e.CellIndex)
            {
                case 0:
                    //MessageBox.Show("Handle phone call: " + e.CellData);
                    break;

                case 2:
                    //MessageBox.Show("Handle e-mail.");
                    break;

                case 3:
                    //MessageBox.Show("Handle web link.");
                    break;

                case 4:
                    //MessageBox.Show("Show more info.");
                    break;

                case 5:
                    //MessageBox.Show("Show more info.");
                    break;
            }
        }

        public override void ShowPanel(BelugaDb db)
        {
            dbcontacts = db.GetContactList();
            
            
            
        }

   //

        private Contact dbcontacts;
    }
}
