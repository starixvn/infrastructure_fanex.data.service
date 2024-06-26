#if NET7_0_OR_GREATER

using System.Collections.Generic;
using System;
using Fanex.Data.Repository;
using Xunit;

namespace Fanex.Data.ProxyClient.Tests;

using System.Threading.Tasks;

public class GetBalanceInfoCriteria : CriteriaBase
{
    private static readonly List<string> SettingKeys = new()
    {
        "Age_CustomerSelBalanceInfo_Ag",
        "Age_CustomerSelBalanceInfo_MAg",
        "Age_CustomerSelBalanceInfo_SAg"
    };

    public GetBalanceInfoCriteria(int userLevel, int custId, DateTime moveBetsDay)
    {
        UserLevel = userLevel;
        CustId = custId;
        MoveBetsDay = moveBetsDay;
    }

    [SpParam(Ignored = true)]
    public int UserLevel { get; }

    public int CustId { get; }

    public DateTime MoveBetsDay { get; }

    public override string GetSettingKey() => SettingKeys[UserLevel];

    public override bool IsValid() => CustId > 0;
}

public class BalanceInfo
{
    public string UserName { get; set; }

    public decimal CashBalance { get; set; }

    public decimal YesterdayCashBalance { get; set; }

    public decimal TotalBalance { get; set; }

    public decimal YesterdayTotalBalance { get; set; }

    public decimal TodayWinLoss { get; set; }

    public decimal YesterdayWinLoss { get; set; }

    public decimal GivenCredit { get; set; }

    public decimal MemberCredit { get; set; }

    public decimal PendingTransfer3rd { get; set; }
}

[Collection("Fanex Data Client collection")]
public class TestAlpha
{
    private readonly DynamicRepository proxy = new();


    [Fact]
    public async Task GetBalance()
    {
        var custIds = new List<int>
        {
            23994075,
            23071808,
            23003140
        };

        var moveBetsDay = new DateTime(2024, 5, 24);
        var random = new Random(DateTime.Now.Millisecond);
        for (var i = 0; i < 100; i++)
        {
            var index = random.Next(0, 3);
            var criteria = new GetBalanceInfoCriteria(index, custIds[index], moveBetsDay);

            var model = proxy.Get<BalanceInfo>(criteria);

            Assert.NotEmpty(model.UserName);

            await Task.Delay(100);
        }
    }

}

#endif