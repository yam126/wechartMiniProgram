using ncc2019.Common;
using ncc2019.Common.BLL;
using ncc2019.Common.Enum;
using ncc2019.Common.Tool;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class MyController : ControllerBase
    {
        //private ncc2019Entities db = new ncc2019Entities();
        //
        // GET: /My/

        public ActionResult Index()
        {
            if (this.CurMemberInfo.IsOwner)
            {

            }
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult Info()
        {
            ViewBag.isweixin = Common.Tool.UserAgentHelper.IsWeiXin();
            ViewBag.imgurl = "http://" + SettingBLL.WebDomain + "/api/GetQrCode?qraction=linkmember&param="
                + Common.Tool.DESEncrypt.Encrypt(this.CurMemberInfo.MemnerID);
            var user = db.Members.Find(CurMemberInfo.MemnerID);
            return View(user);
        }

        public ActionResult GetWeiXinInfo()
        {
            var url = TenPayManager.GetAuthorizeUrl("http://" + SettingBLL.MobileDomain + "/My/SaveWeiXinInfo/");
            return Redirect(url);
        }

        public ActionResult SaveWeiXinInfo(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }
            OAuthAccessTokenResult result = null;

            //通过，用code换取access_token
            try
            {
                result = OAuthApi.GetAccessToken(SettingBLL.AppID, SettingBLL.AppSecret, code);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }
            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                var member = db.Members.Where(c => c.WechatOpenid == userInfo.openid).FirstOrDefault();
                if (member != null)
                {
                    string nickname = CommonHelper.StripHTML(userInfo.nickname);
                    if (string.IsNullOrEmpty(member.Name))
                    {
                        member.Name = nickname;
                    }
                    if (string.IsNullOrEmpty(member.NiceName))
                    {
                        member.NiceName = nickname;
                    }
                    member.Sex = userInfo.sex;
                    member.Country = userInfo.country;
                    member.City = userInfo.city;
                    member.Province = userInfo.province;
                    member.HeadImgUrl = userInfo.headimgurl;

                    //写入当前用户信息里面
                    CurMemberInfo.HeadImgUrl = member.HeadImgUrl;
                    CurMemberInfo.Name = member.NiceName;

                   
                    db.Entry(member).Property("Balance").IsModified = false;
                    db.Entry(member).Property("Balance_back").IsModified = false;

                    db.SaveChanges();
                }

                return Redirect("http://" + SettingBLL.MobileDomain + "/My/Info/");
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }

        }

        public ActionResult Account()
        {
            ViewBag.Balance = SessionHelper.CurMemberInfo.Balance;
            return View("~/views/my/Account.cshtml");
        }

        [HttpPost]
        public ActionResult DoChangePwd(string passwordold, string passwordnew, string passwordre)
        {
            string messageStr = "";
            var user = db.Members.Find(this.CurMemberInfo.MemnerID);
            if (passwordold == user.Password)
            {
                if (passwordnew == passwordre)
                {
                    user.Password = passwordnew;
                   
                    db.Entry(user).Property("Balance").IsModified = false;
                    db.Entry(user).Property("Balance_back").IsModified = false;
                    db.SaveChanges();
                    messageStr = "密码修改成功！";
                    return Redirect("/my");
                }
                else
                {
                    messageStr = "两次输入的密码不同！";
                    //两次输入的密码不同
                }
            }
            else
            {
                messageStr = "旧密码不正确！";
                //旧密码不正确
            }
            ShowAlertMessage(messageStr);
            return View("~/views/my/ChangePassword.cshtml");
        }
        [HttpPost]
        public ActionResult DoChangeInfo(string name, string nicename, string phone, string sex)
        {
            var user = db.Members.Find(SessionHelper.CurMemberInfo.MemnerID);
            user.Name = name;
            user.NiceName = nicename;
            //user.Email = email;
            user.Phone = phone;
            if (!string.IsNullOrEmpty(sex))
            {
                user.Sex = int.Parse(sex);
            }
            string messageStr = "";
            try
            {

               
                db.Entry(user).Property("Balance").IsModified = false;
                db.Entry(user).Property("Balance_back").IsModified = false;
                db.SaveChanges();
                messageStr = "保存成功！";
                return Redirect("/my");
            }
            catch (Exception error)
            {
                messageStr = "保存失败！";
                ShowAlertMessage(messageStr);
            }


            return View("~/views/my/info.cshtml", user);
        }

      
        public ActionResult SubMemberList()
        {
            var memberlist = db.Members.Where(c => c.RefMemberID == this.CurMemberInfo.MemnerID);
            return View(memberlist);
        }
        public ActionResult GuanZhu()
        {
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            ViewBag.today = db.Members.Where(c => c.IsGuanZhu == 1 && c.RegDate > today).Count();
            ViewBag.total = db.Members.Where(c => c.IsGuanZhu == 1).Count();

            return View();
        }

        public ActionResult LoginMemberList()
        {
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var memberlist = db.Members.Where(c => c.LastDate > today).OrderByDescending(c => c.LastDate);
            return View(memberlist);
        }
    }
}
