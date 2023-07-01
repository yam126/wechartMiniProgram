using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = string.Format("'device_type':'{0}','device_id':'{1}','open_id': '{2}','content': '{3}'"
                      , "gh_1ecefda99708","d","d","d");
            Execute.Run();
        }
    }
}
