namespace Resco.Controls.AdvancedComboBox
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Reflection;

    public class DataConnector : IDataConnector, IDisposable
    {
        internal bool IsDesignTime;
        private bool m_bWasOpen;
        private IDbCommand m_cmd;
        private string m_cmdText;
        private string m_conn;
        private object[] m_data;
        private Resco.Controls.AdvancedComboBox.Mapping m_map;
        private IDataReader m_reader;

        public DataConnector()
        {
        }

        public DataConnector(IDbCommand cmd)
        {
            this.m_cmd = cmd;
        }

        public void Close()
        {
            try
            {
                if (this.m_reader != null)
                {
                    this.m_reader.Close();
                    if (!this.m_bWasOpen && (this.m_cmd.Connection.State == ConnectionState.Open))
                    {
                        this.m_cmd.Connection.Close();
                    }
                }
            }
            finally
            {
                this.m_reader = null;
                this.m_map = null;
                this.m_data = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.IsOpen)
                {
                    try
                    {
                        this.Close();
                    }
                    catch
                    {
                    }
                }
                if (this.m_cmd != null)
                {
                    this.m_cmd.Dispose();
                    this.m_cmd = null;
                }
            }
        }

        public bool MoveNext()
        {
            bool flag = this.m_reader.Read();
            if (flag)
            {
                this.m_data = new object[this.m_reader.FieldCount];
                this.m_reader.GetValues(this.m_data);
                return flag;
            }
            this.m_data = null;
            return flag;
        }

        public bool Open()
        {
            if (this.IsOpen)
            {
                throw new InvalidOperationException("DataConnector is already open.");
            }
            if (this.m_cmd == null)
            {
                throw new ArgumentNullException("Command", "No Command set.");
            }
            if (this.m_cmd.Connection == null)
            {
                throw new ArgumentNullException("Connection", "No database connection set.");
            }
            this.m_bWasOpen = this.m_cmd.Connection.State == ConnectionState.Open;
            try
            {
                if (!this.m_bWasOpen)
                {
                    this.m_cmd.Connection.Open();
                }
                this.m_reader = this.m_cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                this.m_map = new Resco.Controls.AdvancedComboBox.Mapping(this.m_reader);
            }
            finally
            {
                if (((this.m_reader == null) && !this.m_bWasOpen) && (this.m_cmd.Connection.State == ConnectionState.Open))
                {
                    this.m_cmd.Connection.Close();
                }
            }
            return this.IsOpen;
        }

        protected virtual bool ShouldSerializeCommandText()
        {
            return (this.m_cmdText != null);
        }

        protected virtual bool ShouldSerializeConnectionString()
        {
            return (this.m_conn != null);
        }

        public override string ToString()
        {
            return this.CommandText;
        }

        public virtual IDbCommand Command
        {
            get
            {
                if (this.m_cmd == null)
                {
                    try
                    {
                        Assembly assembly = Assembly.Load("System.Data.SqlServerCe, Version=3.0.3600.0, Culture=neutral, PublicKeyToken=3be235df1c8d2ad3");
                        this.m_cmd = (IDbCommand) assembly.CreateInstance("System.Data.SqlServerCe.SqlCeCommand");
                        this.m_cmd.Connection = (IDbConnection) assembly.CreateInstance("System.Data.SqlServerCe.SqlCeConnection");
                    }
                    catch
                    {
                        this.m_cmd = null;
                    }
                }
                return this.m_cmd;
            }
            set
            {
                this.m_cmd = value;
            }
        }

        public string CommandText
        {
            get
            {
                if (this.IsDesignTime)
                {
                    if (this.m_cmdText != null)
                    {
                        return this.m_cmdText;
                    }
                    return "";
                }
                if (this.m_cmd != null)
                {
                    return this.m_cmd.CommandText;
                }
                return "";
            }
            set
            {
                this.m_cmdText = (value == "") ? null : value;
                if (!this.IsDesignTime && (this.Command != null))
                {
                    this.m_cmd.CommandText = value;
                }
            }
        }

        public string ConnectionString
        {
            get
            {
                if (this.IsDesignTime)
                {
                    if (this.m_conn != null)
                    {
                        return this.m_conn;
                    }
                    return "";
                }
                if ((this.m_cmd != null) && (this.m_cmd.Connection != null))
                {
                    return this.m_cmd.Connection.ConnectionString;
                }
                return "";
            }
            set
            {
                this.m_conn = (value == "") ? null : value;
                if ((!this.IsDesignTime && (this.Command != null)) && (this.m_cmd.Connection != null))
                {
                    this.m_cmd.Connection.ConnectionString = value;
                }
            }
        }

        public virtual IList Current
        {
            get
            {
                if (this.m_data != null)
                {
                    return this.m_data;
                }
                return new object[0];
            }
        }

        public bool IsOpen
        {
            get
            {
                return (this.m_reader != null);
            }
        }

        public virtual Resco.Controls.AdvancedComboBox.Mapping Mapping
        {
            get
            {
                return this.m_map;
            }
        }
    }
}

