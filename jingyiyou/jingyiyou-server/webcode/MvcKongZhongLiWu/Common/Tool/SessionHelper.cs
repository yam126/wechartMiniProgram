using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MvcKongZhongLiWu.Common.Tool
{
    public class SessionHelper : IRequiresSessionState
    {       
        
        /// <summary>
        /// 当前用户
        /// </summary>
        public static Common.Model.MemberInfo CurMemberInfo
        {
            get { return (Common.Model.MemberInfo)System.Web.HttpContext.Current.Session["CurMemmber"]; }
            set {  System.Web.HttpContext.Current.Session["CurMemmber"]= value; }
        }
        /// <summary>
        /// 退出
        /// </summary>
        public static void Logout()
        {
            CurMemberInfo = null;
        }
        /// <summary>
        /// 是否登陆
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin()
        {
            if (CurMemberInfo==null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}