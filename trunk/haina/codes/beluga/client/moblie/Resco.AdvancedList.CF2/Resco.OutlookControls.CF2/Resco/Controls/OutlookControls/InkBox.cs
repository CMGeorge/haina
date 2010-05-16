namespace Resco.Controls.OutlookControls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class InkBox : UserControl
    {
        private const int BLACKNESS = 0x42;
        private const int GPTR = 0x40;
        private HScrollBar hScrollBar;
        private Bitmap m_bmp;
        private Bitmap m_bmpBackground;
        private SolidBrush m_brushBack;
        private List<Point> m_currentLine = new List<Point>();
        private bool m_Drawing = false;
        private GradientColor m_gradientBackColor;
        private Graphics m_graphics;
        private Pen m_hPen;
        private Bitmap m_Image;
        private Bitmap m_ImageDC;
        private System.Drawing.Size m_ImageSize;
        private bool m_ImageSizeCustom;
        private bool m_isEmpty = true;
        private bool m_IsQvga = true;
        private Point m_lastPositon = new Point(0, 0);
        private Pen m_penFore;
        private int m_penWidth;
        internal bool m_QuickDraw;
        private List<List<Point>> m_RedoSegments = new List<List<Point>>();
        private List<List<Point>> m_Segments = new List<List<Point>>();
        private bool m_useGradient;
        private const int NOTSRCCOPY = 0x330008;
        private const int SRCCOPY = 0xcc0020;
        private VScrollBar vScrollBar;
        private const int WHITENESS = 0xff0062;

        public event ContentsChangedEventHandler ContentsChanged;

        static InkBox()
        {
            //if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            //{
            //    RescoLicenseMessage.ShowEvaluationMessage(typeof(InkBox), "");
            //}
        }

        public InkBox()
        {
            this.m_lastPositon = new Point(0, 0);
            this.Width = 200;
            this.Height = 80;
            this.m_ImageSize = new System.Drawing.Size(this.Width, this.Height);
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
            this.m_gradientBackColor = new GradientColor();
            this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
            this.m_useGradient = false;
            this.m_penWidth = 0;
            this.InitScrollBar();
            Graphics graphics = base.CreateGraphics();
            this.m_IsQvga = graphics.DpiX == 96f;
            graphics.Dispose();
            graphics = null;
            this.CreateMemoryBitmap();
            this.CreateGdiObjects();
            this.CreatePenObjects();
        }

        private void _DrawLine(int x1, int y1, int x2, int y2)
        {
            this.m_graphics.DrawLine(this.m_penFore, x1, y1, x2, y2);
            using (Graphics graphics = Graphics.FromImage(this.m_ImageDC))
            {
                graphics.DrawLine(this.m_hPen, x1, y1, x2, y2);
            }
        }

        private void AdjustScrollbars()
        {
            if (this.hScrollBar != null)
            {
                if (!this.m_ImageSizeCustom)
                {
                    this.hScrollBar.Hide();
                }
                else
                {
                    int num = this.ImageSize.Width - this.Width;
                    if ((this.vScrollBar != null) && this.vScrollBar.Visible)
                    {
                        num += this.vScrollBar.Width;
                    }
                    if (num <= 0)
                    {
                        this.hScrollBar.Hide();
                    }
                    else
                    {
                        int num2 = this.ImageSize.Width / 5;
                        this.hScrollBar.LargeChange = (num2 < 5) ? 5 : num2;
                        int num3 = this.ImageSize.Width / 10;
                        this.hScrollBar.SmallChange = (num3 < 1) ? 1 : num3;
                        this.hScrollBar.Maximum = num + this.hScrollBar.LargeChange;
                        this.hScrollBar.Show();
                    }
                }
            }
            if (this.vScrollBar != null)
            {
                if (!this.m_ImageSizeCustom)
                {
                    this.vScrollBar.Hide();
                }
                else
                {
                    int num4 = this.ImageSize.Height - this.Height;
                    if ((this.hScrollBar != null) && this.hScrollBar.Visible)
                    {
                        num4 += this.hScrollBar.Height;
                    }
                    if (num4 <= 0)
                    {
                        this.vScrollBar.Hide();
                    }
                    else
                    {
                        int num5 = this.ImageSize.Height / 5;
                        this.vScrollBar.LargeChange = (num5 < 5) ? 5 : num5;
                        int num6 = this.ImageSize.Height / 10;
                        this.vScrollBar.SmallChange = (num6 < 1) ? 1 : num6;
                        this.vScrollBar.Maximum = num4 + this.vScrollBar.LargeChange;
                        this.hScrollBar.Maximum = ((this.ImageSize.Width - this.Width) + this.vScrollBar.Width) + this.hScrollBar.LargeChange;
                        this.vScrollBar.Show();
                    }
                }
            }
        }

        [DllImport("coredll.dll")]
        private static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
        public bool CanRedo()
        {
            return (this.m_RedoSegments.Count > 0);
        }

        public bool CanUndo()
        {
            return (this.m_Segments.Count > 0);
        }

        public void Clear()
        {
            this.m_isEmpty = true;
            this.m_Segments.Clear();
            this.m_RedoSegments.Clear();
            Graphics.FromImage(this.m_bmp).Clear(Color.FromArgb(0xff, 0, 0xff));
            Graphics.FromImage(this.m_ImageDC).Clear(Color.Black);
            this.Refresh();
        }

        private static int ColorToColorRef(Color color)
        {
            int num = color.ToArgb();
            num ^= (num & 0xff) << 0x10;
            num ^= (num >> 0x10) & 0xff;
            return (num ^ ((num & 0xff) << 0x10));
        }

        [DllImport("coredll.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("coredll.dll")]
        private static extern IntPtr CreateDIBSection(IntPtr hdc, IntPtr hdr, uint colors, ref IntPtr pBits, IntPtr hFile, uint offset);
        private void CreateGdiObjects()
        {
            if ((this.m_brushBack == null) || (this.m_brushBack.Color != this.BackColor))
            {
                if (this.m_brushBack != null)
                {
                    this.m_brushBack.Dispose();
                    this.m_brushBack = null;
                }
                this.m_brushBack = new SolidBrush(this.BackColor);
                this.RepaintLines();
            }
        }

        private void CreateMemoryBitmap()
        {
            if (((this.m_Image == null) || (this.m_bmp == null)) || (((this.m_Image.Width != this.ImageSize.Width) || (this.m_Image.Height != this.ImageSize.Height)) || (this.m_ImageDC == null)))
            {
                if (this.m_Image != null)
                {
                    this.m_Image.Dispose();
                    this.m_Image = null;
                }
                if (this.m_graphics != null)
                {
                    this.m_graphics.Dispose();
                    this.m_graphics = null;
                }
                if (this.m_bmp != null)
                {
                    this.m_bmp.Dispose();
                    this.m_bmp = null;
                }
                if ((this.ImageSize.Width <= 0) || (this.ImageSize.Height <= 0))
                {
                    this.m_ImageSize = this.Size;
                }
                this.m_Image = new Bitmap(this.ImageSize.Width, this.ImageSize.Height);
                this.m_graphics = Graphics.FromImage(this.m_Image);
                this.m_bmp = new Bitmap(this.ImageSize.Width, this.ImageSize.Height);
                using (Graphics graphics = Graphics.FromImage(this.m_bmp))
                {
                    graphics.Clear(Color.FromArgb(0xff, 0, 0xff));
                }
                this.AdjustScrollbars();
                this.CreateNativeBitmap();
                this.RepaintLines();
            }
        }

        private void CreateNativeBitmap()
        {
            if (this.m_ImageDC != null)
            {
                this.m_ImageDC.Dispose();
                this.m_ImageDC = null;
            }
            this.m_ImageDC = new Bitmap(this.ImageSize.Width, this.ImageSize.Height);
            using (Graphics graphics = Graphics.FromImage(this.m_ImageDC))
            {
                graphics.Clear(Color.Black);
            }
        }

        [DllImport("coredll.dll")]
        private static extern IntPtr CreatePen(int fnPenStyle, int nWidth, int crColor);
        private void CreatePenObjects()
        {
            if (((((this.m_penFore == null) || (this.m_hPen == null)) || (this.m_penFore.Color != this.ForeColor)) || ((this.m_penWidth > 0) && (this.m_penFore.Width != this.m_penWidth))) || ((this.m_penWidth == 0) && (this.m_penFore.Width != (this.m_IsQvga ? ((float) 1) : ((float) 2)))))
            {
                if (this.m_penFore != null)
                {
                    this.m_penFore.Dispose();
                    this.m_penFore = null;
                }
                this.m_penFore = new Pen(this.ForeColor, (this.m_penWidth > 0) ? ((float) this.m_penWidth) : (this.m_IsQvga ? ((float) 1) : ((float) 2)));
                if (this.m_hPen == null)
                {
                    this.m_hPen = new Pen(Color.White, (this.m_penWidth > 0) ? ((float) this.m_penWidth) : (this.m_IsQvga ? ((float) 1) : ((float) 2)));
                }
                else
                {
                    this.m_hPen.Width = (this.m_penWidth > 0) ? ((float) this.m_penWidth) : (this.m_IsQvga ? ((float) 1) : ((float) 2));
                }
                this.RepaintLines();
            }
        }

        [DllImport("coredll.dll")]
        private static extern void DeleteDC(IntPtr hDC);
        [DllImport("coredll.dll")]
        private static extern void DeleteObject(IntPtr hObj);
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_bmp != null)
                {
                    this.m_bmp.Dispose();
                    this.m_bmp = null;
                }
                if (this.m_bmpBackground != null)
                {
                    this.m_bmpBackground.Dispose();
                    this.m_bmpBackground = null;
                }
                if (this.m_brushBack != null)
                {
                    this.m_brushBack.Dispose();
                    this.m_brushBack = null;
                }
                if (this.m_graphics != null)
                {
                    this.m_graphics.Dispose();
                    this.m_graphics = null;
                }
                if (this.m_hPen != null)
                {
                    this.m_hPen.Dispose();
                    this.m_hPen = null;
                }
                if (this.m_Image != null)
                {
                    this.m_Image.Dispose();
                    this.m_Image = null;
                }
                if (this.m_ImageDC != null)
                {
                    this.m_ImageDC.Dispose();
                    this.m_ImageDC = null;
                }
                if (this.m_penFore != null)
                {
                    this.m_penFore.Dispose();
                    this.m_penFore = null;
                }
            }
            base.Dispose(disposing);
        }

        private void Draw(Graphics gr)
        {
            int x = (this.hScrollBar == null) ? 0 : -this.hScrollBar.Value;
            int y = (this.vScrollBar == null) ? 0 : -this.vScrollBar.Value;
            gr.DrawImage(this.m_Image, x, y);
        }

        [DllImport("coredll.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);
        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleMode=AutoScaleMode.Inherit;
            base.Name = "InkBox";
            base.ResumeLayout(false);
        }

        private void InitScrollBar()
        {
            this.hScrollBar = new HScrollBar();
            this.vScrollBar = new VScrollBar();
            this.hScrollBar.Dock = DockStyle.Bottom;
            this.hScrollBar.Location = new Point(0, 0x89);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(150, 13);
            this.hScrollBar.TabIndex = 0;
            this.hScrollBar.Visible = false;
            this.hScrollBar.ValueChanged += new EventHandler(this.hScrollBar_ValueChanged);
            this.vScrollBar.Dock = DockStyle.Right;
            this.vScrollBar.Location = new Point(0x89, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(13, 0x89);
            this.vScrollBar.TabIndex = 1;
            this.vScrollBar.Visible = false;
            this.vScrollBar.ValueChanged += new EventHandler(this.vScrollBar_ValueChanged);
            base.Controls.Add(this.vScrollBar);
            base.Controls.Add(this.hScrollBar);
        }

        private void LineTo(IntPtr hDC, int x1, int y1, int x2, int y2)
        {
            LPPOINT2 lppt = new LPPOINT2();
            lppt.x1 = x1;
            lppt.y1 = y1;
            lppt.x2 = x2;
            lppt.y2 = y2;
            Polyline(hDC, ref lppt, 2);
        }

        public void Load(Stream stream, BitmapType type, bool setSize)
        {
            BinaryReader reader = new BinaryReader(stream);
            Stream output = stream;
            BMPHEADER bmpheader = null;
            byte[] pBits = null;
            int width = 0;
            int iWidth = 0;
            int height = 0;
            switch (type)
            {
                case BitmapType.Binary:
                case BitmapType.BinaryStrict:
                {
                    BINARYHEADER binaryheader = new BINARYHEADER();
                    binaryheader.Read(reader);
                    width = binaryheader.iWidth;
                    iWidth = binaryheader.iWidth;
                    height = binaryheader.iHeight;
                    if ((width % 8) != 0)
                    {
                        width = (width / 8) + 1;
                    }
                    else
                    {
                        width /= 8;
                    }
                    if ((width % 4) != 0)
                    {
                        width += 4 - (width % 4);
                    }
                    this.ForeColor = Color.FromArgb(binaryheader.cForeground.rgbRed, binaryheader.cForeground.rgbGreen, binaryheader.cForeground.rgbBlue);
                    this.BackColor = Color.FromArgb(binaryheader.cBackground.rgbRed, binaryheader.cBackground.rgbGreen, binaryheader.cBackground.rgbBlue);
                    output = new MemoryStream();
                    BinaryWriter writer = new BinaryWriter(output);
                    bmpheader = new BMPHEADER();
                    bmpheader.header.bfSize = bmpheader.header.bfOffBits + ((uint) (width * height));
                    bmpheader.info.bmiHeader.biWidth = iWidth;
                    bmpheader.info.bmiHeader.biHeight = height;
                    bmpheader.info.bmiHeader.biSizeImage = (uint) (width * height);
                    bmpheader.info.bmiColor1 = new RGBQUAD(this.BackColor.B, this.BackColor.G, this.BackColor.R);
                    bmpheader.info.bmiColor2 = new RGBQUAD(this.ForeColor.B, this.ForeColor.G, this.ForeColor.R);
                    bmpheader.Write(writer);
                    long position = reader.BaseStream.Position;
                    pBits = reader.ReadBytes((int) (reader.BaseStream.Length - reader.BaseStream.Position));
                    if (type == BitmapType.BinaryStrict)
                    {
                        this.SwapLines(pBits, width, height);
                    }
                    writer.Write(pBits);
                    reader.BaseStream.Position = position;
                    break;
                }
                default:
                    bmpheader = new BMPHEADER();
                    bmpheader.Read(reader);
                    width = bmpheader.info.bmiHeader.biWidth;
                    iWidth = bmpheader.info.bmiHeader.biWidth;
                    height = bmpheader.info.bmiHeader.biHeight;
                    if ((width % 8) == 0)
                    {
                        width /= 8;
                    }
                    else
                    {
                        width = (width / 8) + 1;
                    }
                    if ((width % 4) != 0)
                    {
                        width += 4 - (width % 4);
                    }
                    this.BackColor = Color.FromArgb(bmpheader.info.bmiColor1.rgbRed, bmpheader.info.bmiColor1.rgbGreen, bmpheader.info.bmiColor1.rgbBlue);
                    this.ForeColor = Color.FromArgb(bmpheader.info.bmiColor2.rgbRed, bmpheader.info.bmiColor2.rgbGreen, bmpheader.info.bmiColor2.rgbBlue);
                    pBits = reader.ReadBytes((int) bmpheader.info.bmiHeader.biSizeImage);
                    break;
            }
            if (setSize)
            {
                this.m_ImageSize.Width = iWidth;
                this.m_ImageSize.Height = height;
                if (!this.m_ImageSizeCustom)
                {
                    this.Size = this.m_ImageSize;
                }
            }
            base.CreateGraphics();
            this.CreateMemoryBitmap();
            this.CreateGdiObjects();
            this.CreatePenObjects();
            Bitmap image = new Bitmap(this.ImageSize.Width, this.ImageSize.Height);
            Graphics.FromImage(image).Clear(Color.FromArgb(0xff, 0, 0xff));
            Bitmap bitmap2 = new Bitmap(output);
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorKey(this.BackColor, this.BackColor);
            Graphics.FromImage(image).DrawImage(bitmap2, new Rectangle(0, 0, image.Width, image.Height), 0, 0, bitmap2.Width, bitmap2.Height, GraphicsUnit.Pixel, imageAttr);
            if (bitmap2 != null)
            {
                bitmap2.Dispose();
                bitmap2 = null;
            }
            this.Clear();
            IntPtr dC = GetDC(base.Handle);
            IntPtr hdc = CreateCompatibleDC(dC);
            IntPtr zero = IntPtr.Zero;
            IntPtr ptr = LocalAlloc(0x40, (uint) (bmpheader.info.bmiHeader.biSize + 8));
            Marshal.StructureToPtr(bmpheader.info.bmiHeader, ptr, false);
            Marshal.WriteInt32(ptr, bmpheader.info.bmiHeader.biSize, 0);
            Marshal.WriteInt32(ptr, bmpheader.info.bmiHeader.biSize + 4, 0xffffff);
            IntPtr hObj = CreateDIBSection(dC, ptr, 0, ref zero, IntPtr.Zero, 0);
            if ((type == BitmapType.BinaryStrict) && (pBits == null))
            {
                pBits = reader.ReadBytes((int) bmpheader.info.bmiHeader.biSizeImage);
                this.SwapLines(pBits, width, height);
            }
            Marshal.Copy(pBits, 0, zero, (int) bmpheader.info.bmiHeader.biSizeImage);
            pBits = null;
            IntPtr ptr7 = SelectObject(hdc, hObj);
            Graphics graphics = Graphics.FromImage(this.m_ImageDC);
            IntPtr hdcDest = graphics.GetHdc();
            BitBlt(hdcDest, 0, 0, bmpheader.info.bmiHeader.biWidth, bmpheader.info.bmiHeader.biHeight, hdc, 0, 0, 0xcc0020);
            DeleteObject(SelectObject(hdc, ptr7));
            DeleteDC(hdc);
            graphics.ReleaseHdc(hdcDest);
            LocalFree(ptr);
            this.m_isEmpty = false;
            if (this.m_bmp != null)
            {
                this.m_bmp.Dispose();
                this.m_bmp = null;
            }
            this.m_bmp = image;
            this.Refresh();
        }

        public void Load(string filename, BitmapType type, bool setSize)
        {
            FileStream stream = new FileStream(filename, FileMode.Open);
            this.Load(stream, type, setSize);
            stream.Close();
        }

        [DllImport("coredll.dll")]
        private static extern IntPtr LocalAlloc(uint flags, uint cb);
        [DllImport("coredll.dll")]
        private static extern IntPtr LocalFree(IntPtr hMem);
        private void m_gradientBackColor_PropertyChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void OnContentsChanged(ContentsChangedType aContentsChangedType)
        {
            if (this.ContentsChanged != null)
            {
                ContentsChangedEventArgs args = new ContentsChangedEventArgs(aContentsChangedType);
                this.ContentsChanged(this, args);
            }
        }

        private void OnControlSizeChanged()
        {
            if (!this.m_ImageSizeCustom)
            {
                this.m_ImageSize = this.Size;
            }
        }

        private void OnImageSizeChanged()
        {
            if (this.m_ImageSize.Height < this.Height)
            {
                this.Height = this.m_ImageSize.Height;
            }
            if (this.m_ImageSize.Width < this.Width)
            {
                this.Width = this.m_ImageSize.Width;
            }
            this.AdjustScrollbars();
            base.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.m_Drawing = true;
            this.m_lastPositon.X = e.X;
            if (this.hScrollBar.Visible)
            {
                this.m_lastPositon.X += this.hScrollBar.Value;
            }
            this.m_lastPositon.Y = e.Y;
            if (this.vScrollBar.Visible)
            {
                this.m_lastPositon.Y += this.vScrollBar.Value;
            }
            this.m_currentLine.Clear();
            this.m_currentLine.Add(this.m_lastPositon);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.m_Drawing)
            {
                int x = e.X;
                if (this.hScrollBar.Visible)
                {
                    x += this.hScrollBar.Value;
                }
                int y = e.Y;
                if (this.vScrollBar.Visible)
                {
                    y += this.vScrollBar.Value;
                }
                this._DrawLine(this.m_lastPositon.X, this.m_lastPositon.Y, x, y);
                this.m_lastPositon.X = x;
                this.m_lastPositon.Y = y;
                this.m_currentLine.Add(this.m_lastPositon);
            }
            base.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.m_Drawing)
            {
                int x = e.X;
                if (this.hScrollBar.Visible)
                {
                    x += this.hScrollBar.Value;
                }
                int y = e.Y;
                if (this.vScrollBar.Visible)
                {
                    y += this.vScrollBar.Value;
                }
                if ((this.m_lastPositon.X == x) && (this.m_lastPositon.Y == y))
                {
                    if (((x >= 0) && (y >= 0)) && ((x < this.m_Image.Width) && (y < this.m_Image.Height)))
                    {
                        this._DrawLine(x, y, x + 1, y + 1);
                    }
                    else
                    {
                        this._DrawLine(this.m_lastPositon.X, this.m_lastPositon.Y, x, y);
                    }
                }
                this.m_currentLine.Add(new Point(x, y));
                List<Point> list = new List<Point>(this.m_currentLine);
                this.m_Segments.Add(list);
                this.m_RedoSegments.Clear();
                this.OnContentsChanged(ContentsChangedType.ByMouse);
            }
            this.m_Drawing = false;
            base.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.CreateGdiObjects();
            this.CreateMemoryBitmap();
            this.CreatePenObjects();
            Rectangle clipRectangle = e.ClipRectangle;
            if (this.m_QuickDraw)
            {
                e.Graphics.Clip = new Region(clipRectangle);
            }
            this.Draw(e.Graphics);
            if (this.m_QuickDraw)
            {
                e.Graphics.ResetClip();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.AdjustScrollbars();
            this.Refresh();
        }

        [DllImport("coredll.dll")]
        private static extern int Polyline(IntPtr hdc, ref LPPOINT2 lppt, int cPoints);
        public void Redo()
        {
            if (this.CanRedo())
            {
                List<Point> list = this.m_RedoSegments[this.m_RedoSegments.Count - 1];
                this.m_Segments.Add(list);
                this.m_RedoSegments.RemoveAt(this.m_RedoSegments.Count - 1);
                this.Refresh();
                this.OnContentsChanged(ContentsChangedType.ByRedo);
            }
        }

        public override void Refresh()
        {
            if (this.IsEmpty)
            {
                this.CreateGdiObjects();
                this.CreateMemoryBitmap();
                this.CreatePenObjects();
            }
            this.RepaintLines();
            base.Refresh();
        }

        private void RepaintLines()
        {
            if (this.m_Image != null)
            {
                if (!this.m_useGradient)
                {
                    this.m_graphics.Clear(this.BackColor);
                }
                else
                {
                    Rectangle rc = new Rectangle(0, 0, this.ImageSize.Width, this.ImageSize.Height);
                    this.m_gradientBackColor.DrawGradient(this.m_graphics, rc);
                }
                if (this.m_bmpBackground != null)
                {
                    Rectangle srcRect = new Rectangle(0, 0, this.m_bmpBackground.Width, this.m_bmpBackground.Height);
                    Rectangle rectangle3 = new Rectangle(0, 0, this.ImageSize.Width, this.ImageSize.Height);
                    this.m_graphics.DrawImage(this.m_bmpBackground, rectangle3, srcRect, GraphicsUnit.Pixel);
                }
                ImageAttributes imageAttr = new ImageAttributes();
                imageAttr.SetColorKey(Color.FromArgb(0xff, 0, 0xff), Color.FromArgb(0xff, 0, 0xff));
                Rectangle destRect = new Rectangle(0, 0, this.ImageSize.Width, this.ImageSize.Height);
                this.m_graphics.DrawImage(this.m_bmp, destRect, 0, 0, this.ImageSize.Width, this.ImageSize.Height, GraphicsUnit.Pixel, imageAttr);
                if (this.m_ImageDC != null)
                {
                    Bitmap image = new Bitmap(this.ImageSize.Width, this.ImageSize.Height);
                    Graphics graphics = Graphics.FromImage(image);
                    imageAttr.SetColorKey(this.ForeColor, this.ForeColor);
                    graphics.Clear(Color.White);
                    Rectangle rectangle5 = new Rectangle(0, 0, this.ImageSize.Width, this.ImageSize.Height);
                    graphics.DrawImage(this.m_bmp, rectangle5, 0, 0, this.ImageSize.Width, this.ImageSize.Height, GraphicsUnit.Pixel, imageAttr);
                    Graphics graphics2 = Graphics.FromImage(this.m_ImageDC);
                    graphics2.Clear(Color.Black);
                    imageAttr.SetColorKey(Color.FromArgb(0xff00ff), Color.FromArgb(0xff00ff));
                    Rectangle rectangle6 = new Rectangle(0, 0, this.ImageSize.Width, this.ImageSize.Height);
                    graphics2.DrawImage(image, rectangle6, 0, 0, this.ImageSize.Width, this.ImageSize.Height, GraphicsUnit.Pixel, imageAttr);
                    if (graphics != null)
                    {
                        graphics.Dispose();
                        graphics = null;
                    }
                    if (image != null)
                    {
                        image.Dispose();
                        image = null;
                    }
                }
                for (int i = 0; i < this.m_Segments.Count; i++)
                {
                    List<Point> list = this.m_Segments[i];
                    for (int j = 0; j < (list.Count - 1); j++)
                    {
                        if ((list[j].X == list[j+1].X) && (list[j].Y == list[j+1].Y))
                        {
                            if (((list[j].X >= 0) && (list[j].X < this.ImageSize.Width)) && ((list[j].Y >= 0) && (list[j].Y < this.ImageSize.Height)))
                            {
                                this._DrawLine(list[j].X, list[j].Y, list[j].X + 1, list[j].Y + 1);
                            }
                        }
                        else
                        {
                            this._DrawLine(list[j].X, list[j].Y, list[j + 1].X, list[j+1].Y);
                        }
                    }
                }
            }
        }

        public void Resize(ZoomPercent aZoom)
        {
            MemoryStream stream = new MemoryStream();
            this.Save(stream, BitmapType.Bmp1, false, aZoom);
            stream.Seek(0L, SeekOrigin.Begin);
            this.Load(stream, BitmapType.Bmp1, true);
            stream.Close();
        }

        public void Save(string filename, BitmapType type)
        {
            FileStream stream = new FileStream(filename, FileMode.Create);
            this.Save(stream, type, false);
            stream.Close();
        }

        public void Save(Stream stream, BitmapType type, bool withBorder)
        {
            this.Save(stream, type, withBorder, this.ImageSize.Width, this.ImageSize.Height, this.m_ImageDC);
        }

        public void Save(string filename, BitmapType type, bool withBorder)
        {
            FileStream stream = new FileStream(filename, FileMode.Create);
            this.Save(stream, type, withBorder);
            stream.Close();
        }

        public void Save(Stream stream, BitmapType type, bool withBorder, ZoomPercent aZoom)
        {
            Bitmap thumbnailBmp = GetThumbnail.GetThumbnailBmp(this.m_ImageDC, aZoom);
            this.Save(stream, type, withBorder, thumbnailBmp.Width, thumbnailBmp.Height, thumbnailBmp);
        }

        private void Save(Stream stream, BitmapType type, bool withBorder, int aWidth, int aHeight, Bitmap aSrcImage)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            int width = 0;
            int height = aHeight;
            if ((aWidth % 8) == 0)
            {
                width = aWidth / 8;
            }
            else
            {
                width = (aWidth / 8) + 1;
            }
            if ((width % 4) != 0)
            {
                width += 4 - (width % 4);
            }
            BMPHEADER bmpheader = new BMPHEADER();
            bmpheader.header.bfSize = bmpheader.header.bfOffBits + ((uint) (width * height));
            bmpheader.info.bmiHeader.biWidth = aWidth;
            bmpheader.info.bmiHeader.biHeight = height;
            bmpheader.info.bmiHeader.biSizeImage = (uint) (width * height);
            bmpheader.info.bmiColor1 = new RGBQUAD(this.BackColor.B, this.BackColor.G, this.BackColor.R);
            bmpheader.info.bmiColor2 = new RGBQUAD(this.ForeColor.B, this.ForeColor.G, this.ForeColor.R);
            switch (type)
            {
                case BitmapType.Bmp1:
                    bmpheader.Write(writer);
                    break;

                case BitmapType.Binary:
                case BitmapType.BinaryStrict:
                {
                    BINARYHEADER binaryheader = new BINARYHEADER();
                    binaryheader.iWidth = aWidth;
                    binaryheader.iHeight = height;
                    binaryheader.cBackground = new RGBQUAD(this.BackColor.B, this.BackColor.G, this.BackColor.R);
                    binaryheader.cForeground = new RGBQUAD(this.ForeColor.B, this.ForeColor.G, this.ForeColor.R);
                    binaryheader.Write(writer);
                    break;
                }
            }
            Graphics graphics = Graphics.FromImage(aSrcImage);
            IntPtr hdc = graphics.GetHdc();
            IntPtr ptr2 = CreateCompatibleDC(hdc);
            SelectObject(ptr2, aSrcImage.GetHbitmap());
            IntPtr ptr3 = CreateCompatibleDC(hdc);
            graphics.ReleaseHdc(hdc);
            IntPtr zero = IntPtr.Zero;
            IntPtr ptr = LocalAlloc(0x40, (uint) (bmpheader.info.bmiHeader.biSize + 8));
            Marshal.StructureToPtr(bmpheader.info.bmiHeader, ptr, false);
            new RGBQUAD(0, 0, 0).WriteToPtr(ptr, bmpheader.info.bmiHeader.biSize);
            new RGBQUAD(0xff, 0xff, 0xff).WriteToPtr(ptr, bmpheader.info.bmiHeader.biSize + 4);
            IntPtr hObj = CreateDIBSection(ptr2, ptr, 0, ref zero, IntPtr.Zero, 0);
            IntPtr ptr7 = SelectObject(ptr3, hObj);
            BitBlt(ptr3, 0, 0, bmpheader.info.bmiHeader.biWidth, bmpheader.info.bmiHeader.biHeight, ptr2, 0, 0, 0xcc0020);
            if (withBorder)
            {
                IntPtr ptr8 = CreatePen(0, this.m_IsQvga ? 1 : 2, 0xffffff);
                if ((ptr3 != IntPtr.Zero) && (ptr8 != IntPtr.Zero))
                {
                    SelectObject(ptr3, ptr8);
                }
                this.LineTo(ptr3, 0, 0, aWidth - 1, 0);
                this.LineTo(ptr3, aWidth - 1, 0, aWidth - 1, aHeight - 1);
                this.LineTo(ptr3, aWidth - 1, aHeight - 1, 0, aHeight - 1);
                this.LineTo(ptr3, 0, aHeight - 1, 0, 0);
                if (ptr8 != IntPtr.Zero)
                {
                    DeleteObject(ptr8);
                }
            }
            byte[] destination = new byte[bmpheader.info.bmiHeader.biSizeImage];
            Marshal.Copy(zero, destination, 0, (int) bmpheader.info.bmiHeader.biSizeImage);
            if (type == BitmapType.BinaryStrict)
            {
                this.SwapLines(destination, width, height);
            }
            writer.Write(destination);
            DeleteObject(SelectObject(ptr3, ptr7));
            DeleteDC(ptr3);
            DeleteDC(ptr2);
            LocalFree(ptr);
        }

        [DllImport("coredll.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObj);
        protected virtual bool ShouldSerializeGradientBackColor()
        {
            return (((this.m_gradientBackColor.StartColor.ToArgb() != Color.White.ToArgb()) | (this.m_gradientBackColor.EndColor.ToArgb() != Color.White.ToArgb())) | (this.m_gradientBackColor.FillDirection != FillDirection.Vertical));
        }

        private void SwapLines(byte[] pBits, int width, int height)
        {
            byte[] sourceArray = pBits.Clone() as byte[];
            if (sourceArray != null)
            {
                for (int i = 0; i < height; i++)
                {
                    Array.Copy(sourceArray, i * width, pBits, ((height - i) - 1) * width, width);
                }
            }
            sourceArray = null;
        }

        public void Undo()
        {
            if (this.CanUndo())
            {
                List<Point> list = this.m_Segments[this.m_Segments.Count - 1];
                this.m_RedoSegments.Add(list);
                this.m_Segments.RemoveAt(this.m_Segments.Count - 1);
                this.Refresh();
                this.OnContentsChanged(ContentsChangedType.ByUndo);
            }
        }

        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        public System.Drawing.Image BackgroundImage
        {
            get
            {
                return this.m_bmpBackground;
            }
            set
            {
                if (this.m_bmpBackground != value)
                {
                    this.m_bmpBackground = (Bitmap) value;
                    this.Refresh();
                }
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        public System.Drawing.Image GetBackgrndImg
        {
            get
            {
                return this.m_bmpBackground;
            }
        }

        public System.Drawing.Image GetForegrndImg
        {
            get
            {
                return this.m_bmp;
            }
        }

        public System.Drawing.Image GetImg
        {
            get
            {
                return this.m_Image;
            }
        }

        public System.Drawing.Image GetImgDC
        {
            get
            {
                return this.m_ImageDC;
            }
        }

        public GradientColor GradientBackColor
        {
            get
            {
                return this.m_gradientBackColor;
            }
            set
            {
                if (this.m_gradientBackColor != value)
                {
                    this.m_gradientBackColor.PropertyChanged -= new EventHandler(this.m_gradientBackColor_PropertyChanged);
                    this.m_gradientBackColor = value;
                    this.m_gradientBackColor.PropertyChanged += new EventHandler(this.m_gradientBackColor_PropertyChanged);
                }
                this.Refresh();
            }
        }

        public int Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
                this.OnControlSizeChanged();
            }
        }

        public Bitmap Image
        {
            get
            {
                return this.m_Image;
            }
            set
            {
                this.m_Image = value;
            }
        }

        public System.Drawing.Size ImageSize
        {
            get
            {
                if (this.m_ImageSizeCustom)
                {
                    return this.m_ImageSize;
                }
                return this.Size;
            }
            set
            {
                this.m_ImageSize = value;
                this.OnImageSizeChanged();
            }
        }

        public bool ImageSizeCustom
        {
            get
            {
                return this.m_ImageSizeCustom;
            }
            set
            {
                this.m_ImageSizeCustom = value;
                this.OnControlSizeChanged();
            }
        }

        public Bitmap ImageWithoutBackgrnd
        {
            get
            {
                return this.m_ImageDC;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this.m_isEmpty && (this.m_Segments.Count == 0));
            }
        }

        public IList Lines
        {
            get
            {
                return this.m_Segments;
            }
        }

        public int PenWidth
        {
            get
            {
                return this.m_penWidth;
            }
            set
            {
                if (this.m_penWidth != value)
                {
                    this.m_penWidth = value;
                    this.Refresh();
                }
            }
        }

        public int ScrollBarHeight
        {
            get
            {
                return this.hScrollBar.Height;
            }
            set
            {
                this.hScrollBar.Height = value;
            }
        }

        public int ScrollBarWidth
        {
            get
            {
                return this.vScrollBar.Width;
            }
            set
            {
                this.vScrollBar.Width = value;
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
                this.OnControlSizeChanged();
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        public bool UseGradient
        {
            get
            {
                return this.m_useGradient;
            }
            set
            {
                if (this.m_useGradient != value)
                {
                    this.m_useGradient = value;
                    this.Refresh();
                }
            }
        }

        public int Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
                this.OnControlSizeChanged();
            }
        }

        private class BINARYHEADER
        {
            public InkBox.RGBQUAD cBackground = new InkBox.RGBQUAD(0xff, 0xff, 0xff);
            public InkBox.RGBQUAD cForeground = new InkBox.RGBQUAD(0xff, 0xff, 0xff);
            public int iHeight;
            public int iWidth;

            public void Read(BinaryReader stream)
            {
                this.iWidth = stream.ReadInt32();
                this.iHeight = stream.ReadInt32();
                this.cBackground.Read(stream);
                this.cForeground.Read(stream);
            }

            public void Write(BinaryWriter stream)
            {
                stream.Write(this.iWidth);
                stream.Write(this.iHeight);
                this.cBackground.Write(stream);
                this.cForeground.Write(stream);
            }
        }

        private class BITMAPFILEHEADER
        {
            public uint bfOffBits = 0x3e;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public uint bfSize;
            public ushort bfType = 0x4d42;

            public void Read(BinaryReader stream)
            {
                this.bfType = stream.ReadUInt16();
                this.bfSize = stream.ReadUInt32();
                this.bfReserved1 = stream.ReadUInt16();
                this.bfReserved2 = stream.ReadUInt16();
                this.bfOffBits = stream.ReadUInt32();
            }

            public void Write(BinaryWriter stream)
            {
                stream.Write(this.bfType);
                stream.Write(this.bfSize);
                stream.Write(this.bfReserved1);
                stream.Write(this.bfReserved2);
                stream.Write(this.bfOffBits);
            }
        }

        private class BITMAPINFO
        {
            public InkBox.RGBQUAD bmiColor1 = new InkBox.RGBQUAD(0xff, 0xff, 0xff);
            public InkBox.RGBQUAD bmiColor2 = new InkBox.RGBQUAD(0, 0, 0);
            public InkBox.BITMAPINFOHEADER bmiHeader = new InkBox.BITMAPINFOHEADER();

            public void Read(BinaryReader stream)
            {
                this.bmiHeader.Read(stream);
                this.bmiColor1.Read(stream);
                this.bmiColor2.Read(stream);
            }

            public void Write(BinaryWriter stream)
            {
                this.bmiHeader.Write(stream);
                this.bmiColor1.Write(stream);
                this.bmiColor2.Write(stream);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private class BITMAPINFOHEADER
        {
            public int biSize = 40;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes = 1;
            public ushort biBitCount = 1;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter = 0xec4;
            public int biYPelsPerMeter = 0xec4;
            public uint biClrUsed = 2;
            public uint biClrImportant;
            public void Write(BinaryWriter stream)
            {
                stream.Write(this.biSize);
                stream.Write(this.biWidth);
                stream.Write(this.biHeight);
                stream.Write(this.biPlanes);
                stream.Write(this.biBitCount);
                stream.Write(this.biCompression);
                stream.Write(this.biSizeImage);
                stream.Write(this.biXPelsPerMeter);
                stream.Write(this.biYPelsPerMeter);
                stream.Write(this.biClrUsed);
                stream.Write(this.biClrImportant);
            }

            public void Read(BinaryReader stream)
            {
                this.biSize = stream.ReadInt32();
                this.biWidth = stream.ReadInt32();
                this.biHeight = stream.ReadInt32();
                this.biPlanes = stream.ReadUInt16();
                this.biBitCount = stream.ReadUInt16();
                this.biCompression = stream.ReadUInt32();
                this.biSizeImage = stream.ReadUInt32();
                this.biXPelsPerMeter = stream.ReadInt32();
                this.biYPelsPerMeter = stream.ReadInt32();
                this.biClrUsed = stream.ReadUInt32();
                this.biClrImportant = stream.ReadUInt32();
            }
        }

        public enum BitmapType
        {
            Bmp1,
            Binary,
            BinaryStrict
        }

        private class BMPHEADER
        {
            public InkBox.BITMAPFILEHEADER header = new InkBox.BITMAPFILEHEADER();
            public InkBox.BITMAPINFO info = new InkBox.BITMAPINFO();

            public void Read(BinaryReader stream)
            {
                this.header.Read(stream);
                this.info.Read(stream);
            }

            public void Write(BinaryWriter stream)
            {
                this.header.Write(stream);
                this.info.Write(stream);
            }
        }

        public delegate void ContentsChangedEventHandler(object sender, ContentsChangedEventArgs args);

        [StructLayout(LayoutKind.Sequential)]
        private struct LPPOINT2
        {
            public int x1;
            public int y1;
            public int x2;
            public int y2;
        }

        private class RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;

            public RGBQUAD(byte b, byte g, byte r)
            {
                this.rgbBlue = b;
                this.rgbGreen = g;
                this.rgbRed = r;
                this.rgbReserved = 0xff;
            }

            public void Read(BinaryReader stream)
            {
                this.rgbBlue = stream.ReadByte();
                this.rgbGreen = stream.ReadByte();
                this.rgbRed = stream.ReadByte();
                this.rgbReserved = stream.ReadByte();
            }

            public byte[] ToArray()
            {
                return new byte[] { this.rgbBlue, this.rgbGreen, this.rgbRed, this.rgbReserved };
            }

            public void Write(BinaryWriter stream)
            {
                stream.Write(this.rgbBlue);
                stream.Write(this.rgbGreen);
                stream.Write(this.rgbRed);
                stream.Write(this.rgbReserved);
            }

            public void WriteToPtr(IntPtr ptr, int ofs)
            {
                Marshal.WriteByte(ptr, ofs, this.rgbBlue);
                Marshal.WriteByte(ptr, ofs + 1, this.rgbGreen);
                Marshal.WriteByte(ptr, ofs + 2, this.rgbRed);
                Marshal.WriteByte(ptr, ofs + 3, this.rgbReserved);
            }
        }
    }
}

