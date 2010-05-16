namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Drawing;

    internal class GetThumbnail
    {
        private static ZoomPercent m_Zoom = ZoomPercent.Zoom100;

        internal GetThumbnail()
        {
        }

        internal static Bitmap GetThumbnailBmp(Bitmap fullSizeImage, ZoomPercent aZoomValue)
        {
            m_Zoom = aZoomValue;
            int height = fullSizeImage.Height;
            int width = fullSizeImage.Width;
            int aPreviewHeight = (int) (ZoomValue * height);
            int aPreviewWidth = (int) (ZoomValue * width);
            return GetThumbnailBmp(fullSizeImage, aPreviewWidth, aPreviewHeight);
        }

        internal static Bitmap GetThumbnailBmp(Bitmap fullSizeImage, int aPreviewWidth, int aPreviewHeight)
        {
            int num = aPreviewWidth;
            int num2 = aPreviewHeight;
            decimal width = fullSizeImage.Width;
            decimal height = fullSizeImage.Height;
            decimal num5 = Math.Min((decimal) (num / width), (decimal) (num2 / height));
            decimal num6 = num5 * width;
            decimal num7 = num5 * height;
            return GetThumbnailImage(fullSizeImage, (int) num7, (int) num6);
        }

        private static Bitmap GetThumbnailImage(Bitmap aBitmap, int aWidth, int aHeight)
        {
            Bitmap image = new Bitmap(aHeight, aWidth);
            Graphics graphics = Graphics.FromImage(image);
            graphics.DrawImage(aBitmap, new Rectangle(0, 0, aHeight, aWidth), new Rectangle(0, 0, aBitmap.Width, aBitmap.Height), GraphicsUnit.Pixel);
            graphics.Dispose();
            graphics = null;
            return image;
        }

        internal static ZoomPercent Zoom
        {
            get
            {
                return m_Zoom;
            }
            private set
            {
                if (m_Zoom != value)
                {
                    m_Zoom = value;
                }
            }
        }

        private static float ZoomValue
        {
            get
            {
                return (((float) (m_Zoom + 1)) * 0.25f);
            }
        }
    }
}

