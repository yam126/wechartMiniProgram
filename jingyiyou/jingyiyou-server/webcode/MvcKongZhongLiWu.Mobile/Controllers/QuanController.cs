using ncc2019.Common.Enum;
using ncc2019.Common.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class QuanController : ControllerBaseNoCheck
    {
        //
        // GET: /Quan/

        public ActionResult Index(string quancode)
        {
            ViewBag.quancode = quancode;
            return View();
        }
     

      
        public ActionResult TakenFail()
        {
            return View();
        }
    }
}
