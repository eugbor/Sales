using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Core.Entities;
using Core.Managers;

namespace Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected internal void Application_BeginRequest(object sender, EventArgs e)
        {
            // Get objects.
            HttpContext context = base.Context;
            var q = context.Request;

            var statistcs = new Statistic
            {
                Date = DateTime.Now,
                Url = context.Request.Path
            };

            using (var manager = new StatisticManager())
            {
                manager.Save(statistcs);
            }
        }

    }
}
