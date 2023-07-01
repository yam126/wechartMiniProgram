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
                order.FromName = toname;
                order.FromPhone = phone;
                order.SayEtc = sayect;
                order.ThePass = thepass;

                //db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Redirect("~/Send?orderid=" + orderid);
        }


    }
}
