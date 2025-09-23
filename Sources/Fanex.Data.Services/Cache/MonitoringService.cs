using System.Collections.Concurrent;
using System.Data;
using Fanex.Data.Services.Proxy;

namespace Fanex.Data.Services.Cache;

using Newtonsoft.Json;

public static class Monitoring
{
    private const int ResponseStatisticSize = 50;

    private const int RequestStatisticSize = 3 * 24 * 60;

    public static long[] ResponseStatistics { get; set; } = CreateResponseStatistics();

    public static long[] RequestStatistics { get; set; } = CreateRequestStatistics();

    private static DateTimeOffset StartTime { get; } = DateTimeOffset.UtcNow;

    public static ConcurrentDictionary<string, int> DbServerStatistics { get; } = new();

    public static ConcurrentDictionary<string, int> SpNameStatistics { get; } = new();

    public static long[] CreateResponseStatistics()
    {
        return Enumerable.Repeat(0L, ResponseStatisticSize).ToArray();
    }

    public static long[] CreateRequestStatistics()
    {
        return Enumerable.Repeat(0L, RequestStatisticSize).ToArray();
    }

    public static void UpdateRequestStatistic()
    {
        var minute = (DateTimeOffset.UtcNow - StartTime).TotalMinutes;
        var index = (int)Math.Floor(Math.Abs(minute));
        index %= RequestStatisticSize;
        Interlocked.Increment(ref RequestStatistics[index]);

        var next = (index + 1) % RequestStatisticSize;
        RequestStatistics[next] = 0;
    }

    public static void UpdateResponseStatistic(DataSet dataset, decimal duration, string spName, Dictionary<string, object> parameters)
    {
        if (dataset == null || dataset.Tables.Count == 0)
        {
            return;
        }

        var maxCount = (from DataTable table in dataset.Tables select table.Rows.Count).Max();

        UpdateResponseStatistic(maxCount, duration, spName, parameters);
    }

    public static void UpdateResponseStatistic(List<dynamic> models, decimal duration, string spName, Dictionary<string, object> parameters)
    {
        if (models == null)
        {
            return;
        }

        UpdateResponseStatistic(models.Count, duration, spName, parameters);
    }

    public static void UpdateResponseStatistic(List<dynamic>[] models, decimal duration, string spName, Dictionary<string, object> parameters)
    {
        if (models == null)
        {
            return;
        }

        var maxCount = models.Select(m => m?.Count ?? 0).Max();
        UpdateResponseStatistic(maxCount, duration, spName, parameters);
    }

    private static void UpdateResponseStatistic(int dataLength, decimal duration, string spName, Dictionary<string, object> parameters)
    {
        var index = (int)Math.Floor((decimal)dataLength / 1000);
        index = index >= ResponseStatisticSize ? ResponseStatisticSize - 1 : index;

        Interlocked.Increment(ref ResponseStatistics[index]);

        LogSpWhichReturnsMoreThan10kRows(dataLength, duration, spName, parameters);
    }

    private static void LogSpWhichReturnsMoreThan10kRows(int dataLength, decimal duration, string spName, Dictionary<string, object> parameters)
    {
        if (duration < 30000 && dataLength < 20000)
        {
            return;
        }

        var json = JsonConvert.SerializeObject(parameters);
        json = json.Length > 10000 ? json[..10000] : json;
        Console.WriteLine($"At: {DateTimeOffset.Now}, {spName} took {duration} milliseconds to return {dataLength} rows. Parameters: {json}");
    }

    public static void UpdateRequestStatisticsByDbServerAndSp(ProxyParameter parameter)
    {
        var dbName = parameter?.DbSetting?.DbName;
        var spName = parameter?.DbSetting?.SpName;
        if (string.IsNullOrWhiteSpace(dbName) || string.IsNullOrWhiteSpace(spName))
        {
            return;
        }

        DbServerStatistics.AddOrUpdate(
            dbName,
            1,
            (_, existingCount) => existingCount + 1);
        SpNameStatistics.AddOrUpdate(
            spName,
            1,
            (_, existingCount) => existingCount + 1);
    }
}