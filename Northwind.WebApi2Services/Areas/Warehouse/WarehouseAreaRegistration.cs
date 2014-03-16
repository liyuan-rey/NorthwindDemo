using System.Web.Mvc;

namespace Northwind.WebApi2Services.Areas.Warehouse
{
    using System.Web.Http;

    public class WarehouseAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Warehouse";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                "WarehouseApi",
                "api/Warehouse/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
            //context.MapRoute(
            //    "Warehouse_default",
            //    "Warehouse/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}