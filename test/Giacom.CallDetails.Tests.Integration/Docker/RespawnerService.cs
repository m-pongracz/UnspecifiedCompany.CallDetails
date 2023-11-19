using Microsoft.Data.SqlClient;
using Respawn;
using Respawn.Graph;

namespace Giacom.CallDetails.Tests.Integration.Docker;

public class RespawnerService
{
    private readonly string _dbConnectionString;
    private Respawner? _respawner;
    
    public RespawnerService(TestContainers testContainers)
    {
        _dbConnectionString = testContainers.GetDbConnectionString();
    }

    public async Task InitAsync()
    {
        await using var dbConnection = GetDbConnection();
        await dbConnection.OpenAsync();
        
        _respawner = await Respawner.CreateAsync(dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            TablesToIgnore = new Table[]
            {
                "__EFMigrationsHistory"
            }
        });
    }

    public async Task RunRespawner()
    {
        if (_respawner == null)
        {
            throw new ApplicationException($"Respawner instance was not initialized. Call {nameof(InitAsync)} before calling this method.");
        }
        
        await using var dbConnection = GetDbConnection();
        await dbConnection.OpenAsync();

        await _respawner.ResetAsync(dbConnection);
    }
    
    private SqlConnection GetDbConnection()
    {
        return new SqlConnection(_dbConnectionString);
    }
}
