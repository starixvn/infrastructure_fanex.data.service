namespace Fanex.Data.Services.Extensions;

internal static class DictionaryExtensions
{
    internal static IEnumerable<dynamic> MapCollection(this IDictionary<string, string> columnMaps, string typeName, List<dynamic> sources)
    {
        if (sources is null)
        {
            return [];
        }

        for (var i = 0; i < sources.Count; i++)
        {
            sources[i] = MapSingle(columnMaps, typeName, sources[i]);
        }

        return sources;
    }

    internal static dynamic MapSingle(this IDictionary<string, string> columnMaps, string typeName, dynamic source)
    {
        if (source is not IDictionary<string, object> dictionary)
        {
            return source;
        }

        var result = new Dictionary<string, object>();

        foreach (var (key, value) in dictionary)
        {
            var keyByTypeName = $"{key};{typeName}";

            if (TryGetValue(columnMaps, keyByTypeName, out var newKeyByTypeName))
            {
                result.TryAdd(newKeyByTypeName, value);
            }

            if (TryGetValue(columnMaps, key, out var newKey))
            {
                result.TryAdd(newKey, value);
            }

            result[key] = value;
        }

        return result;
    }

    private static bool TryGetValue(IDictionary<string, string> columnMaps, string key, out string value)
    {
        var pair = columnMaps.FirstOrDefault(p => string.Equals(key, p.Key, StringComparison.CurrentCultureIgnoreCase));
        if (!string.IsNullOrEmpty(pair.Key))
        {
            value = pair.Value;
            return true;
        }

        value = string.Empty;
        return false;
    }
}