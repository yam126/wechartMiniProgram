using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ncc2019.Common.Weixin.Base;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Senparc.Weixin.MP.Entities;
using ncc2019.Common.BLL;

namespace ncc2019.Common.Weixin.QrCode
{
    public class QrCodeMessage : MessageBase
    {
        public string Ticket { get; set; }
        
        public static QrCodeMessage InitMessage(MessageBase message, string xmlStr)
        {

            StringReader sr = new StringReader(xmlStr);
            XmlReader reader = XmlReader.Create(sr);
            var doc = XDocument.Load(reader);
            QrCodeMessage qrmessage = (from c in doc.Descendants("xml")
                                       select new QrCodeMessage()
                                       {
                                           Ticket = c.Element("Ticket") == null ? "" : c.Element("Ticket").Value
                                       }).First();

            sr.Close();
            reader.Close();

            message.MakeMessage(qrmessage);
            return qrmessage;
        }


        private void LinkMember(string param)
        {
            //绑定用户到微信
            string memberid = Common.Tool.DESEncrypt.Decrypt(param);
            var memberModel = db.Members.Find(int.Parse(memberid));
            memberModel.WechatOpenid = this.FromUserName;
            //获取其他信息
            //db.Entry(memberModel).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        private void LinkOrder(string param)
        {

            //绑定订单到微信
            string orderid = Common.Tool.DESEncrypt.Decrypt(param);
            var order = db.Orders.Find(int.Parse(orderid));
            //if (string.IsNullOrEmpty(order.ToWeChatOpenid))
            //{
            //    order.ToWeChatOpenid = this.FromUserName;
            //    var memberModel = db.Members.Where(c => c.WechatOpenid == order.ToWeChatOpenid).FirstOrDefault();
            //    if (memberModel != null)
            //    {
            //        order.ToMemberID = memberModel.MemberID;
            //    }
            //    //db.Entry(order).State = System.Data.EntityState.Modified;
            //    db.SaveChanges();
            //}


            CustomHelper.SendText(this.FromUserName, "恭喜您已经成功将礼物和微信账户绑定，请点击菜单里面的[我的礼物]选项查看更多信息！");

        }
        private void GetGift(string param)
        {

            string orderid = Common.Tool.DESEncrypt.Decrypt(param);
            Orders order = db.Orders.Find(int.Parse(orderid));

            Article article = new Article()
            {
               // Description = order.SayEtc,
                //PicUrl = "http://" + SettingBLL.WebDomain + "/" + order.Goods.ImgUrl,
                Url = "http://" + SettingBLL.MobileDomain + "/gift/" + order.ShortUrl,
                Title = "礼物到啦！"
            };

            ////先发送一个提示语
            //CustomHelper.SendText(this.FromUserName,
            //    string.Format("恭喜您已经成功购买{0}！请长按以下信息2秒，转发给好友即可。", order.Goods.Name));
            //发送礼物链接
            CustomHelper.SendNews(this.FromUserName, article);
        }

        private void WebLogin(string param)
        {
            //TODO:将登陆用户信息写到内存服务器

            //先发送一个提示语
            CustomHelper.SendText(this.FromUserName,
                string.Format("欢迎您来到空中礼物，您通过Web登陆成功！"));
        }
        /// <summary>
        /// 关联代理和用户
        /// </summary>
        /// <param name="param"></param>
        private void AgentMember(string param)
        {
            if (this.EventKey.Contains("qrscene_M-"))
            {
            
                string main_memberid = this.EventKey.Replace("qrscene_M-", "");


            }
        }
    }
}
