using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using ncc2019.Common.Enum;
using ncc2019.Common.Tool;

namespace ncc2019.Controllers
{
    public class AdminCFJController : ControllerAdminBaseNoCheck
    {
        //
        // GET: /AdminCFJ/

        public ActionResult Index()
        {
            ViewBag.jumpurl = SessionHelper.GetJumpUrl();
            return View();
        }

        public ActionResult DoLogin(string loginname, string password, string jumpurl)
        {
            //验证登陆
            var memberList = from c in db.Members
                             where c.LoginName == loginname && c.Password == password && c.ISCFJUser == (int)ShiFouStatus.是
                                 && c.UserLevel == (int)UserLevel.管理员
                             select c;
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
                    //db.Entry(member).Property("Balance").IsModified = false;
                    //db.Entry(member).Property("Balance_back").IsModified = false;
                    db.SaveChanges();

                    SessionHelper.CurMemberInfo = minfo;

                    //string url = SessionHelper.GetJumpUrl();

                    //LoggerHelper.Info("Login--" + url);

                    return Redirect("~/admincfj/memberlist");


                }
                else
                {
                    return LoginError();
                }

            }
            else
            {
                ShowAlertMessage("用户名或密码错误！");
                return View("~/Views/admincfj/index.cshtml");
            }

        }

        public ActionResult LoginError()
        {
            ShowAlertMessage("用户名或密码错误！");
            return View("Index");
        }

        public ActionResult MemberList(int? pageindex = 1, int guanzhu = -1)
        {
            check();

            var memberList = db.Members.Where(c => c.ISCFJUser == (int)ShiFouStatus.是);

            int pageIndex = pageindex ?? 1;
            int totalCount = memberList.Count();
            ViewBag.totalCount = totalCount;
            PagedList<Members> mPage = memberList.OrderByDescending(c => c.MemberID).AsQueryable().ToPagedList(pageIndex, 10);
            mPage.TotalItemCount = totalCount;
            mPage.CurrentPageIndex = pageIndex;

            return View(mPage);
        }

        public ActionResult PaymentList(int? pageindex = 1)
        {
            check();

            var paymentlist = db.PayLog.Where(c => c.Param == "cfj");

            int pageIndex = pageindex ?? 1;
            int totalCount = paymentlist.Count();
            ViewBag.totalCount = totalCount;
            PagedList<PayLog> mPage = paymentlist.OrderByDescending(c => c.MemberID).AsQueryable().ToPagedList(pageIndex, 10);
            mPage.TotalItemCount = totalCount;
            mPage.CurrentPageIndex = pageIndex;

            return View(mPage);
        }

        public ActionResult ScanList(int? pageindex = 1)
        {
            check();


            var scanlist = db.CFJControl;

            int pageIndex = pageindex ?? 1;
            int totalCount = scanlist.Count();
            ViewBag.totalCount = totalCount;
            PagedList<CFJControl> mPage = scanlist.OrderByDescending(c => c.CFJControlID).AsQueryable().ToPagedList(pageIndex, 10);
            mPage.TotalItemCount = totalCount;
            mPage.CurrentPageIndex = pageIndex;

            return View(mPage);
        }

        //public ActionResult MachineList(int? pageindex = 1)
        //{
        //    check();


        //    var machineList = db.CFJMachine.Where(c => c.enabled == 1);

        //    int pageIndex = pageindex ?? 1;
        //    int totalCount = machineList.Count();
        //    ViewBag.totalCount = totalCount;
        //    PagedList<CFJMachine> mPage = machineList.OrderByDescending(c => c.LastDate).AsQueryable().ToPagedList(pageIndex, 10);
        //    mPage.TotalItemCount = totalCount;
        //    mPage.CurrentPageIndex = pageIndex;

        //    return View(mPage);
        //}

        public void check()
        {
            if (CurMemberInfo == null)
            {
                System.Web.HttpContext.Current.Response.Redirect("/admincfj/");
            }
        }

    }
}
