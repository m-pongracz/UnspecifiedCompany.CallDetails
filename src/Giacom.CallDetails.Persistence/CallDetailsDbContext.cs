using System.ComponentModel;
using Giacom.CallDetails.Domain.CallDetails;
using Giacom.CallDetails.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Giacom.CallDetails.Persistence;

public class CallDetailsDbContext : DbContext
{
    public DbSet<CallDetail> CallDetails => Set<CallDetail>();
    
    public CallDetailsDbContext(DbContextOptions<CallDetailsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CallDetailConfiguration());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);
        
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
    }
}