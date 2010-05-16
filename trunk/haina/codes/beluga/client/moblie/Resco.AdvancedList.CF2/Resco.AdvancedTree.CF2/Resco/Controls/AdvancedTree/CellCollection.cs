namespace Resco.Controls.AdvancedTree
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class CellCollection : CollectionBase
    {
        private NodeTemplate m_Owner;

        public CellCollection(NodeTemplate rt)
        {
            this.m_Owner = rt;
        }

        public int Add(Cell value)
        {
            return base.List.Add(value);
        }

        public bool Contains(Cell value)
        {
            return base.List.Contains(value);
        }

        public int IndexOf(Cell value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, Cell value)
        {
            base.List.Insert(index, value);
        }

        protected override void OnClear()
        {
            if (this.m_Owner.Parent != null)
            {
                this.m_Owner.Parent.SuspendRedraw();
            }
            foreach (Cell cell in base.List)
            {
                cell.Owner = null;
            }
            base.OnClear();
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            this.m_Owner.AutoHeight = false;
            if (this.m_Owner.Parent != null)
            {
                this.m_Owner.Parent.ResumeRedraw();
            }
        }

        protected override void OnInsert(int index, object value)
        {
            NodeTemplate owner = ((Cell) value).Owner;
            if ((owner != null) && (owner != this.m_Owner))
            {
                throw new ArgumentException("Cell is already part of other NodeTemplate");
            }
            if (this.m_Owner.Parent != null)
            {
                this.m_Owner.Parent.SuspendRedraw();
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            Cell cell = (Cell) value;
            cell.Owner = this.m_Owner;
            cell.Bounds = cell.Bounds;
            if (cell.AutoHeight || cell.CustomizeCell)
            {
                this.m_Owner.RefreshProperties();
            }
            if (this.m_Owner.Parent != null)
            {
                this.m_Owner.Parent.ResumeRedraw();
            }
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            Cell cell = (Cell) value;
            cell.Owner = null;
            if ((this.m_Owner.AutoHeight && cell.AutoHeight) || (this.m_Owner.HasCustomizedCells && cell.CustomizeCell))
            {
                this.m_Owner.RefreshProperties();
            }
            if (this.m_Owner.Parent != null)
            {
                this.m_Owner.Parent.ResumeRedraw();
            }
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            NodeTemplate owner = ((Cell) newValue).Owner;
            if ((owner != null) && (owner != this.m_Owner))
            {
                throw new ArgumentException("Cell is already part of other NodeTemplate");
            }
            if (this.m_Owner.Parent != null)
            {
                this.m_Owner.Parent.SuspendRedraw();
            }
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            Cell cell = (Cell) newValue;
            Cell cell2 = (Cell) oldValue;
            cell.Owner = this.m_Owner;
            cell.Bounds = cell.Bounds;
            ((Cell) oldValue).Owner = null;
            if ((cell.AutoHeight != cell2.AutoHeight) || (cell.CustomizeCell != cell2.CustomizeCell))
            {
                this.m_Owner.RefreshProperties();
            }
            if (this.m_Owner.Parent != null)
            {
                this.m_Owner.Parent.ResumeRedraw();
            }
        }

        public void Remove(Cell value)
        {
            base.List.Remove(value);
        }

        public Cell this[string name]
        {
            get
            {
                for (int i = 0; i < base.List.Count; i++)
                {
                    Cell cell = (Cell) base.InnerList[i];
                    if (cell.Name == name)
                    {
                        return cell;
                    }
                }
                return null;
            }
            set
            {
                for (int i = 0; i < base.List.Count; i++)
                {
                    Cell cell = (Cell) base.InnerList[i];
                    if (cell.Name == name)
                    {
                        base.List[i] = value;
                        return;
                    }
                }
            }
        }

        public Cell this[int i]
        {
            get
            {
                return (Cell) base.InnerList[i];
            }
            set
            {
                base.List[i] = value;
            }
        }
    }
}

