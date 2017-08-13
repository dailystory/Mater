using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mater.Library;
using Newtonsoft.Json;
using System.Text;

namespace Mater.Controllers
{
    public class SiteMapController : Controller
    {
#if RELEASE
        [OutputCache(Duration = 86400)]
#endif
        public ActionResult Json()
        {
            List<SiteMapUrl> list = Mater.Library.SiteMap.GetSiteMap(Server.MapPath("~/articles/"));

            // Return the view
            return Content(JsonConvert.SerializeObject(list), "application/json");

        }

#if RELEASE
        [OutputCache(Duration = 86400)]
#endif
        public ActionResult Xml()
        {
            // Get the settings, as we need the Site Url
            Settings settings = Settings.GetSettings(Server.MapPath("~/articles/settings.json"));

            // XML Serialization is overkill for this
            // single case where we need to generate XML
            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            xml.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" >");

            List<SiteMapUrl> list = Mater.Library.SiteMap.GetSiteMap(Server.MapPath("~/articles/"));

            // The optional <priority> is not included
            // as all documents have the same relative
            // priority
            string url = "<url><loc>{0}</loc><lastmod>{1}</lastmod><changefreq>{2}</changefreq></url>";

            // Iterate and build the <url> node
            foreach (SiteMapUrl urlItem in list)
            {
                string changeFrequency = "monthly";

                // new documents get priority, defaults to monthly
                if (urlItem.LastModifiedDate > DateTime.UtcNow.AddHours(-24))
                    changeFrequency = "hourly";
                else if (urlItem.LastModifiedDate > DateTime.UtcNow.AddDays(-7))
                    changeFrequency = "daily";
                else if (urlItem.LastModifiedDate > DateTime.UtcNow.AddDays(-30))
                    changeFrequency = "weekly";

                // Construct the full path to the file
                string fullUrlPath = settings.Site + "/" + urlItem.Path;

                string s = string.Format(url, fullUrlPath, urlItem.LastModifiedDate.ToString("yyyy-MM-dd"), changeFrequency);
                xml.Append(s);
            }

            // close the urlset
            xml.Append("</urlset>");

            // Return the view
            return Content(xml.ToString(), "text/xml");
        }

    }
}