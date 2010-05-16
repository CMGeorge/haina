namespace Utils
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    public class Tester
    {
        private static int m_relaTime;

        [Conditional("DEBUG")]
        public static void StartTimer()
        {
            m_relaTime = Environment.TickCount;
        }

        public static int StopTimer()
        {
            return (Environment.TickCount - m_relaTime);
        }

        [Conditional("DEBUG")]
        public static void StopTimer(string aTitle)
        {
            MessageBox.Show(aTitle + ((Environment.TickCount - m_relaTime)).ToString() + " ms");
        }
    }
}

