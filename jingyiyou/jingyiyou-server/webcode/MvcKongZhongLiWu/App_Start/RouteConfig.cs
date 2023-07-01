using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ncc2019
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default4",
                url: "gift/ok",
                defaults: new { controller = "gift", action = "ok" }
            );
            routes.MapRoute(
                name: "Default3",
                url: "gift/docreate",
                defaults: new { controller = "gift", action = "docreate" }
            );
            routes.MapRoute(
                name: "Default5",
                url: "gift/verf",
                defaults: new { controller = "gift", action = "verf" }
            );
            routes.MapRoute(
                name: "Default2",
                url: "gift/{shorturl}",
                defaults: new { controller = "gift", action = "Index" },
                constraints: new { controller = "gift", action = "Index" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }

}