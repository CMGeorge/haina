using System;
using System.IO;
using System.Net;
using BeefWrap.Common.Net;

namespace BeefWrap.Net
{
    public class CommonController : AControl
    {
        public CommonController()
        {
        }

        public override string RequestAddress
        {
            get
            {
                if (string.IsNullOrEmpty(this.RequestHost))
                {
                    throw new Exception("服务器地址为空");
                }
                return this.RequestHost + "/Common.do?method=";
            }
        }

        #region HlrSync

        /// <summary>
        /// token
        /// </summary>
        private static string tokenHlrSync = null;

        /// <summary>
        /// HlrSync
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool HlrSync(string mobile, out Result result)
        {
            string name = string.Format("{0}_HlrSync_{1}", Command.C_COMMON_HLR_SYNC_REQUEST, mobile);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenHlrSync = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}HlrSync?mobile={1}", this.RequestAddress, mobile);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenHlrSync, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenHlrSync.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp);

                // 写缓存
                if (HttpStatusCode.OK.Equals(resultTmp.StatusCode))
                {
                    if (!WriteCache(name, ExpireSeconds, resultTmp))
                    {
                        DeleteCache(name);
                    }
                }
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion
    }
}
