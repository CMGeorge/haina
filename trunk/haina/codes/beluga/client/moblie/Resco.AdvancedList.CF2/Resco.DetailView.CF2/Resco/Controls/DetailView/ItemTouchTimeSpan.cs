namespace Resco.Controls.DetailView
{
    using Resco.Controls.DetailView.DetailViewInternal;
    using System;

    public class ItemTouchTimeSpan : ItemTimeSpan
    {
        public ItemTouchTimeSpan()
        {
        }

        public ItemTouchTimeSpan(Item toCopy) : base(toCopy)
        {
        }

        public ItemTouchTimeSpan(string label) : base(label)
        {
        }

        internal override Type GetDateTimePickerType()
        {
            return DateTimePickerInterface.GetTouchDateTimePickerType();
        }

        public override bool ShowTimeNone
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public override bool ShowTimeUpDown
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
    }
}

