using ncc2019.Common.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class ToDetailController : ControllerBase
    {
        //
        // GET: /ToDetail/
        //private ncc2019Entities db = new ncc2019Entities();
        public ActionResult Index(string orderid)
        {
            ViewBag.OrderID = orderid;
            return View();
        }
        public ActionResult Create(string orderid, string toname, string sayect, string phone, string memo, string thepass)
        {
            var order = db.Orders.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(orderid)));
            if (order != null)
            {
                order.ToName = toname;
                order.ToPhone = phone;
                order.SayEtc = sayect;
                order.ThePass = thepass;
                order.GivenStatus = (int)GivenStatus.已送出;
                
                db.SaveChanges();
            }
            return Redirect("~/MyOrder");
        }


    }
}
