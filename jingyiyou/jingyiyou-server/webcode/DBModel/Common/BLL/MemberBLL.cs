using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.BLL
{
    public class MemberBLL
    {
        public static int AddTiLi(ncc2019Entities db, int memberid)
        {
            return db.Database.ExecuteSqlCommand(" update members set tilinum=tilinum+1 where memberid={0} ", memberid);
        }

        public static int ReduceTiLi(ncc2019Entities db, int memberid)
        {
            return db.Database.ExecuteSqlCommand(" update members set tilinum=tilinum-1 where memberid={0} ", memberid);
        }
        public static int ReduceTiLi(ncc2019Entities db, int memberid, int tiliNum)
        {
            return db.Database.ExecuteSqlCommand(" update members set tilinum=tilinum-{1} where memberid={0} ", memberid, tiliNum);
        }
        public static string GetPartPhoneNum(string phone)
        {
            if (phone.Length < 11)
            {
                if (phone.Length > 3)
                {
                    return phone.Substring(0, 3) + "***";
                }
                else
                {
                    return phone;
                }

            }
            return phone.Substring(0, 3) + "***" + phone.Substring(7, 4);
        }
    }
}
