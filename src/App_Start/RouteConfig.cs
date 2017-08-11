using System.Web.Mvc;
using System.Web.Routing;

namespace Mater
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*allfiles}", new { allfiles = @".*\.(css|js|gif|jpg|png)" });

			routes.MapRoute(
				name: "All",
				url: "{*path}",
				defaults: new { controller = "Articles", action = "Article" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{action}/{id}",
				defaults: new { controller = "Articles", action = "Article", id = UrlParameter.Optional }
			);

		}
	}
}
