namespace Fanex.Data.Services.Middleware;

using Data;

public class ErrorHandler
{
    private readonly RequestDelegate next;

    public ErrorHandler(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ObjectDbException exception)
        {
            await HandleException(context, exception);
        }
    }

    private static async Task HandleException(HttpContext context, ObjectDbException exception)
    {
        var exceptionInfo = GetObjectDbExceptionInfo(exception);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync(exception.Message + Environment.NewLine);
        foreach (var pair in exceptionInfo)
        {
            await context.Response.WriteAsync($"{pair.Key}={pair.Value}{Environment.NewLine}");
        }
    }

    private static IDictionary<string, object> GetObjectDbExceptionInfo(ObjectDbException exception)
    {
        var connectionStringBuilder = new ConnectionStringBuilder(exception.ConnectionString);

        return new Dictionary<string, object>
        {
            ["Server"] = connectionStringBuilder.GetServer(),
            [nameof(exception.DbName)] = exception.DbName,
            ["UserId"] = connectionStringBuilder.GetUserId(),
            [nameof(exception.SpName)] = exception.SpName,
            ["Parameters"] = GetParameters(exception.Parameters),
            [nameof(exception.CommandTimeout)] = exception.CommandTimeout
        };
    }

    private static string GetParameters(IDictionary<string, object> parameters)
        => parameters.Count > 0
            ? parameters
                .Select(parameter => $"{parameter.Key}={parameter.Value}")
                .Aggregate((firstPair, secondPair) => $"{firstPair};{secondPair}")
            : string.Empty;
}