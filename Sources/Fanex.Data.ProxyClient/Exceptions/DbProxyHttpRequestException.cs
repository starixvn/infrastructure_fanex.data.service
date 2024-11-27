namespace Fanex.Data;

using System;

public sealed class DbProxyHttpRequestException(string message, Exception innerException)
    : DbProxyException(message, innerException)
{
    internal static DbProxyHttpRequestException Create(
        string endpoint,
        string requestJson,
        Exception e)
    {
        var message = $"Exception Message: {e.Message}" + Environment.NewLine
                                                        + "HttpRequestException occurs" + Environment.NewLine
                                                        + "responseJson: Empty"
                                                        + $"requestJson: {ObscurePassword(requestJson)}" + Environment.NewLine
                                                        + $"endpoint: {DbSettingProviderManager.ServiceApiUri}{endpoint}" + Environment.NewLine;

        return new DbProxyHttpRequestException(message, e);
    }
}