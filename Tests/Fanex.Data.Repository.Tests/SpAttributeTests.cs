using Fanex.Data.Repository.Tests.ParameterSets.Criteria;
using Fanex.Data.SampleData;
using Fanex.Data.SampleData.Models;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Data.Repository.Tests;

[Collection(nameof(FanexDataConfigurationManagerCollection))]
public class SpAttributeTests(DynamicRepositoryFixture fixture) : IClassFixture<DynamicRepositoryFixture>
{
    private readonly IDynamicRepository repository = fixture.Repository;

    private const string TestedCustomerId = "VINET";

    [Fact]
    public async Task Params_Always_InputCorrectly()
    {
        // Arrange
        var criteria = new CustomerByIdCriteria { CustomerId = TestedCustomerId };

        // Act
        var customers = await repository.GetAsync<Customer>(criteria);

        // Assert
        Assert.Equal(ExpectedData.ExpectedCustomer, customers);
    }

    [Fact]
    public async Task SqlParamNameAttribute_Always_MapCorrectly()
    {
        // Arrange
        var criteria = new SqlParamNameMapCriteria { Id = TestedCustomerId };

        // Act
        var customers = await repository.GetAsync<Customer>(criteria);

        // Assert
        Assert.Equal(ExpectedData.ExpectedCustomer, customers);
    }

    [Fact]
    public async Task IgnoreParamAttribute_Always_IgnoreCorrectly()
    {
        // Arrange
        var criteria = new AttributeIgnoreCriteria { CustomerId = TestedCustomerId, IgnoredParam = 1 };

        // Act
        var customers = await repository.GetAsync<Customer>(criteria);

        // Assert
        Assert.Equal(ExpectedData.ExpectedCustomer, customers);
    }

    [Fact]
    public async Task AllowNullAttribute_Always_Correctly()
    {
        // Arrange
        var criteria = new AttributeAllowNullCriteria();

        // Act
        var customer = await repository.GetAsync<Customer>(criteria);

        // Assert
        Assert.Equal("Owner", customer.ContactTitle);
    }

    [Fact]
    public async Task OutPut_Always_UpdateCorrectly()
    {
        // Arrange
        var criteria = new GetTopCustomerCriteria { Top = 10, RemainingCustomers = 0 };

        // Act
        await repository.FetchAsync<Customer>(criteria);

        // Assert
        Assert.True(criteria.RemainingCustomers > 0);
    }
}