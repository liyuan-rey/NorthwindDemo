﻿using System.Web;
using System.Web.Mvc;

namespace NorthwindDemo.Mvc5App
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}