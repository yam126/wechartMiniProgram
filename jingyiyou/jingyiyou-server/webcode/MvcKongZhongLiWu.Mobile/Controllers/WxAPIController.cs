using Senparc.Weixin.MP.AdvancedAPIs.Media;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Tool;

namespace ncc2019.Controllers
{
    public class WxAPIController : ControllerBaseNoCheck
    {

        [HttpPost]
        public JsonResult UploadVioce(string mediaid)
        {
            if (!string.IsNullOrEmpty(mediaid))
            {
                var order = GetOrder();
                string mediaid_new = order.WxVioceMediaID;
                if (order.WxVioceUploadDate == null || order.WxVioceUploadDate < DateTime.Now.AddDays(-1))
                {
                    string localpath = Server.MapPath("~/upload_vioce/") + mediaid + ".amr";
                    UploadResultJson json = MediaApi.Upload(Common.Tool.TokenHelper.GetToken(), Senparc.Weixin.MP.UploadMediaFileType.voice, localpath);
                    mediaid_new = json.media_id;                    

                    db.Database.ExecuteSqlCommand(" update orders set WxVioceUploadDate={0} where orderid={1} "
                          , new object[] {  DateTime.Now, order.OrderID });
                }                

                return Json(new { state = "ok", mediaid = mediaid_new });
            }
            return Json(new { state = "error" });
        }

        [HttpPost]
        public string DownVioce(string mediaid)
        {
            string result = "error";
            if (!string.IsNullOrEmpty(mediaid))
            {
                try
                {
                    var order = GetOrder();
                    if (order.WxVioceMediaID != mediaid)
                    {
                        string localpath = Server.MapPath("~/upload_vioce/") + mediaid + ".amr";


                        MemoryStream ms = new MemoryStream();
                        Senparc.Weixin.MP.AdvancedAPIs.Media.MediaApi.Get(Common.Tool.TokenHelper.GetToken(), mediaid, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        FileStream fs = new FileStream(localpath, FileMode.OpenOrCreate);
                        BinaryWriter w = new BinaryWriter(fs);
                        w.Write(ms.ToArray());
                        fs.Close();
                        ms.Close();                        

                        db.Database.ExecuteSqlCommand(" update orders set WxVioceMediaID={0} where orderid={1} "
                            , new object[] { mediaid, order.OrderID });
                    }

                    //进行本地的转换
                    string ffmpegPath = Server.MapPath("/Other/");
                    string amrPath = Server.MapPath("/upload_vioce/" + mediaid + ".amr");
                    string mp3Path = Server.MapPath("/upload_vioce/" + mediaid + ".mp3");
                    AmrConvertToMp3.ConvertToMp3(ffmpegPath, amrPath, mp3Path);


                    result = "ok";
                }
                catch (Exception error)
                {

                }


            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { state = result, jsoncallback = Request["jsoncallback"] });
        }

        [HttpPost]
        public string DownImage(string mediaid)
        {
            string result = "error";
            if (!string.IsNullOrEmpty(mediaid))
            {
                try
                {
                    var order = GetOrder();
                    if (order.WxVioceMediaID != mediaid)
                    {
                        string localpath = Server.MapPath("~/upload_image/") + mediaid + ".jpg";


                        MemoryStream ms = new MemoryStream();
                        Senparc.Weixin.MP.AdvancedAPIs.Media.MediaApi.Get(Common.Tool.TokenHelper.GetToken(), mediaid, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        FileStream fs = new FileStream(localpath, FileMode.OpenOrCreate);
                        BinaryWriter w = new BinaryWriter(fs);
                        w.Write(ms.ToArray());
                        fs.Close();
                        ms.Close();
                                                
                    }

                    result = "ok";
                }
                catch (Exception error)
                {

                }


            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { state = result, jsoncallback = Request["jsoncallback"] });
        }
        [HttpPost]
        public string JsError(string content)
        {
            return "";
        }
    }
}
