namespace VessiFlowers.Web.Controllers
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using VessiFlowers.Business.Contracts;
    using VessiFlowers.Business.Models.UserModels;
    using VessiFlowers.Data.Entities;
    using VessiFlowers.Web.Extensions;

    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager signInManager;
        private ApplicationUserManager userManager;
        private readonly IEmailService emailService;
        private readonly IPageService pageService;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IEmailService emailService, IPageService pageService)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            this.pageService = pageService ?? throw new ArgumentNullException(nameof(pageService));
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return Redirect("/flowers");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = true });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterViewModel { SubscribeMe = true });
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var domain = Assembly.GetExecutingAssembly().GetName().Name;
                var user = new User { Domain = domain, UserName = model.Email, Email = model.Email, IsSubscribed = model.SubscribeMe };
                var result = await this.UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    string code = await this.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = this.Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: this.Request.Url.Scheme);
                    var email = ConfigurationManager.AppSettings["Email"];
                    var welcome = await this.pageService.GetPage("Welcome");
                    var body = welcome.Content.Replace("@callbackUrl", callbackUrl);
                    this.emailService.Send(email, user.Email, welcome.Title, body, true);

                    this.AddNotification("Беше изпратено писмо за потвърждение. Моля проверете вашият е-майл и потвърдете вашият акаунт.", NotificationType.SUCCESS);
                    return this.Redirect("/login");
                }

                this.AddNotification("Registration failed", NotificationType.ERROR);
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (!result.Succeeded)
            {
                return View("Error");
            }

            this.AddNotification("Вашият акаунт беше потвърден успешно.", NotificationType.SUCCESS);
            return this.Redirect("/login");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await this.UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist
                    this.AddNotification("Линк за промяна на паролата беше изпратен на вашата електронна поща.", NotificationType.SUCCESS);
                    return this.View();
                }

                string code = await this.UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = this.Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: this.Request.Url.Scheme);

                var email = ConfigurationManager.AppSettings["Email"];
                var subject = "VessiFlowers рестартиране на парола";
                var body = string.Format("Вие можете да рестартирае вашата парола като кликнете на следния линк: <a href=\"{0}\">VessiFlowers линк за рестартиране на парола</a>", callbackUrl);
                this.emailService.Send(email, user.Email, subject, body, true);

                this.AddNotification("Линк за промяна на паролата беше изпратен на вашата електронна поща.", NotificationType.SUCCESS);
                return this.View();
            }

            return this.View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Redirect("/resetpassword");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                this.AddNotification("Вашата парола беше сменена успешно.", NotificationType.SUCCESS);
                return Redirect("/login");
            }

            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Redirect("/flowers");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
                }

                if (signInManager != null)
                {
                    signInManager.Dispose();
                    signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}