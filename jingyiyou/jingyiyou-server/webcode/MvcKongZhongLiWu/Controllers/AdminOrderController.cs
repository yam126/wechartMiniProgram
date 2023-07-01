using ncc2019.Common.BLL;
using ncc2019.Common.Enum;
using ncc2019.Common.Weixin.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ncc2019.Controllers
{
    public class AdminOrderController : ControllerAdminBase
    {


        //
        // GET: /AdminOrder/

        public ActionResult Index(int? pageindex = 1, int paystatus = 0, int givenstatus = 0, int transferstatus = 0)
        {
            var orders = db.Orders.Where(c => c.OrderID > 0);
            if (paystatus != 0)
            {
                orders = orders.Where(c => c.PayStatus == paystatus);
            }
            if (transferstatus != 0)
            {
                orders = orders.Where(c => c.TranceStatus == transferstatus);
            }
            if (givenstatus != 0)
            {
                orders = orders.Where(c => c.GivenStatus == givenstatus);
            }
            int pageIndex = pageindex ?? 1;
            int totalCount = orders.Count();
            ViewBag.totalCount = totalCount;
            PagedList<Orders> mPage = orders.OrderByDescending(c => c.AddDate).AsQueryable().ToPagedList(pageIndex, 10);
            mPage.TotalItemCount = totalCount;
            mPage.CurrentPageIndex = pageIndex;



            return View(mPage);
        }
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult Send(string orderid = "")
        {
            ViewBag.orderid = orderid;
            var order = GetOrder();

            return View(order);
        }


        /// <summary>
        /// 保存快递单号和发货状态等
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="orderno"></param>
        /// <param name="com"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Send(string orderid = "", string deliverNo = "", string com = "")
        {
            ViewBag.orderid = orderid;
            var order = GetOrder();
            order.DeliveryCompany = com;
            order.DeliveryNo = deliverNo;
            order.TranceStatus = (int)Common.Enum.TransferStatus.已发货;


            try
            {
                #region 记录打开动作
                ActionLog actionLog = new ActionLog()
                {
                    ActionDate = DateTime.Now,
                    AtionType = (int)AtionType.发送礼物,
                    Title = "Ta的礼物已经邮寄出去！",
                    MemberID = this.CurMemberInfo.MemnerID,
                    OrderID = order.OrderID
                };
                db.ActionLog.Add(actionLog);

                #endregion

                var entry = db.Entry(order);
                //entry.State = EntityState.Unchanged;
                entry.Property(c => c.DeliveryCompany).IsModified = true;
                entry.Property(c => c.DeliveryNo).IsModified = true;
                entry.Property(c => c.TranceStatus).IsModified = true;
                db.SaveChanges();

                if (!string.IsNullOrEmpty(order.ToWeChatOpenid))
                {
                    //CustomHelper.SendText(order.ToWeChatOpenid, string.Format("您的礼物 {0} 已经发货了。{2} 单号为：{1}！"
                    //    , order.Goods.Name, order.DeliveryNo, RouteHelper.GetComName(order.DeliveryCompany)));
                }


                showSuccessMessage("保存成功！");
            }
            catch (Exception error)
            {
                ShowAlertMessage(error.Message);
                Common.Tool.LoggerHelper.Debug(error.ToString());

            }

            return View(order);
        }
        //
        // GET: /AdminOrder/Details/5

        public ActionResult Details(int id = 0)
        {
            Orders order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // GET: /AdminOrder/Create

        public ActionResult Create()
        {
            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "Name");
            ViewBag.GoodID = new SelectList(db.Goods, "GoodID", "Name");
            return View();
        }

        //
        // POST: /AdminOrder/Create

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
            ViewBag.GoodID = new SelectList(db.Goods, "GoodID", "Name", order.GoodID);
            return View(order);
        }

        //
        // GET: /AdminOrder/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Orders order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "Name", order.MemberID);
            ViewBag.GoodID = new SelectList(db.Goods, "GoodID", "Name", order.GoodID);
            return View(order);
        }

        //
        // POST: /AdminOrder/Edit/5

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
            ViewBag.GoodID = new SelectList(db.Goods, "GoodID", "Name", order.GoodID);
            return View(order);
        }

        //
        // GET: /AdminOrder/Delete/5

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
        // POST: /AdminOrder/Delete/5

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