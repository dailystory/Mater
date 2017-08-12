using System;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;
using System.Text.RegularExpressions;

namespace Mater.Library
{
    [SerializePropertyNamesAsCamelCase]
    public class ArticleDocument
    {
        [IsSearchable]
        public string Body { get; set; }

        [System.ComponentModel.DataAnnotations.Key]
        [IsSearchable]
        public string Key
        {
            get
            {
                return Path.GetHashCode().ToString();
            }
            set
            {
                // ignore
            }
        }

        [IsSearchable, IsFilterable]
        public string Path {get;set;}

        [IsSortable]
        public DateTime LastModified { get; set; }

        [IsSearchable]
        public string Title { get; set; }

        [IsSearchable]
        public string Description { get; set; }

        public void FromArticle(Article a)
        {
            this.Body = a.Body;
            this.Path = a.UrlPath;
            this.LastModified = a.LastModified;
            this.Title = a.Title;
            this.Description = a.Description;
        }

        public string Excerpt(int size, bool includeEllipsis = true)
        {
            string excerpt = Regex.Replace(Body, "<.*?>", String.Empty);

            excerpt = excerpt.Substring(0, size);

            if (includeEllipsis)
                excerpt += " ...";

            return excerpt;
        }

        public static async Task DeleteArticlesAsync(string[] ids)
        {
            // Get the index client for the contacts index
            ISearchIndexClient indexClient = Search.GetSearchServiceClient().Indexes.GetClient(Search.INDEX_NAME);

            // create the batch
            var batch = IndexBatch.Delete("path", ids);

            await indexClient.Documents.IndexAsync(batch);
        }

        public static async Task<PagedList<ArticleDocument>> GetArticleDocumentsAsync(string search, int pageIndex, int pageSize)
        {
            if (pageIndex < 0)  // Force valid pageindex
                pageIndex = 0;

            if (pageSize < 1)  // Force valid pagesize
                pageSize = 1;

            PagedList<ArticleDocument> articles = new PagedList<ArticleDocument>();

            DocumentSearchResult<ArticleDocument> results;
            ISearchIndexClient indexClient = Search.GetSearchIndexClient();

            // all results filtered to tenant and status
            SearchParameters parameters = new SearchParameters()
            {
                IncludeTotalResultCount = true,
                OrderBy = new string[] { "lastModified" }
            };

            if (string.IsNullOrEmpty(search))
                search = "*";

            // handling paging
            parameters.Top = pageSize;
            if (pageIndex > 0)
            {
                parameters.Skip = pageIndex * pageSize;
            }

            // Get results
            results = await indexClient.Documents.SearchAsync<ArticleDocument>(search, parameters);

            // Setup the PagedList
            articles.PageIndex = pageIndex;
            articles.PageSize = pageSize;
            articles.TotalCount = (int)results.Count;

            // build the return object
            foreach (SearchResult<ArticleDocument> result in results.Results)
            {
                articles.Add(result.Document);
            }

            return articles;
        }
    }
}
