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
                      "~/Content/css/validation.css",
                       "~/Content/bootstrap/bootstrap.min.css",
                      "~/Content/fonts/font-awesome.min.css"
                      ));

            bundles.Add(new ScriptBundle("~/Content/scripts/application").Include(
                      "~/Content/typeahead/typeahead.bundle.min.js",
                      "~/Content/scripts/validation_jqueryextend.js",
                      "~/Content/scripts/common.js"));

            //t_login
            bundles.Add(new StyleBundle("~/Content/r_login").Include(
                       "~/Content/fonts/ionicons.min.css",
                      "~/Content/css/Clients-UI.css",
                      "~/Content/css/Contact-Form-Clean.css",
                      "~/Content/css/Login-Form-Clean.css",
                      "~/Content/css/Soft-UI-Aside-Navbar.css",
                      "~/Content/css/style.css"
                      ));

            //css common
            bundles.Add(new StyleBundle("~/Content/realEstate_css_common").Include(
                      "~/Content/css/style.css",
                      "~/Content/css/index.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/r_login").Include(
                     "~/Scripts/r_login.js"));

            bundles.Add(new ScriptBundle("~/bundles/r_contact").Include(
                    "~/Scripts/r_contact.js"));

            bundles.Add(new ScriptBundle("~/bundles/r_dashboard").Include(
                    "~/Scripts/r_dashboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/r_issueslist").Include(
                    "~/Scripts/r_issueslist.js",
                    "~/Content/scripts/sidebar.js",
                    "~/Content/scripts/table_header_sort.js"));
        }
    }
}
