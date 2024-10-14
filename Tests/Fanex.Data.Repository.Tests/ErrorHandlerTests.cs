using Fanex.Data.Repository.Tests.ParameterSets.Criteria;
using Fanex.Data.Repository.Tests.ParameterSets.Criteria.ErrorCriteria;
using Fanex.Data.SampleData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Data.Repository.Tests;

[Collection(nameof(FanexDataConfigurationManagerCollection))]
public class ErrorHandlerTests(DynamicRepositoryFixture fixture) : IClassFixture<DynamicRepositoryFixture>
{
    private readonly IDynamicRepository repository = fixture.Repository;

    [Fact]
    public async Task Error_TimeOut_ThrowException()
    {
        // Arrange.
        var criteria = new GetTopCustomerCriteria
        {
            Top = 30,
            WaitFor = "00:00:03"
        };

        var exception = await Record.ExceptionAsync(() => repository.GetAsync<Customer>(criteria));

        // Assert.
        Assert.IsType<ObjectDbException>(exception);
        Assert.Equal("Execution Timeout Expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.", exception.Message);
    }

    [Fact]
    public async Task Error_SPNameNotConfigured_ThrowException()
    {
        // Arrange
        var criteria = new SpNameNotConfiguredCriteria();

        // Act
        var exception = await Record.ExceptionAsync(() => repository.GetAsync<int>(criteria));

        // Assert
        Assert.IsType<KeyNotFoundException>(exception);
        Assert.Equal("There is no Stored Procedure configuration with the key named 'testSpNameNotConfigured' found.", exception.Message);
    }

    [Fact]
    public async Task Error_SettingKeyEmpty_ThrowException()
    {
        // Arrange
        var criteria = new SpNameKeyEmptyCriteria();

        // Act
        var exception = await Record.ExceptionAsync(() => repository.GetAsync<int>(criteria));

        // Assert
        Assert.IsType<ArgumentException>(exception);
        Assert.StartsWith("Key must not be null or empty.", exception.Message);
    }

    [Fact]
    public async Task Error_SpNotExist_ThrowException()
    {
        // Arrange
        var criteria = new SpNameNotExistCriteria();

        // Act
        var exception = await Record.ExceptionAsync(() => repository.GetAsync<int>(criteria));

        // Assert
        Assert.IsType<ObjectDbException>(exception);
        Assert.Equal("The stored procedure 'SpNotExist' doesn't exist.", exception.Message);
    }

    [Fact]
    public async Task Error_FromSp_ThrowException()
    {
        // Arrange
        // Passing wrong time type
        var criteria = new GetTopCustomerCriteria { WaitFor = "time_not_valid" };

        // Act
        var exception = await Record.ExceptionAsync(() => repository.GetAsync<Customer>(criteria));

        // Assert
        Assert.IsType<ObjectDbException>(exception);
        Assert.Equal("Conversion failed when converting date and/or time from character string.", exception.Message);
    }
}