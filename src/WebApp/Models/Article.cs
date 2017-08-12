using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Mater.Library;

namespace Mater
{
	public class Article
	{
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

        [JsonIgnore]
        public string SettingsPath { get; set; }

        [JsonProperty("lastModified")]
        public DateTime LastModified
        {
            get
            {
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
				if (Metadata.ContainsKey("title"))
				{
					return Metadata["title"];
				}

				return string.Empty;
			}
		}

        [JsonProperty("description")]
        public string Description
		{
			get
			{
				if (Metadata.ContainsKey("description"))
				{
					return Metadata["description"];
				}

				return string.Empty;
			}
		}

        [JsonIgnore]
        public string Layout
		{
			get
			{
				if (Metadata.ContainsKey("layout"))
				{
					return Metadata["layout"];
				}

				return "_ArticleLayout";
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
