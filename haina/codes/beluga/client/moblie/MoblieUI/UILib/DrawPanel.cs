using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace UILib
{
    public class DrawPanel:Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Pen pen = new Pen(_borderColor, _borderWidth);
            pen.DashStyle = _borderDashStyle;
            g.DrawRectangle(pen, e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
        }

        Color _borderColor = Color.Black;
        /**/
        /// <summary>
        /// ��ȡ�����ñ߿���ɫ
        /// </summary>
        public virtual Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        float _borderWidth = 1f;
        /**/
        /// <summary>
        /// ��ȡ�����ñ߿�Ŀ��
        /// </summary>
        public virtual float BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }

        DashStyle _borderDashStyle = DashStyle.Solid;
        /**/
        /// <summary>
        /// ��ȡ�����ñ߿����ʽ
        /// </summary>
        public DashStyle BorderDashStyle
        {
            get { return _borderDashStyle; }
            set { _borderDashStyle = value; }
        }

    }
}
