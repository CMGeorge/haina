using System.Collections.Generic;
namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;
    using TouchScrolling;

    public class OutlookShortcutBar : UserControl
    {
        private DesignTimeCallback _designTimeCallback;
        private Image m_backgroundImage;
        private Bitmap m_bmp;
        private SolidBrush m_brushGroupsBack;
        private SolidBrush m_brushTitleBack;
        private SolidBrush m_brushTitleFore;
        private OSBConversion m_Conversion;
        private Graphics m_graphics;
        private GroupsCollection m_groups;
        private bool m_KeyNavigation = true;
        private bool m_KeyNavigationGroupOnly;
        private bool m_KeyNavigationSimple = true;
        private bool m_mouseDown;
        private Pen m_penFrame;
        private bool m_showTitle;
        private static Hashtable sBrushes = new Hashtable();
        internal static Color TransparentColor = Color.Transparent;

        public event Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler SelectedGroupIndexChanged;

        public event Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler SelectedShortcutIndexChanged;

        public event Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler ShortcutEntered;

        static OutlookShortcutBar()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(OutlookShortcutBar), "");
            //}
        }

        public OutlookShortcutBar()
        {
            Graphics graphics = base.CreateGraphics();
            ShortcutsCollection.m_scaleFactor = new SizeF(graphics.DpiX / 96f, graphics.DpiY / 96f);
            ShortcutsCollection.m_Spacing = (int) (5f * ShortcutsCollection.m_scaleFactor.Width);
            ShortcutsCollection.ParentControl = this;
            ShortcutsCollection.GestureDetected += new Resco.Controls.OutlookControls.GestureDetectedHandler(this.ShortcutsCollection_GestureDetected);
            ShortcutsCollection.m_Arrow[0] = new Point((int) (3f * ShortcutsCollection.m_scaleFactor.Width), (int) (6f * ShortcutsCollection.m_scaleFactor.Height));
            ShortcutsCollection.m_Arrow[1] = new Point((int) ((13f * ShortcutsCollection.m_scaleFactor.Width) - 1f), (int) (6f * ShortcutsCollection.m_scaleFactor.Height));
            ShortcutsCollection.m_Arrow[2] = new Point((int) ((8f * ShortcutsCollection.m_scaleFactor.Width) - 1f), (int) (11f * ShortcutsCollection.m_scaleFactor.Height));
            GroupsCollection.m_scaleFactor = ShortcutsCollection.m_scaleFactor;
            this.Text = null;
            this.Font = new Font("Tahoma", 8f, FontStyle.Regular);
            this.BackColor = SystemColors.Window;
            this.ForeColor = SystemColors.ControlText;
            this.m_groups = new GroupsCollection(this);
            this.m_groups.InsertCompleteEvent += new InsertCompleteEventHandler(this.OnGroupsUpdateInsert);
            this.m_groups.RemoveEvent += new RemoveEventHandler(this.OnGroupsUpdateRemove);
            this.m_groups.SetCompleteEvent += new SetCompleteEventHandler(this.OnGroupsUpdateSet);
            this.m_groups.SelectedGroupIndexChanged += new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnSelectedGroupIndexChanged);
            this.m_groups.SelectedShortcutIndexChanged += new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnSelectedShortcutIndexChanged);
            this.m_groups.ShortcutEntered += new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnShortcutEntered);
            this.m_groups.Invalidating += new EventHandler(this.OnInvalidating);
        }

        protected void ClearBrushes()
        {
            sBrushes.Clear();
        }

        private void CreateGdiObjects()
        {
            if (this.m_penFrame == null)
            {
                this.m_penFrame = new Pen(SystemColors.WindowFrame);
            }
            if ((this.m_brushTitleFore == null) || (this.m_brushTitleFore.Color != this.ForeColor))
            {
                this.m_brushTitleFore = GetBrush(this.ForeColor);
            }
            if ((this.m_brushTitleBack == null) || (this.m_brushTitleBack.Color != this.BackColor))
            {
                this.m_brushTitleBack = GetBrush(this.BackColor);
            }
            if ((this.m_brushGroupsBack == null) || (this.m_brushGroupsBack.Color != this.GroupsBackColor))
            {
                this.m_brushGroupsBack = GetBrush(this.GroupsBackColor);
            }
        }

        private void CreateMemoryBitmap()
        {
            if (((this.m_bmp == null) || (this.m_bmp.Width != base.ClientSize.Width)) || (this.m_bmp.Height != base.ClientSize.Height))
            {
                if (this.m_bmp != null)
                {
                    this.m_bmp.Dispose();
                    this.m_bmp = null;
                }
                this.m_bmp = new Bitmap(base.ClientSize.Width, base.ClientSize.Height);
                this.m_graphics = Graphics.FromImage(this.m_bmp);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void DesignOnMouseDown(MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void DesignOnMouseUp(MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }

        private void DisableEvents()
        {
            ShortcutsCollection.GestureDetected -= new Resco.Controls.OutlookControls.GestureDetectedHandler(this.ShortcutsCollection_GestureDetected);
            if (this.m_groups != null)
            {
                this.m_groups.InsertCompleteEvent -= new InsertCompleteEventHandler(this.OnGroupsUpdateInsert);
                this.m_groups.RemoveEvent -= new RemoveEventHandler(this.OnGroupsUpdateRemove);
                this.m_groups.SetCompleteEvent -= new SetCompleteEventHandler(this.OnGroupsUpdateSet);
                this.m_groups.SelectedGroupIndexChanged -= new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnSelectedGroupIndexChanged);
                this.m_groups.SelectedShortcutIndexChanged -= new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnSelectedShortcutIndexChanged);
                this.m_groups.ShortcutEntered -= new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnShortcutEntered);
                this.m_groups.Invalidating -= new EventHandler(this.OnInvalidating);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisableEvents();
                if (this.m_backgroundImage != null)
                {
                    this.m_backgroundImage.Dispose();
                    this.m_backgroundImage = null;
                }
                if (this.m_bmp != null)
                {
                    this.m_bmp.Dispose();
                    this.m_bmp = null;
                }
                if (this.m_graphics != null)
                {
                    this.m_graphics.Dispose();
                    this.m_graphics = null;
                }
                if (this.m_penFrame != null)
                {
                    this.m_penFrame.Dispose();
                    this.m_penFrame = null;
                }
                if (this.m_groups != null)
                {
                    this.m_groups.Dispose();
                    this.m_groups = null;
                }
            }
            base.Dispose(disposing);
        }

        public static SolidBrush GetBrush(Color c)
        {
            SolidBrush brush = sBrushes[c] as SolidBrush;
            if (brush == null)
            {
                brush = new SolidBrush(c);
                sBrushes[c] = brush;
            }
            return brush;
        }

        public void LoadXml(string location)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(location);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                this.LoadXml(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                reader = null;
            }
        }

        public void LoadXml(XmlReader reader)
        {
            try
            {
                base.SuspendLayout();
                while (reader.Read())
                {
                    string str2;
                    if (((str2 = reader.Name) != null) && (str2 == "OutlookShortcutBar"))
                    {
                        this.ReadOutlookShortcutBar(reader);
                    }
                }
            }
            finally
            {
                base.ResumeLayout();
                base.Invalidate();
            }
        }

        public void LoadXml(string location, DesignTimeCallback dtc)
        {
            XmlTextReader reader = null;
            this._designTimeCallback = dtc;
            try
            {
                reader = new XmlTextReader(location);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                this.LoadXml(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                reader = null;
                this._designTimeCallback = null;
            }
        }

        private void MoveIndexDown(int num)
        {
            if (this.m_groups.SelectedIndex >= 0)
            {
                if (!this.m_groups[this.m_groups.SelectedIndex].Visible)
                {
                    if ((this.m_groups.SelectedIndex - 1) < 0)
                    {
                        this.m_groups.SelectedIndex = this.m_groups.Count - 1;
                    }
                    else
                    {
                        this.m_groups.SelectedIndex--;
                    }
                    this.MoveIndexDown(num + 1);
                }
                if ((this.m_groups[this.m_groups.SelectedIndex].SelectedIndex + num) < this.m_groups[this.m_groups.SelectedIndex].Shortcuts.Count)
                {
                    if (!this.m_groups[this.m_groups.SelectedIndex].Shortcuts[this.m_groups[this.m_groups.SelectedIndex].SelectedIndex + num].Enabled)
                    {
                        this.MoveIndexDown(num + 1);
                    }
                    else
                    {
                        Group group1 = this.m_groups[this.m_groups.SelectedIndex];
                        group1.SelectedIndex += num;
                    }
                }
                else if (!this.KeyNavigationGroupOnly)
                {
                    if (this.m_groups.SelectedIndex < (this.m_groups.Count - 1))
                    {
                        this.m_groups[this.m_groups.SelectedIndex].SelectedIndex = -1;
                        for (int i = this.m_groups.SelectedIndex + 1; i < this.m_groups.Count; i++)
                        {
                            if (this.m_groups[i].Visible)
                            {
                                this.m_groups.SelectedIndex = i;
                                return;
                            }
                        }
                    }
                    else
                    {
                        this.m_groups.SelectedIndex = -1;
                    }
                }
            }
            else if (!this.KeyNavigationGroupOnly)
            {
                this.m_groups.SelectedIndex = 0;
                for (int j = 0; j < this.m_groups.Count; j++)
                {
                    if (this.m_groups[j].Visible)
                    {
                        this.m_groups.SelectedIndex = j;
                        break;
                    }
                }
                this.m_groups[this.m_groups.SelectedIndex].SelectedIndex = -1;
            }
        }

        private void MoveIndexUp(int num)
        {
            bool flag;
            if (this.m_groups.SelectedIndex >= 0)
            {
                if (!this.m_groups[this.m_groups.SelectedIndex].Visible)
                {
                    if ((this.m_groups.SelectedIndex + 1) >= this.m_groups.Count)
                    {
                        this.m_groups.SelectedIndex = 0;
                    }
                    else
                    {
                        this.m_groups.SelectedIndex++;
                    }
                    this.MoveIndexUp(num + 1);
                }
                if ((this.m_groups[this.m_groups.SelectedIndex].SelectedIndex < 0) && (this.m_groups[this.m_groups.SelectedIndex].Shortcuts.Count > (num - 1)))
                {
                    if (!this.m_groups[this.m_groups.SelectedIndex].Shortcuts[this.m_groups[this.m_groups.SelectedIndex].Shortcuts.Count - num].Enabled)
                    {
                        this.MoveIndexUp(num + 1);
                        return;
                    }
                    this.m_groups[this.m_groups.SelectedIndex].SelectedIndex = this.m_groups[this.m_groups.SelectedIndex].Shortcuts.Count - num;
                    return;
                }
                if ((this.m_groups[this.m_groups.SelectedIndex].SelectedIndex - (num - 1)) > 0)
                {
                    if (!this.m_groups[this.m_groups.SelectedIndex].Shortcuts[this.m_groups[this.m_groups.SelectedIndex].SelectedIndex - num].Enabled)
                    {
                        this.MoveIndexUp(num + 1);
                        return;
                    }
                    Group group1 = this.m_groups[this.m_groups.SelectedIndex];
                    group1.SelectedIndex -= num;
                    return;
                }
                if (this.KeyNavigationGroupOnly)
                {
                    return;
                }
                if (this.m_groups.SelectedIndex < 0)
                {
                    this.m_groups.SelectedIndex = -1;
                    return;
                }
                this.m_groups[this.m_groups.SelectedIndex].SelectedIndex = -1;
                flag = false;
                for (int i = this.m_groups.SelectedIndex - 1; i >= 0; i--)
                {
                    if (this.m_groups[i].Visible)
                    {
                        this.m_groups.SelectedIndex = i;
                        flag = true;
                        break;
                    }
                }
            }
            else
            {
                if (!this.KeyNavigationGroupOnly)
                {
                    this.m_groups.SelectedIndex = this.m_groups.Count - 1;
                    for (int j = this.m_groups.Count - 1; j >= 0; j--)
                    {
                        if (this.m_groups[j].Visible)
                        {
                            this.m_groups.SelectedIndex = j;
                            break;
                        }
                    }
                    this.m_groups[this.m_groups.SelectedIndex].SelectedIndex = -1;
                }
                return;
            }
            if (!flag)
            {
                this.m_groups.SelectedIndex = -1;
            }
        }

        private void OnDetachImageList(object sender, EventArgs e)
        {
            this.GroupsImageList = null;
        }

        private void OnGroupsUpdateInsert(object sender, InsertEventArgs e)
        {
            base.Invalidate();
        }

        private void OnGroupsUpdateRemove(object sender, RemoveEventArgs e)
        {
            base.Invalidate();
        }

        private void OnGroupsUpdateSet(object sender, SetEventArgs e)
        {
            base.Invalidate();
        }

        private void OnInvalidating(object sender, EventArgs e)
        {
            this.Refresh();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.m_graphics == null)
            {
                base.OnKeyDown(e);
            }
            else
            {
                if (this.KeyNavigation)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Left:
                            this.MoveIndexUp(1);
                            break;

                        case Keys.Up:
                            if (!this.KeyNavigationSimple)
                            {
                                if ((this.SelectedIndex > -1) && (this.Groups[this.SelectedIndex].SelectedIndex > -1))
                                {
                                    Group group = this.Groups[this.SelectedIndex];
                                    int num = base.Width / ((group.ShortcutsViewStyle == ViewStyle.List) ? group.ShortcutsWidth : base.Width);
                                    this.MoveIndexUp(num);
                                }
                                else
                                {
                                    this.MoveIndexUp(1);
                                }
                                break;
                            }
                            this.MoveIndexUp(1);
                            break;

                        case Keys.Right:
                            this.MoveIndexDown(1);
                            break;

                        case Keys.Down:
                            if (!this.KeyNavigationSimple)
                            {
                                if ((this.SelectedIndex > -1) && (this.Groups[this.SelectedIndex].SelectedIndex > -1))
                                {
                                    Group group2 = this.Groups[this.SelectedIndex];
                                    int num2 = base.Width / ((group2.ShortcutsViewStyle == ViewStyle.List) ? group2.ShortcutsWidth : base.Width);
                                    this.MoveIndexDown(num2);
                                }
                                else
                                {
                                    this.MoveIndexDown(1);
                                }
                                break;
                            }
                            this.MoveIndexDown(1);
                            break;
                    }
                }
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (((e.KeyCode == Keys.Return) && (this.m_groups.SelectedIndex >= 0)) && (this.m_groups[this.SelectedIndex].SelectedIndex >= 0))
            {
                this.OnShortcutEntered(this, new SelectedIndexChangedEventArgs(this.m_groups.SelectedIndex, this.m_groups[this.SelectedIndex].SelectedIndex, SelectedIndexChangedEventArgs.SelectionType.ByKeyboard));
            }
            base.OnKeyUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.m_graphics == null)
            {
                base.OnMouseDown(e);
            }
            else
            {
                base.Focus();
                Size size = new Size(0, 0);
                if (this.m_showTitle)
                {
                    size = this.m_graphics.MeasureString(this.Text, this.Font).ToSize();
                    size.Height += 9;
                }
                int height = size.Height;
                int y = e.Y - height;
                if (y > -1)
                {
                    Rectangle rect = new Rectangle(0, height, base.ClientSize.Width, base.ClientSize.Height - height);
                    this.m_groups.MouseDown(new Point(e.X, y), rect);
                }
                this.m_mouseDown = true;
                base.OnMouseDown(e);
                base.Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.m_graphics == null)
            {
                base.OnMouseMove(e);
            }
            else
            {
                if (this.m_mouseDown)
                {
                    Size size = new Size(0, 0);
                    if (this.m_showTitle)
                    {
                        size = this.m_graphics.MeasureString(this.Text, this.Font).ToSize();
                        size.Height += 9;
                    }
                    int height = size.Height;
                    int y = e.Y - height;
                    if (y > -1)
                    {
                        Rectangle rect = new Rectangle(0, height, base.ClientSize.Width, base.ClientSize.Height - height);
                        this.m_groups.MouseMove(new Point(e.X, y), rect);
                    }
                }
                base.OnMouseMove(e);
                if (this.m_mouseDown)
                {
                    base.Invalidate();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.m_graphics == null)
            {
                base.OnMouseUp(e);
            }
            else
            {
                Size size = new Size(0, 0);
                if (this.m_showTitle)
                {
                    size = this.m_graphics.MeasureString(this.Text, this.Font).ToSize();
                    size.Height += 9;
                }
                int height = size.Height;
                int y = e.Y - height;
                if (y > -1)
                {
                    Rectangle rect = new Rectangle(0, height, base.ClientSize.Width, base.ClientSize.Height - height);
                    this.m_groups.MouseUp(new Point(e.X, y), rect);
                }
                this.m_mouseDown = false;
                base.OnMouseUp(e);
                base.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.m_groups != null)
            {
                this.CreateGdiObjects();
                this.CreateMemoryBitmap();
                Region region = new Region(e.ClipRectangle);
                this.m_graphics.Clip = region;
                Size size = new Size(0, 0);
                if (this.m_showTitle)
                {
                    size = this.m_graphics.MeasureString(this.Text, this.Font).ToSize();
                    try
                    {
                        if (this.m_brushTitleBack != null)
                        {
                            this.m_graphics.FillRectangle(this.m_brushTitleBack, 0, 0, base.ClientSize.Width, size.Height + 8);
                        }
                    }
                    catch (ArgumentException)
                    {
                    }
                    this.m_graphics.DrawString(this.Text, this.Font, this.m_brushTitleFore, (float) 4f, (float) 4f);
                    this.m_graphics.DrawLine(this.m_penFrame, 0, size.Height + 8, base.ClientSize.Width, size.Height + 8);
                    size.Height += 9;
                }
                int height = size.Height;
                int width = base.ClientSize.Width;
                int num3 = base.ClientSize.Height - height;
                if (num3 < 0)
                {
                    num3 = base.ClientSize.Height;
                }
                Rectangle rect = new Rectangle(0, height, width, num3);
                if (this.m_groups.Show)
                {
                    Rectangle rectangle2 = rect;
                    try
                    {
                        if (this.m_brushGroupsBack != null)
                        {
                            this.m_graphics.FillRectangle(this.m_brushGroupsBack, rectangle2);
                        }
                    }
                    catch (ArgumentException)
                    {
                    }
                    rect = this.m_groups.Draw(this.m_graphics, rectangle2, this.m_penFrame);
                }
                else
                {
                    try
                    {
                        if (this.m_brushTitleBack != null)
                        {
                            this.m_graphics.FillRectangle(this.m_brushTitleBack, rect);
                        }
                    }
                    catch (ArgumentException)
                    {
                    }
                }
                if (!this.m_groups.Show && (this.m_groups.SelectedIndex < 0))
                {
                    this.m_groups.m_bDoInvalidate = false;
                    if (this.m_groups.Count > 0)
                    {
                        this.m_groups.SelectedIndex = 0;
                    }
                    else
                    {
                        this.m_groups.SelectedIndex = -1;
                    }
                }
                if (((this.m_groups.m_selectedIndex >= 0) && (this.m_groups.Count > 0)) && (this.m_groups.m_selectedIndex < this.m_groups.Count))
                {
                    this.m_groups[this.m_groups.m_selectedIndex].Draw(this.m_graphics, rect, this.m_penFrame, this.m_backgroundImage);
                }
                e.Graphics.DrawImage(this.m_bmp, 0, 0);
                region.Dispose();
                region = null;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.Invalidate();
            base.OnResize(e);
        }

        protected void OnSelectedGroupIndexChanged(object sender, SelectedIndexChangedEventArgs e)
        {
            if (this.SelectedGroupIndexChanged != null)
            {
                this.SelectedGroupIndexChanged(sender, e);
            }
        }

        protected void OnSelectedShortcutIndexChanged(object sender, SelectedIndexChangedEventArgs e)
        {
            if (this.SelectedShortcutIndexChanged != null)
            {
                this.SelectedShortcutIndexChanged(sender, e);
            }
        }

        protected void OnShortcutEntered(object sender, SelectedIndexChangedEventArgs e)
        {
            if (this.ShortcutEntered != null)
            {
                this.ShortcutEntered(sender, e);
            }
        }

        private Group ReadGroup(XmlReader reader)
        {
            Group o = null;
            try
            {
                string str = reader["Name"];
                o = new Group();
                if (this._designTimeCallback != null)
                {
                    this._designTimeCallback(o, null);
                }
                o.Name = str;
                if (!reader.IsEmptyElement)
                {
                    goto Label_00C5;
                }
                return o;
            Label_0042:
                try
                {
                    string str2;
                    if (((str2 = reader.Name) == null) || (str2 == ""))
                    {
                        goto Label_00C5;
                    }
                    if (!(str2 == "Group"))
                    {
                        if (str2 == "Shortcut")
                        {
                            goto Label_0089;
                        }
                        if (str2 == "Property")
                        {
                            goto Label_009E;
                        }
                        goto Label_00C5;
                    }
                    return o;
                Label_0089:
                    o.Shortcuts.Add(this.ReadShortcut(reader));
                    goto Label_00C5;
                Label_009E:
                    this.m_Conversion.SetProperty(o, reader["Name"], reader["Value"]);
                }
                catch
                {
                }
            Label_00C5:
                if (reader.Read())
                {
                    goto Label_0042;
                }
            }
            catch
            {
                try
                {
                    if (((reader.Name == "Group") && reader.IsStartElement()) && !reader.IsEmptyElement)
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "Group")
                            {
                                goto Label_0118;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        Label_0118:
            if (o != null)
            {
                return o;
            }
            return new Group();
        }

        private void ReadImageList(XmlReader reader)
        {
            try
            {
                string sName = reader["Name"];
                string sRect = reader["ImageSize"];
                ImageList imageList = this.m_Conversion.GetImageList(sName);
                if (imageList != null)
                {
                    try
                    {
                        Size size = OSBConversion.SizeFromString(sRect);
                        imageList.ImageSize = size;
                    }
                    catch
                    {
                        imageList.ImageSize = new Size(0x10, 0x10);
                    }
                    if (imageList.Images.Count > 0)
                    {
                        PropertyDescriptor descriptor = TypeDescriptor.GetProperties(imageList)["Images"];
                        ((IList) descriptor.GetValue(imageList)).Clear();
                    }
                    if (!reader.IsEmptyElement)
                    {
                        goto Label_00F6;
                    }
                }
                return;
            Label_008F:
                if (reader.Name == "Image")
                {
                    Bitmap o = OSBConversion.ImageFromString(reader["Data"]);
                    if (this._designTimeCallback != null)
                    {
                        this._designTimeCallback(o, imageList);
                    }
                    else
                    {
                        imageList.Images.Add(o);
                    }
                }
                if ((reader.NodeType == XmlNodeType.EndElement) && (reader.Name == "ImageList"))
                {
                    return;
                }
            Label_00F6:
                if (reader.Read())
                {
                    goto Label_008F;
                }
            }
            catch (Exception)
            {
            }
        }

        private void ReadOutlookShortcutBar(XmlReader reader)
        {
            this.m_Conversion = new OSBConversion(this.Site, this._designTimeCallback);
            try
            {
                if (reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                    {
                        try
                        {
                            this.m_Conversion.SetProperty(this, reader.Name, reader.Value);
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    reader.MoveToElement();
                }
                this.Groups.Clear();
                if (!reader.IsEmptyElement)
                {
                    goto Label_0155;
                }
                return;
            Label_0068:
                try
                {
                    string name = reader.Name;
                    if (name != null)
                    {
                        int num;
                        if (BigHas.methodxxx.TryGetValue(name,out num)) //if (<PrivateImplementationDetails>{9FE615AF-0BCB-474D-9B5C-7186A6AC4242}.$$method0x60006b5-1.TryGetValue(name, ref num))
                        {
                            switch (num)
                            {
                                case 0:
                                    goto Label_0155;

                                case 1:
                                    this.Groups.Add(this.ReadGroup(reader));
                                    goto Label_0155;

                                case 2:
                                    return;

                                case 3:
                                    this.m_Conversion.SetProperty(this, reader["Name"], reader["Value"]);
                                    goto Label_0155;

                                case 4:
                                    this.ReadImageList(reader);
                                    goto Label_0155;
                            }
                        }
                        this.m_Conversion.SetProperty(this, reader.Name, reader.ReadString());
                    }
                }
                catch
                {
                }
            Label_0155:
                if (reader.Read())
                {
                    goto Label_0068;
                }
                base.Update();
            }
            catch
            {
            }
            finally
            {
                this.m_Conversion = null;
            }
        }

        private Resco.Controls.OutlookControls.Shortcut ReadShortcut(XmlReader reader)
        {
            Resco.Controls.OutlookControls.Shortcut o = null;
            try
            {
                string str = reader["Name"];
                o = new Resco.Controls.OutlookControls.Shortcut();
                if (this._designTimeCallback != null)
                {
                    this._designTimeCallback(o, null);
                }
                o.Name = str;
                if (!reader.IsEmptyElement)
                {
                    goto Label_00A0;
                }
                return o;
            Label_003F:
                try
                {
                    string str2;
                    if (((str2 = reader.Name) == null) || (str2 == ""))
                    {
                        goto Label_00A0;
                    }
                    if (!(str2 == "Shortcut"))
                    {
                        if (str2 == "Property")
                        {
                            goto Label_0079;
                        }
                        goto Label_00A0;
                    }
                    return o;
                Label_0079:
                    this.m_Conversion.SetProperty(o, reader["Name"], reader["Value"]);
                }
                catch
                {
                }
            Label_00A0:
                if (reader.Read())
                {
                    goto Label_003F;
                }
            }
            catch
            {
                try
                {
                    if (((reader.Name == "Shortcut") && reader.IsStartElement()) && !reader.IsEmptyElement)
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "Shortcut")
                            {
                                goto Label_00F0;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        Label_00F0:
            if (o != null)
            {
                return o;
            }
            return new Resco.Controls.OutlookControls.Shortcut();
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.GroupHeight = (int) (this.GroupHeight * factor.Height);
            ShortcutsCollection.UserScaleFactor = factor;
            GroupsCollection.m_userScaleFactor = factor;
            foreach (Group group in this.m_groups)
            {
                group.ShortcutsWidth = (int) (group.ShortcutsWidth * factor.Width);
            }
            base.ScaleControl(factor, specified);
        }

        private void ShortcutsCollection_GestureDetected(object sender, Resco.Controls.OutlookControls.GestureEventArgs e)
        {
            switch (e.Gesture)
            {
                case OsbTouchTool.GestureType.Left:
                    if ((this.SelectedIndex - 1) < 0)
                    {
                        break;
                    }
                    this.SelectedIndex--;
                    return;

                case OsbTouchTool.GestureType.Right:
                    if ((this.SelectedIndex + 1) < this.Groups.Count)
                    {
                        this.SelectedIndex++;
                    }
                    break;

                default:
                    return;
            }
        }

        protected virtual bool ShouldSerializeGroupStyle()
        {
            if (this.m_groups.GroupStyle == Resco.Controls.OutlookControls.GroupStyle.Buttons)
            {
                return false;
            }
            return true;
        }

        public Image BackgroundImage
        {
            get
            {
                return this.m_backgroundImage;
            }
            set
            {
                if (this.m_backgroundImage != value)
                {
                    this.m_backgroundImage = value;
                    base.Invalidate();
                }
            }
        }

        public int GroupHeight
        {
            get
            {
                return this.m_groups.Height;
            }
            set
            {
                if (this.m_groups.Height != value)
                {
                    this.m_groups.Height = value;
                    base.Invalidate();
                }
            }
        }

        public GroupsCollection Groups
        {
            get
            {
                return this.m_groups;
            }
        }

        public Color GroupsBackColor
        {
            get
            {
                return this.m_groups.BackColor;
            }
            set
            {
                this.m_groups.BackColor = value;
            }
        }

        public bool GroupsBorder
        {
            get
            {
                return this.m_groups.Border;
            }
            set
            {
                this.m_groups.Border = value;
            }
        }

        public Font GroupsFont
        {
            get
            {
                return this.m_groups.Font;
            }
            set
            {
                this.m_groups.Font = value;
            }
        }

        public Color GroupsForeColor
        {
            get
            {
                return this.m_groups.ForeColor;
            }
            set
            {
                this.m_groups.ForeColor = value;
            }
        }

        public ImageList GroupsImageList
        {
            get
            {
                return this.m_groups.ImageList;
            }
            set
            {
                if (value != this.m_groups.ImageList)
                {
                    EventHandler handler = new EventHandler(this.OnDetachImageList);
                    if (this.m_groups.ImageList != null)
                    {
                        this.m_groups.ImageList.Disposed -= handler;
                    }
                    this.m_groups.ImageList = value;
                    if (value != null)
                    {
                        value.Disposed += handler;
                    }
                }
            }
        }

        public bool GroupsImageOnly
        {
            get
            {
                return this.m_groups.ImageOnly;
            }
            set
            {
                this.m_groups.ImageOnly = value;
            }
        }

        public Resco.Controls.OutlookControls.GroupStyle GroupStyle
        {
            get
            {
                return this.m_groups.GroupStyle;
            }
            set
            {
                this.m_groups.GroupStyle = value;
            }
        }

        public bool KeyNavigation
        {
            get
            {
                return this.m_KeyNavigation;
            }
            set
            {
                if (this.m_KeyNavigation != value)
                {
                    this.m_KeyNavigation = value;
                }
            }
        }

        public bool KeyNavigationGroupOnly
        {
            get
            {
                return this.m_KeyNavigationGroupOnly;
            }
            set
            {
                if (this.m_KeyNavigationGroupOnly != value)
                {
                    this.m_KeyNavigationGroupOnly = value;
                }
            }
        }

        public bool KeyNavigationSimple
        {
            get
            {
                return this.m_KeyNavigationSimple;
            }
            set
            {
                if (this.m_KeyNavigationSimple != value)
                {
                    this.m_KeyNavigationSimple = value;
                }
            }
        }

        public int ScrollBarHeight
        {
            get
            {
                return this.m_groups.ScrollBarHeight;
            }
            set
            {
                this.m_groups.ScrollBarHeight = value;
            }
        }

        public int ScrollBarWidth
        {
            get
            {
                return this.m_groups.ScrollBarWidth;
            }
            set
            {
                this.m_groups.ScrollBarWidth = value;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.m_groups.SelectedIndex;
            }
            set
            {
                this.m_groups.SelectedIndex = value;
                base.Invalidate();
            }
        }

        public Color SelGroupsBackColor
        {
            get
            {
                return this.m_groups.SelBackColor;
            }
            set
            {
                this.m_groups.SelBackColor = value;
            }
        }

        public Color SelGroupsForeColor
        {
            get
            {
                return this.m_groups.SelForeColor;
            }
            set
            {
                this.m_groups.SelForeColor = value;
            }
        }

        public bool ShowGroups
        {
            get
            {
                return this.m_groups.Show;
            }
            set
            {
                this.m_groups.Show = value;
            }
        }

        public bool ShowTitle
        {
            get
            {
                return this.m_showTitle;
            }
            set
            {
                if (this.m_showTitle != value)
                {
                    this.m_showTitle = value;
                    base.Invalidate();
                }
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                base.Invalidate();
            }
        }

        public Color TitleBackColor
        {
            get
            {
                return this.BackColor;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = SystemColors.Window;
                }
                this.BackColor = value;
            }
        }

        public Font TitleFont
        {
            get
            {
                return this.Font;
            }
            set
            {
                this.Font = value;
            }
        }

        public Color TitleForeColor
        {
            get
            {
                return this.ForeColor;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = SystemColors.ControlText;
                }
                this.ForeColor = value;
            }
        }

        public bool TouchScrolling
        {
            get
            {
                return OsbTouchTool.EnableTouchScrolling;
            }
            set
            {
                OsbTouchTool.EnableTouchScrolling = value;
            }
        }

        public int TouchSensitivity
        {
            get
            {
                return OsbTouchTool.TouchSensitivity;
            }
            set
            {
                OsbTouchTool.TouchSensitivity = value;
            }
        }

        public bool UseVistaStyle
        {
            get
            {
                return this.m_groups.UseVistaStyle;
            }
            set
            {
                this.m_groups.UseVistaStyle = value;
            }
        }

        public delegate void DesignTimeCallback(object o, object o2);
    }
}

internal class BigHas
    {
        // Fields
        internal static Dictionary<string, int> methodxxx;
    }