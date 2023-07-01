using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ncc2019.Controllers
{
    public class AdminTagController : Controller
    {
        private ncc2019Entities db = new ncc2019Entities();

        //
        // GET: /AdminTag/

        //
        // GET: /AdminTag/Create

        public ActionResult Create()
        {
            return View();
        }

       

      
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}