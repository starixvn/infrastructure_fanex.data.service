using Fanex.Data.ProxyClient.Tests.ParameterSets.Query;
using Fanex.Data.SampleData.Models;

namespace Fanex.Data.ProxyClient.Tests
{
    using Repository;
    using System.Threading.Tasks;
    using Xunit;

    [Collection("Fanex Data Client collection")]
    public class SpAttributeTests
    {
        private readonly DynamicRepository proxy = new DynamicRepository();

        private const string TestedCustomerId = "VINET";

        [Fact]
        public async Task Params_Always_InputCorrectly()
        {
            // Arrange
            var criteria = new CustomerByIdCriteria { CustomerId = TestedCustomerId };

            // Act
            var customers = await proxy.GetAsync<Customer>(criteria);

            // Assert
            Assert.Equal(ExpectedData.ExpectedCustomer, customers);
        }

        [Fact]
        public async Task SqlParamNameAttribute_Always_MapCorrectly()
        {
            // Arrange
            var criteria = new SqlParamNameMapCriteria { Id = TestedCustomerId };

            // Act
            var customers = await proxy.GetAsync<Customer>(criteria);

            // Assert
            Assert.Equal(ExpectedData.ExpectedCustomer, customers);
        }

        [Fact]
        public async Task IgnoreParamAttribute_Always_IgnoreCorrectly()
        {
            // Arrange
            var criteria = new AttributeIgnoreCriteria { CustomerId = TestedCustomerId, IgnoredParam = 1 };

            // Act
            var customers = await proxy.GetAsync<Customer>(criteria);

            // Assert
            Assert.Equal(ExpectedData.ExpectedCustomer, customers);
        }

        [Fact]
        public async Task AllowNullAttribute_Always_Correctly()
        {
            // Arrange
            var criteria = new AttributeAllowNullCriteria();

            // Act
            var customer = await proxy.GetAsync<Customer>(criteria);

            // Assert
            Assert.Equal("Owner", customer.ContactTitle);
        }

        [Fact]
        public async Task OutPut_Always_UpdateCorrectly()
        {
            // Arrange
            var criteria = new GetTopCustomerCriteria { Top = 10, RemainingCustomers = 0 };

            // Act
            await proxy.FetchAsync<Customer>(criteria);

            // Assert
            Assert.True(criteria.RemainingCustomers > 0);
        }
    }
}