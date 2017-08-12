using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Mater.Library
{
    public class SiteMap
    {
        public static List<SiteMapUrl> GetSiteMap(string targetDirectory)
        {
            List<SiteMapUrl> urls = new List<SiteMapUrl>();

            // Recurse directories
            ProcessDirectory(targetDirectory, urls);

            // Walk list and convert to url paths
            foreach (SiteMapUrl u in urls)
            {
                u.Path = u.Path.Remove(0, targetDirectory.Length);
                u.Path = u.Path.Replace("\\", "/");
            }

            return urls;
        }

        public static void ProcessDirectory(string targetDirectory, List<SiteMapUrl> urls)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            { 
                ProcessFile(fileName, urls);
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            { 
                ProcessDirectory(subdirectory, urls);
            }
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path, List<SiteMapUrl> urls)
        {
            if (path.EndsWith(".md"))
            {
                urls.Add(new SiteMapUrl() { Path = path, LastModifiedDate = File.GetLastWriteTime(path) });
            }

        }
    }

    public class SiteMapUrl
    {

        [JsonProperty("path")]
        public string Path { get; set; }


        [JsonProperty("lastmod")]
        public DateTime LastModifiedDate { get; set; } 

    }
}
