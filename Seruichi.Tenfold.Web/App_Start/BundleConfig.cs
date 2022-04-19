using System.Web;
using System.Web.Optimization;

namespace Seruichi.Tenfold.Web
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
            bundles.Add(new StyleBundle("~/Content/t_login").Include(
                      "~/Content/t_login/bootstrap/css/bootstrap.min.css",
                      "~/Content/t_login/fonts/font-awesome.min.css",
                      "~/Content/t_login/fonts/ionicons.min.css",
                      "~/Content/t_login/css/Clients-UI.css",
                      "~/Content/t_login/css/Contact-Form-Clean.css",
                      "~/Content/t_login/css/Login-Form-Clean.css",
                      "~/Content/t_login/css/Soft-UI-Aside-Navbar.css",
                      "~/Content/t_login/css/style.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/t_login").Include(
                     "~/Scripts/t_login.js"));

            //t_seller_mansion
            bundles.Add(new StyleBundle("~/Content/t_seller_mansion").Include(
                      "~/Content/t_dashboard/bootstrap/css/bootstrap.min.css",
                      "~/Content/fonts/font-awesome.min.css",
                      "~/Content/t_dashboard/css/style.css",
                      "~/Content/t_dashboard/css/index.css",
                      "~/Content/css/validation.css",
                      "~/Content/css/loading.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/t_seller_mansion").Include(
                     "~/Scripts/t_seller_mansion.js"));

            //t_seller_list
            bundles.Add(new StyleBundle("~/Content/t_seller_list").Include(
                      "~/Content/t_seller_list/bootstrap/css/bootstrap.min.css",
                      "~/Content/t_seller_list/fonts/font-awesome.min.css",
                      "~/Content/t_seller_list/css/style.css",
                      "~/Content/t_seller_list/css/index.css",
                      "~/Content/css/validation.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/t_seller_list").Include(
                     "~/Scripts/t_seller_list.js"));

            //t_dashboard
            bundles.Add(new StyleBundle("~/Content/t_dashboard").Include(
                      "~/Content/t_dashboard/bootstrap/css/bootstrap.min.css",
                      "~/Content/fonts/font-awesome.min.css",
                      "~/Content/t_dashboard/css/style.css",
                      "~/Content/t_dashboard/css/index.css",
                      "~/Content/t_dashboard/css/jquery.dataTables.min.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/t_dashboard").Include(
                     "~/Scripts/t_dashboard.js"));


        }
    }
}
