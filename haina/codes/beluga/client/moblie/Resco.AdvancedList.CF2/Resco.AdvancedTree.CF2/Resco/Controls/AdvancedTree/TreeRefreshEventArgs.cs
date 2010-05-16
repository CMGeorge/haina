namespace Resco.Controls.AdvancedTree
{
    using System;

    internal class TreeRefreshEventArgs : TreeChangedEventArgs
    {
        public bool ResetBounds;
        public NodeTemplate Template;

        public TreeRefreshEventArgs() : this(null, false)
        {
        }

        public TreeRefreshEventArgs(NodeTemplate nt) : this(nt, true)
        {
        }

        public TreeRefreshEventArgs(bool bResetBounds) : this(null, bResetBounds)
        {
        }

        private TreeRefreshEventArgs(NodeTemplate nt, bool bResetBounds) : base(TreeEventArgsType.Refresh, null)
        {
            this.Template = nt;
            this.ResetBounds = bResetBounds;
        }
    }
}

