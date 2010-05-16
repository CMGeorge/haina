namespace Resco.Controls.DetailView
{
    using System;
    using System.ComponentModel;

    public class PageChangeEventArgs : CancelEventArgs
    {
        public readonly int NewPageIndex;
        public readonly int OldPageIndex;

        public PageChangeEventArgs(int oldIndex, int newIndex) : base(false)
        {
            this.OldPageIndex = oldIndex;
            this.NewPageIndex = newIndex;
        }
    }
}

