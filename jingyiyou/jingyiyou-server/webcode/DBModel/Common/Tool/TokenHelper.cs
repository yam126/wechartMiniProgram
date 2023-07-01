using ncc2019.Common.BLL;
using ncc2019.Common.WinxinPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.Tool
{
    public class TokenHelper
    {
        public static string GetToken()
        {
            HttpHelper http = new HttpHelper();
            LoggerHelper.Debug("TokenHelper:" + SettingBLL.WebDomain);
            string tokenStr = http.Get("http://" + SettingBLL.WebDomain + "/API/GetToken");
            return tokenStr;
        }

        public static string GetJsTicket()
        {
            HttpHelper http = new HttpHelper();
            string ticketStr = http.Get("http://" + SettingBLL.WebDomain + "/API/GetJsTicket");
            return ticketStr;
        }

        private static string strToken = "";
        private static string strTicket = "";
        private static DateTime curTime_token = DateTime.MinValue;
        private static DateTime curTime_ticket = DateTime.MinValue;
        public static string GetXCXToken()
        {
            if (curTime_token < DateTime.Now.Subtract(new TimeSpan(1, 0, 0)) || strToken == "")
            {
                try
                {

                    HttpHelper http = new HttpHelper();
                    string tokenStr = http.Get(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}"
                         , "wx799e0ba77ae49b9b", "c12a560b1a3cdf8fbf258a05092be569"));
                    
                    tokenObj token = Newtonsoft.Json.JsonConvert.DeserializeObject<tokenObj>(tokenStr);
                    strToken = token.access_token;
                    curTime_token = DateTime.Now;
                    LoggerHelper.Info("xcxtoken:"+strToken);
                }
                catch (Exception error)
                {

                    throw error;
                }

            }
            return strToken;
        }

    }
}
