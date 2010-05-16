namespace Resco.Controls.DetailView.DetailViewInternal
{
    using System;
    using System.Drawing;

    internal class TextPart
    {
        public string Format;
        public bool IsEditable;
        public System.Drawing.Size Size;
        public string Text;

        public TextPart(Graphics gr, DateTime date, string format, Font f)
        {
            this.IsEditable = this.IsDatePart(format);
            this.Format = this.CorrectFormat(format);
            this.Size = gr.MeasureString(this.ToString(date), f).ToSize();
            this.Text = this.ToString(date);
        }

        private string CorrectFormat(string f)
        {
            if ((f.Length == 1) && this.IsEditable)
            {
                return (" " + f);
            }
            if ((f[0] == '\x00ff') && (f[f.Length - 1] == '\x00ff'))
            {
                return f.Substring(1, f.Length - 2);
            }
            return f;
        }

        private bool IsDatePart(string f)
        {
            if (f[0] == '\x00ff')
            {
                return false;
            }
            return ((f.IndexOf("d") >= 0) || ((f.IndexOf("M") >= 0) || ((f.IndexOf("y") >= 0) || ((f.IndexOf("m") >= 0) || ((f.IndexOf("h") >= 0) || ((f.IndexOf("H") >= 0) || ((f.IndexOf("s") >= 0) || (f.IndexOf("t") >= 0))))))));
        }

        private string ToString(DateTime date)
        {
            if (!this.IsEditable)
            {
                return this.Format;
            }
            return date.ToString(this.Format).Trim();
        }
    }
}

