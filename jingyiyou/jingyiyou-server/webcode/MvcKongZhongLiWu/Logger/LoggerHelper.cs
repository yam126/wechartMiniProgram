using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcKongZhongLiWu
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
    }
}