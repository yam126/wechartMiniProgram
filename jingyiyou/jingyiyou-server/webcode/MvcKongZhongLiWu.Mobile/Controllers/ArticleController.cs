using ncc2019.Common.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Enum;

namespace ncc2019.Controllers
{
    public class ArticleController : ControllerBaseNoCheck
    {
        //
        // GET: /Article/

        public ActionResult Index(int id = 0, string mid = "")
        {
            ViewBag.infoid = id;
            ViewBag.js_json = Common.TenPayManager.MakeUpJsParam();
            ViewBag.domain = "http://" + Common.BLL.SettingBLL.MobileDomain + "/";
            SessionHelper.SetSession("premid", mid);
            if (CurMemberInfo != null)
            {
                ViewBag.mid = Common.Tool.DESEncrypt.Encrypt(CurMemberInfo.MemnerID);
            }
            var info = db.Info.Find(id);
            if (info == null)
            {
                info = new Info();
            }
            if (!string.IsNullOrEmpty(mid))
            {
                string quanmid = Common.Tool.DESEncrypt.Decrypt(mid);
                string curmid = CurMemberInfo.MemnerID.ToString();
                //加载券
                //Quan quan = db.Quan.Where(c => c.TheType == (int)QuanType.一个微信号生成一个
                //    && (c.Param == quanmid || c.Param == curmid)).OrderByDescending(c => c.QuanID).FirstOrDefault();
                //if (quan == null)
                //{
                //    quan = new Quan()
                //    {
                //        IsUsed = (int)ShiFouStatus.否,
                //        Price = 10,
                //        QuanCode = ShortUrlHelper.GetQuanCode(),
                //        Param = quanmid,
                //        TheType = (int)QuanType.一个微信号生成一个

                //    };
                //    db.Quan.Add(quan);
                //    db.SaveChanges();
                //}
                //ViewBag.quancode = quan.QuanCode;
            }

            if (id==19)
            {
                return View("/views/article/bookmark.cshtml");
            }
            return View(info);
        }


        public ActionResult List()
        {
            ViewBag.js_json = Common.TenPayManager.MakeUpJsParam();
            ViewBag.domain = "http://" + Common.BLL.SettingBLL.MobileDomain + "/";
            var info = db.Info.Find(12);
            return View(info);
        }

    }
}
