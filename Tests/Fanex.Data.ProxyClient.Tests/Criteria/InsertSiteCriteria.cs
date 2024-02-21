using System.Collections.Generic;

namespace Fanex.Data.ProxyClient.Tests.Criteria
{
    public class InsertSiteCriteria
    {
        private const string UrlTpl = "<SiteURL URL='{0}' Environment='{1}' IsDefault='{2}' IsActive='{3}'></SiteURL>";

        public int SiteId { get; set; }

        public string SiteName { get; set; }

        public string Description { get; set; }

        [SpParam(Ignored = true)]
        public List<string> Urls { get; set; }

        public string SiteUrls => UrlTpl;

        public bool IsActive { get; set; }

        public string Environment { get; set; } = "UAT";

        public string SecretAccessKey { get; set; }

        public string SiteKey { get; set; }

        public string GetSettingKey() => "NAP_Site_InsertWithSiteURL";

        public bool IsValid() => true;
    }
}
