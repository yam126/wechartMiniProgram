using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcKongZhongLiWu;
using MvcKongZhongLiWu.Common.Enum;
using MvcKongZhongLiWu.Common.BLL;
using MvcKongZhongLiWu.Common.Weixin.Base;
using MvcKongZhongLiWu.Common.Tool;

namespace Common
{
    public class Execute : ExeBase
    {

        public static void Run()
        {
            //try
            //{
            //    var order = db.Orders.Find(1593);
            //    //oUxxctyrXkPZtpN_ycsAId8_coyg
            //    CustomHelper.SendZhongChouOKUseTemplate(order.Members.WechatOpenid, order);
            //    LoggerHelper.Debug("ZhongChouSuccessNotify--" + order.Members.WechatOpenid);
            //}
            //catch (Exception error)
            //{

            //    LoggerHelper.Debug(error.ToString());
            //}


            while (true)
            {
                LoggerHelper.Debug("RunStart At " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Do();
                Console.WriteLine("RunEnd");
                Console.WriteLine("Sleeping");
                System.Threading.Thread.Sleep(30000);

            }
        }
        /// <summary>
        /// 执行
        /// </summary>
        public static void Do()
        {
            try
            {
               
                AddGoodNum();
            }
            catch (Exception error)
            {
                LoggerHelper.Debug(error.ToString());

            }

        }

        public static void AddGoodNum()
        {
            if (DateTime.Now.Hour % 3 == 0)
            {

            }

        }

       
    }
}
