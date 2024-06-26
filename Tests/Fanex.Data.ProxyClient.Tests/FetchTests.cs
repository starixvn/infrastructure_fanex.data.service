using Fanex.Data.ProxyClient.Tests.ParameterSets.Query;
using Fanex.Data.Repository;
using Fanex.Data.SampleData.Models;

namespace Fanex.Data.ProxyClient.Tests
{
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
            var criteria = new Get100ThousandItemsCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<Customer>(criteria);

            // Assert.
            Assert.Equal(100_000, actualResult.Count());
        }

        [Fact]
        public async Task FetchAsync_MappingPropertiesName_ReturnCorrectly()
        {
            // Arrange.
            var criteria = new GetTopCustomerWithMapping
            {
                Top = 10
            };

            // Act.
            var actualResult = await proxy.FetchAsync<CustomerWithMappingProperty>(criteria);

            // Assert.
            Assert.NotNull(actualResult.First().Id);
            Assert.NotNull(actualResult.First().Title);
            Assert.NotNull(actualResult.First().Code);
        }

        [Fact]
        public async Task FetchAsync_MappingWithClassEmpty_IgnoreAttributeClassAndMapNormally()
        {
            // Arrange.
            var criteria = new GetTopCustomerWithMapping
            {
                Top = 10,
                ClassEmpty = true
            };

            // Act.
            var actualResult = await proxy.FetchAsync<Customer>(criteria);

            // Assert.
            Assert.NotNull(actualResult.First().Id);
        }

        [Fact]
        public async Task FetchAsync_ByteArray_FetchSuccess()
        {
            // Arrange.
            var criteria = new GetImageCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<Image>(criteria);

            // Assert.
            Assert.Equal(10, actualResult.Count());
        }
    }
}