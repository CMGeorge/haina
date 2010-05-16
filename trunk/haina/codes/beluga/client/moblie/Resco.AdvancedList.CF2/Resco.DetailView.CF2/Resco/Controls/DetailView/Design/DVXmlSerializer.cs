namespace Resco.Controls.DetailView.Design
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;

    internal class DVXmlSerializer
    {
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

        private string GetAssemblyName(string fileName)
        {
            return fileName.ToLower().Replace(".dll", "").Replace(".cf2", "").Replace(",cf3", "");
        }

        public static void SaveXml(string fileName, Control detailView)
        {
            XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument(true);
            writer.WriteStartElement("DSDetailView");
            writer.WriteAttributeString("xmlns", null, "http://www.rescodeveloper.net/schemas/DSDetailView.xsd");
            new DVXmlSerializer().XmlSerialize(writer, detailView);
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }

        public static void SaveXml(XmlWriter writer, Control detailView)
        {
            new DVXmlSerializer().XmlSerialize(writer, detailView);
        }

        internal string Serialize(object value)
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
            if (ReflectionHelper.IsType(value, "GradientColor"))
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
                Color color = (Color) properties["StartColor"].GetValue(value);
                Color color2 = (Color) properties["EndColor"].GetValue(value);
                object obj2 = properties["FillDirection"].GetValue(value);
                return string.Format("{0},{1},{2}", this.ColorToName(color), this.ColorToName(color2), obj2);
            }
            return string.Format(CultureInfo.InvariantCulture, "{0}", new object[] { value });
        }

        private void SerializeCollection(XmlWriter writer, PropertyDescriptor pd, object data)
        {
            if (!(data is IList) || (((IList) data).Count != 0))
            {
                IEnumerable enumerable = (IEnumerable) data;
                writer.WriteStartElement("Collection");
                writer.WriteAttributeString("Name", pd.Name);
                writer.WriteAttributeString("Type", data.GetType().FullName);
                if (!data.GetType().Assembly.ManifestModule.Name.ToLower().StartsWith("resco.detailview"))
                {
                    writer.WriteAttributeString("Assembly", this.GetAssemblyName(data.GetType().Assembly.ManifestModule.Name));
                }
                foreach (object obj2 in enumerable)
                {
                    writer.WriteStartElement("Object");
                    writer.WriteAttributeString("Type", obj2.GetType().FullName);
                    if (!obj2.GetType().Assembly.ManifestModule.Name.ToLower().StartsWith("resco.detailview"))
                    {
                        writer.WriteAttributeString("Assembly", this.GetAssemblyName(obj2.GetType().Assembly.ManifestModule.Name));
                    }
                    this.SerializeProperties(writer, obj2);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }

        private void SerializeDetailViewProperties(XmlWriter writer, Control detailView)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(detailView))
            {
                CustomPropertyDescriptor descriptor2 = new CustomPropertyDescriptor(descriptor);
                if (descriptor2.Attributes.Contains(Resco.Controls.DetailView.Design.BrowsableAttribute.Yes) && descriptor2.Attributes.Contains(Resco.Controls.DetailView.Design.DesignerSerializationVisibilityAttribute.Visible))
                {
                    object obj2 = descriptor2.GetValue(detailView);
                    if ((obj2 != null) && descriptor2.ShouldSerializeValue(detailView))
                    {
                        writer.WriteStartElement("Property");
                        writer.WriteAttributeString("Name", descriptor2.Name);
                        writer.WriteAttributeString("Value", this.Serialize(obj2));
                        writer.WriteEndElement();
                    }
                }
            }
        }

        private void SerializeProperties(XmlWriter writer, object data)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(data))
            {
                CustomPropertyDescriptor pd = new CustomPropertyDescriptor(descriptor);
                if (pd.SerializationVisibility != Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Hidden)
                {
                    object obj2 = pd.GetValue(data);
                    if ((obj2 != null) && pd.ShouldSerializeValue(data))
                    {
                        if ((pd.SerializationVisibility == Resco.Controls.DetailView.Design.DesignerSerializationVisibility.Content) && (obj2 is IEnumerable))
                        {
                            this.SerializeCollection(writer, pd, obj2);
                        }
                        else
                        {
                            writer.WriteStartElement("Property");
                            writer.WriteAttributeString("Name", pd.Name);
                            writer.WriteAttributeString("Value", this.Serialize(obj2));
                            writer.WriteEndElement();
                        }
                    }
                }
            }
        }

        public void XmlSerialize(XmlWriter writer, Control detailView)
        {
            writer.WriteStartElement("DetailView");
            int dpiX = 0x60;
            int dpiY = 0x60;
            if (true)
            {
                Graphics graphics = detailView.CreateGraphics();
                dpiX = (int) graphics.DpiX;
                dpiY = (int) graphics.DpiY;
                graphics.Dispose();
                graphics = null;
            }
            writer.WriteAttributeString("DpiX", Convert.ToString(dpiX));
            writer.WriteAttributeString("DpiY", Convert.ToString(dpiY));
            this.SerializeDetailViewProperties(writer, detailView);
            IList propertyValue = (IList) ReflectionHelper.GetPropertyValue(detailView, "Items");
            foreach (object obj2 in propertyValue)
            {
                writer.WriteStartElement("Item");
                Type type = obj2.GetType();
                if (type.Assembly.ManifestModule.Name.ToLower().StartsWith("resco.detailview"))
                {
                    writer.WriteAttributeString("Type", type.FullName);
                }
                else
                {
                    writer.WriteAttributeString("Type", type.AssemblyQualifiedName);
                }
                this.SerializeProperties(writer, obj2);
                writer.WriteEndElement();
            }
        }
    }
}

