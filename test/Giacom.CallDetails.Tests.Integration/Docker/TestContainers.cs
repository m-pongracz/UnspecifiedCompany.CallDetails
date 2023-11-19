using System.Data.Common;
using System.Net.NetworkInformation;
using DotNet.Testcontainers.Builders;
using Testcontainers.MsSql;

namespace Giacom.CallDetails.Tests.Integration.Docker;

public class TestContainers : IAsyncDisposable
{
    private readonly MsSqlContainer _dbContainer;
    private const string TestRunnerKeepDatabaseSetting = "KeepDatabaseBetweenTests";

    private const int SqlPort = MsSqlBuilder.MsSqlPort;

    private readonly bool _keepDatabase;
    private readonly bool _startContainers = true;

    public TestContainers()
    {
        var environment = Environment.GetEnvironmentVariable(TestRunnerKeepDatabaseSetting);
        _ = bool.TryParse(environment, out _keepDatabase);

        var containerInnerDbConnectionString = new DbConnectionStringBuilder
        {
            { "server", "IntegrationTests" },
            { "user id", MsSqlBuilder.DefaultUsername },
            { "password", MsSqlBuilder.DefaultPassword },
            { "database", "master" },
        }.ConnectionString;

        var dbBuilder = new MsSqlBuilder()
            .WithEnvironment("ConnectionStrings__DefaultConnection", containerInnerDbConnectionString)
            .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
            .WithName($"Sql-{Guid.NewGuid()}")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(SqlPort))
            .WithAutoRemove(!_keepDatabase)
            .WithCleanUp(!_keepDatabase);

        if (_keepDatabase)
        {
            if (!ArePortsFree(SqlPort))
            {
                _startContainers = false;
            }


            dbBuilder = dbBuilder
                .WithPortBinding(SqlPort, SqlPort);
        }
        else
        {
            dbBuilder = dbBuilder
                .WithPortBinding(SqlPort, true);
        }

        _dbContainer = dbBuilder.Build();
    }

    public async Task InitializeAsync()
    {
        if (!_startContainers)
        {
            return;
        }

        await _dbContainer.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_keepDatabase)
        {
            return;
        }

        await _dbContainer.DisposeAsync();
    }

    public string GetDbConnectionString()
    {
        DbConnectionStringBuilder connectionStringBuilder;

        if (_keepDatabase)
        {
            connectionStringBuilder = new DbConnectionStringBuilder
            {
                { "Data Source", $"localhost,{SqlPort}" },
                { "TrustServerCertificate", true },
                { "Integrated Security", false },
                { "User ID", MsSqlBuilder.DefaultUsername },
                { "Password", MsSqlBuilder.DefaultPassword },
                { "TrustServerCertificate", true }
            };
        }
        else
        {
            var cnnString = _dbContainer.GetConnectionString();

            connectionStringBuilder = new DbConnectionStringBuilder { ConnectionString = cnnString, };

            connectionStringBuilder.Add("TrustServerCertificate", true);
        }

        return connectionStringBuilder.ConnectionString;
    }

    private static bool ArePortsFree(params int[] ports)
    {
        var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
        var ipEndPoints = ipProperties.GetActiveTcpListeners();

        return ports.All(port => ipEndPoints.All(endPoint => endPoint.Port != port));
    }
}