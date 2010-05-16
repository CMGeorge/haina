namespace Resco.Controls.AdvancedTree
{
    using System;

    public class CustomizeCellEventArgs : EventArgs
    {
        private Resco.Controls.AdvancedTree.Cell m_cell;
        private object m_data;
        private Resco.Controls.AdvancedTree.Node m_node;

        public CustomizeCellEventArgs(Resco.Controls.AdvancedTree.Cell cell, object data, Resco.Controls.AdvancedTree.Node node)
        {
            this.m_cell = cell.Clone();
            this.m_cell.Owner = cell.Owner;
            this.m_data = data;
            this.m_node = node;
        }

        public Resco.Controls.AdvancedTree.Cell Cell
        {
            get
            {
                return this.m_cell;
            }
        }

        public object Data
        {
            get
            {
                return this.m_data;
            }
            set
            {
                this.m_data = value;
            }
        }

        public Resco.Controls.AdvancedTree.Node Node
        {
            get
            {
                return this.m_node;
            }
        }
    }
}

