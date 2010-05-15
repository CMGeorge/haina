using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Net;
using BeefWrap.Net;

namespace BeefWrap
{
    public partial class FrmMain : Form
    {
        public static readonly string host = "jackyho-acfd27d";
        string url = "http://" + host + ":8120/BeefWrap";

        bool boolValue = true;
        float floatValue = 111100.222255F;
        int intValue = 333399;
        long longValue = 444455L;
        string strValue = "你好！";
        double doubleValue = 666600.777788D;

        #region

        public FrmMain()
        {
            InitializeComponent();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }

        private delegate void DelegateLog(object str);
        private void Log(object obj)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateLog(this.Log), new object[] { obj });
                return;
            }
            if (null == obj)
            {
                this.listBoxResult.Items.Add("null");
                return;
            }
            this.listBoxResult.Items.Add(obj.ToString());
            Application.DoEvents();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.listBoxResult.Items.Clear();
        }

        private delegate void DelegateProgress(int value);
        private void UpdateProgress(int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateProgress(this.UpdateProgress), new object[] { value });
                return;
            }
            this.progressBar1.Value = value;
            Application.DoEvents();
        }

        #endregion

        #region 同步请求
        string tokenRW = "";// 标识请求的唯一令牌

        private void btnTestSync_Click(object sender, EventArgs e)
        {
            this.btnTestSync.Enabled = false;
            try
            {
                BeefWrapNet net = new BeefWrapNet();
                this.tokenRW = NetUtility.GetNextToken();
                // 同步请求
                net.HttpPostRequest(url + @"/app", this.tokenRW, new RequestPreCallBack(RequestPreRW), new RequestWriteCallBack(RequestWriteRW), new ResponseReadCallBack(ResponseReadRW), new MessageCallBack(MessageRW));
            }
            catch { }
            this.btnTestSync.Enabled = true;
        }

        private void RequestPreRW(string token, HttpWebRequest request)
        {
            if (!this.tokenRW.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 设置请求头
            request.ContentLength = NetUtility.GetLength(boolValue)
                + NetUtility.GetLength(floatValue)
                + NetUtility.GetLength(intValue)
                + NetUtility.GetLength(longValue)
                + NetUtility.GetLength(strValue)
                + NetUtility.GetLength(doubleValue);
        }

        private void RequestWriteRW(string token, ref Stream stream)
        {
            if (!this.tokenRW.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 写基本类型
            this.Log("写入：" + NetWriter.WriteBool(ref stream, boolValue));
            this.Log("写入：" + NetWriter.WriteFloat(ref stream, floatValue));
            this.Log("写入：" + NetWriter.WriteInt(ref stream, intValue));
            this.Log("写入：" + NetWriter.WriteLong(ref stream, longValue));
            this.Log("写入：" + NetWriter.WriteUTF(ref stream, strValue));
            this.Log("写入：" + NetWriter.WriteDouble(ref stream, doubleValue));
        }

        private void ResponseReadRW(string token, ref Stream stream, HttpWebResponse response)
        {
            if (!this.tokenRW.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 读基本类型
            this.Log("读取：" + NetReader.ReadBool(ref stream));
            this.Log("读取：" + NetReader.ReadFloat(ref stream));
            this.Log("读取：" + NetReader.ReadInt(ref stream));
            this.Log("读取：" + NetReader.ReadLong(ref stream));
            this.Log("读取：" + NetReader.ReadUTF(ref stream));
            this.Log("读取：" + NetReader.ReadDouble(ref stream));
        }

        private void MessageRW(string token, string message, int code)
        {
            if (!this.tokenRW.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 请求状态信息
            this.Log("code = " + code + ",message = " + message);
        }

        #endregion

        #region 异步请求

        string tokenRWAsync = "";// 标识请求的唯一令牌

        private void btnTestASync_Click(object sender, EventArgs e)
        {
            try
            {
                BeefWrapNet net = new BeefWrapNet();
                this.tokenRWAsync = NetUtility.GetNextToken();
                // 异步请求
                net.HttpPostRequestAsync(url + @"/app", this.tokenRWAsync, new RequestPreCallBack(RequestPreRWAsync), new RequestWriteCallBack(RequestWriteRWAsync), new ResponseReadCallBack(ResponseReadRWAsync), new MessageCallBack(MessageRWAsync));
            }
            catch { }
        }

        private void RequestPreRWAsync(string token, HttpWebRequest request)
        {
            if (!this.tokenRWAsync.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 设置请求头
            request.ContentLength = NetUtility.GetLength(boolValue)
                + NetUtility.GetLength(floatValue)
                + NetUtility.GetLength(intValue)
                + NetUtility.GetLength(longValue)
                + NetUtility.GetLength(strValue)
                + NetUtility.GetLength(doubleValue);
        }

        private void RequestWriteRWAsync(string token, ref Stream stream)
        {
            if (!this.tokenRWAsync.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 写基本类型
            this.Log("写入：" + NetWriter.WriteBool(ref stream, boolValue));
            this.Log("写入：" + NetWriter.WriteFloat(ref stream, floatValue));
            this.Log("写入：" + NetWriter.WriteInt(ref stream, intValue));
            this.Log("写入：" + NetWriter.WriteLong(ref stream, longValue));
            this.Log("写入：" + NetWriter.WriteUTF(ref stream, strValue));
            this.Log("写入：" + NetWriter.WriteDouble(ref stream, doubleValue));
        }

        private void ResponseReadRWAsync(string token, ref Stream stream, HttpWebResponse response)
        {
            if (!this.tokenRWAsync.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 读基本类型
            this.Log("读取：" + NetReader.ReadBool(ref stream));
            this.Log("读取：" + NetReader.ReadFloat(ref stream));
            this.Log("读取：" + NetReader.ReadInt(ref stream));
            this.Log("读取：" + NetReader.ReadLong(ref stream));
            this.Log("读取：" + NetReader.ReadUTF(ref stream));
            this.Log("读取：" + NetReader.ReadDouble(ref stream));
        }

        private void MessageRWAsync(string token, string message, int code)
        {
            if (!this.tokenRWAsync.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 请求状态信息
            this.Log("code = " + code + ",message = " + message);
        }

        #endregion

        #region 下载

        string picUrl = "http://" + host + ":8120/BeefWrap/down.jpg";
        string tokenDownLoad = "";// 标识下载的唯一令牌

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            this.btnDownLoad.Enabled = false;
            this.progressBar1.Value = 0;
            try
            {
                BeefWrapNet net = new BeefWrapNet();
                this.tokenDownLoad = NetUtility.GetNextToken();
                // 异步下载
                net.HttpGetRequestAsync(picUrl, this.tokenDownLoad, null, null, new ResponseReadCallBack(ResponseReadDownLoad), new MessageCallBack(MessageDownLoad));
            }
            catch { }
            this.btnDownLoad.Enabled = true;
        }

        private void RequestPreDownLoad(string token, HttpWebRequest request)
        {
            if (!this.tokenDownLoad.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 设置请求头
            request.ContentLength = NetUtility.GetLength(boolValue)
                + NetUtility.GetLength(floatValue)
                + NetUtility.GetLength(intValue)
                + NetUtility.GetLength(longValue)
                + NetUtility.GetLength(strValue)
                + NetUtility.GetLength(doubleValue);
        }

        private void ResponseReadDownLoad(string token, ref Stream stream, HttpWebResponse response)
        {
            if (!this.tokenDownLoad.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 文件下载
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            byte[] bytes = new byte[1024];
            long pos = 0;
            FileStream fs = new FileStream(path + @"\" + picUrl.Substring(picUrl.LastIndexOf("/") + 1), FileMode.Create, FileAccess.Write);
            this.Log("开始下载...");
            while (true)
            {
                if (!this.tokenDownLoad.Equals(token))
                {
                    throw new Exception("请求已失效");
                }
                int len = stream.Read(bytes, 0, bytes.Length);
                if (len <= 0)
                {
                    break;
                }
                pos += len;
                fs.Write(bytes, 0, len);
                fs.Flush();
                this.UpdateProgress((int)((pos * 100) / response.ContentLength));
            }
            fs.Close();
        }

        private void MessageDownLoad(string token, string message, int code)
        {
            if (!this.tokenDownLoad.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 请求状态信息
            this.Log("code = " + code + ",message = " + message);
        }

        #endregion

        #region 上传文件

        string upUrl = "http://" + host + ":8120/BeefWrap/up";
        string tokenUp = "";// 标识下载的唯一令牌

        private void btnUpFile_Click(object sender, EventArgs e)
        {
            this.btnUpFile.Enabled = false;
            this.progressBar1.Value = 0;
            try
            {
                BeefWrapNet net = new BeefWrapNet();
                this.tokenUp = NetUtility.GetNextToken();
                // 异步上传
                net.HttpPostRequestAsync(upUrl, this.tokenUp, new RequestPreCallBack(RequestPreUp), new RequestWriteCallBack(RequestWriteUp), null, new MessageCallBack(MessageUp));
            }
            catch { }
            this.btnUpFile.Enabled = true;
        }

        private void RequestPreUp(string token, HttpWebRequest request)
        {
            if (!this.tokenUp.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // TODO:设置请求头
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            FileInfo fileInfo = new FileInfo(path + @"\" + picUrl.Substring(picUrl.LastIndexOf("/") + 1));
            request.ContentLength = fileInfo.Length;
        }

        private void RequestWriteUp(string token, ref Stream stream)
        {
            if (!this.tokenUp.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 文件下载
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            byte[] bytes = new byte[1024];
            long pos = 0;
            FileStream fs = new FileStream(path + @"\" + picUrl.Substring(picUrl.LastIndexOf("/") + 1), FileMode.Open, FileAccess.Read);
            FileInfo fileInfo = new FileInfo(path + @"\" + picUrl.Substring(picUrl.LastIndexOf("/") + 1));
            this.Log("开始上传...");
            while (true)
            {
                if (!this.tokenUp.Equals(token))
                {
                    throw new Exception("请求已失效");
                }
                int len = fs.Read(bytes, 0, bytes.Length);
                if (len <= 0)
                {
                    break;
                }
                pos += len;
                stream.Write(bytes, 0, len);
                this.UpdateProgress((int)((pos * 100) / fileInfo.Length));
            }
            fs.Close();
        }

        private void MessageUp(string token, string message, int code)
        {
            if (!this.tokenUp.Equals(token))
            {
                throw new Exception("请求已失效");
            }
            // 请求状态信息
            this.Log("code = " + code + ",message = " + message);
        }

        #endregion
    }
}