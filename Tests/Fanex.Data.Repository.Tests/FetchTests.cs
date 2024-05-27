using System.Linq;

namespace Fanex.Data.Repository.Tests
{
    using Fanex.Data.SampleData.Models;
    using ParameterSets.Criteria;
    using SampleData;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    [Collection(nameof(FanexDataConfigurationManagerCollection))]
    public class FetchTests : IClassFixture<DynamicRepositoryFixture>
    {
        private readonly IDynamicRepository repository;

        public FetchTests(DynamicRepositoryFixture fixture)
            => repository = fixture.Repository;

        [Fact]
        public void Fetch_NullCriteria_ThrowArgumentNullException()
        {
            // Arrange.
            var criteria = default(ICriteria);

            // Act.
            var exception = Record.Exception(() => repository.Fetch<MostExpensiveProduct>(criteria));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Fetch_InvalidCriteria_ReturnsEmptyList()
        {
            // Arrange.
            var criteria = new CustomerByIdCriteria();

            // Act.
            var actualResult = repository.Fetch<Customer>(criteria);

            // Assert.
            Assert.Empty(actualResult);
        }

        [Fact]
        public void Fetch_ValidCriteria_ReturnExceptedData()
        {
            // Arrange.
            var criteria = new TenMostExpensiveProductCriteria();

            // Act.
            var actualResult = repository.Fetch<MostExpensiveProduct>(criteria);

            // Assert.
            Assert.Equal(ExpectedData.ExpectedMostExpensiveProducts, actualResult);
        }

        [Fact]
        public async Task FetchAsync_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange.
            var criteria = default(ICriteria);

            // Act.
            var exception = await Record.ExceptionAsync(() => repository.FetchAsync<MostExpensiveProduct>(criteria));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task FetchAsync_InvalidCriteria_ReturnsEmptyList()
        {
            // Arrange.
            var criteria = new CustomerByIdCriteria();

            // Act.
            var actualResult = await repository.FetchAsync<Customer>(criteria);

            // Assert.
            Assert.Empty(actualResult);
        }

        [Fact]
        public async Task FetchAsync_ValidCriteria_ReturnExceptedData()
        {
            // Arrange.
            var criteria = new TenMostExpensiveProductCriteria();

            // Act.
            var actualResult = await repository
                .FetchAsync<MostExpensiveProduct>(criteria);

            // Assert.
            Assert.Equal(ExpectedData.ExpectedMostExpensiveProducts, actualResult);
        }

        [Fact]
        public async Task FetchAsync_EmptyCollection_ReturnEmpty()
        {
            // Arrange.
            var criteria = new GetTopCustomerCriteria();

            // Act.
            var actualResult = await repository
                .FetchAsync<Customer>(criteria);

            // Assert.
            Assert.Empty(actualResult);
        }

        [Fact]
        public async Task FetchAsync_100kItem_ReturnNormally()
        {
            // Arrange.
            var criteria = new Get100ThousandItemsCriteria();

            // Act.
            var actualResult = await repository
                .FetchAsync<Customer>(criteria);

            // Assert.
            Assert.Equal(100_000, actualResult.Count());
        }
    }
}