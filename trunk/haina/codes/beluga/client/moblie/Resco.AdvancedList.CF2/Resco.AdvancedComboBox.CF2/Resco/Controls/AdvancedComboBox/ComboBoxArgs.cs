namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class ComboBoxArgs : EventArgs
    {
        private static ComboBoxArgs sBox;
        private static ComboBoxArgs sDefault;
        private static ComboBoxArgs sList;
        public ComboBoxUpdateRange UpdateRange;

        public ComboBoxArgs()
        {
            this.UpdateRange = ComboBoxUpdateRange.All;
        }

        public ComboBoxArgs(ComboBoxUpdateRange cbur)
        {
            this.UpdateRange = cbur;
        }

        public static ComboBoxArgs Box
        {
            get
            {
                if (sBox == null)
                {
                    sBox = new ComboBoxArgs(ComboBoxUpdateRange.Box);
                }
                return sBox;
            }
        }

        public static ComboBoxArgs Default
        {
            get
            {
                if (sDefault == null)
                {
                    sDefault = new ComboBoxArgs();
                }
                return sDefault;
            }
        }

        public static ComboBoxArgs List
        {
            get
            {
                if (sList == null)
                {
                    sList = new ComboBoxArgs(ComboBoxUpdateRange.List);
                }
                return sList;
            }
        }
    }
}

