using System.Web;
using System.Web.Optimization;

namespace MikMak.WebFront
{
    public class BundleConfig
    {
        // Pour plus d’informations sur le Bundling, accédez à l’adresse http://go.microsoft.com/fwlink/?LinkId=254725 (en anglais)
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                       "~/Scripts/bootstrap/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery/jquery.unobtrusive*",
                        "~/Scripts/jquery/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/mikmak").Include(
                        "~/Scripts/mikmak.gameSample.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/other/modernizr-*"));

            // Css
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap/css/bootstrap.css"));
            
            bundles.Add(new StyleBundle("~/Content/bootstrapResponsive").Include("~/Content/bootstrap/css/bootstrap-responsive.min.css"));
            
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            

        }
    }
}