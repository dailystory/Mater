using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Mater.Library;
using System.Net;
using Newtonsoft.Json;

namespace Mater.WebJobs
{
    public class JobIndexContent
    {
        public static async Task RunAsync()
        {
            // Get the Mater site we're going to index
            string site = ConfigurationManager.AppSettings["site"];
            site = site.TrimEnd('/');

            List<SiteMapUrl> urlList = null;
            List<Article> articlesToIndex = new List<Article>();
            Dictionary<string, bool> inSiteMap = new Dictionary<string, bool>();

            // Verify that we have a valid site
            if (string.IsNullOrEmpty(site))
            {
                Console.WriteLine("A \"site\" key in <appSettings> is required.");
                return;
            }

            // 1. Get the list of Mater documents that need to be indexed
            // -----------------------------------------------------------
            urlList = await GetJsonSiteMapAsync(site);

            // 2. Loop through and update files that are new
            // -----------------------------------------------------------
            foreach (SiteMapUrl url in urlList)
            {
                // add to lookup index, this is just for better
                // performance when we call VerifyAndClean
                inSiteMap.Add(url.Path, true);

//                if (url.LastModifiedDate > DateTime.UtcNow.AddDays(-1))
//                {
                    Article article = await FetchArticleAsync(site, url);
                    articlesToIndex.Add(article);
//                }
            }

            // 3. Index articles
            // -----------------------------------------------------------
            await IndexArticlesAsync(articlesToIndex);

            // 4. Check state of index
            // -----------------------------------------------------------
            await VerifyAndCleanIndexAsync(inSiteMap);

        }

        /// <summary>
        /// Walks through the index and ensures the index does not contain
        /// files that are not in the sitemap list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static async Task VerifyAndCleanIndexAsync(Dictionary<string, bool> sitemap)
        {
            // do work to clean azure search here

            // retrieve a list of all paths in azure index

            // compare paths to sitemap

            // remove items from azure index not in sitemap

        }

        /// <summary>
        /// Indexes an Article in Azure search
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        static async Task IndexArticlesAsync(List<Article> articles)
        {
            List<ArticleDocument> list = new List<ArticleDocument>();

            // create the index if it does not exist
            Search.CreateIndex();

            // Convert to ArticleDocument
            foreach (Article a in articles)
            {
                ArticleDocument d = new ArticleDocument();
                d.FromArticle(a);
                list.Add(d);
            }

            await Search.MergeOrUploadAsync(list);
        }

        /// <summary>
        /// Retrieve a Mater Article that needs to be indexed.
        /// </summary>
        /// <param name="site"></param>
        /// <param name="sitemapItem"></param>
        /// <returns></returns>
        static async Task<Article> FetchArticleAsync(string site, SiteMapUrl sitemapItem)
        {
            string jsonFile = "{0}/json/{1}";
            string url = string.Format(jsonFile, site, sitemapItem.Path);
            Article article = null;

            // Get the json file
            try
            {
                using (WebClient client = new WebClient())
                {
                    string json = await client.DownloadStringTaskAsync(url);

                    article = JsonConvert.DeserializeObject<Article>(json);
                }
            }
            catch
            {
                Console.WriteLine("Problem reading or parsing Json from '" + url + "' are you running the latest version of Mater?");
            }

            return article;
        }

        /// <summary>
        /// Returns a list of SiteMapUrl items from the Mater Json Sitemap
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static async Task<List<SiteMapUrl>> GetJsonSiteMapAsync(string url)
        {
            List<SiteMapUrl> list = null;
            string jsonSiteMap = "{0}/json/sitemap";

            // Set SSL certificates
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Get the json sitemap
            try
            {
                using (WebClient client = new WebClient())
                {
                    url = string.Format(jsonSiteMap, url);
                    string json = await client.DownloadStringTaskAsync(url);

                    list = JsonConvert.DeserializeObject<List<SiteMapUrl>>(json);
                }
            }
            catch
            {
                Console.WriteLine("Problem reading or parsing Sitemap Json from '" + url + "' are you running the latest version of Mater?");
            }

            return list;

        }

    }
}
