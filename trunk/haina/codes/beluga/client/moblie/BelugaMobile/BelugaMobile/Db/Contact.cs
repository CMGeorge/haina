using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLiteClient;

namespace BelugaMobile.Db
{
    public class Contact : IDisposable
    {
        private SQLiteDataReader _reader;

        public Contact(SQLiteDataReader reader)
        {
            _reader = reader;
        }

        public void Dispose()
        {
            _reader.Close();
        }

        public bool Next()
        {
            return _reader.Read();
        }

        public Object this[String name]
        {
            get
            {
                return _reader[_reader.GetOrdinal(name)];
            }
        }

        public Object this[int i]
        {
            get
            {
                return _reader[i];
            }
        }
    }

    public class ContactDetail : IDisposable
    {
        private SQLiteDataReader _reader;

        public ContactDetail(SQLiteDataReader reader)
        {
            _reader = reader;
        }
        
        public void Dispose()
        {
            _reader.Close();
        }

        public Object this[String name]
        {
            get
            {
                return _reader[_reader.GetOrdinal(name)];
            }
        }

        public Object this[int i]
        {
            get
            {
                return _reader[i];
            }
        }
    }
}
