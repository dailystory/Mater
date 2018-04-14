using System.Web.Optimization;

namespace Mater
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/base/css").Include(
                "~/Content/styles.css"));
        }
    }
}