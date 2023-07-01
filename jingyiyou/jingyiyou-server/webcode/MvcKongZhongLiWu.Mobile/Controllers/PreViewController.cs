using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class PreViewController : Controller
    {
        private ncc2019Entities db = new ncc2019Entities();
        //
        // GET: /PreView/

        public ActionResult Index(string orderid)
        {
            var order = db.Orders.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(orderid)));
            return View(order);
        }

    }
}
