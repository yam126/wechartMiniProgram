using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.BLL
{
    public class QuanBLL
    {
        /// <summary>
        /// 代金券是否过期
        /// </summary>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static bool QuanIsExpired(DateTime? enddate)
        {
            if (enddate == null)
            {
                return false;
            }

            DateTime curdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, 23, 59, 59);
            if (enddate > curdate)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
