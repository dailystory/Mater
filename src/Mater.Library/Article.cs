using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;

namespace Mater.Library
{
    public class Article
    {
        string urlPath = string.Empty;
        string title = string.Empty;
        string description = string.Empty;
        string layout = string.Empty;

        [JsonIgnore]
        public Dictionary<string, string> Metadata { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonIgnore]
        public string Markdown { get; set; }

        [JsonIgnore]
        public string FilePath { get; set; }

        [JsonIgnore]
        public string EditPath { get; set; }

        /// <summary>
        /// This returns the URL friendly path of the article relative to the domain
        /// </summary>
        [JsonProperty("path")]
        public string UrlPath
        {
            get
            {
                if (string.IsNullOrEmpty(urlPath))
                {
                    // Remove the articles directory from path
                    if (EditPath.Contains("articles"))
                        urlPath = EditPath.Remove(0, "articles".Length);
                    else
                        urlPath = EditPath;

                    // Fix index paths
                    if (EditPath.IndexOf("index.md") > 0)
                        urlPath = urlPath.Remove(urlPath.IndexOf("index.md"));
                    else
                        urlPath = urlPath.Remove(urlPath.IndexOf(".md"));
                }

                return urlPath;

            }
            set
            {
                urlPath = value;
            }
        }

        [JsonIgnore]
        public string SettingsPath { get; set; }

        [JsonProperty("lastModified")]
        public DateTime LastModified
        {
            get
            {
                if (null == FilePath)
                    return DateTime.UtcNow;

                return System.IO.File.GetLastWriteTime(FilePath);
            }
        }

        [JsonIgnore]
        public Settings SiteSettings
        {
            get
            {
                return Settings.GetSettings(SettingsPath);
            }
        }

        [JsonProperty("title")]
        public string Title
        {
            get
            {
                if (null != Metadata)
                {
                    if (Metadata.ContainsKey("title"))
                    {
                        this.title = Metadata["title"];
                    }
                }

                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        [JsonProperty("description")]
        public string Description
        {
            get
            {
                if (null != Metadata)
                {
                    if (Metadata.ContainsKey("description"))
                    {
                        this.description = Metadata["description"];
                    }
                }

                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        [JsonIgnore]
        public string Layout
        {
            get
            {
                if (!string.IsNullOrEmpty(this.layout))
                    return this.layout;

                if (null != Metadata)
                {
                    if (Metadata.ContainsKey("layout"))
                    {
                        this.layout = Metadata["layout"];
                    }
                } 

                // Still no layout set
                if (string.IsNullOrEmpty(this.layout))
                {
                    // default to article layout
                    this.layout = "_ArticleLayout";
                }

                return this.layout;
            }
            set
            {
                this.layout = value;
            }
        }


        [JsonIgnore]
        public string TableOfContents
        {
            get
            {
                if (Metadata.ContainsKey("toc"))
                {
                    return Metadata["toc"];
                }

                return string.Empty;
            }
        }

        public void ProcessMarkdown()
        {
            if (string.IsNullOrEmpty(Markdown))
                throw new Exception("Markdown data must exist before Article model can be created.");

            // Get metadata from the markdown file
            Metadata = GetJsonMetaData(Markdown);

            // Get the markdown file without json settings
            string cleanMd = RemoveJsonMetaData(Markdown);

            // Create an instance of MarkdownDeep and set options
            var md = new MarkdownDeep.Markdown();
            md.ExtraMode = true;
            md.SafeMode = false;

            // Perform the markdown tranform and set to Body
            Body = md.Transform(cleanMd);
        }

        static int WordCount(string body)
        {
            var text = body.Trim();
            int wordCount = 0, index = 0;

            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                    index++;

                wordCount++;

                // skip whitespace until next word
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                    index++;
            }

            return wordCount;
        }

        public int TimeToReadInMinutes()
        {
            // get a count of the number of words in the body
            int wordCount = WordCount(this.Body);

            // we'll assume a reading speed of 200 words-per-minute
            int readingSpeed = 200;

            double minutes = Math.Round((double)wordCount / (double)readingSpeed);

            return (int)minutes;
        }


        /// <summary>
        /// Process the settings found in the markdown 
        /// </summary>
        /// <param name="md"></param>
        /// <returns></returns>
        static Dictionary<string, string> GetJsonMetaData(string md)
        {
            Dictionary<string, string> metadata = null;

            if (string.IsNullOrEmpty(md))
                throw new ArgumentException("The string containing markdown is empty.");

            if (md.IndexOf('{') != 0 || md.IndexOf('}') == 0)
                return new Dictionary<string, string>();

            // json settings are expected at the beginning of the file
            string json = md.Substring(0, md.IndexOf('}') + 1);

            // deserialized meta data into dictionary
            try
            {
                metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch (JsonReaderException)
            {
#if DEBUG
                throw new Exception("There is a problem with the settings for this page.");
#endif
                // in production we ignore this exception
                metadata = new Dictionary<string, string>();
            }

            return metadata;
        }

        /// <summary>
        /// Remove the json settings from the markdown 
        /// </summary>
        /// <param name="md"></param>
        /// <returns></returns>
        static string RemoveJsonMetaData(string md)
        {
            if (string.IsNullOrEmpty(md))
                throw new ArgumentException("The string containing markdown is empty.");

            if (md.IndexOf('{') != 0 || md.IndexOf('}') == 0)
                return md;

            // Clean the markdown file
            return md.Substring(md.IndexOf('}') + 1);
        }
        
    }
}
