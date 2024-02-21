namespace Fanex.Data
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class DbProxyNotSuccessHttpException : DbProxyException
    {
        public DbProxyNotSuccessHttpException(string message)
            : base(message)
        {
        }

        internal static DbProxyNotSuccessHttpException Create(
            string endpoint,
            string requestJson,
            string responseJson)
        {
            const string messageHeader = "HttpClient exception. HttpResponse is not successful";
            var message = $"{messageHeader}" + Environment.NewLine
                          + $"endpoint: {DbSettingProviderManager.ServiceApiUri}{endpoint}" + Environment.NewLine
                          + $"requestJson: {ObscurePassword(requestJson)}" + Environment.NewLine
                          + $"responseJson: {responseJson}";

            return new DbProxyNotSuccessHttpException(message);
        }

        private DbProxyNotSuccessHttpException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}