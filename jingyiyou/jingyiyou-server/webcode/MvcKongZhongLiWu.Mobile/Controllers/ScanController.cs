using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class ScanController : Controller
    {
        //
        // GET: /Scan/

        public ActionResult Index(string sellerid)
        {

            return Redirect("/ncc/index?info=reg&sno=" + sellerid);
        }

    }
}
