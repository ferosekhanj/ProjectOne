using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectOne
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Error404",
                url: "Error/404",
                defaults: new { controller = "Error", action = "Error404" }
            );
            routes.MapRoute(
                name: "Error500",
                url: "Error/500",
                defaults: new { controller = "Error", action = "Error500" }
            );
            routes.MapRoute(
                name: "LogDate",
                url: "Log/{year}/{month}/{day}",
                defaults: new { controller = "Log", action = "Index" }
            );

            routes.MapRoute(
                name: "ReviewMonth",
                url: "Review/Month/{year}/{month}",
                defaults: new { controller = "Review", action = "Month", year = DateTime.Now.Year, month=DateTime.Now.Month}
            );

            routes.MapRoute(
                name: "ReviewYear",
                url: "Review/Year/{year}",
                defaults: new { controller = "Review", action = "Year", year=DateTime.Now.Year }
            );

            routes.MapRoute(
                name: "ResetPassword",
                url: "Account/ResetPassword/{token}",
                defaults: new { controller = "Account", action = "ResetPassword"}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index"}
            );
        }
    }
}