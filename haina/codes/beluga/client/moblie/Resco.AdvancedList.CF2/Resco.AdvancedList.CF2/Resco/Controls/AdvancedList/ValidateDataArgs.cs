namespace Resco.Controls.AdvancedList
{
    using System;

    public class ValidateDataArgs
    {
        public Row DataRow;
        public int InsertIndex;
        public bool Skip = false;

        public ValidateDataArgs(Row r, int insertIndex)
        {
            this.DataRow = r;
            this.InsertIndex = insertIndex;
        }
    }
}

