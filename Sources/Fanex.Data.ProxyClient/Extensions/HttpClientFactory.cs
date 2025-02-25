namespace Fanex.Data.Extensions;

using System;
using System.Net.Http;

internal static class HttpClientFactory
{
    internal static HttpClient Client { get; }
        
    static HttpClientFactory()
    {
        var maxSpTimeout = DbSettingProviderManager.GetMaxSpTimeout();
        var timeout = maxSpTimeout + 300;
        var serviceApiUri = DbSettingProviderManager.ServiceApiUri;
        
        Client = new HttpClient
        {
            BaseAddress = serviceApiUri,
            Timeout = TimeSpan.FromSeconds(timeout)
        };
    }
}