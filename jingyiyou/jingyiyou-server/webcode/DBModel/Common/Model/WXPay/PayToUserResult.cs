using System.ComponentModel;
using System.Xml.Serialization;

namespace ncc2019.Common.Model
{
    [XmlType(TypeName = "xml")]
    public class PayToUserResult
    {
        public string return_code { get; set; }

        public string return_msg { get; set; }



        public string result_code { get; set; }

        public ErrCode err_code { get; set; }

        public string err_code_des { get; set; }

        public string mch_id { get; set; }

        public string partner_trade_no { get; set; }

        public int amount { get; set; }

        public string nonce_str { get; set; }

        public string sign { get; set; }

        public string payment_no { get; set; }

        public string cmms_amt { get; set; }

    }

    public enum ErrCode
    {
        [Description("无效的请求，商户系统异常导致，商户权限异常、证书错误、频率限制等。使用原单号以及原请求参数重试。")]
        INVALID_REQUEST,
        [Description("业务错误导致交易失败。请先调用查询接口，查询此次付款结果，如结果为不明确状态（如订单号不存在），请务必使用原商户订单号及原请求参数重试")]
        SYSTEMERROR,
        [Description("参数错误，商户系统异常导致。商户检查请求参数是否合法，证书，签名")]
        PARAM_ERROR,
        [Description("签名错误")]
        SIGNERROR,
        [Description("超额；已达到今日付款金额上限或已达到今日银行卡收款金额上限")]
        AMOUNT_LIMIT,
        [Description("受理失败，订单已存在")]
        ORDERPAID,
        [Description("已存在该单，并且订单信息不一致；或订单太老")]
        FATAL_ERROR,
        [Description("账号余额不足")]
        NOTENOUGH,
        [Description("超过每分钟600次的频率限制")]
        FREQUENCY_LIMITED,
        [Description("Wx侧受理成功")]
        SUCCESS
    }

}