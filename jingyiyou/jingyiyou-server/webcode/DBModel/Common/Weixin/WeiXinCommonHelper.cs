using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ncc2019.Common.Tool;
using System.Xml;
using System.IO;
using ncc2019.Common.Model;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace ncc2019.Common.Weixin
{
    public class WeiXinCommonHelper
    {
        public static string GetPublicKey_Back()
        {
            FileStream fs = File.Open(System.Web.HttpContext.Current.Server.MapPath("/key/publickey.pem"), FileMode.Open, FileAccess.Read, FileShare.Delete);
            StreamReader reader = new StreamReader(fs);
            string block = reader.ReadToEnd();
            return block;

            string url = "https://fraud.mch.weixin.qq.com/risk/getpublickey";

            var dic = new Dictionary<string, string>
            {

                {"mch_id", ncc2019.Common.BLL.SettingBLL.TenPayV3_MchId},
                {"nonce_str", GetRandomString(20)/*Random.Next().ToString()*/}


            };
            //加入签名  
            dic.Add("sign", GetSignString(dic));
            var sb = new StringBuilder();
            sb.Append("<xml>");


            foreach (var d in dic)
            {
                sb.Append("<" + d.Key + ">" + d.Value + "</" + d.Key + ">");
            }
            sb.Append("</xml>");
            var xml = new XmlDocument();

            HttpHelper helper = new HttpHelper();
            string result = helper.Post(url, sb.ToString());
            //LoggerHelper.Debug(result);
            var res = System.Xml.Linq.XDocument.Parse(result);
            if (res.Element("xml").Element("result_code") != null
                && res.Element("xml").Element("result_code").Value == "SUCCESS")
            {
                string pub_key = res.Element("xml").Element("pub_key").Value;
                return pub_key;
            }
            return "";
        }
        /// <summary>  
        /// 从字符串里随机得到，规定个数的字符串.  
        /// </summary>  
        /// <param name="allChar"></param>  
        /// <param name="CodeCount"></param>  
        /// <returns></returns>  
        public static string GetRandomString(int CodeCount)
        {
            string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(allCharArray.Length - 1);
                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }
                temp = t;
                RandomCode += allCharArray[t];
            }

            return RandomCode;
        }
        public static string GetSignString(Dictionary<string, string> dic)
        {
            string key = ncc2019.Common.BLL.SettingBLL.TenPayV3_Key;//商户平台 API安全里面设置的KEY  32位长度  
                                                                    //排序  
            dic = dic.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);
            //连接字段  
            var sign = dic.Aggregate("", (current, d) => current + (d.Key + "=" + d.Value + "&"));
            sign += "key=" + key;
            //MD5  
            // sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sign, "MD5").ToUpper();  
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            sign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sign))).Replace("-", null);
            return sign;
        }

        private static string GetPublicKey()
        {

            SortedDictionary<string, object> di = new SortedDictionary<string, object>();
            di.Add("mch_id", Config.merchant_no);
            di.Add("nonce_str", WeChatUtil.getNoncestr());
            di.Add("sign_type", "MD5");
            di.Add("sign", WeChatUtil.CreateSign(di, Config.pay_secret));
            var data = WeChatUtil.WeChatSignXml(di);
            string result = WeChatUtil.WxCerHttpPost("https://fraud.mch.weixin.qq.com/risk/getpublickey", data, Config.WeChatCre, Config.merchant_no);
            if (!string.IsNullOrWhiteSpace(result))
            {
                //Log4Helper.LogInfo("PayLogger", result);
                var returnXML = XmlUtil.XmlToObect<PublicKeyModel>(result);
                if (returnXML.return_code == "SUCCESS")
                {
                    if (returnXML.result_code == "SUCCESS")
                    {
                        return returnXML.pub_key;
                    }
                }
            }
            return "";
        }

        private static string RSAEncrypt(string EncryptString)
        {
            if (!File.Exists(Config.PubKey))
            {
                var PublicKey = GetPublicKey();
                if (!string.IsNullOrWhiteSpace(PublicKey))
                {
                    File.WriteAllText(Config.PubKey, PublicKey);
                }
                else
                    return "获取公钥失败！";
            }
            string R;
            // 加载公钥
            RsaKeyParameters pubkey;
            using (var sr = new StreamReader(Config.PubKey))
            {
                var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(sr);
                pubkey = (RsaKeyParameters)pemReader.ReadObject();
            }

            // 初始化cipher
            var cipher = (BufferedAsymmetricBlockCipher)CipherUtilities.GetCipher("RSA/ECB/OAEPWITHSHA-1ANDMGF1PADDING");
            cipher.Init(true, pubkey);

            // 加密message
            var message = Encoding.UTF8.GetBytes(EncryptString);
            var output = EncryptUtil.Encrypt(message, cipher);
            R = Convert.ToBase64String(output);
            return R;
        }

        public static string PayToUser(PayToUser pay)
        {
            /*加密银行卡号和用户姓名*/
            pay.enc_bank_no = RSAEncrypt(pay.enc_bank_no);
            pay.enc_true_name = RSAEncrypt(pay.enc_true_name);
            /*1.生成签名*/
            SortedDictionary<string, object> dics = new SortedDictionary<string, object>();
            dics.Add("mch_id", Config.merchant_no);
            dics.Add("partner_trade_no", pay.partner_trade_no);
            dics.Add("nonce_str", WeChatUtil.getNoncestr());
            dics.Add("enc_bank_no", pay.enc_bank_no);
            dics.Add("enc_true_name", pay.enc_true_name);
            dics.Add("bank_code", pay.bank_code);
            dics.Add("amount", pay.amount * 100);
            dics.Add("sign", WeChatUtil.CreateSign(dics, Config.pay_secret));
            /*2.生成xml数据*/
            var sb = WeChatUtil.WeChatSignXml(dics);
            /*开始请求接口*/
            try
            {
                //Log4Helper.LogInfo("PayLogger", sb);
                string xmlStr = WeChatUtil.WxCerHttpPost("https://api.mch.weixin.qq.com/mmpaysptrans/pay_bank", sb, Config.WeChatCre, Config.merchant_no);
                if (!string.IsNullOrWhiteSpace(xmlStr))
                {
                    //Log4Helper.LogInfo("PayLogger", xmlStr);
                    var returnXML = XmlUtil.XmlToObect<PayToUserResult>(xmlStr);
                    if (returnXML.return_code == "SUCCESS")
                    {
                        if (returnXML.result_code == "SUCCESS")
                        {
                            return "ok";
                        }
                    }
                    return returnXML.err_code_des;
                }
            }
            catch (Exception ex)
            {
                //Log4Helper.LogInfo("PayLogger", ex.Message, ex);
            }
            return "内部错误";
        }
    }
}
