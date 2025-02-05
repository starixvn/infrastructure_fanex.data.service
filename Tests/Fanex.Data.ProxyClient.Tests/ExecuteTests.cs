using Fanex.Data.ProxyClient.Tests.Models;
using Fanex.Data.ProxyClient.Tests.ParameterSets.Commands;
using Fanex.Data.ProxyClient.Tests.ParameterSets.Query;
using System.Linq;

namespace Fanex.Data.ProxyClient.Tests;

using Repository;
using System;
using System.Threading.Tasks;
using Xunit;

[Collection("Fanex Data Client collection")]
public class ExecuteTests
{
    private readonly DynamicRepository proxy = new();

    private const int TestedEmployeeId = 1;
    private const string TestedTerritoryId = "40222";

    [Fact]
    public async Task ExecuteAsync_NullCommand_ThrowNullReferenceException()
    {
        // Arrange.
        var command = default(INonQueryCommand);

        // Act.
        var exception = await Record.ExceptionAsync(() => proxy.ExecuteAsync(command));

        // Assert.
        Assert.IsType<NullReferenceException>(exception);
    }

    [Fact]
    public async Task ExecuteAsync_ValidCommand_ExecuteSP()
    {
        // Arrange.
        var command = new NewEmployeeTerritoryCommand
        {
            EmployeeId = TestedEmployeeId,
            TerritoryId = TestedTerritoryId
        };
        await DeleteEmployeeTerritory();

        // Act.
        await proxy.ExecuteAsync(command);

        // Assert.
        var expectedData = await GetEmployeeTerritory();

        Assert.NotNull(expectedData);
    }

    [Fact]
    public async Task ExecuteAsync_InvalidCommand_DoesNotCallSp()
    {
        // Arrange.
        var command = new NewEmployeeTerritoryCommand
        {
            EmployeeId = -1,
            TerritoryId = string.Empty
        };

        // Act.
        var result = await proxy.ExecuteAsync(command);

        // Assert.

        Assert.Equal(0, result);
    }

    [Fact]
    public async Task ExecuteAsync_InvalidCommand_ReturnsDefaultInt()
    {
        // Arrange.
        var command = new NewEmployeeTerritoryCommand
        {
            EmployeeId = 0,
            TerritoryId = string.Empty
        };

        // Act.
        var result = await proxy.ExecuteAsync(command);

        // Assert.
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task ExecuteAsync_ByteArray_ExecuteSuccess()
    {
        // Arrange.
        var command = new PassByteArray
        {
            ImageId = 1,
            Image = [1, 5, 4, 6, 8]

        };

        // Act.
        await proxy.ExecuteAsync(command);

        // Assert.
        Assert.True(command.Image.SequenceEqual(command.Response));
    }

    private async Task DeleteEmployeeTerritory()
    {
        var criteria = new DeleteEmployeeTerritoryCommand
        {
            EmployeeId = TestedEmployeeId,
            TerritoryId = TestedTerritoryId
        };

        await proxy.ExecuteAsync(criteria);

        var expectedData = await GetEmployeeTerritory();

        Assert.Null(expectedData);
    }

    private async Task<EmployeeTerritory> GetEmployeeTerritory()
    {
        var criteria = new GetEmployeeTerritoryCriteria
        {
            EmployeeId = TestedEmployeeId,
            TerritoryId = TestedTerritoryId
        };

        return await proxy.GetAsync<EmployeeTerritory>(criteria);
    }
}