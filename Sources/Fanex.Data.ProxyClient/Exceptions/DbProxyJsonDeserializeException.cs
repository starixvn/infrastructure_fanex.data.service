namespace Fanex.Data
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class DbProxyJsonDeserializeException : DbProxyException
    {
        public DbProxyJsonDeserializeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

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
}