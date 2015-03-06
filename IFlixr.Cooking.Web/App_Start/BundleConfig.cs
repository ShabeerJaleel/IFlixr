﻿using System.Web;
using System.Web.Optimization;

namespace IFlixr.Cooking.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                "~/Content/js/modernizr.custom.26633.js",
                        "~/Content/js/jquery.js",
                         "~/Content/js/easing.min.js",
                         "~/Content/js/touch-swipe.js",
                         "~/Content/js/boostrap.min.js",
                         //"~/Content/js/flexnav.min.js",
                         //"~/Content/js/countdown.min.js",
                         //"~/Content/js/magnific.min.js",
                         //"~/Content/js/mediaelement.min.js",
                         /*"~/Content/js/fitvids.min.js",*/
                          /*"~/Content/js/gridrotator.min.js",*/
                         /*"~/Content/js/fredsel.min.js",*/
                         //"~/Content/js/backgroundsize.min.js",
                         //"~/Content/js/superslides.min.js",
                          //"~/Content/js/one-page-nav.min.js",
                         "~/Content/js/scroll-to.js"
                         //"~/Content/js/gmap3.min.js",
                         //"~/Content/js/tweet.min.js",
                         //"~/Content/js/mixitup.min.js",
                         //"~/Content/js/mail.min.js",
                         //"~/Content/js/transit-modified.js",
                         //"~/Content/js/layerslider-transitions.min.js",
                         //"~/Content/js/layerslider.js",
                         /*"~/Content/js/custom.js"*/));




            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/fancybox").Include(
            //    "~/Scripts/jquery.easing.1.3.js",
            //    "~/Scripts/jquery.fancybox.js",  
            //    "~/Scripts/jquery.fancybox-buttons.js",  
            //    "~/Scripts/jquery.fancybox-media.js",  
            //    "~/Scripts/jquery.fancybox-thumbs.js"
            //    ));



            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //    "~/Content/site.css",
            //      "~/Content/jquery.fancybox.css",
            //    "~/Content/jquery.fancybox-thumbs.css",
            //    "~/Content/jquery.fancybox-buttons.css",
            //    "~/Content/chosen.css",
            //    "~/Content/chosen.dark.css",
            //    "~/Content/jquery.qtip.css"
            //    ));
            
            //BundleTable.EnableOptimizations = true;
        }
    }
}