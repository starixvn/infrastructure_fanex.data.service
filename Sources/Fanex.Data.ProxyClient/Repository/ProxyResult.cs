namespace Fanex.Data.Repository;

internal class ProxyResult<T>
{
    public string ParameterSet { get; set; }

    public T Result { get; set; }
}