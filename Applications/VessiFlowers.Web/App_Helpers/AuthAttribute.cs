using System.Web.Mvc;
using System.Web.Routing;

namespace VessiFlowers.Web.App_Helpers
{
    public class AuthAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                var areaName = filterContext.RouteData.DataTokens["area"];
                if (areaName != null && areaName.Equals("Admin"))
                {
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Account", action = "Login", area = "Admin" }));
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Account", action = "Login" }));
                }
            }
        }
    }
}