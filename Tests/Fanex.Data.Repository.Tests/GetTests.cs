namespace Fanex.Data.Repository.Tests
{
    using Fanex.Data.SampleData.Models;
    using Models;
    using ParameterSets.Criteria;
    using SampleData;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    [Collection(nameof(FanexDataConfigurationManagerCollection))]
    public class GetTests : IClassFixture<DynamicRepositoryFixture>
    {
        private const string TestedCustomerId = "VINET";

        private readonly IDynamicRepository repository;

        public GetTests(DynamicRepositoryFixture fixture)
            => repository = fixture.Repository;

        [Fact]
        public void Get_NullCriteria_ThrowArgumentNullException()
        {
            // Arrange.
            var criteria = default(ICriteria);

            // Act.
            var exception = Record.Exception(() => repository.Get<Customer>(criteria));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Get_InvalidCriteria_ReturnsNull()
        {
            // Arrange.
            var criteria = new CustomerByIdCriteria
            {
                CustomerId = string.Empty
            };

            // Act.
            var actualResult = repository.Get<Customer>(criteria);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void Get_ValidCriteria_GetCorrectData()
        {
            // Arrange.
            var criteria = new CustomerByIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var actualResult = repository.Get<Customer>(criteria);

            // Assert.
            Assert.Equal(ExpectedData.ExpectedCustomer, actualResult);
        }

        [Fact]
        public void Get_SpReturnEmpty_ReturnNull()
        {
            // Arrange.
            var criteria = new CustomerByIdCriteria
            {
                CustomerId = "test_id_not_exist"
            };

            // Act.
            var actualResult = repository.Get<Customer>(criteria);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void Get_RedundantProperties_ShouldBeNull()
        {
            // Arrange.
            var parameterSet = new CustomerByIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var actualResult = repository.Get<CustomerWithRedundantProperties>(parameterSet);

            // Assert.
            Assert.Null(actualResult.Int);
            Assert.Null(actualResult.String);
            Assert.Null(actualResult.Object);
        }

        [Fact]
        public void Get_LackProperties_MapValueForRemainingProperties()
        {
            // Arrange.
            var parameterSet = new CustomerByIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var actualResult = repository.Get<CustomerWithLackProperty>(parameterSet);

            // Assert.
            Assert.Equal(TestedCustomerId, actualResult.Id);
        }

        [Fact]
        public async Task GetAsync_NullCriteria_ThrowArgumentNullException()
        {
            // Arrange.
            var criteria = default(ICriteria);

            // Act.
            var exception = await Record.ExceptionAsync(() => repository.GetAsync<Customer>(criteria));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task GetAsync_InvalidCriteria_ReturnsNull()
        {
            // Arrange.
            var criteria = new CustomerByIdCriteria
            {
                CustomerId = string.Empty
            };

            // Act.
            var actualResult = await repository.GetAsync<Customer>(criteria);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public async Task GetAsync_ValidCriteria_GetCorrectData()
        {
            // Arrange.
            var parameterSet = new CustomerByIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var actualResult = await repository.GetAsync<Customer>(parameterSet);

            // Assert.
            Assert.Equal(ExpectedData.ExpectedCustomer, actualResult);
        }

        [Fact]
        public async Task GetAsync_InsensitiveColumn_MapCorrectly()
        {
            // Arrange.
            var parameterSet = new CustomerByIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var actualResult = await repository.GetAsync<CustomerWithUpperProperties>(parameterSet);

            // Assert.
            Assert.Equal(TestedCustomerId, actualResult.ID);
        }

        [Fact]
        public async Task GetAsync_RedundantProperties_ShouldBeNull()
        {
            // Arrange.
            var parameterSet = new CustomerByIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var actualResult = await repository.GetAsync<CustomerWithRedundantProperties>(parameterSet);

            // Assert.
            Assert.Null(actualResult.Int);
            Assert.Null(actualResult.String);
            Assert.Null(actualResult.Object);
        }

        [Fact]
        public async Task GetAsync_LackProperties_MapValueForRemainingProperties()
        {
            // Arrange.
            var parameterSet = new CustomerByIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var actualResult = await repository.GetAsync<CustomerWithLackProperty>(parameterSet);

            // Assert.
            Assert.Equal(TestedCustomerId, actualResult.Id);
        }

        [Fact]
        public async Task GetAsync_SpReturnEmpty_ReturnNull()
        {
            // Arrange.
            var criteria = new CustomerByIdCriteria
            {
                CustomerId = "test_id_not_exist"
            };

            // Act.
            var actualResult = await repository.GetAsync<Customer>(criteria);

            // Assert.
            Assert.Null(actualResult);
        }
    }
}