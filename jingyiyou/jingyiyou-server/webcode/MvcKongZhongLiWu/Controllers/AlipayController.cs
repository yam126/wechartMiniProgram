using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AM = Com.AlipayM;
using System.Collections.Specialized;
using System.Xml;

namespace ncc2019.Controllers
{
    public class AlipayController : Controller
    {
        private ncc2019Entities db = new ncc2019Entities();
        public ActionResult CallBack()
        {
            Dictionary<string, string> sPara = GetRequestGet();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                AM.Notify aliNotify = new AM.Notify();
                bool verifyResult = aliNotify.VerifyReturn(sPara, Request.QueryString["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表

                    //商户订单号
                    string out_trade_no = Request.QueryString["out_trade_no"];

                    //支付宝交易号
                    string trade_no = Request.QueryString["trade_no"];

                    //交易状态
                    string result = Request.QueryString["result"];


                    if (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                    {
                        out_trade_no = Common.Tool.DESEncrypt.Decrypt(out_trade_no);
                        Orders order = db.Orders.Find(int.Parse(out_trade_no));
                        order.PayStatus = (int)Common.Enum.PayStatus.已支付;
                        //db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序
                    }
                    else
                    {
                        Response.Write("trade_status=" + Request.QueryString["trade_status"]);
                    }

                    //打印页面
                    Response.Write("验证成功<br />");

                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    Response.Write("验证失败");
                }
            }
            else
            {
                Response.Write("无返回参数");
            }
            return null;
        }

        public ActionResult Notify()
        {
            Dictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                AM.Notify aliNotify = new AM.Notify();
                bool verifyResult = aliNotify.VerifyNotify(sPara, Request.Form["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //解密（如果是RSA签名需要解密，如果是MD5签名则下面一行清注释掉）
                    sPara = aliNotify.Decrypt(sPara);

                    //XML解析notify_data数据
                    try
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(sPara["notify_data"]);
                        //商户订单号
                        string out_trade_no = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
                        //支付宝交易号
                        string trade_no = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;
                        //交易状态
                        string trade_status = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;

                        if (trade_status == "TRADE_FINISHED")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //该种交易状态只在两种情况下出现
                            //1、开通了普通即时到账，买家付款成功后。
                            //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。

                            Response.Write("success");  //请不要修改或删除
                        }
                        else if (trade_status == "TRADE_SUCCESS")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。


                            Response.Write("success");  //请不要修改或删除
                        }
                        else
                        {
                            Response.Write(trade_status);
                        }

                    }
                    catch (Exception exc)
                    {
                        Response.Write(exc.ToString());
                    }



                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    Response.Write("fail");
                }
            }
            else
            {
                Response.Write("无通知参数");
            }
            return null;
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestGet()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

    }
}
