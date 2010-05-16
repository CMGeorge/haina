namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Collections;

    public sealed class Header : Node
    {
        public Header()
        {
        }

        public Header(int templateIndex, IList dataList) : base(templateIndex, templateIndex, dataList)
        {
        }

        public override void Collapse()
        {
        }

        public override void CollapseAll()
        {
        }

        public override void Expand()
        {
        }

        public override void ExpandAll()
        {
        }

        public override int GetNodeCount(bool includeSubTrees)
        {
            return 0;
        }

        public override void Remove()
        {
        }

        public override void Toggle()
        {
        }

        public override int CurrentTemplateIndex
        {
            get
            {
                return base.m_iTemplate;
            }
        }

        public override bool HidePlusMinus
        {
            get
            {
                return true;
            }
        }

        public override bool IsExpanded
        {
            get
            {
                return false;
            }
        }

        public override int Level
        {
            get
            {
                return 0;
            }
        }

        public override NodeCollection Nodes
        {
            get
            {
                return null;
            }
        }

        public override bool Selected
        {
            get
            {
                return false;
            }
        }

        public override int SelectedTemplateIndex
        {
            get
            {
                return base.m_iTemplate;
            }
            set
            {
            }
        }

        public override int TemplateIndex
        {
            get
            {
                return base.m_iTemplate;
            }
            set
            {
                if (base.m_iTemplate != value)
                {
                    base.m_iTemplate = value;
                    base.OnNodeChanged(NodeEventArgsType.Empty, null);
                }
            }
        }
    }
}

