using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using Microsoft.WindowsMobile.Forms;
using System.Drawing;

namespace BelugaMobile.BaseControl
{
    public delegate void closeHandle(object sender, CancelEventArgs e);
    public delegate void SelectPicHandle(ArrayList array, string File);
    public class SelectPictureDialog : BaseForm// Component
    {

        Bitmap background;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (background != null)
                e.Graphics.DrawImage(background, 0, 0);
            else
                base.OnPaint(e);
        }

        public Bitmap BackgroundImage
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
            }
        }

        // Fields
        private const string DefaultFilter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
        private const string DefaultTitle = "Select Picture";
        private string file;
        private string filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
        private int filterIndex = 1;
        private string initialDir;
        private const int MAX_PATH = 260;
        private int maxFilterIndex = 1;
        private OpenFileNameEx ofnex;
        private Control owner;
        private SortOrder sortOrder = SortOrder.DateDescending;
        private string title = "选择图片";

        public event closeHandle SelectClose;
        public event SelectPicHandle selectPic;

        // Methods
        public SelectPictureDialog()
        {
            this.ofnex.lStructSize = (uint)Marshal.SizeOf(this.ofnex);
            this.ofnex.maxFile = 260;
            this.ofnex.ExFlags = 2;
        }

        [DllImport("coredll.dll")]
        private static extern IntPtr GetFocus();
        [DllImport("aygshell.dll")]
        private static extern int GetOpenFileNameEx(ref OpenFileNameEx ofnex);
        [DllImport("coredll.dll")]
        private static extern IntPtr LocalAlloc(uint flags, uint bytes);
        [DllImport("coredll.dll")]
        private static extern IntPtr LocalFree(IntPtr mem);
        [DllImport("coredll.dll")]
        private static extern IntPtr SetFocus(IntPtr hWnd);
        [DllImport("coredll.dll")]
        private static extern int SHGetSpecialFolderPath(IntPtr hwndOwner, StringBuilder path, int folder, int fCreate);
        public DialogResult ShowDialog()
        {
            DialogResult cancel = DialogResult.Cancel;
            this.file = "";
            if (this.owner != null)
            {
                IntPtr focus = GetFocus();
                this.owner.Focus();
                this.ofnex.hwndOwner = GetFocus();
                SetFocus(focus);
            }
            try
            {
                if (this.title != "Select Picture")
                {
                    this.ofnex.pszTitle = ToUnmanagedStr(this.title);
                }
                if (this.initialDir != "")
                {
                    this.ofnex.pszInitialDir = ToUnmanagedStr(this.initialDir);
                }
                this.ofnex.pszFile = ToUnmanagedStr(this.file, (int)this.ofnex.maxFile);
                if ((this.filter != null) && (this.filter != ""))
                {
                    StringBuilder builder = new StringBuilder(this.filter);
                    builder.Replace('|', '\0');
                    builder.Append('\0');
                    this.ofnex.pszFilter = ToUnmanagedStr(builder.ToString());
                }
                if (this.filterIndex < 0)
                {
                    this.ofnex.nFilterIndex = 0;
                }
                else
                {
                    this.ofnex.nFilterIndex = (this.filterIndex > this.maxFilterIndex) ? ((uint)this.maxFilterIndex) : ((uint)this.filterIndex);
                }
                this.ofnex.dwSortOrder = (uint)(((((int)this.SortOrder) >> 1) + 1) | (((int)~((int)this.SortOrder & 1)) << 31));
                if (GetOpenFileNameEx(ref this.ofnex) == 0)
                {
                    return cancel;
                }
                char[] destination = new char[this.ofnex.maxFile];
                Marshal.Copy(this.ofnex.pszFile, destination, 0, (int)this.ofnex.maxFile);
                this.file = new string(destination);
                int index = this.file.IndexOf('\0');
                if (index != -1)
                {
                    this.file = this.file.Substring(0, index);
                }
                cancel = DialogResult.OK;
            }
            finally
            {
                LocalFree(this.ofnex.pszFilter);
                this.ofnex.pszFilter = IntPtr.Zero;
                LocalFree(this.ofnex.pszFile);
                this.ofnex.pszFile = IntPtr.Zero;
                LocalFree(this.ofnex.pszInitialDir);
                this.ofnex.pszInitialDir = IntPtr.Zero;
                LocalFree(this.ofnex.pszTitle);
                this.ofnex.pszTitle = IntPtr.Zero;
            }
            return cancel;
        }


        public void Show()
        {
            this.file = "";
            if (this.owner != null)
            {
                IntPtr focus = GetFocus();
                this.owner.Focus();
                this.ofnex.hwndOwner = GetFocus();
                SetFocus(focus);
            }
            try
            {
                if (this.title != "Select Picture")
                {
                    this.ofnex.pszTitle = ToUnmanagedStr(this.title);
                }
                if (this.initialDir != "")
                {
                    this.ofnex.pszInitialDir = ToUnmanagedStr(this.initialDir);
                }
                this.ofnex.pszFile = ToUnmanagedStr(this.file, (int)this.ofnex.maxFile);
                if ((this.filter != null) && (this.filter != ""))
                {
                    StringBuilder builder = new StringBuilder(this.filter);
                    builder.Replace('|', '\0');
                    builder.Append('\0');
                    this.ofnex.pszFilter = ToUnmanagedStr(builder.ToString());
                }
                if (this.filterIndex < 0)
                {
                    this.ofnex.nFilterIndex = 0;
                }
                else
                {
                    this.ofnex.nFilterIndex = (this.filterIndex > this.maxFilterIndex) ? ((uint)this.maxFilterIndex) : ((uint)this.filterIndex);
                }
                this.ofnex.dwSortOrder = (uint)(((((int)this.SortOrder) >> 1) + 1) | (((int)~((int)this.SortOrder & 1)) << 31));
                if (GetOpenFileNameEx(ref this.ofnex) == 0)
                {
                    if (SelectClose != null)
                    {
                        SelectClose(this, new CancelEventArgs());
                    }
                    return;
                }
                char[] destination = new char[this.ofnex.maxFile];
                Marshal.Copy(this.ofnex.pszFile, destination, 0, (int)this.ofnex.maxFile);
                this.file = new string(destination);
                int index = this.file.IndexOf('\0');
                if (index != -1)
                {
                    this.file = this.file.Substring(0, index);
                    int indexk = file.LastIndexOf('\\');
                    if (indexk != -1)
                    {
                        this.initialDir = this.file.Substring(0, indexk);
                    }
                    if (!tempFiles.Contains(file))
                        tempFiles.Add(file);
                    else
                    {
                        MessageBox.Show("该图片您已添加!");
                    }


                    if (selectPic != null)
                    {
                        // this.Closed();
                        selectPic(tempFiles, file);
                    }
                }
                // Show();
            }
            catch { }
        }

        public ArrayList tempFiles = new ArrayList();

        public void Closed()
        {
            LocalFree(this.ofnex.pszFilter);
            this.ofnex.pszFilter = IntPtr.Zero;
            LocalFree(this.ofnex.pszFile);
            this.ofnex.pszFile = IntPtr.Zero;
            LocalFree(this.ofnex.pszInitialDir);
            this.ofnex.pszInitialDir = IntPtr.Zero;
            LocalFree(this.ofnex.pszTitle);
            this.ofnex.pszTitle = IntPtr.Zero;

        }

        private static IntPtr ToUnmanagedStr(string str)
        {
            if (str == null)
            {
                return IntPtr.Zero;
            }
            return ToUnmanagedStr(str, str.Length + 1);
        }

        private static IntPtr ToUnmanagedStr(string str, int cchSize)
        {
            if ((str.Length + 1) > cchSize)
            {
                throw new ArgumentOutOfRangeException("String too long for buffer");
            }
            IntPtr destination = LocalAlloc(0, (uint)(cchSize * Marshal.SystemDefaultCharSize));
            if (destination == IntPtr.Zero)
            {
                throw new OutOfMemoryException("Failed to allocate unmanaged buffer");
            }
            char[] source = (str + '\0').ToCharArray();
            Marshal.Copy(source, 0, destination, source.Length);
            return destination;
        }

        // Properties
        public bool CameraAccess
        {
            get
            {
                return ((this.ofnex.ExFlags & 512) == 0);
            }
            set
            {
                if (value)
                {
                    this.ofnex.flags &= 4294966783;
                }
                else
                {
                    this.ofnex.flags |= 512;
                }
            }
        }

        public string FileName
        {
            get
            {
                return this.file;
            }
        }

        public string Filter
        {
            get
            {
                return this.filter;
            }
            set
            {
                int num = -1;
                if (value != null)
                {
                    int index = -1;
                    do
                    {
                        index = value.IndexOf("|", (int)(index + 1));
                        num++;
                    }
                    while (index != -1);
                    if ((num > 0) && ((num & 1) == 0))
                    {
                        throw new ArgumentException("Invalid filter");
                    }
                }
                this.maxFilterIndex = (num + 1) >> 1;
                this.filter = value;
            }
        }

        public int FilterIndex
        {
            get
            {
                return this.filterIndex;
            }
            set
            {
                this.filterIndex = value;
            }
        }

        public string InitialDirectory
        {
            get
            {
                return this.initialDir;
            }
            set
            {
                this.initialDir = value;
            }
        }

        public bool LockDirectory
        {
            get
            {
                return ((this.ofnex.ExFlags & 256) != 0);
            }
            set
            {
                if (value)
                {
                    this.ofnex.ExFlags |= 256;
                }
                else
                {
                    this.ofnex.ExFlags &= 4294967039;
                }
            }
        }

        public Control Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                this.owner = value;
            }
        }

        public bool ShowDrmContent
        {
            get
            {
                return (0 == (this.ofnex.ExFlags & 65536));
            }
            set
            {
                if (value)
                {
                    this.ofnex.ExFlags &= 4294901759;
                }
                else
                {
                    this.ofnex.ExFlags |= 65536;
                }
            }
        }

        public bool ShowForwardLockedContent
        {
            get
            {
                return (0 == (this.ofnex.ExFlags & 131072));
            }
            set
            {
                if (value)
                {
                    this.ofnex.ExFlags &= 4294836223;
                }
                else
                {
                    this.ofnex.ExFlags |= 131072;
                }
            }
        }

        public SortOrder SortOrder
        {
            get
            {
                return this.sortOrder;
            }
            set
            {
                if (!Enum.IsDefined(typeof(SortOrder), value))
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.sortOrder = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        // Nested Types
        private enum CSIDL
        {
            PERSONAL = 5
        }

        [Flags]
        private enum ExFlags : uint
        {
            DetailsView = 1,
            HideDRMForwardLocked = 131072,
            HideDRMProtected = 65536,
            LockDirectory = 256,
            NoFileCreate = 512,
            ThumbnailView = 2
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct OpenFileNameEx
        {
            public uint lStructSize;
            public IntPtr hwndOwner;
            public IntPtr hInstance;
            public IntPtr pszFilter;
            public IntPtr lpstrCustomFilter;
            public uint nMaxCustFilter;
            public uint nFilterIndex;
            public IntPtr pszFile;
            public uint maxFile;
            public IntPtr lpstrFileTitle;
            public uint nMaxFileTitle;
            public IntPtr pszInitialDir;
            public IntPtr pszTitle;
            public uint flags;
            public ushort nFileOffset;
            public ushort nFileExtension;
            public IntPtr lpstrDefExt;
            public uint lCustData;
            public IntPtr lpfnHook;
            public IntPtr lpTemplateName;
            public uint dwSortOrder;
            public uint ExFlags;
        }
    }
}
