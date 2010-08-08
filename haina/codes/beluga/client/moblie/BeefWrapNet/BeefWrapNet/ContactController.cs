using System;
using System.IO;
using System.Net;
using BeefWrap.Common.Net;

namespace BeefWrap.Net
{
    public class ContactController : AControl
    {
        public ContactController()
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
                return this.RequestHost + "/Contact.do?method=";
            }
        }

        #region UserIdCheck

        /// <summary>
        /// token
        /// </summary>
        private static string tokenUserIdCheck = null;

        /// <summary>
        /// UserIdCheck
        /// </summary>
        /// <param name="contactMobile"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool UserIdCheck(string contactMobile, out Result result)
        {
            string name = string.Format("{0}_UserIdCheck_{1}", Command.C_CONTACT_USERID_CHECK_REQUEST, contactMobile);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenUserIdCheck = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}UserIdCheck?contactMobile={1}", this.RequestAddress, contactMobile);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenUserIdCheck, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenUserIdCheck.Equals(token))
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

        #region Locate

        /// <summary>
        /// token
        /// </summary>
        private static string tokenLocate = null;

        /// <summary>
        /// Locate
        /// </summary>
        /// <param name="contactMobile"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Locate(string contactMobile, out Result result)
        {
            string name = string.Format("{0}_Locate_{1}", Command.C_CONTACT_LOCATE_REQUEST, contactMobile);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenLocate = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}Locate?contactMobile={1}", this.RequestAddress, contactMobile);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenLocate, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenLocate.Equals(token))
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

        #region Map

        /// <summary>
        /// token
        /// </summary>
        private static string tokenMap = null;

        /// <summary>
        /// Map
        /// </summary>
        /// <param name="contactMobile"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Map(string contactMobile, out Result result)
        {
            string name = string.Format("{0}_Map_{1}", Command.C_CONTACT_MAP_REQUEST, contactMobile);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenMap = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}Map?contactMobile={1}", this.RequestAddress, contactMobile);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenMap, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenMap.Equals(token))
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

        #region Sync

        /// <summary>
        /// token
        /// </summary>
        private static string tokenSync = null;

        /// <summary>
        /// Sync
        /// </summary>
        /// <param name="email"></param>
        /// <param name="contacts"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Sync(string email, string contacts, out Result result)
        {
            string name = string.Format("{0}_Sync_{1}_{2}", Command.C_CONTACT_SYNC_REQUEST, email, contacts);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenSync = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}Sync?email={1}&contacts={2}", this.RequestAddress, email, contacts);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenSync, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenSync.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);

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

        #region Signature

        /// <summary>
        /// token
        /// </summary>
        private static string tokenSignature = null;

        /// <summary>
        /// Signature
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Signature(string mobile, out Result result)
        {
            string name = string.Format("{0}_Signature_{1}", Command.C_CONTACT_SIGNATURE_REQUEST, mobile);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenSignature = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}Signature?mobile={1}", this.RequestAddress, mobile);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenSignature, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenSignature.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);

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

        #region Feed

        /// <summary>
        /// token
        /// </summary>
        private static string tokenFeed = null;

        /// <summary>
        /// Feed
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="curPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Feed(string mobile, int curPage, int pageSize, out Result result)
        {
            string nameBase = string.Format("{0}_Feed_{1}", Command.C_CONTACT_FEED_REQUEST, mobile);
            string name = string.Format("{0}_{1}_{2}", nameBase, curPage, pageSize);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenFeed = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}Feed?mobile={1}&curPage={2}&pageSize={3}", this.RequestAddress, mobile, curPage, pageSize);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenFeed, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenFeed.Equals(token))
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

        #region LatestUserMsg

        /// <summary>
        /// token
        /// </summary>
        private static string tokenLatestUserMsg = null;

        /// <summary>
        /// LatestUserMsg
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="curPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool LatestUserMsg(string mobile, int curPage, int pageSize, out Result result)
        {
            string nameBase = string.Format("{0}_LatestUserMsg_{1}", Command.C_CONTACT_LATEST_USERMSG_REQUEST, mobile);
            string name = string.Format("{0}_{1}_{2}", nameBase, curPage, pageSize);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenLatestUserMsg = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}LatestUserMsg?mobile={1}&curPage={2}&pageSize={3}", this.RequestAddress, mobile, curPage, pageSize);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenLatestUserMsg, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenLatestUserMsg.Equals(token))
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

        #region PageUserMsg

        /// <summary>
        /// token
        /// </summary>
        private static string tokenPageUserMsg = null;

        /// <summary>
        /// PageUserMsg
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="curPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool PageUserMsg(string mobile, int curPage, int pageSize, out Result result)
        {
            string nameBase = string.Format("{0}_PageUserMsg_{1}", Command.C_CONTACT_PAGE_USERMSG_REQUEST, mobile);
            string name = string.Format("{0}_{1}_{2}", nameBase, curPage, pageSize);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenPageUserMsg = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}PageUserMsg?mobile={1}&curPage={2}&pageSize={3}", this.RequestAddress, mobile, curPage, pageSize);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenPageUserMsg, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenPageUserMsg.Equals(token))
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

        #region Weather

        /// <summary>
        /// token
        /// </summary>
        private static string tokenWeather = null;

        /// <summary>
        /// Weather
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Weather(string mobile, out Result result)
        {
            string name = string.Format("{0}_Weather_{1}", Command.C_CONTACT_WEATHER_REQUEST, mobile);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenWeather = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}Weather?mobile={1}", this.RequestAddress, mobile);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenWeather, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenWeather.Equals(token))
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

        #region ContactsFeed

        /// <summary>
        /// token
        /// </summary>
        private static string tokenContactsFeed = null;

        /// <summary>
        /// ContactsFeed
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="curPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool ContactsFeed(string mobile, int curPage, int pageSize, out Result result)
        {
            string nameBase = string.Format("{0}_ContactsFeed_{1}", Command.C_CONTACTS_FEED_REQUEST, mobile);
            string name = string.Format("{0}_{1}_{2}", nameBase, curPage, pageSize);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenContactsFeed = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}ContactsFeed?mobile={1}&curPage={2}&pageSize={3}", this.RequestAddress, mobile, curPage, pageSize);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenContactsFeed, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenContactsFeed.Equals(token))
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

    }
}
