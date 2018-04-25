namespace VessiFlowers.Web.Controllers
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using VessiFlowers.Business.Contracts;
    using VessiFlowers.Business.Models.DomainModels;
    using VessiFlowers.Web.Extensions;

    public class HomeController : BaseController
    {
        private readonly IPageService pageService;
        private readonly IEmailService emailService;

        public HomeController(IPageService pageService, IEmailService emailService)
        {
            this.pageService = pageService ?? throw new ArgumentNullException(nameof(pageService));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<ActionResult> About()
        {
            var about = await this.pageService.GetPage(nameof(this.About));
            return View(about);
        }

        public async Task<ActionResult> OthersForMe()
        {
            var forMe = await this.pageService.GetPage(nameof(this.OthersForMe));
            return View(forMe);
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                string template = string.Empty;

                using (var sr = new StreamReader(Server.MapPath("~/ContactTemplate.txt")))
                {
                    template = sr.ReadToEnd();
                }

                string to = ConfigurationManager.AppSettings["Email"];
                string subject = "Запитване от " + cvm.Email;
                string body = template.Replace("HowCanIHelpYou", cvm.HowCanIHelpYou)
                                      .Replace("WhatIsYourParty", cvm.WhatIsYourParty)
                                      .Replace("WhenIsYourParty", cvm.WhenIsYourParty)
                                      .Replace("WhatTypeIsTheMan", cvm.WhatTypeIsTheMan)
                                      .Replace("HisHerFlowersColors", cvm.HisHerFlowersColors)
                                      .Replace("WhatAreYouGoingToDo", cvm.WhatAreYouGoingToDo)
                                      .Replace("Email", cvm.Email)
                                      .Replace("AmIMissingSomething", cvm.AmIMissingSomething);

                this.emailService.Send(cvm.Email, to, subject, body, true);
                this.AddNotification("Вашето запитване е изпратено успешно.", NotificationType.SUCCESS);
                return RedirectToAction("Contact");
            }
            catch (Exception e)
            {
                this.AddNotification("Възникна грешка и вашето запитване не е изпратено.", NotificationType.ERROR);
                return View();
            }
        }
    }
}