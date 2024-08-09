namespace Fanex.Data
{
    using System;
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
            var message = $"responseJson: {responseJson}" + Environment.NewLine
                          + "HttpClient exception. HttpResponse is not successful" + Environment.NewLine
                          + $"requestJson: {ObscurePassword(requestJson)}" + Environment.NewLine
                          + $"endpoint: {DbSettingProviderManager.ServiceApiUri}{endpoint}" + Environment.NewLine;
            return new DbProxyNotSuccessHttpException(message);
        }
    }
}