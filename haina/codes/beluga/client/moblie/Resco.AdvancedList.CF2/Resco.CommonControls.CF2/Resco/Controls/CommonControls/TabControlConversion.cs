namespace Resco.Controls.CommonControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    internal class TabControlConversion
    {
        public Resco.Controls.CommonControls.TabControl.DesignTimeCallback m_DesignTimeCallback;
        private Hashtable m_htImageLists;
        private ISite m_site;

        public TabControlConversion(Resco.Controls.CommonControls.TabControl.DesignTimeCallback _designTimeCallback)
        {
            this.m_htImageLists = new Hashtable();
            this.m_DesignTimeCallback = _designTimeCallback;
        }

        public TabControlConversion(ISite site, Resco.Controls.CommonControls.TabControl.DesignTimeCallback _designTimeCallback)
        {
            this.m_htImageLists = new Hashtable();
            this.m_site = site;
            this.m_DesignTimeCallback = _designTimeCallback;
        }

        public static Color ColorFromString(string sColor)
        {
            PropertyInfo property = typeof(Color).GetProperty(sColor);
            if (property != null)
            {
                return (Color) property.GetValue(null, null);
            }
            property = typeof(SystemColors).GetProperty(sColor);
            if (property != null)
            {
                return (Color) property.GetValue(null, null);
            }
            return Color.FromArgb(int.Parse(sColor, NumberStyles.HexNumber));
        }

        public static DateTime DateTimeFromString(string sData)
        {
            return Convert.ToDateTime(sData, CultureInfo.InvariantCulture);
        }

        public static Enum EnumFromString(Type enumType, string sValue)
        {
            return (Enum) enumType.GetField(sValue.Trim()).GetValue(null);
        }

        public Font FontFromString(string sFont)
        {
            sFont = sFont.Trim();
            int length = sFont.Length;
            int index = sFont.IndexOf(',');
            if (index < 0)
            {
                index = length;
            }
            int num3 = (index < (length - 1)) ? sFont.IndexOf(',', index + 1) : -1;
            if (num3 < 0)
            {
                num3 = length;
            }
            string familyName = sFont.Substring(0, index);
            string str2 = (index < length) ? sFont.Substring(index + 1, num3 - (index + 1)).Trim() : "8";
            string str3 = (num3 < length) ? sFont.Substring(num3 + 1, length - (num3 + 1)).Trim() : "";
            int num4 = str2.Length;
            int num5 = 0;
            while (num5 < num4)
            {
                if (char.IsLetter(str2[num5]))
                {
                    break;
                }
                num5++;
            }
            float emSize = Convert.ToSingle(str2.Substring(0, num5).Trim());
            FontStyle regular = FontStyle.Regular;
            if ((str3.Length > 0) && str3.StartsWith("style"))
            {
                num5 = str3.IndexOf('=', 5);
                if (num5 > 0)
                {
                    foreach (string str4 in str3.Substring(num5 + 1).Split(new char[] { ',' }))
                    {
                        regular |= (FontStyle) EnumFromString(typeof(FontStyle), str4);
                    }
                }
            }
            Font o = new Font(familyName, emSize, regular);
            if (this.m_DesignTimeCallback != null)
            {
                this.m_DesignTimeCallback(o, null);
            }
            return o;
        }

        public ImageList GetImageList(string sName)
        {
            if ((sName == null) || (sName == ""))
            {
                return null;
            }
            if (this.m_htImageLists.Contains(sName))
            {
                return (ImageList) this.m_htImageLists[sName];
            }
            if (this.m_site != null)
            {
                Type serviceType = Type.GetType("System.ComponentModel.Design.IReferenceService, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                object service = this.m_site.GetService(serviceType);
                if (service != null)
                {
                    object obj3 = serviceType.GetMethod("GetReference", new Type[] { typeof(string) }).Invoke(service, new object[] { sName });
                    if (obj3 != null)
                    {
                        if (obj3 is ImageList)
                        {
                            this.m_htImageLists.Add(sName, (ImageList) obj3);
                            return (obj3 as ImageList);
                        }
                        return null;
                    }
                }
            }
            ImageList list2 = new ImageList();
            this.m_htImageLists.Add(sName, list2);
            if (this.m_site != null)
            {
                IContainer container = (IContainer) this.m_site.GetService(typeof(IContainer));
                if (container != null)
                {
                    container.Add(list2, sName);
                }
            }
            return list2;
        }

        public static Bitmap ImageFromString(string sImage)
        {
            Bitmap bitmap = null;
            try
            {
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(sImage));
                bitmap = new Bitmap(stream);
            }
            catch
            {
            }
            return bitmap;
        }

        public static int Int32FromString(string sInt)
        {
            return Convert.ToInt32(sInt);
        }

        public static Point PointFromString(string sRect)
        {
            string[] strArray = sRect.Split(new char[] { ',' });
            return new Point(int.Parse(strArray[0]), int.Parse(strArray[1]));
        }

        public static Rectangle RectangleFromString(string sRect)
        {
            string[] strArray = sRect.Split(new char[] { ',' });
            return new Rectangle(int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2]), int.Parse(strArray[3]));
        }

        public void SetProperty(object obj, string name, object value)
        {
            if (value != DBNull.Value)
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(obj)[name];
                if (descriptor != null)
                {
                    if (descriptor.PropertyType == typeof(Color))
                    {
                        descriptor.SetValue(obj, ColorFromString((string) value));
                    }
                    else if (descriptor.PropertyType.IsSubclassOf(typeof(Enum)))
                    {
                        descriptor.SetValue(obj, EnumFromString(descriptor.PropertyType, (string) value));
                    }
                    else if (descriptor.PropertyType == typeof(Font))
                    {
                        descriptor.SetValue(obj, this.FontFromString((string) value));
                    }
                    else if (descriptor.PropertyType == typeof(DateTime))
                    {
                        descriptor.SetValue(obj, DateTimeFromString((string) value));
                    }
                    else if (descriptor.PropertyType == typeof(string[]))
                    {
                        descriptor.SetValue(obj, StringDataFromString((string) value));
                    }
                    else if (descriptor.PropertyType == typeof(Rectangle))
                    {
                        descriptor.SetValue(obj, RectangleFromString((string) value));
                    }
                    else if (descriptor.PropertyType == typeof(Size))
                    {
                        descriptor.SetValue(obj, SizeFromString((string) value));
                    }
                    else if (descriptor.PropertyType == typeof(int))
                    {
                        descriptor.SetValue(obj, Int32FromString((string) value));
                    }
                    else if (descriptor.PropertyType == typeof(Point))
                    {
                        descriptor.SetValue(obj, PointFromString((string) value));
                    }
                    else if (descriptor.PropertyType == typeof(Image))
                    {
                        descriptor.SetValue(obj, ImageFromString((string) value));
                    }
                    else if (descriptor.PropertyType == typeof(ImageList))
                    {
                        descriptor.SetValue(obj, this.GetImageList((string) value));
                    }
                    else
                    {
                        descriptor.SetValue(obj, Convert.ChangeType(value, descriptor.PropertyType, CultureInfo.CurrentCulture));
                    }
                }
            }
        }

        public static Size SizeFromString(string sSize)
        {
            string[] strArray = sSize.Split(new char[] { ',' });
            return new Size(int.Parse(strArray[0]), int.Parse(strArray[1]));
        }

        public static string[] StringDataFromString(string sData)
        {
            return sData.Split(new char[] { ',' });
        }
    }
}

