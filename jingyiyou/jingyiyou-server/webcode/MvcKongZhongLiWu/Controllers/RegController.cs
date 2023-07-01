using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class RegController : Controller
    {
        private ncc2019Entities db = new ncc2019Entities();

        //
        // GET: /Reg/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Reg/Details/5

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
        // GET: /Reg/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Reg/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Members member)
        {
            if (ModelState.IsValid)
            {
                member.RegDate = DateTime.Now;
                member.LoginCount = 1;
                member.Status = (int)Common.Enum.MemmberStatus.正常;
                member.LastDate = DateTime.Now;
                db.Members.Add(member);
                db.SaveChanges();
                return Redirect("~/Home");
            }

            return View(member);
        }

        //
        // GET: /Reg/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Members member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        //
        // POST: /Reg/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Members member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        //
        // GET: /Reg/Delete/5

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
        // POST: /Reg/Delete/5

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