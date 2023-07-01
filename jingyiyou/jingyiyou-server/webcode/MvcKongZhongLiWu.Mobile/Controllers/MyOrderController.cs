using ncc2019.Common.BLL;
using ncc2019.Common.Tool;
using ncc2019.Common.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                ViewBag.show = "您暂时还没有送出礼物呦！";
                var orders = db.Orders.Where(c => c.MemberID == this.CurMemberInfo.MemnerID
                    && c.GoodID != null && c.OrderType == (int)OrderType.普通订单).OrderByDescending(c => c.AddDate);
                return View(orders.ToList());
            }
            else
            {
                //收获的礼物
                ViewBag.Title = "收获的礼物";
                ViewBag.show = "您还没有收到礼物呦！";
                var orders = db.Orders.Where(c => (c.ToMemberID == this.CurMemberInfo.MemnerID
                    || c.ToWeChatOpenid == this.CurMemberInfo.WeChatOpenid)
                    && (c.IsForMe == (int)ShiFouStatus.否 || c.IsForMe == null)
                    && c.GoodID != null && c.OrderType == (int)OrderType.普通订单).OrderByDescending(c => c.AddDate);

                return View(orders.ToList());
            }


        }


        #region 路由信息

        public ActionResult RouteInfo(string orderid, string content)
        {

            var order = GetOrder();
            if (!string.IsNullOrEmpty(order.DeliveryNo) && !string.IsNullOrEmpty(order.DeliveryCompany))
            {
                ViewBag.json = RouteHelper.GetRouteInfo(order.DeliveryNo, order.DeliveryCompany);
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

        #endregion

        public ActionResult JiangJia()
        {
            ViewBag.type = "send";
            ViewBag.Title = "抢购单";
            ViewBag.show = "您暂时还没抢到东西呦！";
            var orders = db.Orders.Where(c => c.MemberID == this.CurMemberInfo.MemnerID
                && c.GoodID != null && c.OrderType == (int)OrderType.自动降价 && c.PayStatus == (int)PayStatus.已支付).OrderByDescending(c => c.AddDate);
            return View("/views/myorder/index.cshtml", orders.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}