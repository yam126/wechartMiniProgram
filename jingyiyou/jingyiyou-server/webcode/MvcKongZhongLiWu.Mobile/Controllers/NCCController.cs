using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Enum;
using System.Collections;
using ncc2019.Common;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.vod.Model.V20170321;

namespace ncc2019.Controllers
{
    public class NCCController : ControllerBaseNoCheck
    {
        //
        // GET: /NCC/

        public ActionResult Index(string info, string sellerid)
        {

            //ViewBag.js_json = TenPayManager.MakeUpJsParam();
            //string url = "&redirect_url=" + System.Web.HttpUtility.UrlEncode("https://m.ncc.renxingpao.com/ncc/payok");
            //url = mp3() + url;

            var list = db.Lottery;
            ViewBag.lotterylist = list;

            //ViewBag.url = url;

            ViewBag.info = info;

            ViewBag.sellerid = string.IsNullOrEmpty(sellerid) ? "" : sellerid;

            return View();
        }
        public ActionResult Help()
        {

            return View();
        }
        public ActionResult KeFu()
        {

            return View();
        }
        public ActionResult Apply()
        {

            return View();
        }

        public ActionResult WithDraw()
        {

            return View();
        }
        public ActionResult HisLottery()
        {

            return View();
        }
        public ActionResult OrderList()
        {

            return View();
        }
        public ActionResult Test()
        {

            return View();
        }
        public ActionResult MyAccount()
        {

            return View();
        }
        private void GetVideoPlayAuth(DefaultAcsClient client)
        {
            GetVideoPlayAuthRequest request = new GetVideoPlayAuthRequest();
            // request.VideoId = "";
            try
            {
                GetVideoPlayAuthResponse response = client.GetAcsResponse(request);
                Console.WriteLine("RequestId = " + response.RequestId);
                Console.WriteLine("PlayAuth = " + response.PlayAuth);
                Console.WriteLine("Title = " + response.VideoMeta.Title);
                Console.WriteLine("VideoId = " + response.VideoMeta.VideoId);
                Console.WriteLine("CoverURL = " + response.VideoMeta.CoverURL);
                Console.WriteLine("Duration = " + response.VideoMeta.Duration);
                Console.WriteLine("Status = " + response.VideoMeta.Status);
                ViewBag.playauth = response.PlayAuth;
                ViewBag.vid = response.VideoMeta.VideoId;
            }
            catch (ServerException e)
            {
                Console.WriteLine(e.ErrorCode);
                Console.WriteLine(e.ErrorMessage);
            }
            catch (ClientException e)
            {
                Console.WriteLine(e.ErrorCode);
                Console.WriteLine(e.ErrorMessage);
            }
        }

        public ActionResult Play()
        {
            //var clientProfile = DefaultProfile.GetProfile("cn-shanghai", "LTAIIMMPDTMYFxV0", "2UXRmeqtvsZ8eNeH5w5YyjqXwjTnKt");
            //DefaultAcsClient client = new DefaultAcsClient(clientProfile);
            //GetVideoPlayAuth(client);
            return View();
        }

        public ActionResult payok()
        {
            return View();
        }
        //public ActionResult MakeSeller(string sno)
        //{
        //    ViewBag.url = "" +;
        //    return View();
        //}


    }
}
