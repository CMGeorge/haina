namespace Resco.Controls.DetailView
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Reflection;
    using System.Windows.Forms;

    internal class DVConversion
    {
        internal static Resco.Controls.DetailView.DetailView.DesignTimeCallback DesignTimeCallback;

        public static AnchorStyles AnchorStylesFromString(string sValue)
        {
            AnchorStyles none = AnchorStyles.None;
            foreach (string str in sValue.Split(new char[] { ',' }))
            {
                none |= (AnchorStyles) EnumFromString(typeof(AnchorStyles), str);
            }
            return none;
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
            sValue = sValue.Trim();
            return (Enum) enumType.GetField(sValue).GetValue(null);
        }

        public static Font FontFromString(string sFont)
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
            float emSize = Convert.ToSingle(str2.Substring(0, num5).Trim(), NumberFormatInfo.InvariantInfo);
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
            if (DesignTimeCallback != null)
            {
                DesignTimeCallback(2, o);
            }
            return o;
        }

        public static GradientColor GradientColorFromString(string sGradientColor)
        {
            string[] strArray = sGradientColor.Split(new char[] { ',' });
            return new GradientColor(ColorFromString(strArray[0]), ColorFromString(strArray[1]), (FillDirection) EnumFromString(typeof(FillDirection), strArray[2]));
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
                    else if (descriptor.PropertyType == typeof(AnchorStyles))
                    {
                        descriptor.SetValue(obj, AnchorStylesFromString((string) value));
                    }
                    else if (descriptor.PropertyType.IsSubclassOf(typeof(Enum)))
                    {
                        descriptor.SetValue(obj, EnumFromString(descriptor.PropertyType, (string) value));
                    }
                    else if (descriptor.PropertyType == typeof(Font))
                    {
                        descriptor.SetValue(obj, FontFromString((string) value));
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
                    else if (descriptor.PropertyType == typeof(Point))
                    {
                        descriptor.SetValue(obj, PointFromString((string) value));
                    }
                    else if (descriptor.PropertyType == typeof(GradientColor))
                    {
                        descriptor.SetValue(obj, GradientColorFromString((string) value));
                    }
                    else
                    {
                        try
                        {
                            descriptor.SetValue(obj, Convert.ChangeType(value, descriptor.PropertyType, CultureInfo.InvariantCulture));
                        }
                        catch
                        {
                            try
                            {
                                descriptor.SetValue(obj, Convert.ChangeType(value, descriptor.PropertyType, CultureInfo.CurrentCulture));
                            }
                            catch
                            {
                                descriptor.SetValue(obj, value);
                            }
                        }
                    }
                }
            }
        }

        public static Size SizeFromString(string sRect)
        {
            string[] strArray = sRect.Split(new char[] { ',' });
            return new Size(int.Parse(strArray[0]), int.Parse(strArray[1]));
        }

        public static string[] StringDataFromString(string sData)
        {
            return sData.Split(new char[] { ',' });
        }
    }
}

