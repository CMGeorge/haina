namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.Windows.Forms;

    public class Office11Renderer : AbstractRenderer
    {
        internal static Color BackgroundImageColor(Image image)
        {
            using (Bitmap bitmap = new Bitmap(image))
            {
                return bitmap.GetPixel(0, 0);
            }
        }

        public static Rectangle CalculateGripRect(Rectangle rect, CustomAppointment appointment, int hourLines, int halfHourHeight)
        {
            Rectangle rectangle = rect;
            rectangle.Y++;
            rectangle.Height--;
            int num = 60 / hourLines;
            double num2 = ((double) halfHourHeight) / ((double) num);
            if (appointment.StartDate.Minute != 0)
            {
                int num3 = (int) ((appointment.StartDate.Minute % num) * num2);
                rectangle.Y += num3;
                rectangle.Height -= num3;
            }
            if ((appointment.EndDate.Minute != 0) && ((appointment.EndDate.Minute % num) > 0))
            {
                rectangle.Height -= (int) ((num - (appointment.EndDate.Minute % num)) * num2);
            }
            return rectangle;
        }

        public override string DayHeader(DateTime aDate, int aMaxWidth)
        {
            if (aMaxWidth < 0x69)
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(aDate.DayOfWeek);
            }
            return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(aDate.DayOfWeek);
        }

        public override void DrawAllDayAppointment(Graphics gr, CustomAppointment app, ref Rectangle borderRect, bool anIsSelected)
        {
            float width = 1f;
            Color black = Color.Black;
            using (SolidBrush brush = new SolidBrush(app.BorderColor))
            {
                gr.FillRectangle(brush, borderRect);
            }
            if (anIsSelected)
            {
                width = 2f;
                black = this.SelectionColor;
                borderRect.Inflate(-1, -1);
            }
            using (Pen pen = new Pen(black, width))
            {
                gr.DrawRectangle(pen, borderRect);
            }
        }

        public override void DrawAppointment(Graphics g, Rectangle rect, CustomAppointment appointment, bool isSelected, int gripWidth, int hourLines, int halfHourHeight)
        {
            DrawEventArgs e = new DrawEventArgs(g, rect, appointment, isSelected, gripWidth);
            if (!base.OnResolveDrawAppointment(e))
            {
                Pen pen = new Pen(SystemColors.WindowFrame, 1f);
                if (rect.Width <= base.AppointmentMinimalWidth)
                {
                    Rectangle rectangle = this.DrawGripper(g, rect, appointment, isSelected, rect.Width, hourLines, halfHourHeight);
                    g.DrawRectangle(pen, rectangle);
                    if (isSelected)
                    {
                        Rectangle rectangle2 = rectangle;
                        rectangle2.Inflate(-2, -2);
                        using (Pen pen2 = new Pen(this.SelectionColor, 2f))
                        {
                            g.DrawRectangle(pen2, rectangle2);
                        }
                        g.DrawRectangle(pen, rectangle2);
                    }
                    pen.Dispose();
                    pen = null;
                }
                else
                {
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    Rectangle rectangle3 = rect;
                    rectangle3.X += gripWidth + 2;
                    rectangle3.Width -= gripWidth + 1;
                    using (SolidBrush brush = new SolidBrush(appointment.Color))
                    {
                        g.FillRectangle(brush, rectangle3);
                    }
                    Rectangle rectangle4 = this.DrawGripper(g, rect, appointment, isSelected, gripWidth, hourLines, halfHourHeight);
                    g.DrawRectangle(pen, rectangle4);
                    Rectangle aTextRect = rectangle3;
                    aTextRect.Inflate(-2, -2);
                    Rectangle layoutRectangle = DrawIcons(g, aTextRect, appointment, base.AppIcons);
                    using (SolidBrush brush2 = new SolidBrush(appointment.TextColor))
                    {
                        g.DrawString(appointment.Title, this.FontBase, brush2, layoutRectangle, format);
                    }
                    DrawToolTipArrowIfNeeded(g, appointment.Title, this.FontBase, layoutRectangle, rectangle3, halfHourHeight);
                    if (isSelected)
                    {
                        Rectangle rectangle7 = rect;
                        rectangle7.Inflate(-2, -2);
                        using (Pen pen3 = new Pen(appointment.BorderColor, 4f))
                        {
                            g.DrawRectangle(pen3, rectangle7);
                        }
                        rectangle7.Inflate(2, 2);
                        g.DrawRectangle(pen, rectangle7);
                        rectangle7.Inflate(-4, -4);
                        g.DrawRectangle(pen, rectangle7);
                    }
                    else
                    {
                        using (Pen pen4 = new Pen(SystemColors.InactiveBorder, 1f))
                        {
                            g.DrawRectangle(pen4, rectangle3);
                        }
                    }
                    pen.Dispose();
                    pen = null;
                }
            }
        }

        public override void DrawDayBackground(Graphics g, Rectangle rect)
        {
            using (Brush brush = new SolidBrush(this.HourColor))
            {
                g.FillRectangle(brush, rect);
            }
        }

        public override void DrawDayHeader(Graphics g, Rectangle rect, DateTime date)
        {
            string text = this.DayHeader(date, rect.Width);
            this.DrawDayHeader(g, rect, text);
        }

        public override void DrawDayHeader(Graphics g, Rectangle rect, string text)
        {
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.FormatFlags = StringFormatFlags.NoWrap;
            format.LineAlignment = StringAlignment.Center;
            g.DrawRectangle(new Pen(SystemColors.InactiveBorder), rect);
            g.DrawString(text, this.FontHeader, new SolidBrush(SystemColors.WindowText), rect, format);
        }

        public Rectangle DrawGripper(Graphics g, Rectangle rect, CustomAppointment appointment, bool isSelected, int gripWidth, int hourLines, int halfHourHeight)
        {
            DrawEventArgs e = new DrawEventArgs(g, rect, appointment, isSelected, gripWidth);
            if (base.OnResolveDrawGripper(e))
            {
                return rect;
            }
            Rectangle rc = CalculateGripRect(rect, appointment, hourLines, halfHourHeight);
            rc.Width = gripWidth + 1;
            if (base.m_useGradient)
            {
                if (appointment.GradientBackColor == null)
                {
                    new GradientColor(GradientFill.GetColorLighter(appointment.BorderColor), GradientFill.GetColorDarker(appointment.BorderColor), FillDirection.Vertical).DrawGradient(g, rc);
                    return rc;
                }
                appointment.GradientBackColor.DrawGradient(g, rc);
                return rc;
            }
            using (SolidBrush brush = new SolidBrush(appointment.BorderColor))
            {
                g.FillRectangle(brush, rc);
            }
            return rc;
        }

        public override void DrawHalfHourLabel(Graphics g, Rectangle rect, string aText, bool selected)
        {
            string text = aText;
            Color color = selected ? this.HourLabelSelectedColor : this.HourLabelColor;
            Color color2 = selected ? this.HourLabelSelectedBGColor : this.HourLabelBGColor;
            using (SolidBrush brush = new SolidBrush(color2))
            {
                g.FillRectangle(brush, rect);
            }
            using (Pen pen = new Pen(SystemColors.InactiveBorder))
            {
                g.DrawLine(pen, rect.Left, rect.Y, rect.Width, rect.Y);
            }
            rect.X += 4;
            SizeF ef = g.MeasureString(text, this.FontMinute);
            rect.Y += (int) ((rect.Height - ef.Height) / 2f);
            g.DrawString(text, this.FontMinute, new SolidBrush(color), rect);
        }

        public override void DrawHourLabel(Graphics g, Rectangle rect, int hour, bool selected)
        {
            Color color = selected ? this.HourLabelSelectedColor : this.HourLabelColor;
            Color color2 = selected ? this.HourLabelSelectedBGColor : this.HourLabelBGColor;
            using (SolidBrush brush = new SolidBrush(color2))
            {
                g.FillRectangle(brush, rect);
            }
            using (Pen pen = new Pen(SystemColors.InactiveBorder))
            {
                g.DrawLine(pen, rect.Left, rect.Y, rect.Width, rect.Y);
            }
            if ((CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.IndexOf("h") != -1) && (hour > 12))
            {
                hour = hour % 12;
            }
            string s = hour.ToString("00");
            g.DrawString(s, this.FontHour, new SolidBrush(color), rect);
        }

        public static Rectangle DrawIcons(Graphics gr, Rectangle aTextRect, CustomAppointment anAppointment, ImageList anAppIcons)
        {
            if ((anAppIcons != null) && (anAppointment.IconIndexes != null))
            {
                int length = anAppointment.IconIndexes.Length;
                int width = anAppIcons.ImageSize.Width;
                int num3 = aTextRect.Width / 3;
                if ((aTextRect.Width / 2) > width)
                {
                    int x = (aTextRect.Right - 1) - width;
                    int y = aTextRect.Y + 1;
                    Region clip = gr.Clip;
                    Region region2 = new Region(aTextRect);
                    region2.Intersect(clip);
                    gr.Clip = region2;
                    for (int i = length - 1; i >= 0; i--)
                    {
                        using (Image image = anAppIcons.Images[anAppointment.IconIndexes[i]])
                        {
                            Rectangle rect = new Rectangle(x, y, width, width);
                            DrawSingleIcon(gr, rect, image);
                        }
                        x -= width;
                        if (num3 >= (x - aTextRect.X))
                        {
                            aTextRect.Width -= width * (length - i);
                            return aTextRect;
                        }
                    }
                    gr.ResetClip();
                    region2.Dispose();
                    region2 = null;
                    if (clip != null)
                    {
                        gr.Clip = clip;
                    }
                    aTextRect.Width -= width * length;
                }
            }
            return aTextRect;
        }

        private static bool DrawSingleIcon(Graphics gr, Rectangle rect, Image anImage)
        {
            if (anImage != null)
            {
                ImageAttributes imageAttr = new ImageAttributes();
                Color colorLow = BackgroundImageColor(anImage);
                imageAttr.SetColorKey(colorLow, colorLow);
                gr.DrawImage(anImage, rect, 0, 0, anImage.Width, anImage.Height, GraphicsUnit.Pixel, imageAttr);
                return true;
            }
            return false;
        }

        private static void DrawToolTipArrow(Graphics gr, Rectangle aRect, int aHalfHourHeight)
        {
            Point[] pointArray=new Point[3];
            int num = aHalfHourHeight / 2;
            pointArray = new Point[] { new Point((aRect.X + aRect.Width) - 1, (aRect.Y + aRect.Height) - num), new Point(pointArray[0].X, (pointArray[0].Y + num) - 1), new Point(pointArray[1].X - num, pointArray[1].Y) };
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                gr.FillPolygon(brush, pointArray);
            }
        }

        public static void DrawToolTipArrowIfNeeded(Graphics gr, string title, Font font, Rectangle aTitleRect, Rectangle aBodyRect, int halfHourHeight)
        {
            SizeF ef = gr.MeasureString(title, font);
            int num = ((int) (ef.Width / ((float) aTitleRect.Width))) + 1;
            int num2 = (int) (ef.Height * num);
            if (num2 > aTitleRect.Height)
            {
                DrawToolTipArrow(gr, aBodyRect, halfHourHeight);
            }
        }
    }
}

