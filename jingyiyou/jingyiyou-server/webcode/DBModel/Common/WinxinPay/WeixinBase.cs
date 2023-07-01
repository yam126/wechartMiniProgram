using ncc2019.Common.Weixin.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.WinxinPay
{
    public class WeixinBase
    {

        /// <summary>  
        /// DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time"> DateTime时间格式</param>  
        /// <returns>Unix时间戳格式</returns>  
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        /// <summary>
        /// sha工具
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SHA1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        public static string GetRandomStr()
        {
            string randomstr = new Guid().ToString().Replace("-", "").ToLower();
            return randomstr;
        }

        public static string MD5PayStr(string appId, string timeStamp, string nonceStr, string package, ref string signType)
        {
            ArrayList AL = new ArrayList();
            AL.Add(appId);
            AL.Add(timeStamp);
            AL.Add(nonceStr);
            AL.Add(package);
            AL.Add(signType);
            AL.Sort(new WXBizMsgCrypt.DictionarySort());
            string raw = "";
            for (int i = 0; i < AL.Count; ++i)
            {
                raw += AL[i];
            }
           
            
            string paySign = "";
            try
            {
                byte[] result = Encoding.Default.GetBytes(raw);    //tbPass为输入密码的文本框
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(result);
                paySign = BitConverter.ToString(output).Replace("-", "");  //tbMd5pass为输出加密文本的文本框
            }
            catch (Exception)
            {
                //return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ComputeSignature_Error;
            }
            return paySign;
        }
    }

}
