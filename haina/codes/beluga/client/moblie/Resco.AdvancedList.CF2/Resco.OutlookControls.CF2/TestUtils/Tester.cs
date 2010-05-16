namespace TestUtils
{
    using System;
    using System.Diagnostics;

    internal class Tester
    {
        private static int m_relaTime;

        [Conditional("DEBUG")]
        public static void StartTimer()
        {
            m_relaTime = Environment.TickCount;
        }

        public static string StopTimer()
        {
            int num = Environment.TickCount - m_relaTime;
            return (num.ToString() + " ms");
        }
    }
}

