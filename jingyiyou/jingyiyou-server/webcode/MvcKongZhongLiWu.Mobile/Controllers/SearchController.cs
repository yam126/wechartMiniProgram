using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class SearchController : ControllerBaseNoCheck
    {

        //
        // GET: /Search/

        public ActionResult Index(string key, string sort, string sorttype, string order, string t = "")
        {
            if (t == "")
            {
                int temp = new Random().Next(0, 100000000);
                return Redirect(Request.Url.PathAndQuery + "&t=" + temp);

            }
            ViewBag.t = t;
            ViewBag.key = System.Web.HttpUtility.UrlEncode(key);
            ViewBag.title = System.Web.HttpUtility.UrlDecode(key);
            ViewBag.link = "/Home/Details?id={{GoodID}}";
            ViewBag.goodtag = "礼物价格";
            if (key == "自动降价")
            {
                ViewBag.showjjdes = true;
                ViewBag.goodtag = "当前价格";
                ViewBag.link = "/jiangjia/good?id={{GoodID}}";
            }
            return View();
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="amount">pagesize</param>
        /// <param name="last">获取过多少</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AGoods(int amount, int last, string key, string order, string sort = "")
        {


        

            string message = "";
            try
            {
                var goodlist = from c in db.Goods where c.Status == (int)Common.Enum.GoodStatus.上架 select c;
                if (!string.IsNullOrEmpty(key))
                {
                    //goodlist = from c in goodlist where ("," + c.Tags + ",").Contains("," + key + ",") select c;
                    goodlist = from c in goodlist
                               where
                               c.Tags.Contains(key) || c.Name.Contains(key) || c.Intro.Contains(key)
                               select c;
                }


                #region 按默认排序
                if (sort == "")
                {

                    goodlist = from c in goodlist
                               orderby c.GoodOrder descending
                               , c.GoodID descending
                               select c;

                }
                #endregion

                #region 按销量排序
                if (sort == "seal")
                {
                    if (order == "up")
                    {
                        goodlist = from c in goodlist
                                   orderby c.BuyCount, c.GoodID
                                   select c;
                    }
                    else
                    {
                        goodlist = from c in goodlist
                                   orderby c.BuyCount descending, c.GoodID descending
                                   select c;
                    }

                }
                #endregion

                #region 按价格排序
                if (sort == "price")
                {
                    if (order == "up")
                    {
                        goodlist = from c in goodlist
                                   orderby c.Payment, c.GoodID
                                   select c;
                    }
                    else
                    {
                        goodlist = from c in goodlist
                                   orderby c.Payment descending, c.GoodID descending
                                   select c;
                    }


                }
                #endregion



                var result = from c in goodlist
                             select new
                             {
                                 c.GoodID,
                                 c.Name,
                                 c.ImgUrl,
                                 c.Payment,
                                 c.BuyCount,
                                 c.Tags,
                                 isover = DateTime.Now > c.EndDate ? true : false
                             };

                return Json(result.Take(last + amount).Skip(last).ToList());
            }
            catch (Exception error)
            {

                message = error.Message;
            }




            return Json(new { error = message });
        }


    }
}
