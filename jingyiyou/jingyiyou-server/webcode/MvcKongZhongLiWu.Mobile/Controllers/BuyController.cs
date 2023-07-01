using ncc2019.Common.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Enum;
using System.Text;
using Com.AlipayM;
using ncc2019.Common;
using ncc2019.Common.BLL;

namespace ncc2019.Controllers
{
    public class BuyController : ControllerBase
    {
        //
        // GET: /Buy/
        //private ncc2019Entities db = new ncc2019Entities();
        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int id, int num)
        {

            ViewBag.IsWeiXin = UserAgentHelper.IsWeiXin();//判断是否是微信环境      
            var good = db.Goods.Find(id);
            if (num <= 0)
            {
                ViewBag.num = 1;
            }
            else
            {
                ViewBag.num = num;
            }
            ViewBag.totalPay = good.Payment * num;

            return View(good);
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="number"></param>
        /// <param name="goodid"></param>
        /// <returns></returns>

        public ActionResult DoBuy(int goodid, int forme, string property, int number = 1)
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
                    IsForMe = forme,
                    Property = property
                };
                db.Orders.Add(order);
                db.SaveChanges();
                if (forme == (int)ShiFouStatus.是)
                {
                    //跳转到支付界面
                    return Redirect("/Address?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID));
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


        //public ActionResult Address(string orderid)
        //{

        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Address(string orderid, string name, string phone, string address)
        //{
        //    var order = GetOrder();
        //    order.ToName = name;
        //    order.ToPhone = phone;
        //    order.ToAddress = address;

        //    db.Entry(order).Property(c => c.ToName).IsModified = true;
        //    db.Entry(order).Property(c => c.ToPhone).IsModified = true;
        //    db.Entry(order).Property(c => c.ToAddress).IsModified = true;
        //    db.SaveChanges();

        //    return Redirect("/Pay/My?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID));
        //}

        /// <summary>
        /// 支付宝支付
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private ActionResult AliPay(Orders order)
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
          //  string subject = order.Goods.Name;
            //必填

            //付款金额
            string total_fee = order.Payment.ToString();
            //必填

            //请求业务参数详细
           // string req_dataToken = "<direct_trade_create_req><notify_url>" + notify_url + "</notify_url><call_back_url>" + call_back_url + "</call_back_url><seller_account_name>" + seller_email + "</seller_account_name><out_trade_no>" + out_trade_no + "</out_trade_no><subject>" + subject + "</subject><total_fee>" + total_fee + "</total_fee><merchant_url>" + merchant_url + "</merchant_url></direct_trade_create_req>";
            //必填

            //把请求参数打包成数组
            Dictionary<string, string> sParaTempToken = new Dictionary<string, string>();
            sParaTempToken.Add("partner", Config.Partner);
            sParaTempToken.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTempToken.Add("sec_id", Config.Sign_type.ToUpper());
            sParaTempToken.Add("service", "alipay.wap.trade.create.direct");
            sParaTempToken.Add("format", format);
            sParaTempToken.Add("v", v);
            sParaTempToken.Add("req_id", req_id);
            //sParaTempToken.Add("req_data", req_dataToken);

            //建立请求
            string sHtmlTextToken = Submit.BuildRequest(GATEWAY_NEW, sParaTempToken);
            //URLDECODE返回的信息
            Encoding code = Encoding.GetEncoding(Config.Input_charset);
            sHtmlTextToken = HttpUtility.UrlDecode(sHtmlTextToken, code);

            //解析远程模拟提交后返回的信息
            Dictionary<string, string> dicHtmlTextToken = Submit.ParseResponse(sHtmlTextToken);

            //获取token
            string request_token = dicHtmlTextToken["request_token"];

            ////////////////////////////////////////////根据授权码token调用交易接口alipay.wap.auth.authAndExecute////////////////////////////////////////////


            //业务详细
            string req_data = "<auth_and_execute_req><request_token>" + request_token + "</request_token></auth_and_execute_req>";
            //必填

            //把请求参数打包成数组
            Dictionary<string, string> sParaTemp = new Dictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("sec_id", Config.Sign_type.ToUpper());
            sParaTemp.Add("service", "alipay.wap.auth.authAndExecute");
            sParaTemp.Add("format", format);
            sParaTemp.Add("v", v);
            sParaTemp.Add("req_data", req_data);

            //建立请求
            string sHtmlText = Submit.BuildRequest(GATEWAY_NEW, sParaTemp, "get", "确认");
            Response.Write(sHtmlText);

            return null;
        }

    }
}
