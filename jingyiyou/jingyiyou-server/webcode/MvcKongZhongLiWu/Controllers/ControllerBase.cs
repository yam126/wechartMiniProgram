using ncc2019.Common.BLL;
using ncc2019.Common.Tool;
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
        public Common.Model.MemberInfo CurMemberInfo { get { return SessionHelper.CurMemberInfo; } }
        protected ncc2019Entities db = new ncc2019Entities();
        public ControllerBase()
        {
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
                SessionHelper.SetJumpUrl(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                System.Web.HttpContext.Current.Response.Redirect("~/Login");
                return false;
            }
            else
            {
                return true;
            }
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (Common.Tool.UserAgentHelper.IsMobile())
            {
                string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
                if (!url.ToLower().Contains(SettingBLL.WebDomain + "/api/"))
                {
                    url = url.Replace(SettingBLL.WebDomain, SettingBLL.MobileDomain);
                    System.Web.HttpContext.Current.Response.Redirect(url);
                    return;
                }
            }
            if (!CheckUser())
            {
                return;
            }

            base.Initialize(requestContext);

        }
        protected virtual void SupInitialize(System.Web.Routing.RequestContext requestContext)
        {
            //SessionHelper.SetSession("key", "kk");
            base.Initialize(requestContext);
        }

        /// <summary>
        /// 显示提醒消息
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            ViewBag.Message = string.Format("<script>alert('{0}')</script>", message);
            //Response.Write(
            //    string.Format("<script>alert('{0}')</script>", message));
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

        /// <summary>
        /// 获得订单信息--根据前台传递来的加密后的orderid
        /// </summary>
        /// <returns></returns>
        public Orders GetOrder()
        {
            var orderid = Request.QueryString["orderid"];
            if (string.IsNullOrEmpty(orderid))
            {
                orderid = Request.Form["orderid"];
            }
            var order = db.Orders.Find(int.Parse(Common.Tool.DESEncrypt.Decrypt(orderid)));
            ViewBag.orderid = orderid;
            return order;
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

        //public bool UpdateMemberBalance(Members member)
        //{
        //    bool result = false;
        //    try
        //    {
        //        db.Entry(order).State = EntityState.Modified;
        //        db.SaveChanges();
        //        result = true;
        //    }
        //    catch (Exception error)
        //    {
        //        //写入日志
        //        Common.Tool.LoggerHelper.Info(error.ToString());
        //    }
        //    return result;
        //}

        #endregion
    }

    public class ControllerBaseNoCheck : ControllerBase
    {
        protected override bool CheckUser()
        {
            return true;
        }
    }
    public class ControllerAdminBase : ControllerBase
    {
        protected override bool CheckUser()
        {
            if (SessionHelper.CurMemberInfo == null || SessionHelper.CurMemberInfo.UserLevel != (int)Common.Enum.UserLevel.管理员)
            {

                SessionHelper.SetJumpUrl(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                System.Web.HttpContext.Current.Response.Redirect("/adminlogin");
                return false;
            }
            else
            {
                #region 如果是吹风机用户则不允许进入空中礼物管理后台
                if (SessionHelper.CurMemberInfo.MemnerID != 1)
                {//将用户为1的设置为特殊用户
                    if (SessionHelper.CurMemberInfo.ISCFJUser == (int)Common.Enum.ShiFouStatus.是)
                    {
                        System.Web.HttpContext.Current.Response.Redirect("/admincfj");
                        return false;
                    }
                }
               
                #endregion
                return true;
            }
        }

    }
    public class ControllerAdminBaseNoCheck : ControllerBase
    {
        protected override bool CheckUser()
        {
            return true;
        }
    }
}