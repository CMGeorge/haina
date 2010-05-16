namespace Resco.Controls.ImageBox
{
    using System;
    using System.IO;

    internal class IOStream : IIOStream
    {
        private Stream _stream;

        public IOStream(Stream stream)
        {
            this._stream = stream;
        }

        public int Read(byte[] buffer, int count)
        {
            try
            {
                return this._stream.Read(buffer, 0, count);
            }
            catch
            {
                return -1;
            }
        }

        public int Seek(int offset, int origin)
        {
            SeekOrigin begin;
            if (origin == 0)
            {
                begin = SeekOrigin.Begin;
            }
            else if (origin == 2)
            {
                begin = SeekOrigin.End;
            }
            else
            {
                begin = SeekOrigin.Current;
            }
            try
            {
                return (int) this._stream.Seek((long) offset, begin);
            }
            catch
            {
                return -1;
            }
        }

        public int Write(byte[] buffer, int count)
        {
            try
            {
                this._stream.Write(buffer, 0, count);
                return count;
            }
            catch
            {
                return -1;
            }
        }

        public int Length
        {
            get
            {
                try
                {
                    return (int) this._stream.Length;
                }
                catch
                {
                    return -1;
                }
            }
        }

        public int Position
        {
            get
            {
                try
                {
                    return (int) this._stream.Position;
                }
                catch
                {
                    return -1;
                }
            }
        }
    }
}

