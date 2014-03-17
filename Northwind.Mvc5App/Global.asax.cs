// Global.asax.cs

namespace Northwind.Mvc5App
{
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Properties;
    using WebApi2Services.Dto;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            NorthwindApi.StringBaseAddress = Settings.Default.WebApiBaseAddress;
            if (string.IsNullOrWhiteSpace(NorthwindApi.StringBaseAddress))
                throw new ConfigurationErrorsException("无效的 BaseAddress 配置数据.");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}