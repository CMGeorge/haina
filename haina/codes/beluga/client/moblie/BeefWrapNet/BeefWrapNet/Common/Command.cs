using System;

using System.Collections.Generic;
using System.Text;

namespace BeefWrap.Common.Net
{
    /// <summary>
    /// 命令原语定义
    /// </summary>
    public class Command
    {

        #region request
        public static readonly int C_USER_REG_REQUEST = 10001;
        public static readonly int C_USER_LOGIN_REQUEST = 10002;
        public static readonly int C_USER_LOGOUT_REQUEST = 10003;
        public static readonly int C_USER_EXIST_CHECK_REQUEST = 10004;
        public static readonly int C_USER_PWD_UPDATE_REQUEST = 10005;
        public static readonly int C_USER_PWD_RETRIEVE_REQUEST = 10006;
        public static readonly int C_USER_EMAIL_UPDATE_REQUEST = 10007;
        public static readonly int C_USER_EMAIL_RETRIEVE_REQUEST = 10008;
        public static readonly int C_USER_MOBILE_UPDATE_REQUEST = 10009;
        public static readonly int C_USER_IMEI_UPDATE_REQUEST = 10010;
        public static readonly int C_USER_INFO_UPDATE_REQUEST = 10011;
        public static readonly int C_USER_SIGNATURE_UPDATE_REQUEST = 10012;
        public static readonly int C_USER_PAGE_MSGBOARD_REQUEST = 10013;
        public static readonly int C_USER_FEED_REQUEST = 10014;
        public static readonly int C_USER_WEATHER_REQUEST = 10015;
        public static readonly int C_USER_WEEK_WEATHER_REQUEST = 10016;
        public static readonly int C_USER_LOCATE_SYNC_REQUEST = 10017;
        public static readonly int C_USER_LOCATE_REQUEST = 10018;
        public static readonly int C_USER_MAP_REQUEST = 10019;
        public static readonly int C_CONTACT_USERID_CHECK_REQUEST = 10020;
        public static readonly int C_CONTACT_LOCATE_REQUEST = 10021;
        public static readonly int C_CONTACT_MAP_REQUEST = 10022;
        public static readonly int C_CONTACT_SYNC_REQUEST = 10023;
        public static readonly int C_CONTACT_SIGNATURE_REQUEST = 10024;
        public static readonly int C_CONTACT_FEED_REQUEST = 10025;
        public static readonly int C_CONTACT_LATEST_USERMSG_REQUEST = 10026;
        public static readonly int C_CONTACT_PAGE_USERMSG_REQUEST = 10027;
        public static readonly int C_CONTACT_WEATHER_REQUEST = 10028;
        public static readonly int C_CONTACTS_FEED_REQUEST = 10029;
        public static readonly int C_ALBUM_COVERS_REQUEST = 10030;
        public static readonly int C_ALBUM_PHOTOS_REQUEST = 10031;
        public static readonly int C_ALBUM_PHOTO_VIEW_REQUEST = 10032;
        public static readonly int C_ALBUM_UPDATE_REQUEST = 10033;
        public static readonly int C_ALBUM_ADD_REQUEST = 10034;
        public static readonly int C_ALBUM_DEL_REQUEST = 10035;
        public static readonly int C_PHOTO_COMMENT_DEL_REQUEST = 10036;
        public static readonly int C_PHOTO_COMMENT_ADD_REQUEST = 10037;
        public static readonly int C_PHOTOS_DEL_REQUEST = 10038;
        public static readonly int C_PHOTOS_ADD_REQUEST = 10039;
        public static readonly int C_USERMSGS_DEL_REQUEST = 10040;
        public static readonly int C_USERMSGS_ADD_REQUEST = 10041;
        public static readonly int C_USERMSG_REPLY_REQUEST = 10042;
        public static readonly int C_SYS_ALERTS_RECEIVE_REQUEST = 10043;
        public static readonly int C_SYS_ALERTS_DEL_REQUEST = 10044;
        public static readonly int C_INNERMSGS_RECEIVE_REQUEST = 10045;
        public static readonly int C_INNERMSG_SEND_REQUEST = 10046;
        public static readonly int C_INNERMSG_PICTURE_SEND_REQUEST = 10047;
        public static readonly int C_APP_CATALOG_REQUEST = 10048;
        public static readonly int C_APP_LIST_REQUEST = 10049;
        public static readonly int C_APP_HOT_LIST_REQUEST = 10050;
        public static readonly int C_APP_LATEST_LIST_REQUEST = 10051;
        public static readonly int C_COMMON_HLR_SYNC_REQUEST = 10052;
        #endregion

        #region response
        public static readonly int S_USER_REG_RESPONSE = 20001;
        public static readonly int S_USER_LOGIN_RESPONSE = 20002;
        public static readonly int S_USER_LOGOUT_RESPONSE = 20003;
        public static readonly int S_USER_EXIST_CHECK_RESPONSE = 20004;
        public static readonly int S_USER_PWD_UPDATE_RESPONSE = 20005;
        public static readonly int S_USER_PWD_RETRIEVE_RESPONSE = 20006;
        public static readonly int S_USER_EMAIL_UPDATE_RESPONSE = 20007;
        public static readonly int S_USER_EMAIL_RETRIEVE_RESPONSE = 20008;
        public static readonly int S_USER_MOBILE_UPDATE_RESPONSE = 20009;
        public static readonly int S_USER_IMEI_UPDATE_RESPONSE = 20010;
        public static readonly int S_USER_INFO_UPDATE_RESPONSE = 20011;
        public static readonly int S_USER_SIGNATURE_UPDATE_RESPONSE = 20012;
        public static readonly int S_USER_PAGE_MSGBOARD_RESPONSE = 20013;
        public static readonly int S_USER_FEED_RESPONSE = 20014;
        public static readonly int S_USER_WEATHER_RESPONSE = 20015;
        public static readonly int S_USER_WEEK_WEATHER_RESPONSE = 20016;
        public static readonly int S_USER_LOCATE_SYNC_REQUEST = 20017;
        public static readonly int S_USER_LOCATE_RESPONSE = 20018;
        public static readonly int S_USER_MAP_RESPONSE = 20019;
        public static readonly int S_CONTACT_USERID_CHECK_RESPONSE = 20020;
        public static readonly int S_CONTACT_LOCATE_RESPONSE = 20021;
        public static readonly int S_CONTACT_MAP_RESPONSE = 20022;
        public static readonly int S_CONTACT_SYNS_RESPONSE = 20023;
        public static readonly int S_CONTACT_SIGNATURE_RESPONSE = 20024;
        public static readonly int S_CONTACT_FEED_RESPONSE = 20025;
        public static readonly int S_CONTACT_LATEST_USERMSG_RESPONSE = 20026;
        public static readonly int S_CONTACT_PAGE_USERMSG_RESPONSE = 20027;
        public static readonly int S_CONTACT_WEATHER_RESPONSE = 20028;
        public static readonly int S_CONTACTS_FEED_RESPONSE = 20029;
        public static readonly int S_ALBUM_COVERS_RESPONSE = 20030;
        public static readonly int S_ALBUM_PHOTOS_RESPONSE = 20031;
        public static readonly int S_ALBUM_PHOTO_VIEW_RESPONSE = 20032;
        public static readonly int S_ALBUM_UPDATE_RESPONSE = 20033;
        public static readonly int S_ALBUM_ADD_RESPONSE = 20034;
        public static readonly int S_ALBUM_DEL_RESPONSE = 20035;
        public static readonly int S_PHOTO_COMMENT_DEL_RESPONSE = 20036;
        public static readonly int S_PHOTO_COMMENT_ADD_RESPONSE = 20037;
        public static readonly int S_PHOTOS_DEL_RESPONSE = 20038;
        public static readonly int S_PHOTOS_ADD_RESPONSE = 20039;
        public static readonly int S_USERMSGS_DEL_RESPONSE = 20040;
        public static readonly int S_USERMSGS_ADD_RESPONSE = 20041;
        public static readonly int S_USERMSG_REPLY_RESPONSE = 20042;
        public static readonly int S_SYS_ALERTS_RECEIVE_RESPONSE = 20043;
        public static readonly int S_SYS_ALERTS_DEL_RESPONSE = 20044;
        public static readonly int S_INNERMSGS_RECEIVE_RESPONSE = 20045;
        public static readonly int S_INNERMSG_SEND_RESPONSE = 20046;
        public static readonly int S_INNERMSG_PICTURE_SEND_RESPONSE = 20047;
        public static readonly int S_APP_CATALOG_RESPONSE = 20048;
        public static readonly int S_APP_LIST_RESPONSE = 20049;
        public static readonly int S_APP_HOT_LIST_RESPONSE = 20050;
        public static readonly int S_APP_LATEST_LIST_RESPONSE = 20051;
        public static readonly int S_COMMON_HLR_SYNS_RESPONSE = 20052;
        #endregion

    }
}
