using ncc2019.Common.Weixin.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;


namespace ncc2019.Controllers
{
    public class AdminMemberController : ControllerAdminBase
    {

        //
        // GET: /AdminMember/

        public ActionResult Index(int? pageindex = 1, int guanzhu = -1)
        {
            var members = db.Members.Where(c => c.MemberID != -1);
            if (guanzhu != -1)
            {
                members = members.Where(c => c.IsGuanZhu == guanzhu);
            }
            int pageIndex = pageindex ?? 1;
            int totalCount = members.Count();
            ViewBag.totalCount = totalCount;
            PagedList<Members> mPage = members.OrderByDescending(c => c.MemberID).AsQueryable().ToPagedList(pageIndex, 10);
            mPage.TotalItemCount = totalCount;
            mPage.CurrentPageIndex = pageIndex;

            return View(mPage);
        }

        public ActionResult SendMsg(int id = 0)
        {
            ViewBag.mid = id;
            
            return View();
        }
        [HttpPost]
        public ActionResult SendMsg(int mid = 0, string msg = "")
        {
            var members = db.Members.Find(mid);
            CustomHelper.SendText(members.WechatOpenid,
                        string.Format("{0}您好,欢迎来到空中礼物。<a href='http://www.baidu.com'>点击进入百度</a>", ""));
            return View();
        }

        
        //
        // GET: /AdminMember/Details/5

        public ActionResult Details(int id = 0)
        {
            Members member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        //
        // GET: /AdminMember/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminMember/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Members member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        //
        // GET: /AdminMember/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Members member = db.Members.Find(id);

            return View(member);
        }

        //
        // POST: /AdminMember/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Members member)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(member).State = EntityState.Modified;
                db.Members.Add(member);
                var entry = db.Entry(member);
                entry.State = EntityState.Unchanged;
                entry.Property(e => e.Birth).IsModified = true;
                entry.Property(e => e.Name).IsModified = true;
                entry.Property(e => e.LoginName).IsModified = true;
                entry.Property(e => e.Password).IsModified = true;
                //                entry.Property(e => e.LoginCount).IsModified = true;
                entry.Property(e => e.IsGuanZhu).IsModified = true;
                entry.Property(e => e.WechatOpenid).IsModified = true;
                entry.Property(e => e.Status).IsModified = true;
                entry.Property(e => e.UserLevel).IsModified = true;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        //
        // GET: /AdminMember/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Members member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        //
        // POST: /AdminMember/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Members member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}