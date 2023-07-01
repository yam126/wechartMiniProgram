using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Collections;

namespace ncc2019.Common.Tool
{
    public class SessionHelper : IRequiresSessionState
    {
        private static Hashtable htSession = new Hashtable();
        private static ncc2019Entities db = new ncc2019Entities();
        /// <summary>
        /// 当前用户
        /// </summary>
        public static Common.Model.MemberInfo CurMemberInfo
        {
            get { return (Common.Model.MemberInfo)System.Web.HttpContext.Current.Session["CurMemmber"]; }
            set { System.Web.HttpContext.Current.Session["CurMemmber"] = value; htSession[value.MemnerID] = System.Web.HttpContext.Current.Session["CurMemmber"]; }
        }
        /// <summary>
        /// 退出
        /// </summary>
        public static void Logout()
        {
            htSession.Remove(CurMemberInfo.MemnerID);
            System.Web.HttpContext.Current.Session["CurMemmber"] = null;

        }
        /// <summary>
        /// 是否登陆
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin()
        {
            if (CurMemberInfo == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 是否存在email  存在返回true  否则返回false
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ExistEmail(string email)
        {
            var memberList = from c in db.Members where c.Email == email select c;
            if (memberList.Count() > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetSession(string key, object value)
        {
            System.Web.HttpContext.Current.Session[key] = value;
        }
        /// <summary>
        /// 获取session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSession(string key)
        {
            return System.Web.HttpContext.Current.Session[key] == null ? "" : System.Web.HttpContext.Current.Session[key].ToString();
        }

        public static object GetSessionObj(string key)
        {
            return System.Web.HttpContext.Current.Session[key];
        }

        public static void SetJumpUrl(string url)
        {
            SetSession("JumpURL", url);
        }
        /// <summary>
        /// 优化跳转功能，session设置需要时间，提前进行初始化，防止后面设置的时候不生效
        /// </summary>
        /// <param name="url"></param>
        public static void InitJumpUrl(string url)
        {

            if (System.Web.HttpContext.Current.Session["JumpURL"] == null)
            {
                System.Web.HttpContext.Current.Session["JumpURL"] = "";
            }
        }

        public static string GetJumpUrl()
        {
            return GetSession("JumpURL").ToString();
        }
        public static void Reflush(int memberid)
        {
            Common.Model.MemberInfo infoModel = (Common.Model.MemberInfo)htSession[memberid];

            var member = db.Members.AsNoTracking().FirstOrDefault(c => c.MemberID == memberid);
            //Common.Model.MemberInfo minfo = Common.Model.MemberInfo.BuildMemberInfo(member);
            infoModel.Name = member.Name;

        }
        /// <summary>
        /// 设置标记需要跳转回微信
        /// </summary>
        public static void SetJumpInWeiXin()
        {
            SetSession("IsInWeiXin", "true");
        }

        /// <summary>
        /// 是否需要跳回微信
        /// </summary>
        /// <returns></returns>
        public static bool IsJumpInWeiXin()
        {
            object obj = GetSessionObj("IsInWeiXin");
            if (obj != null && obj.ToString() == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}