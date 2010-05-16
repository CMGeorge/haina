namespace Resco.Controls.DetailView
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    internal class BackBufferManager
    {
        private static Bitmap m_bmpBackBuffer = null;
        private static Graphics m_grBackBuffer = null;
        private static int m_refCount = 0;
        private static Size m_screenSize = Screen.PrimaryScreen.Bounds.Size;

        public static void AddRef()
        {
            m_refCount++;
        }

        public static void Dispose()
        {
            if (m_bmpBackBuffer != null)
            {
                m_bmpBackBuffer.Dispose();
            }
            m_bmpBackBuffer = null;
            if (m_grBackBuffer != null)
            {
                m_grBackBuffer.Dispose();
            }
            m_grBackBuffer = null;
        }

        ~BackBufferManager()
        {
            Dispose();
        }

        public static Graphics GetBackBufferGraphics(int width, int height)
        {
            RealocateBackBuffer(width, height);
            return m_grBackBuffer;
        }

        public static Bitmap GetBackBufferImage(int width, int height)
        {
            RealocateBackBuffer(width, height);
            return m_bmpBackBuffer;
        }

        public static void RealocateBackBuffer(int width, int height)
        {
            int num = (width > m_screenSize.Width) ? width : m_screenSize.Width;
            int num2 = (height > m_screenSize.Height) ? height : m_screenSize.Height;
            if (((m_grBackBuffer == null) || (m_bmpBackBuffer == null)) || ((num > m_bmpBackBuffer.Width) || (num2 > m_bmpBackBuffer.Height)))
            {
                Dispose();
                try
                {
                    m_bmpBackBuffer = new Bitmap(num, num2);
                }
                catch
                {
                    m_bmpBackBuffer = new Bitmap(num, num2, PixelFormat.Format16bppRgb565);
                }
                m_grBackBuffer = Graphics.FromImage(m_bmpBackBuffer);
            }
        }

        public static void Release()
        {
            if (--m_refCount <= 0)
            {
                Dispose();
            }
        }
    }
}

