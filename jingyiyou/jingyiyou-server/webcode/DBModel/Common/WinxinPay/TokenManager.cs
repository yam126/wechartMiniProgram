using ncc2019.Common.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.WinxinPay
{
    public class TokenManager
    {
        private static DateTime LastTime;
        private static tokenObj token;
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            if (DateTime.Now.Subtract(LastTime) > new TimeSpan(1, 0, 0) || token == null)
            {
                string AppID = ncc2019.Common.BLL.SettingBLL.AppID;
                string AppSecret = ncc2019.Common.BLL.SettingBLL.AppSecret;
                HttpHelper http = new HttpHelper();
                //获取token --appid、secret  为每个微信账号所特有的固定密匙
                string tokenStr = http.Get(
                    string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppID, AppSecret));
                //解析token
                token = Newtonsoft.Json.JsonConvert.DeserializeObject<tokenObj>(tokenStr);

                //更新最后刷新时间
                LastTime = DateTime.Now;
            }
            return token.access_token;
        }
    }
    class tokenObj
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }
}
