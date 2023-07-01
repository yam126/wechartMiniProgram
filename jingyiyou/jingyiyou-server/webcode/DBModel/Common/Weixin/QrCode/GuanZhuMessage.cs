using ncc2019.Common.Tool;
using ncc2019.Common.Weixin.Base;
//using Senparc.Weixin.MP.AdvancedAPIs;
//using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using com.sf.openapi.security.sample.dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace ncc2019.Common.Weixin.QrCode
{
    public class GuanZhuMessage : MessageBase
    {
        public string Ticket { get; set; }
        public static GuanZhuMessage InitMessage(MessageBase message, string xmlStr)
        {
            GuanZhuMessage qrmessage = new GuanZhuMessage();
            try
            {
                LoggerHelper.Debug(xmlStr);
                StringReader sr = new StringReader(xmlStr);
                XmlReader reader = XmlReader.Create(sr);
                var doc = XDocument.Load(reader);
                qrmessage = (from c in doc.Descendants("xml")
                             select new GuanZhuMessage()
                             {
                                 Ticket = c.Element("Ticket") == null ? "" : c.Element("Ticket").Value
                             }).First();

                sr.Close();
                reader.Close();

                message.MakeMessage(qrmessage);
            }
            catch (Exception error)
            {

                Common.Tool.LoggerHelper.Debug(error.ToString());
            }

            return qrmessage;
        }

        public void DoMessage()
        {

            try
            {
                if (this.Event == "subscribe")
                {
                    Create();
                }
                else if (this.Event == "unsubscribe")
                {
                    Cancel();
                }
            }
            catch (Exception error)
            {

                Common.Tool.LoggerHelper.Debug(error.ToString());
            }




        }

        private void Create()
        {

            var member = db.Members.Where(c => c.WechatOpenid == this.FromUserName).FirstOrDefault();
            var userInfo = OAuthApi.GetUserInfo_Subscribe(Common.Tool.TokenHelper.GetToken(), this.FromUserName);
            userInfo.nickname = Common.Tool.CommonHelper.StripHTML(userInfo.nickname);
            string guanzhuTip = "";//System.Configuration.ConfigurationManager.AppSettings["guanzhutip"];
            //guanzhuTip = "艾玛可来了，\n赶紧免费抽大奖吧！\r\n<a href='http://m.kongzhongliwu.com/prize?f=guanzhu'>戳我抽大奖~~</a>\r\n是不是该拼拼人气，让小伙伴们给你众筹礼物啦？\r\n<a href='http://m.kongzhongliwu.com/zhongchou/?f=guanzhu'>戳我筹礼物~~</a>\r\n更多创新的礼物交互方式，等你去发现......";
            guanzhuTip = "智裹裹在你家门口为你搭建私人菜鸟驿站！\r\n<a href='http://zgg.renxingpao.com/zhongchou/zhiguoguo'>查看详情~~</a>";
            CustomHelper.SendText(this.FromUserName, guanzhuTip);
            LoggerHelper.Debug(userInfo.nickname);
            if (member == null)
            {
                member = new Members()
                {
                    WechatOpenid = this.FromUserName,
                    LoginCount = 1,
                    Status = (int)Common.Enum.MemmberStatus.正常,
                    RegDate = DateTime.Now,
                    LastDate = DateTime.Now,
                    Balance = 0,
                    Balance_back = 0,
                    LoginName = this.FromUserName,
                    Email = this.FromUserName,
                    IsGuanZhu = 1,
                    Sex = userInfo.sex,
                    HeadImgUrl = userInfo.headimgurl,
                    Country = userInfo.country,
                    City = userInfo.city,
                    Province = userInfo.province,
                    NiceName = userInfo.nickname,
                    Name = userInfo.nickname,
                    UserLevel = (int)Common.Enum.UserLevel.普通账户
                };
                //LoggerHelper.Debug(this.EventKey);
                if (this.EventKey.Contains("qrscene_M-"))
                {
                    string main_memberid = this.EventKey.Replace("qrscene_M-", "");
                    //LoggerHelper.Debug(main_memberid);
                    member.RefMemberID = int.Parse(main_memberid);
                    //LoggerHelper.Debug(member.RefMemberID);
                }
                db.Members.Add(member);
                db.SaveChanges();
               
                ////刷以前的冗余信息
                //db.Database.ExecuteSqlCommand("update ZhongChouPay set HeadImgUrl='{0}',Name='{1}' where MemberID={2} "
                //    , member.HeadImgUrl, member.Name, member.MemberID);
                //new HttpHelper().Get("http://" + Common.BLL.SettingBLL.MobileDomain + "/login/reflush?mid=" + member.MemberID);

            }
            else if (member.IsGuanZhu != 1)
            {//如果没有关注则拉取信息
                member.IsGuanZhu = 1;
                if (string.IsNullOrEmpty(member.Name))
                {
                    member.Name = userInfo.nickname;
                }
                if (string.IsNullOrEmpty(member.NiceName))
                {
                    member.NiceName = userInfo.nickname;
                }
                member.Sex = userInfo.sex;
                member.Country = userInfo.country;
                member.City = userInfo.city;
                member.Province = userInfo.province;
                member.HeadImgUrl = userInfo.headimgurl;

                //db.Entry(member).State = EntityState.Modified;
                db.Entry(member).Property("Balance").IsModified = false;
                db.Entry(member).Property("Balance_back").IsModified = false;

                db.SaveChanges();
            }
        }

        private void Cancel()
        {
            var member = db.Members.Where(c => c.WechatOpenid == this.FromUserName).FirstOrDefault();
            if (member != null)
            {
                member.IsGuanZhu = 2;

                //db.Entry(member).State = EntityState.Modified;
                db.Entry(member).Property("Balance").IsModified = false;
                db.Entry(member).Property("Balance_back").IsModified = false;

                db.SaveChanges();
            }
        }
    }
}
