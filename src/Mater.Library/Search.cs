using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Mater.Library
{
    public class Search
    {
        public static string INDEX_NAME = "mater";

        public static SearchServiceClient GetSearchServiceClient()
        {
            SearchServiceClient s = null;
            string serviceName = Environment.GetEnvironmentVariable("AZURE_SEARCH_SERVICE_NAME");
            string adminApiKey = Environment.GetEnvironmentVariable("AZURE_SEARCH_ADMIN_API_KEY");

            return new SearchServiceClient(serviceName, new SearchCredentials(adminApiKey));
        }

        public static ISearchIndexClient GetSearchIndexClient()
        {
            string serviceName = Environment.GetEnvironmentVariable("AZURE_SEARCH_SERVICE_NAME");
            string apiKey = Environment.GetEnvironmentVariable("AZURE_SEARCH_API_KEY");

            return new SearchIndexClient(serviceName, INDEX_NAME, new SearchCredentials(apiKey));
        }

        /// <summary>
        /// Main method to perform updates inserts for Articles
        /// </summary>
        /// <param name="forceUpdate">Forces an update of all records. Used when rebuilding an index</param>
        /// <returns></returns>
        public static async Task MergeOrUploadAsync(List<ArticleDocument> articles)
        {
            // Do we have results? If not, return
            if (articles.Count == 0) return;

            // Get the service client
            SearchServiceClient serviceClient = GetSearchServiceClient();

            // Get the index to update
            ISearchIndexClient indexClient = serviceClient.Indexes.GetClient(INDEX_NAME);

            var updateBatch = IndexBatch.MergeOrUpload(articles);

            // Index the documents that were uploaded
            try
            {
                if (articles.Count > 0)
                    await indexClient.Documents.IndexAsync(updateBatch);

            }
            catch (IndexBatchException)
            {
                Console.WriteLine("Indexing failed. Try again later.");
                return;
            }

        }

        /// <summary>
        /// Used to create the mater index
        /// </summary>
        /// <param name="serviceClient"></param>
        public static void CreateIndex()
        {
            SearchServiceClient serviceClient = GetSearchServiceClient();

            if (serviceClient.Indexes.Exists(INDEX_NAME))
                return;

            var definition = new Index()
            {
                Name = INDEX_NAME,
                Fields = FieldBuilder.BuildForType<ArticleDocument>()
            };

            serviceClient.Indexes.Create(definition);

        }

        /// <summary>
        /// Used to delete the mater index
        /// </summary>
        /// <param name="serviceClient"></param>
        private static void DeleteIndexIfExists()
        {
            SearchServiceClient serviceClient = GetSearchServiceClient();

            if (serviceClient.Indexes.Exists(INDEX_NAME))
            {
                serviceClient.Indexes.Delete(INDEX_NAME);
            }
        }

    }
}
