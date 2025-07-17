namespace Fanex.Data;

using System.Text;
using Fanex.Data.Exceptions;

public sealed class DbProxyNotSuccessHttpException(string message) : DbProxyException(message)
{
    internal static DbProxyNotSuccessHttpException Create(
        string endpoint,
        string requestJson,
        string responseJson)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("responseJson:");
        stringBuilder.AppendLine(responseJson);
        stringBuilder.AppendLine("HttpClient exception. HttpResponse is not successful");
        stringBuilder.AppendLine("requestJson:");
        stringBuilder.AppendLine(PasswordObfuscator.Obfuscate(requestJson));
        stringBuilder.AppendLine($"endpoint: {DbSettingProviderManager.ServiceApiUri}{endpoint}");
        
        return new DbProxyNotSuccessHttpException(stringBuilder.ToString());
    }
}