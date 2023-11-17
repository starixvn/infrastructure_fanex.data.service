using System.Diagnostics.CodeAnalysis;

namespace Fanex.Data.ProxyClient.Tests
{
    using Repository;
    using System;
    using System.Threading.Tasks;
    using ParameterSets.Query;
    using Xunit;

    [Collection("Fanex Data Client collection")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class GetSimpleTypeTests
    {
        private readonly DynamicRepository proxy = new DynamicRepository();

        [Fact]
        public async Task GetAsync_Int_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new TestScalarIntCriteria();

            // Act.
            var actualResult = await proxy.GetAsync<int>(parameterSet);

            // Assert.
            Assert.Equal(100, actualResult);
        }
        [Fact]
        public async Task GetAsync_String_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new TestScalarStringCriteria();

            // Act.
            var actualResult = await proxy.GetAsync<string>(parameterSet);

            // Assert.
            Assert.Equal("Hello", actualResult);
        }
        [Fact]
        public async Task GetAsync_Boolean_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new TestScalarBooleanCriteria();

            // Act.
            var actualResult = await proxy.GetAsync<bool>(parameterSet);

            // Assert.
            Assert.True(actualResult);
        }
        [Fact]
        public async Task GetAsync_Byte_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new TestScalarByteCriteria();

            // Act.
            var actualResult = await proxy.GetAsync<byte>(parameterSet);

            // Assert.
            Assert.Equal(100, actualResult);
        }
        [Fact]
        public async Task GetAsync_Double_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new TestScalarDoubleCriteria();

            // Act.
            var actualResult = await proxy.GetAsync<double>(parameterSet);

            // Assert.
            Assert.Equal(10.5, actualResult);
        }
        [Fact]
        public async Task GetAsync_Decimal_ReturnValueCorrectly()
        {
            // Arrange.
            var parameterSet = new TestScalarDecimalCriteria();

            // Act.
            var actualResult = await proxy.GetAsync<decimal>(parameterSet);

            // Assert.
            Assert.Equal(10.5m, actualResult);
        }
        [Fact]
        public async Task GetAsync_DateTime_ReturnValueCorrectly()
        {
            // Arrange.
            var expected = new DateTime(2023, 12, 1, 0, 0, 0, DateTimeKind.Local);
            var parameterSet = new TestScalarDateTimeCriteria();

            // Act.
            var actualResult = await proxy.GetAsync<DateTime>(parameterSet);

            // Assert.
            Assert.Equal(expected, actualResult);
        }

    }
}