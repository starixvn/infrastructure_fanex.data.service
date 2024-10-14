using Fanex.Data.SampleData.Models;

namespace Fanex.Data.Repository.Tests.Models;

public class CustomerWithRedundantProperties : Customer
{
    public string Int { get; set; }

    public string String { get; set; }

    public object Object { get; set; }
}