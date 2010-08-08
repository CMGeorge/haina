using System;
using System.IO;
using System.Net;
using BeefWrap.Common.Net;

namespace BeefWrap.Net
{
    public class MsgController : AControl
    {
        public MsgController()
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
                return this.RequestHost + "/Msg.do?method=";
            }
        }

        #region UserMsgsDel

        /// <summary>
        /// token
        /// </summary>
        private static string tokenUserMsgsDel = null;

        /// <summary>
        /// UserMsgsDel
        /// </summary>
        /// <param name="usermsgIds"></param>
        /// <param name="srcEmail"></param>
        /// <param name="distEmail"></param>
        /// <param name="email"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool UserMsgsDel(string usermsgIds, string srcEmail, string distEmail, string email, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenUserMsgsDel = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}UserMsgsDel?usermsgIds={1}&srcEmail={2}&distEmail={3}&email={4}", this.RequestAddress, usermsgIds, srcEmail, distEmail, email);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenUserMsgsDel, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenUserMsgsDel.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region UserMsgsAdd

        /// <summary>
        /// token
        /// </summary>
        private static string tokenUserMsgsAdd = null;

        /// <summary>
        /// UserMsgsAdd
        /// </summary>
        /// <param name="usermsg"></param>
        /// <param name="srcEmail"></param>
        /// <param name="distEmail"></param>
        /// <param name="email"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool UserMsgsAdd(string usermsg, string srcEmail, string distEmail, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenUserMsgsAdd = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}UserMsgsAdd?usermsg={1}&srcEmail={2}&distEmail={3}", this.RequestAddress, usermsg, srcEmail, distEmail);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenUserMsgsAdd, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenUserMsgsAdd.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region UserMsgsReply

        /// <summary>
        /// token
        /// </summary>
        private static string tokenUserMsgsReply = null;

        /// <summary>
        /// UserMsgsReply
        /// </summary>
        /// <param name="usermsgId"></param>
        /// <param name="usermsgReplying"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool UserMsgsReply(string usermsgId, string usermsgReplying, out Result result)
        {
            string name = string.Format("{0}_UserMsgsReply_{1}_{2}", Command.C_USERMSG_REPLY_REQUEST, usermsgId, usermsgReplying);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenUserMsgsReply = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}UserMsgsReply?usermsgId={1}&usermsgReplying={2}", this.RequestAddress, usermsgId, usermsgReplying);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenUserMsgsReply, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenUserMsgsReply.Equals(token))
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

        #region SysAlertsReceive

        /// <summary>
        /// token
        /// </summary>
        private static string tokenSysAlertsReceive = null;

        /// <summary>
        /// SysAlertsReceive
        /// </summary>
        /// <param name="email"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool SysAlertsReceive(string email, out Result result)
        {
            string name = string.Format("{0}_SysAlertsReceive_{1}", Command.C_SYS_ALERTS_RECEIVE_REQUEST, email);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenSysAlertsReceive = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}SysAlertsReceive?email={1}", this.RequestAddress, email);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenSysAlertsReceive, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenSysAlertsReceive.Equals(token))
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

        #region SysAlertsDel

        /// <summary>
        /// token
        /// </summary>
        private static string tokenSysAlertsDel = null;

        /// <summary>
        /// SysAlertsDel
        /// </summary>
        /// <param name="sysAlertIds"></param>
        /// <param name="email"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool SysAlertsDel(string sysAlertIds, string email, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenSysAlertsDel = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}SysAlertsDel?sysAlertIds={1}&email={2}", this.RequestAddress, sysAlertIds, email);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenSysAlertsDel, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenSysAlertsDel.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region InnerMsgsReveive

        /// <summary>
        /// token
        /// </summary>
        private static string tokenInnerMsgsReceive = null;

        /// <summary>
        /// InnerMsgsReceive
        /// </summary>
        /// <param name="srcEmail"></param>
        /// <param name="distEmail"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool InnerMsgsReceive(string srcEmail, string distEmail, out Result result)
        {
            string name = string.Format("{0}_InnerMsgsReceive_{1}_{2}", Command.C_INNERMSGS_RECEIVE_REQUEST, srcEmail, distEmail);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenInnerMsgsReceive = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}InnerMsgsReceive?srcEmail={1}&distEmail={2}", this.RequestAddress, srcEmail, distEmail);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenInnerMsgsReceive, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenInnerMsgsReceive.Equals(token))
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

        #region InnerMsgSend

        /// <summary>
        /// token
        /// </summary>
        private static string tokenInnerMsgSend = null;

        /// <summary>
        /// InnerMsgSend
        /// </summary>
        /// <param name="innermsg"></param>
        /// <param name="srcEmail"></param>
        /// <param name="distEmail"></param>
        /// <param name="innermsgNo"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool InnerMsgSend(string innermsg, string srcEmail, string distEmail, string innermsgNo, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenInnerMsgSend = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}InnerMsgSend?innermsg={1}&srcEmail={2}&distEmail={3}&innermsgNo={4}", this.RequestAddress, innermsg, srcEmail, distEmail, innermsgNo);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenInnerMsgSend, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenInnerMsgSend.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region InnerMsgPicture

        /// <summary>
        /// token
        /// </summary>
        private static string tokenInnerMsgPictureSend = null;

        /// <summary>
        /// InnerMsgPictureSend
        /// </summary>
        /// <param name="innermsgPictures"></param>
        /// <param name="innermsgNo"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool InnerMsgPictureSend(string innermsgPictures, string innermsgNo, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenInnerMsgPictureSend = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}InnerMsgPictureSend?innermsgPictures={1}&innermsgNo={2}", this.RequestAddress, innermsgPictures, innermsgNo);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenInnerMsgPictureSend, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenInnerMsgPictureSend.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp, false);
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
