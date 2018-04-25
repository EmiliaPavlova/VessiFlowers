namespace VessiFlowers.Web.App_Helpers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using VessiFlowers.Data.Repositories;

    public class SlugRouteHandler : MvcRouteHandler
    {
        private static IVessiFlowersContext data;
        private static object rootSync = new object();

        public SlugRouteHandler()
        {
            lock (rootSync)
            {
                if (data == null)
                {
                    data = new VessiFlowersContext();
                }
            }
        }

        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var url = requestContext.HttpContext.Request.Path.TrimStart('/');

            if (!string.IsNullOrEmpty(url))
            {
                var slug = data.Slugs.FirstOrDefault(s => s.Url == url);
                if (slug != null)
                {
                    FillRequest(slug.Controller,
                        slug.Action ?? "GetStatic",
                        slug.Param.ToString(),
                        requestContext);
                }
            }

            return base.GetHttpHandler(requestContext);
        }

        private static void FillRequest(string controller, string action, string id, RequestContext requestContext)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            requestContext.RouteData.Values["controller"] = controller;
            requestContext.RouteData.Values["action"] = action;
            requestContext.RouteData.Values["id"] = id;
        }
    }
}