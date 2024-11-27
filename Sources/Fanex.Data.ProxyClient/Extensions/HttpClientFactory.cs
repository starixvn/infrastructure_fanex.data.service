namespace Fanex.Data.Extensions;

using System;
using System.Net.Http;

internal static class HttpClientFactory
{
    internal static HttpClient Client { get; }
        
    static HttpClientFactory()
    {
        var serviceApiUri = DbSettingProviderManager.ServiceApiUri;
        var timeout = TimeSpan.FromSeconds(DbSettingProviderManager.GetMaxSpTimeout() + 30);


        Client = new HttpClient
        {
            BaseAddress = serviceApiUri,
            Timeout = timeout
        };
    }
}