using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AM = Com.AlipayM;
using System.Collections.Specialized;
using System.Xml;
using ncc2019.Common.Tool;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using ncc2019.Common.Weixin.Base;
using System.Xml.Linq;
using ncc2019.Common.BLL;
using Senparc.Weixin.MP.TenPayLibV3;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin;
using ncc2019.Common.Enum;
using System.Data.Entity;

namespace ncc2019.Controllers
{
    public class APIController : ControllerBaseNoCheck
    {

        #region 支付宝
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AliCallBack()
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

                    //订单号以web- 开头为从web通道而来  订单开头以mob- 为手机通道
                    bool isweb = false;
                    if (out_trade_no.StartsWith("web-"))
                    {
                        isweb = true;
                    }
                    out_trade_no = out_trade_no.Replace("web-", "").Replace("mob-", "");

                    out_trade_no = Common.Tool.DESEncrypt.Decrypt(out_trade_no);//解密订单号
                    if (out_trade_no.StartsWith("zc_"))
                    {
                        out_trade_no = out_trade_no.Replace("zc_", "");
                        return ZhongChouPayBack(out_trade_no);
                    }
                    else if (out_trade_no.StartsWith("tl_"))
                    {
                        out_trade_no = out_trade_no.Replace("tl_", "");
                        return CommonPayBack(out_trade_no);
                    }
                    else
                    {
                        Orders order = db.Orders.Find(int.Parse(out_trade_no));

                        //防止服务器多次调用后产生多次记录
                        if (order.PayStatus == (int)Common.Enum.PayStatus.未支付)
                        {
                            db.Database.ExecuteSqlCommand("update goods set BuyCount=BuyCount+1 where goodid = " + order.GoodID, new object[] { });

                            PayLog paylog = new PayLog()
                            {
                                InDate = DateTime.Now,
                                MemberID = order.MemberID,
                                Payment = order.NeedPay,
                                PayDirection = (int)Common.Enum.PayDirection.购买,
                                RefOrderID = order.OrderID
                            };
                            db.PayLog.Add(paylog);
                            if (order.TotalPayment > order.NeedPay)
                            {

                                PayLog paylog_fu = new PayLog()
                                {
                                    InDate = DateTime.Now,
                                    MemberID = order.MemberID,
                                    Payment = order.TotalPayment - order.NeedPay,
                                    PayDirection = (int)Common.Enum.PayDirection.副账户购买,
                                    RefOrderID = order.OrderID
                                };
                                db.PayLog.Add(paylog_fu);

                                db.Database.ExecuteSqlCommand("update members set Balance_back=Balance_back-{0} where memberid = {1} "
                                             , new object[] { paylog_fu.Payment, order.MemberID });//从副账户扣除冻结了的金额
                            }

                            order.PayStatus = (int)Common.Enum.PayStatus.已支付;
                            //db.Entry(order).State = EntityState.Modified;



                            db.SaveChanges();
                        }
                        //打印页面
                        //Response.Write("验证成功<br />");
                        if (order.IsForMe == (int)Common.Enum.ShiFouStatus.是)
                        {//买给自己的

                            //if (order.Goods.GoodType == (int)Common.Enum.GoodType.自动降价)
                            //{
                            //    return Redirect(string.Format("http://" + SettingBLL.MobileDomain + "/address?type=jj&orderid={0}"
                            //        , Common.Tool.DESEncrypt.Encrypt(order.OrderID)));
                            //}
                            if (isweb)
                            {
                                return Redirect("http://" + SettingBLL.WebDomain + "/gift/ok?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID));
                            }
                            else
                            {
                                return Redirect("http://" + SettingBLL.MobileDomain + "/gift/ok?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID));
                            }
                        }
                        else
                        {
                            if (isweb)
                            {
                                return Redirect("http://" + SettingBLL.WebDomain + "/send?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID));
                            }
                            else
                            {
                                return Redirect("http://" + SettingBLL.MobileDomain + "/send?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID));
                            }
                        }
                    }




                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    //return Redirect("http://wxmtest.kongzhongliwu.com/error/");
                }
            }
            else
            {
                //return Redirect("http://wxmtest.kongzhongliwu.com/error/");
            }
            return Redirect("http://" + SettingBLL.MobileDomain + "/error/");
        }

        private ActionResult ZhongChouPayBack(string out_trade_no)
        {

            var order = db.Orders.Find(int.Parse(out_trade_no));
            decimal? totalpayment = order.TotalPayment;

            //防止服务器多次调用后产生多次记录
            if (order.PayStatus == (int)Common.Enum.PayStatus.未支付)
            {
                //通知发起人有人参与
                // CustomHelper.SendZhongChouPayUseTemplate(order.Members.WechatOpenid, pay);


                PayLog paylog = new PayLog()
                {
                    InDate = DateTime.Now,
                    MemberID = order.MemberID,
                    Payment = order.TotalPayment,
                    PayDirection = (int)Common.Enum.PayDirection.购买,
                    RefOrderID = order.OrderID
                };
                db.PayLog.Add(paylog);

                order.PayStatus = (int)Common.Enum.PayStatus.已支付;
                db.SaveChanges();
            }


            return null;
        }

        private ActionResult CommonPayBack(string out_trade_no, bool isredirect = true)
        {
            CommonPay pay = db.CommonPay.Find(int.Parse(out_trade_no));

            //防止服务器多次调用后产生多次记录
            if (pay.PayStatus == (int)Common.Enum.PayStatus.未支付)
            {


                PayLog paylog = new PayLog()
                {
                    InDate = DateTime.Now,
                    MemberID = pay.MemberID,
                    Payment = pay.Payment,
                    PayDirection = (int)Common.Enum.PayDirection.购买,
                    RefOrderID = pay.CommonPayID
                };
                db.PayLog.Add(paylog);
                if (pay.Payment > pay.NeedPay)
                {

                    PayLog paylog_fu = new PayLog()
                    {
                        InDate = DateTime.Now,
                        MemberID = pay.MemberID,
                        Payment = pay.Payment - pay.NeedPay,
                        PayDirection = (int)Common.Enum.PayDirection.副账户购买,
                        RefOrderID = pay.CommonPayID
                    };
                    db.PayLog.Add(paylog_fu);

                    db.Database.ExecuteSqlCommand("update members set Balance_back=Balance_back-{0} where memberid = {1} "
                                 , new object[] { paylog_fu.Payment, pay.MemberID });//从副账户扣除冻结了的金额
                }
                pay.PayStatus = (int)Common.Enum.PayStatus.已支付;
                db.SaveChanges();

                db.Database.ExecuteSqlCommand("update members set tilinum=tilinum+{0} where memberid = {1} "
                              , new object[] { pay.Payment, pay.MemberID });//从副账户扣除冻结了的金额
            }

            if (isredirect)
            {
                return Redirect("http://" + SettingBLL.MobileDomain + "/common/backtoweixin");
            }
            else
            {
                return null;
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AliNotify()
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

                        #region 处理订单


                        //订单号以web- 开头为从web通道而来  订单开头以mob- 为手机通道
                        bool isweb = false;
                        if (out_trade_no.StartsWith("web-"))
                        {
                            isweb = true;
                        }
                        out_trade_no = out_trade_no.Replace("web-", "").Replace("mob-", "");

                        if (out_trade_no.StartsWith("zc_"))
                        {
                            out_trade_no = out_trade_no.Replace("zc_", "");
                            return ZhongChouPayBack(out_trade_no);
                        }
                        else if (out_trade_no.StartsWith("tl_"))
                        {
                            out_trade_no = out_trade_no.Replace("tl_", "");
                            return CommonPayBack(out_trade_no);
                        }
                        else
                        {
                            out_trade_no = Common.Tool.DESEncrypt.Decrypt(out_trade_no);//解密订单号
                            Orders order = db.Orders.Find(int.Parse(out_trade_no));

                            //防止服务器多次调用后产生多次记录
                            if (order.PayStatus == (int)Common.Enum.PayStatus.未支付)
                            {
                                db.Database.ExecuteSqlCommand("update goods set BuyCount=BuyCount+1 where goodid = " + order.GoodID, new object[] { });

                                PayLog paylog = new PayLog()
                                {
                                    InDate = DateTime.Now,
                                    MemberID = order.MemberID,
                                    Payment = order.NeedPay,
                                    PayDirection = (int)Common.Enum.PayDirection.购买,
                                    RefOrderID = order.OrderID
                                };
                                db.PayLog.Add(paylog);
                                if (order.TotalPayment > order.NeedPay)
                                {

                                    PayLog paylog_fu = new PayLog()
                                    {
                                        InDate = DateTime.Now,
                                        MemberID = order.MemberID,
                                        Payment = order.TotalPayment - order.NeedPay,
                                        PayDirection = (int)Common.Enum.PayDirection.副账户购买,
                                        RefOrderID = order.OrderID
                                    };
                                    db.PayLog.Add(paylog_fu);

                                    db.Database.ExecuteSqlCommand("update members set Balance_back=Balance_back-{0} where memberid = {1} "
                                                 , new object[] { paylog_fu.Payment, order.MemberID });//从副账户扣除冻结了的金额
                                }

                                order.PayStatus = (int)Common.Enum.PayStatus.已支付;
                                //db.Entry(order).State = EntityState.Modified;



                                db.SaveChanges();
                            }


                            //打印页面                        
                            Response.Write("success");
                        }
                        #endregion
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

        #endregion

        #region 微信
        public ActionResult PayNotifyUrl()
        {
            LoggerHelper.Debug("info:PayNotifyUrl");
            ResponseHandler resHandler = new ResponseHandler(null);
            string result_code = resHandler.GetParameter("result_code");
            string appid = resHandler.GetParameter("appid");
            string mch_id = resHandler.GetParameter("mch_id");
            string device_info = resHandler.GetParameter("device_info");
            string nonce_str = resHandler.GetParameter("nonce_str");
            string sign = resHandler.GetParameter("sign");
            string err_code = resHandler.GetParameter("err_code");
            string err_code_des = resHandler.GetParameter("err_code_des");
            string openid = resHandler.GetParameter("openid");
            string is_subscribe = resHandler.GetParameter("is_subscribe");
            string trade_type = resHandler.GetParameter("trade_type");
            string bank_type = resHandler.GetParameter("bank_type");
            string total_fee = resHandler.GetParameter("total_fee");
            string coupon_fee = resHandler.GetParameter("coupon_fee");
            string fee_type = resHandler.GetParameter("fee_type");
            string transaction_id = resHandler.GetParameter("transaction_id");
            string out_trade_no = resHandler.GetParameter("out_trade_no");
            string attach = resHandler.GetParameter("attach");
            string time_end = resHandler.GetParameter("time_end");

            LoggerHelper.Debug("info:" + out_trade_no);
            //if (out_trade_no.Length >= 32)
            //{
            //    //LoggerHelper.Debug("chuifengji" + out_trade_no);
            //    //发一封邮件到QQ邮箱
            //    SmtpHelper.SendByPayTest("416402144@qq.com", out_trade_no);
            //    return Content("Success");
            //}
            //out_trade_no = Common.Tool.DESEncrypt.Decrypt(out_trade_no);//解密订单号

            try
            {
                if (out_trade_no.Contains("recharge-"))
                {
                    //给账户充值
                    out_trade_no = out_trade_no.Replace("recharge-", "");
                    CommonPay pay = db.CommonPay.Find(int.Parse(out_trade_no));
                    if (pay.PayStatus == (int)PayStatus.未支付)
                    {

                        pay.PayStatus = (int)PayStatus.已支付;
                        pay.PayDate = DateTime.Now;


                        //将数据保存到数据库
                        db.Database.ExecuteSqlCommand(
                            TransactionalBehavior.EnsureTransaction,
                            "update members set Balance=Balance+{0} where memberid = {1} "
                            , pay.Payment, pay.MemberID);
                        db.SaveChanges();
                    }
                }
                else if (out_trade_no.Contains("buy-"))
                {
                    //购买彩票
                    out_trade_no = out_trade_no.Replace("buy-", "");
                    CommonPay pay = db.CommonPay.Find(int.Parse(out_trade_no));

                    //Orders order = db.Orders.Find(int.Parse(out_trade_no));

                    //防止服务器多次调用后产生多次记录
                    if (pay.PayStatus == (int)PayStatus.未支付)
                    {

                        //如果有扣除账户余额的情况
                        if (pay.TotalPayment > pay.NeedPay)
                        {

                            decimal payment = pay.TotalPayment.Value - pay.NeedPay.Value;
                            db.Database.ExecuteSqlCommand(
                                  TransactionalBehavior.EnsureTransaction,
                                  "update members set Balance=Balance-{0} where memberid = {1} "
                                         , payment, pay.MemberID);//从账户扣除金额

                        }
                        pay.PayStatus = (int)PayStatus.已支付;
                        pay.PayDate = DateTime.Now;

                        //分配刮开渠道
                        //var member = db.Members.Find(pay.MemberID);
                        //Seller seller = null;
                        ////优先分配推荐渠道为开奖渠道
                        //if (member != null && member.RefSellerID != null)
                        //{
                        //    seller = db.Seller.Where(c => c.IsOnline == (int)ShiFouStatus.是 && c.Status == (int)SellerStatus.正常
                        //    && c.SellerID == member.RefSellerID).FirstOrDefault();

                        //}
                        ////推荐渠道若不在线则随机分配
                        //if (seller == null)
                        //{
                        //    seller = db.Seller.Where(c => c.IsOnline == (int)ShiFouStatus.是 && c.Status == (int)SellerStatus.正常)
                        //    .OrderByDescending(c => c.TheOrder).OrderByDescending(c => c.QueueNum).FirstOrDefault();
                        //}


                        if (pay.RefOrderID != null)
                        {
                            var order = db.NCCOrders.Find(pay.RefOrderID);
                            order.PayStatus = (int)PayStatus.已支付;
                            //if (seller != null)
                            //{
                            //    order.SellerID = seller.SellerID;//分配卖家

                            //}
                            var lotterylist = db.NCCLottery.Where(c => c.RefOrderID == order.OrderID);
                            foreach (var item in lotterylist)
                            {
                                //if (seller != null)
                                //    item.RefSellerID = seller.SellerID;//分配卖家
                                item.RefMemberID = order.MemberID;
                                item.LotteryStatus = (int)LotteryStatus.已支付;
                            }
                            //将商家的待开奖队列进行累加
                            db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction,
                               "update seller set QueueNum =QueueNum+{1} where sellerid={0}",
                               order.SellerID, order.BuyNum);

                        }


                        db.SaveChanges();
                    }
                }
                else if (out_trade_no.Contains("payuserprize-"))
                {
                    //店家为用户兑奖--通过微信支付
                    out_trade_no = out_trade_no.Replace("payuserprize-", "");
                    CommonPay pay = db.CommonPay.Find(int.Parse(out_trade_no));
                    DateTime curTime = DateTime.Now;
                    if (pay.PayStatus == (int)PayStatus.未支付)
                    {

                        pay.PayStatus = (int)PayStatus.已支付;
                        pay.PayDate = curTime;

                        var lottery = db.NCCLottery.Find(pay.GoodID);
                        if (lottery != null)
                        {
                            lottery.PrizeDate = curTime;
                            lottery.PrizeStatus = (int)PrizeStatus.已中奖;
                            lottery.Bonus = pay.Payment;
                        }

                        //计算彩票销售分润情况
                        var member = db.Members.Find(pay.MemberID);//开奖商家
                        decimal payment = pay.Payment.Value;//彩票中奖金额
                        decimal buylotterypayment = lottery.Payment.Value * 97 / 100;
                        decimal forgivenpayment = lottery.Payment.Value * 1 / 100;
                        decimal forgivenpaymentsys = lottery.Payment.Value * 2 / 100;//系统得到2个点的提成
                        var buyer = db.Members.Find(lottery.RefMemberID);//买彩票的人
                        var memberForgiven = db.Members.Find(buyer.RefMemberID);//推荐者
                        //if (buyer.RefMemberID == pay.MemberID)
                        //{//若推荐人是商家自己 则多派发一个点
                        //    buylotterypayment = lottery.Payment.Value * 98 / 100;
                        //}
                        var sellerMember = db.Members.Find(pay.MemberID);
                        //店家给用户兑奖的款项--彩票中奖金额，需要在开奖店铺余额里面扣除
                        CommonPay payfrom = new CommonPay()
                        {
                            AddDate = curTime,
                            GoodID = lottery.NCCLotteryID,
                            HeadImgUrl = member.HeadImgUrl,
                            MemberID = member.MemberID,
                            Name = member.Name,
                            NeedPay = payment,
                            Payment = payment,
                            PayDirection = (int)PayDirection.中奖后打款,
                            PayStatus = (int)PayStatus.已支付,
                            PayType = (int)PayType.自己账户,
                            RefOrderID = lottery.RefOrderID,
                            TotalPayment = payment,
                            PayDate = curTime
                        };
                        //将销售彩票款打给店家
                        //用户中奖后会收到相应兑奖金额
                        CommonPay payto = new CommonPay()
                        {
                            AddDate = curTime,
                            GoodID = lottery.NCCLotteryID,
                            HeadImgUrl = buyer.HeadImgUrl,
                            MemberID = buyer.MemberID,
                            Name = buyer.Name,
                            NeedPay = payment,
                            Payment = payment,
                            PayDirection = (int)PayDirection.中奖后收款,
                            PayStatus = (int)PayStatus.已支付,
                            PayType = (int)PayType.自己账户,
                            RefOrderID = lottery.RefOrderID,
                            TotalPayment = payment,
                            PayDate = curTime
                        };
                        //兑奖后为店家派发销售彩票款
                        CommonPay payprize = new CommonPay()
                        {
                            AddDate = curTime,
                            GoodID = lottery.NCCLotteryID,
                            HeadImgUrl = member.HeadImgUrl,
                            MemberID = member.MemberID,
                            Name = member.Name,
                            NeedPay = buylotterypayment,
                            Payment = buylotterypayment,
                            PayDirection = (int)PayDirection.销售彩票款,
                            PayStatus = (int)PayStatus.已支付,
                            PayType = (int)PayType.系统账户,
                            RefOrderID = lottery.RefOrderID,
                            TotalPayment = buylotterypayment,
                            PayDate = curTime
                        };
                        //推荐商家将得到销售提成
                        CommonPay payforgiven = new CommonPay()
                        {
                            AddDate = curTime,
                            GoodID = lottery.NCCLotteryID,
                            HeadImgUrl = memberForgiven.HeadImgUrl,
                            MemberID = memberForgiven.MemberID,
                            Name = memberForgiven.Name,
                            NeedPay = forgivenpayment,
                            Payment = forgivenpayment,
                            PayDirection = (int)PayDirection.销售网点推荐提成,
                            PayStatus = (int)PayStatus.已支付,
                            PayType = (int)PayType.系统账户,
                            RefOrderID = lottery.RefOrderID,
                            TotalPayment = forgivenpayment,
                            PayDate = curTime
                        };
                        //系统得到2点提成
                        CommonPay payforgivensys = new CommonPay()
                        {
                            AddDate = curTime,
                            GoodID = lottery.NCCLotteryID,
                            HeadImgUrl = "",
                            MemberID = SettingBLL.SysMemberID,
                            Name = "",
                            NeedPay = forgivenpaymentsys,
                            Payment = forgivenpaymentsys,
                            PayDirection = (int)PayDirection.销售网点推荐提成,
                            PayStatus = (int)PayStatus.已支付,
                            PayType = (int)PayType.系统账户,
                            RefOrderID = lottery.RefOrderID,
                            TotalPayment = forgivenpaymentsys,
                            PayDate = curTime
                        };


                        db.CommonPay.Add(payfrom);
                        db.CommonPay.Add(payto);
                        db.CommonPay.Add(payprize);
                        db.CommonPay.Add(payforgiven);
                        db.CommonPay.Add(payforgivensys);

                        //消除队列里面的彩票数量
                        db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction,
                                    "update seller set QueueNum =QueueNum-1 where sellerid={0}",
                                    lottery.RefSellerID);


                        db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction
                           , "update Members set Balance =Balance+{1} where MemberID={0}"
                           , buyer.MemberID, payment);
                        db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction
                           , "update Members set Balance =Balance+{1} where MemberID={0}"
                           , member.MemberID, buylotterypayment);
                        db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction
                           , "update Members set Balance =Balance+{1} where MemberID={0}"
                           , memberForgiven.MemberID, forgivenpayment);
                        db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction
                           , "update Members set Balance =Balance+{1} where MemberID={0}"
                           , SettingBLL.SysMemberID, forgivenpaymentsys);


                        //db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance-{1} where MemberID={0}"
                        //                   , member.MemberID, payment)); 微信款已经入总账户，不需要在开奖者私人账户里扣除
                        //db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
                        //            , buyer.MemberID, payment));//中奖者获得彩票中奖金额
                        //db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
                        //            , member.MemberID, buylotterypayment));//把购买彩票的彩票款派发给开奖者
                        //db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
                        //            , memberForgiven.MemberID, forgivenpayment));//将销售提成分给推荐者
                        //db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
                        //         , SettingBLL.SysMemberID, forgivenpaymentsys));//将销售提成分给系统

                        db.SaveChanges();
                    }
                }
                else
                {
                    LoggerHelper.Debug("error:" + out_trade_no);
                }


            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }






            return Content("Success");
        }
        private ActionResult ZGGPayBack(string out_trade_no)
        {

            var payModel = db.ZGGPay.Find(int.Parse(out_trade_no));
            decimal? totalpayment = payModel.Payment;

            //防止服务器多次调用后产生多次记录
            if (payModel.State == (int)Common.Enum.PayStatus.未支付)
            {
                //通知发起人有人参与
                // CustomHelper.SendZhongChouPayUseTemplate(order.Members.WechatOpenid, pay);


                PayLog paylog = new PayLog()
                {
                    InDate = DateTime.Now,
                    MemberID = payModel.MemberID,
                    Payment = payModel.Payment,
                    PayDirection = (int)Common.Enum.PayDirection.购买,
                    RefOrderID = payModel.ZGGPayID
                };
                db.PayLog.Add(paylog);

                var useControlModel = db.ZGGUseControl.Where(c => c.ZGGPayID == payModel.ZGGPayID).FirstOrDefault();
                if (useControlModel != null)
                {
                    useControlModel.PayState = (int)Common.Enum.PayStatus.已支付;
                }


                payModel.State = (int)Common.Enum.PayStatus.已支付;

                db.SaveChanges();
            }


            return null;
        }
        private ActionResult CFJPayBack(string out_trade_no)
        {
            CFJPay pay = db.CFJPay.Find(int.Parse(out_trade_no));

            //防止服务器多次调用后产生多次记录
            if (pay.State == (int)Common.Enum.PayStatus.未支付)
            {


                PayLog paylog = new PayLog()
                {
                    InDate = DateTime.Now,
                    MemberID = pay.MemberID,
                    Payment = pay.Payment,
                    PayDirection = (int)Common.Enum.PayDirection.购买,
                    RefOrderID = pay.CFJPayID,
                    Param = "cfj"

                };
                db.PayLog.Add(paylog);

                pay.State = (int)Common.Enum.PayStatus.已支付;
                db.SaveChanges();

                db.Database.ExecuteSqlCommand("update members set CFJMemberTypeID={0},ISCFJUser=1 where memberid = {1} "
                              , new object[] { pay.MemberTypeID, pay.MemberID });//给用户赋予相应的角色权限
            }

            return null;


        }

        #endregion

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

        /// <summary>
        /// 微信服务账号
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public string WeiXinServer()
        {
            //return Request.QueryString["echostr"];
            string postStr = "";
            if (Request.HttpMethod.ToLower() == "post")
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                postStr = Encoding.UTF8.GetString(b);
                s.Close();

            }
            //            postStr = @"<xml><ToUserName><![CDATA[gh_9d291a1e3ca6]]></ToUserName>
            //<FromUserName><![CDATA[oUxxct58v2x8g9ZvtjqFFIiWqQ38]]></FromUserName>
            //<CreateTime>1428312101</CreateTime>
            //<MsgType><![CDATA[event]]></MsgType>
            //<Event><![CDATA[SCAN]]></Event>
            //<EventKey><![CDATA[123]]></EventKey>
            //<Ticket><![CDATA[gQFu8DoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xLzNFelktTWZtcmR2VTdPWmM3MktYAAIExUwiVQMECAcAAA==]]></Ticket>
            //</xml>";
            //找到相关的指令  --执行指令


            LoggerHelper.Info(postStr);
            LoggerHelper.Info("DoMessage");
            MessageHelpercs.DoMessage(postStr);


            //            string resultstr = @"<xml>
            //<ToUserName><![CDATA[oUxxct58v2x8g9ZvtjqFFIiWqQ38]]></ToUserName>
            //<FromUserName><![CDATA[gh_9d291a1e3ca6]]></FromUserName>
            //<CreateTime>12345678</CreateTime>
            //<MsgType><![CDATA[news]]></MsgType>
            //<ArticleCount>1</ArticleCount>
            //<Articles>
            //<item>
            //<Title><![CDATA[title1]]></Title> 
            //<Description><![CDATA[description1]]></Description>
            //<PicUrl><![CDATA[picurl]]></PicUrl>
            //<Url><![CDATA[url]]></Url>
            //</item>
            //</Articles>
            //</xml> ";
            string resultstr = "";

            return resultstr;
        }
        [HttpGet]
        public string WeiXin()
        {
            string postStr = "";

            postStr = @"<xml><ToUserName><![CDATA[gh_9d291a1e3ca6]]></ToUserName>
                        <FromUserName><![CDATA[oUxxct58v2x8g9ZvtjqFFIiWqQ38]]></FromUserName>
                        <CreateTime>1428312101</CreateTime>
                        <MsgType><![CDATA[event]]></MsgType>
                        <Event><![CDATA[SCAN]]></Event>
                        <EventKey><![CDATA[123]]></EventKey>
                        <Ticket><![CDATA[gQFu8DoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xLzNFelktTWZtcmR2VTdPWmM3MktYAAIExUwiVQMECAcAAA==]]></Ticket>
                        </xml>";
            MessageHelpercs.DoMessage(postStr);

            //找到相关的指令  --执行指令            


            LoggerHelper.Info(postStr);
            return "ok";
        }
        [HttpGet]
        public string DevMsg()
        {
            //string  echostr= Request.QueryString["echostr"];

            LoggerHelper.Debug("DevMsg");
            return "DevMsg";
        }
        public FileResult GetQrCode(string qraction, string param)
        {
            //HttpHelper http = new HttpHelper();
            //string tokenStr = TokenHelper.GetToken();
            //LoggerHelper.Debug("webdomain:" + SettingBLL.WebDomain);
            //LoggerHelper.Debug("tokenStr:" + tokenStr);
            //http.PostContentType = "json";
            //string qrresult = http.Post("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + tokenStr
            //       , Newtonsoft.Json.JsonConvert.SerializeObject(
            //       new { expire_seconds = 1800, action_name = "QR_SCENE", action_info = new { scene = new { scene_id = 123 } } }
            //       ));
            //LoggerHelper.Debug("qrresult:" + qrresult);


            ////返回图像
            //QrCodeHelper qrHelper = new QrCodeHelper();
            //qrHelper.Content = result.url;
            //byte[] buffer = qrHelper.Render();
            //return File(buffer, "image/png");
            return null;
        }

        public FileResult GetLimitQrCode()
        {

            //返回图像
            QrCodeHelper qrHelper = new QrCodeHelper();
            qrHelper.Content = "http://weixin.qq.com/q/GEzmf3vmutvDEST20WSX";
            //ticket=gQFg8DoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL0dFem1mM3ZtdXR2REVTVDIwV1NYAAIEvQcyVQMEAAAAAA==
            //"scene_str": "linkorder"
            byte[] buffer = qrHelper.Render();
            return File(buffer, "image/png");
        }

        public FileResult GetComQrCode(string param)
        {

            //返回图像
            QrCodeHelper qrHelper = new QrCodeHelper();
            qrHelper.Content = param;
            //ticket=gQFg8DoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL0dFem1mM3ZtdXR2REVTVDIwV1NYAAIEvQcyVQMEAAAAAA==
            //"scene_str": "linkorder"
            byte[] buffer = qrHelper.Render();
            return File(buffer, "image/png");
        }

        public FileResult GetOpenGiftQrCode(string param)
        {

            //返回图像
            QrCodeHelper qrHelper = new QrCodeHelper();
            qrHelper.Content = "http://" + Common.BLL.SettingBLL.MobileDomain + "/gift/" + param;
            //ticket=gQFg8DoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL0dFem1mM3ZtdXR2REVTVDIwV1NYAAIEvQcyVQMEAAAAAA==
            //"scene_str": "linkorder"
            byte[] buffer = qrHelper.Render();
            return File(buffer, "image/png");
        }


        private static string strToken = "";
        private static string strTicket = "";
        private static DateTime curTime_token = DateTime.MinValue;
        private static DateTime curTime_ticket = DateTime.MinValue;
        [HttpGet]
        public string GetToken()
        {
            LoggerHelper.Debug("getoken-token:" + strToken);
            LoggerHelper.Debug("getoken-time:" + curTime_token.ToString());
            if (curTime_token < DateTime.Now.Subtract(new TimeSpan(1, 0, 0)) || strToken == "")
            {
                try
                {

                    HttpHelper http = new HttpHelper();
                    string tokenStr = http.Get(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}"
                         , SettingBLL.AppID, SettingBLL.AppSecret));
                    LoggerHelper.Debug("getoken-in:" + tokenStr);
                    tokenObj token = Newtonsoft.Json.JsonConvert.DeserializeObject<tokenObj>(tokenStr);
                    strToken = token.access_token;
                    curTime_token = DateTime.Now;
                }
                catch (Exception error)
                {

                    throw error;
                }

            }
            return strToken;
        }
        [HttpGet]
        public string GetJsTicket()
        {
            if (curTime_ticket < DateTime.Now.Subtract(new TimeSpan(1, 0, 0)) || strTicket == "")
            {
                HttpHelper http = new HttpHelper();
                //获得ticket 此ticket用于配置js中的config  根据token得来
                string ticketStr = http.Get(
                    string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", GetToken()));
                //解析
                var ticket = Newtonsoft.Json.JsonConvert.DeserializeObject<ticketObj>(ticketStr);
                strTicket = ticket.ticket;

                curTime_ticket = DateTime.Now;
            }

            return strTicket;
        }

        public ActionResult NativePayBack()
        {
            string AppId = "wx845dee61803241bd";
            string MchId = "1233345402";
            string Key = "865315EEBE03DA3FA739AFD399F3327A";

            ResponseHandler resHandler = new ResponseHandler(null);

            //返回给微信的请求
            RequestHandler res = new RequestHandler(null);

            string openId = resHandler.GetParameter("openid");
            string productId = resHandler.GetParameter("product_id");

            if (openId == null || productId == null)
            {
                res.SetParameter("return_code", "FAIL");
                res.SetParameter("return_msg", "回调数据异常");
            }

            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);

            var sp_billno = Guid.NewGuid().ToString().Replace("-", "");//DateTime.Now.ToString("HHmmss") + TenPayV3Util.BuildRandomStr(28);
            var nonceStr = TenPayV3Util.GetNoncestr();

            //创建请求统一订单接口参数
            packageReqHandler.SetParameter("appid", AppId);
            packageReqHandler.SetParameter("mch_id", MchId);
            packageReqHandler.SetParameter("nonce_str", nonceStr);
            packageReqHandler.SetParameter("body", "电吹风智能服务");
            packageReqHandler.SetParameter("out_trade_no", sp_billno);
            packageReqHandler.SetParameter("total_fee", "1");
            packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);
            packageReqHandler.SetParameter("notify_url", "http://www.kongzhongliwu.com/API/PayNotifyUrl/");
            packageReqHandler.SetParameter("trade_type", "NATIVE");
            packageReqHandler.SetParameter("openid", openId);
            packageReqHandler.SetParameter("product_id", productId);

            string sign = packageReqHandler.CreateMd5Sign("key", Key);
            packageReqHandler.SetParameter("sign", sign);

            string data = packageReqHandler.ParseXML();

            try
            {
                //调用统一订单接口
                var result = TenPayV3.Unifiedorder(data);
                var unifiedorderRes = XDocument.Parse(result);
                string prepayId = unifiedorderRes.Element("xml").Element("prepay_id").Value;

                //创建应答信息返回给微信
                res.SetParameter("return_code", "SUCCESS");
                res.SetParameter("return_msg", "OK");
                res.SetParameter("appid", AppId);
                res.SetParameter("mch_id", MchId);
                res.SetParameter("nonce_str", nonceStr);
                res.SetParameter("prepay_id", prepayId);
                res.SetParameter("result_code", "SUCCESS");
                res.SetParameter("err_code_des", "OK");

                string nativeReqSign = res.CreateMd5Sign("key", Key);
                res.SetParameter("sign", nativeReqSign);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString() + "----" + data);
                res.SetParameter("return_code", "FAIL");
                res.SetParameter("return_msg", "统一下单失败");
            }

            return Content(res.ParseXML());
        }
        [HttpGet]
        public string Dev(string signature, string timestamp, string nonce, string echostr)
        {
            return echostr;
        }


    }

    class QrCodeResult
    {
        public string ticket { get; set; }
        public string expire_seconds { get; set; }
        public string url { get; set; }
    }
    class tokenObj
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }

    public class ticketObj
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public string expires_in { get; set; }

    }
}
