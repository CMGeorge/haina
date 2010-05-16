namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class ComboBoxRefreshArgs : ComboBoxArgs
    {
        public bool ResetBounds;
        public ItemTemplate Template;

        public ComboBoxRefreshArgs() : this(null, false)
        {
        }

        public ComboBoxRefreshArgs(ComboBoxUpdateRange cbur) : this(null, false, cbur)
        {
        }

        public ComboBoxRefreshArgs(ItemTemplate it) : this(it, true)
        {
        }

        public ComboBoxRefreshArgs(bool bResetBounds) : this(null, bResetBounds)
        {
        }

        public ComboBoxRefreshArgs(ItemTemplate it, ComboBoxUpdateRange cbur) : this(it, true, cbur)
        {
        }

        private ComboBoxRefreshArgs(ItemTemplate it, bool bResetBounds)
        {
            this.Template = it;
            this.ResetBounds = true;
        }

        public ComboBoxRefreshArgs(bool bResetBounds, ComboBoxUpdateRange cbur) : this(null, bResetBounds, cbur)
        {
        }

        private ComboBoxRefreshArgs(ItemTemplate it, bool bResetBounds, ComboBoxUpdateRange cbur) : base(cbur)
        {
            this.Template = it;
            this.ResetBounds = bResetBounds;
        }
    }
}

