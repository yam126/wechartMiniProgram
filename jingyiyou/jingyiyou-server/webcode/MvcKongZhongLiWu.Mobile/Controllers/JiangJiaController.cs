using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Tool;
using ncc2019.Common.Enum;
using ncc2019.Common.BLL;
using ncc2019.Common;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin;
using System.Data;
using Senparc.Weixin.Exceptions;
using System.Text;
using MC = Com.AlipayM;

namespace ncc2019.Controllers
{
    public class JiangJiaController : ControllerBase
    {
        //
        // GET: /JiangJia/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Good(int id)
        {
            if (CheckUserCur() != null)
            {
                return CheckUserCur();
            }

            #region 个人信息

            ViewBag.tilinum = 0;

            #endregion

            var goodModel = db.Goods.AsNoTracking().FirstOrDefault(c => c.GoodID == id);

            ViewBag.goodid = id;
            ViewBag.isselling = false;
            ViewBag.isweixin = UserAgentHelper.IsWeiXin() ? "true" : "false";
            if (goodModel.BeginDate < DateTime.Now && goodModel.EndDate > DateTime.Now)
            {
                ViewBag.isselling = true;
            }
            ViewBag.goodfee = goodModel.Payment;
            ViewBag.plist = db.GoodProperty.Where(c => c.GoodID == id).OrderBy(c => c.GoodPropertyID).ToList();
            TimeSpan lastSp = goodModel.EndDate.Value.Subtract(DateTime.Now);
            ViewBag.lasttime = lastSp.TotalMilliseconds;
            ViewBag.issellout = false;
            if (goodModel.BuyCount >= goodModel.TotalNum)
            {
                ViewBag.issellout = true;
            }


            goodModel.ViewCount++;
            db.SaveChanges();
            ViewBag.gotologin = "true";
            SessionHelper.SetJumpUrl("/jiangjia/good?id=" + id);
            if (CurMemberInfo != null)
            {
                ViewBag.gotologin = "false";
                ViewBag.tilinum = SessionHelper.CurMemberInfo.TiLiNum;

                #region 查看是否参与了此次活动

                var right = db.GoodRight.AsNoTracking().Where(c => c.BeginDate < DateTime.Now && c.EndDate > DateTime.Now && c.MemberID == CurMemberInfo.MemnerID && c.GoodID == id).FirstOrDefault();
                ViewBag.isjoin = false;
                if (right != null)
                {
                    //有资格参加
                    ViewBag.isjoin = true;
                }

                #endregion

                #region 锁定产品的价格
                //如果此人已经锁定价格在未完成支付前，看到的价格就是锁定的价格
                var orderModel = db.Orders.AsNoTracking().Where(c => c.GoodID == id && c.PayStatus == (int)PayStatus.未支付 && c.MemberID == CurMemberInfo.MemnerID).FirstOrDefault();
                if (orderModel != null)
                {
                    ViewBag.goodfee = orderModel.Payment;
                }
                #endregion

                #region 分享参数
                ViewBag.link = "http://" + SettingBLL.MobileDomain + "/jiangjia/Support?goodid=" + id + "&mid=" + GetOrderId_Encrypt(CurMemberInfo.MemnerID);
                #endregion
            }
            #region 分享参数
            ViewBag.js_json = TenPayManager.MakeUpJsParam();

            ViewBag.imgurl = "http://" + SettingBLL.MobileDomain + "/" + goodModel.ImgUrl;
            #endregion

            return View(goodModel);
        }

        /// <summary>
        /// 检查是否可以参加降价活动
        /// </summary>
        /// <returns></returns>
        public ActionResult Join(int goodid)
        {
            var goodModel = db.Goods.Find(goodid);
            //判断体力是否达标
            if (CurMemberInfo.TiLiNum >= goodModel.NeedTiLiNum)
            {
                //记录参与权限
                var rightModel = new GoodRight()
                {
                    BeginDate = DateHelper.MinTimeOfToday,
                    EndDate = DateHelper.MaxTimeOfToday,
                    GoodID = goodid,
                    MemberID = CurMemberInfo.MemnerID
                };
                db.GoodRight.Add(rightModel);
                db.SaveChanges();
                //进行降价
                db.Database.ExecuteSqlCommand("update goods set payment = payment-{0},JoinPeopleNum=JoinPeopleNum+1 where goodid={1}", goodModel.DownPayment, goodModel.GoodID);
                MemberBLL.ReduceTiLi(db, CurMemberInfo.MemnerID, goodModel.NeedTiLiNum.Value);

            }


            return Redirect("/jiangjia/good?id=" + goodid);
        }

        public ActionResult Buy(int goodid, string property)
        {
            var goodModel = db.Goods.Find(goodid);
            if (goodModel.BuyCount < goodModel.TotalNum)
            {
                //还有剩余-跳转到支付页面
                var orderModel = new Orders()
                {
                    AddDate = DateTime.Now,
                    MemberID = CurMemberInfo.MemnerID,
                    OrderStatus = (int)OrderStatus.正常,
                    PayStatus = (int)PayStatus.未支付,
                    ShortUrl = ShortUrlHelper.GetShortUrl(),
                    TranceStatus = (int)TransferStatus.未发货,
                    GivenStatus = (int)GivenStatus.未送出,
                    IsForMe = (int)ShiFouStatus.是,
                    OrderType = (int)OrderType.自动降价,
                    ToShowPrice = (int)ShiFouStatus.是,
                    EndDate = DateTime.Now.AddMinutes(30),
                    Payment = goodModel.Payment,
                    TotalPayment = goodModel.Payment + goodModel.ExpressFee,//锁定当前价格
                    BuyNum = 1,
                    DeliveryPay = goodModel.ExpressFee,
                    Property = property,
                    GoodID = goodid
                };
                orderModel.NeedPay = orderModel.TotalPayment;
                db.Orders.Add(orderModel);
                db.SaveChanges();

                return Redirect("/Pay/My?type=jj&orderid=" + GetOrderId_Encrypt(orderModel.OrderID));
            }

            return null;
        }

        public ActionResult Pay(string orderid)
        {
            var orderModel = GetOrder();
           // ViewBag.Payment = orderModel.Goods.Payment;
            ViewBag.paydata = Newtonsoft.Json.JsonConvert.SerializeObject(TenPayManager.MakeUpJsParam(CurMemberInfo.WeChatOpenid, orderModel));
            ViewBag.yue = SessionHelper.CurMemberInfo.Balance;
            //ViewBag.totalpay = order.TotalPayment;
            ViewBag.apiurl = Common.BLL.SettingBLL.WebDomain;

            return View("/views/jiangjia/pay.cshtml", orderModel);
        }

        [HttpPost]
        public ActionResult Pay(string orderid, string wayofpay2)
        {
            string wayofpay = wayofpay2;
            var orderModel = GetOrder();
            if (orderModel.PayStatus == (int)Common.Enum.PayStatus.已支付)
            {
                return Redirect("/jiangjia/ok?goodid=" + orderModel.GoodID);
            }



            if (wayofpay == "0")
            {
                orderModel.PayType = (int)PayType.自己账户;
            }
            else if (wayofpay == "1")
            {
                orderModel.PayType = (int)PayType.手机支付宝;
            }
            else
            {
                orderModel.PayType = (int)PayType.微信;
            }
            int decode_orderid = orderModel.OrderID;
            decimal? needpay = orderModel.NeedPay.Value;
            decimal? totalpay = orderModel.TotalPayment.Value;

            if (wayofpay != "0" && SessionHelper.CurMemberInfo.Balance >= 0 && SessionHelper.CurMemberInfo.Balance < totalpay)
            {
                //扣除账户中的余额
                Members curMember = db.Members.Find(SessionHelper.CurMemberInfo.MemnerID);
                needpay = totalpay - curMember.Balance;
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
                    MemberID = curMember.MemberID,
                    Payment = curMember.Balance_back,
                    PayDirection = (int)Common.Enum.PayDirection.副账户充值,
                    RefOrderID = decode_orderid
                };
                db.PayLog.Add(paylog);
            }
            else if (wayofpay == "0" && SessionHelper.CurMemberInfo.Balance >= 0 && SessionHelper.CurMemberInfo.Balance >= totalpay)
            {
                Members curMember = db.Members.Find(SessionHelper.CurMemberInfo.MemnerID);
                //curMember.Balance_back = order.TotalPayment;
                //curMember.Balance = curMember.Balance - order.TotalPayment;//直接扣除余额
                needpay = 0;
                //修改订单信息
                orderModel.PayStatus = (int)PayStatus.已支付;



                db.Database.ExecuteSqlCommand("update members set Balance=Balance-{0} where memberid = {1} "
                    , new object[] { totalpay, curMember.MemberID });//直接扣除余额
                db.Database.ExecuteSqlCommand("update goods set BuyCount=BuyCount+1 where goodid = {0} "
                    , new object[] { orderModel.GoodID });//直接扣除余额


                //db.Entry(curMember).State = System.Data.EntityState.Modified;
                //db.SaveChanges();
                //return Redirect("/send?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID));
                PayLog paylog = new PayLog()
                {
                    InDate = DateTime.Now,
                    MemberID = CurMemberInfo.MemnerID,
                    Payment = totalpay,
                    PayDirection = (int)Common.Enum.PayDirection.购买,
                    RefOrderID = decode_orderid
                };
                db.PayLog.Add(paylog);
            }
            else
            {//全额付款
                needpay = totalpay;
                //return AliPay(order);
            }



            int flag = db.SaveChanges();
            //if (flag == 0)
            //{
            //    ShowAlertMessage("付款遇到问题，请稍后重试！");
            //    return View();
            //}

            if (needpay == 0 && wayofpay == "0")
            {
                return Redirect("/address?type=jj&orderid=" + orderid);
            }
            else
            {
                if (UserAgentHelper.IsWeiXin())
                {
                    return Redirect("/jiangjia/weixinalipayorder?orderid=" + Common.Tool.DESEncrypt.Encrypt(orderModel.OrderID));
                }
                else
                {
                   // return AliPay(needpay.Value, orderModel.Goods.Name, Common.Tool.DESEncrypt.Encrypt(orderModel.OrderID), orderModel.GoodID);
                }
                return null;
            }
        }


        [HttpPost]
        /// <summary>
        /// 获取实时动态
        /// </summary>
        /// <param name="goodid"></param>
        /// <returns></returns>
        public JsonResult GetDetail(int goodid)
        {
            try
            {
                var goodModel = db.Goods.AsNoTracking().FirstOrDefault(c => c.GoodID == goodid);
                decimal? goodfee = goodModel.Payment;

                //如果此人已经锁定价格在未完成支付前，看到的价格就是锁定的价格
                var orderModel = db.Orders.AsNoTracking().Where(c => c.GoodID == goodid && c.PayStatus == (int)PayStatus.未支付 && c.MemberID == CurMemberInfo.MemnerID).OrderBy(c => c.OrderID).FirstOrDefault();
                if (orderModel != null)
                {
                    goodfee = orderModel.Payment;
                }

                return Json(new
                {
                    result = "ok",
                    data = new
                    {
                        goodfee = goodfee.Value.ToString("0"),
                        curgnum = goodModel.TotalNum - goodModel.BuyCount,
                        curjoinnum = goodModel.JoinPeopleNum,
                        tili = CurMemberInfo.TiLiNum
                    }
                });
            }
            catch (Exception)
            {

                return Json(new
               {
                   result = "ok"
               });
            }



        }

        public ActionResult Support(string mid, int goodid = 0)
        {
            if (CheckUserCur() != null)
            {
                return CheckUserCur();
            }

            int memberid = int.Parse(GetOrderId_Decrypt(mid));

            //ViewBag.cansupport = false;
            ViewBag.gotologin = "true";
            ViewBag.tilinum = 0;
            SessionHelper.SetJumpUrl(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
            if (CurMemberInfo != null)
            {
                ViewBag.gotologin = "false";
                //一天只能支持一个人一次
                ViewBag.cansupport = false;
               

                #region 分享参数
                ViewBag.link = "http://" + SettingBLL.MobileDomain + "/jiangjia/Support?goodid=" + goodid + "&mid=" + GetOrderId_Encrypt(CurMemberInfo.MemnerID);
                #endregion

            }




            var goodModel = db.Goods.Find(goodid);

            ViewBag.mid = mid;
            ViewBag.goodid = goodid;


            #region 分享参数
            ViewBag.js_json = TenPayManager.MakeUpJsParam();
            ViewBag.linkmore = "http://" + SettingBLL.MobileDomain + "/jiangjia/Support?goodid=" + goodid + "&mid=" + mid;
            ViewBag.imgurl = "http://" + SettingBLL.MobileDomain + "/" + goodModel.ImgUrl;
            #endregion

            return View(goodModel);
        }

        [HttpPost]
        public ActionResult Support(string mid, string goodid)
        {
            int memberid = int.Parse(GetOrderId_Decrypt(mid));
            MemberBLL.AddTiLi(db, memberid);
            MemberBLL.ReduceTiLi(db, CurMemberInfo.MemnerID);
            //记录支持过
            

            return Redirect("/jiangjia/support?mid=" + mid + "&goodid=" + goodid);
        }
        public ActionResult CheckUserCur()
        {
            if (CurMemberInfo != null && CurMemberInfo.IsGuanZhu == false && string.IsNullOrEmpty(CurMemberInfo.Name))
            {
                return Redirect("/jiangjia/getweixininfo?page=" + HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
            }
            return null;
        }

        public ActionResult GetWeiXinInfo(string page)
        {
            var url = TenPayManager.GetAuthorizeUrl("http://" + SettingBLL.MobileDomain + "/jiangjia/SaveWeiXinInfo?page=" + HttpUtility.UrlEncode(page));
            return Redirect(url);
        }

        public ActionResult SaveWeiXinInfo(string code, string state, string page)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }
            OAuthAccessTokenResult result = null;

            //通过，用code换取access_token
            try
            {
                result = OAuthApi.GetAccessToken(SettingBLL.AppID, SettingBLL.AppSecret, code);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }
            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                var member = db.Members.Where(c => c.WechatOpenid == userInfo.openid).FirstOrDefault();
                if (member != null)
                {
                    string nickname = CommonHelper.StripHTML(userInfo.nickname);
                    if (string.IsNullOrEmpty(member.Name))
                    {
                        member.Name = nickname;
                    }
                    if (string.IsNullOrEmpty(member.NiceName))
                    {
                        member.NiceName = nickname;
                    }
                    member.Sex = userInfo.sex;
                    member.Country = userInfo.country;
                    member.City = userInfo.city;
                    member.Province = userInfo.province;
                    member.HeadImgUrl = userInfo.headimgurl;

                    //写入当前用户信息里面
                    CurMemberInfo.HeadImgUrl = member.HeadImgUrl;
                    CurMemberInfo.Name = member.NiceName;

                    //db.Entry(member).State = EntityState.Modified;
                    //db.Entry(member).Property("Balance").IsModified = false;
                    //db.Entry(member).Property("Balance_back").IsModified = false;

                    db.SaveChanges();
                }

                return Redirect(HttpUtility.UrlDecode(page));
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }

        }
        public ActionResult TiLiNum(int goodid)
        {
            ViewBag.goodid = goodid;
            return View();
        }
        [HttpPost]
        public ActionResult TiLiNum(int tilinum, int goodid)
        {
            CommonPay commonpay = null;//ZhongChouPayBLL.GetCurPayByMemberID(db, order.OrderID, CurMemberInfo.MemnerID);

            if (commonpay == null)
            {
                commonpay = new CommonPay()
                {
                    AddDate = DateTime.Now,
                    MemberID = CurMemberInfo.MemnerID,
                    Payment = tilinum,
                    NeedPay = tilinum,
                    PayStatus = (int)PayStatus.未支付,
                    Name = CurMemberInfo.Name,
                    HeadImgUrl = CurMemberInfo.HeadImgUrl,
                    GoodID = goodid
                };
                db.CommonPay.Add(commonpay);
            }
            else
            {
                commonpay.Payment = tilinum;
            }

            db.SaveChanges();
            return Redirect("/Pay/My?type=jjtl&pid=" + GetOrderId_Encrypt(commonpay.CommonPayID));
        }

        public ActionResult PayTiLi(string pid)
        {
            int commonpayid = int.Parse(GetOrderId_Decrypt(pid));
            CommonPay commonpayModel = db.CommonPay.AsNoTracking().FirstOrDefault(c => c.CommonPayID == commonpayid);

            //ViewBag.js_json = TenPayManager.MakeUpJsParam();
            ViewBag.Payment = commonpayModel.NeedPay;
            ViewBag.paydata = Newtonsoft.Json.JsonConvert.SerializeObject(TenPayManager.MakeUpJsParam(CurMemberInfo.WeChatOpenid, commonpayModel));
            ViewBag.yue = SessionHelper.CurMemberInfo.Balance;
            //ViewBag.totalpay = order.TotalPayment;
            ViewBag.apiurl = Common.BLL.SettingBLL.WebDomain;
            ViewBag.pid = pid;

            return View("/views/jiangjia/paytili.cshtml", commonpayModel);
        }

        [HttpPost]
        public ActionResult PayTiLi(string pid, string wayofpay2)
        {
            string wayofpay = wayofpay2;
            int commonpayid = int.Parse(GetOrderId_Decrypt(pid));
            CommonPay commonpayModel = db.CommonPay.AsNoTracking().FirstOrDefault(c => c.CommonPayID == commonpayid);


            if (commonpayModel.PayStatus == (int)Common.Enum.PayStatus.已支付)
            {
                return Redirect("/jiangjia/oktili?goodid=" + commonpayModel.GoodID);
            }

            if (wayofpay == "0")
            {
                commonpayModel.PayType = (int)PayType.自己账户;
            }
            else if (wayofpay == "1")
            {
                commonpayModel.PayType = (int)PayType.手机支付宝;
            }
            else
            {
                commonpayModel.PayType = (int)PayType.微信;
            }
            decimal? needpay = commonpayModel.NeedPay.Value;
            decimal? totalpay = commonpayModel.Payment.Value;

            if (wayofpay != "0" && SessionHelper.CurMemberInfo.Balance >= 0 && SessionHelper.CurMemberInfo.Balance < totalpay)
            {
                //扣除账户中的余额
                Members curMember = db.Members.Find(SessionHelper.CurMemberInfo.MemnerID);
                needpay = totalpay - curMember.Balance;
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
                    MemberID = curMember.MemberID,
                    Payment = curMember.Balance_back,
                    PayDirection = (int)Common.Enum.PayDirection.副账户充值,
                    RefOrderID = commonpayModel.CommonPayID
                };
                db.PayLog.Add(paylog);
            }
            else if (wayofpay == "0" && SessionHelper.CurMemberInfo.Balance >= 0 && SessionHelper.CurMemberInfo.Balance >= totalpay)
            {
                Members curMember = db.Members.Find(SessionHelper.CurMemberInfo.MemnerID);
                //curMember.Balance_back = order.TotalPayment;
                //curMember.Balance = curMember.Balance - order.TotalPayment;//直接扣除余额
                needpay = 0;
                //修改订单信息
                commonpayModel.PayStatus = (int)PayStatus.已支付;


                db.Database.ExecuteSqlCommand("update members set Balance=Balance-{0},tilinum=tilinum+{0} where memberid = {1} "
                    , new object[] { totalpay, curMember.MemberID });//直接扣除余额


                //db.Entry(curMember).State = System.Data.EntityState.Modified;
                //db.SaveChanges();
                //return Redirect("/send?orderid=" + Common.Tool.DESEncrypt.Encrypt(order.OrderID));
                PayLog paylog = new PayLog()
                {
                    InDate = DateTime.Now,
                    MemberID = CurMemberInfo.MemnerID,
                    Payment = totalpay,
                    PayDirection = (int)Common.Enum.PayDirection.购买,
                    RefOrderID = commonpayModel.CommonPayID
                };
                db.PayLog.Add(paylog);
            }
            else
            {//全额付款
                needpay = totalpay;
                //return AliPay(order);
            }

            commonpayModel.NeedPay = needpay;

            int flag = db.SaveChanges();
            //if (flag == 0)
            //{
            //    ShowAlertMessage("付款遇到问题，请稍后重试！");
            //    return View();
            //}

            if (needpay == 0 && wayofpay == "0")
            {
                return Redirect("/jiangjia/oktili?goodid=" + commonpayModel.GoodID);
            }
            else
            {
                if (UserAgentHelper.IsWeiXin())
                {
                    return Redirect("/jiangjia/weixinalipay?pid=" + Common.Tool.DESEncrypt.Encrypt(commonpayModel.CommonPayID));
                }
                else
                {
                    return AliPay(needpay.Value, "体力购买", Common.Tool.DESEncrypt.Encrypt("tl_" + commonpayModel.CommonPayID), commonpayModel.GoodID);
                }
            }


        }

        public ActionResult OKTiLi(string goodid)
        {


            //没有关注的话引导关注
            ViewBag.guanzhu = false;
            ViewBag.goodid = goodid;
            ViewBag.linkurl = "http://" + SettingBLL.MobileDomain + "/jiangjia/good?id=" + goodid;
            //if (this.CurMemberInfo != null)
            //{
            //    var member = db.Members.Where(c => c.MemberID == this.CurMemberInfo.MemnerID).FirstOrDefault();
            //    if (member.IsGuanZhu == 1)
            //    {
            //        ViewBag.guanzhu = true;
            //    }
            //    ViewBag.myname = this.CurMemberInfo.Name;
            //}
            //ViewBag.js_json = TenPayManager.MakeUpJsParam();
            ViewBag.imgurl = "http://" + SettingBLL.WebDomain + "/api/GetQrCode?qraction=gz&param=" + goodid + "&rnd=" + DateTime.Now.ToString("fffff");

            return View();
        }

        public ActionResult OK(string goodid)
        {
            Goods goodModel = null;
            if (!string.IsNullOrEmpty(goodid))
            {
                goodModel = db.Goods.Find(int.Parse(goodid));
            }
            ViewBag.linkurl = "http://" + SettingBLL.MobileDomain + "/jiangjia/good?id=" + goodid;
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

            ViewBag.imgurl = "http://" + SettingBLL.WebDomain + "/api/GetQrCode?qraction=gz&param=" + goodid + "&rnd=" + DateTime.Now.ToString("fffff");


            return View(goodModel);
        }

        public ActionResult WeixinAlipay(string pid)
        {
            var commonpayModel = db.CommonPay.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(pid)));

            if (UserAgentHelper.IsWeiXin())
            {
                SessionHelper.SetJumpInWeiXin();
                return View();
            }
            else
            {
                return AliPay(commonpayModel.NeedPay.Value, "体力购买", Common.Tool.DESEncrypt.Encrypt("tl_" + commonpayModel.CommonPayID), commonpayModel.GoodID);
            }

        }

        public ActionResult WeixinAlipayOrder(string orderid)
        {
            var orderModel = GetOrder();

            if (UserAgentHelper.IsWeiXin())
            {
                SessionHelper.SetJumpInWeiXin();
                return View("/views/jiangjia/WeixinAlipay.cshtml");
            }
            else
            {
               // return AliPay(orderModel.NeedPay.Value, orderModel.Goods.Name, Common.Tool.DESEncrypt.Encrypt(orderModel.OrderID), orderModel.GoodID);
            }
            return null;

        }

        private ActionResult AliPay(decimal payment, string payfordatil, string reforderid, int? goodid)
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
            string merchant_url = "http://" + SettingBLL.MobileDomain + "/common/backtoweixin?type=jj&goodid=" + goodid;
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

    }
}
