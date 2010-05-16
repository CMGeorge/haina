namespace Resco.Controls.DetailView
{
    using System;

    public class ItemEventArgs
    {
        public int Index;
        public Item item;
        public string Name;

        public ItemEventArgs()
        {
            this.item = null;
            this.Index = 0;
            this.Name = null;
        }

        public ItemEventArgs(Item i, int index, string name)
        {
            this.item = i;
            this.Index = index;
            this.Name = name;
        }
    }
}

