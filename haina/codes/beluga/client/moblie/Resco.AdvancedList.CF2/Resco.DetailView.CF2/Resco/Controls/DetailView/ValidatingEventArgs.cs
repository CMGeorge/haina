namespace Resco.Controls.DetailView
{
    using System;
    using System.ComponentModel;

    public class ValidatingEventArgs : CancelEventArgs
    {
        public Item item;
        public string NewText;
        public object NewValue;
        public readonly bool TextToValue;

        public ValidatingEventArgs(Item i, object newValue, string newText, bool textToValue) : base(false)
        {
            this.item = i;
            this.NewValue = newValue;
            this.NewText = newText;
            this.TextToValue = textToValue;
        }

        public object OldText
        {
            get
            {
                return this.item.Text;
            }
        }

        public object OldValue
        {
            get
            {
                return this.item.Value;
            }
        }
    }
}

