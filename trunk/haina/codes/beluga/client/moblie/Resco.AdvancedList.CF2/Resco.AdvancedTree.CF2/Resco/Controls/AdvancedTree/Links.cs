namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Collections;

    internal class Links
    {
        private static Hashtable sVisited = new Hashtable();

        public static void AddLink(string target)
        {
            if (((target != null) && (target.Length > 0)) && !sVisited.Contains(target))
            {
                sVisited.Add(target, null);
            }
        }

        public static bool IsVisited(string target)
        {
            return sVisited.Contains(target);
        }

        public static Hashtable Visited
        {
            get
            {
                return sVisited;
            }
        }
    }
}

