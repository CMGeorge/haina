namespace Resco.Controls.ImageBox
{
    using System;
    using System.Runtime.InteropServices;

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("BFEDF19A-DF54-4f4a-8CA6-9C39BD381F6E")]
    public interface IIOStream
    {
        int Read([Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] byte[] buffer, int count);
        int Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] byte[] buffer, int count);
        int Seek(int offset, int origin);
        int Position { get; }
        int Length { get; }
    }
}

