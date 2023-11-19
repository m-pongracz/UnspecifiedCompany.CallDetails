using Microsoft.EntityFrameworkCore;

namespace Giacom.CallDetails.Persistence;

public class EfRepositoryBase<TKey, TEntity> where TEntity : class
{
    protected readonly CallDetailsDbContext DbContext;
    protected readonly DbSet<TEntity> Entities;

    public EfRepositoryBase(CallDetailsDbContext dbContext)
    {
        DbContext = dbContext;
        Entities = DbContext.Set<TEntity>();
    }
}