namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GroupsCollection : CollectionBase
    {
        private bool m_bAutoScaleEnabled = true;
        internal bool m_bDoInvalidate = true;
        private bool m_border = true;
        private SolidBrush m_brushGroupsFore;
        private SolidBrush m_brushSelGroupsBack;
        private SolidBrush m_brushSelGroupsFore;
        private System.Drawing.Font m_Font = new System.Drawing.Font("Tahoma", 8f, FontStyle.Regular);
        private Color m_groupsBackColor = SystemColors.Control;
        private Color m_groupsForeColor = SystemColors.WindowText;
        private Resco.Controls.OutlookControls.GroupStyle m_groupStyle;
        private int m_Height = 0x10;
        private System.Windows.Forms.ImageList m_ImageList;
        private bool m_ImageOnly;
        private string m_Name = "Groups";
        private Control m_Parent;
        private int m_pushedIndex = -1;
        internal static SizeF m_scaleFactor = new SizeF(1f, 1f);
        internal int m_selectedIndex = -1;
        private Color m_selGroupsBackColor = Color.Empty;
        private Color m_selGroupsForeColor = SystemColors.HighlightText;
        private bool m_showGroups = true;
        internal static SizeF m_userScaleFactor = new SizeF(1f, 1f);
        private bool m_UseVistaStyle;

        public event InsertCompleteEventHandler InsertCompleteEvent;

        public event InsertEventHandler InsertEvent;

        public event EventHandler Invalidating;

        public event RemoveCompleteEventHandler RemoveCompleteEvent;

        public event RemoveEventHandler RemoveEvent;

        public event Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler SelectedGroupIndexChanged;

        public event Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler SelectedShortcutIndexChanged;

        public event SetCompleteEventHandler SetCompleteEvent;

        public event SetEventHandler SetEvent;

        public event Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler ShortcutEntered;

        internal GroupsCollection(Control _parent)
        {
            this.m_Parent = _parent;
        }

        public int Add(Group value)
        {
            return base.List.Add(value);
        }

        public int CalculateHeight()
        {
            switch (this.m_groupStyle)
            {
                case Resco.Controls.OutlookControls.GroupStyle.Buttons:
                    if (this.m_showGroups)
                    {
                        int num = 0;
                        for (int i = 0; i < base.Count; i++)
                        {
                            if (this[i].Visible)
                            {
                                num += this.m_Height;
                            }
                        }
                        return num;
                    }
                    return 0;

                case Resco.Controls.OutlookControls.GroupStyle.Links:
                {
                    int height = this.Height;
                    int width = 0;
                    foreach (Group group in base.List)
                    {
                        if (this.m_ImageOnly)
                        {
                            if (this.m_ImageList != null)
                            {
                                width += this.m_ImageList.ImageSize.Width;
                                if (width > this.m_Parent.Width)
                                {
                                    height += this.Height;
                                    width = this.m_ImageList.ImageSize.Width;
                                }
                            }
                        }
                        else
                        {
                            string text = group.Text + ((base.List.IndexOf(group) == (base.List.Count - 1)) ? "" : " ");
                            Size size = this.m_Parent.CreateGraphics().MeasureString(text, this.Font).ToSize();
                            width = ((((width + 2) + 5) + 4) + size.Width) + ((((this.m_ImageList != null) && (group.ImageIndex >= 0)) && (group.ImageIndex < this.m_ImageList.Images.Count)) ? this.m_ImageList.ImageSize.Width : 0);
                            if (width > this.m_Parent.Width)
                            {
                                height += this.Height;
                                width = 0;
                            }
                        }
                    }
                    return height;
                }
            }
            return this.m_Height;
        }

        private bool CheckLinkPressed(Point p, Rectangle rect)
        {
            Graphics graphics = this.m_Parent.CreateGraphics();
            int x = (int) (5f * m_scaleFactor.Width);
            int y = (int) (1f * m_scaleFactor.Height);
            if (this.m_ImageOnly)
            {
                x = 0;
                y = 0;
            }
            int num3 = this.CalculateHeight();
            int num4 = p.Y - (rect.Height - (this.m_showGroups ? num3 : 0));
            if (num4 > -1)
            {
                int num5 = 0;
                int width = 0;
                foreach (Group group in base.List)
                {
                    if (!group.Visible)
                    {
                        continue;
                    }
                    if (this.m_ImageOnly)
                    {
                        if (this.m_ImageList != null)
                        {
                            width = (int) (this.m_ImageList.ImageSize.Width * m_userScaleFactor.Width);
                        }
                    }
                    else
                    {
                        string text = group.Text + ((base.List.IndexOf(group) == (base.List.Count - 1)) ? "" : " ");
                        Size size = graphics.MeasureString(text, this.Font).ToSize();
                        width = (int) ((((5f * m_scaleFactor.Width) + (2f * m_scaleFactor.Width)) + size.Width) + ((((this.m_ImageList != null) && (group.ImageIndex >= 0)) && (group.ImageIndex < this.m_ImageList.Images.Count)) ? (this.m_ImageList.ImageSize.Width * m_userScaleFactor.Width) : 0f));
                    }
                    num5 = x + width;
                    if (num5 > this.m_Parent.Width)
                    {
                        y += this.Height;
                        x = 0;
                        num5 = 0;
                    }
                    Rectangle rectangle = new Rectangle(x, y, width, this.Height);
                    if (rectangle.Contains(p.X, num4))
                    {
                        this.m_pushedIndex = base.List.IndexOf(group);
                        this.m_bDoInvalidate = false;
                        this.SelectedIndex = (this.m_pushedIndex != this.m_selectedIndex) ? ((this.m_pushedIndex == -1) ? this.m_selectedIndex : this.m_pushedIndex) : this.m_selectedIndex;
                        this.m_pushedIndex = -1;
                    }
                    x += width;
                }
            }
            else if (this.SelectedIndex > -1)
            {
                return false;
            }
            return true;
        }

        public bool Contains(Group value)
        {
            return base.List.Contains(value);
        }

        private void CreateGdiObjects()
        {
            if ((this.m_brushGroupsFore == null) || (this.m_brushGroupsFore.Color != this.ForeColor))
            {
                this.m_brushGroupsFore = OutlookShortcutBar.GetBrush(this.ForeColor);
            }
            if ((this.m_brushSelGroupsBack == null) || (this.m_brushSelGroupsBack.Color != this.SelBackColor))
            {
                this.m_brushSelGroupsBack = OutlookShortcutBar.GetBrush(this.SelBackColor);
            }
            if ((this.m_brushSelGroupsFore == null) || (this.m_brushSelGroupsFore.Color != this.SelForeColor))
            {
                this.m_brushSelGroupsFore = OutlookShortcutBar.GetBrush(this.SelForeColor);
            }
        }

        internal void Dispose()
        {
            this.m_Parent = null;
            if (this.m_Font != null)
            {
                this.m_Font.Dispose();
                this.m_Font = null;
            }
            if (this.m_ImageList != null)
            {
                this.m_ImageList.Dispose();
                this.m_ImageList = null;
            }
            for (int i = 0; i < base.Count; i++)
            {
                Group group = this[i];
                if (group != null)
                {
                    group.SelectedShortcutIndexChanged -= new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.GroupSelectedShortcutIndexChanged);
                    group.Invalidating -= new EventHandler(this.OnInvalidating);
                    group.ShortcutEntered -= new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnGroupsShortcutEntered);
                    group.Parent = null;
                    if (group != null)
                    {
                        group.Dispose();
                        group = null;
                    }
                }
            }
            base.Clear();
        }

        internal Rectangle Draw(Graphics gr, Rectangle rect, Pen frame)
        {
            this.CreateGdiObjects();
            int x = rect.X;
            int y = rect.Y;
            Rectangle rectangle2 = rect;
            if (this.m_groupStyle == Resco.Controls.OutlookControls.GroupStyle.Links)
            {
                y += rect.Height - (this.CalculateHeight() + 1);
                rectangle2.Height = y - rect.Y;
                if (this.m_border)
                {
                    gr.DrawLine(frame, rect.X, y, rect.X + rect.Width, y);
                }
                y++;
            }
            int num3 = 0;
            foreach (Group group in base.List)
            {
                Rectangle rectangle;
                string str;
                if (!group.Visible)
                {
                    continue;
                }
                switch (this.m_groupStyle)
                {
                    case Resco.Controls.OutlookControls.GroupStyle.Buttons:
                        x = rect.X;
                        if (!this.m_ImageOnly)
                        {
                            break;
                        }
                        if (((this.m_ImageList != null) && (group.ImageIndex >= 0)) && (group.ImageIndex < this.m_ImageList.Images.Count))
                        {
                            using (Image image = this.m_ImageList.Images[group.ImageIndex])
                            {
                                Rectangle dest = new Rectangle(x, y, rect.Width, this.Height);
                                Rectangle src = new Rectangle(0, 0, image.Width, image.Height);
                                this.DrawImage(gr, image, dest, src, group.m_ia, group.AutoTransparent);
                            }
                            x += rect.Width;
                        }
                        goto Label_0440;

                    case Resco.Controls.OutlookControls.GroupStyle.Links:
                        if (!this.m_ImageOnly)
                        {
                            goto Label_05F9;
                        }
                        if (this.m_ImageList != null)
                        {
                            if ((group.ImageIndex >= 0) && (group.ImageIndex < this.m_ImageList.Images.Count))
                            {
                                using (Image image3 = this.m_ImageList.Images[group.ImageIndex])
                                {
                                    Rectangle rectangle7 = new Rectangle(x, y, (int) (image3.Width * m_userScaleFactor.Width), this.Height);
                                    Rectangle rectangle8 = new Rectangle(0, 0, image3.Width, image3.Height);
                                    this.DrawImage(gr, image3, rectangle7, rectangle8, group.m_ia, group.AutoTransparent);
                                }
                            }
                            x += (int) (this.m_ImageList.ImageSize.Width * m_userScaleFactor.Width);
                            int num6 = x + ((int) (this.m_ImageList.ImageSize.Width * m_userScaleFactor.Width));
                            if (num6 > rect.Width)
                            {
                                y += this.Height;
                                x = rect.X;
                            }
                        }
                        goto Label_08B7;

                    default:
                        goto Label_08B7;
                }
                if (this.IndexOf(group) == this.m_selectedIndex)
                {
                    rectangle = new Rectangle(x, y, rect.Width, this.Height);
                    if (!this.m_UseVistaStyle)
                    {
                        gr.FillRectangle(this.m_brushSelGroupsBack, rectangle);
                    }
                    else
                    {
                        GradientFill.DrawVistaGradient(gr, this.SelBackColor, rectangle, FillDirection.Vertical);
                    }
                }
                else if (this.m_UseVistaStyle)
                {
                    rectangle = new Rectangle(x, y, rect.Width, this.Height);
                    GradientFill.DrawVistaGradient(gr, this.BackColor, rectangle, FillDirection.Vertical);
                }
                if (((this.m_ImageList != null) && (group.ImageIndex >= 0)) && (group.ImageIndex < this.m_ImageList.Images.Count))
                {
                    using (Image image2 = this.m_ImageList.Images[group.ImageIndex])
                    {
                        Rectangle rectangle5;
                        Rectangle rectangle6;
                        if (this.m_bAutoScaleEnabled)
                        {
                            rectangle5 = new Rectangle(x + ((int) (5f * m_scaleFactor.Width)), y, (int) (image2.Width * m_userScaleFactor.Width), this.Height);
                            rectangle6 = new Rectangle(0, (int) ((((image2.Height * m_userScaleFactor.Height) - this.Height) / 2f) / m_userScaleFactor.Height), image2.Width, (int) (((float) this.Height) / m_userScaleFactor.Height));
                        }
                        else
                        {
                            int num4 = y + ((this.Height - image2.Height) / 2);
                            rectangle5 = new Rectangle(x + ((int) (5f * m_scaleFactor.Width)), num4, image2.Width, image2.Height);
                            rectangle6 = new Rectangle(0, 0, image2.Width, image2.Height);
                        }
                        this.DrawImage(gr, image2, rectangle5, rectangle6, group.m_ia, group.AutoTransparent);
                        x += 2 + rectangle5.Width;
                    }
                }
                Size size = gr.MeasureString(group.Text, this.Font).ToSize();
                if (this.IndexOf(group) == this.m_selectedIndex)
                {
                    rectangle = new Rectangle(x + ((int) (7f * m_scaleFactor.Width)), y + ((this.Height - size.Height) / 2), rect.Width, this.Height);
                    gr.DrawString(group.Text, this.Font, this.m_brushSelGroupsFore, rectangle);
                }
                else
                {
                    rectangle = new Rectangle(x + ((int) (7f * m_scaleFactor.Width)), y + ((this.Height - size.Height) / 2), rect.Width, this.Height);
                    gr.DrawString(group.Text, this.Font, this.m_brushGroupsFore, rectangle);
                }
            Label_0440:
                if (this.m_border)
                {
                    gr.DrawRectangle(frame, rect.X, y, rect.Width - 1, this.Height);
                }
                y += this.Height;
                if (this.IndexOf(group) == this.m_selectedIndex)
                {
                    int num5 = ((rect.Height - y) + rect.Top) - (((this.VisibleCount() - num3) - 1) * this.Height);
                    rectangle2.Y = y;
                    rectangle2.Height = (num5 < 0) ? 0 : num5;
                    y += (num5 < 0) ? 0 : num5;
                }
                goto Label_08B7;
            Label_05F9:
                str = group.Text + ((base.List.IndexOf(group) == (base.List.Count - 1)) ? "" : " ");
                size = gr.MeasureString(str, this.Font).ToSize();
                int num7 = (int) ((((5f * m_scaleFactor.Width) + (2f * m_scaleFactor.Width)) + size.Width) + ((((this.m_ImageList != null) && (group.ImageIndex >= 0)) && (group.ImageIndex < this.m_ImageList.Images.Count)) ? (this.m_ImageList.ImageSize.Width * m_userScaleFactor.Width) : 0f));
                int num8 = x + num7;
                if (num8 > rect.Width)
                {
                    y += this.Height;
                    x = rect.X;
                }
                if (((this.m_ImageList != null) && (group.ImageIndex >= 0)) && (group.ImageIndex < this.m_ImageList.Images.Count))
                {
                    using (Image image4 = this.m_ImageList.Images[group.ImageIndex])
                    {
                        Rectangle rectangle9 = new Rectangle(x + ((int) (5f * m_scaleFactor.Width)), y, (int) (image4.Width * m_userScaleFactor.Width), (int) (image4.Height * m_userScaleFactor.Height));
                        Rectangle rectangle10 = new Rectangle(0, 0, image4.Width, image4.Height);
                        this.DrawImage(gr, image4, rectangle9, rectangle10, group.m_ia, group.AutoTransparent);
                        x += (int) (image4.Width * m_userScaleFactor.Width);
                    }
                }
                if (this.IndexOf(group) == this.m_selectedIndex)
                {
                    rectangle = new Rectangle((int) ((x + (2f * m_scaleFactor.Width)) + (5f * m_scaleFactor.Width)), y, rect.Width, this.Height);
                    gr.DrawString(str, this.Font, this.m_brushSelGroupsFore, rectangle);
                }
                else
                {
                    rectangle = new Rectangle((int) ((x + (2f * m_scaleFactor.Width)) + (5f * m_scaleFactor.Width)), y, rect.Width, this.Height);
                    gr.DrawString(str, this.Font, this.m_brushGroupsFore, rectangle);
                }
                x += (int) ((size.Width + (2f * m_scaleFactor.Width)) + (5f * m_scaleFactor.Width));
            Label_08B7:
                num3++;
            }
            return rectangle2;
        }

        internal void DrawImage(Graphics gr, Image image, Rectangle dest, Rectangle src, ImageAttributes ia, bool bAutoTransparent)
        {
            if (bAutoTransparent)
            {
                Bitmap bitmap = new Bitmap(image);
                Color pixel = bitmap.GetPixel(0, 0);
                ia.SetColorKey(pixel, pixel);
                bitmap.Dispose();
                bitmap = null;
            }
            gr.DrawImage(image, dest, src.X, src.Y, src.Width, src.Height, GraphicsUnit.Pixel, ia);
        }

        private void GroupSelectedShortcutIndexChanged(object sender, SelectedIndexChangedEventArgs e)
        {
            if (this.SelectedShortcutIndexChanged != null)
            {
                this.SelectedShortcutIndexChanged(sender, new SelectedIndexChangedEventArgs(this.m_selectedIndex, e.ShortcutIndex, e.SelectionAction));
            }
        }

        public int IndexOf(Group value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, Group value)
        {
            base.List.Insert(index, value);
        }

        public void Invalidate()
        {
            if (this.Invalidating != null)
            {
                this.Invalidating(this, new EventArgs());
            }
        }

        internal void MouseDown(Point p, Rectangle rect)
        {
            switch (this.m_groupStyle)
            {
                case Resco.Controls.OutlookControls.GroupStyle.Links:
                {
                    int num5 = this.CalculateHeight();
                    if (!this.CheckLinkPressed(p, rect))
                    {
                        this[this.SelectedIndex].MouseDown(p, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - num5));
                    }
                    return;
                }
            }
            int num2 = this.CalculateHeight();
            int num3 = this.RealVisibleIndex(this.m_selectedIndex);
            if (this.m_showGroups && (this.m_Height != 0))
            {
                this.m_pushedIndex = p.Y / this.m_Height;
                if ((num3 < 0) || (num3 == (base.Count - 1)))
                {
                    if (this.m_pushedIndex >= base.Count)
                    {
                        this.m_pushedIndex = -1;
                    }
                }
                else if (this.m_pushedIndex > num3)
                {
                    int num4 = rect.Height - (((this.VisibleCount() - num3) - 1) * this.m_Height);
                    int num = p.Y - num4;
                    num2 = (num3 + 1) * this.m_Height;
                    if (num > -1)
                    {
                        this.m_pushedIndex = ((num / this.Height) + num3) + 1;
                    }
                    else
                    {
                        this.m_pushedIndex = -1;
                    }
                }
            }
            else
            {
                num2 = 0;
                this.m_pushedIndex = -1;
            }
            p.Y -= num2;
            if (((this.m_pushedIndex == -1) && (p.Y > -1)) && (this.m_selectedIndex > -1))
            {
                this[this.SelectedIndex].MouseDown(p, new Rectangle(rect.X, rect.Y + num2, rect.Width, rect.Height - this.CalculateHeight()));
            }
        }

        internal void MouseMove(Point p, Rectangle rect)
        {
            switch (this.m_groupStyle)
            {
                case Resco.Controls.OutlookControls.GroupStyle.Links:
                {
                    int num5 = this.CalculateHeight();
                    if (!this.CheckLinkPressed(p, rect))
                    {
                        this[this.SelectedIndex].MouseMove(p, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - num5));
                    }
                    return;
                }
            }
            int num2 = this.CalculateHeight();
            int num3 = this.RealVisibleIndex(this.m_selectedIndex);
            if (this.m_showGroups && (this.m_Height != 0))
            {
                this.m_pushedIndex = p.Y / this.m_Height;
                if ((num3 < 0) || (num3 == (base.Count - 1)))
                {
                    if (this.m_pushedIndex >= base.Count)
                    {
                        this.m_pushedIndex = -1;
                    }
                }
                else if (this.m_pushedIndex > num3)
                {
                    int num4 = rect.Height - (((this.VisibleCount() - num3) - 1) * this.m_Height);
                    int num = p.Y - num4;
                    num2 = (num3 + 1) * this.m_Height;
                    if (num > -1)
                    {
                        this.m_pushedIndex = ((num / this.Height) + num3) + 1;
                    }
                    else
                    {
                        this.m_pushedIndex = -1;
                    }
                }
            }
            else
            {
                num2 = 0;
                this.m_pushedIndex = -1;
            }
            if (this.m_pushedIndex >= base.Count)
            {
                this.m_pushedIndex = base.Count - 1;
            }
            p.Y -= num2;
            if (((this.m_pushedIndex == -1) && (p.Y > -1)) && (this.m_selectedIndex > -1))
            {
                this[this.SelectedIndex].MouseMove(p, new Rectangle(rect.X, rect.Y + num2, rect.Width, rect.Height - this.CalculateHeight()));
            }
        }

        internal void MouseUp(Point p, Rectangle rect)
        {
            bool flag = false;
            switch (this.m_groupStyle)
            {
                case Resco.Controls.OutlookControls.GroupStyle.Links:
                {
                    int num5 = this.CalculateHeight();
                    if (!this.CheckLinkPressed(p, rect))
                    {
                        this[this.SelectedIndex].MouseUp(p, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - num5));
                    }
                    return;
                }
            }
            int num2 = this.CalculateHeight();
            int num3 = this.RealVisibleIndex(this.m_selectedIndex);
            if (this.m_showGroups && (this.m_Height != 0))
            {
                this.m_pushedIndex = p.Y / this.m_Height;
                if ((num3 < 0) || (num3 == (base.Count - 1)))
                {
                    if (this.m_pushedIndex >= base.Count)
                    {
                        this.m_pushedIndex = -1;
                    }
                }
                else if (this.m_pushedIndex > num3)
                {
                    int num4 = rect.Height - (((this.VisibleCount() - num3) - 1) * this.m_Height);
                    int num = p.Y - num4;
                    num2 = (num3 + 1) * this.m_Height;
                    if (num > -1)
                    {
                        this.m_pushedIndex = ((num / this.Height) + num3) + 1;
                    }
                    else
                    {
                        this.m_pushedIndex = -1;
                    }
                }
                if (this.m_pushedIndex > (base.Count - 1))
                {
                    flag = true;
                    this.m_pushedIndex = -1;
                }
                this.m_bDoInvalidate = false;
                this.VisibilityIndexCorrection(ref this.m_pushedIndex);
                this.SelectedIndex = (this.m_pushedIndex != this.m_selectedIndex) ? ((this.m_pushedIndex == -1) ? this.m_selectedIndex : this.m_pushedIndex) : this.m_selectedIndex;
                flag = true;
            }
            else
            {
                num2 = 0;
                this.m_pushedIndex = -1;
            }
            p.Y -= num2;
            if (((this.m_pushedIndex == -1) && (p.Y > -1)) && (this.m_selectedIndex > -1))
            {
                this[this.SelectedIndex].MouseUp(p, new Rectangle(rect.X, rect.Y + num2, rect.Width, rect.Height - this.CalculateHeight()));
            }
            if (flag)
            {
                this.m_pushedIndex = -1;
            }
        }

        protected override void OnClear()
        {
            base.OnClear();
            this.OnInvalidating(this, new EventArgs());
        }

        private void OnGroupsShortcutEntered(object sender, SelectedIndexChangedEventArgs e)
        {
            if (this.ShortcutEntered != null)
            {
                this.ShortcutEntered(sender, new SelectedIndexChangedEventArgs(this.m_selectedIndex, e.ShortcutIndex, e.SelectionAction));
            }
        }

        protected override void OnInsert(int index, object value)
        {
            if (!typeof(Group).IsInstanceOfType(value))
            {
                throw new ArgumentException("Value must be of type Group.");
            }
            if (this.InsertEvent != null)
            {
                this.InsertEvent(this, new InsertEventArgs(index, value));
            }
            base.OnInsert(index, value);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            if (this.InsertCompleteEvent != null)
            {
                this.InsertCompleteEvent(this, new InsertEventArgs(index, value));
            }
            ((Group) value).SelectedShortcutIndexChanged += new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.GroupSelectedShortcutIndexChanged);
            ((Group) value).Invalidating += new EventHandler(this.OnInvalidating);
            ((Group) value).ShortcutEntered += new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnGroupsShortcutEntered);
            ((Group) value).Parent = this;
            base.OnInsertComplete(index, value);
        }

        private void OnInvalidating(object sender, EventArgs e)
        {
            if (this.Invalidating != null)
            {
                this.Invalidating(sender, e);
            }
        }

        protected override void OnRemove(int index, object value)
        {
            if (!typeof(Group).IsInstanceOfType(value))
            {
                throw new ArgumentException("Value must be of type Group.");
            }
            if (this.RemoveEvent != null)
            {
                this.RemoveEvent(this, new RemoveEventArgs(index, value));
            }
            base.OnRemove(index, value);
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            if (this.RemoveCompleteEvent != null)
            {
                this.RemoveCompleteEvent(this, new RemoveEventArgs(index, value));
            }
            if ((this.m_selectedIndex > -1) && (base.Count == 0))
            {
                this.m_selectedIndex = -1;
            }
            ((Group) value).SelectedShortcutIndexChanged -= new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.GroupSelectedShortcutIndexChanged);
            ((Group) value).Invalidating -= new EventHandler(this.OnInvalidating);
            ((Group) value).ShortcutEntered -= new Resco.Controls.OutlookControls.SelectedIndexChangedEventHandler(this.OnGroupsShortcutEntered);
            base.OnRemoveComplete(index, value);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (!typeof(Group).IsInstanceOfType(newValue))
            {
                throw new ArgumentException("Value must be of type Group.");
            }
            if (this.SetEvent != null)
            {
                this.SetEvent(this, new SetEventArgs(index, oldValue, newValue));
            }
            base.OnSet(index, oldValue, newValue);
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (this.SetCompleteEvent != null)
            {
                this.SetCompleteEvent(this, new SetEventArgs(index, oldValue, newValue));
            }
            base.OnSetComplete(index, oldValue, newValue);
        }

        protected override void OnValidate(object value)
        {
            if (!typeof(Group).IsInstanceOfType(value))
            {
                throw new ArgumentException("Value must be of type Group.");
            }
            base.OnValidate(value);
        }

        private int RealVisibleIndex(int aSelectedIndex)
        {
            if ((aSelectedIndex < 0) || (aSelectedIndex >= base.Count))
            {
                return -1;
            }
            int num = aSelectedIndex;
            for (int i = 0; i < aSelectedIndex; i++)
            {
                if (!this[i].Visible)
                {
                    num--;
                }
            }
            return num;
        }

        public void Remove(Group value)
        {
            base.List.Remove(value);
        }

        private void VisibilityIndexCorrection(ref int aPushedIndex)
        {
            if ((aPushedIndex >= 0) && (aPushedIndex < base.Count))
            {
                int num = aPushedIndex;
                for (int i = 0; i < base.Count; i++)
                {
                    if (this[i].Visible)
                    {
                        num--;
                        if (num == -1)
                        {
                            aPushedIndex = i;
                            return;
                        }
                    }
                }
            }
        }

        private int VisibleCount()
        {
            int num = 0;
            for (int i = 0; i < base.Count; i++)
            {
                if (this[i].Visible)
                {
                    num++;
                }
            }
            return num;
        }

        public Group[] All
        {
            get
            {
                Group[] array = new Group[base.List.Count];
                base.List.CopyTo(array, 0);
                return array;
            }
            set
            {
                base.List.Clear();
                foreach (Group group in value)
                {
                    this.Add(group);
                }
            }
        }

        public bool AutoScaleEnabled
        {
            get
            {
                return this.m_bAutoScaleEnabled;
            }
            set
            {
                this.m_bAutoScaleEnabled = value;
            }
        }

        public Color BackColor
        {
            get
            {
                return this.m_groupsBackColor;
            }
            set
            {
                if (value != this.m_groupsBackColor)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.Control;
                    }
                    this.m_groupsBackColor = value;
                    this.Invalidate();
                }
            }
        }

        public bool Border
        {
            get
            {
                return this.m_border;
            }
            set
            {
                if (this.m_border != value)
                {
                    this.m_border = value;
                    this.Invalidate();
                }
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this.m_Font;
            }
            set
            {
                if (value != this.m_Font)
                {
                    this.m_Font = value;
                    this.Invalidate();
                }
            }
        }

        public Color ForeColor
        {
            get
            {
                return this.m_groupsForeColor;
            }
            set
            {
                if (value != this.m_groupsForeColor)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.WindowText;
                    }
                    this.m_groupsForeColor = value;
                    this.Invalidate();
                }
            }
        }

        public Resco.Controls.OutlookControls.GroupStyle GroupStyle
        {
            get
            {
                return this.m_groupStyle;
            }
            set
            {
                if (this.m_groupStyle != value)
                {
                    this.m_groupStyle = value;
                    this.Invalidate();
                }
            }
        }

        public int Height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                if (value != this.m_Height)
                {
                    this.m_Height = value;
                    this.Invalidate();
                }
            }
        }

        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                return this.m_ImageList;
            }
            set
            {
                this.m_ImageList = value;
                this.Invalidate();
            }
        }

        public bool ImageOnly
        {
            get
            {
                return this.m_ImageOnly;
            }
            set
            {
                if (value != this.m_ImageOnly)
                {
                    this.m_ImageOnly = value;
                    this.Invalidate();
                }
            }
        }

        public Group this[int index]
        {
            get
            {
                if ((index < 0) || (index >= base.List.Count))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return (Group) base.List[index];
            }
            set
            {
                if ((index < 0) || (index >= base.List.Count))
                {
                    throw new ArgumentOutOfRangeException();
                }
                base.List[index] = value;
            }
        }

        public Group this[string name]
        {
            get
            {
                int num = 0;
                foreach (Group group in base.List)
                {
                    if (group.Name == name)
                    {
                        return group;
                    }
                    num++;
                }
                throw new ArgumentOutOfRangeException();
            }
            set
            {
                int num = 0;
                foreach (Group group in base.List)
                {
                    if (group.Name == name)
                    {
                        base.List[num] = value;
                        return;
                    }
                    num++;
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }

        internal int ScrollBarHeight
        {
            get
            {
                if ((base.List == null) || (base.List.Count == 0))
                {
                    return 0;
                }
                Group group = base.List[0] as Group;
                if (group == null)
                {
                    return 0;
                }
                return group.ScrollBarHeight;
            }
            set
            {
                foreach (Group group in base.List)
                {
                    group.ScrollBarHeight = value;
                }
            }
        }

        internal int ScrollBarWidth
        {
            get
            {
                if ((base.List == null) || (base.List.Count == 0))
                {
                    return 0;
                }
                Group group = base.List[0] as Group;
                if (group == null)
                {
                    return 0;
                }
                return group.ScrollBarWidth;
            }
            set
            {
                foreach (Group group in base.List)
                {
                    group.ScrollBarWidth = value;
                }
            }
        }

        public Color SelBackColor
        {
            get
            {
                return this.m_selGroupsBackColor;
            }
            set
            {
                if (value.IsEmpty)
                {
                    value = SystemColors.ControlDark;
                }
                if (value != this.m_selGroupsBackColor)
                {
                    this.m_selGroupsBackColor = value;
                    this.Invalidate();
                }
            }
        }

        public int SelectedIndex
        {
            get
            {
                if (this.m_pushedIndex != -1)
                {
                    return this.m_pushedIndex;
                }
                return this.m_selectedIndex;
            }
            set
            {
                if (this.m_selectedIndex != value)
                {
                    if (value >= base.Count)
                    {
                        throw new ArgumentOutOfRangeException("SelectedIndex of groups is out of index.");
                    }
                    this.m_selectedIndex = value;
                    if (this.m_bDoInvalidate)
                    {
                        this.Invalidate();
                    }
                    if (this.SelectedGroupIndexChanged != null)
                    {
                        int shortcutIndex = (this.m_selectedIndex < 0) ? -1 : this[this.m_selectedIndex].SelectedIndex;
                        this.SelectedGroupIndexChanged(this, new SelectedIndexChangedEventArgs(this.m_selectedIndex, shortcutIndex));
                    }
                }
                this.m_bDoInvalidate = true;
            }
        }

        public Color SelForeColor
        {
            get
            {
                return this.m_selGroupsForeColor;
            }
            set
            {
                if (value != this.m_selGroupsForeColor)
                {
                    if (value.IsEmpty)
                    {
                        value = SystemColors.HighlightText;
                    }
                    this.m_selGroupsForeColor = value;
                    this.Invalidate();
                }
            }
        }

        public bool Show
        {
            get
            {
                return this.m_showGroups;
            }
            set
            {
                if (this.m_showGroups != value)
                {
                    this.m_showGroups = value;
                    this.Invalidate();
                }
            }
        }

        public bool UseVistaStyle
        {
            get
            {
                return this.m_UseVistaStyle;
            }
            set
            {
                this.m_UseVistaStyle = value;
                this.Invalidate();
            }
        }
    }
}

