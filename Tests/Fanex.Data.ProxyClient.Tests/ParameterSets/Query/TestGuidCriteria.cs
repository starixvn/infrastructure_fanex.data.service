using System;
using Fanex.Data.Repository;

namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

public class TestGuidCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "TestGuid";

    [SpParam(IsSpecialType = true)]
    public Guid SessionId { get; set; }

    public override bool IsValid()
        => true;
}