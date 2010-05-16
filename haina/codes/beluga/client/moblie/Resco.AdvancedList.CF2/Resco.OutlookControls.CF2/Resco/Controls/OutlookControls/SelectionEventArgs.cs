namespace Resco.Controls.OutlookControls
{
    using System;

    public class SelectionEventArgs : EventArgs
    {
        private SelectionType m_selType = SelectionType.ByKeyboard;

        public SelectionEventArgs(SelectionType aSelectionType)
        {
            this.m_selType = aSelectionType;
        }

        public SelectionType SelectionAction
        {
            get
            {
                return this.m_selType;
            }
        }

        public enum SelectionType
        {
            ByMouse,
            ByKeyboard
        }
    }
}

