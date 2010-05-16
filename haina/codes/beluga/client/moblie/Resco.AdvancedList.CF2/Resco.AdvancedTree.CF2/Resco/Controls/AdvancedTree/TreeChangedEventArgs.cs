namespace Resco.Controls.AdvancedTree
{
    using System;

    internal class TreeChangedEventArgs : EventArgs
    {
        private object m_oParam;
        private TreeEventArgsType m_Type;

        public TreeChangedEventArgs(TreeEventArgsType type, object oParam)
        {
            this.m_Type = type;
            this.m_oParam = oParam;
        }

        public object Param
        {
            get
            {
                return this.m_oParam;
            }
            set
            {
                this.m_oParam = value;
            }
        }

        public virtual TreeEventArgsType Type
        {
            get
            {
                return this.m_Type;
            }
        }
    }
}

