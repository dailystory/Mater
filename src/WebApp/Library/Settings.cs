using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Runtime.Caching;

namespace Mater.Library
{
    public class Settings
    {
        public static string CACHE_KEY__SETTINGS = "CACHE_KEY__SETTINGS";

        [JsonProperty("isSearchEnabled")]
        public bool IsSearchEnabled { get; private set; }

        [JsonProperty("editPath")]
        public string EditPath { get; private set; }

        [JsonProperty("logo")]
        public NavigationItem Logo { get; private set; } 

        [JsonProperty("navigation")]
        public List<NavigationItem> Navigation { get; private set; } 

        [JsonProperty("backButton")]
        public NavigationItem BackButton { get; private set; } 

        public static Settings GetSettings(string path)
        {
            if (MemoryCache.Default.Contains(CACHE_KEY__SETTINGS))
                return (Settings) MemoryCache.Default[CACHE_KEY__SETTINGS];

            // Read the file
            string json = System.IO.File.ReadAllText(path);

            Settings s = JsonConvert.DeserializeObject<Settings>(json);

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddDays(1);

            List<string> filePaths = new List<string>();
            filePaths.Add(path);

            policy.ChangeMonitors.Add(new  HostFileChangeMonitor(filePaths));

            MemoryCache.Default.Add(CACHE_KEY__SETTINGS, s, policy);

            return s;
        }

    }

    public class NavigationItem
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("path")]
        public string ImagePath { get; set; }
    }
}