namespace Fanex.Data.Services.Extensions;

public static class DictionaryExtensions
{
    public static void MapCollection(this IDictionary<string, string> columnMaps, string typeName, IEnumerable<dynamic> sources)
    {
        if (sources is null)
        {
            return;
        }

        foreach (var item in sources)
        {
            MapSingle(columnMaps, typeName, item);
        }
    }

    public static void MapSingle(this IDictionary<string, string> columnMaps, string typeName, dynamic source)
    {
        if (source is not IDictionary<string, object>)
        {
            return;
        }

        IDictionary<string, object> result = source;

        foreach (var (key, value) in result)
        {
            var keyByTypeName = $"{key};{typeName}";

            if (TryGetValue(columnMaps, keyByTypeName, out var newKeyByTypeName))
            {
                result[newKeyByTypeName] = value;
            }

            if (TryGetValue(columnMaps, key, out var newKey))
            {
                result[newKey] = value;
            }
        }
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

    public static object GetFirstProperty(dynamic source)
    {
        if (source is not IDictionary<string, object>)
        {
            return null;
        }

        IDictionary<string, object> model = source;
        if (model.Keys.Count == 0)
        {
            return null;
        }

        var key = model.Keys.First();
        var result = model[key];

        return result;
    }

    public static IEnumerable<object> GetFirstColumn(IEnumerable<dynamic> sources)
    {
        if (sources == null || !sources.Any())
        {
            return Enumerable.Empty<object>();
        }

        var firstModel = sources.First();
        if (firstModel is not IDictionary<string, object>)
        {
            return Enumerable.Empty<object>();
        }

        IDictionary<string, object> firstDictionary = firstModel;
        if (!firstDictionary.Keys.Any())
        {
            return Enumerable.Empty<object>();
        }

        var key = firstDictionary.Keys.First();

        return sources.OfType<IDictionary<string, object>>().Select(sourceDictionary => sourceDictionary[key]).ToList();
    }
}