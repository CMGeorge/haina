namespace Resco.Controls.AdvancedList
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

        Resco.Controls.AdvancedList.Mapping Mapping { get; }
    }
}

