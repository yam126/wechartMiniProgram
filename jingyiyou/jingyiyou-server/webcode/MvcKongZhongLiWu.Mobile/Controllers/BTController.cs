using ncc2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class BTController : Controller
    {
        //
        // GET: /BT/

        public ActionResult Index()
        {
            ViewBag.openid = "ddd";
            ViewBag.js_json = TenPayManager.MakeUpJsParam();//jsconfig 参数
            return View();
        }
        public string init()
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;            
            //response.Headers.Clear();
            response.Headers.Set("value", "#400&300&8&215&170*");//输出最高电压&输出最低电压&输出电流&输入最高电压&输入最低电压
            //response.End();



            return "00";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hbat">高压端电压</param>
        /// <param name="lbat">低压端电压</param>
        /// <returns></returns>
        public string command(string hbat,string lbat)
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;

           
            //response.Headers.Clear();
            response.Headers.Set("value", "#1*");//1 放电；2 充电；0 停止
            //response.End();



            return "00";
        }

    }
}
