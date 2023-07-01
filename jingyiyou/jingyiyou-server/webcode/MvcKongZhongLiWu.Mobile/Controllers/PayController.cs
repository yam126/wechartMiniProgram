using ncc2019.Common;
using ncc2019.Common.BLL;
using ncc2019.Common.Enum;
using ncc2019.Common.Tool;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MC = Com.AlipayM;

namespace ncc2019.Controllers
{
    public class PayController : ControllerBaseNoCheck
    {
        //
        // GET: /Pay/        
        public ActionResult Index(string orderid = "")
        {
            var order = GetOrder();
            if (order.PayStatus == (int)Common.Enum.PayStatus.已支付)
            {
                return Redirect("/send/?orderid=" + orderid);
            }


            ViewBag.openid = this.CurMemberInfo.WeChatOpenid;
            ViewBag.js_json = TenPayManager.MakeUpJsParam();

            DateTime curdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, 23, 59, 59);
           

            ViewBag.yue = SessionHelper.CurMemberInfo.Balance;
            ViewBag.totalpay = order.TotalPayment;

            ViewBag.mediaid = order.WxVioceMediaID;
            ViewBag.apiurl = Common.BLL.SettingBLL.WebDomain;
            return View(order);
        }

        public ActionResult My(string type = "", string orderid = "", string pid = "")
        {
            if (type == "zc")
            {//众筹环节支付
                return new ZhongChouController().ZGGPay(orderid);

            }
            else if (type == "zgg")
            {//分享降价环节支付
                return new MainController().ZggPayDetail();
            }
            else if (type == "kdpay")
            {//分享降价购买体力
                return new MainController().PayOpen();
            }            
            else
            {
                return null;
            }
            

        }

        [HttpPost]
        public JsonResult CheckPay(string orderid)
        {
            var order = GetOrder();
            if (order.PayStatus == (int)Common.Enum.PayStatus.已支付)
            {
                return Json(new { state = "error", url = "/send?orderid=" + orderid });
            }
            else
            {
                return Json(new { state = "ok" });
            }
        }


        [HttpPost]
        public JsonResult SaveDetail(string orderid, string sayect, string wayofpay, string openid,
            string needpass, string thepasstip, string thepass, string qcode)
        {
            var order = GetOrder();
            if (order.SayEtc == null)
            {
                order.SayEtc = sayect;
            }


            //需要密码登陆
            if (needpass == "2")
            {
                order.ThePassTip = thepasstip;
                order.ThePass = thepass;
            }

            if (wayofpay == "1")
            {
                order.PayType = (int)PayType.手机支付宝;
            }
            else if (wayofpay == "0")
            {
                order.PayType = (int)PayType.自己账户;
            }
            else
            {
                order.PayType = (int)PayType.微信;
            }

            //计算需要支付多少钱
            if (order.NeedPay == null)
            {
                

                if (wayofpay != "0" && SessionHelper.CurMemberInfo.Balance >= 0 && SessionHelper.CurMemberInfo.Balance <= order.TotalPayment)
                {
                    //扣除账户中的余额
                    Members curMember = db.Members.Find(SessionHelper.CurMemberInfo.MemnerID);
                    order.NeedPay = order.TotalPayment - curMember.Balance;
                    curMember.Balance_back = curMember.Balance;//冻结当前金额
                    curMember.Balance = 0;//清空当前金额
                    db.Database.ExecuteSqlCommand("update members set Balance_back=Balance_back+{0},Balance=Balance-{0} where memberid = {1} "
                          , new object[] { curMember.Balance_back, curMember.MemberID });
                    //db.Entry(curMember).State = System.Data.EntityState.Modified;
                    //db.SaveChanges();
                    //return AliPay(order);

                    PayLog paylog = new PayLog()
                    {
                        InDate = DateTime.Now,
                        MemberID = order.MemberID,
                        Payment = curMember.Balance_back,
                        PayDirection = (int)Common.Enum.PayDirection.副账户充值,
                        RefOrderID = order.OrderID
                    };
                    db.PayLog.Add(paylog);
                }
                else if (wayofpay == "0" && SessionHelper.CurMemberInfo.Balance >= 0 && SessionHelper.CurMemberInfo.Balance >= order.TotalPayment)
                {
                    Members curMember = db.Members.Find(SessionHelper.CurMemberInfo.MemnerID);
                    //curMember.Balance_back = order.TotalPayment;
                    //curMember.Balance = curMember.Balance - order.TotalPayment;//直接扣除余额                   

                    int canbuy = db.Database.ExecuteSqlCommand("update goods set buycount=buycount+1 where goodid = {0} and TotalNum>buycount "
                          , new object[] { order.GoodID });//标记为售出一件
                    //需要预先确认是否有产品可以卖
                    if (canbuy > 0)
                    {
                        order.NeedPay = 0;
                        //修改订单信息
                        order.PayStatus = (int)PayStatus.已支付;

                        

                        //如果有可买的产品
                        db.Database.ExecuteSqlCommand("update members set Balance=Balance-{0} where memberid = {1} "
                        , new object[] { order.TotalPayment, curMember.MemberID });//直接扣除余额

                        PayLog paylog = new PayLog()
                        {
                            InDate = DateTime.Now,
                            MemberID = order.MemberID,
                            Payment = order.TotalPayment,
                            PayDirection = (int)Common.Enum.PayDirection.购买,
                            RefOrderID = order.OrderID
                        };
                        db.PayLog.Add(paylog);
                    }
                    else
                    {
                        return Json(new { state = "error", message = "购买失败，产品已经抢光啦！" });
                    }



                }
                else
                {//全额付款
                    order.NeedPay = order.TotalPayment;
                    //return AliPay(order);
                }
            }
            //为微信支付生产参数
            var js_result = TenPayManager.MakeUpJsParam(openid, order);
            order.FromName = CurMemberInfo.Name;

            if (UpdateOrder(order))//修改订单状态
            {
                string url = "";
                if (order.IsForMe == (int)ShiFouStatus.是)
                {
                    url = "/gift/ok?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID);
                }
                else
                {
                    url = "/send?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID);
                }
                return Json(new { state = "ok", needpay = order.NeedPay, url = url, data = js_result });
            }
            else
            {
                return Json(new { state = "error", message = "订单处理失败，请稍后再试！" });
            }
        }
        [HttpPost]
        public ActionResult DoPay(string orderid)
        {
            if (UserAgentHelper.IsWeiXin())
            {
                return Redirect("/pay/weixinalipay?orderid=" + orderid);
            }
            else
            {
                var order = GetOrder();
                return AliPay(order);
            }

        }

        public ActionResult WeixinAlipay(string orderid)
        {
            if (UserAgentHelper.IsWeiXin())
            {
                return View("/views/zhongchou/weixinalipay.cshtml");
            }
            else
            {
                var order = GetOrder();
                return AliPay(order);
            }
        }

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
            string subject = "智裹裹";
            //必填

            //付款金额
            string total_fee = order.Payment.ToString();
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
    }
}
