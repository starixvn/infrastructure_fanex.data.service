namespace Fanex.Data.Repository.Tests;

using Models;
using ParameterSets.Commands;
using System;
using System.Threading.Tasks;
using Xunit;

[Collection(nameof(FanexDataConfigurationManagerCollection))]
public class ExecuteTests(DynamicRepositoryFixture fixture) : IClassFixture<DynamicRepositoryFixture>
{
    private const int TestedEmployeeId = 1;
    private const string TestedTerritoryId = "40222";

    private readonly IDynamicRepository repository = fixture.Repository;
    private readonly dynamic _assertingParameterSet = new
    {
        EmployeeId = TestedEmployeeId,
        TerritoryId = TestedTerritoryId
    };

    [Fact]
    public void Execute_NullCommand_ThrowArgumentNullException()
    {
        // Arrange.
        var command = default(INonQueryCommand);

        // Act.
        var exception = Record.Exception(() => repository.Execute(command));

        // Assert.
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Execute_ValidCommand_ExecuteSP()
    {
        // Arrange.
        EnsureExpectedDataHaveNotInserted(_assertingParameterSet);

        // Act.
        repository.Execute(new NewEmployeeTerritoryCommand
        {
            EmployeeId = TestedEmployeeId,
            TerritoryId = TestedTerritoryId
        });

        // Assert.
        var expectedData = GetEmployeeTerritory(_assertingParameterSet);

        Assert.NotNull(expectedData);
    }

    [Fact]
    public void Execute_InvalidCommand_DoesNotCallSp()
    {
        // Arrange.
        EnsureExpectedDataHaveNotInserted(_assertingParameterSet);

        // Act.
        repository.Execute(new NewEmployeeTerritoryCommand
        {
            EmployeeId = 0,
            TerritoryId = string.Empty
        });

        // Assert.
        var expectedData = GetEmployeeTerritory(_assertingParameterSet);

        Assert.Null(expectedData);
    }

    [Fact]
    public void Execute_InvalidCommand_ReturnsZero()
    {
        // Arrange.
        EnsureExpectedDataHaveNotInserted(_assertingParameterSet);

        // Act.
        var affectedRows = repository.Execute(new NewEmployeeTerritoryCommand
        {
            EmployeeId = 0,
            TerritoryId = string.Empty
        });

        // Assert.
        Assert.Equal(0, affectedRows);
    }

    [Fact]
    public async Task ExecuteAsync_NullCommand_ThrowArgumentNullException()
    {
        // Arrange.
        var command = default(INonQueryCommand);

        // Act.
        var exception = await Record
                .ExceptionAsync(() => repository.ExecuteAsync(command))
            ;

        // Assert.
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task ExecuteAsync_ValidCommand_ExecuteSP()
    {
        // Arrange.
        var parameterSet = new NewEmployeeTerritoryCommand
        {
            EmployeeId = TestedEmployeeId,
            TerritoryId = TestedTerritoryId
        };
        EnsureExpectedDataHaveNotInserted(_assertingParameterSet);

        // Act.
        await repository.ExecuteAsync(parameterSet);

        // Assert.
        var expectedData = GetEmployeeTerritory(_assertingParameterSet);

        Assert.NotNull(expectedData);
    }

    [Fact]
    public async Task ExecuteAsync_InvalidCommand_DoesNotCallSp()
    {
        // Arrange.
        var parameterSet = new NewEmployeeTerritoryCommand
        {
            EmployeeId = -1,
            TerritoryId = string.Empty
        };

        EnsureExpectedDataHaveNotInserted(_assertingParameterSet);

        // Act.
        await repository.ExecuteAsync(parameterSet);

        // Assert.
        var expectedData = GetEmployeeTerritory(_assertingParameterSet);

        Assert.Null(expectedData);
    }

    [Fact]
    public async Task ExecuteAsync_InvalidCommand_ReturnsZero()
    {
        // Arrange.
        var parameterSet = new NewEmployeeTerritoryCommand
        {
            EmployeeId = 0,
            TerritoryId = string.Empty
        };

        EnsureExpectedDataHaveNotInserted(_assertingParameterSet);

        // Act.
        var affectedRows = await repository.ExecuteAsync(parameterSet);

        // Assert.
        Assert.Equal(0, affectedRows);
    }

    private static void DeleteEmployeeTerritory(object parameterSet)
    {
        const string settingKey = "DeleteEmployeeTerritory";

        ObjectDbFactory
            .CreateInstance(settingKey)
            .ExecuteNonQuery(parameterSet);
    }

    private static EmployeeTerritory GetEmployeeTerritory(object parameterSet)
    {
        const string settingKey = "GetEmployeeTerritory";

        return ObjectDbFactory
            .CreateInstance(settingKey)
            .QueryEntity<EmployeeTerritory>(parameterSet);
    }

    private static void EnsureExpectedDataHaveNotInserted(object parameterSet)
    {
        DeleteEmployeeTerritory(parameterSet);

        var unexpectedData = GetEmployeeTerritory(parameterSet);

        Assert.Null(unexpectedData);
    }
}