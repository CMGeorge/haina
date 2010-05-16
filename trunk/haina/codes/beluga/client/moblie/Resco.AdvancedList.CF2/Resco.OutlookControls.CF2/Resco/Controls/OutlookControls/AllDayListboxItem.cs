namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;

    internal class AllDayListboxItem
    {
        private bool m_NeedToolTip;
        private Color m_RowsBackcolor;
        private string m_Subject;
        private string m_TooltipSubjects;

        public AllDayListboxItem(string aSubject, Color aRowsBackcolor)
        {
            this.m_Subject = aSubject;
            this.m_TooltipSubjects = aSubject;
            this.m_RowsBackcolor = aRowsBackcolor;
        }

        public bool NeedToolTip
        {
            get
            {
                return this.m_NeedToolTip;
            }
            set
            {
                this.m_NeedToolTip = value;
            }
        }

        public Color RowsBackcolor
        {
            get
            {
                return this.m_RowsBackcolor;
            }
        }

        public string Subject
        {
            get
            {
                return this.m_Subject;
            }
            set
            {
                this.m_Subject = value;
            }
        }

        public string TooltipSubjects
        {
            get
            {
                return this.m_TooltipSubjects;
            }
        }
    }
}

