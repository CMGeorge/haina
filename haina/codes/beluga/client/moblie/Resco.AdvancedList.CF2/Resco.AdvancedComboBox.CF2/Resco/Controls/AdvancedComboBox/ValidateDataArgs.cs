namespace Resco.Controls.AdvancedComboBox
{
    using System;

    public class ValidateDataArgs
    {
        public int InsertIndex;
        public Resco.Controls.AdvancedComboBox.ListItem ListItem;
        public bool Skip = false;

        public ValidateDataArgs(Resco.Controls.AdvancedComboBox.ListItem item, int insertIndex)
        {
            this.ListItem = item;
            this.InsertIndex = insertIndex;
        }
    }
}

