namespace Giacom.CallDetails.Persistence.Migrations;

public interface IMigrationService
{
    Task MigrateAsync(CancellationToken cancellationToken = default);
}
