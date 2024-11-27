using System.Data;

namespace Fanex.Data.Services.Cache;

using Newtonsoft.Json;

public static class Monitoring
{
    private const int ResponseStatisticSize = 50;

    private const int RequestStatisticSize = 3 * 24 * 60;

    public static long[] ResponseStatistics { get; set; } = CreateResponseStatistics();

    public static long[] RequestStatistics { get; set; } = CreateRequestStatistics();

    private static DateTimeOffset StartTime { get; } = DateTimeOffset.UtcNow;

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

    public static void UpdateResponseStatistic(DataSet dataset, string spName, Dictionary<string, object> parameters)
    {
        if (dataset == null || dataset.Tables.Count == 0)
        {
            return;
        }

        var maxCount = (from DataTable table in dataset.Tables select table.Rows.Count).Max();

        UpdateResponseStatistic(maxCount, spName, parameters);
    }

    public static void UpdateResponseStatistic(List<dynamic> models, string spName, Dictionary<string, object> parameters)
    {
        if (models == null)
        {
            return;
        }

        UpdateResponseStatistic(models.Count, spName, parameters);
    }

    public static void UpdateResponseStatistic(List<dynamic>[] models, string spName, Dictionary<string, object> parameters)
    {
        if (models == null)
        {
            return;
        }

        var maxCount = models.Select(m => m?.Count ?? 0).Max();
        UpdateResponseStatistic(maxCount, spName, parameters);
    }

    private static void UpdateResponseStatistic(int dataLength, string spName, Dictionary<string, object> parameters)
    {
        var index = (int)Math.Floor((decimal)dataLength / 1000);
        index = index >= ResponseStatisticSize ? ResponseStatisticSize - 1 : index;

        Interlocked.Increment(ref ResponseStatistics[index]);

        LogSpWhichReturnsMoreThan10kRows(dataLength, spName, parameters);
    }

    private static void LogSpWhichReturnsMoreThan10kRows(int dataLength, string spName, Dictionary<string, object> parameters)
    {
        if (dataLength < 10000)
        {
            return;
        }

        var json = JsonConvert.SerializeObject(parameters);
        Console.WriteLine($"At: {DateTimeOffset.Now}, {spName} returns {dataLength} rows. Parameters: {json}");
    }
}