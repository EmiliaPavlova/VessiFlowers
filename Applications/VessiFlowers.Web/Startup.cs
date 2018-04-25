using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VessiFlowers.Web.Startup))]
namespace VessiFlowers.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
