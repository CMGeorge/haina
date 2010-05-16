namespace Resco.Controls.AdvancedList.Design
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;

    internal class ALXmlSerializer
    {
        internal List<ImageList> ImageLists = new List<ImageList>();
        private int m_ILUnique = 1;
        internal string[] SystemColorNames = new string[] { 
            "ScrollBar", "Desktop", "ActiveCaption", "InactiveCaption", "Menu", "Window", "WindowFrame", "MenuText", "WindowText", "ActiveCaptionText", "ActiveBorder", "InactiveBorder", "AppWorkspace", "Highlight", "HighlightText", "Control", 
            "ControlDark", "GrayText", "ControlText", "InactiveCaptionText", "ControlLightLight", "ControlDarkDark", "ControlLight", "InfoText", "Info", "", "HotTrack"
         };

        internal string ColorToName(Color color)
        {
            string str = color.ToArgb().ToString("x");
            if (color.IsSystemColor)
            {
                try
                {
                    object obj2 = color.GetType().GetField("m_clr", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(color);
                    int index = (int) obj2.GetType().GetField("m_nVal", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj2);
                    str = this.SystemColorNames[index];
                }
                catch
                {
                }
            }
            return str;
        }

        public static void SaveXml(string fileName, Control advancedList)
        {
            XmlWriter writer = null;
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent=true;
                settings.Encoding=Encoding.UTF8;
                settings.CloseOutput=true;
                writer = XmlWriter.Create(fileName, settings);
                writer.WriteStartDocument(true);
                writer.WriteStartElement("DSAdvancedList", "http://www.rescodeveloper.net/schemas/DSAdvancedList.xsd");
                SaveXml(writer, advancedList);
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                writer = null;
            }
        }

        public static void SaveXml(XmlWriter writer, Control advancedList)
        {
            new ALXmlSerializer().XmlSerialize(writer, advancedList);
        }

        public string Serialize(object value)
        {
            if ((value == null) || (value == DBNull.Value))
            {
                return null;
            }
            if (value is Color)
            {
                return this.ColorToName((Color) value);
            }
            if (value is Font)
            {
                Font font1 = (Font) value;
                return string.Format(CultureInfo.InvariantCulture, "{0}, {1}pt, style={2}", new object[] { ((Font) value).Name, ((Font) value).Size, ((Font) value).Style });
            }
            if (value is string[])
            {
                return string.Join(",", (string[]) value);
            }
            if (value is Rectangle)
            {
                object[] args = new object[4];
                Rectangle rectangle = (Rectangle) value;
                args[0] = rectangle.X;
                Rectangle rectangle2 = (Rectangle) value;
                args[1] = rectangle2.Y;
                Rectangle rectangle3 = (Rectangle) value;
                args[2] = rectangle3.Width;
                Rectangle rectangle4 = (Rectangle) value;
                args[3] = rectangle4.Height;
                return string.Format("{0},{1},{2},{3}", args);
            }
            if (value is Point)
            {
                Point point = (Point) value;
                Point point2 = (Point) value;
                return string.Format("{0},{1}", point.X, point2.Y);
            }
            if (value is Size)
            {
                Size size = (Size) value;
                Size size2 = (Size) value;
                return string.Format("{0},{1}", size.Width, size2.Height);
            }
            if (value is Image)
            {
                Image image = (Image) value;
                MemoryStream stream = new MemoryStream();
                try
                {
                    image.Save(stream, ImageFormat.Png);
                }
                catch
                {
                    image.Save(stream, ImageFormat.Bmp);
                }
                return Convert.ToBase64String(stream.ToArray());
            }
            if (value is ImageList)
            {
                ImageList c = value as ImageList;
                ISite site = c.Site;
                if (!this.ImageLists.Contains(c))
                {
                    this.ImageLists.Add(c);
                }
                if (((site == null) || (site.Name == null)) || (site.Name == ""))
                {
                    string n = "imageList" + this.m_ILUnique;
                    this.m_ILUnique++;
                    foreach (ImageList list2 in this.ImageLists)
                    {
                        if ((list2.Site != null) && (list2.Site.Name == n))
                        {
                            n = "imageList" + this.m_ILUnique;
                            this.m_ILUnique++;
                        }
                    }
                    if (site == null)
                    {
                        site = new RTImageListSite(c, n);
                    }
                    else
                    {
                        site.Name = n;
                    }
                    c.Site = site;
                }
                if (site != null)
                {
                    return site.Name;
                }
                return null;
            }
            if (value is bool)
            {
                if (!((bool) value))
                {
                    return "false";
                }
                return "true";
            }
            if (ReflectionHelper.IsType(value, "GradientColor"))
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
                Color color = (Color) properties["StartColor"].GetValue(value);
                Color color2 = (Color) properties["MiddleColor1"].GetValue(value);
                Color color3 = (Color) properties["MiddleColor2"].GetValue(value);
                Color color4 = (Color) properties["EndColor"].GetValue(value);
                object obj2 = properties["FillDirection"].GetValue(value);
                return string.Format("{0},{1},{2},{3},{4}", new object[] { this.ColorToName(color), this.ColorToName(color4), obj2, this.ColorToName(color2), this.ColorToName(color3) });
            }
            return value.ToString();
        }

        private void SerializeCell(XmlWriter writer, object cell)
        {
            writer.WriteStartElement("Cell");
            Type type = cell.GetType();
            writer.WriteAttributeString("Type", type.FullName);
            this.SerializeObjectsProperties(writer, cell);
            writer.WriteEndElement();
        }

        private void SerializeCollection(XmlWriter writer, object data)
        {
            IEnumerable enumerable = (IEnumerable) data;
            foreach (object obj2 in enumerable)
            {
                if (ReflectionHelper.IsType(obj2, "Cell"))
                {
                    this.SerializeCell(writer, obj2);
                }
                else
                {
                    writer.WriteStartElement(obj2.GetType().Name);
                    this.SerializeObjectsProperties(writer, obj2);
                    writer.WriteEndElement();
                }
            }
        }

        private void SerializeImage(XmlWriter writer, Image image)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                image.Save(stream, ImageFormat.Png);
            }
            catch
            {
                image.Save(stream, ImageFormat.Bmp);
            }
            writer.WriteStartElement("Image");
            writer.WriteStartElement("Data");
            byte[] buffer = stream.ToArray();
            writer.WriteBase64(buffer, 0, buffer.Length);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        private void SerializeObjectsProperties(XmlWriter writer, object o)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(o))
            {
                CustomPropertyDescriptor descriptor2 = new CustomPropertyDescriptor(descriptor);
                if (descriptor2.Attributes.Contains(Resco.Controls.AdvancedList.Design.BrowsableAttribute.Yes) || ((descriptor2.Name == "Name") && (descriptor2.SerializationVisibility == Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Visible)))
                {
                    object obj2 = descriptor2.GetValue(o);
                    if ((obj2 != null) && descriptor2.ShouldSerializeValue(o))
                    {
                        if ((descriptor2.SerializationVisibility == Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Content) && (descriptor2.Name != "StringData"))
                        {
                            if (obj2 is IEnumerable)
                            {
                                if (!ReflectionHelper.IsType(obj2, "RowCollection"))
                                {
                                    this.SerializeCollection(writer, obj2);
                                }
                                continue;
                            }
                            if (!ReflectionHelper.IsType(obj2, "CellSource"))
                            {
                                continue;
                            }
                        }
                        if (ReflectionHelper.IsType(obj2, "HeaderRow"))
                        {
                            writer.WriteStartElement(descriptor2.Name);
                            this.SerializeObjectsProperties(writer, obj2);
                            writer.WriteEndElement();
                        }
                        else
                        {
                            writer.WriteStartElement("Property");
                            writer.WriteAttributeString("Name", descriptor2.Name);
                            writer.WriteAttributeString("Value", this.Serialize(obj2));
                            writer.WriteEndElement();
                        }
                    }
                }
            }
        }

        public void XmlSerialize(XmlWriter writer, Control advancedList)
        {
            writer.WriteStartElement("AdvancedList");
            this.SerializeObjectsProperties(writer, advancedList);
            writer.WriteStartElement("DbConnector");
            object propertyValue = ReflectionHelper.GetPropertyValue(advancedList, "DbConnector");
            writer.WriteElementString("CommandText", Convert.ToString(ReflectionHelper.GetPropertyValue(propertyValue, "CommandText")));
            writer.WriteElementString("ConnectionString", Convert.ToString(ReflectionHelper.GetPropertyValue(propertyValue, "ConnectionString")));
            writer.WriteEndElement();
            writer.WriteEndElement();
            foreach (ImageList list in this.ImageLists)
            {
                writer.WriteStartElement("ImageList");
                writer.WriteAttributeString("Name", list.Site.Name);
                writer.WriteAttributeString("ImageSize", this.Serialize(list.ImageSize));
                IList list2 = TypeDescriptor.GetProperties(list)["Images"].GetValue(list) as IList;
                for (int i = 0; i < list2.Count; i++)
                {
                    object obj3 = list2[i];
                    Image image = null;
                    PropertyInfo property = null;
                    property = obj3.GetType().GetProperty("RawImage", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    if (property == null)
                    {
                        property = obj3.GetType().GetProperty("Image", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    }
                    if (property != null)
                    {
                        image = property.GetGetMethod(true).Invoke(obj3, new object[0]) as Image;
                    }
                    if (image == null)
                    {
                        image = obj3 as Image;
                    }
                    this.SerializeImage(writer, image);
                }
                writer.WriteEndElement();
            }
        }
    }
}

