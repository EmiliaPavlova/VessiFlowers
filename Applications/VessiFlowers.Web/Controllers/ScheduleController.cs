namespace VessiFlowers.Web.Controllers
{
    using System.Configuration;
    using System.Web.Mvc;

    [Authorize]
    public class ScheduleController : Controller
    {
        // GET: Schedule
        public ActionResult Index()
        {
            ViewBag.Email = ConfigurationManager.AppSettings["Email"];
            ViewBag.GoogleCalendarApiKey = ConfigurationManager.AppSettings["GoogleCalendarApiKey"];
            return View();
        }
    }
}