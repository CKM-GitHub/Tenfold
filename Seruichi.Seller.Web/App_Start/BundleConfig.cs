using System.Web;
using System.Web.Optimization;

namespace Seruichi.Seller.Web
{
    public class BundleConfig
    {
        // バンドルの詳細については、https://go.microsoft.com/fwlink/?LinkId=301862 を参照してください
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// 開発と学習には、Modernizr の開発バージョンを使用します。次に、実稼働の準備が
            //// 運用の準備が完了したら、https://modernizr.com のビルド ツールを使用し、必要なテストのみを選択します。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));


            //The bundle hierarchy and the physical folder hierarchy must match
            bundles.Add(new StyleBundle("~/Content/css/error").Include(
                      "~/Content/bootstrap/Soft-UI-Aside-Navbar.css",
                      "~/Content/css/errorpage.css"));

            bundles.Add(new StyleBundle("~/Content/css/application").Include(
                      "~/Content/bootstrap/Soft-UI-Aside-Navbar.css",
                      "~/Content/typeahead/typeahead.custom.css",
                      "~/Content/css/style.css",
                      "~/Content/css/loading.css",
                      "~/Content/css/validation.css"));

            bundles.Add(new ScriptBundle("~/Content/scripts/application").Include(
                      "~/Content/typeahead/typeahead.bundle.min.js",
                      "~/Content/scripts/validation_jqueryextend.js",
                      "~/Content/scripts/common.js"));

            //a_index
            bundles.Add(new StyleBundle("~/Content/css/a_index").Include(
                      "~/Content/css/auctor-index.css"));

            bundles.Add(new ScriptBundle("~/bundles/a_index").Include(
                      "~/Scripts/a_index.js"));

            //t_login
            bundles.Add(new ScriptBundle("~/bundles/t_login").Include(
                     "~/Scripts/t_login.js"));

            //t_seller_list
            bundles.Add(new ScriptBundle("~/bundles/t_seller_list").Include(
                     "~/Scripts/t_seller_list.js"));


            //a_login
            bundles.Add(new StyleBundle("~/Content/css/a_login").Include(
                      "~/Content/css/Clients-UI.css",
                      "~/Content/css/Contact-Form-Clean.css",
                      "~/Content/css/Login-Form-Clean.css"));

            bundles.Add(new ScriptBundle("~/bundles/a_login").Include(
                      "~/Scripts/a_login.js"));

        }
    }
}
