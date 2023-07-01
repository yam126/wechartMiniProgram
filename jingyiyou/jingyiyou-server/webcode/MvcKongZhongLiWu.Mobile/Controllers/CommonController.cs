using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 玩法介绍
        /// </summary>
        /// <returns></returns>
        public ActionResult WanFa()
        {

            return View();
        }

        public ActionResult BackToWeiXin(string type, string goodid)
        {
            ViewBag.jumpurl = "http://weixin.qq.com/r/hUzA2IbEcNsPrcK79xmX";
            if (type == "jj")
            {
                ViewBag.jumpurl = "/jingjia/ok?type=jj&goodid=" + goodid;
                ViewBag.intro = "恭喜你购买成功，我们会立即为您进行配送，";
            }
            return View();
        }

        public ActionResult Message()
        {
            ViewBag.message = "业主没有设置任何密码";
            return View();
        }


    }
}
