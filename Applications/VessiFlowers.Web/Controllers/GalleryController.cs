namespace VessiFlowers.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using VessiFlowers.Business.Contracts;

    public class GalleryController : BaseController
    {
        private readonly IGalleryService galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            this.galleryService = galleryService ?? throw new ArgumentNullException(nameof(galleryService));
        }

        public async Task<ActionResult> Index(int id = 1)
        {
            var galleryType = await this.galleryService.GetGalleryType(id);
            return View(galleryType);
        }

        public async Task<ActionResult> Details(int id)
        {
            var gallery = await this.galleryService.GetGallery(id);
            return View(gallery);
        }
    }
}