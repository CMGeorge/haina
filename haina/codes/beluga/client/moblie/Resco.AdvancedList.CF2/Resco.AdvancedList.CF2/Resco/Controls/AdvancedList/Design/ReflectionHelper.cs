namespace Resco.Controls.AdvancedList.Design
{
    using System;
    using System.Reflection;

    internal class ReflectionHelper
    {
        internal static object CreateInstance(string type, Type[] types, object[] parameters)
        {
            return Type.GetType("Resco.Controls.AdvancedList." + type).GetConstructor(types).Invoke(parameters);
        }

        internal static object GetFieldValue(object obj, string fieldName)
        {
            return obj.GetType().GetField(fieldName).GetValue(obj);
        }

        internal static object GetPropertyValue(object obj, string propertyName)
        {
            PropertyInfo property = obj.GetType().GetProperty(propertyName);
            if (property != null)
            {
                return property.GetValue(obj, null);
            }
            return null;
        }

        internal static object GetStaticPropertyValue(string type, string propertyName)
        {
            return Type.GetType("Resco.Controls.AdvancedList." + type).GetProperty(propertyName).GetValue(null, null);
        }

        internal static Type GetType(string name)
        {
            return Type.GetType("Resco.Controls.AdvancedList." + name);
        }

        internal static object InvokeMethod(object obj, string methodName, object[] parameters)
        {
            return obj.GetType().GetMethod(methodName).Invoke(obj, parameters);
        }

        internal static bool IsType(object obj, string type)
        {
            string typeName = "Resco.Controls.AdvancedList." + type;
            Type c = Type.GetType(typeName);
            if (!(obj.GetType().FullName == typeName))
            {
                return obj.GetType().IsSubclassOf(c);
            }
            return true;
        }

        internal static void SetPropertyValue(object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, value, null);
        }
    }
}

