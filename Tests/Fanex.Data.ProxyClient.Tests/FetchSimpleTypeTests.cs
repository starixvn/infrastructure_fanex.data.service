using System.Diagnostics.CodeAnalysis;

namespace Fanex.Data.ProxyClient.Tests
{
    using Repository;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using ParameterSets.Query;
    using Xunit;

    [Collection("Fanex Data Client collection")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class FetchSimpleTypeTests
    {
        private readonly DynamicRepository proxy = new DynamicRepository();

        [Fact]
        public async Task FetchAsync_Int_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new TestScalarIntCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<int>(parameterSet);

            // Assert.
            Assert.Equal(100, actualResult.First());
        }
        [Fact]
        public async Task FetchAsync_String_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new TestScalarStringCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<string>(parameterSet);

            // Assert.
            Assert.Equal("Hello", actualResult.First());
        }
        [Fact]
        public async Task FetchAsync_Boolean_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new TestScalarBooleanCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<bool>(parameterSet);

            // Assert.
            Assert.True(actualResult.First());
        }
        [Fact]
        public async Task FetchAsync_Byte_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new TestScalarByteCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<byte>(parameterSet);

            // Assert.
            Assert.Equal(100, actualResult.First());
        }
        [Fact]
        public async Task FetchAsync_Double_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new TestScalarDoubleCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<double>(parameterSet);

            // Assert.
            Assert.Equal(10.5, actualResult.First());
        }
        [Fact]
        public async Task FetchAsync_Decimal_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new TestScalarDecimalCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<decimal>(parameterSet);

            // Assert.
            Assert.Equal(10.5m, actualResult.First());
        }
        [Fact]
        public async Task FetchAsync_DateTime_ReturnValueCorrectly()
        {
            // Arrange.
            var expected = new DateTime(2023, 12, 1, 0, 0, 0, DateTimeKind.Local);
            var parameterSet = new TestScalarDateTimeCriteria();

            // Act.
            var actualResult = await proxy.FetchAsync<DateTime>(parameterSet);

            // Assert.
            Assert.Equal(expected, actualResult.First());
        }

    }
}