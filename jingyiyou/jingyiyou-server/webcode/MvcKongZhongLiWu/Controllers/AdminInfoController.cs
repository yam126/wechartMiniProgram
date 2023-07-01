using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class AdminInfoController : ControllerAdminBase
    {
        

        //
        // GET: /AdminInfo/

        public ActionResult Index()
        {
            return View(db.Info.ToList());
        }

        //
        // GET: /AdminInfo/Details/5

        public ActionResult Details(int id = 0)
        {
            Info info = db.Info.Find(id);
            if (info == null)
            {
                return HttpNotFound();
            }
            return View(info);
        }

        //
        // GET: /AdminInfo/Create

        public ActionResult Create()
        {
            return View(new Info());
        }

        //
        // POST: /AdminInfo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Info info)
        {
            if (ModelState.IsValid)
            {
                info.AddDate = DateTime.Now;
                info.UpeDate = DateTime.Now;
                info.OpeUserID = CurMemberInfo.MemnerID;
                info.ViewCount = 0;
                db.Info.Add(info);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(info);
        }
        
        //
        // GET: /AdminInfo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Info info = db.Info.Find(id);
            if (info == null)
            {
                return HttpNotFound();
            }
            return View(info);
        }

        //
        // POST: /AdminInfo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Info info)
        {
            if (ModelState.IsValid)
            {
                info.UpeDate = DateTime.Now;
                info.OpeUserID = CurMemberInfo.MemnerID;
                db.Entry(info).State = EntityState.Modified;
                db.Entry(info).Property(c => c.AddDate).IsModified = false;
                db.Entry(info).Property(c => c.ViewCount).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(info);
        }

        //
        // GET: /AdminInfo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Info info = db.Info.Find(id);
            if (info == null)
            {
                return HttpNotFound();
            }
            return View(info);
        }

        //
        // POST: /AdminInfo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Info info = db.Info.Find(id);
            db.Info.Remove(info);
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