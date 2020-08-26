using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CameronBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Post",
                url: "{controller}/{action}/{slug}",
                defaults: new { controller = "Posts", action = "preview", 
                    id = UrlParameter.Optional },
                 namespaces: new[] { "CameronBlog.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "FrontPage", 
                    id = UrlParameter.Optional },
                 namespaces: new[] { "CameronBlog.Controllers" }
            );
        }
    }
}
