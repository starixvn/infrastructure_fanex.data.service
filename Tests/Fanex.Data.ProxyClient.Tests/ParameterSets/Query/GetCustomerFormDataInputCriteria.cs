using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

using Repository;

[SuppressMessage("ReSharper", "UnusedType.Global")]
internal class GetCustomerFormDataInputCriteria : CriteriaBase
{
    public DataSet InputData { get; set; }

    public string CustomerId { get; set; }

    public override string GetSettingKey() => "GetCustomerFromTableInput";

    public override bool IsValid() => true;
}