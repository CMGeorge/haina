using System;
using Microsoft.WindowsMobile.PocketOutlook.MessageInterception;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsMobile.PocketOutlook;

namespace BelugaMobile
{
    public delegate void MessageInterceptorHandler(Message message);
    public class ListenMessage
    {
        MessageInterceptor msgInterceptor = null;
        /// <summary>
        /// 启动短信监听
        /// </summary>
        internal void Start()
        {
            msgInterceptor = new MessageInterceptor();
            msgInterceptor.InterceptionAction = InterceptionAction.NotifyAndDelete; //InterceptionAction.NotifyAndDelete;
            MessageCondition msgCondition = new MessageCondition();
            msgCondition.ComparisonType = MessagePropertyComparisonType.Contains;
            msgCondition.Property = MessageProperty.Sender;
            msgInterceptor.MessageCondition = msgCondition;
            msgInterceptor.MessageReceived += new MessageInterceptorEventHandler(msgInterceptor_MessageReceived);
        }

        /// <summary>
        /// 短信事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void msgInterceptor_MessageReceived(object sender, MessageInterceptorEventArgs e)
        {
            if (MessageInterceptor != null)
            {
                MessageInterceptor(e.Message);
            }
        }



        public event MessageInterceptorHandler MessageInterceptor;

    }
}
