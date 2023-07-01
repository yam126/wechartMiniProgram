using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ncc2019.Common.Tool
{
    public class ParamHelper
    {
        public static int GetOrderID()
        {
            int orderid = 0;
            string orderidStr = System.Web.HttpContext.Current.Request.QueryString["Orderid"];
            return orderid;
        }
    }
}