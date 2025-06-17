using Fanex.Data.Exceptions;

namespace Fanex.Data;

using System;
using System.Text;

public sealed class DbProxyJsonDeserializeException(string message, Exception innerException)
    : DbProxyException(message, innerException)
{
    internal static DbProxyJsonDeserializeException Create(
        string endpoint,
        string requestJson,
        string responseJson,
        Exception e)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("responseJson:");
        stringBuilder.AppendLine(responseJson);
        stringBuilder.AppendLine("Json exception occurs when deserializing the json result");
        stringBuilder.AppendLine($"Exception Message: {e.Message}");
        stringBuilder.AppendLine("requestJson:");
        stringBuilder.AppendLine(PasswordObfuscator.Obfuscate(requestJson));
        stringBuilder.AppendLine($"endpoint: {DbSettingProviderManager.ServiceApiUri}{endpoint}");

        return new DbProxyJsonDeserializeException(stringBuilder.ToString(), e);
    }
}