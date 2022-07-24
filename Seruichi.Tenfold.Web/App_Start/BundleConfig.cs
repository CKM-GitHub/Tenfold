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

            bundles.Add(new StyleBundle("~/Content/css/error").Include(
                        "~/Content/bootstrap/bootstrap.min.css",
                     "~/Content/fonts/font-awesome.min.css",
                     "~/Content/css/style.css",
                     "~/Content/css/index.css"));

            //The bundle hierarchy and the physical folder hierarchy must match
            bundles.Add(new StyleBundle("~/Content/css/application").Include(
                      "~/Content/typeahead/typeahead.custom.css",
                      "~/Content/css/loading.css",
                      "~/Content/css/validation.css",
                       "~/Content/bootstrap/bootstrap.min.css",
                      "~/Content/fonts/font-awesome.min.css",
                      "~/Content/css/jquery.dataTables.min.css",
                      "~/Content/css/tree.css"
                      ));

            bundles.Add(new ScriptBundle("~/Content/scripts/application").Include(
                      "~/Content/typeahead/typeahead.bundle.min.js",
                      "~/Content/scripts/validation_jqueryextend.js",
                      "~/Content/scripts/common.js"));

            //t_login
            bundles.Add(new StyleBundle("~/Content/t_login").Include(
                      "~/Content/fonts/ionicons.min.css",
                      "~/Content/css/Clients-UI.css",
                      "~/Content/css/Contact-Form-Clean.css",
                      "~/Content/css/Login-Form-Clean.css",
                      "~/Content/css/Soft-UI-Aside-Navbar.css",
                      "~/Content/css/style.css"  
                      ));

            bundles.Add(new ScriptBundle("~/bundles/t_login").Include(
                     "~/Scripts/t_login.js"));

            //css common
            bundles.Add(new StyleBundle("~/Content/tenfold_css_common").Include(
                      "~/Content/css/style.css",
                      "~/Content/css/index.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/t_seller_mansion").Include(
                     "~/Scripts/t_seller_mansion.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_seller_list").Include(
                     "~/Scripts/t_seller_list.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_dashboard").Include(
                     "~/Scripts/t_dashboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_seller_assessment").Include(
                  "~/Scripts/t_common.js",
                     "~/Scripts/t_seller_assessment.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_mansion_list").Include(
                     "~/Scripts/t_mansion_list.js"));          

            bundles.Add(new ScriptBundle("~/bundles/t_mansion_new").Include(
                     "~/Scripts/t_mansion_new.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_admin").Include(
                     "~/Scripts/t_admin.js",
                     "~/Scripts/t_common.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_reale_list").Include(
                     "~/Scripts/t_reale_list.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_reale_purchase").Include(
                     "~/Scripts/t_reale_purchase.js",
                     "~/Scripts/t_common.js",
                     "~/Content/scripts/table_header_sort.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_mansion").Include(
                     "~/Scripts/t_mansion.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_reale_asmhis").Include(
                     "~/Scripts/t_reale_asmhis.js",
                      "~/Scripts/t_common.js",
                     "~/Content/scripts/table_header_sort.js" 
                     ));

            bundles.Add(new ScriptBundle("~/bundles/t_assess_guide").Include(
                 "~/Scripts/t_assess_guide.js",
                  "~/Scripts/t_common.js",
                 "~/Content/scripts/table_header_sort.js" 
                 ));

            bundles.Add(new ScriptBundle("~/bundles/t_seller_account").Include(
                     "~/Scripts/t_seller_account.js",
                     "~/Scripts/t_common.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_seller_memo").Include(
                     "~/Scripts/t_seller_memo.js",
                     "~/Scripts/t_common.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_reale_memo").Include(
                     "~/Scripts/t_reale_memo.js",
                     "~/Scripts/t_common.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_reale_account").Include(
                 "~/Scripts/t_reale_account.js",
                 "~/Scripts/t_common.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_seller_new").Include(
                     "~/Scripts/t_seller_new.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_reale_new").Include(
                "~/Scripts/t_reale_new.js",
                "~/Scripts/t_common.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_seller_profile").Include(
                     "~/Scripts/t_seller_profile.js",
                     "~/Scripts/t_common.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_reale_profile").Include(
               "~/Scripts/t_reale_profile.js",
               "~/Scripts/t_common.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_seller_orderdetails").Include(
              "~/Scripts/t_seller_orderdetails.js",
              "~/Content/scripts/table_header_sort.js",
              "~/Scripts/t_common.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_assess_soudan").Include(
                     "~/Scripts/t_assess_soudan.js",
                     "~/Content/scripts/table_header_sort.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_reale_invoice").Include(
                    "~/Scripts/t_reale_invoice.js",
                    "~/Scripts/t_common.js",
                    "~/Content/scripts/table_header_sort.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_assess_list").Include(
                   "~/Scripts/t_assess_list.js",
                   "~/Content/scripts/table_header_sort.js"));

            bundles.Add(new ScriptBundle("~/bundles/t_seller_config").Include(
                   "~/Scripts/t_seller_config.js"));
        }
    }
}
