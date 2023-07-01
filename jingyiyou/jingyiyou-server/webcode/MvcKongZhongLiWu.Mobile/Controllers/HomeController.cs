using ncc2019.Common.BLL;
using ncc2019.Common.Enum;
using ncc2019.Common.Tool;
using ncc2019.Common.Weixin.Base;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class HomeController : ControllerBaseNoCheck
    {

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return Redirect("/ncc/");
            //var guanzhuTip = "艾玛可来了，\n赶紧免费抽大奖吧！\r\n<a href='http://m.kongzhongliwu.com/prize?f=guanzhu'>戳我抽大奖~~</a>\r\n是不是该拼拼人气，让小伙伴们给你众筹礼物啦？\r\n<a href='http://m.kongzhongliwu.com/zhongchou/?f=guanzhu'>戳我筹礼物~~</a>\r\n更多创新的礼物交互方式，等你去发现......";
            //CustomHelper.SendText("o2masuN6nytq05vP54oex35hZVQc", guanzhuTip);
            //return Redirect("http://weixin.qq.com/q/XkzdcqDml9vuh2Tt6mKX");
            //SessionHelper.InitJumpUrl("");

            //ViewBag.IsIndex = true;
            //ViewBag.GoodSort = db.GoodSort.ToList();
            ////var goodlist = db.Goods.Where(c => c.Status == (int)GoodStatus.上架).ToList();
            //return View();
        }

        /// <summary>
        /// 礼物详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id = 0)
        {
            Goods good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            else
            {
                good.ViewCount++;
                db.Entry(good).Property(c => c.ViewCount).IsModified = true;

              

                db.SaveChanges();

                var property = db.GoodProperty.Where(c => c.GoodID == id).OrderBy(c => c.GoodPropertyID).ToList();
                ViewBag.plist = property;
            }
            return View(good);
        }

        /// <summary>
        /// 分类
        /// </summary>
        /// <returns></returns>
        public ActionResult Category()
        {
            var sortList = db.GoodSort.Where(c => c.Enabled == (int)Enabled.启用).OrderBy(c => c.GoodSortOrder).ToList();
            return View(sortList);
        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult FeedBack()
        {
            return View();
        }

        public ActionResult SaveFeedBack(string content)
        {
            FeedBack feedback = new FeedBack()
            {
                AddDate = DateTime.Now,
                TheContent = content
            };
            if (this.CurMemberInfo != null)
            {
                feedback.OperID = this.CurMemberInfo.MemnerID;
            }
            if (ModelState.IsValid)
            {
                db.FeedBack.Add(feedback);
                db.SaveChanges();
                showSuccessMessage("保存成功！  感谢您对我们的支持与鼓励。");

            }

            return View("/views/home/feedback.cshtml");
        }
        /// <summary>
        /// 我也去买一个
        /// </summary>
        /// <returns></returns>
        public ActionResult BuyOne(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ViewBag.showgood = true;
                string keyStr = Common.Tool.DESEncrypt.Decrypt(key);
                string[] keyArry = keyStr.Split('_');
                var order = db.Orders.Find(int.Parse(keyArry[1]));
                var member = db.Members.Find(order.MemberID);
                if (member != null)
                {
                    ViewBag.fromimgurl = member.HeadImgUrl;
                }
                return View(order);
            }
            else
            {
                ViewBag.showgood = false;
                return View();
            }

        }

    }
}