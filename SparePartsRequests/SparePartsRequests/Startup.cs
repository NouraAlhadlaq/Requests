using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SparePartsRequests.Startup))]
namespace SparePartsRequests
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
