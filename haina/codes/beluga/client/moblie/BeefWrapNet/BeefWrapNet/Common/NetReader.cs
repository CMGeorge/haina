using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace BeefWrap.Common.Net
{
    /// <summary>
    /// 操作输出流
    /// </summary>
    public class NetReader
    {
        /// <summary>
        /// 读取指定数量的字节
        /// </summary>
        /// <param name="streamSrc"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte[] Read(ref Stream streamSrc, int count)
        {
            if (count <= 0)
            {
                return new byte[0];
            }
            sbyte[] sbytes = new sbyte[count];
            ReadInput(streamSrc, sbytes, 0, sbytes.Length);
            byte[] bytes = ToByteArray(sbytes);
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < bytes.Length / 2; i++)
                {
                    byte b = bytes[i];
                    bytes[i] = bytes[bytes.Length - 1 - i];
                    bytes[bytes.Length - 1 - i] = b;
                }
            }
            return bytes;
        }

        /// <summary>
        /// 读取一个bool类型
        /// </summary>
        /// <param name="streamSrc"></param>
        /// <returns></returns>
        public static bool ReadBool(ref Stream streamSrc)
        {
            byte[] bytes = Read(ref streamSrc, 1);
            return BitConverter.ToBoolean(bytes, 0);
        }

        /// <summary>
        /// 读取一个int类型
        /// </summary>
        /// <param name="streamSrc"></param>
        /// <returns></returns>
        public static int ReadInt(ref Stream streamSrc)
        {
            byte[] bytes = Read(ref streamSrc, 4);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// 读取一个long类型
        /// </summary>
        /// <param name="streamSrc"></param>
        /// <returns></returns>
        public static long ReadLong(ref Stream streamSrc)
        {
            byte[] bytes = Read(ref streamSrc, 8);
            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// 读取一个doubule类型
        /// </summary>
        /// <param name="streamSrc"></param>
        /// <returns></returns>
        public static double ReadDouble(ref Stream streamSrc)
        {
            byte[] bytes = Read(ref streamSrc, 8);
            return BitConverter.ToDouble(bytes, 0);
        }

        /// <summary>
        /// 读取一个float类型
        /// </summary>
        /// <param name="streamSrc"></param>
        /// <returns></returns>
        public static float ReadFloat(ref Stream streamSrc)
        {
            byte[] bytes = Read(ref streamSrc, 4);
            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary>
        /// 读取一个字符串
        /// </summary>
        /// <param name="streamSrc"></param>
        /// <returns></returns>
        public static string ReadUTF(ref Stream streamSrc)
        {
            return ReadUTF(ref streamSrc, Encoding.UTF8);
        }

        /// <summary>
        /// 读取一个字符串
        /// </summary>
        /// <param name="streamSrc"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadUTF(ref Stream streamSrc, Encoding encoding)
        {
            sbyte[] sbyte1 = new sbyte[2];
            ReadInput(streamSrc, sbyte1, 0, sbyte1.Length);
            byte[] byte2 = ToByteArray(sbyte1);
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < byte2.Length / 2; i++)
                {
                    byte b = byte2[i];
                    byte2[i] = byte2[byte2.Length - 1 - i];
                    byte2[byte2.Length - 1 - i] = b;
                }
            }
            UInt16 string_length = BitConverter.ToUInt16(byte2, 0);
            if (string_length == 0)
            {
                return "";
            }

            sbyte[] str_byte = new sbyte[string_length];
            int returnLen = ReadInput(streamSrc, str_byte, 0, str_byte.Length);
            string data = encoding.GetString(ToByteArray(str_byte), 0, returnLen);

            return data;
        }

        /// <summary>
        /// 从输入流中读取指定数量的字节到字节数组中
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="target"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static int ReadInput(Stream sourceStream, sbyte[] target, int start, int count)
        {
            // Returns 0 bytes if not enough space in target
            if (target.Length == 0)
                return 0;

            byte[] receiver = new byte[target.Length];
            int bytesRead = sourceStream.Read(receiver, start, count);

            // Returns -1 if EOF
            if (bytesRead == 0)
            {
                return -1;
            }

            for (int i = start; i < start + bytesRead; i++)
            {
                target[i] = (sbyte)receiver[i];
            }

            return bytesRead;
        }

        /// <summary>
        /// 字节类型转换sbyte[]=>byte[]
        /// </summary>
        /// <param name="sbyteArray"></param>
        /// <returns></returns>
        private static byte[] ToByteArray(sbyte[] sbyteArray)
        {
            byte[] byteArray = null;

            if (sbyteArray != null)
            {
                byteArray = new byte[sbyteArray.Length];
                for (int index = 0; index < sbyteArray.Length; index++)
                {
                    byteArray[index] = (byte)sbyteArray[index];
                }
            }
            return byteArray;
        }
    }
}
