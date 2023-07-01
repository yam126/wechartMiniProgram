using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ncc2019.Model
{
    public class AddOrderParm
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public int MemberID { get; set; }

        /// <summary>
        /// 导游ID
        /// </summary>
        public int GuideId { get; set; }

        /// <summary>
        /// 讲解景点
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 讲解时长
        /// </summary>
        public int ServiceMinute { get; set; }

        /// <summary>
        /// 客户付款金额
        /// </summary>
        public int Payment { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public int TotalPayment { get; set; }
    }
}