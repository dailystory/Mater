using System.Web.Mvc;
using System.Web.Routing;

namespace Mater
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Any requests for static files are allowed to pass through
            //routes.IgnoreRoute("{*allfiles}", new { allfiles = @".*\.(css|js|gif|jpg|png)" });

            // return json sitemap this returns all .md paths in the 
            // ~/articles directory and all children. This is used by 
            // search indexer job
            routes.MapRoute(
                name: "XML Sitemap",
                url: "sitemap",
                defaults: new { controller = "SiteMap", action = "Xml" }
            );

            // return json sitemap this returns all .md paths in the 
            // ~/articles directory and all children. This is used by 
            // search indexer job
            routes.MapRoute(
                name: "Json Sitemap",
                url: "JSON/sitemap",
                defaults: new { controller = "SiteMap", action = "Json" }
            );

            // Return JSon versions of a specific article
            routes.MapRoute(
                name: "Json",
                url: "JSON/{*path}",
                defaults: new { controller = "Articles", action = "JSON" }
            );

            // return json sitemap this returns all .md paths in the 
            // ~/articles directory and all children. This is used by 
            // search indexer job
            routes.MapRoute(
                name: "Search",
                url: "search",
                defaults: new { controller = "Articles", action = "Search" }
            );

            // Catch all other requests and redirect them to the main 
            // Article Controller
            routes.MapRoute(
				name: "All",
				url: "{*path}",
				defaults: new { controller = "Articles", action = "GitRDone" }
			);

            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Articles", action = "Article", id = UrlParameter.Optional }
            );

        }
	}
}
