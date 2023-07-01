using ncc2019.Common.BLL;
using ncc2019.Common.Tool;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class LoginController : ControllerBase
    {
        //
        // GET: /Login/

        public ActionResult Index(string type)
        {
            ViewBag.type = type;
            return View();
        }
        public ActionResult Logout()
        {
            SessionHelper.Logout();
            return Redirect("/login");
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ActionResult DoLogin(string loginname, string password)
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
                    
                    db.SaveChanges();

                    SessionHelper.CurMemberInfo = minfo;

                    string url = SessionHelper.GetJumpUrl();
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
                return LoginError();
            }


        }

        [HttpPost]
        public ActionResult DoReg(Members user, string type)
        {
            string messageStr = "";

            if (SessionHelper.ExistEmail(user.Email))
            {
                ShowAlertMessage("电子邮箱地址已经存在！");
                return View("~/Views/Login/Reg.cshtml");
            }
            else
            {
                try
                {

                    user.RegDate = DateTime.Now;
                    user.LastDate = DateTime.Now;
                    user.Status = (int)Common.Enum.MemmberStatus.正常;
                    user.LoginCount = 1;
                    user.Balance = 0;
                    user.Balance_back = 0;
                    user.LoginName = user.Email.Trim();
                    user.UserLevel = (int)Common.Enum.UserLevel.普通账户;
                    user.TiLiNum = 1;
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



                    db.SaveChanges();
                    SessionHelper.CurMemberInfo = minfo;

                    if (type == "jj")
                    {
                        return Redirect(SessionHelper.GetJumpUrl());
                    }
                    else
                    {
                        return Redirect("/");
                    }

                }
                catch (Exception error)
                {
                    messageStr = "注册失败！";
                    ShowAlertMessage(messageStr);
                }
            }


            return View("~/Views/Login/Reg.cshtml");

        }

        public ActionResult Reg(string type)
        {
            ViewBag.type = type;
            return View("~/Views/Login/Reg.cshtml");

        }

        public ActionResult LoginError()
        {
            ShowAlertMessage("用户名或密码错误！");
            //Response.Write("<script>alert('用户名或密码错误！')</script>");
            return View("Index");
        }
        protected override bool CheckUser()
        {
            return true;
            //return base.CheckUser();
        }
        protected override bool CheckUserWeiXin()
        {
            return true;
        }
        /// <summary>
        /// 如果是用微信打开的则需要获取微信用户的基础信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult WeiXinLogin(string code = "", string state = "")
        {
            LoggerHelper.Debug("WeiXinLogin");
            OAuthAccessTokenResult result = null;
            //通过，用code换取access_token
            try
            {
                result = OAuthApi.GetAccessToken(SettingBLL.AppID, SettingBLL.AppSecret, code);

            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex.ToString());
                return Content(ex.Message);
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }
            LoggerHelper.Debug(result.openid);
            Members member = db.Members.Where(c => c.WechatOpenid == result.openid).FirstOrDefault();
            if (member == null)
            {
                //不存在此用户的话需要自动建立一个特殊账户
                member = new Members()
                {
                    WechatOpenid = result.openid,
                    LoginCount = 0,
                    Status = (int)Common.Enum.MemmberStatus.正常,
                    RegDate = DateTime.Now,
                    LastDate = DateTime.Now,
                    Balance = 0,
                    Balance_back = 0,
                    LoginName = result.openid,
                    Email = result.openid,
                    UserLevel = (int)Common.Enum.UserLevel.普通账户,
                    TiLiNum = 1
                };
                db.Members.Add(member);
                db.SaveChanges();

            }

            Common.Model.MemberInfo minfo = Common.Model.MemberInfo.BuildMemberInfo(member);
            string premid = SessionHelper.GetSession("premid");
            //记录登陆信息
            LoginLog log = new LoginLog()
            {
                Agent = Request.UserAgent,
                IP = Request.UserHostAddress,
                LoginTime = DateTime.Now,
                SystemInfo = UserAgentHelper.GetOSNameByUserAgent(Request.UserAgent),
                MemberID = member.MemberID
            };
            if (!string.IsNullOrEmpty(premid))
            {
                log.RefMemberID = int.Parse(Common.Tool.DESEncrypt.Decrypt(premid));
            }

            //保存最后登录时间
            member.LastDate = log.LoginTime;
            member.LoginCount++;
            db.Entry(member).Property(c => c.LastDate).IsModified = true;
            db.Entry(member).Property(c => c.LoginCount).IsModified = true;

            db.LoginLog.Add(log);

            db.SaveChanges();
            SessionHelper.CurMemberInfo = minfo;
            LoggerHelper.Debug(state);
            return Redirect(state);
        }

        public JsonResult Reflush(int mid)
        {
            SessionHelper.Reflush(mid);
            return null;
        }

    }
}
