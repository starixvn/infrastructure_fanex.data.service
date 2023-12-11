namespace Fanex.Data
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;

    internal static class HttpClientFactory
    {
        internal static HttpClient Client { get; }
        
        static HttpClientFactory()
        {
            var serviceApiUri = DbSettingProviderManager.ServiceApiUri;
            var timeout = TimeSpan.FromSeconds(DbSettingProviderManager.GetMaxSpTimeout() + 30);

#if NET5_0_OR_GREATER
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Brotli
            };
#else
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            };
#endif

            Client = new HttpClient(handler)
            {
                BaseAddress = serviceApiUri,
                Timeout = timeout
            };
#if NET5_0_OR_GREATER
            Client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
#else
            Client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
#endif
        }
    }
}
