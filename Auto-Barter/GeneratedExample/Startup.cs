using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GeneratedExample.Startup))]
namespace GeneratedExample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
