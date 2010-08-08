using System;
using System.IO;
using System.Net;
using BeefWrap.Common.Net;

namespace BeefWrap.Net
{
    public class AppController : AControl
    {
        public AppController()
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
                return this.RequestHost + "/App.do?method=";
            }
        }

        #region AppCatalog

        /// <summary>
        /// token
        /// </summary>
        private static string tokenAppCatalog = null;

        /// <summary>
        /// AppCatalog
        /// </summary>
        /// <param name="appCatalogParentId"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool AppCatalog(string appCatalogParentId, out Result result)
        {
            string name = string.Format("{0}_AppCatalog_{1}", Command.C_APP_CATALOG_REQUEST, appCatalogParentId);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenAppCatalog = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}AppCatalog?appCatalogParentId={1}", this.RequestAddress, appCatalogParentId);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenAppCatalog, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenAppCatalog.Equals(token))
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

        #region AppList

        /// <summary>
        /// token
        /// </summary>
        private static string tokenAppList = null;

        /// <summary>
        /// AppList
        /// </summary>
        /// <param name="appCatalogId"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool AppList(string appCatalogId, out Result result)
        {
            string name = string.Format("{0}_AppList_{1}", Command.C_APP_LIST_REQUEST, appCatalogId);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenAppList = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}AppList?appCatalogId={1}", this.RequestAddress, appCatalogId);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenAppList, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenAppList.Equals(token))
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

        #region AppHotList

        /// <summary>
        /// token
        /// </summary>
        private static string tokenAppHotList = null;

        /// <summary>
        /// AppHotList
        /// </summary>
        /// <param name="curPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool AppHotList(int curPage, int pageSize, out Result result)
        {
            string nameBase = string.Format("{0}_AppHotList", Command.C_APP_HOT_LIST_REQUEST);
            string name = string.Format("{0}_{1}_{2}", nameBase, curPage, pageSize);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenAppHotList = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}AppHotList?curPage={1}&pageSize={2}", this.RequestAddress, curPage, pageSize);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenAppHotList, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenAppHotList.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp);

                // 写缓存
                if (HttpStatusCode.OK.Equals(resultTmp.StatusCode))
                {
                    if (curPage == 1)
                    {
                        DeleteCacheLike(nameBase);
                    }
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

        #region AppLatestList

        /// <summary>
        /// token
        /// </summary>
        private static string tokenAppLatestList = null;

        /// <summary>
        /// AppLatestList
        /// </summary>
        /// <param name="createTime"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool AppLatestList(string createTime, out Result result)
        {
            string name = string.Format("{0}_AppLatestList_{1}", Command.C_APP_LATEST_LIST_REQUEST, createTime);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenAppLatestList = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}AppLatestList?createTime={1}", this.RequestAddress, createTime);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenAppLatestList, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenAppLatestList.Equals(token))
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
