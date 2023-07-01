using Com.Alipay;
using ncc2019.Common.BLL;
using ncc2019.Common.Enum;
using ncc2019.Common.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class PayController : ControllerBase
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
         
         

            ViewBag.yue = SessionHelper.CurMemberInfo.Balance;
            ViewBag.totalpay = order.TotalPayment;
            

            return View(order);
        }

        public ActionResult My(string orderid = "")
        {
            var order = GetOrder();
            if (order.PayStatus == (int)Common.Enum.PayStatus.已支付)
            {
                return Redirect("/send/?orderid=" + orderid);
            }

           

            ViewBag.yue = SessionHelper.CurMemberInfo.Balance;
            ViewBag.totalpay = order.TotalPayment;


            return View(order);
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

        /// <summary>
        /// 购买物品
        /// </summary>
        /// <param name="wayofpay"></param>
        /// <param name="num"></param>
        /// <param name="goodid"></param>
        /// <returns></returns>

        public ActionResult DoPay(string wayofpay, string orderid
            , string sayect, string fromname, string needpass, string thepasstip, string thepass, string qcode
            , string toname, string tophone, string toaddress)
        {
            var order = GetOrder();
            if (order.IsForMe == (int)ShiFouStatus.是)
            {
                order.ToName = toname;
                order.ToPhone = tophone;
                order.ToAddress = toaddress;
            }
            else
            {             
                order.SayEtc = sayect;
                order.FromName = fromname;
                //需要密码登陆
                if (needpass == "2")
                {
                    order.ThePassTip = thepasstip;
                    order.ThePass = thepass;
                }
            }
            

           

            //计算需要支付多少钱
            if (order.NeedPay == null)
            {
               

                // Orders order = db.Orders.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(orderid)));

                if (wayofpay == "1" && SessionHelper.CurMemberInfo.Balance >= 0 && SessionHelper.CurMemberInfo.Balance <= order.TotalPayment)
                {
                    //扣除账户中的余额
                    Members curMember = db.Members.Find(SessionHelper.CurMemberInfo.MemnerID);
                    order.NeedPay = order.TotalPayment - curMember.Balance;
                    curMember.Balance_back = curMember.Balance;//冻结当前金额
                    curMember.Balance = 0;//清空当前金额
                    order.PayType = (int)PayType.Web支付宝;
                    db.Database.ExecuteSqlCommand("update members set Balance_back=Balance_back+{0},Balance=Balance-{0} where memberid = {1} "
                            , new object[] { curMember.Balance_back, curMember.MemberID });

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
                    order.NeedPay = 0;
                    order.PayType = (int)PayType.自己账户;
                    //修改订单信息
                    

                    db.Database.ExecuteSqlCommand("update members set Balance=Balance-{0} where memberid = {1} "
                           , new object[] { order.TotalPayment, curMember.MemberID });//直接扣除余额
                    db.Database.ExecuteSqlCommand("update goods set buycount=buycount+1 where goodid = {0} "
                           , new object[] { order.GoodID });//标记为售出一件
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
                {//全额付款
                    order.PayType = (int)PayType.Web支付宝;
                    order.NeedPay = order.TotalPayment;

                }
            }

            if (UpdateOrder(order))//修改订单状态
            {
                if (order.NeedPay==0)
                {
                    if (order.IsForMe==(int)ShiFouStatus.是)
                    {
                        return Redirect("/gift/ok?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID));
                    }
                    else
                    {
                        return Redirect("/send?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID));
                    }
                    
                }
                else
                {
                    return AliPay(order);
                }
                
            }
            else
            {
                ShowAlertMessage("订单处理失败，请稍后再试！");
                return View();
            }


        }

        private ActionResult AliPay(Orders order)
        {
            ////////////////////////////////////////////请求参数////////////////////////////////////////////
            //支付类型
            string payment_type = "1";
            //必填，不能修改
            //服务器异步通知页面路径
            string notify_url = "http://" + SettingBLL.WebDomain + "/API/AliNotify";
            //需http://格式的完整路径，不能加?id=123这类自定义参数

            //页面跳转同步通知页面路径
            string return_url = "http://" + SettingBLL.WebDomain + "/API/AliCallBack";
            //需http://格式的完整路径，不能加?id=123这类自定义参数，不能写成http://localhost/

            //卖家支付宝帐户
            string seller_email = "b.i.h@qq.com";
            //必填

            //商户订单号
            string out_trade_no = "web-" + Common.Tool.DESEncrypt.Encrypt(order.OrderID);
            //商户网站订单系统中唯一订单号，必填

            //订单名称
         //   string subject = order.Goods.Name;
            //必填

            //付款金额
            string total_fee = order.NeedPay.Value.ToString();//实付金额
            //必填

            //订单描述

          //  string body = order.Goods.Intro;


            //商品展示地址
            string show_url = "http://" + SettingBLL.WebDomain + "/Home/Details?id=" + order.GoodID;
            //需以http://开头的完整路径，例如：http://www.商户网址.com/myorder.html

            //防钓鱼时间戳
            string anti_phishing_key = "";
            //若要使用请调用类文件submit中的query_timestamp函数

            //客户端的IP地址
            string exter_invoke_ip = "";
            //非局域网的外网IP地址，如：221.0.0.1


            ////////////////////////////////////////////////////////////////////////////////////////////////

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("service", "create_direct_pay_by_user");
            sParaTemp.Add("payment_type", payment_type);
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("return_url", return_url);
            sParaTemp.Add("seller_email", seller_email);
            sParaTemp.Add("out_trade_no", out_trade_no);
            //sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
           // sParaTemp.Add("body", body);
            sParaTemp.Add("show_url", show_url);
            sParaTemp.Add("anti_phishing_key", anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", exter_invoke_ip);

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            Response.Clear();
            Response.Write(sHtmlText);

            return null;
        }
    }
}
