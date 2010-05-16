namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Globalization;

    public class PredefinedRollers
    {
        private Roller m_AmAndPmRoller;
        private Roller m_DayRoller;
        private int m_FirstHour;
        private Roller m_HourRoller;
        private int m_MaxEnabledDay = 0x1f;
        private int m_MinuteInterval = 5;
        private Roller m_MinuteRoller;
        private Roller m_MonthRoller;
        private Roller m_YearRoller;

        internal void InitAmAndPmRoller()
        {
            if (this.m_AmAndPmRoller == null)
            {
                this.m_AmAndPmRoller = new Roller();
                this.m_AmAndPmRoller.Width = 50;
                this.m_AmAndPmRoller.RollerDataType = RollerType.AmPm;
            }
            this.m_AmAndPmRoller.RowItems.Clear();
            string aMDesignator = DateTimeFormatInfo.CurrentInfo.AMDesignator;
            string pMDesignator = DateTimeFormatInfo.CurrentInfo.PMDesignator;
            if ((aMDesignator == null) || (aMDesignator.Length == 0))
            {
                aMDesignator = "AM";
            }
            if ((pMDesignator == null) || (pMDesignator.Length == 0))
            {
                pMDesignator = "PM";
            }
            this.m_AmAndPmRoller.RowItems.Add(new DateTimePickerRow(aMDesignator));
            this.m_AmAndPmRoller.RowItems.Add(new DateTimePickerRow(pMDesignator));
            this.m_AmAndPmRoller.SelectedIndex = (DateTime.Now.Hour < 12) ? 0 : 1;
        }

        internal void InitDayRoller()
        {
            if (this.m_DayRoller == null)
            {
                this.m_DayRoller = new Roller();
                this.m_DayRoller.Width = 40;
                this.m_DayRoller.RollerDataType = RollerType.Day;
            }
            this.m_DayRoller.RowItems.Clear();
            bool anEnabled = true;
            bool anIsDefault = false;
            int day = DateTime.Today.Day;
            for (int i = 1; i <= 0x1f; i++)
            {
                if (day == i)
                {
                    anIsDefault = true;
                }
                else
                {
                    anIsDefault = false;
                }
                if (i > this.m_MaxEnabledDay)
                {
                    anEnabled = false;
                }
                this.m_DayRoller.RowItems.Add(new DateTimePickerRow(i.ToString(), anEnabled, anIsDefault));
            }
            this.m_DayRoller.SelectedIndex = day - 1;
            this.m_DayRoller.Continues = true;
        }

        internal void InitHourRoller()
        {
            string shortTimePattern = DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
            this.InitHourRoller(shortTimePattern);
        }

        internal void InitHourRoller(string aStrTimePattern)
        {
            if (this.m_HourRoller == null)
            {
                this.m_HourRoller = new Roller();
                this.m_HourRoller.Width = 50;
                this.m_HourRoller.RollerDataType = RollerType.Hour;
            }
            this.m_HourRoller.RowItems.Clear();
            int num = aStrTimePattern.StartsWith("h") ? 12 : 0x17;
            this.m_FirstHour = (num == 12) ? 1 : 0;
            for (int i = this.m_FirstHour; i <= num; i++)
            {
                string aText = string.Format("{0:00}", i);
                this.m_HourRoller.RowItems.Add(new DateTimePickerRow(aText));
            }
            this.m_HourRoller.Continues = true;
            int num3 = (DateTime.Now.Hour > num) ? (DateTime.Now.Hour % num) : DateTime.Now.Hour;
            this.m_HourRoller.SelectedIndex = num3;
        }

        internal void InitMinutesRoller()
        {
            if (this.m_MinuteRoller == null)
            {
                this.m_MinuteRoller = new Roller();
                this.m_MinuteRoller.Width = 50;
                this.m_MinuteRoller.RollerDataType = RollerType.Minute;
            }
            this.m_MinuteRoller.RowItems.Clear();
            for (int i = 0; i < 60; i += this.m_MinuteInterval)
            {
                string aText = string.Format("{0:00}", i);
                this.m_MinuteRoller.RowItems.Add(new DateTimePickerRow(aText));
            }
            this.m_MinuteRoller.SelectedIndex = DateTime.Now.Minute / this.m_MinuteInterval;
            this.m_MinuteRoller.Continues = true;
        }

        internal void InitMonthRoller()
        {
            if (this.m_MonthRoller == null)
            {
                this.m_MonthRoller = new Roller();
                this.m_MonthRoller.Width = 100;
                this.m_MonthRoller.RollerDataType = RollerType.Month;
            }
            this.m_MonthRoller.RowItems.Clear();
            bool anEnabled = true;
            bool anIsDefault = false;
            int num = DateTime.Today.Month - 1;
            for (int i = 0; i < (DateTimeFormatInfo.CurrentInfo.MonthNames.Length - 1); i++)
            {
                string aText = DateTimeFormatInfo.CurrentInfo.MonthNames[i];
                if (num == i)
                {
                    anIsDefault = true;
                }
                else
                {
                    anIsDefault = false;
                }
                this.m_MonthRoller.RowItems.Add(new DateTimePickerRow(aText, anEnabled, anIsDefault));
            }
            this.m_MonthRoller.SelectedIndex = num;
            this.m_MonthRoller.Continues = true;
            this.m_MonthRoller.SelectedIndexChanged += new Roller.SelectedIndexChangedEventHandler(this.MonthRoller_SelectedIndexChanged);
        }

        internal void InitYearRoller()
        {
            if (this.m_YearRoller == null)
            {
                this.m_YearRoller = new Roller();
                this.m_YearRoller.Width = 60;
                this.m_YearRoller.RollerDataType = RollerType.Year;
            }
            this.m_YearRoller.RowItems.Clear();
            int year = DateTime.Today.Year;
            this.m_YearRoller.SelectedIndex = year - 0x76c;
        }

        private void MonthRoller_SelectedIndexChanged(object sender, RollerItemEventArgs e)
        {
            this.ValidDayRoller();
        }

        internal void ValidDayRoller()
        {
            if (((this.m_DayRoller != null) && (this.m_MonthRoller != null)) && (((this.m_YearRoller != null) && (this.m_YearRoller.SelectedText != null)) && (this.m_YearRoller.SelectedText.Length != 0)))
            {
                int month = this.m_MonthRoller.SelectedIndex + 1;
                int year = Convert.ToInt32(this.m_YearRoller.SelectedText);
                this.m_MaxEnabledDay = DateTimeFormatInfo.CurrentInfo.Calendar.GetDaysInMonth(year, month);
                for (int i = 0; i < this.m_DayRoller.RowItems.Count; i++)
                {
                    this.m_DayRoller.RowItems[i].Enabled = i < this.m_MaxEnabledDay;
                }
                if (this.m_DayRoller.ValidSelectedIndex())
                {
                    this.m_DayRoller.Invalidate();
                    this.m_DayRoller.OnSelectedIndexChanged();
                }
            }
        }

        public Roller AmAndPmRoller
        {
            get
            {
                return this.m_AmAndPmRoller;
            }
            set
            {
                this.m_AmAndPmRoller = value;
            }
        }

        public Roller DayRoller
        {
            get
            {
                return this.m_DayRoller;
            }
            set
            {
                this.m_DayRoller = value;
            }
        }

        public int FirstHour
        {
            get
            {
                return this.m_FirstHour;
            }
            set
            {
                this.m_FirstHour = value;
            }
        }

        public Roller HourRoller
        {
            get
            {
                return this.m_HourRoller;
            }
            set
            {
                this.m_HourRoller = value;
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
                this.InitMinutesRoller();
            }
        }

        public Roller MinuteRoller
        {
            get
            {
                return this.m_MinuteRoller;
            }
            set
            {
                this.m_MinuteRoller = value;
            }
        }

        public Roller MonthRoller
        {
            get
            {
                return this.m_MonthRoller;
            }
            set
            {
                this.m_MonthRoller = value;
            }
        }

        public Roller YearRoller
        {
            get
            {
                return this.m_YearRoller;
            }
            set
            {
                this.m_YearRoller = value;
            }
        }
    }
}

