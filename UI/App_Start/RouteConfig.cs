using System.Web.Mvc;
using System.Web.Routing;

namespace UI
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
	"JSError", // Route name
	"Error/JSErrorHandler",
	new { controller = "Error", action = "JSErrorHandler" }
);

			routes.MapRoute("CustomError", "Error/Index", new {controller = "Error", action = "Index"});

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);


		}
	}
}
