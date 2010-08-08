using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace BeefWrap.Common.Net
{
    /// <summary>
    /// 操作输入流
    /// </summary>
    public class NetWriter
    {
        /// <summary>
        /// 写入指定数量的字节到输入流中
        /// </summary>
        /// <param name="streamDest"></param>
        /// <param name="bytesSrc"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public static void Write(ref Stream streamDest, byte[] bytesSrc, int offset, int count)
        {
            if (null == bytesSrc || bytesSrc.Length == 0)
            {
                return;
            }
            byte[] bytes = bytesSrc;
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < bytes.Length / 2; i++)
                {
                    byte b = bytes[i];
                    bytes[i] = bytes[bytes.Length - 1 - i];
                    bytes[bytes.Length - 1 - i] = b;
                }
            }
            streamDest.Write(bytes, offset, count);
        }

        /// <summary>
        /// 写入一个bool类型
        /// </summary>
        /// <param name="streamDest"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool WriteBool(ref Stream streamDest, bool data)
        {
            byte[] bytes = BitConverter.GetBytes(data);
            Write(ref streamDest, bytes, 0, bytes.Length);
            return data;
        }

        /// <summary>
        /// 写入一个float类型
        /// </summary>
        /// <param name="streamDest"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static float WriteFloat(ref Stream streamDest, float data)
        {
            byte[] bytes = BitConverter.GetBytes(data);
            Write(ref streamDest, bytes, 0, bytes.Length);
            return data;
        }

        /// <summary>
        /// 写入一个double类型
        /// </summary>
        /// <param name="streamDest"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double WriteDouble(ref Stream streamDest, double data)
        {
            byte[] bytes = BitConverter.GetBytes(data);
            Write(ref streamDest, bytes, 0, bytes.Length);
            return data;
        }

        /// <summary>
        /// 写入一个int类型
        /// </summary>
        /// <param name="streamDest"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int WriteInt(ref Stream streamDest, int data)
        {
            byte[] bytes = BitConverter.GetBytes(data);
            Write(ref streamDest, bytes, 0, bytes.Length);
            return data;
        }

        /// <summary>
        /// 写入一个long类型
        /// </summary>
        /// <param name="streamDest"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static long WriteLong(ref Stream streamDest, long data)
        {
            byte[] bytes = BitConverter.GetBytes(data);
            Write(ref streamDest, bytes, 0, bytes.Length);
            return data;
        }

        /// <summary>
        /// 按照utf8编码写入一个字符串
        /// </summary>
        /// <param name="streamDest"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string WriteUTF(ref Stream streamDest, string data)
        {
            return WriteUTF(ref streamDest, data, Encoding.UTF8);
        }

        /// <summary>
        /// 写入一个字符串
        /// </summary>
        /// <param name="streamDest"></param>
        /// <param name="data"></param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string WriteUTF(ref Stream streamDest, string data, Encoding encoding)
        {
            byte[] bytSend = null;
            if (string.IsNullOrEmpty(data))
            {
                bytSend = new byte[0];
            }
            else
            {
                bytSend = encoding.GetBytes(data);
                if (bytSend.Length > 65535)
                {
                    throw new Exception("string类型的数据长度(" + bytSend.Length + ")超过 65535 个字节");
                }
            }

            int utflen = bytSend.Length;
            byte a = (byte)((utflen >> 8) & 0xFF);
            byte b = (byte)((utflen >> 0) & 0xFF);
            streamDest.Write(new byte[] { a, b }, 0, 2);

            int iCount = 0;
            while (iCount < bytSend.Length)
            {
                if (iCount + 1024 > bytSend.Length)
                {
                    streamDest.Write(bytSend, iCount, bytSend.Length - iCount);
                    iCount = bytSend.Length;
                }
                else
                {
                    streamDest.Write(bytSend, iCount, 1024);
                    iCount += 1024;
                }
            }
            return data;
        }
    }
}
