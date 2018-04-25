namespace VessiFlowers.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System;
    using VessiFlowers.Web.App_Helpers;
    using VessiFlowers.Business.Models.DomainModels;
    using VessiFlowers.Business.Contracts;
    using System.Threading.Tasks;

    public class GalleryController : BaseController
    {
        private readonly IGalleryService galleryService;
        private readonly MediaHelper mediaHelper;

        public GalleryController(IGalleryService galleryService, MediaHelper mediaHelper)
        {
            this.galleryService = galleryService ?? throw new ArgumentNullException(nameof(galleryService));
            this.mediaHelper = mediaHelper ?? throw new ArgumentNullException(nameof(mediaHelper));
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult Menu()
        {
            var galleryTypes = this.galleryService.GetGalleryTypes();
            return this.View(galleryTypes);
        }

        public ActionResult Create(int id = 1)
        {
            this.LoadGaleryTypes();
            return View(new GalleryViewModel { GalleryTypeId = id, Position = 1, Created = DateTime.Now });
        }

        [HttpPost]
        public async Task<ActionResult> Create(int id, GalleryViewModel gvm)
        {
            if (!ModelState.IsValid)
            {
                this.LoadGaleryTypes();
                return View(new GalleryViewModel { GalleryTypeId = id, Position = 1, Created = DateTime.Now });
            }

            var gid = await this.galleryService.CreateGallery(gvm);

            return this.RedirectToAction("Details", new { id = gid });
        }

        public async Task<ActionResult> Details(int id = 1)
        {
            this.LoadGaleryTypes();
            var gallery = await this.galleryService.GetGallery(id);
            return View(gallery);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details(GalleryViewModel gvm)
        {
            if (!ModelState.IsValid)
            {
                this.LoadGaleryTypes();
                return this.View();
            }

            await this.galleryService.UpdateGallery(gvm);
            return this.RedirectToAction("Details", gvm.Id);
        }

        public async Task<ActionResult> AddOrUpdateImage(int id = 0, int gid = 0, bool isPalette = false)
        {
            var media = await this.galleryService.GetMedia(id, gid, isPalette);

            return View(media);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrUpdateImage(int id, MediaViewModel mvm)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Details", new { id = mvm.GalleryId });
            }

            if (id == 0)
            {
                this.mediaHelper.Save(this.Server, mvm);
                await this.galleryService.CreateMedia(mvm);
            }
            else
            {
                var existingMedia = await this.galleryService.GetMedia(id);
                if (existingMedia == null)
                {
                    return this.RedirectToAction("Details", new { id = mvm.GalleryId });
                }

                if (mvm.File != null)
                {
                    this.mediaHelper.Delete(this.Server, existingMedia.Url);
                    this.mediaHelper.Save(this.Server, mvm);
                    await this.galleryService.UpdateMedia(mvm);
                }
            }

            return this.RedirectToAction("Details", new { id = mvm.GalleryId });
        }

        public async Task<ActionResult> Delete(int id)
        {
            var existingMedia = await this.galleryService.GetMedia(id);
            int? gid = 1;
            if (existingMedia != null)
            {
                this.mediaHelper.Delete(this.Server, existingMedia.Url);
                gid = existingMedia.GalleryId;
                await this.galleryService.DeleteMeldia(id);
            }

            return this.RedirectToAction("Details", new { id = gid });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(int id)
        {
            if (id != 0)
            {
                var gallery = await this.galleryService.GetGallery(id);
                if (gallery != null)
                {
                    int gtid = gallery.GalleryTypeId;
                    foreach (var image in gallery.Images)
                    {
                        this.mediaHelper.Delete(this.Server, image.Url);
                    }

                    await this.galleryService.RemoveGallery(id);

                    await this.galleryService.RePositionGalleries(gtid);

                }
            }

            return RedirectToAction("Details");
        }

        private void LoadGaleryTypes()
        {
            ViewBag.GalleryTypes = this.galleryService.GetGalleryTypes()
                                                      .Select(gt => new SelectListItem
                                                      {
                                                          Text = gt.Name,
                                                          Value = gt.Id.ToString()
                                                      })
                                                      .ToList();
        }
    }
}