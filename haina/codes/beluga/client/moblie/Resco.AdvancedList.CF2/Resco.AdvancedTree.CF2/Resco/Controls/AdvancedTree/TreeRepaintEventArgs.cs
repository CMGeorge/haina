namespace Resco.Controls.AdvancedTree
{
    using System;

    internal class TreeRepaintEventArgs : TreeChangedEventArgs
    {
        public static readonly TreeRepaintEventArgs Empty = new TreeRepaintEventArgs();

        public TreeRepaintEventArgs() : base(TreeEventArgsType.Repaint, null)
        {
        }
    }
}

