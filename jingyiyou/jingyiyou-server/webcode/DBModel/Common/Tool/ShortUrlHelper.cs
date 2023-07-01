using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ncc2019.Common.Tool
{
    public class ShortUrlHelper
    {
        /// <summary>
        /// 获取礼物的短链接编码
        /// </summary>
        /// <returns></returns>
        public static string GetShortUrl()
        {
            //需要生成短链接
            string shorturl = Guid.NewGuid().ToString().ToLower().Split('-')[0];
            //跟数据库对比短链接是否重复
            ncc2019Entities db = new ncc2019Entities();
            var urlList = from c in db.Orders where c.ShortUrl == shorturl select c;
            if (urlList.Count()>0)
            {
                return GetShortUrl();
            }
            //TODO:需要将短链接存到内存里面            

            return shorturl;
        }

       
    }
}