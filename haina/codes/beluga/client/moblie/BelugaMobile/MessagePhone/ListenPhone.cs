using System;
using Microsoft.WindowsMobile.Status;
using System.Collections.Generic;
using System.Text;

namespace MessagePhone
{
    public delegate void PhoneIncomingCallHandler(string phoneNum,DateTime dt);
    public class BelugaMobile
    {
         SystemState systemState =null;
         /// <summary>
        /// 启动短信监听
        /// </summary>
        internal void Start()
        {
            systemState=new  SystemState(SystemProperty.PhoneIncomingCall);
            systemState.Changed += new ChangeEventHandler(systemState_Changed);
        }

        public event PhoneIncomingCallHandler PhoneIncomingCallEvent;

        void systemState_Changed(object sender, Microsoft.WindowsMobile.Status.ChangeEventArgs args)
        {
            if (PhoneIncomingCallEvent != null)
            {
 
            }
        }
        
    }
}
