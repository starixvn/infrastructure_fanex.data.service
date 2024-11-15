namespace Fanex.Data;

using System;
public sealed class DbProxyJsonDeserializeException(string message, Exception innerException)
    : DbProxyException(message, innerException)
{
    internal static DbProxyJsonDeserializeException Create(
        string endpoint,
        string requestJson,
        string responseJson,
        Exception e)
    {
        var message = $"responseJson: {responseJson}" + Environment.NewLine
                                                      + "Json exception occurs when deserializing the json result" + Environment.NewLine
                                                      + $"Exception Message: {e.Message}" + Environment.NewLine
                                                      + $"requestJson: {ObscurePassword(requestJson)}" + Environment.NewLine
                                                      + $"endpoint: {DbSettingProviderManager.ServiceApiUri}{endpoint}" + Environment.NewLine;


        return new DbProxyJsonDeserializeException(message, e);
    }
}