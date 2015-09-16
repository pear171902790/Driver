using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Driver.Startup))]
namespace Driver
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
