namespace VessiFlowers.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using VessiFlowers.Business.Contracts;

    public class UsersController : BaseController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException();
        }

        // GET: Admin/Users
        public async Task<ActionResult> Index()
        {
            var users = await this.userService.Read();
            return View(users);
        }
    }
}