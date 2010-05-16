namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class OutlookWeekCalendar : UserControl
    {
        internal Graphics _BackBufferGraphics;
        private Image _BackBufferImage;
        private bool bPressed;
        private IContainer components;
        private ImageList imageListQVGA;
        private ImageList imageListVGA;
        public static int KMaxAllowedParallelAppointments = 30;
        internal ITool m_activeTool;
        private List<CustomAppointment> m_AllDayEvents;
        private AllDayListbox m_AllDayListbox;
        private bool m_allowScroll = true;
        private int m_appointmentGripWidth = 3;
        internal Dictionary<CustomAppointment, AppointmentView> m_AppointmentViews = new Dictionary<CustomAppointment, AppointmentView>();
        private bool m_bDefaultIconsused = true;
        private bool m_bEnableTouchScrolling;
        private Rectangle m_BodyRectangle;
        private Hashtable m_cachedAppointments = new Hashtable();
        private System.Windows.Forms.ContextMenu m_ContextMenu;
        private CursorDirection m_CurDirection;
        private int m_dayHeadersHeight = 20;
        private int m_daysToShow = 1;
        private bool m_DockedToolTip;
        internal DrawTool m_drawTool;
        private bool m_EnableHalfHourSlots = true;
        private bool m_EnableToolTip;
        private int m_halfHourHeight = 15;
        private int m_hourLabelIndent = 1;
        private int m_hourLabelWidth = 20;
        private int m_HourLines = 2;
        private ImageList m_Icons;
        private bool m_IsDisposed;
        private DateTime m_lastEndDate;
        private List<CustomAppointment> m_lastLoadedAppointments;
        private DateTime m_lastStartDate;
        private int m_relaTime;
        private AbstractRenderer m_renderer;
        private bool m_SaturdayIsWorkday;
        private CustomAppointment m_selectedAppointment;
        private Resco.Controls.OutlookControls.SelectionType m_selection;
        private DateTime m_selectionEnd;
        private DateTime m_selectionStart;
        private Timer m_SelectionTimer;
        private DateTime m_startDate;
        private int m_startHour = 8;
        private Resco.Controls.OutlookControls.ControlToolTip m_ToolTip;
        private int m_touchSensitivity = 8;
        private TouchTool m_TouchTool;
        private bool m_useGradient;
        private int m_VgaOffset = -1;
        private DateTime m_workEnd;
        private int m_workingHourEnd = 0x11;
        private int m_workingHourStart = 8;
        private int m_workingMinuteEnd;
        private int m_workingMinuteStart;
        private DateTime m_workStart;
        internal VScrollBar scrollbar;

        public event DrawDayHeaderEventHandler DrawDayHeader;

        public event ResolveAppointmentsEventHandler ResolveAppointments;

        public event SelectionChangedEventHandler SelectionChanged;

        public event EventHandler TouchScrollLeft;

        public event EventHandler TouchScrollRight;

        static OutlookWeekCalendar()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(OutlookWeekCalendar), "");
            //}
        }

        public OutlookWeekCalendar()
        {
            this.InitializeComponent();
            using (Graphics graphics = base.CreateGraphics())
            {
                this.m_VgaOffset = (graphics.DpiX == 192f) ? 2 : 1;
            }
            this.InitScrollBar();
            this.m_drawTool = new DrawTool();
            this.m_drawTool.OutlookWeekCalendarControl = this;
            this.m_TouchTool = new TouchTool();
            this.m_TouchTool.GestureDetected += new TouchTool.GestureDetectedHandler(this.TouchTool_GestureDetected);
            this.m_TouchTool.OutlookWeekCalendarControl = this;
            this.m_activeTool = this.m_TouchTool;
            this.UpdateWorkingHours();
            this.m_useGradient = false;
            this.Renderer = new Office11Renderer();
            if (this.m_Icons != null)
            {
                this.Renderer.AppIcons = this.m_Icons;
            }
            this.InitToolTipControl();
            this.InitAllDayEventControl();
            this.WeekCalendarControl_Resize();
        }

        private int AdjustAllDayControl(List<CustomAppointment> anAppointments)
        {
            if ((this.m_AllDayEvents == null) || (this.m_AllDayEvents.Count == 0))
            {
                this.m_AllDayEvents = this.FilterAlldayEvent(anAppointments);
            }
            if (this.m_daysToShow != 1)
            {
                this.m_AllDayListbox.Visible = false;
                this.AdjustScrollbar();
                return 0;
            }
            if ((this.m_AllDayEvents != null) && (this.m_AllDayEvents.Count > 0))
            {
                this.m_AllDayListbox.CustomAppList = this.m_AllDayEvents;
                this.m_AllDayListbox.Visible = true;
                this.m_AllDayListbox.Invalidate();
                this.AdjustScrollbar();
                return (this.m_AllDayListbox.Height / this.m_AllDayListbox.RowHeight);
            }
            this.AdjustScrollbar();
            this.m_AllDayListbox.Visible = false;
            return 0;
        }

        private void AdjustScrollbar()
        {
            if (this.scrollbar != null)
            {
                this.scrollbar.Maximum = ((this.m_HourLines * 0x18) - this.DisplayedHalfHourCount) + 1;
                this.scrollbar.Minimum = 0;
                int x = base.Width - this.scrollbar.Width;
                this.scrollbar.Location = new Point(x, this.DaysTop);
                int num2 = base.Height - this.DaysTop;
                this.scrollbar.Height = num2;
            }
        }

        private void AllDayListbox_GotFocus(object sender, EventArgs e)
        {
        }

        private void AllDayListbox_LostFocus(object sender, EventArgs e)
        {
            base.Focus();
        }

        private void AllDayListbox_RowEntered(object sender, AllDayListbox.RowEventArgs e)
        {
            this.m_selectedAppointment = this.m_AllDayEvents[e.EnteredRowIndex];
            this.RaiseSelectionChanged(SelectionEventArgs.SelectionType.ByMouse);
        }

        private void AllDayListbox_ToolTipRequired(object sender, AllDayListbox.RowEventArgs e)
        {
            AppointmentView appointmentViewAt = this.GetAppointmentViewAt(e.Mouse.X, e.Mouse.Y + this.HeaderHeight);
            if (appointmentViewAt != null)
            {
                this.ShowToolTip(appointmentViewAt.Appointment, appointmentViewAt.Rectangle, e.Mouse.X, e.Mouse.Y);
            }
        }

        internal static Color BackgroundImageColor(Image image)
        {
            using (Bitmap bitmap = new Bitmap(image))
            {
                return bitmap.GetPixel(0, 0);
            }
        }

        private void ClearBackBuffer()
        {
            if (this._BackBufferGraphics != null)
            {
                this._BackBufferGraphics.Dispose();
                this._BackBufferGraphics = null;
            }
            if (this._BackBufferImage != null)
            {
                this._BackBufferImage.Dispose();
                this._BackBufferImage = null;
            }
        }

        private void CreateBackBuffer()
        {
            this.ClearBackBuffer();
            this._BackBufferImage = new Bitmap(base.Width, base.Height);
            this._BackBufferGraphics = Graphics.FromImage(this._BackBufferImage);
        }

        protected override void Dispose(bool disposing)
        {
            this.m_IsDisposed = true;
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
                if (this.m_TouchTool != null)
                {
                    this.m_TouchTool.Reset();
                }
            }
            base.Dispose(disposing);
        }

        private void DrawAllDayAppointments(Graphics gr, Rectangle rect, DateTime time)
        {
            if ((this.m_daysToShow > 1) && (this.m_AllDayEvents != null))
            {
                bool anIsSelected = false;
                for (int i = 0; i < this.m_AllDayEvents.Count; i++)
                {
                    CustomAppointment app = this.m_AllDayEvents[i];
                    if (app.Start.Date.Equals(time.Date))
                    {
                        Rectangle rectangle2 = rect;
                        rectangle2.Width = 5;
                        Rectangle borderRect = rectangle2;
                        if ((this.m_selectedAppointment != null) && app.Equals(this.m_selectedAppointment))
                        {
                            anIsSelected = true;
                        }
                        this.m_renderer.DrawAllDayAppointment(gr, app, ref borderRect, anIsSelected);
                        AppointmentView view = new AppointmentView();
                        view.Rectangle = rectangle2;
                        view.Appointment = app;
                        this.m_AppointmentViews[app]= view;
                    }
                }
            }
        }

        private void DrawAppointments(Graphics gr, Rectangle rect, DateTime time)
        {
            AppointmentList appointments = (AppointmentList) this.m_cachedAppointments[time.ToShortDateString()];
            if (appointments != null)
            {
                HalfHourLayout[] maxParalelAppointments = this.GetMaxParalelAppointments(appointments);
                List<CustomAppointment> list2 = new List<CustomAppointment>();
                for (int i = 0; i < (0x18 * this.m_HourLines); i++)
                {
                    HalfHourLayout layout = maxParalelAppointments[i];
                    if ((layout != null) && (layout.Count > 0))
                    {
                        for (int j = 0; j < layout.Count; j++)
                        {
                            CustomAppointment appointment = layout.Appointments[j];
                            if (!appointment.m_Drawn)
                            {
                                AppointmentView view;
                                Rectangle baseRectangle = rect;
                                int num3 = rect.Width / appointment.m_ConflictCount;
                                int x = 0;
                                for (int k = 0; k < layout.Appointments.Length; k++)
                                {
                                    CustomAppointment appointment2 = layout.Appointments[k];
                                    if ((appointment2 != null) && this.m_AppointmentViews.ContainsKey(appointment2))
                                    {
                                        view = this.m_AppointmentViews[appointment2];
                                        if (x < view.Rectangle.X)
                                        {
                                            x = view.Rectangle.X;
                                        }
                                    }
                                }
                                if ((x + (num3 * 2)) > (rect.X + rect.Width))
                                {
                                    x = 0;
                                }
                                baseRectangle.Width = num3 - 5;
                                if (x > 0)
                                {
                                    baseRectangle.X = x + num3;
                                }
                                baseRectangle = this.GetHourRangeRectangle(appointment.StartDate, appointment.EndDate, baseRectangle);
                                view = new AppointmentView();
                                view.Rectangle = baseRectangle;
                                view.Appointment = appointment;
                                this.m_AppointmentViews[appointment]=view;
                                Region region = new Region(rect);
                                gr.Clip = region;
                                bool isSelected = false;
                                if ((this.m_selectedAppointment != null) && appointment.Equals(this.m_selectedAppointment))
                                {
                                    isSelected = true;
                                }
                                this.m_renderer.DrawAppointment(gr, baseRectangle, appointment, isSelected, this.AppointmentGripWidth, this.m_HourLines, this.HalfHourHeight);
                                gr.ResetClip();
                                region.Dispose();
                                region = null;
                                list2.Add(appointment);
                                appointment.m_Drawn = true;
                            }
                        }
                    }
                }
            }
        }

        private bool DrawArrow(Graphics gr, Rectangle rect, Arrows arrowType)
        {
            Image image;
            Rectangle rectangle;
            if (this.m_VgaOffset == 2)
            {
                image = this.imageListVGA.Images[(int) arrowType];
            }
            else
            {
                image = this.imageListQVGA.Images[(int) arrowType];
            }
            if (image == null)
            {
                return false;
            }
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorKey(BackgroundImageColor(image), BackgroundImageColor(image));
            if (arrowType == Arrows.Down)
            {
                rectangle = new Rectangle(((rect.X + rect.Width) - image.Width) - 5, (((rect.Y + rect.Height) - image.Height) - this.DaysTop) - 5, image.Width, image.Height);
            }
            else
            {
                rectangle = new Rectangle(((rect.X + rect.Width) - image.Width) - 5, rect.Y + 5, image.Width, image.Height);
            }
            gr.DrawImage(image, rectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            return true;
        }

        private void DrawArrows(Graphics gr, Rectangle rect, DateTime time)
        {
            bool flag = false;
            bool flag2 = false;
            AppointmentList list = (AppointmentList) this.m_cachedAppointments[time.ToShortDateString()];
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (this.m_AppointmentViews.ContainsKey(list[i]))
                    {
                        Rectangle rectangle = this.m_AppointmentViews[list[i]].Rectangle;
                        if (!rect.IntersectsWith(rectangle))
                        {
                            if (rectangle.Y > rect.Y)
                            {
                                if (!flag)
                                {
                                    flag = this.DrawArrow(gr, rect, Arrows.Down);
                                }
                            }
                            else if (!flag2)
                            {
                                flag2 = this.DrawArrow(gr, rect, Arrows.Up);
                            }
                        }
                    }
                }
            }
        }

        private void DrawControl()
        {
            ResolveAppointmentsEventArgs args = new ResolveAppointmentsEventArgs(this.StartDate, this.StartDate.AddDays((double) this.m_daysToShow));
            this.OnResolveAppointments(args);
            int num = this.AdjustAllDayControl(args.Appointments);
            if (this._BackBufferGraphics == null)
            {
                this.CreateBackBuffer();
            }
            Graphics gr = this._BackBufferGraphics;
            if (this.m_renderer != null)
            {
                gr.Clear(this.m_renderer.BackColor);
            }
            Rectangle rectangle = new Rectangle(0, num * this.m_AllDayListbox.RowHeight, base.Width - this.scrollbar.Width, base.Height);
            Rectangle rect = rectangle;
            rect.Y += this.HeaderHeight;
            this.DrawHourLabels(gr, rect);
            Rectangle rectangle3 = rectangle;
            rectangle3.X += this.HourLabelWidth;
            rectangle3.Y += this.HeaderHeight;
            rectangle3.Width -= this.HourLabelWidth;
            Rectangle rectangle4 = new Rectangle((int) gr.ClipBounds.X, (int) gr.ClipBounds.Y, (int) gr.ClipBounds.Width, (int) gr.ClipBounds.Height);
            if (rectangle4.IntersectsWith(rectangle3))
            {
                this.DrawDays(gr, rectangle3);
            }
            Rectangle rectangle5 = new Rectangle(this.HourLabelWidth, 0, (base.Width - this.HourLabelWidth) - this.scrollbar.Width, this.HeaderHeight);
            Rectangle rectangle6 = new Rectangle((int) gr.ClipBounds.X, (int) gr.ClipBounds.Y, (int) gr.ClipBounds.Width, (int) gr.ClipBounds.Height);
            if (rectangle6.IntersectsWith(rectangle5))
            {
                this.DrawDayHeaders(gr, rectangle5);
            }
        }

        private void DrawDay(Graphics gr, Rectangle rect, DateTime time)
        {
            Rectangle rectangle = this.GetHourRangeRectangle(this.m_workStart, this.m_workEnd, rect);
            if (rectangle.Y < this.DaysTop)
            {
                rectangle.Height -= Math.Abs(rectangle.Y) + this.DaysTop;
                rectangle.Y = this.DaysTop;
            }
            Region region = new Region(rect);
            gr.Clip = region;
            if ((time.DayOfWeek == DayOfWeek.Saturday) && !this.m_renderer.BackColor.Equals(this.m_renderer.SaturdayBackColor))
            {
                using (SolidBrush brush = new SolidBrush(this.m_renderer.SaturdayBackColor))
                {
                    gr.FillRectangle(brush, rect);
                }
            }
            if ((time.DayOfWeek == DayOfWeek.Sunday) && !this.m_renderer.BackColor.Equals(this.m_renderer.SundayBackColor))
            {
                using (SolidBrush brush2 = new SolidBrush(this.m_renderer.SundayBackColor))
                {
                    gr.FillRectangle(brush2, rect);
                }
            }
            if (((time.DayOfWeek != DayOfWeek.Saturday) || this.m_SaturdayIsWorkday) && (time.DayOfWeek != DayOfWeek.Sunday))
            {
                this.m_renderer.DrawHourRange(gr, rectangle, false, false);
            }
            if ((this.m_selection == Resco.Controls.OutlookControls.SelectionType.DateRange) && (time.Day == this.m_selectionStart.Day))
            {
                Rectangle rectangle2 = this.GetHourRangeRectangle(this.m_selectionStart, this.m_selectionEnd, rect);
                if (rectangle2.Y >= this.DaysTop)
                {
                    this.m_renderer.DrawHourRange(gr, rectangle2, false, true);
                }
            }
            for (int i = 0; i < (0x18 * this.m_HourLines); i++)
            {
                int num2 = rect.Top + ((i - this.scrollbar.Value) * this.HalfHourHeight);
                using (Pen pen = new Pen(((i % this.m_HourLines) == 0) ? this.m_renderer.HourSeperatorColor : this.m_renderer.HalfHourSeperatorColor))
                {
                    gr.DrawLine(pen, rect.Left, num2, rect.Right, num2);
                }
                if (num2 > rect.Bottom)
                {
                    break;
                }
            }
            this.m_renderer.DrawDayGripper(gr, rect, this.AppointmentGripWidth);
            gr.ResetClip();
            region.Dispose();
            region = null;
            this.DrawAllDayAppointments(gr, rect, time);
            this.DrawAppointments(gr, rect, time);
            this.DrawArrows(gr, rect, time);
        }

        private void DrawDayHeaders(Graphics gr, Rectangle rect)
        {
            int width = rect.Width / this.m_daysToShow;
            Rectangle rectangle = new Rectangle(rect.Left, rect.Top, width, rect.Height);
            DateTime startDate = this.m_startDate;
            for (int i = 0; i < this.m_daysToShow; i++)
            {
                if (this.DrawDayHeader != null)
                {
                    DrawDayHeaderEventArgs args = new DrawDayHeaderEventArgs(this.m_renderer.DayHeader(startDate, rectangle.Width), startDate, rectangle.Width);
                    this.DrawDayHeader(this, args);
                    this.m_renderer.DrawDayHeader(gr, rectangle, args.HeaderText);
                }
                else
                {
                    this.m_renderer.DrawDayHeader(gr, rectangle, startDate);
                }
                rectangle.X += width;
                startDate = startDate.AddDays(1.0);
            }
        }

        private void DrawDays(Graphics gr, Rectangle rect)
        {
            int num = rect.Width / this.m_daysToShow;
            DateTime startDate = this.m_startDate;
            Rectangle rectangle = rect;
            rectangle.Width = num;
            this.m_AppointmentViews.Clear();
            for (int i = 0; i < this.m_daysToShow; i++)
            {
                this.DrawDay(gr, rectangle, startDate);
                rectangle.X += num;
                startDate = startDate.AddDays(1.0);
            }
        }

        private void DrawHourLabels(Graphics gr, Rectangle rect)
        {
            Region region = new Region(rect);
            gr.Clip = region;
            bool selected = false;
            for (int i = 0; i < 0x18; i++)
            {
                Rectangle rectangle = rect;
                rectangle.Y = (rect.Y + ((i * this.m_HourLines) * this.HalfHourHeight)) - (this.scrollbar.Value * this.HalfHourHeight);
                rectangle.X += this.m_hourLabelIndent;
                rectangle.Width = this.HourLabelWidth;
                rectangle.Height = this.HalfHourHeight;
                if (rectangle.Y >= (this.DaysTop / this.m_HourLines))
                {
                    selected = this.IsHourSelected(i, 0);
                    this.m_renderer.DrawHourLabel(gr, rectangle, i, selected);
                }
                if (this.m_EnableHalfHourSlots && (this.m_HourLines == 4))
                {
                    rectangle.Y += this.HalfHourHeight;
                    selected = this.IsHourSelected(i, 15);
                    this.m_renderer.DrawHalfHourLabel(gr, rectangle, "1 5", selected);
                }
                rectangle.Y += this.HalfHourHeight;
                if (this.m_EnableHalfHourSlots && (rectangle.Y > (this.DaysTop / this.m_HourLines)))
                {
                    selected = this.IsHourSelected(i, 30);
                    this.m_renderer.DrawHalfHourLabel(gr, rectangle, "3 0", selected);
                }
                if (this.m_EnableHalfHourSlots && (this.m_HourLines == 4))
                {
                    rectangle.Y += this.HalfHourHeight;
                    selected = this.IsHourSelected(i, 0x2d);
                    this.m_renderer.DrawHalfHourLabel(gr, rectangle, "4 5", selected);
                }
            }
            gr.ResetClip();
            region.Dispose();
            region = null;
        }

        public void EnsureVisible(DateTime aTime, bool aRefresh)
        {
            int num = (aTime.Minute == 30) ? 1 : 0;
            if (this.m_HourLines == 4)
            {
                num = aTime.Minute / 15;
            }
            this.EnsureVisible((int) ((aTime.Hour * this.m_HourLines) + num), aRefresh);
        }

        public void EnsureVisible(int anIndex, bool aRefresh)
        {
            if (anIndex < this.scrollbar.Value)
            {
                this.scrollbar.Value = anIndex;
                if (aRefresh)
                {
                    this.Refresh();
                }
            }
            else if (anIndex >= (this.scrollbar.Value + this.DisplayedHalfHourCount))
            {
                this.scrollbar.Value = (anIndex - this.DisplayedHalfHourCount) + 1;
                if (aRefresh)
                {
                    this.Refresh();
                }
            }
        }

        private List<CustomAppointment> FilterAlldayEvent(List<CustomAppointment> aCustomApps)
        {
            List<CustomAppointment> list = new List<CustomAppointment>();
            for (int i = 0; i < aCustomApps.Count; i++)
            {
                if (aCustomApps[i].AllDayEvent)
                {
                    list.Add(aCustomApps[i]);
                }
            }
            return list;
        }

        public CustomAppointment GetAppointmentAt(int x, int y)
        {
            AppointmentView appointmentViewAt = this.GetAppointmentViewAt(x, y);
            if (appointmentViewAt != null)
            {
                return appointmentViewAt.Appointment;
            }
            return null;
        }

        private AppointmentView GetAppointmentViewAt(int x, int y)
        {
            int daysTop = this.DaysTop;
            if ((this.m_AllDayListbox != null) && this.m_AllDayListbox.Visible)
            {
                daysTop -= this.m_AllDayListbox.Height;
                if (y < daysTop)
                {
                    return null;
                }
                if (y < this.DaysTop)
                {
                    int appointmentAt = this.m_AllDayListbox.GetAppointmentAt(x, y - daysTop);
                    if ((appointmentAt >= 0) && (appointmentAt < this.m_AllDayEvents.Count))
                    {
                        AppointmentView view = new AppointmentView();
                        view.Appointment = this.m_AllDayEvents[appointmentAt];
                        view.Rectangle = this.m_AllDayListbox.GetAppRectAt(x, y - daysTop);
                        return view;
                    }
                }
            }
            else if (y < daysTop)
            {
                return null;
            }
            List<CustomAppointment> list = new List<CustomAppointment>(this.m_AppointmentViews.Keys);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                AppointmentView view2 = this.m_AppointmentViews[list[i]];
                if (view2.Rectangle.Contains(x, y))
                {
                    return view2;
                }
            }
            return null;
        }

        private Rectangle GetHourRangeRectangle(DateTime start, DateTime end, Rectangle baseRectangle)
        {
            Rectangle rectangle = baseRectangle;
            int num = this.HalfHourHeight * ((start.Hour * this.m_HourLines) + (start.Minute / (60 / this.m_HourLines)));
            int num2 = this.HalfHourHeight * (end.Hour * this.m_HourLines);
            if (end.Minute > 0)
            {
                num2 += this.HalfHourHeight;
            }
            if ((end.Minute > 15) && (this.m_HourLines == 4))
            {
                num2 += this.HalfHourHeight;
            }
            if (end.Minute > 30)
            {
                num2 += this.HalfHourHeight;
            }
            if ((end.Minute > 0x2d) && (this.m_HourLines == 4))
            {
                num2 += this.HalfHourHeight;
            }
            rectangle.Y = (num - (this.scrollbar.Value * this.HalfHourHeight)) + this.DaysTop;
            rectangle.Height = num2 - num;
            return rectangle;
        }

        private HalfHourLayout[] GetMaxParalelAppointments(List<CustomAppointment> appointments)
        {
            HalfHourLayout[] layoutArray = new HalfHourLayout[0x18 * this.m_HourLines];
            for (int i = 0; i < appointments.Count; i++)
            {
                CustomAppointment appointment = appointments[i];
                appointment.m_ConflictCount = 1;
                appointment.m_Drawn = false;
            }
            for (int j = 0; j < appointments.Count; j++)
            {
                CustomAppointment appointment2 = appointments[j];
                int num3 = (appointment2.StartDate.Hour * this.m_HourLines) + (appointment2.StartDate.Minute / (60 / this.m_HourLines));
                int num4 = (appointment2.EndDate.Hour * this.m_HourLines) + (appointment2.EndDate.Minute / (60 / this.m_HourLines));
                if ((num3 == num4) && ((num4 + 1) <= layoutArray.Length))
                {
                    num4++;
                }
                for (int k = num3; k < num4; k++)
                {
                    HalfHourLayout layout = layoutArray[k];
                    if (layout == null)
                    {
                        layout = new HalfHourLayout();
                        layout.Appointments = new CustomAppointment[KMaxAllowedParallelAppointments];
                        layoutArray[k] = layout;
                    }
                    layout.Appointments[layout.Count] = appointment2;
                    layout.Count++;
                    for (int m = 0; m < layout.Appointments.Length; m++)
                    {
                        CustomAppointment appointment3 = layout.Appointments[m];
                        if ((appointment3 != null) && (appointment3.m_ConflictCount < layout.Count))
                        {
                            appointment3.m_ConflictCount = layout.Count;
                        }
                    }
                }
            }
            return layoutArray;
        }

        public bool GetTimeAt(int x, int y, out DateTime time)
        {
            if (!this.m_BodyRectangle.Contains(x, y))
            {
                time = new DateTime();
                return false;
            }
            int num = (base.Width - (this.scrollbar.Width + this.HourLabelWidth)) / this.m_daysToShow;
            int num2 = ((y - this.DaysTop) / this.HalfHourHeight) + this.scrollbar.Value;
            x -= this.HourLabelWidth;
            time = this.m_startDate;
            time = time.Date;
            time = time.AddDays((double) (x / num));
            if ((num2 > 0) && (num2 < (0x18 * this.m_HourLines)))
            {
                time = time.AddMinutes((double) (num2 * (60 / this.m_HourLines)));
            }
            return true;
        }

        private void HandleUpDownKey()
        {
            DateTime time;
            DateTime time2;
            int num = 60 / this.m_HourLines;
            switch (this.m_CurDirection)
            {
                case CursorDirection.Up:
                    if ((this.SelectionStart.Hour != 0) || (this.SelectionStart.Minute != 0))
                    {
                        time = this.SelectionStart.AddMinutes((double) -num);
                        time2 = time.AddMinutes((double) num);
                        if ((this.m_selectedAppointment == null) || (this.m_selectedAppointment.m_ConflictCount == 1))
                        {
                            this.SelectionStart = time;
                            this.SelectionEnd = time2;
                        }
                        if (!this.SelectNextAppointment())
                        {
                            this.SelectionStart = time;
                            this.SelectionEnd = time2;
                            this.SelectNextAppointment();
                        }
                        this.EnsureVisible(this.SelectionStart, false);
                        this.Refresh();
                        if (this.m_selection == Resco.Controls.OutlookControls.SelectionType.Appointment)
                        {
                            this.RaiseSelectionChanged(SelectionEventArgs.SelectionType.ByKeyboard);
                        }
                        break;
                    }
                    if (((this.m_AllDayEvents != null) && (this.m_AllDayEvents.Count > 0)) && (this.m_AllDayListbox != null))
                    {
                        this.m_AllDayListbox.ParentFocused = false;
                        this.m_SelectionTimer.Enabled = false;
                    }
                    return;

                case CursorDirection.None:
                    break;

                case CursorDirection.Down:
                    if ((this.SelectionStart.Hour != 0x17) || (((this.SelectionStart.Minute != 30) || (this.m_HourLines != 2)) && ((this.SelectionStart.Minute != 0) || (this.m_HourLines != 1))))
                    {
                        time = this.SelectionStart.AddMinutes((double) num);
                        time2 = time.AddMinutes((double) num);
                        if ((this.m_selectedAppointment == null) || (this.m_selectedAppointment.m_ConflictCount == 1))
                        {
                            this.SelectionStart = time;
                            this.SelectionEnd = time2;
                        }
                        if (!this.SelectNextAppointment())
                        {
                            this.SelectionStart = time;
                            this.SelectionEnd = time2;
                            this.SelectNextAppointment();
                        }
                        this.EnsureVisible(this.SelectionStart, false);
                        this.Refresh();
                        if (this.m_selection != Resco.Controls.OutlookControls.SelectionType.Appointment)
                        {
                            break;
                        }
                        this.RaiseSelectionChanged(SelectionEventArgs.SelectionType.ByKeyboard);
                        return;
                    }
                    return;

                default:
                    return;
            }
        }

        public void HideTooltip()
        {
            this.m_ToolTip.Hide();
        }

        private void InitAllDayEventControl()
        {
            this.m_AllDayListbox = new AllDayListbox();
            this.m_AllDayListbox.RowHeight *= this.m_VgaOffset;
            this.m_AllDayListbox.Location = new Point(0, this.m_dayHeadersHeight);
            this.m_AllDayListbox.Size = new Size(base.Width, this.m_AllDayListbox.RowHeight);
            this.m_AllDayListbox.GridColor = this.HourSeperatorColor;
            this.m_AllDayListbox.Visible = false;
            this.m_AllDayListbox.ScrollbarWidth = this.ScrollbarWidth;
            this.m_AllDayListbox.RowEntered += new AllDayListbox.RowEnteredEventHandler(this.AllDayListbox_RowEntered);
            this.m_AllDayListbox.ToolTipRequired += new AllDayListbox.ToolTipRequiredEventHandler(this.AllDayListbox_ToolTipRequired);
            this.m_AllDayListbox.LostFocus += new EventHandler(this.AllDayListbox_LostFocus);
            this.m_AllDayListbox.GotFocus += new EventHandler(this.AllDayListbox_GotFocus);
            base.Controls.Add(this.m_AllDayListbox);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(OutlookWeekCalendar));
            this.imageListQVGA = new ImageList();
            this.m_SelectionTimer = new Timer();
            this.imageListVGA = new ImageList();
            this.m_Icons = new ImageList();
            base.SuspendLayout();
            this.imageListQVGA.ImageSize = new Size(13, 12);
            this.imageListQVGA.Images.Clear();
            this.imageListQVGA.Images.Add((Image) manager.GetObject("resource"));
            this.imageListQVGA.Images.Add((Image) manager.GetObject("resource1"));
            this.m_SelectionTimer.Interval = 10;
            this.m_SelectionTimer.Tick += new EventHandler(this.m_SelectionTimer_Tick);
            this.imageListVGA.ImageSize = new Size(0x20, 0x20);
            this.imageListVGA.Images.Clear();
            this.imageListVGA.Images.Add((Image) manager.GetObject("resource2"));
            this.imageListVGA.Images.Add((Image) manager.GetObject("resource3"));
            this.m_Icons.ImageSize = new Size(0x12, 0x12);
            this.m_Icons.Images.Clear();
            this.m_Icons.Images.Add((Image) manager.GetObject("resource4"));
            this.m_Icons.Images.Add((Image) manager.GetObject("resource5"));
            this.m_Icons.Images.Add((Image) manager.GetObject("resource6"));
            this.m_Icons.Images.Add((Image) manager.GetObject("resource7"));
            this.m_Icons.Images.Add((Image) manager.GetObject("resource8"));
            this.m_Icons.Images.Add((Image) manager.GetObject("resource9"));
            base.AutoScaleDimensions=(new SizeF(96f, 96f));
            //base.set_AutoScaleMode(2);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.Name = "OutlookWeekCalendar";
            base.ResumeLayout(false);
        }

        private void InitScrollBar()
        {
            this.scrollbar = new VScrollBar();
            this.scrollbar.Width = 15;
            this.scrollbar.LargeChange = 2;
            this.scrollbar.SmallChange = 1;
            this.scrollbar.Visible = this.m_allowScroll;
            this.scrollbar.ValueChanged += new EventHandler(this.scrollbar_ValueChanged);
            this.AdjustScrollbar();
            this.scrollbar.Value = this.m_startHour * this.m_HourLines;
            base.Controls.Add(this.scrollbar);
        }

        private void InitToolTipControl()
        {
            this.m_ToolTip = new Resco.Controls.OutlookControls.ControlToolTip();
            this.m_ToolTip.Visible = false;
            this.m_ToolTip.Location = new Point(0, 0);
            this.m_ToolTip.Size = new Size(base.Width, 30);
            base.Controls.Add(this.m_ToolTip);
        }

        private bool IsHourSelected(int hour, int minutes)
        {
            if (this.m_selectedAppointment != null)
            {
                return false;
            }
            DateTime time = this.SelectionStart.Date.AddHours((double) hour).AddMinutes((double) minutes);
            return ((time >= this.SelectionStart) && (time < this.SelectionEnd));
        }

        private bool IsTooltipClicked(Rectangle anAppRect, int x, int y)
        {
            if ((((anAppRect.X + anAppRect.Width) - 20) < x) && (((anAppRect.Y + anAppRect.Height) - 20) < y))
            {
                this.m_drawTool.ToolTipClicked = true;
                return true;
            }
            return false;
        }

        private void m_SelectionTimer_Tick(object sender, EventArgs e)
        {
            if (this.m_CurDirection != CursorDirection.None)
            {
                this.HandleUpDownKey();
            }
        }

        protected virtual void OnDaysToShowChanged()
        {
            if (this.m_daysToShow != 1)
            {
                this.m_AllDayListbox.Visible = false;
            }
            base.Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
        }

        protected void OnHalfHourHeightChanged()
        {
            this.AdjustScrollbar();
            base.Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.bPressed)
            {
                e.Handled = true;
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        this.m_CurDirection = CursorDirection.Up;
                        break;

                    case Keys.Down:
                        this.m_CurDirection = CursorDirection.Down;
                        break;

                    default:
                        this.m_SelectionTimer.Enabled = false;
                        this.m_CurDirection = CursorDirection.None;
                        break;
                }
                if ((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down))
                {
                    this.HandleUpDownKey();
                    this.m_SelectionTimer.Enabled = true;
                    this.bPressed = true;
                }
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            this.m_SelectionTimer.Enabled = false;
            this.bPressed = false;
            this.m_CurDirection = CursorDirection.None;
            base.OnKeyUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            bool flag = false;
            bool flag2 = false;
            if (this.m_bEnableTouchScrolling)
            {
                this.m_activeTool = this.m_TouchTool;
            }
            else
            {
                this.m_activeTool = this.m_drawTool;
            }
            base.Focus();
            this.m_AllDayListbox.SelectedIndex = -1;
            this.m_AllDayListbox.Refresh();
            AppointmentView appointmentViewAt = this.GetAppointmentViewAt(e.X, e.Y);
            CustomAppointment anAppointment = (appointmentViewAt == null) ? null : appointmentViewAt.Appointment;
            if (anAppointment == null)
            {
                DateTime time;
                if (this.m_selectedAppointment != null)
                {
                    this.m_selectedAppointment = null;
                    base.Invalidate();
                }
                this.m_selection = Resco.Controls.OutlookControls.SelectionType.DateRange;
                if (((e.Button == MouseButtons.Left) && this.GetTimeAt(e.X, e.Y, out time)) && ((this.SelectionStart.CompareTo(time) <= 0) && (this.SelectionEnd.CompareTo(time) > 0)))
                {
                    flag = true;
                }
                this.m_ToolTip.Hide();
            }
            else
            {
                bool flag3 = false;
                Rectangle anAppRect = appointmentViewAt.Rectangle;
                if (this.m_EnableToolTip)
                {
                    if ((this.m_daysToShow > 1) || this.IsTooltipClicked(anAppRect, e.X, e.Y))
                    {
                        this.ShowToolTip(anAppointment, anAppRect, e.X, e.Y);
                        flag3 = true;
                    }
                    else
                    {
                        this.m_ToolTip.Hide();
                    }
                }
                if (!flag3 || anAppointment.AllDayEvent)
                {
                    this.m_selectedAppointment = anAppointment;
                    this.m_selection = Resco.Controls.OutlookControls.SelectionType.Appointment;
                }
                this.Refresh();
            }
            if ((this.m_activeTool != null) && !flag)
            {
                this.m_drawTool.MouseDown(e);
                this.m_TouchTool.MouseDown(e);
                flag2 = true;
            }
            if ((this.ContextMenu != null) && MouseUtils.IsContextMenu(e, base.Handle))
            {
                this.ContextMenu.Show(this, new Point(e.X, e.Y));
            }
            else
            {
                if ((this.m_activeTool != null) && !flag2)
                {
                    this.m_activeTool.MouseDown(e);
                }
                base.OnMouseDown(e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.m_activeTool != null)
            {
                this.m_activeTool.MouseMove(e);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.m_activeTool != null)
            {
                this.m_activeTool.MouseUp(e);
            }
            if (!object.ReferenceEquals(this.m_activeTool, this.m_drawTool))
            {
                this.m_drawTool.MouseUp(e);
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.DrawControl();
            e.Graphics.DrawImage(this._BackBufferImage, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected void OnRendererChanged()
        {
            this.Font = this.m_renderer.FontBase;
            base.Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            this.ClearBackBuffer();
            this.WeekCalendarControl_Resize();
            base.OnResize(e);
        }

        protected virtual void OnResolveAppointments(ResolveAppointmentsEventArgs args)
        {
            if ((this.m_lastStartDate.CompareTo(args.StartDate) > 0) || (this.m_lastEndDate.CompareTo(args.EndDate) < 0))
            {
                if (this.ResolveAppointments != null)
                {
                    this.ResolveAppointments(this, args);
                }
                this.m_cachedAppointments.Clear();
                if (this.m_AllDayEvents != null)
                {
                    this.m_AllDayEvents.Clear();
                }
                for (int i = 0; i < args.Appointments.Count; i++)
                {
                    string str;
                    AppointmentList list;
                    CustomAppointment anAppointment = args.Appointments[i];
                    if (!anAppointment.AllDayEvent)
                    {
                        DateTime start = anAppointment.Start;
                        DateTime end = anAppointment.End;
                        if (anAppointment.Start.CompareTo(anAppointment.End) >= 0)
                        {
                            Debugger.Break();
                        }
                        else
                        {
                            str = "-1";
                            if (anAppointment.StartDate.Date.Equals(anAppointment.EndDate.Date))
                            {
                                str = anAppointment.StartDate.ToShortDateString();
                                goto Label_0158;
                            }
                            str = "-1";
                            foreach (CustomAppointment appointment2 in this.SplitAppointment(anAppointment))
                            {
                                str = appointment2.StartDate.ToShortDateString();
                                list = (AppointmentList) this.m_cachedAppointments[str];
                                if (list == null)
                                {
                                    list = new AppointmentList();
                                    this.m_cachedAppointments[str] = list;
                                }
                                list.Add(appointment2);
                            }
                        }
                    }
                    continue;
                Label_0158:
                    list = (AppointmentList) this.m_cachedAppointments[str];
                    if (list == null)
                    {
                        list = new AppointmentList();
                        this.m_cachedAppointments[str] = list;
                    }
                    list.Add(anAppointment);
                }
                this.m_lastStartDate = args.StartDate;
                this.m_lastEndDate = args.EndDate;
                this.m_lastLoadedAppointments = args.Appointments;
            }
        }

        protected virtual void OnStartDateChanged()
        {
            this.m_startDate = this.m_startDate.Date;
            this.m_selectedAppointment = null;
            this.m_selection = Resco.Controls.OutlookControls.SelectionType.DateRange;
            base.Invalidate();
        }

        protected virtual void OnStartHourChanged()
        {
            this.SetScrollbar(this.m_startHour * this.m_HourLines);
            base.Invalidate();
        }

        internal void RaiseSelectionChanged(SelectionEventArgs.SelectionType aType)
        {
            SelectionEventArgs args = new SelectionEventArgs(aType);
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, args);
            }
        }

        public void RefreshData()
        {
            this.m_lastStartDate = DateTime.MinValue;
            this.m_lastEndDate = DateTime.MinValue;
            this.m_selectedAppointment = null;
            this.Refresh();
        }

        private void scrollbar_ValueChanged(object sender, EventArgs e)
        {
            if (this.m_ToolTip != null)
            {
                this.m_ToolTip.Hide();
            }
            this.Refresh();
        }

        private bool SelectNextAppointment()
        {
            AppointmentList list = (AppointmentList) this.m_cachedAppointments[this.SelectionStart.ToShortDateString()];
            AppointmentList list2 = new AppointmentList();
            if (list == null)
            {
                return false;
            }
            CustomAppointment selectedAppointment = this.m_selectedAppointment;
            for (int i = 0; i < list.Count; i++)
            {
                if (this.m_AppointmentViews.ContainsKey(list[i]))
                {
                    AppointmentView view = this.m_AppointmentViews[(list[i])];
                    if ((view.Appointment.StartDate >= this.SelectionStart) && (view.Appointment.StartDate < this.SelectionEnd))
                    {
                        this.SetSelectedAppointment(view.Appointment);
                        if (this.m_selectedAppointment.m_ConflictCount == 1)
                        {
                            return true;
                        }
                        list2.Add(this.m_selectedAppointment);
                    }
                }
            }
            if (list2.Count == 0)
            {
                this.SetSelectedAppointment(null);
                return false;
            }
            for (int j = 0; j < list2.Count; j++)
            {
                CustomAppointment appointment2 = list2[j];
                if (selectedAppointment == null)
                {
                    this.SetSelectedAppointment(list2[j]);
                    return true;
                }
                if (selectedAppointment.Equals(appointment2))
                {
                    if ((j + 1) < list2.Count)
                    {
                        this.SetSelectedAppointment(list2[j + 1]);
                        return true;
                    }
                    this.SetSelectedAppointment(null);
                    return false;
                }
            }
            return true;
        }

        private void SetBodyRectangle()
        {
            if (this.scrollbar != null)
            {
                int width = base.Width;
                if (this.scrollbar.Visible)
                {
                    width -= this.scrollbar.Width;
                }
                int height = base.Height - this.DaysTop;
                this.m_BodyRectangle = new Rectangle(0, this.DaysTop, width, height);
            }
        }

        private int SetScrollbar(int scrollValue)
        {
            if (this.scrollbar == null)
            {
                return 0;
            }
            if (this.scrollbar.Maximum <= scrollValue)
            {
                this.scrollbar.Value = this.scrollbar.Maximum - 1;
            }
            else if (scrollValue < this.scrollbar.Minimum)
            {
                this.scrollbar.Value = this.scrollbar.Minimum;
            }
            else
            {
                this.scrollbar.Value = scrollValue;
            }
            return this.scrollbar.Value;
        }

        private void SetSelectedAppointment(CustomAppointment app)
        {
            this.m_selectedAppointment = app;
            if (this.m_selectedAppointment == null)
            {
                this.m_selection = Resco.Controls.OutlookControls.SelectionType.DateRange;
            }
            else
            {
                this.m_selection = Resco.Controls.OutlookControls.SelectionType.Appointment;
            }
        }

        protected virtual bool ShouldSerializeHalfHourSeperatorColor()
        {
            return !this.m_renderer.HalfHourSeperatorColor.Equals(SystemColors.GrayText);
        }

        protected virtual bool ShouldSerializeHourSeperatorColor()
        {
            return !this.m_renderer.HourSeperatorColor.Equals(SystemColors.GrayText);
        }

        protected virtual bool ShouldSerializeIcons()
        {
            if (this.Icons == null)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeIndexOfSelectedAppointment()
        {
            if (this.IndexOfSelectedAppointment == -1)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeSaturdayBackColor()
        {
            return !this.m_renderer.SaturdayBackColor.Equals(SystemColors.Control);
        }

        protected virtual bool ShouldSerializeSelectedAppointment()
        {
            if (this.SelectedAppointment == null)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeToolTipBackColor()
        {
            return !this.m_ToolTip.BackColor.Equals(SystemColors.Info);
        }

        protected virtual bool ShouldSerializeToolTipForeColor()
        {
            return !this.m_ToolTip.ForeColor.Equals(SystemColors.InfoText);
        }

        public void ShowTooltip()
        {
            this.m_ToolTip.Show();
            this.m_ToolTip.BringToFront();
        }

        public void ShowToolTip(CustomAppointment anAppointment)
        {
            if (anAppointment != null)
            {
                AppointmentView view = this.m_AppointmentViews[anAppointment];
                if (view != null)
                {
                    this.ShowToolTip(anAppointment, view.Rectangle, 0, 0);
                }
            }
        }

        private void ShowToolTip(CustomAppointment anAppointment, Rectangle anAppRect, int aX, int aY)
        {
            string aText = ((anAppointment.ToolTip != null) && (anAppointment.ToolTip.Length > 0)) ? anAppointment.ToolTip : anAppointment.Title;
            if (this.m_DockedToolTip)
            {
                this.m_ToolTip.Location = new Point(0, 0);
                this.m_ToolTip.Size = new Size(base.Width, 30);
                this.m_ToolTip.ShowText(aText);
            }
            else
            {
                int num;
                int num2;
                Size size = this.m_ToolTip.GetSize(aText);
                if (anAppointment.AllDayEvent && (this.m_daysToShow > 1))
                {
                    num = aX - size.Width;
                    if (num < 0)
                    {
                        num = 0;
                    }
                    num2 = aY;
                    if ((num2 + size.Height) > base.ClientRectangle.Bottom)
                    {
                        num2 = base.ClientRectangle.Bottom - size.Height;
                    }
                }
                else
                {
                    num = ((anAppRect.X + anAppRect.Width) - size.Width) - 5;
                    num2 = (anAppRect.Y + anAppRect.Height) - size.Height;
                    if (num2 < (base.Height / 2))
                    {
                        num2 += 5;
                    }
                    else
                    {
                        num2 -= 5;
                    }
                }
                int x = (num > 0) ? num : 3;
                int y = (num2 > 0) ? num2 : 3;
                this.m_ToolTip.Location = new Point(x, y);
                this.m_ToolTip.ShowText(aText, true, true);
            }
        }

        private CustomAppointment[] SplitAppointment(CustomAppointment anAppointment)
        {
            TimeSpan span = (TimeSpan) (anAppointment.EndDate.Date - anAppointment.StartDate.Date);
            if (anAppointment.EndDate.Date == anAppointment.StartDate.Date)
            {
                return new CustomAppointment[] { anAppointment };
            }
            DateTime startDate = anAppointment.StartDate;
            DateTime endDate = anAppointment.StartDate.Date.AddDays(1.0).AddSeconds(-1.0);
            CustomAppointment[] appointmentArray = new CustomAppointment[((int) span.TotalDays) + 1];
            for (int i = 0; i <= ((int) span.TotalDays); i++)
            {
                CustomAppointment appointment = new CustomAppointment(anAppointment);
                appointment.StartDate = startDate;
                appointment.EndDate = endDate;
                appointmentArray[i] = appointment;
                if (i == ((int) span.TotalDays))
                {
                    return appointmentArray;
                }
                if (anAppointment.EndDate.Date == endDate.AddDays(1.0).Date)
                {
                    endDate = anAppointment.EndDate;
                }
                else
                {
                    endDate = endDate.AddDays(1.0);
                }
                startDate = startDate.AddDays(1.0).Date;
            }
            return appointmentArray;
        }

        private void StartTimer()
        {
        }

        private void StopTimer(string aTitle)
        {
        }

        private void TouchTool_GestureDetected(object sender, TouchTool.GestureEventArgs e)
        {
            if ((e.Gesture == TouchTool.GestureType.Left) || (e.Gesture == TouchTool.GestureType.Right))
            {
                if ((e.Gesture == TouchTool.GestureType.Left) && (this.TouchScrollLeft != null))
                {
                    this.TouchScrollLeft(this, EventArgs.Empty);
                }
                else if ((e.Gesture == TouchTool.GestureType.Right) && (this.TouchScrollRight != null))
                {
                    this.TouchScrollRight(this, EventArgs.Empty);
                }
                else
                {
                    int num = this.DaysToShow * ((e.Gesture == TouchTool.GestureType.Right) ? -1 : 1);
                    this.StartDate = this.StartDate.AddDays((double) num);
                }
            }
        }

        private void UpdateWorkingHours()
        {
            this.m_workStart = new DateTime(1, 1, 1, this.m_workingHourStart, this.m_workingMinuteStart, 0);
            this.m_workEnd = new DateTime(1, 1, 1, this.m_workingHourEnd, this.m_workingMinuteEnd, 0);
            base.Invalidate();
        }

        private void WeekCalendarControl_Resize()
        {
            this.AdjustScrollbar();
            this.SetBodyRectangle();
            if (this.m_AllDayListbox != null)
            {
                this.m_AllDayListbox.Width = base.Width;
            }
            if (this.m_ToolTip != null)
            {
                this.m_ToolTip.Hide();
                this.m_ToolTip.Width = base.Width;
            }
        }

        public Color AllDayBackColor
        {
            get
            {
                return this.m_AllDayListbox.BackColor;
            }
            set
            {
                this.m_AllDayListbox.BackColor = value;
            }
        }

        public Color AllDayForeColor
        {
            get
            {
                return this.m_AllDayListbox.ForeColor;
            }
            set
            {
                this.m_AllDayListbox.ForeColor = value;
            }
        }

        public Color AllDayGridColor
        {
            get
            {
                return this.m_AllDayListbox.GridColor;
            }
            set
            {
                this.m_AllDayListbox.GridColor = value;
            }
        }

        public Color AllDaySelectedBackColor
        {
            get
            {
                return this.m_AllDayListbox.SelectedBackColor;
            }
            set
            {
                this.m_AllDayListbox.SelectedBackColor = value;
            }
        }

        public Color AllDaySelectedForeColor
        {
            get
            {
                return this.m_AllDayListbox.SelectedForeColor;
            }
            set
            {
                this.m_AllDayListbox.SelectedForeColor = value;
            }
        }

        [DefaultValue(true)]
        public bool AllowScroll
        {
            get
            {
                return this.m_allowScroll;
            }
            set
            {
                this.m_allowScroll = value;
            }
        }

        public int AppointmentGripWidth
        {
            get
            {
                return (this.m_appointmentGripWidth * this.m_VgaOffset);
            }
            set
            {
                if (this.m_appointmentGripWidth != value)
                {
                    this.m_appointmentGripWidth = value;
                    this.Refresh();
                }
            }
        }

        public int AppointmentMinimalWidth
        {
            get
            {
                return this.Renderer.AppointmentMinimalWidth;
            }
            set
            {
                this.Renderer.AppointmentMinimalWidth = value;
            }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                this.m_renderer.BackColor = value;
                base.Invalidate();
            }
        }

        public override System.Windows.Forms.ContextMenu ContextMenu
        {
            get
            {
                return this.m_ContextMenu;
            }
            set
            {
                this.m_ContextMenu = value;
                this.m_AllDayListbox.ContextMenu = value;
            }
        }

        private int DaysTop
        {
            get
            {
                int num = this.m_dayHeadersHeight * this.m_VgaOffset;
                if ((this.m_AllDayListbox != null) && this.m_AllDayListbox.Visible)
                {
                    num += this.m_AllDayListbox.Height;
                }
                return num;
            }
        }

        [DefaultValue(1)]
        public int DaysToShow
        {
            get
            {
                return this.m_daysToShow;
            }
            set
            {
                if (value > 7)
                {
                    this.m_daysToShow = 7;
                }
                else if (value < 1)
                {
                    this.m_daysToShow = 1;
                }
                else
                {
                    this.m_daysToShow = value;
                }
                this.OnDaysToShowChanged();
            }
        }

        private int DisplayedHalfHourCount
        {
            get
            {
                return ((base.Height - this.DaysTop) / this.HalfHourHeight);
            }
        }

        public bool DockedToolTip
        {
            get
            {
                return this.m_DockedToolTip;
            }
            set
            {
                this.m_DockedToolTip = value;
            }
        }

        public bool EnableHalfHourSlots
        {
            get
            {
                return this.m_EnableHalfHourSlots;
            }
            set
            {
                if (this.m_EnableHalfHourSlots != value)
                {
                    this.m_EnableHalfHourSlots = value;
                    this.m_HourLines = this.m_EnableHalfHourSlots ? 2 : 1;
                    this.AdjustScrollbar();
                    this.SetScrollbar(this.m_startHour * this.m_HourLines);
                    this.SelectionEnd = new DateTime(0L);
                    this.SelectionStart = new DateTime(0L);
                    base.Invalidate();
                }
            }
        }

        public bool EnableToolTip
        {
            get
            {
                return this.m_EnableToolTip;
            }
            set
            {
                this.m_EnableToolTip = value;
                this.m_AllDayListbox.EnableToolTip = value;
                if (!this.m_EnableToolTip)
                {
                    this.m_ToolTip.Hide();
                }
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
                if (value != this.Renderer.FontBase)
                {
                    this.Renderer.FontBase = value;
                    base.Font = value;
                    base.Invalidate();
                }
            }
        }

        public System.Drawing.Font FontHeader
        {
            get
            {
                return this.Renderer.FontHeader;
            }
            set
            {
                if (value != this.Renderer.FontHeader)
                {
                    this.Renderer.FontHeader = value;
                    base.Invalidate();
                }
            }
        }

        public System.Drawing.Font FontHour
        {
            get
            {
                return this.Renderer.FontHour;
            }
            set
            {
                if (value != this.Renderer.FontHour)
                {
                    this.Renderer.FontHour = value;
                    base.Invalidate();
                }
            }
        }

        public System.Drawing.Font FontMinute
        {
            get
            {
                return this.Renderer.FontMinute;
            }
            set
            {
                if (value != this.Renderer.FontMinute)
                {
                    this.Renderer.FontMinute = value;
                    base.Invalidate();
                }
            }
        }

        public int HalfHourHeight
        {
            get
            {
                return (this.m_halfHourHeight * this.m_VgaOffset);
            }
            set
            {
                this.m_halfHourHeight = value;
                this.OnHalfHourHeightChanged();
            }
        }

        public Color HalfHourSeperatorColor
        {
            get
            {
                return this.m_renderer.HalfHourSeperatorColor;
            }
            set
            {
                this.m_renderer.HalfHourSeperatorColor = value;
                base.Invalidate();
            }
        }

        public int HeaderHeight
        {
            get
            {
                return (this.m_dayHeadersHeight * this.m_VgaOffset);
            }
            set
            {
                this.m_dayHeadersHeight = value;
                if (this.m_AllDayListbox != null)
                {
                    this.m_AllDayListbox.Location = new Point(0, this.m_dayHeadersHeight);
                }
                base.Invalidate();
            }
        }

        public Color HourLabelBGColor
        {
            get
            {
                return this.m_renderer.HourLabelBGColor;
            }
            set
            {
                this.m_renderer.HourLabelBGColor = value;
                base.Invalidate();
            }
        }

        public Color HourLabelColor
        {
            get
            {
                return this.m_renderer.HourLabelColor;
            }
            set
            {
                this.m_renderer.HourLabelColor = value;
                base.Invalidate();
            }
        }

        public Color HourLabelSelectedBGColor
        {
            get
            {
                return this.m_renderer.HourLabelSelectedBGColor;
            }
            set
            {
                this.m_renderer.HourLabelSelectedBGColor = value;
                base.Invalidate();
            }
        }

        public Color HourLabelSelectedColor
        {
            get
            {
                return this.m_renderer.HourLabelSelectedColor;
            }
            set
            {
                this.m_renderer.HourLabelSelectedColor = value;
                base.Invalidate();
            }
        }

        private int HourLabelWidth
        {
            get
            {
                return (this.m_hourLabelWidth * this.m_VgaOffset);
            }
        }

        public int HourLines
        {
            get
            {
                return this.m_HourLines;
            }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                if ((value == 3) || (value > 4))
                {
                    value = 4;
                }
                if (this.m_HourLines != value)
                {
                    this.m_HourLines = value;
                    this.SetScrollbar(this.m_startHour * this.m_HourLines);
                    base.Invalidate();
                }
            }
        }

        public Color HourSeperatorColor
        {
            get
            {
                return this.m_renderer.HourSeperatorColor;
            }
            set
            {
                this.m_renderer.HourSeperatorColor = value;
                base.Invalidate();
            }
        }

        public ImageList Icons
        {
            get
            {
                if (this.m_bDefaultIconsused)
                {
                    return null;
                }
                return this.Renderer.AppIcons;
            }
            set
            {
                this.Renderer.AppIcons = value;
                this.m_bDefaultIconsused = false;
            }
        }

        public int IndexOfSelectedAppointment
        {
            get
            {
                if ((this.m_selectedAppointment != null) && (this.m_lastLoadedAppointments != null))
                {
                    return this.m_lastLoadedAppointments.IndexOf(this.m_selectedAppointment);
                }
                return -1;
            }
            set
            {
                if ((value <= this.m_lastLoadedAppointments.Count) && (value >= 0))
                {
                    this.m_selectedAppointment = this.m_lastLoadedAppointments[value];
                }
                if (value < 0)
                {
                    this.m_selectedAppointment = null;
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

        public AbstractRenderer Renderer
        {
            get
            {
                return this.m_renderer;
            }
            set
            {
                this.m_renderer = value;
                this.OnRendererChanged();
            }
        }

        public Color SaturdayBackColor
        {
            get
            {
                return this.m_renderer.SaturdayBackColor;
            }
            set
            {
                this.m_renderer.SaturdayBackColor = value;
                base.Invalidate();
            }
        }

        public bool SaturdayIsWorkday
        {
            get
            {
                return this.m_SaturdayIsWorkday;
            }
            set
            {
                if (this.m_SaturdayIsWorkday != value)
                {
                    this.m_SaturdayIsWorkday = value;
                    base.Invalidate();
                }
            }
        }

        internal int ScrollbarValue
        {
            get
            {
                return this.scrollbar.Value;
            }
            set
            {
                if (this.scrollbar.Value != value)
                {
                    this.SetScrollbar(value);
                }
            }
        }

        [DefaultValue(15)]
        public int ScrollbarWidth
        {
            get
            {
                return this.scrollbar.Width;
            }
            set
            {
                this.scrollbar.Width = value;
                if (this.m_AllDayListbox != null)
                {
                    this.m_AllDayListbox.ScrollbarWidth = value;
                }
            }
        }

        public CustomAppointment SelectedAppointment
        {
            get
            {
                return this.m_selectedAppointment;
            }
            set
            {
                if ((this.m_lastLoadedAppointments != null) && (this.m_lastLoadedAppointments.Count != 0))
                {
                    if (value == null)
                    {
                        this.m_selectedAppointment = null;
                    }
                    else if (this.m_lastLoadedAppointments.Contains(value))
                    {
                        this.m_selectedAppointment = value;
                    }
                }
            }
        }

        public Resco.Controls.OutlookControls.SelectionType Selection
        {
            get
            {
                return this.m_selection;
            }
        }

        public Color SelectionColor
        {
            get
            {
                return this.m_renderer.SelectionColor;
            }
            set
            {
                this.m_renderer.SelectionColor = value;
                base.Invalidate();
            }
        }

        public DateTime SelectionEnd
        {
            get
            {
                return this.m_selectionEnd;
            }
            set
            {
                if (((value.Ticks != 0L) && (value.Hour == 0)) && (value.Minute == 0))
                {
                    this.m_selectionEnd = value.AddMinutes(-1.0);
                }
                else
                {
                    this.m_selectionEnd = value;
                }
            }
        }

        public DateTime SelectionStart
        {
            get
            {
                return this.m_selectionStart;
            }
            set
            {
                this.m_selectionStart = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.m_startDate;
            }
            set
            {
                this.m_startDate = value;
                this.OnStartDateChanged();
            }
        }

        [DefaultValue(8)]
        public int StartHour
        {
            get
            {
                return this.m_startHour;
            }
            set
            {
                if ((value >= 0) && (value < 0x18))
                {
                    this.m_startHour = value;
                    this.OnStartHourChanged();
                }
            }
        }

        public Color SundayBackColor
        {
            get
            {
                return this.m_renderer.SundayBackColor;
            }
            set
            {
                this.m_renderer.SundayBackColor = value;
                base.Invalidate();
            }
        }

        public Color ToolTipBackColor
        {
            get
            {
                return this.m_ToolTip.BackColor;
            }
            set
            {
                this.m_ToolTip.BackColor = value;
                this.m_ToolTip.Invalidate();
            }
        }

        public Color ToolTipForeColor
        {
            get
            {
                return this.m_ToolTip.ForeColor;
            }
            set
            {
                this.m_ToolTip.ForeColor = value;
                this.m_ToolTip.Invalidate();
            }
        }

        public bool TouchScrolling
        {
            get
            {
                return this.m_bEnableTouchScrolling;
            }
            set
            {
                this.m_bEnableTouchScrolling = value;
            }
        }

        public int TouchSensitivity
        {
            get
            {
                return this.m_touchSensitivity;
            }
            set
            {
                this.m_touchSensitivity = value;
            }
        }

        public bool UseAppointmentsColor
        {
            get
            {
                return this.m_AllDayListbox.UseAppointmentsColor;
            }
            set
            {
                this.m_AllDayListbox.UseAppointmentsColor = value;
            }
        }

        public bool UseGradient
        {
            get
            {
                return this.m_useGradient;
            }
            set
            {
                if (this.m_useGradient != value)
                {
                    this.m_useGradient = value;
                    this.m_renderer.UseGradient = this.m_useGradient;
                    this.Refresh();
                }
            }
        }

        public Color WorkingHourColor
        {
            get
            {
                return this.m_renderer.WorkingHourColor;
            }
            set
            {
                this.m_renderer.WorkingHourColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(0x11)]
        public int WorkingHourEnd
        {
            get
            {
                return this.m_workingHourEnd;
            }
            set
            {
                this.m_workingHourEnd = value;
                this.UpdateWorkingHours();
            }
        }

        [DefaultValue(8)]
        public int WorkingHourStart
        {
            get
            {
                return this.m_workingHourStart;
            }
            set
            {
                this.m_workingHourStart = value;
                this.UpdateWorkingHours();
            }
        }

        [DefaultValue(0)]
        public int WorkingMinuteEnd
        {
            get
            {
                return this.m_workingMinuteEnd;
            }
            set
            {
                this.m_workingMinuteEnd = value;
                this.UpdateWorkingHours();
            }
        }

        [DefaultValue(0)]
        public int WorkingMinuteStart
        {
            get
            {
                return this.m_workingMinuteStart;
            }
            set
            {
                this.m_workingMinuteStart = value;
                this.UpdateWorkingHours();
            }
        }

        private class AppointmentList : List<CustomAppointment>
        {
        }

        internal class AppointmentView
        {
            public CustomAppointment Appointment;
            public System.Drawing.Rectangle Rectangle;
        }

        private enum Arrows
        {
            Down,
            Up
        }

        private enum CursorDirection
        {
            Down = 1,
            None = 0,
            Up = -1
        }

        private class HalfHourLayout
        {
            public CustomAppointment[] Appointments;
            public int Count;
        }
    }
}

