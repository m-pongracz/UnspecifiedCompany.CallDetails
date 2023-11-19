using Giacom.CallDetails.Persistence;
using Giacom.CallDetails.Tests.Integration.Docker;
using Giacom.CallDetails.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Giacom.CallDetails.Tests.Integration;

// ReSharper disable once ClassNeverInstantiated.Global
public class TestingWebApplicationFactory : WebApplicationFactory<Startup>, IAsyncLifetime
{
    private readonly TestContainers _testContainers = new();
    private RespawnerService? _respawnerService;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(SetupServices);
    }

    public IServiceScope CreateScope()
    {
        return base.Services.CreateScope();
    }

    private void SetupServices(IServiceCollection services)
    {
        var dbContextOptionsDesc =
            services.Single(d => d.ServiceType == typeof(DbContextOptions<CallDetailsDbContext>));
        services.Remove(dbContextOptionsDesc);

        services.AddDbContext<CallDetailsDbContext>(options =>
        {
            options
                .UseSqlServer(_testContainers.GetDbConnectionString(),
                    x => x.MigrationsAssembly(MigrationConstants.MigrationAssembly));
        });
    }

    public async Task ResetDatabase()
    {
        await _respawnerService!.RunRespawner();

        using var scope = Server.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<CallDetailsDbContext>();
        dbContext.ChangeTracker.Clear();
    }

    public virtual async Task InitializeAsync()
    {
        await _testContainers.InitializeAsync();

        using var scope = Server.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CallDetailsDbContext>();
        await dbContext.Database.MigrateAsync();

        // must be instantiated after containers are running so cnnString can be retrieved from the test container
        _respawnerService = new RespawnerService(_testContainers);
        await _respawnerService.InitAsync();
    }

    public new async Task DisposeAsync()
    {
        await _testContainers.DisposeAsync();
        await base.DisposeAsync();
    }
}