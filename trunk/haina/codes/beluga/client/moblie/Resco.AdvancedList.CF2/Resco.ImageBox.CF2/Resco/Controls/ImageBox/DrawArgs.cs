namespace Resco.Controls.ImageBox
{
    using System;
    using System.Drawing;

    internal class DrawArgs
    {
        internal Native.DRAWARGS _DrawArgs;

        public DrawArgs()
        {
            this._DrawArgs.Margins = Rectangle.Empty;
            this._DrawArgs.Zoom = 0f;
            this._DrawArgs.Rotation = 0;
            this._DrawArgs.Origin = Point.Empty;
            this._DrawArgs.BackgroundColor = 0;
            this._DrawArgs.DrawingMode = 1;
            this._DrawArgs.GammaArgs.Brightness = 0;
            this._DrawArgs.GammaArgs.Contrast = 0;
            this._DrawArgs.GammaArgs.Red = this._DrawArgs.GammaArgs.Green = (sbyte) (this._DrawArgs.GammaArgs.Blue = 0);
            this._DrawArgs.GammaArgs.Invert = 0;
            this._DrawArgs.CropBounds = Rectangle.Empty;
        }

        public void OffsetCropBounds(int x, int y)
        {
            this._DrawArgs.CropBounds.left += x;
            this._DrawArgs.CropBounds.right += x;
            this._DrawArgs.CropBounds.top += y;
            this._DrawArgs.CropBounds.bottom += y;
        }

        public Color BackgroundColor
        {
            get
            {
                return Color.FromArgb(this._DrawArgs.BackgroundColor);
            }
            set
            {
                this._DrawArgs.BackgroundColor = Native.RIL_ColorToCOLORREF(value);
            }
        }

        public sbyte Brightness
        {
            get
            {
                return this._DrawArgs.GammaArgs.Brightness;
            }
            set
            {
                this._DrawArgs.GammaArgs.Brightness = value;
            }
        }

        public sbyte Contrast
        {
            get
            {
                return this._DrawArgs.GammaArgs.Contrast;
            }
            set
            {
                this._DrawArgs.GammaArgs.Contrast = value;
            }
        }

        public Rectangle CropBounds
        {
            get
            {
                return (Rectangle) this._DrawArgs.CropBounds;
            }
            set
            {
                this._DrawArgs.CropBounds = value;
            }
        }

        public Resco.Controls.ImageBox.DrawingMode DrawingMode
        {
            get
            {
                return (Resco.Controls.ImageBox.DrawingMode) this._DrawArgs.DrawingMode;
            }
            set
            {
                this._DrawArgs.DrawingMode = (int) value;
            }
        }

        public sbyte Gamma
        {
            set
            {
                this._DrawArgs.GammaArgs.Red = this._DrawArgs.GammaArgs.Green = this._DrawArgs.GammaArgs.Blue = value;
            }
        }

        public sbyte GammaBlue
        {
            get
            {
                return this._DrawArgs.GammaArgs.Blue;
            }
            set
            {
                this._DrawArgs.GammaArgs.Blue = value;
            }
        }

        public sbyte GammaGreen
        {
            get
            {
                return this._DrawArgs.GammaArgs.Green;
            }
            set
            {
                this._DrawArgs.GammaArgs.Green = value;
            }
        }

        public sbyte GammaRed
        {
            get
            {
                return this._DrawArgs.GammaArgs.Red;
            }
            set
            {
                this._DrawArgs.GammaArgs.Red = value;
            }
        }

        public bool Invert
        {
            get
            {
                return (this._DrawArgs.GammaArgs.Invert != 0);
            }
            set
            {
                this._DrawArgs.GammaArgs.Invert = value ? ((byte) 1) : ((byte) 0);
            }
        }

        public Rectangle Margins
        {
            get
            {
                return (Rectangle) this._DrawArgs.Margins;
            }
            set
            {
                this._DrawArgs.Margins = value;
            }
        }

        public Point Origin
        {
            get
            {
                return (Point) this._DrawArgs.Origin;
            }
            set
            {
                this._DrawArgs.Origin = value;
            }
        }

        public Resco.Controls.ImageBox.Rotation Rotation
        {
            get
            {
                return (Resco.Controls.ImageBox.Rotation) this._DrawArgs.Rotation;
            }
            set
            {
                this._DrawArgs.Rotation = (int) value;
            }
        }

        public float Zoom
        {
            get
            {
                return this._DrawArgs.Zoom;
            }
            set
            {
                this._DrawArgs.Zoom = value;
            }
        }
    }
}

