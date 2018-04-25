namespace VessiFlowers.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using VessiFlowers.Business.Contracts;
    using VessiFlowers.Business.Models.DomainModels;
    using VessiFlowers.Web.Extensions;

    public class PageController : BaseController
    {
        private readonly IPageService pageService;

        public PageController(IPageService pageService)
        {
            this.pageService = pageService ?? throw new ArgumentNullException(nameof(pageService));
        }

        public async Task<ActionResult> About()
        {
            var aboutUsPage = await this.pageService.GetPage(nameof(this.About));
            return View(aboutUsPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> About(PageViewModel pvm)
        {
            await this.pageService.UpdatePage(pvm);
            return RedirectToAction(nameof(this.About));
        }

        public async Task<ActionResult> OthersForMe()
        {
            var forMePage = await this.pageService.GetPage(nameof(this.OthersForMe));
            return View(forMePage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OthersForMe(PageViewModel pvm)
        {
            await this.pageService.UpdatePage(pvm);
            return RedirectToAction(nameof(this.OthersForMe));
        }

        public async Task<ActionResult> Welcome()
        {
            var welcomePage = await this.pageService.GetPage(nameof(this.Welcome));
            return View(welcomePage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Welcome(PageViewModel pvm)
        {
            await this.pageService.UpdatePage(pvm);
            return RedirectToAction(nameof(this.Welcome));
        }

        public async Task<ActionResult> Bulletin(string months)
        {
            var baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            var blogImageUrl = "/Images/blogImage.jpg";
            var bulletinPage = await this.pageService.GetBulletinPage(months, baseUrl, blogImageUrl);
            return View(bulletinPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Bulletin(PageViewModel pvm)
        {
            var sentCount = await this.pageService.SendBulletinAsync(pvm);
            if (sentCount > 0)
            {
                this.AddNotification($"Успешно изпратено на {sentCount} абоната", NotificationType.SUCCESS);
            }

            return RedirectToAction(nameof(this.Bulletin), new { months = 1 });
        }
    }
}