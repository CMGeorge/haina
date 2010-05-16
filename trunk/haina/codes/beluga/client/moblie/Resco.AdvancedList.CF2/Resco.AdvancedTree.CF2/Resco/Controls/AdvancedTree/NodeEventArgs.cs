namespace Resco.Controls.AdvancedTree
{
    using System;

    public class NodeEventArgs : EventArgs
    {
        public Resco.Controls.AdvancedTree.Node Node;
        public int NodeIndex;
        public int Offset;

        public NodeEventArgs(Resco.Controls.AdvancedTree.Node r, int i, int yoff)
        {
            this.Node = r;
            this.NodeIndex = i;
            this.Offset = yoff;
        }
    }
}

