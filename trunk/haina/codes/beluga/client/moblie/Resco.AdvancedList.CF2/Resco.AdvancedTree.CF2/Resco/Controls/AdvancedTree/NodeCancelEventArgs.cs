namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.ComponentModel;

    public class NodeCancelEventArgs : CancelEventArgs
    {
        public Resco.Controls.AdvancedTree.Node Node;
        public int NodeIndex;
        public int Offset;

        public NodeCancelEventArgs(Resco.Controls.AdvancedTree.Node node, int index, int offset)
        {
            this.Node = node;
            this.NodeIndex = index;
            this.Offset = offset;
            base.Cancel = false;
        }
    }
}

