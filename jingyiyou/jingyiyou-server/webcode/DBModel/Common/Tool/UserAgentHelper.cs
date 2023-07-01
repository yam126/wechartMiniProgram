using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.Tool
{
    public class UserAgentHelper
    {
        /// <summary>
        /// 根据 User Agent 获取操作系统名称
        /// </summary>
        public static string GetOSNameByUserAgent(string userAgent)
        {
            string osVersion = "未知";

            if (userAgent.Contains("NT 6.3"))
            {
                osVersion = "Windows 10";
            }
            if (userAgent.Contains("NT 6.1"))
            {
                osVersion = "Windows Server 2008 R2";
            }
            else if (userAgent.Contains("NT 6.0"))
            {
                osVersion = "Windows Vista/Server 2008";
            }
            else if (userAgent.Contains("NT 5.2"))
            {
                osVersion = "Windows Server 2003";
            }
            else if (userAgent.Contains("iPhone"))
            {
                osVersion = "iPhone";
            }
            else if (userAgent.Contains("Android"))
            {
                osVersion = "Android";
            }
            else if (userAgent.Contains("NT 5.1"))
            {
                osVersion = "Windows XP";
            }
            else if (userAgent.Contains("NT 5"))
            {
                osVersion = "Windows 2000";
            }
            else if (userAgent.Contains("NT 4"))
            {
                osVersion = "Windows NT4";
            }
            else if (userAgent.Contains("Me"))
            {
                osVersion = "Windows Me";
            }
            else if (userAgent.Contains("98"))
            {
                osVersion = "Windows 98";
            }
            else if (userAgent.Contains("95"))
            {
                osVersion = "Windows 95";
            }
            else if (userAgent.Contains("Mac"))
            {
                osVersion = "Mac";
            }
            else if (userAgent.Contains("Unix"))
            {
                osVersion = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                osVersion = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                osVersion = "SunOS";
            }
            if (userAgent.Contains("MicroMessenger"))
            {
                osVersion += " 微信";
            }

            //iPhone  Android
            return osVersion;
        }

        public static bool IsWeb(string userAgent)
        {
            string os = GetOSNameByUserAgent(userAgent);
            if (os.Contains("iPhone") || os.Contains("Android"))
            {
                return false;
            }
            return true;
        }

        public static bool IsWeb()
        {
            string userAgent = System.Web.HttpContext.Current.Request.UserAgent;
            string os = GetOSNameByUserAgent(userAgent);
            if (os.Contains("iPhone") || os.Contains("Android"))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 判断当前的请求是否为微信发出
        /// </summary>
        /// <returns></returns>
        public static bool IsWeiXin()
        {
            string userAgent = System.Web.HttpContext.Current.Request.UserAgent;
            if (userAgent.IndexOf("MicroMessenger") > -1)
            {
                return true;
            }
            return false;
        }

        public static bool IsMobile()
        {
            string userAgent = System.Web.HttpContext.Current.Request.UserAgent;
            string os = GetOSNameByUserAgent(userAgent);
           
            if (os.Contains("iPhone") || os.Contains("Android"))
            {
                return true;
            }
            return false;
        }
        public static bool IsIhpnoe()
        {
            string userAgent = System.Web.HttpContext.Current.Request.UserAgent;
            string os = GetOSNameByUserAgent(userAgent);

            if (os.Contains("iPhone"))
            {
                return true;
            }
            return false;
        }
    }
}
