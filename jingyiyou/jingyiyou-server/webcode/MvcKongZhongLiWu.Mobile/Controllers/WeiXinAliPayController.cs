using ncc2019.Common.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MC = Com.AlipayM;

namespace ncc2019.Controllers
{
    public class WeiXinAliPayController : ControllerBaseNoCheck
    {
        //
        // GET: /WeiXinAliPay/

        public ActionResult Index()
        {
            //if (UserAgentHelper.IsWeiXin())
            //{
            //    return View("/views/zhongchou/weixinalipay.cshtml");
            //}
            //else
            //{
            //    var order = GetOrder();
            //    return AliPay(order);
            //}
            return null;
        }

    }
}
