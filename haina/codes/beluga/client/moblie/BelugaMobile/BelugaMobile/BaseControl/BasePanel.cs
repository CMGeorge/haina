using System;

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace BelugaMobile.BaseControl
{
    class BasePanel : System.Windows.Forms.Panel, IControlBackground
    {
        Bitmap background;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (background != null)
                e.Graphics.DrawImage(background, 0, 0);
            else
                base.OnPaint(e);
        }

        public Bitmap BackgroundImage
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
            }
        }
    }
}
