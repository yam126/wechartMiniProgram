using ncc2019.Common;
using ncc2019.Common.BLL;
using ncc2019.Common.Tool;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Xml;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Configuration;
using System.Data;
using ncc2019.Common.Enum;
using ncc2019.Common.Weixin;
using System.Data.Entity;
using ncc2019.Model;

namespace ncc2019.Controllers
{
    public class APISController : ControllerBaseNoCheck
    {

        //[HttpGet]
        //public JsonResult GetOrderList()
        //{
        //    string result = "ok";
        //    //int sellerid = int.Parse(GetOrderId_Decrypt(sid));
        //    ArrayList resultList = new ArrayList();
        //    try
        //    {
        //        int sellerid = 1850;//this.CurMemberInfo.MemnerID;
        //        var orderlist = db.NCCOrders.Where(c => c.SellerID == sellerid && c.PayStatus == (int)PayStatus.已支付 && c.Exchanged == (int)ShiFouStatus.否);

        //        foreach (var order in orderlist)
        //        {
        //            var member = db.Members.Find(order.MemberID);
        //            //var good = db.Goods.Find(order.GoodID);
        //            var ncclottery = db.NCCLottery.Find(order.NCCLotteryID);
        //            resultList.Add(new
        //            {
        //                orderid = GetOrderId_Encrypt(order.OrderID),
        //                buynum = order.BuyNum,
        //                payment = order.Payment,
        //                membername = member.NiceName,
        //                exchanged = order.Exchanged,
        //                lotteryname = ncclottery.Name,
        //                memberurl = member.HeadImgUrl
        //            });
        //        }

        //    }
        //    catch (Exception error)
        //    {
        //        result = "error";

        //    }


        //    return Json(new { result = result, list = resultList }, JsonRequestBehavior.AllowGet);
        //}
        /// <summary>
        /// 保存商家可以出售的彩票品种
        /// </summary>
        /// <param name="token"></param>
        /// <param name="lotteryidlist"></param>
        /// <returns></returns>
        public JsonResult SaveLotterySellerRef(string token, string lotteryidlist)
        {
            try
            {
                var member = GetCurrentMember();
                if (member != null)
                {
                    string[] arrayLotteryIDs = lotteryidlist.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime curTime = DateTime.Now;
                    foreach (var item in arrayLotteryIDs)
                    {
                        int lotteryid = int.Parse(item);
                        var refobj = new LotterySellerRef()
                        {
                            AddDate = curTime,
                            LotteryID = lotteryid,
                            OpeMemberID = member.MemberID,
                            SellerID = member.RefSellerID
                        };
                        db.LotterySellerRef.Add(refobj);
                    }
                    db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction
                         , "delete LotterySellerRef where SellerID ={0}"
                            , member.RefSellerID);
                    db.SaveChanges();
                }


                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);



            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public string GetScan(int sellerid)
        {
            return "http://m.ncc.renxingpao.com/scan?sellerid=" + GetOrderId_Encrypt(sellerid);
        }
        /// <summary>
        /// 当扫码后会跳转到登陆注册页
        /// </summary>
        /// <param name="sellerid"></param>
        /// <returns></returns>
        public ActionResult Scan(string sellerid)
        {
            return Redirect("/ncc/index?info=reg&sellerid=" + sellerid);


        }
        //[HttpGet]
        //public JsonResult GetSellerLotteryList()
        //{
        //    string result = "ok";
        //    //int sellerid = int.Parse(GetOrderId_Decrypt(sid));
        //    ArrayList resultList = new ArrayList();
        //    try
        //    {
        //        int sellerid = 1850;//this.CurMemberInfo.MemnerID;
        //        var orderlist = db.NCCOrders.Where(c => c.SellerID == sellerid && c.PayStatus == (int)PayStatus.已支付
        //        && c.Exchanged == (int)ShiFouStatus.否);

        //        foreach (var order in orderlist)
        //        {
        //            var member = db.Members.Find(order.MemberID);
        //            //var good = db.Goods.Find(order.GoodID);
        //            var ncclottery = db.NCCLottery.Find(order.NCCLotteryID);
        //            resultList.Add(new
        //            {
        //                orderid = GetOrderId_Encrypt(order.OrderID),
        //                buynum = order.BuyNum,
        //                payment = order.Payment,
        //                membername = member.NiceName,
        //                exchanged = order.Exchanged,
        //                lotteryname = ncclottery.Name,
        //                memberurl = member.HeadImgUrl
        //            });
        //        }

        //    }
        //    catch (Exception error)
        //    {
        //        result = "error";

        //    }


        //    return Json(new { result = result, list = resultList }, JsonRequestBehavior.AllowGet);
        //}
        /// <summary>
        /// 获取所有在线的店家数量
        /// </summary>
        /// <param name="token"></param>
        /// <returns>返回第一个店家的名称</returns>
        public JsonResult GetOnlineSellerCount(string token)
        {
            try
            {
                var member = GetCurrentMember();
                string sellername = "";
                if (member != null)
                {
                    var memberss = db.Members.Find(member.RefMemberID);
                    var seller = db.Seller.Find(memberss.RefSellerID);
                    if (seller != null)
                    {
                        sellername = seller.Name;
                    }
                }

                var sellerlist = db.Seller.Where(c => c.IsOnline == (int)ShiFouStatus.是);
                if (sellername == "" && sellerlist.Count() > 0) sellername = sellerlist.First().Name;
                if (sellername.Length > 0) { sellername = sellername.Substring(0, 4) + "***"; }
                int count = sellerlist.Count();

                return Json(new { result = "ok", sellername = sellername, count = count }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }

            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyInfo(string token)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {



                    var obj = new
                    {
                        phone = member.Phone,
                        phonepart = MemberBLL.GetPartPhoneNum(member.Phone),
                        name = member.Name,
                        realname = member.RealName

                    };
                    return Json(new { result = "ok", obj = obj }, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{
                    //    return Json(new { result = "reload", msg = "用户与二维码信息不对应" }, JsonRequestBehavior.AllowGet);
                    //}
                }


            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveMyinfo(string token, string realname, string phone)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {

                    member.RealName = realname;
                    member.Phone = phone;
                    db.SaveChanges();


                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{
                    //    return Json(new { result = "reload", msg = "用户与二维码信息不对应" }, JsonRequestBehavior.AllowGet);
                    //}
                }


            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PayTest(string token, string name, string phone)
        {
            ViewBag.js_json = TenPayManager.MakeUpJsParam();
            string url = "&redirect_url=" + System.Web.HttpUtility.UrlEncode("https://m.ncc.renxingpao.com/ncc/payok");
            url = mp3() + url;
            //live.aliyuncs.com

            ViewBag.url = url;
            return View();
        }

        public JsonResult CreateUploadVideo()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Action", "CreateUploadVideo");
            dic.Add("Title", "exampleTitle");
            dic.Add("FileName", "example.avi");
            dic.Add("FileSize", "10485760");
            dic.Add("Format", "JSON");
            dic.Add("AccessKeyId", "LTAIIMMPDTMYFxV0");
            dic.Add("Version", System.Web.HttpUtility.UrlEncode("2017-03-21"));

            dic.Add("SignatureMethod", System.Web.HttpUtility.UrlEncode("Hmac-SHA1"));
            dic.Add("SignatureNonce", System.Web.HttpUtility.UrlEncode(SignatureHelper.generateRandom()));
            dic.Add("SignatureVersion", "1.0");
            dic.Add("Timestamp", System.Web.HttpUtility.UrlEncode(SignatureHelper.GetTimeStamp()));
            string url = "http://vod.cn-shanghai.aliyuncs.com/?";


            string sign = SignatureHelper.GetSignString(dic);
            //dic.Add("Signature", GetSignString(dic));
            var sb = new StringBuilder();


            foreach (var d in dic)
            {
                sb.Append(d.Key + "=" + d.Value + "&");
            }
            sb.Append("Signature=" + System.Web.HttpUtility.UrlEncode(sign));
            url += sb.ToString();

            HttpHelper http = new HttpHelper();
            //string block = http.Get(url);
            LoggerHelper.Info(url);

            return Json(new { result = "ok", url = url }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendVerifCode(string phone)
        {
            try
            {
                ValidateCode Valcode = new ValidateCode();
                string code = Valcode.CreateValidateCode(4);
                SessionHelper.SetSession("VerifCode", code);//记录到Session
                string result = MessageHelper.SendVerifCodeSMS(phone, code);
                if (result == "ok")
                {
                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SendVerifCodeWithToken(string token)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                ValidateCode Valcode = new ValidateCode();
                string code = Valcode.CreateValidateCode(4);
                SessionHelper.SetSession("VerifCodeIn", code);//记录到Session
                string result = MessageHelper.SendVerifCodeSMS(member.Phone, code);
                if (result == "ok")
                {
                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 用户取现到银行卡
        /// </summary>
        /// <param name="token"></param>
        /// <param name="bankcode"></param>
        /// <param name="bankno"></param>
        /// <param name="amount"></param>
        /// <param name="verifycode"></param>
        /// <returns></returns>
        public JsonResult PayToUserBank(string token, string bankcode, string bankno, string amount, string verifycode)
        {
            try
            {
                if (!(SessionHelper.GetSession("VerifCode") == verifycode && verifycode != ""))
                {
                    return Json(new { result = "error", message = "验证码错误" }, JsonRequestBehavior.AllowGet);
                }
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member.Balance < decimal.Parse(amount.Trim()))
                {
                    return Json(new { result = "error", message = "账户余额不足" }, JsonRequestBehavior.AllowGet);
                }
                CommonPay pay = new CommonPay()
                {
                    AddDate = DateTime.Now,
                    HeadImgUrl = member.HeadImgUrl,
                    MemberID = member.MemberID,
                    Name = member.Name,
                    NeedPay = decimal.Parse(amount.Trim()),
                    PayDirection = (int)PayDirection.取现到银行卡,
                    PayStatus = (int)PayStatus.未支付,
                    Payment = decimal.Parse(amount.Trim()),
                    PayType = (int)PayType.微信,
                    TotalPayment = decimal.Parse(amount.Trim())
                };
                db.CommonPay.Add(pay);
                db.SaveChanges();
                string result = WeiXinCommonHelper.PayToUser(new Common.Model.PayToUser()
                {
                    amount = decimal.Parse(amount.Trim()),
                    bank_code = bankcode,
                    enc_bank_no = bankno,
                    enc_true_name = member.RealName,
                    partner_trade_no = "paytobank_" + pay.CommonPayID
                });
                if (result == "ok")
                {
                    pay.PayStatus = (int)PayStatus.已支付;
                    db.SaveChanges();
                }
                else
                {
                    return Json(new { result = "error", message = result }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception error)
            {

                LoggerHelper.BigError(error.ToString());
            }


            return Json(new { result = "error", message = "内部错误" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 店家登陆小程序后台自动改变状态
        /// </summary>
        /// <param name="token"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public JsonResult SetOnlineState(string token, string state)
        {
            try
            {
                string pushurl = "";
                var member = GetCurrentMember();
                if (member.RefSellerID != null)
                {
                    var seller = db.Seller.Find(member.RefSellerID);
                    seller.IsOnline = state == "1" ? 1 : 0;
                    seller.LastDate = DateTime.Now;
                    //如果为下线操作则将商家当前所有分配的用户进行释放
                    if (state == "0")
                    {
                        var lotterylist = db.NCCLottery.Where(c => c.RefSellerID == seller.SellerID
                            && c.PrizeStatus == (int)PrizeStatus.未开奖
                            && c.LotteryStatus == (int)LotteryStatus.已支付);
                        foreach (var item in lotterylist)
                        {
                            item.RefSellerID = null;
                        }
                        seller.QueueNum = 0;
                    }
                    pushurl = string.Format(seller.VideoPushURL, seller.SellerNo, "common");
                    db.SaveChanges();
                }


                return Json(new { result = "ok", pushurl = pushurl }, JsonRequestBehavior.AllowGet);



            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public JsonResult GetOnlineState(string token)
        {
            try
            {
                int isonline = 0;
                var member = GetCurrentMember();
                if (member.RefSellerID != null)
                {
                    var seller = db.Seller.Find(member.RefSellerID);
                    isonline = seller.IsOnline.Value;
                }


                return Json(new { result = "ok", isonline = isonline }, JsonRequestBehavior.AllowGet);



            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 上传图片到阿里云后需要到服务器端更新图片地址
        /// </summary>
        /// <param name="lotteryid"></param>
        /// <param name="imgurl"></param>
        /// <returns></returns>
        public JsonResult SetLotteryImgUrl(string lotteryid, string imgurl)
        {
            try
            {
                var lottery = GetNCCLottery();
                if (lottery != null)
                {
                    lottery.OrgImgUrl2 = imgurl;
                    lottery.ImgUrl2 = imgurl;

                    db.SaveChanges();
                }


                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);



            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 开始刮奖时 标记开始时间
        /// </summary>
        /// <param name="token"></param>
        /// <param name="lotteryid"></param>
        /// <returns></returns>
        public JsonResult SetOpenLotteryState(string token, string lotteryid)
        {
            try
            {
                var member = GetCurrentMember();
                string pushurl = "";
                if (member != null)
                {
                    var lottery = GetNCCLottery();
                    lottery.BeginOpenDate = DateTime.Now;

                    var seller = db.Seller.Find(lottery.RefSellerID);
                    pushurl = string.Format(seller.VideoPushURL, seller.SellerNo, "common");

                    db.SaveChanges();
                }

                return Json(new { result = "ok", pushurl = pushurl }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 进行实名认证
        /// </summary>
        /// <param name="token"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public JsonResult DoAuthen(string token, string truename, string idcard)
        {
            try
            {
                var member = GetCurrentMember();
                if (member != null)
                {

                    //TODO: API去做实名认证
                    member.IsAuthen = (int)ShiFouStatus.是;
                    member.RealName = truename;
                    member.IDCard = idcard;
                    db.SaveChanges();
                }


                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);



            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 退款给用户
        /// </summary>
        /// <param name="token"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public JsonResult DoPayBackToUser(string token, string lotteryid, string reason)
        {
            try
            {
                var member = GetCurrentMember();
                var lottery = GetNCCLottery();
                if (member != null)
                {
                    string reasonDetail = "";
                    switch (reason)
                    {
                        case "1":
                            reasonDetail = "此类型彩票已售罄"; break;
                        case "2":
                            reasonDetail = "店小二临时有事外出"; break;
                        case "9":
                            reasonDetail = "其他原因"; break;
                        default:
                            break;
                    }


                    NCCCommonRule.PayBack(lottery, DateTime.Now, reasonDetail);
                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error", message = "系统错误" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FastLogin(string phone, string verifycode, string refsellerid, string test)
        {
            //LoggerHelper.Debug("FastLogin");
            try
            {
                if (string.IsNullOrEmpty(test))
                {
                    if (!(SessionHelper.GetSession("VerifCode") == verifycode && verifycode != ""))
                    {
                        return Json(new { result = "error", message = "验证码错误" }, JsonRequestBehavior.AllowGet);
                    }
                }

                Members member = db.Members.Where(c => c.Phone == phone.Trim()).FirstOrDefault();
                if (member == null)
                {
                    Random random = new Random();
                    int num = random.Next(1, 15);
                    //不存在此用户的话需要自动建立一个特殊账户
                    member = new Members()
                    {
                        LoginCount = 0,
                        Status = (int)Common.Enum.MemmberStatus.正常,
                        RegDate = DateTime.Now,
                        LastDate = DateTime.Now,
                        Balance = 0,
                        Balance_back = 0,
                        Name = "用户" + DateTime.Now.ToString("yyyyMMddHHmmssff"),
                        UserLevel = (int)Common.Enum.UserLevel.普通账户,
                        TiLiNum = 1,
                        IsAuthen = (int)ShiFouStatus.否,
                        Token = Guid.NewGuid().ToString().Replace("-", ""),
                        Phone = phone,
                        HeadImgUrl = "http://m.ncc.renxingpao.com/images/ncc/heads/head" + num + ".jpg"
                    };
                    if (!string.IsNullOrEmpty(refsellerid))
                    {
                        int sellerid = int.Parse(GetOrderId_Decrypt(refsellerid));
                        var refseller = db.Seller.Where(c => c.SellerID == sellerid).FirstOrDefault();
                        if (refseller != null)
                        {
                            member.RefMemberID = refseller.ManagerMemberID;
                            member.OwnerSellerID = refseller.SellerID;
                        }
                    }
                    else
                    {
                        int sellerid = SettingBLL.SysSellerID;
                        var refseller = db.Seller.Where(c => c.SellerID == sellerid).FirstOrDefault();
                        member.RefMemberID = refseller.ManagerMemberID;
                        member.OwnerSellerID = refseller.SellerID;
                    }
                    db.Members.Add(member);
                    db.SaveChanges();
                }

                Common.Model.MemberInfo minfo = Common.Model.MemberInfo.BuildMemberInfo(member);

                //记录登陆信息
                LoginLog log = new LoginLog()
                {
                    Agent = Request.UserAgent,
                    IP = Request.UserHostAddress,
                    LoginTime = DateTime.Now,
                    SystemInfo = UserAgentHelper.GetOSNameByUserAgent(Request.UserAgent),
                    MemberID = member.MemberID
                };

                //保存最后登录时间
                member.LastDate = log.LoginTime;
                member.LoginCount++;
                db.Entry(member).Property(c => c.LastDate).IsModified = true;
                db.Entry(member).Property(c => c.LoginCount).IsModified = true;
                db.LoginLog.Add(log);
                db.SaveChanges();

                SessionHelper.CurMemberInfo = minfo;

                var user =
                new
                {
                    name = member.LoginName,
                    realname = member.RealName,
                    balance = member.Balance.Value.ToString("###0.##"),
                    token = member.Token,
                    isauthen = member.IsAuthen,
                    headimgurl = member.HeadImgUrl,
                    phone = MemberBLL.GetPartPhoneNum(member.Phone),
                    ownersellerid = member.OwnerSellerID
                };
                return Json(new { result = "ok", user = user }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "登陆失败!" }, JsonRequestBehavior.AllowGet);
            }



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult GetLotteryTemplateList(string token, int pageIndex, int pageSize)
        {

            try
            {
                var member = GetCurrentMember();
                var lotterysellerref = db.LotterySellerRef.Where(c => c.SellerID == member.RefSellerID);


                int startIndex = pageIndex * pageSize;
                var lotterylisttemp = from c in db.Lottery where c.Enabled == (int)ShiFouStatus.是 select c;
                lotterylisttemp = from c in lotterylisttemp orderby c.TheOrder select c;
                var lotterylist = lotterylisttemp.Skip(startIndex).Take(pageSize);
                ArrayList resultList = new ArrayList();
                foreach (var item in lotterylist)
                {
                    bool checkedStr = false;
                    var lotteryref = from c in lotterysellerref where c.LotteryID == item.LotteryID select c;
                    if (lotteryref.Count() > 0)
                    {
                        checkedStr = true;
                    }
                    resultList.Add(new
                    {
                        lotteryid = item.LotteryID,
                        lotteryname = item.Name,
                        lotterypayment = item.Payment.Value.ToString("###0.##"),
                        imgurl = item.ImgUrl,
                        intro = item.Intro,
                        ischecked = checkedStr
                    });
                }
                return Json(new { result = "ok", list = resultList }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }



        }
        /// <summary>
        /// 获取可以销售的彩票类型
        /// </summary>
        /// <param name="token"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetCanSellLotteryList(string token, string sellerid, int pageIndex, int pageSize)
        {
            try
            {

                var member = GetCurrentMember();
                ArrayList resultList = new ArrayList();
                string sellernameStr = "";
                string selleridStr = "";
                if (member != null)
                {

                    var seller = db.Seller.Where(c => c.SellerID == member.OwnerSellerID
                                   && c.IsOnline == (int)ShiFouStatus.是).FirstOrDefault();
                    if (seller == null)
                    {
                        seller = db.Seller.Where(c => c.IsOnline == (int)ShiFouStatus.是).OrderBy(c => c.TheOrder).FirstOrDefault();
                    }
                    int? selleridInt = 0;
                    if (seller != null)
                    {
                        selleridInt = seller.SellerID;
                        sellernameStr = seller.Name;
                        selleridStr = GetOrderId_Encrypt(selleridInt.Value);
                    }



                    int startIndex = pageIndex * pageSize;
                    var lotterylisttemp = from c in db.Lottery
                                          join r in db.LotterySellerRef
                                          on c.LotteryID equals r.LotteryID
                                          where
                                          r.SellerID == selleridInt &&
                                          c.Enabled == (int)ShiFouStatus.是
                                          select c;
                    lotterylisttemp = from c in lotterylisttemp orderby c.TheOrder select c;
                    var lotterylist = lotterylisttemp.Skip(startIndex).Take(pageSize);

                    foreach (var item in lotterylist)
                    {
                        resultList.Add(new
                        {
                            lotteryid = item.LotteryID,
                            lotteryname = item.Name,
                            lotterypayment = item.Payment.Value.ToString("###0.##"),
                            imgurl = item.ImgUrl,
                            intro = item.Intro
                        });
                    }
                }
                //加载默认分组
                if (resultList.Count == 0)
                {
                    int? selleridInt = 0;
                    int startIndex = pageIndex * pageSize;
                    var lotterylisttemp = from c in db.Lottery
                                          join r in db.LotterySellerRef
                                          on c.LotteryID equals r.LotteryID
                                          where
                                          r.SellerID == selleridInt &&
                                          c.Enabled == (int)ShiFouStatus.是
                                          select c;
                    lotterylisttemp = from c in lotterylisttemp orderby c.TheOrder select c;
                    var lotterylist = lotterylisttemp.Skip(startIndex).Take(pageSize);

                    foreach (var item in lotterylist)
                    {
                        resultList.Add(new
                        {
                            lotteryid = item.LotteryID,
                            lotteryname = item.Name,
                            lotterypayment = item.Payment.Value.ToString("###0.##"),
                            imgurl = item.ImgUrl,
                            intro = item.Intro
                        });
                    }

                }

                return Json(new { result = "ok", list = resultList, sellername = sellernameStr, sellerid = selleridStr }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }



        }
        /// <summary>
        /// 根据token 获得所买彩票列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult GetLotteryList(string token, int pageIndex, int pageSize, int type)
        {
            //LoggerHelper.Debug("GetLotteryList");
            try
            {
                int startIndex = pageIndex * pageSize;
                // var chatlist = db.ZGGBBS.OrderByDescending(c => c.AddDate).Skip(startIndex).Take(pageSize);
                var curMember = GetCurrentMember();
                if (curMember != null)
                {

                    int memberid = curMember.MemberID;//CurMemberInfo.MemnerID;
                    var lotterylisttemp = from c in db.NCCLottery where c.RefMemberID == memberid select c;
                    if (type == 1)
                    {
                        lotterylisttemp = from c in lotterylisttemp where c.PrizeStatus == (int)PrizeStatus.已中奖 select c;
                    }
                    if (type == 9)
                    {
                        lotterylisttemp = from c in lotterylisttemp
                                          where c.PrizeStatus == (int)PrizeStatus.未中奖
                || c.PrizeStatus == (int)PrizeStatus.已中奖
                                          select c;
                    }
                    lotterylisttemp = from c in lotterylisttemp orderby c.AddDate descending select c;
                    var lotterylist = lotterylisttemp.Skip(startIndex).Take(pageSize);
                    ArrayList resultList = new ArrayList();
                    foreach (var item in lotterylist)
                    {
                        var seller = db.Seller.Find(item.RefSellerID);
                        string sellername = "";
                        if (seller != null)
                        {
                            sellername = seller.Name;
                        }
                        resultList.Add(new
                        {
                            lotteryid = item.NCCLotteryID,
                            lotteryno = item.LotteryNo,
                            adddate = item.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            date = item.AddDate.Value.ToString("MM/dd"),
                            time = item.AddDate.Value.ToString("HH:mm"),
                            prizestate = item.LotteryStatus == (int)LotteryStatus.已经退款 ? "已退款" : EnumTool.GetEnumName(typeof(PrizeStatus), item.PrizeStatus),
                            lotteryname = item.Name,
                            lotterypayment = item.Payment.Value.ToString("###0.##"),
                            bonus = item.Bonus.Value.ToString("###0.##"),
                            imgurl = item.ImgUrl,
                            prizeimg = item.ImgUrl2,
                            sellername = sellername
                        });

                    }



                    return Json(new { result = "ok", list = resultList }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "ok", list = new ArrayList() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }



        }
        /// <summary>
        /// 在彩票展示面板获取商家开奖过的彩票信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetLotteryListBySeller(string token, int pageIndex, int pageSize)
        {

            try
            {
                int startIndex = pageIndex * pageSize;
                var member = GetCurrentMember();
                //var seller = db.Seller.Find(member.RefSellerID);
                //int selleridInt = int.Parse(sellerid);
                var lotterylisttemp = from c in db.NCCLottery
                                      where c.RefSellerID == member.RefSellerID
               && (c.LotteryStatus == (int)LotteryStatus.已支付
               || c.LotteryStatus == (int)LotteryStatus.已经退款)
                                      select c;
                var count = lotterylisttemp.Count();
                lotterylisttemp = from c in lotterylisttemp orderby c.AddDate descending select c;
                var lotterylist = lotterylisttemp.Skip(startIndex).Take(pageSize);


                ArrayList resultList = new ArrayList();
                foreach (var item in lotterylist)
                {
                    var buyer = db.Members.Find(item.RefMemberID);
                    var ownerSeller = db.Seller.Find(buyer.OwnerSellerID);
                    var tempObj = new
                    {
                        lotteryid = item.NCCLotteryID,
                        lotteryno = item.LotteryNo,
                        name = item.Name,
                        ownersellername = ownerSeller.Name,
                        imgurl = item.ImgUrl,
                        adddate = item.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        date = item.AddDate.Value.ToString("MM/dd"),
                        time = item.AddDate.Value.ToString("HH:mm"),
                        prizestate = item.LotteryStatus == (int)LotteryStatus.已经退款 ? "已退款" : EnumTool.GetEnumName(typeof(PrizeStatus), item.PrizeStatus),
                        prizeimgurl = item.OrgImgUrl2,
                        lotteryname = item.Name,
                        lotterypayment = item.Payment.Value.ToString("###0.##"),
                        bonus = item.Bonus == null ? null : item.Bonus.Value.ToString("###0.##"),
                        hiddenbonus = item.PrizeStatus == (int)PrizeStatus.已中奖 ? false : true

                    };

                    resultList.Add(tempObj);
                }
                //int topOrderid = lotterylist.First().RefOrderID.Value;
                //var topOrder = db.Orders.Find(topOrderid);
                //int unUseCount = db.NCCLottery.Where(c => c.RefOrderID == topOrderid && c.PrizeStatus == (int)PrizeStatus.未开奖).Count();

                return Json(new { result = "ok", list = resultList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }



        }
        /// <summary>
        /// 获取导游信息
        /// </summary>
        /// <param name="guideid"></param>
        /// <returns></returns>
        public JsonResult GetGuideInfo(int guideid)
        {
            try
            {
                var guideObj = (from c in db.Guide
                                where c.GuideID == guideid
                                select c).FirstOrDefault();
              


                return Json(new { result = "ok", obj = guideObj }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取失败!" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetGuideInfoByPhoneNumber(string PhoneNumber)
        {
            try
            {
                var guideObj = (from c in db.Guide
                                where c.PhoneNumber == PhoneNumber
                                select c).FirstOrDefault();
                if(guideObj==null)
                    return Json(new { result = "error", message = "没有读取到导游信息" }, JsonRequestBehavior.AllowGet);
                int orderNumber = 0;
                int toDayOrderNum = 0;
                decimal totalRevenue = 0;
                decimal withDrawable = 0;
                if (db.Orders.Any(q => q.GuideOpenid == guideObj.GuideNo))
                {
                    orderNumber = db.Orders.Where(q => q.GuideOpenid == guideObj.GuideNo).ToList().Count;
                    if (db.Orders.Any(q => q.GuideOpenid == guideObj.GuideNo && q.AddDate.GetValueOrDefault().ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")))
                        toDayOrderNum = db.Orders.Where(q => q.GuideOpenid == guideObj.GuideNo && q.AddDate.GetValueOrDefault().ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")).ToList().Count;
                    if (db.Orders.Any(q => q.GuideOpenid == guideObj.GuideNo))
                    {
                        totalRevenue = db.Orders.Where(q => q.GuideOpenid == guideObj.GuideNo).Sum(q => q.Payment).GetValueOrDefault();
                        withDrawable = totalRevenue;
                    }
                }
                return Json(new { 
                    result = "ok", 
                    obj = new { 
                        guidInfo=guideObj,
                        orderNumber= orderNumber,
                        toDayOrderNum=toDayOrderNum,
                        withDrawable=withDrawable,
                        totalRevenue=totalRevenue
                    } 
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取失败!" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GuideIsRegist(string PhoneNumber) 
        {
            try
            {
                var guideObj = (from c in db.Guide
                                where c.PhoneNumber == PhoneNumber
                                select c).FirstOrDefault();

                var result = false;
                if (guideObj != null)
                    result = true;

                return Json(new { result = "ok", obj = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取失败!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ChangeGuidBusyState(string PhoneNumber,int BusyState)
        {
            try 
            {
                var guideObj = (from c in db.Guide
                                where c.PhoneNumber == PhoneNumber
                                select c).FirstOrDefault();
                if(guideObj!=null)
                {
                    guideObj.BusyStatus = BusyState;
                    db.SaveChanges();
                }
                return Json(new
                {
                    result = "ok",
                    obj = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception error) 
            {
                return Json(new { result = "error", message = $"失败!{error.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UploadFile(HttpPostedFileBase file) 
        {
            if (file == null) 
            {
                return Json(new { result = "error", message = $"没有文件不能上传" }, JsonRequestBehavior.AllowGet);
            }
            string uploadFilePath = "~/upFiles";
            var filePath = Request.MapPath(uploadFilePath);
            var fileName = Path.Combine(Request.MapPath(uploadFilePath), Path.GetFileName(file.FileName));
            try {
                if (!System.IO.Directory.Exists(filePath))
                    System.IO.Directory.CreateDirectory(filePath);
                file.SaveAs(fileName);
                return Json(new {
                    result = "ok",
                    message = $"成功",
                    obj = "upFiles/" + Path.GetFileName(file.FileName)
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception error) 
            {
                return Json(new { result = "error", message = $"失败!{error.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveGuide(SaveGuideParm parm) 
        {
            if(parm==null)
                return Json(new { result = "error", message = "参数为空" }, JsonRequestBehavior.AllowGet);
            try {
                var guideObj = (from c in db.Guide
                                where c.GuideID == parm.GuidId
                                select c).FirstOrDefault();
                if(guideObj==null)
                    return Json(new { result = "error", message = "没有找到要修改的导游信息" }, JsonRequestBehavior.AllowGet);
                if(db.Guide.Any(q=>q.PhoneNumber==parm.PhoneNumber&&q.GuideID!=parm.GuidId))
                    return Json(new { result = "error", message = "手机号已被另一个账号注册" }, JsonRequestBehavior.AllowGet);
                guideObj.GuideID = parm.GuidId;
                guideObj.HeadUrl = parm.userFaceUrl;
                guideObj.ImageUrl = parm.userFaceUrl;
                guideObj.VideoShowUrl = parm.userVideoUrl;
                guideObj.Skills = parm.Skills;
                guideObj.SkillPoints = parm.SkillPointsStr;
                guideObj.Intorduction = parm.Intorduction;
                guideObj.Name = parm.nickName;
                guideObj.PhoneNumber = parm.PhoneNumber;
                guideObj.IDCard = parm.IDCard;
                db.SaveChanges();
                return Json(new { result = "ok", message = $"成功",obj=guideObj }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception error) 
            {
                return Json(new { result = "error", message = $"失败!{error.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UserQuickRegist(string PhoneNumber) 
        {
            if(string.IsNullOrEmpty(PhoneNumber))
                return Json(new { result = "error", message = $"手机号不能为空" }, JsonRequestBehavior.AllowGet);
            try {
                var account = db.Account.Where(q => q.Phone == PhoneNumber).FirstOrDefault();
                if(account==null)
                {
                    account = new Account();
                    account.Phone = PhoneNumber;
                    account.RegistDate = DateTime.Now;
                    db.Account.Add(account);
                    db.SaveChanges();
                }
                return Json(new { result = "ok", message = $"成功",obj=account }, JsonRequestBehavior.AllowGet);

            }
            catch(Exception error) 
            {
                return Json(new { result = "error", message = $"失败!{error.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddOrder(AddOrderParm parm) 
        {
            if (parm == null) 
            {
                return Json(new { result = "error", message = $"参数不能为空" }, JsonRequestBehavior.AllowGet);
            }
            try {
                var guidInfo = db.Guide.Where(q => q.GuideID == parm.GuideId).FirstOrDefault();
                if(guidInfo==null)
                    return Json(new { result = "error", message = $"找不到导游的数据" }, JsonRequestBehavior.AllowGet);
                var user = db.Account.Where(q => q.AccountID == parm.MemberID).FirstOrDefault();
                if(user==null)
                    return Json(new { result = "error", message = $"找不到用户的数据" }, JsonRequestBehavior.AllowGet);
                Orders order = new Orders();
                order.GuideID = guidInfo.GuideID;
                order.MemberID = user.AccountID;
                order.Memo = parm.Memo;
                order.Payment = parm.Payment;
                order.TotalPayment = parm.TotalPayment;
                order.ServiceMinute = parm.ServiceMinute;
                order.AddDate = DateTime.Now;
                db.Orders.Add(order);
                db.SaveChanges();
                return Json(new { result = "ok", message = $"成功" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception error) 
            {
                return Json(new { result = "error", message = $"失败!{error.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取指定用户的所有订单
        /// </summary>
        /// <param name="vuserid">用户编号</param>
        /// <returns>指定用户的所有订单</returns>
        public JsonResult GetUserOrders(int? vuserid) 
        {
            if (vuserid == null || vuserid == 0) 
                return Json(new { result = "error", message = $"用户编号不能为空" }, JsonRequestBehavior.AllowGet);
            try 
            {
                int userid = vuserid.GetValueOrDefault();
                var orders = new List<vw_Orders>();
                var userinfo = db.Account.Where(q => q.AccountID == userid).FirstOrDefault();
                if(userinfo==null)
                    return Json(new { result = "error", message = $"用户信息不能为空" }, JsonRequestBehavior.AllowGet);
                if(!db.vw_Orders.Any(q=>q.MemberID == userid))
                    return Json(new { result = "ok", message = $"",obj= orders }, JsonRequestBehavior.AllowGet);
                var orderList = db.vw_Orders.Where(q => q.MemberID == userid).ToList();
                return Json(new { result = "ok", message = $"", obj = orderList }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception error) 
            {
                return Json(new { result = "error", message = $"失败!{error.Message}" }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 获取指定导游的所有订单
        /// </summary>
        /// <param name="vguideId">导游编号</param>
        /// <returns>指定导游的所有订单</returns>
        public JsonResult GetGuideOrders(int? vguideId)
        {
            if (vguideId == null || vguideId == 0)
                return Json(new { result = "error", message = $"导游编号不能为空" }, JsonRequestBehavior.AllowGet);
            try
            {
                int guideId = vguideId.GetValueOrDefault();
                var orders = new List<vw_Orders>();
                var guideinfo = db.Guide.Where(q => q.GuideID == guideId).FirstOrDefault();
                if (guideinfo == null)
                    return Json(new { result = "error", message = $"导游信息不能为空" }, JsonRequestBehavior.AllowGet);
                if (!db.vw_Orders.Any(q => q.GuideID == guideId))
                    return Json(new { result = "ok", message = $"", obj = orders }, JsonRequestBehavior.AllowGet);
                var orderList = db.vw_Orders.Where(q => q.GuideID == guideId).ToList();
                return Json(new { result = "ok", message = $"", obj = orderList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { result = "error", message = $"失败!{error.Message}" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GuidQuickRegist(GuidQuickRegistParm parm) 
        {
            try 
            {
                Guide guide = new Guide();
                if (parm == null) 
                    return Json(new { result = "error", message = "参数为空" }, JsonRequestBehavior.AllowGet);
                guide.GuideNo = Guid.NewGuid().ToString().Split('-')[0];
                guide.PhoneNumber = parm.PhoneNumber;
                guide.Name = parm.NickName;
                guide.HeadUrl = parm.UserFaceUrl;
                guide.ImageUrl = parm.UserFaceUrl;
                guide.BusyStatus = 2;
                guide.StartLevel = 1;
                db.Guide.Add(guide);
                db.SaveChanges();
                return Json(new { result = "ok", obj = guide }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception error) 
            {
                return Json(new { result = "error", message = "获取失败!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllGuideList()
        {
            try
            {
                var guidelist = from c in db.Guide
                                select c;


                return Json(new { result = "ok", list = guidelist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 首页获得导游列表，根据用户的当前位置
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetGuideList(int pageIndex, int pageSize)
        {
            try
            {
                var guidelist = from c in db.Guide
                                select c;


                return Json(new { result = "ok", list = guidelist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// 获取用户刮过的彩票--店家统计用
        /// </summary>
        /// <param name="memberid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetMemberLotteryListBySeller(string memberid, int pageIndex, int pageSize)
        {

            try
            {
                int memberidInt = int.Parse(GetOrderId_Decrypt(memberid));
                int startIndex = pageIndex * pageSize;
                var member = db.Members.Find(memberidInt);
                //var seller = db.Seller.Find(member.RefSellerID);
                //int selleridInt = int.Parse(sellerid);
                var lotterylisttemp = from c in db.NCCLottery
                                      where c.RefMemberID == member.MemberID
                                      && (c.LotteryStatus == (int)LotteryStatus.已支付
                                      || c.LotteryStatus == (int)LotteryStatus.已经退款)
                                      select c;
                lotterylisttemp = from c in lotterylisttemp orderby c.AddDate descending select c;
                var lotterylist = lotterylisttemp.Skip(startIndex).Take(pageSize);

                ArrayList resultList = new ArrayList();
                foreach (var item in lotterylist)
                {
                    var seller = db.Seller.Find(item.RefSellerID);
                    var tempObj = new
                    {
                        lotteryid = item.NCCLotteryID,
                        lotteryno = item.LotteryNo,
                        name = item.Name,
                        imgurl = item.ImgUrl,
                        adddate = item.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        date = item.AddDate.Value.ToString("MM/dd"),
                        time = item.AddDate.Value.ToString("HH:mm"),
                        prizestate = item.LotteryStatus == (int)LotteryStatus.已经退款 ? "已退款" : EnumTool.GetEnumName(typeof(PrizeStatus), item.PrizeStatus),
                        prizeimgurl = item.OrgImgUrl2,
                        lotteryname = item.Name,
                        lotterypayment = item.Payment.Value.ToString("###0.##"),
                        bonus = item.Bonus.Value.ToString("###0.##"),
                        hiddenbonus = item.PrizeStatus == (int)PrizeStatus.已中奖 ? false : true,
                        sellername = seller.Name
                    };

                    resultList.Add(tempObj);
                }
                //int topOrderid = lotterylist.First().RefOrderID.Value;
                //var topOrder = db.Orders.Find(topOrderid);
                //int unUseCount = db.NCCLottery.Where(c => c.RefOrderID == topOrderid && c.PrizeStatus == (int)PrizeStatus.未开奖).Count();

                return Json(new { result = "ok", list = resultList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }



        }
        /// <summary>
        /// 获取商家消费的账户明细
        /// </summary>
        /// <param name="token"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetAccountDetailListBySeller(string token, int pageIndex, int pageSize)
        {

            try
            {

                int startIndex = pageIndex * pageSize;
                var member = GetCurrentMember();
                //var seller = db.Seller.Find(member.RefSellerID);
                //int selleridInt = int.Parse(sellerid);
                var paylisttemp = from c in db.CommonPay
                                  where c.MemberID == member.MemberID
               && c.PayStatus == (int)PayStatus.已支付
                                  select c;
                paylisttemp = from c in paylisttemp orderby c.AddDate descending select c;
                var paylist = paylisttemp.Skip(startIndex).Take(pageSize);

                ArrayList resultList = new ArrayList();
                foreach (var item in paylist)
                {
                    //            购买 = 1,
                    //充值 = 2,
                    //副账户充值 = 3,
                    //副账户购买 = 4,
                    //差额退款 = 5,
                    //彩票销售结算 = 6,
                    //销售网点推荐提成 = 7,
                    //中奖后打款 = 8,
                    //中奖后收款 = 9,
                    //取现到银行卡 = 99
                    string paydirction = "";
                    string fontcolor = "red";
                    string zhengfu = "";
                    switch (item.PayDirection)
                    {
                        case 1: paydirction = "购彩"; fontcolor = "green"; zhengfu = "-"; break;
                        case 2: paydirction = "充值"; fontcolor = "red"; zhengfu = "+"; break;
                        case 6: paydirction = "结算"; fontcolor = ""; zhengfu = ""; break;
                        case 7: paydirction = "推荐提成"; fontcolor = "red"; zhengfu = "+"; break;
                        case 8: paydirction = "兑奖"; fontcolor = "green"; zhengfu = "-"; break;
                        case 9: paydirction = "中奖"; fontcolor = "red"; zhengfu = "+"; break;
                        case 10: paydirction = "售彩"; fontcolor = "red"; zhengfu = "+"; break;
                        case 99: paydirction = "取现"; fontcolor = "green"; zhengfu = "-"; break;
                        default:
                            break;
                    }

                    var tempObj = new
                    {
                        paydirction = paydirction,
                        adddate = item.AddDate.Value.ToString("MM/dd HH:mm"),
                        date = item.AddDate.Value.ToString("MM/dd"),
                        time = item.AddDate.Value.ToString("HH:mm"),
                        payment = zhengfu + item.NeedPay.Value.ToString("###0.##"),
                        fontcolor = fontcolor
                    };

                    resultList.Add(tempObj);
                }
                //int topOrderid = lotterylist.First().RefOrderID.Value;
                //var topOrder = db.Orders.Find(topOrderid);
                //int unUseCount = db.NCCLottery.Where(c => c.RefOrderID == topOrderid && c.PrizeStatus == (int)PrizeStatus.未开奖).Count();

                return Json(new { result = "ok", list = resultList, balance = member.Balance.Value.ToString("###0.##") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }



        }
        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public JsonResult SendKefuMessage(string token, string name, string phone, string desc)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();


                var kefumessage = new KeFuMessage()
                {
                    AddDate = DateTime.Now,
                    Desc = desc,
                    Name = name,
                    Phone = phone
                };
                if (member != null)
                {
                    kefumessage.FromMemberID = member.MemberID;
                }
                db.KeFuMessage.Add(kefumessage);
                db.SaveChanges();

                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 店家获得会员账户列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetMemberListBySeller(string token, int pageIndex, int pageSize)
        {

            try
            {
                int startIndex = pageIndex * pageSize;
                var member = GetCurrentMember();
                var seller = db.Seller.Find(member.RefSellerID);
                //int selleridInt = int.Parse(sellerid);
                var memberlisttemp = from c in db.Members where c.RefMemberID == seller.ManagerMemberID select c;
                memberlisttemp = from c in memberlisttemp orderby c.RegDate descending select c;
                var memberlist = memberlisttemp.Skip(startIndex).Take(pageSize);

                ArrayList resultList = new ArrayList();
                foreach (var item in memberlist)
                {
                    var tempObj = new
                    {
                        memberid = GetOrderId_Encrypt(item.MemberID),
                        name = item.Name,
                        headimgurl = item.HeadImgUrl,
                        phone = MemberBLL.GetPartPhoneNum(item.Phone),
                        regdate = item.RegDate.Value.ToString("yyyy-MM-dd HH:mm:ss")

                    };

                    resultList.Add(tempObj);
                }
                //int topOrderid = lotterylist.First().RefOrderID.Value;
                //var topOrder = db.Orders.Find(topOrderid);
                //int unUseCount = db.NCCLottery.Where(c => c.RefOrderID == topOrderid && c.PrizeStatus == (int)PrizeStatus.未开奖).Count();

                return Json(new { result = "ok", list = resultList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }



        }
        /// <summary>
        /// 操作台-获取待开奖用户
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonResult GetLotteryUserListBySeller(string token)
        {
            //检查是否有未刮的用户
            NCCCommonRule.CheckSellerIsOnlineAsyn();

            try
            {
                var member = GetCurrentMember();
                var seller = db.Seller.Find(member.RefSellerID);
                //int selleridInt = int.Parse(sellerid);
                var lotterylist = db.NCCLottery.Where(c => c.RefMemberID != null
                && c.RefSellerID == seller.SellerID
                 && c.LotteryStatus == (int)LotteryStatus.已支付
                && c.PrizeStatus == (int)PrizeStatus.未开奖);

                int totalcount = lotterylist.Count();
                lotterylist = lotterylist.OrderBy(c => c.RefOrderID).OrderBy(c => c.NCCLotteryID).Take(5);

                ArrayList resultList = new ArrayList();
                foreach (var item in lotterylist)
                {
                    var buyer = db.Members.Find(item.RefMemberID);
                    resultList.Add(new
                    {
                        lotteryid = GetOrderId_Encrypt(item.NCCLotteryID),
                        sno = item.NCCLotteryID,
                        headurl = buyer.HeadImgUrl,
                        name = buyer.Name,
                        lotteryname = item.Name,
                        lotterypayment = item.Payment,
                        prizestate = EnumTool.GetEnumName(typeof(PrizeStatus), item.PrizeStatus)
                    });
                }
                //seller.LastDate = DateTime.Now;
                //db.SaveChanges();
                db.Database.ExecuteSqlCommand(string.Format("update seller set lastdate='{1}' where sellerid={0}",
                       seller.SellerID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                //int topOrderid = lotterylist.First().RefOrderID.Value;
                //var topOrder = db.Orders.Find(topOrderid);
                //int unUseCount = db.NCCLottery.Where(c => c.RefOrderID == topOrderid && c.PrizeStatus == (int)PrizeStatus.未开奖).Count();

                return Json(new { result = "ok", totalcount = totalcount, balance = member.Balance, list = resultList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }



        }
        /// <summary>
        /// web端退出按钮
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonResult Logout(string token)
        {
            //变更token
            try
            {
                var member = GetCurrentMember();
                member.Token = Guid.NewGuid().ToString().Replace("-", "");
                db.SaveChanges();
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.BigError(error.ToString());
            }
            return Json(new { result = "error", message = "退出失败!" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Web加载系统分配的卖家
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonResult GetCurSeller(string token)
        {
            try
            {
                var member = GetCurrentMember();

                var seller = db.Seller.Where(c => c.SellerID == member.OwnerSellerID
                               && c.IsOnline == (int)ShiFouStatus.是 && c.Status == (int)SellerStatus.正常).FirstOrDefault();
                if (seller == null)
                {
                    seller = db.Seller.Where(c => c.IsOnline == (int)ShiFouStatus.是 && c.Status == (int)SellerStatus.正常).OrderBy(c => c.TheOrder).FirstOrDefault();
                }
                return Json(new { result = "ok", sellerid = GetOrderId_Encrypt(seller.SellerID), sellername = seller.Name }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }



        }
        /// <summary>
        /// 获取视频信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string GetVideoInfo()
        {
            return "none";

        }
        public JsonResult SetBeginPrizeTime(string token, string lotteryid)
        {
            try
            {
                //var member = GetCurrentMember();
                //var lottery = base.GetNCCLottery();
                //if (member != null && lottery != null)
                //{
                //    lottery.BeginOpenDate = DateTime.Now;
                //}
                Aliyun.Acs.live.Model.V20161101.AddCasterComponentRequest request = new Aliyun.Acs.live.Model.V20161101.AddCasterComponentRequest()
                {
                    AccessKeyId = "LTAIUyoNMleivPYZ",
                    SecurityToken = "zAqFV8qfN1VdPTJCfLLFrFpyjbLxtq",
                    Url = "https://live.aliyuncs.com",
                    Action = "AddCasterComponent",
                    CasterId = "LIVEPRODUCER_POST-cn-4590omy9k02",
                    ComponentName = "text",
                    LocationId = "RC01",
                    ComponentType = "text",
                    //ComponentLayer= "{HeightNormalized:0,WidthNormalized:0,PositionNormalized:[0,0],PositionRefer:'topLeft'}",
                    // TextLayerContent= "{Text:'你是我的',Color:'0xff0000',FontName:'楷体',SizeNormalized:1,BorderWidthNormalized:0,BorderColor:''}"
                };

                HttpHelper http = new HttpHelper();
                string block = http.Get(string.Format("https://live.aliyuncs.com?Action={0}&CasterId={1}&ComponentName={2}&LocationId={3}&ComponentType={4}"

                      , "AddCasterComponent", "LIVEPRODUCER_POST-cn-4590omy9k02", "text", "RC01", "text"
                      ));



                return Json(new { result = "ok", block = block }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }



        }
        /// <summary>
        /// web 获取待开奖用户列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="sellerid">当前分配的开奖店铺，若为空则需要系统进行重新分配</param>
        /// <returns></returns>
        public JsonResult GetLotteryUserList(string token, string sellerid)
        {
            //检查是否有未刮的用户
            NCCCommonRule.CheckSellerIsOnlineAsyn();


            try
            {

                var lotterylist = db.NCCLottery.Where(c => c.RefMemberID != null
                            && c.PrizeStatus == (int)PrizeStatus.未开奖
                            && c.LotteryStatus == (int)LotteryStatus.已支付);

                int totalcount = lotterylist.Count();
                lotterylist = lotterylist.OrderBy(c => c.NCCLotteryID).Take(8);

                ArrayList resultList = new ArrayList();
                foreach (var item in lotterylist)
                {
                    var member = db.Members.Find(item.RefMemberID);

                    resultList.Add(new
                    {
                        headurl = member.HeadImgUrl,
                        name = member.Name
                    });

                }
                string videoShowURL = "";
                string mysellername = "";
                string selleridStr = "";
                string sellernameStr = "";
                string prizestatus = "";
                string prizeenddate = "";
                string prizebegindate = "";
                var curMember = GetCurrentMember();
                string tokenStr = token;
                decimal balance = 0;
                if (curMember != null)
                {
                    int memberid = curMember.MemberID;//
                    var mylottery = db.NCCLottery.Where(c => c.RefMemberID == memberid
                                    && c.PrizeStatus == (int)PrizeStatus.未开奖
                                     && c.LotteryStatus == (int)LotteryStatus.已支付
                                    && c.RefSellerID != null).OrderByDescending(c => c.BeginOpenDate).FirstOrDefault();

                    if (mylottery != null)
                    {
                        var seller = db.Seller.Find(mylottery.RefSellerID);

                        if (mylottery.BeginOpenDate != null)
                        {
                            prizebegindate = DateHelper.GetTimeStamp(mylottery.BeginOpenDate.Value, 13);
                        }
                        mysellername = seller.Name;
                        videoShowURL = string.Format(seller.VideoShowURL, seller.SellerNo, "common");
                    }
                    balance = curMember.Balance.Value;


                    if (string.IsNullOrEmpty(sellerid))
                    {
                        var seller = db.Seller.Where(c => c.SellerID == curMember.OwnerSellerID
                                     && c.IsOnline == (int)ShiFouStatus.是 && c.Status == (int)SellerStatus.正常).FirstOrDefault();
                        if (seller == null)
                        {
                            seller = db.Seller.Where(c => c.IsOnline == (int)ShiFouStatus.是
                               && c.Status == (int)SellerStatus.正常).OrderBy(c => c.TheOrder).OrderBy(c => c.QueueNum).FirstOrDefault();
                        }
                        if (seller != null)
                        {
                            selleridStr = GetOrderId_Encrypt(seller.SellerID);
                            sellernameStr = seller.Name;
                            if (videoShowURL == "")
                                videoShowURL = string.Format(seller.VideoShowURL, seller.SellerNo, "common");
                        }
                    }
                    else
                    {
                        if (videoShowURL == "")
                        {
                            int selleridInt = int.Parse(GetOrderId_Decrypt(sellerid));
                            var seller = db.Seller.Find(selleridInt);
                            sellernameStr = seller.Name;
                            videoShowURL = string.Format(seller.VideoShowURL, seller.SellerNo, "common");
                        }


                    }




                    //获取中奖提醒信息
                    var prizefirst = db.NCCLottery.Where(c => c.RefMemberID == memberid
                                   && c.PrizeStatus != (int)PrizeStatus.未开奖
                                   && c.PrizeDate != null
                                    && c.LotteryStatus == (int)LotteryStatus.已支付
                                    && c.IsNotifyBuyerPrizeInfo != (int)ShiFouStatus.是
                                   && c.RefSellerID != null).FirstOrDefault();
                    //var prizelist = db.NCCLottery.Where(c => c.LotteryStatus c.IsNotifyBuyerPrizeInfo == (int)ShiFouStatus.否);
                    if (prizefirst != null)
                    {
                        if (prizefirst.PrizeStatus == (int)PrizeStatus.已中奖)
                            prizestatus = "恭喜中奖:" + prizefirst.Bonus.Value.ToString("###0.##") + "元";
                        else if (prizefirst.PrizeStatus == (int)PrizeStatus.未中奖)
                            prizestatus = "助力公益，乐善人生";
                        prizeenddate = DateHelper.GetTimeStamp(prizefirst.PrizeDate.Value, 13);
                        prizefirst.NotifyBuyerPrizeInfoDate = DateTime.Now;
                        prizefirst.IsNotifyBuyerPrizeInfo = (int)ShiFouStatus.是;
                        LoggerHelper.Debug(prizestatus);
                        db.SaveChanges();
                    }

                }
                else
                {
                    tokenStr = "";//找不到此人或token过期
                }

                if (videoShowURL == "")
                {

                    var seller = db.Seller.Find(SettingBLL.SysSellerID);
                    videoShowURL = string.Format(seller.VideoShowURL, seller.SellerNo, "common");
                }


                return Json(new
                {
                    result = "ok",
                    token = tokenStr,
                    totalcount = totalcount,
                    balance = balance,
                    list = resultList,
                    videoshowurl = videoShowURL,
                    mysellername = mysellername,
                    sellerid = selleridStr,
                    sellername = sellernameStr,
                    prizestatus = prizestatus,
                    prizeenddate = prizeenddate,
                    prizebegindate = prizebegindate
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "获取列表失败!" }, JsonRequestBehavior.AllowGet);
            }



        }
        public JsonResult MakeOrder(int lotteryid, string sellerid, int lotterynum)
        {

            try
            {
                if (string.IsNullOrEmpty(sellerid))
                {
                    return Json(new { result = "error", message = "订单创建失败!" }, JsonRequestBehavior.AllowGet);
                }
                int selleridInt = int.Parse(GetOrderId_Decrypt(sellerid));
                var seller = db.Seller.Find(selleridInt);

                var lottery = db.Lottery.Find(lotteryid);
                var member = GetCurrentMember();
                decimal? totalpayment = lotterynum * lottery.Payment;
                decimal? needpay = totalpayment;
                if (member.Balance > 0)
                {
                    if (member.Balance < totalpayment)
                    {
                        needpay = totalpayment - member.Balance;
                    }
                    else if (member.Balance >= totalpayment)
                    {
                        needpay = 0;
                    }
                }
                NCCOrders order = new NCCOrders()
                {
                    AddDate = DateTime.Now,
                    BuyNum = lotterynum,
                    EndDate = DateTime.Now.AddMinutes(30),
                    Exchanged = (int)ShiFouStatus.否,
                    LotteryID = lottery.LotteryID,
                    MemberHeadUrl = member.HeadImgUrl,
                    NeedPay = needpay,
                    PayStatus = (int)PayStatus.未支付,
                    Phone = member.Phone,
                    Payment = lottery.Payment,
                    TotalPayment = totalpayment,
                    OrderType = (int)OrderType.普通订单,
                    OrderStatus = (int)OrderStatus.正常,
                    MemberID = member.MemberID,
                    SellerID = seller.SellerID,
                    OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfffff")
                };
                db.NCCOrders.Add(order);
                db.SaveChanges();
                for (int i = 0; i < order.BuyNum; i++)
                {
                    NCCLottery ncclottery = new NCCLottery()
                    {
                        Name = lottery.Name,
                        Intro = lottery.Intro,
                        ImgUrl = lottery.ImgUrl,
                        Payment = lottery.Payment,
                        RefOrderID = order.OrderID,
                        TheOrder = lottery.TheOrder,
                        //RefMemberID = order.MemberID, 当支付成功后写入
                        RefSellerID = seller.SellerID,//分配刮卡卖家
                        RefLotteryID = lottery.LotteryID,
                        AddDate = DateTime.Now,
                        PrizeStatus = (int)PrizeStatus.未开奖,
                        LotteryStatus = (int)LotteryStatus.正常,
                        Bonus = 0,
                        LotteryNo = "L" + DateTime.Now.ToString("yyyyMMddHHmmssfffff")
                    };
                    db.NCCLottery.Add(ncclottery);
                    //db.SaveChanges();
                    //order.NCCLotteryID = ncclottery.NCCLotteryID;
                }
                db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction,
                               "update seller set QueueNum =QueueNum+{1} where sellerid={0}",
                               seller.SellerID, order.BuyNum);
                db.SaveChanges();
                return Json(new { result = "ok", orderid = GetOrderId_Encrypt(order.OrderID), balance = member.Balance }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                return Json(new { result = "error", message = "订单创建失败!" }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetWXPayUrl(string orderid)
        {
            try
            {
                var order = GetNCCOrder();
                var member = GetCurrentMember();
                if (member != null)
                {
                    CommonPay pay = new CommonPay()
                    {
                        AddDate = DateTime.Now,
                        HeadImgUrl = member.HeadImgUrl,
                        MemberID = member.MemberID,
                        Name = member.Name,
                        NeedPay = order.NeedPay,
                        PayDirection = (int)PayDirection.购买,
                        PayStatus = (int)PayStatus.未支付,
                        Payment = order.Payment,
                        PayType = (int)PayType.微信,
                        RefOrderID = order.OrderID,
                        TotalPayment = order.TotalPayment
                    };
                    db.CommonPay.Add(pay);
                    db.SaveChanges();

                    string url = MakeUpWXPayURL(order.NeedPay.Value, "购买", "buy-" + pay.CommonPayID);
                    url += "&redirect_url=" + System.Web.HttpUtility.UrlEncode("http://m.ncc.renxingpao.com/ncc?info=payok");

                    return Json(new { result = "ok", url = url }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWXPayUrlRecharge(decimal payment)
        {
            try
            {
                var member = GetCurrentMember();
                if (member != null)
                {
                    CommonPay pay = new CommonPay()
                    {
                        AddDate = DateTime.Now,
                        HeadImgUrl = member.HeadImgUrl,
                        MemberID = member.MemberID,
                        Name = member.Name,
                        NeedPay = payment,
                        PayDirection = (int)PayDirection.充值,
                        PayStatus = (int)PayStatus.未支付,
                        Payment = payment,
                        PayType = (int)PayType.微信
                    };
                    db.CommonPay.Add(pay);
                    db.SaveChanges();

                    string url = MakeUpWXPayURL(payment, "充值", "recharge-" + pay.CommonPayID);

                    url += "&redirect_url=" + System.Web.HttpUtility.UrlEncode("http://m.ncc.renxingpao.com/ncc?info=payok");


                    return Json(new { result = "ok", url = url }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取人员的账户余额
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public JsonResult GetBalance(string token)
        {
            try
            {
                var member = GetCurrentMember();
                if (member != null)
                {
                    return Json(new { result = "ok", balance = member.Balance }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SendMessage(string token, string code, string content, string towid, string formid)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                var tomember = db.Members.Where(c => c.WechatOpenid == towid).FirstOrDefault();
                if (member != null)
                {
                    var messageModel = new Messages()
                    {
                        AddDate = DateTime.Now,
                        Content = content,
                        FromImgUrl = member.HeadImgUrl,
                        FromMemberID = member.MemberID,
                        FromName = member.NiceName,
                        ToMenberID = tomember.MemberID,
                        Type = 2

                    };
                    db.Messages.Add(messageModel);
                    db.SaveChanges();
                }
                var cc = db.CarCall.Where(c => c.Code == code).FirstOrDefault();
                if (cc != null)
                {
                    MessageHelper.SendSMS(content, cc.Phone, cc.CarNo);
                }


                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult CheckLotteryProcess(string token, string lotteryid)
        //{
        //    try
        //    {
        //        int lotteryidInt = int.Parse(GetOrderId_Decrypt(lotteryid));
        //        var lottery = db.NCCLottery.Find(lotteryidInt);
        //        string state = "0";
        //        if (lottery != null)
        //        {
        //            if (string.IsNullOrEmpty(lottery.OrgImgUrl2))
        //            {//没有拍照取证
        //                state = "1";
        //            }
        //            else if (lottery.PrizeStatus == (int)PrizeStatus.未开奖)
        //            {//没有派发奖金
        //                state = "2";
        //            }

        //            if (state != "0")
        //            {
        //                return Json(new { result = "nopass", state = state }, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    var member = db.Members.Where(c => c.Token == token).First();
        //                    decimal paymentSeller = (lottery.Payment.Value * 97) / 100;
        //                    decimal paymentFromer = (lottery.Payment.Value) / 100;
        //                    PayLog log1 = new PayLog()
        //                    {
        //                        Payment = paymentSeller,
        //                        InDate = DateTime.Now,
        //                        MemberID = member.MemberID,
        //                        PayDirection = (int)PayDirection.彩票销售结算,
        //                        RefOrderID = lottery.RefOrderID


        //                    };
        //                    PayLog log2 = new PayLog()
        //                    {
        //                        Payment = paymentFromer,
        //                        InDate = DateTime.Now,
        //                        MemberID = member.RefMemberID,
        //                        PayDirection = (int)PayDirection.销售网点推荐提成,
        //                        RefOrderID = lottery.RefOrderID
        //                    };
        //                    db.PayLog.Add(log1);
        //                    db.PayLog.Add(log2);
        //                    db.SaveChanges();

        //                    //商家结算
        //                    db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
        //                        , member.MemberID, paymentSeller));
        //                    //推荐者返点
        //                    db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
        //                        , member.RefMemberID, paymentFromer));
        //                    return Json(new { result = "pass" }, JsonRequestBehavior.AllowGet);
        //                }
        //                catch (Exception error)
        //                {
        //                    LoggerHelper.BigError(error.ToString());
        //                    return Json(new { result = "error", message = "出现内部错误" }, JsonRequestBehavior.AllowGet);
        //                }


        //            }

        //        }

        //        return Json(new { result = "error", message = "出现内部错误" }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception error)
        //    {

        //        LoggerHelper.Debug(error.ToString());
        //    }


        //    return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult Upload()
        {
            try
            {
                if (Request.HttpMethod.ToLower() == "post")
                {
                    string folder = DateTime.Now.ToString("yyyyMMdd");
                    //string token = System.Web.HttpContext.Current.Request.Form["token"].ToString();
                    string type = System.Web.HttpContext.Current.Request.Form["type"].ToString();
                    string lotteryid = System.Web.HttpContext.Current.Request.Form["lotteryid"].ToString();
                    //string noopenid = System.Web.HttpContext.Current.Request.Form["noopenid"] == null ? "0" : System.Web.HttpContext.Current.Request.Form["noopenid"].ToString();
                    //var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                    //if (member != null)
                    //{
                    //    openid = member.WechatOpenid;
                    //}
                    int lotteryidInt = int.Parse(GetOrderId_Decrypt(lotteryid));
                    var lottery = db.NCCLottery.Find(lotteryidInt);
                    Stream s = System.Web.HttpContext.Current.Request.Files[0].InputStream;
                    //System.Web.HttpContext.Current.Request.InputStream.Position = 0;
                    byte[] b = new byte[s.Length];
                    s.Read(b, 0, (int)s.Length);
                    s.Close();
                    //LoggerHelper.Debug("length :"+ System.Web.HttpContext.Current.Request.Form["file"].Length);
                    LoggerHelper.Debug("length :" + System.Web.HttpContext.Current.Request.Files.Count);
                    string savePath = savePath = Server.MapPath("~/upload/snapshot/") + folder;

                    if (!System.IO.Directory.Exists(savePath))
                    {
                        System.IO.Directory.CreateDirectory(savePath);
                    }
                    string filename = Guid.NewGuid().ToString().Replace("-", "");
                    string orgImgPathtemp = savePath + "\\" + filename + "_temp.jpg";
                    string orgImgPath = savePath + "\\" + filename + ".jpg";
                    string resizePath = savePath + "\\" + filename + "_fast.jpg";


                    FileStream fs = new FileStream(orgImgPathtemp, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Delete);
                    fs.Write(b, 0, b.Length);

                    fs.Close();
                    //list.Add(filename);
                    LoggerHelper.Debug("upload ok");
                    Image img1 = Bitmap.FromFile(orgImgPathtemp);
                    Image img2 = CombinImageHelper.ResizeImage(img1, img1.Width / 4, img1.Height / 4, 0);
                    img1 = CombinImageHelper.ResizeImage(img1, img1.Width / 2, img1.Height / 2, 0);
                    img2.Save(resizePath);
                    img1.Save(orgImgPath);
                    LoggerHelper.Debug("resize ok");

                    if (type == "1")
                    {
                        lottery.OrgImgUrl1 = orgImgPath;
                        lottery.ImgUrl1 = resizePath;
                    }
                    if (type == "2")
                    {
                        lottery.OrgImgUrl2 = orgImgPath;
                        lottery.ImgUrl2 = resizePath;
                    }
                    db.SaveChanges();

                    return Json(new { result = "ok", filename = folder + "/" + filename + ".jpg", filename2 = folder + "/" + filename + "_fast.jpg" });
                }
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());

            }

            return Json(new { result = "error" });
        }

        [HttpPost]
        public JsonResult SaveChat(string token, string title, string content, string imglist, int xiaoquid, int quanziid)
        {
            var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
            try
            {
                if (member != null)
                {
                    ZGGBBS bbsModel = new ZGGBBS()
                    {
                        AddDate = DateTime.Now,
                        Content = content,
                        Title = title,
                        ImgList = imglist,
                        OpenID = member.WechatOpenid,
                        UserID = member.MemberID,
                        ParentID = -1,
                        QuanZiID = quanziid,
                        XiaoQuID = xiaoquid
                    };
                    db.ZGGBBS.Add(bbsModel);
                    db.SaveChanges();
                    return Json(new { result = "ok" });
                }
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());

            }



            return Json(new { result = "error" });
        }

        public JsonResult GetChatList(string token, int pageIndex, int pageSize, int xiaoquid, int quanziid)
        {
            try
            {
                int startIndex = pageIndex * pageSize;
                // var chatlist = db.ZGGBBS.OrderByDescending(c => c.AddDate).Skip(startIndex).Take(pageSize);

                var chatlisttemp = from c in db.ZGGBBS where c.XiaoQuID == xiaoquid && c.QuanZiID == quanziid orderby c.AddDate descending select c;
                var chatlist = chatlisttemp.Skip(startIndex).Take(pageSize);
                ArrayList resultList = new ArrayList();
                foreach (var item in chatlist)
                {
                    var member = db.Members.Find(item.UserID);
                    string[] imgArray = item.ImgList.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    ArrayList imgList = new ArrayList();
                    foreach (var imgname in imgArray)
                    {
                        imgList.Add(new { src = "http://m.zguoguo.cc/upload/chat/" + item.OpenID + "/" + imgname });
                    }

                    resultList.Add(new
                    {
                        id = item.BBSID,
                        idx = item.BBSID,
                        title = item.Title,
                        content = item.Content,
                        headimg = member.HeadImgUrl,
                        nicename = member.NiceName,
                        imglist = imgList,
                        adddate = item.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    });

                }
                return Json(new { result = "ok", data = resultList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGoodList(string token, int pageIndex, int pageSize, string tag)
        {
            string message = "";
            try
            {
                int startIndex = pageIndex * pageSize;
                // var chatlist = db.Goods.OrderByDescending(c => c.AddDate).Skip(startIndex).Take(pageSize);

                var accountlisttemp = from c in db.Account
                                          //where c. == tag
                                      orderby c.AccountID descending
                                      select new
                                      {
                                          id = c.AccountID,
                                          thetype = c.TheType,
                                          payment = c.Payment,
                                          vusercount = c.VUserCount,
                                          maxvusercount = c.MaxVUserCount,
                                          cansellcount = c.MaxVUserCount - c.VUserCount,
                                          logoimg = "",
                                          title = "",
                                          vipenddate = c.VIPEndDate,
                                          channelid = c.ChannelID

                                      };
                //var chatlisttemp = from c in db.Goods
                //                   where c.Tags == tag
                //                   orderby c.AddDate descending
                //                   select c;                                   
                var accountlist = accountlisttemp.Skip(startIndex).Take(pageSize);
                ArrayList list = new ArrayList();
                foreach (var a in accountlist)
                {
                    var channel = db.Channel.Where(c => c.ChannelID == a.channelid).FirstOrDefault();

                    list.Add(new
                    {
                        id = a.id,
                        thetype = a.thetype,
                        payment = a.payment,
                        vusercount = a.vusercount,
                        maxvusercount = a.maxvusercount,
                        cansellcount = a.cansellcount,
                        logoimg = channel.ImgURL,
                        title = channel.Title,
                        vipenddate = a.vipenddate.Value.ToString("yyyy-MM-dd"),
                        channelid = a.channelid
                    });
                }


                return Json(new { result = "ok", list = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                message = error.ToString();
                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error", message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVUserList(string token, int pageIndex, int pageSize, string tag)
        {
            string message = "";
            try
            {
                int startIndex = pageIndex * pageSize;
                // var chatlist = db.Goods.OrderByDescending(c => c.AddDate).Skip(startIndex).Take(pageSize);

                var vuserlisttemp = from c in db.VIPUser
                                        //where c. == tag
                                    orderby c.VIPUserID descending
                                    select new
                                    {
                                        id = c.VIPUserID,
                                        payment = 0,
                                        vusercount = 0,
                                        maxvusercount = 0,
                                        cansellcount = 0,
                                        logoimg = "",
                                        title = "",
                                        vipenddate = c.AddDate,
                                        accountid = c.AccountID

                                    };
                //var chatlisttemp = from c in db.Goods
                //                   where c.Tags == tag
                //                   orderby c.AddDate descending
                //                   select c;                                   
                var vuserlist = vuserlisttemp.Skip(startIndex).Take(pageSize);
                ArrayList list = new ArrayList();
                foreach (var v in vuserlist)
                {
                    var account = db.Account.Where(c => c.AccountID == v.accountid).FirstOrDefault();
                    var channel = db.Channel.Where(c => c.ChannelID == account.ChannelID).FirstOrDefault();

                    list.Add(new
                    {
                        id = v.id,
                        payment = account.Payment,
                        vusercount = account.VUserCount,
                        maxvusercount = account.MaxVUserCount,
                        cansellcount = account.MaxVUserCount - account.VUserCount,
                        logoimg = channel.ImgURL,
                        title = channel.Title,
                        vipenddate = account.VIPEndDate.Value.ToString("yyyy-MM-dd"),
                        accountid = account.AccountID

                    });
                }


                return Json(new { result = "ok", list = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                message = error.ToString();
                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error", message = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVUserDetail(int id)
        {
            try
            {
                var good = db.Account.Find(id);
                if (good != null)
                {
                    //var member = db.Members.Where(c => c.MemberID == account.AddMemberID).FirstOrDefault();
                    var channel = db.Channel.Where(c => c.ChannelID == good.ChannelID).FirstOrDefault();

                    var goodobj = new
                    {
                        id = good.AccountID,
                        title = channel.Title,
                        desc = channel.Desc,
                        imgurl = channel.ImgURL,
                        payment = good.Payment,
                        vusercount = good.VUserCount,
                        maxvusercount = good.MaxVUserCount,
                        cansellcount = good.MaxVUserCount - good.VUserCount,
                        vipenddate = good.VIPEndDate.Value.ToString("yyyy-MM-dd"),
                        phone = good.Phone,
                        intro = channel.Intro,
                        iskami = channel.IsKaMi
                    };
                    return Json(new { result = "ok", obj = goodobj }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPinList(string token, int pageIndex, int pageSize, string tag)
        {
            try
            {
                int startIndex = pageIndex * pageSize;
                // var chatlist = db.Goods.OrderByDescending(c => c.AddDate).Skip(startIndex).Take(pageSize);

                var pinlisttemp = from c in db.Pin
                                  join m in db.Members on c.MemberID equals m.MemberID
                                  where c.Type == tag
                                  orderby c.AddDate descending
                                  select new
                                  {
                                      id = c.PinID,
                                      content = c.Content,
                                      call = c.Call,
                                      imgurl = m.HeadImgUrl,
                                      nicename = m.NiceName
                                  };


                var pinlist = pinlisttemp.Skip(startIndex).Take(pageSize);


                return Json(new { result = "ok", list = pinlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQuanZiList(string token, int pageIndex, int pageSize, string tag)
        {
            try
            {
                int startIndex = pageIndex * pageSize;
                // var chatlist = db.Goods.OrderByDescending(c => c.AddDate).Skip(startIndex).Take(pageSize);

                var qzlisttemp = from c in db.QuanZi
                                 where c.XiaoQuID == 1
                                 orderby c.AddDate descending
                                 select new
                                 {
                                     name = c.Name,
                                     qzid = c.QuanZiID,
                                     pnum = c.PeopleNum
                                 };


                var qzlist = qzlisttemp.Skip(startIndex).Take(pageSize);


                return Json(new { result = "ok", list = qzlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetShangJiaList(string token, int pageIndex, int pageSize, string tag)
        {
            try
            {
                int startIndex = pageIndex * pageSize;
                // var chatlist = db.Goods.OrderByDescending(c => c.AddDate).Skip(startIndex).Take(pageSize);

                var sjlisttemp = from c in db.ShangJia
                                 where c.ISPassed == 1
                                 orderby c.AddDate descending
                                 select new
                                 {
                                     id = c.ShangJiaID,
                                     imglist = c.ImgList,
                                     name = c.Name,
                                     address = c.Address,
                                     call = c.Call,
                                     imgurl = c.ImgUrl
                                 };


                var sjlist = sjlisttemp.Skip(startIndex).Take(pageSize);


                return Json(new { result = "ok", list = sjlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGoodDetail(int goodid)
        {
            try
            {
                var good = db.Account.Find(goodid);
                if (good != null)
                {
                    //var member = db.Members.Where(c => c.MemberID == account.AddMemberID).FirstOrDefault();
                    var channel = db.Channel.Where(c => c.ChannelID == good.ChannelID).FirstOrDefault();
                    var goodobj = new
                    {
                        id = good.AccountID,
                        title = channel.Title,
                        desc = channel.Desc,
                        imgurl = channel.ImgURL,
                        payment = good.Payment,
                        vusercount = good.VUserCount,
                        maxvusercount = good.MaxVUserCount,
                        cansellcount = good.MaxVUserCount - good.VUserCount,
                        vipenddate = good.VIPEndDate.Value.ToString("yyyy-MM-dd")
                    };
                    return Json(new { result = "ok", obj = goodobj }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetShangJiaDetail(int id)
        {
            try
            {
                var good = db.ShangJia.Find(id);
                if (good != null)
                {

                    string[] imgArray = good.ImgList.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    ArrayList imgList = new ArrayList();
                    foreach (var imgname in imgArray)
                    {
                        imgList.Add(new { src = "http://m.zguoguo.cc/upload/shangjia/" + imgname });
                    }
                    var shangjiaobj = new
                    {
                        id = good.ShangJiaID,
                        name = good.Name,
                        desc = good.Desc,
                        imgurl = good.ImgUrl,
                        imglist = imgList,
                        call = good.Call,
                        address = good.Address,
                        adddate = good.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    return Json(new { result = "ok", obj = shangjiaobj }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="goodid"></param>
        /// <returns></returns>
        public JsonResult SaveUserInfo(string nickName, string avatarUrl, string gender, string province, string city,
           string country, string signature, string encryptedData, string iv)
        {
            string userinfoStr = UserInfoHelper.DecodeUserInfo("", "", "", "");
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 用code 换取登陆信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonResult SetLoginInfo(string code, string encryptedData, string iv, string ver)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";
                url = string.Format(url, SettingBLL.AppID_XCX, SettingBLL.AppSecret_XCX, code);
                HttpHelper http = new HttpHelper();
                string block = http.Get(url);
                JObject ja = (JObject)JsonConvert.DeserializeObject(block);

                LoggerHelper.Info(block);
                string openid = ja["openid"].ToString();
                string session_key = ja["session_key"].ToString();
                string unionid = "";//ja["unionid"].ToString();

                var member = db.Members.Where(c => c.WechatOpenid == openid).FirstOrDefault();
                string pushurl = "";
                if (member != null)
                {
                    member.Session_key = session_key;
                    member.Unionid = unionid;
                    member.LoginCount = member.LoginCount + 1;
                    member.LastDate = DateTime.Now;
                    if (member.RefSellerID != null)
                    {
                        var seller = db.Seller.Find(member.RefSellerID);
                        if (seller != null)
                            pushurl = string.Format(seller.VideoPushURL, seller.SellerNo, "main");
                    }

                }
                else
                {
                    LoggerHelper.Debug(encryptedData + "," + iv);
                    string userinfoStr = UserInfoHelper.DecodeUserInfo(session_key, "", encryptedData, iv);
                    JObject userinfoAry = (JObject)JsonConvert.DeserializeObject(userinfoStr);
                    member = new Members()
                    {
                        NiceName = userinfoAry["nickName"].ToString(),
                        Sex = int.Parse(userinfoAry["gender"].ToString()),
                        City = userinfoAry["city"].ToString(),
                        Province = userinfoAry["province"].ToString(),
                        Country = userinfoAry["country"].ToString(),
                        HeadImgUrl = userinfoAry["avatarUrl"].ToString(),
                        WechatOpenid = openid,
                        Session_key = session_key,
                        Unionid = unionid,
                        Token = Guid.NewGuid().ToString().Replace("-", ""),
                        LastDate = DateTime.Now,
                        RegDate = DateTime.Now,
                        Balance = 0,
                        IsGuanZhu = 1,
                        Name = userinfoAry["nickName"].ToString(),
                        Status = (int)Common.Enum.MemmberStatus.正常,
                        UserLevel = (int)Common.Enum.UserLevel.普通账户,
                        LoginCount = 1
                    };
                    db.Members.Add(member);
                }
                db.SaveChanges();
                bool showhide = false;
                var version = db.OnLineVersion.Where(c => c.Version == ver).FirstOrDefault();
                if (version != null)
                {
                    showhide = version.State == 1 ? true : false;
                }
                return Json(new
                {
                    result = "ok",
                    obj = new
                    {
                        token = member.Token,
                        openid= openid,
                        showhide = showhide,
                        showchat = true,
                        pushurl = pushurl,
                        isrenzheng = member.IsRenZhengPass
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMemberList(string token, int pageIndex, int pageSize, string tag)
        {
            try
            {
                int startIndex = pageIndex * pageSize;
                var m_mamberList = from c in db.Members
                                   where c.XiaoQuID == 1
                                   orderby c.RegDate descending
                                   select new
                                   {
                                       imgurl = c.HeadImgUrl,
                                       nicename = c.NiceName,
                                       wid = c.WechatOpenid

                                   };
                var memberlist = m_mamberList.Skip(startIndex).Take(pageSize);
                return Json(new { result = "ok", list = memberlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }

            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQuanZiMemberList(int xiaoquid, string quanziid)
        {
            try
            {

                var m_mamberList = from c in db.Members
                                   where c.XiaoQuID == xiaoquid && ("," + c.QuanIDs + ",").Contains("," + quanziid + ",")
                                   orderby c.RegDate descending
                                   select new
                                   {
                                       imgurl = c.HeadImgUrl,
                                       nicename = c.NiceName,
                                       wid = c.WechatOpenid

                                   };

                return Json(new { result = "ok", list = m_mamberList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }

            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 彩站加入申请
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="address"></param>
        /// <param name="good"></param>
        /// <param name="intro"></param>
        /// <returns></returns>
        public JsonResult SaveCZApply(string token, string name, string phone, string address, string desc)
        {
            try
            {
                var memeber = db.Members.Where(c => c.Token == token).FirstOrDefault();
                var apply = new Apply()
                {
                    Name = name,
                    AddDate = DateTime.Now,
                    Address = address,
                    Good = "彩站申请",
                    Intro = desc,
                    Phone = phone
                };

                if (memeber != null)
                {
                    apply.MemberID = memeber.MemberID;
                }
                db.Apply.Add(apply);
                db.SaveChanges();

                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }

            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="wid">通过微信ID获得信息</param>
        /// <returns></returns>
        public JsonResult GetMemberInfo(string token, string wid)
        {
            try
            {
                Members member = null;
                if (token != "")
                {
                    member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                }
                else
                {
                    member = db.Members.Where(c => c.WechatOpenid == wid).FirstOrDefault();
                }

                var sex = "未知";
                if (member.Sex == 1)
                {
                    sex = "男";
                }
                else if (member.Sex == 0)
                {
                    sex = "女";
                }
                string carcode = "";
                var cc = db.CarCall.Where(c => c.WXOpenID == member.WechatOpenid).FirstOrDefault();
                if (cc != null)
                {
                    carcode = cc.Code;
                }

                return Json(new
                {
                    result = "ok",
                    obj = new
                    {
                        imgurl = member.HeadImgUrl,
                        nicename = member.NiceName,
                        sex = sex,
                        province = member.Province,
                        city = member.City,
                        carcode = carcode
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }

            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQuanZiInfo(string token, int quanziid)
        {
            try
            {
                bool isManager = false;
                bool isJoin = false;
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();

                var quanzi = db.QuanZi.Where(c => c.QuanZiID == quanziid).FirstOrDefault();
                if (member != null)
                {
                    if (member.MemberID == quanzi.AddMemberID)
                    {
                        isManager = true;
                    }
                }
                if (("," + member.QuanIDs + ",").Contains("," + quanzi.QuanZiID + ","))
                {
                    isJoin = true;
                }
                if (quanzi != null)
                {
                    return Json(new
                    {
                        result = "ok",
                        obj = new
                        {
                            quanziid = quanzi.QuanZiID,
                            ismanager = isManager,
                            gonggao = quanzi.GongGao,
                            isjoin = isJoin
                        }
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }

            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNewsList(string token, int pageIndex, int pageSize, string tag)
        {
            try
            {
                int startIndex = pageIndex * pageSize;
                var newslisttemp = from c in db.News
                                   where c.IsPublish == 1
                                   orderby c.NewsID descending
                                   select new
                                   {
                                       imgurl = c.ImgURL,
                                       title = c.Title,
                                       id = c.NewsID,
                                       author = c.Author

                                   };
                var newslist = newslisttemp.Skip(startIndex).Take(pageSize);
                return Json(new { result = "ok", list = newslist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }

            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNewsDetail(int newsid)
        {
            try
            {

                var news = db.News.Find(newsid);
                if (news != null)
                {
                    return Json(new
                    {
                        result = "ok",
                        obj = new
                        {
                            imgurl = news.ImgURL,
                            title = news.Title,
                            id = news.NewsID,
                            content = news.Content,
                            author = news.Author
                        }
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }

            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWYHJoinMember(int xiaoquid)
        {
            try
            {
                var memberList = db.Members.Where(c => c.XiaoQuID == xiaoquid).OrderByDescending(c => c.RegDate).Select(c => new
                {
                    imgurl = c.HeadImgUrl
                });
                return Json(new { result = "ok", list = memberList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());

            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveWYHApply(string token, string name, string phone, string address, string good, string intro)
        {
            try
            {
                var memeber = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (memeber != null)
                {
                    var apply = new Apply()
                    {
                        Name = name,
                        AddDate = DateTime.Now,
                        Address = address,
                        Good = good,
                        Intro = intro,
                        MemberID = memeber.MemberID,
                        Phone = phone
                    };
                    db.Apply.Add(apply);
                    db.SaveChanges();
                }
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }

            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JoinYWH(string token, int xiaoquid)
        {
            try
            {
                var memeber = db.Members.Where(c => c.Token == token).FirstOrDefault();
                memeber.XiaoQuID = xiaoquid;
                db.SaveChanges();
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddPin(string token, string content, string call, string tag)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {
                    Pin pinModel = new Pin()
                    {
                        Content = content,
                        Call = call,
                        AddDate = DateTime.Now,
                        MemberID = member.MemberID,
                        Type = tag
                    };
                    db.Pin.Add(pinModel);
                    db.SaveChanges();
                }


                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveFeedBack(string token, string feedback)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {
                    var feedbackModel = new FeedBack()
                    {
                        AddDate = DateTime.Now,
                        OperID = member.MemberID,
                        TheContent = feedback
                    };
                    db.FeedBack.Add(feedbackModel);
                    db.SaveChanges();
                }

                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveHello(string token, string hello, string towid)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                var tomember = db.Members.Where(c => c.WechatOpenid == towid).FirstOrDefault();
                if (member != null)
                {
                    var messageModel = new Messages()
                    {
                        AddDate = DateTime.Now,
                        Content = hello,
                        FromImgUrl = member.HeadImgUrl,
                        FromMemberID = member.MemberID,
                        FromName = member.NiceName,
                        ToMenberID = tomember.MemberID,
                        Type = 1

                    };
                    db.Messages.Add(messageModel);
                    db.SaveChanges();
                }

                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddQuanZi(string token, string name, int xiaoquid)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {
                    var qzModel = new QuanZi()
                    {
                        AddDate = DateTime.Now,
                        Name = name,
                        XiaoQuID = 1,
                        AddMemberID = member.MemberID
                    };
                    db.QuanZi.Add(qzModel);
                    db.SaveChanges();
                    if (!string.IsNullOrEmpty(member.QuanIDs))
                    {
                        member.QuanIDs = "," + qzModel.QuanZiID;
                    }
                    else
                    {
                        member.QuanIDs = qzModel.QuanZiID.ToString();
                    }
                    db.SaveChanges();
                }

                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddXianYu(string token, string intro, string imglist, string call, int selltype, decimal payment, string tags, int xiaoquid)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {
                    string imgurl = "";
                    if (imglist.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length > 0)
                    {
                        imgurl = imglist.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0];
                        imgurl = "http://m.zguoguo.cc/upload/xianyu/" + member.WechatOpenid + "/" + imgurl.Trim('|');
                    }

                    Goods goodModel = new Goods()
                    {
                        AddDate = DateTime.Now,
                        AddMemberID = member.MemberID,
                        Intro = intro,
                        Payment = payment,
                        ImgUrl = imgurl,
                        ImgList = imglist,
                        SellType = selltype,
                        ViewCount = 0,
                        Status = (int)Common.Enum.GoodStatus.上架,
                        XiaoQuID = xiaoquid,
                        Call = call,
                        Tags = tags,
                        NiceName = member.NiceName,
                        AddMemberHeadImgURL = member.HeadImgUrl

                    };
                    db.Goods.Add(goodModel);
                    db.SaveChanges();
                }

                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JoinQuanZi(string token, int quanziid)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {

                    if (!string.IsNullOrEmpty(member.QuanIDs))
                    {
                        member.QuanIDs += "," + quanziid;
                    }
                    else
                    {
                        member.QuanIDs = quanziid.ToString();
                    }
                    db.SaveChanges();
                }

                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddRenZheng(string token, string imgurl, string xiaoquid)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {
                    member.RenZhengUrl = imgurl;
                    db.SaveChanges();
                }

                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddGongGao(string token, string content, int quanziid)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {
                    //var gonggaoModel = new GongGao()
                    //{
                    //    AddDate = DateTime.Now,
                    //    QuanZiID = quanziid,
                    //    AddMemberID = member.MemberID,
                    //    Content = content
                    //};
                    //db.GongGao.Add(gonggaoModel);
                    var quanzi = db.QuanZi.Find(quanziid);
                    if (quanzi != null)
                    {
                        quanzi.GongGao = content;
                    }

                    db.SaveChanges();
                }

                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddTongZhi(string token, string content, int quanziid)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {
                    var tongzhiModel = new TongZhi()
                    {
                        AddDate = DateTime.Now,
                        QuanZiID = quanziid,
                        AddMemberID = member.MemberID,
                        Content = content
                    };
                    db.TongZhi.Add(tongzhiModel);


                    db.SaveChanges();
                }

                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetPayParam2()
        //{
        //    return GetPayParam("1ddcc01037a2467e948294ef96aba4a7", "1", 6);
        //}
        /// <summary>
        /// 用户直接购买彩票
        /// </summary>
        /// <param name="token"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public JsonResult DirectBuy(string token, string orderid)
        {
            var order = GetNCCOrder();
            var member = GetCurrentMember();
            if (order != null && member != null)
            {
                if (member.Balance >= order.NeedPay)
                {
                    decimal balance = member.Balance.Value - order.TotalPayment.Value;
                    member.Balance = balance;
                    order.PayDate = DateTime.Now;
                    order.PayStatus = (int)PayStatus.已支付;
                    var lotteryList = db.NCCLottery.Where(c => c.RefOrderID == order.OrderID);
                    foreach (var item in lotteryList)
                    {
                        item.RefMemberID = member.MemberID;
                        item.LotteryStatus = (int)LotteryStatus.已支付;
                        //item.RefSellerID = 0;
                    }
                    db.SaveChanges();
                    //db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance-{1} where MemberID={0}"
                    //                     , member.MemberID, payment));
                    return Json(new { result = "ok", balance = balance }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error", message = "账户余额不足" }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { result = "error", message = "内部错误" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 直接兑奖给用户--自己的账户，无中奖情况
        /// </summary>
        /// <param name="token"></param>
        /// <param name="lotteryid"></param>
        /// <param name="payment"></param>
        /// <param name="paytype"></param>
        /// <returns></returns>
        public JsonResult DirectPay(string token, string lotteryid, decimal payment, string paytype)
        {
            try
            {
                int lotteryidInt = int.Parse(GetOrderId_Decrypt(lotteryid));
                var member = GetCurrentMember();//开奖店铺商家

                if (member != null)
                {
                    var lottery = db.NCCLottery.Find(lotteryidInt);

                    if (string.IsNullOrEmpty(lottery.OrgImgUrl2))
                    {//没有拍照取证
                        return Json(new { result = "error", message = "还没有进行拍照验证" }, JsonRequestBehavior.AllowGet);
                    }

                    if (lottery.PrizeStatus != (int)PrizeStatus.未开奖)
                    {
                        return Json(new { result = "error", message = "此票已经兑奖" }, JsonRequestBehavior.AllowGet);
                    }
                    if (lottery.LotteryStatus != (int)LotteryStatus.已支付)
                    {
                        return Json(new { result = "error", message = "异常-此票未支付" }, JsonRequestBehavior.AllowGet);
                    }
                    if (paytype == "1")
                    {
                        if (member.Balance >= payment)
                        {
                            try
                            {
                                DateTime curTime = DateTime.Now;
                                decimal buylotterypayment = lottery.Payment.Value * 97 / 100;
                                decimal forgivenpayment = lottery.Payment.Value * 1 / 100;
                                decimal forgivenpaymentsys = lottery.Payment.Value * 2 / 100;
                                var buyer = db.Members.Find(lottery.RefMemberID);//购买者
                                //var seller = db.Seller.Find(lottery.RefSellerID);
                                var memberForgiven = db.Members.Find(buyer.RefMemberID);//推荐者
                                //if (buyer.RefMemberID == member.MemberID)
                                //{//若推荐人是商家自己 则多派发一个点
                                //    buylotterypayment = lottery.Payment.Value * 98 / 100;
                                //}
                                //店家给用户兑奖的款项--彩票中奖金额，需要在开奖店铺余额里面扣除
                                CommonPay payfrom = new CommonPay()
                                {
                                    AddDate = curTime,
                                    GoodID = lotteryidInt,
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
                                //用户中奖后会收到相应兑奖金额，需要累加到用户账户
                                CommonPay payto = new CommonPay()
                                {
                                    AddDate = curTime,
                                    GoodID = lotteryidInt,
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
                                //兑奖后为店家派发销售彩票款,系统为兑奖者派发彩票销售款
                                CommonPay payprize = new CommonPay()
                                {
                                    AddDate = curTime,
                                    GoodID = lotteryidInt,
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
                                //推荐商家将得到销售提成,
                                CommonPay payforgiven = new CommonPay()
                                {
                                    AddDate = curTime,
                                    GoodID = lotteryidInt,
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


                                lottery.EndOpenDate = curTime;
                                lottery.PrizeDate = curTime;
                                lottery.PrizeStatus = (int)PrizeStatus.已中奖;
                                lottery.Bonus = payment;
                                //启动事务
                                //db.Database.Connection.Open();
                                //var trans = db.Database.Connection.BeginTransaction();


                                db.CommonPay.Add(payfrom);
                                db.CommonPay.Add(payto);
                                db.CommonPay.Add(payprize);
                                db.CommonPay.Add(payforgiven);
                                db.CommonPay.Add(payforgivensys);
                                //db.SaveChanges();

                                db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction,
                                    "update seller set QueueNum =QueueNum-1 where sellerid={0}",
                                    lottery.RefSellerID);

                                db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction
                                   , "update Members set Balance =Balance-{1} where MemberID={0}"
                                   , member.MemberID, payment);
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
                                db.SaveChanges();

                                // db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance-{1} where MemberID={0}"
                                //             , member.MemberID, payment));
                                // db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
                                //             , buyer.MemberID, payment));
                                // db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
                                //             , member.MemberID, buylotterypayment));
                                // db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
                                //             , memberForgiven.MemberID, forgivenpayment));
                                // db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
                                //, SettingBLL.SysMemberID, forgivenpaymentsys));
                                //提交事务


                                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                            }
                            catch (Exception error)
                            {
                                LoggerHelper.BigError(error.ToString());

                                return Json(new { result = "error", message = "发生内部错误" }, JsonRequestBehavior.AllowGet);
                            }

                        }
                        else
                        {
                            return Json(new { result = "error", message = "账户余额不足" }, JsonRequestBehavior.AllowGet);
                        }


                    }
                    else if (paytype == "0")
                    {

                        DateTime curTime = DateTime.Now;
                        //直接将状态置未已经兑过奖金
                        lottery.EndOpenDate = curTime;
                        lottery.PrizeDate = curTime;
                        lottery.PrizeStatus = (int)PrizeStatus.未中奖;

                        decimal buylotterypayment = lottery.Payment.Value * 97 / 100;
                        decimal forgivenpayment = lottery.Payment.Value * 1 / 100;
                        decimal forgivenpaymentsys = lottery.Payment.Value * 2 / 100;
                        var buyer = db.Members.Find(lottery.RefMemberID);
                        var memberForgiven = db.Members.Find(buyer.RefMemberID);//推荐者
                                                                                //if (buyer.RefMemberID == member.MemberID)
                                                                                //{//若推荐人是商家自己 则多派发一个点
                                                                                //    buylotterypayment = lottery.Payment.Value * 98 / 100;
                                                                                //}
                                                                                //给开奖者打款-彩票销售款                                                        //兑奖后为店家派发销售彩票款
                        CommonPay payprize = new CommonPay()
                        {
                            AddDate = curTime,
                            GoodID = lotteryidInt,
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
                            GoodID = lotteryidInt,
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

                        //启动事务
                        //db.Database.Connection.Open();
                        //var trans = db.Database.Connection.BeginTransaction(IsolationLevel.ReadUncommitted);



                        db.CommonPay.Add(payprize);
                        db.CommonPay.Add(payforgiven);
                        db.CommonPay.Add(payforgivensys);
                        //db.SaveChanges();

                        db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction,
                            "update seller set QueueNum =QueueNum-1 where sellerid={0}",
                            lottery.RefSellerID);


                        db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction
                                   , "update Members set Balance =Balance+{1} where MemberID={0}"
                                   , member.MemberID, buylotterypayment);
                        db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction
                                   , "update Members set Balance =Balance-{1} where MemberID={0}"
                                   , memberForgiven.MemberID, forgivenpayment);
                        db.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction
                                   , "update Members set Balance =Balance-{1} where MemberID={0}"
                                   , SettingBLL.SysMemberID, forgivenpaymentsys);
                        //db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
                        //                  , member.MemberID, buylotterypayment));
                        //db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
                        //                 , memberForgiven.MemberID, forgivenpayment));
                        //db.Database.ExecuteSqlCommand(string.Format("update Members set Balance =Balance+{1} where MemberID={0}"
                        //       , SettingBLL.SysMemberID, forgivenpaymentsys));



                        //提交事务
                        //trans.Commit();
                        db.SaveChanges();

                    }
                    return Json(new { result = "ok", lotteryid = lotteryid }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error", message = "内部错误" }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error", message = "内部错误" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 给用户兑奖通过微信
        /// </summary>
        /// <param name="token"></param>
        /// <param name="lotteryid"></param>
        /// <param name="payment"></param>
        /// <returns></returns>
        public JsonResult GetPayParamPrizeing(string token, string lotteryid, decimal payment)
        {
            try
            {
                int lotteryidInt = int.Parse(GetOrderId_Decrypt(lotteryid));
                var member = GetCurrentMember();
                JsResult js_result = null;
                if (member != null)
                {
                    var lottery = db.NCCLottery.Find(lotteryidInt);
                    if (lottery.PrizeStatus != (int)PrizeStatus.未开奖)
                    {
                        return Json(new { result = "error", message = "此票已经兑奖" }, JsonRequestBehavior.AllowGet);
                    }
                    if (lottery.LotteryStatus != (int)LotteryStatus.已支付)
                    {
                        return Json(new { result = "error", message = "异常-此票未支付" }, JsonRequestBehavior.AllowGet);
                    }
                    CommonPay pay = new CommonPay()
                    {
                        AddDate = DateTime.Now,
                        GoodID = lotteryidInt,
                        HeadImgUrl = member.HeadImgUrl,
                        MemberID = member.MemberID,
                        Name = member.Name,
                        NeedPay = payment,
                        Payment = payment,
                        PayDirection = (int)PayDirection.中奖后打款,
                        PayStatus = (int)PayStatus.未支付,
                        PayType = (int)PayType.微信,
                        RefOrderID = lottery.RefOrderID,
                        TotalPayment = payment
                    };


                    db.CommonPay.Add(pay);
                    db.SaveChanges();

                    //js_result = MakeUpJsParam(member.WechatOpenid, payment, "商户支付", Guid.NewGuid().ToString().Split('-')[0]);
                    js_result = MakeUpJsParam2(member.WechatOpenid, payment, "商户支付", "payuserprize-" + pay.CommonPayID);

                    //db.SaveChanges();
                }

                return Json(new { result = "ok", obj = js_result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error", message = "内部错误" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 商户微信充值接口
        /// </summary>
        /// <param name="token"></param>
        /// <param name="lotteryid"></param>
        /// <param name="payment"></param>
        /// <returns></returns>
        public JsonResult GetPayParamRecharge(string token, decimal payment)
        {
            try
            {

                var member = GetCurrentMember();
                JsResult js_result = null;
                if (member != null)
                {

                    CommonPay pay = new CommonPay()
                    {
                        AddDate = DateTime.Now,
                        HeadImgUrl = member.HeadImgUrl,
                        MemberID = member.MemberID,
                        Name = member.Name,
                        NeedPay = payment,
                        PayDirection = (int)PayDirection.充值,
                        PayStatus = (int)PayStatus.未支付,
                        Payment = payment,
                        PayType = (int)PayType.微信
                    };


                    db.CommonPay.Add(pay);
                    db.SaveChanges();

                    //js_result = MakeUpJsParam(member.WechatOpenid, payment, "商户支付", Guid.NewGuid().ToString().Split('-')[0]);
                    js_result = MakeUpJsParam2(member.WechatOpenid, payment, "商户支付", "recharge-" + pay.CommonPayID);

                    //db.SaveChanges();
                }

                return Json(new { result = "ok", obj = js_result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error", message = "内部错误" }, JsonRequestBehavior.AllowGet);
        }
        private JsResult MakeUpJsParam(string openid, decimal payment, string payfordatil, string reforderid)
        {


            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            //初始化
            packageReqHandler.Init();
            //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

            string timeStamp = TenPayV3Util.GetTimestamp();
            string nonceStr = TenPayV3Util.GetNoncestr();

            //设置package订单参数
            packageReqHandler.SetParameter("appid", ncc2019.Common.BLL.SettingBLL.AppID);		  //公众账号ID
            packageReqHandler.SetParameter("mch_id", ncc2019.Common.BLL.SettingBLL.TenPayV3_MchId);		  //商户号
            packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
            packageReqHandler.SetParameter("body", payfordatil);
            packageReqHandler.SetParameter("out_trade_no", reforderid);		//商家订单号
            packageReqHandler.SetParameter("total_fee", Decimal.ToInt32(payment * 100).ToString());			        //商品金额,以分为单位(money * 100).ToString()
            //packageReqHandler.SetParameter("spbill_create_ip", "223.20.167.245");   //用户的公网ip，不是商户服务器IP
            //packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
            packageReqHandler.SetParameter("notify_url", "http://m.vip.renxingpao.com/API/PayNotifyUrl/");		    //接收财付通通知的URL
            packageReqHandler.SetParameter("trade_type", Senparc.Weixin.MP.TenPayV3Type.JSAPI.ToString());	                    //交易类型
            packageReqHandler.SetParameter("openid", openid);	                    //用户的openId

            string sign = packageReqHandler.CreateMd5Sign("key", ncc2019.Common.BLL.SettingBLL.TenPayV3_Key).ToString();
            packageReqHandler.SetParameter("sign", sign);	                    //签名

            string data = packageReqHandler.ParseXML();

            var result = Senparc.Weixin.MP.AdvancedAPIs.TenPayV3.Unifiedorder(data);
            LoggerHelper.Debug(result);
            var res = System.Xml.Linq.XDocument.Parse(result);
            if (res.Element("xml").Element("result_code") != null
                && res.Element("xml").Element("result_code").Value == "SUCCESS")
            {
                string prepayId = res.Element("xml").Element("prepay_id").Value;

                //设置支付参数
                RequestHandler paySignReqHandler = new RequestHandler(null);
                paySignReqHandler.SetParameter("appId", ncc2019.Common.BLL.SettingBLL.AppID);
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");
                string paySign = paySignReqHandler.CreateMd5Sign("key", ncc2019.Common.BLL.SettingBLL.TenPayV3_Key);



                JsResult js_result = new JsResult()
                {
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
        public JsResult mp2()
        {
            return MakeUpJsParam2("oHuoD5kxSL0fZGSe5STGsGpQ3Qkg", 6, "2", "111");
        }
        public string mp3()
        {
            return MakeUpWXPayURL(6, "2", DateTime.Now.ToString("yyyyMMddHHmmssffffff"));
        }

        //public string mp4()
        //{
        //    return WeiXinCommonHelper.PayToUser(new Common.Model.PayToUser()
        //    {
        //        amount = 1,
        //        bank_code = "1002",
        //        enc_bank_no = "6212260200079933582",
        //        enc_true_name = "周斌",
        //        partner_trade_no = DateTime.Now.ToString("yyyyMMddHHmmssffffff")
        //    });
        //    //return MakeUpJsParam4(1, "2", DateTime.Now.ToString("yyyyMMddHHmmssffffff"));

        //}
        public JsResult MakeUpJsParam2(string openid, decimal payment, string payfordatil, string reforderid)
        {


            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            //初始化
            packageReqHandler.Init();
            //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

            string timeStamp = TenPayV3Util.GetTimestamp();
            string nonceStr = TenPayV3Util.GetNoncestr();



            System.Random Random = new System.Random();



            var dic = new Dictionary<string, string>
            {
                {"appid", ncc2019.Common.BLL.SettingBLL.AppID_XCX},
                {"mch_id", ncc2019.Common.BLL.SettingBLL.TenPayV3_MchId},
                {"nonce_str", GetRandomString(20)/*Random.Next().ToString()*/},
                {"body",payfordatil},
                {"out_trade_no",reforderid},//商户自己的订单号码  
                {"total_fee",Decimal.ToInt32(payment * 100).ToString()},
                {"spbill_create_ip","123.57.137.75"},//服务器的IP地址  
                {"notify_url","http://ncc.renxingpao.com/API/PayNotifyUrl/"},//异步通知的地址，不能带参数  
                {"trade_type","JSAPI" },
                {"openid",openid}
            };
            //加入签名  
            dic.Add("sign", GetSignString(dic));

            var sb = new StringBuilder();
            sb.Append("<xml>");


            foreach (var d in dic)
            {
                sb.Append("<" + d.Key + ">" + d.Value + "</" + d.Key + ">");
            }
            sb.Append("</xml>");
            var xml = new XmlDocument();
            //  xml.LoadXml(GetPostString("https://api.mch.weixin.qq.com/pay/unifiedorder", sb.ToString()));  
            //CookieCollection coo = new CookieCollection();
            //Encoding en = Encoding.GetEncoding("UTF-8");

            //HttpWebResponse response = CreatePostHttpResponse("https://api.mch.weixin.qq.com/pay/unifiedorder", sb.ToString(), en);
            ////打印返回值  
            //Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            //StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            //string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            //Console.WriteLine(html);  
            HttpHelper helper = new HttpHelper();
            string result = helper.Post("https://api.mch.weixin.qq.com/pay/unifiedorder", sb.ToString());
            LoggerHelper.Debug(result);
            var res = System.Xml.Linq.XDocument.Parse(result);
            if (res.Element("xml").Element("result_code") != null
                && res.Element("xml").Element("result_code").Value == "SUCCESS")
            {
                string prepayId = res.Element("xml").Element("prepay_id").Value;

                //设置支付参数
                RequestHandler paySignReqHandler = new RequestHandler(null);
                paySignReqHandler.SetParameter("appId", ncc2019.Common.BLL.SettingBLL.AppID);
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");
                string paySign = paySignReqHandler.CreateMd5Sign("key", ncc2019.Common.BLL.SettingBLL.TenPayV3_Key);



                JsResult js_result = new JsResult()
                {
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

        public string MakeUpJsParam4(decimal payment, string payfordatil, string reforderid)
        {


            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            //初始化
            packageReqHandler.Init();
            //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

            string timeStamp = TenPayV3Util.GetTimestamp();
            string nonceStr = TenPayV3Util.GetNoncestr();
            System.Random Random = new System.Random();
            // TODO Auto-generated method stub
            String source = "周斌";  //持卡人姓名
            String pank = "6212260200079933582";    //银行卡号
                                                    //注意 这里的  publicKeyPKCS8  是微信支付公钥后经openssl 转化成PKCS8格式的公钥，需要转换一次，详情见微信官方文档
            String publicKeyPKCS8 = "";//WeiXinCommonHelper.GetPublicKey();
            String enc_true_name = RSAHelper.RSAEncrypt(publicKeyPKCS8, source);
            String enc_bank_no = RSAHelper.RSAEncrypt(publicKeyPKCS8, pank);

            //parameters1.put("mch_id", WChatInfo.MCH_ID);
            //parameters1.put("partner_trade_no", partner_trade_no);
            //parameters1.put("nonce_str", nonce_str1);
            //parameters1.put("enc_bank_no", enc_bank_no);
            //parameters1.put("enc_true_name", enc_true_name);
            //parameters1.put("bank_code", bank_code);
            //parameters1.put("amount", amount);
            //parameters1.put("desc", desc);
            string bank_code = "1002";
            var dic = new Dictionary<string, string>
            {
                //{"mch_appid", ncc2019.Common.BLL.SettingBLL.AppID_XCX},
                {"mch_id", ncc2019.Common.BLL.SettingBLL.TenPayV3_MchId},
                {"nonce_str", GetRandomString(20)/*Random.Next().ToString()*/},
                {"enc_bank_no",enc_bank_no},
                {"enc_true_name",enc_true_name},
                {"partner_trade_no",reforderid},//商户自己的订单号码  
                {"bank_code",bank_code},
                {"amount",Decimal.ToInt32(payment * 100).ToString()},
                //{"spbill_create_ip","123.57.137.75"},//服务器的IP地址  
                //{"notify_url","http://ncc.renxingpao.com/API/PayNotifyUrl/"},//异步通知的地址，不能带参数  
                //{"trade_type","JSAPI" },
                {"desc",payfordatil}
            };
            //加入签名  
            dic.Add("sign", GetSignString(dic));

            var sb = new StringBuilder();
            sb.Append("<xml>");


            foreach (var d in dic)
            {
                sb.Append("<" + d.Key + ">" + d.Value + "</" + d.Key + ">");
            }
            sb.Append("</xml>");
            var xml = new XmlDocument();
            //  xml.LoadXml(GetPostString("https://api.mch.weixin.qq.com/pay/unifiedorder", sb.ToString()));  
            //CookieCollection coo = new CookieCollection();
            //Encoding en = Encoding.GetEncoding("UTF-8");

            //HttpWebResponse response = CreatePostHttpResponse("https://api.mch.weixin.qq.com/pay/unifiedorder", sb.ToString(), en);
            ////打印返回值  
            //Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            //StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            //string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            //Console.WriteLine(html);  
            HttpHelper helper = new HttpHelper();
            string result = helper.Post("https://api.mch.weixin.qq.com/mmpaysptrans/pay_bank", sb.ToString());
            return result;
            LoggerHelper.Debug(result);
            var res = System.Xml.Linq.XDocument.Parse(result);
            if (res.Element("xml").Element("result_code") != null
                && res.Element("xml").Element("result_code").Value == "SUCCESS")
            {
                string prepayId = res.Element("xml").Element("prepay_id").Value;

                //设置支付参数
                RequestHandler paySignReqHandler = new RequestHandler(null);



                return "ok";
            }
            else
            {

                return "error";
            }


        }
        public string MakeUpWXPayURL(decimal payment, string payfordatil, string reforderid)
        {


            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            //初始化
            packageReqHandler.Init();
            //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

            string timeStamp = TenPayV3Util.GetTimestamp();
            string nonceStr = TenPayV3Util.GetNoncestr();



            System.Random Random = new System.Random();



            var dic = new Dictionary<string, string>
            {
                {"appid", ncc2019.Common.BLL.SettingBLL.AppID_XCX},
                {"mch_id", ncc2019.Common.BLL.SettingBLL.TenPayV3_MchId},
                {"nonce_str", GetRandomString(20)/*Random.Next().ToString()*/},
                {"body",payfordatil},
                {"out_trade_no",reforderid},//商户自己的订单号码  
                {"total_fee",Decimal.ToInt32(payment * 100).ToString()},
                {"spbill_create_ip",IPAddressHelper.IPAddress},//服务器的IP地址  
                {"notify_url","http://ncc.renxingpao.com/API/PayNotifyUrl/"},//异步通知的地址，不能带参数  
                {"trade_type","MWEB" }
            };
            //加入签名  
            dic.Add("sign", GetSignString(dic));

            var sb = new StringBuilder();
            sb.Append("<xml>");


            foreach (var d in dic)
            {
                sb.Append("<" + d.Key + ">" + d.Value + "</" + d.Key + ">");
            }
            sb.Append("</xml>");
            var xml = new XmlDocument();
            //  xml.LoadXml(GetPostString("https://api.mch.weixin.qq.com/pay/unifiedorder", sb.ToString()));  
            //CookieCollection coo = new CookieCollection();
            //Encoding en = Encoding.GetEncoding("UTF-8");

            //HttpWebResponse response = CreatePostHttpResponse("https://api.mch.weixin.qq.com/pay/unifiedorder", sb.ToString(), en);
            ////打印返回值  
            //Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            //StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            //string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            //Console.WriteLine(html);  
            HttpHelper helper = new HttpHelper();
            LoggerHelper.Debug(sb.ToString());
            string result = helper.Post("https://api.mch.weixin.qq.com/pay/unifiedorder", sb.ToString());
            LoggerHelper.Debug(result);
            var res = System.Xml.Linq.XDocument.Parse(result);
            if (res.Element("xml").Element("result_code") != null
                && res.Element("xml").Element("result_code").Value == "SUCCESS")
            {
                string prepayId = res.Element("xml").Element("prepay_id").Value;
                string url = res.Element("xml").Element("mweb_url").Value;




                return url;
            }
            else
            {

                return "";
            }


        }
        /// <summary>    
        /// 获取时间戳    
        /// </summary>    
        /// <returns></returns>    
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }



        public string GetSignString(Dictionary<string, string> dic)
        {
            string key = ncc2019.Common.BLL.SettingBLL.TenPayV3_Key;//商户平台 API安全里面设置的KEY  32位长度  
                                                                    //排序  
            dic = dic.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);
            //连接字段  
            var sign = dic.Aggregate("", (current, d) => current + (d.Key + "=" + d.Value + "&"));
            sign += "key=" + key;
            //MD5  
            // sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sign, "MD5").ToUpper();  
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            sign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sign))).Replace("-", null);
            return sign;
        }


        /// <summary>  
        /// 从字符串里随机得到，规定个数的字符串.  
        /// </summary>  
        /// <param name="allChar"></param>  
        /// <param name="CodeCount"></param>  
        /// <returns></returns>  
        public string GetRandomString(int CodeCount)
        {
            string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(allCharArray.Length - 1);
                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }
                temp = t;
                RandomCode += allCharArray[t];
            }

            return RandomCode;
        }
        [HttpGet]
        public string PayToMember(string openid, decimal payment, string payfordatil, string reforderid)
        {

            reforderid = Guid.NewGuid().ToString().Split('-')[0];
            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            //初始化
            packageReqHandler.Init();
            //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

            string timeStamp = TenPayV3Util.GetTimestamp();
            string nonceStr = TenPayV3Util.GetNoncestr();

            //设置package订单参数
            packageReqHandler.SetParameter("mch_appid", "wx799e0ba77ae49b9b");		  //公众账号ID
            packageReqHandler.SetParameter("mchid", "1466759502");		  //商户号
            packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
            packageReqHandler.SetParameter("desc", payfordatil);
            packageReqHandler.SetParameter("check_name", "NO_CHECK");
            packageReqHandler.SetParameter("partner_trade_no", reforderid);		//商家订单号
            packageReqHandler.SetParameter("amount", Decimal.ToInt32(payment * 100).ToString());                    //商品金额,以分为单位(money * 100).ToString()
                                                                                                                    //packageReqHandler.SetParameter("spbill_create_ip", "223.20.167.245");   //用户的公网ip，不是商户服务器IP
                                                                                                                    //packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
                                                                                                                    //packageReqHandler.SetParameter("notify_url", "http://m.zguoguo.cc/API/PayNotifyUrl/");		    //接收财付通通知的URL
                                                                                                                    //packageReqHandler.SetParameter("trade_type", Senparc.Weixin.MP.TenPayV3Type.JSAPI.ToString());	                    //交易类型
            packageReqHandler.SetParameter("check_name", "NO_CHECK");
            packageReqHandler.SetParameter("spbill_create_ip", "123.57.137.75");
            packageReqHandler.SetParameter("openid", openid);	                    //用户的openId

            string sign = packageReqHandler.CreateMd5Sign("key", "9809c47d853241a49f9de42963f594cb").ToString();
            packageReqHandler.SetParameter("sign", sign);	                    //签名

            string data = packageReqHandler.ParseXML();

            string block = "";
            HttpHelper helper = new HttpHelper();
            try
            {
                block = helper.Post("https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers", data);
                // var result = Senparc.Weixin.MP.AdvancedAPIs.TenPayV3.Unifiedorder(data);
                LoggerHelper.Debug(block);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }

            return block;


        }
        [HttpGet]
        public JsonResult CreatePayQrCode(string token, string shangjiaid)
        {


            try
            {
                string imgurl = "";

                var shangjia = db.ShangJia.Find(int.Parse(shangjiaid));
                if (shangjia != null)
                {
                    string wxtoken = TokenHelper.GetXCXToken();
                    HttpHelper http = new HttpHelper();
                    byte[] buffer = http.GetByteByPost("https://api.weixin.qq.com/wxa/getwxacode?access_token=" + wxtoken, "{\"path\": \"pages/pay/index?shangjiaid=" + shangjiaid + "\", \"width\": 430}");
                    string filename = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                    string filepath = Server.MapPath("~/upload/pay/") + filename;
                    FileStream fs = System.IO.File.Open(filepath, FileMode.Create);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                    imgurl = "https://m.zguoguo.cc/upload/pay/" + filename;

                    shangjia.QrCodeUrl = imgurl;
                    db.SaveChanges();
                }




                return Json(new { result = "ok", obj = new { imgurl = imgurl } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error", message = "内部错误" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CreateCarQrCode(string token, string carcode)
        {


            try
            {
                string imgurl = "";

                var cc = db.CarCall.Where(c => c.Code == carcode).FirstOrDefault();
                if (cc != null)
                {
                    if (string.IsNullOrEmpty(cc.QrCodeURL))
                    {
                        string wxtoken = TokenHelper.GetXCXToken();
                        HttpHelper http = new HttpHelper();
                        byte[] buffer = http.GetByteByPost("https://api.weixin.qq.com/wxa/getwxacode?access_token=" + wxtoken, "{\"path\": \"pages/qiche/show?code=" + carcode + "\", \"width\": 430}");
                        string filename = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                        string filepath = Server.MapPath("~/upload/carcall/") + filename;

                        Image bitmap = Bitmap.FromStream(new MemoryStream(buffer));//Bitmap.FromFile(@"D:\八零创想\项目\智裹裹\code\ncc2019.Mobile\upload\pay\2b0a526a5011425da8d1012a81d5a0f1.jpg");

                        Image img = CombinImageHelper.CreateImage("微信扫一扫联系车主", false, 30);

                        Bitmap bmp = CombinImageHelper.CombinImage(bitmap, img, 0, 25);
                        //         MemoryStream ms = new MemoryStream();
                        //       bmp.Save(ms, ImageFormat.Png);

                        bmp.Save(filepath);

                        //FileStream fs = System.IO.File.Open(filepath, FileMode.Create);
                        //fs.Write(buffer, 0, buffer.Length);
                        //fs.Close();
                        imgurl = "https://m.zguoguo.cc/upload/carcall/" + filename;

                        cc.QrCodeURL = imgurl;
                        db.SaveChanges();
                    }
                    else
                    {
                        imgurl = cc.QrCodeURL;
                    }

                }




                return Json(new { result = "ok", obj = new { imgurl = imgurl } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error", message = "内部错误" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CreateRegCarQrCode()
        {


            try
            {
                string imgurl = "";


                //string wxtoken = TokenHelper.GetXCXToken();
                //HttpHelper http = new HttpHelper();
                //byte[] buffer = http.GetByteByPost("https://api.weixin.qq.com/wxa/getwxacode?access_token=" + wxtoken, "{\"path\": \"pages/qiche/reg\", \"width\": 430}");
                string filename = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                string filepath = Server.MapPath("~/upload/carcall/") + filename;

                //Image bitmap = Bitmap.FromStream(new MemoryStream(buffer));//Bitmap.FromFile(@"D:\八零创想\项目\智裹裹\code\ncc2019.Mobile\upload\pay\2b0a526a5011425da8d1012a81d5a0f1.jpg");
                Image bitmap = Bitmap.FromFile(@"D:\八零创想\项目\智裹裹\code\ncc2019.Mobile\upload\carcall\reg.jpg");

                Image img = CombinImageHelper.CreateImage("获取保护隐私临时停车云码", false, 22);//获取保护隐私临时停车云码
                //扫一扫获取临时挪车电子停车牌
                Bitmap bmp = CombinImageHelper.CombinImage(bitmap, img, 0, 25);


                bmp.Save(filepath);

                //FileStream fs = System.IO.File.Open(filepath, FileMode.Create);
                //fs.Write(buffer, 0, buffer.Length);
                //fs.Close();
                imgurl = "https://m.zguoguo.cc/upload/carcall/" + filename;


                return Json(new { result = "ok", obj = new { imgurl = imgurl } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error", message = "内部错误" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddShangjia(string token, string name, string address, string call, string desc, string imglist)
        {
            try
            {
                int shangjiaid = 0;
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();

                if (member != null)
                {
                    string imgurl = "";
                    if (imglist.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Length > 0)
                        imgurl = "http://m.zguoguo.cc/upload/shangjia/" + imglist.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[0];

                    var shangjia = new ShangJia()
                    {
                        AddDate = DateTime.Now,
                        Address = address,
                        AddMemberID = member.MemberID,
                        Name = name,
                        Call = call,
                        Desc = desc,
                        ImgList = imglist,
                        ImgUrl = imgurl
                    };
                    db.ShangJia.Add(shangjia);
                    db.SaveChanges();
                    shangjiaid = shangjia.ShangJiaID;

                    return Json(new { result = "ok", obj = new { shangjiaid = shangjiaid } }, JsonRequestBehavior.AllowGet);
                }



            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error", message = "内部错误" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateShangjia(string token, int shangjiaid, string managername, string managercall, int needpayqr)
        {
            try
            {

                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {
                    var shangjia = db.ShangJia.Find(shangjiaid);
                    if (shangjia != null)
                    {
                        shangjia.ManagerCall = managercall;
                        shangjia.ManagerName = managername;
                        shangjia.NeedPayQr = needpayqr;
                    }

                    db.SaveChanges();

                }


                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error", message = "内部错误" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddCarCall(string token, string carname, string carno, string phone, string name, string memo, int isshowphone, int ishidephone, int isrecwxmessage)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {
                    var cc = db.CarCall.Where(c => c.MemberID == member.MemberID).FirstOrDefault();
                    string code = "";
                    if (cc == null)
                    {
                        code = Guid.NewGuid().ToString().Replace("-", "");
                        cc = new CarCall()
                        {
                            AddDate = DateTime.Now,
                            CarName = carname,
                            CarNo = carno,
                            Phone = phone,
                            Name = name,
                            Memo = memo,
                            MemberID = member.MemberID,
                            WXOpenID = member.WechatOpenid,
                            ISHidePhone = ishidephone,
                            ISRecWXMessage = isrecwxmessage,
                            ISShowPhone = isshowphone,
                            Code = code
                        };
                        db.CarCall.Add(cc);
                        db.SaveChanges();
                    }
                    else
                    {
                        code = cc.Code;
                    }


                    return Json(new { result = "ok", carcode = code }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCarCall(string token, string code, string carname, string carno, string phone, string name, string memo, int isshowphone, int ishidephone, int isrecwxmessage)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {
                    var cc = db.CarCall.Where(c => c.Code == code).FirstOrDefault();
                    if (cc != null)
                    {
                        cc.CarName = carname;
                        cc.CarNo = carno;
                        cc.Phone = phone;
                        cc.Name = name;
                        cc.ISShowPhone = isshowphone;
                        cc.ISHidePhone = ishidephone;
                        cc.ISRecWXMessage = isrecwxmessage;
                        cc.Memo = memo;

                        db.SaveChanges();
                    }

                }

                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCarCall(string token, string code, string thetype)
        {
            try
            {
                var member = db.Members.Where(c => c.Token == token).FirstOrDefault();
                if (member != null)
                {
                    var cc = db.CarCall.Where(c => c.Code == code).FirstOrDefault();
                    //if (cc.WXOpenID == member.WechatOpenid)
                    //{
                    var isowner = 0;
                    if (member.WechatOpenid == cc.WXOpenID) isowner = 1;
                    string phone = cc.Phone;
                    if (cc.ISShowPhone != 1 && thetype != "set")
                    {
                        phone = cc.Phone.Substring(0, 3) + "****";
                    }

                    var obj = new
                    {
                        phone = phone,
                        name = cc.Name,
                        carno = cc.CarNo,
                        carname = cc.CarName,
                        wxopenid = cc.WXOpenID,
                        isshowphone = cc.ISShowPhone,
                        isrecwxmessage = cc.ISRecWXMessage,
                        ishidephone = cc.ISHidePhone,
                        memo = cc.Memo,
                        isowner = isowner
                    };
                    return Json(new { result = "ok", obj = obj }, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{
                    //    return Json(new { result = "reload", msg = "用户与二维码信息不对应" }, JsonRequestBehavior.AllowGet);
                    //}
                }


            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPhoneNo(string token, string code, string encryptedData, string iv)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";
                url = string.Format(url, SettingBLL.AppID_XCX, SettingBLL.AppSecret_XCX, code);
                HttpHelper http = new HttpHelper();
                string block = http.Get(url);
                JObject ja = (JObject)JsonConvert.DeserializeObject(block);

                LoggerHelper.Info(block);
                string openid = ja["openid"].ToString();
                string session_key = ja["session_key"].ToString();
                string unionid = "";//ja["unionid"].ToString();
                string userinfoStr = UserInfoHelper.DecodeUserInfo(session_key, "", encryptedData, iv);
                JObject userinfoAry = (JObject)JsonConvert.DeserializeObject(userinfoStr);

                return Json(new { result = "ok", phone = userinfoAry["purePhoneNumber"].ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }


            return Json(new { result = "error", message = "内部错误" }, JsonRequestBehavior.AllowGet);
        }
    }

}
