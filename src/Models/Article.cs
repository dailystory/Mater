using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Mater
{
	public class Article
	{
		public Dictionary<string, string> Metadata { get; set; }
		public string Body { get; set; }
		public string Markdown { get; set; }
		public string FilePath { get; set; }
        public string EditPath { get; set; }

		public Article()
		{
		}

		public Article(string markdown, string filepath, string editpath)
		{
			Markdown = markdown;
			FilePath = filepath;
            EditPath = editpath;

			ProcessMarkdown();
		}

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

		void ProcessMarkdown()
		{
			if (string.IsNullOrEmpty(Markdown))
				throw new Exception("Markdown data must exist before Article model can be created.");

			// Get metadata from the markdown file
			Metadata = GetMetaData(Markdown);

			// Remove any misc from the markdown file
			string cleanMd = CleanMarkdown(Markdown);

			// Create an instance of MarkdownDeep and set options
			var md = new MarkdownDeep.Markdown();
			md.ExtraMode = true;
			md.SafeMode = false;

			// Perform the markdown tranform and set to Body
			Body = md.Transform(cleanMd);
		}

		Dictionary<string, string> GetMetaData(string md)
		{
			Dictionary<string, string> metadata = new Dictionary<string, string>();

			// Get meta data
            var v = Regex.Match(md, @"(?<=---\r\n).*?(?=\r\n---)", RegexOptions.Singleline);

            // Split the string by newline
            string[] lines = v.Value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

			// Split and trim each line
			for (int i = 0; i < lines.Length; i++)
			{
				string[] line = lines[i].Split(':');

                try { 
    				metadata.Add(line[0], line[1].Trim());
                } catch { }

            }

			return metadata;
		}

		string CleanMarkdown(string markdown)
		{
			// Clean the markdown file
			return Regex.Replace(markdown, @"(---\r\n).*?(\r\n---)", string.Empty, RegexOptions.Singleline);

		}
	}
}
