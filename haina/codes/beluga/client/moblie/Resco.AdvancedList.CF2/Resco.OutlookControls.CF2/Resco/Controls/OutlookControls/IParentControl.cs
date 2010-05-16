namespace Resco.Controls.OutlookControls
{
    using System;

    internal interface IParentControl
    {
        void ForceRefresh();
        DateTime GetMaxDate();
        DateTime GetMinDate();
        DateTimePickerRow GetYearText(int aRowIndex);
    }
}

