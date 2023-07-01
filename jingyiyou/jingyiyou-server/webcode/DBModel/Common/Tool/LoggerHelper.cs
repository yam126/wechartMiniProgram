using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.Tool
{
    public class LoggerHelper
    {
        private static NLog.Logger log = NLog.LogManager.GetLogger("n");

        public static void Info(string content)
        {
            log.Info(content);
        }

        public static void Debug(string content)
        {
            log.Debug(content);
        }


        public static void BigError(string content)
        {
            log.Debug("严重错误："+content);
        }

    }
}
