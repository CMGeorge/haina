namespace Resco.Controls.OutlookControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TouchDtpRollerControl : UserControl, IParentControl
    {
        private IContainer components;
        public Resco.Controls.OutlookControls.Day FirstDayOfWeek;
        private Graphics gxOff;
        public bool IsNone;
        private int KHeightCtrlBottomBar = 0x1a;
        private Bitmap m_bmpOffscreen;
        private int m_ClickedRollerIndex;
        private ControlBottomBar m_ControlBottomBar;
        private int m_MinuteInterval = 5;
        private TouchDTPRenderer m_Renderer = new TouchDTPRenderer();
        private RollerItemCollection m_RollerItems = new RollerItemCollection();
        internal static SizeF m_ScaleFactor;
        //private int m_ScrollDirection;
        internal string m_strYearFormat;
        private DateTime m_Value;
        public DateTime MaxDate = new DateTime(0xb54, 12, 0x1f);
        public DateTime MinDate = new DateTime(0x76c, 1, 1);
        public DateTime TodayDate;

        public event EventHandler CloseUp;

        public event EventHandler DayInfoClicked;

        public event EventHandler NoneButtonPressed;

        public event EventHandler TodayButtonPressed;

        public TouchDtpRollerControl()
        {
            this.InitRenderer();
            this.InitControlBottomBar();
        }

        private void AdjustRollersByValue()
        {
            for (int i = 0; i < this.m_RollerItems.Count; i++)
            {
                int num2;
                Roller roller = this.m_RollerItems[i];
                switch (roller.RollerDataType)
                {
                    case RollerType.Year:
                    {
                        roller.SetSelectedIndexWithoutInvalidate(this.m_Value.Year - this.MinDate.Year);
                        continue;
                    }
                    case RollerType.Month:
                    {
                        roller.SetSelectedIndexWithoutInvalidate(this.m_Value.Month - 1);
                        continue;
                    }
                    case RollerType.Day:
                    {
                        roller.SetSelectedIndexWithoutInvalidate(this.m_Value.Day - 1);
                        continue;
                    }
                    case RollerType.Hour:
                        if (!DateTimeFormatInfo.CurrentInfo.ShortTimePattern.StartsWith("h:"))
                        {
                            break;
                        }
                        num2 = this.GetSelectedIdxFor12hours();
                        goto Label_00B7;

                    case RollerType.Minute:
                    {
                        roller.SetSelectedIndexWithoutInvalidate(this.m_Value.Minute / this.m_MinuteInterval);
                        continue;
                    }
                    case RollerType.AmPm:
                    {
                        roller.SetSelectedIndexWithoutInvalidate((this.m_Value.Hour < 12) ? 0 : 1);
                        continue;
                    }
                    default:
                    {
                        continue;
                    }
                }
                num2 = this.GetSelectedIdxFor24hours();
            Label_00B7:
                roller.SetSelectedIndexWithoutInvalidate(num2);
            }
        }

        private void AdjustRollersPosition()
        {
            int num = 0;
            int num2 = 0;
            if (this.m_Renderer.ShowDayInfo)
            {
                num2 = this.m_Renderer.DayInfoHeight + 2;
            }
            int num3 = 0;
            if (this.VisibleBottomBar)
            {
                num3 = this.m_ControlBottomBar.Height + 1;
            }
            for (int i = 0; i < this.m_RollerItems.Count; i++)
            {
                num += this.m_RollerItems[i].Width + ((int) this.m_Renderer.KSeparatorPenWidth);
                this.m_RollerItems[i].Height = (base.Height - num2) - num3;
            }
            int x = (base.Width - num) / 2;
            x += (int) this.m_Renderer.KSeparatorPenWidth;
            int y = num2;
            for (int j = 0; j < this.m_RollerItems.Count; j++)
            {
                this.m_RollerItems[j].Location = new Point(x, y);
                x += this.m_RollerItems[j].Width + ((int) this.m_Renderer.KSeparatorPenWidth);
            }
        }

        private void ControlBottomBar_ButtonClick(object sender, BottomBarButton aButton)
        {
            if (aButton.Type == Resco.Controls.OutlookControls.ButtonType.None)
            {
                this.OnNoneButtonPressed();
            }
            else if (aButton.Type == Resco.Controls.OutlookControls.ButtonType.Today)
            {
                this.OnTodayButtonPressed();
            }
        }

        private void CreateOffScreenGraphics()
        {
            if (this.m_bmpOffscreen == null)
            {
                this.m_bmpOffscreen = new Bitmap(base.ClientSize.Width, base.ClientSize.Height);
                if (this.gxOff != null)
                {
                    this.gxOff.Dispose();
                    this.gxOff = null;
                }
                this.gxOff = Graphics.FromImage(this.m_bmpOffscreen);
            }
        }

        internal void Display(bool visible, int x, int y)
        {
            if (visible)
            {
                base.Left = x;
                base.Top = y;
                base.BringToFront();
                base.Focus();
            }
            base.Visible = visible;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
                if (this.m_RollerItems != null)
                {
                    for (int i = 0; i < this.m_RollerItems.Count; i++)
                    {
                        this.m_RollerItems[i].Dispose();
                    }
                    this.m_RollerItems.Clear();
                }
            }
            base.Dispose(disposing);
        }

        private void DrawControl(Graphics gxOff)
        {
            this.AdjustRollersPosition();
            this.m_Renderer.Draw(gxOff, base.ClientRectangle);
        }

        public void ForceRefresh()
        {
            this.Refresh();
        }

        private int GetClickedRollerIdx(MouseEventArgs e)
        {
            for (int i = 0; i < this.m_RollerItems.Count; i++)
            {
                if (this.m_RollerItems[i].ClientRectangle.Contains(e.X, e.Y))
                {
                    return i;
                }
            }
            return -1;
        }

        public DateTime GetMaxDate()
        {
            return this.MaxDate;
        }

        public DateTime GetMinDate()
        {
            return this.MinDate;
        }

        private int GetSelectedIdxFor12hours()
        {
            int num = 12;
            int num2 = (this.m_Value.Hour > num) ? (this.m_Value.Hour % num) : this.m_Value.Hour;
            if (this.m_Value.Hour == 0)
            {
                num2 = num;
            }
            num2--;
            return num2;
        }

        private int GetSelectedIdxFor24hours()
        {
            int num = 0x17;
            if (this.m_Value.Hour <= num)
            {
                return this.m_Value.Hour;
            }
            return (this.m_Value.Hour % num);
        }

        public DateTimePickerRow GetYearText(int aRowIndex)
        {
            bool anEnabled = true;
            bool anIsDefault = false;
            int year = DateTime.Today.Year;
            DateTime minDate = this.MinDate;
            if ((aRowIndex < 0) || ((aRowIndex + this.MinDate.Year) > this.MaxDate.Year))
            {
                return new DateTimePickerRow();
            }
            minDate = minDate.AddYears(aRowIndex);
            if (year == minDate.Year)
            {
                anIsDefault = true;
            }
            else
            {
                anIsDefault = false;
            }
            return new DateTimePickerRow(string.Format("{0:" + this.m_strYearFormat + "}", minDate), anEnabled, anIsDefault);
        }

        private void InitControlBottomBar()
        {
            this.m_ControlBottomBar = new ControlBottomBar(this);
            this.ResizeControlBottomBar();
            this.m_ControlBottomBar.ButtonClick += new ControlBottomBar.ButtonClickEventHandler(this.ControlBottomBar_ButtonClick);
            this.m_ControlBottomBar.Visible = false;
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions=(new SizeF(96f, 96f));
            base.AutoScaleMode=AutoScaleMode.Dpi;
            base.Name = "TouchDtpRollerControl";
            base.ResumeLayout(false);
        }

        private void InitRenderer()
        {
            this.m_RollerItems.RefreshRequired += new RefreshRequiredEventHandler(this.RollerItems_RefreshRequired);
            this.m_RollerItems.m_Parent = this;
            this.m_Renderer.Parent = this;
            this.m_Renderer.RollerItems = this.m_RollerItems;
        }

        private void InitTestRollers()
        {
        }

        private void OnDayInfoClicked()
        {
            if (this.DayInfoClicked != null)
            {
                this.DayInfoClicked(this, EventArgs.Empty);
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                base.Capture = true;
                this.m_ClickedRollerIndex = this.GetClickedRollerIdx(e);
                if (this.m_ClickedRollerIndex >= 0)
                {
                    this.m_RollerItems[this.m_ClickedRollerIndex].OnMouseDown(e);
                }
                else if (this.m_Renderer.ShowDayInfo && (this.m_Renderer.DayInfoHeight >= e.Y))
                {
                    this.OnDayInfoClicked();
                }
                if (this.VisibleBottomBar && this.m_ControlBottomBar.ClientRectangle.Contains(new Point(e.X, e.Y)))
                {
                    this.m_ControlBottomBar.MouseDown(e);
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.m_ClickedRollerIndex >= 0)
                {
                    this.m_RollerItems[this.m_ClickedRollerIndex].OnMouseMove(e);
                }
                if (this.VisibleBottomBar)
                {
                    this.m_ControlBottomBar.MouseMove(e);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                base.Capture = false;
                if (this.m_ClickedRollerIndex >= 0)
                {
                    this.m_RollerItems[this.m_ClickedRollerIndex].OnMouseUp(e);
                }
                if (this.VisibleBottomBar && this.m_ControlBottomBar.ClientRectangle.Contains(new Point(e.X, e.Y)))
                {
                    this.m_ControlBottomBar.MouseUp(e);
                }
            }
            base.OnMouseUp(e);
        }

        private void OnNoneButtonPressed()
        {
            if (this.NoneButtonPressed != null)
            {
                this.NoneButtonPressed(this, EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.CreateOffScreenGraphics();
            this.DrawControl(this.gxOff);
            if (this.VisibleBottomBar)
            {
                this.m_ControlBottomBar.DrawControl(this.gxOff);
            }
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorKey(this.m_Renderer.m_ParentTransparentKeyColor, this.m_Renderer.m_ParentTransparentKeyColor);
            Rectangle destRect = new Rectangle(0, 0, this.m_bmpOffscreen.Width, this.m_bmpOffscreen.Height);
            e.Graphics.DrawImage(this.m_bmpOffscreen, destRect, 0, 0, this.m_bmpOffscreen.Width, this.m_bmpOffscreen.Height, GraphicsUnit.Pixel, imageAttr);
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
                if (this.m_bmpOffscreen != null)
                {
                    this.m_bmpOffscreen.Dispose();
                    this.m_bmpOffscreen = null;
                }
                if (this.m_Renderer != null)
                {
                    this.m_Renderer.DisposePreparedBitmaps();
                }
                if (this.VisibleBottomBar)
                {
                    this.ResizeControlBottomBar();
                }
            }
        }

        private void OnTodayButtonPressed()
        {
            if (this.TodayButtonPressed != null)
            {
                this.TodayButtonPressed(this, EventArgs.Empty);
            }
        }

        private void ResizeControlBottomBar()
        {
            this.m_ControlBottomBar.Size = new Size(base.Width, (int) (this.KHeightCtrlBottomBar * Roller.m_ScaleFactor.Height));
            this.m_ControlBottomBar.Location = new Point(0, base.Height - this.m_ControlBottomBar.Height);
        }

        private void RollerItems_RefreshRequired(object sender, RollerItemEventArgs e)
        {
            this.Refresh();
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            m_ScaleFactor = factor;
            Roller.m_ScaleFactor = factor;
            base.ScaleControl(factor, specified);
        }

        internal ControlBottomBar BottomBar
        {
            get
            {
                return this.m_ControlBottomBar;
            }
            set
            {
                this.m_ControlBottomBar = value;
            }
        }

        internal int DayInfoHeight
        {
            get
            {
                return this.m_Renderer.DayInfoHeight;
            }
            set
            {
                this.m_Renderer.DayInfoHeight = value;
                base.Invalidate();
            }
        }

        internal string DayInfoText
        {
            get
            {
                return this.m_Renderer.DayInfoText;
            }
            set
            {
                this.m_Renderer.DayInfoText = value;
                base.Invalidate();
            }
        }

        public RollerItemCollection Items
        {
            get
            {
                return this.m_RollerItems;
            }
            set
            {
                this.m_RollerItems = value;
            }
        }

        internal int Margin
        {
            get
            {
                return this.m_Renderer.Margin;
            }
            set
            {
                this.m_Renderer.Margin = value;
            }
        }

        public int MinuteInterval
        {
            get
            {
                return this.m_MinuteInterval;
            }
            set
            {
                this.m_MinuteInterval = value;
            }
        }

        public string NoneText
        {
            get
            {
                return this.m_ControlBottomBar.NoneButton.Text;
            }
            set
            {
                this.m_ControlBottomBar.NoneButton.Text = value;
            }
        }

        internal TouchDTPRenderer Renderer
        {
            get
            {
                return this.m_Renderer;
            }
            set
            {
                this.m_Renderer = value;
            }
        }

        public bool ShowDayInfo
        {
            get
            {
                return this.m_Renderer.ShowDayInfo;
            }
            set
            {
                this.m_Renderer.ShowDayInfo = value;
                base.Invalidate();
            }
        }

        public bool ShowNone
        {
            get
            {
                return this.m_ControlBottomBar.NoneButton.Visible;
            }
            set
            {
                if (this.m_ControlBottomBar.NoneButton.Visible != value)
                {
                    this.m_ControlBottomBar.NoneButton.Visible = value;
                    base.Invalidate();
                }
            }
        }

        public string TodayText
        {
            get
            {
                return this.m_ControlBottomBar.TodayButton.Text;
            }
            set
            {
                this.m_ControlBottomBar.TodayButton.Text = value;
            }
        }

        public DateTime Value
        {
            get
            {
                return this.m_Value;
            }
            set
            {
                this.m_Value = value;
                this.DayInfoText = DateTimeFormatInfo.CurrentInfo.DayNames[(int) value.DayOfWeek];
                this.AdjustRollersByValue();
            }
        }

        public bool VisibleBottomBar
        {
            get
            {
                return ((this.m_ControlBottomBar != null) && this.m_ControlBottomBar.Visible);
            }
            set
            {
                if (this.m_ControlBottomBar != null)
                {
                    this.m_ControlBottomBar.Visible = value;
                }
            }
        }
    }
}

