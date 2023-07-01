using ncc2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class QingTieController : ControllerBaseNoCheck
    {
        //
        // GET: /QingTie/

        public ActionResult Index()
        {
            ViewBag.js_json = TenPayManager.MakeUpJsParam();
            return View();
        }

        public ActionResult NiangJia()
        {
            ViewBag.js_json = TenPayManager.MakeUpJsParam();
            return View("/views/qingtie/niangjia.cshtml");
        }
        public ActionResult XiangCe()
        {
            ViewBag.js_json = TenPayManager.MakeUpJsParam();
            return View();
        }

        public ActionResult JiuDian()
        {
            ViewBag.js_json = TenPayManager.MakeUpJsParam();
            return View();
        }
        public ActionResult FeedBack(string qname, string qjoin, string qnum, string qphone, string qzhufu)
        {
            if (!string.IsNullOrEmpty(qname))
            {
                string joinnum = qnum;
                if (qjoin == "0")
                {
                    joinnum = "0";
                }

             
                db.SaveChanges();

            }
            return Redirect("/qingtie/");
        }


        public ActionResult SMSAlert(string remindname, string remindphone)
        {
            if (!string.IsNullOrEmpty(remindname))
            {
               
                db.SaveChanges();
            }
            return Redirect("/qingtie/");
        }
    }
}
