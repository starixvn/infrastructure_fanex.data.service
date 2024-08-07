using System.Data;

namespace Fanex.Data.Services.Cache;

public static class Monitoring
{
    public static DateTimeOffset StartTime { get; set; } = DateTimeOffset.UtcNow;

    private const int ResponseStatisticSize = 20;

    private const int RequestStatisticSize = 3 * 24 * 60;

    public static int[] ResponseStatistics { get; set; } = CreateResponseStatistics();

    public static int[] RequestStatistics { get; set; } = CreateRequestStatistics();


    public static int[] CreateResponseStatistics()
    {
        return Enumerable.Repeat(0, ResponseStatisticSize).ToArray();
    }

    public static int[] CreateRequestStatistics()
    {
        return Enumerable.Repeat(0, RequestStatisticSize).ToArray();
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

    public static void UpdateResponseStatistic(DataSet dataset)
    {
        if (dataset == null || dataset.Tables.Count == 0)
        {
            return;
        }

        var maxCount = (from DataTable table in dataset.Tables select table.Rows.Count).Max();

        UpdateResponseStatistic(maxCount);
    }

    public static void UpdateResponseStatistic(List<dynamic> models)
    {
        if (models == null)
        {
            return;
        }

        UpdateResponseStatistic(models.Count);
    }

    public static void UpdateResponseStatistic(List<dynamic>[] models)
    {
        if (models == null)
        {
            return;
        }

        var maxCount = models.Select(m => m?.Count ?? 0).Max();
        UpdateResponseStatistic(maxCount);
    }

    private static void UpdateResponseStatistic(int dataLength)
    {
        var index = (int)Math.Floor((decimal)dataLength / 500);
        index = index >= ResponseStatisticSize ? ResponseStatisticSize - 1 : index;

        Interlocked.Increment(ref ResponseStatistics[index]);
    }
}