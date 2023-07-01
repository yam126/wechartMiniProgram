using ncc2019.Common.BLL;
using ncc2019.Common.Tool;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class ControllerBase : Controller
    {
        protected ncc2019Entities db = new ncc2019Entities();

        public Common.Model.MemberInfo CurMemberInfo { get { return SessionHelper.CurMemberInfo; } }
        public ControllerBase()
        {
            #region 判断当前登录的是否为微信环境
            ViewBag.isweixin = false;
            if (Common.Tool.UserAgentHelper.IsWeiXin())
            {
                ViewBag.isweixin = true;
            }
            #endregion

            #region 全景统计参数生成
            ViewBag.cnzz = new CS(1274096931).TrackPageView();
            #endregion

            SessionHelper.InitJumpUrl("");
        }
        /// <summary>
        /// 检查是否存在此用户  不存在则返回false
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckUser()
        {


            if (SessionHelper.CurMemberInfo == null)
            {
                if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["uid"]))
                {
                    Members memberModel = db.Members.Find(int.Parse(System.Web.HttpContext.Current.Request.QueryString["uid"]));
                    SessionHelper.CurMemberInfo = Common.Model.MemberInfo.BuildMemberInfo(memberModel);
                    SessionHelper.CurMemberInfo.CurMeachineID = 1;
                }
                return true;

                //SessionHelper.SetJumpUrl(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                //System.Web.HttpContext.Current.Response.Redirect("~/Login");
                //return false;

            }
            else
            {

                return true;
            }
        }
        /// <summary>
        /// 检查是否获得到了微信 openid
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckUserWeiXin()
        {
            if (SessionHelper.CurMemberInfo == null || SessionHelper.CurMemberInfo.WeChatOpenid == null)
            {
                LoggerHelper.Debug("CheckUserWeiXin");
                //第一次进入页面需要进行跳转
                string urlStr = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
                string url = OAuthApi.GetAuthorizeUrl(SettingBLL.AppID, "http://" + SettingBLL.MobileDomain + "/login/weixinlogin"
                    , System.Web.HttpUtility.UrlEncode(urlStr), OAuthScope.snsapi_base);
                LoggerHelper.Debug(url);
                System.Web.HttpContext.Current.Response.Redirect(url);
                //Response.RedirectLocation = "http://www.baidu.com";
                //Response.Redirect("http://www.baidu.com");
                return false;
            }
            else
            {
                return true;
            }
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            // LoggerHelper.Debug("Initialize");
            if (Common.Tool.UserAgentHelper.IsWeiXin())
            {
                if (!CheckUserWeiXin())
                {
                    return;
                }

            }
            else
            {
                if (!CheckUser())
                {
                    return;
                }
            }

            base.Initialize(requestContext);


        }
        /// <summary>
        /// 显示提醒消息
        /// </summary>
        /// <param name="message"></param>
        public void ShowAlertMessage(string message)
        {
            @ViewBag.dangermessage = message;
            //ViewBag.Message = string.Format("<script>alert('{0}')</script>", message);
            //Response.Write(
            //    string.Format("<script>alert('{0}')</script>", message));
        }

        public void showSuccessMessage(string message)
        {
            @ViewBag.successmessage = message;
            //ViewBag.Message = string.Format("<script>alert('{0}')</script>", message);
            //Response.Write(
            //    string.Format("<script>alert('{0}')</script>", message));
        }


        #region 业务集成方法
        public void CheckRight(string url)
        {
            var order = GetOrder();
            if (order != null)
            {
                if (this.CurMemberInfo != null && this.CurMemberInfo.MemnerID == order.MemberID)
                {
                    //属于自己的订单
                }
                else
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        System.Web.HttpContext.Current.Response.Redirect(url);
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Response.Redirect("/gift/NoRight");
                    }

                }
            }

        }

        /// <summary>
        /// 获得订单信息--根据前台传递来的加密后的orderid
        /// </summary>
        /// <returns></returns>
        public Orders GetOrder()
        {
            var orderid = System.Web.HttpContext.Current.Request.QueryString["orderid"];
            if (string.IsNullOrEmpty(orderid))
            {
                orderid = System.Web.HttpContext.Current.Request.Form["orderid"];
            }
            var order = db.Orders.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(orderid)));
            ViewBag.orderid = orderid;
            return order;
        }
        public Orders GetOrder(string encode_orderid)
        {

            var order = db.Orders.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(encode_orderid)));
            ViewBag.orderid = encode_orderid;
            return order;
        }
       

        public string GetOrderId_Encrypt(int orderid)
        {
            return Common.Tool.DESEncrypt.Encrypt(orderid);
        }
        public string GetOrderId_Decrypt(string orderid)
        {
            return Common.Tool.DESEncrypt.Decrypt(orderid);
        }

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool UpdateOrder(Orders order)
        {
            bool result = false;
            try
            {
                //db.Entry(order).State = EntityState.Modified;
                db.Entry(order).Property(c => c.TotalPayment).IsModified = false;
                //db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                result = true;
            }
            catch (Exception error)
            {
                //写入日志
                Common.Tool.LoggerHelper.Info(error.ToString());
            }
            return result;
        }
        /// <summary>
        /// 绑定设备到用户
        /// </summary>
        /// <param name="backcode"></param>
        /// <returns></returns>
        public bool BindMachine(string backcode)
        {
            bool result = false;
            //绑定设备
            Common.Tool.HttpHelper http = new Common.Tool.HttpHelper();
            string block = http.Post("https://api.weixin.qq.com/device/compel_bind?access_token=" + Common.Tool.TokenHelper.GetToken(),
                 "{" + string.Format("\"device_id\": \"{0}\",\"openid\":\"{1}\""
                 , backcode, this.CurMemberInfo.WeChatOpenid) + "}");
            if (block.Contains("ok"))
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 解除绑定设备到用户
        /// </summary>
        /// <param name="backcode"></param>
        /// <returns></returns>
        public bool UnBindMachine(string backcode)
        {
            bool result = false;
            //绑定设备
            Common.Tool.HttpHelper http = new Common.Tool.HttpHelper();
            string block = http.Post("https://api.weixin.qq.com/device/compel_unbind?access_token=" + Common.Tool.TokenHelper.GetToken(),
                 "{" + string.Format("\"device_id\": \"{0}\",\"openid\":\"{1}\""
                 , backcode, this.CurMemberInfo.WeChatOpenid) + "}");
            if (block.Contains("ok"))
            {
                result = true;
            }
            return result;
        }

        #endregion
    }

    public class ControllerBaseNoCheck : Controller
    {
        protected ncc2019Entities db = new ncc2019Entities();
        public Common.Model.MemberInfo CurMemberInfo { get { return SessionHelper.CurMemberInfo; } }
        public ControllerBaseNoCheck()
        {
            ViewBag.cnzz = new CS(1274096931).TrackPageView();
        }
        /// <summary>
        /// 显示提醒消息
        /// </summary>
        /// <param name="message"></param>
        public void ShowAlertMessage(string message)
        {
            @ViewBag.dangermessage = message;
            //ViewBag.Message = string.Format("<script>alert('{0}')</script>", message);
            //Response.Write(
            //    string.Format("<script>alert('{0}')</script>", message));
        }

        public void showSuccessMessage(string message)
        {
            @ViewBag.successmessage = message;
            //ViewBag.Message = string.Format("<script>alert('{0}')</script>", message);
            //Response.Write(
            //    string.Format("<script>alert('{0}')</script>", message));
        }
        /// <summary>
        /// 根据token获得当前用户的信息
        /// </summary>
        /// <returns></returns>
        public Members GetCurrentMember()
        {

            var token = System.Web.HttpContext.Current.Request.QueryString["token"];
            if (string.IsNullOrEmpty(token))
            {
                token = System.Web.HttpContext.Current.Request.Form["token"];
            }
            if (string.IsNullOrEmpty(token))
            {
                token = System.Web.HttpContext.Current.Request.Params["token"];
            }
            if (string.IsNullOrEmpty(token))
            {
                token = System.Web.HttpContext.Current.Request["token"];
            }
            var member = db.Members.Where(c => c.Token == token).FirstOrDefault();

            return member;
        }
        #region 业务集成方法
        public void CheckRight(string url)
        {
            var order = GetOrder();
            if (order != null)
            {
                if (this.CurMemberInfo != null && this.CurMemberInfo.MemnerID == order.MemberID)
                {
                    //属于自己的订单
                }
                else
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        System.Web.HttpContext.Current.Response.Redirect(url);
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Response.Redirect("/gift/NoRight");
                    }

                }
            }

        }

        /// <summary>
        /// 获得订单信息--根据前台传递来的加密后的orderid
        /// </summary>
        /// <returns></returns>
        public NCCOrders GetNCCOrder()
        {
            var orderid = System.Web.HttpContext.Current.Request.QueryString["orderid"];
            if (string.IsNullOrEmpty(orderid))
            {
                orderid = System.Web.HttpContext.Current.Request.Form["orderid"];
            }
            var order = db.NCCOrders.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(orderid)));
            ViewBag.orderid = orderid;
            return order;
        }
        public NCCLottery GetNCCLottery()
        {
            var lotteryid = System.Web.HttpContext.Current.Request.QueryString["lotteryid"];
            if (string.IsNullOrEmpty(lotteryid))
            {
                lotteryid = System.Web.HttpContext.Current.Request.Form["lotteryid"];
            }
            var lottery = db.NCCLottery.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(lotteryid)));
            ViewBag.lotteryid = lotteryid;
            return lottery;
        }
        public Orders GetOrder()
        {
            var orderid = System.Web.HttpContext.Current.Request.QueryString["orderid"];
            if (string.IsNullOrEmpty(orderid))
            {
                orderid = System.Web.HttpContext.Current.Request.Form["orderid"];
            }
            var order = db.Orders.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(orderid)));
            ViewBag.orderid = orderid;
            return order;
        }
        public Orders GetOrder(string encode_orderid)
        {

            var order = db.Orders.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(encode_orderid)));
            ViewBag.orderid = encode_orderid;
            return order;
        }

        public string GetOrderId_Encrypt(int orderid)
        {
            return Common.Tool.DESEncrypt.Encrypt(orderid);
        }
        public string GetOrderId_Decrypt(string orderid)
        {
            return Common.Tool.DESEncrypt.Decrypt(orderid);
        }

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool UpdateOrder(Orders order)
        {
            bool result = false;
            try
            {
                //db.Entry(order).State = EntityState.Modified;
                db.Entry(order).Property(c => c.TotalPayment).IsModified = false;
                //db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                result = true;
            }
            catch (Exception error)
            {
                //写入日志
                Common.Tool.LoggerHelper.Info(error.ToString());
            }
            return result;
        }
        /// <summary>
        /// 绑定设备到用户
        /// </summary>
        /// <param name="backcode"></param>
        /// <returns></returns>
        public bool BindMachine(string backcode)
        {
            bool result = false;
            //绑定设备
            Common.Tool.HttpHelper http = new Common.Tool.HttpHelper();
            string block = http.Post("https://api.weixin.qq.com/device/compel_bind?access_token=" + Common.Tool.TokenHelper.GetToken(),
                 "{" + string.Format("\"device_id\": \"{0}\",\"openid\":\"{1}\""
                 , backcode, this.CurMemberInfo.WeChatOpenid) + "}");
            if (block.Contains("ok"))
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 解除绑定设备到用户
        /// </summary>
        /// <param name="backcode"></param>
        /// <returns></returns>
        public bool UnBindMachine(string backcode)
        {
            bool result = false;
            //绑定设备
            Common.Tool.HttpHelper http = new Common.Tool.HttpHelper();
            string block = http.Post("https://api.weixin.qq.com/device/compel_unbind?access_token=" + Common.Tool.TokenHelper.GetToken(),
                 "{" + string.Format("\"device_id\": \"{0}\",\"openid\":\"{1}\""
                 , backcode, this.CurMemberInfo.WeChatOpenid) + "}");
            if (block.Contains("ok"))
            {
                result = true;
            }
            return result;
        }

        #endregion
        //protected override bool CheckUser()
        //{
        //    return true;
        //}
    }
    public class CS
    {
        private int SiteId = 0;
        private const string ImageDomain = "c.cnzz.com";
        public CS(int SiteId)
        {
            this.SiteId = SiteId;
        }
        public string TrackPageView()
        {
            return "";
            //HttpRequest request = HttpContext.Current.Request;
            //string scheme = request != null ? request.IsSecureConnection ? "https" : "http" : "http";
            //string referer = request != null && request.UrlReferrer != null && "" != request.UrlReferrer.ToString() ? request.UrlReferrer.ToString() : "";
            //String rnd = new Random().Next(0x7fffffff).ToString();
            //return scheme + "://" + CS.ImageDomain + "/wapstat.php" + "?siteid=" + this.SiteId + "&r=" + HttpUtility.UrlEncode(referer) + "&rnd=" + rnd;
        }
    }
}