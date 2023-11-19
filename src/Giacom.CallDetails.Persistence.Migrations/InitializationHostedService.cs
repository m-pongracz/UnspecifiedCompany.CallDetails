using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Giacom.CallDetails.Persistence.Migrations;

public class InitializationHostedService : IHostedService
{
    private int? _exitCode;

    private readonly IHostApplicationLifetime _appLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public InitializationHostedService(
        IHostApplicationLifetime appLifetime, IServiceScopeFactory serviceScopeFactory
        )
    {
        _appLifetime = appLifetime;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(() => RunMigrationAsync(cancellationToken));

        return Task.CompletedTask;
    }
    
    private async void RunMigrationAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            
            var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationService>();
            
            await migrationService.MigrateAsync(cancellationToken);

            _exitCode = 1;
        }
        catch (Exception)
        {
            _exitCode = 0;
        }
        finally
        {
            _appLifetime.StopApplication();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Environment.ExitCode = _exitCode.GetValueOrDefault(2);
        return Task.CompletedTask;
    }
}
