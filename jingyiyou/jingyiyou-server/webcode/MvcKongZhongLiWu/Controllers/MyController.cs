using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Tool;

namespace ncc2019.Controllers
{
    public class MyController : ControllerBase
    {
        //
        // GET: /My/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoChangePwd(string passwordold, string passwordnew, string passwordre)
        {
            string messageStr = "";
            var user = db.Members.Find(SessionHelper.CurMemberInfo.MemnerID);
            if (passwordold == user.Password)
            {
                if (passwordnew == passwordre)
                {
                    user.Password = passwordnew;
                    //db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    messageStr = "密码修改成功！";
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
            ShowMessage(messageStr);
            return View("~/views/my/ChangePassword.cshtml");
        }

        public ActionResult Info()
        {
            var user = db.Members.Find(SessionHelper.CurMemberInfo.MemnerID);
            return View(user);
        }
        public ActionResult Account()
        {
            ViewBag.Balance = SessionHelper.CurMemberInfo.Balance;
            return View("~/views/my/Account.cshtml");
        }
        [HttpPost]
        public ActionResult DoChangeInfo(string name, string nicename, string email, string phone, string sex, string birth_year, string birth_month, string birth_day)
        {
            var user = db.Members.Find(SessionHelper.CurMemberInfo.MemnerID);
            user.Name = name;
            user.NiceName = nicename;
            user.Email = email;
            user.Phone = phone;           
            if (string.IsNullOrEmpty(birth_year)) birth_year = "0";
            if (string.IsNullOrEmpty(birth_month)) birth_month = "0";
            if (string.IsNullOrEmpty(birth_day)) birth_day = "0";
            if (birth_year!="0")
            {
                user.Birth = new DateTime(int.Parse(birth_year), int.Parse(birth_month), int.Parse(birth_day));
            }
            
            if (!string.IsNullOrEmpty(sex))
            {
                user.Sex = int.Parse(sex);
            }
            string messageStr = "";
            try
            {
                user.MemberID = SessionHelper.CurMemberInfo.MemnerID;
                //db.Entry(user).State = EntityState.Modified;
                db.Entry(user).Property("Balance").IsModified = false;
                db.Entry(user).Property("Balance_back").IsModified = false;

                db.SaveChanges();
                messageStr = "保存成功！";
            }
            catch (Exception error)
            {
                messageStr = "保存失败！";
            }

            showSuccessMessage(messageStr);
            return View("~/views/my/info.cshtml", user);
        }

    }
}
