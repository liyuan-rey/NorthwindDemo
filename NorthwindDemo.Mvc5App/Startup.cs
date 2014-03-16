using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NorthwindDemo.Mvc5App.Startup))]
namespace NorthwindDemo.Mvc5App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
