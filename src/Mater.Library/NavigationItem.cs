using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mater.Library
{
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
