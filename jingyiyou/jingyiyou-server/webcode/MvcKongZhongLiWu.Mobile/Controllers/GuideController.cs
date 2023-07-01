using ncc2019.Common;
using ncc2019.Common.Enum;
using ncc2019.Common.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class GuideController : ControllerBase
    {
        //
        // GET: /Guide/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 送给谁
        /// </summary>
        /// <returns></returns>
        public ActionResult SendTo()
        {

            return View();
        }

        public ActionResult COrder(int forme)
        {
            int endday = int.Parse(System.Configuration.ConfigurationManager.AppSettings["endday"]);
            var order = new Orders()
            {
                AddDate = DateTime.Now,
                MemberID = SessionHelper.CurMemberInfo.MemnerID,
                OrderStatus = (int)OrderStatus.正常,
                PayStatus = (int)PayStatus.未支付,
                ShortUrl = ShortUrlHelper.GetShortUrl(),
                TranceStatus = (int)TransferStatus.未发货,
                GivenStatus = (int)GivenStatus.未送出,
                IsForMe = forme
            };
            db.Orders.Add(order);
            db.SaveChanges();

            if (forme == (int)Common.Enum.ShiFouStatus.否)
            {
                return Redirect("/guide/toword?orderid=" + GetOrderId_Encrypt(order.OrderID));
                //return Redirect("/guide/sayetc?orderid=" + GetOrderId_Encrypt(order.OrderID));
            }
            else
            {
                return Redirect("/guide/goodsort/?orderid=" + GetOrderId_Encrypt(order.OrderID));
            }
        }
        /// <summary>
        /// 说点啥
        /// </summary>
        /// <returns></returns>
        public ActionResult SayEtc(string orderid)
        {
            ViewBag.orderid = orderid;
            return View();
        }
        /// <summary>
        /// 写文字祝福
        /// </summary>
        /// <returns></returns>
        public ActionResult ToWord(string orderid)
        {
            var order = GetOrder();
            ViewBag.mediaid = order.WxVioceMediaID;
            ViewBag.js_json = TenPayManager.MakeUpJsParam();

            return View();
        }
        [HttpPost]
        public ActionResult ToWord(string orderid, string content, int isme)
        {
            var order = GetOrder();
            order.SayEtc = content;
            db.SaveChanges();

            if (isme == (int)ShiFouStatus.是)
            {//选择权在购买者手里
                return Redirect("/guide/goodsort?orderid=" + GetOrderId_Encrypt(order.OrderID));
            }
            else
            {
                return Redirect("/guide/settingfor?orderid=" + GetOrderId_Encrypt(order.OrderID));
            }

        }
        /// <summary>
        /// 礼物分类
        /// </summary>
        /// <returns></returns>
        public ActionResult GoodSort(string orderid)
        {
            ViewBag.orderid = orderid;
            ViewBag.control = "guide";
            var sortList = db.GoodSort.Where(c => c.Enabled == (int)Enabled.启用).OrderBy(c => c.GoodSortOrder).ToList();
            return View(sortList);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Search(string key, string orderid, string t)
        {
            ViewBag.t = t;
            ViewBag.orderid = orderid;
            ViewBag.control = "guide";
            ViewBag.key = System.Web.HttpUtility.UrlEncode(key);
            ViewBag.title = System.Web.HttpUtility.UrlDecode(key);
            return View();
        }
        /// <summary>
        /// 产品详细
        /// </summary>
        /// <returns></returns>
        public ActionResult Good(int id = 0, string orderid = "")
        {

            var order = GetOrder();

            ViewBag.formaction = CurMemberInfo.MemnerID == order.MemberID ? "dobuy" : "doget";
            ViewBag.isforme = order.IsForMe == (int)ShiFouStatus.是 ? true : false;
            ViewBag.showprice = order.ToShowPrice == (int)ShiFouStatus.是 ? true : false;
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
        public ActionResult GoodEx(int id = 0, string orderid = "")
        {
            var order = GetOrder();
            ViewBag.toshowprice = order.ToShowPrice;
            ViewBag.action = "dobuy";
            ViewBag.control = "guide";

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
        /// 购买
        /// </summary>
        /// <param name="number"></param>
        /// <param name="goodid"></param>
        /// <param name="forme"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoBuy(string orderid, int number, int goodid, int forme, string property)
        {


            bool result = false;
            var good = db.Goods.Find(goodid);
            int endday = int.Parse(System.Configuration.ConfigurationManager.AppSettings["endday"]);
            var order = GetOrder();
            if (order != null)
            {
                try
                {
                    order.GoodID = good.GoodID;
                    order.Payment = good.Payment;
                    order.TotalPayment = number * good.Payment;
                    order.Property = property;
                    order.BuyNum = number;
                    order.EndDate = DateTime.Now.AddDays(endday);


                    db.SaveChanges();
                    result = true;
                }
                catch (Exception)
                {


                }

            }
            if (result == true)
            {
                if (order.IsForMe == (int)ShiFouStatus.是)
                {
                    //
                    return Redirect("/guide/address/?orderid=" + GetOrderId_Encrypt(order.OrderID));
                }
                else
                {

                    return Redirect("/guide/Setting/?orderid=" + GetOrderId_Encrypt(order.OrderID));
                }


            }
            else
            {
                ShowAlertMessage("保存出错，请稍后再试！");
                return View(string.Format("/guide/Good?id={0}&orderid={1}", goodid, orderid));
            }




        }
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult Setting(string orderid)
        {
            ViewBag.orderid = orderid;
            return View();
        }
        [HttpPost]
        public ActionResult Setting(string orderid, string noshowparice, string addressbysef
            , string toname, string tophone, string toaddress)
        {
            var order = GetOrder();
            order.ToShowPrice = noshowparice == "on" ? 0 : 1;
            if (addressbysef != "on")
            {
                order.ToName = toname;
                order.ToPhone = tophone;
                order.ToAddress = toaddress;
            }
            db.SaveChanges();

            return Redirect("/Pay/My/?orderid=" + orderid);
        }
        /// <summary>
        /// 为对方进行配置
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult SettingFor(string orderid)
        {

            var order = GetOrder();
            #region 显示一共有多少礼物可以让他们选

            string key = order.ToGoodSort;//TODO:多个逗号分隔的情况需要处理
            if (order.ToPriceZone != null)
            {
                string[] pricelist = order.ToPriceZone.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                decimal price1 = decimal.Parse(pricelist[0]);
                decimal price2 = 99999999;
                if (pricelist.Count() > 1)
                {
                    price2 = decimal.Parse(pricelist[1]);
                }

                var goods = db.Goods.Where(c => c.Status == (int)GoodStatus.上架 && c.Tags.Contains(key) && c.Payment >= price1 && c.Payment <= price2);
                int num = goods.Count();
                ViewBag.canselnum = num;

            }

            #endregion


            return View(order);
        }
        [HttpPost]
        public ActionResult SettingFor(string orderid, bool noshowparice, bool paylate)
        {
            var order = GetOrder();
            order.ToShowPrice = !noshowparice ? 1 : 0;//这里需要转换下意思
            order.PayLate = paylate ? 1 : 0;

            #region 将虚拟商品添加到订单中--用户选择的商品进行多退少补
            string[] pricelist = order.ToPriceZone.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            string maxprice = pricelist.Count() == 2 ? pricelist[1] : "-1";
            int goodid = 1;
            switch (maxprice)
            {
                case "100": goodid = 1; break;
                case "200": goodid = 2; break;
                case "300": goodid = 3; break;
                case "500": goodid = 5; break;
                case "-1": goodid = 9; break;
                default:
                    break;
            }


            var good = db.Goods.Find(goodid);
            order.GoodID = good.GoodID;
            order.Payment = good.Payment;
            order.BuyNum = 1;
            order.TotalPayment = good.Payment;


            #endregion

            db.SaveChanges();



            //return View(order);
            if (order.PayLate == (int)ShiFouStatus.是)
            {
                //直接发送
                return Redirect("/send?orderid=" + orderid);
            }
            else
            {
                return Redirect("/Pay/My/?orderid=" + orderid);
            }

        }
        /// <summary>
        /// 设置分类
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult SetGoodSort(string orderid)
        {
            ViewBag.orderid = orderid;
            var sortList = db.GoodSort.Where(c => c.Enabled == (int)Enabled.启用).OrderBy(c => c.GoodSortOrder).ToList();
            return View(sortList);
        }
        [HttpPost]
        public ActionResult SetGoodSort(string orderid, string sortname)
        {
            var order = GetOrder();
            order.ToGoodSort = sortname;
            db.SaveChanges();
            return Redirect("/guide/settingfor?orderid=" + orderid);
        }
        /// <summary>
        /// 价格区间
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult SetPriceZone(string orderid)
        {
            var order = GetOrder();

            //GetPriceZoneNum("0-100", order.ToGoodSort, 0, 100);
            //GetPriceZoneNum("100-200", order.ToGoodSort, 100, 200);
            //GetPriceZoneNum("200-300", order.ToGoodSort, 200, 300);
            //GetPriceZoneNum("200-300", order.ToGoodSort, 200, 300);
            //GetPriceZoneNum("300-500", order.ToGoodSort, 300, 500);
            return View();
        }
        [HttpPost]
        public ActionResult SetPriceZone(string orderid, string pricezone)
        {
            var order = GetOrder();
            order.ToPriceZone = pricezone;
            db.SaveChanges();
            return Redirect("/guide/settingfor?orderid=" + orderid);
        }

        public void GetPriceZoneNum(string pricetag, string sort, int minprice, int maxprice)
        {
            var goods = db.Goods.Where(c => c.Status == (int)GoodStatus.上架 && c.Tags.Contains(sort) && c.Payment >= minprice && c.Payment <= maxprice);

            int num = goods.Count();

            ViewData[pricetag] = num;


        }

        public ActionResult ToChoiceGood(string orderid)
        {

            var order = GetOrder();
            return View(order);
        }

        [HttpPost]
        public JsonResult AGoods(int amount, int last, string key, string order, string orderid, string sort = "")
        {

            var orderModel = GetOrder();

            string message = "";
            try
            {
                var goodlist = from c in db.Goods where c.Status == (int)Common.Enum.GoodStatus.上架 select c;

                if (orderModel != null)
                {
                    key = orderModel.ToGoodSort;//TODO:多个逗号分隔的情况需要处理
                    string[] pricelist = orderModel.ToPriceZone.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    decimal price1 = decimal.Parse(pricelist[0]);
                    decimal price2 = 99999999;
                    if (pricelist.Count() > 1)
                    {
                        price2 = decimal.Parse(pricelist[1]);
                    }
                    goodlist = from c in goodlist
                               where
                               c.Payment >= price1 && c.Payment <= price2
                               select c;

                }
                if (!string.IsNullOrEmpty(key))
                {
                    //goodlist = from c in goodlist where ("," + c.Tags + ",").Contains("," + key + ",") select c;
                    goodlist = from c in goodlist
                               where
                               c.Tags.Contains(key) || c.Name.Contains(key) || c.Intro.Contains(key)
                               select c;
                }

                goodlist = goodlist.OrderByDescending(c => c.AddDate);


                var result = from c in goodlist
                             select new
                             {
                                 c.GoodID,
                                 c.Name,
                                 c.ImgUrl,
                                 c.Payment,
                                 c.BuyCount,
                                 c.Tags
                             };

                return Json(result.Take(last + amount).Skip(last).ToList());
            }
            catch (Exception error)
            {

                message = error.Message;
            }




            return Json(new { error = message });
        }
        /// <summary>
        /// 获取礼物的环节
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="number"></param>
        /// <param name="goodid"></param>
        /// <param name="forme"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoGet(string orderid, int number, int goodid, int forme, string property)
        {


            bool result = false;
            var good = db.Goods.Find(goodid);
            int endday = int.Parse(System.Configuration.ConfigurationManager.AppSettings["endday"]);
            var order = GetOrder();
            if (order != null)
            {
                try
                {
                    order.GoodID = good.GoodID;
                    order.Payment = good.Payment;
                    order.TotalPayment = number * good.Payment;
                    order.Property = property;
                    order.BuyNum = number;
                    order.EndDate = DateTime.Now.AddDays(endday);


                    db.SaveChanges();
                    result = true;
                }
                catch (Exception)
                {


                }

            }
            if (result == true)
            {
                if (string.IsNullOrEmpty(order.ToAddress))
                {
                    return Redirect("/guide/Address/?orderid=" + GetOrderId_Encrypt(order.OrderID));
                }
                else
                {
                    return Redirect("/gift/ok/?orderid=" + GetOrderId_Encrypt(order.OrderID));
                }
                //跳转到支付界面


            }
            else
            {
                ShowAlertMessage("保存出错，请稍后再试！");
                return View(string.Format("/guide/GoodEx?id={0}&orderid={1}", goodid, orderid));
            }




        }

        public ActionResult Address(string orderid)
        {
            LoggerHelper.Debug("Address index");
            ViewBag.action = "address";
            ViewBag.control = "guide";
            var orderModel = GetOrder();
            return View("/views/address/index.cshtml", orderModel);
        }

        [HttpPost]
        public ActionResult Address(string orderid, string recname, string recphone, string recaddress)
        {
            var order = GetOrder();
            order.ToName = recname;
            order.ToPhone = recphone;
            order.ToAddress = recaddress;
            order.ToMemberID = CurMemberInfo.MemnerID;
            order.ToWeChatOpenid = CurMemberInfo.WeChatOpenid;
            order.GivenStatus = (int)Common.Enum.GivenStatus.已送出;

            db.SaveChanges();


            return Redirect("/Pay/My/?type=zc&orderid=" + GetOrderId_Encrypt(order.OrderID));


        }
    }
}
