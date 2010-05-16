namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.ComponentModel;

    public class NodeAddingEventArgs : CancelEventArgs
    {
        private int m_level;
        private int m_nNodeIndex;
        private Resco.Controls.AdvancedTree.Node m_node;
        private Resco.Controls.AdvancedTree.Node m_parentNode;

        public NodeAddingEventArgs(Resco.Controls.AdvancedTree.Node parentNode, int nodeIndex)
        {
            this.m_nNodeIndex = nodeIndex;
            this.m_parentNode = parentNode;
            this.m_node = null;
            if (this.m_parentNode != null)
            {
                this.m_level = this.m_parentNode.Level + 1;
            }
            else
            {
                this.m_level = 0;
            }
        }

        public int Level
        {
            get
            {
                return this.m_level;
            }
        }

        public Resco.Controls.AdvancedTree.Node Node
        {
            get
            {
                return this.m_node;
            }
            set
            {
                this.m_node = value;
            }
        }

        public int NodeIndex
        {
            get
            {
                return this.m_nNodeIndex;
            }
        }

        public Resco.Controls.AdvancedTree.Node ParentNode
        {
            get
            {
                return this.m_parentNode;
            }
        }
    }
}

