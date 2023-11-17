using Fanex.Data.ProxyClient.Tests.ParameterSets.Query;
using Fanex.Data.SampleData.Models;

namespace Fanex.Data.ProxyClient.Tests
{
    using Repository;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    [Collection("Fanex Data Client collection")]
    public class FetchTests
    {
        private readonly DynamicRepository proxy = new DynamicRepository();

        [Fact]
        public async Task FetchAsync_NullCriteria_ThrowArgumentNullException()
        {
            // Arrange.
            var criteria = default(ICriteria);

            // Act.
            var exception = await Record.ExceptionAsync(() => proxy.FetchAsync<MostExpensiveProduct>(criteria));

            // Assert.
            Assert.IsType<NullReferenceException>(exception);
        }

        [Fact]
        public async Task FetchAsync_InvalidCriteria_ReturnsEmptyList()
        {
            // Arrange.
            var criteria = new CustomerByIdCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<Customer>(criteria);

            // Assert.
            Assert.Empty(actualResult);
        }

        [Fact]
        public async Task FetchAsync_ValidCriteria_ReturnExceptedData()
        {
            // Arrange.
            var criteria = new TenMostExpensiveProductCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<MostExpensiveProduct>(criteria);

            // Assert.
            Assert.Equal(ExpectedData.ExpectedMostExpensiveProducts, actualResult);
        }

        [Fact]
        public async Task FetchAsync_EmptyCollection_ReturnEmpty()
        {
            // Arrange.
            var criteria = new GetTopCustomerCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<Customer>(criteria);

            // Assert.
            Assert.Empty(actualResult);
        }

        [Fact]
        public async Task FetchAsync_100kItem_ReturnNormally()
        {
            // Arrange.
            var criteria = new Get100kItemsCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<Customer>(criteria);

            // Assert.
            Assert.Equal(100_000, actualResult.Count());
        }
    }
}