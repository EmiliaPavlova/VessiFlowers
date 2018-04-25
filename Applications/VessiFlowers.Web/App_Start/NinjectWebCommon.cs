[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(VessiFlowers.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(VessiFlowers.Web.App_Start.NinjectWebCommon), "Stop")]

namespace VessiFlowers.Web.App_Start
{
    using System;
    using System.Web;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using VessiFlowers.Business.Contracts;
    using VessiFlowers.Business.Services;
    using VessiFlowers.Data.Entities;
    using VessiFlowers.Data.Repositories;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IVessiFlowersContext>().To<VessiFlowersContext>();
            kernel.Bind<IUserStore<User>>().To<UserStore<User>>().WithConstructorArgument("context", kernel.Get<IVessiFlowersContext>());
            kernel.Bind<UserManager<User>>().ToSelf();
            kernel.Bind<SignInManager<User, string>>().To<ApplicationSignInManager>();
            kernel.Bind<IAuthenticationManager>().ToMethod(c => HttpContext.Current.GetOwinContext().Authentication);

            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IEmailService>().To<EmailService>();
            kernel.Bind<IPageService>().To<PageService>();
            kernel.Bind<IGalleryService>().To<GalleryService>();
            kernel.Bind<IBlogService>().To<BlogService>();
        }        
    }
}
