using System.ComponentModel;
using System.Xml.Serialization;

namespace ncc2019.Common.Model
{
    [XmlType(TypeName = "xml")]
    public class PublicKeyModel
    {
        public string return_code { get; set; }

        public string return_msg { get; set; }


        public string result_code { get; set; }

        public PublicKeyCode err_code { get; set; }

        public string err_code_des { get; set; }

        public string mch_id { get; set; }

        public string pub_key { get; set; }
    }

    public enum PublicKeyCode
    {
        [Description("签名错误")]
        SIGNERROR,
        [Description("系统繁忙，请稍后重试")]
        SYSTEMERROR
    }
}
