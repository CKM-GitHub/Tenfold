using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Seruichi.Tenfold.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            FilterConfig.RegisterWebApiFilters(GlobalConfiguration.Configuration.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            StaticCache.SetIniInfo();
            StaticCache.SetMessageCache();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Server != null)
            {
                var ex = Server.GetLastError();
                if (ex != null)
                {
                    if (ex is HttpException)
                    {
                        int statusCode = ((HttpException)ex).GetHttpCode();
                        if (statusCode == (int)HttpStatusCode.BadRequest ||
                            statusCode == (int)HttpStatusCode.Unauthorized ||
                            statusCode == (int)HttpStatusCode.Forbidden ||
                            statusCode == (int)HttpStatusCode.NotFound)
                        {
                            return;
                        }
                    }

                    Logger.GetInstance().Error(ex);

                    if (ex.InnerException != null)
                    {
                        Logger.GetInstance().Error(ex.InnerException);
                    }
                }
            }
        }
    }
}
