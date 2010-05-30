using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Data;
using System.Data.SQLiteClient;
using BelugaMobile.Db.Xsd;

namespace BelugaMobile.Db
{
    class MsgFace
    {
        public string face_symbol;
        public string face_picture;
        public string face_desc;
    }

    class UserConfig
    {
        public string user_id;
        public string imei;
        public string username;
        public string mobile;
        public string sex; 
        public string photo;
        public string signature;
        public string home_tel;
        public string work_tel;
        public string other_tel;
        public string fax;
        public string person_email;
        public string work_email;
        public string other_email;
        public string home_street;
        public string home_city;
        public string home_province;
        public string work_street;
        public string work_city;
        public string work_province;
        public string other_street;
        public string other_city;
        public string other_province;
        public string qq;
        public string msn;
        public string other_im;
        public string url;
        public DateTime birthday;
        public string org;
        public string title;
        public string note;
    }

    class BelugaDb
    {
        private static string RUN_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        private static string DATA_PATH = RUN_PATH + @"\data";
        private static string MSG_FACE_PATH = DATA_PATH + @"\msg_face";
        private static string MSG_FACE_XML = DATA_PATH + @"\msg_face.xml";
        private static string QUICK_MSG_XML = DATA_PATH + @"\quick_msg.xml";
        private static string USER_CONFIG_XML = DATA_PATH + @"\user_config.xml";

        private UserConfig _userconfig = new UserConfig();
        private SQLiteConnection _Connection;

        public BelugaDb()
        {
            _Connection = new SQLiteConnection("Data Source=" + DATA_PATH + @"\db\beluga.db;NewDatabase=False;Synchronous=Off;Encoding=UTF8");
        }

        /* 打开数据库，在客户端应用初始化时调用 */
        public void Open()
        {
            _Connection.Open();
            InitQuickMsgList();
            InitMsgFaceList();
        }

        /* 关闭数据库，在客户端应用关闭时调用 */
        public void Close()
        {
            _Connection.Close();
        }

        /* 读取和设置用户配置信息 */
        public UserConfig UserConfig
        {
            get 
            {
                if (!string.IsNullOrEmpty(_userconfig.user_id))
                    return _userconfig;

                userconfig usercfg = new userconfig();
                if (File.Exists(USER_CONFIG_XML))
                {
                    usercfg.ReadXml(USER_CONFIG_XML);

                    userconfig.user_configRow row = usercfg.user_config[0];
                    _userconfig.user_id = row.user_id;
                    _userconfig.username = row.username;
                    _userconfig.imei = row.imei;
                    _userconfig.mobile = row.mobile;
                    _userconfig.sex = row.sex;
                    _userconfig.photo = row.photo;
                    _userconfig.signature = row.signature;
                    _userconfig.home_tel = row.home_tel;
                    _userconfig.work_tel = row.work_tel;
                    _userconfig.other_tel = row.other_tel;
                    _userconfig.fax = row.fax;
                    _userconfig.person_email = row.person_email;
                    _userconfig.work_email = row.work_email;
                    _userconfig.other_email = row.other_email;
                    _userconfig.home_street = row.home_street;
                    _userconfig.home_city = row.home_city;
                    _userconfig.home_province = row.home_province;
                    _userconfig.work_street = row.work_street;
                    _userconfig.work_city = row.work_city;
                    _userconfig.work_province = row.work_province;
                    _userconfig.other_street = row.other_street;
                    _userconfig.other_city = row.other_city;
                    _userconfig.other_province = row.other_province;
                    _userconfig.qq = row.qq;
                    _userconfig.msn = row.msn;
                    _userconfig.other_im = row.other_im;
                    _userconfig.url = row.url;
                    _userconfig.birthday = row.birthday;
                    _userconfig.org = row.org;
                    _userconfig.title = row.title;
                    _userconfig.note = row.note;
                }
                return _userconfig; 
            }
            set 
            { 
                _userconfig = value as UserConfig;
                if (File.Exists(USER_CONFIG_XML))
                    File.Delete(USER_CONFIG_XML);

                userconfig usercfg = new userconfig();
                userconfig.user_configRow row = usercfg.user_config.Newuser_configRow();

                row.user_id = _userconfig.user_id;
                row.username = _userconfig.username;
                row.imei = _userconfig.imei;
                row.mobile = _userconfig.mobile;
                row.sex = _userconfig.sex;
                row.photo = _userconfig.photo;
                row.signature = _userconfig.signature;
                row.home_tel = _userconfig.home_tel;
                row.work_tel = _userconfig.work_tel;
                row.other_tel = _userconfig.other_tel;
                row.fax = _userconfig.fax;
                row.person_email = _userconfig.person_email;
                row.work_email = _userconfig.work_email;
                row.other_email = _userconfig.other_email;
                row.home_street = _userconfig.home_street;
                row.home_city = _userconfig.home_city;
                row.home_province = _userconfig.home_province;
                row.work_street = _userconfig.work_street;
                row.work_city = _userconfig.work_city;
                row.work_province = _userconfig.work_province;
                row.other_street = _userconfig.other_street;
                row.other_city = _userconfig.other_city; 
                row.other_province = _userconfig.other_province;
                row.qq = _userconfig.qq;
                row.msn = _userconfig.msn;
                row.other_im = _userconfig.other_im;
                row.url = _userconfig.url;
                row.birthday = _userconfig.birthday;
                row.org = _userconfig.org;
                row.title = _userconfig.title;
                row.note = _userconfig.note;

                usercfg.user_config.Adduser_configRow(row);
            }
        }

        /* 读取用户的联系人列表， 该列表只包含联系人的基本信息：
         *  该联系人的数据库id，添加该联系的用户社区user_id, 联系人的社区user_id，手机号，用户对该联系人的称呼，称呼的拼音 */
        public Contact GetContactList()
        {
            SQLiteCommand cmd = _Connection.CreateCommand("select * from t_contact order by name_spell asc;");
            SQLiteDataReader reader = cmd.ExecuteReader();

            Contact contacts = new Contact(reader);
            return contacts;
        }

        /* 读取用户的联系人总数 */
        public int GetContactCount()
        {
            SQLiteCommand cmd = _Connection.CreateCommand("select count(*) from t_contact;");
            return (int)cmd.ExecuteScalar();
        }

        /* 读取联系人基本信息，根据联系人数据库Id */
        public Contact GetContact(int contactId)
        {
            SQLiteCommand cmd = _Connection.CreateCommand("select * from t_contact where cid = @contactId;");
            cmd.Parameters.Add("@contactId", DbType.Int32).Value = contactId;
            SQLiteDataReader reader = cmd.ExecuteReader();

            Contact contacts = new Contact(reader);
            return contacts;
        }

        /* 读取联系人详细信息，根据联系人数据库Id */
        public ContactDetail GetContactDetail(int contactId)
        {
            SQLiteCommand cmd = _Connection.CreateCommand("select * from t_contact_detail where cid = @contactId;");
            cmd.Parameters.Add("@contactId", DbType.Int32).Value = contactId;
            SQLiteDataReader reader = cmd.ExecuteReader();

            ContactDetail contactdetail = new ContactDetail(reader);
            return contactdetail;
        }

        /* 删除一个联系人，根据联系人数据库Id */
        public void DeleteContact(int contactId)
        {
            SQLiteCommand cmd = _Connection.CreateCommand("delete from t_contact where cid = @contactId;");
            cmd.Parameters.Add("@contactId", DbType.Int32).Value = contactId;
            cmd.ExecuteNonQuery();
        }

        /* 根据姓名首字母匹配联系人 */
        public Contact FindContactList(string Letter)
        {
            SQLiteCommand cmd = _Connection.CreateCommand("select * from t_contact where name_spell like '@Letter%%' order by name_spell asc;");
            cmd.Parameters.Add("@Letter", DbType.String).Value = Letter;
            SQLiteDataReader reader = cmd.ExecuteReader();

            Contact contacts = new Contact(reader);
            return contacts;
        }

        /* 根据电话号码匹配最精确的联系人以获取其姓名 */
        public Contact FindContact(string tel)
        {
            SQLiteCommand cmd = _Connection.CreateCommand("select * from t_contact where dst_user_mobile = '@tel' or cid in " + 
                                                    "(select cid from t_contact_detail where home_tel = '@tel' or work_tel = '@tel' or other_tel = '@tel');");
            cmd.Parameters.Add("@tel", DbType.String).Value = tel;
            SQLiteDataReader reader = cmd.ExecuteReader();

            Contact contacts = new Contact(reader);
            return contacts;
        }

        /* 新增一个联系人到数据库，如果该联系人不是四湖用户，则user_id留空串；不填的字段留空串。注意不能为null */
        public void AddNewContact(string user_id, string mobile, string name, string photo,
            string home_tel, string work_tel, string other_tel, string fax, string person_email, string work_email,
            string other_email, string home_street, string home_city, string home_province, string work_street,
            string work_city, string work_province, string other_street, string other_city, string other_province,
            string qq, string msn, string other_im, string url, DateTime birthday, string org, string title, string note,
            string ring)
        {
            SQLiteCommand cmd = _Connection.CreateCommand("insert into t_contact (cid, src_user_id, dst_user_id, dst_user_mobile, name, name_spell, photo, create_time, delete_flag) " +
                "values (null, @src_user_id, @dst_user_id, @dst_user_mobile, @name, @name_spell, @photo, @create_time, @delete_flag);");

            cmd.Parameters.Add("@src_user_id", DbType.String).Value = _userconfig.user_id;
            cmd.Parameters.Add("@dst_user_id", DbType.String).Value = user_id;
            cmd.Parameters.Add("@dst_user_mobile", DbType.String).Value = mobile;
            cmd.Parameters.Add("@name", DbType.String).Value = name;
            cmd.Parameters.Add("@name_spell", DbType.String).Value = Han2Pinyin.Chinese2PY(name);
            cmd.Parameters.Add("@photo", DbType.String).Value = photo;
            cmd.Parameters.Add("@create_time", DbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@delete_flag", DbType.Int16).Value = 0;
            cmd.ExecuteNonQuery();

            int LastContactId = (int)_Connection.CreateCommand("select max(cid) from t_contact;").ExecuteScalar();

            cmd = _Connection.CreateCommand("insert into t_contact_detail (cdid, cid, home_tel, work_tel, other_tel, fax, person_email, work_email, other_email, " +
                "home_street, home_city, home_province, work_street, work_city, work_province, other_street, other_city, other_province, qq, msn, other_im, " +
                "url, birthday, org, title, note, ring, create_time, delete_flag) " +
                "values (null, @cid, @home_tel, @work_tel, @other_tel, @fax, @person_email, @work_email, @other_email, " +
                "@home_street, @home_city, @home_province, @work_street, @work_city, @work_province, @other_street, @other_city, @other_province, " + 
                "@qq, @msn, @other_im, @url, @birthday, @org, @title, @note, @ring, @create_time, @delete_flag);");

            cmd.Parameters.Add("@cid", DbType.Int32).Value = LastContactId;
            cmd.Parameters.Add("@home_tel", DbType.String).Value = home_tel;
            cmd.Parameters.Add("@work_tel", DbType.String).Value = work_tel;
            cmd.Parameters.Add("@other_tel", DbType.String).Value = other_tel;
            cmd.Parameters.Add("@fax", DbType.String).Value = fax;
            cmd.Parameters.Add("@person_email", DbType.String).Value = person_email;
            cmd.Parameters.Add("@work_email", DbType.String).Value = work_email;
            cmd.Parameters.Add("@other_email", DbType.String).Value = other_email;
            cmd.Parameters.Add("@home_street", DbType.String).Value = home_street;
            cmd.Parameters.Add("@home_city", DbType.String).Value = home_city;
            cmd.Parameters.Add("@home_province", DbType.String).Value = home_province;
            cmd.Parameters.Add("@work_street", DbType.String).Value = work_street;
            cmd.Parameters.Add("@work_city", DbType.String).Value = work_city;
            cmd.Parameters.Add("@work_province", DbType.String).Value = work_province;
            cmd.Parameters.Add("@other_street", DbType.String).Value = other_street;
            cmd.Parameters.Add("@other_city", DbType.String).Value = other_city;
            cmd.Parameters.Add("@other_province", DbType.String).Value = other_province;
            cmd.Parameters.Add("@qq", DbType.String).Value = qq;
            cmd.Parameters.Add("@msn", DbType.String).Value = msn;
            cmd.Parameters.Add("@other_im", DbType.String).Value = other_im;
            cmd.Parameters.Add("@url", DbType.String).Value = url;
            cmd.Parameters.Add("@birthday", DbType.DateTime).Value = birthday;
            cmd.Parameters.Add("@org", DbType.String).Value = org;
            cmd.Parameters.Add("@title", DbType.String).Value = title;
            cmd.Parameters.Add("@note", DbType.String).Value = note;
            cmd.Parameters.Add("@ring", DbType.String).Value = ring;
            cmd.Parameters.Add("@create_time", DbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@delete_flag", DbType.Int16).Value = 0;
            cmd.ExecuteNonQuery();
        }

        /* 根据联系人数据库id，编辑一个联系人到数据库，不修改的字段必须将原字段值传给参数，清空字段值使用空字符串。注意不能为null */
        public void EditContact(int contact_id, string user_id, string mobile, string name, string photo,
            string home_tel, string work_tel, string other_tel, string fax, string person_email, string work_email,
            string other_email, string home_street, string home_city, string home_province, string work_street,
            string work_city, string work_province, string other_street, string other_city, string other_province,
            string qq, string msn, string other_im, string url, DateTime birthday, string org, string title, string note,
            string ring)
        {
            Contact contact = GetContact(contact_id);
            SQLiteCommand cmd = _Connection.CreateCommand("update t_contact set dst_user_id = @user_id, dst_user_mobile = @mobile, " +
                "name = @name, name_spell = @name_spell, photo = @photo where cid = @contact_id;");

            if (string.IsNullOrEmpty(contact["dst_user_id"].ToString()) || contact["dst_user_mobile"].ToString().Equals(mobile))
                cmd.Parameters.Add("@dst_user_id", DbType.String).Value = user_id;
            else
                cmd.Parameters.Add("@dst_user_id", DbType.String).Value = "";

            cmd.Parameters.Add("@dst_user_mobile", DbType.String).Value = mobile;
            cmd.Parameters.Add("@name", DbType.String).Value = name;
            cmd.Parameters.Add("@name_spell", DbType.String).Value = Han2Pinyin.Chinese2PY(name);
            cmd.Parameters.Add("@photo", DbType.String).Value = photo;
            cmd.Parameters.Add("@contact_id", DbType.Int32).Value = contact_id;
            cmd.ExecuteNonQuery();

            ContactDetail detail = GetContactDetail(contact_id);
            cmd = _Connection.CreateCommand("update t_contact_detail set home_tel = @home_tel, work_tel = @home_tel, " +
                "other_tel = @other_tel, fax = @fax, person_email = @person_email, work_email = @work_email, other_email = @other_email, " +
                "home_street = @home_street, home_city = @home_city, home_province = @home_province, work_street = @work_street, " +
                "work_city = @work_city, work_province @work_province, other_street = @other_street, other_city = @other_city, " +
                "other_province = @other_province, qq = @qq, msn = @msn, other_im = @other_im, url = @url, birthday = @birthday, " +
                "org = @org, title = @title, note = @note, ring = @ring where cid = @contact_id;");

            cmd.Parameters.Add("@home_tel", DbType.String).Value = home_tel;
            cmd.Parameters.Add("@work_tel", DbType.String).Value = work_tel;
            cmd.Parameters.Add("@other_tel", DbType.String).Value = other_tel;
            cmd.Parameters.Add("@fax", DbType.String).Value = fax;
            cmd.Parameters.Add("@person_email", DbType.String).Value = person_email;
            cmd.Parameters.Add("@work_email", DbType.String).Value = work_email;
            cmd.Parameters.Add("@other_email", DbType.String).Value = other_email;
            cmd.Parameters.Add("@home_street", DbType.String).Value = home_street;
            cmd.Parameters.Add("@home_city", DbType.String).Value = home_city;
            cmd.Parameters.Add("@home_province", DbType.String).Value = home_province;
            cmd.Parameters.Add("@work_street", DbType.String).Value = work_street;
            cmd.Parameters.Add("@work_city", DbType.String).Value = work_city;
            cmd.Parameters.Add("@work_province", DbType.String).Value = work_province;
            cmd.Parameters.Add("@other_street", DbType.String).Value = other_street;
            cmd.Parameters.Add("@other_city", DbType.String).Value = other_city;
            cmd.Parameters.Add("@other_province", DbType.String).Value = other_province;
            cmd.Parameters.Add("@qq", DbType.String).Value = qq;
            cmd.Parameters.Add("@msn", DbType.String).Value = msn;
            cmd.Parameters.Add("@other_im", DbType.String).Value = other_im;
            cmd.Parameters.Add("@url", DbType.String).Value = url;
            cmd.Parameters.Add("@birthday", DbType.DateTime).Value = birthday;
            cmd.Parameters.Add("@org", DbType.String).Value = org;
            cmd.Parameters.Add("@title", DbType.String).Value = title;
            cmd.Parameters.Add("@note", DbType.String).Value = note;
            cmd.Parameters.Add("@ring", DbType.String).Value = ring;
            cmd.Parameters.Add("@contact_id", DbType.Int32).Value = contact_id;
            cmd.ExecuteNonQuery();
        }

        /* 读取快捷消息列表 */
        public List<string> GetQuickMsgList()
        {
            List<string> quickmsglist = new List<string>();
            quickmsg qm = new quickmsg();
            if (File.Exists(QUICK_MSG_XML))
            {
                qm.ReadXml(QUICK_MSG_XML);
            }

            foreach (quickmsg.quick_msgRow row in qm.quick_msg)
            {
                quickmsglist.Add(row.content);
            }

            return quickmsglist;
        }

        /* 读取表情列表 */
        public List<MsgFace> GetMsgFaceList()
        {
            List<MsgFace> facelist = new List<MsgFace>();
            msgface mf = new msgface();
            if (File.Exists(MSG_FACE_XML))
            {
                mf.ReadXml(MSG_FACE_XML);
            }

            foreach (msgface.msg_faceRow row in mf.msg_face)
            {
                MsgFace item = new MsgFace();
                item.face_symbol = row.symbol;
                item.face_picture = row.picture;
                item.face_desc = row.description;
                facelist.Add(item);
            }

            return facelist;
        }

        /* 根据表情符号查询表情图片 */
        public string GetMsgFacePicture(string symbol)
        {
            msgface mf = new msgface();
            if (File.Exists(MSG_FACE_XML))
            {
                mf.ReadXml(MSG_FACE_XML);
            }

            foreach (msgface.msg_faceRow row in mf.msg_face)
            {
                if (row.symbol.Equals(symbol))
                    return row.picture;
            }

            return "";
        }


        /* 初始化快捷消息列表，当前不支持编辑快捷回复 */
        private void InitQuickMsgList()
        {
            string[] quickmsgs = {"现在可以聊天吗？", 
                                    "我等你回复哦！", 
                                    "最近忙什么呢？", 
                                    "我现在正忙，稍后给你回复！"};

            quickmsg qm = new quickmsg();

            if (File.Exists(QUICK_MSG_XML))
                return;

            for (int i = 0; i < quickmsgs.Length; i++)
            {
                qm.quick_msg.Addquick_msgRow(quickmsgs[i]);
            }

            qm.WriteXml(QUICK_MSG_XML);
        }

        /* 初始化表情列表，当前不支持增加表情 */
        private void InitMsgFaceList()
        {
            if (!File.Exists(MSG_FACE_XML))
            {
                string[] symbols = {
                    ":-)", 
                    ":-<", 
                    ":-D", 
                    "-(",
                    ";-)",
                    ":-0",
                    "%-)",
                    "|-P",
                    ":-(",
                    ":-()",
                    "<@_@>",
                    "^-^",
                    "?_?",
                    "~zZ",
                    "/ToT/"
                };

                string[] descriptions = {
                    "微笑", 
                    "苦笑", 
                    "大笑", 
                    "痛苦", 
                    "抛媚眼",
                    "惊讶",
                    "大跌眼镜",
                    "捧腹大笑",
                    "生气",
                    "震惊",
                    "晕",
                    "害羞",
                    "疑惑",
                    "痛哭"
                };

                string[] pictures = { 
                    "mantou_005.png", 
                    "mantou_001.png", 
                    "mantou_004.png", 
                    "mantou_016.png", 
                    "mantou_015.png",
                    "mantou_009.png",
                    "mantou_013.png",
                    "mantou_010.png",
                    "mantou_018.png",
                    "mantou_003.png",
                    "mantou_020.png",
                    "mantou_006.png",
                    "mantou_012.png",
                    "mantou_017.png"
                };

                msgface mf = new msgface();
                for (int i = 0; i < symbols.Length; i++)
                {
                    mf.msg_face.Addmsg_faceRow(symbols[i], pictures[i], descriptions[i]);
                }

                mf.WriteXml(MSG_FACE_XML);
            }
        }
    }
}
