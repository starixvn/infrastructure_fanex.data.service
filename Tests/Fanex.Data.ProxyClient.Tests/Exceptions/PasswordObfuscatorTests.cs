namespace Fanex.Data.ProxyClient.Tests.Exceptions;

using Fanex.Data.Exceptions;
using Xunit;

public class PasswordObfuscatorTests
{
    [Fact]
    
    public void Obfuscate_ShouldReplacePasswordWithX()
    {
        // Arrange
        const string message = "'DbSetting':{'Key':'NAP_User_GetById','DbName':'DBPOL.NAP_UserControl.wsv_nap'," +
                               "'ConnectionString':'data source=172.16.40.21;initial catalog=NAP_UserControl;" +
                               "user id=wsv_nap;password=wsN3v3rG1v3Up3ncrypt!0n!v','SpName':'NAP_User_GetById'}";
        const string expected = "'DbSetting':{'Key':'NAP_User_GetById','DbName':'DBPOL.NAP_UserControl.wsv_nap',"+
                                "'ConnectionString':'data source=172.16.40.21;initial catalog=NAP_UserControl;"+
                                "user id=wsv_nap;password=xxxx-xxxx-xxxx-xxxx','SpName':'NAP_User_GetById'}";

        // Act
        var actual = PasswordObfuscator.Obfuscate(message);

        // Assert
        Assert.Equal(expected, actual);
    }
}