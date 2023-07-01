using ncc2019.Common.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019;
using System.Data;
using ncc2019.Common.BLL;

namespace ncc2019.Controllers
{
    public class LoginController : ControllerBaseNoCheck
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            ViewBag.jumpurl = SessionHelper.GetJumpUrl();
            return View();
        }
        public ActionResult Logout()
        {
            SessionHelper.Logout();
            return View("~/Views/Login/index.cshtml");
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoLogin(string loginname, string password, string jumpurl)
        {
            //验证登陆
            var memberList = from c in db.Members where c.LoginName == loginname && c.Password == password select c;
            if (memberList.Count() > 0)
            {
                var member = memberList.First();
                if (member.Status == (int)Common.Enum.MemmberStatus.正常)
                {
                    Common.Model.MemberInfo minfo = Common.Model.MemberInfo.BuildMemberInfo(member);
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

                    string url = SessionHelper.GetJumpUrl();

                    LoggerHelper.Info("Login--" + url);
                    if (string.IsNullOrEmpty(url))
                    {
                        return Redirect("~/Home");
                    }
                    else
                    {
                        SessionHelper.SetJumpUrl("");
                        return Redirect(url);
                    }

                }
                else
                {
                    return LoginError();
                }

            }
            else
            {
                ShowAlertMessage("用户名或密码错误！");
                return View("~/Views/Login/index.cshtml");
            }


        }
        public ActionResult Reg()
        {
            return View("~/Views/Login/Reg.cshtml");

        }
        [HttpPost]
        public JsonResult IsReg(string loginname)
        {
            string messageStr = "OK";
            if (SessionHelper.ExistEmail(loginname))
            {
                messageStr = "Error";
            }

            return Json(new { message = messageStr });
        }
        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            string messageStr = "OK";
            var memberList = from c in db.Members where c.Email == email select c;
            if (memberList.Count() > 0)
            {
                messageStr = "Error";
            }
            return Json(new { message = messageStr });
        }
        [HttpPost]
        public ActionResult DoReg(Members user, string code)
        {
            string messageStr = "";
            if (SessionHelper.GetSession("ValidateCode") != code)
            {
                messageStr = "验证码错误！";
            }
            else
            {
                try
                {

                    user.RegDate = DateTime.Now;
                    user.LastDate = DateTime.Now;
                    user.Status = (int)Common.Enum.MemmberStatus.正常;
                    user.LoginCount = 1;
                    user.LoginName = user.Email.Trim();
                    user.Balance = 0;
                    user.Balance_back = 0;
                    user.UserLevel = (int)Common.Enum.UserLevel.普通账户;
                    db.Members.Add(user);
                    db.SaveChanges();
                    //messageStr = "注册成功！";

                    Common.Model.MemberInfo minfo = Common.Model.MemberInfo.BuildMemberInfo(user);

                    //记录登陆信息
                    LoginLog log = new LoginLog()
                    {
                        Agent = Request.UserAgent,
                        IP = Request.UserHostAddress,
                        LoginTime = DateTime.Now,
                        SystemInfo = UserAgentHelper.GetOSNameByUserAgent(Request.UserAgent),
                        MemberID = user.MemberID
                    };
                    db.LoginLog.Add(log);

                    SessionHelper.CurMemberInfo = minfo;

                    return View("~/Views/Login/RegOK.cshtml");
                }
                catch (Exception error)
                {
                    messageStr = "注册失败！";
                }
            }


            ShowMessage(messageStr);

            return View("~/Views/Login/Reg.cshtml");

        }

        public ActionResult GetPassword()
        {
            return View();
        }

        /// <summary>
        /// 发送修改密码消息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ActionResult DoGetPassword(string email)
        {
            var member = db.Members.Where(c => c.Email == email.Trim()).FirstOrDefault();
            if (member == null)
            {
                ShowAlertMessage("邮箱不存在！");
                return View("~/Views/Login/GetPassword.cshtml");
            }
            else
            {
                ncc2019.GetPassword password = new ncc2019.GetPassword
                {
                    BeginDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1),
                    GUID = Guid.NewGuid().ToString().Replace("-", ""),
                    RefEmail = email.Trim()
                };
                db.GetPassword.Add(password);
                db.SaveChanges();

                string url = "http://" + Common.BLL.SettingBLL.WebDomain + "/login/changepassword?key=" + password.GUID;
                SmtpHelper.Send(email.Trim(), url);
                return View("~/Views/Login/GetPasswordOK.cshtml");
            }

            //showSuccessMessage("找回密码成功！");

        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult ChangePassword(string key)
        {
            key = key.Trim();
            ViewBag.key = key;
            var password = db.GetPassword.Where(c => c.GUID == key).FirstOrDefault();

            if (password == null)
            {
                //出错了
                return View("/views/shared/error.cshtml");

            }
            else
            {
                return View();
            }

        }
        /// <summary>
        /// 修改密码操作
        /// </summary>
        /// <param name="passwordnew"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult DoChangePassword(string passwordnew, string key)
        {
            key = key.Trim();
            var password = db.GetPassword.Where(c => c.GUID == key).FirstOrDefault();

            if (password == null)
            {
                //出错了

                return View("/views/shared/error.cshtml");
            }
            else
            {
                var member = db.Members.Where(c => c.Email == password.RefEmail).FirstOrDefault();
                member.Password = passwordnew;

                //db.Entry(member).State = EntityState.Modified;
                db.Entry(member).Property("Balance").IsModified = false;
                db.Entry(member).Property("Balance_back").IsModified = false;



                password.GetDate = DateTime.Now;
                //db.Entry(password).State = EntityState.Modified;
                db.SaveChanges();

                return View("/views/login/ChangePasswordOK.cshtml");
            }

        }

        public ActionResult WeiXinLogin()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            @ViewBag.imgurl = "http://" + SettingBLL.WebDomain + "/api/GetQrCode?qraction=weblogin&param=" + guid;
            @ViewBag.guid = guid;
            return View();
        }

        public ActionResult LoginError()
        {
            ShowAlertMessage("用户名或密码错误！");
            return View("Index");
        }

    }
}
