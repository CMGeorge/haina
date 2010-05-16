namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class ComboBoxIndexChangedArgs : ComboBoxArgs
    {
        public int OldTemplateIndex;

        public ComboBoxIndexChangedArgs()
        {
            this.OldTemplateIndex = -1;
        }

        public ComboBoxIndexChangedArgs(int oti)
        {
            this.OldTemplateIndex = oti;
        }
    }
}

