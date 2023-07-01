using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcKongZhongLiWu.Common.Enum
{
    public enum MemmberStatus
    {
        正常 = 1,
        禁用 = 2
    }
    public enum PayStatus
    {
        已支付 = 1,
        未支付 = 2
    }
    public enum OrderStatus
    {
        正常 = 1,
        退款 = 2
    }
    public enum TransferStatus
    {
        未发货 = 1,
        已发货 = 2,
        已签收 = 3
    }

    public class EnumTool
    {
        public static string GetEnumName(Type type, int? value)
        {
            return System.Enum.GetName(type, value);
        }
    }
}