using Fanex.Data.ProxyClient.Tests.ParameterSets.Query;
using Fanex.Data.SampleData.Models;

namespace Fanex.Data.ProxyClient.Tests;

using ParameterSets.Query.ErrorCriteria;
using Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

[Collection("Fanex Data Client collection")]
public class ErrorHandlerTests
{
    private readonly DynamicRepository proxy = new();

    [Fact]
    public async Task GetAsync_SPNameNotConfigured_ThrowException()
    {
        // Arrange
        var criteria = new SpNameNotConfiguredCriteria();

        // Act
        var exception = await Record.ExceptionAsync(() => proxy.GetAsync<int>(criteria));

        // Assert
        Assert.IsType<KeyNotFoundException>(exception);
        Assert.Equal("There is no Stored Procedure configuration with the key named 'testSpNameNotConfigured' found.", exception.Message);
    }

    [Fact]
    public async Task GetAsync_SpNotExist_ThrowException()
    {
        // Arrange
        var criteria = new SpNameNotExistCriteria();

        // Act
        var exception = await Record.ExceptionAsync(() => proxy.GetAsync<int>(criteria));

        // Assert
        Assert.IsType<DbProxyNotSuccessHttpException>(exception);
        Assert.Contains("The stored procedure 'SpNotExist' doesn't exist.", exception.Message);
    }

    [Fact]
    public async Task Error_TimeOut_ThrowException()
    {
        // Arrange.
        var criteria = new GetTopCustomerCriteria
        {
            Top = 30,
            WaitFor = "00:00:03"
        };

        var exception = await Record.ExceptionAsync(() => proxy.GetAsync<Customer>(criteria));

        // Assert.
        Assert.IsType<DbProxyNotSuccessHttpException>(exception);
        Assert.Contains("Execution Timeout Expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.", exception.Message);
    }

    [Fact]
    public async Task Error_SettingKeyEmpty_ThrowException()
    {
        // Arrange
        var criteria = new SpNameKeyEmptyCriteria();

        // Act
        var exception = await Record.ExceptionAsync(() => proxy.GetAsync<int>(criteria));

        // Assert
        Assert.IsType<ArgumentException>(exception);
        Assert.StartsWith("Key must not be null or empty.", exception.Message);
    }

    [Fact]
    public async Task Error_FromSp_ThrowException()
    {
        // Arrange
        // Passing wrong time type
        var criteria = new GetTopCustomerCriteria { Top = 1, WaitFor = "time_not_valid" };

        // Act
        var exception = await Record.ExceptionAsync(() => proxy.GetAsync<Customer>(criteria));

        // Assert
        Assert.IsType<DbProxyNotSuccessHttpException>(exception);
        Assert.Contains("Conversion failed when converting date and/or time from character string.", exception.Message);
    }
}