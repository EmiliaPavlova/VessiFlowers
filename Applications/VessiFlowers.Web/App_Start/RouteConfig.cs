namespace VessiFlowers.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using VessiFlowers.Web.App_Helpers;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new
                {
                    controller = "Gallery",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                new string[] { "VessiFlowers.Web.Controllers" }
            ).RouteHandler = new SlugRouteHandler();
        }
    }
}
