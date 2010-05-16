namespace Resco.Controls.AdvancedList
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    internal class BackBufferManager
    {
        private static Bitmap m_bmpBackBuffer = null;
        private static Bitmap m_bmpGradient = null;
        private static Bitmap m_bmpTmp = null;
        private static Graphics m_grBackBuffer = null;
        private static Graphics m_grGradient = null;
        private static Graphics m_grTmp = null;
        private static WeakReference m_lastDrawControl;
        private static WeakReference m_lastDrawControlGradient;
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
            if (m_bmpTmp != null)
            {
                m_bmpTmp.Dispose();
            }
            m_bmpTmp = null;
            if (m_grTmp != null)
            {
                m_grTmp.Dispose();
            }
            m_grTmp = null;
        }

        public static void DisposeGradient()
        {
            if (m_bmpGradient != null)
            {
                m_bmpGradient.Dispose();
            }
            m_bmpGradient = null;
            if (m_grGradient != null)
            {
                m_grGradient.Dispose();
            }
            m_grGradient = null;
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

        public static Graphics GetGradientGraphics(int width, int height)
        {
            RealocateGradient(width, height);
            return m_grGradient;
        }

        public static Bitmap GetGradientImage(int width, int height)
        {
            RealocateGradient(width, height);
            return m_bmpGradient;
        }

        public static Graphics GetTempGraphics(int width, int height)
        {
            RealocateBackBuffer(width, height);
            return m_grTmp;
        }

        public static Bitmap GetTempImage(int width, int height)
        {
            RealocateBackBuffer(width, height);
            return m_bmpTmp;
        }

        public static bool IsValid(Control control)
        {
            if ((m_lastDrawControl != null) && m_lastDrawControl.IsAlive)
            {
                if (m_lastDrawControl.Target == control)
                {
                    return true;
                }
                m_lastDrawControl.Target = control;
                return false;
            }
            m_lastDrawControl = new WeakReference(control, false);
            return false;
        }

        public static bool IsValidGradient(Control control)
        {
            if ((m_lastDrawControlGradient != null) && m_lastDrawControlGradient.IsAlive)
            {
                if (m_lastDrawControlGradient.Target == control)
                {
                    return true;
                }
                m_lastDrawControlGradient.Target = control;
                return false;
            }
            m_lastDrawControlGradient = new WeakReference(control, false);
            return false;
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
                try
                {
                    m_bmpTmp = new Bitmap(num, num2);
                }
                catch
                {
                    m_bmpTmp = new Bitmap(num, num2, PixelFormat.Format16bppRgb565);
                }
                m_grTmp = Graphics.FromImage(m_bmpTmp);
            }
        }

        public static void RealocateGradient(int width, int height)
        {
            int num = (width > m_screenSize.Width) ? width : m_screenSize.Width;
            int num2 = (height > m_screenSize.Height) ? height : m_screenSize.Height;
            if (((m_grGradient == null) || (m_bmpGradient == null)) || ((num > m_bmpGradient.Width) || (num2 > m_bmpGradient.Height)))
            {
                DisposeGradient();
                try
                {
                    m_bmpGradient = new Bitmap(num, num2);
                }
                catch
                {
                    m_bmpGradient = new Bitmap(num, num2, PixelFormat.Format16bppRgb565);
                }
                m_grGradient = Graphics.FromImage(m_bmpGradient);
            }
        }

        public static void Release()
        {
            if (--m_refCount <= 0)
            {
                DisposeGradient();
                Dispose();
            }
        }
    }
}

