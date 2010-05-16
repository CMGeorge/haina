namespace Resco.Controls.OutlookControls
{
    using System;

    public class SetEventArgs : EventArgs
    {
        public int index;
        public object newValue;
        public object oldValue;

        public SetEventArgs(int i, object oldV, object newV)
        {
            this.index = i;
            this.oldValue = oldV;
            this.newValue = newV;
        }
    }
}

