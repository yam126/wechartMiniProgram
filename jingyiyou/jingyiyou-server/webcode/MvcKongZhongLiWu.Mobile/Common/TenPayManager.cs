using ncc2019.Common.BLL;
using ncc2019.Common.Tool;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace ncc2019.Common
{
    public class TenPayManager
    {
        private static TenPayV3Info _tenPayV3Info;

        public static TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[System.Configuration.ConfigurationManager.AppSettings["TenPayV3_MchId"]];
                }
                return _tenPayV3Info;
            }
        }
        /// <summary>
        /// 获得认证接口
        /// </summary>
        /// <returns></returns>
        public static string GetAuthorizeUrl(string backurl)
        {
            return OAuthApi.GetAuthorizeUrl(TenPayV3Info.AppId, backurl, "taken", OAuthScope.snsapi_userinfo);
        }

        /// <summary>
        /// 微信支付js权限
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static JsResult MakeUpJsParam(string openid, Orders order)
        {
            return MakeUpJsParam(openid, order.TotalPayment.Value, "智裹裹", Common.Tool.DESEncrypt.Encrypt("zc_" + order.OrderID));

            
        }
      

        public static JsResult MakeUpJsParam(string openid, CommonPay pay)
        {
            return MakeUpJsParam(openid, pay.Payment.Value, "体力购买", Common.Tool.DESEncrypt.Encrypt("jj_" + pay.CommonPayID));

        }
        public static JsResult MakeUpJsParam(string openid, ZGGPay pay)
        {
            return MakeUpJsParam(openid, pay.Payment.Value, "智裹裹使用支付", Common.Tool.DESEncrypt.Encrypt("zgg_" + pay.ZGGPayID));

        }

        public static JsResult MakeUpJsParam(string openid, decimal payment, int payid)
        {
            return MakeUpJsParam(openid, payment, "吹风机使用", Common.Tool.DESEncrypt.Encrypt("cfj_" + payid));

        }

        private static JsResult MakeUpJsParam(string openid, decimal payment, string payfordatil, string reforderid)
        {
            if (!Common.Tool.UserAgentHelper.IsWeiXin())
            {
               // return new JsResult();
            }

            //var openIdResult = OAuthApi.GetAccessToken(TenPayV3Info.AppId, TenPayV3Info.AppSecret, code);

            //if (openIdResult.errcode != ReturnCode.请求成功)
            //{
            //    throw new Exception("错误：" + openIdResult.errmsg);
            //    //return Content("错误：" + openIdResult.errmsg);
            //}
            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            //初始化
            packageReqHandler.Init();
            //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

            string timeStamp = TenPayV3Util.GetTimestamp();
            string nonceStr = TenPayV3Util.GetNoncestr();

            //设置package订单参数
            packageReqHandler.SetParameter("appid",  TenPayV3Info.AppId);		  //公众账号ID
            packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
            packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
            packageReqHandler.SetParameter("body", payfordatil);
            packageReqHandler.SetParameter("out_trade_no", reforderid);		//商家订单号
            packageReqHandler.SetParameter("total_fee", Decimal.ToInt32(payment * 100).ToString());			        //商品金额,以分为单位(money * 100).ToString()
            //packageReqHandler.SetParameter("spbill_create_ip", "223.20.167.245");   //用户的公网ip，不是商户服务器IP
            //packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
            packageReqHandler.SetParameter("notify_url", TenPayV3Info.TenPayV3Notify);		    //接收财付通通知的URL
            packageReqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString());	                    //交易类型
            packageReqHandler.SetParameter("openid", openid);	                    //用户的openId

            string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key).ToString();
            packageReqHandler.SetParameter("sign", sign);	                    //签名

            string data = packageReqHandler.ParseXML();

            var result = TenPayV3.Unifiedorder(data);
            LoggerHelper.Debug(result);
            var res = XDocument.Parse(result);
            if (res.Element("xml").Element("result_code") != null
                && res.Element("xml").Element("result_code").Value == "SUCCESS")
            {
                string prepayId = res.Element("xml").Element("prepay_id").Value;

                //设置支付参数
                RequestHandler paySignReqHandler = new RequestHandler(null);
                paySignReqHandler.SetParameter("appId", TenPayV3Info.AppId);
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");
                string paySign = paySignReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);



                JsResult js_result = new JsResult()
                {
                    appId = TenPayV3Info.AppId,
                    timeStamp = timeStamp,
                    nonceStr = nonceStr,
                    package = string.Format("prepay_id={0}", prepayId),
                    paySign = paySign,
                    signType = "MD5",
                    state = "ok"

                };
                return js_result;
            }
            else
            {
                JsResult js_result = new JsResult()
                {
                    state = "Error"
                };
                return js_result;
            }

        }
        /// <summary>
        /// 微信普通js权限
        /// </summary>
        /// <returns></returns>
        public static JsResult MakeUpJsParam()
        {
            if (!Common.Tool.UserAgentHelper.IsWeiXin())
            {
                //return new JsResult();
            }
            RequestHandler packageReqHandler = new RequestHandler(null);
            string timeStamp = TenPayV3Util.GetTimestamp();
            string nonceStr = TenPayV3Util.GetNoncestr();
            string ticketStr = Common.Tool.TokenHelper.GetJsTicket();

            //timestamp、url 都为加密干扰参数  其中url为动态获取的当前地址。
            string key = "jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}";
            key = string.Format(key, ticketStr, nonceStr, timeStamp, System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
            //进行sha序列生成
            string paySign = SHA1(key).ToLower();
            //string paySign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key).ToString();

            JsResult js_result = new JsResult()
            {
                appId = TenPayV3Info.AppId,
                timeStamp = timeStamp,
                nonceStr = nonceStr,
                package = "",
                paySign = paySign,
                signType = "",
                state = "ok"

            };
            return js_result;
        }
        ///// <summary>
        ///// 普通js权限config 代码片段
        ///// </summary>
        ///// <returns></returns>
        //public static string MakeUpJsConfig()
        //{
        //    JsResult jsresult = MakeUpJsParam();
        //    string resultStr = "";
        //}
        private static string SHA1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
    }

    /// <summary>
    /// 用于回传js需要的值
    /// </summary>
    public class JsResult
    {
        public string appId { get; set; }
        public string timeStamp { get; set; }
        public string nonceStr { get; set; }
        public string package { get; set; }
        public string paySign { get; set; }
        public string state { get; set; }
        public string signType { get; set; }
    }
}