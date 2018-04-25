namespace VessiFlowers.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using VessiFlowers.Business.Contracts;
    using VessiFlowers.Business.Models.DomainModels;
    using VessiFlowers.Web.App_Helpers;

    public class BlogController : BaseController
    {
        private readonly IBlogService blogService;
        private readonly MediaHelper mediaHelper;

        public BlogController(IBlogService blogService, MediaHelper mediaHelper)
        {
            this.blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
            this.mediaHelper = mediaHelper ?? throw new ArgumentNullException(nameof(mediaHelper));
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult Posts()
        {
            var posts = this.blogService.ReadPosts();
            return this.View(posts);
        }

        [HttpGet]
        public ActionResult Create(int id = 0)
        {
            if (id > 0)
            {
                return this.RedirectToAction(nameof(this.Details));
            }

            var userId = this.User.Identity.GetUserId();
            var model = new PostViewModel()
            {
                UserId = userId,
                Created = DateTime.Now,
                IsActive = false
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int id, PostViewModel pvm)
        {
            if (!ModelState.IsValid)
            {
                return View(pvm);
            }

            var postId = await this.blogService.CreatePost(pvm);
            return this.RedirectToAction(nameof(this.Details), new { id = postId });
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id = 1)
        {
            var post = await this.blogService.GetPost(id);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details(int id, PostViewModel pvm)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            pvm.UserId = this.User.Identity.GetUserId();
            var postId = await this.blogService.UpdatePost(pvm);
            return this.RedirectToAction(nameof(this.Details), new { id = postId });
        }

        [HttpGet]
        public async Task<ActionResult> AddOrUpdateImage(int id = 0, int pid = 0)
        {
            var media = new MediaViewModel();
            if (id != 0)
            {
                media = await this.blogService.GetMedia(id, pid);
            }

            if (pid != 0)
            {
                media.PostId = pid;
            }

            return View(media);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrUpdateImage(int id, MediaViewModel mvm)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Details), new { id = mvm.PostId });
            }

            if (id == 0)
            {
                this.mediaHelper.Save(this.Server, mvm);
                await this.blogService.CreateMedia(mvm);
            }
            else
            {
                var existingMedia = await this.blogService.GetMedia(id);
                if (existingMedia == null)
                {
                    return this.RedirectToAction(nameof(this.Details), new { id = mvm.PostId });
                }

                if (mvm.File != null)
                {
                    this.mediaHelper.Delete(this.Server, existingMedia.Url);
                    this.mediaHelper.Save(this.Server, mvm);
                    await this.blogService.UpdateMedia(mvm);
                }
            }

            return this.RedirectToAction(nameof(this.Details), new { id = mvm.PostId });
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var existingMedia = await this.blogService.GetMedia(id);
            if (existingMedia == null)
            {
                return this.RedirectToAction(nameof(this.Details), new { id = 1 });
            }

            this.mediaHelper.Delete(this.Server, existingMedia.Url);
            await this.blogService.DeleteMeldia(id);

            return this.RedirectToAction(nameof(this.Details), new { id = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(int id)
        {
            if (id != 0)
            {
                var post = await this.blogService.GetPost(id);
                if (post != null)
                {
                    foreach (var image in post.Images)
                    {
                        this.mediaHelper.Delete(this.Server, image.Url);
                    }

                    await this.blogService.DeletePost(id);
                }
            }

            return RedirectToAction(nameof(Create));
        }
    }
}