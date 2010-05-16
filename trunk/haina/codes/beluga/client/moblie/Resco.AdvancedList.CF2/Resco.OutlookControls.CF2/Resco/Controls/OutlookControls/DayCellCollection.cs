namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class DayCellCollection : CollectionBase
    {
        public int Add(Resco.Controls.OutlookControls.DayCell value)
        {
            return base.List.Add(value);
        }

        public void Clear(BoldedDateType type)
        {
            for (int i = 0; i < base.List.Count; i++)
            {
                if (((Resco.Controls.OutlookControls.DayCell) base.List[i]).Type == type)
                {
                    base.List.RemoveAt(i);
                    i--;
                }
            }
        }

        public bool Contains(Resco.Controls.OutlookControls.DayCell value)
        {
            return base.List.Contains(value);
        }

        internal IList GetBoldedDates(BoldedDateType type)
        {
            ArrayList list = new ArrayList();
            foreach (Resco.Controls.OutlookControls.DayCell cell in base.List)
            {
                if (cell.Type == type)
                {
                    list.Add(cell);
                }
            }
            return list;
        }

        internal IList GetBoldedDates(DateTime date)
        {
            ArrayList list = new ArrayList();
            foreach (Resco.Controls.OutlookControls.DayCell cell in base.List)
            {
                switch (cell.Type)
                {
                    case BoldedDateType.Monthly:
                    {
                        if (date.Day == cell.Date.Day)
                        {
                            list.Add(cell);
                        }
                        continue;
                    }
                    case BoldedDateType.Annually:
                    {
                        if ((date.Day == cell.Date.Day) && (date.Month == cell.Date.Month))
                        {
                            list.Add(cell);
                        }
                        continue;
                    }
                }
                if (((date.Day == cell.Date.Day) && (date.Month == cell.Date.Month)) && (date.Year == cell.Date.Year))
                {
                    list.Add(cell);
                }
            }
            return list;
        }

        public int IndexOf(Resco.Controls.OutlookControls.DayCell value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, Resco.Controls.OutlookControls.DayCell value)
        {
            base.List.Insert(index, value);
        }

        public void Remove(Resco.Controls.OutlookControls.DayCell value)
        {
            base.List.Remove(value);
        }

        public Resco.Controls.OutlookControls.DayCell[] All
        {
            get
            {
                Resco.Controls.OutlookControls.DayCell[] array = new Resco.Controls.OutlookControls.DayCell[base.List.Count];
                base.List.CopyTo(array, 0);
                return array;
            }
            set
            {
                base.List.Clear();
                foreach (Resco.Controls.OutlookControls.DayCell cell in value)
                {
                    this.Add(cell);
                }
            }
        }

        public Resco.Controls.OutlookControls.DayCell this[int index]
        {
            get
            {
                if ((index < 0) || (index >= base.List.Count))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return (Resco.Controls.OutlookControls.DayCell) base.List[index];
            }
            set
            {
                if ((index < 0) || (index >= base.List.Count))
                {
                    throw new ArgumentOutOfRangeException();
                }
                base.List[index] = value;
            }
        }

        public Resco.Controls.OutlookControls.DayCell[] this[DateTime date]
        {
            get
            {
                IList boldedDates = this.GetBoldedDates(date);
                Resco.Controls.OutlookControls.DayCell[] cellArray = new Resco.Controls.OutlookControls.DayCell[boldedDates.Count];
                for (int i = 0; i < boldedDates.Count; i++)
                {
                    cellArray[i] = (Resco.Controls.OutlookControls.DayCell) boldedDates[i];
                }
                return cellArray;
            }
        }
    }
}

