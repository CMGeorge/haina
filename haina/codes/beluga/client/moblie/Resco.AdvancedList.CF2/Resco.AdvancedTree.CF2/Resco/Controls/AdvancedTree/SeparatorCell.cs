namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Drawing;

    public class SeparatorCell : Cell
    {
        private Resco.Controls.AdvancedTree.SeparatorType m_separatorType;

        public SeparatorCell()
        {
            this.m_separatorType = Resco.Controls.AdvancedTree.SeparatorType.Horizontal;
            this.Border = BorderType.None;
        }

        public SeparatorCell(SeparatorCell cell) : base(cell)
        {
            this.m_separatorType = cell.m_separatorType;
            this.Border = BorderType.None;
        }

        public override Cell Clone()
        {
            return new SeparatorCell(this);
        }

        protected override void DrawContent(Graphics gr, Rectangle drawbounds, object data)
        {
            if (this.m_separatorType != Resco.Controls.AdvancedTree.SeparatorType.Empty)
            {
                int left = 0;
                int top = 0;
                int right = 0;
                int bottom = 0;
                Pen pen = Resco.Controls.AdvancedTree.AdvancedTree.GetPen(this.GetColor(ColorCategory.Foreground));
                switch (this.m_separatorType)
                {
                    case Resco.Controls.AdvancedTree.SeparatorType.Horizontal:
                        left = drawbounds.Left + (drawbounds.Width / 2);
                        right = left;
                        top = drawbounds.Top;
                        bottom = drawbounds.Bottom;
                        break;

                    case Resco.Controls.AdvancedTree.SeparatorType.Vertical:
                        left = drawbounds.Left;
                        right = drawbounds.Right;
                        top = drawbounds.Top + (drawbounds.Height / 2);
                        bottom = top;
                        break;
                }
                gr.DrawLine(pen, left, top, right, bottom);
            }
        }

        protected virtual bool ShouldSerializeSeparatorType()
        {
            return (this.m_separatorType != Resco.Controls.AdvancedTree.SeparatorType.Horizontal);
        }

        public override BorderType Border
        {
            get
            {
                return BorderType.None;
            }
            set
            {
            }
        }

        public Resco.Controls.AdvancedTree.CellSource CellSource
        {
            get
            {
                return base.CellSource;
            }
            set
            {
            }
        }

        public Resco.Controls.AdvancedTree.SeparatorType SeparatorType
        {
            get
            {
                return this.m_separatorType;
            }
            set
            {
                if (this.m_separatorType != value)
                {
                    this.m_separatorType = value;
                    this.OnChanged(this, TreeRepaintEventArgs.Empty);
                }
            }
        }
    }
}

