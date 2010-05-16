namespace Resco.Controls.OutlookControls
{
    using System;

    public class InsertEventArgs : EventArgs
    {
        public int index;
        public object value;

        public InsertEventArgs(int i, object v)
        {
            this.index = i;
            this.value = v;
        }
    }
}

