using ncc2019.Common.Enum;
using ncc2019.Common.Tool;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class QrCodeController : ControllerBaseNoCheck
    {
        //
        // GET: /QrCode/

        public ActionResult Index()
        {
            return View();
        }

       

        public ActionResult ScanTest(string type)
        {
            string orderid = "";

            if (type == "bookmark")
            {
                ViewBag.js_json = Common.TenPayManager.MakeUpJsParam();
                Orders order = db.Orders.Where(c => c.OrderType == (int)OrderType.书签类二维码测试 && c.MemberID == CurMemberInfo.MemnerID).FirstOrDefault();

                if (order == null)
                {
                    //创建一个新的订单
                    order = new Orders()
                    {
                        AddDate = DateTime.Now,
                        MemberID = CurMemberInfo.MemnerID,
                        OrderStatus = (int)OrderStatus.正常,
                        PayStatus = (int)PayStatus.已支付,
                        ShortUrl = ShortUrlHelper.GetShortUrl(),
                        TranceStatus = (int)TransferStatus.未发货,
                        GivenStatus = (int)GivenStatus.未送出,
                        IsForMe = (int)ShiFouStatus.是,
                        OrderType = (int)OrderType.书签类二维码测试,
                        ToShowPrice = (int)ShiFouStatus.是,
                        TotalPayment = 0,
                        FromName = CurMemberInfo.Name
                        //EndDate = DateTime.Now.AddDays(endday)
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();

                    //跳转到编辑界面
                    orderid = GetOrderId_Encrypt(order.OrderID);
                    return Redirect("/qrcode/edit?orderid=" + orderid);
                }
                else
                {

                    orderid = GetOrderId_Encrypt(order.OrderID);
                    if (string.IsNullOrEmpty(order.SayEtc))
                    {
                        if (order.MemberID == CurMemberInfo.MemnerID)
                        {
                            return Redirect("/qrcode/edit?orderid=" + orderid);

                        }
                        else
                        {
                            return Redirect("/qrcode/show?orderid=" + orderid);
                        }

                    }
                    else
                    {
                        //如果已经设置了留言则直接展示
                        if (order.MemberID == CurMemberInfo.MemnerID)
                        {
                            ViewBag.canedit = true;
                        }
                        return Redirect("/qrcode/show?orderid=" + orderid);
                    }

                }




            }
            else
            {
                return HttpNotFound();
            }

        }

        /// <summary>
        /// 展示
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult Show(string orderid)
        {
            //ViewBag.js_json = Common.TenPayManager.MakeUpJsParam();

            var order = GetOrder();

            if (!string.IsNullOrEmpty(order.WxVioceMediaID))
            {
                ViewBag.hasvioce = true;
                ViewBag.viocesrc = "/upload_vioce/" + order.WxVioceMediaID + ".mp3";
                ViewBag.mediaid = order.WxVioceMediaID;
            }

            ViewBag.sayetc = order.SayEtc;
            ViewBag.fromname = order.FromName;

            #region 保存浏览人信息
            if (order.MemberID != CurMemberInfo.MemnerID)
            {
                order.ToMemberID = CurMemberInfo.MemnerID;
                db.SaveChanges();
            }


            #endregion

            //展示订单
            if (order.MemberID == CurMemberInfo.MemnerID)
            {
                //是自己打开的
                ViewBag.canedit = true;
                //可以编辑--在未有人浏览前
                if (order.ToMemberID != null)
                {
                    ViewBag.canedit = false;
                }

                return View("/views/cards/book/1.cshtml");

            }
            else
            {
                //如果不是自己打开的--展示留言
                return View("/views/cards/book/1.cshtml");
            }

        }
        public ActionResult Edit(string orderid)
        {
            ViewBag.js_json = Common.TenPayManager.MakeUpJsParam();
            var order = GetOrder();
            if (order.MemberID != CurMemberInfo.MemnerID)
            {
                ViewBag.canedit = false;
                //如果发现不是制作人打开了这个页面则强制跳转到展示页面
                return View("/views/cards/book/1.cshtml");
            }

            ViewBag.isiphone = UserAgentHelper.IsIhpnoe();
            //ViewBag.WxVioceMediaID = order.WxVioceMediaID;
            ViewBag.mediaid = order.WxVioceMediaID;
            ViewBag.content = order.SayEtc;

            return View("/views/qrcode/edit.cshtml");
        }
        [HttpPost]
        public ActionResult Edit(string orderid, string content, IEnumerable audiofiles)
        {
            var order = GetOrder();
            order.SayEtc = content;
            db.SaveChanges();


            foreach (HttpPostedFileBase audiofile in audiofiles)
            {
                if (audiofile != null)
                {
                    string mediaid = Guid.NewGuid().ToString().Replace("-", "");
                    string localpath = Server.MapPath("~/upload_vioce/") + mediaid + ".amr";
                    audiofile.SaveAs(localpath);
                    //进行本地的转换
                    string ffmpegPath = Server.MapPath("/Other/");
                    string amrPath = Server.MapPath("/upload_vioce/" + mediaid + ".amr");
                    string mp3Path = Server.MapPath("/upload_vioce/" + mediaid + ".mp3");
                    AmrConvertToMp3.ConvertToMp3(ffmpegPath, amrPath, mp3Path);
                    order.WxVioceMediaID = mediaid;
                    db.SaveChanges();
                }

            }



            return Redirect("/qrcode/show?orderid=" + orderid);
        }
        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult Privew(string orderid)
        {
            // CommonLog log=
            return View();
        }

        public ActionResult NativePay()
        {
            string AppId = "wx845dee61803241bd";
            string MchId = "1233345402";
            string Key = "865315EEBE03DA3FA739AFD399F3327A";
            RequestHandler nativeHandler = new RequestHandler(null);
            string timeStamp = TenPayV3Util.GetTimestamp();
            string nonceStr = TenPayV3Util.GetNoncestr();

            //商品Id，用户自行定义
            string productId = DateTime.Now.ToString("yyyyMMddHHmmss");

            nativeHandler.SetParameter("appid", AppId);
            nativeHandler.SetParameter("mch_id", MchId);
            nativeHandler.SetParameter("time_stamp", timeStamp);
            nativeHandler.SetParameter("nonce_str", nonceStr);
            nativeHandler.SetParameter("product_id", productId);
            string sign = nativeHandler.CreateMd5Sign("key", Key);

            var url = TenPayV3.NativePay(AppId, timeStamp, MchId, nonceStr, productId, sign);

            //BitMatrix bitMatrix;
            //bitMatrix = new MultiFormatWriter().encode(url, BarcodeFormat.QR_CODE, 600, 600);
            //BarcodeWriter bw = new BarcodeWriter();

            //var ms = new MemoryStream();
            //var bitmap = bw.Write(bitMatrix);
            //bitmap.Save(ms, ImageFormat.Png);
            ////return File(ms, "image/png");
            //ms.WriteTo(Response.OutputStream);
            //Response.ContentType = "image/png";
            ViewBag.url = url;
            return View();
        }

    }
}
