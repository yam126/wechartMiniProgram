using ncc2019.Common.BLL;
using ncc2019.Common.Tool;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Custom;
using Senparc.Weixin.MP.AdvancedAPIs.CustomService;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.Weixin.Base
{
    public class CustomHelper
    {
        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="articles"></param>
        /// <returns></returns>
        public static Senparc.Weixin.MP.Entities.WxJsonResult SendNews(string openid, Article articles)
        {
            string accessToken = TokenHelper.GetToken();
            return CustomApi.SendNews(accessToken, openid, new List<Article>() { articles });
        }

        /// <summary>
        /// 发送文本信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Senparc.Weixin.MP.Entities.WxJsonResult SendText(string openId, string content)
        {
            string accessToken = TokenHelper.GetToken();
            try
            {
                return CustomApi.SendText(accessToken, openId, content);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());
                //如果发生不成功则说明可能很久没有互动或者取消关注了
                //return GroupMessageApi.SendTextGroupMessageByOpenId(accessToken, content, new string[] { openId });

            }
            return null;

        }
        /// <summary>
        /// 未支付提醒通知
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="orderModel"></param>
        public static void SendUnpayAlertUseTemplate(string openId, Orders orderModel)
        {
            try
            {
                string accessToken = TokenHelper.GetToken();
                string url = "http://" + SettingBLL.WebDomain + "/pay/my?orderid=" + Common.Tool.DESEncrypt.Encrypt(orderModel.OrderID);

                var testData = new UnPayAlert()
                {
                    first = new TemplateDataItem("您的好友已经选择了礼物！"),
                    type = new TemplateDataItem("礼物", "#000"),
                    //e_title = new TemplateDataItem(orderModel.Goods.Name),
                    o_id = new TemplateDataItem(orderModel.OrderID.ToString()),
                    o_money = new TemplateDataItem("人民币：" + orderModel.TotalPayment.Value.ToString() + "元"),
                    order_date = new TemplateDataItem(orderModel.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss")),
                    remark = new TemplateDataItem("付款后我们将开始为Ta进行配送，点击完成付款！", "#FF6633")
                };

                TemplateApi.SendTemplateMessage(accessToken, openId
                        //, "1-ncxOo-U04HEA8Hkkz_pLvEnX4iMSHBr-bd54mxD1w"
                        , System.Configuration.ConfigurationManager.AppSettings["unpayAlertTemplateid"]
                        , "#FF0000", url, testData);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }
        }
        /// <summary>
        /// 差额退款通知
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="orderModel"></param>
        public static void SendPaymentBackUseTemplate(string openId, Orders orderModel, decimal paymentback)
        {
            try
            {
                string accessToken = TokenHelper.GetToken();
                string url = "http://" + SettingBLL.WebDomain + "/my/account";

                var testData = new PaymentBackAlert()
                {
                   // first = new TemplateDataItem(string.Format("您的好友已经选择了礼物 {0} 价值人民币{1}元！", orderModel.Goods.Name, orderModel.Goods.Payment)),
                    keyword1 = new TemplateDataItem(orderModel.OrderID.ToString()),
                    keyword2 = new TemplateDataItem(paymentback.ToString()),
                    keyword3 = new TemplateDataItem("平台账户"),
                    keyword4 = new TemplateDataItem("立即"),
                    remark = new TemplateDataItem("差额已经到账，点击查看！", "#FF6633")
                };

                TemplateApi.SendTemplateMessage<object>(accessToken, openId
                        //, "1-ncxOo-U04HEA8Hkkz_pLvEnX4iMSHBr-bd54mxD1w"
                        , System.Configuration.ConfigurationManager.AppSettings["paymentbackTemplateid"]
                        , "#FF0000", url, testData);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }
        }

    
       
        /// <summary>
        /// 众筹失败退款
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="orderModel"></param>
        public static void SendZhongChouFailUseTemplate(string openId, Orders orderModel)
        {
            try
            {
                string accessToken = TokenHelper.GetToken();
                string url = "http://" + SettingBLL.MobileDomain + "/my/account/";


                var testData = new CommonAlertTemplate()
                {
                    first = new TemplateDataItem(string.Format("您参与的众筹项目，众筹失败，您支持的金额已经打到你的个人账户！")),
                   // keyword1 = new TemplateDataItem(orderModel.Goods.Name),
                    keyword2 = new TemplateDataItem(orderModel.EndDate.Value.ToString("yyyy-MM-dd")),
                    keyword3 = new TemplateDataItem("无"),
                    remark = new TemplateDataItem("点击查看余额！", "#FF6633")
                };

                TemplateApi.SendTemplateMessage<object>(accessToken, openId
                        , "N4PE3qAYYwnGrdr1nEHCG0InrZ1iPbJAg9O9T6ZTN_8"
                        , "#FF0000", url, testData);
            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }
        }
        /// <summary>
        /// 每日发送众筹通知
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="orderModel"></param>
        public static void SendZhongChouNotifyUseTemplate(string openId, Orders orderModel)
        {
            try
            {
                string accessToken = TokenHelper.GetToken();
                string url = "http://" + SettingBLL.MobileDomain + "/zhongchou/sendok?orderid=" + DESEncrypt.Encrypt(orderModel.OrderID);

                ncc2019Entities db = new ncc2019Entities();
                var count = ZhongChouPayBLL.GetPayCount(db, orderModel.OrderID);
                decimal totalpay = ZhongChouPayBLL.GetTotalPaymentByOrderID(db, orderModel.OrderID);

                var testData = new CommonAlertTemplate()
                {
                    first = new TemplateDataItem(string.Format("目前已经有{0}人次支持、共筹集{1}元", count, totalpay.ToString("0"))),
                    //keyword1 = new TemplateDataItem(orderModel.Goods.Name + " 众筹"),
                    keyword2 = new TemplateDataItem(orderModel.EndDate.Value.ToString("yyyy-MM-dd")),
                    keyword3 = new TemplateDataItem("无"),
                    remark = new TemplateDataItem("您可以分享到朋友圈更新当前的众筹状态！点击分享。", "#FF6633")
                };

                TemplateApi.SendTemplateMessage<object>(accessToken, openId
                        , "N4PE3qAYYwnGrdr1nEHCG0InrZ1iPbJAg9O9T6ZTN_8"
                        , "#FF0000", url, testData);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());

            }
        }
        /// <summary>
        /// 众筹快到期的提醒信息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="orderModel"></param>
        public static void SendZhongChouDelayAlertUseTemplate(string openId, Orders orderModel)
        {
            try
            {
                string accessToken = TokenHelper.GetToken();
                string url = "http://" + SettingBLL.MobileDomain + "/zhongchou/sendok?orderid=" + DESEncrypt.Encrypt(orderModel.OrderID);

                ncc2019Entities db = new ncc2019Entities();
                //var count = ZhongChouPayBLL.GetPayCount(db, orderModel.OrderID);
                decimal totalpay = ZhongChouPayBLL.GetTotalPaymentByOrderID(db, orderModel.OrderID);

                var endday = new DateTime(orderModel.EndDate.Value.Year, orderModel.EndDate.Value.Month, orderModel.EndDate.Value.Day, 23, 59, 59);
                int days = endday.Subtract(DateTime.Now).Days;
                if (days == 0)
                {
                    days = 1;
                }


                var testData = new CommonAlertTemplate()
                {
                    first = new TemplateDataItem(string.Format("目前仅剩{0}天众筹时间，还差{1}元才能众筹成功", days, (orderModel.TotalPayment.Value - totalpay).ToString("0"))),
                    //keyword1 = new TemplateDataItem(orderModel.Goods.Name + " 众筹"),
                    keyword2 = new TemplateDataItem(orderModel.EndDate.Value.ToString("yyyy-MM-dd")),
                    keyword3 = new TemplateDataItem("无"),
                    remark = new TemplateDataItem("小提示：众筹失败后所有众筹款将会退还到支持者的私人账户，您可以分享到朋友圈得到更多人的支持！点击分享。", "#FF6633")
                };

                TemplateApi.SendTemplateMessage<object>(accessToken, openId
                        , "N4PE3qAYYwnGrdr1nEHCG0InrZ1iPbJAg9O9T6ZTN_8"
                        , "#FF0000", url, testData);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());

            }
        }

        /// <summary>
        /// 发送众筹消息通知
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="orderModel"></param>
        public static void SendZCMessageNotifyUseTemplate(string openId, Comments commentModel)
        {
            try
            {
                string accessToken = TokenHelper.GetToken();
                string url = "http://" + SettingBLL.MobileDomain + "/zhongchou/memberlist?orderid=" + DESEncrypt.Encrypt(commentModel.OrderID.Value);
                ncc2019Entities db = new ncc2019Entities();
                var memberModel = db.Members.Find(commentModel.MemberID);

                var testData = new CommonAlertTemplate()
                {
                    first = new TemplateDataItem(string.Format("您有一条众筹回复")),
                    keyword1 = new TemplateDataItem(memberModel.Name),
                    keyword2 = new TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    keyword3 = new TemplateDataItem(commentModel.Content),
                    remark = new TemplateDataItem("点击查看众筹好友的留言吧！", "#FF6633")
                };

                TemplateApi.SendTemplateMessage<object>(accessToken, openId
                        , "Hsv1Yz3ZPJBXliThCmm2_qfRaqA6dDU4x1qcWiff0HY"
                        , "#FF0000", url, testData);
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());

            }
        }
    }


    public class UnPayAlert
    {
        /// <summary>
        // {{first.DATA}}
        //{{type.DATA}}名称：{{e_title.DATA}}
        //订单编号：{{o_id.DATA}}
        //下单时间：{{order_date.DATA}}
        //订单金额：{{o_money.DATA}}
        //{{remark.DATA}} 
        /// </summary>
        public TemplateDataItem first { get; set; }
        public TemplateDataItem type { get; set; }

        public TemplateDataItem e_title { get; set; }

        public TemplateDataItem o_id { get; set; }

        public TemplateDataItem order_date { get; set; }
        public TemplateDataItem o_money { get; set; }
        public TemplateDataItem remark { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PaymentBackAlert
    {
        /// <summary>
        //{{first.DATA}}
        //订单编号：{{keyword1.DATA}}
        //退款金额：{{keyword2.DATA}}
        //退款方式：{{keyword3.DATA}}
        //到账时间：{{keyword4.DATA}}
        //{{remark.DATA}}
        /// </summary>
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }

        public TemplateDataItem keyword2 { get; set; }

        public TemplateDataItem keyword3 { get; set; }

        public TemplateDataItem keyword4 { get; set; }
        public TemplateDataItem remark { get; set; }
    }
    public class CommonAlertTemplate
    {
        //{{first.DATA}}
        //客户名称：{{keyword1.DATA}}
        //收款单号：{{keyword2.DATA}}
        //收款金额：{{keyword3.DATA}}
        //{{remark.DATA}}
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }

        public TemplateDataItem keyword2 { get; set; }

        public TemplateDataItem keyword3 { get; set; }

        public TemplateDataItem remark { get; set; }
    }
}
