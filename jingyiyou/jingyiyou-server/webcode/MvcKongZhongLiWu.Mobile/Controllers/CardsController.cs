using ncc2019.Common;
using ncc2019.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class CardsController : ControllerBase
    {
        //
        // GET: /Cards/

        public ActionResult Index(string orderid)
        {
            var orderModel = GetOrder();

            //if (orderModel.Goods.GoodType == (int)GoodType.贺卡)
            //{
            //    if (CurMemberInfo != null && CurMemberInfo.MemnerID != orderModel.MemberID)
            //    {
            //        orderModel.ToMemberID = CurMemberInfo.MemnerID;
            //        orderModel.ToWeChatOpenid = CurMemberInfo.WeChatOpenid;
            //        orderModel.GivenStatus = (int)GivenStatus.已送出;
            //        db.SaveChanges();
            //    }
            //    if (!string.IsNullOrEmpty(orderModel.WxVioceMediaID))
            //    {
            //        ViewBag.hasvioce = true;
            //        ViewBag.viocesrc = "/upload_vioce/" + orderModel.WxVioceMediaID + ".mp3";
            //        ViewBag.mediaid = orderModel.WxVioceMediaID;
            //    }

            //    //ViewBag.js_json = TenPayManager.MakeUpJsParam();

            //    ViewBag.sayetc = orderModel.SayEtc;
            //    ViewBag.fromname = orderModel.FromName;
            //    //return View(orderModel.Goods.Param);
            //    return null;
            //}
            return View();
        }

    }
}
