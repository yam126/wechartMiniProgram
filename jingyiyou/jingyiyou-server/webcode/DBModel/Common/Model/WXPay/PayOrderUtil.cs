using System;

namespace ncc2019.Common.Model
{
    public class PayOrderUtil
    {
        public static PayToUser GetPayToUser()
        {
            PayToUser user = new PayToUser();
            user.partner_trade_no = "JZ" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000000, 9999999);
            user.amount = 100;
            user.bank_code = "1002";//银行卡编号
            user.enc_bank_no = "6212263602071851026";//银行卡号
            user.enc_true_name = "李岳武";
            return user;
        }
    }
}

