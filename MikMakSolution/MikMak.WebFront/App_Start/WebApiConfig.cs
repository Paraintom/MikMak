using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;
using MikMak.WebFront.Formatter;

namespace MikMak.WebFront
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "Game/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional}
            );
            config.Routes.MapHttpRoute(
                name: "Connect",
                routeTemplate: "api/{action}",
                defaults: new { 
                    controller = "Mikmak"
                }
            );

            // Custom customization
            config.Formatters.Clear();
            config.Formatters.Add(new JsonpMediaTypeFormatter());
        }
    }
}
