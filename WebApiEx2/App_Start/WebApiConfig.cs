using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiEx2.Controllers;

namespace WebApiEx2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //GlobalConfiguration.Configuration.MessageHandlers.Add(new CertificateAuthHandler());
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
