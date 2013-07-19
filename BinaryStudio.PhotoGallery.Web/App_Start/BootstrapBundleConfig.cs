using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(BinaryStudio.PhotoGallery.Web.App_Start.BootstrapBundleConfig), "RegisterBundles")]

namespace BinaryStudio.PhotoGallery.Web.App_Start
{
	public class BootstrapBundleConfig
	{
		public static void RegisterBundles()
		{
			// Add @Styles.Render("~/Content/bootstrap") in the <head/> of your _Layout.cshtml view
			// Add @Scripts.Render("~/bundles/bootstrap") after jQuery in your _Layout.cshtml view
			// When <compilation debug="true" />, MVC4 will render the full readable version. When set to <compilation debug="false" />, the minified version will be rendered automatically
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/scripts/jquery").Include(
                "~/Scripts/jquery-{version}.js"
                ));
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/scripts/bootstrap").Include(
                "~/Scripts/bootstrap.js"
                ));
            BundleTable.Bundles.Add(new StyleBundle("~/bundles/styles/bootstrap").Include(
                "~/Content/style-libs/bootstrap.css",
                "~/Content/style-libs/bootstrap-responsive.css"
                ));
		}
	}
}