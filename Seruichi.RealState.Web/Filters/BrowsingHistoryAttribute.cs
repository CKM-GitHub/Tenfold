using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.RealState.Web
{
    public class BrowsingHistoryAttribute : FilterAttribute, IActionFilter
    {
        public static string PREVIOUS_URL = "PREVIOUS_URL";
        public static string CURRENT_URL = "CURRENT_URL";

        public bool Enabled { get; set; }

        public BrowsingHistoryAttribute()
        {
            Enabled = true;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!this.Enabled)
            {
                return;
            }

            var httpContext = filterContext.HttpContext;
            var session = httpContext.Session;
            var request = httpContext.Request;

            if (request.HttpMethod == "POST" || request.IsAjaxRequest())
            {
                return;
            }

            var url = this.GetRequestUrl(httpContext);
            if (session[CURRENT_URL] != null && url == session[CURRENT_URL].ToString())
            {
                return;
            }

            session[PREVIOUS_URL] = session[CURRENT_URL];
            session[CURRENT_URL] = url;
        }

        private string GetRequestUrl(HttpContextBase httpContext)
        {
            var routeData = System.Web.Routing.RouteTable.Routes.GetRouteData(httpContext);
            var controllerName = routeData.Values["controller"].ToString().ToLower();
            var actionName = routeData.Values["action"].ToString().ToLower();
            var queryString = httpContext.Request.QueryString;
            var appPath = httpContext.Request.ApplicationPath;
            if (appPath == "/")
            {
                appPath = "";
            }

            if (queryString.Count == 0)
            {
                return string.Format("{0}/{1}/{2}", appPath, controllerName, actionName);
            }
            else
            {
                return string.Format("{0}/{1}/{2}?{3}", appPath, controllerName, actionName, QuerySerialize(queryString));
            }
        }

        private string QuerySerialize(NameValueCollection queryString)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < queryString.Count; i++)
            {
                sb.AppendFormat("{0}={1}&", queryString.GetKey(i), queryString[i]);
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}