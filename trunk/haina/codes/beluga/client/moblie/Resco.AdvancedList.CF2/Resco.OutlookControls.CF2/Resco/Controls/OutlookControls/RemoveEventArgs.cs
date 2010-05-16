namespace Resco.Controls.OutlookControls
{
    using System;

    public class RemoveEventArgs : EventArgs
    {
        public int index;
        public object value;

        public RemoveEventArgs(int i, object v)
        {
            this.index = i;
            this.value = v;
        }
    }
}

