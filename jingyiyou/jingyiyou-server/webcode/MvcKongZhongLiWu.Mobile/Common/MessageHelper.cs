using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using ncc2019.Common.Tool;
using Newtonsoft.Json.Linq;
using Submail.AppConfig;
using Submail.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ncc2019.Common
{
    public class MessageHelper
    {
        public static void XCXMessage(string message, string towid, string formid)
        {
            Common.Tool.HttpHelper http = new Tool.HttpHelper();
            string token = TokenHelper.GetXCXToken();
            string content = string.Format("{{\"touser\": \"{0}\",  \"template_id\": \"{1}\", \"form_id\": \"{2}\",\"data\":"
            + "{{ \"keyword1\": {{\"value\": \"{3}\", \"color\": \"#173177\"}}, \"keyword2\": {{ \"value\": \"{4}\", \"color\": \"#173177\"}}}},"
            + " \"emphasis_keyword\": \"keyword1.DATA\"}}"
            , towid, "QXoU3jVuZc1tZg3ZtiroKhUhxYHA2VRztcgULXO5-1w",
            formid, message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            string block = http.PostEx("https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + token, content);
            LoggerHelper.Debug(content);
            LoggerHelper.Debug(block);
        }

        public static void SendSMS(string message, string phone, string carno)
        {
            string msg = "";
            String product = "Dysmsapi";//短信API产品名称
            String domain = "dysmsapi.aliyuncs.com";//短信API产品域名
            String accessId = "LTAI335G1NBz8FfP";
            String accessSecret = "Hb8bhETomUEiqPI28E4Yw3tD7IPaJi";
            String regionIdForPop = "cn-hangzhou";
            IClientProfile profile = DefaultProfile.GetProfile(regionIdForPop, accessId, accessSecret);
            DefaultProfile.AddEndpoint(regionIdForPop, regionIdForPop, product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            request.PhoneNumbers = phone;
            request.SignName = "飞鱼小区";
            request.TemplateCode = "SMS_107930124";
            request.TemplateParam = "{\"carno\":\"" + carno + "\"}";
            request.OutId = "";
            //请求失败这里会抛ClientException异常
            SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
            //System.Console.WriteLine(sendSmsResponse.Message);
            if (sendSmsResponse.Message.ToLower() == "ok")
            {

            }
            else
            {

            }
            LoggerHelper.Info("send sms:" + sendSmsResponse.Message.ToLower());
        }

        public static string SendVerifCodeSMSAli(string phone, string code)
        {
            string msg = "";
            String product = "Dysmsapi";//短信API产品名称
            String domain = "dysmsapi.aliyuncs.com";//短信API产品域名
            String accessId = "LTAImuwWWfFxqUWT";
            String accessSecret = "N1GkzfkIPffbHSINjYTSdCx8ErRtWO";
            String regionIdForPop = "cn-hangzhou";
            IClientProfile profile = DefaultProfile.GetProfile(regionIdForPop, accessId, accessSecret);
            DefaultProfile.AddEndpoint(regionIdForPop, regionIdForPop, product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            request.PhoneNumbers = phone;
            request.SignName = "阿里云短信测试专用";
            request.TemplateCode = "SMS_108385096";
            request.TemplateParam = "{\"code\":\"" + code + "\"}";
            request.OutId = "";
            //请求失败这里会抛ClientException异常
            SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
            //System.Console.WriteLine(sendSmsResponse.Message);
            if (sendSmsResponse.Message.ToLower() == "ok")
            {
                msg = "ok";

            }
            else
            {
                msg = "error";
            }
            LoggerHelper.Info("send sms:" + sendSmsResponse.Message.ToLower());
            return msg;
        }
        /// <summary>
        /// https://www.mysubmail.com/  18600527610 killer007
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string SendVerifCodeSMS(string phone, string code)
        {
            string msg = "";
            IAppConfig appConfig = new MessageConfig("24779", "42896bb940eae15aded3411599287b1d", SignType.md5);
            MessageSend messageSend = new MessageSend(appConfig);
            messageSend.AddTo(phone);
            messageSend.AddContent("【新彩Club】验证码" + code + "，您正在登录，若非本人操作，请勿泄露。");
            //messageSend.AddTag("新彩Club");
            string returnMessage = string.Empty;
            messageSend.Send(out returnMessage);
            JObject result = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(returnMessage);
            //System.Console.WriteLine(sendSmsResponse.Message);
            if (result["status"].ToString() == "success")
            {
                msg = "ok";

            }
            else
            {
                msg = "error";
            }
            LoggerHelper.Info("send sms:" + result["send_id"].ToString());
            return msg;
        }
    }
}