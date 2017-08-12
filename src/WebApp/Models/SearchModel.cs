using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mater.Library;

namespace Mater
{
    public class SearchModel
    {
        Settings s = null;

        public PagedList<ArticleDocument> SearchResults { get; set; }

        public string SearchText { get; set; }

        public string SettingsPath { get; set; }

        public double SearchTime { get; set; }

        public Settings SiteSettings
        {
            get
            {
                if (null == s)
                {
                    s = Settings.GetSettings(SettingsPath);
                }

                return s;
            }
        }

    }
}