using System;
using System.IO;
using System.Net;
using BeefWrap.Common.Net;

namespace BeefWrap.Net
{
    public class UserController : AControl
    {
        public UserController()
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
                return this.RequestHost + "/User.do?method=";
            }
        }

        #region login

        /// <summary>
        /// token
        /// </summary>
        private static string tokenLogin = null;

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <param name="expires"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Login(string email, string pwd, int expires, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenLogin = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}Login?email={1}&pwd={2}&expires={3}", this.RequestAddress, email, pwd, expires);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenLogin, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenLogin.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region logout

        /// <summary>
        /// token
        /// </summary>
        private static string tokenLogout = null;

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="email"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Logout(string email, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenLogout = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}Logout?email={1}", this.RequestAddress, email);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenLogout, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenLogout.Equals(token))
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

        #region register

        /// <summary>
        /// token
        /// </summary>
        private static string tokenRegister = null;

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="imei"></param>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Register(string mobile, string imei, string email, string pwd, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenRegister = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}Register?mobile={1}&imei={2}&email={3}&pwd={4}", this.RequestAddress, mobile, imei, email, pwd);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenRegister, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenRegister.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region Exist

        /// <summary>
        /// token
        /// </summary>
        private static string tokenExist = null;

        /// <summary>
        /// Exist
        /// </summary>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Exist(string email, string mobile, out Result result)
        {
            // 读缓存
            string name = string.Format("{0}_Exist_{1}_{2}", Command.C_USER_EXIST_CHECK_REQUEST, email, mobile);
            if (ReadCache(name, out result))
            {
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenExist = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}Exist?email={1}&mobile={2}", this.RequestAddress, email, mobile);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenExist, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenExist.Equals(token))
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

            result = resultTmp;
            if (null == result)
            {
                result = new Result();
            }
            return boolRtn;
        }

        #endregion

        #region PWDUpdate

        /// <summary>
        /// token
        /// </summary>
        private static string tokenPWDUpdate = null;

        /// <summary>
        /// PWDUpdate
        /// </summary>
        /// <param name="email"></param>
        /// <param name="oldPwd"></param>
        /// <param name="pwd"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool PWDUpdate(string email, string oldPwd, string pwd, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenPWDUpdate = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}PWDUpdate?email={1}&oldPwd={2}&pwd={3}", this.RequestAddress, email, oldPwd, pwd);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenPWDUpdate, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenPWDUpdate.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region PwdRetrieve

        /// <summary>
        /// token
        /// </summary>
        private static string tokenPwdRetrieve = null;

        /// <summary>
        /// PwdRetrieve
        /// </summary>
        /// <param name="email"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool PwdRetrieve(string email, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenPwdRetrieve = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}PwdRetrieve?email={1}", this.RequestAddress, email);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenPwdRetrieve, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenPwdRetrieve.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region EmailUpdate

        /// <summary>
        /// token
        /// </summary>
        private static string tokenEmailUpdate = null;

        /// <summary>
        /// EmailUpdate
        /// </summary>
        /// <param name="oldEmail"></param>
        /// <param name="email"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool EmailUpdate(string oldEmail, string email, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenEmailUpdate = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}EmailUpdate?oldEmail={1}&email={2}", this.RequestAddress, oldEmail, email);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenEmailUpdate, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenEmailUpdate.Equals(token))
                {
                    throw new Exception("令牌过期，取消操作");
                }

                // 读基本类型
                this.ReadResult(ref stream, out resultTmp);
            }), null);

            if (null == resultTmp)
            {
                resultTmp = new Result();
            }
            result = resultTmp;
            return boolRtn;
        }

        #endregion

        #region EmailRetrieve

        /// <summary>
        /// token
        /// </summary>
        private static string tokenEmailRetrieve = null;

        /// <summary>
        /// EmailRetrieve
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="pwd"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool EmailRetrieve(string mobile, string pwd, out Result result)
        {
            string name = string.Format("{0}_EmailRetrieve_{1}", Command.C_USER_LOCATE_SYNC_REQUEST, mobile);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenEmailRetrieve = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}EmailRetrieve?mobile={1}&pwd={2}", this.RequestAddress, mobile, pwd);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenEmailRetrieve, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenEmailRetrieve.Equals(token))
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

        #region MobileUpdate

        /// <summary>
        /// token
        /// </summary>
        private static string tokenMobileUpdate = null;

        /// <summary>
        /// MobileUpdate
        /// </summary>
        /// <param name="email"></param>
        /// <param name="oldMobile"></param>
        /// <param name="mobile"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool MobileUpdate(string email, string oldMobile, string mobile, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenMobileUpdate = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}MobileUpdate?email={1}&oldMobile={2}&mobile={3}", this.RequestAddress, email, oldMobile, mobile);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenMobileUpdate, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenMobileUpdate.Equals(token))
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

        #region ImeiUpdate

        /// <summary>
        /// token
        /// </summary>
        private static string tokenImeiUpdate = null;

        /// <summary>
        /// ImeiUpdate
        /// </summary>
        /// <param name="email"></param>
        /// <param name="oldImei"></param>
        /// <param name="imei"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool ImeiUpdate(string email, string oldImei, string imei, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenImeiUpdate = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}ImeiUpdate?email={1}&oldImei={2}&imei={3}", this.RequestAddress, email, oldImei, imei);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenImeiUpdate, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenImeiUpdate.Equals(token))
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

        #region InfoUpdate

        /// <summary>
        /// token
        /// </summary>
        private static string tokenInfoUpdate = null;

        /// <summary>
        /// InfoUpdate
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="age"></param>
        /// <param name="nickname"></param>
        /// <param name="homePageUrl"></param>
        /// <param name="note"></param>
        /// <param name="contryId"></param>
        /// <param name="privenceId"></param>
        /// <param name="cityId"></param>
        /// <param name="districtId"></param>
        /// <param name="streetId"></param>
        /// <param name="photo"></param>
        /// <param name="homeTel"></param>
        /// <param name="workTel"></param>
        /// <param name="otherTel"></param>
        /// <param name="fax"></param>
        /// <param name="otherEmail"></param>
        /// <param name="homeArea"></param>
        /// <param name="workArea"></param>
        /// <param name="otherArea"></param>
        /// <param name="qq"></param>
        /// <param name="msn"></param>
        /// <param name="otherIm"></param>
        /// <param name="brithday"></param>
        /// <param name="org"></param>
        /// <param name="title"></param>
        /// <param name="ring"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool InfoUpdate(string sex, int age, string nickname, string homePageUrl, string note, string contryId, string privenceId, string cityId, string districtId, string streetId, string photo, string homeTel, string workTel, string otherTel, string fax, string otherEmail, string homeArea, string workArea, string otherArea, string qq, string msn, string otherIm, string brithday, string org, string title, string ring, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenInfoUpdate = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}InfoUpdate?sex={1}&age={2}&nickname={3}&homePageUrl={4}&note={5}&contryId={6}&privenceId={7}&cityId={8}&districtId={9}&streetId={10}&photo={11}&homeTel={13}&workTel={14}&otherTel={15}&fax={16}&otherEmail={17}&&homeArea={18}&workArea={19}&otherArea={20}&qq={21}&msn={22}&otherIm={23}&brithday={24}&org={25}&title={26}&ring={27}", this.RequestAddress, sex, age, nickname, homePageUrl, note, contryId, privenceId, cityId, districtId, streetId, photo, homeTel, workTel, otherTel, fax, otherEmail, homeArea, workArea, otherArea, qq, msn, otherIm, brithday, org, title, ring);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenInfoUpdate, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenInfoUpdate.Equals(token))
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

        #region SignatureUpdate

        /// <summary>
        /// token
        /// </summary>
        private static string tokenSignatureUpdate = null;

        /// <summary>
        /// SignatureUpdate
        /// </summary>
        /// <param name="email"></param>
        /// <param name="signature"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool SignatureUpdate(string email, string signature, out Result result)
        {
            BeefWrapNet net = new BeefWrapNet();
            tokenSignatureUpdate = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}SignatureUpdate?email={1}&signature={2}", this.RequestAddress, email, signature);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenSignatureUpdate, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenSignatureUpdate.Equals(token))
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

        #region PageMsgboard

        /// <summary>
        /// token
        /// </summary>
        private static string tokenPageMsgboard = null;

        /// <summary>
        /// PageMsgboard
        /// </summary>
        /// <param name="email"></param>
        /// <param name="curPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool PageMsgboard(string email, int curPage, int pageSize, out Result result)
        {
            string nameBase = string.Format("{0}_PageMsgboard_{1}", Command.C_USER_PAGE_MSGBOARD_REQUEST, email);
            string name = string.Format("{0}_{1}_{2}", nameBase, curPage, pageSize);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenPageMsgboard = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}PageMsgboard?email={1}&curPage={2}&pageSize={3}", this.RequestAddress, email, curPage, pageSize);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenPageMsgboard, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenPageMsgboard.Equals(token))
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

        #region Feed

        /// <summary>
        /// token
        /// </summary>
        private static string tokenFeed = null;

        /// <summary>
        /// Feed
        /// </summary>
        /// <param name="email"></param>
        /// <param name="curPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Feed(string email, int curPage, int pageSize, out Result result)
        {
            string nameBase = string.Format("{0}_Feed_{1}", Command.C_USER_FEED_REQUEST, email);
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
            string url = string.Format("{0}Feed?email={1}&curPage={2}&pageSize={3}", this.RequestAddress, email, curPage, pageSize);
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
            string name = string.Format("{0}_Weather_{1}", Command.C_USER_WEATHER_REQUEST, mobile);
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

        #region WeekWeather

        /// <summary>
        /// token
        /// </summary>
        private static string tokenWeekWeather = null;

        /// <summary>
        /// WeekWeather
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool WeekWeather(string mobile, out Result result)
        {
            string name = string.Format("{0}_WeekWeather_{1}", Command.C_USER_WEEK_WEATHER_REQUEST, mobile);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenWeekWeather = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}WeekWeather?mobile={1}", this.RequestAddress, mobile);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenWeekWeather, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenWeekWeather.Equals(token))
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

        #region LocateSync

        /// <summary>
        /// token
        /// </summary>
        private static string tokenLocateSync = null;

        /// <summary>
        /// LocateSync
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="mcc"></param>
        /// <param name="mnc"></param>
        /// <param name="lac"></param>
        /// <param name="cid"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool LocateSync(string mobile, string mcc, string mnc, string lac, string cid, out Result result)
        {
            string name = string.Format("{0}_LocateSync_{1}_{2}_{3}_{4}_{5}", Command.C_USER_LOCATE_SYNC_REQUEST, mobile, mcc, mnc, lac, cid);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenLocateSync = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}LocateSync?mobile={1}&mcc={2}&mnc={3}&lac={4}&cid={5}", this.RequestAddress, mobile, mcc, mnc, lac, cid);
            Result resultTmp = null;

            bool boolRtn = net.HttpGetRequest(url, tokenLocateSync, null, null, new ResponseReadCallBack(delegate(string token, ref Stream stream, HttpWebResponse response)
            {
                if (!tokenLocateSync.Equals(token))
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

        #region Locate

        /// <summary>
        /// token
        /// </summary>
        private static string tokenLocate = null;

        /// <summary>
        /// Locate
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="mcc"></param>
        /// <param name="mnc"></param>
        /// <param name="lac"></param>
        /// <param name="cid"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Locate(string mobile, string mcc, string mnc, string lac, string cid, out Result result)
        {
            string name = string.Format("{0}_Locate_{1}_{2}_{3}_{4}_{5}", Command.C_USER_LOCATE_SYNC_REQUEST, mobile, mcc, mnc, lac, cid);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenLocate = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}Locate?mobile={1}&mcc={2}&mnc={3}&lac={4}&cid={5}", this.RequestAddress, mobile, mcc, mnc, lac, cid);
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
        /// <param name="mobile"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Map(string mobile, out Result result)
        {
            string name = string.Format("{0}_Map_{1}", Command.C_USER_MAP_REQUEST, mobile);
            // 读取缓存
            if (ReadCache(name, out result))
            {
                result = new Result();
                return true;
            }

            BeefWrapNet net = new BeefWrapNet();
            tokenMap = NetUtility.GetNextToken();

            // 同步请求
            string url = string.Format("{0}Map?mobile={1}", this.RequestAddress, mobile);
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

    }
}
