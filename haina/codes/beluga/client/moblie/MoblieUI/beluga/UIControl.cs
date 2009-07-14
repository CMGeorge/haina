using System;
using System.Collections.Generic;
using System.Text;

namespace beluga
{
    public enum ContactTag:int
    {
        /// <summary>
        /// �ֻ���ϵ�˷�ҳ
        /// </summary>
        Contact_Page_Mobile=1,
        /// <summary>
        /// QQ��ҳ
        /// </summary>
        Contact_Page_IM_QQ = 2,//�ݴ�
        /// <summary>
        /// MSN��ҳ
        /// </summary>
        Contact_Page_IM_MSN=3,//�ύ(���)
        /// <summary>
        /// �������칤�߷�ҳ����GTalk
        /// </summary>
        Contact_Page_IM_Other=4,
        /// <summary>
        /// ���������ϵ�˷�����ʾ
        /// </summary>
        Contact_Recent_Call=5,
        /// <summary>
        /// ���������ϵ�˷�����ʾ
        /// </summary>
        Contact_Recent_Sms=6,
        /// <summary>
        /// ���QQ������ѷ�����ʾ
        /// </summary>
        Contact_Recent_QQ=7,	
        /// <summary>
        /// ���MSN������ѷ�����ʾ
        /// </summary>
        Contact_Recent_MSN=8	

     
    }


    public delegate void TxTSearchChageHandler(object sender,EventArgs e,ContactTag tag); 
}
