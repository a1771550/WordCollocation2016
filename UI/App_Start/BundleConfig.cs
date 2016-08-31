using System.Web.Configuration;
using System.Web.Optimization;

namespace UI
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/layout").Include("~/Scripts/jquery-{version}.js").Include("~/Scripts/bootstrap.js").Include("~/Scripts/tinynav.js").Include("~/Scripts/template.js").Include("~/Scripts/socialLinks.js").Include("~/Scripts/jquery-cookie-plugin.js").Include("~/Scripts/timezone.js").Include("~/Scripts/jquery-ui.custom/jquery-ui.js").Include("~/Scripts/suggestion.js").Include("~/Scripts/searchbox_submit.js"));

			//bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
			//			"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.unobtrusive*",
						"~/Scripts/jquery.validate*"));

			//bundles.Add(new ScriptBundle("~/bundles/jqueryval_unobtrusive").Include(
			//			"~/Scripts/jquery.validate.unobtrusive*"));

			//< script src = "@Url.Content("~/ Scripts / checkEmail.js")" ></ script >
			//    < script src = "@Url.Content("~/ Scripts / user_validation.js")" ></ script >
			//    < script src = "@Url.Content("~/ Scripts / MaskedPassword.js")" ></ script >
			//	      < script src = "@Url.Content("~/ Scripts / addUser.js")" ></ script >
			bundles.Add(new ScriptBundle("~/bundles/register").Include("~/Scripts/checkEmail.js").Include("~/Scripts/user_validation.js").Include("~/Scripts/MaskedPassword.js").Include("~/Scripts/addUser.js"));

			bundles.Add(new ScriptBundle("~/bundles/resetPassword").Include("~/Scripts/jquery-{version}.js").Include("~/Scripts/checkEmail.js").Include("~/Scripts/user_validation.js").Include("~/Scripts/MaskedPassword.js").Include("~/Scripts/resetPassword.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			//bundles.Add(new StyleBundle("~/Content/css").Include(
			//		  "~/Content/bootstrap.css",
			//		  "~/Content/site.css"));

			bundles.Add(new StyleBundle("~/bundles/css").Include("~/Content/wisewords/magic-bootstrap.css").Include("~/Content/wisewords/bootstrap.css").Include("~/Content/wisewords/bootstrap-responsive.css").Include("~/Content/wisewords/socialicons.css").Include("~/Content/wisewords/glyphicons.css").Include("~/Content/wisewords/halflings.css").Include("~/Content/wisewords/template.css").Include("~/Content/wisewords/colors/color-classic.css").Include("~/Content/site.css").Include("~/Content/adminLinks.css").Include("~/Content/jquery-ui.css").Include("~/Content/jquery-ui.structure.css").Include("~/Content/jquery-ui.theme.css").Include("~/Content/suggestion.css").Include("~/Content/progress.css"));

			bundles.Add(new StyleBundle("~/bundles/css_debug").Include("~/Content/wisewords/bootstrap.css").Include("~/Content/wisewords/bootstrap-responsive.css").Include("~/Content/wisewords/socialicons.css").Include("~/Content/wisewords/glyphicons.css").Include("~/Content/wisewords/halflings.css").Include("~/Content/wisewords/template.css").Include("~/Content/wisewords/colors/color-classic.css").Include("~/Content/site.css").Include("~/Content/adminLinks.css").Include("~/Content/jquery-ui.css").Include("~/Content/jquery-ui.structure.css").Include("~/Content/jquery-ui.theme.css").Include("~/Content/suggestion.css").Include("~/Content/progress.css"));

			// Code removed for clarity.
			BundleTable.EnableOptimizations = bool.Parse(WebConfigurationManager.AppSettings.Get("EnableBundleOptimizations"));
			//BundleTable.EnableOptimizations = true;
		}
	}
}
