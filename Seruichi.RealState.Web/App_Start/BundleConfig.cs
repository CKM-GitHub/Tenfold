using System.Web;
using System.Web.Optimization;

namespace Seruichi.RealEstate.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            //The bundle hierarchy and the physical folder hierarchy must match
            bundles.Add(new StyleBundle("~/Content/css/application").Include(
                      "~/Content/typeahead/typeahead.custom.css",
                      "~/Content/css/loading.css",
                      "~/Content/css/validation.css"));

            bundles.Add(new ScriptBundle("~/Content/scripts/application").Include(
                      "~/Content/typeahead/typeahead.bundle.min.js",
                      "~/Content/scripts/validation_jqueryextend.js",
                      "~/Content/scripts/common.js"));

            //t_login
            bundles.Add(new StyleBundle("~/Content/r_login").Include(
                      "~/Content/r_login/bootstrap/css/bootstrap.min.css",
                      "~/Content/r_login/fonts/font-awesome.min.css",
                      "~/Content/r_login/fonts/ionicons.min.css",
                      "~/Content/r_login/css/Clients-UI.css",
                      "~/Content/r_login/css/Contact-Form-Clean.css",
                      "~/Content/r_login/css/Login-Form-Clean.css",
                      "~/Content/r_login/css/Soft-UI-Aside-Navbar.css",
                      "~/Content/r_login/css/style.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/r_login").Include(
                     "~/Scripts/r_login.js"));
        }
    }
}
