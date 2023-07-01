using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace ncc2019.Common.Model
{
    public class WeChatUtil
    {
        /// <summary>
        /// 生成微信 字典排序的 xml格式字符串
        /// </summary>
        /// <param name="sdo">C#内置对象，默认排序</param>
        /// <returns></returns>
        public static string WeChatSignXml(SortedDictionary<string, object> sdo)
        {
            StringBuilder sXML = new StringBuilder("<xml>");
            foreach (var dr in sdo)
            {
                sXML.AppendFormat("<{0}>{1}</{0}>", dr.Key, dr.Value);
            }
            sXML.Append("</xml>");
            return sXML.ToString();
        }

        /// <summary>
        /// 获取 签名 MD5加密字符串
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string CreateSign(SortedDictionary<string, object> sd, string key)
        {
            Dictionary<string, object> dic = sd.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);
            var sign = dic.Aggregate("", (current, d) => current + (d.Key + "=" + d.Value + "&"));
            sign += "key=" + key;
            return GetMD5(sign, "UTF-8");
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        public static string getNoncestr()
        {
            Random random = new Random();
            return GetMD5(random.Next(1000).ToString(), "UTF-8");
        }

        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="encypStr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        /// <summary>
        /// 微信证书post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string WxCerHttpPost(string url, string param, string path, string key)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            ServicePointManager.ServerCertificateValidationCallback = new
            RemoteCertificateValidationCallback(CheckValidationResult);
            X509Certificate cer = new X509Certificate(path, key);
            request.ClientCertificates.Add(cer);

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }

        private static bool CheckValidationResult(object sender,
    X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //if (errors == SslPolicyErrors.None)
            return true;
            //return false;
        }
    }
}