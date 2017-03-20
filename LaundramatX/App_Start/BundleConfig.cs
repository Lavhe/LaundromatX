using System.Web;
using System.Web.Optimization;

namespace LaundramatX
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.11.2.min.js",
                        "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/UzzieScripts").Include(
                "~/Scripts/materialize.clockpicker.js",
                "~/Scripts/typer.js",
                "~/Scripts/plugins/croppie.min.js",
            "~/Scripts/LaundramatX.js"));

            bundles.Add(new ScriptBundle("~/bundles/Addons").Include(
                "~/Scripts/materialize.js",
                "~/Scripts/imagesloaded.pkgd.min.js",
                "~/Scripts/masonry.pkgd.min.js",
                "~/Scripts/jquery.geocomplete.min.js",
                "~/Scripts/Addons/3perfect-scrollbar.min.js",
                "~/Scripts/Addons/4prism.js",
                "~/Scripts/Addons/5chart.min.js",
                "~/Scripts/Addons/7jquery.sparkline.min.js",
                "~/Scripts/Addons/8sparkline-script.js",
                "~/Scripts/Addons/12plugins.min.js"
    ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/materialize.min.css"));

            bundles.Add(new StyleBundle("~/Content/UzzieCss").Include(
                "~/Content/jquery-ui.min.css",
                   "~/Content/animate.css",
                   "~/Content/plugins/croppie.css",
                   "~/Content/materialize.clockpicker.css",
                "~/Content/Site.css",
                 "~/Content/Addons/2style.min.css",
                "~/Content/Addons/4perfect-scrollbar.css",
                "~/Content/Addons/5chartist.min.css",
                "~/Content/Addons/6jquery-jvectormap.css",
                "~/Content/Addons/7prism.css"
                ));
            
        }
    }
}
