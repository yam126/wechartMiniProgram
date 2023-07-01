using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Enum;

namespace ncc2019.Controllers
{
    public class VIPController : ControllerBaseVIPNoCheck
    {
        //
        // GET: /VIP/
        [HttpGet]
        public JsonResult SendSMS(string fromPhone, string content)
        {
            string verifyCode = "";
            string result = "error";
            var account = db.Account.Where(c => c.Phone == fromPhone).FirstOrDefault();
            if (account != null)
            {
                //提取验证码
                var channel = db.Channel.Where(c => c.ChannelID == account.ChannelID).FirstOrDefault();
                if (channel != null)
                {
                    if (content.Contains(channel.SMSTag) && content.Contains("验证码"))
                    {
                        verifyCode = GetNum(content);
                    }
                }

                var vipuser = db.VIPUser.Where(c => c.AccountID == account.AccountID
                       && c.SMSState == (int)SMSSendStatus.发送中
                       && c.PayState == (int)PayStatus.已支付).FirstOrDefault();
                if (vipuser != null)
                {
                    try
                    {
                        vipuser.VerifyCode = verifyCode;
                        vipuser.GetVerifyCodeCount = vipuser.GetVerifyCodeCount + 1;
                        vipuser.VerifyContent = content;
                        //vipuser.SMSState = (int)SMSSendStatus.未发送;
                        db.SaveChanges();
                        result = "ok";
                    }
                    catch (Exception)
                    {


                    }

                }
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }

        public string GetNum(string content)
        {

            /**  \\d+\\.?\\d*
            * \d 表示数字
            * + 表示前面的数字有一个或多个（至少出现一次）
            * \. 此处需要注意，. 表示任何原子，此处进行转义，表示单纯的 小数点
            * ? 表示0个或1个
            * * 表示0次或者多次
            */
            Regex r = new Regex("\\d+\\.?\\d*");
            bool ismatch = r.IsMatch(content);
            MatchCollection mc = r.Matches(content);

            string result = string.Empty;
            for (int i = 0; i < mc.Count; i++)
            {
                result += mc[i];//匹配结果是完整的数字，此处可以不做拼接的
            }
            return result;
        }
    }
}
