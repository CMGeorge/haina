namespace Resco.Controls.OutlookControls
{
    using System;

    public class ContentsChangedEventArgs : EventArgs
    {
        private ContentsChangedType m_ContentsChangedType;

        public ContentsChangedEventArgs(ContentsChangedType aContentsChangedType)
        {
            this.m_ContentsChangedType = aContentsChangedType;
        }

        public ContentsChangedType ChangedBy
        {
            get
            {
                return this.m_ContentsChangedType;
            }
        }
    }
}

