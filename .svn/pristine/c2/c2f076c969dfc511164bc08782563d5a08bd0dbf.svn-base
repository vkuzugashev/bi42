using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace bi42
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                       name: "Index",
                       url: "",
                       defaults: new { controller = "Home", action = "Index" },
                       namespaces: new[] { "bi42.Controllers" }
                   );

            //routes.MapRoute(
            //    name: "About",
            //    url: "About",
            //    defaults: new { controller = "Home", action = "About" },
            //    namespaces: new[] { "bi42.Controllers" }
            //);

            //routes.MapRoute(
            //    name: "Contact",
            //    url: "Contact",
            //    defaults: new { controller = "Home", action = "Contact" },
            //    namespaces: new[] { "bi42.Controllers" }
            //);


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { area = "", controller = "Home", action = UrlParameter.Optional, id = UrlParameter.Optional },
                namespaces: new[] { "bi42.Controllers" }
            );
        }
    }
}
