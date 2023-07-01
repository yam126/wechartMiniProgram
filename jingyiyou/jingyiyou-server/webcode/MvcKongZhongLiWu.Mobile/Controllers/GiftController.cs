using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Enum;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using ncc2019.Common.BLL;
using Senparc.Weixin;
using ncc2019.Common;
using System.Collections;
using ncc2019.Common.Weixin.Base;

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
            ViewBag.shorturl = shorturl;
            var orderModel = db.Orders.Where(c => c.ShortUrl == shorturl).FirstOrDefault();
            ViewBag.orderid = Common.Tool.DESEncrypt.Encrypt(orderModel.OrderID);



            if (orderModel == null)
            {
                return Content("此礼物不存在！");
            }
            ViewBag.passtip = string.IsNullOrEmpty(orderModel.ThePassTip) ? "这家伙很懒没有留下任何问题或提示！" : orderModel.ThePassTip;

            //if (orderModel.Goods.GoodType == (int)GoodType.贺卡)
            //{
            //    ViewBag.sayetc = orderModel.SayEtc;
            //    ViewBag.fromname = orderModel.FromName;
            //    if (orderModel.ThePass != null)
            //    {
            //        return Redirect("/gift/Take?shorturl=" + orderModel.ShortUrl);
            //    }
            //    else
            //    {
            //        return Redirect("/cards?orderid=" + ViewBag.orderid);
            //    }

            //}

            //参数准备
            SetShareParam2(orderModel);

            return View(orderModel);

            //if (string.IsNullOrEmpty(orderModel.ThePass))
            //{              

            //    if (orderModel.GivenStatus == (int)Common.Enum.GivenStatus.未送出)
            //    {
            //        ViewBag.isopen = false;
            //        if (this.CurMemberInfo != null && this.CurMemberInfo.MemnerID != orderModel.MemberID)
            //        {

            //            int count = db.Database.ExecuteSqlCommand("update orders set GivenStatus={0},ToMemberID={3},ToWeChatOpenid={4} where orderid={1} and GivenStatus={2} "
            //                    , new object[] { (int)Common.Enum.GivenStatus.已经打开, orderModel.OrderID, (int)Common.Enum.GivenStatus.未送出
            //                , this.CurMemberInfo.MemnerID,this.CurMemberInfo.WeChatOpenid});

            //            if (count > 0)
            //            {
            //                #region 记录打开动作
            //                ActionLog actionLog = new ActionLog()
            //                {
            //                    ActionDate = DateTime.Now,
            //                    AtionType = (int)AtionType.打开礼物,
            //                    Title = this.CurMemberInfo.Name + " 打开了礼物",
            //                    MemberID = this.CurMemberInfo.MemnerID,
            //                    OrderID = orderModel.OrderID
            //                };
            //                db.ActionLog.Add(actionLog);
            //                db.SaveChanges();
            //                #endregion

            //                //获取礼物成功
            //                ViewBag.isopen = false;
            //            }
            //            else
            //            {
            //                //获取礼物失败
            //                ViewBag.isopen = true;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.isopen = true;
            //    }

            //    SetShareParam(orderModel);

            //    ViewBag.js_json = TenPayManager.MakeUpJsParam();

            //    //则是免密码模式
            //    return View("~/Views/Gift/index.cshtml", orderModel);
            //}
            //else
            //{
            //    return View("~/Views/Gift/jump.cshtml", orderModel);
            //}




        }
        private void SetShareParam2(Orders orderModel)
        {
            //获取到所有的评论
            List<Comments> comments = db.Comments.Where(c => c.OrderID == orderModel.OrderID).OrderByDescending(c => c.CommentID).ToList();
            ViewBag.comments = comments;

            //分享链接准备
          //  ViewBag.goodimgurl = "http://" + SettingBLL.WebDomain + "/" + orderModel.Goods.ImgUrl;
            string key = "1_" + orderModel.OrderID;
            if (this.CurMemberInfo != null)
            {
                key = this.CurMemberInfo.MemnerID + "_" + orderModel.OrderID;
            }
            ViewBag.linkurl = "http://" + SettingBLL.MobileDomain + "/home/buyone?key=" + Common.Tool.DESEncrypt.Encrypt(key);
            //没有关注的话引导关注
            ViewBag.guanzhu = false;
            if (this.CurMemberInfo != null)
            {
                //var member = db.Members.Where(c => c.MemberID == this.CurMemberInfo.MemnerID).FirstOrDefault();
                if (this.CurMemberInfo.IsGuanZhu == true)
                {
                    ViewBag.guanzhu = true;
                }
                ViewBag.myname = this.CurMemberInfo.Name;
            }
            //
            ViewBag.frommemerid = orderModel.MemberID;
            //发送礼物者的个人头像信息
            var member = db.Members.Where(c => c.MemberID == orderModel.MemberID).FirstOrDefault();
            if (member != null)
            {
                ViewBag.fromuserimgurl = member.HeadImgUrl;
            }

            ViewBag.js_json = TenPayManager.MakeUpJsParam();
            //分配token    
            ViewBag.token = Guid.NewGuid().ToString().Replace("-", "");
        }

        private void SetShareParam(Orders orderModel)
        {
            var tomemeber = db.Members.Where(c => c.MemberID == orderModel.ToMemberID || c.WechatOpenid == orderModel.ToWeChatOpenid).FirstOrDefault();
            if (tomemeber != null)
            {
                ViewBag.toname = tomemeber.Name;
                ViewBag.touserimgurl = tomemeber.HeadImgUrl;
            }
         //   ViewBag.goodimgurl = "http://" + SettingBLL.WebDomain + "/" + orderModel.Goods.ImgUrl;
            string key = "1_" + orderModel.OrderID;
            if (this.CurMemberInfo != null)
            {
                key = this.CurMemberInfo.MemnerID + "_" + orderModel.OrderID;
            }
            ViewBag.linkurl = "http://" + SettingBLL.MobileDomain + "/home/buyone?key=" + Common.Tool.DESEncrypt.Encrypt(key);
            //没有关注的话引导关注
            ViewBag.guanzhu = false;
            if (this.CurMemberInfo != null)
            {
                //var member = db.Members.Where(c => c.MemberID == this.CurMemberInfo.MemnerID).FirstOrDefault();
                //if (member.IsGuanZhu == 1)
                //{
                //    ViewBag.guanzhu = true;
                //}
                ViewBag.myname = this.CurMemberInfo.Name;
            }


        }
        [HttpPost]
        public ActionResult Index(string shorturl = "", string sayback = "", string token = "")
        {

            if (token == "") token = "null";
            string state = "error";
            ViewBag.shorturl = shorturl;
            var orderModel = db.Orders.Where(c => c.ShortUrl == shorturl).FirstOrDefault();
            ViewBag.orderid = Common.Tool.DESEncrypt.Encrypt(orderModel.OrderID);

            if (orderModel != null)
            {
                try
                {
                    //检查token
                    Hashtable ht = (Hashtable)Common.Tool.SessionHelper.GetSessionObj("sayback_token");
                    if (ht == null)
                    {
                        ht = new Hashtable();
                    }
                    if (!ht.ContainsKey(token))
                    {
                        Comments comment = new Comments()
                        {
                            Content = sayback,
                            Name = CurMemberInfo.Name,
                            HeadImgUrl = CurMemberInfo.HeadImgUrl,
                            MemberID = CurMemberInfo.MemnerID,
                            OrderID = orderModel.OrderID,
                            AddDate = DateTime.Now

                        };
                        db.Comments.Add(comment);
                        db.SaveChanges();


                        if (!ht.ContainsKey(token))
                        {
                            ht.Add(token, token);
                        }
                        Common.Tool.SessionHelper.SetSession("sayback_token", ht);

                        state = "ok";

                        showSuccessMessage("回复成功！");
                    }

                }
                catch (Exception error)
                {
                    Common.Tool.LoggerHelper.Info(error.ToString());
                    ShowAlertMessage("回复失败，请稍后再试！");
                }

            }

            SetShareParam2(orderModel);

            return View("~/Views/Gift/index.cshtml", orderModel);
        }
        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="thepass"></param>
        /// <param name="shorturl"></param>
        /// <returns></returns>
        public ActionResult Verf(string thepass, string shorturl)
        {

            var orderModel = db.Orders.Where(c => c.ShortUrl == shorturl).FirstOrDefault();
            ViewBag.orderid = ncc2019.Common.Tool.DESEncrypt.Encrypt(orderModel.OrderID);

            if (orderModel == null)
            {
                ShowAlertMessage("请重新打开礼物！");
                return View("~/Views/Gift/jump.cshtml", orderModel);
            }
            else
            {
                #region 检查礼物是否属于自己
                //如果礼物已经被认领 且打开的人不是本人一律不允许打开
                if (!string.IsNullOrEmpty(orderModel.ToWeChatOpenid) || !string.IsNullOrEmpty(orderModel.ToMemberID.ToString()))
                {
                    if (this.CurMemberInfo == null)
                    {
                        return View("/views/gift/noright.cshtml");
                    }
                    if (this.CurMemberInfo.MemnerID == orderModel.ToMemberID || this.CurMemberInfo.WeChatOpenid == orderModel.ToWeChatOpenid)
                    {
                        //此礼物被自己领取
                    }
                    else
                    {
                        return View("/views/gift/noright.cshtml");
                    }

                }
                #endregion

                var member = db.Members.Where(c => c.MemberID == orderModel.MemberID).FirstOrDefault();
                if (member != null)
                {
                    ViewBag.fromuserimgurl = member.HeadImgUrl;
                }
                ViewBag.shorturl = shorturl;
                if (orderModel.ThePass == thepass)
                {
                    if (Common.Tool.UserAgentHelper.IsWeiXin())
                    {
                        ViewBag.js_json = TenPayManager.MakeUpJsParam();
                    }
                    else
                    {
                        ViewBag.js_json = new JsResult();
                    }
                    if (orderModel.GivenStatus == (int)Common.Enum.GivenStatus.未送出)
                    {
                        ViewBag.isopen = false;
                        if (this.CurMemberInfo != null && this.CurMemberInfo.MemnerID != orderModel.MemberID)
                        {
                            int count = db.Database.ExecuteSqlCommand("update orders set GivenStatus={0},ToMemberID={3},ToWeChatOpenid={4} where orderid={1} and GivenStatus={2} "
                                , new object[] { (int)Common.Enum.GivenStatus.已经打开, orderModel.OrderID, (int)Common.Enum.GivenStatus.未送出
                            , this.CurMemberInfo.MemnerID,this.CurMemberInfo.WeChatOpenid});

                            if (count > 0)
                            {
                                #region 记录打开动作
                                ActionLog actionLog = new ActionLog()
                                {
                                    ActionDate = DateTime.Now,
                                    AtionType = (int)AtionType.打开礼物,
                                    Title = this.CurMemberInfo.Name + " 打开了礼物",
                                    MemberID = this.CurMemberInfo.MemnerID,
                                    OrderID = orderModel.OrderID
                                };
                                db.ActionLog.Add(actionLog);
                                db.SaveChanges();
                                #endregion

                                SetShareParam(orderModel);
                                //获取礼物成功
                                return VerfOK(orderModel);
                            }
                        }
                        else if (this.CurMemberInfo != null && this.CurMemberInfo.MemnerID == orderModel.MemberID)
                        {
                            SetShareParam(orderModel);
                            //如果是自己打开的直接放行
                            return VerfOK(orderModel);
                        }
                        else if (this.CurMemberInfo == null)
                        {
                            SetShareParam(orderModel);
                            //如果没有登陆也可以放行
                            return VerfOK(orderModel);
                        }
                    }
                    else
                    {
                        if (this.CurMemberInfo == null)
                        {
                            //若礼物已经被占用  则不允许匿名打开了
                            return View("/views/gift/noright.cshtml");
                        }
                        else
                        {
                            if (this.CurMemberInfo.WeChatOpenid == orderModel.ToWeChatOpenid || this.CurMemberInfo.MemnerID == orderModel.ToMemberID)
                            {
                                SetShareParam(orderModel);
                                //若自己登陆了  且礼物属于自己则放过
                                return VerfOK(orderModel);
                            }

                        }
                    }
                    //礼物已经被抢占
                    //ShowAlertMessage("很抱歉此礼物已经被领取！");
                    //return View("~/Views/Gift/jump.cshtml");
                    return View("/views/gift/noright.cshtml");
                }
                else
                {
                    ShowAlertMessage("密码错误！");
                    //Response.Write("<script>alert('密码错误！')</script>");
                    return View("~/Views/Gift/jump.cshtml", orderModel);
                }


            }
        }

        private ActionResult VerfOK(Orders orderModel)
        {
            //if (orderModel.Goods.GoodType == (int)GoodType.贺卡)
            //{
            //    return Redirect("/cards?orderid=" + GetOrderId_Encrypt(orderModel.OrderID));
            //}
            //else
            //{
            //    return View("~/Views/Gift/take.cshtml", orderModel);
            //}
            return null;

        }

        /// <summary>
        /// 填写接收地址
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Take(string shorturl)
        {

            var orderModel = db.Orders.Where(c => c.ShortUrl == shorturl).FirstOrDefault();
            ViewBag.orderid = ncc2019.Common.Tool.DESEncrypt.Encrypt(orderModel.OrderID);

            #region 当此礼物为虚拟盒子的时候，跳到礼物挑选界面
            if (orderModel.GoodID < 20)
            {
                return Redirect("/guide/ToChoiceGood?orderid=" + ViewBag.orderid);
            }
            #endregion


            if (!string.IsNullOrEmpty(orderModel.ThePass))
            {
                return View("~/Views/Gift/jump.cshtml", orderModel);
            }

            if (!string.IsNullOrEmpty(orderModel.ToAddress))
            {
                if (orderModel.ToMemberID == null)
                {
                    orderModel.ToMemberID = CurMemberInfo.MemnerID;
                    orderModel.ToWeChatOpenid = CurMemberInfo.WeChatOpenid;
                    db.SaveChanges();
                }

                return View("~/Views/Gift/taken.cshtml", orderModel);
            }
            else
            {
                return View("~/Views/Gift/take.cshtml", orderModel);
            }


            //var orderModel = db.Orders.Where(c => c.ShortUrl == "994ba544").FirstOrDefault();
            //return View("~/Views/Gift/take.cshtml", orderModel);
        }
        /// <summary>
        /// 浏览个人信息以及物流信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private ActionResult Taken(Orders orderModel)
        {

            //var orderModel = db.Orders.Where(c => c.ShortUrl == code).FirstOrDefault();
            return View("~/Views/Gift/take.cshtml", orderModel);

            //var orderModel = db.Orders.Where(c => c.ShortUrl == "994ba544").FirstOrDefault();
            //return View("~/Views/Gift/take.cshtml", orderModel);
        }
        /// <summary>
        /// 获取礼物详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id = 0)
        {
            Goods good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            return View(good);
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
        public ActionResult DoCreate(string orderid, string recname, string recphone, string recaddress)
        {
            var orderModel = GetOrder();
            orderModel.ToName = recname;
            orderModel.ToPhone = recphone;
            orderModel.ToAddress = recaddress;
            //orderModel.Memo = memo;
            orderModel.GivenStatus = (int)GivenStatus.已送出;

            if (this.CurMemberInfo != null && this.CurMemberInfo.MemnerID == orderModel.MemberID)
            {
                //如果是自己送出的在此环节进行绑定礼物
                int count = db.Database.ExecuteSqlCommand("update orders set GivenStatus={0},ToMemberID={3},ToWeChatOpenid={4} where orderid={1} and GivenStatus={2} "
                            , new object[] { (int)Common.Enum.GivenStatus.已送出, orderModel.OrderID, (int)Common.Enum.GivenStatus.未送出
                            , this.CurMemberInfo.MemnerID,this.CurMemberInfo.WeChatOpenid});
                if (count > 0)
                {
                    #region 记录打开动作
                    ActionLog actionLog = new ActionLog()
                    {
                        ActionDate = DateTime.Now,
                        AtionType = (int)AtionType.填写地址,
                        Title = this.CurMemberInfo.Name + " 填写了接收地址",
                        MemberID = this.CurMemberInfo.MemnerID,
                        OrderID = orderModel.OrderID
                    };
                    db.ActionLog.Add(actionLog);

                    #endregion

                    db.Entry(orderModel).Property(c => c.ToName).IsModified = true;
                    db.Entry(orderModel).Property(c => c.ToPhone).IsModified = true;
                    db.Entry(orderModel).Property(c => c.ToAddress).IsModified = true;
                    db.SaveChanges();
                }
                else
                {
                    ShowAlertMessage("礼物已经被其他人领取！");
                    return View("~/Views/Gift/take.cshtml", orderModel);
                }

            }
            else
            {
                //如果是自己送出的在此环节进行绑定礼物
                int count = db.Database.ExecuteSqlCommand("update orders set GivenStatus={0},ToMemberID={3},ToWeChatOpenid={4} where orderid={1} and GivenStatus!={2} "
                            , new object[] { (int)Common.Enum.GivenStatus.已送出, orderModel.OrderID, (int)Common.Enum.GivenStatus.已送出
                            , this.CurMemberInfo.MemnerID,this.CurMemberInfo.WeChatOpenid});
                if (count > 0)
                {
                    #region 记录打开动作
                    ActionLog actionLog = new ActionLog()
                    {
                        ActionDate = DateTime.Now,
                        AtionType = (int)AtionType.填写地址,
                        Title = this.CurMemberInfo.Name + " 填写了接收地址",
                        MemberID = this.CurMemberInfo.MemnerID,
                        OrderID = orderModel.OrderID
                    };
                    db.ActionLog.Add(actionLog);

                    #endregion
                    //如果不是自己送出的在上一环节已经绑定礼物
                    db.Entry(orderModel).Property(c => c.ToName).IsModified = true;
                    db.Entry(orderModel).Property(c => c.ToPhone).IsModified = true;
                    db.Entry(orderModel).Property(c => c.ToAddress).IsModified = true;
                    //db.Entry(orderModel).Property(c => c.GivenStatus).IsModified = true;
                    db.SaveChanges();
                }
            }



            return Redirect("~/Gift/OK?orderid=" + orderid);
        }

        /// <summary>
        /// 配送成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult OK(string orderid = "")
        {


            var order = GetOrder();
            if (order.IsForMe == (int)Common.Enum.ShiFouStatus.是)
            {
                ViewBag.isforme = true;
            }
            else
            {
                ViewBag.isforme = false;
            }
            #region 如果为后支付订单，需要通知购买人完成支付

            if (order.PayLate == (int)ShiFouStatus.是)
            {
                CustomHelper.SendUnpayAlertUseTemplate("", order);
            }

            #endregion

            #region 如果为先支付的，且购买的为礼物包，则退款到购买人账户

            if (order.PayStatus == (int)PayStatus.已支付 && order.ToPriceZone != null)
            {
                string[] pricelist = order.ToPriceZone.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                if (pricelist.Length == 2)
                {
                    decimal price1 = decimal.Parse(pricelist[0]);
                    decimal price2 = decimal.Parse(pricelist[1]);
                    decimal price_back = price2 - order.TotalPayment.Value;
                    if (price_back > 0)
                    {

                        int paylogCount = db.PayLog.Where(c => c.RefOrderID == order.OrderID && c.PayDirection == (int)PayDirection.退款).Count();
                        if (paylogCount == 0)
                        {//防止多次提交，多次退款
                            PayLog paylog_cha = new PayLog()
                            {
                                InDate = DateTime.Now,
                                MemberID = order.MemberID,
                                Payment = price_back,
                                PayDirection = (int)Common.Enum.PayDirection.退款,
                                RefOrderID = order.OrderID
                            };
                            db.PayLog.Add(paylog_cha);
                            db.SaveChanges();

                            db.Database.ExecuteSqlCommand("update Members set Balance = Balance + {0} where MemberID = {1} ", price_back, order.MemberID);
                            CustomHelper.SendPaymentBackUseTemplate("", order, price_back);
                        }


                    }

                }

            }

            #endregion

           // ViewBag.goodimgurl = "http://" + SettingBLL.WebDomain + "/" + order.Goods.ImgUrl;
            string key = "1_" + order.OrderID;
            if (this.CurMemberInfo != null)
            {
                key = this.CurMemberInfo.MemnerID + "_" + order.OrderID;
            }
            ViewBag.linkurl = "http://" + SettingBLL.MobileDomain + "/home/buyone?key=" + Common.Tool.DESEncrypt.Encrypt(key);
            //没有关注的话引导关注
            ViewBag.guanzhu = false;
            if (this.CurMemberInfo != null)
            {
                var member = db.Members.Where(c => c.MemberID == this.CurMemberInfo.MemnerID).FirstOrDefault();
                if (member.IsGuanZhu == 1)
                {
                    ViewBag.guanzhu = true;
                }
                ViewBag.myname = this.CurMemberInfo.Name;
            }

            ViewBag.js_json = TenPayManager.MakeUpJsParam();


            //var orderModel = db.Orders.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(orderid)));
            //if (string.IsNullOrEmpty(orderModel.ToWeChatOpenid))
            //{
            //    orderModel.ToWeChatOpenid = this.CurMemberInfo.WeChatOpenid;

            //    db.Entry(orderModel).State = EntityState.Unchanged;
            //    db.Entry(orderModel).Property(c => c.ToWeChatOpenid).IsModified = true;
            //    db.SaveChanges();
            //}

            ViewBag.imgurl = "http://" + SettingBLL.WebDomain + "/api/GetQrCode?qraction=linkorder&param=" + orderid + "&rnd=" + DateTime.Now.ToString("fffff");

            return View(order);
        }
    }
}
