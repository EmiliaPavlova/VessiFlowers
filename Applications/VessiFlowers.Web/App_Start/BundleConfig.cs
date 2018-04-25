namespace VessiFlowers.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/style").Include(
                "~/Content/custom-bootstrap.css",
                "~/Content/fullcalendar.css",
                "~/Content/style.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-3.3.1.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                "~/Scripts/script.js"
                ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
