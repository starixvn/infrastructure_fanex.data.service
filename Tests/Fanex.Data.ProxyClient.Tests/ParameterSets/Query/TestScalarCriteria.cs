namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query
{
    using Repository;

    public class TestScalarIntCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "TestScalarInt";

        public override bool IsValid() => true;
    }

    public class TestScalarStringCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "TestScalarString";

        public override bool IsValid() => true;
    }

    public class TestScalarBooleanCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "TestScalarBoolean";

        public override bool IsValid() => true;
    }

    public class TestScalarByteCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "TestScalarByte";

        public override bool IsValid() => true;
    }

    public class TestScalarDoubleCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "TestScalarDouble";

        public override bool IsValid() => true;
    }

    public class TestScalarDecimalCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "TestScalarDecimal";

        public override bool IsValid() => true;
    }

    public class TestScalarDateTimeCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "TestScalarDateTime";

        public override bool IsValid() => true;
    }

    public class TestGetNullValueCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "TestGetListNull";

        public override bool IsValid() => true;
    }
}
