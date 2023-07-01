using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.Tool
{
    public class SignatureHelper
    {
        ///*对所有参数名称和参数值做URL编码*/
        //public static List<String> getAllParams(Dictionary<String, String> publicParams, Dictionary<String, String> privateParams)
        //{
        //    publicParams = publicParams.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);
        //    privateParams = privateParams.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);
        //    List<String> encodeParams = new List<String>();
        //    if (publicParams != null)
        //    {
        //        foreach (String key in publicParams.Keys)
        //        {
        //            String value = publicParams[key];
        //            //将参数和值都urlEncode一下。
        //            String encodeKey = System.Web.HttpUtility.UrlEncode(key);
        //            String encodeVal = System.Web.HttpUtility.UrlEncode(value);
        //            encodeParams.Add(encodeKey + "=" + encodeVal);
        //        }
        //    }
        //    if (privateParams != null)
        //    {
        //        foreach (String key in privateParams.Keys)
        //        {
        //            String value = publicParams[key];
        //            //将参数和值都urlEncode一下。
        //            String encodeKey = System.Web.HttpUtility.UrlEncode(key);
        //            String encodeVal = System.Web.HttpUtility.UrlEncode(value);
        //            encodeParams.Add(encodeKey + "=" + encodeVal);
        //        }
        //    }
        //    return encodeParams;
        //}
        ///*获取 CanonicalizedQueryString*/
        //public static String getCQS(List<String> allParams)
        //{            
        //    String cqString = "";
        //    for (int i = 0; i < allParams.Count; i++)
        //    {
        //        cqString += allParams[i];
        //        if (i != allParams.Count - 1)
        //        {
        //            cqString += "&";
        //        }
        //    }
        //    return cqString;
        //}

        public static string GetSignString(Dictionary<string, string> dic)
        {

            dic = dic.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);
            //连接字段  
            var sign = dic.Aggregate("", (current, d) => current + (d.Key + "=" + d.Value + "&"));
            sign = sign.TrimEnd('&');
            LoggerHelper.Info("sign=" + sign);
            sign = EncryptToSha1("GET&%2F&" + sign, "2UXRmeqtvsZ8eNeH5w5YyjqXwjTnKt");
            return sign;
        }

        #region 获取由SHA1加密的字符串

        /// <summary>
        /// sha1 加密，与php加密结果一样
        /// </summary>
        /// <param name="str"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string EncryptToSha1(string signatureString, string secretKey)
        {
            return hash_hmac(signatureString, secretKey, true);
        }
        private static string hash_hmac(string signatureString, string secretKey, bool raw_output = false)
        {
            var enc = Encoding.UTF8;
            HMACSHA1 hmac = new HMACSHA1(enc.GetBytes(secretKey));
            hmac.Initialize();

            byte[] buffer = enc.GetBytes(signatureString);
            if (raw_output)
            {
                return Convert.ToBase64String(hmac.ComputeHash(buffer));
            }
            else
            {
                return BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower();
            }
        }
        #endregion
        /// <summary>    
        /// 获取时间戳    
        /// </summary>    
        /// <returns></returns>    
        public static string GetTimeStamp()
        {

            return DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
        }
        public static String generateRandom()
        {
            String signatureNonce = Guid.NewGuid().ToString();
            return signatureNonce;
        }
    }
}
