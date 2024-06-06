namespace Fanex.Data.ProxyClient.Tests
{
    using Xunit;

    public class CryptographyTests
    {

        [Fact]
        public void TestDecryptConnectionString()
        {
            // Arrange.
            var connectionString = "packet size=4096;data source=10.18.200.61\\Devdb053;initial catalog=FanexDataTests;connect timeout=600;integrated security=false;user id=FanexUser;password=\"h9Sky5eJOAeB0EOPr1IhVw==\";trustservercertificate=True";

            // Act.
            var plainText = Cryptography.DecryptConnectionString(connectionString);

            // Assert.
            Assert.NotEmpty(plainText);
        }

    }
}
