namespace System.Media
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class SystemSounds
    {
        private static SystemSounds m_asterisk;
        private static SystemSounds m_beep;
        private static SystemSounds m_exclamation;
        private static SystemSounds m_hand;
        private static SystemSounds m_question;
        private MessageBoxIcon m_soundType;

        internal SystemSounds(MessageBoxIcon soundType)
        {
            this.m_soundType = soundType;
        }

        [DllImport("coredll.dll", CharSet=CharSet.Auto)]
        private static extern int MessageBeep(int type);
        internal void Play()
        {
            MessageBeep((int) this.m_soundType);
        }

        internal static SystemSounds Asterisk
        {
            get
            {
                if (m_asterisk == null)
                {
                    m_asterisk = new SystemSounds(MessageBoxIcon.Asterisk);
                }
                return m_asterisk;
            }
        }

        internal static SystemSounds Beep
        {
            get
            {
                if (m_beep == null)
                {
                    m_beep = new SystemSounds(MessageBoxIcon.None);
                }
                return m_beep;
            }
        }

        internal static SystemSounds Exclamation
        {
            get
            {
                if (m_exclamation == null)
                {
                    m_exclamation = new SystemSounds(MessageBoxIcon.Exclamation);
                }
                return m_exclamation;
            }
        }

        internal static SystemSounds Hand
        {
            get
            {
                if (m_hand == null)
                {
                    m_hand = new SystemSounds(MessageBoxIcon.Hand);
                }
                return m_hand;
            }
        }

        internal static SystemSounds Question
        {
            get
            {
                if (m_question == null)
                {
                    m_question = new SystemSounds(MessageBoxIcon.Question);
                }
                return m_question;
            }
        }
    }
}

