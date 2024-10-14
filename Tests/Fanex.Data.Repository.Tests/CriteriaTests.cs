namespace Fanex.Data.Repository.Tests;

using NSubstitute;
using Xunit;

public class CriteriaTests
{
    [Fact]
    public void IsInvalid_Always_OppositeOfIsValid()
    {
        // Arrange.
        var criteria = Substitute.ForPartsOf<CriteriaBase>();

        criteria.IsValid().Returns(true);

        // Act.
        var actualIsInvalidValue = criteria.IsInvalid();

        // Assert.
        Assert.False(actualIsInvalidValue);
    }
}