namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class ImageButton : UserControl, ISupportInitialize
    {
        private IContainer components;
        private bool m_AutoTransparent = true;
        private bool m_bFocused;
        private Bitmap m_BmpDest;
        private Bitmap m_bmpOffscreen;
        private Bitmap m_BmpSrc;
        private Color m_BorderColor = Color.DarkGray;
        private bool m_BorderEnabled;
        private bool m_bPushed;
        private ButtonType m_ButtonStyle;
        private ConvertToGrayscaleInBackgrnd m_Convertor;
        private Bitmap m_DimmedImage;
        private Bitmap m_DimmedImageVga;
        private static bool m_DisableGrayScaleConverter;
        private bool m_Disposed;
        private bool m_Enabled = true;
        private Color m_FocusedBackColor = Color.Transparent;
        private Color m_FocusedForeColor = Color.Black;
        private Color m_ForeColorPressed = SystemColors.ControlText;
        private GradientColor m_GradientColors = new GradientColor(Color.LightGray, Color.Black);
        private GradientColor m_GradientColorsPressed = new GradientColor(Color.Black, Color.LightGray);
        private Alignment m_ImageAlignment = Alignment.MiddleCenter;
        private Image m_ImageDefault;
        private int m_ImageIndexDefault = -1;
        private int m_ImageIndexPressed = -1;
        private System.Windows.Forms.ImageList m_ImageList;
        private Point m_ImageLocation = new Point(-1, -1);
        private Image m_ImagePressed;
        private Image m_ImageVgaDefault;
        private Image m_ImageVgaPressed;
        private bool m_Initializing;
        private Size m_MaxStretchImageSize = new Size(-1, -1);
        private object m_Mutex = new object();
        private int m_offsetVGA;
        private Color m_PressedBackgrndColor = Color.PowderBlue;
        private Color m_PressedBorderColor = Color.SteelBlue;
        private bool m_StretchImage;
        private string m_Text = "";
        private Alignment m_TextAlignment = Alignment.MiddleCenter;
        private Point m_TextLocation = new Point(-1, -1);
        private Size m_VistaButtonInflate = new Size(-4, -4);
        private bool m_VistaStyleBgrnd;
        private Thread m_WorkThread;

        static ImageButton()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(ImageButton), "");
            //}
        }

        public ImageButton()
        {
            this.InitializeComponent();
            this.InitConvertor();
            this.bPushed = false;
            base.Size = new Size(0x15, 0x15);
            this.Font = new Font("Tahoma", 9f, FontStyle.Regular);
            this.m_offsetVGA = ((base.CurrentAutoScaleDimensions.Width / 96f) > 1f) ? 1 : 0;
            this.m_GradientColors.PropertyChanged += new EventHandler(this.GradientColors_PropertyChanged);
            this.m_GradientColorsPressed.PropertyChanged += new EventHandler(this.GradientColors_PropertyChanged);
        }

        private Color BackgroundImageColor(Image image)
        {
            using (Bitmap bitmap = new Bitmap(image))
            {
                return bitmap.GetPixel(0, 0);
            }
        }

        private void ControlImageButton_GotFocus(object sender, EventArgs e)
        {
            this.bFocused = true;
            base.Invalidate();
        }

        private void ControlImageButton_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                    base.Parent.SelectNextControl(this, false, true, false, true);
                    return;

                case Keys.Right:
                case Keys.Down:
                    base.Parent.SelectNextControl(this, true, true, false, true);
                    return;

                case Keys.Return:
                    if (this.m_ButtonStyle == ButtonType.CheckBox)
                    {
                        this.m_bPushed = !this.m_bPushed;
                    }
                    else
                    {
                        this.bPushed = true;
                    }
                    base.Invalidate();
                    return;
            }
        }

        private void ControlImageButton_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return) && ((this.m_ButtonStyle != ButtonType.CheckBox) && this.bPushed))
            {
                this.bPushed = false;
                base.Invalidate();
                base.OnClick(EventArgs.Empty);
            }
        }

        private void ControlImageButton_LostFocus(object sender, EventArgs e)
        {
            this.bFocused = false;
            base.Invalidate();
        }

        private void DisableEvents()
        {
            base.GotFocus -= new EventHandler(this.ControlImageButton_GotFocus);
            base.KeyUp -= new KeyEventHandler(this.ControlImageButton_KeyUp);
            base.LostFocus -= new EventHandler(this.ControlImageButton_LostFocus);
            base.KeyDown -= new KeyEventHandler(this.ControlImageButton_KeyDown);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.m_Disposed = true;
                if (this.m_WorkThread != null)
                {
                    this.m_WorkThread.Abort();
                }
                if (this.m_Convertor != null)
                {
                    this.m_Convertor.ConversionDone -= new EventHandler(this.OnConvertor_ConversionDone);
                    this.m_Convertor.Dispose();
                    this.m_Convertor = null;
                }
                this.DisableEvents();
                if (this.m_ImageDefault != null)
                {
                    this.m_ImageDefault.Dispose();
                    this.m_ImageDefault = null;
                }
                if (this.m_ImagePressed != null)
                {
                    this.m_ImagePressed.Dispose();
                    this.m_ImagePressed = null;
                }
                if (this.m_ImageVgaDefault != null)
                {
                    this.m_ImageVgaDefault.Dispose();
                    this.m_ImageVgaDefault = null;
                }
                if (this.m_ImageVgaPressed != null)
                {
                    this.m_ImageVgaPressed.Dispose();
                    this.m_ImageVgaPressed = null;
                }
                if (this.m_DimmedImage != null)
                {
                    this.m_DimmedImage.Dispose();
                    this.m_DimmedImage = null;
                }
                if (this.m_DimmedImageVga != null)
                {
                    this.m_DimmedImageVga.Dispose();
                    this.m_DimmedImageVga = null;
                }
                if (this.m_bmpOffscreen != null)
                {
                    this.m_bmpOffscreen.Dispose();
                    this.m_bmpOffscreen = null;
                }
                if (this.m_BmpDest != null)
                {
                    this.m_BmpDest.Dispose();
                    this.m_BmpDest = null;
                }
                if (this.m_BmpSrc != null)
                {
                    this.m_BmpSrc.Dispose();
                    this.m_BmpSrc = null;
                }
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DrawAreaRectangle(Graphics gr, Rectangle rc)
        {
            if ((this.Site != null) && this.Site.DesignMode)
            {
                using (Pen pen = new Pen(Color.White))
                {
                    gr.DrawRectangle(pen, rc);
                }
                using (new Pen(Color.Black))
                {
                    int num = rc.X + rc.Width;
                    int y = rc.Y;
                    int num3 = rc.Y + rc.Height;
                    for (int i = rc.X; i < num; i += 2)
                    {
                        if ((i >= 0) && (i < this.m_bmpOffscreen.Width))
                        {
                            if ((y >= 0) && (y < this.m_bmpOffscreen.Height))
                            {
                                this.m_bmpOffscreen.SetPixel(i, y, Color.Black);
                            }
                            if ((num3 >= 0) && (num3 < this.m_bmpOffscreen.Height))
                            {
                                this.m_bmpOffscreen.SetPixel(i, num3, Color.Black);
                            }
                        }
                    }
                    int num5 = rc.Y + rc.Height;
                    int x = rc.X;
                    int num7 = rc.X + rc.Width;
                    for (int j = rc.Y; j <= num5; j += 2)
                    {
                        if ((j >= 0) && (j < this.m_bmpOffscreen.Height))
                        {
                            if ((x >= 0) && (x < this.m_bmpOffscreen.Width))
                            {
                                this.m_bmpOffscreen.SetPixel(x, j, Color.Black);
                            }
                            if ((num7 >= 0) && (num7 < this.m_bmpOffscreen.Width))
                            {
                                this.m_bmpOffscreen.SetPixel(num7, j, Color.Black);
                            }
                        }
                    }
                }
            }
        }

        private void DrawFocusRectangle(Graphics gr)
        {
            if (this.m_FocusedForeColor != Color.Transparent)
            {
                Rectangle clientRectangle = base.ClientRectangle;
                clientRectangle.Width--;
                clientRectangle.Height--;
                using (Pen pen = new Pen(this.m_FocusedForeColor))
                {
                    gr.DrawRectangle(pen, clientRectangle);
                }
            }
        }

        private void DrawGradientButton(Graphics gr)
        {
            if ((!this.bPushed && this.bFocused) || (((this.Site != null) && this.Site.DesignMode) && !this.m_FocusedBackColor.Equals(Color.Transparent)))
            {
                gr.Clear(this.m_FocusedBackColor);
            }
            else
            {
                gr.Clear(this.ParentBackColor);
            }
            this.DrawGradientButtonBackgrnd(gr);
            if ((this.GetImagePressed() != null) && this.bPushed)
            {
                this.DrawImage(gr, this.GetImagePressed(), false);
            }
            else if (this.GetImageDefault() != null)
            {
                this.DrawImage(gr, this.GetImageDefault(), this.bPushed);
            }
            if ((!this.bPushed && this.bFocused) || ((this.Site != null) && this.Site.DesignMode))
            {
                this.DrawFocusRectangle(gr);
            }
        }

        private void DrawGradientButtonBackgrnd(Graphics gr)
        {
            Color pressedBorderColor;
            Rectangle rc = new Rectangle(0, 0, this.m_bmpOffscreen.Width, this.m_bmpOffscreen.Height);
            rc.Inflate(this.m_VistaButtonInflate.Width, this.m_VistaButtonInflate.Height);
            if (this.bPushed)
            {
                this.m_GradientColorsPressed.DrawGradient(gr, rc);
                pressedBorderColor = this.m_PressedBorderColor;
            }
            else
            {
                this.m_GradientColors.DrawGradient(gr, rc);
                pressedBorderColor = this.m_BorderColor;
            }
            using (Pen pen = new Pen(pressedBorderColor))
            {
                gr.DrawLine(pen, rc.X, rc.Y - 1, (rc.X + rc.Width) - 1, rc.Y - 1);
                gr.DrawLine(pen, rc.X, rc.Y + rc.Height, (rc.X + rc.Width) - 1, rc.Y + rc.Height);
                gr.DrawLine(pen, rc.X - 1, rc.Y, rc.X - 1, (rc.Y + rc.Height) - 1);
                gr.DrawLine(pen, rc.X + rc.Width, rc.Y, rc.X + rc.Width, (rc.Y + rc.Height) - 1);
            }
        }

        private Rectangle DrawImage(Graphics gr, Image image, bool pushed)
        {
            if (((image == null) || (gr == null)) || this.m_Disposed)
            {
                return Rectangle.Empty;
            }
            Rectangle destRect = this.GetImageDestRect(image.Width, image.Height, pushed);
            if (this.m_AutoTransparent)
            {
                ImageAttributes imageAttr = new ImageAttributes();
                Color colorLow = this.BackgroundImageColor(image);
                imageAttr.SetColorKey(colorLow, colorLow);
                gr.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
                return destRect;
            }
            Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
            if ((image != null) && (gr != null))
            {
                gr.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            }
            return destRect;
        }

        private void DrawImageButton(Graphics gr)
        {
            if (!this.m_Disposed)
            {
                if (this.bPushed && (this.GetImagePressed() == null))
                {
                    gr.Clear(this.m_PressedBackgrndColor);
                }
                else if (((this.m_ButtonStyle != ButtonType.PictureBox) && (((!this.bPushed && this.bFocused) || (this.bFocused && (this.m_ButtonStyle == ButtonType.CheckBox))) || ((this.Site != null) && this.Site.DesignMode))) && !this.m_FocusedBackColor.Equals(Color.Transparent))
                {
                    gr.Clear(this.m_FocusedBackColor);
                }
                else
                {
                    gr.Clear(this.ParentBackColor);
                }
                if ((this.GetImagePressed() != null) && this.bPushed)
                {
                    this.DrawImage(gr, this.GetImagePressed(), false);
                }
                else if (this.GetImageDefault() != null)
                {
                    this.DrawImage(gr, this.GetImageDefault(), this.bPushed);
                }
                if (this.bPushed && (this.GetImagePressed() == null))
                {
                    Rectangle clientRectangle = base.ClientRectangle;
                    clientRectangle.Width--;
                    clientRectangle.Height--;
                    using (Pen pen = new Pen(this.m_PressedBorderColor))
                    {
                        gr.DrawRectangle(pen, clientRectangle);
                    }
                }
                if (this.m_BorderEnabled)
                {
                    Rectangle rect = base.ClientRectangle;
                    rect.Width--;
                    rect.Height--;
                    using (Pen pen2 = new Pen(this.m_BorderColor))
                    {
                        gr.DrawRectangle(pen2, rect);
                    }
                }
                if ((this.m_ButtonStyle != ButtonType.PictureBox) && (((!this.bPushed && this.bFocused) || (this.bFocused && (this.m_ButtonStyle == ButtonType.CheckBox))) || ((this.Site != null) && this.Site.DesignMode)))
                {
                    this.DrawFocusRectangle(gr);
                }
            }
        }

        private void DrawText(Graphics gr)
        {
            if ((this.m_Text != null) && (this.m_Text.Length != 0))
            {
                SolidBrush brush;
                Rectangle rectangle;
                int num4;
                int num5;
                if (this.bPushed)
                {
                    brush = new SolidBrush(this.ForeColorPressed);
                }
                else
                {
                    brush = new SolidBrush(this.ForeColor);
                }
                string[] strArray = this.SplitText(this.m_Text, @"\n");
                SizeF ef = new SizeF(0f, 0f);
                float width = 0f;
                float height = 0f;
                for (int i = 0; i < strArray.Length; i++)
                {
                    ef = gr.MeasureString(strArray[i], this.Font);
                    if (width < ef.Width)
                    {
                        width = ef.Width;
                    }
                    if (height < ef.Height)
                    {
                        height = ef.Height;
                    }
                }
                SizeF textSize = new SizeF(width, height * strArray.Length);
                this.GetTextLocation(textSize, out num4, out num5);
                if ((this.bPushed && (this.m_ButtonStyle != ButtonType.VistaStyleImageButton)) && ((this.m_ButtonStyle != ButtonType.GradientImageButton) && (this.GetImagePressed() == null)))
                {
                    rectangle = new Rectangle(num4 + 1, num5 + 1, ((int) textSize.Width) + 1, ((int) textSize.Height) + 1);
                }
                else
                {
                    rectangle = new Rectangle(num4, num5, ((int) textSize.Width) + 1, ((int) textSize.Height) + 1);
                }
                this.DrawAreaRectangle(gr, rectangle);
                Region region = new Region(rectangle);
                gr.Clip = region;
                rectangle.Height = (int) height;
                StringFormat stringFormat = this.GetStringFormat(this.m_TextAlignment);
                for (int j = 0; j < strArray.Length; j++)
                {
                    gr.DrawString(strArray[j], this.Font, brush, rectangle, stringFormat);
                    rectangle.Y += (int) height;
                }
                gr.ResetClip();
                region.Dispose();
                region = null;
                brush.Dispose();
                brush = null;
            }
        }

        private void DrawVistaButton(Graphics gr)
        {
            if ((!this.bPushed && this.bFocused) || (((this.Site != null) && this.Site.DesignMode) && !this.m_FocusedBackColor.Equals(Color.Transparent)))
            {
                gr.Clear(this.m_FocusedBackColor);
            }
            else
            {
                gr.Clear(this.ParentBackColor);
            }
            this.DrawVistaButtonImage(gr);
            if ((this.GetImagePressed() != null) && this.bPushed)
            {
                this.DrawImage(gr, this.GetImagePressed(), false);
            }
            else if (this.GetImageDefault() != null)
            {
                this.DrawImage(gr, this.GetImageDefault(), this.bPushed);
            }
            if ((!this.bPushed && this.bFocused) || ((this.Site != null) && this.Site.DesignMode))
            {
                this.DrawFocusRectangle(gr);
            }
        }

        private void DrawVistaButton2(Graphics gr)
        {
            if ((!this.bPushed && this.bFocused) || (((this.Site != null) && this.Site.DesignMode) && !this.m_FocusedBackColor.Equals(Color.Transparent)))
            {
                gr.Clear(this.m_FocusedBackColor);
            }
            else
            {
                gr.Clear(this.ParentBackColor);
            }
            Bitmap image = new Bitmap(this.m_bmpOffscreen.Width, this.m_bmpOffscreen.Height);
            Graphics graphics = Graphics.FromImage(image);
            Rectangle destRect = new Rectangle(0, 0, this.m_bmpOffscreen.Width, this.m_bmpOffscreen.Height);
            this.DrawVistaButtonImage(graphics);
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorKey(Color.Pink, Color.Pink);
            gr.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            if ((!this.bPushed && this.bFocused) || ((this.Site != null) && this.Site.DesignMode))
            {
                this.DrawFocusRectangle(gr);
            }
            graphics.Dispose();
            graphics = null;
            image.Dispose();
            image = null;
        }

        private void DrawVistaButtonImage(Graphics gr)
        {
            Color pressedBackgrndColor;
            Color pressedBorderColor;
            Rectangle aRect = new Rectangle(0, 0, this.m_bmpOffscreen.Width, this.m_bmpOffscreen.Height);
            aRect.Inflate(this.m_VistaButtonInflate.Width, this.m_VistaButtonInflate.Height);
            if (this.bPushed)
            {
                pressedBackgrndColor = this.m_PressedBackgrndColor;
                pressedBorderColor = this.m_PressedBorderColor;
            }
            else
            {
                pressedBackgrndColor = this.BackColor;
                pressedBorderColor = this.m_BorderColor;
            }
            GradientFill.DrawVistaGradient(gr, pressedBackgrndColor, aRect, FillDirection.Vertical);
            using (Pen pen = new Pen(pressedBorderColor))
            {
                gr.DrawLine(pen, aRect.X, aRect.Y - 1, (aRect.X + aRect.Width) - 1, aRect.Y - 1);
                gr.DrawLine(pen, aRect.X, aRect.Y + aRect.Height, (aRect.X + aRect.Width) - 1, aRect.Y + aRect.Height);
                gr.DrawLine(pen, aRect.X - 1, aRect.Y, aRect.X - 1, (aRect.Y + aRect.Height) - 1);
                gr.DrawLine(pen, aRect.X + aRect.Width, aRect.Y, aRect.X + aRect.Width, (aRect.Y + aRect.Height) - 1);
            }
        }

        private Image GetImageDefault()
        {
            if ((this.m_offsetVGA == 1) && (this.ImageVgaDefault != null))
            {
                if ((!this.m_Enabled && (this.ButtonStyle != ButtonType.PictureBox)) && !m_DisableGrayScaleConverter)
                {
                    return this.m_DimmedImageVga;
                }
                return this.ImageVgaDefault;
            }
            if ((!this.m_Enabled && (this.ButtonStyle != ButtonType.PictureBox)) && !m_DisableGrayScaleConverter)
            {
                return this.m_DimmedImage;
            }
            return this.ImageDefault;
        }

        private Rectangle GetImageDestRect(int anImgWidth, int anImgHeight, bool aPushed)
        {
            int num;
            int num2;
            if (this.m_StretchImage)
            {
                int num3 = (this.m_MaxStretchImageSize.Width <= 0) ? base.Width : (this.m_MaxStretchImageSize.Width + this.m_VistaButtonInflate.Width);
                int num4 = (this.m_MaxStretchImageSize.Height <= 0) ? base.Height : (this.m_MaxStretchImageSize.Height + this.m_VistaButtonInflate.Height);
                this.GetImageLocation(num3, num4, out num, out num2);
                Rectangle rectangle = new Rectangle(num, num2, num3, num4);
                rectangle.Inflate(this.m_VistaButtonInflate.Width, this.m_VistaButtonInflate.Height);
                return rectangle;
            }
            this.GetImageLocation(anImgWidth, anImgHeight, out num, out num2);
            if (!aPushed)
            {
                return new Rectangle(num, num2, anImgWidth, anImgHeight);
            }
            return new Rectangle(num + 1, num2 + 1, anImgWidth, anImgHeight);
        }

        private void GetImageLocation(int anImgWidth, int anImgHeight, out int imageLeft, out int imageTop)
        {
            if ((this.m_ImageLocation.X < 0) && (this.m_ImageLocation.Y < 0))
            {
                this.GetLocationByAlignment(this.m_ImageAlignment, new SizeF((float) anImgWidth, (float) anImgHeight), out imageLeft, out imageTop);
            }
            else
            {
                if (this.m_ImageLocation.X < 0)
                {
                    imageLeft = (base.Width - anImgWidth) / 2;
                }
                else
                {
                    imageLeft = this.m_ImageLocation.X;
                }
                if (this.m_ImageLocation.Y < 0)
                {
                    imageTop = (base.Height - anImgHeight) / 2;
                }
                else
                {
                    imageTop = this.m_ImageLocation.Y;
                }
            }
        }

        private Image GetImagePressed()
        {
            if ((this.m_offsetVGA == 1) && (this.ImageVgaPressed != null))
            {
                return this.ImageVgaPressed;
            }
            return this.ImagePressed;
        }

        private void GetLocationByAlignment(Alignment anAlignment, SizeF aControlSize, out int aTextLeft, out int aTextTop)
        {
            int num = 2;
            int num2 = 2;
            switch (anAlignment)
            {
                case Alignment.TopLeft:
                    aTextTop = 1;
                    aTextLeft = 1;
                    return;

                case Alignment.TopCenter:
                    aTextTop = 1;
                    aTextLeft = (base.Width - ((int) aControlSize.Width)) / 2;
                    return;

                case Alignment.TopRight:
                    aTextTop = 1;
                    aTextLeft = (base.Width - ((int) aControlSize.Width)) - num;
                    return;

                case Alignment.MiddleLeft:
                    aTextTop = (base.Height - ((int) aControlSize.Height)) / 2;
                    aTextLeft = 1;
                    return;

                case Alignment.MiddleCenter:
                    aTextLeft = (base.Width - ((int) aControlSize.Width)) / 2;
                    aTextTop = (base.Height - ((int) aControlSize.Height)) / 2;
                    return;

                case Alignment.MiddleRight:
                    aTextTop = (base.Height - ((int) aControlSize.Height)) / 2;
                    aTextLeft = (base.Width - ((int) aControlSize.Width)) - num;
                    return;

                case Alignment.BottomLeft:
                    aTextTop = (base.Height - ((int) aControlSize.Height)) - num2;
                    aTextLeft = 1;
                    return;

                case Alignment.BottomCenter:
                    aTextTop = (base.Height - ((int) aControlSize.Height)) - num2;
                    aTextLeft = (base.Width - ((int) aControlSize.Width)) / 2;
                    return;

                case Alignment.BottomRight:
                    aTextTop = (base.Height - ((int) aControlSize.Height)) - 2;
                    aTextLeft = (base.Width - ((int) aControlSize.Width)) - num;
                    return;
            }
            aTextLeft = (base.Width - ((int) aControlSize.Width)) / 2;
            aTextTop = (base.Height - ((int) aControlSize.Height)) / 2;
        }

        private StringFormat GetStringFormat(Alignment anAlignment)
        {
            StringFormat format = new StringFormat();
            switch (anAlignment)
            {
                case Alignment.TopLeft:
                    format.LineAlignment = StringAlignment.Near;
                    format.Alignment = StringAlignment.Near;
                    return format;

                case Alignment.TopCenter:
                    format.LineAlignment = StringAlignment.Near;
                    format.Alignment = StringAlignment.Center;
                    return format;

                case Alignment.TopRight:
                    format.LineAlignment = StringAlignment.Near;
                    format.Alignment = StringAlignment.Far;
                    return format;

                case Alignment.MiddleLeft:
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Near;
                    return format;

                case Alignment.MiddleCenter:
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Center;
                    return format;

                case Alignment.MiddleRight:
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Far;
                    return format;

                case Alignment.BottomLeft:
                    format.LineAlignment = StringAlignment.Far;
                    format.Alignment = StringAlignment.Near;
                    return format;

                case Alignment.BottomCenter:
                    format.LineAlignment = StringAlignment.Far;
                    format.Alignment = StringAlignment.Center;
                    return format;

                case Alignment.BottomRight:
                    format.LineAlignment = StringAlignment.Far;
                    format.Alignment = StringAlignment.Far;
                    return format;
            }
            return format;
        }

        private void GetTextLocation(SizeF textSize, out int aTextLeft, out int aTextTop)
        {
            if ((this.m_TextLocation.X < 0) && (this.m_TextLocation.Y < 0))
            {
                this.GetLocationByAlignment(this.m_TextAlignment, textSize, out aTextLeft, out aTextTop);
            }
            else
            {
                if (this.m_TextLocation.X < 0)
                {
                    aTextLeft = (base.Width - ((int) textSize.Width)) / 2;
                }
                else
                {
                    aTextLeft = this.m_TextLocation.X;
                }
                if (this.m_TextLocation.Y < 0)
                {
                    aTextTop = (base.Height - ((int) textSize.Height)) / 2;
                }
                else
                {
                    aTextTop = this.m_TextLocation.Y;
                }
            }
        }

        private void GradientColors_PropertyChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void InitConvertor()
        {
            this.m_Convertor = new ConvertToGrayscaleInBackgrnd();
            this.m_Convertor.ConversionDone += new EventHandler(this.OnConvertor_ConversionDone);
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions=(new SizeF(96f, 96f));
            //base.set_AutoScaleMode(2);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackColor = Color.Transparent;
            base.Name = "ImageButton";
            base.Size = new Size(0x8a, 0x3d);
            base.GotFocus += new EventHandler(this.ControlImageButton_GotFocus);
            base.KeyUp += new KeyEventHandler(this.ControlImageButton_KeyUp);
            base.LostFocus += new EventHandler(this.ControlImageButton_LostFocus);
            base.KeyDown += new KeyEventHandler(this.ControlImageButton_KeyDown);
            base.ResumeLayout(false);
        }

        protected override void OnClick(EventArgs e)
        {
        }

        private void OnConvertor_ConversionDone(object sender, EventArgs e)
        {
            this.RefreshControlAfterConvert();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }

        private void OnImageListChanged()
        {
            if (((this.m_ImageList != null) && (this.m_ImageIndexPressed >= 0)) && (this.m_ImageList.Images.Count > this.m_ImageIndexPressed))
            {
                this.ImagePressed = this.m_ImageList.Images[this.m_ImageIndexPressed];
            }
            if (((this.m_ImageList != null) && (this.m_ImageIndexDefault >= 0)) && (this.m_ImageList.Images.Count > this.m_ImageIndexDefault))
            {
                this.ImageDefault = this.m_ImageList.Images[this.m_ImageIndexDefault];
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.m_ButtonStyle == ButtonType.CheckBox)
            {
                this.m_bPushed = !this.m_bPushed;
            }
            else
            {
                this.bPushed = true;
            }
            base.OnMouseDown(e);
            base.Capture = true;
            this.Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (base.ClientRectangle.Contains(new Point(e.X, e.Y)))
            {
                base.OnClick(e);
            }
            if (this.m_ButtonStyle == ButtonType.CheckBox)
            {
                base.OnMouseUp(e);
                base.Capture = false;
            }
            else
            {
                this.bPushed = false;
                base.OnMouseUp(e);
                this.Refresh();
                base.Capture = false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!this.m_Disposed)
            {
                if (this.m_bmpOffscreen == null)
                {
                    this.m_bmpOffscreen = new Bitmap(base.ClientSize.Width, base.ClientSize.Height);
                }
                Graphics gr = Graphics.FromImage(this.m_bmpOffscreen);
                switch (this.m_ButtonStyle)
                {
                    case ButtonType.GradientImageButton:
                        this.DrawGradientButton(gr);
                        break;

                    case ButtonType.VistaStyleImageButton:
                        this.DrawVistaButton(gr);
                        break;

                    default:
                        this.DrawImageButton(gr);
                        break;
                }
                this.DrawText(gr);
                e.Graphics.DrawImage(this.m_bmpOffscreen, 0, 0);
                base.OnPaint(e);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            if (this.m_bmpOffscreen != null)
            {
                this.m_bmpOffscreen.Dispose();
                this.m_bmpOffscreen = null;
            }
            base.OnResize(e);
        }

        private void PrepareDimmedImage(ThreadPriority aPriority)
        {
            if (!m_DisableGrayScaleConverter)
            {
                if (this.m_offsetVGA == 0)
                {
                    if (this.m_DimmedImage == null)
                    {
                        this.ReloadDimmedImage(aPriority);
                    }
                }
                else if ((this.m_DimmedImageVga == null) || ((this.m_DimmedImage == null) && (this.m_ImageVgaDefault == null)))
                {
                    this.ReloadDimmedImage(aPriority);
                }
            }
        }

        private void RefreshControlAfterConvert()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    base.Invoke(new InvokeDelegate(this.RefreshControlAfterConvert));
                }
                catch (ObjectDisposedException)
                {
                }
            }
            else if (this.ButtonStyle != ButtonType.PictureBox)
            {
                if ((this.m_offsetVGA == 0) || (this.m_ImageVgaDefault == null))
                {
                    if (this.m_DimmedImage != null)
                    {
                        this.m_DimmedImage.Dispose();
                        this.m_DimmedImage = null;
                    }
                    if (this.m_Convertor.BmpGray != null)
                    {
                        this.m_DimmedImage = new Bitmap(this.m_Convertor.BmpGray);
                    }
                }
                else
                {
                    if (this.m_DimmedImageVga != null)
                    {
                        this.m_DimmedImageVga.Dispose();
                        this.m_DimmedImageVga = null;
                    }
                    if (this.m_Convertor.BmpGray != null)
                    {
                        this.m_DimmedImageVga = new Bitmap(this.m_Convertor.BmpGray);
                    }
                }
                if (!this.m_Enabled)
                {
                    this.Refresh();
                }
            }
        }

        private void ReloadDimmedImage(ThreadPriority aPriority)
        {
            if ((!m_DisableGrayScaleConverter && ((this.Site == null) || !this.Site.DesignMode)) && (this.ButtonStyle != ButtonType.PictureBox))
            {
                Bitmap aBmp = ((this.m_offsetVGA == 0) || (this.m_ImageVgaDefault == null)) ? (this.m_ImageDefault as Bitmap) : (this.m_ImageVgaDefault as Bitmap);
                if (aBmp != null)
                {
                    this.m_Convertor.Convert(aBmp, aPriority);
                }
            }
        }

        private void ResetImageDefault()
        {
            if (this.m_ImageDefault != null)
            {
                this.m_ImageDefault.Dispose();
                this.m_ImageDefault = null;
            }
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
        }

        protected virtual bool ShouldSerializeBorderColor()
        {
            return !this.m_BorderColor.Equals(Color.DarkGray);
        }

        protected virtual bool ShouldSerializeBorderEnabled()
        {
            return this.m_BorderEnabled;
        }

        protected virtual bool ShouldSerializeButtonStyle()
        {
            return true;
        }

        protected virtual bool ShouldSerializeDisableGrayScaleConverter()
        {
            return true;
        }

        protected virtual bool ShouldSerializeFocusedBackColor()
        {
            return !this.m_FocusedBackColor.Equals(Color.Transparent);
        }

        protected virtual bool ShouldSerializeGradientColors()
        {
            return (this.m_GradientColors != null);
        }

        protected virtual bool ShouldSerializeGradientColorsPressed()
        {
            return (this.m_GradientColorsPressed != null);
        }

        protected virtual bool ShouldSerializeImageAlignment()
        {
            if (this.m_ImageAlignment == Alignment.MiddleCenter)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeImageDefault()
        {
            if (this.m_ImageDefault == null)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeImageIndexDefault()
        {
            return (this.m_ImageIndexDefault != -1);
        }

        protected virtual bool ShouldSerializeImageIndexPressed()
        {
            return (this.m_ImageIndexPressed != -1);
        }

        protected virtual bool ShouldSerializeImageList()
        {
            return (this.m_ImageList != null);
        }

        protected virtual bool ShouldSerializeImageLocation()
        {
            if ((this.m_ImageLocation.X == -1) && (this.m_ImageLocation.Y == -1))
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeImagePressed()
        {
            if (this.m_ImagePressed == null)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeImageVgaDefault()
        {
            if (this.m_ImageVgaDefault == null)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeImageVgaPressed()
        {
            if (this.m_ImageVgaPressed == null)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeMaxStretchImageSize()
        {
            if ((this.m_MaxStretchImageSize.Height <= 0) && (this.m_MaxStretchImageSize.Width <= 0))
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeTextAlignment()
        {
            if (this.m_TextAlignment == Alignment.MiddleCenter)
            {
                return false;
            }
            return true;
        }

        protected virtual bool ShouldSerializeTextDefault()
        {
            return ((this.TextDefault != null) && (this.TextDefault.Length != 0));
        }

        protected virtual bool ShouldSerializeVistaStyleBackground()
        {
            return false;
        }

        private string[] SplitText(string aText, string aSeparator)
        {
            List<string> list = new List<string>();
            string str = aText;
            for (int i = str.IndexOf(aSeparator); i > -1; i = str.IndexOf(aSeparator))
            {
                list.Add(str.Substring(0, i));
                str = str.Remove(0, i + aSeparator.Length);
            }
            list.Add(str);
            return list.ToArray();
        }

        void ISupportInitialize.BeginInit()
        {
            this.m_Initializing = true;
        }

        void ISupportInitialize.EndInit()
        {
            this.m_Initializing = false;
        }

        public bool AutoTransparent
        {
            get
            {
                return this.m_AutoTransparent;
            }
            set
            {
                if (this.m_AutoTransparent != value)
                {
                    this.m_AutoTransparent = value;
                    base.Invalidate();
                }
            }
        }

        private bool bFocused
        {
            get
            {
                return this.m_bFocused;
            }
            set
            {
                if (this.m_ButtonStyle == ButtonType.PictureBox)
                {
                    this.m_bFocused = false;
                }
                else
                {
                    this.m_bFocused = value;
                }
            }
        }

        public Color BorderColor
        {
            get
            {
                return this.m_BorderColor;
            }
            set
            {
                this.m_BorderColor = value;
                base.Invalidate();
            }
        }

        public bool BorderEnabled
        {
            get
            {
                return this.m_BorderEnabled;
            }
            set
            {
                if (this.m_BorderEnabled != value)
                {
                    this.m_BorderEnabled = value;
                    base.Invalidate();
                }
            }
        }

        private bool bPushed
        {
            get
            {
                return this.m_bPushed;
            }
            set
            {
                if (this.m_ButtonStyle == ButtonType.PictureBox)
                {
                    this.m_bPushed = false;
                }
                else
                {
                    this.m_bPushed = value;
                }
            }
        }

        public ButtonType ButtonStyle
        {
            get
            {
                return this.m_ButtonStyle;
            }
            set
            {
                if (this.m_ButtonStyle != value)
                {
                    this.m_ButtonStyle = value;
                    if (this.m_ButtonStyle == ButtonType.PictureBox)
                    {
                        this.Enabled = false;
                    }
                    else
                    {
                        this.Enabled = true;
                    }
                    base.Invalidate();
                }
            }
        }

        public bool Checked
        {
            get
            {
                return this.m_bPushed;
            }
            set
            {
                if ((this.m_ButtonStyle == ButtonType.CheckBox) && (this.m_bPushed != value))
                {
                    this.m_bPushed = value;
                    base.Invalidate();
                }
            }
        }

        public static bool DisableGrayScaleConverter
        {
            get
            {
                return m_DisableGrayScaleConverter;
            }
            set
            {
                m_DisableGrayScaleConverter = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.m_Enabled;
            }
            set
            {
                if (this.m_Enabled != value)
                {
                    this.m_Enabled = value;
                    base.Enabled = value;
                    if (!this.m_Enabled)
                    {
                        if (this.m_offsetVGA == 0)
                        {
                            if (this.m_DimmedImage == null)
                            {
                                this.PrepareDimmedImage(ThreadPriority.AboveNormal);
                            }
                            else
                            {
                                base.Invalidate();
                            }
                        }
                        else if (this.m_DimmedImageVga == null)
                        {
                            this.PrepareDimmedImage(ThreadPriority.AboveNormal);
                        }
                        else
                        {
                            base.Invalidate();
                        }
                    }
                    else
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        public Color FocusedBackColor
        {
            get
            {
                return this.m_FocusedBackColor;
            }
            set
            {
                this.m_FocusedBackColor = value;
                base.Invalidate();
            }
        }

        public Color FocusedColor
        {
            get
            {
                return this.m_FocusedForeColor;
            }
            set
            {
                this.m_FocusedForeColor = value;
                base.Invalidate();
            }
        }

        public Color ForeColorPressed
        {
            get
            {
                return this.m_ForeColorPressed;
            }
            set
            {
                this.m_ForeColorPressed = value;
                base.Invalidate();
            }
        }

        public GradientColor GradientColors
        {
            get
            {
                return this.m_GradientColors;
            }
            set
            {
                this.m_GradientColors = value;
                base.Invalidate();
            }
        }

        public GradientColor GradientColorsPressed
        {
            get
            {
                return this.m_GradientColorsPressed;
            }
            set
            {
                this.m_GradientColorsPressed = value;
                base.Invalidate();
            }
        }

        public Alignment ImageAlignment
        {
            get
            {
                return this.m_ImageAlignment;
            }
            set
            {
                if (this.m_ImageAlignment != value)
                {
                    this.m_ImageAlignment = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue((string) null)]
        public Image ImageDefault
        {
            get
            {
                return this.m_ImageDefault;
            }
            set
            {
                this.m_ImageDefault = value;
                if (this.m_offsetVGA == 0)
                {
                    if (!this.m_Enabled)
                    {
                        this.ReloadDimmedImage(ThreadPriority.AboveNormal);
                    }
                    else
                    {
                        this.ReloadDimmedImage(ThreadPriority.BelowNormal);
                    }
                }
                base.Invalidate();
            }
        }

        public int ImageIndexDefault
        {
            get
            {
                return this.m_ImageIndexDefault;
            }
            set
            {
                this.m_ImageIndexDefault = value;
                if (((this.m_ImageList != null) && (this.m_ImageIndexDefault >= 0)) && (this.m_ImageList.Images.Count > this.m_ImageIndexDefault))
                {
                    this.ImageDefault = this.m_ImageList.Images[this.m_ImageIndexDefault];
                }
            }
        }

        public int ImageIndexPressed
        {
            get
            {
                return this.m_ImageIndexPressed;
            }
            set
            {
                this.m_ImageIndexPressed = value;
                if (((this.m_ImageList != null) && (this.m_ImageIndexPressed >= 0)) && (this.m_ImageList.Images.Count > this.m_ImageIndexPressed))
                {
                    this.ImagePressed = this.m_ImageList.Images[this.m_ImageIndexPressed];
                }
            }
        }

        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                return this.m_ImageList;
            }
            set
            {
                this.m_ImageList = value;
                this.OnImageListChanged();
                base.Invalidate();
            }
        }

        public Point ImageLocation
        {
            get
            {
                return this.m_ImageLocation;
            }
            set
            {
                if (!this.m_ImageLocation.Equals(value) && ((value.X < base.Width) && (value.Y < base.Height)))
                {
                    this.m_ImageLocation = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue((string) null)]
        public Image ImagePressed
        {
            get
            {
                return this.m_ImagePressed;
            }
            set
            {
                this.m_ImagePressed = value;
            }
        }

        [DefaultValue((string) null)]
        public Image ImageVgaDefault
        {
            get
            {
                return this.m_ImageVgaDefault;
            }
            set
            {
                this.m_ImageVgaDefault = value;
                if (this.m_offsetVGA == 1)
                {
                    if (!this.m_Enabled)
                    {
                        this.ReloadDimmedImage(ThreadPriority.AboveNormal);
                    }
                    else
                    {
                        this.ReloadDimmedImage(ThreadPriority.BelowNormal);
                    }
                }
                base.Invalidate();
            }
        }

        [DefaultValue((string) null)]
        public Image ImageVgaPressed
        {
            get
            {
                return this.m_ImageVgaPressed;
            }
            set
            {
                this.m_ImageVgaPressed = value;
            }
        }

        public Size MaxStretchImageSize
        {
            get
            {
                return this.m_MaxStretchImageSize;
            }
            set
            {
                if (this.m_MaxStretchImageSize != value)
                {
                    this.m_MaxStretchImageSize = value;
                    base.Invalidate();
                }
            }
        }

        private Color ParentBackColor
        {
            get
            {
                if (!this.BackColor.Equals(Color.Transparent) && (this.m_ButtonStyle != ButtonType.VistaStyleImageButton))
                {
                    return this.BackColor;
                }
                return base.Parent.BackColor;
            }
        }

        public Color PressedBackColor
        {
            get
            {
                return this.m_PressedBackgrndColor;
            }
            set
            {
                this.m_PressedBackgrndColor = value;
                base.Invalidate();
            }
        }

        public Color PressedBorderColor
        {
            get
            {
                return this.m_PressedBorderColor;
            }
            set
            {
                this.m_PressedBorderColor = value;
                base.Invalidate();
            }
        }

        public bool StretchImage
        {
            get
            {
                return this.m_StretchImage;
            }
            set
            {
                if (this.m_StretchImage != value)
                {
                    this.m_StretchImage = value;
                    base.Invalidate();
                }
            }
        }

        public Alignment TextAlignment
        {
            get
            {
                return this.m_TextAlignment;
            }
            set
            {
                if (this.m_TextAlignment != value)
                {
                    this.m_TextAlignment = value;
                    base.Invalidate();
                }
            }
        }

        public string TextDefault
        {
            get
            {
                return this.m_Text;
            }
            set
            {
                if (this.m_Text != value)
                {
                    this.m_Text = value;
                    base.Invalidate();
                }
            }
        }

        public Point TextLocation
        {
            get
            {
                return this.m_TextLocation;
            }
            set
            {
                if (!this.m_TextLocation.Equals(value) && ((value.X < base.Width) && (value.Y < base.Height)))
                {
                    this.m_TextLocation = value;
                    base.Invalidate();
                }
            }
        }

        public Size VistaButtonInflate
        {
            get
            {
                return this.m_VistaButtonInflate;
            }
            set
            {
                if (this.m_VistaButtonInflate != value)
                {
                    this.m_VistaButtonInflate = value;
                    base.Invalidate();
                }
            }
        }

        public enum ButtonType
        {
            ImageButton,
            GradientImageButton,
            VistaStyleImageButton,
            PictureBox,
            CheckBox
        }

        private delegate void InvokeDelegate();
    }
}

