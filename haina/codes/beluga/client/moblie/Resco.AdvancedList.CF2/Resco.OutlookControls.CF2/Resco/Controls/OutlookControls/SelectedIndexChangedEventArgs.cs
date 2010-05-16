namespace Resco.Controls.OutlookControls
{
    using System;

    public class SelectedIndexChangedEventArgs : EventArgs
    {
        private int m_groupIndex;
        private SelectionType m_selType;
        private int m_shortcutIndex;

        public SelectedIndexChangedEventArgs(int groupIndex, int shortcutIndex)
        {
            this.m_groupIndex = groupIndex;
            this.m_shortcutIndex = shortcutIndex;
        }

        public SelectedIndexChangedEventArgs(int groupIndex, int shortcutIndex, SelectionType aSelectionType)
        {
            this.m_groupIndex = groupIndex;
            this.m_shortcutIndex = shortcutIndex;
            this.m_selType = aSelectionType;
        }

        public int GroupIndex
        {
            get
            {
                return this.m_groupIndex;
            }
        }

        public SelectionType SelectionAction
        {
            get
            {
                return this.m_selType;
            }
        }

        public int ShortcutIndex
        {
            get
            {
                return this.m_shortcutIndex;
            }
        }

        public enum SelectionType
        {
            ByMouse,
            ByKeyboard
        }
    }
}

