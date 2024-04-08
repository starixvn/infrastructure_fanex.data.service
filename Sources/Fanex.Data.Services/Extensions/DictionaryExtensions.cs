namespace Fanex.Data.Services.Extensions;

public static class DictionaryExtensions
{
    public static IEnumerable<dynamic> MapCollection(this IDictionary<string, string> columnMaps, string typeName, List<dynamic> sources)
    {
        if (sources is null)
        {
            return null;
        }

        for (var i = 0; i < sources.Count; i++)
        {
            sources[i] = MapSingle(columnMaps, typeName, sources[i]);
        }

        return sources;
    }

    public static dynamic MapSingle(this IDictionary<string, string> columnMaps, string typeName, dynamic source)
    {
        if (source is not IDictionary<string, object> dictionary)
        {
            return source;
        }

        var result = new Dictionary<string, object>();

        foreach (var (key, value) in dictionary)
        {
            var keyByTypeName = $"{key};{typeName}";

            if (TryGetValue(columnMaps, keyByTypeName, out var newKeyByTypeName) && !result.ContainsKey(newKeyByTypeName))
            {
                result[newKeyByTypeName] = value;
            }

            if (TryGetValue(columnMaps, key, out var newKey) && !result.ContainsKey(newKey))
            {
                result[newKey] = value;
            }

            result[key] = value;
        }

        return result;
    }

    private static bool TryGetValue(IDictionary<string, string> columnMaps, string key, out string value)
    {
        foreach (var pair in columnMaps)
        {
            if (string.Equals(key, pair.Key, StringComparison.CurrentCultureIgnoreCase))
            {
                value = pair.Value;
                return true;
            }
        }

        value = string.Empty;
        return false;
    }
}