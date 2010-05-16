namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class ComboBoxScrollArgs : ComboBoxArgs
    {
        public int Dif;
        public int Index;

        public ComboBoxScrollArgs() : base(ComboBoxUpdateRange.List)
        {
            this.Dif = 0;
            this.Index = -1;
        }

        public ComboBoxScrollArgs(int d) : base(ComboBoxUpdateRange.List)
        {
            this.Dif = d;
        }

        public ComboBoxScrollArgs(int d, int i) : base(ComboBoxUpdateRange.List)
        {
            this.Dif = d;
            this.Index = i;
        }
    }
}

