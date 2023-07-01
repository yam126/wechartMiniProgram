using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class SendController : ControllerBase
    {
        //
        // GET: /Send/

        public ActionResult Index(string orderid)
        {
            var order = GetOrder();
            if (order.PayStatus == (int)Common.Enum.PayStatus.未支付)
            {
                return Redirect("/pay/?orderid=" + orderid);
            }

            ViewBag.WeiXinQrCode = "/API/GetQrCode?qraction=getgift&param=" + orderid;
            return View();
        }

    }
}
