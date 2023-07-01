using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class GiftController : ControllerBaseNoCheck
    {

        //
        // GET: /Gift/

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult Index(string shorturl)
        {
            var orderModel = db.Orders.Where(c => c.ShortUrl == shorturl).FirstOrDefault();
            if (orderModel != null)
            {
                //ViewBag.imgurl = "http://" + Common.BLL.SettingBLL.MobileDomain + "/gift/" + shorturl;

                ViewBag.WeiXinQrCode = "/API/GetOpenGiftQrCode?qraction=opengift&param=" + shorturl;
            }
            else
            {
                ShowAlertMessage("请确认打开链接是否有效！");
            }

            //ViewBag.shorturl = shorturl;
            return View("~/Views/Gift/jump.cshtml");

        }
        public ActionResult Verf(string thepass, string shorturl)
        {

            var orderModel = db.Orders.Where(c => c.ShortUrl == shorturl).FirstOrDefault();
            if (orderModel.ThePass != thepass)
            {
                ViewBag.shorturl = shorturl;
                Response.Write("<script>alert('密码错误！')</script>");
                return View("~/Views/Gift/jump.cshtml");
            }
            if (!string.IsNullOrEmpty(orderModel.ToName))
            {
                return View("~/Views/GiftX/index.cshtml", orderModel);
            }
            else
            {
                return View("~/Views/Gift/index.cshtml", orderModel);
            }
        }
        /// <summary>
        /// 保存订单配送信息
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="recname"></param>
        /// <param name="recphone"></param>
        /// <param name="recaddress"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        public ActionResult DoCreate(string orderid, string recname, string recphone, string recaddress, string memo)
        {
            var orderModel = db.Orders.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(orderid)));
            orderModel.ToName = recname;
            orderModel.ToPhone = recphone;
            orderModel.ToAddress = recaddress;
            orderModel.Memo = memo;


            //db.Entry(orderModel).State = EntityState.Modified;
            db.SaveChanges();

            return Redirect("~/Gift/OK?orderid=" + orderid);
        }
        /// <summary>
        /// 配送成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult OK(string orderid)
        {
            @ViewBag.orderid = orderid;
            return View();
        }
    }
}
