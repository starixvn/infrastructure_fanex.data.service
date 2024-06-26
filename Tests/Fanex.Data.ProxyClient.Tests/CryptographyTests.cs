namespace Fanex.Data.ProxyClient.Tests
{
    using Xunit;

    public class CryptographyTests
    {

        [Fact]
        public void TestDecryptConnectionString()
        {
            // Arrange.
            var connectionString = "packet size=4096; Data Source=10.18.200.58;Initial Catalog=bodb02;Connect Timeout=600;integrated security=false; user id=wsv_alp; password=tMTqX4Jh8mPg0pKcjbLJHwfrTY60ti4j3+7jLOmC0KM==; TrustServerCertificate=True;";

            // Act.
            var plainText = Cryptography.DecryptConnectionString(connectionString);

            // Assert.
            Assert.NotEmpty(plainText);
        }

    }
}
