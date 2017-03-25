using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Billing.Identity.Startup))]
namespace Billing.Identity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
