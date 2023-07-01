using ncc2019.Common.BLL;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class MyOrderExController : MyOrderController
    {
        //
        // GET: /MyOrderEx/

        public ActionResult List(string code = "", string state = "")
        {
            
            var orders = db.Orders.Where(c => c.ToWeChatOpenid == this.CurMemberInfo.WeChatOpenid).OrderByDescending(c => c.AddDate);
            return View("~/Views/MyOrder/Index.cshtml", orders.ToList());

        }
        protected override bool CheckUser()
        {
            return true;
            //return base.CheckUser();

        }

    }
}
