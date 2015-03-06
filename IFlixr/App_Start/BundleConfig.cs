using System.Web;
using System.Web.Optimization;

namespace IFlixr
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/fancybox").Include(
                "~/Scripts/jquery.easing.1.3.js",
                "~/Scripts/jquery.fancybox.js",  
                "~/Scripts/jquery.fancybox-buttons.js",  
                "~/Scripts/jquery.fancybox-media.js",  
                "~/Scripts/jquery.fancybox-thumbs.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/iflixr").Include(
                "~/Scripts/jquery.carouFredSel-6.1.0.js",      
                "~/Scripts/jquery.isotope.js",
                "~/Scripts/jquery.infinitescroll.js",
                "~/Scripts/jquery.qtip.js",
                "~/Scripts/chosen.jquery.js",
                "~/Scripts/chosen.jquery.js",
                "~/Scripts/jquery.lazyload.js",
                "~/Scripts/iFlixr.js"

                      ));
         
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                  "~/Content/jquery.fancybox.css",
                "~/Content/jquery.fancybox-thumbs.css",
                "~/Content/jquery.fancybox-buttons.css",
                "~/Content/chosen.css",
                "~/Content/chosen.dark.css",
                "~/Content/jquery.qtip.css"
                ));
            

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}