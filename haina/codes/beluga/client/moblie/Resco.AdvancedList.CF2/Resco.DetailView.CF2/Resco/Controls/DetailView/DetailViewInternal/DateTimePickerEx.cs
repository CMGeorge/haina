namespace Resco.Controls.DetailView.DetailViewInternal
{
    using Resco.Controls.DetailView;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DateTimePickerEx : Control
    {
        private Point[] m_arrowPoints = new Point[3];
        private Bitmap m_bmp;
        private bool m_border = true;
        private SolidBrush m_brushBack;
        private SolidBrush m_brushDisabled;
        private SolidBrush m_brushFore;
        private SolidBrush m_brushFrame;
        private SolidBrush m_brushSelected;
        private SolidBrush m_brushSelectedText;
        private bool m_bTimePickerDirty;
        private string m_customFormat = "";
        private MonthCalendarEx m_dayPicker = new MonthCalendarEx();
        private bool m_doParse = true;
        private Point[] m_downPoints = new Point[3];
        private bool m_downPressed;
        private LeftRightAligment m_dropDownAlign;
        private DateTime m_endTime = new DateTime(1, 1, 1, 0x17, 0x3b, 0);
        private Resco.Controls.DetailView.DateTimePickerFormat m_format = Resco.Controls.DetailView.DateTimePickerFormat.Long;
        private Graphics m_graphics;
        private double m_minuteInterval = 30.0;
        private Pen m_penFrame;
        private float m_scaleFactor = 1f;
        private bool m_showDropDown = true;
        private bool m_showTimeNone;
        private bool m_showUpDown;
        private DateTime m_startTime = new DateTime(1, 1, 1, 0, 0, 0);
        private string m_TextCached = "";
        private ArrayList m_TextParts = new ArrayList();
        private int m_TextSelected = -1;
        private ListBoxEx m_timePicker = new ListBoxEx();
        private Point[] m_upPoints = new Point[3];
        private bool m_upPressed;
        public static readonly DateTime MaxDateTime = new DateTime(0x270e, 12, 0x1f);
        public static readonly DateTime MinDateTime = new DateTime(0x6d9, 1, 1);

        public event EventHandler CloseUp;

        public event EventHandler DropDown;

        public event EventHandler NoneSelected;

        public event EventHandler ValueChanged;

        internal DateTimePickerEx()
        {
            this.m_scaleFactor = base.CreateGraphics().DpiX / 96f;
            this.m_dayPicker.Visible = false;
            this.m_dayPicker.m_doCloseUp = true;
            this.m_dayPicker.CloseUp += new EventHandler(this.OnDayPickerCloseUp);
            this.m_dayPicker.ValueChanged += new EventHandler(this.OnDayPickerValueChanged);
            this.m_dayPicker.NoneSelected += new EventHandler(this.OnDayPickerNoneSelected);
            this.m_timePicker.Visible = false;
            this.m_timePicker.KeyUp += new KeyEventHandler(this.OnTimePickerKeyUp);
            this.m_timePicker.Click += new EventHandler(this.OnTimePickerClick);
            this.m_timePicker.SelectedIndexChanged += new EventHandler(this.OnTimePickerSelectedIndexChanged);
            this.m_timePicker.LostFocus += new EventHandler(this.OnTimePickerLostFocus);
            this.FillTimePicker();
            this.Format = Const.DefaultFormat;
            this.Value = Const.DefaultStartDate;
            base.Size = Const.DefaultDateTimePickerSize;
            this.Font = new System.Drawing.Font("Tahoma", 8f, FontStyle.Regular);
        }

        internal void _OnLostFocus(EventArgs e)
        {
            this.OnLostFocus(e);
        }

        private void CreateGdiObjects()
        {
            if (this.m_brushFrame == null)
            {
                this.m_brushFrame = new SolidBrush(SystemColors.WindowFrame);
            }
            if (this.m_penFrame == null)
            {
                this.m_penFrame = new Pen(SystemColors.WindowFrame);
            }
            if ((this.m_brushFore == null) || (this.m_brushFore.Color != this.ForeColor))
            {
                this.m_brushFore = new SolidBrush(this.ForeColor);
            }
            if ((this.m_brushBack == null) || (this.m_brushBack.Color != this.BackColor))
            {
                this.m_brushBack = new SolidBrush(this.BackColor);
            }
            if (this.m_brushDisabled == null)
            {
                this.m_brushDisabled = new SolidBrush(SystemColors.GrayText);
            }
            if (this.m_brushSelected == null)
            {
                this.m_brushSelected = new SolidBrush(SystemColors.Highlight);
            }
            if (this.m_brushSelectedText == null)
            {
                this.m_brushSelectedText = new SolidBrush(SystemColors.HighlightText);
            }
        }

        private void CreateMemoryBitmap()
        {
            if (((this.m_bmp == null) || (this.m_bmp.Width != base.Width)) || (this.m_bmp.Height != base.Height))
            {
                this.m_bmp = new Bitmap(base.Width, base.Height);
                this.m_graphics = Graphics.FromImage(this.m_bmp);
                this.m_arrowPoints[0].X = (base.Width - Const.DropArrowSize.Width) - 4;
                this.m_arrowPoints[0].Y = ((base.Height - Const.DropArrowSize.Height) + 1) / 2;
                this.m_arrowPoints[1].X = this.m_arrowPoints[0].X + Const.DropArrowSize.Width;
                this.m_arrowPoints[1].Y = this.m_arrowPoints[0].Y;
                this.m_arrowPoints[2].X = this.m_arrowPoints[0].X + (Const.DropArrowSize.Width / 2);
                this.m_arrowPoints[2].Y = this.m_arrowPoints[0].Y + Const.DropArrowSize.Height;
                this.m_downPoints[0].X = (base.Width - Const.DropArrowSize.Width) - 3;
                this.m_downPoints[0].Y = (base.Height / 2) + (((base.Height / 2) - Const.DropArrowSize.Height) / 2);
                this.m_downPoints[1].X = this.m_downPoints[0].X + Const.DropArrowSize.Width;
                this.m_downPoints[1].Y = this.m_downPoints[0].Y;
                this.m_downPoints[2].X = this.m_downPoints[0].X + (Const.DropArrowSize.Width / 2);
                this.m_downPoints[2].Y = this.m_downPoints[0].Y + Const.DropArrowSize.Height;
                this.m_upPoints[0].X = (base.Width - Const.DropArrowSize.Width) - 4;
                this.m_upPoints[0].Y = (((base.Height / 2) - Const.DropArrowSize.Height) / 2) + Const.DropArrowSize.Height;
                this.m_upPoints[1].X = (this.m_upPoints[0].X + Const.DropArrowSize.Width) + 2;
                this.m_upPoints[1].Y = this.m_upPoints[0].Y;
                this.m_upPoints[2].X = (this.m_upPoints[0].X + (Const.DropArrowSize.Width / 2)) + 1;
                this.m_upPoints[2].Y = (this.m_upPoints[0].Y - Const.DropArrowSize.Height) - 1;
            }
        }

        private void DateIncDec(int direction)
        {
            if ((this.m_TextSelected >= 0) && (this.m_TextSelected < this.m_TextParts.Count))
            {
                TextPart part = (TextPart) this.m_TextParts[this.m_TextSelected];
                DateTime valueInternal = this.GetValueInternal();
                if ((part.Format.IndexOf("h") >= 0) || (part.Format.IndexOf("H") >= 0))
                {
                    this.Value = valueInternal.AddHours((double) direction);
                }
                else if (part.Format.IndexOf("m") >= 0)
                {
                    this.Value = valueInternal.AddMinutes((double) direction);
                }
                else if (part.Format.IndexOf("d") >= 0)
                {
                    this.Value = valueInternal.AddDays((double) direction);
                }
                else if (part.Format.IndexOf("M") >= 0)
                {
                    this.Value = valueInternal.AddMonths(direction);
                }
                else if (part.Format.IndexOf("y") >= 0)
                {
                    this.Value = valueInternal.AddYears(direction);
                }
                else if (part.Format.IndexOf("s") >= 0)
                {
                    this.Value = valueInternal.AddSeconds((double) direction);
                }
                else if (part.Format.IndexOf("t") >= 0)
                {
                    this.Value = valueInternal.AddHours((double) (12 * direction));
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_TextParts != null)
                {
                    this.m_TextParts.Clear();
                }
                this.m_TextParts = null;
                if (this.m_timePicker != null)
                {
                    this.m_timePicker.Dispose();
                }
                this.m_timePicker = null;
                if (this.m_dayPicker != null)
                {
                    this.m_dayPicker.Dispose();
                }
                this.m_dayPicker = null;
            }
            base.Dispose(disposing);
        }

        private void FillTimePicker()
        {
            if (this.m_minuteInterval == 0.0)
            {
                this.m_minuteInterval = 30.0;
            }
            if (this.m_endTime < this.m_startTime)
            {
                this.m_endTime = this.m_startTime;
            }
            this.m_timePicker.Items.Clear();
            if (this.ShowTimeNone && this.ShowCheckBox)
            {
                this.m_timePicker.Items.Add(this.NoneText);
            }
            for (DateTime time = this.m_startTime; time <= this.m_endTime; time = time.AddMinutes(this.m_minuteInterval))
            {
                string str = this.TextParse(base.CreateGraphics(), time);
                this.m_timePicker.Items.Add(str);
            }
        }

        private DateTime GetValueInternal()
        {
            return this.m_dayPicker.Value;
        }

        public void Hide()
        {
            if ((this.m_dayPicker != null) && this.m_dayPicker.Visible)
            {
                this.m_dayPicker.Hide();
            }
            if ((this.m_timePicker != null) && this.m_timePicker.Visible)
            {
                this.m_timePicker.Hide();
            }
            base.Hide();
        }

        protected virtual void OnCloseUp(EventArgs e)
        {
            if (this.CloseUp != null)
            {
                this.CloseUp(this, e);
            }
        }

        private void OnDayPickerCloseUp(object sender, EventArgs e)
        {
            if (this.CloseUp != null)
            {
                this.CloseUp(this, EventArgs.Empty);
            }
        }

        private void OnDayPickerNoneSelected(object sender, EventArgs e)
        {
            base.Invalidate();
            if (this.NoneSelected != null)
            {
                this.NoneSelected(this, e);
            }
        }

        private void OnDayPickerValueChanged(object sender, EventArgs e)
        {
            base.Invalidate();
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        protected virtual void OnDropDown(EventArgs e)
        {
            if (this.DropDown != null)
            {
                this.DropDown(this, e);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    this.SelectPrevious();
                    break;

                case Keys.Up:
                    this.DateIncDec(1);
                    break;

                case Keys.Right:
                    this.SelectNext();
                    break;

                case Keys.Down:
                    this.DateIncDec(-1);
                    break;
            }
            if (this.m_TextSelected < 0)
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            int v = 0;
            int num2 = 0;
            if ((this.m_TextSelected < 0) || (this.m_TextSelected >= this.m_TextParts.Count))
            {
                base.OnKeyPress(e);
            }
            else
            {
                if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
                {
                    num2 = e.KeyChar - '0';
                }
                else
                {
                    return;
                }
                TextPart p = (TextPart) this.m_TextParts[this.m_TextSelected];
                DateTime valueInternal = this.GetValueInternal();
                if ((p.Format.IndexOf("h") >= 0) || (p.Format.IndexOf("H") >= 0))
                {
                    v = valueInternal.Hour;
                }
                else if (p.Format.IndexOf("m") >= 0)
                {
                    v = valueInternal.Minute;
                }
                else if (p.Format.IndexOf("d") >= 0)
                {
                    v = valueInternal.Day;
                }
                else if (p.Format.IndexOf("M") >= 0)
                {
                    v = valueInternal.Month;
                }
                else
                {
                    if (p.Format.IndexOf("y") >= 0)
                    {
                        this.m_doParse = false;
                        string str = p.Text + num2;
                        if (str.Length > p.Format.Length)
                        {
                            str = str.Substring(1, str.Length - 1);
                        }
                        p.Text = str;
                        v = valueInternal.Year;
                        base.Invalidate();
                        return;
                    }
                    if (p.Format.IndexOf("s") >= 0)
                    {
                        v = valueInternal.Second;
                    }
                }
                v = (v * 10) + num2;
                try
                {
                    valueInternal = this.SetKeyDate(p, v, valueInternal);
                }
                catch
                {
                    try
                    {
                        string str2 = v.ToString();
                        if (str2.Length > 1)
                        {
                            v = Convert.ToInt32(str2.Remove(0, 1));
                        }
                        valueInternal = this.SetKeyDate(p, v, valueInternal);
                    }
                    catch
                    {
                        try
                        {
                            valueInternal = this.SetKeyDate(p, num2, valueInternal);
                        }
                        catch
                        {
                            return;
                        }
                    }
                }
                this.Value = valueInternal;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if ((this.m_format == Resco.Controls.DetailView.DateTimePickerFormat.Time) || (this.m_format == Resco.Controls.DetailView.DateTimePickerFormat.CustomTime))
                {
                    this.ShowTimePicker();
                }
                else
                {
                    this.ShowDayPicker();
                }
            }
            if (this.m_TextSelected < 0)
            {
                base.OnKeyUp(e);
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (((this.m_dayPicker == null) || !this.m_dayPicker.Focused) && ((this.m_timePicker == null) || !this.m_timePicker.Focused))
            {
                this.ValidateYear();
                this.m_TextSelected = -1;
                base.Invalidate();
                base.OnLostFocus(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.ValidateYear();
            bool visible = this.m_dayPicker.Visible;
            bool flag2 = this.m_timePicker.Visible;
            base.Focus();
            Rectangle rectangle = new Rectangle(base.Width - Const.DropDownWidth, 0, Const.DropDownWidth, base.Height);
            if (rectangle.Contains(e.X, e.Y))
            {
                if (this.m_showDropDown)
                {
                    if (this.m_showUpDown)
                    {
                        this.m_upPressed = false;
                        this.m_downPressed = false;
                        if (e.Y < (base.Height / 2))
                        {
                            this.m_upPressed = true;
                        }
                        else
                        {
                            this.m_downPressed = true;
                        }
                        base.Invalidate();
                    }
                    else if ((this.m_format != Resco.Controls.DetailView.DateTimePickerFormat.Time) && (this.m_format != Resco.Controls.DetailView.DateTimePickerFormat.CustomTime))
                    {
                        if (!visible)
                        {
                            this.ShowDayPicker();
                        }
                    }
                    else if (!flag2)
                    {
                        this.ShowTimePicker();
                    }
                }
            }
            else if (!this.m_dayPicker.IsNone)
            {
                int x = 4;
                foreach (TextPart part in this.m_TextParts)
                {
                    rectangle = new Rectangle(x, 0, part.Size.Width, base.Height);
                    if (rectangle.Contains(e.X, e.Y) && part.IsEditable)
                    {
                        this.m_TextSelected = this.m_TextParts.IndexOf(part);
                        base.Invalidate();
                        return;
                    }
                    x += part.Size.Width;
                }
                this.m_TextSelected = -1;
                base.Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Rectangle rectangle = new Rectangle(base.Width - Const.DropDownWidth, 0, Const.DropDownWidth, base.Height);
            if ((this.m_showDropDown && this.m_showUpDown) && rectangle.Contains(e.X, e.Y))
            {
                if ((e.Y < (base.Height / 2)) && this.m_upPressed)
                {
                    this.DateIncDec(1);
                }
                else if ((e.Y > (base.Height / 2)) && this.m_downPressed)
                {
                    this.DateIncDec(-1);
                }
            }
            this.m_upPressed = false;
            this.m_downPressed = false;
            base.Invalidate();
        }

        protected virtual void OnNoneSelected(EventArgs e)
        {
            if (this.NoneSelected != null)
            {
                this.NoneSelected(this, e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.CreateMemoryBitmap();
            this.CreateGdiObjects();
            this.m_graphics.Clear(this.BackColor);
            if (this.m_doParse)
            {
                this.m_TextCached = this.TextParse(this.m_graphics, this.GetValueInternal());
            }
            int x = 4;
            if (this.m_dayPicker.IsNone)
            {
                Size size = this.m_graphics.MeasureString(this.m_dayPicker.NoneText, this.Font).ToSize();
                this.m_graphics.DrawString(this.m_dayPicker.NoneText, this.Font, base.Enabled ? this.m_brushFore : this.m_brushDisabled, (float) x, (float) ((base.Height - size.Height) / 2));
            }
            else
            {
                foreach (TextPart part in this.m_TextParts)
                {
                    if (this.m_TextSelected == this.m_TextParts.IndexOf(part))
                    {
                        this.m_graphics.FillRectangle(this.m_brushSelected, x, (base.Height - part.Size.Height) / 2, part.Size.Width, part.Size.Height);
                        this.m_graphics.DrawString(part.Text, this.Font, base.Enabled ? this.m_brushSelectedText : this.m_brushDisabled, (float) x, (float) ((base.Height - part.Size.Height) / 2));
                    }
                    else
                    {
                        this.m_graphics.DrawString(part.Text, this.Font, base.Enabled ? this.m_brushFore : this.m_brushDisabled, (float) x, (float) ((base.Height - part.Size.Height) / 2));
                    }
                    x += part.Size.Width;
                }
            }
            if (this.m_showDropDown)
            {
                if (!this.m_showUpDown)
                {
                    this.m_graphics.FillPolygon(this.m_brushFrame, this.m_arrowPoints);
                }
                else
                {
                    if (this.m_upPressed)
                    {
                        this.m_graphics.FillRectangle(this.m_brushFrame, base.Width - Const.DropDownWidth, 0, Const.DropDownWidth, base.Height / 2);
                    }
                    else
                    {
                        this.m_graphics.DrawRectangle(this.m_penFrame, base.Width - Const.DropDownWidth, 0, Const.DropDownWidth - 1, (base.Height - 1) / 2);
                    }
                    if (this.m_downPressed)
                    {
                        this.m_graphics.FillRectangle(this.m_brushFrame, base.Width - Const.DropDownWidth, base.Height / 2, Const.DropDownWidth, base.Height / 2);
                    }
                    else
                    {
                        this.m_graphics.DrawRectangle(this.m_penFrame, (int) (base.Width - Const.DropDownWidth), (int) (base.Height / 2), (int) (Const.DropDownWidth - 1), (int) ((base.Height - 1) / 2));
                    }
                    this.m_graphics.FillPolygon(this.m_upPressed ? this.m_brushBack : this.m_brushFrame, this.m_upPoints);
                    this.m_graphics.FillPolygon(this.m_downPressed ? this.m_brushBack : this.m_brushFrame, this.m_downPoints);
                }
            }
            if (this.m_border)
            {
                this.m_graphics.DrawRectangle(this.m_penFrame, 0, 0, base.Width - 1, base.Height - 1);
            }
            e.Graphics.DrawImage(this.m_bmp, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            base.Invalidate();
        }

        private void OnTimePickerClick(object sender, EventArgs e)
        {
            this.UpdateTime();
            base.Focus();
            this.m_timePicker.Hide();
            this.OnCloseUp(EventArgs.Empty);
        }

        private void OnTimePickerKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.UpdateTime();
                base.Focus();
                this.m_timePicker.Hide();
                this.OnCloseUp(EventArgs.Empty);
            }
        }

        private void OnTimePickerLostFocus(object sender, EventArgs e)
        {
            this.m_timePicker.Hide();
            base.Focus();
            this.OnCloseUp(e);
        }

        private void OnTimePickerSelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateTime();
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        public void SelectNext()
        {
            if (!this.m_dayPicker.IsNone)
            {
                this.ValidateYear();
                do
                {
                    this.m_TextSelected++;
                    if (this.m_TextSelected >= this.m_TextParts.Count)
                    {
                        this.m_TextSelected = -1;
                        break;
                    }
                }
                while (!((TextPart) this.m_TextParts[this.m_TextSelected]).IsEditable);
                base.Invalidate();
            }
        }

        public void SelectPrevious()
        {
            if (this.m_dayPicker.IsNone)
            {
                return;
            }
            this.ValidateYear();
        Label_0014:
            if (this.m_TextSelected < 0)
            {
                this.m_TextSelected = this.m_TextParts.Count - 1;
            }
            else
            {
                this.m_TextSelected--;
            }
            if (this.m_TextSelected < 0)
            {
                this.m_TextSelected = -1;
            }
            else if (!((TextPart) this.m_TextParts[this.m_TextSelected]).IsEditable)
            {
                goto Label_0014;
            }
            base.Invalidate();
        }

        private DateTime SetKeyDate(TextPart p, int v, DateTime date)
        {
            if ((p.Format.IndexOf("h") >= 0) || (p.Format.IndexOf("H") >= 0))
            {
                return new DateTime(date.Year, date.Month, date.Day, v, date.Minute, date.Second);
            }
            if (p.Format.IndexOf("m") >= 0)
            {
                return new DateTime(date.Year, date.Month, date.Day, date.Hour, v, date.Second);
            }
            if (p.Format.IndexOf("d") >= 0)
            {
                return new DateTime(date.Year, date.Month, v, date.Hour, date.Minute, date.Second);
            }
            if (p.Format.IndexOf("M") >= 0)
            {
                return new DateTime(date.Year, v, date.Day, date.Hour, date.Minute, date.Second);
            }
            if (p.Format.IndexOf("s") >= 0)
            {
                return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, v);
            }
            return date;
        }

        private void ShowDayPicker()
        {
            if (this.m_dayPicker.Parent == null)
            {
                base.TopLevelControl.Controls.Add(this.m_dayPicker);
            }
            Point point = new Point(base.Left, base.Bottom + 1);
            Point point2 = base.Parent.PointToScreen(base.Parent.Location);
            Point point3 = base.TopLevelControl.PointToScreen(base.Parent.Location);
            if (this.m_dropDownAlign == LeftRightAligment.Left)
            {
                point.Offset(point2.X - point3.X, point2.Y - point3.Y);
            }
            else
            {
                point.Offset(((point2.X - point3.X) + base.Width) - this.m_dayPicker.Width, point2.Y - point3.Y);
            }
            if ((point.Y + this.m_dayPicker.Size.Height) > base.TopLevelControl.ClientRectangle.Height)
            {
                point.Y -= (base.Height + this.m_dayPicker.Size.Height) + 2;
                if (point.Y < 0)
                {
                    point.Y = base.TopLevelControl.ClientRectangle.Height - this.m_dayPicker.Size.Height;
                }
            }
            if ((point.X + this.m_dayPicker.Size.Width) > base.TopLevelControl.ClientRectangle.Width)
            {
                point.X = base.TopLevelControl.ClientRectangle.Width - this.m_dayPicker.Size.Width;
            }
            this.m_dayPicker.Display(!this.m_dayPicker.Visible, point.X, point.Y, this.BackColor, this.ForeColor, this);
            if (this.m_dayPicker.Visible && (this.DropDown != null))
            {
                this.DropDown(this, EventArgs.Empty);
            }
            if (!this.m_dayPicker.Visible && (this.CloseUp != null))
            {
                this.CloseUp(this, EventArgs.Empty);
            }
        }

        private void ShowTimePicker()
        {
            if (this.m_bTimePickerDirty)
            {
                this.FillTimePicker();
            }
            this.m_bTimePickerDirty = false;
            this.m_timePicker.Width = base.Size.Width;
            this.m_timePicker.Height = (int) (86f * this.m_scaleFactor);
            if (this.m_timePicker.Parent == null)
            {
                base.TopLevelControl.Controls.Add(this.m_timePicker);
            }
            Point p = new Point(base.Left, base.Bottom + 1);
            Point point2 = base.Parent.PointToScreen(base.Parent.Location);
            Point point3 = base.TopLevelControl.PointToScreen(base.Parent.Location);
            p.Offset(point2.X - point3.X, point2.Y - point3.Y);
            if ((p.Y + this.m_timePicker.Size.Height) > base.TopLevelControl.ClientRectangle.Height)
            {
                p.Y -= (base.ClientSize.Height + this.m_timePicker.Size.Height) + 2;
                if (p.Y < 0)
                {
                    p.Y = base.TopLevelControl.ClientRectangle.Height - this.m_timePicker.Size.Height;
                }
            }
            if ((base.TopLevelControl.PointToScreen(p).Y + this.m_timePicker.Size.Height) > (Screen.PrimaryScreen.WorkingArea.Y + Screen.PrimaryScreen.WorkingArea.Height))
            {
                Point point4 = new Point(p.X, (Screen.PrimaryScreen.WorkingArea.Top + Screen.PrimaryScreen.WorkingArea.Height) - this.m_timePicker.Size.Height);
                p.Y = base.TopLevelControl.PointToClient(point4).Y;
                if (p.Y < 0)
                {
                    p.Y = 0;
                }
            }
            if ((p.X + this.m_timePicker.Size.Width) > base.TopLevelControl.ClientRectangle.Width)
            {
                p.X = base.TopLevelControl.ClientRectangle.Width - this.m_timePicker.Size.Width;
            }
            this.m_timePicker.Location = p;
            DateTime now = DateTime.Now;
            try
            {
                this.m_TextCached = this.TextParse((this.m_graphics == null) ? base.CreateGraphics() : this.m_graphics, this.GetValueInternal());
                now = Convert.ToDateTime(this.Text);
            }
            catch
            {
            }
            if (this.ShowTimeNone && !this.Checked)
            {
                this.m_timePicker.SelectedIndex = 0;
                this.m_timePicker.EnsureVisible(0);
            }
            else
            {
                foreach (string str in this.m_timePicker.Items)
                {
                    if (((!this.ShowTimeNone || !this.ShowCheckBox) || (str != this.NoneText)) && (Convert.ToDateTime(str) >= now))
                    {
                        int index = this.m_timePicker.Items.IndexOf(str);
                        this.m_timePicker.SelectedIndex = index;
                        this.m_timePicker.EnsureVisible(index);
                        break;
                    }
                }
            }
            this.m_timePicker.Show();
            this.m_timePicker.BringToFront();
            this.m_timePicker.Focus();
        }

        private string TextParse(Graphics gr, DateTime value)
        {
            string longDatePattern = "";
            switch (this.m_format)
            {
                case Resco.Controls.DetailView.DateTimePickerFormat.Long:
                    longDatePattern = DateTimeFormatInfo.CurrentInfo.LongDatePattern;
                    break;

                case Resco.Controls.DetailView.DateTimePickerFormat.Short:
                    longDatePattern = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                    break;

                case Resco.Controls.DetailView.DateTimePickerFormat.Time:
                    longDatePattern = DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
                    break;

                default:
                    if (this.m_customFormat != "")
                    {
                        longDatePattern = this.m_customFormat;
                    }
                    else if (this.m_format == Resco.Controls.DetailView.DateTimePickerFormat.CustomTime)
                    {
                        longDatePattern = DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
                    }
                    else
                    {
                        longDatePattern = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                    }
                    break;
            }
            this.m_TextParts = new ArrayList();
            longDatePattern = longDatePattern + '\x00ff';
            char ch = '\x00ff';
            string format = "";
            bool flag = false;
            foreach (char ch2 in longDatePattern)
            {
                if (ch2 == '\'')
                {
                    if (flag)
                    {
                        format = format + '\x00ff';
                    }
                    flag = !flag;
                    if (format != "")
                    {
                        this.m_TextParts.Add(new TextPart(gr, value, format, this.Font));
                        format = "";
                    }
                    if (flag)
                    {
                        format = format + '\x00ff';
                    }
                }
                else if (flag)
                {
                    format = format + ch2;
                }
                else
                {
                    if ((ch2 != ch) && (format != ""))
                    {
                        this.m_TextParts.Add(new TextPart(gr, value, format, this.Font));
                        format = "";
                    }
                    format = format + ch2;
                    ch = ch2;
                }
            }
            string str3 = "";
            foreach (TextPart part in this.m_TextParts)
            {
                str3 = str3 + part.Text;
            }
            return str3;
        }

        private void UpdateTime()
        {
            DateTime time = this.Value;
            try
            {
                if ((this.ShowTimeNone && this.ShowCheckBox) && (this.m_timePicker.SelectedIndex == 0))
                {
                    this.Checked = false;
                }
                else
                {
                    if (this.m_timePicker.SelectedIndex >= 0)
                    {
                        DateTime time2 = Convert.ToDateTime(this.m_timePicker.Items[this.m_timePicker.SelectedIndex]);
                        time = new DateTime(time.Year, time.Month, time.Day, time2.Hour, time2.Minute, time2.Second);
                    }
                    this.Value = time;
                }
            }
            catch (Exception)
            {
            }
        }

        private void ValidateYear()
        {
            if (!this.m_doParse)
            {
                TextPart part = null;
                foreach (TextPart part2 in this.m_TextParts)
                {
                    if (part2.Format.IndexOf("y") >= 0)
                    {
                        part = part2;
                        break;
                    }
                }
                if (part != null)
                {
                    DateTime valueInternal = this.GetValueInternal();
                    int year = valueInternal.Year;
                    try
                    {
                        year = Convert.ToDateTime(string.Concat(new object[] { valueInternal.Month, "-", valueInternal.Day, "-", part.Text, " ", valueInternal.Hour, ":", valueInternal.Minute, ":", valueInternal.Second }), DateTimeFormatInfo.InvariantInfo).Year;
                    }
                    catch
                    {
                    }
                    this.Value = new DateTime(year, valueInternal.Month, valueInternal.Day, valueInternal.Hour, valueInternal.Minute, valueInternal.Second);
                    this.m_doParse = true;
                    base.Invalidate();
                }
            }
            this.m_doParse = true;
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
                    base.Invalidate();
                }
            }
        }

        public bool Checked
        {
            get
            {
                return !this.m_dayPicker.IsNone;
            }
            set
            {
                this.m_dayPicker.IsNone = !value;
            }
        }

        public string CustomFormat
        {
            get
            {
                return this.m_customFormat;
            }
            set
            {
                if (value != this.m_customFormat)
                {
                    this.m_customFormat = value;
                    if (this.m_format == Resco.Controls.DetailView.DateTimePickerFormat.CustomTime)
                    {
                        this.m_bTimePickerDirty = true;
                    }
                    base.Invalidate();
                }
            }
        }

        public string DaysOfWeek
        {
            get
            {
                return this.m_dayPicker.DaysOfWeek;
            }
            set
            {
                this.m_dayPicker.DaysOfWeek = value;
                base.Invalidate();
            }
        }

        public bool DroppedDown
        {
            get
            {
                if (!this.m_dayPicker.Visible)
                {
                    return this.m_timePicker.Visible;
                }
                return true;
            }
            set
            {
                if ((this.m_format == Resco.Controls.DetailView.DateTimePickerFormat.Time) || (this.m_format == Resco.Controls.DetailView.DateTimePickerFormat.CustomTime))
                {
                    if ((value != this.m_timePicker.Visible) && !this.ShowUpDown)
                    {
                        if (value)
                        {
                            this.ShowTimePicker();
                        }
                        else
                        {
                            this.OnTimePickerLostFocus(this, EventArgs.Empty);
                        }
                    }
                }
                else if (value != this.m_dayPicker.Visible)
                {
                    this.ShowDayPicker();
                }
            }
        }

        public Resco.Controls.DetailView.DetailViewInternal.Day FirstDayOfWeek
        {
            get
            {
                return this.m_dayPicker.FirstDayOfWeek;
            }
            set
            {
                this.m_dayPicker.FirstDayOfWeek = value;
                base.Invalidate();
            }
        }

        public override bool Focused
        {
            get
            {
                if (!base.Focused && !this.m_dayPicker.Focused)
                {
                    return this.m_timePicker.Focused;
                }
                return true;
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                if (value != base.Font)
                {
                    base.Font = value;
                }
            }
        }

        public Resco.Controls.DetailView.DateTimePickerFormat Format
        {
            get
            {
                return this.m_format;
            }
            set
            {
                this.m_format = value;
                if ((value == Resco.Controls.DetailView.DateTimePickerFormat.CustomTime) || (value == Resco.Controls.DetailView.DateTimePickerFormat.Time))
                {
                    this.m_bTimePickerDirty = true;
                }
                base.Invalidate();
            }
        }

        public DateTime MaxDate
        {
            get
            {
                return this.m_dayPicker.MaxDate;
            }
            set
            {
                if (value != this.m_dayPicker.MaxDate)
                {
                    this.m_dayPicker.MaxDate = value;
                }
            }
        }

        public DateTime MinDate
        {
            get
            {
                return this.m_dayPicker.MinDate;
            }
            set
            {
                if (value != this.m_dayPicker.MinDate)
                {
                    this.m_dayPicker.MinDate = value;
                }
            }
        }

        public MonthCalendarEx MonthCalendar
        {
            get
            {
                return this.m_dayPicker;
            }
        }

        public string NoneText
        {
            get
            {
                return this.m_dayPicker.NoneText;
            }
            set
            {
                this.m_dayPicker.NoneText = value;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.m_TextSelected;
            }
        }

        public bool ShowCheckBox
        {
            get
            {
                return this.m_dayPicker.ShowNone;
            }
            set
            {
                this.m_dayPicker.ShowNone = value;
            }
        }

        public bool ShowDropDown
        {
            get
            {
                return this.m_showDropDown;
            }
            set
            {
                if (this.m_showDropDown != value)
                {
                    this.m_showDropDown = value;
                    base.Invalidate();
                }
            }
        }

        public bool ShowTimeNone
        {
            get
            {
                return this.m_showTimeNone;
            }
            set
            {
                if (value != this.m_showTimeNone)
                {
                    this.m_showTimeNone = value;
                    base.Invalidate();
                }
            }
        }

        public bool ShowUpDown
        {
            get
            {
                return this.m_showUpDown;
            }
            set
            {
                if (value != this.m_showUpDown)
                {
                    this.m_showUpDown = value;
                    base.Invalidate();
                }
            }
        }

        public override string Text
        {
            get
            {
                return this.m_TextCached;
            }
            set
            {
                this.Value = DateTime.Parse(value);
            }
        }

        public ListBoxEx TimeListBox
        {
            get
            {
                return this.m_timePicker;
            }
        }

        public DateTime TimePickerEndTime
        {
            get
            {
                return this.m_endTime;
            }
            set
            {
                this.m_endTime = new DateTime(1, 1, 1, value.Hour, value.Minute, 0);
                this.m_bTimePickerDirty = true;
            }
        }

        public double TimePickerMinuteInterval
        {
            get
            {
                return this.m_minuteInterval;
            }
            set
            {
                this.m_minuteInterval = value;
                this.m_bTimePickerDirty = true;
            }
        }

        public DateTime TimePickerStartTime
        {
            get
            {
                return this.m_startTime;
            }
            set
            {
                this.m_startTime = new DateTime(1, 1, 1, value.Hour, value.Minute, 0);
                this.m_bTimePickerDirty = true;
            }
        }

        public string TodayText
        {
            get
            {
                return this.m_dayPicker.TodayText;
            }
            set
            {
                this.m_dayPicker.TodayText = value;
            }
        }

        public DateTime Value
        {
            get
            {
                this.ValidateYear();
                return this.m_dayPicker.Value;
            }
            set
            {
                this.m_dayPicker.Value = value;
                base.Invalidate();
            }
        }

        internal class Const
        {
            public static Size DefaultDateTimePickerSize = new Size(200, 20);
            public static Resco.Controls.DetailView.DateTimePickerFormat DefaultFormat = Resco.Controls.DetailView.DateTimePickerFormat.Long;
            public static DateTime DefaultStartDate = DateTime.Today;
            public static Size DropArrowSize = new Size(7, 4);
            public static int DropDownWidth = 13;
        }
    }
}

