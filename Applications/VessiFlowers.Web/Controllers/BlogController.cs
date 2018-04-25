namespace VessiFlowers.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using VessiFlowers.Business.Contracts;

    public class BlogController : BaseController
    {
        private readonly IBlogService blogService; 

        public BlogController(IBlogService blogService)
        {
            this.blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
        }

        public async Task<ActionResult> Index(int page = 1)
        {
            var blog = await this.blogService.GetBlog(page);
            return View(blog);
        }

        public async Task<ActionResult> Post(int id)
        {
            var post = await this.blogService.GetPost(id);
            if (post == null)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            return View(post);
        }
    }
}