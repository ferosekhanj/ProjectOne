﻿using ProjectOne.Models;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectOne
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Initialize before the membership provider creates the tables
            using (var d = new DatabaseContext())
            {
                d.Database.Initialize(false);
            }
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            MembershipConfig.RegisterDatabase();
        }
    }
}