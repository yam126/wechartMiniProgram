using ncc2019.Common.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ncc2019.Common.BLL
{
    public class GetGoodsFromAli
    {
        public TheGoods AliGet(string url)
        {
            //url = "http://detail.1688.com/offer/40004120085.html?spm=a2615.7691456.0.0.Yw1A12";
            HttpHelper http = new HttpHelper();
            http.ReponseEncoding = Encoding.Default;
            string block = http.Get(url);

            //ScriptEngine script = new ScriptEngine();
            //string html = script.Run(Resource1.test, "gethtml", block).ToString();

            string regexString = "<div class=\"widget-custom offerdetail_common_description\" data-widget-name=\"offerdetail_common_description\">(?<v>[\\s\\S]*?)</div>";
            string html = GetString(block, regexString);

            regexString = "<h1 class=\"d-title\">(?<v>[\\s\\S]*?)</h1>";
            string name = GetString(block, regexString);

            //<a class="box-img"[\s\S]*?src="(?<v>[\s\S]*?)"[\s\S]*?</a>
            regexString = "<a class=\"box-img\"[\\s\\S]*?src=\"(?<v>[\\s\\S]*?)\"[\\s\\S]*?</a>";
            string imgurl = GetString(block, regexString);
            imgurl = GetImagePath(imgurl);

            regexString = "data-tfs-url=\"(?<v>[\\s\\S]*?)\"";
            string _url = GetString(html, regexString);

            string content = http.Get(_url);

            regexString = "src=\"(?<v>[\\s\\S]*?)\"";
            List<string> list = GetList(content, regexString);


            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                //if (item.Contains("http://img.china.alibaba.com"))
                //{
                string filename = GetImagePath(item);
                if (filename != "")
                {
                    sb.AppendLine(string.Format("<img src='{0}' />", filename));
                }
                //}
            }

            string resultHtml = sb.ToString();

            TheGoods good = new TheGoods()
            {
                Desc = resultHtml,
                Name = name,
                ImgURL = imgurl

            };
            return good;

        }

        public TheGoods TmallGet(string url)
        {
            //url = "http://detail.1688.com/offer/40004120085.html?spm=a2615.7691456.0.0.Yw1A12";
            HttpHelper http = new HttpHelper();
            http.ReponseEncoding = Encoding.Default;
            string block = http.Get(url);

            //ScriptEngine script = new ScriptEngine();
            //string html = script.Run(Resource1.test, "gethtml", block).ToString();

            string regexString = "\"descUrl\":\"(?<v>[\\s\\S]*?)\"";
            string _url = GetString(block, regexString);

            regexString = "<img id=\"J_ImgBooth\" alt=\"(?<v>[\\s\\S]*?)\" src=\"[\\s\\S]*?\"[\\s\\S]*?/>";
            string name = GetString(block, regexString);

            //<a class="box-img"[\s\S]*?src="(?<v>[\s\S]*?)"[\s\S]*?</a>
            regexString = "<img id=\"J_ImgBooth\" alt=\"[\\s\\S]*?\" src=\"(?<v>[\\s\\S]*?)\"[\\s\\S]*?/>";
            string imgurl = GetString(block, regexString);
            imgurl = GetImagePath(imgurl);

            string content = http.Get(_url);

            regexString = "src=\"(?<v>[\\s\\S]*?)\"";
            List<string> list = GetList(content, regexString);


            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                //if (item.Contains("http://img.china.alibaba.com"))
                //{
                string filename = GetImagePath(item);
                if (filename != "")
                {
                    sb.AppendLine(string.Format("<img src='{0}' />", filename));
                }
                //}
            }

            string resultHtml = sb.ToString();

            TheGoods good = new TheGoods()
            {
                Desc = resultHtml,
                Name = name,
                ImgURL = imgurl

            };
            return good;

        }


        private string GetImagePath(string url)
        {
            HttpHelper http = new HttpHelper();
            http.ReponseEncoding = Encoding.Default;
            string filename = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
            System.Drawing.Bitmap bitmap = http.GetImage(url, "");
            if (bitmap != null && bitmap.Width > 200)
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/editor/attached/aliimgage/");
                string datapath = DateTime.Now.ToString("yyyyMMdd");
                path += datapath;
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                path += "\\" + filename;
                bitmap.Save(path);
                bitmap.Dispose();
                return "/editor/attached/aliimgage/" + datapath + "/" + filename;

            }
            return "";
        }
        private string GetString(string block, string regexString)
        {

            //string regexString = "<div class=\"widget-custom offerdetail_common_description\" data-widget-name=\"offerdetail_common_description\">(?<v>[\\s\\S]*?)</div>";
            Regex regex = new Regex(regexString, RegexOptions.Singleline);
            MatchCollection matchs;
            matchs = regex.Matches(block);

            string html = "";
            if (matchs.Count > 0)
            {
                html = matchs[0].Groups["v"].Value;
            }

            return html;

        }

        private List<string> GetList(string block, string regexString)
        {

            //string regexString = "<div class=\"widget-custom offerdetail_common_description\" data-widget-name=\"offerdetail_common_description\">(?<v>[\\s\\S]*?)</div>";
            Regex regex = new Regex(regexString, RegexOptions.Singleline);
            MatchCollection matchs;
            matchs = regex.Matches(block);

            List<string> htmlList = new List<string>();
            if (matchs.Count > 0)
            {
                foreach (Match match in matchs)
                {
                    htmlList.Add(match.Groups["v"].Value);
                }

            }
            return htmlList;
        }
    }
    public class TheGoods
    {
        public string Desc { get; set; }
        public string Name { get; set; }

        public string ImgURL { get; set; }
    }
}
