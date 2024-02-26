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
            const string messageHeader = "Json exception occurs when deserializing the json result";

            var message = $"{messageHeader}" + Environment.NewLine
                          + $"Exception Message: {e.Message}" + Environment.NewLine
                          + $"endpoint: {DbSettingProviderManager.ServiceApiUri}{endpoint}" + Environment.NewLine
                          + $"requestJson: {ObscurePassword(requestJson)}" + Environment.NewLine
                          + $"responseJson: {responseJson}" + Environment.NewLine;

            return new DbProxyJsonDeserializeException(message, e);
        }

        private DbProxyJsonDeserializeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}