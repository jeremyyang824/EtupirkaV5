using System.Web.Optimization;

namespace Etupirka.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            //==============================Vendor=================================
            bundles.Add(
               new StyleBundle("~/Bundles/Vendor/Main/css")
                   .Include("~/Content/themes/base/all.css", new CssRewriteUrlTransform())
                   .Include("~/Content/bootstrap.min.css", new CssRewriteUrlTransform())
                   //MSG
                   .Include("~/Content/toastr.min.css", new CssRewriteUrlTransform())
                   //.Include("~/Content/sweetalert.css", new CssRewriteUrlTransform())
                   .Include("~/Content/sweetalert2.css", new CssRewriteUrlTransform())
                   //Plugin
                   .Include("~/Content/iCheck/all.css", new CssRewriteUrlTransform())
                   .Include("~/Scripts/jstree/themes/default/style.css", new CssRewriteUrlTransform())
                   //ICON
                   .Include("~/Content/flags/famfamfam-flags.css", new CssRewriteUrlTransform())
                   .Include("~/Content/font-awesome.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/ionicons.min.css", new CssRewriteUrlTransform())
               );

            bundles.Add(
                new ScriptBundle("~/Bundles/Vendor/Main/js")
                    .Include(
                        "~/Abp/Framework/scripts/utils/ie10fix.js",
                        //"~/Scripts/es6-promise/es6-promise.min.js",
                        "~/Scripts/bluebird/bluebird.min.js",
                        "~/Scripts/json2.min.js",
                        "~/Scripts/underscore.min.js",

                        "~/Scripts/modernizr-2.8.3.js",

                        "~/Scripts/jquery-2.2.4.min.js",
                        "~/Scripts/jquery-ui-1.12.1.min.js",
                        "~/Scripts/jquery.icheck.min.js",

                        "~/Scripts/bootstrap.min.js",

                        "~/Scripts/moment-with-locales.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.i18n.zh.js",
                        "~/Scripts/jquery.blockUI.js",
                        "~/Scripts/toastr.min.js",
                        "~/Scripts/jquery.slimscroll.min.js",

                        //"~/Scripts/sweetalert/sweetalert.min.js",
                        "~/Scripts/sweetalert2/sweetalert2.js",
                        "~/Scripts/spinjs/spin.min.js",
                        "~/Scripts/spinjs/jquery.spin.js",
                        "~/Scripts/jstree/jstree.js",

                        "~/Abp/Framework/scripts/abp.js",
                        "~/Abp/Framework/scripts/libs/abp.jquery.js",
                        "~/Abp/Framework/scripts/libs/abp.toastr.js",
                        "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                        "~/Abp/Framework/scripts/libs/abp.spin.js",
                        "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",

                        "~/Scripts/jquery.signalR-2.2.1.min.js"
                    )
                );


            //==============================Vendor.AngularJS=========================

            bundles.Add(
                new StyleBundle("~/Bundles/Vendor/Angular/css")
                    .Include("~/Content/ui-grid.min.css", new CssRewriteUrlTransform())
                    //angular-loading-bar
                    .Include("~/Content/loading-bar.min.css", new CssRewriteUrlTransform())
                );

            bundles.Add(
                 new ScriptBundle("~/Bundles/Vendor/Angular/js")
                     .Include(
                         //"~/Scripts/angular.min.js",
                         //"~/Scripts/angular-animate.min.js",
                         //"~/Scripts/angular-sanitize.min.js",
                         //"~/Scripts/angular-ui-router.min.js",
                         //"~/Scripts/angular-ui/ui-bootstrap.min.js",
                         //"~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                         //"~/Scripts/angular-ui/ui-utils.min.js",
                         "~/Scripts/angular.js",
                         "~/Scripts/angular-animate.js",
                         "~/Scripts/angular-sanitize.js",
                         "~/Scripts/angular-ui-router.js",
                         "~/Scripts/angular-ui/ui-bootstrap.js",
                         "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                         "~/Scripts/angular-ui/ui-utils.js",
                         "~/Scripts/i18n/angular-locale_zh-cn.js",

                         "~/Scripts/ui-grid.min.js",
                         "~/Scripts/ng-file-upload-all.min.js",
                         "~/Scripts/ng-file-upload-shim.min.js",

                         //angular-loading-bar
                         //"~/Scripts/loading-bar.min.js",
                         "~/Scripts/loading-bar.js",

                         "~/Abp/Framework/scripts/libs/angularjs/abp.ng.js"
                     )
                 );



            //============================Site===================================
            bundles.Add(
                new StyleBundle("~/Bundles/Site/css")
                   //AdminLTE
                   .Include("~/Content/AdminLTE.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/skins/skin-tobacco.css", new CssRewriteUrlTransform())
                   //Site Custom
                   .Include("~/Content/site.css", new CssRewriteUrlTransform())
                );

            bundles.Add(
               new ScriptBundle("~/Bundles/Site/js")
                   .Include(
                        //"~/Scripts/admin-lte/app.min.js",
                        "~/Scripts/admin-lte/app.js",
                        "~/Scripts/_site-common.js")    //Site Common
               );


            //============================App.Main===================================
            bundles.Add(
                new StyleBundle("~/Bundles/App/Main/css")
                    .IncludeDirectory("~/App/Main", "*.css", true)
                );

            //~/Bundles/App/Main/js
            bundles.Add(
               new ScriptBundle("~/Bundles/App/Main/js")
                   .IncludeDirectory("~/App/Main", "*.js", true)
               );

        }
    }
}