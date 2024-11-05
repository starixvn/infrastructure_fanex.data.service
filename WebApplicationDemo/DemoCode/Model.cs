namespace WebApplicationDemo.DemoCode;

using Fanex.Data.Repository;

public class Site
{
	public int SiteId { get; set; }

	public string SiteName { get; set; }
}

public class SiteGetAllCriteria : CriteriaBase
{
	public override string GetSettingKey() => "NAP_Site_GetAll";

	public override bool IsValid() => true;
}