﻿using System.Web.Http;

namespace ComicsLibrary.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "API/{controller}/{action}" /*,*/
                //defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}