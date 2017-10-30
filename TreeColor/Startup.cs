using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TreeColor.Startup))]
namespace TreeColor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
