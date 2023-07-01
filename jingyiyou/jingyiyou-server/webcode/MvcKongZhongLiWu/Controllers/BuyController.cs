using ncc2019.Common.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Enum;
using Com.Alipay;
using AM = Com.AlipayM;
using System.Collections.Specialized;
using System.Xml;
using System.Data;
using ncc2019.Common.BLL;

namespace ncc2019.Controllers
{
    public class BuyController : ControllerBase
    {
        //
        // GET: /Buy/

        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int id, int num)
        {
            var good = db.Goods.Find(id);
            if (num <= 0)
            {
                ViewBag.num = 1;
            }
            else
            {
                ViewBag.num = num;
            }


            return View(good);
        }

        public ActionResult DoBuy(int number, int goodid, int forme = 0)
        {

            try
            {
                var good = db.Goods.Find(goodid);
                int endday = int.Parse(System.Configuration.ConfigurationManager.AppSettings["endday"]);
                var order = new Orders()
                {
                    AddDate = DateTime.Now,
                    BuyNum = number,
                    GoodID = goodid,
                    MemberID = SessionHelper.CurMemberInfo.MemnerID,
                    OrderStatus = (int)OrderStatus.正常,
                    Payment = good.Payment,
                    PayStatus = (int)PayStatus.未支付,
                    ShortUrl = ShortUrlHelper.GetShortUrl(),
                    TotalPayment = number * good.Payment,
                    TranceStatus = (int)TransferStatus.未发货,
                    GivenStatus = (int)GivenStatus.未送出,
                    EndDate = DateTime.Now.AddDays(endday),
                    IsForMe = forme
                };

                db.Orders.Add(order);
                db.SaveChanges();
                if (order.IsForMe==(int)ShiFouStatus.是)
                {
                    //跳转到支付界面
                    return Redirect("/Pay/My?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID));
                }
                else
                {
                    //跳转到支付界面
                    return Redirect("/Pay/?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID));
                }
                
            }
            catch (Exception)
            {

                throw;
            }


        }






    }
}
