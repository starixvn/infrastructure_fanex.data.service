namespace Fanex.Data
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class DbProxyHttpRequestException : DbProxyException
    {
        public DbProxyHttpRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        internal static DbProxyHttpRequestException Create(
            string endpoint,
            string requestJson,
            Exception e)
        {
            var messageHeader = $"HttpRequestException occurs";
            var message = $"{messageHeader}" + Environment.NewLine
                          + $"Exception Message: {e.Message}" + Environment.NewLine
                          + $"endpoint: {DbSettingProviderManager.ServiceApiUri}{endpoint}" + Environment.NewLine
                          + $"requestJson: {ObscurePassword(requestJson)}" + Environment.NewLine
                          + "responseJson: Empty";

            return new DbProxyHttpRequestException(message, e);
        }

        private DbProxyHttpRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}