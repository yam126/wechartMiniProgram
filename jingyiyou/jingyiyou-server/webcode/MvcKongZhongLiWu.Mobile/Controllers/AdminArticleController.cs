using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class AdminArticleController : ControllerBase
    {
        //
        // GET: /AdminArticle/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            ViewBag.js_json = Common.TenPayManager.MakeUpJsParam();
            ViewBag.domain = "http://" + Common.BLL.SettingBLL.MobileDomain + "/";
            ViewBag.mid = Common.Tool.DESEncrypt.Encrypt(CurMemberInfo.MemnerID);
            var list = db.Info.OrderByDescending(c => c.InfoID);
            return View(list);
        }
    }
}
