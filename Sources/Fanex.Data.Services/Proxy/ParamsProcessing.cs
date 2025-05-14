namespace Fanex.Data.Services.Proxy;

public static class ParamsProcessing
{
    public static void Process(ProxyParameter proxyParameter)
    {
        foreach (var specialType in proxyParameter.SpecialTypes)
        {
            var hasValue = proxyParameter.ParameterSet.TryGetValue(specialType.Key, out var value);

            if (!hasValue)
            {
                continue;
            }


            if (specialType.Value == "Byte[]")
            {
                proxyParameter.ParameterSet[specialType.Key] = Convert.FromBase64String((string)value);
            }

            if (specialType.Value == "Guid")
            {
                proxyParameter.ParameterSet[specialType.Key] = new Guid((string)value);
            }
        }
    }
}
