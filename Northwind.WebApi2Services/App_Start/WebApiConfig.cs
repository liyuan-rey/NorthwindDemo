// WebApiConfig.cs

namespace Northwind.WebApi2Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Net;
    using System.Net.Http;
    using System.Security;
    using System.Web.Http;
    using Filters;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            GlobalConfiguration.Configuration.Filters.Add(
                new UnhandledExceptionFilterAttribute()
                    .Register<KeyNotFoundException>(HttpStatusCode.NotFound)
                    .Register<SecurityException>(HttpStatusCode.Forbidden)
                    .Register<SqlException>(
                        (exception, request) =>
                        {
                            var sqlException = exception as SqlException;

                            if (sqlException.Number > 50000)
                            {
                                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.BadRequest);
                                response.ReasonPhrase = sqlException.Message.Replace(Environment.NewLine, String.Empty);

                                return response;
                            }
                            return request.CreateResponse(HttpStatusCode.InternalServerError);
                        }
                    )
                );

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi", "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
                );
        }
    }
}