namespace Resco.Controls.AdvancedTree
{
    using System;

    internal class TreeResizeEventArgs : TreeChangedEventArgs
    {
        public static readonly TreeResizeEventArgs Empty = new TreeResizeEventArgs();
        private int m_offset;

        public TreeResizeEventArgs() : this(0)
        {
        }

        public TreeResizeEventArgs(int offset) : base(TreeEventArgsType.Resize, null)
        {
            this.m_offset = offset;
        }

        public int Offset
        {
            get
            {
                return this.m_offset;
            }
            set
            {
                this.m_offset = value;
            }
        }
    }
}

