namespace Resco.Controls.DetailView
{
    using System;
    using System.Collections;

    internal class PageEnumerator : IEnumerator
    {
        private Item m_current;
        private int m_iCurrent;
        private Page m_page;

        public PageEnumerator(Page p)
        {
            this.m_page = p;
            this.m_iCurrent = -1;
            this.m_current = null;
        }

        public bool MoveNext()
        {
            this.m_iCurrent++;
            this.m_current = this.m_page[this.m_iCurrent];
            return (this.m_current != null);
        }

        public void Reset()
        {
            this.m_iCurrent = -1;
            this.m_current = null;
        }

        public object Current
        {
            get
            {
                if (this.m_current == null)
                {
                    throw new InvalidOperationException();
                }
                return this.m_current;
            }
        }
    }
}

