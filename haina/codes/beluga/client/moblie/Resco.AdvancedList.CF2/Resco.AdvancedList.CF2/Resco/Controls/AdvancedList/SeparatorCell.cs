namespace Resco.Controls.AdvancedList
{
    using Resco.Controls.AdvancedList.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class SeparatorCell : Cell
    {
        private Resco.Controls.AdvancedList.SeparatorType m_separatorType;

        public SeparatorCell()
        {
            this.m_separatorType = Resco.Controls.AdvancedList.SeparatorType.Horizontal;
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
            if (this.m_separatorType != Resco.Controls.AdvancedList.SeparatorType.Empty)
            {
                int left = 0;
                int top = 0;
                int right = 0;
                int bottom = 0;
                Pen pen = Resco.Controls.AdvancedList.AdvancedList.GetPen(this.GetColor(ColorCategory.Foreground));
                switch (this.m_separatorType)
                {
                    case Resco.Controls.AdvancedList.SeparatorType.Horizontal:
                        left = drawbounds.Left + (drawbounds.Width / 2);
                        right = left;
                        top = drawbounds.Top;
                        bottom = drawbounds.Bottom;
                        break;

                    case Resco.Controls.AdvancedList.SeparatorType.Vertical:
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
            return (this.m_separatorType != Resco.Controls.AdvancedList.SeparatorType.Horizontal);
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedList.Design.Browsable(false)]
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

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedList.Design.Browsable(false)]
        public Resco.Controls.AdvancedList.CellSource CellSource
        {
            get
            {
                return base.CellSource;
            }
            set
            {
            }
        }

        [DefaultValue(1)]
        public Resco.Controls.AdvancedList.SeparatorType SeparatorType
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
                    base.OnChanged(this, GridEventArgsType.Repaint, null);
                }
            }
        }
    }
}

