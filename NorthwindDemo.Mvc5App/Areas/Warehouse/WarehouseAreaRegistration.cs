﻿using System.Web.Mvc;

namespace NorthwindDemo.Mvc5App.Areas.Warehouse
{
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
            context.MapRoute(
                "Warehouse_default",
                "Warehouse/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}