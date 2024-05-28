namespace Fanex.Data.Services.Middleware;

using Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public class ErrorHandlerMiddleware(RequestDelegate next)
{
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
        catch (Exception exception)
        {
            await HandleException(context, exception);
        }
    }

    private static async Task HandleException(HttpContext context, ObjectDbException exception)
    {
        try
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(exception.Message + Environment.NewLine);
            await context.Response.WriteAsync($"Container: {Environment.MachineName}" + Environment.NewLine);
            await context.Response.WriteAsync(GetInfoException(exception));
            var exceptionInfo = GetObjectDbExceptionInfo(exception);
            foreach (var pair in exceptionInfo)
            {
                await context.Response.WriteAsync($"{pair.Key}={pair.Value}{Environment.NewLine}");
            }
        }
        catch { }        
    }

    private static string GetInfoException(Exception exception)
    {
        try
        {
            var lines = exception.StackTrace.Split("\n");
            return lines.Length > 0 ? lines[0] + Environment.NewLine : string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
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
            [nameof(exception.CommandTimeout)] = exception.CommandTimeout,
            ["CommandDefinition"] = JsonConvert.SerializeObject(exception.CommandDefinition)
        };
    }

    private static string GetParameters(IDictionary<string, object> parameters)
        => parameters.Count > 0
            ? parameters
                .Select(parameter => $"{parameter.Key}={parameter.Value}")
                .Aggregate((firstPair, secondPair) => $"{firstPair};{secondPair}")
            : string.Empty;

    private static async Task HandleException(HttpContext context, Exception exception)
    {
        try
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(exception.Message + Environment.NewLine);
            await context.Response.WriteAsync($"Container: {Environment.MachineName}" + Environment.NewLine);
        }
        catch { }
    }
}