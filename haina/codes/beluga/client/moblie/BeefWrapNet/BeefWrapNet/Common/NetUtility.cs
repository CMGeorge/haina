using System;

using System.Collections.Generic;
using System.Text;

namespace BeefWrap.Common.Net
{
    public class NetUtility
    {
        /// <summary>
        /// 标识请求的唯一令牌
        /// </summary>
        /// <returns></returns>
        public static string GetNextToken()
        {
            return DateTime.Now.ToString("yy-MM-dd HH:mm:ss ") + new Random((int)DateTime.Now.Ticks / 1000).Next(999).ToString();
        }

        /// <summary>
        /// 计算bool类型的长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetLength(bool data)
        {
            return 1;
        }

        /// <summary>
        /// 计算float类型的长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetLength(float data)
        {
            return 4;
        }

        /// <summary>
        /// 计算double类型的长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetLength(double data)
        {
            return 8;
        }

        /// <summary>
        /// 计算int类型的长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetLength(int data)
        {
            return 4;
        }

        /// <summary>
        /// 计算long类型的长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetLength(long data)
        {
            return 8;
        }

        /// <summary>
        /// 计算string类型的长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetLength(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return 2;
            }
            return 2 + Encoding.UTF8.GetBytes(data).Length;
        }

        /// <summary>
        /// 计算string类型的长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetLength(string data, Encoding encoding)
        {
            if (string.IsNullOrEmpty(data))
            {
                return 2;
            }
            return 2 + encoding.GetBytes(data).Length;
        }
    }
}
