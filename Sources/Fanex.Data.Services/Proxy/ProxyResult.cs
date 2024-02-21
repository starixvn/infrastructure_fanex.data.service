namespace Fanex.Data.Services.Proxy;

public class ProxyResult<T>
{
    public string ParameterSet { get; set; }

    public T Result { get; set; }
}