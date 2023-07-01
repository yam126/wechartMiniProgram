using ncc2019.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M = ncc2019;

namespace ncc2019.Common.BLL
{
    public class ZhongChouPayBLL
    {
       
        
        /// <summary>
        /// 获取某个订单的总支付额
        /// </summary>
        /// <param name="db"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static decimal GetTotalPaymentByOrderID(ncc2019Entities db, int orderid)
        {
            decimal totalpayment = db.Database.SqlQuery<decimal>("select  case  WHEN a.s is null  then 0 else a.s end from (SELECT SUM(Payment) s FROM ZhongChouPay where OrderID={0} and State={1} ) a"
                                , orderid, (int)PayStatus.已支付).FirstOrDefault();
            return totalpayment;
        }


        /// <summary>
        /// 获取订单的支付次数
        /// </summary>
        /// <param name="db"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static int GetPayCount(ncc2019Entities db, int orderid)
        {
            int count = db.Database.SqlQuery<int>("SELECT count(*) FROM ZhongChouPay where OrderID={0} and State={1} "
                       , orderid, (int)PayStatus.已支付).FirstOrDefault();

            return count;
        }
    }
}
