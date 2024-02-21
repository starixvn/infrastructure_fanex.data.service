using Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

namespace Fanex.Data.ProxyClient.Tests
{
    using Repository;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    [Collection("Fanex Data Client collection")]
    public class FetchDataSetTests
    {
        private readonly DynamicRepository proxy = new DynamicRepository();

        [Fact]
        public async Task FetchDataSetAsync_NullCriteria_ThrowArgumentNullException()
        {
            // Arrange.
            var criteria = default(ICriteria);

            // Act.
            var exception = await Record.ExceptionAsync(() => proxy.FetchDataSetAsync(criteria));

            // Assert.
            Assert.IsType<NullReferenceException>(exception);
        }

        [Fact]
        public async Task FetchDataSetAsync_InvalidCriteria_ReturnsEmptyListTable()
        {
            // Arrange.
            var criteria = new CustomerByIdCriteria();

            // Act.
            var dataSet = await proxy.FetchDataSetAsync(criteria);

            // Assert.
            Assert.Empty(dataSet.Tables);
        }

        [Fact]
        public async Task FetchDataSetAsync_ValidCriteria_ReturnsDataSetCorrectly()
        {
            // Arrange.
            var criteria = new GetTopCustomerCriteria { Top = 10 };

            // Act.
            var actualResult = await proxy.FetchDataSetAsync(criteria);

            // Assert.
            Assert.Equal(10, actualResult.Tables[0].Rows.Count);
        }
    }
}