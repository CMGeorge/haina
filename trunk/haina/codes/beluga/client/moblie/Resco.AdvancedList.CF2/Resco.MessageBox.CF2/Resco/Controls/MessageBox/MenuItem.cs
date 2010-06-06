namespace Resco.Controls.MessageBox
{
    using System;
    using System.Drawing;

    public class MenuItem
    {
        private bool enabled;
        private Bitmap image;
        private object result;
        private string text;

        public MenuItem(string text, object result)
        {
            this.Enabled = true;
            this.Text = text;
            this.Result = result;
        }

        public void Set(string text, object result)
        {
            this.Text = text;
            this.Result = result;
        }

        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled= value;
            }
        }

        public Bitmap Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
            }
        }

        public object Result
        {
            get
            {
                return this.result;
            }
            private set
            {
                this.result = value;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
    }
}

