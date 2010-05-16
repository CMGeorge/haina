namespace Resco.Controls.ScrollBar
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class LetterBar : ScrollBarExtensionBase
    {
        private bool _borderClosed;
        private System.Drawing.Color _borderColor;
        private ScrollBarBorderStyle _borderStyle;
        private System.Drawing.Color _color;
        private System.Drawing.Color _disabledColor;
        private uint _disabledLetters;
        private System.Drawing.Font _font;
        private float _fontSize = 0f;
        private System.Drawing.Color _foreColor;
        private Resco.Controls.ScrollBar.GradientColor _gradientColor;
        private System.Drawing.Color _highlightColor;
        private System.Drawing.Image _image;
        private List<char> _letters;
        private StringFormat _stringCenter = new StringFormat(StringFormatFlags.NoWrap);

        public LetterBar()
        {
            this._stringCenter.Alignment = StringAlignment.Center;
            this._stringCenter.LineAlignment = StringAlignment.Center;
            this._letters = new List<char>(0x1a);
            this._disabledLetters = 0;
            this._borderStyle = ScrollBarBorderStyle.Solid;
            this._borderColor = SystemColors.ControlText;
            this._borderClosed = false;
            this._color = SystemColors.Control;
            this._gradientColor = new Resco.Controls.ScrollBar.GradientColor(FillDirection.Horizontal);
            this._gradientColor.PropertyChanged += new EventHandler(this.GradientColor_PropertyChanged);
            this._image = null;
            this._font = new System.Drawing.Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);
            this._foreColor = SystemColors.ControlText;
            this._highlightColor = SystemColors.HighlightText;
            this._disabledColor = SystemColors.GrayText;
        }

        private void CreateLetterList(bool horizontal, List<char> list, float fontSize, Rectangle drawRect)
        {
            list.Clear();
            int num = 0;
            int num2 = 0x1a;
            if (horizontal)
            {
                num = drawRect.Width / ((int) this._fontSize);
            }
            else
            {
                num = drawRect.Height / ((int) this._fontSize);
            }
            if (num > num2)
            {
                num = num2;
            }
            if (num > 0)
            {
                char ch;
                for (ch = 'A'; ch <= 'Z'; ch = (char) (ch + '\x0001'))
                {
                    list.Add(ch);
                }
                if (num < num2)
                {
                    ch = 'A';
                    List<char> list2 = new List<char>(num2);
                    for (uint i = this._disabledLetters; i != 0; i = i >> 1)
                    {
                        if ((i & 1) == 1)
                        {
                            list2.Add(ch);
                            list.Remove(ch);
                        }
                        ch = (char) (ch + '\x0001');
                    }
                    int num4 = list.Count;
                    if (num4 > num)
                    {
                        if (num == 1)
                        {
                            list.RemoveRange(1, num4 - 1);
                        }
                        else
                        {
                            int num5 = num4 - num;
                            double num6 = ((double) num4) / ((double) (num5 + 1));
                            double a = num6;
                            num6--;
                            for (int j = 0; j < num5; j++)
                            {
                                list.RemoveAt((int) Math.Round(a));
                                a += num6;
                            }
                        }
                    }
                    else if (num4 < num)
                    {
                        int num9 = num - num4;
                        double num10 = ((double) list2.Count) / ((double) (num9 + 1));
                        double num11 = num10;
                        int num12 = 0;
                        for (int k = 0; k < num9; k++)
                        {
                            char ch2 = list2[(int) Math.Round(num11)];
                            while (list[num12] < ch2)
                            {
                                num12++;
                            }
                            list.Insert(num12, ch2);
                            num11 += num10;
                        }
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._gradientColor != null)
                {
                    this._gradientColor.PropertyChanged -= new EventHandler(this.GradientColor_PropertyChanged);
                }
                if (this._image != null)
                {
                    this._image.Dispose();
                }
            }
            this._gradientColor = null;
            this._image = null;
            this._font = null;
            base.Dispose(disposing);
        }

        private void DrawLetters(Graphics gr, Rectangle drawRect)
        {
            int num;
            ScrollBarExtensionBase.ComponentLocation location = base.Location;
            bool horizontal = (location == ScrollBarExtensionBase.ComponentLocation.Top) || (location == ScrollBarExtensionBase.ComponentLocation.Bottom);
            if (this._fontSize == 0f)
            {
                this._fontSize = this.GetFontSize(gr, horizontal);
                this._letters.Clear();
            }
            if (this._letters.Count == 0)
            {
                this.CreateLetterList(horizontal, this._letters, this._fontSize, drawRect);
            }
            SolidBrush brush = Resco.Controls.ScrollBar.ScrollBar.GetBrush(this._foreColor);
            int index = base.Index;
            if (horizontal)
            {
                float width = ((float) drawRect.Width) / ((float) this._letters.Count);
                RectangleF layoutRectangle = new RectangleF((float) drawRect.Left, (float) drawRect.Top, width, (float) drawRect.Height);
                foreach (char ch in this._letters)
                {
                    num = ch - 'A';
                    brush.Color = (num == index) ? this._highlightColor : (((this._disabledLetters & (((int) 1) << num)) == 0L) ? this._foreColor : this._disabledColor);
                    gr.DrawString(ch.ToString(), this._font, brush, layoutRectangle, this._stringCenter);
                    layoutRectangle.X += width;
                }
            }
            else
            {
                float height = ((float) drawRect.Height) / ((float) this._letters.Count);
                RectangleF ef2 = new RectangleF((float) drawRect.Left, (float) drawRect.Top, (float) drawRect.Width, height);
                foreach (char ch2 in this._letters)
                {
                    num = ch2 - 'A';
                    brush.Color = (num == index) ? this._highlightColor : (((this._disabledLetters & (((int) 1) << num)) == 0L) ? this._foreColor : this._disabledColor);
                    gr.DrawString(ch2.ToString(), this._font, brush, ef2, this._stringCenter);
                    ef2.Y += height;
                }
            }
        }

        int Letters2uint(Letters letter)
        {
            char c = (char)letter;
            return (int)c;

        }

        public void EnableLetter(Letters letter, bool enabled)
        {
            uint num = this._disabledLetters;
            //uint num2 = ((int)1) << Letters2uint(letter);
            uint num2 = (uint)(((int)1) << Letters2uint(letter)); //chage by lws
            if (enabled)
            {
                this._disabledLetters &= ~num2;
            }
            else
            {
                this._disabledLetters |= num2;
            }
            if (this._disabledLetters != num)
            {
                this.HandleVisualChange();
            }
        }

        private float GetFontSize(Graphics gr, bool horizontal)
        {
            float height = 0f;
            System.Drawing.Font font = this.Font;
            if (horizontal)
            {
                for (char ch = 'A'; ch <= 'Z'; ch = (char) (ch + '\x0001'))
                {
                    float width = gr.MeasureString(ch.ToString(), font).Width;
                    if (width > height)
                    {
                        height = width;
                    }
                }
            }
            else
            {
                height = gr.MeasureString("A", font).Height;
            }
            return (float) Math.Round((double) height);
        }

        private void GradientColor_PropertyChanged(object sender, EventArgs e)
        {
            this.HandleVisualChange();
        }

        private void HandleVisualChange()
        {
            this.OnPropertyChanged(new ScrollBarExtensionBase.PropertyChangedEventArgs(true));
        }

        public char IndexToLetter(int index)
        {
            if ((index >= 0) && (index <= 0x19))
            {
                return (char) (index + 0x41);
            }
            return '\0';
        }

        public int LetterToIndex(char letter)
        {
            char ch = char.ToUpper(letter);
            if ((ch >= 'A') && (ch <= 'Z'))
            {
                return (ch - 'A');
            }
            return -1;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int num;
            Rectangle bounds = base.Bounds;
            if (this._borderStyle != ScrollBarBorderStyle.None)
            {
                bounds.Inflate(-1, -1);
            }
            if ((base.Location == ScrollBarExtensionBase.ComponentLocation.Top) || (base.Location == ScrollBarExtensionBase.ComponentLocation.Bottom))
            {
                float num2 = ((float) bounds.Width) / ((float) this._letters.Count);
                num = (int) (((float) (e.X - bounds.Left)) / num2);
            }
            else
            {
                float num3 = ((float) bounds.Height) / ((float) this._letters.Count);
                num = (int) (((float) (e.Y - bounds.Top)) / num3);
            }
            if ((num >= 0) && (num < this._letters.Count))
            {
                num = this._letters[num] - 'A';
                if ((this._disabledLetters & (((int) 1) << num)) == 0L)
                {
                    base.Index = num;
                }
                this.HandleVisualChange();
            }
            base.OnMouseDown(e);
        }

        protected override void OnPaint(ScrollBarExtensionBase.DrawExtensionEventArgs e)
        {
            Rectangle bounds = e.Bounds;
            Graphics gr = e.Graphics;
            if (this._borderStyle != ScrollBarBorderStyle.None)
            {
                ScrollBarExtensionBase.ComponentLocation location = base.Location;
                Resco.Controls.ScrollBar.ScrollBar.BorderSide all = Resco.Controls.ScrollBar.ScrollBar.BorderSide.All;
                if (!this._borderClosed)
                {
                    switch (location)
                    {
                        case ScrollBarExtensionBase.ComponentLocation.Left:
                            all &= ~Resco.Controls.ScrollBar.ScrollBar.BorderSide.Right;
                            break;

                        case ScrollBarExtensionBase.ComponentLocation.Right:
                            all &= ~Resco.Controls.ScrollBar.ScrollBar.BorderSide.Left;
                            break;

                        case ScrollBarExtensionBase.ComponentLocation.Top:
                            all &= ~Resco.Controls.ScrollBar.ScrollBar.BorderSide.Down;
                            break;

                        case ScrollBarExtensionBase.ComponentLocation.Bottom:
                            all &= ~Resco.Controls.ScrollBar.ScrollBar.BorderSide.Up;
                            break;
                    }
                }
                Resco.Controls.ScrollBar.ScrollBar.DoDrawBorder(gr, this._borderStyle, this._borderColor, bounds, all, e.ParentColor);
                bounds.Inflate(-1, -1);
                if (all != Resco.Controls.ScrollBar.ScrollBar.BorderSide.All)
                {
                    switch (location)
                    {
                        case ScrollBarExtensionBase.ComponentLocation.Left:
                        case ScrollBarExtensionBase.ComponentLocation.Right:
                            bounds.Width++;
                            if (location == ScrollBarExtensionBase.ComponentLocation.Right)
                            {
                                bounds.X--;
                            }
                            goto Label_00D6;
                    }
                    bounds.Height++;
                    if (location == ScrollBarExtensionBase.ComponentLocation.Bottom)
                    {
                        bounds.Y--;
                    }
                }
            }
        Label_00D6:
            if ((bounds.Width > 0) && (bounds.Height > 0))
            {
                if (this._image != null)
                {
                    gr.DrawImage(this._image, bounds, new Rectangle(0, 0, this._image.Width, this._image.Height), GraphicsUnit.Pixel);
                }
                else if (this._gradientColor.CanDraw())
                {
                    this._gradientColor.DrawGradient(gr, bounds);
                }
                else
                {
                    gr.FillRectangle(Resco.Controls.ScrollBar.ScrollBar.GetBrush(this._color), bounds);
                }
                this.DrawLetters(gr, bounds);
            }
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            this._letters.Clear();
            base.Invalidate();
            base.OnResize(e);
        }

        protected virtual bool ShouldSerializeBorderStyle()
        {
            return (this._borderStyle != ScrollBarBorderStyle.Solid);
        }

        protected virtual bool ShouldSerializeGradientColor()
        {
            return Resco.Controls.ScrollBar.ScrollBar.ShouldSerializeGradientColor(this._gradientColor);
        }

        protected override bool ValidateIndex(int index)
        {
            return (((index >= 0) && (index <= 0x19)) && ((this._disabledLetters & (((int) 1) << index)) == 0L));
        }

        protected override bool ValidateValue(int value)
        {
            return (value >= 0);
        }

        public bool BorderClosed
        {
            get
            {
                return this._borderClosed;
            }
            set
            {
                if (this._borderClosed != value)
                {
                    this._borderClosed = value;
                    this.HandleVisualChange();
                }
            }
        }

        public System.Drawing.Color BorderColor
        {
            get
            {
                return this._borderColor;
            }
            set
            {
                if (this._borderColor != value)
                {
                    this._borderColor = value;
                    this.HandleVisualChange();
                }
            }
        }

        public ScrollBarBorderStyle BorderStyle
        {
            get
            {
                return this._borderStyle;
            }
            set
            {
                if (this._borderStyle != value)
                {
                    this._borderStyle = value;
                    this.HandleVisualChange();
                }
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this._color;
            }
            set
            {
                if (this._color != value)
                {
                    this._color = value;
                    this.HandleVisualChange();
                }
            }
        }

        public System.Drawing.Color DisabledColor
        {
            get
            {
                return this._disabledColor;
            }
            set
            {
                if (this._disabledColor != value)
                {
                    this._disabledColor = value;
                    this.HandleVisualChange();
                }
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this._font;
            }
            set
            {
                if (this._font != value)
                {
                    this._font = value;
                    this._fontSize = 0f;
                    this.HandleVisualChange();
                }
            }
        }

        public System.Drawing.Color ForeColor
        {
            get
            {
                return this._foreColor;
            }
            set
            {
                if (this._foreColor != value)
                {
                    this._foreColor = value;
                    this.HandleVisualChange();
                }
            }
        }

        public Resco.Controls.ScrollBar.GradientColor GradientColor
        {
            get
            {
                return this._gradientColor;
            }
            set
            {
                if (this._gradientColor != value)
                {
                    this._gradientColor.PropertyChanged -= new EventHandler(this.GradientColor_PropertyChanged);
                    this._gradientColor = value;
                    this._gradientColor.PropertyChanged += new EventHandler(this.GradientColor_PropertyChanged);
                    this.HandleVisualChange();
                }
            }
        }

        public System.Drawing.Color HighlightColor
        {
            get
            {
                return this._highlightColor;
            }
            set
            {
                if (this._highlightColor != value)
                {
                    this._highlightColor = value;
                    this.HandleVisualChange();
                }
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this._image;
            }
            set
            {
                if (this._image != value)
                {
                    this._image = value;
                    this.HandleVisualChange();
                }
            }
        }

        public enum Letters
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
            K,
            L,
            M,
            N,
            O,
            P,
            Q,
            R,
            S,
            T,
            U,
            V,
            W,
            X,
            Y,
            Z
        }
    }
}

