using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class AdminLoginLogController : Controller
    {
        private ncc2019Entities db = new ncc2019Entities();

        //
        // GET: /AdminLoginLog/

        public ActionResult Index()
        {
            return View(db.LoginLog.ToList());
        }

        //
        // GET: /AdminLoginLog/Details/5

        public ActionResult Details(int id = 0)
        {
            LoginLog loginlog = db.LoginLog.Find(id);
            if (loginlog == null)
            {
                return HttpNotFound();
            }
            return View(loginlog);
        }

        //
        // GET: /AdminLoginLog/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminLoginLog/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LoginLog loginlog)
        {
            if (ModelState.IsValid)
            {
                db.LoginLog.Add(loginlog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loginlog);
        }

        //
        // GET: /AdminLoginLog/Edit/5

        public ActionResult Edit(int id = 0)
        {
            LoginLog loginlog = db.LoginLog.Find(id);
            if (loginlog == null)
            {
                return HttpNotFound();
            }
            return View(loginlog);
        }

        //
        // POST: /AdminLoginLog/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LoginLog loginlog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loginlog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loginlog);
        }

        //
        // GET: /AdminLoginLog/Delete/5

        public ActionResult Delete(int id = 0)
        {
            LoginLog loginlog = db.LoginLog.Find(id);
            if (loginlog == null)
            {
                return HttpNotFound();
            }
            return View(loginlog);
        }

        //
        // POST: /AdminLoginLog/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoginLog loginlog = db.LoginLog.Find(id);
            db.LoginLog.Remove(loginlog);
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