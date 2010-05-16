namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using TouchScrolling;

    public class Roller
    {
        private Timer m_AdjustSelectedItemTimer = new Timer();
        private Rectangle m_ClientRectangle;
        private bool m_Continues;
        private int m_CursorDifY;
        private bool m_IsDisposed;
        private List<DateTimePickerRow> m_Items = new List<DateTimePickerRow>();
        private int m_MaxIndex;
        internal IParentControl m_Parent;
        private RollerType m_RollerDataType;
        private int m_RowHeight = 20;
        internal static SizeF m_ScaleFactor;
        private int m_ScrollbarOffset = 0;
        private int m_ScrollDirection;
        private int m_SelectedIndex = 0;
        private Font m_TextFont = new Font("Tahoma", 11f, FontStyle.Bold);
        private int m_TextOffsetY;
        private TouchScrolling.TouchTool m_TouchNavigatorTool;

        internal event RefreshRequiredEventHandler RefreshRequired;

        internal event SelectedIndexChangedEventHandler SelectedIndexChanged;

        public Roller()
        {
            this.m_AdjustSelectedItemTimer.Interval = 1;
            this.m_AdjustSelectedItemTimer.Tick += new EventHandler(this.AdjustSelectedItemTimer_Tick);
            this.InitTouchTool();
        }

        internal void AdjustSelectedItem(int aScrollDirection, bool aCenterNearest)
        {
            this.m_ScrollDirection = aScrollDirection;
            int rowHeight = this.RowHeight;
            int scrollbarOffset = this.ScrollbarOffset;
            int selectedIndex = this.SelectedIndex;
            if (this.m_ScrollDirection == 1)
            {
                this.m_CursorDifY = Math.Abs((int) (((selectedIndex + 1) * rowHeight) - scrollbarOffset));
                if (aCenterNearest && (this.m_CursorDifY > (rowHeight / 2)))
                {
                    this.m_ScrollDirection = -1;
                    this.m_CursorDifY = rowHeight - this.m_CursorDifY;
                }
            }
            else
            {
                this.m_CursorDifY = Math.Abs((int) ((selectedIndex * rowHeight) - scrollbarOffset));
                if (aCenterNearest && (this.m_CursorDifY > (rowHeight / 2)))
                {
                    this.m_ScrollDirection = 1;
                    this.m_CursorDifY = rowHeight - this.m_CursorDifY;
                }
            }
            this.m_AdjustSelectedItemTimer.Enabled = true;
        }

        private void AdjustSelectedItemTimer_Tick(object sender, EventArgs e)
        {
            int cursorDifY = (int) (4f * m_ScaleFactor.Height);
            if (cursorDifY > this.m_CursorDifY)
            {
                cursorDifY = this.m_CursorDifY;
            }
            this.ScrollbarOffset += this.m_ScrollDirection * cursorDifY;
            this.Refresh();
            this.m_CursorDifY -= cursorDifY;
            if (this.m_CursorDifY <= 0)
            {
                this.m_AdjustSelectedItemTimer.Enabled = false;
                if (this.ValidSelectedIndex())
                {
                    this.Refresh();
                    this.OnSelectedIndexChanged();
                }
            }
        }

        private void DeinitTouchTool()
        {
            if (this.m_TouchNavigatorTool != null)
            {
                this.m_TouchNavigatorTool.MouseMoveEnded -= new EventHandler(this.TouchNavigatorTool_MouseMoveEnded);
                this.m_TouchNavigatorTool.MouseMoveDetected -= new TouchScrolling.TouchTool.MouseMoveDetectedHandler(this.TouchNavigatorTool_MouseMoveDetected);
                this.m_TouchNavigatorTool.ParentControl = null;
                this.m_TouchNavigatorTool.Dispose();
                this.m_TouchNavigatorTool = null;
            }
        }

        internal void Dispose()
        {
            if (this.m_TextFont != null)
            {
                this.m_TextFont.Dispose();
                this.m_TextFont = null;
            }
            if (this.m_Items != null)
            {
                this.m_Items.Clear();
            }
        }

        internal string GetTextAt(int anIndex, out bool anIsDefaultItem, out bool anIsEnabled)
        {
            anIsDefaultItem = false;
            anIsEnabled = false;
            if (this.m_Continues)
            {
                if (anIndex < 0)
                {
                    anIndex += this.MaxIndex;
                }
                if (anIndex >= this.MaxIndex)
                {
                    anIndex -= this.MaxIndex;
                }
            }
            else if ((anIndex < 0) || (anIndex >= this.m_Items.Count))
            {
                return string.Empty;
            }
            anIsDefaultItem = this.m_Items[anIndex].IsDefault;
            anIsEnabled = this.m_Items[anIndex].Enabled;
            return this.m_Items[anIndex].Text;
        }

        internal List<DateTimePickerRow> GetTextFromTo(int anIndex, int aCount)
        {
            List<DateTimePickerRow> list = new List<DateTimePickerRow>();
            int num = anIndex + aCount;
            for (int i = anIndex; i < num; i++)
            {
                int aRowIndex = i;
                if (this.m_Continues)
                {
                    if (aRowIndex < 0)
                    {
                        aRowIndex = aRowIndex % this.MaxIndex;
                        if (aRowIndex < 0)
                        {
                            aRowIndex += this.MaxIndex;
                        }
                    }
                    else if (aRowIndex >= this.MaxIndex)
                    {
                        aRowIndex = aRowIndex % this.MaxIndex;
                    }
                }
                else if ((this.m_RollerDataType != RollerType.Year) && ((aRowIndex < 0) || (aRowIndex >= this.m_Items.Count)))
                {
                    list.Add(new DateTimePickerRow());
                    continue;
                }
                if (this.m_RollerDataType != RollerType.Year)
                {
                    list.Add(this.m_Items[aRowIndex]);
                }
                else
                {
                    list.Add(this.GetYearText(aRowIndex));
                }
            }
            return list;
        }

        private DateTimePickerRow GetYearText(int aRowIndex)
        {
            return this.m_Parent.GetYearText(aRowIndex);
        }

        private void InitTouchTool()
        {
            if (this.m_TouchNavigatorTool == null)
            {
                this.m_TouchNavigatorTool = new TouchScrolling.TouchTool(this);
                this.m_TouchNavigatorTool.MouseMoveEnded += new EventHandler(this.TouchNavigatorTool_MouseMoveEnded);
                this.m_TouchNavigatorTool.MouseMoveDetected += new TouchScrolling.TouchTool.MouseMoveDetectedHandler(this.TouchNavigatorTool_MouseMoveDetected);
            }
        }

        internal void Invalidate()
        {
            this.OnRefreshRequired();
        }

        private void MouseClickUp(MouseEventArgs e)
        {
            this.AdjustSelectedItem(this.m_ScrollDirection, true);
        }

        internal void OnMouseDown(MouseEventArgs e)
        {
            this.StopAdjustCursor();
            this.m_TouchNavigatorTool.MouseDown(e.X, e.Y);
        }

        internal void OnMouseMove(MouseEventArgs e)
        {
            this.m_TouchNavigatorTool.MouseMove(e.X, e.Y);
        }

        internal void OnMouseUp(MouseEventArgs e)
        {
            if (!this.m_TouchNavigatorTool.MouseUp(e.X, e.Y))
            {
                this.MouseClickUp(e);
            }
        }

        protected virtual void OnRefreshRequired()
        {
            if (this.RefreshRequired != null)
            {
                this.RefreshRequired(this, new RollerItemEventArgs(this));
            }
        }

        internal virtual void OnSelectedIndexChanged()
        {
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(this, new RollerItemEventArgs(this));
            }
        }

        private void QuickAdjustSelectedItem()
        {
            int cursorDifY = (int) (4f * m_ScaleFactor.Height);
            do
            {
                if (cursorDifY > this.m_CursorDifY)
                {
                    cursorDifY = this.m_CursorDifY;
                }
                this.ScrollbarOffset += this.m_ScrollDirection * cursorDifY;
                this.Refresh();
                this.m_CursorDifY -= cursorDifY;
            }
            while (this.m_CursorDifY > 0);
            if (this.ValidSelectedIndex())
            {
                this.Refresh();
                this.OnSelectedIndexChanged();
            }
        }

        private void Refresh()
        {
            if (this.m_Parent != null)
            {
                this.m_Parent.ForceRefresh();
            }
            else
            {
                this.Invalidate();
            }
        }

        internal void SetSelectedIndexWithoutInvalidate(int aSelectedIndex)
        {
            if (this.m_SelectedIndex != aSelectedIndex)
            {
                this.m_SelectedIndex = aSelectedIndex;
                this.ValidSelectedIndex();
            }
        }

        private void StopAdjustCursor()
        {
            this.m_AdjustSelectedItemTimer.Enabled = false;
            this.m_CursorDifY = 0;
        }

        private void TouchNavigatorTool_MouseMoveDetected(object sender, TouchScrolling.TouchTool.MouseMoveEventArgs e)
        {
            this.ScrollbarOffset += e.MoveY;
            this.m_ScrollDirection = (e.MoveY >= 0) ? 1 : -1;
            this.Refresh();
        }

        private void TouchNavigatorTool_MouseMoveEnded(object sender, EventArgs e)
        {
            this.AdjustSelectedItem(this.m_ScrollDirection, false);
        }

        internal bool ValidSelectedIndex()
        {
            bool flag = false;
            while (((this.m_SelectedIndex >= 0) && (this.m_SelectedIndex < this.m_Items.Count)) && !this.m_Items[this.m_SelectedIndex].Enabled)
            {
                this.m_SelectedIndex--;
                this.m_ScrollbarOffset = this.m_SelectedIndex * this.m_RowHeight;
                flag = true;
            }
            if (this.m_RollerDataType != RollerType.Year)
            {
                if (this.m_SelectedIndex < 0)
                {
                    this.m_SelectedIndex = 0;
                    flag = true;
                }
                if (this.m_SelectedIndex >= this.MaxIndex)
                {
                    this.m_SelectedIndex = this.MaxIndex - 1;
                    flag = true;
                }
            }
            return flag;
        }

        internal Rectangle ClientRectangle
        {
            get
            {
                Rectangle clientRectangle = this.m_ClientRectangle;
                clientRectangle.Width = (int) (clientRectangle.Width * m_ScaleFactor.Width);
                return clientRectangle;
            }
            set
            {
                this.m_ClientRectangle = value;
            }
        }

        public bool Continues
        {
            get
            {
                return this.m_Continues;
            }
            set
            {
                this.m_Continues = value;
            }
        }

        public int Height
        {
            get
            {
                return this.m_ClientRectangle.Height;
            }
            internal set
            {
                if (this.m_ClientRectangle.Height != value)
                {
                    this.m_ClientRectangle.Height = value;
                    this.m_RowHeight = value / 5;
                    this.m_ScrollbarOffset = this.m_SelectedIndex * this.m_RowHeight;
                    this.m_TextOffsetY = (this.m_ClientRectangle.Height - (this.RowHeight * 5)) / 2;
                }
            }
        }

        internal bool IsDisposed
        {
            get
            {
                return this.m_IsDisposed;
            }
        }

        public Point Location
        {
            get
            {
                return this.m_ClientRectangle.Location;
            }
            internal set
            {
                this.m_ClientRectangle.Location = value;
            }
        }

        public int MaxIndex
        {
            get
            {
                if (this.m_RollerDataType == RollerType.Year)
                {
                    this.m_MaxIndex = (this.m_Parent.GetMaxDate().Year - this.m_Parent.GetMinDate().Year) + 1;
                }
                else
                {
                    this.m_MaxIndex = this.m_Items.Count;
                }
                return this.m_MaxIndex;
            }
            internal set
            {
                this.m_MaxIndex = value;
            }
        }

        internal RollerType RollerDataType
        {
            get
            {
                return this.m_RollerDataType;
            }
            set
            {
                this.m_RollerDataType = value;
            }
        }

        internal int RowHeight
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

        internal List<DateTimePickerRow> RowItems
        {
            get
            {
                return this.m_Items;
            }
        }

        internal int ScrollbarOffset
        {
            get
            {
                return this.m_ScrollbarOffset;
            }
            set
            {
                int num;
                if (this.m_Continues)
                {
                    num = (this.MaxIndex * this.m_RowHeight) - 1;
                }
                else
                {
                    num = (this.MaxIndex - 1) * this.m_RowHeight;
                }
                if (value < 0)
                {
                    if (!this.m_Continues)
                    {
                        value = 0;
                    }
                    else
                    {
                        value = value % num;
                        if (value < 0)
                        {
                            value += num;
                        }
                    }
                }
                if (value > num)
                {
                    if (!this.m_Continues)
                    {
                        value = num;
                    }
                    else
                    {
                        value = value % num;
                    }
                }
                if (this.m_ScrollbarOffset != value)
                {
                    this.m_ScrollbarOffset = value;
                    this.m_SelectedIndex = this.m_ScrollbarOffset / this.m_RowHeight;
                    this.OnSelectedIndexChanged();
                }
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.m_SelectedIndex;
            }
            set
            {
                if (this.m_SelectedIndex != value)
                {
                    this.m_SelectedIndex = value;
                    this.ValidSelectedIndex();
                    this.Invalidate();
                    this.OnSelectedIndexChanged();
                }
            }
        }

        public string SelectedText
        {
            get
            {
                if (this.m_RollerDataType == RollerType.Year)
                {
                    return this.GetYearText(this.m_SelectedIndex).Text;
                }
                if (((this.m_Items != null) && (this.m_SelectedIndex < this.m_Items.Count)) && (this.m_SelectedIndex >= 0))
                {
                    return this.m_Items[this.m_SelectedIndex].Text;
                }
                return string.Empty;
            }
        }

        public Font TextFont
        {
            get
            {
                return this.m_TextFont;
            }
            set
            {
                this.m_TextFont = value;
                this.Invalidate();
            }
        }

        internal int TextOffsetY
        {
            get
            {
                return this.m_TextOffsetY;
            }
            set
            {
                this.m_TextOffsetY = value;
            }
        }

        public int Width
        {
            get
            {
                return (int) (this.m_ClientRectangle.Width * m_ScaleFactor.Width);
            }
            set
            {
                this.m_ClientRectangle.Width = value;
                this.Invalidate();
            }
        }

        public delegate void SelectedIndexChangedEventHandler(object sender, RollerItemEventArgs e);
    }
}

