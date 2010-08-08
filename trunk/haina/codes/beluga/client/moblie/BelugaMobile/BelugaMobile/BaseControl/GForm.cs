using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace BelugaMobile.Control
{
    public interface IControlBackground
    {
        Bitmap BackgroundImage { get; }
    }

    public class GForm : Form,IControlBackground
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
