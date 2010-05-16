namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    internal class ImageCache
    {
        private static ImageCache m_gc = new ImageCache();
        private Hashtable m_htIls = new Hashtable();

        public void Clear()
        {
            foreach (Hashtable hashtable in this.m_htIls.Values)
            {
                foreach (Image image in hashtable.Values)
                {
                    image.Dispose();
                }
                hashtable.Clear();
            }
            this.m_htIls.Clear();
        }

        public void Clear(ImageList il)
        {
            if (this.m_htIls.Contains(il))
            {
                Hashtable hashtable = (Hashtable) this.m_htIls[il];
                foreach (Image image in hashtable.Values)
                {
                    image.Dispose();
                }
                hashtable.Clear();
                this.m_htIls.Remove(il);
            }
        }

        public static ImageCache GlobalCache
        {
            get
            {
                return m_gc;
            }
        }

        public Image this[ImageList il, int index]
        {
            get
            {
                Hashtable hashtable = null;
                Image image = null;
                hashtable = this.m_htIls[il] as Hashtable;
                if (hashtable != null)
                {
                    image = hashtable[index] as Image;
                }
                if (image == null)
                {
                    image = il.Images[index];
                    if (hashtable == null)
                    {
                        hashtable = new Hashtable(il.Images.Count);
                        this.m_htIls.Add(il, hashtable);
                    }
                    hashtable.Add(index, image);
                }
                return image;
            }
        }
    }
}

