namespace Resco.Controls.ScrollBar
{
    using System;
    using System.Drawing;

    public class MarginPadding
    {
        private int m_Bottom;
        private static MarginPadding m_Default = new MarginPadding();
        private int m_Left;
        private int m_Right;
        private int m_Top;

        public MarginPadding() : this(0)
        {
        }

        public MarginPadding(int all)
        {
            this.m_Left = all;
            this.m_Right = all;
            this.m_Top = all;
            this.m_Bottom = all;
        }

        public MarginPadding(int left, int top, int right, int bottom)
        {
            this.m_Left = left;
            this.m_Right = right;
            this.m_Top = top;
            this.m_Bottom = bottom;
        }

        public override bool Equals(object o)
        {
            return ((o is MarginPadding) && (((MarginPadding) o) == this));
        }

        public override int GetHashCode()
        {
            return (((this.m_Left ^ (this.m_Top << 8)) ^ (this.m_Right << 0x10)) ^ (this.m_Bottom << 0x18));
        }

        public static bool operator ==(MarginPadding left, MarginPadding right)
        {
            return (((left.Left == right.Left) && (left.Top == right.Top)) && ((left.Right == right.Right) && (left.Bottom == right.Bottom)));
        }

        public static bool operator !=(MarginPadding left, MarginPadding right)
        {
            return !(left == right);
        }

        public static MarginPadding operator *(MarginPadding left, SizeF right)
        {
            return new MarginPadding((int) (left.Left * right.Width), (int) (left.Top * right.Height), (int) (left.Right * right.Width), (int) (left.Bottom * right.Height));
        }

        public override string ToString()
        {
            return (Convert.ToString(this.m_Left) + "; " + Convert.ToString(this.m_Top) + "; " + Convert.ToString(this.m_Right) + "; " + Convert.ToString(this.m_Bottom));
        }

        public int Bottom
        {
            get
            {
                return this.m_Bottom;
            }
        }

        public static MarginPadding Default
        {
            get
            {
                return m_Default;
            }
        }

        public int Left
        {
            get
            {
                return this.m_Left;
            }
        }

        public int Right
        {
            get
            {
                return this.m_Right;
            }
        }

        public int Top
        {
            get
            {
                return this.m_Top;
            }
        }
    }
}

