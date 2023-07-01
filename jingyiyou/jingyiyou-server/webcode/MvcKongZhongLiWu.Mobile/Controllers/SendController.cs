using ncc2019.Common;
using ncc2019.Common.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Enum;

namespace ncc2019.Controllers
{
    public class SendController : ControllerBase
    {
        //
        // GET: /Send/

        public ActionResult Index(string orderid)
        {
            var order = GetOrder();
            //以防分享的时候js没有加载完全就点击了分享按钮
            if (CurMemberInfo.MemnerID != order.MemberID)
            {
                return Redirect("/gift/" + order.ShortUrl);
            }
            if (order.PayLate == (int)ShiFouStatus.否 && order.PayStatus == (int)Common.Enum.PayStatus.未支付)
            {
                return Redirect("/pay/?orderid=" + orderid);
            }
            var member = db.Members.Where(c => c.MemberID == this.CurMemberInfo.MemnerID).FirstOrDefault();
            // ViewBag.guanzhu = false;
            //没有关注的话引导关注
            if (member.IsGuanZhu != 1)
            {
                ViewBag.guanzhu = false;
                ViewBag.imgurl = "http://" + SettingBLL.WebDomain + "/api/GetQrCode?qraction=linkorder&param=" + orderid;
            }
            ViewBag.isopen = false;//如果被好友打开或者绑定过，将不能再进行发送给好友功能
            if (order.ToMemberID != null || order.ToWeChatOpenid != null)
            {
                ViewBag.isopen = true;
            }


            ViewBag.goodurl = "http://" + SettingBLL.WebDomain + "/gift/" + order.ShortUrl;
            ViewBag.js_json = TenPayManager.MakeUpJsParam();//jsconfig 参数
           // ViewBag.goodimgurl = "http://" + SettingBLL.WebDomain + "/" + order.Goods.ImgUrl;

            return View(order);
        }
        public ActionResult OK(string orderid)
        {
            var order = GetOrder();
            ViewBag.orderid = Common.Tool.DESEncrypt.Encrypt(order.OrderID);
            var actionList = db.ActionLog.Where(c => c.OrderID == order.OrderID).ToList();
            var tomemeber = db.Members.Where(c => c.MemberID == order.ToMemberID || c.WechatOpenid == order.ToWeChatOpenid).FirstOrDefault();
            if (tomemeber != null)
            {
                ViewBag.toname = tomemeber.Name;
                ViewBag.touserimgurl = tomemeber.HeadImgUrl;
            }
            var frommember = db.Members.Where(c => c.MemberID == order.MemberID).FirstOrDefault();
            if (frommember != null)
            {
                ViewBag.toname = tomemeber.Name;
                ViewBag.fromuserimgurl = frommember.HeadImgUrl;
            }



            foreach (var action in actionList)
            {
                if (action.AtionType == (int)Common.Enum.AtionType.打开礼物)
                {
                    ViewBag.opengift = action.ActionDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (action.AtionType == (int)Common.Enum.AtionType.填写地址)
                {
                    ViewBag.writeaddress = action.ActionDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (action.AtionType == (int)Common.Enum.AtionType.发送礼物)
                {
                    ViewBag.sendgift = action.ActionDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            ViewBag.js_json = TenPayManager.MakeUpJsParam();//jsconfig 参数
            return View(order);
        }
    }
}
