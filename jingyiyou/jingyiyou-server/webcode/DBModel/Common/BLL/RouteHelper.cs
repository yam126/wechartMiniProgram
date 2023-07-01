using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ncc2019.Common.Tool;
using System.Web.Mvc;
using System.Collections;

namespace ncc2019.Common.BLL
{
    public class RouteHelper
    {
        static Hashtable ht = new Hashtable();
        public static string GetRouteInfo2(string DeliveryNo, string Com)
        {
            string id = "107842";
            string secret = "4bb83c881fbcf133ff3c5249fb3f016a";
            string type = "json";
            string encode = "utf8";
            string ord = "asc";
            string urlStr = "http://api.ickd.cn/?id={0}&secret={1}&com={2}&nu={3}&type={4}&encode={5}&ord={6}&ver=2";
            urlStr = string.Format(urlStr, id, secret, Com, DeliveryNo, type, encode, ord);
            HttpHelper http = new HttpHelper();
            string json = http.Get(urlStr);



            return json;
        }
        public static string GetRouteInfo(string DeliveryNo, string Com)
        {
            string result_json = "";
            string id = "dd497cd6984dadc3";
            //string urlStr = "http://api.kuaidi100.com/api?id={0}&com={1}&nu={2}&show=0&muti=1&order=asc";
            //urlStr = string.Format(urlStr, id, Com, DeliveryNo);
            string urlStr = "http://www.kuaidi100.com/query?id=1&type={0}&postid={1}&valicode=&temp=" + DateTime.Now.ToString("fffff");
            urlStr = string.Format(urlStr, Com, DeliveryNo);
            if (!ht.ContainsKey(urlStr))
            {
                HttpHelper http = new HttpHelper();
                result_json = http.Get(urlStr);
                var result = new RoteJsonReuslt()
                {
                    json = result_json,
                    lastdate = DateTime.Now
                };

                ht.Add(urlStr, result);
            }
            else if (((RoteJsonReuslt)ht[urlStr]).lastdate.AddMinutes(1) > DateTime.Now)
            {
                result_json = ((RoteJsonReuslt)ht[urlStr]).json;
            }
            else
            {
                ht.Remove(urlStr);
                HttpHelper http = new HttpHelper();
                result_json = http.Get(urlStr);
                var result = new RoteJsonReuslt()
                {
                    json = result_json,
                    lastdate = DateTime.Now
                };
                ht.Add(urlStr, result);
            }

            return result_json;
        }
        /// <summary>
        /// 根据快递代码获取快递名称
        /// </summary>
        /// <param name="comCode"></param>
        /// <returns></returns>
        public static string GetComName(string comCode)
        {
            var kuaidiModel = DBEntities.GetEntities().KuaiDiSet.Where(c => c.CompanyCode == comCode).FirstOrDefault();
            if (kuaidiModel == null)
            {
                return "未知";
            }
            return kuaidiModel.CompanyName;
            //if (comCode == "shunfeng")
            //{
            //    return "顺丰快递";
            //}
            //if (comCode == "yuantong")
            //{
            //    return "圆通快递";
            //}
            //if (comCode == "shentong")
            //{
            //    return "申通快递";
            //}
            //if (comCode == "huitongkuaidi")
            //{
            //    return "百世汇通";
            //}
            //if (comCode == "zhongtong")
            //{
            //    return "中通快递";
            //}
            //if (comCode == "yunda")
            //{
            //    return "韵达快递";
            //}

            //return "未知";
        }
        /// <summary>
        /// 获取快递公司列表
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetComList()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var kuaidiList = DBEntities.GetEntities().KuaiDiSet.OrderBy(c => c.KuaiDiSetID).ToList();
            foreach (var item in kuaidiList)
            {
                list.Add(new SelectListItem() { Text = item.CompanyName, Value = item.CompanyCode });
            }
            //list.Add(new SelectListItem() { Text = "圆通快递", Value = "yuantong" });
            //list.Add(new SelectListItem() { Text = "顺丰快递", Value = "shunfeng" });
            //list.Add(new SelectListItem() { Text = "申通快递", Value = "shentong" });
            //list.Add(new SelectListItem() { Text = "百世汇通", Value = "huitongkuaidi" });
            //list.Add(new SelectListItem() { Text = "中通快递", Value = "zhongtong" });
            //list.Add(new SelectListItem() { Text = "韵达快递", Value = "yunda" });
            return list;
        }
    }
    class RoteJsonReuslt
    {
        public string json { get; set; }
        public DateTime lastdate { get; set; }
    }
}
