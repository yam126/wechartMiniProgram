using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ncc2019.Common.Tool;
using ncc2019.Common.Enum;
using ncc2019.Common.Weixin.Base;

namespace ncc2019.Controllers
{
    public class HomeController : ControllerBase
    {

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return Redirect("http://m.ncc.renxingpao.com/ncc");

            //db.Database.ExecuteSqlCommand("update goods set BuyCount=BuyCount+1 where goodid = 1", new object[] { });
            ViewBag.IsIndex = true;
            ViewBag.GoodSort = db.GoodSort.Where(c => c.Enabled == (int)Common.Enum.Enabled.启用).OrderBy(c => c.GoodSortOrder).ToList();
            var goodindexshow = db.GoodSort.Where(c => c.Enabled == (int)Common.Enum.Enabled.启用 && c.ShowInIndex == (int)ShiFouStatus.是).OrderBy(c => c.GoodSortOrder).ToList();
            ViewBag.GoodSortIndexShow = goodindexshow;
            foreach (var item in goodindexshow)
            {
                string keyword = item.Name;
                ViewData["data" + item.GoodSortID] = db.Goods.Where(c => c.Status == (int)Common.Enum.GoodStatus.上架 && c.Tags.Contains(keyword)).OrderByDescending(c => c.GoodOrder).ThenByDescending(c => c.GoodID).Take(10).ToList();
            }
            //var goodlist_new = db.Goods.Where(c => c.Status == (int)Common.Enum.GoodStatus.上架).OrderByDescending(c => c.GoodID).Take(5).ToList();

            //ViewBag.goodlist_xinqi = db.Goods.Where(c => c.Status == (int)Common.Enum.GoodStatus.上架 && c.Tags.Contains("新奇创意")).OrderByDescending(c => c.GoodOrder).ThenByDescending(c => c.GoodID).Take(10).ToList();

            //ViewBag.goodlist_gaokeji = db.Goods.Where(c => c.Status == (int)Common.Enum.GoodStatus.上架 && c.Tags.Contains("高科技")).OrderByDescending(c => c.GoodOrder).ThenByDescending(c => c.GoodID).Take(10).ToList();

            var goodlistBanner = db.Goods.Where(c => c.Status == (int)Common.Enum.GoodStatus.上架 && c.IsShowBanner == 1).OrderByDescending(o => o.BannerOrder).ThenByDescending(c => c.GoodID).Take(3).ToList()
                .Select(c => new BannerObj()
                {
                    imgurl = c.BannerImgUrl,
                    link = "/Home/Details?id=" + c.GoodID.ToString()
                });
            var infoListBanner = db.Info.Where(c => c.IsShowInBanner == (int)ShiFouStatus.是).OrderByDescending(c => c.InfoID).Take(1).ToList()
                .Select(c => new BannerObj()
                {
                    imgurl = c.BannerImgUrl,
                    link = "/search/?key=" + c.Title
                });
            var list = infoListBanner.Concat(goodlistBanner).Take(3);
            ViewBag.Banner = list;

            return View();
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id = 0)
        {
            Goods good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            else
            {
                good.ViewCount++;
                db.Entry(good).Property(c => c.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            return View(good);
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Goods good)
        {
            if (ModelState.IsValid)
            {
                db.Goods.Add(good);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(good);
        }

        //
        // GET: /Home/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Goods good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            return View(good);
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Goods good)
        {
            if (ModelState.IsValid)
            {
                db.Entry(good).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(good);
        }

        //
        // GET: /Home/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Goods good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            return View(good);
        }

        //
        // POST: /Home/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Goods good = db.Goods.Find(id);
            db.Goods.Remove(good);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(5);
            SessionHelper.SetSession("ValidateCode", code);
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Goods()
        {
            return View();
        }
        #region Info
        public ActionResult Info(int id = 0)
        {
            Info info = db.Info.Find(id);
            if (info == null)
            {
                return HttpNotFound();
            }
            else
            {
                info.ViewCount++;
                db.Entry(info).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(info);
        }
        #endregion

        protected override bool CheckUser()
        {
            return true;
        }
    }
   public class BannerObj
    {
        public string link = "";
        public string imgurl = "";
    }
}