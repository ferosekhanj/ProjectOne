using System.Web;
using System.Web.Optimization;

namespace ProjectOne
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Styles/Site.css", "~/Content/Styles/Responsive.css"));
        }
    }
}