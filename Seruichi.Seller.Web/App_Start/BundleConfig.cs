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
                      "~/Content/css/Soft-UI-Aside-Navbar.css",
                      "~/Content/css/style.css"));

            bundles.Add(new StyleBundle("~/Content/css/application").Include(
                      "~/Content/css/Soft-UI-Aside-Navbar.css",
                      "~/Content/css/style.css",
                      "~/Content/css/loading.css",
                      "~/Content/css/validation.css",
                      "~/Content/typeahead/typeahead.custom.css"
                      ));

            bundles.Add(new ScriptBundle("~/Content/scripts/application").Include(
                      "~/Content/typeahead/typeahead.bundle.min.js",
                      "~/Content/scripts/validation_jqueryextend.js",
                      "~/Content/scripts/common.js"));

            //a_index ----------
            bundles.Add(new StyleBundle("~/Content/css/a_index").Include(
                      "~/Content/css/auctor-index.css"));

            bundles.Add(new ScriptBundle("~/bundles/a_index").Include(
                      "~/Scripts/a_index.js"));

            //a_login ----------
            bundles.Add(new StyleBundle("~/Content/css/a_login").Include(
                      "~/Content/css/Clients-UI.css",
                      "~/Content/css/Contact-Form-Clean.css",
                      "~/Content/css/Login-Form-Clean.css"));

            bundles.Add(new ScriptBundle("~/bundles/a_login").Include(
                      "~/Content/scripts/sha256.js",
                      "~/Scripts/a_login.js"));

            //a_register ----------
            bundles.Add(new StyleBundle("~/Content/css/a_register").Include(
                        "~/Content/css/Contact-Form-Clean.css",
                        "~/Content/css/Contact-FormModal-Contact-Form-with-Google-Map.css",
                        "~/Content/css/Highlight-Blue.css",
                        "~/Content/css/Highlight-Clean.css",
                        "~/Content/css/Login-Form-Clean.css",
                        "~/Content/css/Ludens-Users---2-Register.css",
                        "~/Content/css/Ludens-Users---25-After-Register.css",
                        "~/Content/css/Newsletter-Subscription-Form.css",
                        "~/Content/css/Profile-Edit-Form-1.css",
                        "~/Content/css/Profile-Edit-Form.css",
                        "~/Content/css/Registration-Form-with-Photo.css",
                        "~/Content/css/Clients-UI.css"));

            bundles.Add(new ScriptBundle("~/bundles/a_register").Include(
                      "~/Scripts/a_register.js"));

            //a_mypage_uinfo ----------
            bundles.Add(new StyleBundle("~/Content/css/a_mypage_uinfo").Include(
                        "~/Content/css/Clients-UI.css",
                        "~/Content/css/Ludens-Users---25-After-Register.css",
                        "~/Content/css/Sidebar.css"));

            bundles.Add(new ScriptBundle("~/bundles/a_mypage_uinfo").Include(
                      "~/Scripts/a_mypage_uinfo.js"));

            //a_mailregister ----------
            bundles.Add(new ScriptBundle("~/bundles/a_mailregister").Include(
                      "~/Scripts/a_mailregister.js"));

            //a_contact ----------
            bundles.Add(new ScriptBundle("~/bundles/a_contact").Include(
                      "~/Scripts/a_contact.js"));



            //a_mypage_plus ----------
            bundles.Add(new StyleBundle("~/Content/css/a_mypage_plus").Include(
                        "~/Content/css/font-awesome.min.css",
                        "~/Content/css/Soft-UI-Aside-Navbar.css",
                        "~/Content/css/style.css",
                        "~/Content/css/Clients-UI.css",
                        "~/Content/css/Ludens-Users---25-After-Register.css",
                        "~/Content/css/Sidebar.css"));

            bundles.Add(new ScriptBundle("~/bundles/a_mypage_plus").Include(
                     "~/Scripts/a_mypage_plus.js"));

        }
    }
}
