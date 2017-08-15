using System;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Mater.Library;

namespace Mater.Controllers
{
    public class ArticlesController : Controller
    {
		/// <summary>
		/// Main Controller method that handles all requests
		/// </summary>
		public ActionResult GitRDone(string path)
		{
            Article article = null;

            try
            {
                article = LoadArticle(path);
            } catch
            {
                // redirect to 404
                return View("~/Views/Home/404.cshtml");

            }

            // Return the view
            return View("~/Views/Articles/Index.cshtml", article);
		}

        public ActionResult JSON(string path)
        {
            Article article = null;

            try
            {
                article = LoadArticle(path);
            }
            catch
            {
                // redirect to 404
                return View("~/Views/Home/404.cshtml");

            }

            // Return the view
            return Content(JsonConvert.SerializeObject(article), "application/json");

        }

        public ActionResult Amp(string path)
        {
            Article article = null;

            try
            {
                article = LoadArticle(path);
            }
            catch
            {
                // redirect to 404
                return View("~/Views/Home/404.cshtml");

            }

            // set the layout to Amp
            article.Layout = "_AmpLayout";

            // Return the view
            return View("~/Views/Articles/Index.cshtml", article);

        }

        public async Task<ActionResult> Search(string s, int i = 0, int ps = 25)
        {

            SearchModel m = new SearchModel() { SettingsPath = Server.MapPath("~/articles/settings.json") };
            m.SearchText = s;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            m.SearchResults = await ArticleDocument.GetArticleDocumentsAsync(s, i, ps);
            watch.Stop();

            m.SearchTime = watch.Elapsed.TotalSeconds;

            return View("~/Views/Articles/Search.cshtml", m);
        }


        Article LoadArticle(string path)
        {
            Article article = null;

            // Reference to markdown file
            string mdFile = string.Empty;

            // Reference to markdown path
            string mdPath = string.Empty;

            // Attempt to load the requested markdown
            // file for the path that was asked for

            mdPath = GetMarkdownPath(Response, path);

            // Read the physical file into Mater
            mdFile = System.IO.File.ReadAllText(mdPath);
#if RELEASE
            // Only enable output caching in release mode

            // Add file dependency for output cache
            Response.AddFileDependency(mdPath);

            // Set additional properties to enable caching.
            Response.Cache.SetExpires(DateTime.Now.AddDays(1));
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetValidUntilExpires(true);
#endif

            // Create an instance of the Article model
            article = new Article()
            {
                Markdown = mdFile,
                FilePath = mdPath,
                EditPath = mdPath.Replace(Server.MapPath("~/"), string.Empty).Replace("\\", "/"),
                SettingsPath = Server.MapPath("~/articles/settings.json")
            };

            // Process the article markdown
            article.ProcessMarkdown();

            return article;
        }

        /// <summary>
        /// Attempt to determine the file path to the requested markdown file
        /// the path could be part of the same Mater directory or could be a
        /// virtual directory mapped in IIS or Azure
        /// </summary>
        /// <param name="Response">ASP.NET Response object.</param>
        /// <param name="path">Web path used for finding the requested md file</param>
        /// <returns></returns>
        string GetMarkdownPath(HttpResponseBase Response, string path)
		{

			// Root, e.g. "/"
			if (null == path)
			{
				return Server.MapPath("~/articles/index.md");
			}

			// Directory, e.g. "/home/" attempts to find an index.md file
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
		/// Renders a table of contents for the
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
