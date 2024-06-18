using Fanex.Data.SampleData.Models;
using System.Diagnostics.CodeAnalysis;

namespace Fanex.Data.ProxyClient.Tests
{
    using Models;
    using ParameterSets.Query;
    using Repository;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    [Collection("Fanex Data Client collection")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class GetTests
    {
        private readonly DynamicRepository proxy = new DynamicRepository();

        private const string TestedCustomerId = "VINET";

        [Fact]
        public async Task GetAsync_NullCriteria_ThrowNullException()
        {
            // Arrange.
            var criteria = default(ICriteria);

            // Act.
            var exception = await Record.ExceptionAsync(() => proxy.GetAsync<Customer>(criteria));

            // Assert.
            Assert.IsType<NullReferenceException>(exception);
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
            var actualResult = await proxy.GetAsync<Customer>(criteria);

            // Assert.
            Assert.Null(actualResult);
        }


        [Fact]
        public async Task GetAsync_WithValidCriteria_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new CustomerByIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var actualResult = await proxy.GetAsync<Customer>(parameterSet);

            // Assert.
            Assert.Equal(ExpectedData.ExpectedCustomer, actualResult);
        }

        [Fact]
        public void Get_WithValidCriteria_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new CustomerByIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var actualResult = proxy.Get<Customer>(parameterSet);

            // Assert.
            Assert.Equal(ExpectedData.ExpectedCustomer, actualResult);
        }


        [Fact]
        public async Task GetAsyncWithCorrectServerName_WithValidCriteria_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new CustomerByIdWithFakeServerNameCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var actualResult = await proxy.GetAsync<Customer>(parameterSet, "10.18.200.61\\Devdb053");

            // Assert.
            Assert.Equal(ExpectedData.ExpectedCustomer, actualResult);
        }

        [Fact]
        public async Task GetAsyncWithWrongServerName_WithValidCriteria_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new CustomerByIdWithFakeServerNameCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act
            var exception = await Record.ExceptionAsync(() => proxy.GetAsync<Customer>(parameterSet));

            // Assert
            Assert.IsType<DbProxyNotSuccessHttpException>(exception);
            Assert.Contains("A network-related or instance-specific error occurred while establishing a connection to SQL Server", exception.Message);
        }

        [Fact]
        public async Task GetAsync_ExistColumnWithNullValueAndMappingPropertyNotHasDefaultValue_ThatPropertyShouldBeNull()
        {
            // Arrange.
            var parameterSet = new CustomerByIdCriteria
            {
                CustomerId = "ANTON"
            };

            // Act.
            var actualResult = await proxy.GetAsync<Customer>(parameterSet);

            // Assert.
            Assert.Null(actualResult.Fax);
        }

        [Fact]
        public async Task GetAsync_ColumnHasValueAndMappingPropertyHasDefaultValue_ReturnValueFromDB()
        {
            // Arrange.
            var parameterSet = new CustomerByIdCriteria
            {
                CustomerId = "BOTTM"
            };

            // Act.
            var actualResult = await proxy.GetAsync<Customer>(parameterSet);

            // Assert.
            Assert.NotEmpty(actualResult.Region);
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
            var actualResult = await proxy.GetAsync<CustomerWithUpperProperties>(parameterSet);

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
            var actualResult = await proxy.GetAsync<CustomerWithRedundantProperties>(parameterSet);

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
            var actualResult = await proxy.GetAsync<CustomerWithLackProperty>(parameterSet);

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
            var actualResult = await proxy.GetAsync<Customer>(criteria);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public async Task GetAsync_DateTimeInput_ReturnCorrectly()
        {
            // Arrange.

            var criteria = new GetDateTimeNowCriteria
            {
                Now = DateTime.Today
            };

            // Act.
            var actualResult = await proxy.GetAsync<DateTime>(criteria);

            // Assert.
            Assert.Equal(criteria.Now, actualResult);
        }

        [Fact]
        public async Task GetAsync_DateTimeInputWithMilliSecond_ReturnCorrectly()
        {
            // Arrange.
            var now = new DateTime(2024,6,16,8,12,15,587);
            var criteria = new GetDateTimeNowCriteria()
            {
                Now = now
            };

            // Act.
            var actualResult = await proxy.GetAsync<DateTime>(criteria);

            // Assert.
            Assert.Equal(now.Date, actualResult.Date);
            Assert.Equal(now.Hour, actualResult.Hour);
            Assert.Equal(now.Minute, actualResult.Minute);
            Assert.Equal(now.Second, actualResult.Second);
            Assert.Equal(now.Millisecond, actualResult.Millisecond);
        }

        [Fact]
        public async Task GetAsync_ByteArray_GetSuccess()
        {
            // Arrange.
            var criteria = new GetImageCriteria();

            // Act.
            var actualResult = await proxy.GetAsync<Image>(criteria);

            // Assert.
            Assert.NotNull(actualResult);
        }
    }
}