using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Models;

namespace ncc2019.Controllers
{
    public class AdminGoodSortController : ControllerAdminBase
    {
       

        //
        // GET: /AdminGoodSort/

        public ActionResult Index()
        {
            return View(db.GoodSort.ToList());
        }

        //
        // GET: /AdminGoodSort/Details/5

        public ActionResult Details(int id = 0)
        {
            GoodSort goodsort = db.GoodSort.Find(id);
            if (goodsort == null)
            {
                return HttpNotFound();
            }
            return View(goodsort);
        }

        //
        // GET: /AdminGoodSort/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminGoodSort/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GoodSort goodsort)
        {
            if (ModelState.IsValid)
            {
                goodsort.AddDate = DateTime.Now;
                goodsort.AddMemberID = CurMemberInfo.MemnerID;
                db.GoodSort.Add(goodsort);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goodsort);
        }

        //
        // GET: /AdminGoodSort/Edit/5

        public ActionResult Edit(int id = 0)
        {
            GoodSort goodsort = db.GoodSort.Find(id);
            if (goodsort == null)
            {
                return HttpNotFound();
            }
            return View(goodsort);
        }

        //
        // POST: /AdminGoodSort/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GoodSort goodsort)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goodsort).State = EntityState.Modified;
                db.Entry(goodsort).Property(c => c.AddDate).IsModified = false;
                db.Entry(goodsort).Property(c => c.AddMemberID).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goodsort);
        }

        //
        // GET: /AdminGoodSort/Delete/5

        public ActionResult Delete(int id = 0)
        {
            GoodSort goodsort = db.GoodSort.Find(id);
            if (goodsort == null)
            {
                return HttpNotFound();
            }
            return View(goodsort);
        }

        //
        // POST: /AdminGoodSort/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GoodSort goodsort = db.GoodSort.Find(id);
            db.GoodSort.Remove(goodsort);
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