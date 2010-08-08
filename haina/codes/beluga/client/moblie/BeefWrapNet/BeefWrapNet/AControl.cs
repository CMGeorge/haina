using System.IO;
using BeefWrap.Common.Net;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

namespace BeefWrap.Net
{
    public abstract class AControl
    {
        /// <summary>
        /// 获取请求地址
        /// </summary>
        public abstract string RequestAddress
        {
            get;
        }

        /// <summary>
        /// 服务器地址，不要以‘/’结尾
        /// </summary>
        protected string RequestHost
        {
            get
            {
                // 可以改成动态从配置文件读取
                return "http://www.sihu.com";
            }
        }

        /// <summary>
        /// 客户端版本
        /// </summary>
        protected string Version
        {
            get
            {
                // 可以改成动态从配置文件读取
                return "1.0";
            }
        }
        /// <summary>
        /// 读取Response的结果
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        protected void ReadResult(ref Stream stream, out Result result)
        {
            ReadResult(ref stream, out result, true);
        }
        /// <summary>
        /// 读取Response的结果
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        /// <param name="readValue"></param>
        protected void ReadResult(ref Stream stream, out Result result, bool readValue)
        {
            result = new Result();
            result.StatusCode = NetReader.ReadInt(ref stream);
            result.StatusText = NetReader.ReadUTF(ref stream);
            if (readValue)
            {
                result.Value = NetReader.ReadUTF(ref stream);
            }
        }
        /// <summary>
        /// 方法调用是否成功（逻辑判断）
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected bool IsResultOK(ref Result result)
        {
            if (result.StatusCode == 0)
            {
                return true;
            }
            return false;
        }

        #region 缓存方法

        /// <summary>
        /// 缓存目录
        /// </summary>
        private static string CacheFolderFullName
        {
            get
            {
                string exeFullName = Assembly.GetCallingAssembly().ManifestModule.FullyQualifiedName;
                if (string.IsNullOrEmpty(exeFullName))
                {
                    exeFullName = Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName;
                }
                exeFullName = exeFullName.Substring(0, exeFullName.LastIndexOf(@"\"));
                return exeFullName + @"\Cache";
            }
        }

        private static int _ExpireSeconds = 60; // 默认缓存保存时间60秒
        public static int ExpireSeconds
        {
            get
            {
                if (_ExpireSeconds < 0)
                {
                    return 0;
                }
                return _ExpireSeconds;
            }
            set
            {
                _ExpireSeconds = value;
            }
        }

        /// <summary>
        /// 缓存文件扩展名
        /// </summary>
        private static readonly string CacheFileExtension = ".mem";

        [DllImport("BeefWrapCache.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool ReadCacheLength(string cacheFolderFullName, string name, ref Int32 statusTextLength, ref Int32 strValueLength);

        [DllImport("BeefWrapCache.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool DeleteCache(string cacheFolderFullName, string name);

        [DllImport("BeefWrapCache.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool DeleteCacheLike(string cacheFolderFullName, string namePre);

        [DllImport("BeefWrapCache.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool ReadCache(string cacheFolderFullName, string name, ref Int32 statusCode, StringBuilder statusText, StringBuilder strValue);

        [DllImport("BeefWrapCache.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool WriteCache(string cacheFolderFullName, string name, Int32 expireSeconds, Int32 statusCode, string statusText, string strValue);

        /// <summary>
        /// 清除本地缓存
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteCache(string name)
        {
            bool boolRtn = DeleteCache(CacheFolderFullName, name + CacheFileExtension);
            int error = Marshal.GetLastWin32Error();
            if (!boolRtn)
            {
                Console.WriteLine("DeleteCache error = " + error);
            }
            return boolRtn;
        }

        /// <summary>
        /// 清除所有缓存名字包含指定字符前缀的本地缓存
        /// 
        /// 主要用于删除存在分页机制的缓存
        /// </summary>
        /// <param namePre="namePre"></param>
        /// <returns></returns>
        public bool DeleteCacheLike(string namePre)
        {
            bool boolRtn = DeleteCacheLike(CacheFolderFullName, namePre);
            int error = Marshal.GetLastWin32Error();
            if (!boolRtn)
            {
                Console.WriteLine("DeleteCacheLike error = " + error);
            }
            return boolRtn;
        }

        /// <summary>
        /// 读缓存长度
        /// </summary>
        /// <param name="name">在缓存中的唯一标识</param>
        /// <param name="statusTextLength"></param>
        /// <param name="strValueLength"></param>
        /// <returns></returns>
        public bool ReadCacheLength(string name, out Int32 statusTextLength, out Int32 strValueLength)
        {
            statusTextLength = 0;
            strValueLength = 0;

            bool boolRtn = ReadCacheLength(CacheFolderFullName, name + CacheFileExtension, ref statusTextLength, ref strValueLength);
            int error = Marshal.GetLastWin32Error();
            if (!boolRtn)
            {
                Console.WriteLine("ReadCacheLength error = " + error);
            }
            return boolRtn;
        }

        /// <summary>
        /// 读缓存
        /// </summary>
        /// <param name="name">在缓存中的唯一标识</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool ReadCache(string name, out Result result)
        {
            result = new Result();

            Int32 statusCode = 0, statusTextLength, strValueLength;

            bool boolRtn = ReadCacheLength(name, out statusTextLength, out strValueLength);
            int error = Marshal.GetLastWin32Error();
            if (!boolRtn)
            {
                Console.WriteLine("ReadCache error1 = " + error);
                return false;
            }

            StringBuilder statusText = new StringBuilder();
            statusText.Capacity = statusTextLength;
            StringBuilder strValue = new StringBuilder();
            strValue.Capacity = strValueLength;

            boolRtn = ReadCache(CacheFolderFullName, name + CacheFileExtension, ref statusCode, statusText, strValue);
            error = Marshal.GetLastWin32Error();
            if (!boolRtn)
            {
                Console.WriteLine("ReadCache error2 = " + error);
            }

            result.StatusCode = statusCode;
            result.StatusText = statusText.ToString();
            result.Value = strValue.ToString();
            return boolRtn;
        }

        /// <summary>
        /// 写缓存
        /// 
        /// 如果要保存图像等二进制数据，可以用base64等转码成字符串后保存
        /// </summary>
        /// <param name="name">在缓存中的唯一标识</param>
        /// <param name="expireSeconds">缓存过期秒数</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool WriteCache(string name, Int32 expireSeconds, Result result)
        {
            if (expireSeconds <= 0)
            {
                return true;
            }
            string statusText = string.Format("{0}", result.StatusText);
            string strValue = string.Format("{0}", result.Value);

            bool boolRtn = WriteCache(CacheFolderFullName, name + CacheFileExtension, expireSeconds, result.StatusCode, statusText, strValue);

            int error = Marshal.GetLastWin32Error();
            if (!boolRtn)
            {
                Console.WriteLine("WriteCache error = " + error);
            }
            return boolRtn;
        }

        #endregion
    }
}
