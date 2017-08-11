using System;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Mater.Controllers
{
    public class ArticlesController : Controller
    {
		/// <summary>
		/// Renders a markdown article from a requested path
		/// </summary>
		public ActionResult Article(string path)
		{
			string mdFile = string.Empty;
			string mdPath = string.Empty;
            string editPath = string.Empty;

			try {
				// Load the requested mark down file
				mdPath = GetMarkdownPath(Response, path);

				mdFile = System.IO.File.ReadAllText(mdPath);

                editPath = mdPath.Replace(Server.MapPath("~/"), string.Empty).Replace("\\", "/");
			}
            catch (Exception)
            {
                // redirect to 404
                return View("~/Views/Home/404.cshtml");
            }

            // Create an instance of the Article model
            Article article = new Article(mdFile, mdPath, editPath);

			return View("~/Views/Articles/Index.cshtml", article);
		}

		public string GetMarkdownPath(HttpResponseBase Response, string path)
		{

			// Root, e.g. "/"
			if (null == path)
			{
				return Server.MapPath("~/articles/index.md");
			}

			// Directory, e.g. "/home/"
			if (path.EndsWith("/", StringComparison.InvariantCulture))
			{
				return Server.MapPath("~/articles/" + path + "index.md");
			}

			// Directory or file, e.g. "/home"
			if (System.IO.File.Exists(Server.MapPath("~/articles/" + path + "/index.md")))
			{
				// 301
				Response.RedirectPermanent("/" + path + "/", true);
			}
			else if (System.IO.File.Exists(Server.MapPath("~/articles/" + path + ".md"))) {
				return Server.MapPath("~/articles/" + path + ".md");
			}

			throw new System.IO.FileNotFoundException("No directories or files found for " + path);
		}

		/// <summary>
		/// Renders a table of contents
		/// </summary>
		[ChildActionOnly]
		public PartialViewResult _ArticleTOC(Article article)
		{
			string mdFile = string.Empty;
			ArticleToc toc = null;

			try
			{
				// attempt to get a TOC.md file
				mdFile = System.IO.File.ReadAllText(Server.MapPath("~/articles/TOC.md"));

				// Create an instance of the ArticleToc model
				toc = new ArticleToc(mdFile);

			}
            catch (Exception)
			{
				return PartialView("~/Views/Shared/_ArticleTOC.cshtml");
			}

			return PartialView("~/Views/Shared/_ArticleTOC.cshtml", toc);
		} 
    }
}
