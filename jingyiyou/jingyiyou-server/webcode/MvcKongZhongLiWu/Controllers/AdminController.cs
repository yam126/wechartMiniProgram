using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Tool;
using Webdiyer.WebControls.Mvc;
using ncc2019.Common.BLL;

namespace ncc2019.Controllers
{
    public class AdminController : ControllerAdminBase
    {

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return Redirect("/adminlogin");
        }

        public ActionResult Main()
        {
            return View();
        }
        public ActionResult ProductList(int? pageindex = 1, string sname = "", string stag = "")
        {
            var goods = db.Goods.Where(c => c.GoodID != 0);

            if (stag != "")
            {
                goods = goods.Where(c => c.Tags.Contains(stag));
            }
            if (sname != "")
            {
                goods = goods.Where(c => c.Name.Contains(sname));
            }
            int pageIndex = pageindex ?? 1;
            int totalCount = goods.Count();
            ViewBag.totalCount = totalCount;
            PagedList<Goods> mPage = goods.OrderByDescending(c => c.GoodID).AsQueryable().ToPagedList(pageIndex, 10);
            mPage.TotalItemCount = totalCount;
            mPage.CurrentPageIndex = pageIndex;

            //var productList = db.Goods;
            return View(mPage);
        }
        public ActionResult ProductEdit(int id = 0)
        {
            //ViewBag.SortList = db.GoodSorts.Select(c => new SelectListItem() { Text = c.Name, Value = c.GoodSortID.ToString() });
            //ViewBag.SortList = new List<SelectListItem>() { };
            Goods good = db.Goods.Find(id);

            List<SelectListItem> SortList = new List<SelectListItem>();
            foreach (var item in db.GoodSort)
            {
                var sort = new SelectListItem() { Text = item.Name, Value = item.GoodSortID.ToString() };
                if (good != null)
                {
                    //if (good.GoodSortMapping.Where(c => c.GoodSortID == item.GoodSortID).Count() > 0)
                    //{
                    //    sort.Selected = true;
                    //}
                }
                SortList.Add(sort);
            }
            ViewBag.SortList = SortList;

            //if (good == null)
            //{
            //    return HttpNotFound();
            //}
            if (good == null)
            {
                good = new Goods();
                good.Status = (int)ncc2019.Common.Enum.GoodStatus.上架;
                good.BuyCount = 0;
                good.TotalNum = 10;
                good.BannerOrder = 1;
                good.GoodID = 0;
                good.NeedTiLiNum = 5;
                good.JoinPeopleNum = 0;
                good.ViewCount = 0;
                good.ExpressFee = 10;
                good.DownPayment = (decimal)0.10;
            }
            else
            {
                ViewBag.goodp = db.GoodProperty.Where(c => c.GoodID == good.GoodID).OrderBy(c => c.GoodID).ToList();

            }


            return View(good);
        }

        public ActionResult GetGoodInfo()
        {

            return View();
        }
        [HttpPost]
        public ActionResult GetGoodInfo(string url, string gettype)
        {
            GetGoodsFromAli ali = new GetGoodsFromAli();
            TheGoods thegood = new TheGoods();
            if (gettype == "ali")
            {
                thegood = ali.AliGet(url);
            }
            else
            {
                thegood = ali.TmallGet(url);
            }

            Goods good = new Goods()
            {
                Desc = thegood.Desc,
                Name = thegood.Name,
                AddDate = DateTime.Now,
                Status = (int)Common.Enum.GoodStatus.下架,
                BuyCount = 0,
                TotalNum = 10,
                IsShowBanner = (int)Common.Enum.ShiFouStatus.否,
                ViewCount = 0,
                ImgUrl = thegood.ImgURL,
                Memo = url,
                AddMemberID = CurMemberInfo.MemnerID
            };
            db.Configuration.ValidateOnSaveEnabled = false;
            db.Goods.Add(good);
            db.SaveChanges();

            ViewBag.goodid = good.GoodID;
            showSuccessMessage("保存成功！");
            return View();
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ProductEdit(Goods good)
        {
            if (ModelState.IsValid)
            {

                if (good.GoodID == 0)
                {
                    good.AddDate = DateTime.Now;
                    good.ViewCount = 0;
                    good.AddMemberID = CurMemberInfo.MemnerID;                    
                    db.Goods.Add(good);
                }
                else
                {
                    //db.Entry(good).State = EntityState.Modified;
                    db.Entry(good).Property(c => c.AddDate).IsModified = false;
                    db.Entry(good).Property(c => c.ViewCount).IsModified = false;
                    db.Entry(good).Property(c => c.AddMemberID).IsModified = false;
                }
                db.SaveChanges();


                //var mapList = db.GoodSortMapping.Where(c => c.GoodID == good.GoodID);
                //foreach (var map in mapList)
                //{
                //    db.GoodSortMapping.Remove(map);
                //}
                string goodsort = Request.Form["goodsort"];
                //if (!string.IsNullOrEmpty(goodsort))
                //{
                //    string[] list = goodsort.Split(',');
                //    foreach (var sort in list)
                //    {
                //        GoodSortMapping map = new GoodSortMapping()
                //        {
                //            GoodID = good.GoodID,
                //            GoodSortID = int.Parse(sort)
                //        };
                //        db.GoodSortMapping.Add(map);
                //    }

                //}

                Response.Redirect("~/Admin/ProductList");
                return null;

                //return RedirectToAction("Index");
            }
            return View(good);
        }


        public ActionResult ProductDelete(int id)
        {
            Goods good = db.Goods.Find(id);
            db.Goods.Remove(good);
            db.SaveChanges();
            return RedirectToAction("ProductList");
        }

        public ActionResult ProductSortList()
        {
            var goodsort = db.GoodSort;
            return View(goodsort);
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="upImg"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase upImg)
        {
            string fileName = Guid.NewGuid() + "." + System.IO.Path.GetFileName(upImg.FileName).Split('.')[1];
            string filePhysicalPath = Server.MapPath("~/upload/" + fileName);
            string pic = "", error = "";
            try
            {
                upImg.SaveAs(filePhysicalPath);
                pic = "/upload/" + fileName;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(new
            {
                pic = pic,
                error = error
            }, "text/html", JsonRequestBehavior.AllowGet);
        }

        public JsonResult CutImg(string xstr, string ystr, string wstr, string hstr, string url)
        {
            string sourceFile = Server.MapPath(url.Replace("../../", "~/"));
            string filename = Guid.NewGuid() + ".jpg";
            string savePath = "~/CutImage/" + filename;
            int x = 0;
            int y = 0;
            int w = 1;
            int h = 1;
            try
            {
                x = int.Parse(xstr);
                y = int.Parse(ystr);
                w = int.Parse(wstr);
                h = int.Parse(hstr);
            }
            catch { }

            ImageCut ic = new ImageCut(x, y, w, h);
            System.Drawing.Bitmap cuted = ic.KiCut(new System.Drawing.Bitmap(sourceFile));
            string cutPath = Server.MapPath(savePath);
            cuted.Save(cutPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            //Response.Write(savePath);
            return Json(new { message = "ok", src = "../../CutImage/" + filename });
        }
        [HttpPost]
        public JsonResult AddProperty(string pname, string pcontent, int goodid)
        {
            string state = "error";
            int pid = 0;
            try
            {
                GoodProperty property = new GoodProperty()
                {
                    AddDate = DateTime.Now,
                    AddMemberID = CurMemberInfo.MemnerID,
                    Content = pcontent,
                    Name = pname,
                    GoodID = goodid
                };
                db.GoodProperty.Add(property);
                db.SaveChanges();
                pid = property.GoodPropertyID;
                state = "ok";
            }
            catch (Exception)
            {


            }


            return Json(new { state = state, pid = pid });
        }
        [HttpPost]
        public JsonResult EditProperty(string pname, string pcontent, int pid)
        {
            string state = "error";
            try
            {
                var property = db.GoodProperty.Find(pid);
                property.Name = pname;
                property.Content = pcontent;

                db.Entry(property).Property(c => c.AddDate).IsModified = false;
                db.Entry(property).Property(c => c.AddMemberID).IsModified = false;
                db.SaveChanges();
                state = "ok";
            }
            catch (Exception)
            {


            }

            return Json(new { state = state });
        }
        [HttpPost]
        public JsonResult DeleteProperty(int pid)
        {
            string state = "error";
            try
            {
                var property = db.GoodProperty.Find(pid);
                db.GoodProperty.Remove(property);
                db.SaveChanges();
                state = "ok";
            }
            catch (Exception)
            {


            }

            return Json(new { state = state });
        }
    }
}
