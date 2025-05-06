namespace Fanex.Data;

using System;
using System.Text;
using Exceptions;

public sealed class DbProxyHttpRequestException(string message, Exception innerException)
    : DbProxyException(message, innerException)
{
    internal static DbProxyHttpRequestException Create(
        string endpoint,
        string requestJson,
        Exception e)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Exception Message: {e.Message}");
        stringBuilder.AppendLine("HttpClient exception occurs");
        stringBuilder.AppendLine("responseJson: Empty");
        stringBuilder.AppendLine("requestJson:");
        stringBuilder.AppendLine(PasswordObfuscator.Obfuscate(requestJson));
        stringBuilder.AppendLine($"endpoint: {DbSettingProviderManager.ServiceApiUri}{endpoint}");

        return new DbProxyHttpRequestException(stringBuilder.ToString(), e);
    }
}