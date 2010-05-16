namespace Resco.Controls.OutlookControls
{
    using System;

    public class DateTimePickerRow
    {
        private bool m_Enabled;
        private bool m_IsDefault;
        private string m_Text;

        public DateTimePickerRow()
        {
            this.Text = string.Empty;
            this.Enabled = false;
            this.IsDefault = false;
        }

        public DateTimePickerRow(string aText)
        {
            this.Text = aText;
            this.Enabled = true;
            this.IsDefault = false;
        }

        public DateTimePickerRow(string aText, bool anEnabled, bool anIsDefault)
        {
            this.Text = aText;
            this.Enabled = anEnabled;
            this.IsDefault = anIsDefault;
        }

        public bool Enabled
        {
            get
            {
                return this.m_Enabled;
            }
            set
            {
                this.m_Enabled = value;
            }
        }

        public bool IsDefault
        {
            get
            {
                return this.m_IsDefault;
            }
            set
            {
                this.m_IsDefault = value;
            }
        }

        public string Text
        {
            get
            {
                return this.m_Text;
            }
            set
            {
                this.m_Text = value;
            }
        }
    }
}

