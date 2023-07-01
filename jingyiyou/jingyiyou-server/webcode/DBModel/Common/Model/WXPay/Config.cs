namespace ncc2019.Common.Model
{
    public class Config
    {
        public static string appid { get; set; } = ncc2019.Common.BLL.SettingBLL.AppID;

        public static string merchant_no { get; set; } = ncc2019.Common.BLL.SettingBLL.TenPayV3_MchId;

        public static string pay_secret { get; set; } = ncc2019.Common.BLL.SettingBLL.TenPayV3_Key;

        public static string PubKey { get; internal set; } = System.Web.HttpContext.Current.Server.MapPath("/key/publickey.pem");//公钥保存路径
        public static string WeChatCre { get; internal set; } = System.Web.HttpContext.Current.Server.MapPath("/key/apiclient_cert.p12"); //证书绝对路径
    }

}