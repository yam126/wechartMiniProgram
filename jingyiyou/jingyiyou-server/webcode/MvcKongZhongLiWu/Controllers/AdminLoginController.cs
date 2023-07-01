using ncc2019.Common.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class AdminLoginController : ControllerAdminBaseNoCheck
    {
        //
        // GET: /AdminLogin/

        public ActionResult Index()
        {
            return View("/views/admin/index.cshtml");
        }

        public ActionResult DoLogin(string loginname, string password)
        {
            var member = db.Members.Where(c => c.LoginName == loginname && c.Password == password 
                               && c.UserLevel == (int)Common.Enum.UserLevel.管理员
                               && c.Status == (int)Common.Enum.MemmberStatus.正常).FirstOrDefault();

            if (member!=null)
            {
                Common.Model.MemberInfo minfo = new Common.Model.MemberInfo()
                {
                    MemnerID = member.MemberID,
                    Name = member.Name,
                    WeChatOpenid = member.WechatOpenid,
                    UserLevel = (int)Common.Enum.UserLevel.管理员
                };
                //记录登陆信息
                LoginLog log = new LoginLog()
                {
                    Agent = Request.UserAgent,
                    IP = Request.UserHostAddress,
                    LoginTime = DateTime.Now,
                    SystemInfo = UserAgentHelper.GetOSNameByUserAgent(Request.UserAgent),
                    MemberID = member.MemberID
                };
                db.LoginLog.Add(log);

                //记录登陆次数
                member.LoginCount += 1;
                member.LastDate = DateTime.Now;
                //db.Entry(member).State = System.Data.EntityState.Modified;
                db.Entry(member).Property("Balance").IsModified = false;
                db.Entry(member).Property("Balance_back").IsModified = false;
                db.SaveChanges();

                SessionHelper.CurMemberInfo = minfo;

                return Redirect("/admin/productlist");
            }
          
            return Redirect("/adminlogin");
        }
    }
}
