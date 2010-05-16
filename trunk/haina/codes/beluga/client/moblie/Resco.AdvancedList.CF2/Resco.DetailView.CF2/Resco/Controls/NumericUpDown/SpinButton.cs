namespace Resco.Controls.NumericUpDown
{
    using Resco.Controls.DetailView;
    using Resco.Controls.NumericUpDown.Utility;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class SpinButton : Control
    {
        public Resco.Controls.NumericUpDown.AdornmentType DefaultAdornmentType;
        private int m_adornmentSize = 4;
        private Resco.Controls.NumericUpDown.AdornmentType m_adornmentType = Resco.Controls.NumericUpDown.AdornmentType.RightArrow;
        private RescoBorderStyle m_borderStyle = RescoBorderStyle.All;
        private GradientColor m_gradientColors = new GradientColor(Color.LightGray, Color.Black);
        private GradientColor m_gradientColorsPressed;
        private Bitmap m_imageDefault;
        private Bitmap m_imagePressed;
        private Bitmap m_imageVgaDefault;
        private Bitmap m_imageVgaPressed;
        private int m_increaseRate;
        private Color m_pressedBackColor = Color.Transparent;
        private Color m_pressedForeColor = Color.Transparent;
        private int m_repeatCount;
        private int m_repeatDelay;
        private int m_repeatIncreaseRate;
        private int m_repeatRate;
        private Timer m_timer;
        private Resco.Controls.NumericUpDown.Utility.Utility m_utility = new Resco.Controls.NumericUpDown.Utility.Utility();
        private Resco.Controls.NumericUpDown.VisualStyle m_visualStyle = Resco.Controls.NumericUpDown.VisualStyle.Colors;

        public event SpinChangedEventHandler SpinChanged;

        static SpinButton()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(SpinButton), "");
            //}
        }

        public SpinButton()
        {
            this.m_gradientColors.PropertyChanged += new EventHandler(this.OnGradientColorPropertyChanged);
            this.m_gradientColorsPressed = new GradientColor(Color.Black, Color.LightGray);
            this.m_gradientColorsPressed.PropertyChanged += new EventHandler(this.OnGradientColorPropertyChanged);
            this.m_imageDefault = null;
            this.m_imagePressed = null;
            this.m_imageVgaDefault = null;
            this.m_imageVgaPressed = null;
            this.m_timer = new Timer();
            this.m_timer.Enabled = false;
            this.m_timer.Tick += new EventHandler(this.OnTimerTick);
            this.m_repeatDelay = 500;
            this.m_repeatRate = 100;
            this.m_repeatIncreaseRate = 20;
            this.m_repeatCount = 0;
            this.m_increaseRate = 1;
            base.BackColor = SystemColors.Window;
            base.Size = new Size(12, 0x16);
        }

        private void OnGradientColorPropertyChanged(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.Capture = true;
            this.m_repeatCount = 0;
            this.m_increaseRate = 1;
            this.m_timer.Interval = this.m_repeatDelay;
            this.m_timer.Enabled = true;
            this.OnSpinChanged(new SpinChangedEventArgs(this.m_increaseRate));
            base.OnMouseDown(e);
            base.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.Capture = false;
            this.m_timer.Enabled = false;
            base.OnMouseUp(e);
            base.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Color color = base.Capture ? ((this.PressedForeColor == Color.Transparent) ? this.BackColor : this.PressedForeColor) : this.ForeColor;
            if (this.AdornmentType == Resco.Controls.NumericUpDown.AdornmentType.Text)
            {
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(this.Text, this.Font, this.m_utility.GetBrush(color), new Rectangle(0, 0, base.Width, base.Height), format);
            }
            else
            {
                this.m_utility.DrawAdornment(e.Graphics, base.Width / 2, base.Height / 2, this.AdornmentSize, color, this.AdornmentType, this.BorderStyle);
            }
            this.m_utility.DrawBorder(e.Graphics, SystemColors.WindowFrame, base.Size, this.m_borderStyle);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Color aColor = base.Capture ? ((this.PressedBackColor == Color.Transparent) ? this.ForeColor : this.PressedBackColor) : this.BackColor;
            if (this.m_visualStyle == Resco.Controls.NumericUpDown.VisualStyle.VistaStyle)
            {
                GradientFill.DrawVistaGradient(e.Graphics, aColor, base.ClientRectangle, FillDirection.Vertical);
            }
            else if (this.m_visualStyle == Resco.Controls.NumericUpDown.VisualStyle.Gradients)
            {
                GradientFill.Fill(e.Graphics, base.ClientRectangle, base.ClientRectangle, base.Capture ? this.m_gradientColorsPressed : this.m_gradientColors);
            }
            else if (this.m_visualStyle == Resco.Controls.NumericUpDown.VisualStyle.Images)
            {
                Image image = null;
                if (base.Capture)
                {
                    image = ((e.Graphics.DpiX == 192f) && (this.m_imageVgaPressed != null)) ? this.m_imageVgaPressed : (image = this.m_imagePressed);
                }
                if (image == null)
                {
                    image = ((e.Graphics.DpiX == 192f) && (this.m_imageVgaDefault != null)) ? this.m_imageVgaDefault : this.m_imageDefault;
                }
                if (image != null)
                {
                    e.Graphics.DrawImage(image, base.ClientRectangle, new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                }
                else
                {
                    e.Graphics.Clear(aColor);
                }
            }
            else
            {
                e.Graphics.Clear(aColor);
            }
        }

        protected virtual void OnSpinChanged(SpinChangedEventArgs e)
        {
            if (this.SpinChanged != null)
            {
                this.SpinChanged(this, e);
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            this.m_repeatCount++;
            if (this.m_repeatCount >= this.m_repeatIncreaseRate)
            {
                this.m_increaseRate++;
                this.m_repeatCount = 0;
            }
            this.OnSpinChanged(new SpinChangedEventArgs(this.m_increaseRate));
            this.m_timer.Interval = this.m_repeatRate;
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            if (factor.Width != 1f)
            {
                this.m_adornmentSize = (int) (this.m_adornmentSize * factor.Width);
            }
            SizeF dpi = new SizeF(96f, 96f);
            ContainerControl topLevelControl = base.TopLevelControl as ContainerControl;
            if (topLevelControl != null)
            {
                dpi = topLevelControl.CurrentAutoScaleDimensions;//.get_CurrentAutoScaleDimensions();
            }
            this.m_utility.Scale(dpi);
            base.ScaleControl(factor, specified);
        }

        protected virtual bool ShouldSerializeAdornmentType()
        {
            return (this.m_adornmentType != this.DefaultAdornmentType);
        }

        protected virtual bool ShouldSerializeBorderStyle()
        {
            return (this.m_borderStyle != RescoBorderStyle.All);
        }

        protected virtual bool ShouldSerializeGradientColors()
        {
            return (((this.m_gradientColors.StartColor != Color.LightGray) | (this.m_gradientColors.EndColor != Color.Black)) | (this.m_gradientColors.FillDirection != FillDirection.Vertical));
        }

        protected virtual bool ShouldSerializeGradientColorsPressed()
        {
            return (((this.m_gradientColorsPressed.StartColor != Color.Black) | (this.m_gradientColorsPressed.EndColor != Color.LightGray)) | (this.m_gradientColorsPressed.FillDirection != FillDirection.Vertical));
        }

        protected virtual bool ShouldSerializeVisualStyle()
        {
            return (this.m_visualStyle != Resco.Controls.NumericUpDown.VisualStyle.Colors);
        }

        public int AdornmentSize
        {
            get
            {
                return this.m_adornmentSize;
            }
            set
            {
                if (this.m_adornmentSize != value)
                {
                    this.m_adornmentSize = value;
                    base.Invalidate();
                }
            }
        }

        public Resco.Controls.NumericUpDown.AdornmentType AdornmentType
        {
            get
            {
                return this.m_adornmentType;
            }
            set
            {
                if (this.m_adornmentType != value)
                {
                    this.m_adornmentType = value;
                    base.Invalidate();
                }
            }
        }

        public RescoBorderStyle BorderStyle
        {
            get
            {
                return this.m_borderStyle;
            }
            set
            {
                if (this.m_borderStyle != value)
                {
                    this.m_borderStyle = value;
                    base.Invalidate();
                }
            }
        }

        public GradientColor GradientColors
        {
            get
            {
                return this.m_gradientColors;
            }
            set
            {
                if (this.m_gradientColors != value)
                {
                    this.m_gradientColors.PropertyChanged -= new EventHandler(this.OnGradientColorPropertyChanged);
                    this.m_gradientColors = null;
                    this.m_gradientColors = value;
                    this.m_gradientColors.PropertyChanged += new EventHandler(this.OnGradientColorPropertyChanged);
                    base.Invalidate();
                }
            }
        }

        public GradientColor GradientColorsPressed
        {
            get
            {
                return this.m_gradientColorsPressed;
            }
            set
            {
                if (this.m_gradientColorsPressed != value)
                {
                    this.m_gradientColorsPressed.PropertyChanged -= new EventHandler(this.OnGradientColorPropertyChanged);
                    this.m_gradientColorsPressed = null;
                    this.m_gradientColorsPressed = value;
                    this.m_gradientColorsPressed.PropertyChanged += new EventHandler(this.OnGradientColorPropertyChanged);
                    base.Invalidate();
                }
            }
        }

        public Bitmap ImageDefault
        {
            get
            {
                return this.m_imageDefault;
            }
            set
            {
                if (this.m_imageDefault != value)
                {
                    this.m_imageDefault = value;
                    base.Invalidate();
                }
            }
        }

        public Bitmap ImagePressed
        {
            get
            {
                return this.m_imagePressed;
            }
            set
            {
                if (this.m_imagePressed != value)
                {
                    this.m_imagePressed = value;
                    base.Invalidate();
                }
            }
        }

        public Bitmap ImageVgaDefault
        {
            get
            {
                return this.m_imageVgaDefault;
            }
            set
            {
                if (this.m_imageVgaDefault != value)
                {
                    this.m_imageVgaDefault = value;
                    base.Invalidate();
                }
            }
        }

        public Bitmap ImageVgaPressed
        {
            get
            {
                return this.m_imageVgaPressed;
            }
            set
            {
                if (this.m_imageVgaPressed != value)
                {
                    this.m_imageVgaPressed = value;
                    base.Invalidate();
                }
            }
        }

        public Color PressedBackColor
        {
            get
            {
                return this.m_pressedBackColor;
            }
            set
            {
                if (value == Color.Empty)
                {
                    value = Color.Transparent;
                }
                if (this.m_pressedBackColor != value)
                {
                    this.m_pressedBackColor = value;
                    base.Invalidate();
                }
            }
        }

        public Color PressedForeColor
        {
            get
            {
                return this.m_pressedForeColor;
            }
            set
            {
                if (value == Color.Empty)
                {
                    value = Color.Transparent;
                }
                if (this.m_pressedForeColor != value)
                {
                    this.m_pressedForeColor = value;
                    base.Invalidate();
                }
            }
        }

        public int RepeatDelay
        {
            get
            {
                return this.m_repeatDelay;
            }
            set
            {
                if (this.m_repeatDelay != value)
                {
                    this.m_repeatDelay = value;
                }
            }
        }

        public int RepeatIncreaseRate
        {
            get
            {
                return this.m_repeatIncreaseRate;
            }
            set
            {
                if (this.m_repeatIncreaseRate != value)
                {
                    this.m_repeatIncreaseRate = value;
                }
            }
        }

        public int RepeatRate
        {
            get
            {
                return this.m_repeatRate;
            }
            set
            {
                if (this.m_repeatRate != value)
                {
                    this.m_repeatRate = value;
                }
            }
        }

        public Resco.Controls.NumericUpDown.VisualStyle VisualStyle
        {
            get
            {
                return this.m_visualStyle;
            }
            set
            {
                if (this.m_visualStyle != value)
                {
                    this.m_visualStyle = value;
                    base.Invalidate();
                }
            }
        }
    }
}

