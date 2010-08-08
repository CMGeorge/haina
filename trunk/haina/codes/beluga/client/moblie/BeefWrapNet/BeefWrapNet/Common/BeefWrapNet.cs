using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Collections;

namespace BeefWrap.Common.Net
{
    /// <summary>
    /// 请求之前回调，如果出现异常会回传消息
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="request"></param>
    public delegate void RequestPreCallBack(string token, HttpWebRequest request);
    /// <summary>
    /// 输入流回调，如果出现异常会回传消息
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="stream">输入流</param>
    public delegate void RequestWriteCallBack(string token, ref Stream stream);
    /// <summary>
    /// 输出流回调，如果出现异常会回传消息
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="stream">输出流</param>
    /// <param name="response"></param>
    public delegate void ResponseReadCallBack(string token, ref Stream stream, HttpWebResponse response);
    /// <summary>
    /// 回传消息，如果回传失败会打印到控制台
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="message"></param>
    /// <param name="code"></param>
    public delegate void MessageCallBack(string token, string message, int code);

    /// <summary>
    /// 自定义封装的Http请求类
    /// </summary>
    public class BeefWrapNet
    {
        public readonly string Post = "POST";
        public readonly string Get = "GET";

        /// <summary>
        /// 提交同步Post请求
        /// 具体参数参考：HttpRequest 方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="token"></param>
        /// <param name="requestPre"></param>
        /// <param name="requestWrite"></param>
        /// <param name="responseRead"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool HttpPostRequest(string url, string token, RequestPreCallBack requestPre, RequestWriteCallBack requestWrite, ResponseReadCallBack responseRead, MessageCallBack message)
        {
            return HttpRequest(url, token, Post, -1, requestPre, requestWrite, responseRead, message);
        }
        /// <summary>
        /// 提交同步Post请求
        /// 具体参数参考：HttpRequest 方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="token"></param>
        /// <param name="contentLength"></param>
        /// <param name="requestPre"></param>
        /// <param name="requestWrite"></param>
        /// <param name="responseRead"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool HttpPostRequest(string url, string token, int contentLength, RequestPreCallBack requestPre, RequestWriteCallBack requestWrite, ResponseReadCallBack responseRead, MessageCallBack message)
        {
            return HttpRequest(url, token, Post, contentLength, requestPre, requestWrite, responseRead, message);
        }

        /// <summary>
        /// 提交异步Post请求
        /// 具体参数参考：HttpRequest 方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="token"></param>
        /// <param name="requestPre"></param>
        /// <param name="requestWrite"></param>
        /// <param name="responseRead"></param>
        /// <param name="message"></param>
        public void HttpPostRequestAsync(string url, string token, RequestPreCallBack requestPre, RequestWriteCallBack requestWrite, ResponseReadCallBack responseRead, MessageCallBack message)
        {
            new Thread(new ThreadStart(
                delegate
                {
                    HttpPostRequest(url, token, requestPre, requestWrite, responseRead, message);
                }
                )).Start();
        }

        /// <summary>
        /// 提交同步Get请求
        /// 具体参数参考：HttpRequest 方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="token"></param>
        /// <param name="requestPre"></param>
        /// <param name="requestWrite"></param>
        /// <param name="responseRead"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool HttpGetRequest(string url, string token, RequestPreCallBack requestPre, RequestWriteCallBack requestWrite, ResponseReadCallBack responseRead, MessageCallBack message)
        {
            return HttpRequest(url, token, Get, -1, requestPre, requestWrite, responseRead, message);
        }

        /// <summary>
        /// 提交同步Get请求
        /// 具体参数参考：HttpRequest 方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="token"></param>
        /// <param name="contentLength"></param>
        /// <param name="rquestPre"></param>
        /// <param name="requestWrite"></param>
        /// <param name="responseRead"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool HttpGetRequest(string url, string token, int contentLength, RequestPreCallBack rquestPre, RequestWriteCallBack requestWrite, ResponseReadCallBack responseRead, MessageCallBack message)
        {
            return HttpRequest(url, token, Get, contentLength, rquestPre, requestWrite, responseRead, message);
        }

        /// <summary>
        /// 提交异步Get请求
        /// 具体参数参考：HttpRequest 方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="token">令牌</param>
        /// <param name="requestPre"></param>
        /// <param name="requestWrite"></param>
        /// <param name="responseRead"></param>
        /// <param name="message"></param>
        public void HttpGetRequestAsync(string url, string token, RequestPreCallBack requestPre, RequestWriteCallBack requestWrite, ResponseReadCallBack responseRead, MessageCallBack message)
        {
            new Thread(new ThreadStart(
                delegate
                {
                    HttpGetRequest(url, token, requestPre, requestWrite, responseRead, message);
                }
                )).Start();
        }

        /// <summary>
        /// 回传消息，如果回传失败会打印到控制台
        /// </summary>
        /// <param name="messageCallBack"></param>
        /// <param name="token"></param>
        /// <param name="message"></param>
        /// <param name="code"></param>
        private void SetMessage(MessageCallBack messageCallBack, string token, string message, int code)
        {
            if (null != message)
            {
                try
                {
                    messageCallBack(token, message, code);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(string.Format("token:{0},code:{1},message:{2},exception:{3}", token, code, message, ex.Message));
                }
            }
            Console.WriteLine(string.Format("token:{0},code:{1},message:{2}", token, code, message));
        }

        /// <summary>
        /// 存储Cookie
        /// </summary>
        private static Hashtable Cookies = new Hashtable();

        /// <summary>
        /// 读取内存Cookies
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static string GetCookies(string host)
        {
            return null;
            if (string.IsNullOrEmpty(host)
                || null == Cookies
                || !Cookies.ContainsKey(host)
                || null == Cookies[host])
            {
                return null;
            }
            Hashtable cookie = (Hashtable)Cookies[host];
            StringBuilder sb = new StringBuilder();
            foreach (string key in cookie.Keys)
            {
                if (string.IsNullOrEmpty(key)
                    || string.IsNullOrEmpty(cookie[key].ToString()))
                {
                    continue;
                }
                sb.AppendFormat(null, "{0}={1};", new object[] { key, cookie[key] });
            }
            return sb.ToString();
        }


        private static void SetCookies(string host, string[] arrayCookies)
        {
            return;
            if (string.IsNullOrEmpty(host)
                || null == arrayCookies)
            {
                return;
            }
            foreach (string cookie in arrayCookies)
            {
                if (string.IsNullOrEmpty(cookie))
                {
                    continue;
                }
                string[] strs = cookie.Split(';');
                if (null == Cookies)
                {
                    Cookies = new Hashtable();
                }
                Hashtable hashCookie = null;
                if (Cookies.ContainsKey(host))
                {
                    hashCookie = (Hashtable)Cookies[host];
                }
                else
                {
                    hashCookie = new Hashtable();
                }
                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str) || str.IndexOf("=") == -1)
                    {
                        continue;
                    }
                    string key = str.Substring(0, str.IndexOf("="));
                    string value = str.Substring(str.IndexOf("=") + 1);
                    hashCookie[key] = value;
                }
                Cookies[host] = hashCookie;
            }
        }

        /// <summary>
        /// 进行Http交互
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="token">令牌</param>
        /// <param name="method">请求方式：POST、GET</param>
        /// <param name="contentLength">当为 -1 时，不设定 ContentLength</param>
        /// <param name="requestPre">请求之前回调</param>
        /// <param name="requestWrite">输入流回调</param>
        /// <param name="responseRead">输出流回调</param>
        /// <param name="message">消息回调</param>
        /// <returns></returns>
        private bool HttpRequest(string url, string token, string method, long contentLength, RequestPreCallBack requestPre, RequestWriteCallBack requestWrite, ResponseReadCallBack responseRead, MessageCallBack message)
        {
            if (string.IsNullOrEmpty(url))
            {
                this.SetMessage(message, token, "请求的地址为空", 0);
                return false;
            }
            HttpWebRequest request = null;
            Stream streamRequest = null;
            HttpWebResponse response = null;
            Stream streamResponse = null;

            string http = "http://";
            string host = url;
            string port = "80";
            string path = url;
            if (url.ToLower().StartsWith("http"))
            {
                http = url.Substring(0, url.IndexOf(':')) + "://";
                host = url.Substring(url.IndexOf(':') + 3);
                path = host;
            }
            int pos = -1;
            if ((pos = host.IndexOf('/')) != -1)
            {
                host = host.Substring(0, pos);
                path = path.Substring(pos);
            }
            else if ((pos = host.IndexOf('?')) != -1)
            {
                host = host.Substring(0, pos);
                path = path.Substring(pos);
            }
            else
            {
                path = "";
            }
            if ((pos = host.IndexOf(":")) != -1)
            {
                port = host.Substring(pos + 1);
                host = host.Substring(0, pos);
            }

            try
            {
                this.SetMessage(message, token, "正在连接网络...", 1);

                request = (HttpWebRequest)WebRequest.Create(url);
                request.ProtocolVersion = HttpVersion.Version11;
                request.Accept = "*/*";
                request.Headers.Add("Accept-Language", "zh-cn");
                request.Headers.Add("UA-OS", "Windows CE (Pocket PC)");
                request.Referer = null;
                request.Headers.Add("Accept-Encoding", "gzip,deflate");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows CE; IEMobile 6.12) - Beluga BeefWrapNet 1.0";
                request.KeepAlive = true;              

                request.AllowAutoRedirect = true;
                request.MaximumAutomaticRedirections = 3;
                // request.Credentials = CredentialCache.DefaultCredentials;
                // request.ContentType = "text/html";
                // request.Headers.Add("Pragma", "no-cache");
                // request.Headers.Add("cache-control", "no-store, no-cache, must-revalidate, post-check=0, pre-check=0");
                request.Timeout = 30 * 1000; // 超时时间 60 秒

                string cookies = GetCookies(host + ":" + port);
                if (!string.IsNullOrEmpty(cookies))
                {
                    request.Headers.Add("Cookie", cookies);
                }

                if (string.IsNullOrEmpty(method)
                    || string.IsNullOrEmpty(method.Trim()))
                {
                    request.Method = Post;
                }
                else
                {
                    request.Method = method;
                }
                if (!string.IsNullOrEmpty(request.Method)
                    && request.Method.Trim().ToLower().Equals(Post.ToLower()))
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                    if (contentLength >= 0)
                    {
                        request.ContentLength = contentLength;
                        request.SendChunked = false;
                    }
                    else
                    {
                        request.SendChunked = true;
                    }
                }
                if (null != requestPre)
                {
                    requestPre(token, request);
                }

                this.SetMessage(message, token, "正在连接服务器...", 2);
                // 发送数据
                if (null != requestWrite)
                {
                    streamRequest = request.GetRequestStream();
                    this.SetMessage(message, token, "连接成功，正在请求数据...", 3);
                    requestWrite(token, ref streamRequest);
                    streamRequest.Flush();
                    streamRequest.Close();
                    streamRequest = null;
                }

                // 接收数据
                response = (HttpWebResponse)request.GetResponse();

                SetCookies(host, response.Headers.GetValues("Set-Cookie"));

                if (null != responseRead)
                {
                    this.SetMessage(message, token, "已收到数据：", 4);
                    streamResponse = response.GetResponseStream();
                    responseRead(token, ref streamResponse, response);
                }
                this.SetMessage(message, token, "完成", 0);
                return true;
            }
            catch (Exception ex)
            {
                this.SetMessage(message, token, "网络故障：" + ex.Message, -1);
            }
            finally
            {
                if (null != response)
                {
                    try { response.Close(); }
                    catch { }
                    response = null;
                }
                if (null != request)
                {
                    try { request.Abort(); }
                    catch { }
                    request = null;
                }
            }
            return false;
        }
    }
}
