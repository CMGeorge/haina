namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal class ConvertToGrayscaleInBackgrnd : IDisposable
    {
        private Bitmap m_BmpDest;
        private object m_Mutex = new object();
        private bool m_StopWorkerThread;
        //private int m_ThreadCounter;
        private Thread m_WorkThread;

        public event EventHandler ConversionCanceled;

        public event EventHandler ConversionDone;

        public void Convert(Bitmap aBmp, ThreadPriority aPriority)
        {
            this.StopConversion();
            if (this.m_BmpDest != null)
            {
                this.m_BmpDest.Dispose();
                this.m_BmpDest = null;
            }
            if (aBmp != null)
            {
                this.m_BmpDest = new Bitmap(aBmp);
                if (this.m_BmpDest != null)
                {
                    this.m_StopWorkerThread = false;
                    this.m_WorkThread = new Thread(new ThreadStart(this.ThreadTask));
                    this.m_WorkThread.IsBackground = true;
                    this.m_WorkThread.Priority = aPriority;
                    this.m_WorkThread.Start();
                }
            }
        }

        private void ConvertToGrayBmp2(Bitmap aGrayBmp)
        {
            Rectangle rect = new Rectangle(0, 0, aGrayBmp.Width, aGrayBmp.Height);
            BitmapData bitmapdata = aGrayBmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            IntPtr source = bitmapdata.Scan0;
            int length = bitmapdata.Stride * bitmapdata.Height;
            byte[] destination = new byte[length];
            Marshal.Copy(source, destination, 0, length);
            int num5 = destination[0];
            int num6 = destination[1];
            int num7 = destination[2];
            for (int i = 0; i < bitmapdata.Height; i++)
            {
                for (int j = 0; (j + 2) < bitmapdata.Stride; j += 3)
                {
                    if (this.m_StopWorkerThread)
                    {
                        i = bitmapdata.Height;
                        break;
                    }
                    int index = (i * bitmapdata.Stride) + j;
                    int num2 = destination[index];
                    int num3 = destination[index + 1];
                    int num4 = destination[index + 2];
                    if (((num5 != num2) || (num6 != num3)) || (num7 != num4))
                    {
                        byte num8 = (byte) (((0.3 * num2) + (0.59 * num3)) + (0.11 * num4));
                        destination[index] = num8;
                        destination[index + 1] = num8;
                        destination[index + 2] = num8;
                    }
                }
            }
            if (!this.m_StopWorkerThread)
            {
                Marshal.Copy(destination, 0, source, length);
            }
            try
            {
                aGrayBmp.UnlockBits(bitmapdata);
            }
            catch (Exception)
            {
            }
        }

        public void Dispose()
        {
            this.StopConversion();
            if (this.m_BmpDest != null)
            {
                this.m_BmpDest.Dispose();
                this.m_BmpDest = null;
            }
        }

        private void OnConversionCanceled()
        {
            if (this.ConversionCanceled != null)
            {
                this.ConversionCanceled(this, EventArgs.Empty);
            }
        }

        private void OnConversionDone()
        {
            if (this.ConversionDone != null)
            {
                this.ConversionDone(this, EventArgs.Empty);
            }
        }

        private void RefreshControlAfterConvert()
        {
            this.OnConversionDone();
        }

        public void StopConversion()
        {
            if (this.m_WorkThread != null)
            {
                this.m_StopWorkerThread = true;
                this.m_WorkThread.Join(0x3e8);
            }
        }

        private void ThreadTask()
        {
            try
            {
                lock (this.m_Mutex)
                {
                    if (this.m_BmpDest == null)
                    {
                        return;
                    }
                    this.ConvertToGrayBmp2(this.m_BmpDest);
                }
                if (!this.m_StopWorkerThread)
                {
                    this.RefreshControlAfterConvert();
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public Bitmap BmpGray
        {
            get
            {
                return this.m_BmpDest;
            }
            set
            {
                this.m_BmpDest = value;
            }
        }

        private delegate void InvokeDelegate();
    }
}

