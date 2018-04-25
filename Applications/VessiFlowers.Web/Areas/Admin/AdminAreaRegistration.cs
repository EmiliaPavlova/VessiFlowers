using System.Web.Mvc;
using System.Web.Optimization;

namespace VessiFlowers.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}/{gid}",
                new { controller = "Users", action = "Index", id = UrlParameter.Optional, gid = UrlParameter.Optional },
                new string[] { "VessiFlowers.Web.Areas.Admin.Controllers" }
            );

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}