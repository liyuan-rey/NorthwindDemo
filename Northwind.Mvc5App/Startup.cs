// Startup.cs

using Microsoft.Owin;
using Northwind.Mvc5App;

[assembly: OwinStartup(typeof (Startup))]

namespace Northwind.Mvc5App
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}