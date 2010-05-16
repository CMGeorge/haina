namespace Resco.Controls.MaskedTextBox
{
    using System;
    using System.ComponentModel;

    public class TypeValidationEventArgs : CancelEventArgs
    {
        private bool m_isValidInput;
        private string m_message;
        private object m_returnValue;
        private Type m_validatingType;

        public TypeValidationEventArgs(Type validatingType, bool isValidInput, object returnValue, string message)
        {
            this.m_validatingType = validatingType;
            this.m_isValidInput = isValidInput;
            this.m_returnValue = returnValue;
            this.m_message = message;
            base.Cancel = false;
        }

        public bool IsValidInput
        {
            get
            {
                return this.m_isValidInput;
            }
        }

        public string Message
        {
            get
            {
                return this.m_message;
            }
        }

        public object ReturnValue
        {
            get
            {
                return this.m_returnValue;
            }
        }

        public Type ValidatingType
        {
            get
            {
                return this.m_validatingType;
            }
        }
    }
}

