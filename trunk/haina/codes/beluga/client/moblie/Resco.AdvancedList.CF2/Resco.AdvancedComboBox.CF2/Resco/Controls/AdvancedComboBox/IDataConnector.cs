namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Collections;

    public interface IDataConnector : IDisposable
    {
        void Close();
        bool MoveNext();
        bool Open();

        IList Current { get; }

        bool IsOpen { get; }

        Resco.Controls.AdvancedComboBox.Mapping Mapping { get; }
    }
}

