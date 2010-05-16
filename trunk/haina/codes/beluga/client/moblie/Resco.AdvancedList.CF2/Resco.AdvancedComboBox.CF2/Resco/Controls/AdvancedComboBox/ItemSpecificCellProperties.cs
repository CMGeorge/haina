namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Drawing;

    public class ItemSpecificCellProperties
    {
        private int m_autoHeight;
        private Rectangle? m_bounds;
        private bool? m_bVisible;
        private CustomizeCellEventArgs m_cea;
        private Cell m_originalCell;

        public ItemSpecificCellProperties(Cell c)
        {
            this.OriginalCell = c;
            this.Visible = null;
            this.CachedAutoHeight = -1;
            this.CachedBounds = null;
            this.Cea = null;
        }

        public ItemSpecificCellProperties(Cell c, bool v, int ah, Rectangle? b)
        {
            this.OriginalCell = c;
            this.Visible = new bool?(v);
            this.CachedAutoHeight = ah;
            this.CachedBounds = b;
        }

        public void ResetCachedBounds()
        {
            this.CachedAutoHeight = -1;
            this.CachedBounds = null;
        }

        public int CachedAutoHeight
        {
            get
            {
                if (!this.Visible.Value)
                {
                    return 0;
                }
                return this.m_autoHeight;
            }
            set
            {
                this.m_autoHeight = value;
            }
        }

        public Rectangle? CachedBounds
        {
            get
            {
                if (this.m_bounds.HasValue)
                {
                    return new Rectangle?(this.Visible.Value ? this.m_bounds.Value : new Rectangle(0, 0, 0, 0));
                }
                return null;
            }
            set
            {
                this.m_bounds = value;
            }
        }

        public CustomizeCellEventArgs Cea
        {
            get
            {
                return this.m_cea;
            }
            set
            {
                this.m_cea = value;
            }
        }

        public Cell OriginalCell
        {
            get
            {
                return this.m_originalCell;
            }
            set
            {
                this.m_originalCell = value;
            }
        }

        public bool? Visible
        {
            get
            {
                if (this.m_bVisible.HasValue)
                {
                    return new bool?(this.m_bVisible.Value);
                }
                return new bool?(this.OriginalCell.Visible);
            }
            set
            {
                this.m_bVisible = value;
            }
        }
    }
}

