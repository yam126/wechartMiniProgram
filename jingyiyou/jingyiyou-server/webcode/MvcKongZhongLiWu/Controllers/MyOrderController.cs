using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Tool;

namespace ncc2019.Controllers
{
    public class MyOrderController : ControllerBase
    {

        //
        // GET: /MyOrder/

        public ActionResult Index(string type)
        {
            ViewBag.type = type;
            if (type == "send")
            {//送出的礼物
                ViewBag.Title = "送出的礼物";
                var orders = db.Orders.Where(c => c.MemberID == SessionHelper.CurMemberInfo.MemnerID).OrderByDescending(c => c.AddDate);
                return View(orders.ToList());
            }
            else
            {
                //收获的礼物
                ViewBag.Title = "收获的礼物";
                var orders = db.Orders.Where(c => c.ToMemberID == SessionHelper.CurMemberInfo.MemnerID
                    || c.ToWeChatOpenid == SessionHelper.CurMemberInfo.WeChatOpenid).OrderByDescending(c => c.AddDate);

                return View("/views/myorder/MyOrderGet.cshtml", orders.ToList());
            }
            //var orders = db.Orders.Where(c => c.MemberID == SessionHelper.CurMemberInfo.MemnerID).OrderByDescending(c => c.AddDate);
            //return View(orders.ToList());
        }

        public ActionResult Route(string orderid = "0")
        {
            var order = GetOrder();
            if (!string.IsNullOrEmpty(order.DeliveryNo) && !string.IsNullOrEmpty(order.DeliveryCompany))
            {
                ViewBag.json = ncc2019.Common.BLL.RouteHelper.GetRouteInfo(order.DeliveryNo, order.DeliveryCompany);
            }
            else
            {
                ViewBag.json = "{data:[]}";
            }
          
            string com = Common.BLL.RouteHelper.GetComName(order.DeliveryCompany);
            if (!string.IsNullOrEmpty(order.DeliveryNo))
            {
                ViewBag.info = "快递公司：" + com + ",&nbsp;&nbsp;&nbsp;&nbsp;快递单号：" + order.DeliveryNo;
            }

            return View(order);
        }


        //
        // GET: /MyOrder/Details/5

        public ActionResult Details(string orderid)
        {
            var order = GetOrder();
            //var quan = db.Quan.Where(c => c.RefOrderID == order.OrderID
            //    && c.IsUsed == (int)Common.Enum.ShiFouStatus.是).FirstOrDefault();
            //if (quan != null)
            //{
            //    ViewBag.quan = quan.Price;
            //}
            return View(order);
        }
        public ActionResult DetailsGet(string orderid)
        {
            var order = GetOrder();
            //var quan = db.Quan.Where(c => c.RefOrderID == order.OrderID
            //    && c.IsUsed == (int)Common.Enum.ShiFouStatus.是).FirstOrDefault();
            //if (quan != null)
            //{
            //    ViewBag.quan = quan.Price;
            //}
            return View(order);
        }

        public ActionResult MyDetails(string orderid)
        {
            var order = GetOrder();
            return View(order);
        }
        //
        // GET: /MyOrder/Create

        public ActionResult Create()
        {
            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "Name");
            return View();
        }

        //
        // POST: /MyOrder/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Orders order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "Name", order.MemberID);
            return View(order);
        }

        //
        // GET: /MyOrder/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Orders order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "Name", order.MemberID);
            return View(order);
        }

        //
        // POST: /MyOrder/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Orders order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "Name", order.MemberID);
            return View(order);
        }

        //
        // GET: /MyOrder/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Orders order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // POST: /MyOrder/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders order = db.Orders.Find(id);
            db.Orders.Remove(order);
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