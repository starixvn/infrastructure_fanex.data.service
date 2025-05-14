using System;

namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria;

public class TestGuidCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "TestGuid";

    public Guid SessionId { get; set; }

    public override bool IsValid()
        => true;
}