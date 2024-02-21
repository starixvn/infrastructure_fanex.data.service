namespace Fanex.Data.ProxyClient.Tests
{
    using Xunit;

    public class CryptographyTests
    {

        [Fact]
        public void TestDecryptConnectionString()
        {
            // Arrange.
            var connectionString = "Data Source=10.18.200.58;Initial Catalog=bodb_MMS;integrated security=false; User ID=bodbAdminNet;Password=i9hNnaZltx04XF6oTNOKTnvvtLC/zfxr=";

            // Act.
            var plainText = Cryptography.DecryptConnectionString(connectionString);

            // Assert.
            Assert.NotEmpty(plainText);
        }

    }
}
