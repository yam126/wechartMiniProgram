using ncc2019.Common;
using ncc2019.Common.BLL;
using ncc2019.Common.Enum;
using ncc2019.Common.Tool;
using ncc2019.Common.Weixin.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MC = Com.AlipayM;

namespace ncc2019.Controllers
{
    public class ZhongChouController : ControllerBase
    {
        //
        // GET: /ZhongChou/

        public ActionResult Index(string orderid = "")
        {
            if (!string.IsNullOrEmpty(orderid))
            {
                var order = GetOrder();
                ViewBag.goodid = order.GoodID;
                if (order.OrderStatus == (int)OrderStatus.众筹中)
                {
                    return Redirect("/zhongchou/showlist");
                }
                return View(order);
            }

            return View();
        }

        public ActionResult zhiguoguo()
        {
            int orderCount = db.Orders.Where(c => c.PayStatus == (int)(Common.Enum.PayStatus.已支付)).Count();
            ViewBag.rs = orderCount;
            ViewBag.jd = (orderCount * 100 / 100);
            return View();
        }

        public ActionResult DoBuy()
        {
            decimal payment = decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["payment"]);
            var order = new Orders()
            {
                AddDate = DateTime.Now,
                GoodID = 1,
                MemberID = CurMemberInfo.MemnerID,
                OrderStatus = (int)OrderStatus.未开启众筹,
                PayStatus = (int)PayStatus.未支付,
                ShortUrl = ShortUrlHelper.GetShortUrl(),
                TranceStatus = (int)TransferStatus.未发货,
                GivenStatus = (int)GivenStatus.未送出,
                IsForMe = (int)ShiFouStatus.是,
                OrderType = (int)OrderType.众筹订单,
                NeedPay = payment,
                ZhongChouPay = payment,
                TotalPayment = payment,
                Payment = payment,
                BuyNum = 1,
                ToShowPrice = (int)ShiFouStatus.是,
                EndDate = new DateTime(2017, 6, 10, 23, 59, 59)
            };
            db.Orders.Add(order);
            db.SaveChanges();

            return Redirect("/guide/address?orderid=" + GetOrderId_Encrypt(order.OrderID));
        }
        [HttpPost]
        public ActionResult DoPay(string orderid)
        {

            return Redirect("/pay/weixinalipay?orderid=" + orderid);


        }
        public ActionResult ZGGPay(string orderid)
        {

            var order = GetOrder();


            //ViewBag.openid = this.CurMemberInfo.WeChatOpenid;
            ViewBag.paydata = Newtonsoft.Json.JsonConvert.SerializeObject(TenPayManager.MakeUpJsParam(CurMemberInfo.WeChatOpenid, order));

            //ViewBag.yue = SessionHelper.CurMemberInfo.Balance;
            ViewBag.totalpay = order.TotalPayment;
            //ViewBag.apiurl = Common.BLL.SettingBLL.WebDomain;

            return View("~/views/zhongchou/zggpay.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult ZGGOK(string orderid)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string orderid = "", string goodid = "", string content = "")
        {
            var order = GetOrder();

            if (order != null)
            {
                order.SayEtc = content;
                order.OrderStatus = (int)OrderStatus.众筹中;
                db.SaveChanges();
                return Redirect("/zhongchou/sendok?orderid=" + orderid);
            }
            else
            {
                return HttpNotFound();
            }



        }
        public ActionResult FeedBack()
        {
            return View();
        }

        public string DoFeedback(string content)
        {
            FeedBack feedbackModel = new FeedBack()
            {
                AddDate = DateTime.Now,
                TheContent = content,
                OperID = CurMemberInfo.MemnerID
            };
            db.FeedBack.Add(feedbackModel);
            db.SaveChanges();

            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult COrder(string goodid)
        {

            int endday = int.Parse(System.Configuration.ConfigurationManager.AppSettings["endday"]);
            var order = new Orders()
            {
                AddDate = DateTime.Now,
                MemberID = CurMemberInfo.MemnerID,
                OrderStatus = (int)OrderStatus.未开启众筹,
                PayStatus = (int)PayStatus.未支付,
                ShortUrl = ShortUrlHelper.GetShortUrl(),
                TranceStatus = (int)TransferStatus.未发货,
                GivenStatus = (int)GivenStatus.未送出,
                IsForMe = (int)ShiFouStatus.是,
                OrderType = (int)OrderType.众筹订单,
                ZhongChouPay = 0,
                ToShowPrice = (int)ShiFouStatus.是,
                EndDate = DateTime.Now.AddDays(endday)
            };
            db.Orders.Add(order);


            if (!string.IsNullOrEmpty(goodid))
            {
                //如果参数已经指明了礼物id
                order.GoodID = int.Parse(goodid);
                db.SaveChanges();
                return Redirect("/zhongchou/?orderid=" + GetOrderId_Encrypt(order.OrderID));
            }
            else
            {
                db.SaveChanges();
                return Redirect("/zhongchou/GoodSort?orderid=" + GetOrderId_Encrypt(order.OrderID));
            }

        }

        /// <summary>
        /// 选择分类
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult GoodSort(string orderid)
        {
            ViewBag.orderid = orderid;
            ViewBag.control = "zhongchou";
            return View("/views/guide/goodsort.cshtml");
        }

        public ActionResult Search(string orderid, string key, string t)
        {
            ViewBag.t = t;
            ViewBag.orderid = orderid;
            ViewBag.control = "zhongchou";
            ViewBag.key = System.Web.HttpUtility.UrlEncode(key);
            ViewBag.title = System.Web.HttpUtility.UrlDecode(key);
            return View("/views/guide/search.cshtml");
        }
        /// <summary>
        /// 选择礼物
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult Good(string orderid, int id, string type)
        {
            var order = GetOrder();
            ViewBag.toshowprice = order.ToShowPrice;
            ViewBag.action = "dobuy";
            ViewBag.control = "zhongchou";
            ViewBag.type = type;

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

            return View("/views/guide/goodex.cshtml", good);
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
            //int endday = int.Parse(System.Configuration.ConfigurationManager.AppSettings["endday"]);
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
                    //order.EndDate = DateTime.Now.AddDays(endday);

                    db.SaveChanges();
                    result = true;
                }
                catch (Exception)
                {


                }

            }
            if (result == true)
            {


                return Redirect("/zhongchou/?orderid=" + GetOrderId_Encrypt(order.OrderID));



            }
            else
            {
                ShowAlertMessage("保存出错，请稍后再试！");
                return View(string.Format("/views/guide/goodex.cshtml?orderid={1}", goodid, orderid));
            }




        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult OK(string orderid)
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult PayDetail(string orderid)
        {
            ViewBag.orderid = orderid;
            var order = GetOrder();
            decimal totalpayment = order.TotalPayment.Value - ZhongChouPayBLL.GetTotalPaymentByOrderID(db, int.Parse(GetOrderId_Decrypt(orderid)));
            if (totalpayment < 0)
            {
                totalpayment = 0;
                if (order.OrderStatus == (int)OrderStatus.众筹中)
                {
                    order.OrderStatus = (int)OrderStatus.众筹成功;
                    db.SaveChanges();
                }
            }
            ViewBag.totalpayment = totalpayment;

            return View();
        }
        [HttpPost]
        public ActionResult PayDetail(string orderid, decimal pay, string content)
        {
            //int count = db.Database.ExecuteSqlCommand("update orders set zhongzhoupay=zhongzhoupay+{0},content='{1}' where orderid={2} "
            //      , pay, content, orderid);
            //if (count == 0)
            //{
            //    //保存失败
            //    ShowAlertMessage("提交保存失败！");
            //}
            var order = GetOrder();

            if (!string.IsNullOrEmpty(content))
            {
                Comments commentModel = new Comments()
                {
                    AddDate = DateTime.Now,
                    Content = content,
                    MemberID = CurMemberInfo.MemnerID,
                    Name = CurMemberInfo.Name,
                    HeadImgUrl = CurMemberInfo.HeadImgUrl,
                    ToMemberID = order.MemberID,
                    OrderID = order.OrderID
                };
                db.Comments.Add(commentModel);
            }


           

            db.SaveChanges();


            return Redirect("/Pay/My/?type=zc&orderid=" + orderid);
        }
        
        private string AliPay(Orders order)
        {
            //支付宝网关地址
            string GATEWAY_NEW = "http://wappaygw.alipay.com/service/rest.htm?";

            ////////////////////////////////////////////调用授权接口alipay.wap.trade.create.direct获取授权码token////////////////////////////////////////////

            //返回格式
            string format = "xml";
            //必填，不需要修改

            //返回格式
            string v = "2.0";
            //必填，不需要修改

            //请求号
            string req_id = DateTime.Now.ToString("yyyyMMddHHmmss");

            //商户订单号
            string out_trade_no = "mob-" + Common.Tool.DESEncrypt.Encrypt(order.OrderID);

            //必填，须保证每次请求都是唯一

            //req_data详细信息

            //服务器异步通知页面路径
            string notify_url = "http://" + SettingBLL.WebDomain + "/API/AliNotify";
            //需http://格式的完整路径，不允许加?id=123这类自定义参数

            //页面跳转同步通知页面路径
            string call_back_url = "http://" + SettingBLL.WebDomain + "/API/AliCallBack";
            //需http://格式的完整路径，不允许加?id=123这类自定义参数

            //操作中断返回地址
            string merchant_url = "http://" + SettingBLL.MobileDomain + "/myorder";
            //用户付款中途退出返回商户的地址。需http://格式的完整路径，不允许加?id=123这类自定义参数

            //卖家支付宝帐户
            string seller_email = "b.i.h@qq.com";
            //必填


            //商户网站订单系统中唯一订单号，必填

            //订单名称
            //string subject = order.Goods.Name;
            //必填

            //付款金额
            string total_fee = order.Payment.ToString();
            //必填

            //请求业务参数详细
          //  string req_dataToken = "<direct_trade_create_req><notify_url>" + notify_url + "</notify_url><call_back_url>" + call_back_url + "</call_back_url><seller_account_name>" + seller_email + "</seller_account_name><out_trade_no>" + out_trade_no + "</out_trade_no><subject>" + subject + "</subject><total_fee>" + total_fee + "</total_fee><merchant_url>" + merchant_url + "</merchant_url></direct_trade_create_req>";
            //必填

            //把请求参数打包成数组
            Dictionary<string, string> sParaTempToken = new Dictionary<string, string>();
            sParaTempToken.Add("partner", MC.Config.Partner);
            sParaTempToken.Add("_input_charset", MC.Config.Input_charset.ToLower());
            sParaTempToken.Add("sec_id", MC.Config.Sign_type.ToUpper());
            sParaTempToken.Add("service", "alipay.wap.trade.create.direct");
            sParaTempToken.Add("format", format);
            sParaTempToken.Add("v", v);
            sParaTempToken.Add("req_id", req_id);
            //sParaTempToken.Add("req_data", req_dataToken);


            StringBuilder paramBuilder = new StringBuilder();
            foreach (var item in sParaTempToken)
            {
                paramBuilder.AppendFormat("&{0}={1}", item.Key, item.Value);
            }
            return GATEWAY_NEW + "?" + paramBuilder.ToString().TrimStart('&');

        }
        private ActionResult AliPay(decimal payment, string payfordatil, string reforderid)
        {
            //支付宝网关地址
            string GATEWAY_NEW = "http://wappaygw.alipay.com/service/rest.htm?";

            ////////////////////////////////////////////调用授权接口alipay.wap.trade.create.direct获取授权码token////////////////////////////////////////////

            //返回格式
            string format = "xml";
            //必填，不需要修改

            //返回格式
            string v = "2.0";
            //必填，不需要修改

            //请求号
            string req_id = DateTime.Now.ToString("yyyyMMddHHmmss");

            //商户订单号
            string out_trade_no = reforderid;

            //必填，须保证每次请求都是唯一

            //req_data详细信息

            //服务器异步通知页面路径
            string notify_url = "http://" + SettingBLL.WebDomain + "/API/AliNotify";
            //需http://格式的完整路径，不允许加?id=123这类自定义参数

            //页面跳转同步通知页面路径
            string call_back_url = "http://" + SettingBLL.WebDomain + "/API/AliCallBack";
            //需http://格式的完整路径，不允许加?id=123这类自定义参数

            //操作中断返回地址
            string merchant_url = "http://" + SettingBLL.MobileDomain + "/myorder";
            //用户付款中途退出返回商户的地址。需http://格式的完整路径，不允许加?id=123这类自定义参数

            //卖家支付宝帐户
            string seller_email = "b.i.h@qq.com";
            //必填


            //商户网站订单系统中唯一订单号，必填

            //订单名称
            string subject = payfordatil;
            //必填

            //付款金额
            string total_fee = payment.ToString();
            //必填

            //请求业务参数详细
            string req_dataToken = "<direct_trade_create_req><notify_url>" + notify_url + "</notify_url><call_back_url>" + call_back_url + "</call_back_url><seller_account_name>" + seller_email + "</seller_account_name><out_trade_no>" + out_trade_no + "</out_trade_no><subject>" + subject + "</subject><total_fee>" + total_fee + "</total_fee><merchant_url>" + merchant_url + "</merchant_url></direct_trade_create_req>";
            //必填

            //把请求参数打包成数组
            Dictionary<string, string> sParaTempToken = new Dictionary<string, string>();
            sParaTempToken.Add("partner", MC.Config.Partner);
            sParaTempToken.Add("_input_charset", MC.Config.Input_charset.ToLower());
            sParaTempToken.Add("sec_id", MC.Config.Sign_type.ToUpper());
            sParaTempToken.Add("service", "alipay.wap.trade.create.direct");
            sParaTempToken.Add("format", format);
            sParaTempToken.Add("v", v);
            sParaTempToken.Add("req_id", req_id);
            sParaTempToken.Add("req_data", req_dataToken);

            //建立请求
            string sHtmlTextToken = MC.Submit.BuildRequest(GATEWAY_NEW, sParaTempToken);
            //URLDECODE返回的信息
            Encoding code = Encoding.GetEncoding(MC.Config.Input_charset);
            sHtmlTextToken = HttpUtility.UrlDecode(sHtmlTextToken, code);

            //解析远程模拟提交后返回的信息
            Dictionary<string, string> dicHtmlTextToken = MC.Submit.ParseResponse(sHtmlTextToken);

            //获取token
            string request_token = dicHtmlTextToken["request_token"];

            ////////////////////////////////////////////根据授权码token调用交易接口alipay.wap.auth.authAndExecute////////////////////////////////////////////


            //业务详细
            string req_data = "<auth_and_execute_req><request_token>" + request_token + "</request_token></auth_and_execute_req>";
            //必填

            //把请求参数打包成数组
            Dictionary<string, string> sParaTemp = new Dictionary<string, string>();
            sParaTemp.Add("partner", MC.Config.Partner);
            sParaTemp.Add("_input_charset", MC.Config.Input_charset.ToLower());
            sParaTemp.Add("sec_id", MC.Config.Sign_type.ToUpper());
            sParaTemp.Add("service", "alipay.wap.auth.authAndExecute");
            sParaTemp.Add("format", format);
            sParaTemp.Add("v", v);
            sParaTemp.Add("req_data", req_data);

            //建立请求
            string sHtmlText = MC.Submit.BuildRequest(GATEWAY_NEW, sParaTemp, "get", "确认");
            Response.Write(sHtmlText);

            return null;
        }

        public ActionResult Detail(string orderid)
        {
            var order = GetOrder();

            return View(order);
        }
        public ActionResult SendOK(string orderid)
        {

            var order = GetOrder();
            if (CurMemberInfo.MemnerID != order.MemberID)
            {
                return Redirect("/zhongchou/memberlist?orderid=" + orderid);
            }
            ViewBag.myname = order.FromName;
           // ViewBag.goodimgurl = "http://" + SettingBLL.WebDomain + "/" + order.Goods.ImgUrl;
            ViewBag.linkurl = "http://" + SettingBLL.MobileDomain + "/zhongchou/memberlist?orderid=" + orderid;
            ViewBag.imgurl = "http://" + SettingBLL.WebDomain + "/api/GetQrCode?qraction=linkorder&param=" + orderid + "&rnd=" + DateTime.Now.ToString("fffff");
            ViewBag.js_json = TenPayManager.MakeUpJsParam();

            return View(order);
        }
        /// <summary>
        /// 查看某个众筹详情
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult MemberList(string orderid)
        {
            var order = GetOrder();
            ViewBag.commentList = db.Comments.Where(c => c.OrderID == order.OrderID).OrderBy(c => c.CommentID).ToList();
           

            if (order.EndDate == null)
            {
                order.EndDate = order.AddDate.Value.AddDays(10);
            }
            var endday = new DateTime(order.EndDate.Value.Year, order.EndDate.Value.Month, order.EndDate.Value.Day, 23, 59, 59);
            int days = endday.Subtract(DateTime.Now).Days;
            if (days == 0)
            {
                days = 1;
            }
            ViewBag.days = days;

            return View(order);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        public JsonResult MemberList(string orderid, string tomemberid, string saycontent)
        {
            var order = GetOrder();
            Comments commentModel = new Comments()
            {
                AddDate = DateTime.Now,
                Content = saycontent,
                MemberID = CurMemberInfo.MemnerID,
                Name = CurMemberInfo.Name,
                HeadImgUrl = CurMemberInfo.HeadImgUrl,
                ToMemberID = int.Parse(DESEncrypt.Decrypt(tomemberid)),
                OrderID = order.OrderID
            };
            db.Comments.Add(commentModel);
            db.SaveChanges();

            try
            {
                var tomemberModel = db.Members.Find(commentModel.ToMemberID);
                CustomHelper.SendZCMessageNotifyUseTemplate(tomemberModel.WechatOpenid, commentModel);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
            }


            ViewBag.commentList = db.Comments.Where(c => c.OrderID == order.OrderID).OrderBy(c => c.CommentID).ToList();
          


            return Json(new { state = "ok", name = CurMemberInfo.Name });
        }
        /// <summary>
        /// 展示众筹墙
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowList()
        {
            var zhongchouList = db.Orders.Where(c => c.OrderType == (int)OrderType.众筹订单
                && c.OrderStatus == (int)OrderStatus.众筹中
                && (c.EndDate > DateTime.Now || c.EndDate == null)).OrderByDescending(c => c.OrderID).ToList();
            var zhongchouList_succ = db.Orders.Where(c => c.OrderType == (int)OrderType.众筹订单
                && c.OrderStatus == (int)OrderStatus.众筹成功).OrderByDescending(c => c.OrderID).ToList();
            zhongchouList.AddRange(zhongchouList_succ);

            return View(zhongchouList);
        }

        public ActionResult MyZhongChouList()
        {
            ViewBag.Title = "我的众筹";
            ViewBag.show = "您暂时还没有众筹礼物呦！";
            var orderlist = db.Orders.Where(c => c.OrderType == (int)OrderType.众筹订单
                      && c.MemberID == CurMemberInfo.MemnerID && c.GoodID != null && c.OrderStatus != (int)OrderStatus.未开启众筹);
            return View(orderlist);
        }

    }
}
