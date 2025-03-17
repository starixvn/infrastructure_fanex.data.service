namespace Fanex.Data;

using System;

public class DbProxyException : Exception
{
    public DbProxyException(string message)
        : base(message)
    {
    }

    public DbProxyException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}