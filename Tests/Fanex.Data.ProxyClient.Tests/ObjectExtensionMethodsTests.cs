namespace Fanex.Data.ProxyClient.Tests;

using Criteria;
using Data.Extensions;
using Xunit;
using Xunit.Asserts.Compare;

[Collection("Fanex Data Client collection")]
public class ObjectExtensionMethodsTests
{
    [Fact]
    public void CopyValueFrom_OnlyCopyPropertiesWhichAreNotIgnored()
    {
        // Arrange.
        var criteria = new InsertSiteCriteria
        {
            SiteId = 1,
            SiteName = "SiteName",
            SiteKey = "SiteKey",
            Description = "Description",
            Environment = "UAT",
            IsActive = true,
            SecretAccessKey = "SecretAccessKey",
            Urls = ["Urls"]
        };

        var parentCriteria = new InsertSiteCriteria
        {
            SiteId = 2,
            SiteName = "NewSiteName",
            SiteKey = "NewSiteKey",
            Description = null,
            Environment = "PROD",
            IsActive = false,
            SecretAccessKey = "NewSecretAccessKey",
            Urls = ["google.com"]
        };


        // Act.
        criteria.CopyValueFrom(parentCriteria);

        // Assert.
        Assert.Equal(2, criteria.SiteId);
        Assert.Equal("NewSiteName", criteria.SiteName);
        Assert.Equal("NewSiteKey", criteria.SiteKey);
        Assert.Equal("Description", criteria.Description);
        Assert.Equal("PROD", criteria.Environment);
        Assert.False(criteria.IsActive);
        Assert.Equal("NewSecretAccessKey", criteria.SecretAccessKey);
        DeepAssert.Equal(["Urls"], criteria.Urls);
    }
}