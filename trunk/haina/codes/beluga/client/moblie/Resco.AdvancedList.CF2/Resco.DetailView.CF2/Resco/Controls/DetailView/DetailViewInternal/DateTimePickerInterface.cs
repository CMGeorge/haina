namespace Resco.Controls.DetailView.DetailViewInternal
{
    using Resco.Controls.DetailView;
    using System;
    using System.Reflection;
    using System.Windows.Forms;

    internal class DateTimePickerInterface
    {
        private static Assembly m_assembly = null;
        private static string m_assemblyName = "Resco.OutlookControls.CF2.dll";
        private static Type m_touchDateTimePicker = null;

        internal static void AddNoneSelectedEvent(Control control, EventHandler handler)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).NoneSelected += handler;
            }
            else
            {
                InvokeAddEvent(control, "NoneButtonPressed", handler);
            }
        }

        internal static void AddValueChangedEvent(Control control, EventHandler handler)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).ValueChanged += handler;
            }
            else
            {
                InvokeAddEvent(control, "ValueChanged", handler);
            }
        }

        internal static bool GetChecked(Control control)
        {
            if (control is DateTimePickerEx)
            {
                return ((DateTimePickerEx) control).Checked;
            }
            return !((bool) InvokeGetProperty(control, "IsNone"));
        }

        internal static bool GetDroppedDown(Control control)
        {
            if (control is DateTimePickerEx)
            {
                return ((DateTimePickerEx) control).DroppedDown;
            }
            return (bool) InvokeGetProperty(control, "DropDownVisible");
        }

        internal static Type GetTouchDateTimePickerType()
        {
            GetTypes();
            return m_touchDateTimePicker;
        }

        private static void GetTypes()
        {
            if (m_touchDateTimePicker == null)
            {
                m_touchDateTimePicker = Type.GetType("Resco.Controls.OutlookControls.TouchDateTimePicker");
                if (m_touchDateTimePicker == null)
                {
                    m_assembly = Assembly.LoadFrom(m_assemblyName);
                    if (m_assembly == null)
                    {
                        throw new MissingMethodException("Unable to load '" + m_assemblyName + "' assembly.");
                    }
                    m_touchDateTimePicker = m_assembly.GetType("Resco.Controls.OutlookControls.TouchDateTimePicker");
                }
                if (m_touchDateTimePicker == null)
                {
                    throw new MissingMethodException("Unable to get 'Resco.Controls.OutlookControls.TouchDateTimePicker' type. You have to reference Resco OutlookControls control to use ItemTouchDateTime type.");
                }
            }
        }

        internal static DateTime GetValue(Control control)
        {
            if (control is DateTimePickerEx)
            {
                return ((DateTimePickerEx) control).Value;
            }
            return (DateTime) InvokeGetProperty(control, "Value");
        }

        internal static void InvokeAddEvent(object obj, string eventName, Delegate handler)
        {
            obj.GetType().GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance).AddEventHandler(obj, handler);
        }

        internal static object InvokeGetProperty(object obj, string name)
        {
            return obj.GetType().GetProperty(name, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance).GetValue(obj, null);
        }

        internal static object InvokeMethod(object obj, string name, object[] parameters)
        {
            return obj.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.Instance).Invoke(obj, parameters);
        }

        internal static void InvokeRemoveEvent(object obj, string eventName, Delegate handler)
        {
            obj.GetType().GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance).RemoveEventHandler(obj, handler);
        }

        internal static void InvokeSetProperty(object obj, string name, object value)
        {
            obj.GetType().GetProperty(name, BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance).SetValue(obj, value, null);
        }

        internal static void RemoveNoneSelectedEvent(Control control, EventHandler handler)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).NoneSelected -= handler;
            }
            else
            {
                InvokeRemoveEvent(control, "NoneButtonPressed", handler);
            }
        }

        internal static void RemoveValueChangedEvent(Control control, EventHandler handler)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).ValueChanged -= handler;
            }
            else
            {
                InvokeRemoveEvent(control, "ValueChanged", handler);
            }
        }

        internal static void SetChecked(Control control, bool value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).Checked = value;
            }
            else
            {
                InvokeSetProperty(control, "IsNone", !value);
            }
        }

        internal static void SetCustomFormat(Control control, string value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).CustomFormat = value;
            }
            else
            {
                InvokeSetProperty(control, "CustomFormat", value);
            }
        }

        internal static void SetDroppedDown(Control control, bool value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).DroppedDown = value;
            }
            else
            {
                InvokeSetProperty(control, "DropDownVisible", value);
            }
        }

        internal static void SetFormat(Control control, Resco.Controls.DetailView.DateTimePickerFormat value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).Format = value;
            }
            else
            {
                InvokeSetProperty(control, "Format", (int) value);
            }
        }

        internal static void SetNoneText(Control control, string value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).NoneText = value;
            }
            else
            {
                InvokeSetProperty(control, "NoneText", value);
            }
        }

        internal static void SetShowDropDown(Control control, bool value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).ShowDropDown = value;
            }
            else
            {
                InvokeSetProperty(control, "ShowDropDown", value);
            }
        }

        internal static void SetShowNone(Control control, bool value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).ShowCheckBox = value;
            }
            else
            {
                InvokeSetProperty(control, "ShowNone", value);
            }
        }

        internal static void SetShowTimeNone(Control control, bool value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).ShowTimeNone = value;
            }
        }

        internal static void SetShowUpDown(Control control, bool value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).ShowUpDown = value;
            }
        }

        internal static void SetTimePickerEndTime(Control control, DateTime value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).TimePickerEndTime = value;
            }
        }

        internal static void SetTimePickerMinuteInterval(Control control, double value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).TimePickerMinuteInterval = value;
            }
            else
            {
                InvokeSetProperty(control, "MinuteInterval", (int) value);
            }
        }

        internal static void SetTimePickerStartTime(Control control, DateTime value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).TimePickerStartTime = value;
            }
        }

        internal static void SetValue(Control control, DateTime value)
        {
            if (control is DateTimePickerEx)
            {
                ((DateTimePickerEx) control).Value = value;
            }
            else
            {
                InvokeSetProperty(control, "Value", value);
            }
        }
    }
}

