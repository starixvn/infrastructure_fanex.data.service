using Fanex.Data.Repository;
using System;

namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query
{
    public class GetDateTimeNowCriteria : CriteriaBase
    {
        public DateTime Now { get; set; }

        public override string GetSettingKey() => "GetDateTimeNow";

        public override bool IsValid() => true;
    }
}
