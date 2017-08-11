using System;
namespace Mater
{
	public class ArticleToc
	{
		public string Body { get; set; }
		public string Markdown { get; set; }

		public ArticleToc()
		{
		}

		public ArticleToc(string tocMd)
		{
			Markdown = tocMd;
			ProcessMarkdown();
		}

		void ProcessMarkdown()
		{
			if (string.IsNullOrEmpty(Markdown))
				throw new Exception("Markdown data must exist before Article model can be created.");

			// Create an instance of MarkdownDeep and set options
			var md = new MarkdownDeep.Markdown();
			md.ExtraMode = true;
			md.SafeMode = false;

			// Perform the markdown tranform and set to Body
			Body = md.Transform(Markdown);
		}
	}
}
