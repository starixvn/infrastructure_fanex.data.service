namespace Fanex.Data.ProxyClient.Tests.Exceptions;

using Fanex.Data.Exceptions;
using Xunit;

public class PasswordObfuscatorTests
{
    [Fact]
    
    public void Obfuscate_ShouldReplacePasswordWithX_TestCase01()
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

    [Fact]
    public void Obfuscate_ShouldReplacePasswordWithX_TestCase02()
    {
        // Arrange
        const string message =
            """{"IgnoreRedundantParameters":false,"DbSetting":{"Key":"NAP_WinLimit_AllCurrencySel","Provider":"System.Data.SqlClient","DbName":"DBCust.CustDB.wsv_nap","ConnectionString":"data source=10.40.40.191;initial catalog=CustDB;integrated security=false;trustservercertificate=true;user id=wsv_nap;password=wsN3v3rG1v3Up3ncrypt!0n!v;max pool size=5000","SpName":"NAP_WinLimit_AllCurrencySel","CommandTimeout":300,"ColumnMaps":{},"FirmParameters":{}},"ParameterSet":{},"TypeNames":["BaseEntity"],"SpecialTypes":{}}""";
        const string expected = """{"IgnoreRedundantParameters":false,"DbSetting":{"Key":"NAP_WinLimit_AllCurrencySel","Provider":"System.Data.SqlClient","DbName":"DBCust.CustDB.wsv_nap","ConnectionString":"data source=10.40.40.191;initial catalog=CustDB;integrated security=false;trustservercertificate=true;user id=wsv_nap;password=xxxx-xxxx-xxxx-xxxx;max pool size=5000","SpName":"NAP_WinLimit_AllCurrencySel","CommandTimeout":300,"ColumnMaps":{},"FirmParameters":{}},"ParameterSet":{},"TypeNames":["BaseEntity"],"SpecialTypes":{}}""";

        // Act
        var actual = PasswordObfuscator.Obfuscate(message);

        // Assert
        Assert.Equal(expected, actual);
    }
}