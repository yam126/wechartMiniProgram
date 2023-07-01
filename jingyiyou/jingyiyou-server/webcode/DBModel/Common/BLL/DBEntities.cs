using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.BLL
{
    public class DBEntities
    {
        private static ncc2019Entities db = new ncc2019Entities();
        public static ncc2019Entities GetEntities()
        {
            
            return db;
        }

        public static ncc2019Entities GetEntitiesNew()
        {

            return new ncc2019Entities();
        }
    }
}
