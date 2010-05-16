namespace Resco.Controls.ScrollBar
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ScrollBarExtensionBase : Component
    {
        private Bitmap _backgroundBufferBitmap = null;
        private Graphics _backgroundBufferGraphics = null;
        private bool _backgroundValid = false;
        private Rectangle _bounds;
        private int _index = 0;
        private bool _inOnIndexToValue = false;
        private ComponentLocation _location;
        private Resco.Controls.ScrollBar.ScrollBar _parent = null;

        public event ValueIndexConversionHandler IndexToValue;

        public event MouseEventHandler MouseDown;

        public event MouseEventHandler MouseMove;

        public event MouseEventHandler MouseUp;

        internal event NeedChangeValueHandler NeedChangeValue;

        public event DrawExtensionHandler Paint;

        internal event PropertyChangedHandler PropertyChanged;

        public event EventHandler Resize;

        public event ValueIndexConversionHandler ValueToIndex;

        protected ScrollBarExtensionBase()
        {
        }

        protected virtual bool DeselectAllIndex(int index)
        {
            return (index < 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._backgroundBufferGraphics != null)
                {
                    this._backgroundBufferGraphics.Dispose();
                }
                if (this._backgroundBufferBitmap != null)
                {
                    this._backgroundBufferBitmap.Dispose();
                }
            }
            this._backgroundBufferGraphics = null;
            this._backgroundBufferBitmap = null;
            base.Dispose(disposing);
        }

        internal void Draw(Graphics gr, Color parentColor)
        {
            bool flag = true;
            Region clip = null;
            DrawExtensionEventArgs e = null;
            Graphics backgroundBuffer = this.GetBackgroundBuffer();
            if (backgroundBuffer != null)
            {
                if (this._backgroundValid)
                {
                    flag = false;
                }
                else
                {
                    e = new DrawExtensionEventArgs(backgroundBuffer, new Rectangle(0, 0, this._bounds.Width, this._bounds.Height), parentColor);
                }
            }
            else
            {
                clip = gr.Clip;
                Region helperRegion = Resco.Controls.ScrollBar.ScrollBar.GetHelperRegion();
                helperRegion.MakeEmpty();
                helperRegion.Union(this._bounds);
                gr.Clip = helperRegion;
                e = new DrawExtensionEventArgs(gr, this._bounds, parentColor);
            }
            if (flag)
            {
                this.OnPaint(e);
                this._backgroundValid = e.Graphics == backgroundBuffer;
            }
            if ((e == null) || (e.Graphics == backgroundBuffer))
            {
                gr.DrawImage(this._backgroundBufferBitmap, this._bounds.Left, this._bounds.Top);
            }
            if (clip != null)
            {
                gr.Clip = clip;
            }
        }

        private Graphics GetBackgroundBuffer()
        {
            if (!Resco.Controls.ScrollBar.ScrollBar.CreateBackBuffer(ref this._backgroundBufferGraphics, ref this._backgroundBufferBitmap, this._bounds.Size))
            {
                this._backgroundValid = false;
            }
            return this._backgroundBufferGraphics;
        }

        protected void Invalidate()
        {
            this._backgroundValid = false;
        }

        internal void Mouse(MouseAction a, MouseEventArgs e)
        {
            switch (a)
            {
                case MouseAction.Down:
                    this.OnMouseDown(e);
                    return;

                case MouseAction.Move:
                    this.OnMouseMove(e);
                    return;

                case MouseAction.Up:
                    this.OnMouseUp(e);
                    return;
            }
        }

        protected virtual void OnIndexToValue(ValueIndexConversionEventArgs e)
        {
            this._inOnIndexToValue = true;
            if (this.IndexToValue != null)
            {
                int num = this.IndexToValue(this, e);
                if ((!e.Cancel && (this.NeedChangeValue != null)) && this.ValidateValue(num))
                {
                    this.NeedChangeValue(this, new ValueIndexConversionEventArgs(num));
                }
            }
            this._inOnIndexToValue = false;
        }

        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            if (this.MouseDown != null)
            {
                this.MouseDown(this, e);
            }
        }

        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            if (this.MouseMove != null)
            {
                this.MouseMove(this, e);
            }
        }

        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            if (this.MouseUp != null)
            {
                this.MouseUp(this, e);
            }
        }

        protected virtual void OnPaint(DrawExtensionEventArgs e)
        {
            if (this.Paint != null)
            {
                this.Paint(this, e);
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.Redraw)
            {
                this._backgroundValid = false;
            }
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        protected virtual void OnResize(EventArgs e)
        {
            if (this.Resize != null)
            {
                this.Resize(this, e);
            }
        }

        protected virtual void OnValueToIndex(ValueIndexConversionEventArgs e)
        {
            if (!this._inOnIndexToValue && (this.ValueToIndex != null))
            {
                int index = this.ValueToIndex(this, e);
                if ((!e.Cancel && !this.DeselectAllIndex(index)) && this.ValidateIndex(index))
                {
                    this._index = index;
                    this._backgroundValid = false;
                }
            }
        }

        internal void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            this.OnValueToIndex(new ValueIndexConversionEventArgs(((Resco.Controls.ScrollBar.ScrollBar) sender).Value));
        }

        protected virtual bool ValidateIndex(int index)
        {
            return true;
        }

        protected virtual bool ValidateValue(int value)
        {
            return true;
        }

        protected internal Rectangle Bounds
        {
            get
            {
                return this._bounds;
            }
            internal set
            {
                if (this._bounds != value)
                {
                    this._bounds = value;
                    this.OnResize(EventArgs.Empty);
                }
            }
        }

        public int Index
        {
            get
            {
                return this._index;
            }
            set
            {
                if (this._index != value)
                {
                    this._index = value;
                    this.OnIndexToValue(new ValueIndexConversionEventArgs(this._index));
                }
            }
        }

        protected internal ComponentLocation Location
        {
            get
            {
                return this._location;
            }
            internal set
            {
                if (this._location != value)
                {
                    this._location = value;
                    this._backgroundValid = false;
                }
            }
        }

        internal Resco.Controls.ScrollBar.ScrollBar Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                if (this._parent != value)
                {
                    if ((this._parent != null) && (value != null))
                    {
                        this._parent.Extension = null;
                    }
                    this._parent = value;
                }
            }
        }

        internal protected enum ComponentLocation
        {
            Left,
            Right,
            Top,
            Bottom
        }

        public class DrawExtensionEventArgs : EventArgs
        {
            private Rectangle _bounds;
            private System.Drawing.Graphics _graphics;
            private Color _parentColor;

            public DrawExtensionEventArgs(System.Drawing.Graphics graphics, Rectangle bounds, Color parentColor)
            {
                this._graphics = graphics;
                this._bounds = bounds;
                this._parentColor = parentColor;
            }

            public Rectangle Bounds
            {
                get
                {
                    return this._bounds;
                }
            }

            public System.Drawing.Graphics Graphics
            {
                get
                {
                    return this._graphics;
                }
            }

            public Color ParentColor
            {
                get
                {
                    return this._parentColor;
                }
            }
        }

        public delegate void DrawExtensionHandler(object sender, ScrollBarExtensionBase.DrawExtensionEventArgs e);

        internal enum MouseAction
        {
            Down,
            Move,
            Up
        }

        internal delegate void NeedChangeValueHandler(object sender, ScrollBarExtensionBase.ValueIndexConversionEventArgs e);

        internal protected class PropertyChangedEventArgs : EventArgs
        {
            private bool _redraw;

            public PropertyChangedEventArgs(bool redraw)
            {
                this._redraw = redraw;
            }

            public bool Redraw
            {
                get
                {
                    return this._redraw;
                }
            }
        }

        internal delegate void PropertyChangedHandler(object sender, ScrollBarExtensionBase.PropertyChangedEventArgs e);

        public class ValueIndexConversionEventArgs : CancelEventArgs
        {
            private int _param;

            public ValueIndexConversionEventArgs(int parameter) : base(false)
            {
                this._param = parameter;
            }

            public int Parameter
            {
                get
                {
                    return this._param;
                }
            }
        }

        public delegate int ValueIndexConversionHandler(object sender, ScrollBarExtensionBase.ValueIndexConversionEventArgs e);
    }
}

