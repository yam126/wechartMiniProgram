using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ncc2019.Common.Enum;
using ncc2019.Common.Tool;
using ncc2019.Common.BLL;
using ncc2019.Controllers;
using System.Data;
using System.Data.Entity;

namespace ncc2019.Common
{
    public class NCCCommonRule
    {
        protected static ncc2019Entities db = DBEntities.GetEntitiesNew();
        static DateTime checkSellerOnlineTimer = DateTime.MinValue;
        static object LockObj = new object();
        static bool IsWorking = false;

        public static void CheckSellerIsOnline()
        {
            DateTime t1 = DateTime.Now;
            try
            {
               
                //LoggerHelper.Debug("CheckSellerIsOnline begin");
                DateTime lastDate = DateTime.Now.AddSeconds(-20);
                DateTime curDate = DateTime.Now;
                //检查是否有需要强制线下的商家
                var sellerList = db.Seller.Where(c => c.IsOnline == (int)ShiFouStatus.是 && c.LastDate.Value < lastDate);
                if (sellerList.Count() > 0)
                {
                    foreach (var item in sellerList)
                    {
                        var lotteryList = db.NCCLottery.Where(c => c.RefSellerID == item.SellerID &&
                        c.LotteryStatus == (int)LotteryStatus.已支付 &&
                        c.PrizeStatus == (int)PrizeStatus.未开奖);
                        if (lotteryList.Count() > 0)
                        {
                            foreach (var subitem in lotteryList)
                            {
                                //在线商家且销售这个类型彩票的商家
                                var seller = (from s in db.Seller
                                              join r in db.LotterySellerRef on s.SellerID equals r.SellerID
                                              where
                                              s.IsOnline == (int)ShiFouStatus.是 && s.Status == (int)SellerStatus.正常 &&
                                              s.LastDate.Value < lastDate
                                              orderby s.TheOrder
                                              orderby s.QueueNum
                                              select s).FirstOrDefault();
                                if (seller != null)
                                {
                                    subitem.RefSellerID = seller.SellerID;
                                }
                                else
                                {
                                    subitem.RefSellerID = null;
                                }

                            }
                        }
                        item.IsOnline = (int)ShiFouStatus.否;
                    }
                    db.SaveChanges();
                }

                //检查是否有未分配的待开奖彩票
                var lotteryListNull = db.NCCLottery.Where(c => c.RefMemberID != null &&
                        c.LotteryStatus == (int)LotteryStatus.已支付 &&
                        c.RefSellerID == null && c.PrizeStatus == (int)PrizeStatus.未开奖);
                if (lotteryListNull.Count() > 0)
                {
                    //   var seller = db.Seller.Where(c => c.IsOnline == (int)ShiFouStatus.是 &&
                    //   c.Status == (int)SellerStatus.正常)
                    //.OrderBy(c => c.TheOrder).OrderBy(c => c.QueueNum).FirstOrDefault();
                    foreach (var subitem in lotteryListNull)
                    {
                        //在线商家且销售这个类型彩票的商家
                        var seller = (from s in db.Seller
                                      join r in db.LotterySellerRef on s.SellerID equals r.SellerID
                                      where
                                      s.IsOnline == (int)ShiFouStatus.是 && s.Status == (int)SellerStatus.正常
                                      orderby s.TheOrder
                                      orderby s.QueueNum
                                      select s).FirstOrDefault();
                        if (seller != null)
                        {
                            subitem.RefSellerID = seller.SellerID;
                            db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction,
                                "update seller set QueueNum =QueueNum+1 where sellerid={0}",
                                seller.SellerID);
                            db.SaveChanges();
                        }
                        else
                        {
                            //找不到开奖卖家则走退款流程                           
                            PayBack(subitem, curDate, "开奖方未分配成功");
                        }
                    }

                }

                //已经分配商家 但是商家不在线 将重新分配这些买家 或者做退款操作
                //和第一个逻辑有点重复
                var lotterylistSellerNotNull = from c in db.NCCLottery
                                               join s in db.Seller on c.RefSellerID equals s.SellerID
                                               where c.RefMemberID != null && c.PrizeStatus == (int)PrizeStatus.未开奖 &&
                                               c.LotteryStatus == (int)LotteryStatus.已支付 &&
                                               s.IsOnline == (int)ShiFouStatus.否 && s.Status == (int)SellerStatus.正常                                                
                                               select c;
                if (lotterylistSellerNotNull.Count() > 0)
                {
                    //var seller = db.Seller.Where(c => c.IsOnline == (int)ShiFouStatus.是 && c.Status == (int)SellerStatus.正常
                    //&& c.LastDate.Value < lastDate)
                    //.OrderBy(c => c.TheOrder).OrderBy(c => c.QueueNum).FirstOrDefault();
                    foreach (var subitem in lotterylistSellerNotNull)
                    {
                        //在线商家且销售这个类型彩票的商家
                        var seller = (from s in db.Seller
                                      join r in db.LotterySellerRef on s.SellerID equals r.SellerID
                                      where
                                      s.IsOnline == (int)ShiFouStatus.是 && s.Status == (int)SellerStatus.正常 &&
                                      s.LastDate.Value < lastDate
                                      orderby s.TheOrder
                                      orderby s.QueueNum
                                      select s).FirstOrDefault();
                        if (seller != null)
                        {
                            subitem.RefSellerID = seller.SellerID;
                            db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction,
                                "update seller set QueueNum =QueueNum+1 where sellerid={0}",
                                seller.SellerID);
                            db.SaveChanges();
                        }
                        else
                        {
                            //找不到开奖卖家则走退款流程                            
                            PayBack(subitem, curDate,"开奖方未分配成功");
                        }

                    }


                }

                //若一个小时还未被刮或分配，则将重新分配或退款
                DateTime timeoutDate = DateTime.Now.AddHours(-1);
                var lotteryListTimeOut = db.NCCLottery.Where(c => c.RefMemberID != null &&
                       c.AddDate > timeoutDate &&
                       c.LotteryStatus == (int)LotteryStatus.已支付 &&
                       c.RefSellerID == null && c.PrizeStatus == (int)PrizeStatus.未开奖);
                if (lotteryListTimeOut.Count() > 0)
                {

                    foreach (var subitem in lotteryListTimeOut)
                    {
                        //直接走退款流程                      
                        PayBack(subitem, curDate,"开奖方长时间未分配成功");
                    }

                }

            }
            catch (Exception error)
            {
                IsWorking = false;
                LoggerHelper.BigError(error.ToString());

            }
            finally
            {
                IsWorking = false;
            }


            DateTime t2 = DateTime.Now;
            //LoggerHelper.Info("++NCCCommonRule:"+t2.Subtract(t1).TotalMilliseconds.ToString());
        }
        /// <summary>
        /// 彩票退款流程
        /// </summary>
        /// <param name="lottery"></param>
        /// <param name="curDate"></param>
        public static void PayBack(NCCLottery tempLottery, DateTime curDate,string reason)
        {
            //启动事务

            
            var lottery = db.NCCLottery.Find(tempLottery.NCCLotteryID);
            //lottery.RefSellerID = null;
            var member = db.Members.Find(lottery.RefMemberID);
            decimal? payment = lottery.Payment;//退款金额为彩票单价
                                               //买家获得退款
            CommonPay payback = new CommonPay()
            {
                AddDate = curDate,
                GoodID = lottery.NCCLotteryID,
                HeadImgUrl = member.HeadImgUrl,
                MemberID = member.MemberID,
                Name = member.Name,
                NeedPay = payment,
                Payment = payment,
                PayDirection = (int)PayDirection.退款,
                PayStatus = (int)PayStatus.已支付,
                PayType = (int)PayType.自己账户,
                RefOrderID = lottery.RefOrderID,
                TotalPayment = payment
            };
            //系统产生一笔退款操作
            CommonPay paybacksys = new CommonPay()
            {
                AddDate = curDate,
                GoodID = lottery.NCCLotteryID,
                HeadImgUrl = "",
                MemberID = SettingBLL.SysMemberID,
                Name = "",
                NeedPay = payment,
                Payment = payment,
                PayDirection = (int)PayDirection.退款,
                PayStatus = (int)PayStatus.已支付,
                PayType = (int)PayType.系统账户,
                RefOrderID = lottery.RefOrderID,
                TotalPayment = payment
            };
            db.CommonPay.Add(payback);
            db.CommonPay.Add(paybacksys);

            lottery.LotteryStatus = (int)LotteryStatus.已经退款;
            lottery.PayBackReason = reason;
            //db.SaveChanges();
            db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction
                , "update Members set Balance =Balance+{1} where MemberID={0}"
                , member.MemberID, payment);
            db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction
                , "update Members set Balance =Balance-{1} where MemberID={0}"
                , SettingBLL.SysMemberID, payment);

            db.SaveChanges();

        }

        public static void CheckSellerIsOnlineAsyn()
        {
            //LoggerHelper.Debug("CheckSellerIsOnlineAsyn begin " + IsWorking);          
            if (checkSellerOnlineTimer.AddSeconds(3) <= DateTime.Now
                && IsWorking == false)
            {
                IsWorking = true;
                lock (LockObj)
                {
                    System.Threading.Thread t = new System.Threading.Thread(c =>
                    {
                        NCCCommonRule.CheckSellerIsOnline();
                        checkSellerOnlineTimer = DateTime.Now;
                    });
                    t.Start();

                }

            }


        }


        public static void CommonRule(int sellerMemberID, int buyerMemberID, decimal lotteryPaymnet, decimal lotteryBouns)
        {
            decimal sellerMoney = lotteryPaymnet * 97 / 100;



        }

    }
}