
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class SearchController : ControllerBaseNoCheck
    {


        public ActionResult Index(string key, string sort, string sorttype, string order)
        {
            key = key.Trim();

           


            
            ViewBag.GoodSort = db.GoodSort.Where(c => c.Enabled == (int)Common.Enum.Enabled.启用).OrderBy(c => c.GoodSortOrder).ToList();
            ViewBag.sort = sort;
            ViewBag.key = key;
            ViewBag.Title = key == "new" ? "新品上架" : key;
            string next_order = "asc";

            ViewBag.time_img_order = ViewBag.price_img_order = "default";
            ViewBag.time_next_order = ViewBag.price_next_order = next_order;
            order = order ?? "asc";
            if (sorttype == "time")
            {
                if (order == "asc")
                {
                    next_order = "desc";
                }
                ViewBag.time_img_order = order;
                ViewBag.time_next_order = next_order;
            }
            else if (sorttype == "price")
            {
                if (order == "asc")
                {
                    next_order = "desc";
                }
                ViewBag.price_img_order = order;
                ViewBag.price_next_order = next_order;
            }


            //if (!string.IsNullOrEmpty(key))
            //{
            //    var goodlist = db.Goods.Where(c => c.Tags.Contains(key)).ToList();
            //    return View(goodlist);
            //}
            if (!string.IsNullOrEmpty(key))
            {
                //int sortInt = int.Parse(sort);
                //IQueryable<Goods> goodlist;
                var goodlist = db.Goods.Where(c => c.Status == (int)Common.Enum.GoodStatus.上架);
                
                if (key == "new")
                {                   
                    goodlist = goodlist.OrderByDescending(c => c.GoodOrder).ThenByDescending(c => c.GoodID).Take(20);
                }
                else
                {
                    goodlist = from c in goodlist
                               where c.Tags.Contains(key) || c.Name.Contains(key) || c.Intro.Contains(key)
                               select c;
                }


                if (sorttype == "time")
                {
                    if (order == "asc")
                    {
                        goodlist = from c in goodlist orderby c.GoodOrder, c.GoodID select c;
                    }
                    else
                    {
                        goodlist = from c in goodlist orderby c.GoodOrder descending, c.GoodID descending select c;
                    }

                }
                else if (sorttype == "price")
                {
                    if (order == "asc")
                    {
                        goodlist = from c in goodlist orderby c.Payment select c;
                    }
                    else
                    {
                        goodlist = from c in goodlist orderby c.Payment descending select c;
                    }
                }
                else
                {
                    goodlist = from c in goodlist orderby c.GoodOrder descending, c.GoodID descending, c.Payment select c;

                }
                

                return View(goodlist.ToList());
            }

            return View(new List<Goods>());
        }


    }
}
