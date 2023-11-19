using Microsoft.EntityFrameworkCore;

namespace Giacom.CallDetails.Persistence.Migrations;

public class MigrationService : IMigrationService
{
    private readonly CallDetailsDbContext _dbContext;

    public MigrationService(CallDetailsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.MigrateAsync(cancellationToken);
    }
}
