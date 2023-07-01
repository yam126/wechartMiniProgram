using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class FeedBackController : ControllerBaseNoCheck
    {
      
        //
        // GET: /FeedBack/

        public ActionResult Index()
        {
            return View(db.FeedBack.ToList());
        }

        //
        // GET: /FeedBack/Details/5

        public ActionResult Details(int id = 0)
        {
            FeedBack feedback = db.FeedBack.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        //
        // GET: /FeedBack/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FeedBack/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeedBack feedback)
        {
            if (ModelState.IsValid)
            {
                db.FeedBack.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(feedback);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(string thecontent)
        {
            FeedBack feedback = new FeedBack()
            {
                AddDate = DateTime.Now,
                TheContent = thecontent
            };
            if (ncc2019.Common.Tool.SessionHelper.CurMemberInfo!=null)
            {
                feedback.OperID = ncc2019.Common.Tool.SessionHelper.CurMemberInfo.MemnerID;
            }
            if (ModelState.IsValid)
            {
                db.FeedBack.Add(feedback);
                db.SaveChanges();
                ShowMessage("提交成功！");
               
            }

            return View("Index");
        }
       
        //
        // GET: /FeedBack/Edit/5

        public ActionResult Edit(int id = 0)
        {
            FeedBack feedback = db.FeedBack.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        //
        // POST: /FeedBack/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FeedBack feedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feedback);
        }

        //
        // GET: /FeedBack/Delete/5

        public ActionResult Delete(int id = 0)
        {
            FeedBack feedback = db.FeedBack.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        //
        // POST: /FeedBack/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FeedBack feedback = db.FeedBack.Find(id);
            db.FeedBack.Remove(feedback);
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