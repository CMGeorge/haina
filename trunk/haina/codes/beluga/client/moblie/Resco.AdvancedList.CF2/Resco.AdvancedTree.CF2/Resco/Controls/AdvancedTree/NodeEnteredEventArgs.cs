namespace Resco.Controls.AdvancedTree
{
    using System;

    public class NodeEnteredEventArgs : EventArgs
    {
        private int m_iNodeIndex;
        private Resco.Controls.AdvancedTree.Node m_node;

        public NodeEnteredEventArgs(Resco.Controls.AdvancedTree.Node n, int iNodeIndex)
        {
            this.m_node = n;
            this.m_iNodeIndex = iNodeIndex;
        }

        public Resco.Controls.AdvancedTree.Node Node
        {
            get
            {
                return this.m_node;
            }
        }

        public int NodeIndex
        {
            get
            {
                return this.m_iNodeIndex;
            }
        }
    }
}

