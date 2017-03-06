using Application.Database.Contexts;
using Application.Database.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace Application
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Data.Entity.Database.SetInitializer(new ApplicationDbInitializer());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }

        void Application_EndRequest(object sender, EventArgs e)
        {
            // dispose db context
            var db = HttpContext.Current.Items["ApplicationDbContext"] as ApplicationDbContext;
            if (db != null)
                db.Dispose();
        }
    }
}
