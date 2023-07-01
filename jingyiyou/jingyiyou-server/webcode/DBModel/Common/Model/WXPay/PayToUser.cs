
namespace ncc2019.Common.Model
{
    public class PayToUser
    {

        public string partner_trade_no { get; set; }
        public decimal amount { get; set; }

        public string enc_bank_no { get; set; }

        public string enc_true_name { get; set; }

        public string bank_code { get; set; }
    }

}