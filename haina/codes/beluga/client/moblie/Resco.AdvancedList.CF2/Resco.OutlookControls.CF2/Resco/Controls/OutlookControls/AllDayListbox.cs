namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class AllDayListbox : UserControl, IDisposable
    {
        private bool bPressed;
        private IContainer components;
        private Graphics gxOff;
        private const int KMaxRows = 6;
        private Bitmap m_bmpOffscreen;
        private System.Windows.Forms.ContextMenu m_ContextMenu;
        private bool m_EnableToolTip;
        private Color m_GridColor;
        private List<AllDayListboxItem> m_Items;
        private Timer m_KeyTimer;
        private KeyDirection m_KeyType;
        private bool m_ParentFocused;
        private int m_RowHeight;
        private int m_scrollWidth;
        private Color m_SelectedBackColor;
        private Color m_SelectedForeColor;
        private int m_SelectedRowIdx;
        private bool m_UseAppointmentsColor;
        private VScrollBar m_vs;
        public const int WM_CHAR = 0x102;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;

        public event RowEnteredEventHandler RowEntered;

        public event ToolTipRequiredEventHandler ToolTipRequired;

        public AllDayListbox()
        {
            this.m_KeyType = KeyDirection.None;
            this.m_GridColor = SystemColors.ActiveBorder;
            this.m_SelectedForeColor = SystemColors.HighlightText;
            this.m_SelectedBackColor = SystemColors.Highlight;
            this.m_RowHeight = 15;
            this.m_SelectedRowIdx = -1;
            this.m_ParentFocused = true;
            this.InitializeComponent();
            this.InitScrollBar();
            this.InitTimer();
            this.BackColor = SystemColors.Control;
            this.Font = new Font(this.Font.Name, this.Font.Size, FontStyle.Bold);
        }

        public AllDayListbox(List<CustomAppointment> aCustomAppList) : this()
        {
            this.CustomAppList = aCustomAppList;
        }

        protected void CustomDispose()
        {
            if (this.gxOff != null)
            {
                this.gxOff.Dispose();
                this.gxOff = null;
            }
            if (this.m_bmpOffscreen != null)
            {
                this.m_bmpOffscreen.Dispose();
                this.m_bmpOffscreen = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.CustomDispose();
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DrawAllDayEvents(Graphics gr, Rectangle aClientRect)
        {
            gr.Clear(this.BackColor);
            Pen pen = new Pen(this.m_GridColor);
            SolidBrush brush = new SolidBrush(this.m_SelectedForeColor);
            SolidBrush brush2 = new SolidBrush(this.ForeColor);
            gr.DrawLine(pen, 0, 0, aClientRect.Width, 0);
            int num = 0;
            for (int i = this.m_vs.Value; i <= (this.m_vs.Value + this.DrawCount); i++)
            {
                SolidBrush brush3;
                if (i >= this.m_Items.Count)
                {
                    return;
                }
                Rectangle rect = new Rectangle(0, num + 1, aClientRect.Width, this.m_RowHeight - 1);
                if (this.m_SelectedRowIdx == i)
                {
                    using (SolidBrush brush4 = new SolidBrush(this.m_SelectedBackColor))
                    {
                        gr.FillRectangle(brush4, rect);
                    }
                    brush3 = brush;
                }
                else if (this.m_UseAppointmentsColor)
                {
                    using (SolidBrush brush5 = new SolidBrush(this.m_Items[i].RowsBackcolor))
                    {
                        gr.FillRectangle(brush5, rect);
                    }
                    brush3 = brush2;
                }
                else
                {
                    brush3 = brush2;
                }
                this.DrawItem(gr, i, rect, brush3);
                gr.DrawLine(pen, 0, num + this.m_RowHeight, aClientRect.Width, num + this.m_RowHeight);
                num += this.m_RowHeight;
            }
            pen.Dispose();
            pen = null;
            brush.Dispose();
            brush = null;
            brush2.Dispose();
            brush2 = null;
        }

        private void DrawCenteredText(Graphics gr, string aText, Rectangle aRect, SolidBrush aBrush)
        {
            SizeF ef = gr.MeasureString(aText, this.Font);
            float x = aRect.X + ((aRect.Width - ef.Width) / 2f);
            float y = aRect.Y + ((aRect.Height - ef.Height) / 2f);
            RectangleF layoutRectangle = new RectangleF(x, y, ef.Width, ef.Height);
            gr.DrawString(aText, this.Font, aBrush, layoutRectangle);
        }

        private void DrawItem(Graphics gr, int anItemIdx, Rectangle rect, SolidBrush aBrush)
        {
            string subject = this.m_Items[anItemIdx].Subject;
            SizeF ef = gr.MeasureString(subject, this.Font);
            this.m_Items[anItemIdx].NeedToolTip = ef.Width > base.Width;
            while (ef.Width > base.Width)
            {
                int length = subject.Length / 2;
                subject = this.m_Items[anItemIdx].Subject.Substring(0, length);
                ef = gr.MeasureString(subject, this.Font);
            }
            if (this.m_Items[anItemIdx].NeedToolTip)
            {
                subject = subject + "...";
            }
            this.DrawCenteredText(gr, subject, rect, aBrush);
            if (this.m_Items[anItemIdx].NeedToolTip)
            {
                this.DrawToolTipArrow(gr, rect);
            }
        }

        private void DrawToolTipArrow(Graphics gr, Rectangle aRect)
        {
            Point[] pointArray= new Point[3] ;
            int num = this.RowHeight / 2;
            pointArray = new Point[] { new Point(aRect.Width - 1, aRect.Y + num), new Point(pointArray[0].X, (pointArray[0].Y + num) - 1), new Point(pointArray[1].X - num, pointArray[1].Y) };
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                gr.FillPolygon(brush, pointArray);
            }
        }

        public void EnsureVisible(int index, bool refresh)
        {
            if (index < this.m_vs.Value)
            {
                this.m_vs.Value = index;
            }
            else if (index >= (this.m_vs.Value + this.DrawCount))
            {
                this.m_vs.Value = (index - this.DrawCount) + 1;
            }
            else if (refresh)
            {
                this.Refresh();
            }
        }

        internal int GetAppointmentAt(int aX, int aY)
        {
            return ((aY / this.m_RowHeight) + this.m_vs.Value);
        }

        internal Rectangle GetAppRectAt(int aX, int aY)
        {
            Rectangle rectangle = new Rectangle();
            if (this.m_vs.Visible)
            {
                rectangle.Width = base.ClientSize.Width - this.m_scrollWidth;
            }
            else
            {
                rectangle.Width = base.ClientSize.Width;
            }
            rectangle.Height = this.m_RowHeight;
            int num = (aY / this.m_RowHeight) + this.m_vs.Value;
            rectangle.Location = new Point(base.Location.X, base.Location.Y + (num * this.m_RowHeight));
            return rectangle;
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions=(new SizeF(96f, 96f));
            base.AutoScaleMode=AutoScaleMode.Dpi;
            base.Name = "AllDayListbox";
            base.Size = new Size(0xe1, 0x3f);
            base.ResumeLayout(false);
        }

        private void InitScrollBar()
        {
            if (this.m_vs == null)
            {
                this.m_vs = new VScrollBar();
                this.m_scrollWidth = this.m_vs.Width;
                this.m_vs.Parent = this;
                this.m_vs.Visible = false;
                this.m_vs.SmallChange = 1;
                this.m_vs.ValueChanged += new EventHandler(this.ScrollValueChanged);
            }
        }

        private void InitTimer()
        {
            this.m_KeyTimer = new Timer();
            this.m_KeyTimer.Interval = 100;
            this.m_KeyTimer.Tick += new EventHandler(this.m_KeyTimer_Tick);
        }

        private void m_KeyTimer_Tick(object sender, EventArgs e)
        {
            if (this.m_KeyType == KeyDirection.Down)
            {
                if ((this.SelectedIndex < this.m_vs.Maximum) && ((this.SelectedIndex + 1) < this.m_Items.Count))
                {
                    int num;
                    this.SelectedIndex = num = this.SelectedIndex + 1;
                    this.EnsureVisible(num, true);
                }
            }
            else if ((this.SelectedIndex > this.m_vs.Minimum) && ((this.SelectedIndex - 1) >= 0))
            {
                int num2;
                this.SelectedIndex = num2 = this.SelectedIndex - 1;
                this.EnsureVisible(num2, true);
            }
            this.Refresh();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.bPressed)
            {
                e.Handled = true;
                return;
            }
            if (this.m_ParentFocused)
            {
                this.SendKeyDownToParent(e.KeyCode);
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        if ((this.SelectedIndex > this.m_vs.Minimum) && ((this.SelectedIndex - 1) >= 0))
                        {
                            int num2;
                            this.SelectedIndex = num2 = this.SelectedIndex - 1;
                            this.EnsureVisible(num2, true);
                            this.bPressed = true;
                            this.m_KeyType = KeyDirection.Up;
                            this.m_KeyTimer.Enabled = true;
                        }
                        goto Label_0118;

                    case Keys.Right:
                        goto Label_0118;

                    case Keys.Down:
                        if ((this.SelectedIndex >= this.m_vs.Maximum) || ((this.SelectedIndex + 1) >= this.m_Items.Count))
                        {
                            this.ParentFocused = true;
                        }
                        else
                        {
                            int num;
                            this.SelectedIndex = num = this.SelectedIndex + 1;
                            this.EnsureVisible(num, true);
                            this.bPressed = true;
                            this.m_KeyType = KeyDirection.Down;
                            this.m_KeyTimer.Enabled = true;
                        }
                        goto Label_0118;

                    case Keys.Return:
                        if (this.SelectedIndex >= 0)
                        {
                            this.OnRowEntered();
                        }
                        goto Label_0118;
                }
            }
        Label_0118:
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (this.m_KeyTimer.Enabled)
            {
                this.m_KeyTimer.Enabled = false;
                this.bPressed = false;
                this.m_KeyType = KeyDirection.None;
            }
            if (this.m_ParentFocused)
            {
                this.SendKeyUpToParent(e.KeyCode);
            }
            base.OnKeyUp(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (this.m_KeyTimer.Enabled)
            {
                this.m_KeyTimer.Enabled = false;
                this.bPressed = false;
                this.m_KeyType = KeyDirection.None;
            }
            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.m_EnableToolTip && (e.X > (base.Width - (this.RowHeight / 2))))
                {
                    this.OnToolTipRequired(e);
                }
                else
                {
                    this.SelectedIndex = (e.Y / this.m_RowHeight) + this.m_vs.Value;
                    this.EnsureVisible(this.SelectedIndex, true);
                    this.OnRowEntered();
                }
            }
            if ((this.ContextMenu != null) && MouseUtils.IsContextMenu(e, base.Handle))
            {
                this.ContextMenu.Show(this, new Point(e.X, e.Y));
            }
            else
            {
                base.OnMouseDown(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.DrawAllDayEvents(this.gxOff, e.ClipRectangle);
            e.Graphics.DrawImage(this.m_bmpOffscreen, 0, 0);
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            if ((base.ClientSize.Width == 0) || (base.ClientSize.Height == 0))
            {
                base.OnResize(e);
            }
            else
            {
                int num = base.ClientSize.Height / this.m_RowHeight;
                if (this.m_vs == null)
                {
                    this.InitScrollBar();
                }
                this.m_vs.Bounds = new Rectangle(base.ClientSize.Width - this.m_scrollWidth, 0, this.m_scrollWidth, base.ClientSize.Height);
                if (this.m_bmpOffscreen != null)
                {
                    this.m_bmpOffscreen.Dispose();
                    this.m_bmpOffscreen = null;
                    this.gxOff.Dispose();
                    this.gxOff = null;
                }
                if ((this.m_Items != null) && (this.m_Items.Count > num))
                {
                    this.m_vs.Visible = true;
                    this.m_vs.LargeChange = num;
                    this.m_vs.Maximum = this.m_Items.Count - 1;
                    this.m_bmpOffscreen = new Bitmap(base.ClientSize.Width - this.m_scrollWidth, base.ClientSize.Height);
                }
                else
                {
                    this.m_vs.Visible = false;
                    this.m_bmpOffscreen = new Bitmap(base.ClientSize.Width, base.ClientSize.Height);
                }
                this.gxOff = Graphics.FromImage(this.m_bmpOffscreen);
            }
        }

        protected virtual void OnRowEntered()
        {
            if (this.RowEntered != null)
            {
                RowEventArgs e = new RowEventArgs(this.m_SelectedRowIdx);
                this.RowEntered(this, e);
            }
        }

        protected virtual void OnToolTipRequired(MouseEventArgs aMouse)
        {
            if (this.RowEntered != null)
            {
                RowEventArgs e = new RowEventArgs(this.m_SelectedRowIdx, aMouse);
                this.ToolTipRequired(this, e);
            }
        }

        protected void ScrollValueChanged(object o, EventArgs e)
        {
            this.Refresh();
        }

        private void SendKeyDownToParent(Keys aKeyCode)
        {
            char wParam = (char) ((ushort) aKeyCode);
            SendMessage(base.Parent.Handle, 0x100, wParam, 0);
        }

        private void SendKeyUpToParent(Keys aKeyCode)
        {
            char wParam = (char) ((ushort) aKeyCode);
            SendMessage(base.Parent.Handle, 0x101, wParam, 0);
        }

        [DllImport("coredll.dll")]
        public static extern int SendMessage(IntPtr hwnd, uint msg, uint wParam, uint lParam);

        public override System.Windows.Forms.ContextMenu ContextMenu
        {
            get
            {
                return this.m_ContextMenu;
            }
            set
            {
                this.m_ContextMenu = value;
            }
        }

        public List<CustomAppointment> CustomAppList
        {
            set
            {
                if (this.m_Items == null)
                {
                    this.m_Items = new List<AllDayListboxItem>();
                }
                else
                {
                    this.m_Items.Clear();
                }
                for (int i = 0; i < value.Count; i++)
                {
                    this.m_Items.Add(new AllDayListboxItem(value[i].Title, value[i].BorderColor));
                }
                int num2 = (this.m_Items.Count > 6) ? 6 : this.m_Items.Count;
                base.Height = num2 * this.m_RowHeight;
            }
        }

        protected int DrawCount
        {
            get
            {
                if ((this.m_vs.Value + this.m_vs.LargeChange) > this.m_vs.Maximum)
                {
                    return ((this.m_vs.Maximum - this.m_vs.Value) + 1);
                }
                return this.m_vs.LargeChange;
            }
        }

        internal bool EnableToolTip
        {
            get
            {
                return this.m_EnableToolTip;
            }
            set
            {
                this.m_EnableToolTip = value;
            }
        }

        public Color GridColor
        {
            get
            {
                return this.m_GridColor;
            }
            set
            {
                this.m_GridColor = value;
            }
        }

        public bool ParentFocused
        {
            get
            {
                return this.m_ParentFocused;
            }
            set
            {
                if (this.m_ParentFocused != value)
                {
                    this.m_ParentFocused = value;
                    if (this.m_ParentFocused)
                    {
                        this.SelectedIndex = -1;
                        base.Invalidate();
                    }
                    else
                    {
                        this.SelectedIndex = this.m_Items.Count - 1;
                        this.EnsureVisible(this.SelectedIndex, true);
                        base.Focus();
                    }
                }
            }
        }

        public int RowHeight
        {
            get
            {
                return this.m_RowHeight;
            }
            set
            {
                this.m_RowHeight = value;
            }
        }

        public int ScrollbarWidth
        {
            get
            {
                return this.m_vs.Width;
            }
            set
            {
                this.m_vs.Width = value;
                this.m_scrollWidth = this.m_vs.Width;
            }
        }

        public Color SelectedBackColor
        {
            get
            {
                return this.m_SelectedBackColor;
            }
            set
            {
                this.m_SelectedBackColor = value;
            }
        }

        public Color SelectedForeColor
        {
            get
            {
                return this.m_SelectedForeColor;
            }
            set
            {
                this.m_SelectedForeColor = value;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.m_SelectedRowIdx;
            }
            set
            {
                this.m_SelectedRowIdx = value;
            }
        }

        internal bool UseAppointmentsColor
        {
            get
            {
                return this.m_UseAppointmentsColor;
            }
            set
            {
                this.m_UseAppointmentsColor = value;
            }
        }

        private enum KeyDirection
        {
            Down = 1,
            None = -1,
            Up = 0
        }

        public delegate void RowEnteredEventHandler(object sender, AllDayListbox.RowEventArgs e);

        public class RowEventArgs : EventArgs
        {
            private MouseEventArgs m_Mouse;
            private int m_RowIndex;

            public RowEventArgs(int aRowIndex)
            {
                this.m_RowIndex = aRowIndex;
                this.m_Mouse = new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0);
            }

            public RowEventArgs(int aRowIndex, MouseEventArgs aMouseEventArgs)
            {
                this.m_RowIndex = aRowIndex;
                this.m_Mouse = aMouseEventArgs;
            }

            public int EnteredRowIndex
            {
                get
                {
                    return this.m_RowIndex;
                }
            }

            public MouseEventArgs Mouse
            {
                get
                {
                    return this.m_Mouse;
                }
                set
                {
                    this.m_Mouse = value;
                }
            }
        }

        public delegate void ToolTipRequiredEventHandler(object sender, AllDayListbox.RowEventArgs e);
    }
}

