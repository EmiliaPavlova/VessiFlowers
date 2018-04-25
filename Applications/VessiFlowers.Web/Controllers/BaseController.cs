namespace VessiFlowers.Web.Controllers
{
    using NLog;
    using System.Web.Mvc;

    public class BaseController : Controller
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        protected override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;
            logger.Log(LogLevel.Error, ex);
            base.OnException(filterContext);
        }
    }
}